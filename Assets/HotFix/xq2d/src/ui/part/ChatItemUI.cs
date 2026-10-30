using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class ChatItemUI : PartUI
    {
        private Action callback;

        public static ChatItemUI create(Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("ChatItemUI", typeof(ChatItemUI));
            one.GetComponent<ChatItemUI>().setSize(Vector2.zero).putSence<ChatItemUI>(parent).setScreenCenter();
            one.GetComponent<ChatItemUI>().draw();
            return one.GetComponent<ChatItemUI>();
        }
        private ChatItemUI draw()
        {
            this.addClk(() => { if (this.callback != null) callback(); });

            GameObject hg = gameObjPool.getInstance().get("hg", typeof(ImgUI));
            hg.transform.SetParent(this.transform, false);
            hg.GetComponent<ImgUI>().setRoundedCorners(1, "#005d5f")
            .setSizePos(new Vector2(100, 100), Vector2.zero);
            hg.AddComponent<UIOutline>().setSome(new Color(0.06666667f, 0.3098039f, 0.3098039f, 1f), new Vector2(2f, -2f));


            GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(this.transform, false);
            icon.GetComponent<ImgUI>().setNoClk()
            .setSizePos(new Vector2(80, 80), new Vector2(10, -10));

            GameObject tx0 = gameObjPool.getInstance().get("name", typeof(TextUI));
            tx0.transform.SetParent(this.transform, false);
            tx0.GetComponent<TextUI>().setAlign("left").setColor("#0CA894").setFontSize(35).horiOut()
            .setSizePos(new Vector2(35 * 7, 50), new Vector2(110, 0));


            //背景要放置在文字之前
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(this.transform, false);
            bg.GetComponent<ImgUI>().setColor(0.06666667f, 0.3098039f, 0.3098039f, 0.5f);

            /*GameObject tx1 = gameObjPool.getInstance().get("content", typeof(TextMeshUI));
            tx1.transform.SetParent(this.transform, false);
            tx1.GetComponent<TextMeshUI>().setAlign("leftCenter").setColor("#ffffff").setFontSize(35).setSpace(10, 75);*/

            GameObject tx1 = gameObjPool.getInstance().get("content", typeof(TextRichUI));
            tx1.transform.SetParent(this.transform, false);
            tx1.GetComponent<TextRichUI>().setFacePy(Vector2.zero);

            GameObject voiceBg = gameObjPool.getInstance().get("voiceBg", typeof(ImgUI));
            voiceBg.transform.SetParent(this.transform, false);
            return this;
        }
        public ChatItemUI addClkIcon(Action ac)
        {
            this.callback = ac;
            return this;
        }

        public ChatItemUI setText(string name, string receiver, string content, JObject en, Action<Vector2> ac)
        {
            this.transform.Find("name").GetComponent<TextUI>().setText(name).setColor("#0CA894");
            if (receiver != null)
            {
                this.transform.Find("name").GetComponent<TextUI>().setText("<color=#8900FF>" + name + "</color> 对 <color=#8900FF>" + receiver + "</color> 说：")
                    .setColor("#0194FF");
            }
            //TextMeshUI contentUI = this.transform.Find("content").GetComponent<TextMeshUI>();
            TextRichUI contentUI = this.transform.Find("content").GetComponent<TextRichUI>();
            contentUI.gameObject.SetActive(true);
            ImgUI bgUI = this.transform.Find("bg").GetComponent<ImgUI>();
            bgUI.gameObject.SetActive(true);
            this.transform.Find("voiceBg").gameObject.SetActive(false);

            contentUI.setSizePos(new Vector2(580, 0), new Vector2(120, -60));
            contentUI.setTextValue(content, 35, en);
            //contentUI.setSizePos(new Vector2(580, contentUI.getTextHeight()), new Vector2(120, -60));

            float w = contentUI.getTextWidth() + 20, h = contentUI.getTextHeight() + 20;
            if (w > 600) w = 600;
            //由文本宽高来决定
            bgUI.setSizePos(new Vector2(w, h), new Vector2(110, -50));
            bgUI.setRoundedCorners(0.3f, new Color32(124, 157, 146, 255));//.drawRound(0.1f, w, h);
            Vector2 size = new Vector2(w, h + 50);
            ac(size);

            /* contentUI.renderFace2(content, () => {
                 TextMeshProUGUI t = contentUI.transform.GetComponent<TextMeshProUGUI>();
                 RectTransform tRc = t.GetComponent<RectTransform>();
                 //先宽度不变测高度，当高度<=34就改为高不变宽自适应
                 // width保持不变
                 tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 580);
                 // 动态设置height
                 tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.preferredHeight);
                 //单个文字高度最大48，会根据不同文字发生变化
                 if (tRc.rect.size.y < 48)
                 {
                     tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tRc.rect.size.y);
                     tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.preferredWidth);
                 }

                 contentUI.setSizePos(new Vector2(tRc.rect.size.x + 20, tRc.rect.size.y), new Vector2(120, -60));
                 //由文本宽高来决定
                 bgUI.setSizePos(new Vector2(tRc.rect.size.x + 20, tRc.rect.size.y + 20), new Vector2(110, -50));
                 bgUI.drawRound(0.1f, tRc.rect.size.x + 20, tRc.rect.size.y + 20);
                 Vector2 size = new Vector2(tRc.rect.size.x + 20, tRc.rect.size.y + 20 + 50);
                 ac(size);
             });*/



            return this;
        }
        /**更新头像*/
        public ChatItemUI updateHead(string head)
        {
            if (head == null) return this;
            //this.transform.Find("hg").GetComponent<ImgUI>().setRoundedCorners(0.5f,"#000000");
            this.transform.Find("icon").GetComponent<ImgUI>().loadRes(head);
            return this;
        }
    }
    class ChatItemUI2 : PartUI
    {
        public static ChatItemUI2 create(Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("ChatItemUI2", typeof(ChatItemUI2));
            one.GetComponent<ChatItemUI2>().setSize(Vector2.zero).putSence<ChatItemUI2>(parent).setScreenCenter();
            one.GetComponent<ChatItemUI2>().draw().addClk(() => { });
            return one.GetComponent<ChatItemUI2>();
        }
        private ChatItemUI2 draw()
        {

            GameObject tx1 = gameObjPool.getInstance().get("content", typeof(TextRichUI));
            tx1.transform.SetParent(this.transform, false);
            tx1.GetComponent<TextRichUI>().setFacePy(Vector2.zero);


            return this;
        }
        public ChatItemUI2 setText(string channel, string name, string content, JObject en, Action<Vector2> ac)
        {

            string color = "#FFC900";
            string str = "[世]";
            if (channel.Equals("0"))
            {
                color = "#FF2800";
                str = "[系]";
            }
            str += (channel.Equals("0") ? "" : (name + "：")) + content;
            TextRichUI contentUI = this.transform.Find("content").GetComponent<TextRichUI>();
            contentUI.setColor(color).setFontStyle().setNoClk();
            contentUI.gameObject.SetActive(true);
            contentUI.setSizePos(new Vector2(ScreenUtils.width - 100, 0), new Vector2(50, -60));
            contentUI.setTextValue(str, 35, en);
            contentUI.gameObject.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));

            float w = contentUI.getTextWidth() + 20, h = contentUI.getTextHeight() + 20;
            if (w > 600) w = 600;

            Vector2 size = new Vector2(w, h + 50);
            ac(size);

            return this;
        }
    }
}
