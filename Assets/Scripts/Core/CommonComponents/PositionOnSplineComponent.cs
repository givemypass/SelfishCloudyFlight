using System;
using SelfishFramework.Src.Core.Components;

namespace Core.CommonComponents
{
    [Serializable]
    public struct PositionOnSplineComponent : IComponent
    {
        public float TPos;
        public int NextKnotIndex;
        public float NextKnotTPos;
    }
}