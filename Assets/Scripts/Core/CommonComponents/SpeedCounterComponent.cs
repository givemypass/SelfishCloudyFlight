using System;
using SelfishFramework.Src.Core.Components;

namespace Core.CommonComponents
{
    [Serializable]
    public struct SpeedCounterComponent : IComponent
    {
        public float Value;
    }
}