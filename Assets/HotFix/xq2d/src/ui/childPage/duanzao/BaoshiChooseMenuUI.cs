using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.HotFix.xq2d.src.ui.childPage.duanzao
{
    class BaoshiChooseMenuUI : PartUI
    {

        public static BaoshiChooseMenuUI create(List<string> list, Transform parent)
        {
            float h = list.Count * 100 + 40 + 300;
            if (h > 740) h = 740;
            Vector2 size = new Vector2(800, h);
            GameObject one = gameObjPool.getInstance().get("BaoshiChooseMenuUI", typeof(BaoshiChooseMenuUI));
            BaoshiChooseMenuUI bs = one.GetComponent<BaoshiChooseMenuUI>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<BaoshiChooseMenuUI>(parent).setLeftPos(Vector2.zero);
            bs.draw(list, size);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public BaoshiChooseMenuUI setDes(string str)
        {
            this.transform.Find("des/text").GetComponent<TextUI>().setText(str);
            return this;
        }

        private BaoshiChooseMenuUI draw(List<string> list, Vector2 size)
        {
            this.draw(size);
            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            GameObject des = gameObjPool.getInstance().get("des", typeof(ImgUI));
            des.transform.SetParent(this.transform, false);
            des.GetComponent<ImgUI>().setColor32(0, 74, 76, 255)
                .setSizePos(new Vector2(size.x - 40, 300), new Vector2(pos.x + 20, pos.y - 20));
            GameObject desText = gameObjPool.getInstance().get("text", typeof(TextUI));
            desText.transform.SetParent(des.transform, false);
            desText.GetComponent<TextUI>().setText("xxx").setAlign().setColor().setFontSize(30).setIsRichText().setFontStyle()
                .setSizePos(new Vector2(size.x - 40, 300), Vector2.zero);
            //内容区
            float h = size.y - 40;
            if (h > 400) h = 400;
            GameObject scroll = gameObjPool.getInstance().get("content", typeof(ScrollUI));
            scroll.transform.SetParent(this.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x - 40, h), new Vector2(pos.x + 20, pos.y - 320));
            scroll.GetComponent<ScrollUI>().initSetting();

            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);
            scroll.GetComponent<ScrollUI>().updateHeight(list.Count * 100);
            /*GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            items.transform.SetParent(this.transform, false);
            items.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x + 20, pos.y - 320));*/

            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                GameObject tb = gameObjPool.getInstance().get("item" + i, typeof(ImgUI));
                tb.transform.SetParent(items.transform, false);
                tb.GetComponent<ImgUI>().setAlpha(0)
                    .setSizePos(new Vector2(size.x - 40, 100), new Vector2(0, -100 * i))
                    .addClk(() =>
                    {
                        this.free();
                        if (callback != null) callback(index);
                    });

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(tb.transform, false);
                text.GetComponent<TextUI>().setText(list[i]).setAlign().setColor().setFontSize(30)
                    .setSizePos(new Vector2(size.x - 40, 100), Vector2.zero);
            }
            return this;
        }
        private Action<int> callback;
        public BaoshiChooseMenuUI addCallback(Action<int> call)
        {
            this.callback = call;
            return this;
        }
        private BaoshiChooseMenuUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setAlpha(0)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero)
                .addClk(() => { this.free(); });

            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

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


            return this;
        }

    }
}
