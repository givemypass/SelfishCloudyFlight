using Core.CommonComponents;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.CommonComponents;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.Features.PlaneFeature.Systems
{
    public sealed partial class PlaneSplineFlySystem : BaseSystem, IUpdatable
    {
        private const float Z_ROTATION_SPEED = 15f;
        private const float SCALE_LERP_SPEED = 10f;
        private const float ROTATION_LERP_SPEED = 10f;

        private float _zRotation;
        private Vector3 _targetScale;
        private Vector3 _maxScale;
        
        public override void InitSystem()
        {
            ref var actorProviderComponent = ref Owner.Get<ActorProviderComponent>();
            
            var actorTransform = actorProviderComponent.Actor.transform;
            _maxScale = actorTransform.localScale;
            
            actorTransform.localScale = Vector3.zero;
            _targetScale = Vector3.zero;
        }

        public void Update()
        {
            if (!Owner.Has<TargetSplineComponent>())
                return;
            
            ref var positionOnSplineComponent = ref Owner.Get<PositionOnSplineComponent>();
            var splineContainer = Owner.Get<TargetSplineComponent>().SplineContainer;
            var actor = Owner.AsActor();
            var actorTransform = actor.transform;

            UpdateScale(actorTransform);
            UpdatePositionAndRotation(actorTransform, splineContainer, positionOnSplineComponent.TPos);
            UpdateProgress(splineContainer, ref positionOnSplineComponent);
        }

        private void UpdateScale(Transform actorTransform)
        {
            actorTransform.localScale = Vector3.Lerp(
                actorTransform.localScale,
                _targetScale,
                SCALE_LERP_SPEED * Time.deltaTime);
        }

        private void UpdatePositionAndRotation(Transform actorTransform, SplineContainer spline, float splineTPosition)
        {
            var position = spline.EvaluatePosition(splineTPosition);
            var tangent = ((Vector3)spline.EvaluateTangent(splineTPosition)).normalized;

            var pos = position;
            pos.z = actorTransform.position.z;
            actorTransform.position = pos;

            var angleDif = Vector3.Angle(tangent, actorTransform.forward);

            _zRotation += angleDif * Z_ROTATION_SPEED * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(tangent, -Vector3.forward) * Quaternion.Euler(0,0,_zRotation);
            targetRotation = Quaternion.Lerp(actorTransform.rotation, targetRotation, ROTATION_LERP_SPEED * Time.deltaTime);
            
            actorTransform.rotation = targetRotation;
        }

        private void UpdateProgress(SplineContainer spline, ref PositionOnSplineComponent positionOnSplineComponent)
        {
            var splineTPosition = positionOnSplineComponent.TPos;
            ref var speedCounterComponent = ref Owner.Get<SpeedCounterComponent>();
            var speed = speedCounterComponent.Value;
            splineTPosition += speed * Time.deltaTime;
            positionOnSplineComponent.TPos = splineTPosition;

            var index = (int) (splineTPosition / (1f / spline.transform.childCount));
            if (index < spline.transform.childCount)
            {
                spline.transform.GetChild(index).gameObject.SetActive(false);
            }

            _targetScale = splineTPosition is > 0 and < 0.95f ? _maxScale : Vector3.zero;
        }
    }
}