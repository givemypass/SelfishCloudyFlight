using Core.Commands;
using Core.Components;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;
using UnityEngine;

namespace Core.Systems
{
    [Injectable]
    public sealed partial class TutorialSystem : BaseSystem, IReactGlobal<TransitionGameStateCommand>, IReactLocal<TutorialCompletedCommand>, IUpdatable
    {
        [Inject] private UIService _uiService;
        
        private Filter _planeFilter;
        private Filter _startEndMarkerFilter;
        private Filter _tutorialUiFilter;
        private Single<PlayerProgressComponent> _playerProgressSingle;

        public override void InitSystem()
        {
            var world = Owner.GetWorld();
            _planeFilter = world.Filter.With<PlaneTagComponent>().With<TargetSplineComponent>().Build();
            _startEndMarkerFilter = world.Filter.With<StartEndMarkersComponent>().Build();
            _tutorialUiFilter = world.Filter
                .With<TutorialUITagComponent>()
                .Without<TutorialIsActiveComponent>().Build();
            _playerProgressSingle = new Single<PlayerProgressComponent>(world); 
        }

        void IReactGlobal<TransitionGameStateCommand>.ReactGlobal(TransitionGameStateCommand command)
        {
            if (command.From == GameStateIdentifierMap.LevelState)
            {
                HideUI();
            }
            
            if (command.To == GameStateIdentifierMap.BootstrapLevelState)
            {
                if(!_playerProgressSingle.Get().TutorialPassed)
                    ShowUI();
            }
        }

        void IReactLocal<TutorialCompletedCommand>.ReactLocal(TutorialCompletedCommand command)
        {
            _playerProgressSingle.Get().TutorialPassed = true;
            SetTimeScale(1);
            HideUI(); 
        }
        
        private static void SetTimeScale(int targetTimeScale)
        {
            DOVirtual
                .Float(Time.timeScale, targetTimeScale, 0.1f, value => Time.timeScale = value)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
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
                        if(!startEndMarkerComponent.Markers.TryPeek(out var marker))
                            continue;
                        var markerIndexPlace = marker.IndexPlace;
                        if (markerIndexPlace - 2 == index)
                        {
                            SetTimeScale(0);
                            tutorialUIEntity.Command(new ActivateCommand());
                            tutorialUIEntity.Set(new TutorialIsActiveComponent());
                            return;
                        }
                    }
                }
            }
        }
        
        private void ShowUI()
        {
            _uiService.ShowUIAsync(UIIdentifierMap.TutorialScreen_UIIdentifier).Forget();
        }

        private void HideUI()
        {
            _uiService.CloseUI(UIIdentifierMap.TutorialScreen_UIIdentifier);
        }
    }
}