using Core.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine;

namespace Core.Systems
{
    public sealed class InitPlaneTargetSplineSystem : BaseSystem, IUpdatable
    {
        private Filter _splineFilter;
        private Filter _planeFilter;
        private Filter _smokeFilter;

        public override void InitSystem()
        {
            _splineFilter = SManager.World.Filter.With<LevelContainerTagComponent>().Build();
            _planeFilter = SManager.World.Filter
                .With<PlaneTagComponent>()
                .Without<TargetSplineComponent>()
                .Build();
            _smokeFilter = SManager.World.Filter.With<WritingSmokeVFXMonoProvider>().Build();
        }

        public void Update()
        {
            foreach (var planeEnt in _planeFilter)
            {
                foreach (var levelEnt in _splineFilter)
                {
                    // ref var levelComponent = ref levelEnt.Get<LevelComponent>();
                    // var levelPrefab = levelComponent.Level;
                    // var levelContainer = levelEnt.AsActor();
                    // var level = Object.Instantiate(levelPrefab, levelContainer.transform);
                    //
                    // foreach (var smoke in _smokeFilter)
                    // {
                    //     var writingSmokeVFXMonoComponent = smoke.Get<WritingSmokeVFXMonoProvider>().Get;
                    //     writingSmokeVFXMonoComponent.SetAlpha(smoke.Has<ManualSmokeTagComponent>());
                    // }
                    //
                    // var splineContainer = level.GetComponent<SplineContainer>();
                    // planeEnt.Set(new TargetSplineComponent
                    // {
                    //     SplineContainer = splineContainer,
                    // });
                    // planeEnt.Get<SpeedCounterComponent>().AddModifier(Guid.NewGuid(), new DefaultFloatModifier
                    // {
                    //     GetCalculationType = ModifierCalculationType.Divide,
                    //     GetValue = splineContainer.CalculateLength() / level.SpeedScale,
                    //     GetModifierType = ModifierValueType.Value,
                    //     ID = 1,
                    //     ModifierGuid = Guid.NewGuid()
                    // });
                    break;
                }
            }
        }
    }
}