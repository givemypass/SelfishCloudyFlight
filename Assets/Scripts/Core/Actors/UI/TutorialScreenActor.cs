using Core.CommonComponents;
using Core.Features.TutorialFeature.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.UI.Actors;
using TutorialScreenUISystem = Core.Features.TutorialFeature.System.TutorialScreenUISystem;

namespace Core.Actors.UI
{
    public partial class TutorialScreenActor : UIActor
    {
        public TutorialUITagComponent TutorialUITagComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<TutorialScreenUISystem>();
        }
    }
}