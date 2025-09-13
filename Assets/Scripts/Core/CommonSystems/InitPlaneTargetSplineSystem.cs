using Core.CommonComponents;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.Systems
{
    public sealed partial class InitPlaneTargetSplineSystem : BaseSystem, IUpdatable
    {
        private Filter _splineFilter;
        private Filter _planeFilter;
        private Filter _smokeFilter;

        public override void InitSystem()
        {
            _splineFilter = Owner.GetWorld().Filter.With<LevelContainerTagComponent>().Build();
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .Without<TargetSplineComponent>()
                .Build();
            _smokeFilter = Owner.GetWorld().Filter.With<WritingSmokeVFXMonoProvider>().Build();
        }

        public void Update()
        {
            foreach (var planeEnt in _planeFilter)
            {
                foreach (var levelEnt in _splineFilter)
                {
                    ref var levelComponent = ref levelEnt.Get<LevelComponent>();
                    var levelPrefab = levelComponent.Level;
                    var levelContainer = levelEnt.AsActor();
                    var level = Object.Instantiate(levelPrefab, levelContainer.transform);
                    
                    foreach (var smoke in _smokeFilter)
                    {
                        var writingSmokeVFXMonoComponent = smoke.Get<WritingSmokeVFXMonoProvider>().MonoComponent;
                        writingSmokeVFXMonoComponent.SetAlpha(smoke.Has<ManualSmokeTagComponent>());
                    }
                    
                    var splineContainer = level.GetComponent<SplineContainer>();
                    planeEnt.Set(new TargetSplineComponent
                    {
                        SplineContainer = splineContainer,
                    });
                    ref var speedCounter = ref planeEnt.Get<SpeedCounterComponent>();
                    speedCounter.Value /= splineContainer.CalculateLength() / level.SpeedScale;
                    break;
                }
            }
        }
    }
}