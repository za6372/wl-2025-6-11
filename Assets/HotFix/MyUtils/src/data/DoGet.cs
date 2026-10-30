using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.data
{

    public class DoGet : MonoBehaviour
    {
        private List<Dictionary<string, object>> reqImgs = new List<Dictionary<string, object>>();

        //private volatile int reqImgsNum = 0;
        //图集里面的小图缓存，用于重复的小图加快读取，所有申请完毕后需要移除
        //private Dictionary<string, Sprite> sheetsSprite = new Dictionary<string, Sprite>();
        public static DoGet getInstance()
        {
            DoGet doGet = GameObject.Find("main").GetComponent<DoGet>();
            return doGet;
        }


        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadAsync(sceneName));
        }
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            Debug.Log(sceneName + "=>" + lightmaps.Length);
            //SceneSetting.addLightD(sceneName);
        }
        /**移除场景*/
        public void RemoveSceneAsync(string sceneName)
        {
            StartCoroutine(RemoveAsync(sceneName));

        }
        IEnumerator RemoveAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
            yield return null;
        }

        IEnumerator LoadAsync(string sceneName)
        {
            //Debug.Log("加载" + sceneName);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // 在场景加载完成前不激活场景
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                // 可以添加一些进度条的逻辑
                //Debug.Log(sceneName+"=>"+asyncLoad.progress);
                if (asyncLoad.progress >= 0.9f)
                {
                    // 必须在=0.9时设置为true，进度会一直在0.9
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null; // 等待下一帧
            }
            //Debug.Log("完成" + sceneName);

            //(SceneManager.GetSceneByName(sceneName).
            /*LightmapData[] lightmaps = LightmapSettings.lightmaps;
            Debug.Log(sceneName + "=>" + lightmaps.Length);
            LightmapSettings.lightmaps = new LightmapData[0];
            LightmapSettings.lightmaps = lightmaps;*/
            /*foreach (LightmapData m in lightmaps)
            {
                Debug.Log(m.nam);

            }*/
            //SceneSetting.addLightD(sceneName);


            yield return null;
        }
        /**所有ab包资源加载的入口*/
        public void loadAnyPrefab(string key, Action<AssetBundle> ac)
        {
            this.GetComponent<Res3dLoadHandle>().add(key, ac);
        }
        /**获取资源主文件*/
        public void getAssetBundleManifest(string[] bundles, Action<List<string>> ac)
        {
            string ak = "AssetBundles";
            if (sysUtils.isMobile()) ak = "androidBundle";
            loadAnyModel(ak, (ab) =>
            {
                AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                //拿到所有相关依赖
                HashSet<string> abNames = new HashSet<string>();
                getMatchBundlesName(bundles, 0, manifest, abNames);
                ac(abNames.ToList());
            });
        }
        /**加载预制体（每个预制体是单独一个ab包）
         path：effect/a.prefab
         */
        public void loadMatchPrefab(string key, Action<AssetBundle> ac)
        {
            string ak = "AssetBundles";
            if (sysUtils.isMobile()) ak = "androidBundle";
            /*IEnumerable<AssetBundle> abs = AssetBundle.GetAllLoadedAssetBundles();
            foreach (AssetBundle a in abs)
            {
                Debug.Log("========" + a.name);
            }*/
            loadAnyModel(ak, (ab) =>
            {
                if (ab == null)
                {
                    ac(null);
                    return;
                }
                AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                string[] bundles = { key };
                HashSet<string> abNames = new HashSet<string>();
                getMatchBundlesName(bundles, 0, manifest, abNames);
                loadAB(abNames.ToList(), 0, () =>
                {
                    AssetBundle target = LoadResource.getFromAB(key);
                    ac(target);
                });
                //对ab资源进行引用计数
                ABRefCount.getInstance().addRef(abNames.ToList());
            });
        }
        private void getMatchBundlesName(string[] bundles, int index, AssetBundleManifest manifest, HashSet<string> abNames)
        {
            if (index >= bundles.Length)
            {
                return;
            }
            string bundleName = bundles[index];
            //Debug.Log(abNames.Count+"=>" +bundleName);
            //foundation_res/scene/xh_1_folder/xh_1_gai2

            if (!abNames.Contains(bundleName))
            {
                abNames.Add(bundleName);
                string[] dps = manifest.GetDirectDependencies(bundleName);
                getMatchBundlesName(dps, 0, manifest, abNames);

            }

            index++;
            getMatchBundlesName(bundles, index, manifest, abNames);
        }
        private void loadAB(List<string> abNames, int index, Action ac)
        {
            if (index >= abNames.Count)
            {
                ac();
                return;
            }
            string bundleName = abNames[index];
            //Debug.Log("加载" + bundleName);
            loadAnyModel(bundleName, (ab) =>
            {
                //Debug.Log("完成" + bundleName);
                index++;
                loadAB(abNames, index, ac);

            });

        }
        /**加载任意模型
        * ABName ab资源的包名（如mapprefab）
        * key ab包内的资源名（如map1）
        */
        private void loadAnyModel(string key, Action<AssetBundle> fn, Action<object, object> progress = null)
        {
            //请求前判断是否已经存在
            AssetBundle ab = null;
            if ((ab = LoadResource.getFromAB(key)) != null)
            {
                fn(ab);
                return;
            }
            //Debug.Log("内存中未加载资源：" + key);
            //网络加载建筑模型
            string url = sysUtils.getPathByPlatform(key);
            //Debug.Log(url);
            //安卓平台需要打成安卓的bundle，windows的bundle不适用
            LoadModel(url, (ab) =>
            {
                fn(ab);
            }, progress);
        }

        /**载入模型*/
        private void LoadModel(string path, Action<AssetBundle> ac, Action<object, object> progress = null)
        {
            StartCoroutine(this.LoadBundle(path, ac, progress));
        }
        /**网络加载bundle*/
        private IEnumerator LoadBundle(string url, Action<AssetBundle> ac, Action<object, object> progress = null)
        {
            //getAB?k=effect/test.prefab&a=bundle
            string name = url.Split('?')[1].Split('&')[0].Split('=')[1];
            //先从本地加载
            string path = sysUtils.getUrl();
            //Debug.Log(path + "/" + name);
            if (!File.Exists(path + "/" + name))
            {//网络加载
             //因使用了DownloadHandlerFile导致downloadProgress无效，故先请求总长度
                /*UnityWebRequest headRequest = UnityWebRequest.Head(url);
                yield return headRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(headRequest.error))
                {
                    Debug.LogError("获取下载的文件大小失败");
                    yield break;
                }
                ulong totalLength = ulong.Parse(headRequest.GetResponseHeader("content-length"));//获取文件总大小
                Debug.Log("总长度"+totalLength);
                headRequest.Dispose();*/

                //var uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
                UnityWebRequest uwr = UnityWebRequest.Get(url);
                //SaveAssetAB(downloadPath, name, uwr);
                uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ProtocolError ||
                uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    yield break;
                }

                while (!uwr.isDone)
                {
                    //loadingUI.showBar(uwr.downloadProgress);
                    //todo:改为事件推送，不直接在里面调取ui
                    //注意：文件太大会分段下载，这是一段的进度及总长
                    if (progress != null) progress(uwr.downloadProgress, uwr.downloadedBytes.ToString());
                    yield return 0;
                }
                if (uwr.isDone)
                {
                    if (uwr.error != null || uwr.downloadHandler.data.Length == 0)
                    {
                        Debug.Log("数据下载错误！" + path + "/" + name);
                        ac(null);
                        //yield return null;
                    }
                    else
                    {
                        //写入
                        writeAB(path, name, uwr.downloadHandler.data);
                        AssetBundle bundle = de0(path + "/" + name);
                        ac(bundle);
                    }
                }
            }
            else
            {//本地文件中加载
             //AssetBundle bundle = AssetBundle.LoadFromFile(path + "/" + name);
                AssetBundle bundle = de0(path + "/" + name);
                ac(bundle);
            }
        }
        private AssetBundle de0(string path)
        {
            var bs = File.ReadAllBytes(path);
            bs = strUtils.decodeBytes(bs, "nmb");
            AssetBundle ab = AssetBundle.LoadFromMemory(bs);

            return ab;

        }
        private void writeAB(string path, string name, byte[] data)
        {
            string filePath = path + "/" + name;
            string[] arr = filePath.Split("/");
            string dir = "";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                dir += arr[i] + "/";
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(path + "/" + name, data);
        }

        public void loadHotUpdateScript(string name, Action<byte[]> callback)
        {
            string path = "http://" + strUtils.ip + "/dll?k=" + name;
            StartCoroutine(reqFileGet(path, (res) =>
            {
                byte[] dll = strUtils.decodeBytes(strUtils.Inflate((byte[])res), "_jkl.1997");
                callback(dll);
            }, null));
        }
        private IEnumerator reqFileGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            return baseReqGet(path, ac, progress);
        }
        /**判断资源是否需要更新*/
        public bool isNeetUpdate(string name, long len)
        {
            string path = sysUtils.getUrl();
            if (!name.Contains("_png") && !name.Contains("_jpg") &&
                !name.Contains("_pwd") && !name.Contains("_aef") && !name.Contains("_mape") && !name.Contains("_xml"))
            {

            }
            else
            {
                //图片资源
                path = path + "/pic";
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            /*if (name.Contains("shaders"))
            {
                Debug.Log(path.Substring(25));
                Debug.Log(name);
                Debug.Log(name + "=>" + len + "=>" + new FileInfo(path + "/" + name).Length);
            }*/
            //不存在就不用更新

            if (!File.Exists(path + "/" + name))
            {
                //Debug.Log("不存在："+path + "/" + name);

                return false;
            }
            //大小不一致就要更新
            FileInfo fileInfo = new FileInfo(path + "/" + name);
            long len0 = fileInfo.Length;
            //Debug.Log(path + "/" + name + "=>" + len0 + "=" + len);
            if (len0 != len)
            {
                Debug.Log(name + "=>" + len0 + "=" + len);
                return true;
            }

            return false;
        }
        /**移除缓存中的资源*/
        public void removeResource(string name)
        {
            string path = sysUtils.getUrl();
            if (!name.Contains("_png") && !name.Contains("_jpg") &&
                !name.Contains("_pwd") && !name.Contains("_aef") && !name.Contains("_mape") && !name.Contains("_xml"))
            {

            }
            else
            {
                //图片资源
                path = path + "/pic";
            }
            if (File.Exists(path + "/" + name)) File.Delete(path + "/" + name);
        }
        /**判断资源是否存在*/
        public bool isExistResource(string name)
        {
            string path = sysUtils.getUrl();
            if (!name.Contains("_png") && !name.Contains("_jpg"))
            {

            }
            else
            {
                //图片资源
                path = path + "/pic";
            }
            if (File.Exists(path + "/" + name)) return true;
            return false;
        }

        public void sendGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            StartCoroutine(this.reqGet(path, ac, progress));
        }

        public void sendPost(string path, Dictionary<string, object> data, Action<object> ac, Action failCall = null)
        {
            this.transform.GetComponent<ReqSendHandle>().addReq(path, data,ac,failCall);
        }
        public void sendWs(string path, JObject msg)
        {
            this.transform.GetComponent<wsSendHandle>().addReq(path, msg);
        }
        /**加载本地资源之前先进行收集，将未下载的资源先下载到本地*/
        public void collectionAnyRes(Action callback, params string[] names)
        {
            loadAnyRes(names, 0, callback);
        }
        public void collectionAnyRes(Action callback, List<string> list)
        {
            loadAnyRes(list.ToArray(), 0, callback);
        }
        public void collectionAnyRes(string[] names, Action callback)
        {
            loadAnyRes(names, 0, callback);
        }
        private void loadAnyRes(string[] names, int i, Action ac)
        {
            if (i >= names.Length)
            {
                ac();
                return;
            }
            //Debug.Log("加载" + names[i]);
            loadAnyRes(names[i], (bs) =>
            {
                //Debug.Log("成功" + names[i]);
                i++;
                loadAnyRes(names, i, ac);
            });
        }
        /**加载mape、pwd、aef等资源*/
        private void loadAnyRes(string name, Action<byte[]> ac)
        {
            //从本地中读取
            byte[] bs = readAny(name);
            if (bs != null)
            {
                ac(bs);
                return;
            }
            //最后才网络请求
            loadImg(name, (b64) =>
            {
                if (b64 == null)
                {
                    Debug.Log("服务端缺少资源：" + name);
                    ac(null);
                    return;
                }
                //直接写入本地，不放入内存
                writePic(name, (byte[])b64);
                ac((byte[])b64);
            });
        }
        //大图集,512*512
        private Dictionary<string, Texture2D> spSheet = new Dictionary<string, Texture2D>();
        //图片名获取对应的图集名及截取的位置信息
        private Dictionary<string, Dictionary<string, object>> spSheetMsg = new Dictionary<string, Dictionary<string, object>>();
        /**读取图集*/
        private Sprite getPicFromSheet(string name)
        {
            if (spSheetMsg.ContainsKey(name))
            {
                //从图集中截取
                Dictionary<string, object> dic = spSheetMsg[name];
                Texture2D big = spSheet[dic["sp"].ToString()];
                Rect v4 = (Rect)dic["pos"];
                Sprite sprite = Sprite.Create(big, v4, new Vector2(0.5f, 0.5f));
                return sprite;
            }
            return null;
        }
        /**添加到图集*/
        public void addPicToSpriteSheet()
        {
            int bigNum = 0;

            int startX = 0;
            int startY = 0;
            int maxH = 0;
            string path = sysUtils.getUrl() + "/pic";
            if (!Directory.Exists(path)) return;
            //先读取本地，将所有图片加入图集
            string[] ns = Directory.GetFiles(path, "*_png");//*_png;*_pwd


            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            for (int i = 0; i < ns.Length; i++)
            {
                string url = ns[i];
                //byte[] bs = File.ReadAllBytes(url);
                string n = Path.GetFileName(url);
                byte[] bs = readFile(n);
                //Debug.Log(n);
                Texture2D t2d = LoadResource.LoadTexture(bs).texture;
                if (t2d.width > 512 || t2d.height > 512) continue;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("key", n);
                dic.Add("t2d", t2d);
                list.Add(dic);
            }
            //先将读取的图片进行大小排序，高的在后
            list = list.OrderBy(x =>
            {
                Texture2D t2d = (Texture2D)x["t2d"];
                return t2d.height;
            }).ToList();

            //大图
            Texture2D big = createNewAtlas();
            for (int i = 0; i < list.Count; i++)
            {
                Dictionary<string, object> a = list[i];
                string n = a["key"].ToString();
                Texture2D t2d = (Texture2D)a["t2d"];
                if (startX + t2d.width > big.width)
                {
                    //行满了，换行
                    startY = maxH;
                    startX = 0;
                }
                if (startY + t2d.height > big.height)
                {
                    startX = 0;
                    startY = 0;
                    maxH = 0;
                    //将上一个的图集缓存好
                    if (sysUtils.isPixelPic)
                    {
                        big.wrapMode = TextureWrapMode.Clamp;
                        big.filterMode = FilterMode.Point;
                        if (big.height % 4 != 0 || big.width % 4 != 0) big.Compress(false);
                    }
                    //else big.Compress(true);
                    big.Apply();
                    spSheet.Add("big_" + bigNum, big);
                    bigNum++;
                    //创建一个新的
                    big = createNewAtlas();
                }
                Color32[] color = t2d.GetPixels32(0);
                //横向排，不够就换行
                big.SetPixels32(startX, startY, t2d.width, t2d.height, color);

                //记录小图位置信息
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("sp", "big_" + bigNum);//大图名
                dic.Add("pos", new Rect(startX, startY, t2d.width, t2d.height));//位置、大小
                spSheetMsg.Add(n, dic);

                //间隔1px避免有竖线
                startX = startX + t2d.width + 1;
                //取最大高度
                if (maxH < (startY + t2d.height + 1)) maxH = startY + t2d.height + 1;
            }
            //缓存
            if (sysUtils.isPixelPic)
            {
                big.wrapMode = TextureWrapMode.Clamp;
                big.filterMode = FilterMode.Point;
                if (big.height % 4 != 0 || big.width % 4 != 0) big.Compress(false);
            }
            //else big.Compress(true);
            big.Apply();
            spSheet.Add("big_" + bigNum, big);

            //遍历图集并输出，查看效果
            /*foreach (string k in spSheet.Keys)
            {
                Texture2D a = spSheet[k];
                byte[] bytes = a.EncodeToPNG();
                string filePath = "C:\\Users\\13245\\Desktop\\合图\\" + k + ".png";
                File.WriteAllBytes(filePath, bytes);

            }
            Debug.Log("生成完毕");*/
        }

        private Texture2D createNewAtlas()
        {
            int chunkSize = 512;
            Texture2D atlas = new Texture2D(chunkSize, chunkSize, TextureFormat.RGBA32, false);
            Color[] clearPixels = new Color[chunkSize * chunkSize];
            for (int i = 0; i < clearPixels.Length; i++)
            {
                clearPixels[i] = Color.clear;
            }
            atlas.SetPixels(clearPixels);
            return atlas;
        }

        /**
         * 1先将申请的图片名放入集合
         * 2.剔除重复的图片
         * 3.调用请求，同时请求资源，每个资源下载完成后直接回调集合中的Action
         * 
         * 使用图集需要 sheetName,sheetKey 但是每次加载一个2000*1000的图后再截取小图性能不是很好（非常卡），所以还是不要使用图集
         * **/
        public void collectionImgMsg(ImgUI img, string path)
        {
            //收集需要的资源
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("img", img);
            msg.Add("path", path);
            reqImgs.Add(msg);

        }

        /**去重并申请资源*/
        public void startReqImg()
        {
            //生成请求任务，上一个执行完才会执行下一个
            taskManager.getInstance().putTask(() =>
            {
                doLoad();
            });
        }
        public void doLoad()
        {
            //执行前需复制一份，并清空原有的
            List<Dictionary<string, object>> copys = new List<Dictionary<string, object>>();
            for (int i = 0; i < reqImgs.Count; i++)
            {
                copys.Add(new Dictionary<string, object>(reqImgs[i]));
            }
            //清理
            reqImgs.Clear();

            int countNum = 0;
            //Debug.Log(reqImgs.Count+"=>"+reqImgsNum);
            //筛选的请求路径
            List<string> list = new List<string>();
            foreach (Dictionary<string, object> a in copys)
            {
                string path = a["path"].ToString();
                //去掉重复的路径申请
                bool b = false;
                foreach (string l in list)
                {
                    if (l.Equals(path))
                    {
                        b = true;
                        break;
                    }
                }
                if (!b)
                {
                    list.Add(path);
                }
            }
            //执行申请,不需要知道是否加载完成
            for (int i = 0; i < list.Count; i++)
            {
                string path = list[i];

                //从路径中请求得到贴图
                loadImg(path, (sp) =>
                {
                    //像素画设置，不需要时请注释掉
                    Texture2D t2d = sp.texture;
                    if (sysUtils.isPixelPic)
                    {
                        if (t2d.filterMode != FilterMode.Point)
                        {
                            t2d.wrapMode = TextureWrapMode.Clamp;
                            t2d.filterMode = FilterMode.Point;
                            t2d.Apply();
                        }

                    }

                    Rect ma = sp.rect;

                    //遍历收集的集合，发现路径相同的就执行回调
                    for (int j = 0; j < copys.Count; j++)
                    {
                        Dictionary<string, object> r = copys[j];
                        string rp = r["path"].ToString();
                        if (rp.Equals(path))
                        {
                            //r只有两个属性 path、img
                            ImgUI img = (ImgUI)r["img"];
                            Rect rc = img.rect;
                            if (img == null)
                            {
                                Debug.Log("ImgUI对象已经被销毁：" + path);
                                continue;
                            }
                            Image image = img.GetComponent<Image>();
                            if (image != null)
                            {
                                //image.type在请求前已经给过了
                                Sprite sprite = null;
                                if (image.type == Image.Type.Simple)//普通类型
                                {
                                    /*if (img.rect == default) img.rect = new Rect(ma.x, ma.y, ma.width, ma.height);
                                    sprite = Sprite.Create(t2d, img.rect, new Vector2(0.5f, 0.5f));*/
                                    if (rc == default) rc = new Rect(0, 0, ma.width, ma.height);

                                    sprite = Sprite.Create(t2d, new Rect(ma.x + rc.x, ma.y + rc.y, rc.width, rc.height), new Vector2(0.5f, 0.5f));
                                    //sprite = sp;
                                }
                                else if (image.type == Image.Type.Sliced)//切分类型（需要用rect）
                                {
                                    //Rect rect：用于精灵的纹理的矩形部分
                                    //Vector2 pivot：精灵的轴心点（相对于其图形矩形而言）
                                    //float pixelsPerUnit：对应世界空间中一个单位的精灵中的像素数
                                    //uint extrude：精灵网格应向外扩展的数量
                                    //:ml-search[SpriteMeshType] meshType：精灵的网格类型
                                    //Vector4 border：精灵的边框大小
                                    //bool generateFallbackPhysicsShape：是否生成备用物理形状
                                    //sprite = Sprite.Create(t2d, new Rect(ma.x, ma.y, ma.width, ma.height), new Vector2(0.5f, 0.5f), 100f, 1, SpriteMeshType.FullRect, img.border);

                                    //没办法，必须新建一个Texture2D，用原来的会有问题

                                    sprite = Sprite.Create(t2d, new Rect(ma.x, ma.y, ma.width, ma.height), new Vector2(0.5f, 0.5f), 100f, 1, SpriteMeshType.FullRect, img.border);
                                }
                                else if (image.type == Image.Type.Tiled)
                                {
                                    if (img.rect == default) img.rect = new Rect(ma.x, ma.y, ma.width, ma.height);
                                    sprite = Sprite.Create(t2d, img.rect, new Vector2(0.5f, 0.5f));
                                }
                                else if (image.type == Image.Type.Filled)
                                {
                                    if (img.rect == default) img.rect = new Rect(ma.x, ma.y, ma.width, ma.height);
                                    sprite = Sprite.Create(t2d, img.rect, new Vector2(0.5f, 0.5f));
                                }

                                //image.sprite = sprite;
                                //执行图片加载完成的回调
                                img.call(sprite);

                            }
                            else
                            {
                                Debug.Log("空" + rp);
                            }

                            countNum++;
                            //最后需要删除reqImgs这个缓存
                            if (countNum == copys.Count)
                            {

                                copys.Clear();
                                //reqImgsNum = 0;
                                //sheetsSprite.Clear();
                                //Debug.Log("加载完成" + copys.Count);
                            }
                        }
                    }
                });
            }
        }
        private void loadImg(string path, Action<Sprite> ac = null)
        {
            //先判断图集中是否存在
            Sprite t2d = getPicFromSheet(path);
            if (t2d != null)
            {
                ac(t2d);
                return;
            }
            //为何不直接读本地，而是读缓存？因为每次读本地都会生成一个贴图对象，而读缓存可以复用
            //先判断缓存中是否存在，不存在再发请求
            t2d = LoadResource.getFromPics(path);
            if (t2d != null)
            {
                ac(t2d);
                return;
            }
            //判断本地是否有
            t2d = readPic(path);
            if (t2d != null)
            {
                //缓存好一份
                LoadResource.putInPics(path, t2d);
                ac(t2d);
                return;
            }
            //最后才网络请求
            loadImg(path, (b64) =>
            {
                if (b64 == null) return;
                //直接写入本地，不放入内存
                writePic(path, (byte[])b64);
                Sprite t2d = LoadResource.LoadTexture((byte[])b64);
                //缓存好一份
                LoadResource.putInPics(path, t2d);
                ac(t2d);
            });
        }

        private byte[] readFile(string name)
        {
            string path = sysUtils.getUrl() + "/pic";
            if (!File.Exists(path + "/" + name))
            {
                return null;
            }
            byte[] bs = File.ReadAllBytes(path + "/" + name);
            return bs;
        }
        public byte[] readAny(string name)
        {
            byte[] bs = readFile(name);
            if (bs == null) return null;
            return LoadResource.LoadAny(bs);
        }
        public string readText(string name)
        {
            byte[] bs = readAny(name);
            if (bs == null) return null;
            return System.Text.Encoding.UTF8.GetString(bs);
        }
        public XDocument readXml(string name)
        {
            string xmlData = readText(name);
            return XDocument.Parse(xmlData);
        }
        private Sprite readPic(string name)
        {
            byte[] bs = readFile(name);
            if (bs == null) return null;
            return LoadResource.LoadTexture(bs);
        }
        private void writePic(string name, byte[] data)
        {
            string path = sysUtils.getUrl() + "/pic";
            //Debug.Log(path + "/" + name);
            if (!File.Exists(path + "/" + name))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(path + "/" + name, data);
            }
        }


        private void loadImg(string path, Action<object> ac)
        {
            StartCoroutine(this.getRes(path, ac));
        }
        /**拉取资源*/
        private IEnumerator getRes(string name, Action<object> ac)
        {
            yield return reqGet("/part1?k=" + name + "&t=1" + "&p=" + sysUtils.projRootDir, ac);
        }
        private IEnumerator reqGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            path = "http://" + strUtils.ip + path;
            return baseReqGet(path, ac, progress);
        }

        private IEnumerator baseReqGet(string path, Action<object> ac, Action<object, object> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            yield return uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                //Debug.Log("当前的下载进度为：" + uwr.downloadedBytes );
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







    }
}
