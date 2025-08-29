using System;
using System.Collections.Generic;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core.Components;

namespace Core.Components
{
    [Serializable]
    public struct StartEndMarkersComponent : IComponent
    {
        public struct Point
        {
            public int Index;
            public float TPos;
            public bool IsStart;
        }
        
        public Queue<StartEndMarkerUIMonoComponent> Markers;
        public Queue<Point> Points;
    }
}