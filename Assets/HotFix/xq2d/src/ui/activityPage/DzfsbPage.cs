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
    class DzfsbPage : PageUI
    {
        public DzfsbPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("斗战封神榜");
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
            "前榜","战报",
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
        private int pageNum = 1;
        private int totalPage;
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("只能挑战跟自身排行不超过10的玩家（超过时将随机匹配），可以消耗直捣黄龙来挑战指定的玩家，每日22点进行排名结算，每日限5次挑战。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));


            gameObjPool.getInstance().freeChildren(content.gameObject);

            pageNum = 1;
            readData();
        }
        private void readData()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            face.activityInterface.getDzfsbOrder(pageNum, (res) =>
            {
                totalPage = (int)res["totalPage"];
                JArray list = (JArray)res["list"];
                JArray m2 = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    JObject b = new JObject();
                    b.Add("name", a["order"] + ". Lv" + a["lever"] + "  【" + GameAttrConst.getJobToSimpleName(a["model"].ToString()) + "】" + a["name"]);
                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    //弹出直捣黄龙、挑战 菜单
                    List<string> ml = new List<string>() { "挑战（随机）", "直捣黄龙", };
                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        if (list[mIndex]["name"].ToString().Equals(face.roleInterface.getRole()["name"].ToString()))
                        {
                            return;
                        }
                        if (mIndex2 == 0)
                        {
                            face.fightInterface.createFightByDzfsb(0, (int)list[mIndex]["order"], () => { });
                        }
                        else
                        {
                            UseNumInputUI.create(this.transform).renderText("请输入挑战的排名").addCallback((num) =>
                            {
                                face.fightInterface.createFightByDzfsb(1,num, () => { });
                            });
                        }
                    });

                }).addScrollTopCall(() =>
                {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    readData();
                }).addScrollBottomCall(() =>
                {
                    pageNum++;
                    if (pageNum > totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    readData();
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
            text.GetComponent<TextUI>().setText("十场与相关的战绩。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            gameObjPool.getInstance().freeChildren(content.gameObject);
            face.activityInterface.getFgLog((res) =>
            {
                JArray m2 = new JArray();
                for (int i = 0; i < res.Count; i++)
                {
                    JObject a = (JObject)res[i];
                    JObject b = new JObject();
                    b.Add("name", "玩家【"+a["n1"] + "】挑战玩家【" + a["n2"] + "】  " +  (a["win"].ToString().Equals("1")?"获胜":"惜败"));
                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {

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
