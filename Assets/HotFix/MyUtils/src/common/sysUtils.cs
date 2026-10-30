using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.HotFix.MyUtils.src.common
{
    public class sysUtils
    {
        //是否屏幕输出日志
        public static bool isOutLogToScreen = false;
        //预制体的比例.原屏幕的ui比例1425*750
        public static float scaleK = 1;
        //项目本地缓存根节点，每个游戏项目都划分好不同的根节点(start类设置)
        public static string projRootDir;
        //图片是否需要设置为
        public static bool isPixelPic;
        public static float getScaleRate(float w, float h, int type = 0)
        {
            float k0 = 1;
            if (type == 0)//高固定
            {
                k0 = Screen.width / w;
                if (k0 * h < Screen.height)
                {
                    //当高度比例*此时的宽度时还是小于屏幕宽度，则采用屏幕宽大放大比例
                    k0 = Screen.height / h;
                }
            }
            else
            {
                k0 = Screen.height / h;
                if (k0 * w < Screen.width)
                {
                    //当高度比例*此时的宽度时还是小于屏幕宽度，则采用屏幕宽大放大比例
                    k0 = Screen.width / w;
                }
            }

            return k0;
        }
        /**根据当前平台获取资源路径
         pt= reqUrl
        key 服务器相对assccs路径
         */
        public static string getPathByPlatform(string key)
        {
            //getAB?k=effect/test.prefab&a=bundle
            string pt = "bundle";
            if (Application.platform == RuntimePlatform.Android)
            {
                //首字母大写
                pt = "android";
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                pt = "web";

            }
            string url = "http://" + strUtils.ip + "/getAB?k=" + key + "&a=" + pt + "&p=" + projRootDir;
            return url;
        }
        
        public static IEnumerator reqFileGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            yield return baseReqGet(path, ac, progress);
        }
        private static IEnumerator baseReqGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ProtocolError ||
                uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                yield break;
            }

            while (!uwr.isDone)
            {
                //注意：文件太大会分段下载，这是一段的进度及总长
                //my_Slider.value = uwr.downloadProgress;
                if (progress != null) progress(uwr.downloadProgress, uwr.downloadedBytes.ToString());
                yield return 0;
            }
            if (uwr.isDone)
            {
                if (uwr.error != null || uwr.downloadHandler.data.Length == 0)
                {
                    Debug.Log("net error =>" + uwr.error);
                    ac(null);
                }
                else
                {
                    byte[] bs = uwr.downloadHandler.data;

                    //一段加密的base64
                    //string str = uwr.downloadHandler.text;

                    //解析成正常的base64后进行返回
                    ac(bs);
                }


            }
        }
        /**获取资源目录*/
        public static string getUrl()
        {
            string path = Application.streamingAssetsPath + "/" + projRootDir;
            //string path = Application.dataPath + "/Res/apple/xqol_/resource";
            if (isMobile()) path = Application.persistentDataPath + "/" + projRootDir;
            return path;
        }
        /**判断是否为移动端*/
        public static bool isMobile()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return true;
            }
            return false;
        }

    }
}
