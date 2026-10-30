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
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    /**紫装飞升*/
    class ZiZhuangUpPage : PageUI
    {
        public ZiZhuangUpPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("紫装飞升");
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
            "加抗","升橙","重铸","注魔","锻造","血契",
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
        }
        private void draw5()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("血契后基础属性提升60%，同时镶嵌孔+1。血契需要消耗血契之石，并且需要装备锻20和装备开启了刻印。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getChengEquip();
            
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];

                MsgSureUI.create(this.transform).show("需要消耗血契之石 x1，是否继续？", () =>
                {
                    face.equipInterface.xueqiChengEquip(obj["Id"].ToString(), () =>
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
            text.GetComponent<TextUI>().setText("使用金锻皇可以将橙装锻造等级提升，失败后装备将损坏。使用轻锻宝石亦可提升橙装等级，失败后不会损坏装备，但锻造等级降低1星。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getChengEquip();
           
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                if (obj["forging"] != null && (int)obj["forging"]["lv"] >= 20)
                {
                    msgCode.showMsg(656);
                    return;
                }
                //弹出使用何种宝石
                string[] m2 = { "10000159", "10000160", };
                List<string> ml = new List<string>();
                for (int i = 0; i < m2.Length; i++)
                {
                    GoodsDes bs = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(m2[i]);
                    string str = "使用" + bs.name;
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
                            face.equipInterface.forgingChengEquip(baoshiKey, obj["Id"].ToString(), (res) =>
                            {
                                if (res)
                                {

                                }
                                else
                                {
                                    if (baoshiKey.Equals("10000159"))
                                    {
                                        //损坏则提示是否跳转修复页面
                                        MsgSureUI.create(this.transform).show("装备损坏，是否前往修复？", () =>
                                        {
                                            this.freeThisPage();
                                            PageUI.createAcPage<DuanzaoEquipPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(1);
                                        }, () => { });
                                    }
                                    else
                                    {

                                    }
                                }
                                //刷新
                                clkTab(4);
                            });
                        }, () => { });
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
            text.GetComponent<TextUI>().setText("魔力等级越高，橙装特殊属性发挥的效果越好，不同模板的魔力上限不同。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getChengEquip();
            
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                //选择注魔道具
                List<string> fuList = new List<string>() { "10000156", "10000157", "10000158" };
                List<string> m2 = new List<string>();
                for (int i = 0; i < fuList.Count; i++)
                {
                    GoodsDes fuMsg = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(fuList[i]);
                    m2.Add(fuMsg.name);
                }
                BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择注魔道具");
                menu2.addCallback((mIndex2) =>
                {
                    string mbKey = fuList[mIndex2];
                    GoodsDes fuMsg = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(mbKey);
                    MsgSureUI.create(this.transform).show("即将消耗" + fuMsg.name + " x1 注入" + (mIndex2 + 1) * 10 + "魔力，是否继续？", () =>
                    {
                        face.equipInterface.czZhuMo(obj["Id"].ToString(), mbKey, () =>
                        {
                            clkTab(3);
                        });
                    }, () => { });
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
            text.GetComponent<TextUI>().setText("使用不同的模板重铸橙装可赋予不同特殊属性。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getChengEquip();
            
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                //选择魔模板
                JArray fuList = face.equipInterface.getMoBan();
                List<string> m2 = new List<string>();
                for (int i = 0; i < fuList.Count; i++)
                {
                    JObject fu = (JObject)fuList[i];
                    MoBan fuMsg = (MoBan)face.goodsInterface.getGoodsMsgByKey(fu["key"].ToString());
                    m2.Add(fuMsg.name);
                }
                BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("请选择模板");
                menu2.addCallback((mIndex2) =>
                {
                    string mbKey = fuList[mIndex2]["key"].ToString();
                    MoBan fuMsg = (MoBan)face.goodsInterface.getGoodsMsgByKey(mbKey);
                    MsgSureUI.create(this.transform).show("即将消耗" + fuMsg.name + " x1，重铸该橙装，是否继续？", () =>
                       {
                           face.equipInterface.zbChongzhu(obj["Id"].ToString(), mbKey, () =>
                           {
                               //刷新
                               clkTab(2);
                           });
                       }, () => { });
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
            text.GetComponent<TextUI>().setText("90级以上且锻10以上的紫装，消耗一定的千年玄铁即可升为橙装，升级橙装后锻造等级+1，孔数+1，装备将自动绑定。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getZiEquip();
            
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                string part = ep.part;
                int xhNum = 0;
                if (part.Equals("wq")) xhNum = 9;
                else if (part.Equals("jb")) xhNum = 6;
                else if (part.Equals("sz")) xhNum = 2;
                else if (part.Equals("wb")) xhNum = 4;
                else if (part.Equals("tb")) xhNum = 4;
                else if (part.Equals("xb")) xhNum = 6;
                else if (part.Equals("yb")) xhNum = 3;
                else if (part.Equals("tuib")) xhNum = 3;
                else if (part.Equals("jiaob")) xhNum = 2;
                int tale = 50000 * xhNum;
                MsgSureUI.create(this.transform).show("消耗千年玄铁 x"+xhNum+"和"+tale+"银两将该装备升为橙装，是否继续？", () =>
                {
                    face.equipInterface.upGold(obj["Id"].ToString(), () =>
                    {
                        //刷新
                        clkTab(1);
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
            text.GetComponent<TextUI>().setText("90级以上且锻10以上的紫装，消耗对应的抗性玉石可以强化装备的抗性，抗性等级越高，需要的玉石就越多。戒指和鞋对应暴击抗性，护腕和腿对应混乱抗性，项链和胸甲对应流血抗性，头部和腰带对应睡眠抗性。（加抗后装备变为绑定）")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray arr = face.equipInterface.getJiaKangEquip();
           
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 9);
            gdItem.addCallback((mIndex) =>
            {
                JObject obj = (JObject)arr[mIndex];
                Equip gds = (Equip)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                string str = null;
                string kxKey = null;
                if (gds.part.Equals("sz") || gds.part.Equals("jiaob"))
                {
                    str = "暴击"; kxKey = "bjlv";
                }
                else if (gds.part.Equals("wb") || gds.part.Equals("tuib"))
                {
                    str = "混乱"; kxKey = "hllv";
                }
                else if (gds.part.Equals("jb") || gds.part.Equals("xb"))
                {
                    str = "流血"; kxKey = "lxlv";
                }
                else if (gds.part.Equals("tb") || gds.part.Equals("yb"))
                {
                    str = "睡眠"; kxKey = "hslv";
                }
                //10个等级 总抗性1520
                //0->6 2000 +35    8->84 168000 +231  9->108 216000 +253
                //v1 35 v2 112 v3 221 v4 332 v5 475
                //50 75 100 125 150 175 200 225 250 275
                int lv = obj.ContainsKey("kxlv") ? (int)obj["kxlv"][kxKey] + 1 : 1;
                int kx = 50 * lv + (int)Math.Pow(2d, lv);
                int num = 6 * lv + (int)Math.Pow(1.5d, lv);
                int tale = 2000 + (lv - 1) * 20000 + (int)Math.Pow(3d, lv);
                MsgSureUI.create(this.transform).show("消耗" + str + "抵抗玉石 x" + num + "和" + tale + "银两对该装备加护，" + str + "抗性提升" + kx + "点，是否继续？", () =>
                {
                    face.equipInterface.upKangxing(obj["Id"].ToString(), () =>
                    {
                        //刷新
                        clkTab(0);
                    });
                }, () => { });
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
