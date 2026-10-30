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
    class BzqjPage : PageUI
    {
        public BzqjPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("百战千军");
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
            "报名","排行","商城"
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
            else if (index == 1)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw1();
            }
            else if (index == 2)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw2();
            }



        }
        private JArray getBgShop()
        {
            JArray al = new JArray();
            al.Add(createShop("10000108", 6000, 2));
            al.Add(createShop("10000160", 8000, 2));
            al.Add(createShop("10000125", 30, 2));
            al.Add(createShop("10000181", 90, 2));
            al.Add(createShop("10000182", 270, 2));
            al.Add(createShop("10000126", 810, 2));
            al.Add(createShop("10000149", 800, 2));
            al.Add(createShop("10000150", 2500, 2));
            al.Add(createShop("10000104", 30, 2));
            al.Add(createShop("10000105", 90, 2));
            al.Add(createShop("10000106", 270, 2));
            al.Add(createShop("10000107", 810, 2));
            al.Add(createShop("10000183", 50, 2));
            al.Add(createShop("10000177", 100, 2));
            al.Add(createShop("10000184", 200, 2));
            al.Add(createShop("10000167", 400, 2));
            al.Add(createShop("10000127", 200, 2));
            al.Add(createShop("10000122", 3000, 2));
            al.Add(createShop("10000121", 3000, 2));

            al.Add(createShop("10110000", 100, 2));
            al.Add(createShop("10110001", 100, 2));
            al.Add(createShop("10110002", 100, 2));
            al.Add(createShop("10110003", 100, 2));
            al.Add(createShop("10110004", 100, 2));
            al.Add(createShop("10110005", 100, 2));
            al.Add(createShop("10110006", 100, 2));
            al.Add(createShop("10110007", 100, 2));
            al.Add(createShop("10110008", 100, 2));
            al.Add(createShop("10110009", 100, 2));

            al.Add(createShop("10030030", 2000, 2));
            al.Add(createShop("10030031", 2000, 2));
            al.Add(createShop("10030032", 2000, 2));
            al.Add(createShop("10030033", 2000, 2));
            al.Add(createShop("10030034", 2000, 2));
            al.Add(createShop("10030035", 2000, 2));
            al.Add(createShop("10030036", 2000, 2));
            al.Add(createShop("10030037", 2000, 2));
            al.Add(createShop("10030038", 2000, 2));
            al.Add(createShop("10030039", 2000, 2));

            for(int i = 0; i < 21; i++)
            {
                string k = i+"";
                if (i < 10) k = "0" + k;
                GoodsDes gd=(GoodsDes) face.goodsInterface.getGoodsMsgByKey("101300" + k);
                al.Add(createShop("101300"+k, gd.getPrice(11), 2));
            }
            return al;
        }
        private JObject createShop(string key, int price, int priceType)
        {
            JObject a = new JObject();
            a.Add("key", key);
            a.Add("price", price);
            a.Add("priceType", priceType);
            return a;
        }
        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            JArray m2 = getBgShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, 5);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    face.goodsInterface.buyGoods(a["key"].ToString(), 1, 11, null, (b) =>
                    {

                    });
                }, () => { });

            });
        }
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            List<string> ml = new List<string>() { "步兵", "骑士", "先锋", "将军", "元帅", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                face.activityInterface.getOrderBzqj(mIndex, (res) =>
                {
                    JArray list = (JArray)res["list"];
                    JArray m2 = new JArray();
                    for (int i = 0; i < list.Count; i++)
                    {
                        JObject a = (JObject)list[i];
                        JObject b = new JObject();
                        b.Add("name", i + 1 + "." + a["name"] + "   【积分：" + a["jf"]+"】");
                        m2.Add(b);
                    }
                    gameObjPool.getInstance().freeChildren(content.gameObject);
                    GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                    gdItem.addCallback((mIndex) =>
                    {

                    });
                });
            });
            tab.clkDefault();

        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("活动每日12:00-13:00/22:00-23:00开始，玩家需报名后方可匹配（同军衔），每场战斗结束后需重新报名，每日限5场，每周进行一次积分结算。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            string[] m2 = { "报名", };
            JArray list = new JArray();
            for (int i = 0; i < m2.Length; i++)
            {
                JObject a = new JObject();
                a.Add("name", m2[i]);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                face.activityInterface.signUpWHJX();
            });
        }
        private void drawBtn(string str, Transform ts, Vector2 pos, Action ac)
        {
            GameObject tb = gameObjPool.getInstance().get("tb", typeof(ImgUI));
            tb.transform.SetParent(ts, false);
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
