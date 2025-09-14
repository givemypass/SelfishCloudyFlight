using Core.Features.TutorialFeature.Components;
using Core.Features.TutorialFeature.System;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.UI.Actors;

namespace Core.Features.TutorialFeature
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