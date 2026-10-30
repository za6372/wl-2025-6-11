using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.factory.basicObj
{
    //todo:注意：3d组件不要加入池管理，因为2d的RectTransform跟3d的Transform都不能被清理，这样会导致位置错乱
    /**相机的基础设置*/
    public class cameraObj : baseObj
    {
        public static cameraObj getInstance(string name)
        {
            GameObject g = new GameObject(name);
            return g.AddComponent<cameraObj>();
        }

        public cameraObj init(int depth, int cullingMask, bool orthographic = true, CameraClearFlags clearFlags = CameraClearFlags.Skybox)
        {
            Camera cam = this.gameObject.AddComponent<Camera>();
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localScale = Vector3.one;

            cam.cullingMask = cullingMask;//1 << 6 | 1 << 7
            cam.gameObject.layer = 0;
            cam.clearFlags = clearFlags;

            cam.orthographic = orthographic; //投射方式：orthographic正交//
            cam.orthographicSize = 5; //投射区域大小//
            cam.nearClipPlane = 0.3f; //前距离//
            cam.farClipPlane = 1000f; //后距离//
            cam.rect = new Rect(0, 0, 1f, 1f);
            cam.depth = depth;//深度

            cam.allowMSAA = true; // 开启MSAA
            //cam.msaaLevel = 4;

            return this;
        }


    }
}
