using Core.CommonComponents;
using Core.Features.LevelFeature.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.Features.PlaneFeature.Systems
{
    public sealed partial class InitPlaneTargetSplineSystem : BaseSystem, IUpdatable
    {
        private Filter _splineFilter;
        private Filter _planeFilter;
        private Filter _smokeFilter;

        public override void InitSystem()
        {
            _splineFilter = World.Filter.With<LevelContainerTagComponent>().Build();
            _smokeFilter = World.Filter.With<WritingSmokeVFXMonoProvider>().Build();
            _planeFilter = World.Filter
                .With<PlaneTagComponent>()
                .Without<TargetSplineComponent>()
                .Build();
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