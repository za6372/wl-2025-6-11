using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    /**ws发送请求处理*/
    public class wsSendHandle:MonoBehaviour
    {
        private Queue<JObject> msgs = new Queue<JObject>();
        private bool isDoing = false;
        void Start()
        {
            StartCoroutine(ExecuteEverySecond());
        }

        IEnumerator ExecuteEverySecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);

                if (isDoing || msgs.Count == 0)
                {

                    continue;
                }
                isDoing = true;

                try
                {
                    startReq(msgs.Dequeue());
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                isDoing = false;
            }
        }
        void startReq(JObject a)
        {
            //Debug.Log(a["path"].ToString()+"=>"+a["data"]);
            netAbstract.getInstance().sendWs(a["path"].ToString(),(JObject)a["data"]);
        }
        public void addReq(string path,JObject data)
        {
            JObject a = new JObject();
            a.Add("path", path);
            a.Add("data", data);
            msgs.Enqueue(strUtils.copyJSON<JObject>(a));
        }
    }
}
