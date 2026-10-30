using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.model;
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
    class CreateRolePage : PageUI
    {
        private string job;

        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("创建角色");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
                PageUI.create<ChooseServerPage>().drawUI();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            JObject body = netUtils.getInstance().body;
            Debug.Log(body);
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, 300), new Vector2(30, -30), content);

            size = k1.getContent().GetComponent<RectTransform>().sizeDelta;

            GameObject nameBg = gameObjPool.getInstance().get("nameBg", typeof(ImgUI));
            nameBg.transform.SetParent(k1.getContent(), false);
            nameBg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, 150), new Vector2(20, -75));
            nameBg.AddComponent<UIOutline>().setSome(new Color32(12, 168, 148, 255), new Vector2(5, -5));

            GameObject text1 = gameObjPool.getInstance().get("text1", typeof(TextUI));
            text1.transform.SetParent(nameBg.transform, false);
            text1.GetComponent<TextUI>()
                .setText("昵称：").setAlign().setFontSize().setFontStyle().setColor("#0CA894")
            .setSizePos(new Vector2(200, 100), new Vector2(0, -25));

            GameObject username = gameObjPool.getInstance().get("username", typeof(InputUI));
            username.transform.SetParent(nameBg.transform, false);
            username.GetComponent<InputUI>().setSizePos(new Vector2(size.x - 40 - 400, 100), new Vector2(200, -25));
            username.GetComponent<InputUI>().initSetting("请输入昵称").addMatchSimple().setContent("").setRoundedCorners(0.5f);

            text1 = gameObjPool.getInstance().get("text1", typeof(TextUI));
            text1.transform.SetParent(nameBg.transform, false);
            text1.GetComponent<TextUI>()
                .setText("进入游戏").setAlign().setFontSize().setFontStyle().setColor("#0CA894")
            .setSizePos(new Vector2(200, 100), new Vector2(size.x - 40 - 200, -25)).addClk(()=> {
                //验证
                if (this.job == null)
                {
                    msgCode.showMsg(929);
                    return;
                }
                if (!strUtils.isMatch(username.GetComponent<InputUI>().getContent(), "[\u4e00-\u9fa5_a-zA-Z0-9_]{1,6}"))
                {
                    msgCode.showMsg(879);
                    return;
                }
                text1.gameObject.SetActive(false);

                face.roleInterface.createRole(username.GetComponent<InputUI>().getContent(), job, (res) =>
                {
                    //secret在登录时已经赋值，所以这里不用再获取
                    JObject data = (JObject)res;
                    netUtils.getInstance().sbh = data["sbh"].ToString();
                    netUtils.getInstance().body = (JObject)data["body"];
                    clkMan();
                });
            });


            size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject bg2 = gameObjPool.getInstance().get("bg2", typeof(ImgUI));
            bg2.transform.SetParent(content.transform, false);
            bg2.GetComponent<ImgUI>().loadRes("dl_3_png", Image.Type.Tiled)
            .setSizePos(new Vector2(size.x, size.y - 360 - 300), new Vector2(0, -360));

            GameObject heads = gameObjPool.getInstance().get("heads", typeof(SimpleUI));
            heads.transform.SetParent(content.transform, false);
            heads.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x, 400), new Vector2(0, -320));

            string[] arr = { "001", "011", "021", "002", "012", "022" };
            for (int i = 0; i < 6; i++)
            {
                int index = i;
                float x = (size.x) / 3f * (i % 3) + ((size.x) / 3f - 156) / 2f;
                float y = -(i / 3) * 200;
                GameObject item = gameObjPool.getInstance().get("item", typeof(ImgUI));
                item.transform.SetParent(heads.transform, false);
                item.GetComponent<ImgUI>()
                .setSizePos(new Vector2(156, 164), new Vector2(x, y)).addClk(() =>
                {
                    for (int j = 0; j < heads.transform.childCount; j++)
                    {
                        Transform it = heads.transform.GetChild(j);
                        if (j == index)
                        {
                            it.GetComponent<ImgUI>().loadRes("dl_0_png");
                            it.Find("icon").GetComponent<ImgUI>().loadRes(arr[j] + "_png");
                        }
                        else
                        {
                            it.GetComponent<ImgUI>().loadRes("dl_0_f_png");
                            it.Find("icon").GetComponent<ImgUI>().loadRes(arr[j] + "_f_png");
                        }
                        string[] jobs = { "xs_nan_t1", "xs_nan_t2", "xs_nan_t3",
                            "xs_nv_t1","xs_nv_t2","xs_nv_t3" };
                        this.job = jobs[index];
                    }

                    DoGet.getInstance().startReqImg();
                    string ky = "xs_nan";
                    if (index > 2) ky = "xs_nv";
                    //绘制人物形象
                    if (content.transform.Find("control_player") != null)
                    {
                        gameObjPool.getInstance().free(content.transform.Find("control_player").gameObject);
                    }
                    AmModelMsg msg = amManager.getAmByKey(ky);

                    string hd = "xs_nan_t1_pwd";
                    if (index == 0) hd = "xs_nan_t1_pwd";
                    else if (index == 1) hd = "xs_nan_t2_pwd";
                    else if (index == 2) hd = "xs_nan_t3_pwd";
                    else if (index == 3) hd = "xs_nv_t1_pwd";
                    else if (index == 4) hd = "xs_nv_t2_pwd";
                    else if (index == 5) hd = "xs_nv_t3_pwd";
                    amManager.changeManHeadPwd(hd, msg);

                    string[] pwds = msg.changguiPwds;
                    string[] aefs = { msg.changguiAef };
                    List<string> all = new List<string>();
                    all.AddRange(pwds);
                    all.AddRange(aefs);
                    DoGet.getInstance().collectionAnyRes(() =>
                    {
                        List<GameObject> list = readModel.drawGameObj(item.transform, pwds, aefs);

                        for (int i = 0; i < list.Count; i++)
                        {
                            GameObject g = list[i].gameObject;
                            g.name = "control_player";
                            g.transform.SetParent(content.transform, false);
                            AniRuntime am = g.GetComponent<AniRuntime>();

                            am.playOnce(3, 4);

                            g.transform.localPosition = new Vector2(0, -280);
                            g.transform.localScale = Vector2.one * 5;

                        }

                    }, all);
                }, false);

                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(item.transform, false);
                icon.GetComponent<ImgUI>().setNoClk()
                .setSizePos(new Vector2(100, 100), new Vector2(28, -32));
            }

            heads.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();



            GameObject desBg = gameObjPool.getInstance().get("desBg", typeof(ImgUI));
            desBg.transform.SetParent(content.transform, false);
            desBg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, 300), new Vector2(20, -size.y + 320));
            desBg.AddComponent<UIOutline>().setSome(new Color32(12, 168, 148, 255), new Vector2(5, -5));
        }

        /**载入角色*/
        private void clkMan()
        {
            JObject role = netUtils.getInstance().body;
            string mapKey = "m_1";
            
            JObject pos = null;
            JObject r = face.roleInterface.getRole();
            if (r["pos"] != null && r["pos"]["map"] != null)
            {
                //将旧的位置取出
                MapData a = face.mapInterface.getMapByKey(r["pos"]["map"].ToString());
                mapKey = r["pos"]["map"].ToString();
                if (!strUtils.isNull(r["pos"]["pos"])) pos = (JObject)r["pos"]["pos"];
            }
            if (netUtils.getInstance().body["mzzd"].ToString().Equals("0")) mapKey = "sgzc";
            //记录地图、位置
            role["pos"] = new JObject();
            role["pos"]["map"] = mapKey;
            role["pos"]["pos"] = pos;
            face.roleInterface.initRolePos(mapKey, pos);
            face.roleInterface.saveRole(role);

            //载入信息
            face.roleInterface.loadData(() =>
            {

                this.freeThisPage();
                PageUI.create<IndexPage>().drawUI();

            });


        }
        public override void init()
        {

        }
    }
}
