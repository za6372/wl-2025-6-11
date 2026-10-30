using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common.eventCall
{
    class elseCallback
    {
        private static elseCallback ws;
        private bool isDoingC = false;
        public static elseCallback getInstance()
        {
            if (ws == null) ws = new elseCallback();
            return ws;
        }
        public void call(Dictionary<string, object> obj)
        {
            string callbackCode = (string)obj["callback"];
            object msg = obj["msg"];
            switch (callbackCode)
            {
                //移动中碰怪及刷新坐标
                case "100":
                    {
                        if (isDoingC) return;
                        isDoingC = true;
                        //靠近npc时半透明
                        buildManager.isNearNpc();
                        //自动生成怪物
                        MonsterCreateHandle.getInstance().create();
                        isDoingC = false;
                        return;
                    }
            }
        }
    }
}
