using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Skywriting.Components
{
    public struct UnityTransformComponent : IComponent
    {
        [NonSerialized]
        public Transform Transform;
    }
}