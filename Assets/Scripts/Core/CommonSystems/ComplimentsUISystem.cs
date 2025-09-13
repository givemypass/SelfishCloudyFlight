using System.Linq;
using Cinemachine.Utility;
using Core.Commands;
using Core.CommonComponents;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core.CommonSystems
{
    public sealed partial class ComplimentsUISystem : BaseSystem,
        IReactGlobal<HitMarkerCommand>,
        IReactGlobal<MissMarkerCommand>,
        IReactGlobal<PassMarkerSuccessfullyCommand>,
        IReactGlobal<ReachedLastMarkerCommand>
    {
        private Single<PlayerSettingsComponent> _playerSettingsComponent;
        
        private Filter _planeFilter;
        private Sequence _sequence;
        
        public override void InitSystem()
        {
            _playerSettingsComponent = new Single<PlayerSettingsComponent>(Owner.GetWorld());
            _planeFilter = Owner.GetWorld().Filter.With<PlaneTagComponent>().Build();
        }

        private static Vector3 GetPlaneScreenPos(Entity planeEntity)
        {
            var planePos = planeEntity.AsActor().transform.position;
            var screenPos = Camera.main.WorldToScreenPoint(planePos);
            return screenPos;
        }

        private void Play(Entity planeEntity, ComplimentsHolderComponent.ComplimentData data)
        {
            ref var complimentsHolderComponent = ref Owner.Get<ComplimentsHolderComponent>();
            var language = _playerSettingsComponent.Get().Language;
            var sprites = data.Compliments.First(a => a.Language == language).Sprites;
            var sprite = sprites[Random.Range(0, sprites.Length)];
            var asActor = Owner.AsActor();
            var compliment = Object.Instantiate(complimentsHolderComponent.Prefab, asActor.transform);
            compliment.Image.sprite = sprite;
            compliment.Image.SetNativeSize();
            var rotationRange = data.RotationRange;
            var targetScale = Random.Range(data.ScaleRange.x, data.ScaleRange.y);
            compliment.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-rotationRange, rotationRange));
            compliment.transform.position = GetPlaneScreenPos(planeEntity);
            var planeTransform = planeEntity.AsActor().transform;
            var planeForward = planeTransform.forward.ProjectOntoPlane(Vector3.forward).normalized;
            compliment.transform.position -= planeForward * complimentsHolderComponent.SpawnOffset;
            compliment.transform.localScale = Vector3.zero;
            var endValue = compliment.transform.localPosition - planeForward * 100;

            _sequence = DOTween.Sequence();
            _sequence
                .Join(compliment.transform.DOScale(Vector3.one * targetScale, 0.5f).SetEase(Ease.OutBack))
                .Join(compliment.Image.DOFade(0, 1f).SetEase(Ease.InOutBack))
                .Join(compliment.transform.DOLocalMove(endValue, 1f).SetEase(Ease.InOutCubic))
                .OnComplete(() => Object.Destroy(compliment.gameObject));
            
            compliment.SetTween(_sequence);

            
            if (data.ParticleSystem != null)
            {
                Object.Instantiate(data.ParticleSystem, planeTransform.position, Quaternion.identity);
            }
        }

        private void PlayFinal(Entity plane)
        {
            ref var complimentsHolderComponent = ref Owner.Get<ComplimentsHolderComponent>();
            var planeTransform = plane.AsActor().transform;

            var rot = Quaternion.LookRotation(Vector3.up);
            var particles = Object.Instantiate(complimentsHolderComponent.CompletedParticle, planeTransform.position, rot);
            particles.Play();
        }

        void IReactGlobal<HitMarkerCommand>.ReactGlobal(HitMarkerCommand command)
        {
            foreach (var plane in _planeFilter)
            {
                ref var complimentsHolderComponent = ref Owner.Get<ComplimentsHolderComponent>();
                Play(plane, complimentsHolderComponent.Hit); 
            }
        }

        void IReactGlobal<MissMarkerCommand>.ReactGlobal(MissMarkerCommand command)
        {
            foreach (var plane in _planeFilter)
            {
                ref var complimentsHolderComponent = ref Owner.Get<ComplimentsHolderComponent>();
                Play(plane, complimentsHolderComponent.Miss); 
            }
        }

        void IReactGlobal<PassMarkerSuccessfullyCommand>.ReactGlobal(PassMarkerSuccessfullyCommand command)
        {
            foreach (var plane in _planeFilter)
            {
                ref var complimentsHolderComponent = ref Owner.Get<ComplimentsHolderComponent>();
                Play(plane, complimentsHolderComponent.PassSuccessfully); 
            }
        }

        void IReactGlobal<ReachedLastMarkerCommand>.ReactGlobal(ReachedLastMarkerCommand command)
        {
            foreach (var plane in _planeFilter)
            {
                PlayFinal(plane);
            }      
        }
    }
}