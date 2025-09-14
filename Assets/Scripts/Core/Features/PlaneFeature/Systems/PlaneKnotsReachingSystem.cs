using Core.CommonComponents;
using Core.Features.LevelFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine.Splines;

namespace Core.Features.PlaneFeature.Systems
{
    public sealed partial class PlaneKnotsReachingSystem : BaseSystem, IUpdatable
    {
        private Filter _planeFilter;
        private Filter _levelFilter;
        private Filter _smokeFilter;

        public override void InitSystem()
        {
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<PositionOnSplineComponent>()
                .With<TargetSplineComponent>()
                .Build();
            _levelFilter = Owner.GetWorld().Filter
                .With<LevelComponent>()
                .Build();
            _smokeFilter = Owner.GetWorld().Filter
                .With<WritingSmokeVFXMonoProvider>()
                .Build();
        }

        public void Update()
        {
            foreach (var plane in _planeFilter)
            {
                ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
                ref var positionOnSplineComponent = ref plane.Get<PositionOnSplineComponent>();
                var spline = targetSplineComponent.SplineContainer.Spline;

                var tPos = positionOnSplineComponent.TPos;
                if (tPos >= positionOnSplineComponent.NextKnotTPos)
                {
                    ApplyOverrides();
                    positionOnSplineComponent.NextKnotIndex++;
                    var nextKnotT = spline.ConvertIndexUnit(positionOnSplineComponent.NextKnotIndex, PathIndexUnit.Knot, PathIndexUnit.Normalized);
                    positionOnSplineComponent.NextKnotTPos = nextKnotT;
                }
            }
        }
        
        private void ApplyOverrides()
        {
            foreach (var level in _levelFilter)
            {
                ref var levelMonoComponent = ref level.Get<LevelComponent>().Level;
                var scale = levelMonoComponent.Scale;

                foreach (var smokeEntity in _smokeFilter)
                {
                    var smoke = smokeEntity.Get<WritingSmokeVFXMonoProvider>().MonoComponent;
                    smoke.ApplyScale(1/smoke.Scale * scale);
                }
            }
        }
    }
}