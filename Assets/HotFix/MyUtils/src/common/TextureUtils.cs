using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    /**图片操作工具*/
    public class TextureUtils
    {
        
        
        /**测量多帧对象的总大小*/
        public static Vector2 countZhenPicSize(GameObject g)
        {
            RectTransform[] arr = g.GetComponentsInChildren<RectTransform>(true);
            List<Vector4> list = new List<Vector4>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].sizeDelta == Vector2.zero) continue;
                Vector4 v4 = new Vector4(arr[i].localPosition.x, arr[i].localPosition.y, arr[i].sizeDelta.x, arr[i].sizeDelta.y);
                list.Add(v4);
            }
            return getAllPicGround(list);
        }
        /**计算合成图的最大范围（以中心为原点）
         x,y.w.h
         */
        private static Vector2 getAllPicGround(List<Vector4> arr)
        {
            //定义边界（左、下为负号）
            float leftX = arr[0].x - arr[0].z /2f;
            float rightX = arr[0].x - arr[0].z / 2f;
            float topY = arr[0].y - arr[0].w / 2f;
            float bottomY = arr[0].y - arr[0].w / 2f;
            for (int p = 0; p < arr.Count; p++)
            {
                Vector4 a = arr[p];//x,y,z,w
                if ((a.x-a.z/2f) < leftX) leftX = a.x - a.z / 2f;
                if ((a.x - a.z / 2f + a.z) > rightX) rightX = a.x - a.z / 2f + a.z;
                if ((a.y - a.w / 2f) < bottomY) bottomY = a.y - a.w / 2f;
                if ((a.y - a.w / 2f + a.w) > topY) topY = a.y - a.w / 2f + a.w;
                //Debug.Log(a.x + "=>" + a.z);
            }
            //Debug.Log("范围："+leftX + "/" + rightX + "/" + topY + "/" + bottomY);
            return new Vector2(Math.Abs(rightX - leftX), Math.Abs(topY - bottomY));
        }
        
        
        
    }
}
