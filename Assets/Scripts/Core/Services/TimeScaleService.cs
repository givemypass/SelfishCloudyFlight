using DG.Tweening;
using UnityEngine;

namespace Core.Services
{
    public class TimeScaleService
    {
        public void SetTimeScale(float timeScale)
        {
            DOVirtual
                .Float(Time.timeScale, timeScale, 0.1f, value => Time.timeScale = value)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
        } 
    }
}