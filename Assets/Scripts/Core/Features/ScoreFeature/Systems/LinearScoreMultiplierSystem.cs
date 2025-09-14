using Core.CommonComponents;
using Core.Features.LevelFeature.Commands;
using Core.Features.ScoreFeature.Components;
using Core.Models;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.ScoreFeature.Systems
{
    [Injectable]
    public sealed partial class LinearScoreMultiplierSystem : BaseSystem,
        IUpdatable,
        IReactGlobal<MarkerReachedCommand>,
        IReactGlobal<MissMarkerCommand>
    {
        [Inject] private GlobalConfigSO _globalConfig;
        
        private Filter _planeFilter;

        public override void InitSystem()
        {
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var plane in _planeFilter)
            {
                ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
                if (scoreComponent.NeedEmitting == plane.Has<PlaneIsEmittingComponent>())
                {
                    scoreComponent.Multiplier = _globalConfig.Get.ScorePositiveMultiplier;
                }
            }
        }

        void IReactGlobal<MarkerReachedCommand>.ReactGlobal(MarkerReachedCommand command)
        {
            ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
            scoreComponent.NeedEmitting = !command.MarkerIsEnd;
            scoreComponent.Multiplier = 0;
        }

        void IReactGlobal<MissMarkerCommand>.ReactGlobal(MissMarkerCommand command)
        {
            ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
            scoreComponent.Multiplier = _globalConfig.Get.ScoreNegativeMultiplier;
        }
    }
}