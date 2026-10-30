using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.data;
using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class ChooseRolePage : PageUI
    {

        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("选择角色");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
                PageUI.create<ChooseServerPage>().drawUI();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            drawK2(content);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            JObject body = netUtils.getInstance().body;
            //Debug.Log(body);
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, 300), new Vector2(30, -30), content);

            size = k1.getContent().GetComponent<RectTransform>().sizeDelta;



            if (body == null)
            {
                GameObject text1 = gameObjPool.getInstance().get("text1", typeof(TextUI));
                text1.transform.SetParent(k1.getContent(), false);
                text1.GetComponent<TextUI>()
                    .setText("点击创建角色").setAlign().setFontSize().setFontStyle().setColor("#0CA894")
                .setSizePos(new Vector2(size.x, 300), new Vector2(0, -0)).addClk(() =>
                {
                    this.freeThisPage();
                    PageUI.create<CreateRolePage>().drawUI();
                });
                return;
            }
            GameObject item = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item.transform.SetParent(k1.getContent(), false);
            item.GetComponent<ImgUI>().loadRes("dl_0_png")
            .setSizePos(new Vector2(156, 164), new Vector2(80, -(size.y - 164) / 2));

            GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item.transform, false);
            icon.GetComponent<ImgUI>().loadRes(GameAttrConst.getRoleHead(body) +"_png")
            .setSizePos(new Vector2(120, 120), new Vector2(28, -22));

            GameObject text = gameObjPool.getInstance().get("name", typeof(TextUI));
            text.transform.SetParent(k1.getContent(), false);
            text.GetComponent<TextUI>().setColor(PageUI.PageDefaltColor)
                .setText("昵称：" + body["name"]).setAlign("left").setFontSize().setFontStyle().setColor("#0CA894")
            .setSizePos(new Vector2(300, 80), new Vector2(250, -30));

            text = gameObjPool.getInstance().get("lv", typeof(TextUI));
            text.transform.SetParent(k1.getContent(), false);
            text.GetComponent<TextUI>().setColor(PageUI.PageDefaltColor)
                .setText("等级：" + body["lever"]).setAlign("left").setFontSize().setFontStyle().setColor("#0CA894")
            .setSizePos(new Vector2(300, 80), new Vector2(250, -30 - 80));

            text = gameObjPool.getInstance().get("job", typeof(TextUI));
            text.transform.SetParent(k1.getContent(), false);
            text.GetComponent<TextUI>().setColor(PageUI.PageDefaltColor)
                .setText("分堂：" + GameAttrConst.getJobToName(body["model"].ToString())).setAlign("left").setFontSize().setFontStyle().setColor("#0CA894")
            .setSizePos(new Vector2(300, 80), new Vector2(250, -30 - 80 * 2));

            string pic = "zy_1";
            if (body["model"].ToString().Contains("ms_") || body["model"].ToString().Contains("dj_"))
            {
                pic = "zy_1";
            }
            else if (body["model"].ToString().Contains("qm_") || body["model"].ToString().Contains("ty_"))
            {
                pic = "zy_2";
            }
            else if (body["model"].ToString().Contains("ym_") || body["model"].ToString().Contains("lc_"))
            {
                pic = "zy_3";
            }
            item = gameObjPool.getInstance().get("t2", typeof(ImgUI));
            item.transform.SetParent(k1.getContent(), false);
            item.GetComponent<ImgUI>().loadRes(pic + "_png")
            .setSizePos(new Vector2(163, 164), new Vector2(size.x - 163 - 80, -(size.y - 164) / 2));
        }
        private void drawK2(Transform content)
        {
            JObject body = netUtils.getInstance().body;
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(content, false);
            mask.GetComponent<ImgUI>().setColor("#BEDECA")
            .setSizePos(new Vector2(size.x, size.x / 240f * 196), new Vector2(0, -(size.y - 360 - (size.x / 240f * 196)) / 2f - 360));
            mask.AddComponent<RectMask2D>().softness = new Vector2Int(100, 100);

            GameObject item = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            item.transform.SetParent(mask.transform, false);
            item.GetComponent<ImgUI>().loadRes("b_png")
            //.setSizePos(new Vector2(size.x, size.x / 240f * 196), new Vector2(0, -360));
            .setSizePos(new Vector2(size.x, size.x / 240f * 196), new Vector2(0, 0));

            size = item.GetComponent<RectTransform>().sizeDelta;

            

            if (body == null) return;

            //坐台
            this.playTeXiao(item.transform, new string[] { "sl_pwd" }, "sl_aef", (am1) =>
            {
                am1.gameObject.transform.localPosition = new Vector2(0, -80);
                //am.setCall(() => { gameObjPool.getInstance().free(am.gameObject); });
                am1.playOnce(0, 1);

                FightModelMsg msg = new FightModelMsg(body["model"].ToString());
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
                        g.transform.SetParent(item.transform, false);
                        AniRuntime am = g.GetComponent<AniRuntime>();

                        am.playOnce(3, 4);

                        /*BindBaseMsg data = g.GetComponent<BindBaseMsg>();
                        Vector2 size = data.size;*/
                        //Vector2 sizeDelta = g.GetComponent<RectTransform>().sizeDelta;
                        //SetGameObj.setLeftPos(new Vector2((size.x - sizeDelta.x) / 2 + 20, -(size.y - sizeDelta.x) / 2 + 20), g);
                        g.transform.localPosition = new Vector2(0, -80);
                        g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();
                        Action ac = () =>
                        {
                            am1.setCall(() => { clkMan(body); });
                            am1.playOnce();
                        };
                        SetGameObj.addClk(g, () =>
                        {
                            SetGameObj.addClk(g, () => { });
                            ac();
                        });
                    }

                }, all);
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
        /**载入角色*/
        private void clkMan(JObject role)
        {
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
