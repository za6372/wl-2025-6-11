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

namespace Assets.HotFix.xq2d.src.ui.part
{
    class Menu : PartUI
    {
        
        public static Menu create(List<string> list, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("Menu", typeof(Menu));
            Menu bs = one.GetComponent<Menu>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<Menu>(parent).setLeftPos(Vector2.zero);
            bs.draw(list);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        
        private Menu draw(List<string> list)
        {
            Vector2 size = new Vector2(400, list.Count * 100 + 40);
            if (size.y > 6 * 100 + 40)
            {
                size.y = 6 * 100 + 40;
            }
            this.draw(size);

            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(this.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x + 20, pos.y - 20));
            scroll.GetComponent<ScrollUI>().initSetting();
            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);

            /*GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            items.transform.SetParent(this.transform, false);
            items.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x+20, pos.y-20));*/

            for(int i = 0; i < list.Count; i++)
            {
                int index = i;
                GameObject tb = gameObjPool.getInstance().get("item" + i, typeof(ImgUI));
                tb.transform.SetParent(items.transform, false);
                tb.GetComponent<ImgUI>().setAlpha(0)
                    .setSizePos(new Vector2(size.x - 40, 100), new Vector2(0, -100 * i))
                    .addClk(() => {
                        this.free();
                        if (callback != null) callback(index);
                    });

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(tb.transform, false);
                text.GetComponent<TextUI>().setText(list[i]).setAlign().setColor().setFontSize(30)
                    .setSizePos(new Vector2(size.x - 40, 100), Vector2.zero);
            }

            scroll.GetComponent<ScrollUI>().updateHeight(list.Count * 100);
            return this;
        }
        private Action<int> callback;
        public Menu addCallback(Action<int> call)
        {
            this.callback = call;
            return this;
        }
        private Menu draw(Vector2 size)
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
