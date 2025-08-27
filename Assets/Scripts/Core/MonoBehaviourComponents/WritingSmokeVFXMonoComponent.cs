using DG.Tweening;
using UnityEngine;

namespace Core.MonoBehaviourComponents
{
    public class WritingSmokeVFXMonoComponent : MonoBehaviour
    {
        [SerializeField] private float zOffset;
        [SerializeField] private ParticleSystem[] particles;

        private bool status;
        private Sequence fadeTween;

        public float ZOffset => zOffset;
        public float Scale { get; private set; } = 1;

        private void Awake()
        {
            status = true;
            Pause();
        }

        public void Play()
        {
            if (status)
                return;
            
            status = true;
            foreach (var particle in particles)
            {
                var module = particle.emission;
                module.enabled = true;
            }
        }

        public void Pause()
        {
            if (!status)
                return;
            
            status = false;
            foreach (var particle in particles)
            {
                var module = particle.emission;
                module.enabled = false;
            }
        }

        public void ApplyScale(float scale)
        {
            Scale *= scale;
            foreach (var particle in particles)
            {
                particle.transform.localScale *= scale;
                var emission = particle.emission;
                var rate = emission.rateOverDistance;
                rate.constant /= scale;
                emission.rateOverDistance = rate;
            }
        }

        public void SetAlpha(bool value)
        {
            fadeTween?.Kill();
            fadeTween = DOTween.Sequence();
            foreach (var particle in particles)
            {
                fadeTween.Join(particle.GetComponent<Renderer>().material.DOFade(value ? 1f : 0f, 0.3f));
            }
        }

        public void SetColor(Color color)
        {
            var mainModule = particles[0].main;
            mainModule.startColor = color;
        }
    }
}