using System;
using SelfishFramework.Src.Core.Components;

namespace Core.Features.ScoreFeature.Components
{
    [Serializable]
    public struct LinearScoreComponent : IComponent
    {
        public bool NeedEmitting;
        public float SpeedMultiplier;
        public float StopPos;
        public float Multiplier;
    }
}