using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using SelfishFramework.Src.Unity.Features.InputFeature.Systems;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        public LevelsHolderComponent LevelsHolderComponent = new();
        public SceneManagerTagComponent SceneManagerTagComponent = new();
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