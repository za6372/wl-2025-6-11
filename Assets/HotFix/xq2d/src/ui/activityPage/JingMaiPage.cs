using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
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
    class JingMaiPage : PageUI
    {
        public JingMaiPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("经脉");
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
            "馈赠","合成","经脉",
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
            text.GetComponent<TextUI>().setText("可分配真元点："+ face.roleInterface.getRole()["attr"]["msg"]["jmPoint"]+"/1160\n"+
                "将真元点分配到各个经脉之中，经脉等级越高需要的真元点越多，提升的属性也越高。洗髓会将所有分配的真元点重置。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            List<string> m1 = new List<string>() {"提升青木长生（耐）",  "提升赤火蛮力（力）", "提升精金疾风（敏）", "提升玄水明智（智）",
                "提升戊土精元（精）","易经洗髓", };
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
                string str = null;
                if (mIndex == 5)
                {
                    str = "那么靓仔，你是否忍心重置它？";
                    MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                    {
                        face.activityInterface.resetJm(() =>
                        {
                            this.clkTab(2);
                        });
                    }, () => { });
                    return;
                }
                string[] arr = { "100210020000", "100210020001", "100210020002", "100210020003", "100210020004", };
                string sklKey = arr[mIndex];
                JObject role = face.roleInterface.getRole();
                JArray list = (JArray)role["attr"]["skill"];
                int lv = 0;
                for (int p = 0; p < list.Count; p++)
                {
                    JObject obj = (JObject)list[p];
                    if (obj.ContainsKey("key") && obj["key"].ToString().Equals(sklKey))
                    {
                        lv = (int)obj["lv"];
                        break;
                    }
                }
                if (lv >= 15)
                {
                    msgCode.showMsg(656);
                    return;
                }
                int[] xhn = { 20, 20, 40, 40, 40, 60, 60, 60, 80, 80, 80, 100, 100, 100, 100 };
                 str = "提升需要消耗" + xhn[lv] + "真元点，提升后增加" + ((lv+1) * 4 + 36) + "点，是否继续？";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                {
                    face.skillInterface.learnSkill(sklKey, (res) =>
                    {
                        this.clkTab(2);
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
            text.GetComponent<TextUI>().setText("每5个修行药匣可兑换一个[初]天元丹。每次合成更高级的丹药需要3颗同类丹药，合成成功则获得1颗更高品质的丹药，失败则损失2颗。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            //初20（40点后无法使用） 中40（200） 高60（440）超80（880） 圣100
            List<string> m1 = new List<string>() { "合成丹药[中]（60%）", "合成丹药[高]（70%）", "合成丹药[超]（80%）", "合成丹药[圣]（90%）", };
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
                face.activityInterface.composeJmDan(mIndex + 1, () => { });
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
            text.GetComponent<TextUI>().setText("在线满30分钟可领取修行药匣，每5个药匣可兑换一颗[初]天元丹。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            List<string> m1 = new List<string>() { "领取修行药匣", "兑换[初]天元丹", };
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
                if (mIndex == 0)
                {
                    face.activityInterface.gainLLD(() => { });
                }
                else if (mIndex == 1)
                {
                    face.activityInterface.composeJmDan(0, () => { });
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
