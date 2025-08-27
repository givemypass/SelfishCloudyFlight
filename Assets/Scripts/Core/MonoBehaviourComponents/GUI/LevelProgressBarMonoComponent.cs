using Unity.Mathematics;
using UnityEngine;

namespace Components.MonoBehaviourComponents
{
    public class LevelProgressBarMonoComponent : MonoBehaviour
    {
        [SerializeField] private float halfAnchorPos;
        [SerializeField] private float speed = 1f;
        
        private RectTransform rectTransform;
        private float targetProgress;


        private void Awake()
        {
            rectTransform = transform as RectTransform;
            targetProgress = 0;
            SetProgress(0);
        }

        private void Update()
        {
            var progress = Mathf.MoveTowards(rectTransform.anchorMax.x, targetProgress, Time.deltaTime * speed);
            SetProgress(progress);
        }

        public void SetProgressAnimated(float progress)
        {
            // progress = math.remap(0, 1, 0, halfAnchorPos * 2, progress);
            progress = math.clamp(progress, 0, 1);
            targetProgress = progress;
        }

        private void SetProgress(float val)
        {
            rectTransform.anchorMax = new Vector2(val, 1);
            SetLeft(0);
            SetRight(0);
        }

        public void SetLeft(float left)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        public void SetRight(float right)
        {
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        } 
    }
}