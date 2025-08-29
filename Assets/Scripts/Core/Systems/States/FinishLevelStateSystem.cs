using System;
using Core.Components;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity.Generated;
using Systems;

namespace Core.Systems.States
{
    public sealed partial class FinishLevelStateSystem : BaseGameStateSystem, IGlobalStart
    {
        private Filter _levelFilter;
        
        public override void InitSystem() { }
    
        public void GlobalStart()
        {
            // playerProgress = EntityManager.Default.GetComponentFromSingleTag<PlayerTagComponent, PlayerProgressComponent>();
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
            
            // AsSingle(ref levelHolder);
            // AsSingleSystem(ref uiSystem);
        }
    
        protected override int State => GameStateIdentifierMap.FinishLevelState;
    
        protected override void ProcessState(int from, int to)
        {
            ProcessStateAsync().Forget();
        }

        private async UniTask ProcessStateAsync()
        {
            // var uiEnt = await uiSystem.ShowUI(UIIdentifierMap.FinishLevelScreen_UIIdentifier);
            // var monoComponent = uiEnt.AsActor().GetComponent<FinishLevelScreenUIMonoComponent>();
            // monoComponent.Reset.onClick.AddListener(OnReset);
            // monoComponent.Next.onClick.AddListener(OnNext);
            // if (IsCompleted())
            // {
            //     playerProgress.LevelIndex = levelHolder.GetNextLevelIndex(playerProgress.LevelIndex);
            // }
            // else
            // {
            //     monoComponent.Next.gameObject.SetActive(false);
            // }
        }


        private bool IsCompleted()
        {
            foreach (var level in _levelFilter)
            {
                ref var levelComponent = ref level.Get<LevelComponent>();
                return levelComponent.LevelProgress >= 0.9f;
            }
            throw new Exception("There is no level component in the level filter");
        }

        protected override void OnExitState()
        {
            // EntityManager.Default.Command(new HideUICommand
            // {
            //     UIViewType = UIIdentifierMap.FinishLevelScreen_UIIdentifier
            // });
        }
        
        private void OnNext()
        {
            EndState();
        }

        private void OnReset()
        {
            // if (IsCompleted())
            // {
            //     playerProgress.LevelIndex = levelHolder.GetPreviousLevelIndex(playerProgress.LevelIndex);
            // }
            // EntityManager.Default.Command(new ForceGameStateTransitionGlobalCommand
            // {
            //     GameState = GameStateIdentifierMap.BootstrapLevelState
            // });
        }
    }
}