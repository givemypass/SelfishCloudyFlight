using Core.CommonComponents;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors.UI
{
    public partial class TutorialManagerActor : Actor
    {
        public TutorialManagerTagComponent TutorialManagerTagComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<TutorialSystem>();
        }
    }
}