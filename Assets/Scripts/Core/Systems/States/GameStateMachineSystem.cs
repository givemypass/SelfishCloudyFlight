﻿using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Features.GameFSM.Systems;
using SelfishFramework.Src.Unity.Generated;

namespace Core.Systems.States
{
    public sealed partial class GameStateMachineSystem : BaseMainGameLogicSystem, ILateStart
    {
        public override void InitSystem() { }

        public void LateStart()
        {
            ChangeGameState(GameStateIdentifierMap.BootstrapState); 
        }

        public override void ProcessEndState(EndGameStateCommand endGameStateCommand)
        {
            switch (endGameStateCommand.GameState)
            {
                case GameStateIdentifierMap.BootstrapState:
                    ChangeGameState(GameStateIdentifierMap.BootstrapLevelState);
                    break;
                case GameStateIdentifierMap.BootstrapLevelState:
                    ChangeGameState(GameStateIdentifierMap.LevelState);
                    break;
                case GameStateIdentifierMap.LevelState:
                    ChangeGameState(GameStateIdentifierMap.FinishLevelState);
                    break;
                case GameStateIdentifierMap.FinishLevelState:
                    ChangeGameState(GameStateIdentifierMap.BootstrapLevelState);
                    break;
            } 
        }
    }
}