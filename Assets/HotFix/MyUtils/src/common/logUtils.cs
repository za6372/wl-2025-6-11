using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    public class logUtils
    {
        /*private static string dir;

        public static void logPrint(string str)
        {
            Debug.Log(str);

            if (sysUtils.isMobile())
            {
                string path2 = null;
                if (dir == null)
                {
                    AndroidJavaClass jclass = new AndroidJavaClass("android.os.Environment");
                    AndroidJavaObject jobj = jclass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
                    dir = jobj.Call<string>("getAbsolutePath");
                    path2 = dir + "/wl-app";
                    if (!Directory.Exists(path2))
                    {
                        //Directory.CreateDirectory(path2);
                        new DirectoryInfo(path2).Create();
                    }
                }
                path2 = dir + "/wl-app/log.txt";

                if (!File.Exists(path2))
                {
                    File.Create(path2);
                }
                File.WriteAllText(path2, str);
            }
        }
        public static void delLog()
        {
            if(sysUtils.isMobile()){
                AndroidJavaClass jclass = new AndroidJavaClass("android.os.Environment");
                AndroidJavaObject jobj = jclass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
                dir = jobj.Call<string>("getAbsolutePath");
                string path2 = dir + "/wl-app/log.txt";
                if (File.Exists(path2))
                    File.Delete(path2);
            }
        }*/
    }
}
