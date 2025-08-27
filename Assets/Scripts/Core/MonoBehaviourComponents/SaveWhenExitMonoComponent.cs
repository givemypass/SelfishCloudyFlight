using UnityEngine;

namespace Core.MonoBehaviourComponents
{
    public class SaveWhenExitMonoComponent : MonoBehaviour
    {
#if !UNITY_EDITOR
        private void OnApplicationPause(bool onPause)
        {
            if (!onPause)
                return;
            if (EntityManager.IsAlive &&
                EntityManager.Default.TryGetSingleComponent(out GameStateComponent gameStateComponent) &&
                gameStateComponent.CurrentState != 0)
            {
                EntityManager.Default.Command(new BeforeSaveCommand());
                EntityManager.Default.Command(new SaveCommand());
            }
        }
#endif
#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            //todo make after implementing commands
            // EntityManager.Default.Command(new BeforeSaveCommand());
            // EntityManager.Default.Command(new SaveCommand());
        }
#endif
    }
}