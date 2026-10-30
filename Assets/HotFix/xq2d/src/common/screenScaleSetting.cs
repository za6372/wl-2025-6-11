using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.common
{
    /**屏幕自适应，绑定到canvas，要求canvas类型是屏幕覆盖*/
    class screenScaleSetting : MonoBehaviour
    {
        public float standard_width = 1080;        //初始宽度  
        public float standard_height = 2460;       //初始高度  
        float device_width = 0f;                //当前设备宽度  
        float device_height = 0f;               //当前设备高度  
        float adjustor = 0f;         //屏幕矫正比例  
        void Start()
        {
            CanvasScaler cs = GetComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(standard_width, standard_height);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            cs.matchWidthOrHeight = 1;
            //获取设备宽高  
            device_width = ScreenUtils.width;
            device_height = ScreenUtils.height;
            //计算宽高比例  
            float standard_aspect = standard_width / standard_height;
            float device_aspect = device_width / device_height;
            //计算矫正比例  
            if (device_aspect < standard_aspect)
            {
                adjustor = standard_aspect / device_aspect;
            }

            CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();
            if (adjustor == 0)
            {
                canvasScalerTemp.matchWidthOrHeight = 1;
            }
            else
            {
                canvasScalerTemp.matchWidthOrHeight = 0;
            }
        }

    }
}
