using System.Drawing;
using Core.Components;
using Core.Models;
using Core.MonoBehaviourComponents;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Systems.States
{
    public sealed partial class BootstrapLevelStateSystem : BaseGameStateSystem, IGlobalStart
    {
        private const string SCENE_NAME = "Level";
        
        private Single<LevelsHolderComponent> _levelsHolder;
        private Single<GlobalConfigComponent> _globalConfig;
        private Single<PlayerProgressComponent> _playerProgress;
        private SingleSystem<SceneManagerTagComponent, SceneManagerSystem> _sceneManager;

        public override void InitSystem() { }

        public void GlobalStart()
        {
            _levelsHolder = new Single<LevelsHolderComponent>(Owner.GetWorld());
            _globalConfig = new Single<GlobalConfigComponent>(Owner.GetWorld());
            _playerProgress = new Single<PlayerProgressComponent>(Owner.GetWorld());
            _sceneManager = new SingleSystem<SceneManagerTagComponent, SceneManagerSystem>(Owner.GetWorld());

            //todo
            // AsSingle(ref actorsContainersHolderComponent);
            // AsSingleSystem(ref uiSystem);
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
            ProcessStateAsync(currentLevel, globalConfig).Forget();
        }
        
        private async UniTask ProcessStateAsync(LevelMonoComponent level, GlobalConfig globalConfig)
        {
            await _sceneManager.Get().LoadScene(SCENE_NAME);
            
            // var levelContainer = actorsContainersHolderComponent.LevelContainer;
            // var levelContainerActor = await levelContainer.GetActor();
            // levelContainerActor.GetHECSComponent<LevelComponent>().Level = level;
            // levelContainerActor.Init();
            //
            // var colors = level.GetColors().Select(c => globalConfig.ColorPallete[c]).ToArray();
            // await ShowUI(colors);
            //
            EndState();
        }

        private async UniTask ShowUI(Color[] colors)
        {
            // var uiEnt = await uiSystem.ShowUI(UIIdentifierMap.LevelScreen_UIIdentifier);
            // var monoComponent = uiEnt.AsActor().GetComponent<LevelScreenUIMonoComponent>();
            // monoComponent.SetLevelColors(colors);
            // monoComponent.Reset.onClick.AddListener(OnReset);
        }

        private void OnReset()
        {
            SManager.World.Command(new ForceGameStateTransitionGlobalCommand
            { 
                GameState= GameStateIdentifierMap.BootstrapLevelState,
            });
        }
    }
}