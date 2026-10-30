using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.fight;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.activityPage;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.data;
using Assets.Res.script.src.touch;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    /**游戏主页*/
    class IndexPage : PageUI
    {

        public void startWs()
        {
            LoadingUI.getOne().updateJd(0, "初始化1");
            netUtils.getInstance().initWs(() =>
            {
                LoadingUI.getOne().updateJd(0, "初始化2");
                JObject r = face.roleInterface.getRole();
                mapManager.getInstance().reloadBef(r["pos"]["map"].ToString(), () => {
                    startThread();
                });
            });
        }
        /**启动定时器*/
        private void startThread()
        {
            timeManager.getTimeManageOne().putPeriodTask(() =>
            {
                //保存上一次拉取的消息
                face.chatInterface.handleMsgQueue();
                //拉取消息
                face.chatInterface.getTimeMsg();
                //同步位置
                face.roleInterface.uploadRolePos();
            }, 2000);
        }
        public void drawUI()
        {
            LoadingUI.getOne();
            

            GameObject map = gameObjPool.getInstance().get("Map", typeof(SimpleUI));
            map.transform.SetParent(this.transform, false);
            map.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height - 300), Vector2.zero);
            map.AddComponent<RectMask2D>();

            GameObject top = gameObjPool.getInstance().get("Top", typeof(SimpleUI));
            top.transform.SetParent(this.transform, false);
            top.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 90), Vector2.zero)
                .addClk(() =>
               {
                   PageUI.createAcPage<MiniMapPage>(PointGet.getIndexPageOfPage()).drawUI();

               });
            drawTop(top.transform);

            GameObject icon = gameObjPool.getInstance().get("Icon", typeof(SimpleUI));
            icon.transform.SetParent(this.transform, false);
            icon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(403, 200), Vector2.zero);
            drawIcon(icon.transform);

            GameObject menu = gameObjPool.getInstance().get("Menu", typeof(SimpleUI));
            menu.transform.SetParent(this.transform, false);
            menu.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 96), new Vector2(0, -(ScreenUtils.height - 300 - 96)));

            drawMenu(menu.transform);

            GameObject talk = gameObjPool.getInstance().get("Talk", typeof(SimpleUI));
            talk.transform.SetParent(this.transform, false);
            talk.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 300), new Vector2(0, -(ScreenUtils.height - 300)))
                .addClk(() =>
                {
                    PageUI.createAcPage<TalkPage>(PointGet.getIndexPageOfPage()).drawUI(1);

                });
            drawTalk(talk.transform);

            GameObject page = gameObjPool.getInstance().get("Page", typeof(SimpleUI));
            page.transform.SetParent(this.transform, false);
            page.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);

            

            DoGet.getInstance().startReqImg();

            this.startWs();

            updateChatMsg();
        }
        private void drawTalk(Transform ts)
        {
            GameObject Bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            Bg.transform.SetParent(ts, false);
            Bg.GetComponent<ImgUI>().setColor("#00BDFF")
                .setSizePos(new Vector2(ScreenUtils.width, 300), Vector2.zero);
            Bg.AddComponent<Mask>();

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat1_png")
                .setSizePos(new Vector2(30, 30), Vector2.zero);

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat2_png")
                .setSizePos(new Vector2(30, 30), new Vector2(ScreenUtils.width - 30, 0));

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat4_png")
                .setSizePos(new Vector2(30, 30), new Vector2(0, -270));

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat3_png")
                .setSizePos(new Vector2(30, 30), new Vector2(ScreenUtils.width - 30, -270));

            //战斗测试
            if (Application.isEditor)
            {
                GameObject testBtn = gameObjPool.getInstance().get("testBtn", typeof(BtnUI));
                testBtn.transform.SetParent(ts.transform, false);
                testBtn.GetComponent<BtnUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 60), new Vector2(10, -20)).addClk(() =>
                    {
                        if (!fightCache.getInstance().isDoing)
                            face.fightInterface.createFightByMonster("gw1000");
                    });
                testBtn.GetComponent<BtnUI>().addText("进入战斗", 26, default, "#ffffff");

                testBtn = gameObjPool.getInstance().get("testBtn", typeof(BtnUI));
                testBtn.transform.SetParent(ts.transform, false);
                testBtn.GetComponent<BtnUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 60), new Vector2(10 + 160, -20)).addClk(() =>
                    {
                        string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                        mapKey = "hhbk_3";
                        mapManager.getInstance().reloadBef(mapKey);
                    });
                testBtn.GetComponent<BtnUI>().addText("刷新地图", 26, default, "#ffffff");

                testBtn = gameObjPool.getInstance().get("testBtn", typeof(BtnUI));
                testBtn.transform.SetParent(ts.transform, false);
                testBtn.GetComponent<BtnUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 60), new Vector2(10 + 160 * 2, -20)).addClk(() =>
                    {
                        PageUI.createAcPage<JinShouZhiPage>(this.transform).drawUI().clkTab(0);
                    });
                testBtn.GetComponent<BtnUI>().addText("金手指", 26, default, "#ffffff");

                
            }
            string n= face.roleInterface.getRole()["name"].ToString();
            if (n.Equals("梅仁义") || n.Equals("林志玲"))
            {
                GameObject testBtn = gameObjPool.getInstance().get("testBtn", typeof(BtnUI));
                testBtn.transform.SetParent(ts.transform, false);
                testBtn.GetComponent<BtnUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 60), new Vector2(10 + 160 * 3, -20)).addClk(() =>
                    {
                        PageUI.createAcPage<SysCheckManagerPage>(this.transform).drawUI().clkTab(0);
                    });
                testBtn.GetComponent<BtnUI>().addText("金额统计", 26, default, "#ffffff");
            }
        }
        public void updateChatMsg()
        {
            Transform ts = this.transform.Find("Talk/Bg");
            gameObjPool.getInstance().freeChildren(ts.gameObject);
            JArray list1 = face.chatInterface.getMsg(null, "0");
            JArray list2 = face.chatInterface.getMsg(null, "2");
            JArray mergedArray = new JArray(list1.Union(list2));
            mergedArray = new JArray(mergedArray.OrderBy(obj => (string)obj["created"]));

            int len = 5;
            if (mergedArray.Count < 5) len = mergedArray.Count;
            JArray arr = new JArray();
            for (int i = mergedArray.Count - len; i < mergedArray.Count; i++)
            {
                arr.AddFirst(mergedArray[i]);
            }
            loadOneChatItem(arr, 0, ts, 0);
            DoGet.getInstance().startReqImg();
        }
        private void loadOneChatItem(JArray arr, int i, Transform ts, float y)
        {
            if (i >= arr.Count) return;
            JObject o = (JObject)arr[i];
            ChatItemUI2 one = ChatItemUI2.create(ts);
            JObject en = null;
            if (!strUtils.isNull(o["en"])) en = (JObject)o["en"];
            //Debug.Log(o);
            one.setText(o["channel"].ToString(), o["sender"].ToString(), o["content"].ToString(),en, (size) =>
             {
                 one.setSizePos(size, new Vector2(10, -300 + size.y + y));
                 y += size.y;
                 i++;
                 loadOneChatItem(arr, i, ts, y);
             });
        }
        /**菜单闪烁*/
        public void shan(string btnName)
        {
            Transform ts = this.transform.Find("Menu/" + btnName).GetChild(0);
            int i = 0;
            if (btnName.Equals("packageBtn")) i = 6;
            else if (btnName.Equals("teamBtn")) i = 7;
            else if (btnName.Equals("chatBtn")) i = 8;
            else if (btnName.Equals("emailBtn")) i = 9;
            ts.GetComponent<ImgUI>().loadRes("cg_icon_png", new Rect(i * 20, 0, 20, 20));
            DoGet.getInstance().startReqImg();
        }
        /**恢复原状态*/
        public void recover(string btnName)
        {
            Transform ts = this.transform.Find("Menu/" + btnName).GetChild(0);
            int i = 0;
            if (btnName.Equals("packageBtn")) i = 1;
            else if (btnName.Equals("teamBtn")) i = 3;
            else if (btnName.Equals("chatBtn")) i = 4;
            else if (btnName.Equals("emailBtn")) i = 5;
            ts.GetComponent<ImgUI>().loadRes("cg_icon_png", new Rect(i * 20, 0, 20, 20));
            DoGet.getInstance().startReqImg();
        }
        /**修改备忘按钮的名字*/
        public void updateTeamStatus()
        {
            string btnName = "备忘";
            if (face.teamInterface.isInTeam() && !face.teamInterface.isCaptain())
            {
                if ((int)face.teamInterface.getSelfFromTeam()["isFollow"] == 1)
                {
                    //入队后跟随
                    //MYCONST.mapPage.playerUI.followCaptain();
                    btnName = "暂离";
                }
                else
                {
                    btnName = "跟随";
                }
            }
            this.transform.Find("Menu/ForgetBtn/text").GetComponent<TextUI>().setText(btnName);
        }
        private void drawMenu(Transform ts)
        {
            GameObject Bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            Bg.transform.SetParent(ts, false);
            Bg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
                .setSizePos(new Vector2(ScreenUtils.width, 96), Vector2.zero);

            GameObject MenuBtn = gameObjPool.getInstance().get("MenuBtn", typeof(ImgUI));
            MenuBtn.transform.SetParent(ts, false);
            MenuBtn.GetComponent<ImgUI>().loadRes("gy_03_png")
                .setSizePos(new Vector2(168, 96), Vector2.zero)
                .addClk(() =>
                {
                    PageUI.createAcPage<MenuPage>(PointGet.getIndexPageOfPage()).drawUI();

                });

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(MenuBtn.transform, false);
            text.GetComponent<TextUI>().setAlign().setColor().setText("菜单").setFontSize(30).setFontStyle()
                .setSizePos(new Vector2(168, 96), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));

            GameObject ForgetBtn = gameObjPool.getInstance().get("ForgetBtn", typeof(ImgUI));
            ForgetBtn.transform.SetParent(ts, false);
            ForgetBtn.GetComponent<ImgUI>().loadRes("gy_02_png")
                .setSizePos(new Vector2(168, 96), new Vector2(ScreenUtils.width - 168, 0))
                .addClk(() =>
                {
                    string str = ts.Find("ForgetBtn/text").GetComponent<TextUI>().getText();
                    if (str.Equals("备忘"))
                    {
                        PageUI.createAcPage<ForgetPage>(PointGet.getIndexPageOfPage()).drawUI();
                    }
                    else
                    {
                        JObject status = face.teamInterface.getMeFromTeam();
                        int isFollow = (int)status["isFollow"] == 1 ? 0 : 1;
                        face.teamInterface.uploadStatus(isFollow);
                    }
                    
                });

            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(ForgetBtn.transform, false);
            text.GetComponent<TextUI>().setAlign().setColor().setText("备忘").setFontSize(30).setFontStyle()
                .setSizePos(new Vector2(168, 96), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));

            //两侧宽度
            //float dw = (ScreenUtils.width - 168 * 2 - 96 * 6) / 2;
            float dw = (ScreenUtils.width - 168 * 2 - 96 * 6) / 7;
            string[] btnName = { "taskBtn", "packageBtn", "roundBtn", "teamBtn", "chatBtn", "emailBtn" };
            for (int i = 0; i < 6; i++)
            {
                int index = i;
                GameObject btn = gameObjPool.getInstance().get(btnName[i], typeof(ImgUI));
                btn.transform.SetParent(ts, false);
                btn.GetComponent<ImgUI>().loadRes("cg_04_png")
                    .setSizePos(new Vector2(96, 96), new Vector2(168 + dw + i * (96 + dw), 0))
                    .addClk(() =>
                    {
                        clkMIcon(index);
                    });

                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(btn.transform, false);
                icon.GetComponent<ImgUI>().loadRes("cg_icon_png", new Rect(i * 20, 0, 20, 20))
                    .setSizePos(new Vector2(80, 80), new Vector2(8, -8));
            }
        }
        private void clkMIcon(int index)
        {
            if (index == 0) PageUI.createAcPage<TaskPage>(PointGet.getIndexPageOfPage()).drawUI().clkDefault();
            else if (index == 1)
            {
                recover("packageBtn");
                PageUI.createAcPage<PackagePage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (index == 2) PageUI.createAcPage<RoundPage>(PointGet.getIndexPageOfPage()).drawUI();
            else if (index == 3)
            {
                recover("teamBtn");
                PageUI.createAcPage<TeamPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (index == 4)
            {
                recover("chatBtn");
                PageUI.createAcPage<TalkPage>(PointGet.getIndexPageOfPage()).drawUI(1);
            }
            else if (index == 5)
            {
                recover("emailBtn");
                PageUI.createAcPage<EmailPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
        }
        private void drawIcon(Transform ts)
        {
            GameObject manIcon = gameObjPool.getInstance().get("ManIcon", typeof(SimpleUI));
            manIcon.transform.SetParent(ts, false);
            manIcon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(304, 200), Vector2.zero)
                .addClk(() =>
                {
                    PageUI.createAcPage<ManPage>(PointGet.getIndexPageOfPage()).drawUI(true);

                });

            GameObject bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            bg.transform.SetParent(manIcon.transform, false);
            bg.GetComponent<ImgUI>().loadRes("cg_a1_png")
                .setSizePos(new Vector2(304, 200), Vector2.zero);

            JObject role = face.roleInterface.getRole();
            GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(bg.transform, false);
            icon.GetComponent<ImgUI>().loadRes(GameAttrConst.getRoleHead(role) + "_png")
            .setSizePos(new Vector2(120, 120), new Vector2(28, -22));

            updateRoleXueTiao();

            //宠物
            manIcon = gameObjPool.getInstance().get("PetIcon", typeof(SimpleUI));
            manIcon.transform.SetParent(ts, false);
            manIcon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(243, 100), new Vector2(160, -50))
                .addClk(() =>
                {
                    PageUI.createAcPage<PetPage>(PointGet.getIndexPageOfPage()).drawUI(true);

                });

            bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            bg.transform.SetParent(manIcon.transform, false);
            bg.GetComponent<ImgUI>().loadRes("cg_a2_png")
                .setSizePos(new Vector2(243, 100), Vector2.zero);

            updatePetHeadIcon();
            updatePetXueTiao();

        }
        public void updatePetHeadIcon()
        {
            Transform bg = this.transform.Find("Icon/PetIcon/Bg");
            if (bg.Find("icon") != null) gameObjPool.getInstance().free(bg.Find("icon").gameObject);
            JObject a = face.petInterface.getIsFightPet();
            if (a != null)
            {
                Pet pet = face.petInterface.getPetDataByKey(a["key"].ToString());
                AmModelMsg am = amManager.getAmByKey(pet.pwdId);
                string[] pwds = am.touxiangPwds;
                string[] aefs = { am.touxiangAef };
                List<string> all = new List<string>();
                all.AddRange(pwds);
                all.AddRange(aefs);
                Debug.Log(pet.pwdId);
                DoGet.getInstance().collectionAnyRes(() =>
                {
                    List<GameObject> list = readModel.drawGameObj(bg, pwds, aefs);

                    GameObject g = list[0].gameObject;
                    Transform ts0 = g.transform.GetChild(0);
                    Transform ts1 = ts0.GetChild(1);
                    if (ts1.childCount > 0)
                    {
                        Transform img = ts1.GetChild(0);
                        img.name = "icon";
                        img.localScale = 3 * Vector2.one;
                        img.transform.SetParent(bg.transform, false);
                        img.transform.localPosition = new Vector2((-243 + 24 * 3) / 2f + 10, 0);
                    }  
                   
                    gameObjPool.getInstance().free(g);
                }, all);
            }

        }

        public void updatePetXueTiao()
        {
            Transform bg = this.transform.Find("Icon/PetIcon/Bg");
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }
            JObject pet = face.petInterface.getIsFightPet();
            if (pet == null) return;
            pet["xrmf"] = face.roleInterface.getXrmf();
            pet["petEquip"] = face.roleInterface.getPetEquip();
            JObject prop = countUtils.countPetProp(pet);
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(110, 13), new Vector2(86, -52f));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(110, 13), new Vector2(86, -74f));
            drawTiao(mp, 1, prop);
        }
        public void updateRoleXueTiao()
        {
            Transform bg = this.transform.Find("Icon/ManIcon/Bg");
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }

            JObject prop = countUtils.countProp(face.roleInterface.getRole());
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(192, 13), new Vector2(38, -157.9f));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(192, 13), new Vector2(38, -179.3f));
            drawTiao(mp, 1, prop);
        }
        private void drawTiao(GameObject attr, int i, JObject prop)
        {
            Vector2 size = attr.GetComponent<RectTransform>().sizeDelta;
            string k1 = "xue";
            string k2 = "max_xue";
            Color32 color = new Color32(255, 16, 0, 255);
            Color32 color1 = new Color32(172, 11, 0, 255);
            if (i == 1)
            {
                k1 = "lan";
                k2 = "max_lan";
                color = new Color32(0, 174, 255, 255);
                color1 = new Color32(2, 140, 205, 255);
            }
            else if (i == 2)
            {
                k1 = "exp";
                k2 = "max_exp";
                color = new Color32(255, 183, 0, 255);
                color1 = new Color32(192, 137, 0, 255);
            }

            GameObject jdBg = gameObjPool.getInstance().get("jdBg", typeof(ImgUI));
            jdBg.transform.SetParent(attr.transform, false);
            jdBg.GetComponent<ImgUI>().setColor("#555555")
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));
            float rate = (float)prop[k1] / (float)prop[k2];
            if (rate > 1) rate = 1;
            GameObject jdt = gameObjPool.getInstance().get("jdt", typeof(ImgUI));
            jdt.transform.SetParent(jdBg.transform, false);
            jdt.GetComponent<ImgUI>()
                .setSizePos(new Vector2((size.x - 6) * rate, size.y - 6), new Vector2(3, -3));
            GradientDefinded gd = jdt.AddComponent<GradientDefinded>();
            gd.color1 = color;
            gd.color2 = color1;


            /*GameObject exp = gameObjPool.getInstance().get("str", typeof(TextUI));
            exp.transform.SetParent(jdBg.transform, false);
            exp.GetComponent<TextUI>().setColor("#ffffff").setAlign("right").setFontSize(24).setText(prop[k1] + "/" + prop[k2])
                .setSizePos(new Vector2(320, 30), new Vector2(0, 30));*/
        }


        private void drawTop(Transform ts)
        {
            GameObject img = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().setColor("#000000")
            .setSizePos(new Vector2(ScreenUtils.width, 90), new Vector2(0, 0));

            img = gameObjPool.getInstance().get("Title", typeof(TextUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<TextUI>().setText("Lv100-恒山村").setFontSize(30).setColor().setAlign().setFontStyle()
            .setSizePos(new Vector2(250, 90), new Vector2(ScreenUtils.width - 250, 0));
            img.AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));
        }
        /**播放任务接取或提交的动画*/
        public void playTaskGainOrSubmitAm(int type)
        {
            string[] arr = null;
            string aef=null;
            if (type == 1)
            {
                arr = new string[] { "jqrw_pwd" };
                aef = "jqrw_aef";
            }
            else if (type == 2)
            {
                arr = new string[] { "wcrw_pwd" };
                aef = "wcrw_aef";
            }
            else return;
            this.playTeXiao(PointGet.getMapPoint(), arr, aef, (am) =>
            {
                am.setCall(() => { gameObjPool.getInstance().free(am.gameObject); });
                am.playOnce();
            });
        }
        /**加载特效*/
        private void playTeXiao(Transform ts, string[] pwds, string aef, Action<AniRuntime> ac)
        {
            string[] aefs = { aef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                GameObject g = list[0].gameObject;
                g.name = "texiao";
                g.transform.SetParent(ts, false);
                AniRuntime am = g.GetComponent<AniRuntime>();
                g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();

                ac(am);
            }, all);
        }
        public override void init()
        {

        }
    }
}
