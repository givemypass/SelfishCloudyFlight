using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class LevelActor : Actor
    {
        public LevelComponent LevelComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LinearScoreSystem>();
            Entity.AddSystem<TrackReachingMarkersSystem>();
        }
    }
}