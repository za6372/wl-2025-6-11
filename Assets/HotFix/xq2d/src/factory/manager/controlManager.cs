using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.Res.script.src.touch;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    class controlManager
    {
        /**重新载入角色形象*/
        public static void reloadPlayer()
        {
            Transform ts = PointGet.getMapPoint();
            Transform me = PointGet.getControlPoint();
            JObject role = face.roleInterface.getRole();
            FightModelMsg msg = new FightModelMsg(role["model"].ToString());
            string[] pwds = msg.changguiPwds;
            string[] aefs = { msg.changguiAef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    //将这里的帧图片替换到原来那个上
                    FrameUtils.frameReplace(me.gameObject, g);
                }

            }, all);
        }

        /**绘制玩家*/
        public static void drawPlayer(Mape mape, Action ac)
        {
            Transform ts = PointGet.getMapPoint();
            JObject role = face.roleInterface.getRole();
            FightModelMsg msg = new FightModelMsg(role["model"].ToString());
            string[] pwds = msg.changguiPwds;
            string[] aefs = { msg.changguiAef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.name = "control_player";
                    g.transform.SetParent(ts, false);
                    AniRuntime am = g.GetComponent<AniRuntime>();
                    am.playOnce(3, 4);

                    g.transform.localPosition = Vector2.zero;
                    g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();



                    g.AddComponent<modelMsgBind>().addModelMsg(msg);

                    GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));
                    TextUI tx = title.GetComponent<TextUI>();
                    tx.setText(role["name"].ToString()).setColor(GameAttrConst.getColorBySf((int)role["attr"]["msg"]["sez"])).setAlign().setFontSize(35)
                    .setFontStyle().setIsRichText().horiOut().setSize(new Vector2(100, 100));
                    title.transform.SetParent(g.transform, false);
                    title.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
                    title.transform.localPosition = new Vector2(0, 50);
                    title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);

                    addVip(g.transform, 3 - role["name"].ToString().Length / 2);

                }
                ac();
            }, all);

        }
        public static void updateShenFu()
        {
            Transform g = PointGet.getControlPoint();

            JObject r = face.roleInterface.getRole();
            if (strUtils.isNull(r["attr"]["shenfu"]) || strUtils.getMillis() > (long)r["attr"]["shenfu"]["end"])
            {
                if (g.Find("shenfu") != null)
                {
                    gameObjPool.getInstance().free(g.Find("shenfu").gameObject);
                }
                return;
            }
            if (g.Find("shenfu") != null) return;

            GameObject hb = gameObjPool.getInstance().get("shenfu", typeof(ImgUI));
            hb.transform.SetParent(g, false);
            hb.GetComponent<ImgUI>().loadRes("cb_k_png")
                .setSize(new Vector2(12 * 3, 12 * 3));
            hb.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
            hb.transform.localPosition = new Vector2(-20, 60);
            DoGet.getInstance().startReqImg();
        }
        public static void addVip(Transform g, int len)
        {
            int lv = face.roleInterface.getVipLv();
            if (lv == 0) return;
            GameObject hb = gameObjPool.getInstance().get("vip", typeof(ImgUI));
            hb.transform.SetParent(g, false);
            hb.GetComponent<ImgUI>().loadRes("vip1_png")
                .setSize(new Vector2(17 * 3, 14 * 3));
            hb.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
            hb.transform.localPosition = new Vector2(-40 + 10 * len, 50);

            GameObject tx1 = gameObjPool.getInstance().get("shuzhi", typeof(TextImgUI));
            tx1.transform.SetParent(hb.transform, false);
            tx1.GetComponent<TextImgUI>().setSize(new Vector2(17 * 3, 22));
            tx1.GetComponent<TextImgUI>().setBase(new Vector2Int(5, 7), "0123456789-+", 22).loadRes("vipzi_png").setText(lv + "");
            tx1.transform.localPosition = new Vector2(15, -12);
        }

        /**刷新橙装特效*/
        public static void updateCzTx()
        {
            Transform ts = PointGet.getControlPoint();
            int lv = face.equipInterface.getGoldType();
            Transform a = ts.Find("cztx");
            if (a != null) gameObjPool.getInstance().free(a.gameObject);
            string tx = null;
            if (lv == -1)
            {
                return;
            }
            else if (lv == 0) tx = "cztx1";
            else if (lv == 1) tx = "cztx2";
            else if (lv == 2) tx = "cztx3";
            string[] pwds = new string[1] { tx + "_pwd" };
            string[] aefs = { tx + "_aef" };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                GameObject g = list[0].gameObject;
                g.name = "cztx";
                g.transform.SetParent(ts, false);
                AniRuntime am = g.GetComponent<AniRuntime>();
                //g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();
                am.play();
            }, all);
        }
        /**切换宠物时刷新模型*/
        public static void updatePetModel()
        {
            JObject petMsg = face.petInterface.getIsFightPet();
            if (petMsg == null || !face.petInterface.isWaiXianPet(petMsg["key"].ToString()))
            {
                Transform a = PointGet.getControlPetPoint();
                if (a != null)
                {
                    gameObjPool.getInstance().free(a.gameObject);
                }
                return;
            }
            //pet节点放入control中
            Transform pts = PointGet.getControlPetPoint();
            if (pts == null)
            {
                //不存在pet节点就创建一个
                drawPet(() =>
                {
                    updatePetPos();
                });
                return;
            }


            //存在pet节点就替换帧即可
            Transform ts = PointGet.getControlPoint();
            Pet pet = face.petInterface.getPetDataByKey(petMsg["key"].ToString());
            string pwd = pet.getIdlePwd((int)petMsg["growBreachLv"]);
            AmModelMsg am = amManager.getAmByKey(pwd);
            string[] aefs = { am.changguiAef };
            string[] pwds = am.changguiPwds;

            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    FrameUtils.frameReplace(pts.gameObject, g);
                }
                updatePetPos();
            }, all);
        }
        /**更新宠物位置*/
        public static void updatePetPos()
        {
            Transform pet = PointGet.getControlPetPoint();
            if (pet == null) return;
            Move mv = PointGet.getMapPoint().GetComponent<Move>();
            Vector2 dir = mv.getDir();
            int len = pet.transform.Find("frames").childCount;
            AniRuntime am = pet.GetComponent<AniRuntime>();
            if (dir.x == 1)//右移动
            {
                pet.localPosition = new Vector2(-150 / GameAttrConst.getMapScaleRate(), 0);
                am.play(len / 2, len);
            }
            else
            {
                pet.localPosition = new Vector2(150 / GameAttrConst.getMapScaleRate(), 0);
                am.play(0, len / 2);
            }
            //移动到父组件下所有子组件的前端
            pet.SetAsFirstSibling();
        }
        /**绘制宠物*/
        private static void drawPet(Action ac)
        {
            Transform ts = PointGet.getControlPoint();

            JObject petMsg = face.petInterface.getIsFightPet();
            if (petMsg == null || !face.petInterface.isWaiXianPet(petMsg["key"].ToString()))
            {
                ac();
                return;
            }
            Pet pet = face.petInterface.getPetDataByKey(petMsg["key"].ToString());
            string pwd = pet.getIdlePwd((int)petMsg["growBreachLv"]);
            Debug.Log("================="+pwd);
            AmModelMsg am = amManager.getAmByKey(pwd);
            string[] aefs = { am.changguiAef };
            string[] pwds = am.changguiPwds;

            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.name = "pet";
                    g.transform.SetParent(ts, false);
                    AniRuntime am = g.GetComponent<AniRuntime>();
                    am.frameInterval = 0.25f;
                    int len = g.transform.childCount;
                    am.play(0, len / 2);

                    g.transform.localPosition = Vector2.zero;


                }
                ac();
            }, all);

        }
    }
}
