using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{

    public interface teamInterface
    {
        public void initTeam();
        public JObject getTeam();
        public void createTeam();
        public void createTeamSuc(JObject r);
        public void inviteTeam(string playerName);
        public void leaveTeam(string playerName);
        public void delTeam();
        public void inTeamSuc(JObject team);
        public bool isInTeam();
        public bool isCaptain();
        public JObject getSelfFromTeam();
        public void reqTeam(string playerName);
        public void agreeInvite(string Id);
        public void agreeTeam(string name);
        public JObject getMeFromTeam();
        public JObject isInSameMapKey();
        public void findCaptainFromPlayer(Action callback);
        public JObject getCaptainPos();
        public void captainToName(string playerName);
        public void uploadStatus(int isFollow);
        public bool isExistPlayer(string name);
        public void saveCaptainPos(JObject pos);
        public void findFormation(Action<JObject> callback);
        public void saveFormation(JObject formation, string petId, Action callback);
        public bool isFollow();
        public bool isAllowedTouchMove();

    }
    public class teamInterfaceImpl : teamInterface
    {
        /**是否允许触屏移动等操作*/
        public bool isAllowedTouchMove()
        {
            if (this.isInTeam() && !this.isCaptain() && this.isFollow())
            {
                return false;
            }
            return true;
        }
        /**保存阵型*/
        public void saveFormation(JObject formation, string petId, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("formation", formation);
            dic.Add("petId", petId);
            DoGet.getInstance().sendPost("/teamService/saveFormation", dic, (res) =>
            {
                //在服务端根据阵位信息判断哪些伙伴需要休息跟上阵，宠物也是

                //这里只需要更新缓存即可

                //todo:处理宠物页、伙伴页点击上阵和休息时改变阵位
                if (petId != null)
                {
                    JArray list = face.petInterface.getPetList();
                    foreach (object p in list)
                    {
                        JObject obj = (JObject)p;
                        if (!obj["Id"].ToString().Equals(petId))
                        {
                            obj["isFight"] = 0;
                        }
                        else
                        {
                            obj["isFight"] = 1;
                        }
                    }
                    face.petInterface.initPetList(list);
                }
                int num = 0;
                List<string> keys = new List<string>();
                IEnumerable<JProperty> properties = formation.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject a = (JObject)item.Value;
                    if ((int)a["type"] == 5) keys.Add(a["key"].ToString());
                    else if ((int)a["type"] == 1) num++;
                }
                if (num == 0)//不存在宠物上阵
                {
                    JArray list = face.petInterface.getPetList();
                    foreach (object p in list)
                    {
                        JObject obj = (JObject)p;
                        obj["isFight"] = 0;
                    }
                    face.petInterface.initPetList(list);
                }




                msgCode.showMsg(200);
                callback();
            });
        }
        /**查找阵型*/
        public void findFormation(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/teamService/findFormation", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**保存队长位置信息 */
        public void saveCaptainPos(JObject pos)
        {
            JObject team = this.getTeam();
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(team["captain"].ToString()))
                {
                    list[p]["pos"] = pos;
                    this.createTeamSuc(team);
                    break;
                }
            }
        }
        /**上传其跟随状态 */
        public void uploadStatus(int isFollow)
        {
            JObject r = face.roleInterface.getRole();
            JObject team = this.getTeam();
            if (team == null) return;
            JObject me = null;
            JObject captain = null;
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(team["captain"].ToString()))
                {
                    captain = (JObject)list[p];
                }
                else if (list[p]["name"].ToString().Equals(r["name"].ToString()))
                {
                    me = (JObject)list[p];
                }
            }
            //未跟随的情况下提示不在同一个地图
            if (isFollow==1&&!me["pos"]["map"].ToString().Equals(captain["pos"]["map"].ToString()))
            {
                msgCode.showMsg(620);
                return;
            }
            JObject obj = new JObject();
            obj.Add("Id", team["Id"].ToString());
            obj.Add("isFollow", isFollow);

            DoGet.getInstance().sendWs("/teamService/setPlayerFollowStatus", obj);
        }
        /**移交队长 */
        public void captainToName(string playerName)
        {
            JObject team = this.getTeam();
            if (team == null) return;
            JObject msg = new JObject();
            msg.Add("Id", team["Id"].ToString());
            msg.Add("playerName", playerName);
            DoGet.getInstance().sendWs("/teamService/captainToName", msg);
        }
        /**获取队长位置 */
        public JObject getCaptainPos()
        {
            JObject team = this.getTeam();
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(team["captain"].ToString()))
                {
                    return (JObject)list[p]["pos"];
                }
            }
            Debug.Log("not get captain pos");
            return null;
        }
        /**从玩家模型中获取队长位置 */
        public void findCaptainFromPlayer(Action callback)
        {
            //队伍存在并且不是队长
            JObject team = this.getTeam();
            if (team == null || this.isCaptain())
            {
                return;
            }
            JArray list = face.playerInterface.getPlayerList();
            //寻找跟队长相同的位置
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(team["captain"].ToString()))
                {
                    JArray members = (JArray)team["list"];
                    for (int j = 0; j < members.Count; j++)
                    {
                        if (members[j]["name"].ToString().Equals(team["captain"].ToString()))
                        {
                            JObject pos = new JObject();
                            pos.Add("pos", list[p]["pos"]);
                            pos.Add("map", members[j]["pos"]["map"]);

                            members[j]["pos"] = pos;
                            this.createTeamSuc(team);
                            callback();
                            break;
                        }
                    }
                    break;
                }
            }
        }
        /**判断是否跟队长在同一个地图 */
        public JObject isInSameMapKey()
        {
            string mapKey = null;
            JObject team = this.getTeam();
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (a["name"].ToString().Equals(team["captain"].ToString()))
                {
                    mapKey = a["pos"]["map"].ToString();
                    break;
                }
            }
            JObject res = new JObject();
            if (!mapKey.Equals(face.roleInterface.getRole()["pos"]["map"].ToString()))
            {
                res.Add("isSame", false);
                res.Add("mapKey", mapKey);
                return res;
            }
            else
            {
                res.Add("isSame", true);
                res.Add("mapKey", mapKey);
            }

            return res;
        }
        /**获取自身状态 */
        public JObject getMeFromTeam()
        {
            JObject r = face.roleInterface.getRole();
            JObject team = this.getTeam();
            if (team != null)
            {
                JArray list = (JArray)team["list"];
                for (int p = 0; p < list.Count; p++)
                {
                    JObject a = (JObject)list[p];
                    if (a["name"].ToString().Equals(r["name"].ToString()))
                    {
                        return a;
                    }
                }
            }
            return null;
        }
        /**同意入队 */
        public void agreeTeam(string name)
        {
            /*JObject r = face.roleInterface.getRole();
            if (!mapInterface.mapIsAllowedTeam(r.pos.map)) {
                msgCodeEvent.matchMsgCode({ code: 630 });
    //删除申请
    //this.delReq(obj.name);
    return;
            }*/
            if (this.isExistPlayer(name))
            {
                //MYCONST.tipUI.tipHtmlText('提示', [{ text: '数量不能超过3个！' }]);
                //删除申请
                //this.delReq(obj.name);
                return;
            }
            JObject team = this.getTeam();
            if (team == null)
            {
                msgCode.showMsg(702);
                return;
            }
            JObject msg = new JObject();
            msg.Add("Id", team["Id"].ToString());
            msg.Add("playerName", name);
            DoGet.getInstance().sendWs("/teamService/agreeTeam", msg);

            //删除申请
            //this.delReq(obj.name);
            //this.noticeUpdateReq();
        }
        /**同意邀请 */
        public void agreeInvite(string Id)
        {
            /*let r = roleInterface.getRole();
            if (!mapInterface.mapIsAllowedTeam(r.pos.map))
            {
                msgCodeEvent.matchMsgCode({ code: 630 });
                return;
            }*/
            JObject msg = new JObject();
            msg.Add("Id", Id);
            DoGet.getInstance().sendWs("/teamService/agreeInvite", msg);
        }
        /**
        * 主动申请入队
        * 点击玩家，点击入队申请，发送请求，服务器判断点击的玩家是否存在队伍，存在则申请，不存在则提示
        */
        public void reqTeam(string playerName)
        {
            JObject msg = new JObject();
            msg.Add("playerName", playerName);
            DoGet.getInstance().sendWs("/teamService/reqTeam", msg);
        }

        /**获取自身状态 */
        public JObject getSelfFromTeam()
        {
            JObject r = face.roleInterface.getRole();
            JObject team = this.getTeam();
            if (team != null)
            {
                JArray list = (JArray)team["list"];
                for (int p = 0; p < list.Count; p++)
                {
                    if (list[p]["name"].ToString().Equals(r["name"].ToString()))
                    {
                        return (JObject)list[p];
                    }
                }
            }
            return null;
        }
        /**判断自己是否为队长 */
        public bool isCaptain()
        {
            JObject r = face.roleInterface.getRole();
            JObject team = this.getTeam();
            if (team != null && team["captain"].ToString().Equals(r["name"].ToString())) return true;
            return false;
        }
        public bool isFollow()
        {
            JObject r = face.roleInterface.getRole();
            JObject team = this.getTeam();
            if (team != null)
            {
                JArray list = (JArray)team["list"];
                for (int p = 0; p < list.Count; p++)
                {
                    if (list[p]["name"].ToString().Equals(r["name"].ToString()) && (int)list[p]["isFollow"] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /**是否已经在队伍中 */
        public bool isInTeam()
        {
            if (this.getTeam() != null) return true;
            return false;
        }
        /**清除队伍缓存 */
        public void delTeam()
        {
            dbHandle.del("team");
            this.noticeUpdateTeam();
        }
        /**入队成功 */
        public void inTeamSuc(JObject team)
        {
            this.createTeamSuc(team);
            this.noticeUpdateTeam();
        }
        /**离开队伍 */
        public void leaveTeam(string playerName)
        {
            JObject team = this.getTeam();
            if (team == null) return;
            JObject msg = new JObject();
            msg.Add("playerName", playerName);
            DoGet.getInstance().sendWs("/teamService/leaveTeam", msg);
        }
        /**邀请入队 */
        public void inviteTeam(string playerName)
        {
            JObject r = face.roleInterface.getRole();
            //判断该场景是否允许组队
            /*if (!mapInterface.mapIsAllowedTeam(r.pos.map))
            {
                msgCodeEvent.matchMsgCode({ code: 630 });
                return;
            }*/
            if (playerName == r["name"].ToString())
            {
                return;
            }
            if (this.isExistPlayer(playerName))
            {
                //msgCodeEvent.matchMsgCode({ code: 626 });
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/teamService/inviteTeam", dic, (res) =>
            {
                this.createTeamSuc((JObject)res);
            });

        }
        /**判断队伍成员中是否已经存在该玩家，且数量<4 */
        public bool isExistPlayer(string playerName)
        {
            JObject team = this.getTeam();
            if (team == null) return false;
            JArray list = (JArray)team["list"];
            if (list.Count > 2) return false;
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(playerName))
                {
                    return true;
                }
            }
            return false;
        }
        /**创建队伍成功后处理 */
        public void createTeamSuc(JObject r)
        {
            dbHandle.save("team", r);
            //通知ui更新
            this.noticeUpdateTeam();
        }
        /**更新ui */
        public void noticeUpdateTeam()
        {
            JObject msg = new JObject();
            msg.Add("callback", "804");
            eventsUtils.dispatchEvent("ws", msg);
        }
        /**创建队伍 */
        public void createTeam()
        {
            //JObject r = face.roleInterface.getRole();
            //TODO:判断该场景是否允许组队
            /*if (!mapInterface.mapIsAllowedTeam(r.pos.map))
            {
                msgCodeEvent.matchMsgCode({ code: 630 });
                return;
            }*/
            DoGet.getInstance().sendPost("/teamService/createTeam", null, (res) =>
            {
                this.createTeamSuc((JObject)res);

            });

        }
        public void initTeam()
        {
            dbHandle.del("team");

        }
        public JObject getTeam()
        {
            return dbHandle.get<JObject>("team");
        }
    }
}
