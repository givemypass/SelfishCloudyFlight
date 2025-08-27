using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Components.MonoBehaviourComponents
{
    public class ComplimentMonoComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        private Tween tween;

        public Image Image => image;

        public void SetTween(Tween tweenToPlay)
        {
            tween = tweenToPlay;
        }

        private void OnDestroy()
        {
            tween?.Kill();
            tween = null;
        }
    }
}