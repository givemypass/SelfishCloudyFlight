using Core.Commands;
using Core.Features.ScoreFeature.Components;
using Core.Models;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace Core.Features.ScoreFeature.Systems
{
    [Injectable]
    public sealed partial class LinearScoreMultiplierSystem : BaseSystem,
        IReactGlobal<MarkerReachedCommand>,
        IReactGlobal<MissMarkerCommand>
    {
        [Inject] private GlobalConfigSO _globalConfig;
        
        public override void InitSystem()
        {
        }
        
        void IReactGlobal<MarkerReachedCommand>.ReactGlobal(MarkerReachedCommand command)
        {
            ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
            scoreComponent.NeedEmitting = !command.MarkerIsEnd;
            scoreComponent.Multiplier = 0;
        }

        void IReactGlobal<MissMarkerCommand>.ReactGlobal(MissMarkerCommand command)
        {
            ref var scoreComponent = ref Owner.Get<LinearScoreComponent>();
            scoreComponent.Multiplier = _globalConfig.Get.ScoreNegativeMultiplier;
        }
    }
}