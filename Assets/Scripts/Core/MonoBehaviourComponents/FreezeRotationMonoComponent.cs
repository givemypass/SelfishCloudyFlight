using UnityEngine;

namespace Core.MonoBehaviourComponents
{
    public class FreezeRotationMonoComponent : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;            
        }
    }
}