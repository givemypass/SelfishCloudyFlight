using System.Collections.Generic;
using Core.CommonComponents;
using Core.Features.LevelFeature.Commands;
using Core.Features.LevelFeature.Components;
using Core.Features.PlaneFeature.Commands;
using Core.Models;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine;

namespace Core.Features.LevelFeature.Systems
{
    [Injectable]
    public sealed partial class TrackReachingMarkersSystem : BaseSystem,
        IGlobalStart,
        IUpdatable,
        IReactGlobal<PlaneEmittingUpdated>,
        IReactGlobal<StartEndMarkersInitializedCommand>
    {
        [Inject] private GlobalConfigSO _globalConfig;
        
        private struct PointData
        {
            public int Point;
            public bool IsEnd;
        }
        
        private Filter _startEndMarkersFilter;
        private Filter _planeFilter;
        private Filter _levelFilter;
        
        private Queue<StartEndMarkersComponent.Point> _points;

        private PointData? _pointData;

        public override void InitSystem()
        {
            _planeFilter = World.Filter
                .With<PlaneTagComponent>()
                .With<TargetSplineComponent>()
                .With<PositionOnSplineComponent>()
                .Build();
            _startEndMarkersFilter = World.Filter.With<StartEndMarkersComponent>().Build();
            _levelFilter = World.Filter.With<LevelComponent>().Build();
        }

        public void GlobalStart()
        {
        }

        public void Update()
        {
            if(_pointData == null)
                return;
            
            foreach (var plane in _planeFilter)
            {
                var index = GetPlanePosIndex(plane);
                var point = _pointData.Value.Point;
                
                if (!_pointData.Value.IsEnd)
                {
                    if (index - point > _globalConfig.Get.TapAccuracyToPass)
                    {
                        //miss
                        ApplyMiss();
                        NextPoint();
                        return;
                    }
                }
                else
                {
                     if (index - point > _globalConfig.Get.ReleaseAccuracyToPass)
                     {
                         //miss
                        ApplyMiss();
                        NextPoint();
                        return;
                     }
                }
            }
        }

        private void ApplyMiss()
        {
            World.Command(new MissMarkerCommand());
        }
        
        private void ApplyPass()
        {
            World.Command(new PassMarkerSuccessfullyCommand());
        }

        private void ApplyHit()
        {
            World.Command(new HitMarkerCommand());
        }

        private static int GetPlanePosIndex(Entity plane)
        {
            ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
            var tPos = plane.Get<PositionOnSplineComponent>().TPos;
            var index = targetSplineComponent.GetCurrentIndexOnSpline(tPos);
            return index;
        }

        private void NextPoint()
        {
            var prevData = _pointData;
            
            if (_points.TryDequeue(out var pointIndex))
            {
                _pointData = new PointData
                {
                    Point = pointIndex.Index,
                    IsEnd = !prevData?.IsEnd ?? false
                };
            }
            else
            {
                _pointData = null;
            }
        }

        private void EmittingStarted()
        {
            var point = _pointData!.Value.Point;
            foreach (var plane in _planeFilter)
            {
                var index = GetPlanePosIndex(plane);
                
                if (Mathf.Abs(index - point) <= _globalConfig.Get.TapAccuracyToHit)
                {
                    //hit
                    ApplyHit();
                    NextPoint();
                    return;
                }
                
                if (Mathf.Abs(index - point) <= _globalConfig.Get.TapAccuracyToPass)
                {
                    //pass
                    ApplyPass();
                    NextPoint();
                    return;
                }

            }
        }

        private void EmittingEnded()
        {
            var point = _pointData!.Value.Point;
            foreach (var plane in _planeFilter)
            {
                var index = GetPlanePosIndex(plane);
                
                if (Mathf.Abs(index - point) <= _globalConfig.Get.ReleaseAccuracyToHit)
                {
                    //hit
                    ApplyHit();
                    NextPoint();
                    return;
                }
                
                if (Mathf.Abs(index - point) <= _globalConfig.Get.ReleaseAccuracyToPass)
                {
                    //pass
                    ApplyPass();
                    NextPoint();
                    return;
                }
                
                //miss
                ApplyMiss();
                NextPoint();
            }
        }

        void IReactGlobal<PlaneEmittingUpdated>.ReactGlobal(PlaneEmittingUpdated command)
        {
            if (command.Status && _pointData is { IsEnd: false })
            {
                EmittingStarted();
            }
            if(!command.Status && _pointData is { IsEnd: true })
            {
                EmittingEnded();
            }
        }

        void IReactGlobal<StartEndMarkersInitializedCommand>.ReactGlobal(StartEndMarkersInitializedCommand command)
        {
            foreach (var entity in _startEndMarkersFilter)
            {
                ref var startEndMarkersComponent = ref entity.Get<StartEndMarkersComponent>();
                _points = new Queue<StartEndMarkersComponent.Point>(startEndMarkersComponent.Points);
                
                foreach (var level in _levelFilter)
                {
                    ref var levelComponent = ref level.Get<LevelComponent>();
                    levelComponent.LevelProgress = 0;
                }

                _pointData = null;
                NextPoint();
                break;
            }
        }
    }
}