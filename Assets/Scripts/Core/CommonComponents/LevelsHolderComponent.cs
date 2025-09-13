using System;
using Core.MonoBehaviourComponents;
using SelfishFramework.Src.Core.Components;

namespace Core.CommonComponents
{
    [Serializable]
    public struct LevelsHolderComponent : IComponent
    {
        public LevelMonoComponent[] Levels;
        
        public bool TryGetLevel(int index, out LevelMonoComponent level)
        {
            if (index < Levels.Length)
            {
                level = Levels[index];
                return true;
            }

            level = null;
            return false;
        }
        
        public int GetNextLevelIndex(int currentLevelIndex)
        {
            return (currentLevelIndex + 1) % Levels.Length;
        }
        
        public int GetPreviousLevelIndex(int currentLevelIndex)
        {
            if (currentLevelIndex - 1 >= 0)
                return currentLevelIndex - 1;
            return Levels.Length - 1;
        } 
    }
}