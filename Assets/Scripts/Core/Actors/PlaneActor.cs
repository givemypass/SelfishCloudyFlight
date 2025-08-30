using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class PlaneActor : Actor
    {
        public PlaneTagComponent PlaneTagComponent = new();
        public InputListenerTagComponent InputListenerTagComponent = new();
        public SpeedCounterComponent SpeedCounterComponent = new();
        public PositionOnSplineComponent PositionOnSplineComponent = new();

        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<PlaneSplineFlySystem>(); 
            Entity.AddSystem<SkywritingSystem>();
        }
    }
}
