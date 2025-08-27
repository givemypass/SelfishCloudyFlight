using Core.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;

using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Commands;

namespace Core.Systems
{
    public sealed partial class SkywritingSystem : BaseSystem, IUpdatable, IReactLocal<InputStartedCommand>, IReactLocal<InputEndedCommand>
    {
        private Filter _smokeFilter;
        private bool _isActive;

        public override void InitSystem()
        {
            _smokeFilter = SManager.World.Filter
                .With<WritingSmokeVFXMonoProvider>()
                .With<ManualSmokeTagComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var smoke in _smokeFilter)
            {
                ref var writingSmokeVFXMonoProvider = ref smoke.Get<WritingSmokeVFXMonoProvider>();
                if (_isActive)
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
            // if (command.Index != InputIdentifierMap.Tap)
                // return;
            _isActive = true;
        }

        public void ReactLocal(InputEndedCommand command)
        {
            // if (command.Index != InputIdentifierMap.Tap)
                // return;
            _isActive = false;
        }
    }
}