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
        
        [Inject] private GlobalConfigSO _globalConfig;
        [Inject] private LevelsConfigSO _levelsConfig;
        [Inject] private SceneService _sceneManager;
        [Inject] private UIService _uiService;

        private Single<PlayerProgressComponent> _playerProgressSingle;

        protected override int State => GameStateIdentifierMap.BootstrapLevelState;

        public override void InitSystem()
        {
            _playerProgressSingle = new Single<PlayerProgressComponent>(Owner.GetWorld());
        }

        protected override void ProcessState(int from, int to)
        {
            _playerProgressSingle.ForceUpdate();
            if (!_playerProgressSingle.Exists())
            {
                SLog.LogError("Required components are not found in the world.");
                return;
            }

            var levelIndex = _playerProgressSingle.Get().LevelIndex;
            _levelsConfig.Get.TryGetLevel(levelIndex, out var currentLevel);
            var globalConfig = _globalConfig.Get;
            var levelActorPrefab = _globalConfig.Get.LevelPrefab;
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