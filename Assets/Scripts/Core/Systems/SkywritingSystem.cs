using Core.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace Core.Systems
{
    public sealed class SkywritingSystem : BaseSystem, IUpdatable
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
                    // writingSmokeVFXMonoProvider.Get.Play();
                }
                else
                {
                    // writingSmokeVFXMonoProvider.Get.Pause();
                }
            }
        }
        
        // public void CommandReact(InputStartedCommand command)
        // {
        //     if (command.Index != InputIdentifierMap.Tap)
        //         return;
        //     _isActive = true;
        // }
        //
        // public void CommandReact(InputEndedCommand command)
        // {
        //     if (command.Index != InputIdentifierMap.Tap)
        //         return;
        //     _isActive = false;
        // } 
    }
}