using Assets.HotFix.xq2d.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.factory.playerObjBehaviour
{
    /**模型身上的一些数据绑定，方便从模型中获取相关数据*/
    public class modelMsgBind : MonoBehaviour
    {
        public ModelMsg msg;
        public bool isTouch;//是否允许点击
        /**非战斗时使用*/
        public modelMsgBind addModelMsg(ModelMsg msg)
        {
            this.msg = msg;
            return this;
        }

    }
}
