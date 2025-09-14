using System;
using SelfishFramework.Src.Core.Components;

namespace Core.CommonComponents
{
    [Serializable]
    public struct PlayerSettingsComponent : IComponent
    {
        public string Language;
    }
}