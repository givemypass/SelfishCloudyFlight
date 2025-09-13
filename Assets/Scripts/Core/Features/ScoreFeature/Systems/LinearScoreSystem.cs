using System.Linq;
using Core.Commands;
using Core.CommonComponents;
using Core.Models;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using UnityEngine;

namespace Core.Features.ScoreFeature.Systems
{
    [Injectable]
    public sealed partial class LinearScoreSystem : BaseSystem,
        IUpdatable,
        IGlobalStart,
        IReactGlobal<MarkerReachedCommand>,
        IReactGlobal<StartEndMarkersInitializedCommand>,
        IReactGlobal<MissMarkerCommand>
    {
        [Inject] private GlobalConfigSO _globalConfig;
        
        private Filter _levelFilter;
        private Filter _planeFilter;
        private Filter _startEndMarkersFilter;
        
        private bool _needTouch;
        private float _speedMultiplier;
        private float _stopPos;
        private float _multiplier;
        
        public override void InitSystem()
        {
        }

        public void GlobalStart()
        {
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
            _startEndMarkersFilter = Owner.GetWorld().Filter.With<StartEndMarkersComponent>().Build();
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<SpeedCounterComponent>()
                .With<PositionOnSplineComponent>()
                .Build();
        }

        public void Update()
        {
            foreach (var level in _levelFilter)
            {
                foreach (var plane in _planeFilter)
                {
                    var tPos = plane.Get<PositionOnSplineComponent>().TPos;
                    if(tPos > _stopPos)
                        continue;
                    if (_needTouch == plane.Has<PlaneIsEmittingComponent>())
                    {
                        _multiplier = _globalConfig.Get.ScorePositiveMultiplier;
                    }
                    
                    var speed = plane.Get<SpeedCounterComponent>().Value * _speedMultiplier;
                    ref var levelComponent = ref level.Get<LevelComponent>();
                    levelComponent.LevelProgress += _multiplier * speed * Time.deltaTime;
                    Owner.GetWorld().Command(new LevelProgressUpdatedCommand());
                }
            }
        }

        void IReactGlobal<MarkerReachedCommand>.ReactGlobal(MarkerReachedCommand command)
        {
            _needTouch = !command.MarkerIsEnd;
            _multiplier = 0;
        }

        void IReactGlobal<StartEndMarkersInitializedCommand>.ReactGlobal(StartEndMarkersInitializedCommand command)
        {
            _startEndMarkersFilter.ForceUpdate();
            foreach (var entity in _startEndMarkersFilter)
            {
                ref var startEndMarkersComponent = ref entity.Get<StartEndMarkersComponent>();
                var lastPoint = startEndMarkersComponent.Points.Last();
                _speedMultiplier = 1 / lastPoint.TPos;
                _stopPos = lastPoint.TPos;
            }
        }

        void IReactGlobal<MissMarkerCommand>.ReactGlobal(MissMarkerCommand command)
        {
            _multiplier = _globalConfig.Get.ScoreNegativeMultiplier;
        }
    }
}