using Assets.HotFix.MyUtils.src;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory.basicObj;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class start
{
    public static void run(string ip, string isLocal, string apkVer)
    {
        
        if (!strUtils.apkVer.Equals(apkVer))
        {
            sysUtils.isOutLogToScreen = true;
            Debug.Log("有新的apk版本，请到qq群上下载安装！");
            return;
        }
        //1080*2460为基准
        //设置窗口大小
        //float dpi = Screen.dpi;
        //float k = 1080 / 2460f;
        //Screen.SetResolution(800, (int)(800/k), true);


        //获取服务器所有存在的游戏
        //本地则采取屏幕输出日志
        if (isLocal == "1")
            sysUtils.isOutLogToScreen = true;

        sysUtils.projRootDir = "xq2d";
        sysUtils.isPixelPic = true;//设置图片模式为像素点
        Application.targetFrameRate = 60;
        strUtils.ip = ip;

        //窗口大小改变时的回调处理
        ScreenAutoAdapt.setWindowChangeCall(() =>
        {
            if (PointGet.getIndexPage() != null)
            {
                Transform a = PointGet.getIndexPage().Find("Map");
                if (a != null)
                {
                    //重新计算地图大小
                    a.GetComponent<Move>().init();
                    mapManager.getInstance().reDrawMap(()=> { });
                }
            }
        });

        //游戏对象装载到main
        GameObject go = new GameObject("main");
        go.AddComponent<ReqSendHandle>();
        go.AddComponent<wsSendHandle>();
        go.AddComponent<DoGet>();
        go.AddComponent<Res3dLoadHandle>();
        go.AddComponent<MsgRecHandle>().addCall((msg) =>
        {
            eventsUtils.dispatchEvent("ws", msg);
        }, () =>
        {
            if (PointGet.getIndexPage() != null || PointGet.getFightPage() != null)
            {
                netErrorUI.create().show();
            }
        });
        go.AddComponent<timeManager>();
        go.AddComponent<ScreenConsole>();
        //go.AddComponent<BackgroundHandle>();
        GameWindowHandle gwh = go.AddComponent<GameWindowHandle>();

        //初始化屏幕比例
        ScreenUtils.initSize(960, 1440);//320*480

        //空闲对象装载到这里
        GameObject free = new GameObject("free");
        free.SetActive(false);
        free.transform.SetParent(go.transform, false);
        //创建活动窗口
        gwh.openNewActivity(() =>
        {
            netUtils.getInstance().gameStartInit();
            PageUI.create<LoadingPage>().drawUI();
            /*if (true)
            {
                sysUtils.isOutLogToScreen = true;
                AndroidJavaClass jclass = new AndroidJavaClass("android.os.Environment");
                AndroidJavaObject jobj = jclass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
                string path = jobj.Call<string>("getAbsolutePath");
                //创建QQ目录
                string dir = path + "/Download/QQ/test.apk";
                aarUtils.showToast(dir);
                aarUtils.InstallApkFile(dir);
                return;
            }*/
        });
        //下方多开按钮
        /*gwh.createDuoKaiBar(()=> {
            PageUI.create<LoadingPage>().drawUI();
        });*/
        //aarUtils.addService();

    }

}

