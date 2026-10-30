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
    class XianRenMiFaPage : PageUI
    {
        public XianRenMiFaPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("秘法仙诀");
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
            "秘法","仙诀","护符","法宝",
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



        }
        private void draw3()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("翻天印的属性分三种化身：神将（增加/减少玩家的伤害）、仙灵（增加/减少宠物伤害）、禽兽（增加/减少怪物伤害）。法宝只能带其中一种化身，可通过洗练来切换化身。法宝装备到身上后会每分钟消耗4点灵气，耗尽时法宝作用消失，可通过注灵来增加灵气。法宝有蓝、紫、橙三种品质，可通过注灵来提升，高品质的会激活宝石镶嵌孔。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            //菜单 法宝强化、法宝洗练、法宝注灵
            string[] m2 = { "法宝强化", "法宝洗练", "法宝注灵", };
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
                JObject role = face.roleInterface.getRole();
                if (strUtils.isNull(role["attr"]["equip"]["fb"]))
                {
                    msgCode.showMsg(953);
                    return;
                }
                JObject fb = (JObject)role["attr"]["equip"]["fb"];

                if (mIndex == 0)
                {
                    //int num = ((int)fb["forging"]["lv"] + 1) * 2;
                    int lv = (int)fb["forging"]["lv"];
                    if (lv >= 50)
                    {
                        msgCode.showMsg(656);
                        return;
                    }
                    int[] nums = {
                        3,3,3,4,5,5,6,8,9,11,
                        12,13,15,16,17,19,21,23,26,29,
                        32,37,42,48,54,60,64,68,73,77,
                        83,90,97,104,111,119,127,135,144,155,
                        166,178,191,204,217,232,247,263,279,294,
                    };
                    int num = nums[lv];
                    float p = 1;
                    /*if ((int)fb["forging"]["lv"] < 30) p = 1;
                    else p = (float)Math.Pow(0.9f, (int)fb["forging"]["lv"] - 29) * (1 + 0.1f);*/
                    MsgSureUI.create(this.transform).show("当前强化等级为Lv" + fb["forging"]["lv"]
                        + "，继续强化需要消耗法宝强化符 x" + num + "，成功率为" + (int)(p * 100) + "%，是否继续？", () =>
                           {
                               face.equipInterface.upFbLv(() =>
                               {
                                   clkTab(3);
                               });
                           }, () => { });
                }
                else if (mIndex == 1)
                {
                    List<string> m2 = new List<string>() { "神将化身", "仙灵化身", "禽兽化身" };
                    string hs = m2[int.Parse(fb["forging"]["type"].ToString())];
                    BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("当前为" + hs + "，请选择要洗练的化身");
                    menu2.addCallback((mIndex2) =>
                    {
                        MsgSureUI.create(this.transform).show("消耗200银两和法宝洗练符 x1 进行洗练，是否继续？", () =>
                        {
                            face.equipInterface.xilianFb(mIndex2, () =>
                             {
                                 clkTab(3);
                             });
                        }, () => { });
                    });

                }
                else if (mIndex == 2)
                {
                    List<string> m2 = new List<string>() { "注灵珍露", "装备注灵", };
                    BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择注灵方式");
                    menu2.addCallback((mIndex2) =>
                    {
                        if (mIndex2 == 0)
                        {
                            MsgSureUI.create(this.transform).show("消耗注灵珍露 x1 进行注灵，是否继续？", () =>
                            {
                                face.equipInterface.zhulingFb(0, null, () =>
                                 {
                                     clkTab(3);
                                 });
                            }, () => { });
                        }
                        else
                        {
                            //弹出选择装备
                            //要求装备>=60级
                            JArray fuList = face.equipInterface.getZhulingEquip();
                            List<string> m3 = new List<string>();
                            for (int i = 0; i < fuList.Count; i++)
                            {
                                JObject fu = (JObject)fuList[i];
                                Equip fuMsg = (Equip)face.goodsInterface.getGoodsMsgByKey(fu["key"].ToString());
                                m3.Add(fuMsg.getPerfectName(fu));
                            }
                            //弹出副装备选择
                            BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, PointGet.getIndexPage()).setDes("请选择注灵装备");
                            menu3.addCallback((mIndex3) =>
                            {
                                MsgSureUI.create(this.transform).show("消耗装备进行注灵，是否继续？", () =>
                                {
                                    face.equipInterface.zhulingFb(1, fuList[mIndex3]["Id"].ToString(), () =>
                                    {
                                        clkTab(3);
                                    });
                                }, () => { });

                            });
                        }

                    });
                }
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
            text.GetComponent<TextUI>().setText("护符用于抵抗暴击、睡眠、流血、混乱的影响，品质越高的护符拥有的抗性就越多。同时声望越高，护符能发挥的能力就越强。每日可免费炼制一次护符。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            //菜单 炼制护符、兑换声望、
            string[] m2 = { "炼制护符", "兑换声望", };
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
                    MsgSureUI.create(this.transform).show("即将炼制护符，是否继续？", () =>
                    {
                        face.equipInterface.getHf(() =>
                        {
                            clkTab(2);
                        });
                    }, () => { });
                }
                else
                {
                    //获取背包中的护符
                    JArray fuList = face.goodsInterface.getAllHufu();
                    List<string> m2 = new List<string>();
                    for (int i = 0; i < fuList.Count; i++)
                    {
                        JObject m = (JObject)fuList[i];
                        GoodsDes fuMsg = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(m["key"].ToString());
                        m2.Add(fuMsg.name);
                    }
                    BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择护符");
                    menu2.addCallback((mIndex2) =>
                    {
                        JObject sk = (JObject)fuList[mIndex2];
                        MsgSureUI.create(this.transform).show("即将兑换声望，是否继续？", () =>
                        {
                            face.equipInterface.exchangeShengwang(sk["Id"].ToString(), () =>
                             {
                                 clkTab(2);
                             });
                        }, () => { });
                    });
                }
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
            text.GetComponent<TextUI>().setText("需玩家70级后开启。分天地人三诀，天、地诀最高等级4级，人诀最高1级，学习仙诀前需消耗人物刻印卷轴来开启刻印。学习人诀会随机选择一个打通的刻印进行覆盖，如果想要取消覆盖需要消耗回天书，请提前将回天书放入背包。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //列举所学的仙诀，每开一个槽就添加一个{type,index}，没有key说明就是未打技能，type是该槽的技能类型，1天2地3人，index表示槽位置（天地都是0，人按index排，10个人技）
            List<object> ms = new List<object>();

            JArray skls = (JArray)face.roleInterface.getRole()["attr"]["skill"];
            JArray list = new JArray();
            for (int i = 0; i < 12; i++)
            {
                string name = null;
                if (i == 0)
                {
                    //找天技
                    JObject skl = face.skillInterface.getSkl(1, 0, skls);
                    if (skl == null)
                    {
                        name = "[天]未开启";
                    }
                    else
                    {
                        if (skl.ContainsKey("key"))
                        {
                            Skill s = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                            name = "[天]" + " Lv" + skl["lv"] + " " + s.name;
                        }
                        else
                        {
                            name = "[天]已开启";
                        }
                    }
                    ms.Add(skl);
                }
                else if (i == 1)
                {
                    //找地技
                    JObject skl = face.skillInterface.getSkl(2, 0, skls);
                    if (skl == null)
                    {
                        name = "[地]未开启";
                    }
                    else
                    {
                        if (skl.ContainsKey("key"))
                        {
                            Skill s = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                            name = "[地]" + " Lv" + skl["lv"] + " " + s.name;
                        }
                        else
                        {
                            name = "[地]已开启";
                        }
                    }
                    ms.Add(skl);
                }
                else
                {
                    //找人技
                    JObject skl = face.skillInterface.getSkl(3, i - 2, skls);
                    if (skl == null)
                    {
                        name = "[人]未开启";
                    }
                    else
                    {
                        if (skl.ContainsKey("key"))
                        {
                            Skill s = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                            name = "[人]" + " Lv" + skl["lv"] + " " + s.name;
                        }
                        else
                        {
                            name = "[人]已开启";
                        }
                    }
                    ms.Add(skl);
                }

                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = null;
                if (ms[mIndex] != null) obj = (JObject)ms[mIndex];
                List<string> menuNames = new List<string>();

                if (obj == null)//弹出 打通刻印
                {
                    menuNames.Add("打通刻印");
                }
                else if (obj.ContainsKey("key"))//弹出 遗忘仙诀、学习仙诀
                {
                    menuNames.Add("遗忘仙诀"); menuNames.Add("学习仙诀");
                }
                else if (!obj.ContainsKey("key"))//弹出 学习仙诀
                {
                    menuNames.Add("学习仙诀");
                }
                Menu menu2 = Menu.create(menuNames, this.transform);
                menu2.addCallback((mIndex2) =>
                {
                    int type = 0;
                    int index = 0;
                    if (mIndex == 0) type = 1;
                    else if (mIndex == 1) type = 2;
                    else
                    {
                        type = 3;
                        index = mIndex - 2;
                    }
                    if (menuNames[mIndex2].Equals("打通刻印"))
                    {
                        MsgSureUI.create(this.transform).show("打通刻印需要人物刻印卷轴x6，是否继续？", () =>
                        {
                            face.skillInterface.openXianJueKeyin(type, index, () => { clkTab(1); });
                        }, () => { });

                    }
                    else if (menuNames[mIndex2].Equals("遗忘仙诀"))
                    {
                        MsgSureUI.create(this.transform).show("即将消耗技能遗忘卷轴 x1来遗忘该技能，是否继续？", () =>
                        {
                            face.skillInterface.forgetSkill(type, index, () => { clkTab(1); });
                        }, () => { });
                    }
                    else if (menuNames[mIndex2].Equals("学习仙诀"))
                    {
                        //弹出仙诀
                        JArray fuList = face.goodsInterface.getXianjue(type);
                        List<string> m2 = new List<string>();
                        for (int i = 0; i < fuList.Count; i++)
                        {
                            JObject m = (JObject)fuList[i];
                            GoodsDes fuMsg = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(m["key"].ToString());
                            m2.Add(fuMsg.name);
                        }
                        BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择要学习的仙诀");
                        menu2.addCallback((mIndex2) =>
                        {
                            JObject sk = (JObject)fuList[mIndex2];
                            JObject sklObj = face.skillInterface.getSkl(type, index, skls);
                            int num = 1;
                            if ((type == 1 || type == 2) && sklObj.ContainsKey("lv"))
                            {
                                num = (int)sklObj["lv"] * 2;
                            }
                            GoodsDes fuMsg = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(sk["key"].ToString());
                            MsgSureUI.create(this.transform).show("即将消耗" + fuMsg.name + " x" + num + "，是否继续？", () =>
                                {
                                    face.skillInterface.learnSkill(sk["key"].ToString(), (res) =>
                                    {
                                        if (res == null)
                                        {
                                            clkTab(1);
                                        }
                                        else
                                        {
                                            JObject fgSkl = face.skillInterface.getSkl(3, (int)res["index"], skls);
                                            GoodsDes fgDes = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(fgSkl["key"].ToString());

                                            MsgSureUI.create(this.transform).maskNoClk().show("即将覆盖" + fgDes.name + "，是否使用人物回天书 x1 进行取消？（点确认则覆盖，点取消则会使用回天书进行取消覆盖，两分钟考虑，超出时间则默认按使用回天书处理，除非背包中没有）", () =>
                                            {
                                                //传入打的技能key
                                                face.skillInterface.sureSkillCover(1, 0, sk["key"].ToString(), (int)res["index"], () => { clkTab(1); });
                                            }, () =>
                                            {
                                                face.skillInterface.sureSkillCover(0, 0, sk["key"].ToString(), (int)res["index"], () => { clkTab(1); });
                                            });
                                            MsgSureUI.create(this.transform).maskNoClk().show("即将覆盖" + fgDes.name + "，是否需要购买人物回天书 x1？（关闭该面板后可看见覆盖确认的面板，两分钟后未做出任何选择会自动选择消耗回天书进行取消，如果没有则会覆盖）", () =>
                                            {
                                                //传入打的技能key
                                                face.goodsInterface.buyGoods("10000128", 1, 0, null, (b) => { });
                                            }, () =>
                                            {

                                            });
                                        }

                                    });
                                }, () => { });
                        });
                    }
                });
                /*MsgSureUI.create(this.transform).show(str + "是否继续？", () =>
                {
                    face.roleInterface.upXrmf(mIndex, () =>
                    {
                        clkTab(1);
                    });
                }, () => { });*/
            });
            //菜单 开启刻印、学习仙绝、
        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("需玩家80级后开启。仙人模式可提升玩家生命上限，仙法模式可提升宠物生命上限。强化最大等级30级。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JObject xrmf = (JObject)face.roleInterface.getRole()["attr"]["msg"]["xrmf"];
            string[] m2 = { "提升仙人模式（Lv" + xrmf["xrLv"] + "）", "提升仙法通灵（Lv" + xrmf["xfLv"] + "）", };
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
                //+25 +30 +40 +50   lever * 20 + Math.pow(1.41,lever)/3;
                //1级20 2级45 3级75 4级115 5级165 12级905 13级1105 15级1575 16级1855 21级3735 22级4225 30级9895
                JObject xrmf = face.roleInterface.getXrmf();

                string str = null;
                int lv = 0;
                long tale = 0;
                int xhNum = 0;
                if (mIndex == 0)
                {
                    int xrLv = (int)xrmf["xrLv"] + 1;
                    if (xrLv > 30) return;
                    int d = (int)(xrLv * 20f + Math.Pow(1.41f, xrLv) / 3f);
                    lv = (int)xrmf["xrLv"] + 1;
                    tale = lv * 1000;
                    int[] xhNums = { 4, 5, 6, 7, 9, 11, 13, 15, 17, 27, 33, 39, 45, 51, 61, 71, 81, 91, 101, 125 };
                    if (lv > 10)
                    {
                        xhNum = xhNums[lv - 11];
                        if (lv > 11) tale = (lv - 10) * 10000;
                    }
                    str = "提升仙人模式角色生命增加" + d + ",需消耗" + tale + "银两，";
                    if (lv > 10) str += "仙人神水 x" + xhNum + "，";
                }
                else
                {
                    int xfLv = (int)xrmf["xfLv"] + 1;
                    if (xfLv > 30) return;
                    int d = (int)(xfLv * 20f + Math.Pow(1.41f, xfLv) / 3f);
                    lv = (int)xrmf["xfLv"] + 1;
                    tale = lv * 1000;
                    if (lv > 10) xhNum = lv - 10;
                    str = "提升仙法通灵宠物生命增加" + d + ",需消耗" + tale + "银两，";
                    if (lv > 10) str += "通灵神水 x" + xhNum + "，";
                }
                float p = 1f;
                //概率公式
                if (lv < 11) p = 1;
                else p = (float)Math.Pow(0.98, lv - 10);
                str += "成功率为" + (int)(p * 100) + "%，";
                MsgSureUI.create(this.transform).show(str + "是否继续？", () =>
                {
                    face.roleInterface.upXrmf(mIndex, () =>
                    {
                        clkTab(0);
                    });
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
