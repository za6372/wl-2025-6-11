using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.HotFix.xq2d.src.touch
{
    /**ui的触摸处理*/
    class touchEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private Action<PointerEventData> downCall;
        private Action<PointerEventData> upCall;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (downCall != null) downCall(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (upCall != null) upCall(eventData);
        }
        public void addTouchCall(Action<PointerEventData> downCall, Action<PointerEventData> upCall)
        {
            this.downCall = downCall;
            this.upCall = upCall;
        }
    }
}
