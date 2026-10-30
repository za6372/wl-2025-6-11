using Assets.Res.script;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadDll : MonoBehaviour
{

    private string ipMiYao = 
        //"RUVDNjE0QzVGNTMxREMyNUIyNDMyMEJERjlFQTlGODFFMjhENzU2QUMwMEFBQTkyQkI0MzBCQUU4MEQwRUQ0NjQ2NjEzNzgyMEYwQzM4ODQ1MEFFMzU2RjAxQTc4MzVDNDY0RUEzNDZFNTREQzExQQ";
        //"Mjk1QTI1RjQxMDlCOEMyRjY0QTMyMzI4RUZFMTAxMkI5NUREQjAyNDIxQ0YyOTA3NjEwNTY5MzExQzQ3OUVDMUUzOTY3MDhDNjg3N0FFODQzRjk3QTQ2QTg2Q0E3QUM0RTEwNzI4N0MxQkRDMUE5NQ";
        //"Mjk1QTI1RjQxMDlCOEMyRjJBQzQ1NjUwNUQwNjNGMkZFRjQwRkY2QzUxOTREQ0QzQkMxNjYzRThDNzJBODREM0UwNERDQkJFMkQ3NTI2NUM2OTc0OUMxQkMyMkFEREI0";  // old (LAN) 192.168.1.2:9668
        "Mjk1QTI1RjQxMDlCOEMyRjAyQ0Y2Q0RFMEQ0RjY5QzIxODQ2OEU1RDdCODRCN0I0Njc4NDVFREE3NTIyMDMzNzJBRDkwOEMwODlBQjFFQ0JBOTk1QzMwNTZEQzI4QUFF";  // local 127.0.0.1:9668
    private string defaultMiYao;
    private string ip;
   
    private string apkVer = "ver=2026.1.21";
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //控制屏幕亮度
        //Screen.brightness = 0.5f;
        //调整屏幕方向
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        //获取设备方向
        //DeviceOrientation deviceOrientation = Input.deviceOrientation;
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        //复制一份作为默认的密钥
        defaultMiYao = ipMiYao + "";
        if (PlayerPrefs.GetString("ServerIp") != null && !PlayerPrefs.GetString("ServerIp").Trim().Equals(""))
        {
            ipMiYao = PlayerPrefs.GetString("ServerIp");
        }
        drawTip();
        try
        {
            var dummyData = ipMiYao.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");//.Replace("_", "/").Replace("-", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            string des = strUtils.DecodeB64(dummyData.Trim('\0'));
            string DESKEY_SIMPLE = "ca#!pq1*";
            string json = strUtils.DecodeDES(des, DESKEY_SIMPLE);
            JObject j = JsonConvert.DeserializeObject<JObject>(json);
            //Debug.Log(j);
            ip = j["ip"].ToString();
        }
        catch (Exception e)
        {
            //Debug.Log(e.StackTrace);
            //解析失败时重新弹出输入框
            drawInputIp();
            return;
        }

        //Debug.Log(ip);

        //this.gameObject.AddComponent<ScreenConsole>();
        //先验证是否联网,注意wifi下会是局域网
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            setTipText("未联网");
            return;
        }
        else
        {
            setTipText("正在检查更新");

            //移动端加载服务器的dll
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                 Application.platform == RuntimePlatform.WindowsPlayer)
            {
                isNeedUpdateDll("MyRoute", (dll) =>
                {
                    if (dll == null)
                    {
                        Debug.Log("网络错误");
                        //创建输入框
                        drawInputIp();

                        return;
                    }
                    int isLocal = 0;
                    if (ip.Contains("192.168.1.") && Application.platform == RuntimePlatform.Android) isLocal = 1;

                    Assembly hotUpdateAss = Assembly.Load(dll);
                    //GameObject.Find("camera2d").transform.Find("canvas").GetComponent<LoadDll>();
                    DestroyImmediate(this);
                    //DestroyImmediate(GameObject.Find("camera2d"));
                    Type type = hotUpdateAss.GetType("start");
                    type.GetMethod("run").Invoke(null, new string[] { ip, isLocal + "", apkVer });
                });
                /*loadHotUpdateScript("MyUtils", (dll0) =>
                {
                    if (dll0 == null)
                    {
                        Debug.Log("网络错误");
                        //创建输入框
                        drawInputIp();

                        return;
                    }
                    Assembly.Load(dll0);

                    loadHotUpdateScript("GameCenter", (dll) =>
                    {
                        Assembly hotUpdateAss = Assembly.Load(dll);
                        if (dll == null)
                        {
                            Debug.Log("网络错误");
                            return;
                        }
                        DestroyImmediate(GameObject.Find("camera2d"));
                        int isLocal = 1;
                        if (!ip.Contains("192.168.1.")) isLocal = 0;
                        //run之后创建登录ui
                        Type type = hotUpdateAss.GetType("start");
                        type.GetMethod("run").Invoke(null, new string[] { ip, isLocal + "" });

                    }, null);

                }, null);*/

            }
            else if (Application.isEditor)
            {
                DestroyImmediate(this);
                //销毁加载程序过程的提示层
                //DestroyImmediate(GameObject.Find("camera2d"));
                // Editor下无需加载，直接查找获得HotUpdate程序集
                /*Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetName().Name == "GameCenter");*/
                Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetName().Name == "MyRoute");
                //除魔纪
                //Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "cmj");

                //run之后创建登录ui
                Type type = hotUpdateAss.GetType("start");
                type.GetMethod("run").Invoke(null, new string[] { ip, "0", apkVer });
            }
        }
    }
    private void setTipText(string text)
    {
        this.transform.Find("tip").GetComponent<Text>().text = text;
    }
    private void drawTip()
    {
        GameObject bg = new GameObject("bg");
        bg.transform.SetParent(this.transform, false);
        Image img = bg.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 1);
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        GameObject tip = new GameObject("tip");
        tip.transform.SetParent(this.transform, false);
        Text pt = tip.AddComponent<Text>();
        pt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        pt.supportRichText = false;
        pt.alignment = TextAnchor.MiddleLeft;
        pt.fontSize = 40;
        pt.color = new Color(0.5924529f, 0.5924529f, 0.5924529f, 1f);
        pt.horizontalOverflow = HorizontalWrapMode.Overflow;
        pt.text = "";
        tip.transform.localPosition = new Vector2(-(Screen.width - 300) / 2f + 50, (Screen.height - 60) / 2f - 50);
        tip.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);
    }
    private void drawInputIp()
    {
        setTipText("服务器维护中或者密钥已过期");

        GameObject input = new GameObject("input");
        input.AddComponent<Image>();
        InputField inf = input.AddComponent<InputField>();
        input.transform.SetParent(this.transform, false);
        input.transform.localPosition = Vector3.zero;
        input.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);

        GameObject placeholder = new GameObject("placeholder");
        placeholder.transform.SetParent(input.transform, false);
        Text pt = placeholder.AddComponent<Text>();
        pt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        pt.supportRichText = false;
        pt.alignment = TextAnchor.MiddleLeft;
        pt.fontSize = 35;
        pt.color = new Color(0.5924529f, 0.5924529f, 0.5924529f, 1f);
        pt.text = "输入新密钥";
        placeholder.transform.localPosition = Vector3.zero;
        placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);

        GameObject textComponent = new GameObject("textComponent");
        textComponent.transform.SetParent(input.transform, false);
        Text tp = textComponent.AddComponent<Text>();
        tp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        tp.supportRichText = false;
        tp.alignment = TextAnchor.MiddleLeft;
        tp.fontSize = 35;
        tp.color = new Color(0, 0, 0, 1f);
        textComponent.transform.localPosition = Vector3.zero;
        textComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);

        inf.placeholder = pt;
        inf.textComponent = tp;

        inf.text = defaultMiYao;

        GameObject btn = new GameObject("btn");
        btn.transform.SetParent(this.transform, false);
        btn.AddComponent<Button>().onClick.AddListener(delegate ()
        {
            ipMiYao = inf.text;
            PlayerPrefs.SetString("ServerIp", ipMiYao);
            Application.Quit(0);
        });
        btn.transform.localPosition = new Vector3(250, 0, 0);
        btn.AddComponent<RectTransform>().sizeDelta = new Vector2(120, 60);
        btn.AddComponent<Image>().color = new Color(0.4377358f, 0.4377358f, 0.4377358f, 1f);
        GameObject btnText = new GameObject("btnText");
        btnText.transform.SetParent(btn.transform, false);
        Text bn = btnText.AddComponent<Text>();
        bn.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        bn.supportRichText = false;
        bn.alignment = TextAnchor.MiddleCenter;
        bn.fontSize = 35;
        bn.text = "确定";
        btnText.transform.localPosition = Vector3.zero;
        btnText.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 60);
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
            int len1 = int.Parse(Encoding.UTF8.GetString((byte[])res));
            if (len1 == len)
            {
                //直接读取缓存的dll
                byte[] dll = decodeBytes(strUtils.Inflate(strUtils.readDll(dName)), "_jkl.1997");
                callback(dll);
            }
            else
            {
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
        }));
    }
    private IEnumerator reqFileGet(string path, Action<object> ac, Action<object, object> progress = null)
    {
        return baseReqGet(path, ac, progress);
    }
    private IEnumerator baseReqGet(string path, Action<object> ac, Action<object, object> progress = null)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(path);
        uwr.timeout = 10;
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            uwr.SetRequestHeader("user-agent", "UnityPlayer-Pc");//决定以什么设备的方式拉取dll
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

    /**
           AfterSceneLoad	在场景加载后。
           BeforeSceneLoad	在场景加载前。
           AfterAssembliesLoaded	加载完所有程序集并初始化预加载资源时的回调。
           BeforeSplashScreen	在显示启动画面之前。
           SubsystemRegistration	用于子系统注册的回调
        */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void init()
    {
        //必须登录场景才允许创建这些，不是login场景的初始化在热更新中
        if (SceneManager.GetActiveScene().name != "login") return;
        Application.targetFrameRate = 30;
        //正交相机
        GameObject camera2d = new GameObject("camera2d");
        Camera cam = camera2d.AddComponent<Camera>();
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localScale = Vector3.one;
        cam.cullingMask = 5;
        cam.gameObject.layer = 0;
        cam.clearFlags = CameraClearFlags.Skybox;
        cam.orthographic = true; //投射方式：orthographic正交//
        cam.orthographicSize = 5; //投射区域大小//
        cam.nearClipPlane = 0.3f; //前距离//
        cam.farClipPlane = 1000f; //后距离//
        cam.rect = new Rect(0, 0, 1f, 1f);
        cam.depth = 2;//深度
        cam.backgroundColor = Color.black;
        //画布
        GameObject canvas = new GameObject("canvas");
        canvas.transform.parent = camera2d.transform;
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        Canvas cs = canvas.GetComponent<Canvas>();
        cs.renderMode = RenderMode.ScreenSpaceOverlay;
        cs.sortingOrder = 0;
        canvas.AddComponent<LoadDll>();
        //事件系统
        GameObject ev = new GameObject("EventSystem");
        ev.AddComponent<EventSystem>();
        ev.AddComponent<StandaloneInputModule>();

    }
    void Awake()
    {
        Application.runInBackground = true;
    }
}
