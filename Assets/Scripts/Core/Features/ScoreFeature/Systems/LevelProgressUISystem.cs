using Core.CommonComponents;
using Core.Features.LevelFeature.Commands;
using Core.Features.LevelFeature.Components;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.ScoreFeature.Systems
{
    public sealed partial class LevelProgressUISystem : BaseSystem,
        IAfterEntityInit,
        IReactGlobal<LevelProgressUpdatedCommand>
    {
        private LevelProgressBarMonoComponent _monoComponent;
        
        private Filter _levelFilter;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent, true);
            _levelFilter = World.Filter.With<LevelComponent>().Build();
        }

        public void AfterEntityInit()
        {
            _monoComponent.SetProgressAnimated(0);     
        }

        public void ReactGlobal(LevelProgressUpdatedCommand command)
        {
            UpdateProgress(); 
        }
        
        private void UpdateProgress()
        {
            foreach (var level in _levelFilter)
            {
                ref var levelComponent = ref level.Get<LevelComponent>();
                
                var progress = levelComponent.LevelProgress;
                _monoComponent.SetProgressAnimated(progress);
            }
        }
    }
}