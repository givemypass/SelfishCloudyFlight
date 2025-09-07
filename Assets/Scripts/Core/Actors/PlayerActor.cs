using Core.Components;
using SelfishFramework.Src.Unity;

namespace Core.Actors
{
    public partial class PlayerActor : Actor
    {
        public PlayerProgressComponent PlayerProgressComponent = new();
        public PlayerSettingsComponent PlayerSettingsComponent = new();
    }
}