using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface msgInterface
    {
        public void init();
        public void receive(JObject msg);
        public JArray read();
        public void del(string Id);
        public JArray readMsg(int type);
    }
    public class msgInterfaceImpl : msgInterface
    {
        public void init()
        {
            dbHandle.save("msg_center", new JArray());
        }
        public void receive(JObject msg)
        {
            JArray arr = dbHandle.get<JArray>("msg_center");
            //重复的消息替换
            for (int i = 0; i < arr.Count; i++)
            {
                if ((int)msg["type"] == (int)arr[i]["type"] &&
                    msg["params"]["name"].ToString().Equals(arr[i]["params"]["name"].ToString()))
                {
                    arr.RemoveAt(i);
                    break;
                }
            }
            arr.Add(msg);
            dbHandle.save("msg_center", arr);
        }
        public JArray readMsg(int type)
        {
            JArray arr = new JArray();
            JArray list = this.read();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if ((int)obj["type"] == type) arr.Add(obj);
            }
            return arr;
        }
        public JArray read()
        {
            JArray arr = dbHandle.get<JArray>("msg_center");

            return arr;
        }
        public void del(string Id)
        {
            JArray arr = dbHandle.get<JArray>("msg_center");
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i]["Id"].ToString().Equals(Id))
                {
                    arr.RemoveAt(i);
                    dbHandle.save("msg_center", arr);
                    break;
                }
            }
        }
    }
}
