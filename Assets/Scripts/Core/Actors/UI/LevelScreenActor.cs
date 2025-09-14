using Core.CommonComponents;
using Core.CommonSystems;
using Core.Features.ColorsFeature;
using Core.Features.ScoreFeature.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.UI.Actors;

namespace Core.Actors.UI
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
            Entity.AddSystem<Features.LevelFeature.Systems.StartEndMarkersSystem>();
            Entity.AddSystem<ComplimentsUISystem>();
            Entity.AddSystem<LevelProgressUISystem>();
            Entity.AddSystem<ColorsUISystem>();
        }
    }
}