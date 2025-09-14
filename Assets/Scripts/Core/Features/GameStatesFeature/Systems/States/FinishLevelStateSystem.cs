using System;
using Core.CommonComponents;
using Core.Models;
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
        [Inject] private LevelsConfigSO _levelsConfig;
        
        private Filter _levelFilter;
        private Single<PlayerProgressComponent> _playerProgressSingle;

        protected override int State => GameStateIdentifierMap.FinishLevelState;

        public override void InitSystem()
        {
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
            _playerProgressSingle = new Single<PlayerProgressComponent>(Owner.GetWorld());
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
            ref var playerProgress = ref _playerProgressSingle.Get();
            playerProgress.LevelIndex = _levelsConfig.Get.GetNextLevelIndex(playerProgress.LevelIndex);
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
                ref var playerProgress = ref _playerProgressSingle.Get();
                playerProgress.LevelIndex = _levelsConfig.Get.GetPreviousLevelIndex(playerProgress.LevelIndex);
            }

            Owner.GetWorld().Command(new ForceGameStateTransitionGlobalCommand
            {
                GameState = GameStateIdentifierMap.BootstrapLevelState,
            });
        }
    }
}