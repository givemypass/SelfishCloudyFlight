using Core.CommonComponents;
using Core.Features.PlaneFeature.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Features.InputFeature.Commands;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Features.PlaneFeature.Systems
{
    public sealed partial class SkywritingSystem : BaseSystem, IUpdatable, IReactLocal<InputStartedCommand>, IReactLocal<InputEndedCommand>
    {
        private Filter _smokeFilter;

        public override void InitSystem()
        {
            _smokeFilter = Owner.GetWorld().Filter
                .With<WritingSmokeVFXMonoProvider>()
                .With<ManualSmokeTagComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var smoke in _smokeFilter)
            {
                ref var writingSmokeVFXMonoProvider = ref smoke.Get<WritingSmokeVFXMonoProvider>();
                if (Owner.Has<PlaneIsEmittingComponent>())
                {
                    writingSmokeVFXMonoProvider.MonoComponent.Play();
                }
                else
                {
                    writingSmokeVFXMonoProvider.MonoComponent.Pause();
                }
            }
        }
        
        public void ReactLocal(InputStartedCommand command)
        {
            if (command.Index != InputIdentifierMap.Tap)
                return;
            Owner.Set(new PlaneIsEmittingComponent());
            Owner.GetWorld().Command(new PlaneEmittingUpdated
            {
                Status = true,
            });
        }

        public void ReactLocal(InputEndedCommand command)
        {
            if (command.Index != InputIdentifierMap.Tap)
                return;
            Owner.Remove<PlaneIsEmittingComponent>();
            Owner.GetWorld().Command(new PlaneEmittingUpdated
            {
                Status = false,
            });           
        }
    }
}