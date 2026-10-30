using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.duanzao;
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
    class GangsAcPage : PageUI
    {
        public GangsAcPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("帮派活动");
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
            "帮战","活动","商店"
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
            if (index == 0)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw0();
            }
            else if (index == 1)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw1();
            }
            else if (index == 2)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw2();
            }
        }
        private JArray getYuBoShop()
        {
            JArray al = new JArray();
            al.Add(createShop("10000104", 9, 0));
            al.Add(createShop("10000105", 38, 0));
            al.Add(createShop("10000175", 120, 0));
            al.Add(createShop("10000111", 120, 0));
            al.Add(createShop("10000106", 168, 0));
            return al;
        }
        private JArray getBgShop()
        {
            JArray al = new JArray();
            al.Add(createShop("10000178", 400, 1));
            al.Add(createShop("10000174", 1500, 1));
            al.Add(createShop("10000105", 420, 1));
            al.Add(createShop("10030010", 350, 1));
            al.Add(createShop("10030011", 350, 1));
            al.Add(createShop("10030012", 350, 1));
            al.Add(createShop("10030013", 350, 1));
            al.Add(createShop("10030014", 350, 1));
            al.Add(createShop("10030015", 350, 1));
            al.Add(createShop("10030016", 350, 1));
            al.Add(createShop("10030017", 350, 1));
            al.Add(createShop("10030018", 350, 1));
            al.Add(createShop("10030019", 350, 1));
            al.Add(createShop("10000179", 320, 1));
            return al;
        }
        private JObject createShop(string key, int price,int priceType)
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

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            List<string> ml = new List<string>() { "玉帛", "帮贡", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                JArray m2 = null;
                if (mIndex == 0) m2 = getYuBoShop();
                else m2 = getBgShop();
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, 5);
                gdItem.addCallback((mIndex2) =>
                {
                    if (mIndex == 1)
                    {
                        GoodsDes gd=(GoodsDes) face.goodsInterface.getGoodsMsgByKey(m2[mIndex2]["key"].ToString());
                        MsgSureUI.create(this.transform).show("购买"+ gd.name + "，是否继续？", () =>
                        {
                            face.goodsInterface.buyGoods(m2[mIndex2]["key"].ToString(), 1, 5, null, (b) =>
                            {
                                if (b)
                                {
                                    //刷新帮贡

                                }
                            });
                        }, () => { });
                    }
                    else
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(m2[mIndex2]["key"].ToString());
                        MsgSureUI.create(this.transform).show("兑换" + gd.name + "，是否继续？", () =>
                        {
                            face.activityInterface.exchangBySyb(m2[mIndex2]["key"].ToString(), (int)m2[mIndex2]["price"], () =>
                            {
                               
                            });
                        }, () => { });
                    }

                });
            });
            tab.clkDefault();
            
        }

        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("举办封赏演武堂需要消耗一个封赏玉帛，每日仅限开启一次。跑商需消耗帮派总贡献10000，每日可开启一次，有四次完成的机会。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            string[] m2 = { "举办封赏演武堂","开启跑商活动"  };
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
                if (mIndex == 0)
                {
                    MsgSureUI.create(this.transform).show("需要消耗封赏玉帛 x1，是否继续？", () =>
                        {
                            face.gangsInterface.openBPActivity(1);
                        }, () => { });
                }
                else if(mIndex == 1)
                {
                    MsgSureUI.create(this.transform).show("需要消耗帮派总贡献10000，是否继续？", () =>
                    {
                        face.gangsInterface.openBPActivity(2);
                    }, () => { });
                }

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
            text.GetComponent<TextUI>().setText("帮战开始后场景会出现箱子，占领后会持续获得积分，不同品质的箱子获得的积分不同，玩家占领期间会遭受其他帮派的抢占，20分钟后结束，按积分给予相应奖励。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            string[] m2 = { "报名帮战", "查看帮战排行", };
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
                if (mIndex == 0)
                {
                    face.gangsInterface.signUpBz(() => { });
                }else if (mIndex == 1)
                {

                }
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
