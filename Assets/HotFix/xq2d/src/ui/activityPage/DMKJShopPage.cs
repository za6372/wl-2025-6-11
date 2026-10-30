using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class DMKJShopPage : PageUI
    {
        public DMKJShopPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("盗梦商城");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
            return this;

        }

        public void clkTab(int index)
        {
            Transform content = this.getStandardPageContent();
            content.Find("Tab").GetComponent<Tab>().clkDefault(index);
        }

        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>() {
            "水晶","元宝","银两","出售"
            };

            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);

            //tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));
            //黑色部分
            GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
            hb.transform.SetParent(k2.transform, false);
            hb.GetComponent<ImgUI>().setColor32(PageSetting.HbColor)
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);
            //列表部分
            //bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform k2 = this.getStandardPageContent().Find("k2");
            if (k2.Find("bgStyle1") != null)
                gameObjPool.getInstance().free(k2.Find("bgStyle1").gameObject);
            if (k2.Find("hb") != null)
            {
                gameObjPool.getInstance().freeChildren(k2.Find("hb").gameObject);
            }
            if (index == 0)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw0();
            }
            /*else if (index == 1)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw1();
            }*/
            else if (index == 1)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw2();
            }
            else if (index == 2)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw3();
            }
            else if (index == 3)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw4();
            }



        }
        private JArray getTaleShop()
        {
            JArray al = new JArray();
            al.Add(createShop("2000银两兑换 Lv30 荣誉之刃 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉项链 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉戒指 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉手环 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉之帽 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉战衣 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉腰带 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉护腿 +7"));
            al.Add(createShop("2000银两兑换 Lv30 荣誉之鞋 +7"));

            al.Add(createShop("4000银两兑换 Lv50 杀戳之刃 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳项链 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳戒指 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳手环 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳之帽 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳战衣 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳腰带 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳护腿 +7"));
            al.Add(createShop("4000银两兑换 Lv50 杀戳之鞋 +7"));

            return al;
        }
        private JArray getGoldShop()
        {
            JArray al = new JArray();
            al.Add(createShop("200元宝兑换 Lv50 残暴之刃 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴项链 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴戒指 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴手环 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴之帽 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴战衣 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴腰带 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴护腿 +10"));
            al.Add(createShop("200元宝兑换 Lv50 残暴之鞋 +10"));

            al.Add(createShop("500元宝兑换 Lv70 血腥之刃 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥项链 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥戒指 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥手环 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥之帽 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥战衣 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥腰带 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥护腿 +10"));
            al.Add(createShop("500元宝兑换 Lv70 血腥之鞋 +10"));

            return al;
        }
        private JArray getShShop()
        {
            JArray al = new JArray();
            al.Add(createShop("30梦幻珊瑚兑换小药囊x1"));
            al.Add(createShop("60梦幻珊瑚兑换大药囊x1"));
            al.Add(createShop("150梦幻珊瑚兑换超级药囊x1"));
            al.Add(createShop("750梦幻珊瑚兑换无敌药囊x1"));
            al.Add(createShop("1500梦幻珊瑚兑换壮士药囊x1"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦之刃 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦战衣 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦之靴 +13"));

            al.Add(createShop("2000梦幻珊瑚兑换幻梦戒指 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦腰带 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦之帽 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦手环 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦护腿 +13"));
            al.Add(createShop("2000梦幻珊瑚兑换幻梦项链 +13"));
            al.Add(createShop("150梦幻珊瑚兑换潜力符石x1"));
            al.Add(createShop("200梦幻珊瑚兑换修复宝石x1"));

            al.Add(createShop("30梦幻珊瑚兑换初锻宝石x1"));
            al.Add(createShop("60梦幻珊瑚兑换锻造宝石x1"));
            al.Add(createShop("150梦幻珊瑚兑换精锻宝石x1"));
            al.Add(createShop("400梦幻珊瑚兑换锻皇宝石x1"));
            al.Add(createShop("30梦幻珊瑚兑换聚灵石x1"));
            al.Add(createShop("60梦幻珊瑚兑换化婴玉x1"));
            al.Add(createShop("150梦幻珊瑚兑换炼神珠x1"));
            al.Add(createShop("400梦幻珊瑚兑换炼神珠x1"));

            al.Add(createShop("100梦幻珊瑚兑换诱敌香草x1"));
            al.Add(createShop("50梦幻珊瑚兑换造化丹x1"));
            al.Add(createShop("30梦幻珊瑚兑换驱敌香草x1"));
            al.Add(createShop("100梦幻珊瑚兑换百里香x1"));
            al.Add(createShop("200梦幻珊瑚兑换聚魔铃x1"));

            return al;
        }
        private JArray getSjShop()
        {
            JArray al = new JArray();
            al.Add(createShop("180梦幻水晶兑换潜力符石x1"));
            al.Add(createShop("200梦幻水晶兑换造化丹x2"));
            al.Add(createShop("300梦幻水晶兑换诱敌香草x3"));
            al.Add(createShop("500梦幻水晶兑换无敌药囊x1"));

            return al;
        }
        private JObject createShop(string name)
        {
            JObject a = new JObject();
            a.Add("name", name);
            return a;
        }
        private void draw4()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            gameObjPool.getInstance().freeChildren(content.gameObject);
            
        }
        private void draw3()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            JArray m2 = getTaleShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    /*face.goodsInterface.buyGoods(a["key"].ToString(), 1, 11, null, (b) =>
                    {

                    });*/
                }, () => { });
            });
        }
        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            JArray m2 = getGoldShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    /*face.goodsInterface.buyGoods(a["key"].ToString(), 1, 11, null, (b) =>
                    {

                    });*/
                }, () => { });
            });
        }
        /*private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            JArray m2 = getShShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    *//*face.goodsInterface.buyGoods(a["key"].ToString(), 1, 11, null, (b) =>
                    {

                    });*//*
                }, () => { });
            });
        }*/
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            JArray m2 = getSjShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    face.activityInterface.exchangeByShuiJing(mIndex, () =>
                    {

                    });
                    /*face.goodsInterface.buyGoods(a["key"].ToString(), 1, 11, null, (b) =>
                    {

                    });*/
                }, () => { });
            });
        }
        private void drawBtn(string str, Transform ts, Vector2 pos, Action ac)
        {
            GameObject tb = gameObjPool.getInstance().get("tb", typeof(ImgUI));
            tb.transform.SetParent(ts);
            tb.GetComponent<ImgUI>().setRoundedCorners(1, "#005D5F")
                .setSizePos(new Vector2(120, 60), pos)
                .addClk(ac);
            tb.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(2, -2));
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(tb.transform);
            text.GetComponent<TextUI>().setText(str).setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 60), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));
        }
        public override void init()
        {

        }
    }
}
