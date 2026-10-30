using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.fight
{
    class playerOperate
    {
        private static playerOperate one;
        private List<GameObject> players = new List<GameObject>();
        public static playerOperate getInstance()
        {
            if (one == null)
            {
                one = new playerOperate();
            }

            return one;
        }
        public playerOperate()
        {

        }
        public void clear()
        {
            this.players.Clear();
        }
        /**将帧同步，每次执行下一个动作之前同步一次，待命阶段同步一次*/
        public void setSameFrame()
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameObject a = players[i];
                AniRuntime am = a.GetComponent<AniRuntime>();
                if (am == null) continue;
                am.resetFrame();
            }
            //Debug.Log("===重置帧===");
        }
        public void setNoTouch()
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameObject a = players[i];
                modelMsgBind mb = a.GetComponent<modelMsgBind>();
                mb.isTouch = false;
                a.transform.Find("touchKuai").gameObject.SetActive(false);
            }
        }
        public void setAllowedTouch(int target, bool isRole)
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameObject a = players[i];
                modelMsgBind mb = a.GetComponent<modelMsgBind>();
                //战斗玩家、怪物模型必然带有modelMsgBind，没有就是灯光之类的玩意
                if (mb == null) continue;
                if ((target == 2 && fightCache.getInstance().isFriend(mb.msg.posKey)) ||
                    (target == 1 && !fightCache.getInstance().isFriend(mb.msg.posKey)) ||
                    (target == 0 && !fightCache.getInstance().isRolePetPosKey(isRole, mb.msg.posKey)))
                {
                    //目标为敌方则去掉友方；目标为友方则去掉敌方;目标为自己则去掉除自己意外的
                    mb.isTouch = false;
                    a.transform.Find("touchKuai").gameObject.SetActive(false);
                    continue;
                }
                mb.isTouch = true;
                a.transform.Find("touchKuai").gameObject.SetActive(true);
            }
        }
        /**将名称隐藏*/
        public void visibleNames(bool b)
        {
            foreach (GameObject a in players)
            {
                if (a != null)
                {
                    a.transform.Find("name").gameObject.SetActive(b);
                    //隐藏状态图标
                    a.transform.Find("buffArea").gameObject.SetActive(b);
                }
            }
        }
        /**将玩家隐藏*/
        public void visiblePlayer(string posKey, bool b)
        {
            modelMsgBind p = getPlayer(posKey);
            if (p != null) p.gameObject.SetActive(b);
        }
        /**由站位获取玩家*/
        public modelMsgBind getPlayer(string posKey)
        {
            foreach (GameObject a in players)
            {
                modelMsgBind p = a.GetComponent<modelMsgBind>();
                if (p.msg.posKey.Equals(posKey)) return p;
            }
            return null;
        }
        public void loadList(JArray list, Action ac)
        {
            loadList(0, list, () => { ac(); });
        }
        private void loadList(int i, JArray list, Action ac)
        {
            if (i >= list.Count)
            {
                ac();
                return;
            }
            JObject obj = (JObject)list[i];
            loadOne(obj, () =>
            {
                i++;
                loadList(i, list, ac);
            });
        }
        /**加载模型*/
        private void loadOne(JObject obj, Action ac)
        {
            //站位-自上而下 05 16 27 38 49
            string posKey = obj["posKey"].ToString();
            int type = (int)obj["type"];

            string model = obj["model"].ToString();
            JObject shenfu = (JObject)obj["shenfu"];
            if (shenfu != null && strUtils.getMillis() < (long)shenfu["end"])
            {
                ShenFu sf = (ShenFu)face.goodsInterface.getGoodsMsgByKey(shenfu["sf_key"].ToString());
                model = sf.petKey;
            }

            /*float direction = 0;
            if (type != 0) direction = 180;*/
            float direction = 180;
            string tag = "friend";
            if (posKey.Contains("l"))
            {
                /*direction = 180f;
                if (type != 0) direction = 0;*/
                direction = 0;
                tag = "foe";
            }
            string name = obj["name"].ToString();
            string color = tag.Equals("friend") ? "#ffffff" : "#FFE200";
            //0人物 1宠物 2怪物 4ai 5伙伴
            if (type == 1 || type == 5)
            {
                name = obj["nickName"].ToString();
            }
            else if (type == 2)
            {
                //替换怪物名
                JObject a = face.monsterInterface.getMonster(obj["key"].ToString());
                name = a["name"].ToString();
                if (obj.ContainsKey("quality"))
                {
                    if ((int)obj["quality"] == -1)
                    {
                        name = "[精]" + name;
                        color = "#E49200";//金黄
                    }
                    else if ((int)obj["quality"] == 2)
                    {
                        name = "[宝]" + name;
                        color = "#6EFF00";//绿色
                    }
                    else if ((int)obj["quality"] == 3)
                    {
                        name = "[异]" + name;
                        color = "#00ECFF";//蓝色
                    }
                    else if ((int)obj["quality"] == 4)
                    {
                        color = "#FF0800";//红色
                    }
                }
            }
            //Debug.Log("======================");
            //Debug.Log(obj);

            //这里会解析出动画、模型资源路径

            FightModelMsg pl = new FightModelMsg(model);

            pl.addTag(tag);
            pl.addRoleType(type);

            //Debug.Log(strUtils.copyJSON<JArray>(pl.zhandouPwds));
            //Debug.Log(pl.zhandouAef);


            string[] pwds = pl.zhandouPwds;
            string[] aefs = { pl.zhandouAef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                Transform ts = PointGet.getModelLayer();
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                GameObject g = list[0].gameObject;

                Transform frames = g.transform.Find("frames");
                //对于人物需要改变方向
                if (model.Contains("_nan") || model.Contains("_nv"))
                {
                    /*for (int i = 0; i < g.transform.childCount; i++)
                    {
                        g.transform.GetChild(i).localRotation = Quaternion.Euler(0, 180, 0);
                    }*/
                    frames.localRotation = Quaternion.Euler(0, 180, 0);
                }

                g.name = name;
                g.transform.SetParent(ts.transform, false);

                //Vector2 size = g.GetComponent<RectTransform>().sizeDelta;
                Vector2 pos = playerOperate.getInstance().getPosByPosKey(posKey);
                //x取中心位置，y取底部
                //SetGameObj.setLeftPos(new Vector2(pos.x + 200 - size.x, pos.y + (size.y - 200)), g);
                g.transform.localPosition = pos;
                g.transform.localScale = Vector2.one * GameAttrConst.getMapScaleRate();
                g.transform.localRotation = Quaternion.Euler(0, direction, 0);
                //todo:需要将y上移对齐，如果发生翻转要左对齐

                pl.posKey = posKey;
                g.AddComponent<modelMsgBind>().addModelMsg(pl);
                //旋转180度会导致无法点击ui的情况，所以要关闭忽略图形背面无法点击
                g.AddComponent<GraphicRaycaster>().ignoreReversedGraphics = false;

                playByStatus(g, 1);

                //橙装特效
                updateCzTx(g.transform, obj);

                GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));
                TextUI tx = title.GetComponent<TextUI>();
                tx.setText(name).setColor(color).setAlign().setFontSize(35)
                .setFontStyle().setIsRichText().horiOut().horiOut().setSize(new Vector2(100, 100));
                title.transform.SetParent(g.transform, false);
                title.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
                title.transform.localPosition = new Vector2(0, 50);
                title.transform.localRotation = Quaternion.Euler(0, direction, 0);
                title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);

                //buff状态图标区
                addBuffStatus(g.transform);

                //选目标触摸的块
                addTouchKuai(g.transform);


                DoGet.getInstance().startReqImg();

                players.Add(g);

                /*Transform touchParent = PointGet.getTouchLayer();
                GameObject obj = gameObjPool.getInstance().get(name, typeof(ImgUI));
                obj.transform.SetParent(touchParent, false);
                obj.GetComponent<ImgUI>().setColor(1, 1, 1, 0.2f).setSize(new Vector2(200, 200)).addClk(() =>
                       {
                           modelMsgBind mb = g.GetComponent<modelMsgBind>();
                           if (!mb.isTouch) return;
                           orderOperate.getInstance().putTarget(mb.msg.posKey);
                           Debug.Log(mb.msg.posKey);
                       });
                obj.transform.localPosition = pos;*/
                ac();
            }, all);
        }
        /**移除过期的buff状态图标*/
        public void clearBuffStatusIcon(Transform ts, int statusType)
        {
            Transform buffArea = ts.Find("buffArea");
            //已经显示了该buff图标则不再创建
            Transform a = buffArea.Find("bf_" + statusType);
            if (a == null) return;
            gameObjPool.getInstance().free(a.gameObject);
        }
        /**将buff图标显示出来*/
        public void showBuffStatusIcon(Transform ts, int statusType, int buffNowAddNum)
        {
            Transform buffArea = ts.Find("buffArea");
            //已经显示了该buff图标则不再创建
            if (buffArea.Find("bf_" + statusType) != null)
            {
                //Debug.Log(statusType + "-----showBuffStatusIcon----" + buffNowAddNum);
                if (buffNowAddNum >= 1 && buffArea.Find("bf_" + statusType).Find("text") != null)//刷新叠加层数
                {
                    buffArea.Find("bf_" + statusType).Find("text").GetComponent<TextUI>().setText(buffNowAddNum.ToString());
                }
                return;
            }
            string png = null;
            if (statusType == 4) png = "jn_006_png";
            else if (statusType == 5) png = "jn_023_png";
            else if (statusType == 9) png = "jn_003_png";
            else if (statusType == 10) png = "jn_004_png";
            else if (statusType == 49) png = "jn_002_png";
            else if (statusType == 52) png = "jn_012_png";
            else if (statusType == 53) png = "jn_017_png";
            else return;

            GameObject icon = gameObjPool.getInstance().get("bf_" + statusType, typeof(ImgUI));
            icon.transform.SetParent(buffArea.transform, false);
            icon.GetComponent<ImgUI>().loadRes(png)
            .setSize(new Vector2(20, 20));
            icon.transform.localPosition = new Vector2(22 * buffArea.childCount, 0);
            if (buffNowAddNum >= 1)
            {
                //显示层数
                GameObject title = gameObjPool.getInstance().get("text", typeof(TextUI));
                TextUI tx = title.GetComponent<TextUI>();
                tx.setText(buffNowAddNum.ToString()).setColor("#ffffff").setAlign().setFontSize(15)
                .setFontStyle().setIsRichText().horiOut().horiOut().setSize(new Vector2(20, 20));
                title.transform.SetParent(icon.transform, false);
                title.transform.localPosition = new Vector2(0, 0);
                title.transform.localRotation = Quaternion.Euler(0, ts.transform.localRotation.eulerAngles.y, 0);
                title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);
            }
        }
        /**显示buff状态的区域*/
        private void addBuffStatus(Transform ts)
        {
            GameObject top = gameObjPool.getInstance().get("buffArea", typeof(SimpleUI));
            top.transform.SetParent(ts.transform, false);
            top.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
            top.transform.localPosition = new Vector2(0, 60);


        }
        /**选择目标对象时的触摸块*/
        public void addTouchKuai(Transform ts)
        {
            GameObject top = gameObjPool.getInstance().get("touchKuai", typeof(ImgUI));
            top.transform.SetParent(ts.transform, false);
            top.GetComponent<ImgUI>().loadRes("xiaojian_png")
            .setSize(new Vector2(15 * 4, 28 * 4)).addClk(() =>
            {
                modelMsgBind mb = ts.GetComponent<modelMsgBind>();
                Debug.Log(mb.msg.posKey);
                if (!mb.isTouch) return;
                orderOperate.getInstance().putTarget(mb.msg.posKey);

                //将所有模型设置为禁止点击
                setNoTouch();
            });
            top.transform.localScale = Vector2.one / GameAttrConst.getMapScaleRate();
            top.transform.localPosition = new Vector2(0, 50);
            top.SetActive(false);
        }
        /**刷新橙装特效*/
        public void updateCzTx(Transform ts, JObject player)
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
        /**播放技能特效*/
        public GameObject playEffect(string key, GameObject target, float dy)
        {
            /*GameObject g = new GameObject(key);
            g.transform.SetParent(PointGet.getModelLayer(), false);
            g.transform.localPosition = target.transform.localPosition * 1f;
            g.transform.localScale = Vector3.one;
            g.transform.localRotation = Quaternion.Euler(target.transform.localRotation.eulerAngles);
            g.AddComponent<lzStopCallback>();

            DoGet.getInstance().loadAnyPrefab("effect/" + key + ".prefab", (ab) =>
            {
                GameObject abObj = ab.LoadAsset<GameObject>(key + ".prefab");
                GameObject tt = GameObject.Instantiate(abObj);
                tt.transform.SetParent(g.transform, false);
                tt.transform.localPosition = new Vector3(0, dy, 0);
            });
            return g;*/
            return null;
        }
        /**添加移动到目标位置的脚本*/
        public void addToTargetPos(GameObject obj, GameObject target, Action startCall, Action endCall)
        {
            /*moveTargetAm m = obj.AddComponent<moveTargetAm>();
            m.startCall = startCall;
            m.endCall = endCall;
            m.target = target;*/

        }
        /**设置图层排序，因为命中时需要将受伤动画图层设置为最顶*/
        public void setLayerOrder(GameObject obj, bool isTop)
        {
            Canvas cv = obj.GetComponent<Canvas>();
            if (isTop)
            {
                cv.overrideSorting = true;
                cv.sortingOrder = 1;
            }
            else
            {
                cv.overrideSorting = false;
                cv.sortingOrder = 0;
            }

        }
        /**对一个包含帧对象的集合对象进行帧重载*/
        private void reloadAm(GameObject obj, string[] pwds, string aef, Action<AniRuntime> ac)
        {
            string[] aefs = { aef };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                Transform ts = PointGet.getModelLayer();
                List<GameObject> list = readModel.drawGameObj(ts, pwds, aefs);

                GameObject g = list[0].gameObject;
                //将帧对象进行替换
                FrameUtils.frameReplace(obj, g);

                ac(obj.GetComponent<AniRuntime>());

            }, all);
        }


        /**播放多段动作*/
        private void playMultyAm(List<AmPlayControl> list, int i, AniRuntime am, Action ac)
        {
            if (i >= list.Count)
            {
                ac();
                return;
            }
            am.setCall(() =>
            {
                i++;
                playMultyAm(list, i, am, ac);
            }).playOnce(list[i].start, list[i].end);
        }
        /**加载boom特效*/
        private void playSjtx(Transform ts, string[] pwds, string aef, Action<AniRuntime> ac)
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
        /**播放爆炸效果（命中特效）*/
        public void playBoom(GameObject obj, Vector2 pos, string sklKey)
        {
            FightModelMsg fm = (FightModelMsg)obj.GetComponent<modelMsgBind>().msg;
            AmModelMsg md = amManager.getAttackTx(fm.amKey, sklKey);
            if (md.zdSjtxPwds != null && md.zdSjtxPwds.Length > 0)
            {
                playSjtx(obj.transform.parent, md.zdSjtxPwds, md.zdSjtxAef, (am1) =>
                {
                    am1.gameObject.transform.localPosition = pos;
                    playMultyAm(md.zdSjtxList, 0, am1, () => { gameObjPool.getInstance().free(am1.gameObject); });
                });
            }
        }
        /**复活，恢复idle状态*/
        public void playReliveStatus(GameObject obj)
        {
            FightModelMsg fm = (FightModelMsg)obj.GetComponent<modelMsgBind>().msg;
            AmModelMsg md = amManager.getAttackTx(fm.amKey, null);
            //请求pwd数据，将原来的帧全部替换
            reloadAm(obj, md.zhandouPwds, md.zhandouAef, (am) =>
            {
                playByStatus(obj, 1);
            });
        }
        public void playByStatus(GameObject obj, int status, string sklKey = null, Action<AmModelMsg> callback = null)
        {
            //0 run 1 idle 2 show2 3 die 4 hit 5 run2 6 show 7 fangyu 8 bianshen 9 battle_run
            //10 attack 11 skill 12 skill1 13 skill2 14 skill3 15 mz 16 mz1 17 mz2 18 mz3 19 mz4 20 sd

            FightModelMsg fm = (FightModelMsg)obj.GetComponent<modelMsgBind>().msg;
            AniRuntime am = obj.GetComponent<AniRuntime>();
            //am.stop();
            if (status == 1)
            {
                am.frameInterval = 0.2f;
                am.setCall(null).play(fm.zdIdle.start, fm.zdIdle.end);
                return;
            }
            else if (status == 3)
            {
                am.frameInterval = 0.2f;
                AmModelMsg md = amManager.getAmByKey("swtx");
                //请求pwd数据，将原来的帧全部替换
                reloadAm(obj, md.changguiPwds, md.changguiAef, (am) =>
                {
                    am.setCall(() =>
                    {
                        am.play(6, 12);
                        if (callback != null) callback(md);
                    }).playOnce(0, 6);
                });
                return;
            }
            else if (status == 4)
            {
                am.frameInterval = 0.05f;
                am.setCall(() => { if (callback != null) callback(null); }).playOnce(fm.zdHurt.start, fm.zdHurt.end);
                return;
            }
            else if (status == 10)
            {
                am.frameInterval = 0.05f;
                AmModelMsg md = amManager.getAttackTx(fm.amKey, sklKey);
                //请求pwd数据，将原来的帧全部替换
                reloadAm(obj, md.zhandouPwds, md.zhandouAef, (am) =>
                {
                    playMultyAm(md.zdSklList, 0, am, () =>
                    {
                        callback(md);
                    });
                });
            }
            else if (status == 15)
            {
                am.frameInterval = 0.05f;
                AmModelMsg md = amManager.getAttackTx(fm.amKey, sklKey);
                //请求pwd数据，将原来的帧全部替换
                reloadAm(obj, md.zhandouPwds, md.zhandouAef, (am) =>
                {
                    setLayerOrder(obj, true);
                    playMultyAm(md.zdMzList, 0, am, () =>
                    {
                        setLayerOrder(obj, false);
                        callback(md);
                    });
                });
            }
            else if (status == 20)
            {
                am.frameInterval = 0.05f;
                am.setCall(() => { if (callback != null) callback(null); }).playOnce(fm.zdSd.start, fm.zdSd.end);
                return;
            }
        }
        
        /**由站位获取模型位置
         * 站位 共20个位置
         * 14 9 4 19
         * 12 7 2 17
         * 10 5 0 15
         * 11 6 1 16
         * 13 8 3 18
         */
        public Vector3 getPosByPosKey(string posKey)
        {
            //从上至下加载，4、9、2、7、0、5、1、6、3、8
            Vector3 v3 = default;
            switch (posKey)
            {
                case "l4":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100, 300 * 2f, 0f); break;
                    }
                case "l2":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100, 300 * 1f, 0f); break;
                    }
                case "l0":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100, 0, 0f); break;
                    }
                case "l1":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100, -300 * 1f, 0f); break;
                    }
                case "l3":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100, -300 * 2f, 0f); break;
                    }

                case "l9":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100 + 150, 300 * 2f - 100, 0f); break;
                    }
                case "l7":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100 + 150, 300 * 1f - 100, 0f); break;
                    }
                case "l5":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100 + 150, 0 - 100, 0f); break;
                    }
                case "l6":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100 + 150, -300 * 1f - 100, 0f); break;
                    }
                case "l8":
                    {
                        v3 = new Vector3(-ScreenUtils.width / 2f + 100 + 150, -300 * 2f - 100, 0f); break;
                    }



                case "r14": return new Vector3(96f, 0f, 101f);
                case "r12": return new Vector3(98f, 0f, 101f);
                case "r10": return new Vector3(100f, 0f, 101f);
                case "r11": return new Vector3(102f, 0f, 101f);
                case "r13": return new Vector3(104f, 0f, 101f);

                case "r19": return new Vector3(96f, 0f, 107f);
                case "r17": return new Vector3(98f, 0f, 107f);
                case "r15": return new Vector3(100f, 0f, 107f);
                case "r16": return new Vector3(102f, 0f, 107f);
                case "r18": return new Vector3(104f, 0f, 107f);

                case "r4":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 100, 300 * 2f, 0f); break;
                    }
                case "r2":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 100, 300 * 1f, 0f); break;
                    }
                case "r0":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 100, 0, 0f); break;
                    }
                case "r1":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 100, -300 * 1f, 0f); break;
                    }
                case "r3":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 100, -300 * 2f, 0f); break;
                    }

                case "r9":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 250, 300 * 2f - 100, 0f); break;
                    }
                case "r7":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 250, 300 * 1f - 100, 0f); break;
                    }
                case "r5":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 250, 0 - 100, 0f); break;
                    }
                case "r6":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 250, -300 * 1f - 100, 0f); break;
                    }
                case "r8":
                    {
                        v3 = new Vector3(ScreenUtils.width / 2f - 250, -300 * 2f - 100, 0f); break;
                    }

                case "l14": return new Vector3(96f, 0f, 99f);
                case "l12": return new Vector3(98f, 0f, 99f);
                case "l10": return new Vector3(100f, 0f, 99f);
                case "l11": return new Vector3(102f, 0f, 99f);
                case "l13": return new Vector3(104f, 0f, 99f);

                case "l19": return new Vector3(96f, 0f, 93f);
                case "l17": return new Vector3(98f, 0f, 93f);
                case "l15": return new Vector3(100f, 0f, 93f);
                case "l16": return new Vector3(102f, 0f, 93f);
                case "l18": return new Vector3(104f, 0f, 93f);
            }
            //高2460是标准，其他分辨率则按其比例计算
            float k = ScreenUtils.height / 2460f;
            v3.y = v3.y * k - 90;//90是top区高度
            return v3;
        }
    }
}
