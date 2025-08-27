using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.MonoBehaviourComponents
{
    public class ColorButtonMonoComponent : MonoBehaviour, IPointerDownHandler
    {
        public Image Image;

        public Action OnPressed;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
        }
    }
}