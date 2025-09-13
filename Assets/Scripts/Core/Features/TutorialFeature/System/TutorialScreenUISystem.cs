using Core.Commands;
using Core.CommonCommands;
using Core.Features.TutorialFeature.Components;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;

namespace Core.Features.TutorialFeature.System
{
    public sealed partial class TutorialScreenUISystem : BaseSystem,
        IReactLocal<ActivateCommand>,
        IReactGlobal<PlaneEmittingUpdated>
    {
        private TutorialUIMonoComponent _monoComponent;
        private Filter _tutorialManagerFilter;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _monoComponent.CanvasGroup.alpha = 0;
            
            _tutorialManagerFilter = Owner.GetWorld().Filter
                .With<TutorialManagerTagComponent>()
                .Build();
        }

        void IReactLocal<ActivateCommand>.ReactLocal(ActivateCommand command)
        {
            _monoComponent.Activate();
            Owner.Set(new TutorialIsActiveComponent());
        }

        void IReactGlobal<PlaneEmittingUpdated>.ReactGlobal(PlaneEmittingUpdated command)
        {
            if (!command.Status || !Owner.Has<TutorialIsActiveComponent>())
                return;
            foreach (var entity in _tutorialManagerFilter) entity.Command(new TutorialCompletedCommand());
        }
    }
}