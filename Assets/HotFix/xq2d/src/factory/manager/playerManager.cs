using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.fight;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.touch;
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
    public class playerManager
    {
        private static playerManager mg;
        //是否允许加载玩家
        private bool allowedLoad = false;
        public static playerManager getInstance()
        {
            if (mg == null)
            {
                mg = new playerManager();
            }
            return mg;
        }
        public static List<GameObject> getPlayers()
        {
            List<GameObject> list = new List<GameObject>();
            Transform ts = PointGet.getMapPoint();
            for (int i = 0; i < ts.childCount; i++)
            {
                Transform a = ts.GetChild(i);
                PlayerMove md = a.GetComponent<PlayerMove>();
                if (md != null)
                {
                    list.Add(a.gameObject);
                }
            }
            return list;
        }

        private GameObject getCreateObj(string name)
        {
            List<GameObject> players = getPlayers();
            foreach (object p in players)
            {
                GameObject pr = (GameObject)p;
                if (pr != null && pr.name.Equals(name))
                {
                    return pr;
                }
            }
            return null;
        }
        public void setAllowedLoad(bool b)
        {
            this.allowedLoad = b;
        }
        /**重新加载*/
        public void reLoad()
        {
            face.playerInterface.initPlayerPos();
            this.allowedLoad = true;
            this.loadPlayers();
        }

        public void keepDis()
        {
            Transform ts = PointGet.getMapMapePoint();
            List<GameObject> players = getPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                GameObject m = players[i];
                if (m == null || m.GetComponent<PlayerMove>() == null) continue;
                Vector2 dis = m.GetComponent<PlayerMove>().getDis();
                m.transform.position = new Vector2(ts.position.x + dis.x, ts.position.y + dis.y);
            }
        }
        /**拿到玩家位置数据后进行处理*/
        public void handlePlayerPos(JArray msg)
        {
            //Debug.Log(msg);
            //缓存当前地图玩家位置
            face.playerInterface.receivePlayersPos(msg);
            if (fightCache.getInstance().isDoing) return;
            //跟随队长时，发现队长跟自己不在一个地图就跳转
            if (face.teamInterface.isInTeam()
                && (int)face.teamInterface.getMeFromTeam()["isFollow"] == 1 &&
                !face.teamInterface.isCaptain())
            {
                //处于队伍中执行
                Action fn = () =>
                {
                    //更新玩家位置
                    this.loadPlayers();
                    //如果是组队的话需要找到队长的位置并执行跟随
                    face.teamInterface.findCaptainFromPlayer(() =>
                    {
                        playerManager.getInstance().followCaptain();
                    });
                };
                JObject a = face.teamInterface.isInSameMapKey();
                if (!(bool)a["isSame"])
                {
                    //注意：队长跳转地图时服务端已经将初始位置设置好了，并通过ws812缓存了队长的位置信息

                    //face.mapInterface.setToMovePos();
                    //处于不同地图时就要跳转地图
                    mapManager.getInstance().reloadBef(a["mapKey"].ToString());

                    return;
                }

                fn();

            }
            else
            {
                //不在队伍中就更新玩家位置
                this.loadPlayers();
            }
        }
        
        /**刷新橙装特效*/
        public static void updateCzTx(Transform ts, JObject player)
        {
            Transform a = ts.Find("cztx");
            if (a != null) gameObjPool.getInstance().free(a.gameObject);
            int lv = player.ContainsKey("goldType") ? int.Parse(player["goldType"].ToString()) : -1;
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
        /**加载显宠*/
        public void loadPet(GameObject pr, JObject petMsg)
        {
            //走路总共8帧，前3帧左走，第4帧静止，后四帧右走（跟左走一致）
            Transform ts = pr.transform;
            if (petMsg == null || !face.petInterface.isWaiXianPet(petMsg["key"].ToString()))
            {
                return;
            }
            Pet pet = face.petInterface.getPetDataByKey(petMsg["key"].ToString());
            string pwd = pet.getIdlePwd((bool)petMsg["isX8"]);
            AmModelMsg am = amManager.getAmByKey(pwd);
            string[] aefs = { am.changguiAef };
            string[] pwds = am.changguiPwds;
            /*FightModelMsg msg = new FightModelMsg(pet["key"].ToString());
            string[] pwds = msg.changguiPwds;
            string[] aefs = { msg.changguiAef };*/
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.name = "player_pet";
                    g.transform.SetParent(ts, false);
                    AniRuntime am = g.GetComponent<AniRuntime>();
                    am.frameInterval = 0.25f;
                    int len = g.transform.Find("frames").childCount;
                    am.play(0, len / 2);
                    g.transform.localPosition = new Vector2(150 / GameAttrConst.getMapScaleRate(), 0);
                    //g.AddComponent<modelMsgBind>().addModelMsg(msg);
                    g.transform.SetAsFirstSibling();
                }
            }, all);
        }
        /**载入当前地图玩家*/
        public void loadPlayers()
        {
            if (!this.allowedLoad || PointGet.getMapPoint() == null || PointGet.getMapMapePoint() == null) return;
            this.allowedLoad = false;
            JArray list = face.playerInterface.getPlayerList();
            //Debug.Log(list);"pet": {"key": "1222", "isX8": false}

            //将不出现在新数据中的旧玩家给移除
            this.removeOld(list);
            Transform ts = PointGet.getMapPoint();
            //加载新数据的玩家
            loadOne(list, 0, ts, () =>
            {
                this.allowedLoad = true;
            });
        }
        private void loadOne(JArray list, int index, Transform ts, Action ac)
        {
            if (index >= list.Count)
            {
                ac();
                return;
            }
            JObject j = (JObject)list[index];
            GameObject pr = this.getCreateObj((string)j["name"]);
            //收到的位置是相对于mape的位置
            JObject e = (JObject)j["pos"];
            if (e == null)
            {
                index++;
                loadOne(list, index, ts, ac);
                return;
            }
            //Debug.Log(j);
            Vector2 vs = PosCountUtils.relativeMapePosToControlPosition(new Vector2((float)e["x"], (float)e["y"]));
            if (pr != null)
            {
                if (pr.GetComponent<PlayerMove>() == null)
                {
                    index++;
                    loadOne(list, index, ts, ac);
                    return;
                }
                //拿到的位置是对方相对于mape的位置
                pr.GetComponent<PlayerMove>().inputPos(vs);
                index++;
                loadOne(list, index, ts, ac);
            }
            else
            {
                //不存在就创建
                string pName = j["name"].ToString();


                FightModelMsg msg = new FightModelMsg(j["model"].ToString());
                string[] pwds = msg.changguiPwds;
                string[] aefs = { msg.changguiAef };
                List<string> all = new List<string>();
                all.AddRange(pwds);
                all.AddRange(aefs);
                DoGet.getInstance().collectionAnyRes(() =>
                {
                    List<GameObject> al = readModel.drawGameObj(ts, pwds, aefs);

                    for (int i = 0; i < al.Count; i++)
                    {
                        GameObject g = al[i].gameObject;
                        g.name = pName;
                        g.transform.SetParent(ts, false);
                        AniRuntime am = g.GetComponent<AniRuntime>();

                        am.playOnce(3, 4);

                        g.transform.position = vs;
                        g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();

                        g.AddComponent<PlayerMove>();

                        GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));
                        TextUI tx = title.GetComponent<TextUI>();
                        tx.setText(pName).setColor(GameAttrConst.getColorBySf((int)j["sez"])).setAlign().setFontSize(35)
                        .setFontStyle().setIsRichText().horiOut().setSize(new Vector2(100, 100));
                        title.transform.SetParent(g.transform, false);
                        title.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
                        title.transform.localPosition = new Vector2(0, 50);
                        title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);

                        loadPet(g, (JObject)j["pet"]);
                        updateCzTx(g.transform, j);

                        addVip((int)j["vip"],g.transform, 3 - pName.Length / 2);

                        updateShenFu(g.transform, j);
                    }
                    index++;
                    loadOne(list, index, ts, ac);

                }, all);

            }
        }
        public static void addVip(int vipExp, Transform g, int len)
        {
            int lv = face.roleInterface.getVipLv(vipExp);
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
        public static void updateShenFu(Transform g, JObject player)
        {

            if(strUtils.isNull(player["shenfu"]) || strUtils.getMillis() > (long)player["shenfu"]["end"])
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
        /**移除地图已经不存在的
         list当前地图所存在的
         */
        public void removeOld(JArray list)
        {
            List<GameObject> players = getPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                //判断当前场景玩家是否存在在新数据中
                GameObject m = players[i];
                if (m != null)
                {
                    if (m.GetComponent<PlayerMove>() == null)
                    {
                        gameObjPool.getInstance().free(m);
                        players.RemoveAt(i);
                        i--;
                        continue;
                    }
                    bool b = false;
                    foreach (object p in list)
                    {
                        JObject a = (JObject)p;
                        if (((string)a["name"]).Equals(m.name))
                        {
                            b = true;
                            break;
                        }
                    }
                    //不存在则移除
                    if (!b)
                    {
                        gameObjPool.getInstance().free(m);
                        players.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    players.RemoveAt(i);
                    i--;
                }


            }
        }
        /**跟随队长*/
        public void followCaptain()
        {
            //Vector3 mapePos = PointGet.getMapMapePoint().position;
            JObject pos = face.teamInterface.getCaptainPos();
            JObject rolePos = (JObject)face.roleInterface.getRole()["pos"];
            //因为队长位置是相对于mape的，所以要转成position
            Vector2 vs = PosCountUtils.relativeMapePosToControlPosition(new Vector2((float)pos["pos"]["x"], (float)pos["pos"]["y"]));
            if (pos["map"].ToString().Equals(rolePos["map"].ToString()))
            {
                Transform map = PointGet.getMapPoint();
                if (map != null)
                {
                    Move mv = map.GetComponent<Move>();
                    mv.putTargetPos(vs);
                }

            }
        }


        /**根据实例id获取该对象*/
        public GameObject getNpcById(int id)
        {
            List<GameObject> players = getPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetInstanceID() == id)
                {
                    return players[i];
                }
            }
            return null;
        }



    }
}
