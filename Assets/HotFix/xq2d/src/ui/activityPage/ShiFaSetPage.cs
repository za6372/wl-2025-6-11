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
    /**施法设置*/
    class ShiFaSetPage : PageUI
    {
        private JObject shifa;
        public ShiFaSetPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("施法");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            face.fightInterface.viewShiFaWay((shifa) =>
            {
                this.shifa = shifa;
                drawK1(content);
                this.clkTab(0);
                DoGet.getInstance().startReqImg();
            });

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
            "角色","宠物",
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

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("宠物施法将列举所有主动技能，按此顺序释放，若当前出战宠物存在该技能则释放，不存在则一直往后推延，不必为每只宠物单独设置")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            gameObjPool.getInstance().freeChildren(content.gameObject);


            JArray sfList = (JArray)shifa["pet_skls"];
            JArray m2 = new JArray();
            for (int i = 0; i < sfList.Count; i++)
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(sfList[i].ToString());
                JObject b = new JObject();
                b.Add("name", skl.name);
                m2.Add(b);
            }

            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                List<string> ml = new List<string>();
                if (mIndex == 0)//顶部
                {
                    if (sfList.Count > 1)//至少两个
                    {
                        ml.Add("下移");
                        ml.Add("末端");
                    }
                }
                else if (mIndex == sfList.Count - 1)//末尾
                {
                    ml.Add("置顶");
                    ml.Add("上移");
                }
                else//中间
                {
                    ml.Add("置顶");
                    ml.Add("上移");
                    ml.Add("下移");
                    ml.Add("末端");
                }
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((mIndex2) =>
                {
                    int order = 0;
                    if (ml[mIndex2].Equals("置顶")) order = 0;
                    else if (ml[mIndex2].Equals("上移")) order = 1;
                    else if (ml[mIndex2].Equals("下移")) order = 2;
                    else order = 3;
                    //上传修改的命令
                    face.fightInterface.uploadShiFaWay(1, 1, order, mIndex,() =>
                    {
                        string k = sfList[mIndex].ToString();
                        sfList.RemoveAt(mIndex);
                        if (ml[mIndex2].Equals("置顶"))
                        {
                            sfList.AddFirst(k);
                        }
                        else if (ml[mIndex2].Equals("上移"))
                        {
                            sfList.Insert(mIndex - 1, k);
                        }
                        else if (ml[mIndex2].Equals("下移"))
                        {
                            sfList.Insert(mIndex + 1, k);
                        }
                        else
                        {
                            sfList.Add(k);
                        }
                        this.clkTab(1);
                    });
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
            text.GetComponent<TextUI>().setText("战斗时按固定的顺序进行施法，比如第一个出现蓝不足或者冷却中等情况会使用第二个。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            JArray sfList = (JArray)shifa["role_skls"];
            JArray m2 = new JArray();
            for (int i = 0; i < sfList.Count; i++)
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(sfList[i].ToString());
                if (skl == null)
                {
                    Debug.Log(sfList[i].ToString());
                }
                JObject b = new JObject();
                b.Add("name", skl.name);
                m2.Add(b);
            }

            GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                List<string> ml = new List<string>();
                if (mIndex == 0)//顶部
                {
                    if (sfList.Count > 1)//至少两个
                    {
                        ml.Add("下移");
                        ml.Add("末端");
                    }
                }
                else if (mIndex == sfList.Count - 1)//末尾
                {
                    ml.Add("置顶");
                    ml.Add("上移");
                }
                else//中间
                {
                    ml.Add("置顶");
                    ml.Add("上移");
                    ml.Add("下移");
                    ml.Add("末端");
                }
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((mIndex2) =>
                {
                    int order = 0;
                    if (ml[mIndex2].Equals("置顶")) order = 0;
                    else if (ml[mIndex2].Equals("上移")) order = 1;
                    else if (ml[mIndex2].Equals("下移")) order = 2;
                    else order = 3;
                    //上传修改的命令
                    face.fightInterface.uploadShiFaWay(1, 0, order, mIndex,() =>
                    {
                        string k = sfList[mIndex].ToString();
                        sfList.RemoveAt(mIndex);
                        if (ml[mIndex2].Equals("置顶"))
                        {
                            sfList.AddFirst(k);
                        }
                        else if (ml[mIndex2].Equals("上移"))
                        {
                            sfList.Insert(mIndex - 1, k);
                        }
                        else if (ml[mIndex2].Equals("下移"))
                        {
                            sfList.Insert(mIndex + 1, k);
                        }
                        else
                        {
                            sfList.Add(k);
                        }
                        this.clkTab(0);
                    });

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
