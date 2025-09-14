using Core.Features.TutorialFeature.Components;
using Core.Features.TutorialFeature.System;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Features.TutorialFeature
{
    public partial class TutorialManagerActor : Actor
    {
        public TutorialManagerTagComponent TutorialManagerTagComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<TutorialSystem>();
            Entity.AddSystem<TutorialFinishSystem>();
        }
    }
}