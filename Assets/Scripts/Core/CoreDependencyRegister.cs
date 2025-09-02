using AssetsManagement;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.UI.Systems;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        public override void Register()
        {
            var world = SManager.World;
            var container = world.DependencyContainer;
            container.Register(new SceneService());
            container.Register(new AssetsService());
            container.Register(new UIService(world));
        }
    }
}