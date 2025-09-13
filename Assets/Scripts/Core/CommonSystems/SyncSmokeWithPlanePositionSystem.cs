using Core.CommonComponents;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Components;

namespace Core.Systems
{
    public sealed partial class SyncSmokeWithPlanePositionSystem : BaseSystem, IUpdatable
    {
        private Filter _smokeFilter;
        private Filter _planeFilter;

        public override void InitSystem()
        {
            _smokeFilter = Owner.GetWorld().Filter
                .With<ActorProviderComponent>()
                .With<WritingSmokeVFXMonoProvider>()
                .Build();
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<TargetSplineComponent>()
                .With<PositionOnSplineComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var plane in _planeFilter)
            {
                ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
                ref var positionOnSplineComponent = ref plane.Get<PositionOnSplineComponent>();

                var tPos = positionOnSplineComponent.TPos;
                var position = targetSplineComponent.SplineContainer.EvaluatePosition(tPos);

                foreach (var smoke in _smokeFilter)
                {
                    ref var writingSmokeVFXMonoProvider = ref smoke.Get<WritingSmokeVFXMonoProvider>();
                    ref var actorProviderComponent = ref smoke.Get<ActorProviderComponent>();
                    
                    var pos = position;
                    pos.z = writingSmokeVFXMonoProvider.MonoComponent.ZOffset;
                    actorProviderComponent.Actor.transform.position = pos;
                }
            } 
        }
    }
}