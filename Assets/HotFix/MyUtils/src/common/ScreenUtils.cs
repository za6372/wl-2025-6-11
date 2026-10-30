using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    public class ScreenUtils
    {
        public static float width;
        public static float height;
        public static float screenScaleRate;
        public static void resetSize()
        {
            initSize(width, height);
        }
        public static void initSize(float w,float h)
        {
            float rate = Screen.width / w;
            float width = w * rate;
            float height = h * rate;
            if (height > Screen.height)
            {
                float k = Screen.height / height;
                rate = rate * k;
                width = w * rate;
                height = h * rate;
            }
            
            ScreenUtils.width = w;
            ScreenUtils.height = h;

            ScreenUtils.screenScaleRate = rate;
        }
        /**获取canvas缩放高度*/
        public static Vector2 getCanvasSize()
        {
            return new Vector2(Screen.width / ScreenUtils.screenScaleRate, Screen.height / ScreenUtils.screenScaleRate);
        }
       
    }
}
