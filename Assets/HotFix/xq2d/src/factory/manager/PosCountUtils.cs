using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    class PosCountUtils
    {
        /**将相对于mape的位置进行还原成position*/
        public static Vector2 relativeMapePosToControlPosition(Vector2 vs)
        {
            Transform mape = PointGet.getMapMapePoint();
            Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //将原点归于左上角
            float x = mape.position.x - size.x / 2f;
            float y = mape.position.y - size.y / 2f - 300;//-300是因为下方有个聊天窗，导致y向上偏移了300
            return new Vector2(vs.x + x, (vs.y + y) + ScreenUtils.height);
        }
        public static Vector2 controlPosRelativeMapePos(Transform control)
        {
            Transform mape = PointGet.getMapMapePoint();
            Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //将原点归于左上角
            float x = mape.position.x - size.x / 2f;
            float y = mape.position.y - size.y / 2f - 300;//-300是因为下方有个聊天窗，导致y向上偏移了300
            //用ScreenUtils.height来减是因为得出的npcY方向是正值，要把下方坐标变成负值所以用它来减
            return new Vector2(control.position.x - x, (control.position.y - y) - ScreenUtils.height);
        }
        public static Vector2 buildPosRelativeMapePos(Transform build)
        {
            Transform mape = PointGet.getMapMapePoint();
            Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //将原点归于左上角
            float x = mape.position.x - size.x / 2f;
            float y = mape.position.y - size.y / 2f - 300;//-300是因为下方有个聊天窗，导致y向上偏移了300
            //用ScreenUtils.height来减是因为得出的npcY方向是正值，要把下方坐标变成负值所以用它来减
            return new Vector2(build.position.x - x, (build.position.y - y) - ScreenUtils.height);
        }
        public static Vector2 npcPosRelativeMapePos(Transform npc)
        {
            //Debug.Log("npc="+npc.localPosition);
            Transform mape = PointGet.getMapMapePoint();
            Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //将原点归于左上角
            float x = mape.position.x - size.x / 2f;
            float y = mape.position.y - size.y / 2f - 300;//-300是因为下方有个聊天窗，导致y向上偏移了300
            //用ScreenUtils.height来减是因为得出的npcY方向是正值，要把下方坐标变成负值所以用它来减
            return new Vector2(npc.position.x - x, (npc.position.y - y) - ScreenUtils.height);
        }
        /**将触点位置转化成相对mape的坐标*/
        public static Vector2 touchPosToRelativeMapePos(Vector2 touch)
        {
            //现在的touch是以屏幕左上角为原点，右侧正值逐渐变大，下侧负值逐渐变下，与mape无关系，仅相对于屏幕
            Transform mape = PointGet.getMapMapePoint();
            //Debug.Log("mape=" + mape.position);
            Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //将原点归于左上角
            float x = mape.position.x - size.x / 2f;
            float y = mape.position.y - size.y / 2f - 300;//-300是因为下方有个聊天窗，导致y向上偏移了300
            //Debug.Log("mape=" + x + "," + y);
            return new Vector2(touch.x - x, touch.y - y);
        }
    }
}
