using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using Skywriting.Components;
using Skywriting.Systems;

namespace Skywriting.Actors
{
    public partial class PlaneActor : Actor
    {
        public PlaneTagComponent PlaneTagComponent = new();

        protected override void SetSystems()
        {
            Entity.AddSystem<PlaneSplineFlySystem>(); 
        }
    }
}
