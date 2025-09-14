using Core.CommonComponents;
using Core.Features.SaveLoadFeature;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class PlayerActor : Actor
    {
        public PlayerProgressComponent PlayerProgressComponent = new();
        public PlayerSettingsComponent PlayerSettingsComponent = new();

        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<SaveLoadSystem>();
        }
    }
}