using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;

namespace Assets.HotFix.MyUtils.src.data
{
    /**后台运行处理*/
    public class BackgroundHandle : MonoBehaviour
    {
        /*void Start()
        {
            // 允许应用在后台运行而不自动暂停
            Application.runInBackground = true;
        }*/
        void Start()
        {
            StartService();
        }

        void StartService()
        {
            if (Application.platform == RuntimePlatform.Android)
            {

                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                // 创建Intent并启动Service
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", activity, new AndroidJavaClass("com.weilai.mylibrary.MyForegroundService"));
                //activity.Call("startService", intent);
                activity.Call("startForegroundService", intent);

            }
        }
        /*void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // 应用进入后台的逻辑
                Debug.Log("应用进入后台");
            }
            else
            {
                // 应用从后台恢复的逻辑
                Debug.Log("应用从后台恢复");
            }
        }*/
    }
}
