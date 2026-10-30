using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.fight;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class netErrorUI : PartUI
    {
        public static netErrorUI create()
        {
            Vector2 size = new Vector2(620, 420);
            GameObject one = gameObjPool.getInstance().get("netErrorUI", typeof(netErrorUI));
            one.GetComponent<netErrorUI>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height))
                .putSence<netErrorUI>(PointGet.getTipCanvas()).setScreenCenter();
            one.GetComponent<netErrorUI>().draw(size).addClk(() => { });


            return one.GetComponent<netErrorUI>();
        }
        public netErrorUI setMaskAlpha(float k)
        {
            this.transform.Find("mask").GetComponent<ImgUI>().setColor(0, 0, 0, k);
            return this;
        }
        public netErrorUI hideBtn()
        {
            this.transform.Find("kuang/bg/cancel").gameObject.SetActive(false);
            this.transform.Find("kuang/bg/sure").gameObject.SetActive(false);
            return this;
        }
        private netErrorUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setColor(0, 0, 0, 0f)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0)).addClk(() => { });

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2, -(ScreenUtils.height - size.y) / 2));

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(kuang.transform, false);
            bg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(20, -20));
            //左顶
            GameObject bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, 0));
            //顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(9, 1, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, 0));
            //左
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(0, -20));
            //右顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, 0));
            //右
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 0, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(size.x - 20, -20));
            //左下
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(0, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, -(size.y - 20)));
            //底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(9, 0, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, -(size.y - 20)));
            //右底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l3_png", new Rect(1, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, -(size.y - 20)));

            //内容区
            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(bg.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));

            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(bg.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(0, -320));
            btn.GetComponent<BtnUI>().addText("取消",30).setTextColor().loadRes("gy_03_png").addClk(() =>
            {
                Application.Quit(0);
            });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bg.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(420, -320));
            btn1.GetComponent<BtnUI>().addText("确定",30).setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                //重连
                btn1.SetActive(false);
                renderText("正在重连。。。");
                //需要重新登录一次
                netUtils.getInstance().sbh = null;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", strUtils.username);
                dic.Add("password", strUtils.password);
                dic.Add("apkVersion", versionUtils.getVersionMsg()["apkVersion"].ToString());
                DoGet.getInstance().sendPost("/loginService/login", dic, (obj) =>
                {
                    int num = 0;
                    if (int.TryParse(obj.ToString(), out num))
                    {
                        if (num == -1)
                        {
                            //连接失败
                            btn1.gameObject.SetActive(true);
                            renderText("重连失败，是否重试？");
                        }
                    }
                    else
                    {
                        JObject o = (JObject)obj;

                        if (o["body"] != null)
                        {

                            //保存账号和密码，用于下次游戏时回显
                            face.roleInterface.saveUser(strUtils.username, strUtils.password);
                            netUtils.getInstance().secret = o["secret"].ToString();
                            if (o["body"].ToString().Equals("1"))
                            {
                                //创建角色

                            }
                            else
                            {
                                netUtils.getInstance().sbh = o["sbh"].ToString();
                                netUtils.getInstance().body = (JObject)o["body"];

                                face.roleInterface.loadData(() =>
                                {
                                    //启动ws
                                    netUtils.getInstance().openWs(() => {

                                        fightCache.getInstance().isDoing = false;
                                        fightCache.getInstance().setAutoFight(false);
                                        this.free();
                                        if (PointGet.getFightPage() != null)
                                        {
                                            //返回主界面先
                                            attackAm.getInstance().clearHandle();
                                        }
                                    });
                                });

                            }
                        }
                    }
                },()=> {
                    //连接失败
                    btn1.gameObject.SetActive(true);
                    renderText("重连失败，是否重试？");
                });
            });
            return this;
        }
        public Transform getContent()
        {
            return this.transform.Find("kuang/bg/content");
        }
        
        /**渲染一段文本*/
        public void renderText(string str)
        {
            Transform ts = getContent();
            if (ts.Find("text") == null)
            {
                Vector2 size = ts.GetComponent<RectTransform>().sizeDelta;
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(ts, false);
                text.GetComponent<TextUI>().setText(str).setFontSize(30).setAlign().setColor().setIsRichText(true)
                .setSizePos(size, Vector2.zero);
            }
            else
            {
                ts.Find("text").GetComponent<TextUI>().setText(str);
            }
            
        }
        
        public netErrorUI show()
        {
            this.renderText("您已掉线，是否重连？");
            DoGet.getInstance().startReqImg();
            return this;
        }
    }
}
