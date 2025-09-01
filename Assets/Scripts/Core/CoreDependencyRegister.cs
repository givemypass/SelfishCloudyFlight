using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        public override void Register()
        {
            var container = SManager.World.DependencyContainer;
            container.Registry(new SceneService());
        }
    }
}