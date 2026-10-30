using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Res.script.src.touch
{
    /**触屏*/
    public class ScreenTouch :ScrollRect, IPointerUpHandler, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (touchUI()) return;
        }
        private bool touchUI()
        {
            //不是一个手指或者发生移动就拒绝，避免两指缩放时走位
            if (sysUtils.isMobile())
            {
                if (Input.touchCount != 1) return true;
            }
            else
            {
                if (!Input.GetMouseButtonDown(0)) return true;
            }
            Debug.Log(Input.mousePosition);
            
            return true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }
}
