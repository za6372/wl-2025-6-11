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
    class DuanzaoEquipPage : PageUI
    {
        public DuanzaoEquipPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("装备强化");
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
            "锻造","修复","合锻","绑定","解绑","刻印","打孔","镶嵌","合镶","精炼","注潜",
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
            else if (index == 8)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw8();
            }
            else if (index == 9)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw9();
            }
            else if (index == 10)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw10();
            }


        }
        private string lqZbId = null;
        private void draw10()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("需先镶嵌宝石后才允许注潜，每次最多注入99颗潜力符石，镶嵌石的属性需要通过练潜将注入的潜力转化为镶嵌石的实际属性值。（1石=100潜力，潜力黑洞、百炼玄塔可以练潜，下线时练潜将自动取消）")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            Action ac = () =>
            {
                JArray arr = face.equipInterface.getZhuQianEquip();
                JArray list = new JArray();
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject obj = (JObject)arr[i];
                    Equip gds = (Equip)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());

                    string name = gds.getPerfectName(obj);
                    if (lqZbId != null && lqZbId.Equals(obj["Id"].ToString())) name += "（练潜中）";
                    JObject a = new JObject();
                    string color = GameAttrConst.getGoodsQualityColor(gds.quality);
                    a.Add("name", "<color=" + color + ">" + name + "</color>");
                    list.Add(a);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    JObject obj = (JObject)arr[mIndex];
                    int isLq = 1;
                    if (lqZbId != null && lqZbId.Equals(obj["Id"].ToString())) isLq = 0;
                    List<string> ml = new List<string>() { "注入潜力", };
                    if (isLq == 1) ml.Add("设置练潜");
                    else ml.Add("取消练潜");
                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mi) =>
                    {
                        if (mi == 0)
                        {
                            //弹出输入数量
                            UseNumInputUI.create(this.transform).addCallback((num) =>
                            {
                                if (num > 99 || num <= 0)
                                {
                                    msgCode.showMsg(939);
                                    return;
                                }
                                MsgSureUI.create(this.transform).show("即将注入潜力符石 x" + num + "，是否继续？", () =>
                                {
                                    face.equipInterface.joinPotential(num, obj["Id"].ToString(), () =>
                                    {
                                        clkTab(10);
                                    });
                                }, () => { });
                            });
                        }
                        else if (mi == 1)
                        {
                            //判断是否有潜力
                            if ((int)obj["potential"]["num"] <= 0)
                            {
                                msgCode.showMsg(663);
                                return;
                            }
                            //设置练潜装
                            face.equipInterface.setPotentialEquip(obj["Id"].ToString(), () =>
                            {
                                this.lqZbId = obj["Id"].ToString();
                                clkTab(10);
                            });
                        }
                    });
                });
            };
            if (this.lqZbId == null)
            {
                face.equipInterface.getPotentialEquip((lqZbId) =>
                {
                    this.lqZbId = lqZbId;
                    ac();
                });
            }
            else
            {
                ac();
            }


        }
        private void draw9()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("将副装备的属性附加到主装备上，不同属性的会被副装备的属性所替换，同时副装备将被吃掉。使用精炼宝石可以选择任意一个属性进行替换或提升，不使用则随机。精炼时需要装备同等级、品质。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取未刻印的装备
            JArray arr = face.equipInterface.getGoodEquip();

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex0) =>
            {
                JObject obj = (JObject)arr[mIndex0];

                //Equip gds = (Equip)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());

                JArray props = null;
                if (obj["randomAttr"] != null)
                {
                    props = (JArray)obj["randomAttr"];

                }
                if (props == null)
                {
                    msgCode.showMsg(937);
                    return;
                }
                //弹出菜单 主装备属性
                List<string> ml = new List<string>();
                for (int i = 0; i < props.Count; i++)
                {
                    JObject p = (JObject)props[i];
                    string k = p["k"].ToString();
                    string v = p["v"].ToString();
                    string str = GameAttrConst.propKeyToName(k) + ":" + v;
                    ml.Add(str);
                }
                /* List<string> keys1 = new List<string>();
                 IEnumerable<JProperty> ps = props.Properties();
                 foreach (JProperty p in ps)
                 {
                     string k = p.Name;
                     string v = p.Value.ToString();
                     string str = GameAttrConst.propKeyToName(k) + ":" + v;
                     ml.Add(str);
                     keys1.Add(k);
                 }*/

                int zhuIndex;
                int fuIndex;
                BaoshiChooseMenuUI menu = BaoshiChooseMenuUI.create(ml, PointGet.getIndexPage()).setDes("请选择要精炼的属性");
                menu.addCallback((mIndex) =>
                {
                    JObject zhu = (JObject)props[mIndex];
                    zhuIndex = mIndex;
                    string zhuK = zhu["k"].ToString();
                    //获取同等级、同部位的装备
                    JArray fuList = face.equipInterface.getJinglianFuEquip(obj);
                    List<string> m2 = new List<string>();
                    for (int i = 0; i < fuList.Count; i++)
                    {
                        JObject fu = (JObject)fuList[i];
                        Equip fuMsg = (Equip)face.goodsInterface.getGoodsMsgByKey(fu["key"].ToString());
                        m2.Add(fuMsg.getPerfectName(fu));
                    }
                    //弹出副装备选择
                    BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择副装备");
                    menu2.addCallback((mIndex2) =>
                    {
                        //弹出选择副装备属性
                        JObject obj2 = (JObject)fuList[mIndex2];
                        JArray fuProps = null;
                        if (obj2["randomAttr"] != null)
                        {
                            fuProps = (JArray)obj2["randomAttr"];
                        }
                        if (fuProps == null)
                        {
                            msgCode.showMsg(937);
                            return;
                        }
                        /*List<string> keys2 = new List<string>();
                        List<string> m3 = new List<string>();
                        IEnumerable<JProperty> fups = fuProps.Properties();
                        foreach (JProperty p in fups)
                        {
                            string k = p.Name;
                            string v = p.Value.ToString();
                            string str = GameAttrConst.propKeyToName(k) + ":" + v;
                            m3.Add(str);
                            keys2.Add(k);
                        }*/
                        List<string> m3 = new List<string>();
                        for (int i = 0; i < fuProps.Count; i++)
                        {
                            JObject p = (JObject)fuProps[i];
                            string k = p["k"].ToString();
                            string v = p["v"].ToString();
                            string str = GameAttrConst.propKeyToName(k) + ":" + v;
                            m3.Add(str);
                        }
                        BaoshiChooseMenuUI menu3 = BaoshiChooseMenuUI.create(m3, PointGet.getIndexPage()).setDes("请选择副装备属性");
                        menu3.addCallback((mIndex3) =>
                        {
                            JObject fu = (JObject)fuProps[mIndex3];
                            fuIndex = mIndex3;
                            string fuK = fu["k"].ToString();
                            Action<int> ac = (isUser) =>
                            {
                                if (isUser == 1 && !face.goodsInterface.isEnoughInPackAndTip("10000109", 1))
                                {
                                    return;
                                }
                                string tip = "副属性[" + GameAttrConst.propKeyToName(fuK) + "]附加到" + "主属性[" + GameAttrConst.propKeyToName(zhuK) + "]上，是否继续?";
                                MsgSureUI.create(this.transform).show(tip, () =>
                                {
                                    face.equipInterface.refineEquip(obj["Id"].ToString(), obj2["Id"].ToString(), zhuIndex, fuIndex, isUser, () =>
                                    {

                                        clkTab(9);
                                    });
                                }, () => { });
                            };
                            MsgSureUI.create(this.transform).show("是否使用精炼宝石？(点取消则不使用)", () =>
                            {
                                ac(1);
                            }, () =>
                            {
                                ac(0);
                            });
                        });
                    });

                });
            });
        }
        private void draw8()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("每次合成需要消耗5颗同品质的镶嵌石，成功则获得一颗更高品质的镶嵌石，失败将损失3颗。（1级合2级90%成功率，2级合3级80%成功率，3级合4级70%成功率，暂无5级，脸黑就没办法）")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));


            //获取背包中的镶嵌石
            JArray arr = face.baoshiInterface.getPlayerBaoshiList();
            JArray list = new JArray();
            for (int i = 0; i < arr.Count; i++)
            {
                GoodsDes bs = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(arr[i]["key"].ToString());
                string str = bs.name;
                JObject a = new JObject();
                a.Add("name", str + " x" + arr[i]["num"]);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                string bsKey = arr[mIndex]["key"].ToString();
                Baoshi bs = (Baoshi)face.goodsInterface.getGoodsMsgByKey(bsKey);
                MsgSureUI.create(this.transform).show("合成需要消耗 " + bs.name + " x5，是否继续？", () =>
                {
                    face.baoshiInterface.composeInlayBaoshi(bsKey, () =>
                    {
                        clkTab(8);
                    });
                }, () => { });

            });
        }
        private void draw7()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("不同部位限制宝石的镶嵌类型，具体请看道具描述。需要有孔槽才能镶嵌宝石，镶嵌后宝石属性需要通过练潜才能转化成真实属性。注意移除宝石不会返还。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取未刻印的装备
            JArray arr = face.equipInterface.getInlayEquip();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                if (!face.equipInterface.isFaBao(obj["key"].ToString()))
                {
                    if (obj["inlay"] == null ||
                        (int)obj["inlay"]["num"] == 0)
                    {
                        msgCode.showMsg(660);
                        return;
                    }
                }
                else
                {
                    if (!obj.ContainsKey("inlay"))
                    {
                        JObject zhuling = (JObject)obj["zhuling"];
                        int quality = (int)zhuling["quality"];
                        int num = 0;
                        if (quality == 2) num = 1;
                        else if (quality == 3) num = 2;
                        JObject a = new JObject();
                        a["num"] = num;
                        a["list"] = new JArray();
                        obj["inlay"] = a;
                    }
                }

                JArray list = (JArray)obj["inlay"]["list"];
                int sum = (int)obj["inlay"]["num"];

                Equip gds = (Equip)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                //弹出菜单 镶嵌宝石、移除宝石
                List<string> ml = new List<string>() { "查看", "镶嵌宝石", "移除宝石" };
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((mi) =>
                {
                    if (mi == 0)
                    {
                        GoodsDesUI.create(obj, this.transform).renderText();
                    }
                    else if (mi == 1)
                    {
                        if (list.Count >= sum)
                        {
                            msgCode.showMsg(660);
                            return;
                        }
                        List<string> m2 = new List<string>();
                        //获取背包中的镶嵌石
                        JArray bsList = face.baoshiInterface.getBaoshiListByPart(gds.part);
                        if (bsList.Count == 0)
                        {
                            msgCode.showMsg(936);
                            return;
                        }
                        for (int i = 0; i < bsList.Count; i++)
                        {
                            JObject a = (JObject)bsList[i];
                            Baoshi bs = (Baoshi)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            m2.Add(bs.name);
                        }
                        //弹出背包中的镶嵌石
                        Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                        menu2.addCallback((mIndex2) =>
                        {
                            JObject chooseBs = (JObject)bsList[mIndex2];
                            Baoshi bs = (Baoshi)face.goodsInterface.getGoodsMsgByKey(chooseBs["key"].ToString());
                            MsgSureUI.create(this.transform).show("需要消耗" + bs.name + " x1，是否继续？", () =>
                             {
                                 face.equipInterface.inlayBaoshi(chooseBs["Id"].ToString(), obj["Id"].ToString(), () =>
                                 {
                                     clkTab(7);
                                 });
                             }, () => { });
                        });
                    }
                    else
                    {
                        if (list.Count == 0)
                        {
                            msgCode.showMsg(788);
                            return;
                        }
                        //选择摘除的位置
                        List<string> m2 = new List<string>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            JObject a = (JObject)list[i];
                            Baoshi bs = (Baoshi)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            m2.Add(bs.name + " (" + a["num"] + "/" + bs.prop + ")");
                        }
                        //弹出宝石内属性
                        Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                        menu2.addCallback((mIndex2) =>
                        {
                            MsgSureUI.create(this.transform).show("摘除后不会返还原有宝石，是否继续？", () =>
                            {
                                face.equipInterface.clearInlay(obj["Id"].ToString(), mIndex2, () =>
                                {
                                    clkTab(7);
                                });
                            }, () => { });
                        });
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
            text.GetComponent<TextUI>().setText("小于5个孔的装备才能开孔，开孔后宝石镶嵌孔+1")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取未刻印的装备
            JArray arr = face.equipInterface.getGoodEquip();

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];

                MsgSureUI.create(this.transform).show("需要消耗打孔器 x1，是否继续？", () =>
                {
                    face.equipInterface.kaikong(obj["Id"].ToString(), () =>
                    {
                        clkTab(6);
                    });
                }, () => { });

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
            text.GetComponent<TextUI>().setText("刻印开启后增加30%基础属性，宝石镶嵌孔+1，同时允许放置刻印")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取未刻印的装备
            JArray arr = face.equipInterface.getKeyinEquip(false);

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];

                MsgSureUI.create(this.transform).show("需要消耗刻印宝石 x1，是否继续？", () =>
                {
                    face.equipInterface.kyEquip(obj["Id"].ToString(), () =>
                    {
                        clkTab(5);
                    });
                }, () => { });

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
            text.GetComponent<TextUI>().setText("解绑后装备可邮寄、出售")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取绑定的装备
            JArray arr = face.equipInterface.getBindEquip(true);

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];

                MsgSureUI.create(this.transform).show("需要消耗5000银两，是否继续？", () =>
                {
                    face.equipInterface.unBindEquip(obj["Id"].ToString(), () =>
                    {
                        clkTab(4);
                    });
                }, () => { });

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
            text.GetComponent<TextUI>().setText("绑定后可增加10%的基础属性（绑定后装备会无法邮寄、出售）")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //获取未绑定的装备
            JArray arr = face.equipInterface.getBindEquip(false);

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];

                MsgSureUI.create(this.transform).show("需要消耗绑定宝石 x1，是否继续？", () =>
                {
                    face.equipInterface.bindEquip(obj["Id"].ToString(), () =>
                    {
                        clkTab(3);
                    });
                }, () => { });

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
            text.GetComponent<TextUI>().setText("每次合成需要消耗5颗同品质的宝石，成功则获得一颗更高品质的宝石，失败将损失3颗。20个圣锻碎片必然合成一颗圣锻宝石。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));


            //获取背包中的锻造石
            string[] arr = { "10000105", "10000106", "10000107", "10000108" };
            JArray list = new JArray();
            for (int i = 0; i < arr.Length; i++)
            {
                GoodsDes bs = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(arr[i]);
                string str = "合成" + bs.name;
                JObject a = new JObject();
                a.Add("name", str);
                list.Add(a);
            }

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                string bsKey = arr[mIndex];
                GoodsDes needGd;
                int tale = 0;
                int needNum = 0;
                if (mIndex == 3)
                {
                    needGd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey("10000148");
                    tale = 2000;
                    needNum = 20;
                }
                else
                {
                    string needKey = int.Parse(bsKey) - 1 + "";
                    needGd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(needKey);
                    tale = mIndex * 1000 + 1000;
                    needNum = 5;
                }
                MsgSureUI.create(this.transform).show("合成需要消耗" + tale + "银两、" + needGd.name + " x" + needNum + "，是否继续？", () =>
                {
                    face.equipInterface.composeForgingBaoshi(bsKey, () =>
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

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("将损坏的装备进行修复")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getRepairEquip();

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                int num = face.equipInterface.getFixNum((int)obj["forging"]["lv"]);
                //获取还差多少数量
                string gKey = "10000111";
                int dis = face.goodsInterface.getChaZhiInPack("10000111", num);
                if (dis != 0)
                {
                    GoodsDes gds = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(gKey);
                    int sumPrice = gds.getPrice(0) * dis;
                    //提示是否购买
                    MsgSureUI.create(this.transform).show("数量不足，是否花费" + sumPrice + "元宝购买" + gds.name + " x" + dis + "，是否继续？", () =>
                    {
                        face.goodsInterface.buyGoods(gKey, dis, 0, null, (b) => { });
                    }, () => { });
                    return;
                }
                MsgSureUI.create(this.transform).show("修复该装备需要消耗修复宝石x" + num + "，是否继续？", () =>
                {
                    face.equipInterface.fixEquip(obj, () =>
                    {
                        this.clkTab(1);
                    });
                }, () => { });

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
            text.GetComponent<TextUI>().setText("锻造可提升装备基础属性，锻造成功强化+1（最高+15，往上需升橙品-咸阳工业区-紫凝-紫装飞升），失败则装备损坏，需要一定的修复宝石进行修复，不同强化等级有不同的成功率，不同的锻造宝石对该成功率有一定的增幅，圣锻宝石是必然成功。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getForgingEquip();

            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                if (obj["forging"] != null && (int)obj["forging"]["lv"] >= 15)
                {
                    msgCode.showMsg(656);
                    return;
                }
                //弹出使用何种宝石
                string[] m2 = { "10000104", "10000105", "10000106", "10000107", "10000108" };
                List<string> ml = new List<string>();
                for (int i = 0; i < m2.Length; i++)
                {
                    GoodsDes bs = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(m2[i]);
                    string str = "使用" + bs.name;
                    float k0 = face.equipInterface.getBaoshiSucRate(m2[i]);
                    str += "(+" + (int)(100 * k0) + "%)";
                    ml.Add(str);
                }
                BaoshiChooseMenuUI menu = BaoshiChooseMenuUI.create(ml, PointGet.getIndexPage()).setDes("锻造可以增加装备的基础属性、孔槽、潜力");
                menu.addCallback((mIndex) =>
                {
                    //判断宝石是否存在
                    string baoshiKey = m2[mIndex];
                    if (!face.goodsInterface.isEnoughInPackAndTip(baoshiKey, 1))
                    {
                        msgCode.showMsg(637);
                        return;
                    }
                    //float k0 = face.equipInterface.getBaoshiSucRate(baoshiKey);
                    JObject msg = face.equipInterface.getForgingMsg(baoshiKey,(int)obj["forging"]["lv"] + 1);
                    GoodsDes bs = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(baoshiKey);
                    MsgSureUI.create(this.transform).show("锻造该装备需要消耗" + bs.name + " x1，银两" +
                        msg["tale"] + "，本次成功率为" + Convert.ToInt32((double)msg["p"] * 100) + "%，是否继续？", () =>
                    {
                        face.equipInterface.forgingEquip(baoshiKey, obj["Id"].ToString(), (res) =>
                        {
                            //刷新
                            clkTab(0);
                        });
                    }, () => { });
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
