using System.Linq;
using Core.Commands;
using Core.CommonComponents;
using Core.Features.ScoreFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.ScoreFeature.Systems
{
    public sealed partial class LinearScoreInitializeSystem : BaseSystem,
        IReactGlobal<StartEndMarkersInitializedCommand>
    {
        private Filter _startEndMarkersFilter;
        
        public override void InitSystem()
        {
            _startEndMarkersFilter = Owner.GetWorld().Filter.With<StartEndMarkersComponent>().Build();
        }

        public void ReactGlobal(StartEndMarkersInitializedCommand command)
        {
            _startEndMarkersFilter.ForceUpdate();
            ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
            foreach (var entity in _startEndMarkersFilter)
            {
                ref var startEndMarkersComponent = ref entity.Get<StartEndMarkersComponent>();
                var lastPoint = startEndMarkersComponent.Points.Last();
                scoreComponent.SpeedMultiplier = 1 / lastPoint.TPos;
                scoreComponent.StopPos = lastPoint.TPos;
            }
        }
    }
}