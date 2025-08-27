using UnityEngine;

namespace Components.MonoBehaviourComponents
{
    public class FreezeRotationMonoComponent : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;            
        }
    }
}