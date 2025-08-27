using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Components
{
    [Serializable]
    public struct SpeedCounterComponent : IComponent
    {
        public float Value;
    }
}