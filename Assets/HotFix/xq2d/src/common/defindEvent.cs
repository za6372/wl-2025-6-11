using Assets.HotFix.xq2d.src.common.eventCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Assets.HotFix.MyUtils.src.common;

namespace Assets.HotFix.xq2d.src.common
{
    /**自定义事件，需进入游戏主页时添加*/
    public class defindEvent : MonoBehaviour
    {

        /**添加使用道具的监听 
             * 道具使用时只需要调取code即可
            */
        public static void addUserGoods()
        {
            /*model.eventListener.addEventListener('userGoods', assembly, (e: model.dataEvent) => {
                //必传{key,msg:{res,num}}
                model.goodsUser.Instance.analysis(e.msg.key, e.msg.msg);
            });*/
            eventsUtils.addEventListener("userGoods", (e) =>
            {
                if (e == null || ((JObject)e)["msg"] == null)
                {
                    Debug.Log("空消息");
                }
                else
                {
                    try
                    {
                        userGoodsCallback.getInstance().call(((JObject)e)["msg"]);
                    }
                    catch (Exception ep)
                    {
                        Debug.Log(ep.StackTrace);
                    }
                    
                }

            });
        }
        /**ws监听 */
        public static void addWs()
        {
            eventsUtils.addEventListener("ws", (e) =>
            {
                
                if (e == null || ((JObject)e)["msg"] == null)
                {
                    Debug.Log("空消息");
                }
                else
                {

                    //注意：不捕获异常会导致ws无法使用（即卡死，无法触发战斗等相关的ws事件）
                    //wsCallback.getInstance().call(((JObject)e)["msg"]);
                    try
                    {
                        wsCallback.getInstance().call(((JObject)e)["msg"]);
                    }
                    catch (Exception ep)
                    {
                        Debug.Log(ep.StackTrace);
                    }

                }

            });
        }
        public static void addElse()
        {
            eventsUtils.addEventListener("else", (e) =>
            {
                try
                {
                    elseCallback.getInstance().call((Dictionary<string, object>)e);
                }
                catch (Exception ep)
                {
                    Debug.Log(ep.StackTrace);
                }
                

            });
        }
        /**ui点击的监听*/
        /*public static void addUiClk()
        {
            eventsUtils.addEventListener("uiClk", (e) =>
            {
                uiClkCallback.getInstance().call(e["msg"]);
            });
        }*/

    }
}
