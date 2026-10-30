using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    public class netUtils : netAbstract
    {
        public new static netUtils getInstance()
        {
            if (one == null) one = new netUtils();
            return (netUtils)one;
        }
        /**游戏入口处就要调用*/
        public void gameStartInit()
        {
            this.addSkipWsOpenCheck(
                "/loginService/login", "/manService/loadMsg", "/sysService/getCode",
                "/sysService/matchCode", "/loginService/createRole", "/ipService/gainIP",
                "/putLog"
                );
        }
        /**初始化ws*/
        public void initWs(Action callback)
        {
            if (this.ws != null && this.ws.State == WebSocketState.Open)
            {
                callback();
                return;
            }
            defindEvent.addUserGoods();
            defindEvent.addWs();
            defindEvent.addElse();
            this.wsCloseCall = () => { MsgRecHandle.setNetErr(); };
            this.recvMsgCall = (res) => {
                MsgRecHandle.addMsg(res); 
            };
            this.reqErrCall = (code) => { msgCode.showMsg(code); };
            this.openWs(() =>
            {
                callback();
            });
        }


    }
}
