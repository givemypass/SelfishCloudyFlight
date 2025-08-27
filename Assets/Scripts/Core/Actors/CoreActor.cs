using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<InitPlaneTargetSplineSystem>();
            Entity.AddSystem<SyncSmokeWithPlanePositionSystem>();
            Entity.AddSystem<PlaneKnotsReachingSystem>();
        }
    }
}