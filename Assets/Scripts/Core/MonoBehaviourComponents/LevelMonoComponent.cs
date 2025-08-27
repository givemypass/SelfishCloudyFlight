using System;
using System.Linq;
using Core.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.MonoBehaviourComponents
{
    [Serializable]
    public struct LevelKnotInfo : IEquatable<LevelKnotInfo>
    {
        // [IdentifierDropDown(nameof(ColorIdentifier))]
        // public int ColorIndex;
        public bool Equals(LevelKnotInfo other)
        {
            return true;
            // return ColorIndex == other.ColorIndex;
        }
        
        public override int GetHashCode()
        {
            return 0;
            // return ColorIndex.GetHashCode();
        }
    }
    
    [Serializable]
    public class LevelKnotInfoMap : SimpleSerializableMap<int, LevelKnotInfo> { }


    [ExecuteInEditMode]
    public class LevelMonoComponent : MonoBehaviour
    {
        [HideInInspector]
        public LevelKnotInfoMap LevelKnotInfoMap;

        [SerializeField]
        private float _scale = 1f;
        [SerializeField]
        private float _speedScale = 1f;
        [SerializeField]
        private float _marksDistScale = 1f;
        //todo after adding identifiers
        // [SerializeField]
        // private ColorIdentifier color1;
        // [SerializeField]
        // private ColorIdentifier color2;
        // [SerializeField] 
        // private ColorIdentifier color3;
        //
        // public float Scale => scale;
        // public float SpeedScale => speedScale;
        // public float MarksDistScale => marksDistScale;
        //
        //
        // public IEnumerable<ColorIdentifier> GetColors()
        // {
        //     yield return color1;
        //     yield return color2;
        //     yield return color3;
        // }
        //
#if UNITY_EDITOR
        private void OnEnable()
        {
            Spline.Changed += OnSplineChanged;
        }

        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void OnSplineChanged(Spline spline, int index, SplineModification modification)
        {
            var orderedIndexes = LevelKnotInfoMap.Keys.OrderBy(a => a);
            switch (modification)
            {
                case SplineModification.KnotInserted:
                    foreach (var i in orderedIndexes)
                    {
                        if (i >= index)
                        {
                            LevelKnotInfoMap[i + 1] = LevelKnotInfoMap[i];
                            LevelKnotInfoMap.Remove(i);
                        }
                    }

                    break;
                case SplineModification.KnotRemoved:
                    foreach (var i in orderedIndexes)
                    {
                        if (i > index)
                        {
                            LevelKnotInfoMap[i - 1] = LevelKnotInfoMap[i];
                            LevelKnotInfoMap.Remove(i);
                        }
                    }

                    break;
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}