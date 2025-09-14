using Core.Features.PlaneFeature.Commands;
using Core.Features.TutorialFeature.Commads;
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
        IReactLocal<TutorialActivateCommand>,
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

        void IReactLocal<TutorialActivateCommand>.ReactLocal(TutorialActivateCommand command)
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