using Core.Components;
using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Components;
using SelfishFramework.Src.Unity.Systems;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        public LevelsHolderComponent LevelsHolderComponent = new();
        public GlobalConfigComponent GlobalConfigComponent = new();
        public SceneManagerTagComponent SceneManagerTagComponent = new();
        public InputActionsComponent InputActionsComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<InitPlaneTargetSplineSystem>();
            Entity.AddSystem<SyncSmokeWithPlanePositionSystem>();
            Entity.AddSystem<PlaneKnotsReachingSystem>();
            Entity.AddSystem<SceneManagerSystem>();
            Entity.AddSystem<InputListenSystem>();
        }
    }
}