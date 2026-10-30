using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface chatInterface
    {
        public void getTimeMsg();
        public void sendMsg(string receiver, string content, string channel, string head, string fj, JObject en);
        public void receiveMsg(object msg);
        public void putTime(string created);
        public void saveMsg(JObject msg);

        public JArray getMsg(string sender, string channel);
        public void initChat();
        public void writeSysMsg(string msg, int channel = 0);
        public void writeMsg(string sender, string msg, int channel, int type, string head, JObject en);
        public void putSxCache(string receiver, string content, string head, JObject en);
        public void handleMsgQueue();
        public JArray getZongHeSiXinMsg();
    }
    public class chatInterfaceImpl : chatInterface
    {
        private Dictionary<string, object> msg = new Dictionary<string, object>{
        { "created", null },
        { "time", strUtils.getMillis() }

    };
        private JArray msgList = new JArray();
        /**放置拉取消息的时间点 */
        public void putTime(string created)
        {
            if (this.msg["created"] != null) return;
            this.msg["created"] = created;

        }
        /**向服务器拉取消息 */
        public void getTimeMsg()
        {
            long now = strUtils.getMillis();
            //Debug.Log("===>" + (now - long.Parse(this.msg["created"]+"")));
            if (this.msg["created"] == null || now - long.Parse(this.msg["created"] + "") < 5000L) return;
            long created = long.Parse(this.msg["created"] + "");
            this.msg["created"] = null;
            //赋予新的时间点
            this.msg["time"] = now;
            JObject obj = new JObject();
            obj.Add("created", created);
            //Debug.Log("拉取消息");
            DoGet.getInstance().sendWs("/chatService/getChatMsg", obj);

        }
        /**发送消息*/
        public void sendMsg(string receiver, string content, string channel, string head, string fj,JObject en)
        {
            if (content.Contains("赔") || content.Contains("赌") || content.Contains("h267471"))
            {
                return;
            }
            JObject role = face.roleInterface.getRole();
            JObject dic = new JObject();
            dic.Add("sender", role["name"]);
            dic.Add("receiver", receiver);
            dic.Add("content", content);
            dic.Add("channel", channel);
            dic.Add("head", head);
            dic.Add("fj", fj);//红包才使用
            dic.Add("en", en);//附件 {Id.name}
            DoGet.getInstance().sendWs("/chatService/send", dic);
        }
        public void receiveMsg(object msg)
        {
            //先放入消息队列，然后一个一个的保存
            msgList.Add((JObject)msg);

        }
        public void handleMsgQueue()
        {
            if (msgList.Count == 0) return;
            JArray list = strUtils.copyJSON<JArray>(msgList);
            msgList.Clear();
            foreach (object l in list)
            {
                this.saveMsg((JObject)l);
            }

            //将消息刷出来
            eventsUtils.dispatchWsEvent("106", null);

        }
        public void initChat()
        {
            dbHandle.del("chat");
        }
        /**缓存聊天信息 */
        public void saveMsg(JObject msg)
        {
            //todo:bug接收时出现了问题，导致json不完整，可能是ws接收问题，也可能是多条消息同时保存
            //channel 0-5 {channel:[]} / 10 {channel:{name:[]}}
            JObject chat = dbHandle.get<JObject>("chat");
            if (chat == null)
            {
                chat = new JObject();
            }
            string k = msg.GetValue("channel") + "";
            if (k.Equals("10"))
            {
                if (chat[k] == null) chat[k] = new JObject();
                JObject a = (JObject)chat[k];
                if (a.GetValue(msg["sender"].ToString()) == null)
                {
                    a.Add(msg["sender"].ToString(), new JArray());
                }

                JArray arr = (JArray)a[msg["sender"].ToString()];
                arr.Add(msg);
                if (arr.Count > 30)
                {
                    arr.RemoveAt(0);
                }
            }
            else
            {
                if (chat.GetValue(k) == null)
                {
                    chat.Add(k, new JArray());
                }

                JArray arr = (JArray)chat.GetValue(k);
                arr.Add(msg);
                if (arr.Count > 30)
                {
                    arr.RemoveAt(0);
                }
            }

            dbHandle.save("chat", chat);
        }
        /**缓存自己发送的私信*/
        public void putSxCache(string receiver, string content, string head, JObject en)
        {
            JObject msg = new JObject();
            msg.Add("Id", strUtils.getId());
            msg.Add("sender", face.roleInterface.getRole()["name"].ToString());
            msg.Add("receiver", receiver);
            msg.Add("content", content);
            msg.Add("created", strUtils.getMillis());
            msg.Add("type", 0);
            msg.Add("head", head);
            msg.Add("en", en);//附件 {Id.name}

            JObject chat = dbHandle.get<JObject>("chat");
            if (chat == null)
            {
                chat = new JObject();
            }
            if (chat["10"] == null) chat["10"] = new JObject();
            JObject a = (JObject)chat["10"];
            if (a.GetValue(msg["receiver"].ToString()) == null)
            {
                a.Add(msg["receiver"].ToString(), new JArray());
            }

            JArray arr = (JArray)a[msg["receiver"].ToString()];
            arr.Add(msg);
            if (arr.Count > 30)
            {
                arr.RemoveAt(0);
            }

            dbHandle.save("chat", chat);
        }
        /**综合私信*/
        public JArray getZongHeSiXinMsg()
        {
            JArray arr = new JArray();
            JObject chat = dbHandle.get<JObject>("chat");
            if (chat == null|| chat["10"] == null) return arr;
            JObject sx = (JObject)chat["10"];
            IEnumerable<JProperty> props= sx.Properties();
            foreach(JProperty p in props)
            {
                JArray al = (JArray)p.Value;
                arr.Merge(al);
            }
            arr = new JArray(arr.OrderBy(obj => (string)obj["created"]));
            return new JArray(arr.Take(30));
        }
        /**读取消息 */
        public JArray getMsg(string sender, string channel)
        {
            JObject chat = dbHandle.get<JObject>("chat");
            if (chat == null) return new JArray();
            if (channel == "10")
            {
                if (chat[channel] == null) return new JArray();
                JArray arr = (JArray)chat[channel][sender];
                if (arr == null)
                {
                    arr = new JArray();
                }
                return arr;
            }
            else
            {
                JArray arr = (JArray)chat[channel];
                arr = arr == null ? new JArray() : arr;
                if (arr.Count > 30)
                {
                    int len = arr.Count - 30;
                    for (int i = 0; i < len; i++)
                    {
                        arr.RemoveAt(0);
                    }
                }
                return arr;
            }
        }
        /**写入系统消息，比如：战斗奖励 */
        public void writeSysMsg(string msg, int channel = 0)
        {

            this.writeMsg("系统", msg, channel, 0, "001_png", null);
        }
        public void writeMsg(string sender, string msg, int channel, int type, string head, JObject en)
        {
            JObject obj = new JObject();
            obj.Add("sender", sender);
            //obj.Add("receiver", face.roleInterface.getRole()["name"].ToString());
            obj.Add("channel", channel);
            obj.Add("head", head);
            obj.Add("content", msg);
            obj.Add("created", strUtils.getMillis());
            obj.Add("type", type);
            obj.Add("en", en);//附件 {Id.name}

            this.receiveMsg(obj);

        }

    }
}
