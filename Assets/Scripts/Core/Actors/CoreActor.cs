using Core.Systems;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class CoreActor : Actor 
    {
        protected override void SetSystems()
        {
            Entity.AddSystem<InitPlaneTargetSplineSystem>();
        }
    }
}