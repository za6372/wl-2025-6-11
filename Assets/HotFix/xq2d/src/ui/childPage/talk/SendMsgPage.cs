using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.email;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.childPage.talk
{
    class SendMsgPage : PageUI
    {
        private string recName;//接收者昵称，私信时使用
        private int channelIndex;
        public static SendMsgPage create(Transform parent, string recName = null)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("SendMsgPage", typeof(SendMsgPage));
            SendMsgPage bs = one.GetComponent<SendMsgPage>();
            bs.setSize(size).putSence<SendMsgPage>(parent).setScreenCenter();
            bs.drawUI(recName);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public void drawUI(string recName)
        {
            this.recName = recName;
            if (this.recName != null) this.channelIndex = 10;
            this.createStandardPageLayout();
            this.setTitle("频道发言");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK2(content);
            DoGet.getInstance().startReqImg();
        }

        public override void init()
        {

        }

        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 60), new Vector2(30, -30), k2.transform);
            this.drawList();

            GameObject sure = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            sure.transform.SetParent(this.transform.Find("Bottom"), false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("发送").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    //获取内容
                    string content = k1.getContent().Find("em-content").GetComponent<InputUI>().getContent();
                    
                    //验证消息
                    //1-100个字
                    if (content.Length > 100)
                        content = content.Substring(0, 100);
                    //缓存
                    JObject r = face.roleInterface.getRole();
                    string head = GameAttrConst.getRoleHead(r) + "_png";
                    if (recName == null)
                        face.chatInterface.writeMsg(r["name"].ToString(), content, this.channelIndex, 0, head, this.en);
                    else
                        face.chatInterface.putSxCache(this.recName, content, head, this.en);
                    
                    face.chatInterface.sendMsg(this.recName, content, this.channelIndex + "", head, null, this.en);
                    this.freeThisPage();
                    //刷新ui
                    eventsUtils.dispatchWsEvent("106", null);
                });
        }
        private void drawList()
        {
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject text = null;
            if (this.recName == null)
            {
                text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(content.transform, false);
                text.GetComponent<TextUI>().setText("选择频道：").setAlign("left").setColor("#000000").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(200, 100), new Vector2(50, -50));

                GameObject tabDi = gameObjPool.getInstance().get("tab", typeof(SimpleUI));
                tabDi.transform.SetParent(content.transform, false);
                tabDi.GetComponent<SimpleUI>()
                    .setSizePos(new Vector2(600, 100), new Vector2((size.x - 600) / 2f, -150));
                List<string> ml = new List<string>();
                ml.Add("世界");
                ml.Add("队伍");
                ml.Add("帮派");
                Tab tab = Tab.create(ml, Vector2.zero, tabDi.transform);
                tab.addCallback((mIndex) =>
                {
                    if (mIndex == 0) this.channelIndex = 2;
                    else if (mIndex == 1) this.channelIndex = 5;
                    else if (mIndex == 2) this.channelIndex = 4;
                });
                tab.clkDefault();
            }
            else
            {
                text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(content.transform, false);
                text.GetComponent<TextUI>().setText("私信人："+this.recName).setAlign("left").setColor("#000000").setFontSize(40).setFontStyle().horiOut()
                    .setSizePos(new Vector2(200, 100), new Vector2(50, -50));
            }


            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText("聊天内容：").setAlign("left").setColor("#000000").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(200, 100), new Vector2(50, -250));

            GameObject et = gameObjPool.getInstance().get("em-content", typeof(InputUI));
            et.transform.SetParent(content.transform, false);
            et.GetComponent<InputUI>()
            .setSizePos(new Vector2((size.x - 100), 300), new Vector2(50, -350));
            et.GetComponent<InputUI>().initSetting("请输入内容", "#5E270F", 40, "#D9D9D9", "leftTop").setMulRow();
            et.GetComponent<InputUI>().setContent("");
            et.AddComponent<UIOutline>().setSome(new Color(0.5f, 0.5f, 0.5f, 1), Vector2.one * 1.5f);

            GameObject lg = gameObjPool.getInstance().get("add", typeof(BtnUI));
            lg.transform.SetParent(content.transform, false);
            lg.GetComponent<BtnUI>().setColor("#B3B3B3")
                .setSizePos(new Vector2(220, 80), new Vector2(50, -660));
            lg.GetComponent<BtnUI>().addText("添加附件", 36).setTextColor("#254B40")
            .addClk(() =>
            {
                AddChatEnsPage.create(this.transform).addCallback((en) =>
                {
                    updateEn(en);
                });
            });

            GameObject fujian = gameObjPool.getInstance().get("fujian", typeof(SimpleUI));
            fujian.transform.SetParent(content.transform, false);
            fujian.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(300, 80), new Vector2(50 + 230, -660));


            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText("添加表情：").setAlign("left").setColor("#000000").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(200, 100), new Vector2(50, -750));

            GameObject fc = gameObjPool.getInstance().get("fc", typeof(SimpleUI));
            fc.transform.SetParent(content.transform, false);
            fc.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(640, 180), new Vector2((size.x - 640) / 2f, -850));
            for (int i = 0; i < 14; i++)
            {
                int index = i;
                float x = (i % 7) * 90;
                float y = -(i / 7) * 90;
                GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
                hb.transform.SetParent(fc.transform, false);
                hb.GetComponent<ImgUI>().setColor("#ffffff")
                    .setSizePos(new Vector2(80, 80), new Vector2(x, y));
                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(hb.transform, false);
                icon.GetComponent<ImgUI>().loadRes("face_" + i + "_png")
                    .setSizePos(new Vector2(60, 60), new Vector2(10, -10)).addClk(() =>
                    {
                        string bq = "#" + (index > 9 ? index : ("0" + index));
                        InputUI inp = et.GetComponent<InputUI>();
                        inp.setContent(inp.getContent() + bq);
                    });
            }
        }
        private JObject en;
        /**刷新附件*/
        private void updateEn(JObject d)
        {
            this.en = d;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            Transform fujian = content.Find("fujian");
            gameObjPool.getInstance().freeChildren(fujian.gameObject);

            if (d == null) return;
            string str = d["name"].ToString();
            GameObject item = gameObjPool.getInstance().get("item", typeof(TextUI));
            item.transform.SetParent(fujian.transform, false);
            item.GetComponent<TextUI>().setText(str).setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(300, 80), new Vector2(0, 0)).addClk(() =>
                {
                    List<string> ml = new List<string>();
                    ml.Add("查看");
                    ml.Add("移除");
                    Menu menu = Menu.create(ml, this.transform);
                    menu.addCallback((mIndex) =>
                    {
                        if (mIndex == 0)
                        {
                            GoodsDesUI.create(d, this.transform).renderText();
                        }
                        else
                        {
                            updateEn(null);
                        }

                    });
                });
            item.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
        }
    }
}
