using System;
using Core.MonoBehaviourComponents;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.LevelFeature.Components
{
    [Serializable]
    public struct LevelComponent : IComponent
    {
        private float _levelProgress;
        public float LevelProgress
        {
            get => _levelProgress;
            set
            {
                _levelProgress = value;
                if (_levelProgress < 0)
                    _levelProgress = 0;
            }
        }

        public LevelMonoComponent Level;
    }
}