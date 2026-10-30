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
    class ChuanBiRoomPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("传壁");
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
            ml.Add("大厅");
            ml.Add("积分榜");
            ml.Add("日志");
            ml.Add("兑换");
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
                draw0();
            }
            else if (index == 1)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);
                draw1();
            }
            else if (index == 2)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 500), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 500), new Vector2(30, -30 - 500), k2.transform);
                draw2();
            }
            else if (index == 3)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 500), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 500), new Vector2(30, -30 - 500), k2.transform);
                draw3();
            }


        }
        private void draw3()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("每3个礼包碎片兑换一个礼包")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));
            List<string> ls = new List<string>();
            for(int i= 10000247; i< 10000257; i++)
            {
                ls.Add(i.ToString());
            }
            JArray list = new JArray();
            for (int i = 0; i < ls.Count; i++)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(ls[i]);
                JObject a = new JObject();
                a.Add("name", gd.name);
                a.Add("key", gd.key);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                face.activityInterface.exchangeXianDanLiBao(list[mIndex]["key"].ToString(), () =>
                {

                });
            });
        }
        private void draw2()
        {

        }
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            /*Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));*/

            string[] m2 = { "白璧积分榜", "蓝璧积分榜", "紫璧积分榜", "橙璧积分榜", "帮派传壁积分榜", };
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
                face.activityInterface.getBiOrder(mIndex, (res) =>
                {
                    JArray list = (JArray)res["list"];
                    JArray m3 = new JArray();
                    for (int i = 0; i < list.Count; i++)
                    {
                        JObject a = (JObject)list[i];
                        JObject b = new JObject();
                        if (mIndex < 4)
                            b.Add("name", a["order"] + ". Lv" + a["lever"] + "  【" + GameAttrConst.getJobToSimpleName(GameAttrConst.getModel(a)) + "】" + a["name"] + " [" + a["jf"] + "积分]");
                        else
                            b.Add("name", a["order"] + ". Lv" + a["lever"] + " " + a["name"] + " [" + a["jf"] + "积分]");
                        m3.Add(b);
                    }
                    gameObjPool.getInstance().freeChildren(content.gameObject);
                    GoodsItem gdItem = GoodsItem.create(m3, content.transform, -1);
                    gdItem.addCallback((mIndex2) =>
                    {

                    });


                });


            });
        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            //随机取10条数据
            Action ac = () =>
            {
                face.activityInterface.viewUpB((arr) =>
                {
                    JArray list = new JArray();
                    for (int i = 0; i < arr.Count; i++)
                    {
                        JObject obj = (JObject)arr[i];
                        GoodsDes gds = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());

                        string name = obj["name"] + " 正在上传 " + gds.name + " 剩余时间 " + strUtils.nowTimeToEndTime(strUtils.getMillis(), (long)obj["created"]);
                        JObject a = new JObject();
                        a.Add("name", name);
                        list.Add(a);
                    }
                    gameObjPool.getInstance().freeChildren(content.gameObject);
                    GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                    gdItem.addCallback((mIndex) =>
                    {
                        MsgSureUI.create(this.transform).show("看他不顺眼，抢他壁？", () =>
                        {
                            JObject obj = (JObject)arr[mIndex];
                            face.activityInterface.qiangB(obj["name"].ToString());
                        }, () => { });
                    });

                });
            };


            drawBtn("传壁", hb, new Vector2(50, -400 + 70), () =>
              {
                  JArray bList = face.goodsInterface.getBi();
                  if (bList.Count == 0)
                  {
                      msgCode.showMsg(637);
                      return;
                  }
                  string gdKey = bList[0]["key"].ToString();
                  //需要提示传壁过程中可能遭遇抢夺，不能离开传送点，过程5分钟
                  MsgSureUI.create(this.transform).show("传壁过程中可能遭遇抢夺，并且不能离开传送点（5分钟）", () =>
                    {
                        face.activityInterface.uploadB(gdKey, () =>
                        {

                        });
                    }, () => { });
              });
            drawBtn("刷新", hb, new Vector2(50 + 140, -400 + 70), () =>
              {
                  //重新拉取一批列表数据
                  ac();
              });
            //显示可抢夺次数、上传次数
            face.activityInterface.getB((res) =>
            {
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(hb.transform);
                text.GetComponent<TextUI>().setText("剩余传壁次数：" + res["uptimes"] + "    剩余抢夺次数：" + res["qbtimes"]).setAlign("left").setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().horiOut()
                    .setSizePos(new Vector2(120, 60), new Vector2(50, -10));
                text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));

                ac();
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
