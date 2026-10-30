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
    class LiftSkillPage : PageUI
    {
        public LiftSkillPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("生活技能");
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
            "学习","研发"
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



        }
        private void draw1()
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

            JArray skls = (JArray)face.roleInterface.getRole()["attr"]["skill"];
            JArray lifeSkls = new JArray();
            for (int i = 0; i < skls.Count; i++)
            {
                JObject skl = (JObject)skls[i];
                //过滤掉非生活技能
                if (!skl.ContainsKey("key") || !skl["key"].ToString().Substring(0, 8).Equals("10021006")) continue;
                lifeSkls.Add(skl);
            }

            JArray list = new JArray();
            for (int i = 0; i < 18; i++)
            {
                int lv = 0;
                Skill sk = (Skill)face.goodsInterface.getGoodsMsgByKey("10021006" + (i < 10 ? "000" + i : "00" + i));
                for (int j = 0; j < lifeSkls.Count; j++)
                {
                    JObject skl = (JObject)lifeSkls[j];
                    if (skl["key"].ToString().Equals(sk.key))
                    {
                        lv = (int)skl["lv"];
                        break;
                    }
                }
                JObject a = new JObject();
                a.Add("name", "Lv" + lv + " " + sk.name);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {

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
            text.GetComponent<TextUI>().setText("学习生活技能需要达到技能相对应的等级，足够的银两和经验以及帮派总帮贡，前30级无需帮贡，之后需要，技能等级限制与帮派研发的技能等级有关。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray skls = (JArray)face.roleInterface.getRole()["attr"]["skill"];
            JArray lifeSkls = new JArray();
            for (int i = 0; i < skls.Count; i++)
            {
                JObject skl = (JObject)skls[i];
                //过滤掉非生活技能
                if (!skl.ContainsKey("key") || !skl["key"].ToString().Substring(0, 8).Equals("10021006")) continue;
                lifeSkls.Add(skl);
            }

            JArray list = new JArray();
            for (int i = 0; i < 18; i++)
            {
                int lv = 0;
                Skill sk = (Skill)face.goodsInterface.getGoodsMsgByKey("10021006" + (i < 10 ? "000" + i : "00" + i));
                for (int j = 0; j < lifeSkls.Count; j++)
                {
                    JObject skl = (JObject)lifeSkls[j];
                    if (skl["key"].ToString().Equals(sk.key))
                    {
                        lv = (int)skl["lv"];
                        break;
                    }
                }
                JObject a = new JObject();
                a.Add("name", "Lv" + lv + " " + sk.name);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                int lv = 0;
                Skill sk = (Skill)face.goodsInterface.getGoodsMsgByKey("10021006" + (mIndex < 10 ? "000" + mIndex : "00" + mIndex));
                string str = sk.getSkillDes(1);
                for (int j = 0; j < lifeSkls.Count; j++)
                {
                    JObject skl = (JObject)lifeSkls[j];
                    if (skl["key"].ToString().Equals(sk.key))
                    {
                        lv = (int)skl["lv"];
                        break;
                    }
                }
                List<string> m2 = new List<string>();
                if (sk.key.Equals("100210060015") || sk.key.Equals("100210060016") || sk.key.Equals("100210060017"))
                {
                    m2.Add("使用");
                }
                m2.Add("升级");
                m2.Add("查看");
                Menu menu2 = Menu.create(m2, this.transform);
                menu2.addCallback((mIndex2) =>
                {
                    if (m2[mIndex2].Equals("使用"))
                    {
                        PageUI.createAcPage<ZhiFuPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(0);
                    }
                    else if (m2[mIndex2].Equals("升级"))
                    {
                        //1400 1700
                        int exp = 3000 + 5000 * lv;
                        int tale = 1100 + 300 * lv;
                        int bg = 0;
                        if (lv >= 60) bg = lv * 100;
                        MsgSureUI.create(this.transform).show("学习该技能需要消耗" + exp + "经验和" + tale + "银两" + (bg > 0 ? ",需总贡度达到" + bg : "") + "," + str + "，是否继续？", () =>
                        {
                            face.skillInterface.learnSkill(sk.key, (res) =>
                            {
                                clkTab(0);
                            });
                        }, () => { });
                    }
                    else
                    {
                        JObject a = null;
                        for (int i = 0; i < skls.Count; i++)
                        {
                            JObject skl = (JObject)skls[i];
                            //过滤掉非生活技能
                            if (!skl.ContainsKey("key") || !skl["key"].ToString().Equals(sk.key)) continue;
                            a = skl;
                            break;
                        }
                        int lv = 1;
                        if (a != null) lv= (int)a["lv"];
                        Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getTipCanvas());
                        dialog.renderText(sk.getSkillDes(lv), "center");
                        DoGet.getInstance().startReqImg();
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
