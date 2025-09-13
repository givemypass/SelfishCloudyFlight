using Core.Commands;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity.Generated;
using Systems;
using Application = UnityEngine.Device.Application;

namespace Core.Systems.States
{
    public sealed partial class BootstrapStateSystem : BaseGameStateSystem, IGlobalStart
    {
        public override void InitSystem() { }
    
        public void GlobalStart()
        {
        }
    
        protected override int State => GameStateIdentifierMap.BootstrapState;
    
        protected override void ProcessState(int from, int to)
        {
            Application.targetFrameRate = 60;
            SManager.World.Command(new LoadProgressCommand());
            EndState();
        }
    }
}