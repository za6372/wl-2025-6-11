using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.part
{

    /**弹窗部件*/
    class Dialog : PartUI
    {

        public static Dialog create(Vector2 size, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("dialog", typeof(Dialog));
            one.GetComponent<Dialog>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<Dialog>(parent).setScreenCenter();
            one.GetComponent<Dialog>().draw(size);


            return one.GetComponent<Dialog>();
        }
        private Dialog draw(Vector2 size)
        {
            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setAlpha(0)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero)
                .addClk(() => { this.free(); });

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, pos);

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(kuang.transform, false);
            bg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(20, -20));
            //左顶
            GameObject bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, 0));
            //顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(9, 1, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, 0));
            //左
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(0, -20));
            //右顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, 0));
            //右
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 0, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(size.x - 20, -20));
            //左下
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(0, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, -(size.y - 20)));
            //底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(9, 0, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, -(size.y - 20)));
            //右底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l3_png", new Rect(1, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, -(size.y - 20)));

            //内容区


            GameObject scroll = gameObjPool.getInstance().get("content", typeof(ScrollUI));
            scroll.transform.SetParent(this.transform, false);
            scroll.GetComponent<ScrollUI>()
               .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x + 20, pos.y - 20));
            scroll.GetComponent<ScrollUI>().initSetting();

            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);
            return this;
        }
        public Transform getContent()
        {
            return this.transform.Find("content").GetComponent<ScrollUI>().getContent().transform;
        }
        public void updateHeight(float h)
        {
            this.transform.Find("content").GetComponent<ScrollUI>().updateHeight(h);
        }
        /**渲染一段文本*/
        public void renderText(string str, string align = "left")
        {
            Transform ts = getContent();
            Vector2 size = this.transform.Find("content").GetComponent<RectTransform>().sizeDelta;
            if (ts.Find("text") == null)
            {
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(ts, false);
                text.GetComponent<TextUI>().setText(str).setFontSize(30).setAlign(align).setColor().setIsRichText(true)
                .setSizePos(size, Vector2.zero);
            }
            TextUI tui = ts.Find("text").GetComponent<TextUI>();
            float h = tui.getRichHeight();
            if (h < size.y) h = size.y;
            this.transform.Find("content").GetComponent<ScrollUI>().updateHeight(h);
        }
        public override void init()
        {

        }
    }
}
