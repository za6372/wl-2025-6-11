using Assets.HotFix.MyUtils.src.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    public class FrameUtils
    {
        /**将对象帧进行替换
         obj原来的,g新的
         */
        public static void frameReplace(GameObject obj, GameObject g)
        {
            AniRuntime am = obj.GetComponent<AniRuntime>();
            // 将帧对象进行替换
            Transform frames= obj.transform.Find("frames");
            //帧方向由原来的来确定
            //Quaternion quaternion = frames.localRotation;
            gameObjPool.getInstance().freeChildren(frames.gameObject);

            Transform newFrames = g.transform.Find("frames");

            for (int i = 0; i < newFrames.childCount; i++)
            {
                Transform cc = newFrames.GetChild(i);
                cc.SetParent(frames.transform, false);
                i--;
            }
            
            am.frameObjs = g.GetComponent<AniRuntime>().frameObjs;
            gameObjPool.getInstance().free(g);
        }
    }
}
