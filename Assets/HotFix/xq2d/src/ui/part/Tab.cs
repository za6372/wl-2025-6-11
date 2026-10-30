using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
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
    class Tab : PartUI
    {
        public float tabWidth;
        public float tabHeight = 60;
        public int chooseIndex;
        public static Tab create(List<string> list, Vector2 pos, Transform parent)
        {
            Vector2 sizeDelta = parent.GetComponent<RectTransform>().sizeDelta;
            Vector2 size = new Vector2(sizeDelta.x, 100);

            return create(120, size, pos, parent).draw(list);
        }
        public static Tab create(float tabWidth, Vector2 size, Vector2 pos, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("Tab", typeof(Tab));
            Tab bs = one.GetComponent<Tab>();
            bs.setSize(size).putSence<Tab>(parent).setLeftPos(pos);
            bs.tabWidth = tabWidth;
            return bs;
        }

        public Tab setBgColor(byte r, byte g, byte b, byte a)
        {
            this.transform.Find("scbg").GetComponent<ImgUI>().setColor32(r, g, b, a);
            return this;
        }




        /**默认水平方向绘制*/
        public Tab draw(List<string> list, bool isHor = true)
        {
            Vector2 size = this.transform.GetComponent<RectTransform>().sizeDelta;

            GameObject scbg = gameObjPool.getInstance().get("scbg", typeof(ImgUI));
            scbg.transform.SetParent(this.transform, false);
            scbg.GetComponent<ImgUI>().setColor32(PageSetting.TabBgColor)
                .setSizePos(new Vector2(size.x, size.y), Vector2.zero);
           

            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(this.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x, size.y), Vector2.zero);

            if (isHor) scroll.GetComponent<ScrollUI>().initSetting(true, false);
            else scroll.GetComponent<ScrollUI>().initSetting(false, true);

            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);

            float dw = 0;
            if (isHor)
            {
                dw = (size.x - list.Count * tabWidth) / (list.Count + 1);
                if (dw < 10) dw = 10;
                scroll.GetComponent<ScrollUI>().updateWidth((tabWidth + dw) * list.Count + dw);
            }
            else
            {
                dw = (size.y - list.Count * tabWidth) / (list.Count + 1);
                if (dw < 10) dw = 10;
                scroll.GetComponent<ScrollUI>().updateHeight((tabWidth + dw) * list.Count);
            }


            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                float x = 0;
                float y = 0;
                float w = 0;
                float h = 0;
                if (isHor)
                {
                    x = dw + (tabWidth + dw) * i;
                    y = -(size.y - tabHeight) / 2f;
                    w = tabWidth;
                    h = tabHeight;
                }
                else
                {
                    x = 0;
                    y = -(dw + (tabWidth + dw) * i);
                    w = size.x;
                    h = tabWidth;
                }
                GameObject tb = gameObjPool.getInstance().get("tb" + i, typeof(ImgUI));
                tb.transform.SetParent(items.transform, false);
                tb.GetComponent<ImgUI>()
                    .setSizePos(new Vector2(w, h), new Vector2(x, y))
                    .addClk(() =>
                    {
                        //变换颜色
                        Transform items = this.transform.Find("scroll").GetComponent<ScrollUI>().getContent().transform;
                        for (int t = 0; t < items.transform.childCount; t++)
                        {
                            Transform a = items.transform.GetChild(t);
                            ImgUI img = a.GetComponent<ImgUI>();
                            if (t == index)
                            {
                                img.setRoundedCorners(1, PageSetting.TabItemColor);//.setColor("#114F4F");
                                img.gameObject.GetComponent<UIOutline>().setSome(PageSetting.TabItemMbColor, new Vector2(2, -2));
                            }
                            else
                            {
                                img.setRoundedCorners(1, PageSetting.TabItemColor);//.setColor("#000000");
                                img.gameObject.GetComponent<UIOutline>().setSome(PageSetting.TabItemColor, new Vector2(2, -2));
                            }
                        }
                        DoGet.getInstance().startReqImg();
                        this.chooseIndex = index;
                        callback(index);
                    }, false);
                tb.AddComponent<UIOutline>();

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(tb.transform, false);
                text.GetComponent<TextUI>().setText(list[i]).setAlign().setColor32(PageSetting.TabTextColor).setFontSize(PageSetting.FontSize).setNoClk().setFontStyle()
                    .setSizePos(new Vector2(w, h), Vector2.zero);
                //text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));
            }
            return this;
        }
        public void clkDefault(int index = 0)
        {
            Transform left = this.transform.Find("scroll").GetComponent<ScrollUI>().getContent().transform;
            if (left.childCount > 0)
            {
                left.GetChild(index).GetComponent<Button>().onClick.Invoke();
            }
        }
        private Action<int> callback;
        public Tab addCallback(Action<int> call)
        {
            this.callback = call;
            return this;
        }
    }
}
