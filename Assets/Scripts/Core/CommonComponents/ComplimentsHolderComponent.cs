using System;
using Core.MonoBehaviourComponents;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace Core.CommonComponents
{
    [Serializable]
    public struct ComplimentsHolderComponent : IComponent
    {
        [Serializable]
        public struct ComplimentSpritesPerLanguage
        {
            public string Language;
            public Sprite[] Sprites;
        }
    
        [Serializable]
        public struct ComplimentData
        {
            public ComplimentSpritesPerLanguage[] Compliments;
            public ParticleSystem ParticleSystem;
            public float RotationRange;
            public Vector2 ScaleRange;
        }
    
        public ComplimentMonoComponent Prefab;
    
        public ComplimentData PassSuccessfully;
        public ComplimentData Hit;
        public ComplimentData Miss;
    
        public float SpawnOffset;
        public ParticleSystem CompletedParticle;
    }
}