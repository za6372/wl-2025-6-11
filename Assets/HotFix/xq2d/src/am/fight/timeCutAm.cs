using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.am.fight
{
    /**战斗倒计时*/
    class timeCutAm : MonoBehaviour
    {
        private int sum = 0;
        private long startTime;
        private bool isCallCancel;
        private Action cancelCall;
        public timeCutAm init()
        {
            startTime = strUtils.getMillis();

            return this;
        }
        /**自动的情况下，前4秒可按取消，不按就自动下达命令*/
        public timeCutAm addCancelCall(Action cancelCall)
        {
            this.cancelCall = cancelCall;
            return this;
        }
        /**重新倒计时*/
        public void resetTime()
        {
            this.startTime = strUtils.getMillis();
            this.isCallCancel = false;
            this.sum = 30;
        }
        private void LateUpdate()
        {
            if (sum <= 0) return;
            long t = strUtils.getMillis();
            int a = 30 - (int)((t - startTime) / 1000);

            if (sum != a)
            {
                sum = a;
                this.GetComponent<TextImgUI>().setText("" + sum);
            }
            if (sum < 26 && !isCallCancel)
            {
                this.isCallCancel = true;
                if (cancelCall != null)
                {
                    cancelCall();
                }
            }
        }
    }
}
