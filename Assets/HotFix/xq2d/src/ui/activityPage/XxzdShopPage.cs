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
    public class XxzdShopPage : PageUI
    {
        public XxzdShopPage drawUI()
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
            "兑换","收集"
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
            



        }
        
        private JArray getShShop()
        {
            JArray al = new JArray();
            al.Add(createShop("200血腥徽章兑换汇通金券x1"));
            al.Add(createShop("30血腥徽章兑换小药囊x1"));
            al.Add(createShop("60血腥徽章兑换大药囊x1"));
            al.Add(createShop("150血腥徽章兑换超级药囊x1"));
            al.Add(createShop("750血腥徽章兑换无敌药囊x1"));
            /*al.Add(createShop("1500血腥徽章兑换壮士药囊x1"));
            al.Add(createShop("2000血腥徽章兑换幻梦之刃 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦战衣 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦之靴 +13"));

            al.Add(createShop("2000血腥徽章兑换幻梦戒指 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦腰带 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦之帽 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦手环 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦护腿 +13"));
            al.Add(createShop("2000血腥徽章兑换幻梦项链 +13"));*/
            al.Add(createShop("150血腥徽章兑换潜力符石x1"));
            al.Add(createShop("200血腥徽章兑换修复宝石x1"));

            al.Add(createShop("30血腥徽章兑换初锻宝石x1"));
            al.Add(createShop("60血腥徽章兑换锻造宝石x1"));
            al.Add(createShop("150血腥徽章兑换精锻宝石x1"));
            al.Add(createShop("400血腥徽章兑换锻皇宝石x1"));
            al.Add(createShop("30血腥徽章兑换聚灵石x1"));
            al.Add(createShop("60血腥徽章兑换化婴玉x1"));
            al.Add(createShop("150血腥徽章兑换炼神珠x1"));
            al.Add(createShop("400血腥徽章兑换天元精髓x1"));

            al.Add(createShop("100血腥徽章兑换诱敌香草x1"));
            al.Add(createShop("50血腥徽章兑换造化丹x1"));
            al.Add(createShop("30血腥徽章兑换驱敌香草x1"));
            al.Add(createShop("100血腥徽章兑换百里香x1"));
            al.Add(createShop("200血腥徽章兑换聚魔铃x1"));
            

            return al;
        }
        
        private JObject createShop(string name)
        {
            JObject a = new JObject();
            a.Add("name", name);
            return a;
        }
        private int pageNum = 1;
        private int totalPage;
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            gameObjPool.getInstance().freeChildren(content.gameObject);
            pageNum = 1;
            readData();
        }
        private void readData()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            face.activityInterface.getXxzdOrder(pageNum, (res) =>
            {
                totalPage = (int)res["totalPage"];
                JArray list = (JArray)res["list"];
                JArray m2 = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    JObject b = new JObject();
                    b.Add("name", a["order"] + ". " + a["name"]+" 【" + a["jf"] +"】");
                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {

                }).addScrollTopCall(() =>
                {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    readData();
                }).addScrollBottomCall(() =>
                {
                    pageNum++;
                    if (pageNum > totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    readData();
                });
            });
        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("活动19:30-20:30，前30分钟刷怪获得血腥徽章，后30分钟随机匹配玩家进行争夺（争夺期无论胜负徽章+5，胜利方额外获得败方5-20枚徽章），徽章在活动结束后可兑换所需奖励，每日0点会将徽章清零，请提前兑换奖励")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray m2 = getShShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                /*MsgSureUI.create(this.transform).show("即将购买，是否继续？", () =>
                {
                    face.activityInterface.exchangeByShuiJing(mIndex, () =>
                    {

                    });
                }, () => { });*/
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