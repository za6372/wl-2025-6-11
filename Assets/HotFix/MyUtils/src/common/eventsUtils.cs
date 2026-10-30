using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.MyUtils.src.common
{
    public class eventsUtils
    {
        private static Dictionary<string, Action<object>> es = new Dictionary<string, Action<object>>();
        /**绑定事件*/
        public static void addEventListener(string code, Action<object> callBack)
        {
            //如果类型已经存在(键相等)
            if (!es.ContainsKey(code))
            {
                es.Add(code, callBack);
            }
        }
        /*public static void addEventListener<T>(string code, Action<T> callBack)
        {
            //如果类型已经存在(键相等)
            if (!es.ContainsKey(code))
            {
                es.Add(code, callBack);
            }
        }*/
        public static void remEventListener(string code)
        {
            es.Remove(code);
        }
        /**触发事件
         code ws\userGoods
         */
        public static void dispatchEvent(string code, JObject msg)
        {
            JObject obj = new JObject();
            obj.Add("code", code);
            obj.Add("msg", msg);
            es[code](obj);
        }
        /**触发ws事件专用
         msg是需要传输的参数，无需二次封装
        callbackCode 如：1000等
         */
        public static void dispatchWsEvent(string callbackCode, JToken msg)
        {
            JObject j = new JObject();
            j.Add("callback", callbackCode);
            j.Add("msg", msg);
            dispatchEvent("ws", j);
        }
        /**触发else事件*/
        public static void dispatchElseEvent(string callbackCode, object msg)
        {
            Dictionary<string, object> j = new Dictionary<string, object>();
            j.Add("callback", callbackCode);
            j.Add("msg", msg);
            es["else"](j);
        }

    }
}
