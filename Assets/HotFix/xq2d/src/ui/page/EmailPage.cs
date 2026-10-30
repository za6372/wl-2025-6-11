using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
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

namespace Assets.HotFix.xq2d.src.ui.page
{
    class EmailPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("邮件");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK2(content);
            DoGet.getInstance().startReqImg();
        }


        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));

            //黑色部分
            GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
            hb.transform.SetParent(k2.transform, false);
            hb.GetComponent<ImgUI>().setColor32(PageSetting.HbColor)
                .setSizePos(new Vector2(size.x, 100), Vector2.zero);

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 60), new Vector2(30, -130), k2.transform);
            this.drawList();
        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        public void updateWindow()
        {
            drawList();
        }
        public void writeNewEmail(string rec="")
        {
            JObject em = new JObject();
            em["title"] = "无";
            em["sender"] = "";
            em["receiver"] = rec;
            em["created"] = 0;
            em["isRead"] = 0;
            em["isObtain"] = 0;
            em["enclosure"] = new JArray();
            em["tale"] = 0;
            em["priceType"] = 0;
            em["price"] = 0;
            em["content"] = "无";
            EmailDesPage.create(em, this.transform, true);
        }
        /**绘制列表*/
        public void drawList()
        {
            JArray arr = face.emailInterface.getEmailList();
            
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform k2 = this.getStandardPageContent().Find("k2");
            Transform hb = k2.Find("hb");
            gameObjPool.getInstance().freeChildren(hb.gameObject);
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("邮件列表 " + arr.Count + "/50").setAlign().setColor32(PageSetting.FontColor).setFontSize(30).setFontStyle()
                .setSizePos(new Vector2(size.x, 100), Vector2.zero);
            //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);


            GoodsItem gdItem = GoodsItem.create(arr, content.transform, 6);
            gdItem.addCallback((mIndex) =>
            {
                JObject email = (JObject)arr[mIndex];
                List<string> ml = new List<string>();
                ml.Add("阅读邮件");
                ml.Add("回复邮件");
                ml.Add("写新邮件");
                if ((int)email["price"] > 0 && (int)email["isObtain"] == 0)
                    ml.Add("退回邮件");
                else
                    ml.Add("删除邮件");
                ml.Add("删除已读");
                ml.Add("删除全部");
                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((mIndex) =>
                {
                    if (mIndex == 0)
                    {
                        face.emailInterface.readEmail(email["Id"].ToString(),()=> {
                            EmailDesPage.create(email, this.transform, false);
                        });
                    }
                    else if (mIndex == 1)
                    {
                        this.writeNewEmail(email["receiver"].ToString());
                    }
                    else if (mIndex == 2)
                    {
                        this.writeNewEmail("");
                    }
                    else if (mIndex == 3)
                    {
                        if ((int)email["price"] > 0 && (int)email["isObtain"] == 0)
                        {
                            face.emailInterface.tuihuiEmail(email["Id"].ToString());
                        }
                        else
                        {
                            face.emailInterface.delEmail(email["Id"].ToString());
                        }    
                    }
                    else if (mIndex == 4)
                    {
                        face.emailInterface.delEmailByType(0);
                    }
                    else if (mIndex == 5)
                    {
                        face.emailInterface.delEmailByType(1);
                    }
                });
            });
            if (arr.Count == 0)
            {
                Vector2 size2 = content.GetComponent<RectTransform>().sizeDelta;
                GameObject wr = gameObjPool.getInstance().get("wr", typeof(TextUI));
                wr.transform.SetParent(content.transform, false);
                wr.GetComponent<TextUI>().setText("点击写新邮件").setAlign().setColor("#0CA894").setFontSize(35).setFontStyle()
                    .setSizePos(size2, Vector2.zero).addClk(() =>
                    {
                        //写邮件界面
                        this.writeNewEmail("");
                    });
                wr.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
            }
        }
        public override void init()
        {

        }
    }
}
