using Core.Commands;
using Core.CommonComponents;
using Core.Features.ScoreFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine;

namespace Core.Features.ScoreFeature.Systems
{
    public sealed partial class LinearScoreIncrementSystem : BaseSystem, IUpdatable
    {
        private Filter _levelFilter;
        private Filter _planeFilter;
        
        public override void InitSystem()
        {
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<SpeedCounterComponent>()
                .With<PositionOnSplineComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var level in _levelFilter)
            {
                foreach (var plane in _planeFilter)
                {
                    ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
                    var tPos = plane.Get<PositionOnSplineComponent>().TPos;
                    if(tPos > scoreComponent.StopPos)
                        continue;
                    var speed = plane.Get<SpeedCounterComponent>().Value * scoreComponent.SpeedMultiplier;
                    ref var levelComponent = ref level.Get<LevelComponent>();
                    levelComponent.LevelProgress += scoreComponent.Multiplier * speed * Time.deltaTime;
                    Owner.GetWorld().Command(new LevelProgressUpdatedCommand());
                }
            }
        }
    }
}