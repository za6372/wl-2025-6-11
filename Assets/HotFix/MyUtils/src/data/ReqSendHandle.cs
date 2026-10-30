using Assets.HotFix.MyUtils.src.common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    /**处理所有http请求*/
    public class ReqSendHandle : MonoBehaviour
    {
        /**加入请求队列*/
        private Queue<Dictionary<string, object>> msgs = new Queue<Dictionary<string, object>>();
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
        IEnumerator test()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            yield return new WaitForSecondsRealtime(1);
        }
        void startReq(Dictionary<string, object> dic)
        {
            string path = dic["path"].ToString();
            Dictionary<string, object> data = (Dictionary<string, object>)dic["data"];
            Action<object> ac = (Action<object>)dic["ac"];
            StartCoroutine(netAbstract.getInstance().sendPost(path, data, (res) =>
            {
                try
                {
                    ac(res);
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                finally
                {
                    //自动删除过期的请求
                    this.delOverTimePost();
                }
            }, () =>
            {
                try
                {
                    if (dic.ContainsKey("failCall") && dic["failCall"] != null)
                    {
                        Action failCall = (Action)dic["failCall"];
                        failCall();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                finally
                {
                    //自动删除过期的请求
                    this.delOverTimePost();
                }
            }));
        }
        public void addReq(string path, Dictionary<string, object> data, Action<object> ac, Action failCall = null)
        {
            //判断队列中是否已经存在相同的请求，把相同的请求给过滤，只保留一个
            if (this.isSamePost(path, data))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("path", path);
            dic.Add("data", data);
            dic.Add("ac", ac);
            dic.Add("failCall", failCall);
            msgs.Enqueue(dic);
        }
        /**加入请求队列*/
        private Dictionary<string, object> postQueue = new Dictionary<string, object>();
        private bool isSamePost(string path, Dictionary<string, object> data)
        {
            string id = this.getPostId(path, data);
            long now = strUtils.getMillis();
            //小于100ms的相同请求给过滤
            if (postQueue.ContainsKey(id) && now - (long)postQueue[id] < 300)
            {
                postQueue[id] = now;
                return true;
            }
            /*long old = postQueue.ContainsKey(id) ? (long)postQueue[id] : 0;
            Debug.Log(path + "/" + now + "/" + (now - old));*/
            /*if (path.Contains("getPlayerPos"))
                Debug.Log(id + "/" + now);*/
            //大于100ms的进行放行
            if (!postQueue.ContainsKey(id)) postQueue.Add(id, now);
            postQueue[id] = now;
            return false;
        }
        /**将请求转化成唯一id*/
        private string getPostId(string path, Dictionary<string, object> data)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("path", path);
            if (data != null) dic.Add("json", data);
            return JsonConvert.SerializeObject(dic);
        }
        private void delOverTimePost()
        {
            long now = strUtils.getMillis();
            List<string> arr = new List<string>();
            foreach (string p in postQueue.Keys)
            {
                if (now - (long)postQueue[p] > 300) arr.Add(p);
            }
            for (int i = 0; i < arr.Count; i++)
            {
                postQueue.Remove(arr[i]);
            }
            arr.Clear();
        }
    }
}
