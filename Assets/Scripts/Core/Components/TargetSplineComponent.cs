using SelfishFramework.Src.Core.Components;
using UnityEngine.Splines;

namespace Skywriting.Components
{
    public struct TargetSplineComponent : IComponent
    {
        public SplineContainer SplineContainer;

        public int GetCurrentIndexOnSpline(float tPos)
        {
            var currentIndex = (int) (tPos / (1f / SplineContainer.transform.childCount));
            return currentIndex;
        }

        public float GetTPosByIndex(int index)
        {
            var tPos = index * (1f / SplineContainer.transform.childCount);
            return tPos;
        }
    }
}