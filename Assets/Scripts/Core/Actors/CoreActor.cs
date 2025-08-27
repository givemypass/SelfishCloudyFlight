using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        public LevelsHolderComponent LevelsHolderComponent = new();
        public GlobalConfigComponent GlobalConfigComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<InitPlaneTargetSplineSystem>();
            Entity.AddSystem<SyncSmokeWithPlanePositionSystem>();
            Entity.AddSystem<PlaneKnotsReachingSystem>();
        }
    }
}