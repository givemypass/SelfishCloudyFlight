using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.UI.Actors;

namespace Core.Actors.UI
{
    public partial class LevelScreenActor : UIActor
    {
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<StartEndMarkersSystem>();
        }
    }
}