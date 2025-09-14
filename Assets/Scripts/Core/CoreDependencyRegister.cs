using Core.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.AssetsManagement;
using SelfishFramework.Src.Unity.UI.Systems;
using UnityEngine;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        [SerializeField] private GlobalConfigSO _globalConfig;
        [SerializeField] private LevelsConfigSO _levelsConfig;
        
        public override void Register()
        {
            var world = SManager.World;
            var container = world.DependencyContainer;
            container.Register(new SceneService());
            container.Register(new AssetsService());
            container.Register(new ActorPoolingService());
            container.Register(new UIService(world));
            container.Register(new TimeScaleService());
            container.Register(_globalConfig);
            container.Register(_levelsConfig);
        }
    }
}