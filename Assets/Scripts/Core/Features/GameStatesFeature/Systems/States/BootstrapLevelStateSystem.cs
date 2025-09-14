using System.Linq;
using Core.CommonComponents;
using Core.Models;
using Core.MonoBehaviourComponents;
using Core.MonoBehaviourComponents.GUI;
using Core.Services;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;
using Systems;
using UnityEngine;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem
    {
        private const string SCENE_NAME = "Level";
        private Single<ActorsHolderComponent> _actorsHolder;
        [Inject] private GlobalConfigSO _globalConfig;

        private Single<LevelsHolderComponent> _levelsHolder;
        private Single<PlayerProgressComponent> _playerProgress;

        [Inject] private SceneService _sceneManager;
        [Inject] private UIService _uiService;

        protected override int State => GameStateIdentifierMap.BootstrapLevelState;

        public override void InitSystem()
        {
            var world = Owner.GetWorld();
            _levelsHolder = new Single<LevelsHolderComponent>(world);
            _playerProgress = new Single<PlayerProgressComponent>(world);
            _actorsHolder = new Single<ActorsHolderComponent>(world);
        }

        protected override void ProcessState(int from, int to)
        {
            if (!_levelsHolder.Exists() || !_playerProgress.Exists())
            {
                SLog.LogError("Required components are not found in the world.");
                return;
            }

            var levelIndex = _playerProgress.Get().LevelIndex;
            _levelsHolder.Get().TryGetLevel(levelIndex, out var currentLevel);
            var globalConfig = _globalConfig.Get;
            var levelActorPrefab = _actorsHolder.Get().LevelActorPrefab;
            ProcessStateAsync(currentLevel, globalConfig, levelActorPrefab).Forget();
        }

        private async UniTask ProcessStateAsync(LevelMonoComponent level, GlobalConfig globalConfig,
            Actor levelActorPrefab)
        {
            await _sceneManager.LoadScene(SCENE_NAME);

            var levelActor = Object.Instantiate(levelActorPrefab);
            levelActor.SetEntity(Owner.GetWorld());
            var levelComponent = levelActor.Entity.Get<LevelComponent>();
            levelComponent.Level = level;
            levelActor.Entity.Set(levelComponent);
            levelActor.InitEntity();

            var colors = level.GetColors().Select(c => globalConfig.ColorPallete[c]).ToArray();
            await ShowUI(colors);

            EndState();
        }

        private async UniTask ShowUI(Color[] colors)
        {
            var actor = await _uiService.ShowUIAsync(UIIdentifierMap.LevelScreen_UIIdentifier);
            var monoComponent = actor.GetComponent<LevelScreenUIMonoComponent>();
            monoComponent.SetLevelColors(colors);
            monoComponent.Reset.onClick.AddListener(OnReset);
        }

        private static void OnReset()
        {
            SManager.World.Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.BootstrapLevelState,
            });
        }
    }
}