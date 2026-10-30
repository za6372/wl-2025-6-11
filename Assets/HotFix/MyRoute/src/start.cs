using Assets.HotFix.MyRoute.src;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start 
{
    /**这个模块只做其他模块的路由*/
    public static void run(string ip, string isLocal,string apkVer)
    {
        Transform cas= GameObject.Find("camera2d").transform.Find("canvas");
        //GameObject.Find("camera2d").AddComponent<ScreenConsole>();
        cas.gameObject.AddComponent<route>().init(ip, isLocal, apkVer);
    }
    
}
