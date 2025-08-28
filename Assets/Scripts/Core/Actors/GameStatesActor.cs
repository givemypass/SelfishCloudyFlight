using Core.Systems.States;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class GameStatesActor : Actor
    {
        public GameStateComponent GameStateComponent = new();
        
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<GameStateMachineSystem>();
        }
    }
}