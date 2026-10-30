using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.childPage.email
{
    class EmailDesPage : PageUI
    {
        private JObject email;
        private bool isWrite;
        public static EmailDesPage create(JObject email, Transform parent, bool isWrite)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("EmailDesPage", typeof(EmailDesPage));
            EmailDesPage bs = one.GetComponent<EmailDesPage>();
            bs.setSize(size).putSence<EmailDesPage>(parent).setScreenCenter();
            bs.drawUI(email, isWrite);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public void drawUI(JObject email, bool isWrite)
        {
            this.email = email;
            this.isWrite = isWrite;
            this.createStandardPageLayout();
            this.setTitle("写邮件");
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

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 60), new Vector2(30, -30), k2.transform);
            this.drawList();
        }
        private void drawList()
        {
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform ts = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //gameObjPool.getInstance().freeChildren(content.gameObject);
            Vector2 size1 =  ts.GetComponent<RectTransform>().sizeDelta;
            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(ts, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size1.x , size1.y ), new Vector2(0, 0));
            scroll.GetComponent<ScrollUI>().initSetting();
            GameObject content = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(content);
            scroll.GetComponent<ScrollUI>().updateHeight(1480);

            GameObject text = gameObjPool.getInstance().get("recMan", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText(isWrite?"收件人：":"发件人：").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(200, 100), new Vector2(50, -50));
            //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            GameObject et = gameObjPool.getInstance().get("receiver", typeof(InputUI));
            et.transform.SetParent(content.transform, false);
            et.GetComponent<InputUI>()
            .setSizePos(new Vector2(400, 100), new Vector2(250, -50));
            et.GetComponent<InputUI>().initSetting("请输入收件人", "#0CA894", 40)
                .setContent(isWrite ? this.email["receiver"].ToString() : this.email["sender"].ToString())
                //.drawRound(0.5f, 400, 100);
                .setRoundedCorners(0.1f, new Color32(255, 255, 255, 255));
            if (!isWrite) et.GetComponent<InputUI>().setIsInput(false);
            et.AddComponent<UIOutline>().setSome(new Color32(102, 169, 139, 255), new Vector2(1, -1));

            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText("主题：").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(200, 100), new Vector2(50, -50 - 150));
            //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            et = gameObjPool.getInstance().get("title", typeof(InputUI));
            et.transform.SetParent(content.transform, false);
            et.GetComponent<InputUI>()
            .setSizePos(new Vector2(400, 100), new Vector2(250, -50 - 150));
            et.GetComponent<InputUI>().initSetting("请输入主题", "#0CA894", 40)
                .setContent(this.email["title"].ToString())
                //.drawRound(0.5f, 400, 100);
                .setRoundedCorners(0.1f, new Color32(255, 255, 255, 255));
            if (!isWrite) et.GetComponent<InputUI>().setIsInput(false);
            et.AddComponent<UIOutline>().setSome(new Color32(102, 169, 139, 255), new Vector2(1, -1));

            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText("正文：").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                .setSizePos(new Vector2(200, 100), new Vector2(50, -50 - 150 * 2));
            //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            et = gameObjPool.getInstance().get("em-content", typeof(InputUI));
            et.transform.SetParent(content.transform, false);
            et.GetComponent<InputUI>()
            .setSizePos(new Vector2((size.x - 150), 300), new Vector2(50, -50 - 150 * 2 - 100));
            et.GetComponent<InputUI>().initSetting("请输入正文", "#0CA894", 40, "#D9D9D9", "leftTop").setMulRow()
                //.drawRound(0.1f, size.x - 150, 300);
                .setRoundedCorners(0.1f, new Color32(255, 255, 255, 255));
            if (!isWrite) et.GetComponent<InputUI>().setIsInput(false);
            et.GetComponent<InputUI>().setContent(this.email["content"].ToString());
            et.AddComponent<UIOutline>().setSome(new Color32(102, 169, 139, 255), new Vector2(1, -1));

            if (isWrite)
            {
                GameObject lg = gameObjPool.getInstance().get("add", typeof(BtnUI));
                lg.transform.SetParent(content.transform, false);
                lg.GetComponent<BtnUI>().setColor("#0CA894")
                     //.drawRound(0.1f, 220, 80)
                     .setRoundedCorners(0.1f, "#0CA894")
                    .setSizePos(new Vector2(220, 80), new Vector2(50, -50 - 150 * 2 - 100 - 350));
                lg.GetComponent<BtnUI>().addText("添加附件", 36).setTextColor("#ffffff")
                .addClk(() =>
                {
                    AddEnsPage.create(this.email, this.transform).addCallback(()=> {
                        gameObjPool.getInstance().freeChildren(content.transform.Find("fujian").gameObject);
                        JArray ens = (JArray)this.email["enclosure"];
                        for (int i = 0; i < ens.Count; i++)
                        {
                            JObject d = (JObject)ens[i];
                            int ensIndex = i;
                            int enType = (int)d["enType"];
                            string str = null;
                            if (enType == 0)
                            {
                                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(d["key"].ToString());
                                 str = "Lv " + gd.lv + " " + ((int)d["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                                if (d["forging"] != null && (int)d["forging"]["lv"] > 0) str += " +" + d["forging"]["lv"];
                                str += " x" + d["nowNum"];
                            }
                            else
                            {
                                Pet gd = face.petInterface.getPetDataByKey(d["key"].ToString());
                                str = "Lv " + d["lever"] + " 成" + d["growLv"] + " " + gd.name + " x1";
                            }
                            
                            GameObject item = gameObjPool.getInstance().get("item", typeof(TextUI));
                            item.transform.SetParent(content.transform.Find("fujian"), false);
                            item.GetComponent<TextUI>().setText(str).setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                                .setSizePos(new Vector2(size.x - 150, 80), new Vector2(0, -90 * i)).addClk(()=> {
                                    List<string> ml = new List<string>();
                                    ml.Add("查看");
                                    ml.Add("移除");
                                    Menu menu = Menu.create(ml, this.transform);
                                    menu.addCallback((mIndex) =>
                                    {
                                        if (mIndex == 0)
                                        {
                                            if(enType==0) GoodsDesUI.create(d, this.transform).renderText();
                                            else PageUI.createAcPage<PetPage>(this.transform).drawUI(false, d);

                                        }
                                        else if (mIndex == 1)
                                        {
                                            ens.RemoveAt(ensIndex);
                                            gameObjPool.getInstance().free(item);
                                        }
                                        
                                    });
                                });
                            //item.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
                        }
                    });
                });
                lg.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));
                //列举添加的附件
                GameObject fujian = gameObjPool.getInstance().get("fujian", typeof(SimpleUI));
                fujian.transform.SetParent(content.transform, false);
                fujian.GetComponent<SimpleUI>()
                    .setSizePos(new Vector2(size.x - 150, 300), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90));

                lg = gameObjPool.getInstance().get("add", typeof(BtnUI));
                lg.transform.SetParent(content.transform, false);
                lg.GetComponent<BtnUI>().setColor("#0CA894")
                    //.drawRound(0.1f, 220, 80)
                    .setRoundedCorners(0.1f, "#0CA894")
                    .setSizePos(new Vector2(220, 80), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90 - 310));
                lg.GetComponent<BtnUI>().addText("设置价格", 36).setTextColor("#ffffff")
                .addClk(() =>
                {
                    UseNumInputUI.create(this.transform).addCallback((num) =>
                    {
                        content.transform.Find("price").GetComponent<TextUI>().setText(num.ToString());
                        this.email["price"] = num;
                    });
                });
                lg.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));
                GameObject price = gameObjPool.getInstance().get("price", typeof(TextUI));
                price.transform.SetParent(content.transform, false);
                price.GetComponent<TextUI>().setText("0").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(200, 100), new Vector2(280, -50 - 150 * 2 - 100 - 350 - 90 - 310));
                //price.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

                GameObject priceType = gameObjPool.getInstance().get("priceType", typeof(TextUI));
                priceType.transform.SetParent(content.transform, false);
                priceType.GetComponent<TextUI>().setText("元宝（点击切换）").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(400, 100), new Vector2(480, -50 - 150 * 2 - 100 - 350 - 90 - 310)).addClk(()=> {
                        if ((int)this.email["priceType"] == 0)
                        {
                            this.email["priceType"] = 1;
                            priceType.GetComponent<TextUI>().setText("银两（点击切换）");
                        }
                        else
                        {
                            this.email["priceType"] = 0;
                            priceType.GetComponent<TextUI>().setText("元宝（点击切换）");
                        }

                    });
                //priceType.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

                lg = gameObjPool.getInstance().get("add", typeof(BtnUI));
                lg.transform.SetParent(content.transform, false);
                lg.GetComponent<BtnUI>().setColor("#0CA894")
                    //.drawRound(0.1f, 220, 80)
                    .setRoundedCorners(0.1f, "#0CA894")
                    .setSizePos(new Vector2(220, 80), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90 - 310 - 90));
                lg.GetComponent<BtnUI>().addText("赠送银两", 36).setTextColor("#ffffff")
                .addClk(() =>
                {
                    UseNumInputUI.create(this.transform).addCallback((num) =>
                    {
                        content.transform.Find("tale").GetComponent<TextUI>().setText(num.ToString());
                        this.email["tale"] = num;
                    });
                });
                lg.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));
                GameObject tale = gameObjPool.getInstance().get("tale", typeof(TextUI));
                tale.transform.SetParent(content.transform, false);
                tale.GetComponent<TextUI>().setText("0").setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(200, 100), new Vector2(280, -50 - 150 * 2 - 100 - 350 - 90 - 310 - 90));
                //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));


                GameObject sure = gameObjPool.getInstance().get("sure", typeof(BtnUI));
                sure.transform.SetParent(this.transform.Find("Bottom"),false);
                sure.GetComponent<BtnUI>()
                .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
                sure.GetComponent<BtnUI>().addText("发送").setTextColor().loadRes("gy_03_png")
                    .addClk(() =>
                    {
                        //将输入框的值赋予email
                        this.email["title"] = content.transform.Find("title").GetComponent<InputUI>().getContent();
                        this.email["receiver"] = content.transform.Find("receiver").GetComponent<InputUI>().getContent();
                        this.email["content"] = content.transform.Find("em-content").GetComponent<InputUI>().getContent();
                        
                        
                        face.emailInterface.sendEmail(this.email);
                        this.freeThisPage();
                    });
            }
            else
            {
                
                //列举添加的附件
                GameObject fujian = gameObjPool.getInstance().get("fujian", typeof(SimpleUI));
                fujian.transform.SetParent(content.transform, false);
                fujian.GetComponent<SimpleUI>()
                    .setSizePos(new Vector2(size.x - 150, 300), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90));
                JArray ens = (JArray)this.email["enclosure"];
                for (int i = 0; i < ens.Count; i++)
                {
                    JObject d = (JObject)ens[i];
                    int enType = (int)d["enType"];
                    string str = null;
                    if (enType == 0)
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(d["key"].ToString());
                        str = "Lv " + gd.lv + " " + ((int)d["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                        if (d["forging"] != null && (int)d["forging"]["lv"] > 0) str += " +" + d["forging"]["lv"];
                        str += " x" + d["num"];
                    }
                    else
                    {
                        Pet gd = face.petInterface.getPetDataByKey(d["key"].ToString());
                        str = "Lv " + d["lever"] + " 成" + d["growLv"] + " " + gd.name + " x1";
                    }

                    GameObject item = gameObjPool.getInstance().get("item", typeof(TextUI));
                    item.transform.SetParent(fujian.transform, false);
                    item.GetComponent<TextUI>().setText(str).setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                        .setSizePos(new Vector2(size.x - 150, 80), new Vector2(0, -90 * i)).addClk(() => {
                            List<string> ml = new List<string>();
                            ml.Add("查看");
                            Menu menu = Menu.create(ml, this.transform);
                            menu.addCallback((mIndex) =>
                            {
                                if (mIndex == 0)
                                {
                                    if(enType==0)  GoodsDesUI.create(d, this.transform).renderText();
                                    else PageUI.createAcPage<PetPage>(this.transform).drawUI(false, d);
                                }
                                
                            });
                        });
                    item.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
                }

                text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(content.transform, false);
                text.GetComponent<TextUI>().setText("提取价格：" + this.email["price"] + ((int)email["priceType"] == 0 ? "元宝" : "银两"))
                    .setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(size.x - 150, 100), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90 - 310));
                //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

                text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(content.transform, false);
                text.GetComponent<TextUI>().setText("赠送银两：" + this.email["tale"]).setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle()
                    .setSizePos(new Vector2(size.x - 150, 100), new Vector2(50, -50 - 150 * 2 - 100 - 350 - 90 - 310 - 90));
                //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

                if ((ens.Count > 0 || (int)this.email["tale"] > 0)&&(int)this.email["isObtain"] == 0)
                {
                    GameObject lg = gameObjPool.getInstance().get("add", typeof(BtnUI));
                    lg.transform.SetParent(content.transform, false);
                    lg.GetComponent<BtnUI>().setColor("#0CA894")
                        //.drawRound(0.1f, 220, 80)
                        .setRoundedCorners(0.1f, "#0CA894")
                        .setSizePos(new Vector2(220, 80), new Vector2(50, -50 - 150 * 2 - 100 - 350));
                    lg.GetComponent<BtnUI>().addText("领取邮件", 36).setTextColor("#ffffff")
                    .addClk(() =>
                    {
                        lg.SetActive(false);
                        if ((int)this.email["price"] > 0)
                        {
                            MsgSureUI.create(this.transform).show("此邮件接取需要付费，是否继续？", () => {
                                face.emailInterface.getGoodsEmail(this.email);
                            }, () => { });
                            return;
                        }
                        face.emailInterface.getGoodsEmail(this.email);
                    });
                    lg.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));
                }

            }
        }

        public override void init()
        {

        }
    }
}
