using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Newtonsoft.Json.Linq;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface friendInterface
    {
        public void delFriend(string playerName, Action callback);
        public JArray readFriendList();
        public void initFriendList(JArray list);
        public void addFriend(string playerName, Action callback);

        public bool isFriend(string playerName);
        public void saveFriend(JObject one);
        public void searchMore(string name, Action<JArray> callback);
        public void search(string name, Action<object> callback);



    }
    class friendInterfaceImpl : friendInterface
    {
        /**删除好友 */
        public void delFriend(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/friendService/delOne", dic, (res) =>
            {
                JArray list = this.readFriendList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i]["name"].ToString().Equals(playerName))
                    {
                        list.RemoveAt(i);
                        this.initFriendList(list);
                        break;
                    }
                }
                callback();
            });

        }
        /**读取好友列表 */
        public JArray readFriendList()
        {
            return dbHandle.get<JArray>("friendList");
        }
        /**初始化列表 */
        public void initFriendList(JArray list)
        {
            dbHandle.save("friendList", list);
        }
        /**添加好友 */
        public void addFriend(string playerName, Action callback)
        {
            if (this.isFriend(playerName))
            {
                msgCode.showMsg(771);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/friendService/addOne", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(771);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(772);
                }
                else
                {
                    //保存到缓存中
                    this.saveFriend((JObject)res);
                    if (callback != null)
                    {
                        callback();
                    }
                    msgCode.showMsg(773);
                }

            });
        }
        public bool isFriend(string playerName)
        {
            JArray list = this.readFriendList();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["name"].ToString().Equals(playerName))
                {
                    return true;
                }
            }
            return false;
        }
        public void saveFriend(JObject one)
        {
            JArray list = this.readFriendList();
            list.Add(one);
            this.initFriendList(list);
        }
        /**模糊搜索
         * 只返回3个结果
         */
        public void searchMore(string name, Action<JArray> callback)
        {
            if (strUtils.isNull(name) || this.isFriend(name) ||
                name.Equals(face.roleInterface.getRole()["name"].ToString()))
            {
                callback(null);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/friendService/getMore", dic, (res) =>
            {
                callback((JArray)res);
            });
        }
        public void search(string name, Action<object> callback)
        {
            if (strUtils.isNull(name) || this.isFriend(name) ||
                name.Equals(face.roleInterface.getRole()["name"].ToString()))
            {
                callback(null);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/friendService/getOne", dic, (res) =>
            {
                callback((JArray)res);
            });

        }
    }
}
