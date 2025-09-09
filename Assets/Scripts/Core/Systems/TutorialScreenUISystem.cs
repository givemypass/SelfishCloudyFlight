using Core.Commands;
using Core.Components;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.InputFeature.Commands;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Systems
{
    public sealed partial class TutorialScreenUISystem : BaseSystem, IReactLocal<InputStartedCommand>, IReactLocal<ActivateCommand>, IGlobalStart
    {
        private TutorialUIMonoComponent _monoComponent;
        private Filter _tutorialManagerFilter;

        public override void InitSystem()
        {
            _tutorialManagerFilter = Owner.GetWorld().Filter.With<TutorialManagerTagComponent>().Build();
        }

        public void GlobalStart()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _monoComponent.CanvasGroup.alpha = 0;
        }

        void IReactLocal<InputStartedCommand>.ReactLocal(InputStartedCommand command)
        {
            if (command.Index != InputIdentifierMap.Tap)
                return;
            foreach (var entity in _tutorialManagerFilter)
            {
                entity.Command(new TutorialCompletedCommand());
            }
        }

        void IReactLocal<ActivateCommand>.ReactLocal(ActivateCommand command)
        {
            _monoComponent.Activate();
        }
    }
}