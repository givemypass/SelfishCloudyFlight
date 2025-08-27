using System;
using Core.MonoBehaviourComponents;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Unity;

namespace Core.Components
{
    [Serializable]
    public struct WritingSmokeVFXMonoProvider : IComponent
    {
        public WritingSmokeVFXMonoComponent MonoComponent;
    }
}