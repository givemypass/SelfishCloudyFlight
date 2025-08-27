using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Components
{
    [Serializable]
    public struct PositionOnSplineComponent : IComponent
    {
        public float TPos;
        public int NextKnotIndex;
        public float NextKnotTPos;
    }
}