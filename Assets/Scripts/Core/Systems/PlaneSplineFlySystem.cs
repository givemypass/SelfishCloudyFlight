using Core.Components;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Components;
using UnityEngine;

namespace Core.Systems
{
    public sealed partial class PlaneSplineFlySystem : BaseSystem, IUpdatable
    {
        private const float Z_ROTATION_SPEED = 15f;
        
        private float _zRotation;
        private Vector3 _targetScale;
        private Vector3 _maxScale;
        
        public override void InitSystem()
        {
            ref var actorProviderComponent = ref Owner.Get<ActorProviderComponent>();
            _maxScale = actorProviderComponent.Actor.transform.localScale;
            actorProviderComponent.Actor.transform.localScale = Vector3.zero;
            _targetScale = Vector3.zero;
        }

        public void Update()
        {
            if (!Owner.Has<TargetSplineComponent>())
                return;
            
            ref var targetSplineComponent = ref Owner.Get<TargetSplineComponent>();
            ref var positionOnSplineComponent = ref Owner.Get<PositionOnSplineComponent>();
            ref var speedCounterComponent = ref Owner.Get<SpeedCounterComponent>();
            var actor = Owner.AsActor();
            
            actor.transform.localScale = Vector3.Lerp(actor.transform.localScale,
                _targetScale, 10f * Time.deltaTime);

            var spline = targetSplineComponent.SplineContainer;
            var splineTPosition = positionOnSplineComponent.TPos;
            
            var position = spline.EvaluatePosition(splineTPosition);
            var tangent = ((Vector3)spline.EvaluateTangent(splineTPosition)).normalized;
            

            var pos = position;
            pos.z = actor.transform.position.z;
            actor.transform.position = pos;

            var angleDif = Vector3.Angle(tangent, actor.transform.forward);

            _zRotation += angleDif * Z_ROTATION_SPEED * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(tangent, -Vector3.forward) * Quaternion.Euler(0,0,_zRotation);
            targetRotation = Quaternion.Lerp(actor.transform.rotation, targetRotation, 10f * Time.deltaTime);
            
            actor.transform.rotation = targetRotation;
            
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