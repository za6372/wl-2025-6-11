using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class UseNumInputUI : PartUI
    {
        public static UseNumInputUI create(Transform parent)
        {
            Vector2 size = new Vector2(800, 400);
            GameObject one = gameObjPool.getInstance().get("UseNumInputUI", typeof(UseNumInputUI));
            one.GetComponent<UseNumInputUI>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height))
                .putSence<UseNumInputUI>(parent).setScreenCenter();
            one.GetComponent<UseNumInputUI>().draw(size).addClk(() => { });

            DoGet.getInstance().startReqImg();
            return one.GetComponent<UseNumInputUI>();
        }
        private UseNumInputUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setColor(0, 0, 0, 0f)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0));

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2, -(ScreenUtils.height - size.y) / 2));

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
            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(bg.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));

            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(bg.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(0, -300));
            btn.GetComponent<BtnUI>().addText("取消", 30).setTextColor().loadRes("gy_03_png").addClk(() =>
             {
                 this.free();
             });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bg.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(640, -300));
            btn1.GetComponent<BtnUI>().addText("确定", 30).setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                Transform num = getContent().Find("num");
                if (callback != null)
                    callback(int.Parse(num.GetComponent<InputUI>().getContent()));
                if (callback1 != null)
                    callback1(num.GetComponent<InputUI>().getContent());
                this.free();
            });

            this.renderText("请输入数量");
            return this;
        }
        private Action<int> callback;
        public void addCallback(Action<int> callback)
        {
            this.callback = callback;
        }
        private Action<string> callback1;
        public void addTextCallback(Action<string> callback1)
        {
            this.callback1 = callback1;
        }
        public Transform getContent()
        {
            return this.transform.Find("kuang/bg/content");
        }

        /**渲染一段文本*/
        public UseNumInputUI renderText(string str)
        {
            Transform ts = getContent();
            gameObjPool.getInstance().freeChildren(ts.gameObject);

            Vector2 size = ts.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(ts, false);
            text.GetComponent<TextUI>().setText(str).setFontSize(30).setAlign().setColor().setIsRichText(true)
            .setSizePos(new Vector2(size.x, 50), new Vector2(0, -50));

            GameObject num = gameObjPool.getInstance().get("num", typeof(InputUI));
            num.transform.SetParent(ts, false);
            num.GetComponent<InputUI>().setSizePos(new Vector2(size.x - 100, 60), new Vector2(50, -120));
            num.GetComponent<InputUI>().initSetting(str).addMatchNum().setContent("1").setRoundedCorners(0.5f);

            return this;
        }

    }
}
