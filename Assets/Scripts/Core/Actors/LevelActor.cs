using Core.CommonComponents;
using Core.Features.ScoreFeature.Components;
using Core.Features.ScoreFeature.Systems;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
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