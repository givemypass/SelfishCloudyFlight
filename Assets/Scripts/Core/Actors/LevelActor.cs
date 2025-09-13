using Core.CommonComponents;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Components;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using LinearScoreSystem = Core.Features.ScoreFeature.Systems.LinearScoreSystem;

namespace Core.Actors
{
    public partial class LevelActor : Actor
    {
        public LevelContainerTagComponent LevelContainerTagComponent = new();
        public InputListenerTagComponent InputListenerTagComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<LinearScoreSystem>();
            Entity.AddSystem<TrackReachingMarkersSystem>();
            Entity.AddSystem<ChangeSmokeColorSystem>();
        }
    }
}