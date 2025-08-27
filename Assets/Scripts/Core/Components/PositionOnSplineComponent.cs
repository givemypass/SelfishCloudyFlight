using SelfishFramework.Src.Core.Components;

namespace Skywriting.Components
{
    public struct PositionOnSplineComponent : IComponent
    {
        public float TPos;
        public int NextKnotIndex;
        public float NextKnotTPos;
    }
}