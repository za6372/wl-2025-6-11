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
    class ShiTuPage : PageUI
    {
        public ShiTuPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("师徒");
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
            "登记","排行"
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
            text.GetComponent<TextUI>().setText("每日20点-22点开启，每5分钟一轮。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            gameObjPool.getInstance().freeChildren(content.gameObject);

        }

        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("拜师需15-50级，收徒需70级，50级出师后会根据情义值获得一份礼包，情义值可由师徒组队完成任务、刷野怪获得。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            gameObjPool.getInstance().freeChildren(content.gameObject);

            List<string> list = new List<string>() { "登记收徒", "登记拜师", "解除师徒关系", "取消登记" };

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
                handle(mIndex);
            });
        }
        private void handle(int index)
        {
            if (index == 0) {
                MsgSureUI.create(this.transform).show("即将登记收徒，是否继续？", () =>
                {
                    face.activityInterface.recruit(() => { });
                }, () => { });
            }
            else if (index ==1)
            {
                MsgSureUI.create(this.transform).show("即将登记拜师，是否继续？", () =>
                {
                    face.activityInterface.signBs(() => { });
                }, () => { });
            }
            else if (index == 2)
            {
                string playerName = null;
                JObject role = face.roleInterface.getRole();
                if (!role["attr"]["msg"]["teacher"].ToString().Equals(""))
                {
                    MsgSureUI.create(this.transform).show("解除师徒关系需要消耗银两50000，是否继续？", () =>
                    {
                        face.activityInterface.removeMatter(playerName, () => { });
                    }, () => { });
                }
                else if (!role["attr"]["msg"]["stu1"].ToString().Equals("") ||
                !role["attr"]["msg"]["stu2"].ToString().Equals("") ||
                !role["attr"]["msg"]["stu3"].ToString().Equals(""))
                {
                    //对于师傅需要先弹出徒弟的菜单
                    List<string> ml = new List<string>();
                    if (!role["attr"]["msg"]["stu1"].ToString().Equals("")) ml.Add(role["attr"]["msg"]["stu1"]["name"].ToString());
                    if (!role["attr"]["msg"]["stu2"].ToString().Equals("")) ml.Add(role["attr"]["msg"]["stu2"]["name"].ToString());
                    if (!role["attr"]["msg"]["stu3"].ToString().Equals("")) ml.Add(role["attr"]["msg"]["stu3"]["name"].ToString());
                    Menu menu = Menu.create(ml, PointGet.getIndexPage());
                    menu.addCallback((mIndex) =>
                    {
                        playerName = ml[mIndex];
                        MsgSureUI.create(this.transform).show("解除师徒关系需要消耗银两50000，是否继续？", () =>
                        {
                            face.activityInterface.removeMatter(playerName, () => { });
                        }, () => { });
                    });
                }
                else
                {
                    msgCode.showMsg(1015);
                    return;
                }
               
            }
            else if (index == 3)
            {
                MsgSureUI.create(this.transform).show("即将取消登记，是否继续？", () =>
                {
                    face.activityInterface.cancelBs(() => { });
                }, () => { });
            }
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
