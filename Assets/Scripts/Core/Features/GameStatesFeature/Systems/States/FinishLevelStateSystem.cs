using System;
using Core.CommonComponents;
using Core.MonoBehaviourComponents.GUI;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class FinishLevelStateSystem : BaseGameStateSystem
    {
        [Inject] private UIService _uiService;
        
        private Filter _levelFilter;
        private Single<LevelsHolderComponent> _levelHolder;
        private Single<PlayerProgressComponent> _playerProgressComponent;

        protected override int State => GameStateIdentifierMap.FinishLevelState;

        public override void InitSystem()
        {
            _playerProgressComponent = new Single<PlayerProgressComponent>(Owner.GetWorld());
            _levelHolder = new Single<LevelsHolderComponent>(Owner.GetWorld());
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
        }

        protected override void ProcessState(int from, int to)
        {
            ProcessStateAsync().Forget();
        }

        private async UniTask ProcessStateAsync()
        {
            var uiActor = await _uiService.ShowUIAsync(UIIdentifierMap.FinishLevelScreen_UIIdentifier);
            var monoComponent = uiActor.GetComponent<FinishLevelScreenUIMonoComponent>();
            monoComponent.Reset.onClick.AddListener(OnReset);
            monoComponent.Next.onClick.AddListener(OnNext);
            if (IsCompleted())
                IncrementLevel();
            else
                monoComponent.Next.gameObject.SetActive(false);
        }

        private void IncrementLevel()
        {
            ref var playerProgress = ref _playerProgressComponent.Get();
            playerProgress.LevelIndex = _levelHolder.Get().GetNextLevelIndex(playerProgress.LevelIndex);
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
            _uiService.CloseUI(UIIdentifierMap.FinishLevelScreen_UIIdentifier);
        }

        private void OnNext()
        {
            EndState();
        }

        private void OnReset()
        {
            if (IsCompleted())
            {
                ref var playerProgress = ref _playerProgressComponent.Get();
                playerProgress.LevelIndex = _levelHolder.Get().GetPreviousLevelIndex(playerProgress.LevelIndex);
            }

            SManager.World.Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.BootstrapLevelState,
            });
        }
    }
}