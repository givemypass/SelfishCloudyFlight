using Core.CommonComponents;
using Core.Features.LevelFeature.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;
using Systems;

namespace Core.Features.GameStatesFeature.Systems.States
{
    [Injectable]
    public sealed partial class LevelStateSystem : BaseGameStateSystem, IUpdatable
    {
        private Single<GameStateComponent> _gameState;

        private Filter _planeFilter;
        [Inject] private UIService _uiService;

        protected override int State => GameStateIdentifierMap.LevelState;

        public override void InitSystem()
        {
            _gameState = new Single<GameStateComponent>(Owner.GetWorld());
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<PositionOnSplineComponent>()
                .Build();
        }

        public void Update()
        {
            if (!_gameState.Get().IsNeededState(State))
                return;

            foreach (var entity in _planeFilter)
            {
                var tpos = entity.Get<PositionOnSplineComponent>().TPos;
                if (tpos > 1)
                {
                    SManager.World.Command(new LevelFinishedCommand());
                    EndState();
                }
            }
        }

        protected override void ProcessState(int from, int to)
        {
        }

        protected override void OnExitState()
        {
            _uiService.CloseUI(UIIdentifierMap.LevelScreen_UIIdentifier);
        }
    }
}