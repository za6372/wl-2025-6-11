using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.activityPage;
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

namespace Assets.HotFix.xq2d.src.ui.page
{
    class ManPage : PageUI
    {
        private bool isMe;
        private JObject role;
        public void drawUI(bool isMe, JObject role = null)
        {
            this.isMe = isMe;
            if (isMe) role = face.roleInterface.getRole();
            this.role = role;

            this.createStandardPageLayout();
            this.setTitle(role["name"].ToString());
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();

        }



        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("属性");
            ml.Add("信息");
            ml.Add("装备");
            ml.Add("技能");
            ml.Add("图鉴");
            ml.Add("状态");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {

                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);

            tab.clkDefault();
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

                this.drawAttr();
            }
            else if (index == 1)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.drawMsg();
            }
            else if (index == 2)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 500), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 500), new Vector2(30, -30 - 500), k2.transform);

                this.drawEquip();
            }
            else if (index == 3)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.drawSkill();
            }
            else if (index == 4)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.tujian();
            }
            else if (index == 5)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.zhuangtai();
            }

        }
        private void zhuangtai()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            if (!this.isMe) return;

            JObject status = (JObject)face.roleInterface.getRole()["attr"]["status"];

            JArray list = new JArray();
            List<string> arr = new List<string>() { "ljxg", "ztys", "pk", "moveSpeed", "jsxx", "bjb", "rwzhd", "cwzhd", "ydxc", "qdxc", "blx", "jml", "dblq", "sffs" };
            for (int i = 0; i < arr.Count; i++)
            {
                JObject a = null;
                if (status.ContainsKey(arr[i]))
                {
                    a = (JObject)status[arr[i]];
                }
                else
                {
                    a = new JObject();
                }
                a.Add("key", arr[i]);
                list.Add(a);
            }


            GoodsItem gdItem = GoodsItem.create(list, content.transform, 7);
            gdItem.addBtnCall((dic) =>
            {
                //点击末尾的开关、多档等按钮
                if (dic["key"].ToString().Equals("pk"))
                {
                    face.roleInterface.setStatusPk((bool)dic["status"] ? 1 : 0, () => { });
                }
                else if (dic["key"].ToString().Equals("bjb"))
                {
                    face.roleInterface.setStatusBjb((bool)dic["status"] ? 1 : 0, () => { });
                }
                else if (dic["key"].ToString().Equals("moveSpeed"))
                {
                    JObject st = face.roleInterface.getGameSetting();
                    st["moveSpeedK"] = (int)dic["value"] + 1;
                    face.roleInterface.saveGameSetting(st);
                }
                else if (dic["key"].ToString().Equals("ydxc") || dic["key"].ToString().Equals("qdxc") || dic["key"].ToString().Equals("blx") ||
                dic["key"].ToString().Equals("jml") || dic["key"].ToString().Equals("cwzhd") || dic["key"].ToString().Equals("rwzhd")
                || dic["key"].ToString().Equals("dblq"))
                {
                    face.roleInterface.setStatusLan(dic["key"].ToString(), (bool)dic["status"] ? 1 : 0, (res) =>
                    {
                        if (res == -1)
                        {
                            string k = null;
                            if (dic["key"].ToString().Equals("ydxc")) k = "10000184";
                            else if (dic["key"].ToString().Equals("qdxc")) k = "10000183";
                            else if (dic["key"].ToString().Equals("rwzhd")) k = "10000177";
                            else if (dic["key"].ToString().Equals("cwzhd")) k = "10000178";
                            else if (dic["key"].ToString().Equals("blx")) k = "10000208";
                            else if (dic["key"].ToString().Equals("jml")) k = "10000209";
                            else if (dic["key"].ToString().Equals("dblq")) k = "10000210";
                            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(k);
                            //提示是否购买诱敌香草
                            string str = "需要花费" + gd.getPrice(0) + "元宝购买" + gd.name + "，是否继续？";
                            if (!face.goodsInterface.isEnoughInPackAndTip(k, 1, false))
                            {
                                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                                {
                                    face.goodsInterface.buyGoods(k, 1, 0, null, (b) =>
                                    {
                                        if (!b) return;
                                        face.goodsInterface.userGoods(k, 1);
                                    });
                                }, () => { });
                            }
                            else
                            {
                                str = "需要消耗" + gd.name + " x1，是否继续？";
                                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                                {
                                    face.goodsInterface.userGoods(k, 1);
                                }, () => { });
                            }
                        }
                    });
                }
            }).addCallback((mIndex) =>
            {
                List<string> m2 = new List<string>();
                if (mIndex == 0 || mIndex == 1)
                {
                    m2 = new List<string>() { "设置", };
                }
                else if (mIndex == 4)
                {
                    m2 = new List<string>() { "初始", "门派", "职业" };
                }
                else if (mIndex == 13)
                {
                    m2 = new List<string>() { "智能", "固定顺序", };
                }
                else return;
                JArray list = new JArray();
                for (int i = 0; i < m2.Count; i++)
                {
                    JObject a = new JObject();
                    a.Add("name", m2[i]);
                    list.Add(a);
                }
                Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                menu2.addCallback((mIndex2) =>
                {
                    if (mIndex == 0 && mIndex2 == 0)
                    {
                        PageUI.createAcPage<LvJingPage>(this.transform).drawUI().clkTab(0);
                    }
                    else if (mIndex == 1 && mIndex2 == 0)
                    {
                        PageUI.createAcPage<ThemePage>(this.transform).drawUI().clkTab(0);
                    }
                    else if (mIndex == 4)
                    {
                        face.roleInterface.changeModel(mIndex2, () =>
                        {
                            //刷新玩家模型
                            controlManager.reloadPlayer();
                        });
                    }
                    else if (mIndex == 13)
                    {
                        if (mIndex2 == 0)
                        {
                            face.fightInterface.uploadShiFaWay(0, -1, -1, -1, () => { });
                        }
                        else if (mIndex2 == 1)
                        {
                            PageUI.createAcPage<ShiFaSetPage>(this.transform).drawUI();
                        }
                    }

                });
            });
        }
        private void tujian()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("完成度")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            gameObjPool.getInstance().freeChildren(content.gameObject);

            if (!this.isMe) return;

            List<string> list = new List<string>() { "兽神之符", "人皇之符", "仙魔之符", "天神之符" };
            JArray m2 = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject b = new JObject();
                b.Add("name", list[i]);
                m2.Add(b);
            }
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                //展开
                expTuJian(mIndex);
            });
        }
        private void expTuJian(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            int start = 10140000 + index * 9; int end = 10140000 + (index + 1) * 9;
            List<string> list = new List<string>();
            if (index == 3)
            {
                end = 10140032;
            }
            for (int i = start; i < end; i++)
            {
                list.Add(i + "");
            }
            JArray m2 = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject b = new JObject();
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(list[i]);
                b.Add("name", gd.name);
                b.Add("key", gd.key);
                m2.Add(b);
            }
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                List<string> ml = new List<string>() { "兑换", "查看" };
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((mIndex2) =>
                {
                    if (mIndex2 == 0)
                    {
                        JObject a = (JObject)m2[mIndex];
                        face.activityInterface.dhSfByCaiLiao(a["key"].ToString(), () => { });
                    }
                    else
                    {
                        JObject qp = new JObject();
                        qp.Add("key", list[mIndex]);
                        GoodsDesUI.create(qp, this.transform, 0).renderText();
                    }
                });
            });
        }
        private JArray sortToList(JArray list)
        {
            List<JObject> al = list.OrderBy(x =>
            {
                if (x["key"] == null) return (int)x["type"];
                Skill gd = (Skill)face.goodsInterface.getGoodsMsgByKey(x["key"].ToString());
                return gd.skillType;
            }).Select(x => JObject.FromObject(x)).ToList();

            return new JArray(al);
        }
        private void drawSkill()
        {
            JArray list = new JArray();
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JArray arr = (JArray)role["attr"]["skill"];
            //对技能排序
            arr = sortToList(arr);

            for (int i = 0; i < arr.Count; i++)
            {
                JObject a = (JObject)arr[i];
                //if (!a.ContainsKey("key")) continue;
                if (!a.ContainsKey("key"))
                {
                    string str = "";
                    if (a["type"].ToString().Equals("1")) str = "天";
                    else if (a["type"].ToString().Equals("2")) str = "地";
                    else if (a["type"].ToString().Equals("3")) str = "人";
                    getOneMsg("[" + str + "] 空技能槽", list);
                }
                else
                {
                    Skill eq = (Skill)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                    getOneMsg("Lv" + a["lv"] + " " + eq.name, list);
                }
            }
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)arr[mIndex];
                if (!a.ContainsKey("key")) return;
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getTipCanvas());
                dialog.renderText(skl.getSkillDes((int)a["lv"]), "center");
                DoGet.getInstance().startReqImg();
            });
        }
        private void drawEquip()
        {
            JArray list = new JArray();
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JObject equip = (JObject)role["attr"]["equip"];
            string[] arr = { "wq", "jb", "sz", "wb", "tb", "xb", "yb", "tuib", "jiaob", "hf", "fb", "bjb", };
            for (int i = 0; i < arr.Length; i++)
            {
                string n = "无";
                if (!equip[arr[i]].ToString().Equals(""))
                {
                    JObject a = (JObject)equip[arr[i]];
                    Equip eq = (Equip)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                    string color = GameAttrConst.getGoodsQualityColor(eq.quality);
                    if (face.equipInterface.isFaBao(eq.key))
                    {
                        color = GameAttrConst.getGoodsQualityColor((int)a["zhuling"]["quality"]);
                    }
                    n = "<color=" + color + ">" + eq.getPerfectName(a) + "</color>";
                }
                getOneMsg(GameAttrConst.equipPartKeyToName(arr[i]) + ":" + n, list);
            }
            JObject petEquip = (JObject)role["attr"]["petEquip"];
            //Debug.Log(role["attr"]["petEquip"]);
            string[] arr1 = { "pet_wq", "pet_fj", "pet_sp", };
            for (int i = 0; i < arr1.Length; i++)
            {
                string n = "无";
                string eKey = arr1[i].Split("_")[1];
                if (!petEquip[eKey].ToString().Equals(""))
                {
                    JObject a = (JObject)petEquip[eKey];
                    PetEquip eq = (PetEquip)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                    string color = GameAttrConst.getGoodsQualityColor(eq.quality);
                    n = "<color=" + color + ">" + eq.name + "</color>";
                }
                getOneMsg(GameAttrConst.equipPartKeyToName(arr1[i]) + ":" + n, list);
            }

            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                if (mIndex >= 12)//宠物装备
                {
                    string eKey = arr1[mIndex - 12].Split("_")[1];
                    if (!petEquip[eKey].ToString().Equals(""))
                    {
                        JObject qp = (JObject)petEquip[eKey];
                        List<string> ml = new List<string>();
                        ml.Add("查看");
                        ml.Add("卸下");
                        Menu menu = Menu.create(ml, this.transform);
                        menu.addCallback((mIndex) =>
                        {
                            if (mIndex == 0)
                            {
                                GoodsDesUI.create(qp, this.transform).renderText();
                            }
                            else if (mIndex == 1)
                            {
                                if (!this.isMe) return;
                                face.equipInterface.downPetEquip(eKey, () =>
                                {
                                    this.role = face.roleInterface.getRole();
                                    this.updateTabPage(2);
                                });
                            }
                        });
                    }
                    return;
                }
                if (!equip[arr[mIndex]].ToString().Equals(""))
                {
                    JObject qp = (JObject)equip[arr[mIndex]];
                    List<string> ml = new List<string>();
                    ml.Add("查看");
                    ml.Add("卸下");
                    Menu menu = Menu.create(ml, this.transform);
                    menu.addCallback((mIndex) =>
                    {
                        if (mIndex == 0)
                        {
                            GoodsDesUI.create(qp, this.transform).renderText();
                        }
                        else if (mIndex == 1)
                        {
                            if (!this.isMe) return;
                            Equip g = (Equip)face.goodsInterface.getGoodsMsgByKey(qp["key"].ToString());
                            face.equipInterface.reqEquipToGoods(qp, g.part, () =>
                            {
                                this.role = face.roleInterface.getRole();
                                this.updateTabPage(2);
                            });
                        }
                    });
                }
                else
                {
                    List<string> ml = new List<string>();
                    //获取背包中的同部位同职业的装备
                    JArray al = face.equipInterface.getEquipByJobAndPart(role["model"].ToString(), arr[mIndex]);
                    if (al.Count == 0)
                    {
                        msgCode.showMsg(637);
                        return;
                    }
                    for (int i = 0; i < al.Count; i++)
                    {
                        JObject a = (JObject)al[i];
                        Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                        string str = "Lv" + gd.lv + " [" + GameAttrConst.getJobToSimpleName(gd.part) + "]" + gd.name;
                        if (a["forging"] != null && (int)a["forging"]["lv"] > 0) str += " +" + a["forging"]["lv"];
                        ml.Add(str);
                    }

                    Menu menu = Menu.create(ml, this.transform);
                    menu.addCallback((mIndex) =>
                    {
                        //询问是否装备
                        face.equipInterface.goodsToEquip(al[mIndex]["Id"].ToString(), () =>
                        {
                            this.role = face.roleInterface.getRole();
                            this.updateTabPage(2);
                        });
                    });
                }

            });

            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            FightModelMsg msg = new FightModelMsg(role["model"].ToString());
            string[] pwds = msg.changguiPwds;
            string[] aefs = { msg.changguiAef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(hb, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.name = "md";
                    g.transform.SetParent(hb, false);
                    AniRuntime am = g.GetComponent<AniRuntime>();
                    am.playOnce(3, 4);
                    g.transform.localPosition = new Vector2(-ScreenUtils.width / 2f + 300, -100);
                    g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();
                }

            }, all);

            /* GameObject md = GameObject.Instantiate<GameObject>(PointGet.getControlPoint().gameObject);
             SetGameObj.remComponent(md.GetComponent<modelMsgBind>());
             //将透明度还原
             Image[] al = md.GetComponentsInChildren<Image>(true);
             foreach (Image img in al)
             {
                 img.color = new Color(1, 1, 1, 1f);
             }
             md.transform.SetParent(hb, false);
             md.transform.localPosition = new Vector2(-ScreenUtils.width / 2f + 300, -100);
             md.GetComponent<AniRuntime>().playOnce(3, 4);
             gameObjPool.getInstance().free(md.transform.Find("name").gameObject);*/

            GameObject item = gameObjPool.getInstance().get("item1", typeof(SimpleUI));
            item.transform.SetParent(hb.transform, false);
            item.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(600, 500), new Vector2(0, 0));

            Vector2[] pos = {
                new Vector2(30,-100),new Vector2(400,-70),new Vector2(150,-70),new Vector2(30,-350),new Vector2(275,-30),
                new Vector2(30,-225),new Vector2(150,-380),new Vector2(400,-380),new Vector2(275,-420),new Vector2(520,-100),
                new Vector2(520,-225),new Vector2(520,-350),
            };
            string[] icons = { "eq_0_png" , "eq_1_png", "eq_2_png", "eq_3_png", "eq_4_png", "eq_5_png"
            , "eq_6_png", "eq_7_png", "eq_8_png", "eq_9_png", "eq_10_png", "eq_11_png"
            };

            for (int i = 0; i < arr.Length; i++)
            {
                GameObject lv = gameObjPool.getInstance().get("lv", typeof(ImgUI));
                lv.transform.SetParent(item.transform, false);
                lv.GetComponent<ImgUI>().setColor("#ffffff")
                    .setSizePos(new Vector2(50, 50), pos[i]);
                lv.AddComponent<UIOutline>().setSome(new Color(0.1058824f, 0.5058824f, 0, 1), new Vector2(1.5f, -1.5f));

                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(lv.transform, false);
                icon.GetComponent<ImgUI>().loadRes(icons[i])
                    .setSizePos(new Vector2(40, 40), new Vector2(5, -5));
            }




        }
        private void drawMsg()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //Debug.Log(role);
            string bpName = "";
            if (!strUtils.isNull(role["attr"]["msg"]["bp"]))
            {
                bpName = role["attr"]["msg"]["bp"]["name"].ToString();
            }
            JObject tea = null;
            if (!strUtils.isNull(role["attr"]["msg"]["teacher"]))
            {
                tea = (JObject)role["attr"]["msg"]["teacher"];
            }
            JObject stu1 = null; JObject stu2 = null; JObject stu3 = null;
            if (!strUtils.isNull(role["attr"]["msg"]["stu1"]))
            {
                stu1 = (JObject)role["attr"]["msg"]["stu1"];
            }
            if (!strUtils.isNull(role["attr"]["msg"]["stu2"]))
            {
                stu2 = (JObject)role["attr"]["msg"]["stu2"];
            }
            if (!strUtils.isNull(role["attr"]["msg"]["stu3"]))
            {
                stu3 = (JObject)role["attr"]["msg"]["stu3"];
            }
            JArray list = new JArray();
            getOneMsg("职业:" + GameAttrConst.getJobToName(GameAttrConst.getModel(role)), list);
            getOneMsg("称号:天下第一", list);
            getOneMsg("帮派:" + bpName, list);
            getOneMsg("身份:" + GameAttrConst.getSf((int)role["attr"]["msg"]["sez"]), list);
            getOneMsg("声望等级:" + role["attr"]["msg"]["swdj"]["lv"], list);
            getOneMsg("银两:" + role["attr"]["msg"]["tale"], list);
            getOneMsg("银票:" + role["attr"]["msg"]["yp"], list);
            getOneMsg("竞技积分:" + role["attr"]["msg"]["jf"], list);
            getOneMsg("元宝:" + role["attr"]["msg"]["gold"], list);
            getOneMsg("等级排行:100名以外", list);
            getOneMsg("帮贡:" + role["attr"]["msg"]["bg"], list);
            getOneMsg("师傅:" + (tea != null ? tea["name"] + " | 情义值" + tea["qhd"] : "无"), list);
            getOneMsg("徒弟1:" + (stu1 != null ? stu1["name"] + " | 情义值" + stu1["qhd"] : "无"), list);
            getOneMsg("徒弟2:" + (stu2 != null ? stu2["name"] + " | 情义值" + stu2["qhd"] : "无"), list);
            getOneMsg("徒弟3:" + (stu3 != null ? stu3["name"] + " | 情义值" + stu3["qhd"] : "无"), list);
            getOneMsg("武勋:"+ role["attr"]["msg"]["wxz"], list);
            getOneMsg("军衔:无", list);
            getOneMsg("军功:" + role["attr"]["msg"]["jungong"], list);

            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {

            });
        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        private void drawAttr()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JArray list = new JArray();
            JObject prop = countUtils.countProp(role);
            string[] arr = { "ll", "mj", "zl", "nl", "js",
            "wg", "fg", "wf", "ff", "mz", "sd", "bj", "css",
            "bjkx", "hlkx", "hskx", "lxkx"};
            for (int i = 0; i < arr.Length; i++)
            {
                string key = arr[i];
                string name = GameAttrConst.propKeyToName(key) + ":" + (int)prop[key];
                JObject a = new JObject();
                a.Add("name", name);
                list.Add(a);
            }
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {

            });
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            FightModelMsg msg = new FightModelMsg(role["model"].ToString());
            string[] pwds = msg.changguiPwds;
            string[] aefs = { msg.changguiAef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(hb, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.name = "md";
                    g.transform.SetParent(hb, false);
                    AniRuntime am = g.GetComponent<AniRuntime>();
                    am.playOnce(3, 4);
                    g.transform.localPosition = new Vector2(-ScreenUtils.width / 2f + 100, -100);
                    g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();
                }

            }, all);

            /*GameObject md = GameObject.Instantiate<GameObject>(PointGet.getControlPoint().gameObject);=
            SetGameObj.remComponent(md.GetComponent<modelMsgBind>());
            //将透明度还原
            Image[] al = md.GetComponentsInChildren<Image>(true);
            foreach (Image img in al)
            {
                img.color = new Color(1, 1, 1, 1f);
            }
            md.transform.SetParent(hb, false);
            md.transform.localPosition = new Vector2(-ScreenUtils.width / 2f + 100, -100);
            md.GetComponent<AniRuntime>().playOnce(3, 4);
            gameObjPool.getInstance().free(md.transform.Find("name").gameObject);*/

            GameObject lv = gameObjPool.getInstance().get("lv", typeof(ImgUI));
            lv.transform.SetParent(hb.transform, false);
            lv.GetComponent<ImgUI>().loadRes("jl04_png")
                .setSizePos(new Vector2(33.4f, 26), new Vector2(30, -30));
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(lv.transform, false);
            text.GetComponent<TextUI>().setText(role["lever"].ToString()).setAlign("left").setColor("#FFEB00").setFontSize(26).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 30), new Vector2(36, 2));
            text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(1, -1));

            GameObject hp = gameObjPool.getInstance().get("hp", typeof(ImgUI));
            hp.transform.SetParent(hb.transform, false);
            hp.GetComponent<ImgUI>().loadRes("js01_png")
                .setSizePos(new Vector2(60, 36), new Vector2(300, -80));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(ImgUI));
            mp.transform.SetParent(hb.transform, false);
            mp.GetComponent<ImgUI>().loadRes("js02_png")
                .setSizePos(new Vector2(60, 36), new Vector2(300, -80 - 100));
            drawTiao(mp, 1, prop);

            GameObject exp = gameObjPool.getInstance().get("exp", typeof(ImgUI));
            exp.transform.SetParent(hb.transform, false);
            exp.GetComponent<ImgUI>().loadRes("js03_png")
                .setSizePos(new Vector2(60, 36), new Vector2(300, -80 - 200));
            drawTiao(exp, 2, prop);

            if (this.isMe)
            {
                bool isUp = face.roleInterface.isAllowedUpLv();
                GameObject upLv = gameObjPool.getInstance().get("upLv", typeof(ImgUI));
                upLv.transform.SetParent(hb.transform, false);
                upLv.GetComponent<ImgUI>().loadRes(isUp ? "sj_y_png" : "sj_n_png")//sj_y
                    .setSizePos(new Vector2(126, 69), new Vector2(ScreenUtils.width - 156, -165.5f)).addClk(() =>
                    {
                        if (!isUp) return;
                        face.roleInterface.addLvByBtn(() =>
                        {
                            this.role = face.roleInterface.getRole();
                            this.updateTabPage(0);
                        });
                    });
            }


        }
        private void drawTiao(GameObject attr, int i, JObject prop)
        {
            GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
            item.transform.SetParent(attr.transform, false);
            item.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(320, 20), new Vector2(0, 0));
            string k1 = "xue";
            string k2 = "max_xue";
            Color32 color = new Color32(255, 16, 0, 255);
            Color32 color1 = new Color32(172, 11, 0, 255);
            if (i == 1)
            {
                k1 = "lan";
                k2 = "max_lan";
                color = new Color32(0, 174, 255, 255);
                color1 = new Color32(2, 140, 205, 255);
            }
            else if (i == 2)
            {
                k1 = "exp";
                k2 = "max_exp";
                color = new Color32(255, 183, 0, 255);
                color1 = new Color32(192, 137, 0, 255);
            }

            GameObject jdBg = gameObjPool.getInstance().get("jdBg", typeof(ImgUI));
            jdBg.transform.SetParent(item.transform, false);
            jdBg.GetComponent<ImgUI>().setColor("#555555")
                .setSizePos(new Vector2(320, 12), new Vector2(80, -21));
            float rate = (float)prop[k1] / (float)prop[k2];
            if (rate > 1) rate = 1;
            GameObject jdt = gameObjPool.getInstance().get("jdt", typeof(ImgUI));
            jdt.transform.SetParent(jdBg.transform, false);
            jdt.GetComponent<ImgUI>()
                .setSizePos(new Vector2(314 * rate, 6), new Vector2(3, -3));
            GradientDefinded gd = jdt.AddComponent<GradientDefinded>();
            gd.color1 = color;
            gd.color2 = color1;


            GameObject exp = gameObjPool.getInstance().get("str", typeof(TextUI));
            exp.transform.SetParent(jdBg.transform, false);
            exp.GetComponent<TextUI>().setColor("#ffffff").setAlign("right").setFontSize(24).setText((int)prop[k1] + "/" + (int)prop[k2])
                .setSizePos(new Vector2(320, 30), new Vector2(0, 30));
        }
        public override void init()
        {

        }
    }
}
