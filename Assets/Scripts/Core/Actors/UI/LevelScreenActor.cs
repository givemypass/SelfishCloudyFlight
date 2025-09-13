using Core.CommonComponents;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.UI.Actors;
using ComplimentsUISystem = Core.Features.ScoreFeature.Systems.ComplimentsUISystem;
using LevelProgressUISystem = Core.Features.ScoreFeature.Systems.LevelProgressUISystem;

namespace Core.Actors.UI
{
    public partial class LevelScreenActor : UIActor
    {
        public InputListenerTagComponent InputListenerTagComponent;
        public StartEndMarkersComponent StartEndMarkersComponent;
        public ComplimentsHolderComponent ComplimentsHolderComponent;
        
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