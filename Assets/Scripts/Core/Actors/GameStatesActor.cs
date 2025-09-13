using Core.Systems.States;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity;
using BootstrapLevelStateSystem = Core.CommonSystems.States.BootstrapLevelStateSystem;
using BootstrapStateSystem = Core.CommonSystems.States.BootstrapStateSystem;
using FinishLevelStateSystem = Core.CommonSystems.States.FinishLevelStateSystem;
using GameStateMachineSystem = Core.CommonSystems.States.GameStateMachineSystem;
using LevelStateSystem = Core.CommonSystems.States.LevelStateSystem;

namespace Core.Actors
{
    public partial class GameStatesActor : Actor
    {
        public GameStateComponent GameStateComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<GameStateMachineSystem>();
            
            Entity.AddSystem<BootstrapStateSystem>();
            Entity.AddSystem<BootstrapLevelStateSystem>();
            Entity.AddSystem<LevelStateSystem>();
            Entity.AddSystem<FinishLevelStateSystem>();
        }
    }
}