using Core.CommonSystems.States;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    using Features.GameStatesFeature.Systems;

    public partial class GameStatesActor : Actor
    {
        public GameStateComponent GameStateComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<GameStateMachineSystem>();
            Entity.AddSystem<Features.GameStatesFeature.Systems.States.BootstrapStateSystem>();
            Entity.AddSystem<Features.GameStatesFeature.Systems.States.BootstrapLevelStateSystem>();
            Entity.AddSystem<Features.GameStatesFeature.Systems.States.LevelStateSystem>();
            Entity.AddSystem<Features.GameStatesFeature.Systems.States.FinishLevelStateSystem>();
        }
    }
}