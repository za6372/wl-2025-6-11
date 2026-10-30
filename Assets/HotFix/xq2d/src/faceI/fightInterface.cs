using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
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

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface fightInterface
    {
        public void getFightMsg(string id);
        public bool putOrder(JObject msg);
        public void putAutoOrder(string posKey);
        public void putCancelAutoOrder(string posKey);
        public void signUpWzy(string teamName);
        public void getWzyNameList(int pageNum, Action<object> callback);
        public void signUpWHJX();
        public void createFightByMonster(string npcKey);
        public void attackBPBoss();
        public void attackRobber();
        public void tzBLYT(int lever);
        public void tzCWBK(string id, Action callback);
        public void attackMozun();
        public void createFightByPK(string playerName, Action callback);
        public void putAmPlayedOrder();
        public void createFightBySneak(string playerName, Action callback);
        public void createFightByDzfsb(int useZdhl, int sort, Action callback);
        public void createFightByZsl(string playerName, Action callback);
        public void viewShiFaWay(Action<JObject> ac);
        public void uploadShiFaWay(int way, int type, int order, int index, Action ac);
        public void viewFight(string playerName, Action ac);
        public void closeViewFight(Action ac);
    }

    class fightInterfaceImpl : fightInterface
    {
        public void closeViewFight(Action ac)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", fightCache.getInstance().downloadFightMsg["Id"].ToString());
            DoGet.getInstance().sendPost("/fightRpcService/closeViewFight", dic, (res) =>
            {
                attackAm.getInstance().clearHandle();
                ac();
            });
        }
        public void viewFight(string playerName, Action ac)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/fightRpcService/viewFight", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(769);
                    return;
                }
                showScene((JObject)res, true);
                ac();
            });
        }
        public void viewShiFaWay(Action<JObject> ac)
        {
            DoGet.getInstance().sendPost("/manService/viewShiFaWay", null, (res) =>
            {
                ac((JObject)res);
            });
        }
        /**上传施法方式*/
        public void uploadShiFaWay(int way,int type,int order,int index,Action ac)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("way", way);
            dic.Add("type", type);
            dic.Add("order", order);
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/manService/uploadShiFaWay", dic, (res) =>
            {
                ac();
            });
        }
        public void createFightByZsl(string playerName, Action callback)
        {
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/fightRpcService/createFightByZsl", dic, (res) =>
            {
                if (int.Parse(res.ToString()) != 1)
                {
                    fightCache.getInstance().isDoing = false;
                    msgCode.showMsg(832);
                    return;
                }
                callback();
            }, () => { fightCache.getInstance().isDoing = false; });

        }
        public void createFightByDzfsb(int useZdhl, int sort, Action callback)
        {
            if (useZdhl == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000224", 1)) return;
            }
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("useZdhl", useZdhl);
            dic.Add("sort", sort);
            DoGet.getInstance().sendPost("/fightRpcService/createFightByDzfsb", dic, (res) =>
            {
                if (useZdhl == 1)
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey("10000224", 1);
                }
                callback();
            }, () => { fightCache.getInstance().isDoing = false; });

        }
        public void createFightBySneak(string playerName, Action callback)
        {
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/fightRpcService/createFightBySneak", dic, (res) =>
            {
                if (int.Parse(res.ToString()) != 1)
                {
                    fightCache.getInstance().isDoing = false;
                    msgCode.showMsg(832);
                    return;
                }
                callback();
            }, () => { fightCache.getInstance().isDoing = false; });

        }
        public void putAmPlayedOrder()
        {
            string posKey = fightCache.getInstance().getControlRolePosKey();
            string id = (string)fightCache.getInstance().downloadFightMsg["Id"];
            JObject obj = new JObject();
            obj.Add("Id", id);
            obj.Add("control", posKey);
            DoGet.getInstance().sendWs("/fightService/putAmPlayedOrder", obj);
        }

        public void createFightByPK(string playerName, Action callback)
        {
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/fightRpcService/createFightByPK", dic, (res) =>
            {
                callback();
            }, () => { fightCache.getInstance().isDoing = false; });

        }
        /**挑战成王败寇*/
        public void tzCWBK(string id, Action callback)
        {
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", id);
            DoGet.getInstance().sendPost("/cwbcService/tz", dic, (res) =>
            {
                if (res.ToString() == "0")
                {
                    msgCode.showMsg(797);
                    return;
                }
                else if (res.ToString() == "2")
                {
                    callback();
                }

            }, () => { fightCache.getInstance().isDoing = false; });

        }
        /**挑战百炼妖塔*/
        public void tzBLYT(int lever)
        {
            fightCache.getInstance().isDoing = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("lever", lever);
            DoGet.getInstance().sendPost("/blytService/tz", dic, (res) =>
            {

            }, () => { fightCache.getInstance().isDoing = false; });

        }
        /**攻击魔尊*/
        public void attackMozun()
        {
            fightCache.getInstance().isDoing = true;
            DoGet.getInstance().sendPost("/fightRpcService/attackMozun", null, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    //防止战斗结束后再次触发
                    netUtils.getInstance().body["mzzd"] = 1;
                }


            }, () => { fightCache.getInstance().isDoing = false; });
        }
        /**攻击强盗*/
        public void attackRobber()
        {
            fightCache.getInstance().isDoing = true;
            DoGet.getInstance().sendPost("/gangsService/attackRobber", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(893);
                    //清理所有强盗npc
                    JArray list = new JArray();
                    list.Add("2110");
                    JObject m = new JObject();
                    m.Add("mapKey", "gangs");
                    m.Add("list", new JArray());
                    m.Add("clearList", list);

                    eventsUtils.dispatchWsEvent("102", m);
                }
            }, () => { fightCache.getInstance().isDoing = false; });

        }
        /**帮派boss*/
        public void attackBPBoss()
        {
            fightCache.getInstance().isDoing = true;
            JObject r = face.roleInterface.getRole();
            //先获取boss血量，然后确认攻击
            DoGet.getInstance().sendPost("/gangsService/getBossMsg", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(729);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(730);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(610);
                }
                else
                {
                    //服务端调取战斗
                }
            }, () => { fightCache.getInstance().isDoing = false; });
        }
        /**
         gw1000 普通怪物带前缀 gw 后面四个代表宠物key
         */
        public void createFightByMonster(string npcKey)
        {
            if (npcKey == null)
            {
                Debug.Log("npcKey不能为null");
                return;
            }
            //跟随状态下不允许战斗
            if (!face.teamInterface.isAllowedTouchMove())
            {
                return;
            }
            fightCache.getInstance().isDoing = true;
            //Debug.Log("遇怪：" + npcKey);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("npcKey", npcKey);
            //申请进入战斗，后端会创建战斗数据，之后会通过ws通知客户端创建
            DoGet.getInstance().sendPost("/fightRpcService/createFightByMonster", dic, (res) =>
            {
                //等待ws返回战斗数据后调取创建战斗

                //自动遇怪次数--
                if (GameAttrConst.isAutoYG)
                {
                    GameAttrConst.autoYgTimes--;
                    face.chatInterface.writeSysMsg("自动遇怪剩余次数：" + GameAttrConst.autoYgTimes);
                }
            }, () =>
            {
                fightCache.getInstance().isDoing = false;
            });
        }
        /**报名王侯将相*/
        public void signUpWHJX()
        {
            int lv = (int)face.roleInterface.getRole()["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }

            DoGet.getInstance().sendPost("/pkService/signUpWHJX", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(667);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(884);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(613);
                }
                else if (res.ToString().Equals("3"))
                {
                    msgCode.showMsg(610);
                }
            });
        }
        /**获取武状元对战名单 */
        public void getWzyNameList(int pageNum, Action<object> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/wzyService/getNameList", dic, (res) =>
            {
                callback(res);
            });

        }
        /**报名武状元 */
        public void signUpWzy(string teamName)
        {
            //必须先组队
            JObject team = face.teamInterface.getTeam();
            if (team == null)
            {
                msgCode.showMsg(670);
                return;
            }
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                if ((int)list[p]["lever"] < 60)
                {
                    msgCode.showMsg(613);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("teamName", teamName);
            DoGet.getInstance().sendPost("/wzyService/signUp", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(671);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(666);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(670);
                }
                else if (res.ToString().Equals("3"))
                {
                    msgCode.showMsg(672);
                }
                else if (res.ToString().Equals("4"))
                {
                    msgCode.showMsg(673);
                }
                else if (res.ToString().Equals("5"))
                {
                    msgCode.showMsg(674);
                }
                else if (res.ToString().Equals("6"))
                {
                    msgCode.showMsg(613);
                }
            });
        }
        /**战斗创建完成后，拉取其信息 */
        public void getFightMsg(string id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", id);
            DoGet.getInstance().sendPost("/fightRpcService/getFightMsg", dic, (res) =>
            {
                this.showScene((JObject)res, false);
            }, () =>
            {
                fightCache.getInstance().isDoing = false;
            });

        }
        public void showScene(JObject res, bool isViewFight)
        {
            attackAm.getInstance().nextToFight();
            //战斗数据放入缓存
            fightCache.getInstance().isViewFight = isViewFight;
            fightCache.getInstance().downloadFightMsg = res;
            fightCache.getInstance().isDoing = true;
            this.chooseControlKey();

            JArray roleList = (JArray)res["roleList"];
            JArray monsterList = (JArray)res["monsterList"];
            roleList = posKeyOrder(roleList);
            monsterList = posKeyOrder(monsterList);

            PageUI.create<FightPage>().putOrderRoleAndMonster(roleList, monsterList);
            fightCache.getInstance().isDownloadMsg = true;
            
        }
        /**对站位排序
         从上至下加载，4、9、2、7、0、5、1、6、3、8
         */
        private JArray posKeyOrder(JArray roleList)
        {
            int[] arr = { 4, 9, 2, 7, 0, 5, 1, 6, 3, 8 };
            JArray list = new JArray();
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < roleList.Count; j++)
                {
                    JObject obj = (JObject)roleList[j];
                    if (obj["posKey"].ToString().Substring(1).Equals(arr[i].ToString())) list.Add(obj);
                }
            }
            return list;
        }
        /**选出控制者站位*/
        private void chooseControlKey()
        {
            JObject fgMsg = fightCache.getInstance().downloadFightMsg;
            fgMsg["control"] = new JObject();
            int fgType = (int)fgMsg["type"];
            JArray roleList = (JArray)fgMsg["roleList"];
            //分配控制者
            JObject role = face.roleInterface.getRole();
            string name = role["name"].ToString();
            foreach (object r in roleList)
            {
                JObject obj = (JObject)r;
                string oName = obj["name"].ToString();
                //非论贱（个人、官方）
                if (fgType != 10 && fgType != 11)
                {
                    if (oName.Equals(name))
                    {//角色控制
                        fgMsg["control"]["role"] = obj["posKey"].ToString();
                    }
                    else if (oName.Contains("_pet") && oName.Substring(0, oName.Length - 4).Equals(name))
                    {//宠物控制
                        fgMsg["control"]["pet"] = obj["posKey"].ToString();
                    }

                }
                else
                {
                    string n = oName.Substring(0, oName.Length - 2);
                    if (n.Equals(name) && oName.ToCharArray()[(oName.Length - 1)] == 'a')
                    {
                        fgMsg["control"]["role"] = obj["posKey"].ToString();
                    }
                    else if (n.Equals(name) && oName.ToCharArray()[(oName.Length - 1)] == 'b')
                    {
                        fgMsg["control"]["pet"] = obj["posKey"].ToString();
                    }
                }
            }
            //Debug.Log(fightCache.getInstance().downloadFightMsg["control"]);
        }
        /**发送自动攻击指令*/
        public void putAutoOrder(string posKey)
        {
            string id = (string)fightCache.getInstance().downloadFightMsg["Id"];
            JObject obj = new JObject();
            obj.Add("Id", id);
            obj.Add("control", posKey);
            DoGet.getInstance().sendWs("/fightService/putAutoOrder", obj);
        }
        /**取消自动攻击*/
        public void putCancelAutoOrder(string posKey)
        {
            string id = (string)fightCache.getInstance().downloadFightMsg["Id"];
            JObject obj = new JObject();
            obj.Add("Id", id);
            obj.Add("control", posKey);
            DoGet.getInstance().sendWs("/fightService/putCancelAutoOrder", obj);
        }
        /**发送指令*/
        public bool putOrder(JObject msg)
        {
            if (msg == null || msg.Count == 0)
            {
                return false;
            }
            foreach (object p in msg)
            {
                if (p == null) return false;
            }
            string id = (string)fightCache.getInstance().downloadFightMsg["Id"];
            JObject obj = new JObject();
            obj.Add("Id", id);
            obj.Add("uploadOrder", msg);
            DoGet.getInstance().sendWs("/fightService/putOrder", obj);
            return true;
        }
    }
}
