using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.shaner;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class ShanErPage : PageUI
    {
        public ShanErPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("人气榜");
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
                "通缉", "排行","兑换","悬赏",
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
            }else if (index == 1)//属性
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
            else if (index == 3)//属性
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw3();
            }



        }
        private JArray getBgShop()
        {
            JArray al = new JArray();
            al.Add(createShop("使用60个积德令兑换1点人气"));
            al.Add(createShop("扬善令牌兑换1点人气"));
            
            return al;
        }
        private JObject createShop(string key)
        {
            JObject a = new JObject();
            a.Add("name", key);
            return a;
        }
        private void draw3()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("可对敌视的玩家发布悬赏追击，但需要给接取者给予一定报酬，赏金将在发布后扣除，超时未完成会退回赏金，玩家接取时将扣除一定保证金，完成后会退回，超时则保证金归发布者所有，同时悬赏双方会类似偷袭那样掉落道具。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            drawBtn("悬赏",hb,new Vector2(10,-10),()=> {
                //弹出接取的悬赏信息
                face.activityInterface.findZslByName((res) => {
                    string str = "悬赏玩家【" + res["player"] + "】";
                    if (res.ContainsKey("map"))
                    {
                        MapData mp = face.mapInterface.getMapByKey(res["map"].ToString());
                        str += "，当前所在地图【" + mp.name + "】";
                    }
                    else
                    {
                        str += "，当前不在线";
                    }
                    if (res.ContainsKey("price"))
                    {
                        str += "，赏金："+res["price"]+((int)res["price_type"]==0?"元宝":"银两");
                    }
                    if (res.ContainsKey("destroy"))
                    {
                        str += "，需在"+strUtils.sToTime((long)res["destroy"])+"之前完成";
                    }
                    
                    MsgSureUI.create(this.transform).show(str, () =>
                    {

                    }, () => { });
                });
            });
            drawBtn("发布", hb, new Vector2(150, -10), () => {
                //弹出发布悬赏
                pubUI.create(this.transform).addCallback((res) =>
                {
                    face.activityInterface.pubZsl((int)res["price"], (int)res["priceType"], res["playerName"].ToString(),()=> { this.clkTab(3); });
                });
            });

            pageNum = 1;
            readDataZsl();

            
        }
        private void readDataZsl()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            face.activityInterface.getZsl(pageNum, (res) =>
            {
                totalPage = (int)res["totalPage"];
                JArray list = (JArray)res["list"];
                JArray m2 = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    JObject b = new JObject();
                    b.Add("name", "【"+ a["player"] + "】 悬赏 "+a["price"]+" "+(a["price_type"].ToString().Equals("0")?"元宝":"银两"));
                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    MsgSureUI.create(this.transform).show("接取该悬赏需要先扣除10%的赏金作为保证金，是否继续？", () =>
                    {
                        face.activityInterface.gainZsl((JObject)list[mIndex], () => { this.clkTab(3); });
                    }, () => { });
                }).addScrollTopCall(() => {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    readData();
                }).addScrollBottomCall(() => {
                    pageNum++;
                    if (pageNum > totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    readDataZsl();
                });
            });
        }
        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            JArray m2 = getBgShop();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)m2[mIndex];
                MsgSureUI.create(this.transform).show("即将兑换，是否继续？", () =>
                {
                   
                }, () => { });

            });
        }
        private int pageNum = 1;
        private int totalPage;
        private int tabIndex = 0;
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("人气值越高善人榜越考前，人气值越低恶人榜越靠前，通过完成通缉任务可获得提升人气值的道具。人气值为负的玩家将遭受天兵的通缉（最多10波，之后需要恢复平民身份后再次成为魔头才能打天兵）。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            List<string> ml = new List<string>() { "善人", "恶人", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                tabIndex = mIndex;
                pageNum = 1;
                readData();
            });
            tab.clkDefault();
        }
        private void readData()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            face.activityInterface.getOrderSez(tabIndex, pageNum, (res) =>
            {
                totalPage = (int)res["totalPage"];
                JArray list = (JArray)res["list"];
                JArray m2 = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    JObject b = new JObject();
                    b.Add("name", a["order"] + ". Lv" + a["lever"] + "  【" + GameAttrConst.getJobToSimpleName(a["model"].ToString()) + "】" + a["name"]+" 【人气值："+a["sez"]+"】");
                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {

                }).addScrollTopCall(() => {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    readData();
                }).addScrollBottomCall(() => {
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
            text.GetComponent<TextUI>().setText("通缉任务会随机分配一名魔头，在他出现的场景进行偷袭并将其击败即可完成任务，被击败的魔头会被关进监狱（需累计30分钟方可出狱），玩家需持有通缉任务在身才不会因为偷袭导致人气值降低。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            string[] m2 = { "通缉任务","放弃任务" };
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
                    MsgSureUI.create(this.transform).show("即将接取通缉任务，是否继续？", () =>
                    {
                        face.activityInterface.gainTongjiTask((res) => {
                            string str = "通缉玩家【" + res["tj_name"] + "】";
                            if (res.ContainsKey("isOnline"))
                            {
                                str += "，当前" + (res["isOnline"].ToString().Equals("0") ? "不在线" : "在线");
                            }
                            if (res.ContainsKey("isOver"))
                            {
                                str += "，任务已过期";
                            }
                            if (res.ContainsKey("isFinish"))
                            {
                                str += "，任务已被其他玩家抢先完成";
                            }
                            if (!res.ContainsKey("isOver") && !res.ContainsKey("isFinish") && res.ContainsKey("mapKey"))
                            {
                                MapData mp = face.mapInterface.getMapByKey(res["mapKey"].ToString());
                                str += "，当前所在地图【" + mp.name + "】";
                            }
                            MsgSureUI.create(this.transform).show(str, () =>
                            {

                            }, () => { });
                        });
                    }, () => { });
                }
                else if (mIndex == 1)
                {
                    MsgSureUI.create(this.transform).show("即将放弃通缉任务，是否继续？", () =>
                    {
                        face.activityInterface.cancelTongjiTask(() => { });
                    }, () => { });
                }
            });
        }
        private void drawBtn(string str, Transform ts, Vector2 pos, Action ac)
        {
            GameObject tb = gameObjPool.getInstance().get("tb", typeof(ImgUI));
            tb.transform.SetParent(ts);
            tb.GetComponent<ImgUI>().setRoundedCorners(1, "#000000")
                .setSizePos(new Vector2(120, 60), pos)
                .addClk(ac);
            tb.AddComponent<UIOutline>().setSome(new Color32(93, 93, 93, 255), new Vector2(2, -2));
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(tb.transform);
            text.GetComponent<TextUI>().setText(str).setAlign().setColor("#CDCDAD").setFontSize(30).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 60), Vector2.zero);
            //text.AddComponent<UIOutline>().setSome(new Color32(93, 93, 93, 255), new Vector2(1, -1));
        }
        public override void init()
        {

        }
    }
}
