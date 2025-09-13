using Core.CommonComponents;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using PlaneSplineFlySystem = Core.Features.PlaneFeature.Systems.PlaneSplineFlySystem;
using SkywritingSystem = Core.Features.PlaneFeature.Systems.SkywritingSystem;

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
