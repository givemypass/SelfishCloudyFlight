using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using Skywriting.Components;
using UnityEngine;

namespace Skywriting.Systems
{
    public sealed class PlaneSplineFlySystem : BaseSystem, IUpdatable
    {
        private const float Z_ROTATION_SPEED = 15f;
        
        private float _zRotation;
        private Vector3 _targetScale;
        private Vector3 _maxScale;
        
        public override void InitSystem()
        {
            ref var unityTransformComponent = ref Owner.Get<UnityTransformComponent>();
            _maxScale = unityTransformComponent.Transform.localScale;
            unityTransformComponent.Transform.localScale = Vector3.zero;
            _targetScale = Vector3.zero;
        }

        public void Update()
        {
            if (!Owner.Has<TargetSplineComponent>())
                return;
            
            ref var unityTransformComponent = ref Owner.Get<UnityTransformComponent>();
            ref var targetSplineComponent = ref Owner.Get<TargetSplineComponent>();
            ref var positionOnSplineComponent = ref Owner.Get<PositionOnSplineComponent>();
            ref var speedCounterComponent = ref Owner.Get<SpeedCounterComponent>();
            
            unityTransformComponent.Transform.localScale = Vector3.Lerp(unityTransformComponent.Transform.localScale,
                _targetScale, 10f * Time.deltaTime);

            var spline = targetSplineComponent.SplineContainer;
            var splineTPosition = positionOnSplineComponent.TPos;
            
            var position = spline.EvaluatePosition(splineTPosition);
            var tangent = ((Vector3)spline.EvaluateTangent(splineTPosition)).normalized;
            

            var pos = position;
            pos.z = unityTransformComponent.Transform.position.z;
            unityTransformComponent.Transform.position = pos;

            var angleDif = Vector3.Angle(tangent, unityTransformComponent.Transform.forward);

            _zRotation += angleDif * Z_ROTATION_SPEED * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(tangent, -Vector3.forward) * Quaternion.Euler(0,0,_zRotation);
            targetRotation = Quaternion.Lerp(unityTransformComponent.Transform.rotation, targetRotation, 10f * Time.deltaTime);
            
            unityTransformComponent.Transform.rotation = targetRotation;
            
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