using Assets.HotFix.xq2d.src;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.MyUtils.src.shape;
using System.IO;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.childPage.login;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class LoginPage : PageUI
    {


        public override void init()
        {

        }

        public void drawUI()
        {
            GameObject bg = readStartAm.read((bg) =>
             {
                 GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
                 items.transform.SetParent(this.transform, false);
                 items.GetComponent<SimpleUI>().setSizePos(new Vector2(ScreenUtils.width - 240, 300),
                     new Vector2(120, -(ScreenUtils.height / 2 + 200)));

                 for (int i = 0; i < 7; i++)
                 {
                     addItem(i, items.transform);
                 }


                 /*GameObject text = gameObjPool.getInstance().get("text", typeof(TextRichUI));
                 text.transform.SetParent(this.transform, false);
                 text.GetComponent<TextRichUI>().setFacePy(new Vector2(0, 0))
                     .setSizePos(new Vector2(300, 150), Vector2.zero);
                 text.GetComponent<TextRichUI>().setTextValue("测试#01好家伙#02蛛#03#04#05#06jjjj#07竞技场#08");*/

                 /*List<Pet> list= face.petInterface.getAllPetList();
                 string str = "";
                 for(int i = 0; i < list.Count; i++)
                 {
                     str += "\"" + list[i].name + "\",";
                 }
                 Debug.Log(str);*/
                 //293*672
                 /*float w = ScreenUtils.width, h = ScreenUtils.width / 293f * 672;
                 if (h > ScreenUtils.height)
                 {
                     h = ScreenUtils.height;
                     w = h / 672f * 293;
                 }
                 GameObject test = gameObjPool.getInstance().get("test", typeof(ImgUI));
                 test.transform.SetParent(this.transform, false);
                 test.GetComponent<ImgUI>().loadRes("phone_png")
                 .setSizePos(new Vector2(w, h), new Vector2((ScreenUtils.width - w) / 2f, -(ScreenUtils.height - h) / 2f));*/

                 DoGet.getInstance().startReqImg();
                 handle(2);


             });
            bg.transform.SetParent(this.transform, false);
            RectTransform rt = bg.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;
            SetGameObj.setLeftPos(new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f), bg);
            //320*480
            float h = ScreenUtils.width * 480 / 320f;
            RectMask2D r2d = bg.AddComponent<RectMask2D>();
            r2d.padding = new Vector4(0, (ScreenUtils.height - h) / 2f, 0, (ScreenUtils.height - h) / 2f);
            r2d.softness = new Vector2Int(100, 230);

            //增加一层液晶滤镜
            addTejingEffect();

            string str = "";
            /*for (int i = 10100000; i < 10100078; i++)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(i + "");
                if (gd == null || !gd.name.Contains("卓越")) continue;
                Debug.Log(gd.name);
                JObject a = new JObject();
                a.Add("key", gd.key);
                a.Add("num", 999999);
                a.Add("isBind", 0);
                a.Add("pos", 0);
                a.Add("isBad", 0);
                a.Add("Id", strUtils.getId());
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.None, // 关键：禁用格式化
                    NullValueHandling = NullValueHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(a, settings);
                str += json + ",";

            }*/
            /*for (int i = 10110000; i < 10110030; i++)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(i + "");
                if (gd == null) continue;
                Debug.Log(gd.name);
                JObject a = new JObject();
                a.Add("key", gd.key);
                a.Add("num", 999999);
                a.Add("isBind", 0);
                a.Add("pos", 0);
                a.Add("isBad", 0);
                a.Add("Id", strUtils.getId());
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.None, // 关键：禁用格式化
                    NullValueHandling = NullValueHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(a, settings);
                str += json + ",";

            }*/
            Debug.Log(str);
            /*string str = "";
            for (int i = 10030027; i < 10030070; i++)
            {
                Skill gd = (Skill)face.goodsInterface.getGoodsMsgByKey("1002" + i);
                if (gd == null) continue;
                str += gd.name + ":" + gd.getSkillDes(1) + "\n";
            }
            File.WriteAllText("C:\\Users\\13245\\Desktop\\aaa.txt", str);*/

            /*GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allGameObjects)
            {
                Debug.Log(go.name);
                if (go.name.Equals("main"))
                {
                    Component[] components = go.GetComponents<Component>();

                    // 打印每个组件的名称
                    foreach (Component component in components)
                    {
                        Debug.Log(component.GetType().Name);
                    }
                }
               
            }*/
            /* for (int i = 0; i < SceneManager.sceneCount; i++)
             {
                 Scene scene = SceneManager.GetSceneAt(i);
                 Debug.Log("Loaded Scene: " + scene.name);
             }*/
            /*string[] arr= Directory.GetDirectories("E:\\xq-new\\server\\c_xq_server\\c_wl_server\\src\\main\\resources\\assets\\xq2d\\pic\\npcres");
            string str = "";
            for(int i = 0; i < arr.Length; i++)
            {
                str += "\""+ new DirectoryInfo(arr[i]).Name+"\""  + ",";
            }
            Debug.Log(str);*/

            /*List<string> all = new List<string>() { "task_talk_xml" };
            DoGet.getInstance().collectionAnyRes(() =>
            {
                XDocument doc = DoGet.getInstance().readXml("task_talk_xml");
                //var element = doc.Element("config").Element("data");
                var ds= doc.Descendants("data").Where(b => b.Attribute("id").Value == "1000");
                var list = ds.ToList()[0].Elements();
                JObject a = new JObject();
                foreach (var p1 in list)//get\p0\p1\over
                {
                    var list2 = p1.Elements();
                    JArray arr = new JArray();
                    foreach(var p2 in list2)//it->{type,talk}
                    {
                        JObject it = new JObject();
                        it["type"] = p2.Element("type").Value;
                        it["talk"] = p2.Element("talk").Value;
                        //Debug.Log(it);
                        arr.Add(it);
                    }
                    a[p1.Name.ToString()] = arr;
                }
                Debug.Log(a);

            }, all);*/

            /*XDocument doc = new XDocument();
            // 获取根节点（如果不存在，则添加）
            XElement root = doc.Element("config") ?? new XElement("config");
            doc.Add(root); // 如果之前没有添加根节点，现在添加它

            for (int i = 2163; i < 3276; i++)
            {
                JObject a = face.taskDesInterface.getTaskDes0(i.ToString());
                if (a == null) continue;
                // 添加子节点和内容
                List<XElement> data = new List<XElement>();
                putDataToDoc(a, "get", data);
                for (int p = 0; p < 10; p++)
                {
                    if (!a.ContainsKey("p" + p)) break;
                    putDataToDoc(a, "p" + p, data);
                }
                putDataToDoc(a, "over", data);

                root.Add(new XElement("data", new XAttribute("id", i.ToString()), data.ToArray()));

            }
            //Debug.Log(doc.ToString());
            string path = sysUtils.getUrl();
            File.WriteAllText(path + "/test.xml", doc.ToString());*/
        }
        private void putDataToDoc(JObject a, string key, List<XElement> data)
        {
            if (a.ContainsKey(key))
            {
                JArray list = (JArray)a[key];
                List<XElement> gets = new List<XElement>();
                for (int p = 0; p < list.Count; p++)
                {
                    JObject n = (JObject)list[p];
                    string type = n["type"].ToString();
                    string talk = n["talk"].ToString();
                    List<XElement> ls = new List<XElement>();
                    XElement t0 = new XElement("type", type);
                    XElement t1 = new XElement("talk", talk);
                    ls.Add(t0);
                    ls.Add(t1);
                    XElement it = new XElement("it", ls.ToArray());
                    gets.Add(it);
                }
                XElement get = new XElement(key, gets.ToArray());
                data.Add(get);
            }
        }
        public void addTejingEffect()
        {
            Transform ts = PointGet.getC2dFilterCanvas();
            GameObject bg = gameObjPool.getInstance().get("yejing", typeof(ImgUI));
            bg.transform.SetParent(ts.transform, false);
            bg.GetComponent<ImgUI>().setNoClk()
            .setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).setLayerCenter();
            //加载液晶材质
            /*DoGet.getInstance().loadAnyPrefab("shaders", (ab) =>
            {
                
                Material mat = new Material(ab.LoadAsset<Shader>("GridShader.shader"));
                mat.SetFloat("Grid Size", 100f);
                mat.SetColor("Color 1", new Color(0, 0, 0, 0.11f));
                mat.SetColor("Color 2", new Color(1, 1, 1, 0));
                bg.GetComponent<Image>().material = mat;
            });*/
            bg.SetActive(false);
        }
        private void addItem(int i, Transform canvas)
        {
            float w = numberUtils.getAvWidth(60, 7, 120);
            float x = i * (60 + w);
            float y = 0;

            GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
            item.transform.SetParent(canvas, false);
            item.GetComponent<SimpleUI>().addClk(() =>
            {
                handle(i);
            }).setSizePos(new Vector2(60, 300), new Vector2(x, y));

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(item.transform, false);
            bg.GetComponent<ImgUI>().setColor("#502C2C")//.loadRes("t2_png", new Rect(2, 2, 9, 8))
            .setSizePos(new Vector2(60, 260), new Vector2(0, -20));
            bg.AddComponent<UIOutline>().setSome(new Color32(255, 200, 0, 255), new Vector2(1, -1));

            GameObject top = gameObjPool.getInstance().get("top", typeof(ImgUI));
            top.transform.SetParent(item.transform, false);
            top.GetComponent<ImgUI>().loadRes("t1_png")
            .setSizePos(new Vector2(60, 31.2f), new Vector2(0, 0));
            GameObject bottom = gameObjPool.getInstance().get("bottom", typeof(ImgUI));
            bottom.transform.SetParent(item.transform, false);
            bottom.GetComponent<ImgUI>().loadRes("b1_png")
            .setSizePos(new Vector2(60, 31.2f), new Vector2(0, -278.8f));
            int dx = 0;
            if (i == 0) dx = 2;
            else if (i == 1) dx = 1;
            else if (i == 2) dx = 4;
            else if (i == 3) dx = 3;
            else if (i == 4) dx = 0;
            else if (i == 5) dx = 5;
            else if (i == 6) dx = 6;


            GameObject text = gameObjPool.getInstance().get("text", typeof(ImgUI));
            text.transform.SetParent(item.transform, false);
            text.GetComponent<ImgUI>().loadRes("z_0_png", new Rect(13 * dx, 0, 13, 54))
            .setSizePos(new Vector2(40, 166), new Vector2(10, -(300 - 166) / 2));

        }
        private void handle(int index)
        {
            if (index == 0)//快速登录
            {
                //无需选择角色直接进入游戏
            }
            else if (index == 1)//登录游戏
            {
                //弹出输入账号密码
                LoginUI.create(0, this.transform);
            }
            else if (index == 2)//更新帮助
            {
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getTipCanvas());
                dialog.renderText(versionUtils.getUpdateContent());
                DoGet.getInstance().startReqImg();
            }
            else if (index == 3)//注册账号
            {
                LoginUI.create(1, this.transform);
            }
            else if (index == 4)//进入论坛
            {
                Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getTipCanvas());
                dialog.renderText("论个毛，哪有坛？", "center");
                DoGet.getInstance().startReqImg();
            }
            else if (index == 5)//关于我们
            {
                Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getTipCanvas());
                dialog.renderText("全世界几百亿人的团队！", "center");
                DoGet.getInstance().startReqImg();
            }
            else if (index == 6)//退出游戏
            {
                Application.Quit(0);
            }
        }



    }
}
