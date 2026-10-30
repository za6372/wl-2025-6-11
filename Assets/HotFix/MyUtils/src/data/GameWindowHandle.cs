using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory.basicObj;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    /**多开时使用，游戏窗口对象*/
    public class GameWindowHandle : MonoBehaviour
    {
        //所有多个的窗口对象
        public List<GameObject> wList = new List<GameObject>();
        //当前活动窗口指向
        public int index;
        public static GameWindowHandle getOne()
        {
            return GameObject.Find("main").transform.GetComponent<GameWindowHandle>();
        }
        /**获取当前活动的窗口*/
        public Transform getNowActivity()
        {
            return this.wList[index].transform;
        }
        public Transform getActivity(int p)
        {
            return this.wList[p].transform;
        }
        /**打开指定的窗口*/
        public void openActivity(int p)
        {
            this.index = p;
            for (int i = 0; i < this.wList.Count; i++)
            {
                if (i == p) this.wList[i].transform.localScale=Vector2.one;
                else this.wList[i].transform.localScale = Vector2.zero;
            }
        }
        public void openNewActivity(Action ac)
        {
            createActivity();
            openActivity(this.wList.Count - 1);
            ac();
        }
        /**创建新窗口*/
        private Transform createActivity()
        {
            GameObject go = GameObject.Find("main");
            if (go == null)
            {
                Debug.Log("主节点main不存在！");
            }
            //活动窗口节点
            GameObject activity = new GameObject("activity-" + wList.Count);
            activity.transform.SetParent(go.transform, false);
            ScreenAutoAdapt sca=activity.AddComponent<ScreenAutoAdapt>();

            GameObject camera2d = cameraObj.getInstance("camera2d")
                .init(2, 1 << 5).gameObject;
            camera2d.transform.SetParent(activity.transform, false);
            camera2d.GetComponent<Camera>().backgroundColor = Color.black;

            GameObject canvas = new GameObject("canvas");
            canvas.transform.SetParent(camera2d.transform, false);
            canvas.AddComponent<CanvasUI>().init(RenderMode.ScreenSpaceOverlay);

            GameObject tipCanvas = new GameObject("tipCanvas");
            tipCanvas.transform.SetParent(camera2d.transform, false);
            tipCanvas.AddComponent<CanvasUI>().init(RenderMode.ScreenSpaceOverlay, 1);

            //滤镜区
            GameObject filterCanvas = new GameObject("filterCanvas");
            filterCanvas.transform.SetParent(camera2d.transform, false);
            filterCanvas.AddComponent<CanvasUI>().init(RenderMode.ScreenSpaceOverlay, 2);

            sca.addCavs(new List<GameObject>() { canvas , tipCanvas, filterCanvas });

            this.wList.Add(activity);
            return activity.transform;
        }
        /**多开的工具条*/
        public void createDuoKaiBar(Action ac)
        {
            GameObject go = GameObject.Find("main");
            if (go == null)
            {
                Debug.Log("主节点main不存在！");
            }
            //活动窗口节点
            GameObject activity = new GameObject("duokai");
            activity.transform.SetParent(go.transform, false);

            GameObject camera2d = cameraObj.getInstance("camera2d")
                .init(2, 1 << 5).gameObject;
            camera2d.transform.SetParent(activity.transform, false);
            camera2d.GetComponent<Camera>().backgroundColor = Color.black;

            GameObject canvas = new GameObject("canvas");
            canvas.transform.SetParent(camera2d.transform, false);
            canvas.AddComponent<CanvasUI>().init(RenderMode.ScreenSpaceOverlay, 3);

            GameObject scroll = new GameObject("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(canvas.transform, false);
            scroll.GetComponent<ScrollUI>().init();//非对象池管理的方式创建则需要调用一次初始化
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(Screen.width, 100), new Vector2(0, -Screen.height + 100));
            scroll.GetComponent<ScrollUI>().initSetting(true, false);

            GameObject items = new GameObject("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);

            GameObject add = new GameObject("add", typeof(BtnUI));
            add.transform.SetParent(items.transform, false);
            add.GetComponent<BtnUI>().init();
            add.GetComponent<BtnUI>().setColor("#ffffff")
                .setSizePos(new Vector2(100, 100), new Vector2(0, 0));
            add.GetComponent<BtnUI>().addText("+", 36).setTextColor("#000000")
            .addClk(() =>
            {
                openNewActivity(ac);
                //追加一个窗口切换按钮
                addQieBtn(items);
            });

            addQieBtn(items);
        }
        private void addQieBtn(GameObject items)
        {
            int len = items.transform.childCount;
            GameObject qie = new GameObject("qie", typeof(BtnUI));
            qie.transform.SetParent(items.transform, false);
            qie.GetComponent<BtnUI>().init();
            qie.GetComponent<BtnUI>().setColor("#ffffff")
                .setSizePos(new Vector2(100, 100), new Vector2(110 * len, 0));
            qie.GetComponent<BtnUI>().addText(len.ToString(), 36).setTextColor("#000000")
            .addClk(() =>
            {
                openActivity(len-1);
            });
        }
    }
}
