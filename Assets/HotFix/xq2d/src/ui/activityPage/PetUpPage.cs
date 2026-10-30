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
using Assets.HotFix.xq2d.src.ui.page;
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
    /**宠物强化*/
    class PetUpPage : PageUI
    {
        public PetUpPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("宠物强化");
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
            "重生","资质","基础","技能","制符","洗点","放生","炼化"
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
            else if (index == 3)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw3();
            }
            else if (index == 4)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw4();
            }
            else if (index == 5)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw5();
            }
            else if (index == 6)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw6();
            }
            else if (index == 7)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw7();
            }
        }
        private void draw7()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("炼化后可获得一定数量的宠物口粮，需40级以上的宠物方能炼化。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "炼化", };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        MsgSureUI.create(this.transform).show("即将炼化，是否继续？", () =>
                        {
                            face.petInterface.lianhuaPet(petItem["Id"].ToString(), () =>
                            {
                                clkTab(7);
                            });
                        }, () => { });
                    }
                });
            });
        }
        private void draw6()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("放生后可恢复一定的人气值，其多少与宠物等级、品质有关，每天仅能放生一次且放生的宠物需达到40级。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "放生", };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        MsgSureUI.create(this.transform).show("即将放生，是否继续？", () =>
                        {
                            face.petInterface.fangshengPet(petItem["Id"].ToString(), () =>
                            {
                                clkTab(6);
                            });
                        }, () => { });
                    }
                });
            });
        }
        private void draw5()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("洗点可重置宠物的属性加点（50级前免费，之后需消耗道具）。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "洗点", };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        string str = "即将洗点";
                        if ((int)petItem["lever"] >= 50)
                        {
                            str = "洗点需消耗易经洗髓丹 x1";
                        }
                        MsgSureUI.create(this.transform).show(str + "，是否继续？", () =>
                          {
                              face.petInterface.resetPoint(petItem["Id"].ToString(), () =>
                              {
                                  clkTab(5);
                              });
                          }, () => { });
                    }
                });
            });
        }
        private void draw4()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("提交特定等级的宠物即可兑换对应的神符，1级神符需50级以上，2级60级，3级70级，4级80级，5级90级。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "兑换神符", };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        MsgSureUI.create(this.transform).show("兑换神符后宠物会消失，是否继续？", () =>
                        {
                            /*face.petInterface.addCao(petItem["Id"].ToString(), () =>
                            {
                                clkTab(3);
                            });*/
                        }, () => { });
                    }
                });
            });
        }
        private void draw3()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "天技开启", "灵通悟性", "技能学习", "技能遗忘" };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        MsgSureUI.create(this.transform).show("需要消耗宠物天启卷轴 x1，是否继续？", () =>
                        {
                            face.petInterface.addCao(petItem["Id"].ToString(), () =>
                            {
                                clkTab(3);
                            });
                        }, () => { });
                    }
                    else if (mIndex3 == 2)
                    {
                        List<string> m3 = new List<string>() { "明心悟性丹", "仙灵悟性丹", "神通悟性丹" };
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, this.transform).setDes("选择悟性丹");
                        menu3.addCallback((mIndex4) =>
                        {
                            string danKey = null;
                            if (mIndex4 == 0) danKey = "10000130";
                            else if (mIndex4 == 1) danKey = "10000118";
                            else if (mIndex4 == 2) danKey = "10000119";
                            face.petInterface.eatPetWxd(petItem["Id"].ToString(), danKey, () =>
                            {
                                clkTab(3);
                            });
                        });
                    }
                    else if (mIndex3 == 3)
                    {
                        List<string> m3 = new List<string>();
                        JArray arr = face.goodsInterface.getPetSkillBook();
                        for (int i = 0; i < arr.Count; i++)
                        {
                            JObject gd = (JObject)arr[i];
                            GoodsDes des = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(gd["key"].ToString());
                            string name = des.name;
                            m3.Add(name);
                        }
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, this.transform).setDes("选择技能书");
                        menu3.addCallback((mIndex4) =>
                        {
                            MsgSureUI.create(this.transform).show("技能将随机选择一个位置打入（可能会覆盖技能，请先将回天书放入背包），是否继续？", () =>
                            {
                                face.skillInterface.learnPetSkill(arr[mIndex4]["key"].ToString(), petItem["Id"].ToString(), (res) =>
                               {
                                   if (res == null)
                                   {
                                       clkTab(3);
                                   }
                                   else
                                   {
                                       JObject pet = face.petInterface.getOneById(petItem["Id"].ToString());
                                       JArray skls = (JArray)pet["attr"]["skill"];
                                       JObject fgSkl = (JObject)skls[(int)res["index"]];
                                       GoodsDes fgDes = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(fgSkl["key"].ToString());

                                       MsgSureUI.create(this.transform).maskNoClk().show("即将覆盖" + fgDes.name + "，是否使用宠物回天书 x1 进行取消?（点确认则覆盖，点取消则会使用回天书进行取消覆盖，两分钟考虑，超出时间则默认按使用回天书处理，除非背包中没有）", () =>
                                       {
                                           //传入打的技能key
                                           face.skillInterface.surePetSkillCover(petItem["Id"].ToString(), 1, 1, arr[mIndex4]["key"].ToString(), (int)res["index"], () => { clkTab(3); });
                                       }, () =>
                                       {
                                           face.skillInterface.surePetSkillCover(petItem["Id"].ToString(), 0, 1, arr[mIndex4]["key"].ToString(), (int)res["index"], () => { clkTab(3); });
                                       });
                                       MsgSureUI.create(this.transform).maskNoClk().show("即将覆盖" + fgDes.name + "，是否需要购买宠物回天书 x1？（关闭该面板后可看见覆盖确认的面板，两分钟后未做出任何选择会自动选择消耗回天书进行取消，如果没有则会覆盖）", () =>
                                       {
                                           face.goodsInterface.buyGoods("10000120", 1, 0, null, (b) => { });
                                       }, () =>
                                       {

                                       });
                                   }
                               });
                            }, () => { });
                        });
                    }
                    else if (mIndex3 == 4)
                    {
                        List<string> m3 = new List<string>();
                        JArray skls = (JArray)petItem["attr"]["skill"];
                        for (int i = 0; i < skls.Count; i++)
                        {
                            JObject skl = (JObject)skls[i];
                            string str = null;
                            if ((int)skl["isOpen"] != 1)
                            {
                                str = "未开启";
                            }
                            else if (skl["key"] == null)
                            {
                                str = "普通技能槽";
                            }
                            else
                            {
                                Skill a = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                                str = a.name + " (" + (a.triggerType == 0 ? "主动" : "被动") + ")";
                            }
                            m3.Add(str);
                        }
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, this.transform).setDes("选择遗忘技能");
                        menu3.addCallback((mIndex4) =>
                        {
                            JObject skl = (JObject)skls[mIndex4];
                            if (!skl.ContainsKey("key"))
                            {
                                msgCode.showMsg(208);
                                return;
                            }
                            else if (mIndex4 == 0)
                            {
                                msgCode.showMsg(207);
                                return;
                            }
                            Skill a = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                            MsgSureUI.create(this.transform).show("选择遗忘 " + a.name + "，需要消耗技能遗忘卷轴x1，是否继续？", () =>
                            {
                                face.petInterface.forgetPetSkill(petItem["Id"].ToString(), skl["key"].ToString(), () =>
                                {
                                    clkTab(3);
                                });
                            }, () => { });
                        });
                    }


                });
            });
        }

        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                JObject petItem = (JObject)gds[mIndex2];
                List<string> m2 = new List<string>() { "查看", "吃丹", "吃逆转散", };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else if (mIndex3 == 1)
                    {
                        List<string> m3 = new List<string>();
                        JArray arr = face.goodsInterface.getPetDan();
                        for (int i = 0; i < arr.Count; i++)
                        {
                            JObject gd = (JObject)arr[i];
                            GoodsDes des = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(gd["key"].ToString());
                            string name = des.name;
                            m3.Add(name);
                        }
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, this.transform).setDes("选择健骨丹");
                        menu3.addCallback((mIndex4) =>
                        {
                            MsgSureUI.create(this.transform).show("即将吃健骨丹，是否继续？", () =>
                            {
                                face.petInterface.eatPetDan(petItem["Id"].ToString(), arr[mIndex4]["key"].ToString(), () =>
                                {
                                    clkTab(2);
                                });
                            }, () => { });
                        });
                    }
                    else if (mIndex3 == 2)
                    {
                        List<string> m3 = new List<string>() { "低级天命逆转散", "中级天命逆转散", "高级天命逆转散" };
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, this.transform).setDes("选择逆转散");
                        menu3.addCallback((mIndex4) =>
                        {
                            string danKey = null;
                            if (mIndex4 == 0) danKey = "10000168";
                            else if (mIndex4 == 1) danKey = "10000169";
                            else danKey = "10000170";
                            List<string> m4 = new List<string>() { "力量", "敏捷", "智力", "耐力", "精神" };
                            BaoshiChooseMenuUI menu4 = BaoshiChooseMenuUI.create(m4, this.transform).setDes("选择转出的属性");
                            menu4.addCallback((mIndex5) =>
                            {
                                string[] ps = { "ll", "mj", "zl", "nl", "js" };
                                MsgSureUI.create(this.transform).show("即将转出属性，是否继续？", () =>
                                {
                                    face.petInterface.eatPetNzs(petItem["Id"].ToString(), danKey, ps[mIndex5], () =>
                                     {
                                         clkTab(2);
                                     });
                                }, () => { });
                            });
                        });
                    }

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
            text.GetComponent<TextUI>().setText("宠物合成会吃掉副宠来提升主宠的成长度，满1000后晋升1级，同时资质上限会提升，同一只主宠每日吃宠数量限制10只，并且主宠要求至少60级，副宠40级，且至少为二品，两成长等级差异不超过2，成6后需突破后方能继续提升，成8后需进化后方能继续提升，最大成长等级10。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                List<string> m2 = new List<string>() { "查看", "宠物合成", "成长强化", "潜能突破", "宠物进化", "资质洗练" };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    JObject petItem = (JObject)gds[mIndex2];
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, petItem);
                    }
                    else if (mIndex3 == 1)
                    {
                        List<string> m3 = new List<string>();
                        JArray arr = face.petInterface.getFuPetList(petItem["Id"].ToString());
                        for (int i = 0; i < arr.Count; i++)
                        {
                            JObject gd = (JObject)arr[i];
                            string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];
                            m3.Add(name);
                        }
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, PointGet.getIndexPage()).setDes("选择要吃掉的副宠");
                        menu3.addCallback((mIndex4) =>
                        {
                            MsgSureUI.create(this.transform).show("吃掉后副宠会消失，是否继续？", () =>
                            {
                                face.petInterface.eatPet(petItem["Id"].ToString(), arr[mIndex4]["Id"].ToString(), () =>
                                {
                                    clkTab(1);
                                });
                            }, () => { });
                        });
                    }
                    else if (mIndex3 == 2)
                    {
                        //使用元宝强化
                        int v = face.petInterface.getQiangHuaZhi((int)petItem["growLv"]);
                        if (v == 0)
                        {
                            msgCode.showMsg(212);
                            return;
                        }
                        MsgSureUI.create(this.transform).show("宠物增加"+v+"成长值，需要消耗500元宝，是否继续？", () =>
                        {
                            face.petInterface.petQiangHua(petItem["Id"].ToString(),  () =>
                            {
                                clkTab(1);
                            });
                        }, () => { });
                    }
                    else if (mIndex3 == 3)
                    {
                        MsgSureUI.create(this.transform).show("需要消耗人参果 x5，是否继续？", () =>
                        {
                            face.petInterface.breach(petItem["Id"].ToString(), () =>
                            {
                                clkTab(1);
                            });
                        }, () => { });
                    }
                    else if (mIndex3 == 4)
                    {
                        MsgSureUI.create(this.transform).show("需要消耗宠物进化石 x1，是否继续？", () =>
                        {
                            face.petInterface.evolution(petItem["Id"].ToString(), () =>
                            {
                                clkTab(1);
                            });
                        }, () => { });
                    }
                    else
                    {
                        MsgSureUI.create(this.transform).show("需要消耗资质重置丹 x1，是否继续？", () =>
                        {
                            face.petInterface.reZizhi(petItem["Id"].ToString(), () =>
                            {
                                clkTab(1);
                            });
                        }, () => { });
                    }
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
            text.GetComponent<TextUI>().setText("宠物重生后等级降为1，所有强化效果均消失（包含成长等级），并且成长率会被重置。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray list = new JArray();
            JArray gds = face.petInterface.getPetList();
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                if ((int)gd["isFight"] == 1)
                {
                    gds.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < gds.Count; i++)
            {
                JObject gd = (JObject)gds[i];
                string name = "Lv" + gd["lever"] + " " + gd["nickName"] + " 成" + gd["growLv"];

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex2) =>
            {
                List<string> m2 = new List<string>() { "查看", "重生" };
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex3) =>
                {
                    if (mIndex3 == 0)
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)gds[mIndex2]);
                    }
                    else
                    {
                        List<string> m3 = new List<string>() { "保留技能", "重置技能", };
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, PointGet.getIndexPage()).setDes("请选择宠物重生的方式");
                        menu3.addCallback((mIndex4) =>
                        {
                            MsgSureUI.create(this.transform).show("消耗宠物重生丹 x1 进行重生，是否继续？", () =>
                            {
                                face.petInterface.reLive(gds[mIndex2]["Id"].ToString(), mIndex4, () =>
                                 {
                                     clkTab(0);
                                 });
                            }, () => { });
                        });
                    }
                });
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
