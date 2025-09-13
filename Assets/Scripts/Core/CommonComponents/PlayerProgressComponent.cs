using System;
using SelfishFramework.Src.Core.Components;

namespace Core.CommonComponents
{
    [Serializable]
    public struct PlayerProgressComponent : IComponent
    {
        public int LevelIndex;
        public bool TutorialPassed;
    }
}