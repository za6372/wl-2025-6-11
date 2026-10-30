using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.childPage.login
{
    class LoginUI : PartUI
    {

        public static LoginUI create(int index, Transform parent)
        {
            Vector2 size = new Vector2(800, 600);
            GameObject one = gameObjPool.getInstance().get("LoginUI", typeof(LoginUI));
            LoginUI bs = one.GetComponent<LoginUI>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<LoginUI>(parent).setLeftPos(Vector2.zero);
            bs.draw(index, size);

            DoGet.getInstance().startReqImg();
            return bs;
        }


        private LoginUI draw(int index, Vector2 size)
        {
            this.draw(size);
            if (index == 0)
            {
                toLogin();
            }
            else
            {
                toReg();
            }
            return this;
        }
        private void toReg()
        {
            Transform ts = this.transform.Find("content");
            gameObjPool.getInstance().freeChildren(ts.gameObject);

            GameObject username = gameObjPool.getInstance().get("username", typeof(InputUI));
            username.transform.SetParent(ts.transform, false);
            username.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75));
            username.GetComponent<InputUI>().initSetting("请输入账号").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            username.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject password = gameObjPool.getInstance().get("password", typeof(InputUI));
            password.transform.SetParent(ts.transform, false);
            password.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 1));
            password.GetComponent<InputUI>().initSetting("请输入密码").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            password.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject email = gameObjPool.getInstance().get("password", typeof(InputUI));
            email.transform.SetParent(ts.transform, false);
            email.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 2));
            email.GetComponent<InputUI>().initSetting("请输入邮箱").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            email.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject code = gameObjPool.getInstance().get("password", typeof(InputUI));
            code.transform.SetParent(ts.transform, false);
            code.GetComponent<InputUI>().setSizePos(new Vector2(270, 60), new Vector2(190, -75 - 70 * 3));
            code.GetComponent<InputUI>().initSetting("请输入验证码").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            code.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject send = gameObjPool.getInstance().get("send", typeof(BtnUI));
            send.transform.SetParent(ts.transform, false);
            send.GetComponent<BtnUI>().setRoundedCorners(0.5f, "#CCCCCC")
            .setSizePos(new Vector2(120, 60), new Vector2(190 + 280, -75 - 70 * 3));
            send.GetComponent<BtnUI>().addText("获取", 35, default, "#ffffff").addClk(() =>
            {
                //验证账号合法性
                if (!strUtils.isEnoughLen(username.GetComponent<InputUI>().getContent(), 6, 12) ||
                !strUtils.isMatch(username.GetComponent<InputUI>().getContent(), "[A-Za-z0-9]{6,}"))
                {
                    ts.Find("text").GetComponent<TextUI>().setText("账号为6-12位字母、数字！");
                    return;
                }

                if (!strUtils.isEnoughLen(password.GetComponent<InputUI>().getContent(), 6, 12) ||
                !strUtils.isMatch(password.GetComponent<InputUI>().getContent(), "[A-Za-z0-9]{6,}"))
                {
                    ts.Find("text").GetComponent<TextUI>().setText("密码为6-12位字母、数字！");
                    return;
                }
                if (!strUtils.isMatch(email.GetComponent<InputUI>().getContent(), "[a-zA-Z0-9_-]+@([a-zA-Z0-9]+\\.)+(com|cn|net|org)"))
                {
                    ts.Find("text").GetComponent<TextUI>().setText("邮箱格式有误！（是否遗忘后缀，如@qq.com）");
                    return;
                }
                send.gameObject.SetActive(false);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", username.GetComponent<InputUI>().getContent());
                dic.Add("password", password.GetComponent<InputUI>().getContent());
                dic.Add("email", email.GetComponent<InputUI>().getContent());
                DoGet.getInstance().sendPost("/sysService/getCode", dic, (res) =>
                {
                    Debug.Log(res);
                    Text tip = ts.Find("text").GetComponent<Text>();
                    if (res.ToString().Equals("1"))
                    {
                        tip.text = "发送成功！";
                    }
                    else if (res.ToString().Equals("0"))
                    {
                        tip.text = "验证码已经发送过了！";
                    }
                    else if (res.ToString().Equals("2"))
                    {
                        tip.text = "账号密码有误！";
                        send.gameObject.SetActive(true);
                    }
                    else if (res.ToString().Equals("3"))
                    {
                        tip.text = "一个邮箱最多绑定3个账号！";
                        send.gameObject.SetActive(true);
                    }
                    else if (res.ToString().Equals("4"))
                    {
                        tip.text = "该账号已绑定过邮箱！";
                        send.gameObject.SetActive(true);
                    }
                });
            });

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(ts.transform, false);
            text.GetComponent<TextUI>().setFontSize(30).setColor("#F0E68C").setAlign("center").setText("")
            .setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 4));

            GameObject lg = gameObjPool.getInstance().get("lg", typeof(BtnUI));
            lg.transform.SetParent(ts.transform, false);
            lg.GetComponent<BtnUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 5));
            lg.GetComponent<BtnUI>().addText("立即注册", 30).setTextColor("#ffffff").setColor("#15725f").setRoundedCorners(0.5f, "#15725f")
            .addClk(() =>
            {
                string cd = code.GetComponent<InputUI>().getContent();
                if (cd.Length != 6 || !strUtils.isMatch(cd, "[0-9]{6}"))
                {
                    return;
                }
                string usn = username.GetComponent<InputUI>().getContent();
                if (!strUtils.isEnoughLen(usn, 6, 12) ||
                !strUtils.isMatch(usn, "[A-Za-z0-9]{6,}"))
                {
                    return;
                }

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", usn);
                dic.Add("code", cd);
                DoGet.getInstance().sendPost("/sysService/matchCode", dic, (res) =>
                {
                    if (res.ToString().Equals("1"))
                    {
                        code.GetComponent<InputUI>().setContent("");
                        face.roleInterface.saveUser(usn, password.GetComponent<InputUI>().getContent());
                        this.free();
                        msgCode.showMsg(607);
                    }
                    else
                    {
                        this.transform.Find("tip").GetComponent<TextUI>().setText("验证码错误！（纯数字呀大胸弟）");
                    }
                });
            });
            lg.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));


            DoGet.getInstance().startReqImg();
        }
        private void toLogin()
        {
            Transform ts = this.transform.Find("content");
            gameObjPool.getInstance().freeChildren(ts.gameObject);

            JObject user = face.roleInterface.getUser();
            string usernameStr = "", passwordStr = "";
            if (user != null)
            {
                usernameStr = (string)user["username"];
                passwordStr = (string)user["password"];
            }
            GameObject username = gameObjPool.getInstance().get("username", typeof(InputUI));
            username.transform.SetParent(ts.transform, false);
            username.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75));
            username.GetComponent<InputUI>().initSetting("请输入账号").addMatchSimple().setContent(usernameStr).setRoundedCorners(0.5f);
            username.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject password = gameObjPool.getInstance().get("password", typeof(InputUI));
            password.transform.SetParent(ts.transform, false);
            password.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70));
            password.GetComponent<InputUI>().initSetting("请输入密码").addMatchSimple().setContent(passwordStr).setRoundedCorners(0.5f);
            password.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(ts.transform, false);
            text.GetComponent<TextUI>().setFontSize(30).setColor("#F0E68C").setAlign("center").setText("")
            .setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 2));

            GameObject lg = gameObjPool.getInstance().get("lg", typeof(BtnUI));
            lg.transform.SetParent(ts.transform, false);
            lg.GetComponent<BtnUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 5));
            lg.GetComponent<BtnUI>().addText("立即登录", 30).setTextColor("#ffffff").setColor("#15725f").setRoundedCorners(0.5f, "#15725f")
            .addClk(() =>
            {
                lg.SetActive(false);
                netUtils.getInstance().sbh = null;
                string str = "";
                //验证
                if (!username.GetComponent<InputUI>().isMatch() || !password.GetComponent<InputUI>().isMatch())
                {
                    str = msgCode.getStrByCode(1000);
                    text.GetComponent<TextUI>().setText(str);
                    lg.SetActive(true);
                    return;
                }
                else
                {
                    str = "登陆中";
                }
                
                Debug.Log("=====1======");
                strUtils.username = username.GetComponent<InputUI>().getContent();
                strUtils.password = password.GetComponent<InputUI>().getContent();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", username.GetComponent<InputUI>().getContent());
                dic.Add("password", password.GetComponent<InputUI>().getContent());
                dic.Add("apkVersion", versionUtils.getVersionMsg()["apkVersion"].ToString());
                Debug.Log("=====2======");
                text.GetComponent<TextUI>().setText(str);
                DoGet.getInstance().sendPost("/loginService/login", dic, (res) =>
                {
                    text.GetComponent<TextUI>().setText("登录成功");
                    loginResult(res);
                }, () =>
                {
                    text.GetComponent<TextUI>().setText(msgCode.getStrByCode(1009));
                    lg.SetActive(true);
                });
            });
            lg.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));


            DoGet.getInstance().startReqImg();
        }
        private void loginResult(object obj)
        {
            netUtils.getInstance().Dispose();
            Debug.Log("=====3======");
            Text tip = this.transform.Find("content/text").GetComponent<Text>();
           
            if (strUtils.isNumber(obj))
            {
                tip.text = "登录失败";
                if (obj.ToString().Equals("0"))
                {
                    tip.text = msgCode.getStrByCode(1002);
                }
                else if (obj.ToString().Equals("2"))
                {
                    tip.text = msgCode.getStrByCode(1003);
                }
                else if (obj.ToString().Equals("3"))
                {
                    tip.text = msgCode.getStrByCode(1004);
                }
                else if (obj.ToString().Equals("4"))
                {
                    //进行邮箱验证
                    tip.text = msgCode.getStrByCode(1005);
                    //显示邮箱验证码
                }
                else if (obj.ToString().Equals("5"))
                {
                    tip.text = msgCode.getStrByCode(1006);
                }
                else if (obj.ToString().Equals("-1"))
                {
                    tip.text = msgCode.getStrByCode(1007);
                }
            }
            else
            {
                tip.text = msgCode.getStrByCode(1008);
                JObject o = (JObject)obj;
                if (o["body"] != null)
                {
                    //保存账号和密码，用于下次游戏时回显
                    face.roleInterface.saveUser(strUtils.username, strUtils.password);
                    netUtils.getInstance().secret = o["secret"].ToString();
                    if (o["body"].ToString().Equals("1"))
                    {
                        //创建角色
                        //drawCreateRoleUI();
                    }
                    else
                    {
                        netUtils.getInstance().sbh = o["sbh"].ToString();
                        netUtils.getInstance().body = (JObject)o["body"];
                    }
                    //this.freeThisPage();
                    this.GetComponentInParent<LoginPage>().freeThisPage();
                    PageUI.create<ChooseServerPage>().drawUI();
                    gameObjPool.getInstance().freeChildren(PointGet.getTipCanvas().gameObject);
                }

            }
        }
        private LoginUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setAlpha(0)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero)
                .addClk(() => { this.free(); });

            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, pos);

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
            content.transform.SetParent(this.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x + 20, pos.y - 20));
            return this;
        }
    }
}
