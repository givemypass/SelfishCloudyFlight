using System;
using System.Collections.Generic;
using Core.MonoBehaviourComponents;

namespace Core.Helpers
{
    public static class LevelHelper
    {
        public static void FillSortedKnots(this Span<KeyValuePair<int, LevelKnotInfo>> knots, LevelKnotInfoMap map)
        {
            var i = 0;
            foreach (var levelKnotInfo in map)
            {
                knots[i++] = levelKnotInfo;
            }
            
            //sort
            for (i = 0; i < knots.Length - 1; i++)
            {
                var swapped = false;
        
                for (var j = 0; j < knots.Length - i - 1; j++)
                {
                    if (knots[j].Key <= knots[j + 1].Key) continue;
                    (knots[j], knots[j + 1]) = (knots[j + 1], knots[j]);
                    swapped = true;
                }
        
                if (!swapped)
                    break;
            }
        } 
    }
}