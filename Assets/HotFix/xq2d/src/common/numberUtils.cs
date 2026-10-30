using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    /**计算工具*/
    class numberUtils
    {
        /**获取平均切分宽度
         * num组件数量
         * dw两侧间隔宽
         * **/
        public static float getAvWidth(float w, int num, int dw = 0)
        {
            //算出每一格多宽
            return (ScreenUtils.width - dw * 2 - num * w) / (num - 1);
        }

        /**比较两个float是否相等*/
        public static bool isSameFloat(float a, float b)
        {
            const float epsilon = 0.1f;
            return Math.Abs(a - b) < epsilon;
        }
        /**计算合成图的最大范围（以左上角为原点）
         x,y.w.h
         */
        public static Vector2 getAllPicGround(List<Vector4> arr)
        {
            float leftX = arr[0].x;
            float rightX = arr[0].x;
            float topY = arr[0].y;
            float bottomY = arr[0].y;
            for (int p = 0; p < arr.Count; p++)
            {
                var a = arr[p];
                if (a.x < leftX) leftX = a.x;
                else if ((a.x + a.z) > rightX) rightX = a.x + a.z;
                if (a.y > topY) topY = a.y;
                else if ((a.y - a.w) < bottomY) bottomY = a.y - a.w;
            }
            //Debug.Log("范围："+leftX + "/" + rightX + "/" + topY + "/" + bottomY);
            return new Vector2(Math.Abs(rightX - leftX), Math.Abs(topY - bottomY));
        }
        /**取最左上角的坐标*/
        public static Vector2 getMinXY(List<Vector2> arr)
        {
            float leftX = arr[0].x;
            float topY = arr[0].y;
            for (int p = 0; p < arr.Count; p++)
            {
                var a = arr[p];
                if (a.x < leftX) leftX = a.x;
                if (a.y > topY) topY = a.y;//因为负值，所以应取最大
            }
            return new Vector2(leftX, topY);

        }
    }
}
