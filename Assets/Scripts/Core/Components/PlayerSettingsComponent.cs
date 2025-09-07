using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Components
{
    [Serializable]
    public struct PlayerSettingsComponent : IComponent
    {
        public string Language;
    }
}