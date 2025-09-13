using Core.Commands;
using Core.CommonComponents;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Generated;
using SelfishFramework.Src.Unity.UI.Systems;

namespace Core.Features.TutorialFeature.System
{
    [Injectable]
    public sealed partial class TutorialFinishSystem : BaseSystem,
        IReactLocal<TutorialCompletedCommand>
    {
        [Inject] private UIService _uiService;
        [Inject] private TimeScaleService _timeScaleService;
        
        private Single<PlayerProgressComponent> _playerProgressSingle;
        
        public override void InitSystem()
        {
            _playerProgressSingle = new Single<PlayerProgressComponent>(Owner.GetWorld());
        }
        
        void IReactLocal<TutorialCompletedCommand>.ReactLocal(TutorialCompletedCommand command)
        {
            _playerProgressSingle.Get().TutorialPassed = true;
            _timeScaleService.SetTimeScale(1);
            _uiService.CloseUI(UIIdentifierMap.TutorialScreen_UIIdentifier);
        }
    }
}