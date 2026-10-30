using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.jimai;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class JiMaiPage : PageUI
    {
        public JiMaiPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("寄卖");
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
            "购买","寄卖"
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

        }
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            List<string> ml = new List<string>() { "道具", "宠物", "取回", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                JArray list = new JArray();
                JArray gds = null;
                if (mIndex == 0)
                {
                    gds = face.goodsInterface.getAllGoods();
                    for (int i = 0; i < gds.Count; i++)
                    {
                        JObject gd = (JObject)gds[i];
                        if (face.emailInterface.isNoAllowedSend(gd))
                        {
                            gds.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int i = 0; i < gds.Count; i++)
                    {
                        JObject gd = (JObject)gds[i];

                        GoodsDes des = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(gd["key"].ToString());
                        string name = des.name + " x" + gd["num"] + " ";
                        JObject a = new JObject();
                        a.Add("name", name);
                        a.Add("Id", gd["Id"].ToString());
                        list.Add(a);
                    }
                    renderMenu1(content, list, mIndex, gds, tab);
                }
                else if (mIndex == 1)
                {
                    gds = face.petInterface.getPetList();
                    for (int i = 0; i < gds.Count; i++)
                    {
                        JObject gd = (JObject)gds[i];
                        if ((int)gd["growLv"] >= 6 || (int)gd["isFight"] == 1)
                        {
                            gds.RemoveAt(i);
                            i--;
                        }
                    }
                    for (int i = 0; i < gds.Count; i++)
                    {
                        JObject gd = (JObject)gds[i];

                        string name = gd["nickName"] + " x" + 1 + " ";

                        JObject a = new JObject();
                        a.Add("name", name);
                        a.Add("Id", gd["Id"].ToString());
                        list.Add(a);
                    }
                    renderMenu1(content, list, mIndex, gds, tab);
                }
                else
                {
                    face.goodsInterface.findGroundingBySelf((res) =>
                    {
                        gds = new JArray();
                        for (int i = 0; i < res.Count; i++)
                        {
                            JObject gd = (JObject)res[i];
                            JObject aaa = (JObject)JsonConvert.DeserializeObject(gd["gd"].ToString());
                            aaa["num"] = gd["num"];
                            string name = null;
                            if ((int)gd["gdType"] == 0)
                            {
                                GoodsDes des = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(aaa["key"].ToString());
                                name = des.name + " x" + gd["num"] + " ";
                            }
                            else
                            {
                                name = aaa["nickName"] + " x" + 1 + " ";
                            }

                            JObject a = new JObject();
                            a.Add("name", name);
                            a.Add("Id", gd["Id"].ToString());
                            a.Add("gdType", gd["gdType"].ToString());
                            list.Add(a);
                            gds.Add(aaa);
                        }
                        renderMenu1(content, list, mIndex, gds, tab, false);
                    });
                }


            });
            tab.clkDefault();

        }

        private void renderMenu1(Transform content, JArray list, int mIndex, JArray gds, Tab tab, bool isUp = true)
        {
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                if (!isUp)
                {
                    mIndex = (int)list[mIndex2]["gdType"];
                }
                List<string> m2 = new List<string>() { "查看", };
                if (isUp) m2.Add("上架");
                else m2.Add("取回");
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    string mName = m2[mIndex3];
                    if (mName.Equals("查看"))
                    {
                        if (mIndex == 0)//道具详情
                        {
                            GoodsDesUI.create((JObject)gds[mIndex2], this.transform).renderText();
                        }
                        else//宠物详情
                        {
                            PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                        }
                    }
                    else if (mName.Equals("上架"))
                    {
                        //设置价格、数量、单位
                        GroundingPage.create((JObject)gds[mIndex2], tab.chooseIndex, this.transform).addCallback(() =>
                        {
                            tab.clkDefault(tab.chooseIndex);
                        });
                    }
                    else
                    {
                        face.goodsInterface.undercarriage(list[mIndex2]["Id"].ToString(), () => { tab.clkDefault(2); });
                    }

                });
            });

            DoGet.getInstance().startReqImg();
        }

        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            List<string> ml = new List<string>() { "全部", "宝石", "装备", "技能", "其他", "宠物", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                pageNum = 1;
                handle(mIndex, null);
                DoGet.getInstance().startReqImg();
            });
            tab.clkDefault();
            Vector2 size = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject num = gameObjPool.getInstance().get("num", typeof(InputUI));
            num.transform.SetParent(hb, false);
            num.GetComponent<InputUI>().setSizePos(new Vector2(250, 60), new Vector2(20, -size.y + 80));
            num.GetComponent<InputUI>().initSetting("商品名").addChange((v) =>
            {
                //延时执行
                timeManager.getTimeManageOne().putDelayTask(() =>
                {
                    pageNum = 1;
                    handle(0, v);
                }, 1000);

            }).addMatchNum().setContent("").setRoundedCorners(0.5f);

            //搜索
            /*GameObject inp = gameObjPool.getInstance().get("inp", typeof(InputUI));
            inp.transform.SetParent(hb.transform, false);
            inp.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(20, -320));
            inp.GetComponent<InputUI>().initSetting("搜索商品").addMatchNum().setContent("").setRoundedCorners(0.5f);*/
        }
        private int pageNum = 1;
        private int totalPage = 0;
        private void handle(int type, string keywords)
        {
            if (keywords != null && keywords.Trim().Equals(""))
            {
                keywords = null;
            }
            face.goodsInterface.findGrounding(type, keywords, pageNum, (res) =>
            {
                //Debug.Log(res);
                totalPage = (int)res["totalPage"];
                JArray m2 = (JArray)res["list"];
                JArray list = new JArray();
                for (int i = 0; i < m2.Count; i++)
                {
                    JObject aaa = (JObject)JsonConvert.DeserializeObject(m2[i]["gd"].ToString());
                    string name = null;
                    if (m2[i]["gdType"].ToString().Equals("0"))
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(aaa["key"].ToString());
                        name = gd.name;
                        if (face.equipInterface.isEquip(aaa["key"].ToString()))
                        {
                            name = ((Equip)gd).getPerfectName(aaa);
                        }

                    }
                    else
                    {
                        Pet pet = face.petInterface.getPetDataByKey(aaa["key"].ToString());
                        name = pet.name;
                    }
                    JObject a = new JObject();
                    a.Add("name", name + "    " + m2[i]["price"] + ((int)m2[i]["priceType"] == 0 ? "元宝" : "银两") + "/每件");
                    list.Add(a);
                }
                Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    JObject wp = (JObject)m2[mIndex];
                    JObject aaa = (JObject)JsonConvert.DeserializeObject(wp["gd"].ToString());
                    aaa["num"] = wp["num"];
                    //先获取物品，显示物品详情，显示购买、取消按钮
                    if (wp["gdType"].ToString().Equals("0"))
                    {
                        GoodsDesUI.create(aaa, this.transform).addBuyBtn(() =>
                        {
                            UseNumInputUI.create(this.transform).addCallback((num) =>
                            {
                                face.goodsInterface.buyGrounding(wp["Id"].ToString(), wp["name"].ToString(), num, (int)wp["price"], (int)wp["priceType"], () =>
                                {
                                    this.handle(type, keywords);
                                });
                            });
                        }).renderText();
                    }
                    else
                    {
                        //宠物详情
                        PetPage petPage = PageUI.createAcPage<PetPage>(this.transform);
                        petPage.drawUI(false, aaa);
                        petPage.addBuyBtn(() =>
                        {
                            face.goodsInterface.buyGrounding(wp["Id"].ToString(), wp["name"].ToString(), 1, (int)wp["price"], (int)wp["priceType"], () =>
                            {
                                this.handle(type, keywords);
                            });
                        });

                    }

                }).addScrollTopCall(() =>
                {
                    if (pageNum <= 1) return;
                    pageNum--;
                    handle(type, keywords);
                }).addScrollBottomCall(() =>
                {
                    if (pageNum >= totalPage) return;
                    pageNum++;
                    handle(type, keywords);
                });
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
            text.transform.SetParent(tb.transform, false);
            text.GetComponent<TextUI>().setText(str).setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 60), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));
        }
        public override void init()
        {

        }
    }
}
