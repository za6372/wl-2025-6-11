using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.sklronglian;
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
    class SkillRongLianPage : PageUI
    {
        public SkillRongLianPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("技能熔炼");
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
            "人技","四字宠技","二字宠技"
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
            else if (index == 2)//属性
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
            text.GetComponent<TextUI>().setText("两个二字随机合成一个二字。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //列举可兑换的技能
            string[] list = {
                    "随机兑换一个二字", 
            };
            JArray m2 = new JArray();
            for (int i = 0; i < list.Length; i++)
            {
                JObject b = new JObject();
                b.Add("name", list[i]);
                m2.Add(b);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                //弹出背包的技能选择项
                PetErZiCompose.create(this.transform).addCallback((ens) =>
                {
                    string str = "您选择了 ";
                    for (int i = 0; i < ens.Count; i++)
                    {
                        str += ens[i]["name"] + "x" + ens[i]["nowNum"] + "、";

                    }
                    str = str.Substring(0, str.Length - 1) + "，是否继续？";
                    MsgSureUI.create(this.transform).show(str, () =>
                    {
                        face.activityInterface.petSklTwoToOne(ens, () => { });
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
            text.GetComponent<TextUI>().setText("三个技能合成一个。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //列举可兑换的技能
            string[] list = {
                    "100210010265", "100210010266", "100210010267", "100210010268", "100210010269",
                    "100210010270", "100210010271", "100210010272", "100210010273", "100210010276",
                    "100210010277", "100210010278", "100210010279", "100210010280", "100210010281",
                    "100210010282", "100210010283", "100210010284", "100210010287",
                    "100210010288", "100210010289", "100210010291", "100210010294",
                    "100210010295", "100210010296", "100210010298", "100210010308",
                    "100210010309", "100210010310", "100210010311", "100210010312", "100210010313",
                    "100210010180",
            };
            JArray m2 = new JArray();
            for (int i = 0; i < list.Length; i++)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(list[i]);
                JObject b = new JObject();
                b.Add("name", "兑换 " + gd.name);
                b.Add("key", gd.key);
                m2.Add(b);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                //弹出背包的技能选择项
                PetSklCompose.create(this.transform).addCallback((ens) =>
                {
                    string str = "您选择了 ";
                    for (int i = 0; i < ens.Count; i++)
                    {
                        str += ens[i]["name"] + "x" + ens[i]["nowNum"] + "、";

                    }
                    str = str.Substring(0, str.Length - 1) + "，是否继续？";
                    MsgSureUI.create(this.transform).show(str, () =>
                    {
                        face.activityInterface.petSklThreeToOne(ens, m2[mIndex]["key"].ToString(), () => { });
                    }, () => { });
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
            text.GetComponent<TextUI>().setText("三个二字合成一个四字。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //列举可兑换的技能
            string[] list = {
                "100210030029", "100210030030", "100210030031",
                "100210030032", "100210030033", "100210030034", "100210030035", "100210030036",
                "100210030037", "100210030040", "100210030043", "100210030044", "100210030045", "100210030046",
                "100210030047", "100210030049", "100210030050","100210030070","100210030072"
            };
            JArray m2 = new JArray();
            for (int i = 0; i < list.Length; i++)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(list[i]);
                JObject b = new JObject();
                b.Add("name", "兑换 " + gd.name);
                b.Add("key", gd.key);
                m2.Add(b);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                //弹出背包的技能选择项
                ManSklCompose.create(this.transform).addCallback((ens) =>
                {
                    string str = "您选择了 ";
                    for (int i = 0; i < ens.Count; i++)
                    {
                        str += ens[i]["name"] + "x" + ens[i]["nowNum"] + "、";

                    }
                    str = str.Substring(0, str.Length - 1) + "，是否继续？";
                    MsgSureUI.create(this.transform).show(str, () =>
                    {
                        face.activityInterface.manSklThreeToOne(ens, m2[mIndex]["key"].ToString(), () => { });
                    }, () => { });
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
