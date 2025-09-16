using Core.CommonComponents;
using Core.Features.LevelFeature.Components;
using Core.Features.TutorialFeature.Commads;
using Core.Features.TutorialFeature.Components;
using Core.Services;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;

namespace Core.Features.TutorialFeature.System
{
    [Injectable]
    public sealed partial class TutorialSystem : BaseSystem,
        IUpdatable,
        IReactGlobal<TransitionGameStateCommand>
    {
        private const int TUTORIAL_MARKER_OFFSET = 2;
        
        [Inject] private UIService _uiService;
        [Inject] private TimeScaleService _timeScaleService;
        
        private Single<PlayerProgressComponent> _playerProgressSingle;
        
        private Filter _planeFilter;
        private Filter _startEndMarkerFilter;
        private Filter _tutorialUiFilter;

        public override void InitSystem()
        {
            var world = World;
            _planeFilter = world.Filter.With<PlaneTagComponent>().With<TargetSplineComponent>().Build();
            _startEndMarkerFilter = world.Filter.With<StartEndMarkersComponent>().Build();
            _tutorialUiFilter = world.Filter
                .With<TutorialUITagComponent>()
                .Without<TutorialIsActiveComponent>().Build();
            _playerProgressSingle = new Single<PlayerProgressComponent>(world);
        }

        void IReactGlobal<TransitionGameStateCommand>.ReactGlobal(TransitionGameStateCommand command)
        {
            if (command.To != GameStateIdentifierMap.BootstrapLevelState || _playerProgressSingle.Get().TutorialPassed)
            {
                return;
            }

            _uiService.ShowUIAsync(UIIdentifierMap.TutorialScreen_UIIdentifier).Forget();
        }

        public void Update()
        {
            foreach (var tutorialUIEntity in _tutorialUiFilter)
            {
                foreach (var plane in _planeFilter)
                {
                    ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
                    var tPos = plane.Get<PositionOnSplineComponent>().TPos;
                    var index = targetSplineComponent.GetCurrentIndexOnSpline(tPos);

                    foreach (var ent in _startEndMarkerFilter)
                    {
                        ref var startEndMarkerComponent = ref ent.Get<StartEndMarkersComponent>();
                        if (!startEndMarkerComponent.Markers.TryPeek(out var marker))
                            continue;
                        var markerIndexPlace = marker.IndexPlace;
                        if (markerIndexPlace - TUTORIAL_MARKER_OFFSET == index)
                        {
                            _timeScaleService.SetTimeScale(0);
                            tutorialUIEntity.Command(new TutorialActivateCommand());
                            return;
                        }
                    }
                }
            }
        }
    }
}