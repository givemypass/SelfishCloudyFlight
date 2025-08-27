using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public struct UnityTransformComponent : IComponent
    {
        [NonSerialized]
        public Transform Transform;
    }
}