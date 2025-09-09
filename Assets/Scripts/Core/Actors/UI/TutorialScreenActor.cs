using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.UI.Actors;

namespace Core.Actors.UI
{
    public partial class TutorialScreenActor : UIActor
    {
        public InputListenerTagComponent InputListenerTagComponent = new();
        public TutorialUITagComponent TutorialUITagComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<TutorialScreenUISystem>();
        }
    }
}