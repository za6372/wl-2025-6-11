using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.touch;
using Assets.HotFix.xq2d.src.touch;
using Assets.HotFix.xq2d.src.fight;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    class npcManager
    {
        public static JArray getNpcList()
        {
            JArray arr = new JArray();
            List<GameObject> npcs = getNpcs();
            foreach (GameObject g in npcs)
            {
                string k = g.GetComponent<modelMsgBind>().msg.key;
                arr.Add(k);
            }
            return arr;
        }
        public static List<GameObject> getNpcs()
        {
            List<GameObject> list = new List<GameObject>();
            Transform ts = PointGet.getMapMapePoint();
            for (int i = 0; i < ts.childCount; i++)
            {
                Transform a = ts.GetChild(i);
                modelMsgBind md = a.GetComponent<modelMsgBind>();
                if (a.name.Contains("npc_") && md != null && md.msg is NpcObjData)
                {
                    list.Add(a.gameObject);
                }
            }

            return list;
        }
        public static void clearNpcs()
        {
            //todo:注意：当切入战斗时这个npcs对应在mape节点的npc会放入free区，npcs缓存的则指向free区，当重新回到主页时，可能free区中的一个npc刚好作为mape，
            //todo:如果此刻再执行一次对npcs的free就会把mape给干掉，所以不要使用集合去存储，直接在mape中找就行
            List<GameObject> npcs = getNpcs();
            foreach (GameObject g in npcs)
            {
                gameObjPool.getInstance().free(g);
            }
            npcs.Clear();
        }
        public static void clearNpc(JArray keys)
        {
            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < npcs.Count; j++)
                {
                    GameObject g = npcs[j];
                    string k = g.GetComponent<modelMsgBind>().msg.key;
                    if (k.Equals(keys[i].ToString()))
                    {
                        npcs.RemoveAt(j);
                        j--;
                        gameObjPool.getInstance().free(g);
                    }
                }
            }
        }
        public static void clearNpcByObj(modelMsgBind npc)
        {
            List<GameObject> npcs = getNpcs();
            foreach (GameObject g in npcs)
            {
                if (g.GetComponent<modelMsgBind>() == npc)
                {
                    npcs.Remove(g);
                    gameObjPool.getInstance().free(g);
                    break;
                }
            }
        }
        public static void clearById(int id)//这个id是gameobject id
        {
            List<GameObject> npcs = getNpcs();
            foreach (GameObject g in npcs)
            {
                if (g.GetInstanceID() == id)
                {
                    npcs.Remove(g);
                    gameObjPool.getInstance().free(g);
                    break;
                }
            }
        }
        /**对同key的所有npc进行排序，然后隐藏除了index的npc*/
        public static void clearNpcByKeyExpIndex(string key, int index)
        {
            List<GameObject> npcs = getNpcs();
            List<int> arr = new List<int>();
            for (int j = 0; j < npcs.Count; j++)
            {
                GameObject g = npcs[j];
                string k = g.GetComponent<modelMsgBind>().msg.key;
                if (k.Equals(key))
                {
                    arr.Add(g.GetInstanceID());
                }
            }
            for (int i = 0; i < arr.Count; i++)
            {
                if (i != index)
                {
                    foreach (GameObject g in npcs)
                    {
                        if (g.GetInstanceID() == arr[i])
                        {
                            g.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }

        }
        /**获取同一key的npc下标*/
        public static int getIndexFromNpc(modelMsgBind npc)
        {
            string key = npc.msg.key;
            List<GameObject> npcs = getNpcs();
            List<int> arr = new List<int>();
            for (int j = 0; j < npcs.Count; j++)
            {
                GameObject g = npcs[j];
                string k = g.GetComponent<modelMsgBind>().msg.key;
                if (k.Equals(key))
                {
                    arr.Add(g.GetInstanceID());
                }
            }
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == npc.gameObject.GetInstanceID()) return i;
            }
            return 0;
        }
        /**客户端创建npc时所需的结构*/
        public static JObject getOneData(string npcKey, JObject pos, JObject scale)
        {
            JObject a = new JObject();
            a.Add("key", npcKey);
            a.Add("pos", pos);
            a.Add("scale", scale);
            return a;
        }
        public static JObject getPosData(float x, float y)
        {
            JObject a = new JObject();
            a.Add("x", x);
            a.Add("y", y);
            return a;
        }
        public static JObject getScaleData(float x, float y)
        {
            JObject a = new JObject();
            a.Add("x", x);
            a.Add("y", y);
            return a;
        }
        /**创建采集物*/
        public static void createCjw(string npcKey)
        {
            GameObject npc = getNpcByKey(npcKey);
            if (npc != null) return;
            JArray list = new JArray();
            list.Add(getOneData(npcKey, getPosData(250, 250), null));
            appendNpc(list);
        }
        /**追加npc*/
        public static void appendNpc(JArray arr)
        {
            Transform ts = PointGet.getMapMapePoint();
            if (arr == null)
            {
                return;
            }
            for (int i = 0; i < arr.Count; i++)
            {
                loadOne(i, arr, ts, () => { });
            }
            //仅仅是收集需要加载的文件，startReqImg后才会进行加载
            DoGet.getInstance().startReqImg();
        }

        public static GameObject getNpcById(int npcId)
        {
            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < npcs.Count; i++)
            {
                //Debug.Log(npcs[i].GetInstanceID() + "=>" + npcId);
                if (npcs[i].GetInstanceID() == npcId) return npcs[i];
            }
            return null;
        }
        public static GameObject getNpcByKey(string key)
        {
            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i].name.Equals("npc_" + key)) return npcs[i];
            }
            return null;
        }
        /**获取传送点*/
        public static Transform getTransmit(string mapKey)
        {
            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < npcs.Count; i++)
            {
                modelMsgBind ms = npcs[i].GetComponent<modelMsgBind>();
                NpcObjData data = (NpcObjData)ms.msg;
                if (data.type == 3 && data.toMapKey.Equals(mapKey))//传送
                {
                    return npcs[i].transform;
                }
            }
            return null;
        }

        public static void drawNpc(JArray arr, Mape mape, Action ac)
        {
            clearNpcs();

            //fixme:不能按地图给的npc来，因为会缺少一些npc，并且很多位置都不对
            /*if (mape.npc != null)
            {
                List<NpcData> list = mape.npc.arr;
                for (int i = 0; i < list.Count; i++)
                {
                    NpcData npc = list[i];
                    string picName = npc.getPicName();
                    Debug.Log("加载npc：" + npc.getNpcId() + "=>" + picName + "=>" + npc.getNpcName() +
                        "(" + npc.x + "," + npc.y + ")" + " " + npc.t0 + "/" + npc.t1 + "/" + npc.w + "/" + npc.h);

                }
            }*/
            Debug.Log("111.mape是否为null?" + (PointGet.getMapMapePoint() == null));
            Transform ts = PointGet.getMapMapePoint();
            if (arr == null || arr.Count == 0)
            {
                ac();
                return;
            }
            int num = 0;
            Action fn = () =>
            {
                num++;
                //Debug.Log("计数" + num + "/" + arr.Count);
                if (num == arr.Count)
                {
                    ac();
                }
            };
            for (int i = 0; i < arr.Count; i++)
            {
                Debug.Log(i + "===.mape是否为null?" + (PointGet.getMapMapePoint() == null));
                loadOne(i, arr, ts, fn);
            }
            //仅仅是收集需要加载的文件，startReqImg后才会进行加载
            DoGet.getInstance().startReqImg();
        }
        private static void loadOne(int i, JArray npcList, Transform ts, Action ac)
        {
            if (ts == null)
            {
                Debug.Log((ts == null) + "mape为null，导致无法加载" + i + "/" + npcList[i]);
                LoadingUI.getOne().updateJd(0, "ts不存在，导致无法加载");
                //return;
            }
            JObject npc = (JObject)npcList[i];
            JObject pos = (JObject)npc["pos"];
            string npcKey = npc["key"].ToString();
            NpcObjData a = face.npcInterface.getNpc(npcKey);
            Debug.Log((a == null) + "=>" + npcKey);
            //采集物（根据任务状态来判断是否显示，采集后立即移除）
            if (strUtils.isMatch(npcKey, "cj([0-9]{4})"))
            {
                if (
                    (npcKey.Equals("cj1000") && !face.taskInterface.isDoingTaskAndProgressSame("1003", 0)) ||
                    (npcKey.Equals("cj1001") && !face.taskInterface.isDoingTaskAndProgressSame("1025", 0)) ||
                    (npcKey.Equals("cj1002") && !face.taskInterface.isDoingTaskAndProgressSame("1060", 0)) ||
                    (npcKey.Equals("cj1003") && !face.taskInterface.isDoingTaskAndProgressSame("1066", 0)) ||
                    (npcKey.Equals("cj1012") && !face.taskInterface.isDoingTaskAndProgressSame("3148", 0) && !face.taskInterface.isDoingTaskAndProgressSame("3105", 0)) ||
                    (npcKey.Equals("cj1013") && !face.taskInterface.isDoingTaskAndProgressSame("3149", 0) && !face.taskInterface.isDoingTaskAndProgressSame("3106", 0)) ||
                    (npcKey.Equals("cj1014") && !face.taskInterface.isDoingTaskAndProgressSame("3150", 0) && !face.taskInterface.isDoingTaskAndProgressSame("3107", 0)) ||
                    (npcKey.Equals("cj1015") && !face.taskInterface.isDoingTaskAndProgressSame("3151", 0) && !face.taskInterface.isDoingTaskAndProgressSame("3108", 0)) ||
                    (npcKey.Equals("cj1016") && !face.taskInterface.isDoingTaskAndProgressSame("3152", 0) && !face.taskInterface.isDoingTaskAndProgressSame("3109", 0))
                    )
                {
                    ac();
                    return;
                }
            }
            if (a.picPath == null)
            {
                Debug.Log("npc-picPath=null=>" + npcKey);
            }
            if (a.picPath.Contains("_png"))
            {
                loadPng(pos, a, ts, ac);
            }
            else
            {
                loadPwd(pos, a, ts, ac);
            }
        }
        public static void allowedMove()
        {
            List<GameObject> npcs = getNpcs();
            foreach (GameObject g in npcs)
            {
                monsterMove mv = g.GetComponent<monsterMove>();
                if (mv == null) continue;
                mv.setIsAllowedMove(true);
            }
        }
        /**刷新npc任务状态*/
        public static void updateTaskIcon()
        {

            //键是npcKey
            JObject dic = new JObject();
            //先获取缓存的任务，根据任务的状态及进度来获取当前对应的npc
            //{"progressIndex":0,"key":"1112","status":1,"taskProgress":{"target":{"num":0}}}
            JArray list = face.taskInterface.getTaskListFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject tk = (JObject)list[i];

                int status = (int)tk["status"];

                if (status == 0) continue;
                string npcKey = null;

                task ts = face.taskInterface.getTask(tk["key"].ToString());
                /*if (tk["key"].ToString().Equals("3000"))
                {
                    Debug.Log("========3000===="+ (ts.key)+"/"+status+"/"+ ts.getNpcKey);
                }*/
                if (status == 1)//可接取状态的npc
                {
                    npcKey = ts.getNpcKey;
                }
                else if (status == 2)//进行中，需要按进度来获取相应的npc
                {
                    int progressIndex = (int)tk["progressIndex"];
                    JObject progress = (JObject)ts.progressList[progressIndex];
                    JObject target = (JObject)progress["target"];
                    npcKey = target["toKey"].ToString();
                }
                else if (status == 3)//已完成（可提交）
                {
                    npcKey = ts.subNpcKey;
                }
                if (npcKey == null) continue;
                //只需要npc取最后的状态即可
                if (!dic.ContainsKey(npcKey))
                {
                    dic[npcKey] = status;
                }
                else
                {
                    int v = (int)dic[npcKey];
                    if (v < status) dic[npcKey] = status;
                }

            }
            //Debug.Log(dic);
            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < npcs.Count; i++)
            {
                GameObject b = npcs[i];

                //战斗结束后会立即通知刷新，但此刻indexPage还未加载，b故为null
                if (!b || !b.GetComponent<modelTaskStatusBind>()) continue;

                b.GetComponent<modelTaskStatusBind>().remTaskIcon();
                //b.GetComponent<modelTitleBind>().remTaskIcon();
                ModelMsg msg = b.GetComponent<modelMsgBind>().msg;

                //Debug.Log(msg.key);


                if (!dic.ContainsKey(msg.key)) continue;
                int status = (int)dic[msg.key];
                //Debug.Log(msg.key + "/" + status);
                if (status != 1 && status != 2 && status != 3) continue;


                //b.GetComponent<modelTitleBind>().addTaskIcon(status);
                b.GetComponent<modelTaskStatusBind>().addTaskIcon(status);
            }
            DoGet.getInstance().startReqImg();
        }
        /**靠近npc时处理*/
        public static bool nearNpcHandle(Transform npc)
        {
            if (npc == null) return false;
            modelMsgBind ms = npc.GetComponent<modelMsgBind>();
            NpcObjData data = (NpcObjData)ms.msg;
            if (data.type == 3)//传送
            {
                //自动遇怪的情况下不执行传送
                if (GameAttrConst.isAutoYG) return false;
                PointGet.getMapPoint().GetComponent<Move>().stopMove();
                mapManager.getInstance().savePreMapKey();
                mapManager.getInstance().reloadBef(data.toMapKey);
                return true;
            }
            else if (data.type == 4)//怪物
            {
                //改成线性，避免同时遇多个怪导致繁忙
                if (!fightCache.getInstance().isDoing)
                {
                    taskManager.getInstance().putTask(() =>
                    {
                        if (!fightCache.getInstance().isDoing)
                        {
                            PointGet.getMapPoint().GetComponent<Move>().stopMove();
                            //触发战斗
                            face.fightInterface.createFightByMonster(data.toMonKey);
                        }
                    });
                }
                return true;
            }
            else if (data.type == 5)//采集物
            {
                //移除这个采集物
                clearById(npc.gameObject.GetInstanceID());
                PointGet.getMapPoint().GetComponent<Move>().stopMove();
                face.taskInterface.collectionCjw(ms.msg.key);
                return true;
            }
            return false;
        }
        /**靠近时点击npc*/
        public static bool isTouchNpc(Vector2 touch)
        {
            //if (true) return false;
            // Debug.Log("未变换的touch=>" + touch);

            //触摸点以左下角为原点，mape是左上角为原点
            touch = new Vector2(touch.x, -(ScreenUtils.height - touch.y));
            //现在的touch是以屏幕左上角为原点，右侧正值逐渐变大，下侧负值逐渐变下，与mape无关系，仅相对于屏幕
            //Debug.Log("变换后的touch=>" + touch);

            Vector2 vs = PosCountUtils.touchPosToRelativeMapePos(touch);

            Transform rl = PointGet.getControlPoint();

            float k = ScreenUtils.width / 1080f * ScreenUtils.screenScaleRate;

            List<GameObject> npcs = getNpcs();
            for (int i = 0; i < npcs.Count; i++)
            {
                GameObject npc = npcs[i];

                Vector2 npcPos = PosCountUtils.npcPosRelativeMapePos(npc.transform);
                //Debug.Log("触点坐标：" + vs + " / "+npc.name+"坐标：" + npcPos);

                //只需要判断npc跟玩家的位置，以及触点位置是否在npc上
                if (Math.Abs(vs.x - npcPos.x) < 100 * k &&
                (((vs.y - npcPos.y) < 100 * k) && ((vs.y - npcPos.y) > -100 * k)))
                {
                    Debug.Log("点击了npc：" + npc.name);
                    Debug.Log("touchPos=>" + vs);
                }
                else
                {
                    continue;
                }

                if (Math.Abs(rl.position.x - npc.transform.position.x) < 100 * k &&
                (((rl.position.y - npc.transform.position.y) < 100 * k) && ((rl.position.y - npc.transform.position.y) > -100 * k)))
                {
                    //根据npc类型来弹出
                    modelMsgBind ms = npcs[i].GetComponent<modelMsgBind>();
                    NpcObjData data = (NpcObjData)ms.msg;
                    if (data.type != 2) continue;
                    //弹出任务、活动等
                    TaskTalkUI.showNpcMenu(npcs[i].GetComponent<modelMsgBind>());
                    return true;
                }
            }
            return false;
        }

        private static void loadPng(JObject pos, NpcObjData npc, Transform ts, Action ac)
        {
            GameObject spriteObj = gameObjPool.getInstance().get("npc_" + npc.key, typeof(ImgUI));
            spriteObj.transform.SetParent(ts, false);
            spriteObj.GetComponent<ImgUI>().loadRes(npc.picPath).addCallback((sp) =>
            {
                Vector2 sizeDelta = ts.GetComponent<RectTransform>().sizeDelta;
                spriteObj.GetComponent<Image>().sprite = sp;
                //Texture2D td = sp.texture;

                float rate = GameAttrConst.getMapScaleRate();
                //Vector2 size = new Vector2(td.width * rate, td.height * rate);
                Vector2 size = new Vector2(sp.rect.width, sp.rect.height) * rate;
                spriteObj.GetComponent<RectTransform>().sizeDelta = size;
                float dx = -sizeDelta.x / 2f + ((float)pos["x"]) * rate;
                float dy = sizeDelta.y / 2f + (-(float)pos["y"] + sp.rect.height / 2f) * rate;
                //0,0位于父级的中心点
                spriteObj.transform.localPosition = new Vector2(dx, dy);
                //spriteObj.transform.localScale = Vector2.one * rate;

                spriteObj.AddComponent<modelMsgBind>().addModelMsg(npc);
                spriteObj.AddComponent<modelTaskStatusBind>();


                GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));
                TextUI tx = title.GetComponent<TextUI>();
                tx.setText(npc.name).setColor("#ffde00").setAlign().setFontSize(35)
                .setFontStyle().setIsRichText().horiOut().horiOut().setSize(new Vector2(100, 100));
                title.transform.SetParent(spriteObj.transform, false);
                title.transform.localPosition = new Vector2(0, 150);
                title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);

                ac();
            });

        }
        private static void loadPwd(JObject pos, NpcObjData npc, Transform ts, Action ac)
        {
            AmModelMsg msg = amManager.getAmByKey(npc.picPath);
            List<string> pwds = new List<string>(msg.changguiPwds);
            List<string> aefs = new List<string>();
            aefs.Add(msg.changguiAef);
            /*string[] fs = npc.picPath.Split(",");
            if (fs.Length > 1)
            {

                for (int i = 0; i < fs.Length; i++)
                {
                    if (fs[i].Contains("_pwd")) pwds.Add(fs[i]);
                    else aefs.Add(fs[i]);
                }
            }
            else
            {
                string fileName = npc.picPath.Replace("_pwd", "");
                pwds.Add(fileName + "_pwd");
                aefs.Add(fileName + "_aef");
            }*/
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);

            DoGet.getInstance().collectionAnyRes(() =>
            {
                Vector2 sizeDelta = ts.GetComponent<RectTransform>().sizeDelta;
                List<GameObject> list = readModel.drawGameObj(ts, pwds.ToArray(), aefs.ToArray());
                GameObject g = list[0].gameObject;
                g.name = "npc_" + npc.key;
                g.transform.SetParent(ts, false);
                AniRuntime am = g.GetComponent<AniRuntime>();
                am.frameInterval = 0.25f;
                am.play();
                if (g.GetComponent<RectTransform>() == null) g.AddComponent<RectTransform>();

                Vector2 size = g.GetComponent<RectTransform>().sizeDelta;
                float rate = GameAttrConst.getMapScaleRate();
                float dx = -sizeDelta.x / 2f + ((float)pos["x"] * 1f) * rate;
                float dy = sizeDelta.y / 2f - ((float)pos["y"]) * rate + size.y;
                //Debug.Log(pos["x"] + "===========" + pos["y"] + "===" + size);
                //SetGameObj.setLeftPos(new Vector2(dx,dy), g);
                g.transform.localPosition = new Vector2(dx, dy);
                g.transform.localScale = Vector2.one * rate;
                /*g.AddComponent<modelRealPosMsgBind>().init(new Vector2(0, 0),
                    new Vector2(((float)pos["x"]) * rate - 100,
                    -((float)pos["y"]) * rate + size.y));*/

                g.AddComponent<modelMsgBind>().addModelMsg(npc);
                g.AddComponent<modelTaskStatusBind>();
                if (npc.type == 4 && npc.isMove)
                {
                    //怪物类允许自由移动
                    g.AddComponent<monsterMove>();
                }
                /*GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));

                TextUI tx = title.GetComponent<TextUI>();
                tx.setText(npc.name + "(" + pos["x"] + "," + pos["y"] + ")").setColor("#ffde00").setAlign().setFontSize(35)
                .setFontStyle().setIsRichText().horiOut();
                title.transform.SetParent(g.transform, false);
                title.transform.localScale = Vector2.one / 5f;
                SetGameObj.setSize(new Vector2(300, 60), title);
                SetGameObj.setLeftPos(new Vector2(-(300 - size.x) / 2, 100), title);
                title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);*/

                GameObject title = gameObjPool.getInstance().get("name", typeof(TextUI));
                TextUI tx = title.GetComponent<TextUI>();
                tx.setText(npc.name).setColor("#ffde00").setAlign().setFontSize(35)
                .setFontStyle().setIsRichText().horiOut().setSize(new Vector2(100, 100));
                title.transform.SetParent(g.transform, false);
                title.transform.localScale = Vector2.one / rate;
                title.transform.localPosition = new Vector2(0, 50);
                title.AddComponent<UIOutline>().effectDistance = new Vector2(2, -2);

                ac();
            }, all);
        }
    }
}
