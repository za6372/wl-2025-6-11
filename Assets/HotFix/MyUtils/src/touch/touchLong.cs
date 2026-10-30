using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.HotFix.MyUtils.src.touch
{
    /**长按*/
    public class touchLong : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        //是否按下
        private bool isPress;
        //按下的时刻
        private long startTime;

        public Action clkCallback;
        //长按回调
        public Action longClkCallback;
        //结束后回调
        public Action endCallback;
        //是否一次回调
        public bool isOnce;
        //是否回调了
        public bool isCall;
        void Update()
        {
            //超过1s视为长按
            if (isPress && strUtils.getMillis() - startTime > 1000)
            {
                if (isOnce && isCall)
                {
                    return;
                }
                longClkCallback();
                isCall = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.startTime = strUtils.getMillis();
            this.isPress = true;
            this.isCall = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isPress && strUtils.getMillis() - startTime < 1000)
            {
                if (clkCallback != null) clkCallback();
                else if (longClkCallback != null) longClkCallback();
            }
            else if (isPress && strUtils.getMillis() - startTime >= 1000)
            {
                if (endCallback != null) endCallback();
            }
            this.isPress = false;
            this.isCall = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.isPress = false;
            this.isCall = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

    }
}
