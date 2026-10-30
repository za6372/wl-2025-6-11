
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class ScrollUI : SimpleUI
    {
        //原始大小
        private Vector2 oldSize;
        //旧信息 包含原点y和浏览位置
        private Vector2 oldMsg = default;
        public override void init()
        {
            //scroll由3部分组成
            //画布渲染
            this.gameObject.AddComponent<CanvasRenderer>();
            //底图
            this.gameObject.AddComponent<Image>();
            //滚动矩形
            this.gameObject.AddComponent<ScrollRect>();

            Image img = this.GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 0);
            this.gameObject.layer = 5;

        }
        /**要求组件已经设置大小方可初始化（注意：只在初始化时调用一次，其他情况下不要调用）*/
        public ScrollUI initSetting(bool horizontal = false, bool Vertical = true, ScrollRect.MovementType movementType = ScrollRect.MovementType.Elastic)
        {
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;

            ScrollRect rc = this.GetComponent<ScrollRect>();
            //rc.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

            GameObject viewport = gameObjPool.getInstance().get("viewport", typeof(CanvasRenderer), typeof(Image));


            Image img = viewport.GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 1);
            img.sprite = Resources.Load<Sprite>("UIMask");
            img.type = Image.Type.Sliced;
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            viewport.transform.SetParent(this.transform, false);
            RectTransform vpRt = viewport.GetComponent<RectTransform>();
            vpRt.sizeDelta = rt.sizeDelta;
            vpRt.anchorMax = Vector2.up;
            vpRt.anchorMin = Vector2.up;
            vpRt.anchoredPosition = new Vector2(size.x / 2f, -size.y / 2f);
            //绑定下一级viewport的RectTransform
            rc.viewport = vpRt;

            GameObject content = gameObjPool.getInstance().get("content", typeof(RectTransform));
            content.transform.SetParent(viewport.transform, false);
            RectTransform cRt = content.GetComponent<RectTransform>();
            cRt.sizeDelta = rt.sizeDelta;
            cRt.anchorMax = Vector2.up;
            cRt.anchorMin = Vector2.up;
            cRt.anchoredPosition = new Vector2(size.x / 2f, -size.y / 2f);

            rc.content = cRt;
            rc.horizontal = horizontal;//默认不启用水平滑动
            rc.vertical = Vertical;
            rc.movementType = movementType;
            rc.elasticity = 0.1f;
            rc.inertia = true;
            rc.decelerationRate = 0.135f;
            rc.scrollSensitivity = 1;
            return this;
        }

        /**设置滑动方向*/
        public ScrollUI setHoriAnVert(bool horizontal, bool vertical)
        {
            ScrollRect rc = this.GetComponent<ScrollRect>();
            rc.horizontal = horizontal;
            rc.vertical = vertical;
            return this;
        }

        /**设置内容并返回这个content对象*/
        public GameObject setContent(GameObject obj)
        {
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;

            GameObject a = this.transform.Find("viewport/content").gameObject;
            //给子项标记空闲
            if (a.transform.childCount > 0)
            {
                gameObjPool.getInstance().freeChildren(a);
            }
            //需要先变为原scroll大小
            RectTransform arc = a.GetComponent<RectTransform>();
            arc.sizeDelta = rt.sizeDelta;
            arc.anchorMax = Vector2.up;
            arc.anchorMin = Vector2.up;
            arc.anchoredPosition = new Vector2(size.x / 2f, -size.y / 2f);
            arc.position = Vector2.zero;
            arc.localPosition = Vector2.zero;

            obj.transform.SetParent(a.transform, false);
            if (obj.GetComponent<RectTransform>() == null)
            {
                obj.AddComponent<RectTransform>();
            }
            RectTransform rc = obj.GetComponent<RectTransform>();
            //给obj一个默认大小、描点位置
            rc.sizeDelta = rt.sizeDelta;
            rc.anchorMax = Vector2.up;
            rc.anchorMin = Vector2.up;
            rc.anchoredPosition = new Vector2(size.x / 2f, -size.y / 2f);
            rc.position = Vector2.zero;
            rc.localPosition = Vector2.zero;

            return a;
        }
        public Vector2 getViewportSize()
        {
            RectTransform rt = this.GetComponent<RectTransform>();
            return rt.sizeDelta;
        }
        public GameObject getContent()
        {
            Transform ts = this.transform.Find("viewport/content");
            if (ts.gameObject.transform.childCount == 0) return null;
            return ts.gameObject.transform.GetChild(0).gameObject;
        }
        public void clearContent()
        {
            GameObject g = this.getContent();
            if (g != null)
                gameObjPool.getInstance().freeChildren(g);
        }
        /**刷新内容高度*/
        public ScrollUI updateHeight(float h)
        {
            GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            //设置一个最小高度
            /*if (oldSize.y > h)
            {
                h = oldSize.y;
            }*/
            //当内容小于可视高度时，设置为可视高度
            if (arc.sizeDelta.y > h)
            {
                h = this.transform.GetComponent<RectTransform>().sizeDelta.y;
            }
            arc.sizeDelta = new Vector2(arc.sizeDelta.x, h);
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, -arc.sizeDelta.y / 2);
            //获取内容的下个节点
            Transform obj = a.transform.GetChild(0);
            RectTransform rc = obj.GetComponent<RectTransform>();
            rc.sizeDelta = new Vector2(rc.sizeDelta.x, h);
            rc.anchoredPosition = new Vector2(rc.sizeDelta.x / 2, -rc.sizeDelta.y / 2);

            return this;

        }
        public ScrollUI updateWidth(float w)
        {
            GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            //设置一个最小高度
            /*if (oldSize.x > w)
            {
                w = oldSize.x;
            }*/
            if (arc.sizeDelta.x > w)
            {
                w = this.transform.GetComponent<RectTransform>().sizeDelta.x;
            }
            arc.sizeDelta = new Vector2(w, arc.sizeDelta.y);
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, -arc.sizeDelta.y / 2);
            //获取内容的下个节点
            Transform obj = a.transform.GetChild(0);
            RectTransform rc = obj.GetComponent<RectTransform>();
            rc.sizeDelta = new Vector2(w, arc.sizeDelta.y);
            rc.anchoredPosition = new Vector2(rc.sizeDelta.x / 2, -rc.sizeDelta.y / 2);
            return this;
        }
        /**回归原点*/
        public ScrollUI scrollZero()
        {
            GameObject a = this.transform.Find("viewport/content").gameObject;
            if (a == null) return this;
            RectTransform arc = a.GetComponent<RectTransform>();
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, -arc.sizeDelta.y / 2);
            return this;
        }
        /**滚动到底部*/
        public ScrollUI scrollBotton()
        {
            Transform viewport = this.transform.Find("viewport");
            RectTransform arc0 = viewport.GetComponent<RectTransform>();
            float viewportH = arc0.sizeDelta.y;
            Transform a = viewport.Find("content");
            RectTransform arc = a.GetComponent<RectTransform>();
            float zeroY = -arc.sizeDelta.y / 2f;
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2f, arc.sizeDelta.y + zeroY - viewportH);
            /*GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            float y = arc.anchoredPosition.y;//当前相对位置为0点
                                             //0点+size就是最低点,最低点还要减去最后一项的高度才是实际看到的最低位
            y += arc.sizeDelta.y / 2;
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, y);*/
            return this;
        }
        /**滚动到指定位置*/
        /*public scrollTs scrollPos(float pY)
        {
            GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, pY);
            return this;
        }*/
        //todo:位置算错了，不知道是上一个位置还是这里
        /**滚动到上个浏览位置*/
        public void scrollPrePos()
        {
            if (this.oldMsg == default) return;
            //重新计算位置
            GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            //相对原点位置是 -arc.sizeDelta.y/2 内容高的一半
            float zeroY = -arc.sizeDelta.y / 2f;

            //float pos = zeroY - oldMsg.x + oldMsg.y;
            //改变后的内容高度会发生变化，假设高了，零点位置就会变化，如原先高100，零点位置就是-50，
            //现在高200，零点就为-100
            //零点相对距离+上个位置=当前位置
            float pos = zeroY - oldMsg.x + oldMsg.y;
            arc.anchoredPosition = new Vector2(arc.sizeDelta.x / 2, pos);

        }
        /**滚动到指定位置*/
        public void scrollInpPos(Vector2 inp)
        {
            //重新计算位置
            GameObject a = this.transform.Find("viewport/content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();

            arc.anchoredPosition = inp;

        }
        /**缓存旧的信息*/
        public void saveOldPosSizeMsg()
        {
            //viewport位置会放入scroll中心，viewport里的content也是处于中心
            GameObject viewport = this.transform.Find("viewport").gameObject;
            GameObject a = viewport.transform.Find("content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            //相对原点位置是 -arc.sizeDelta.y/2 内容高的一半
            float zeroY = -arc.sizeDelta.y / 2f;
            //向上滑动时位置会逐渐增加（接近0的方向）
            //计算相对于原点的位置
            //Vector3(0,-251.452179,0)  -585.0007  
            //Debug.Log(arc.anchoredPosition);
            float oldY = arc.anchoredPosition.y;
            oldMsg = new Vector2(zeroY, oldY);
        }
        public Vector2 getNowPos()
        {
            GameObject viewport = this.transform.Find("viewport").gameObject;
            GameObject a = viewport.transform.Find("content").gameObject;
            RectTransform arc = a.GetComponent<RectTransform>();
            return arc.anchoredPosition;
        }

        /**对滚动的监听*/
        public void addTouchMoveScript(Action tCall, Action bCall,Action<Vector2> moveCall=null)
        {
            GameObject viewport = this.transform.Find("viewport").gameObject;
            RectTransform arc = viewport.GetComponent<RectTransform>();
            float h = arc.sizeDelta.y;
            GameObject a = viewport.transform.Find("content").gameObject;
            if (a.GetComponent<scrollTouchMove>() == null)
            {
                a.AddComponent<scrollTouchMove>();

            }
            a.GetComponent<scrollTouchMove>().init(h, tCall, bCall, (vs)=> {
                if(moveCall!=null) moveCall(vs);
            });

        }
        public void remTouchMoveScript()
        {
            GameObject viewport = this.transform.Find("viewport").gameObject;
            GameObject a = viewport.transform.Find("content").gameObject;
            SetGameObj.remComponent(a.GetComponent<scrollTouchMove>());
        }

    }
}
