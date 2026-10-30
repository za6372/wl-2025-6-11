using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.common
{
    public class ScreenAutoAdapt : MonoBehaviour
    {
        private static Action windowChangeCall;

        public static void setWindowChangeCall(Action fn)
        {
            windowChangeCall = fn;
        }

        public List<GameObject> cavList = null;

        void Start()
        {
            //this.resetScreen();
        }
        public void addCavs(List<GameObject> cav)
        {
            cavList = cav;
        }
        public void resetScreen()
        {
            foreach (GameObject g in cavList)
            {
                CanvasScaler cs = g.GetComponent<CanvasScaler>();
                cs.scaleFactor = ScreenUtils.screenScaleRate;
            }
        }
        private int lastWidth = 0;
        private int lastHeight = 0;
        private long old;
        private long now;
        void Update()
        {
            if (cavList == null) return;
            if (Screen.width != lastWidth || Screen.height != lastHeight)
            {

                ScreenUtils.resetSize();
                resetScreen();
                now = strUtils.getMillis();
                lastWidth = Screen.width;
                lastHeight = Screen.height;
            }
            else
            {
                if (now - old > 3000)
                {
                    old = now;
                    foreach (GameObject g in cavList)
                    {
                        if (g.transform.childCount > 0)
                        {
                            Transform ts = g.transform.GetChild(0);
                            SetGameObj.setLayerCenter(ts.gameObject);
                        }
                    }
                    if (windowChangeCall != null) windowChangeCall();

                }
            }


        }
    }
}
