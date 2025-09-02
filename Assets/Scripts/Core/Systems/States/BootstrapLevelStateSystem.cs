using Core.Components;
using Core.Models;
using Core.MonoBehaviourComponents;
using Core.Services;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using UnityEngine;

namespace Core.Systems.States
{
    [Injectable]
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem, IGlobalStart
    {
        private const string SCENE_NAME = "Level";
        
        private Single<LevelsHolderComponent> _levelsHolder;
        private Single<GlobalConfigComponent> _globalConfig;
        private Single<PlayerProgressComponent> _playerProgress;
        private Single<ActorsHolderComponent> _actorsHolder;
        
        [Inject] private SceneService _sceneManager;

        public override void InitSystem() { }

        public void GlobalStart()
        {
            var world = Owner.GetWorld();
            _levelsHolder = new Single<LevelsHolderComponent>(world);
            _globalConfig = new Single<GlobalConfigComponent>(world);
            _playerProgress = new Single<PlayerProgressComponent>(world);
            _actorsHolder = new Single<ActorsHolderComponent>(world);
        }

        protected override int State => GameStateIdentifierMap.BootstrapLevelState;

        protected override void ProcessState(int from, int to)
        {
            if(!_levelsHolder.Exists() || !_globalConfig.Exists() || !_playerProgress.Exists())
            {
                SLog.LogError("Required components are not found in the world.");
                return;
            }

            var levelIndex = _playerProgress.Get().LevelIndex;
            _levelsHolder.Get().TryGetLevel(levelIndex, out var currentLevel);
            var globalConfig = _globalConfig.Get().Get;
            var levelActorPrefab = _actorsHolder.Get().LevelActorPrefab;
            ProcessStateAsync(currentLevel, globalConfig, levelActorPrefab).Forget();
        }
        
        private async UniTask ProcessStateAsync(LevelMonoComponent level, GlobalConfig globalConfig,
            Actor levelActorPrefab)
        {
            await _sceneManager.LoadScene(SCENE_NAME);

            var levelActor = Object.Instantiate(levelActorPrefab);
            levelActor.Init(Owner.GetWorld());
            var levelComponent = levelActor.Entity.Get<LevelComponent>();
            levelComponent.Level = level;
            levelActor.Entity.Set(levelComponent);
            levelActor.InitSystems();
            
            //todo
            // var colors = level.GetColors().Select(c => globalConfig.ColorPallete[c]).ToArray();
            // await ShowUI(colors);
            
            EndState();
        }

        // private async UniTask ShowUI(Color[] colors)
        // {
            // var uiEnt = await uiSystem.ShowUI(UIIdentifierMap.LevelScreen_UIIdentifier);
            // var monoComponent = uiEnt.AsActor().GetComponent<LevelScreenUIMonoComponent>();
            // monoComponent.SetLevelColors(colors);
            // monoComponent.Reset.onClick.AddListener(OnReset);
        // }

        private void OnReset()
        {
            SManager.World.Command(new ForceGameStateTransitionGlobalCommand
            { 
                GameState= GameStateIdentifierMap.BootstrapLevelState,
            });
        }
    }
}