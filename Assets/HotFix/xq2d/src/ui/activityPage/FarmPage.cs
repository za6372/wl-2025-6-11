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
    class FarmPage : PageUI
    {
        public FarmPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("农场");
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
            "我的","好友","帮派",
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
        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("每日限偷取10次。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            face.activityInterface.getGangsMemberFarm((res) =>
            {
                JArray list = new JArray();
                for (int i = 0; i < res.Count; i++)
                {
                    string str = res[i]["name"].ToString();
                    if (res[i]["isOpen"].ToString().Equals("0"))
                    {
                        str += "（未开通农场）";
                    }
                    else
                    {
                        if (res[i]["isGet"] != null)
                        {
                            str += "（可偷菜）";
                        }
                    }
                 
                    JObject a = new JObject();
                    a.Add("name", str);
                    list.Add(a);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    if (res[mIndex]["isOpen"].ToString().Equals("0")) return;
                    List<string> m2 = new List<string>() { "进入他的农场"};
                    Menu menu = Menu.create(m2, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        
                    });
                });
            });

        }
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("每日限偷取10次。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            face.activityInterface.getMyFriendFarmMsg((res) =>
            {
                JArray list = new JArray();
                for (int i = 0; i < res.Count; i++)
                {
                    string str = res[i]["name"].ToString();
                    if (res[i]["isOpen"].ToString().Equals("0"))
                    {
                        str += "（未开通农场）";
                    }
                    else
                    {
                        if (res[i]["isGet"] != null)
                        {
                            str += "（可偷菜）";
                        }
                    }

                    JObject a = new JObject();
                    a.Add("name", str);
                    list.Add(a);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    if (res[mIndex]["isOpen"].ToString().Equals("0")) return;
                    List<string> m2 = new List<string>() { "进入他的农场" };
                    Menu menu = Menu.create(m2, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        viewPlayerFarm(res[mIndex]["name"].ToString());
                    });
                });
            });

        }
        private void viewPlayerFarm(string name)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            List<string> m1 = new List<string>();
            face.activityInterface.getPlayerFarmMsg(name,(res) =>
            {
                JObject td = (JObject)res["td"];
                for (int i = 0; i < 6; i++)
                {
                    JObject pm = (JObject)td["p" + i];
                    string str = null;
                    if (pm["isOpen"].ToString().Equals("0"))
                    {
                        str = "未开启";
                    }
                    else if (pm["isOpen"].ToString().Equals("1") && !pm.ContainsKey("key"))
                    {
                        str = "闲置中";
                    }
                    else
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(this.zzKeyToGdKey(pm["key"].ToString()));
                        if ((long)pm["created"] + (long)pm["tm"] < strUtils.getMillis())
                        {
                            str = gd.name + "  " + pm["num"] + "/" + pm["sum"];
                        }
                        else
                        {
                            str = gd.name + "  " + strUtils.nowTimeToEndTime(strUtils.getMillis(), (long)pm["created"] + (long)pm["tm"]);
                        }

                    }
                    m1.Add(str);
                }

                JArray list = new JArray();
                for (int i = 0; i < m1.Count; i++)
                {
                    JObject a = new JObject();
                    a.Add("name", m1[i]);
                    list.Add(a);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    JObject pm = (JObject)td["p" + mIndex];
                    //弹出收获、铲除、施肥、摘取等
                    List<string> m2 = new List<string>();
                    if (pm["isOpen"].ToString().Equals("0"))
                    {

                    }
                    else if (pm["isOpen"].ToString().Equals("1") && !pm.ContainsKey("key"))
                    {

                    }
                    else
                    {
                        if ((long)pm["created"] + (long)pm["tm"] < strUtils.getMillis())
                        {
                            m2.Add("偷菜");
                        }
                    }
                    if (m2.Count == 0) return;
                    Menu menu = Menu.create(m2, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        face.activityInterface.touCai(name,"p" + mIndex, () => { this.viewPlayerFarm(name); });
                    });
                });
            });
        }
        /**
         * 种子的key转道具的key
         */
        private string zzKeyToGdKey(String zzKey)
        {
            //"玄铁矿", "炼神木", "锄头", "槐树叶", "银票", "丹青", "羊毛笔"
            String[] keys = {
                "10000173", "10000172", "10000218", "10150000", "10000220", "10150001", "10150002",
            };
            int index = int.Parse(zzKey.Substring(4)) / 3;
            return keys[index];
        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            List<string> m1 = new List<string>();
            face.activityInterface.getMyFarmMsg((res) =>
            {
                text.GetComponent<TextUI>().setText("剩余偷取次数：" + (10-(int)res["times"]));
                JObject td = (JObject)res["td"];
                for (int i = 0; i < 6; i++)
                {
                    JObject pm = (JObject)td["p" + i];
                    string str = null;
                    if (pm["isOpen"].ToString().Equals("0"))
                    {
                        str = "未开启";
                    }
                    else if (pm["isOpen"].ToString().Equals("1") && !pm.ContainsKey("key"))
                    {
                        str = "闲置中";
                    }
                    else
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(this.zzKeyToGdKey(pm["key"].ToString()));
                        if ((long)pm["created"] + (long)pm["tm"] < strUtils.getMillis())
                        {
                            str = gd.name + "  " + pm["num"] + "/" + pm["sum"];
                        }
                        else
                        {
                            str = gd.name + "  " + strUtils.nowTimeToEndTime(strUtils.getMillis(), (long)pm["created"] + (long)pm["tm"]);
                        }
                        
                    }
                    m1.Add(str);
                }
                m1.Add("领取种子");

                JArray list = new JArray();
                for (int i = 0; i < m1.Count; i++)
                {
                    JObject a = new JObject();
                    a.Add("name", m1[i]);
                    list.Add(a);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    if (mIndex < 6)
                    {
                        JObject pm = (JObject)td["p" + mIndex];
                        //弹出收获、铲除、施肥、摘取等
                        List<string> m2 = new List<string>();
                        if (pm["isOpen"].ToString().Equals("0"))
                        {
                            m2.Add("开启");
                        }
                        else if (pm["isOpen"].ToString().Equals("1") && !pm.ContainsKey("key"))
                        {
                            m2.Add("播种");
                        }
                        else
                        {
                            if ((long)pm["created"] + (long)pm["tm"] < strUtils.getMillis())
                            {
                                m2.Add("收获");
                            }
                            else
                            {
                                m2.Add("施肥");
                                m2.Add("铲除");
                            }
                        }

                        Menu menu = Menu.create(m2, PointGet.getIndexPage());
                        menu.addCallback((mIndex2) =>
                        {
                            this.handle("p" + mIndex, td, m2[mIndex2]);
                        });
                    }
                    else if (mIndex == 6)
                    {
                        if ((int)res["gain"] == 1)
                        {
                            msgCode.showMsg(784);
                            return;
                        }
                        //领取
                        face.activityInterface.gainZhongZi(() =>
                        {
                            res["gain"] = 1;
                        });
                    }
                });
            });
        }
        private void handle(string pos, JObject td, string mName)
        {
            JObject pm = (JObject)td[pos];
            if (mName.Equals("开启"))
            {
                int num = 10;
                if (pos.Equals("p2")) num = 20;
                else if (pos.Equals("p3")) num = 30;
                else if (pos.Equals("p4")) num = 40;
                else if (pos.Equals("p5")) num = 50;
                MsgSureUI.create(PointGet.getTipCanvas()).show("需要消耗锄头x" + num + "，是否继续？", () =>
                    {
                        face.activityInterface.openFarmPos(pos, () => { this.clkTab(0); });
                    }, () => { });
            }
            else if (mName.Equals("播种"))
            {
                //弹出种子选择
                List<string> m2 = new List<string>();
                JArray list= face.goodsInterface.getZhongZi();
                for(int i = 0; i < list.Count; i++)
                {
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(list[i]["key"].ToString());
                    m2.Add(gd.name);
                }
                
                BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择种子");
                menu2.addCallback((mIndex2) =>
                {
                    face.activityInterface.zhongzhi(list[mIndex2]["key"].ToString(), pos, () => { this.clkTab(0); });
                });

            }
            else if (mName.Equals("收获"))
            {
                face.activityInterface.zhaiqu(pos, () => { this.clkTab(0); });
            }
            else if (mName.Equals("施肥"))
            {

            }
            else if (mName.Equals("铲除"))
            {
                face.activityInterface.chanchu(pos, () => { this.clkTab(0); });
            }
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
