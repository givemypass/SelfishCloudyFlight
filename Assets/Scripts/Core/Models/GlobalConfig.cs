using System;
using Core.Helpers;
using Core.Identifiers;
using UnityEngine;

namespace Core.Models
{
    [Serializable]
    public struct GlobalConfig
    {
        public int TapAccuracyToHit;
        public int TapAccuracyToPass;
        
        public int ReleaseAccuracyToHit;
        public int ReleaseAccuracyToPass;

        public IdentifierToModelMap<ColorIdentifier, Color> ColorPallete;
    }
}