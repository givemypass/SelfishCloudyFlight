using System;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Unity;

namespace Core.CommonComponents
{
    [Serializable]
    public struct ActorsHolderComponent : IComponent
    {
        public Actor LevelActorPrefab;
    }
}