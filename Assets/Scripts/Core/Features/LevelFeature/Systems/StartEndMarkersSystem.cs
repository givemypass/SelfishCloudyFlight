using System.Collections.Generic;
using Core.Commands;
using Core.CommonComponents;
using Core.MonoBehaviourComponents.GUI;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.CommonComponents;
using UnityEngine;

namespace Core.Features.LevelFeature.Systems
{
    public sealed partial class StartEndMarkersSystem : BaseSystem, IUpdatable
    {
        private const int MAX_MARKERS = 100;
        
        private LevelScreenUIMonoComponent _monoComponent;
        
        private Filter _planeFilter;
        private Filter _levelFilter;
        private Single<MainCameraTagComponent> _mainCameraSingle;

        public override void InitSystem()
        {
            Owner.AsActor().TryGetComponent(out _monoComponent);
            _mainCameraSingle = new Single<MainCameraTagComponent>(Owner.GetWorld());
            _levelFilter = Owner.GetWorld().Filter.With<LevelComponent>().Build();
            _planeFilter = Owner.GetWorld().Filter
                .With<PlaneTagComponent>()
                .With<TargetSplineComponent>()
                .Build();
        }

        public void Update()
        {
            ref var component = ref Owner.Get<StartEndMarkersComponent>();
            if (!_planeFilter.IsNotEmpty() || (component.Points is { Count: 0 } && component.Markers.Count == 0))
                return;
            
            if (component.Points == null)
            {
                component.Points = new Queue<StartEndMarkersComponent.Point>();
                InitIndexes(ref component);
                Owner.GetWorld().Command(new StartEndMarkersInitializedCommand());
            }

            foreach (var levelEnt in _levelFilter)
            {
                ref var level = ref levelEnt.Get<LevelComponent>().Level;
                foreach (var plane in _planeFilter)
                {
                    ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
                    var spline = targetSplineComponent.SplineContainer;
                    var tPos = plane.Get<PositionOnSplineComponent>().TPos;
                    var index = targetSplineComponent.GetCurrentIndexOnSpline(tPos);

                    while(component.Points.Count != 0 && component.Markers.Count < MAX_MARKERS)
                    {
                        var point = component.Points.Dequeue();
                        var marker = Object.Instantiate(_monoComponent.MarkerPrefab, _monoComponent.transform);
                        component.Markers.Enqueue(marker);
                        marker.Image.sprite = point.IsStart ? _monoComponent.StartMarker : _monoComponent.EndMarker;
                        marker.gameObject.SetActive(true);
                        var markerIndexPlace = point.Index;
                        marker.transform.position = _mainCameraSingle.Get().Camera.WorldToScreenPoint(spline.transform.GetChild(markerIndexPlace).position);
                        marker.transform.localScale = Vector3.zero;
                        marker.IndexPlace = markerIndexPlace;
                        marker.IsStart = point.IsStart;
                    }

                    foreach (var marker in component.Markers)
                    {
                        var dist = marker.IndexPlace - index;
                        var minDist = Mathf.RoundToInt(_monoComponent.MarkerMinDist * level.MarksDistScale);
                        var maxDist = Mathf.RoundToInt(_monoComponent.MarkerMaxDist * level.MarksDistScale);
                        var scale = 1 - Mathf.Clamp01(((float)dist - maxDist) / minDist);
                        marker.transform.localScale = Vector3.Lerp(marker.transform.localScale,
                            new Vector3(scale, scale, 1), 0.5f);

                        var image = marker.Image;
                        var color = image.color;
                        color.a = scale;
                        image.color = Color.Lerp(image.color, color, 0.5f);
                    }

                    if (component.Markers.TryPeek(out var nearest))
                    {
                        if (index >= nearest.IndexPlace)
                        {
                            component.Markers.Dequeue();
                            if (component.Markers.Count == 0)
                            {
                                Owner.GetWorld().Command(new ReachedLastMarkerCommand());
                            }

                            Owner.GetWorld().Command(new MarkerReachedCommand
                            {
                                ScreenPosition = nearest.transform.position,
                                MarkerIsEnd = !nearest.IsStart,
                            });
                            Object.Destroy(nearest.gameObject);
                        }
                    }

                    break;
                }
            }
        }

        private void InitIndexes(ref StartEndMarkersComponent component)
        {
            foreach (var plane in _planeFilter)
            {
                ref var targetSplineComponent = ref plane.Get<TargetSplineComponent>();
                var spline = targetSplineComponent.SplineContainer;

                var prevStatus = spline.transform.GetChild(0).gameObject.activeSelf;
                    
                for (var i = 1; i < spline.transform.childCount; i++)
                {
                    var point = spline.transform.GetChild(i);
                    if (point.gameObject.activeSelf == prevStatus)
                        continue;
                    component.Points.Enqueue(new StartEndMarkersComponent.Point
                    {
                        Index = i,
                        TPos = targetSplineComponent.GetTPosByIndex(i),
                        IsStart = !prevStatus,
                    });
                    prevStatus = !prevStatus;
                }
            }
        }
    }
}