using System;
using System.Linq;
using Cinemachine.Utility;
using Core.CommonComponents;
using Core.Features.LevelFeature.Commands;
using Core.Features.LevelFeature.Components;
using DG.Tweening;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.CommonComponents;
using UnityEngine;

namespace Core.Features.LevelFeature.Systems
{
    public sealed partial class ComplimentsUISystem : BaseSystem,
        IReactGlobal<HitMarkerCommand>,
        IReactGlobal<MissMarkerCommand>,
        IReactGlobal<PassMarkerSuccessfullyCommand>,
        IReactGlobal<ReachedLastMarkerCommand>
    {
        private const float SCALE_DURATION = 0.5f;
        private const float MOVE_FADE_DURATION = 1f;
        private const float MOVE_DISTANCE = 100f;
        
        private Single<MainCameraTagComponent> _mainCameraSingle;
        private Filter _planeFilter;

        private Single<PlayerSettingsComponent> _playerSettingsComponent;

        void IReactGlobal<HitMarkerCommand>.ReactGlobal(HitMarkerCommand command) =>
            Play(holder => holder.Hit);

        void IReactGlobal<MissMarkerCommand>.ReactGlobal(MissMarkerCommand command) =>
            Play(holder => holder.Miss);

        void IReactGlobal<PassMarkerSuccessfullyCommand>.ReactGlobal(PassMarkerSuccessfullyCommand command) =>
            Play(holder => holder.PassSuccessfully);

        void IReactGlobal<ReachedLastMarkerCommand>.ReactGlobal(ReachedLastMarkerCommand command)
        {
            foreach (var plane in _planeFilter)
                PlayFinal(plane);
        }

        public override void InitSystem()
        {
            var world = Owner.GetWorld();
            _playerSettingsComponent = new Single<PlayerSettingsComponent>(world);
            _mainCameraSingle = new Single<MainCameraTagComponent>(world);
            _planeFilter = world.Filter.With<PlaneTagComponent>().Build();
        }

        private Vector3 GetPlaneScreenPos(Entity planeEntity)
        {
            var planePos = planeEntity.AsActor().transform.position;
            return _mainCameraSingle.Get().Camera.WorldToScreenPoint(planePos);
        }

        private void Play(Entity planeEntity, ComplimentsHolderComponent.ComplimentData data)
        {
            ref var holder = ref Owner.Get<ComplimentsHolderComponent>();
            var language = _playerSettingsComponent.Get().Language;

            var sprites = data.Compliments.First(a => a.Language == language).Sprites;
            var sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];

            var compliment = UnityEngine.Object.Instantiate(holder.Prefab, Owner.AsActor().transform);
            compliment.Image.sprite = sprite;
            compliment.Image.SetNativeSize();

            var planeTransform = planeEntity.AsActor().transform;
            var planeForward = planeTransform.forward.ProjectOntoPlane(Vector3.forward).normalized;

            compliment.transform.position = GetPlaneScreenPos(planeEntity) - planeForward * holder.SpawnOffset;
            compliment.transform.rotation =
                Quaternion.Euler(0, 0, UnityEngine.Random.Range(-data.RotationRange, data.RotationRange));
            compliment.transform.localScale = Vector3.zero;

            var targetScale = UnityEngine.Random.Range(data.ScaleRange.x, data.ScaleRange.y);
            var endValue = compliment.transform.localPosition - planeForward * MOVE_DISTANCE;

            var sequence = DOTween.Sequence()
                .Join(compliment.transform.DOScale(Vector3.one * targetScale, SCALE_DURATION).SetEase(Ease.OutBack))
                .Join(compliment.Image.DOFade(0, MOVE_FADE_DURATION).SetEase(Ease.InOutBack))
                .Join(compliment.transform.DOLocalMove(endValue, MOVE_FADE_DURATION).SetEase(Ease.InOutCubic))
                .OnComplete(() => UnityEngine.Object.Destroy(compliment.gameObject));

            compliment.SetTween(sequence);

            if (data.ParticleSystem != null)
                UnityEngine.Object.Instantiate(data.ParticleSystem, planeTransform.position, Quaternion.identity);
        }

        private void PlayFinal(Entity plane)
        {
            ref var holder = ref Owner.Get<ComplimentsHolderComponent>();
            var planeTransform = plane.AsActor().transform;

            var rot = Quaternion.LookRotation(Vector3.up);
            var particles = UnityEngine.Object.Instantiate(holder.CompletedParticle, planeTransform.position, rot);
            particles.Play();
        }

        private void Play(Func<ComplimentsHolderComponent, ComplimentsHolderComponent.ComplimentData> selector)
        {
            ref var holder = ref Owner.Get<ComplimentsHolderComponent>();
            foreach (var plane in _planeFilter)
            {
                Play(plane, selector(holder));
            }
        }
    }
}