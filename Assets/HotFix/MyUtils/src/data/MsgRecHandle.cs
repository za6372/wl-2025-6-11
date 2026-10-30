using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    /**消息接收并处理
    */
    public class MsgRecHandle : MonoBehaviour
    {
        private static bool isNetErr = false;
        public static Queue<JObject> msgs = new Queue<JObject>();
        private bool isDoing = false;
        private Action<JObject> call;
        private Action errCall;
        private void Update()
        {
            if (msgs.Count > 0 && !isDoing)
            {
                isDoing = true;
                call(msgs.Dequeue());
                isDoing = false;
            }
            if (isNetErr)
            {
                isNetErr = false;
                errCall();
            }

        }
        public MsgRecHandle addCall(Action<JObject> call,Action errCall)
        {
            this.call = call;
            this.errCall = errCall;
            return this;
        }
        public static void setNetErr()
        {
            isNetErr = true;
        }
        public static void addMsg(JObject res)
        {
            msgs.Enqueue(res);
        }
        private void OnDestroy()
        {
            //netUtils.getInstance().Dispose();
        }
    }
}
