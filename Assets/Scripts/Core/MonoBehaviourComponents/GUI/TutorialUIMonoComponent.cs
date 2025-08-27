using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.MonoBehaviourComponents.GUI
{
    public class TutorialUIMonoComponent : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public TextMeshProUGUI TapText;
        
        private const float TWEEN_SCALE = 0.9f;

        private Sequence tween;
        public void Activate()
        {
            tween?.Kill();
            tween = DOTween.Sequence();
            tween
                .Join(CanvasGroup.DOFade(1, 0.5f))
                .Join(TapText.transform.DOScale(new Vector3(TWEEN_SCALE, TWEEN_SCALE, 1f), 1f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(100, LoopType.Yoyo)
                );
        }

        private void OnDestroy()
        {
            tween?.Kill();
            tween = null;
        }
    }
}