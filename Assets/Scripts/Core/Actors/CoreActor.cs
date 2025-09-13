using Core.CommonComponents;
using Core.Features.PlaneFeature.Systems;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.Features.InputFeature.Systems;
using InitPlaneTargetSplineSystem = Core.Features.PlaneFeature.Systems.InitPlaneTargetSplineSystem;
using PlaneKnotsReachingSystem = Core.CommonSystems.PlaneKnotsReachingSystem;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        public LevelsHolderComponent LevelsHolderComponent = new();
        public InputActionsComponent InputActionsComponent = new();
        public ActorsHolderComponent ActorsHolderComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<InitPlaneTargetSplineSystem>();
            Entity.AddSystem<SyncSmokeWithPlanePositionSystem>();
            Entity.AddSystem<PlaneKnotsReachingSystem>();
            Entity.AddSystem<InputListenSystem>();
        }
    }
}