using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.login;
using Assets.HotFix.xq2d.src.ui.childPage.sysCheck;
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
    class SysCheckManagerPage : PageUI
    {
        public SysCheckManagerPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("监控");
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
                "统计","vip","活动"
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
                drawBtn("重新统计", k2.Find("hb").transform, new Vector2(10, -300), () =>
                {
                    k2.Find("hb").transform.Find("tb").gameObject.SetActive(false);
                    face.activityInterface.countMoney(() =>
                    {
                        this.clkTab(0);
                        timeManager.getTimeManageOne().putDelayTask(() =>
                        {
                            k2.Find("hb").transform.Find("tb").gameObject.SetActive(true);
                        }, 5000);
                    });
                });
                Vector2 hbSize = k2.Find("hb").GetComponent<RectTransform>().sizeDelta;
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(k2.Find("hb").transform, false);
                text.GetComponent<TextUI>().setText("每次打开这个页面都是旧的数据，点击重新统计，让系统重新统计一次最新的元宝数据，然后再点击元宝/银两看消耗情况。（当前余额-重启时余额=消耗，如果为负值说明余额在减少，正值就是有所增加）")
                    .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                    .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

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
            text.GetComponent<TextUI>().setText("先报名后开启活动")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            string[] m2 = { "大师兄活动开启", "武状元活动开启", "帮战活动开启", "结算帮战", "血腥之地开启" };
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
                    face.activityInterface.openDsx(() => { });
                }
                else if (mIndex == 1)
                {
                    face.activityInterface.openWzy(() => { });
                }
                else if (mIndex == 2)
                {
                    face.activityInterface.openBz(() => { });
                }
                else if (mIndex == 3)
                {
                    face.activityInterface.closeBz(() => { });
                }
                else if (mIndex == 4)
                {
                    List<string> ml = new List<string>() { "开启", "匹配", "结束" };

                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        if (mIndex2 == 0) face.activityInterface.openXxzd(() => { });
                        else if (mIndex2 == 1) face.activityInterface.openXxzdMatch(() => { });
                        else if (mIndex2 == 2) face.activityInterface.closeXxzd(() => { });
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
            text.GetComponent<TextUI>().setText("1积分=100成长值")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            string[] m2 = { "增加玩家积分", "设置大盘", "设置期货", "修改密码", "给自己元宝", "给自己道具" };
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
                    AddVipJfUI.create(this.transform).addCallback((res) =>
                    {
                        face.activityInterface.addVipValue((int)res["jf"], res["playerName"].ToString(), () => { });
                    });
                }
                else if (mIndex == 1)
                {
                    List<string> ml = new List<string>();
                    ml.Add("涨");//0
                    ml.Add("跌");//1
                    ml.Add("平");//2
                    ml.Add("取消");
                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        if (mIndex2 > 2)
                        {
                            face.activityInterface.cancelCont(0, () => { });
                            return;
                        }
                        face.activityInterface.setCont(0, mIndex2, 0, () => { });
                    });
                }
                else if (mIndex == 2)
                {
                    string[] arr = {
                        "锻造宝石","精炼宝石","镶嵌宝石","修复宝石","黑洞陨石",
                        "天晶石","玉魄石","补天玄石"
                        };
                    List<string> ml = new List<string>(arr);
                    ml.Add("取消");
                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        if (mIndex2 > 7)
                        {
                            face.activityInterface.cancelCont(1, () => { });
                            return;
                        }
                        List<string> m2 = new List<string>();
                        m2.Add("流行");
                        m2.Add("风靡");
                        Menu menu2 = Menu.create(m2, PointGet.getIndexPage());
                        menu2.addCallback((mIndex3) =>
                        {
                            face.activityInterface.setCont(1, mIndex2, mIndex3, () => { });
                        });

                    });
                }
                else if (mIndex == 3)
                {
                    GbPasswordUI.create(this.transform);
                }
                else if (mIndex == 4)
                {
                    List<string> ml = new List<string>() { "元宝", "银两", "银票" };

                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex2) =>
                    {
                        UseNumInputUI.create(PointGet.getIndexPage()).renderText("请输入数值").addTextCallback((num) =>
                        {
                            if (mIndex2 == 0) face.activityInterface.addGoldValue(0, int.Parse(num), () => { });
                            else if (mIndex2 == 1) face.activityInterface.addGoldValue(1, int.Parse(num), () => { });
                            else if (mIndex2 == 2) face.activityInterface.addGoldValue(2, int.Parse(num), () => { });
                        });

                    });
                }
                else if (mIndex == 5)
                {
                    UseNumInputUI.create(PointGet.getIndexPage()).renderText("道具key").addTextCallback((key) =>
                    {
                        UseNumInputUI.create(PointGet.getIndexPage()).renderText("数量").addTextCallback((num) =>
                        {
                            face.activityInterface.addGoods(key, int.Parse(num), () => { });
                        });
                    });
                }
            });
        }
        private int pageNum = 1;
        private int totalPage;
        private int tabIndex = 0;
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            List<string> ml = new List<string>() { "元宝", "银两", };
            Tab tab = Tab.create(ml, Vector2.zero, hb.transform);
            tab.addCallback((mIndex) =>
            {
                tabIndex = mIndex;
                pageNum = 1;
                readData();
            });
            tab.clkDefault();
        }
        private void readData()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            face.activityInterface.getGoldOrder(tabIndex, pageNum, (res) =>
            {
                totalPage = (int)res["totalPage"];
                JArray list = (JArray)res["list"];
                JArray m2 = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    JObject b = new JObject();
                    if (tabIndex == 0)
                        b.Add("name", a["name"] + " " + a["now_gold"] + "-" + a["old_gold"] + " 消耗:" + ((long)a["now_gold"] - (long)a["old_gold"]));
                    else
                        b.Add("name", a["name"] + " " + a["now_tale"] + "-" + a["old_tale"] + " 消耗:" + ((long)a["now_tale"] - (long)a["old_tale"]));

                    m2.Add(b);
                }
                gameObjPool.getInstance().freeChildren(content.gameObject);
                GoodsItem gdItem = GoodsItem.create(m2, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {

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
