using Core.Features.ColorsFeature.Systems;
using Core.Features.LevelFeature.Components;
using Core.Features.LevelFeature.Systems;
using Core.Features.ScoreFeature.Components;
using Core.Features.ScoreFeature.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Features.LevelFeature
{
    public partial class LevelActor : Actor
    {
        public LevelContainerTagComponent LevelContainerTagComponent = new();
        public LinearScoreComponent LinearScoreComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LinearScoreInitializeSystem>();
            Entity.AddSystem<LinearScoreMultiplierSystem>();
            Entity.AddSystem<LinearScoreIncrementSystem>();
            
            Entity.AddSystem<TrackReachingMarkersSystem>();
            Entity.AddSystem<ChangeSmokeColorSystem>();
        }
    }
}