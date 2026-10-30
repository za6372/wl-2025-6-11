using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.childPage.jimai
{
    class GroundingPage : PartUI
    {
        private JObject goods;
        private int gdType;
        public static GroundingPage create(JObject goods, int gdType, Transform parent)
        {
            Vector2 size = new Vector2(800, 600);
            GameObject one = gameObjPool.getInstance().get("GroundingPage", typeof(GroundingPage));
            GroundingPage bs = one.GetComponent<GroundingPage>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<GroundingPage>(parent).setLeftPos(Vector2.zero);
            bs.draw0(goods, gdType, size);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public GroundingPage setDes(string str)
        {
            this.transform.Find("des/text").GetComponent<TextUI>().setText(str);
            return this;
        }

        private GroundingPage draw0(JObject goods, int gdType, Vector2 size)
        {
            this.goods = goods;
            this.gdType = gdType;
            this.draw(size);
            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            int price = 1;
            int num = 1;
            int priceType = 0;

            //设置价格、数量、单位
            GameObject desText = gameObjPool.getInstance().get("text", typeof(TextUI));
            desText.transform.SetParent(this.transform, false);
            desText.GetComponent<TextUI>().setText("设置单价").setAlign().setColor().setFontSize(30).setIsRichText().setFontStyle()
                .setSizePos(new Vector2(200, 60), new Vector2(pos.x + 20, pos.y - 20));

            GameObject priceInp = gameObjPool.getInstance().get("priceInp", typeof(InputUI));
            priceInp.transform.SetParent(this.transform, false);
            priceInp.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(pos.x + 20, pos.y - 20 - 70));
            priceInp.GetComponent<InputUI>().initSetting("设置单价").addMatchNum().setContent("1").setRoundedCorners(0.5f);
            priceInp.GetComponent<InputUI>().addChange((v) => { price = int.Parse(v); });

            GameObject danwei = gameObjPool.getInstance().get("danwei", typeof(TextUI));
            danwei.transform.SetParent(this.transform, false);
            danwei.GetComponent<TextUI>().setText("单位/元宝（点击切换）").setAlign("left").setColor().setFontSize(30).setIsRichText().setFontStyle()
                .setSizePos(new Vector2(400, 60), new Vector2(pos.x + 20 + 410, pos.y - 20 - 70)).addClk(() =>
                {
                    if (priceType == 0)
                    {
                        priceType = 1;
                        danwei.GetComponent<TextUI>().setText("单位/银两（点击切换）");
                    }
                    else
                    {
                        priceType = 0;
                        danwei.GetComponent<TextUI>().setText("单位/元宝（点击切换）");
                    }
                });

            desText = gameObjPool.getInstance().get("text", typeof(TextUI));
            desText.transform.SetParent(this.transform, false);
            desText.GetComponent<TextUI>().setText("设置数量").setAlign().setColor().setFontSize(30).setIsRichText().setFontStyle()
                .setSizePos(new Vector2(200, 60), new Vector2(pos.x + 20, pos.y - 20 - 70 * 2));

            GameObject numInp = gameObjPool.getInstance().get("numInp", typeof(InputUI));
            numInp.transform.SetParent(this.transform, false);
            numInp.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(pos.x + 20, pos.y - 20 - 70 * 3));
            numInp.GetComponent<InputUI>().initSetting("设置数量").addMatchNum().setContent("1").setRoundedCorners(0.5f);
            numInp.GetComponent<InputUI>().addChange((v) =>
            {
                num = int.Parse(v);
                if (gdType == 0)
                {
                    if (num > (int)this.goods["num"])
                    {
                        num = (int)this.goods["num"];
                        numInp.GetComponent<InputUI>().setContent("" + num);
                    }
                }
                else
                {
                    num = 1;
                    numInp.GetComponent<InputUI>().setContent("1");
                }
            });


            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(this.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(pos.x + 20, pos.y - 20 - 500));
            btn.GetComponent<BtnUI>().addText("取消").setTextColor().loadRes("gy_03_png").addClk(() =>
            {
                this.free();
            });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(this.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(pos.x + 20 + 600, pos.y - 20 - 500));
            btn1.GetComponent<BtnUI>().addText("确定").setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                List<string> ml = new List<string>() { "24小时", "48小时" };
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((qx) =>
                {
                    int tale = Convert.ToInt32(num * price * 0.01f * (qx + 1));
                    if (tale <= 0) tale = 1;
                    string str = "需缴纳保证金" + tale + "银两（卖出或过期未卖出则返还保证金（以保证金的1%作为托管费，托管费低于1银两则按1银两收取），提前取回将视为违约不返还保证金）";
                    MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
                    {
                        face.goodsInterface.grounding(this.goods["Id"].ToString(), num, price, priceType, gdType, qx, () =>
                        {
                            callback();
                        });
                        this.free();
                    }, () => { });
                });



            });
            return this;
        }
        private Action callback;
        public GroundingPage addCallback(Action call)
        {
            this.callback = call;
            return this;
        }
        private GroundingPage draw(Vector2 size)
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
