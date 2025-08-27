using UnityEngine;

namespace Core.MonoBehaviourComponents
{
    [ExecuteInEditMode]
    public class BackgroundScalerOrthographic : MonoBehaviour
    {
        private void Start()
        {
            ScaleBackground();
        }

        private void OnValidate()
        {
            ScaleBackground();
        }

        private void ScaleBackground()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null) return;

            var camera = Camera.main;

            // Line up sprite with camera
            transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y,
                transform.position.z);

            // Get viewport sizes
            float worldScreenHeight = camera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            // Scale sprite
            var spriteSize = sr.sprite.bounds.size;
            Vector3 scale = Vector3.one;
            var x = worldScreenWidth / spriteSize.x;
            var y = worldScreenHeight / spriteSize.y;
            if(!float.IsNaN(x))
                scale.x = x;
            if(!float.IsNaN(y))
                scale.y = y;

            transform.localScale = scale;
        }
    }
}
    