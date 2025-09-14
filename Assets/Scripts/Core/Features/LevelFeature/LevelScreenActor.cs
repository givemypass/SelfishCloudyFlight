using Core.Features.ColorsFeature.Systems;
using Core.Features.LevelFeature.Components;
using Core.Features.LevelFeature.Systems;
using Core.Features.ScoreFeature.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.UI.Actors;

namespace Core.Features.LevelFeature
{
    public partial class LevelScreenActor : UIActor
    {
        public InputListenerTagComponent InputListenerTagComponent = new();
        public ComplimentsHolderComponent ComplimentsHolderComponent = new();
        public StartEndMarkersComponent StartEndMarkersComponent = new()
        {
            Markers = new(),
        };
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<StartEndMarkersSystem>();
            Entity.AddSystem<ComplimentsUISystem>();
            Entity.AddSystem<LevelProgressUISystem>();
            Entity.AddSystem<ColorsUISystem>();
        }
    }
}