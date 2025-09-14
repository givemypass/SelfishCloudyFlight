using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/LevelsConfig", fileName = "LevelsConfigSO", order = 0)]
    public class LevelsConfigSO : ScriptableObject
    {
        [SerializeField] private LevelsConfig _config; 
        public LevelsConfig Get => _config;
    }
}