using System;
using Core.Models;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public struct GlobalConfigComponent : IComponent
    {
        [SerializeField] private GlobalConfig _config;
        
        public GlobalConfig Get => _config;
    }
}