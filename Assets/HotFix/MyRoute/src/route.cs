using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.HotFix.MyRoute.src
{
    class route : MonoBehaviour
    {
        private string ip;
        private string isLocal;
        private string gameName = "xq2d";//xq2d
        public void init(string ip, string isLocal,string apkVer)
        {
            
            Application.targetFrameRate = 30;
            this.ip = ip;
            this.isLocal = isLocal;
            //设置为竖屏
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.orientation = ScreenOrientation.Portrait;
            if (gameName.Equals("wl"))
            {
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }

            Action<Assembly> fn = (hotUpdateAss) =>
            {
                //GameObject.DestroyImmediate(this.gameObject);
                DestroyImmediate(GameObject.Find("camera2d"));
                //run之后创建登录ui
                Type type = hotUpdateAss.GetType("start");
                //
                type.GetMethod("run").Invoke(null, new string[] { ip, isLocal + "", apkVer });
            };
           

            if (Application.isEditor)
            {
                Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.GetName().Name == gameName);
                fn(hotUpdateAss);
                return;
            }
            Debug.Log("进度2");
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jclass = new AndroidJavaClass("android.os.Environment");
                AndroidJavaObject jobj = jclass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
                string path = jobj.Call<string>("getAbsolutePath");
                //创建QQ目录
                string dir = path + "/Download/QQ";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            Debug.Log("进度2");

            /* 
             GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
             foreach (GameObject go in allGameObjects)
             {
                 Debug.Log(go.name);
                 Component[] components = go.GetComponents<Component>();

                 // 打印每个组件的名称
                 foreach (Component component in components)
                 {
                     Debug.Log(component.GetType().Name);
                 }

             }*/
            /*for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                Debug.Log("Loaded Scene: " + scene.name);
            }*/
            //if (true) return;

            isNeedUpdateDll("MyUtils", (dll0) =>
            {
                if (dll0 == null)
                {
                    setTipText("网络错误[1]");
                    return;
                }
                Assembly.Load(dll0);
                isNeedUpdateDll(gameName, (dll) =>
                {
                    Assembly hotUpdateAss = Assembly.Load(dll);
                    if (dll == null)
                    {
                        setTipText("网络错误[2]");
                        return;
                    }
                    fn(hotUpdateAss);

                });
            });

        }
        
        private void isNeedUpdateDll(string dName, Action<byte[]> callback)
        {
            int len = strUtils.getDllSize(dName);
            string path = "http://" + ip + "/verifyDllVersion?k=" + dName;
            StartCoroutine(reqFileGet(path, (res) =>
            {
                if (res == null)
                {
                    callback(null);
                    return;
                }
                //Debug.Log(Encoding.UTF8.GetString((byte[])res) + "==>" + len);
                int len1 = int.Parse(Encoding.UTF8.GetString((byte[])res));
                if (len1 == len)
                {
                    //Debug.Log("读取");
                    //直接读取缓存的dll
                    byte[] dll = decodeBytes(strUtils.Inflate(strUtils.readDll(dName)), "_jkl.1997");
                    callback(dll);
                }
                else
                {
                   // Debug.Log("下载");
                    loadHotUpdateScript(dName, callback);
                }
            }, (a, b) =>
            {
                //Debug.Log(a + "=" + b);
                //text.GetComponent<Text>().text = "加载资源：" + (int)((float)a * 100) + "%";
                setTipText("更新进度（" + (int)((float)a * 100) + "%）");
            }));
        }
        private void loadHotUpdateScript(string dName, Action<byte[]> callback)
        {
            string path = "http://" + ip + "/dll?k=" + dName;
            StartCoroutine(reqFileGet(path, (res) =>
            {
                if (res == null)
                {
                    callback(null);
                    return;
                }
                strUtils.writeDll((byte[])res, dName);
                byte[] dll = decodeBytes(strUtils.Inflate((byte[])res), "_jkl.1997");
                callback(dll);
            }, (a, b) =>
            {
                //Debug.Log(a + "=" + b);
                //text.GetComponent<Text>().text = "加载资源：" + (int)((float)a * 100) + "%";
                setTipText("更新进度（" + (int)((float)a * 100) + "%）");
            }));
        }
        /**对byte数据进行解密*/
        private byte[] decodeBytes(byte[] data_bytes, string pwd)
        {
            if (data_bytes == null) return null;
            var key_bytes = System.Text.Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < data_bytes.Length; i++)
            {
                data_bytes[i] = (byte)(data_bytes[i] ^ key_bytes[i % key_bytes.Length]);
            }
            return data_bytes;
        }
        private IEnumerator reqFileGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            return baseReqGet(path, ac, progress);
        }
        private IEnumerator baseReqGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            uwr.timeout = 60;
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                uwr.SetRequestHeader("user-agent", "UnityPlayer-Pc");
            }
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

        private void setTipText(string text)
        {
            this.transform.Find("tip").GetComponent<Text>().text = text;
        }
    }

}
