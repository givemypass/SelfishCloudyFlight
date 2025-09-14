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