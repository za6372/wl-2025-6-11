using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class TaskPage : PageUI
    {
        //当前选中的任务项
        private string taskItemKey;
        public TaskPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("任务列表");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            drawK2(content);
            DoGet.getInstance().startReqImg();
            return this;
        }

        private void drawK2(Transform content)
        {
            //右侧
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 340 - 60, size.y - 60),
                new Vector2(340 + 30, -30), content);

            GameObject items = gameObjPool.getInstance().get("right", typeof(SimpleUI));
            items.transform.SetParent(k1.getContent().transform, false);
            items.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x - 340 - 60, size.y - 60), new Vector2(0, 0));

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(items.transform, false);
            text.GetComponent<TextUI>().setColor("#000000").setFontSize().setAlign("leftTop")
                .setFontStyle(FontStyle.Bold).setText("").setIsRichText().setLineSpacing(1.3f)
                .setSizePos(new Vector2(size.x - 340 - 80, size.y - 160), new Vector2(10, -50));
        }

        private void drawK1(Transform content)
        {
            //左侧
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject left = gameObjPool.getInstance().get("left", typeof(ImgUI));
            left.transform.SetParent(content.transform, false);
            left.GetComponent<ImgUI>().setColor("#005052")
                .setSizePos(new Vector2(340, size.y), new Vector2(0, 0));

            GameObject scroll = gameObjPool.getInstance().get("left-content", typeof(ScrollUI));
            scroll.transform.SetParent(content.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(340, size.y), new Vector2(0, 0));
            scroll.GetComponent<ScrollUI>().initSetting();
            GameObject panel = gameObjPool.getInstance().get("panel", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(panel);

            size = left.GetComponent<RectTransform>().sizeDelta;

            List<object> list = getTask();
            if (list.Count == 0)
            {
                getTaskDes().GetComponent<TextUI>().setText("您当前没有已接任务，请查看【备忘】接取适合的任务！");
            }

            

            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                Dictionary<string, object> tk = (Dictionary<string, object>)list[i];
                GameObject item = gameObjPool.getInstance().get("item" + i, typeof(ImgUI));
                item.transform.SetParent(panel.transform, false);
                item.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
                    .setSizePos(new Vector2(size.x, 100), new Vector2(0, -110 * i - 30))
                    .addClk(() =>
                    {
                        createChildren(index, list, panel);
                    }, false);
                GradientDefinded gd = item.AddComponent<GradientDefinded>();
                gd.color1 = new Color32(24, 115, 117, 255);
                gd.color2 = new Color32(0, 52, 53, 255);

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(item.transform, false);
                text.GetComponent<TextUI>().setText(tk["name"].ToString()).setFontSize().setAlign("left").setNoClk()
                    .setSizePos(new Vector2(size.x - 70, 100), new Vector2(70, 0));
                //+-图标
                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(item.transform, false);
                icon.GetComponent<ImgUI>().loadRes("renwu_jiahao_png")
                    .setSizePos(new Vector2(50, 50), new Vector2(10, -25));
            }


        }
        private Transform getTaskDes()
        {
            Transform des = getStandardPageContent().Find("bgStyle1").GetComponent<bgStyle1>()
                    .getContent().transform.Find("right/text");
            return des;
        }
        public void clkDefault()
        {
            Transform left = getStandardPageContent().Find("left");
            if (left.childCount > 0)
            {
                left.GetChild(0).GetComponent<Button>().onClick.Invoke();
            }
        }
        /**展开子菜单*/
        private void createChildren(int index, List<object> list, GameObject left)
        {
            //先移除所有展开项
            for (int i = 0; i < left.transform.childCount; i++)
            {
                if (left.transform.GetChild(i).name.Contains("child"))
                {
                    gameObjPool.getInstance().free(left.transform.GetChild(i).gameObject);
                    i--;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                Transform ts = left.transform.Find("item" + i);
                if (ts == null) continue;

                ts.GetComponent<ImgUI>().setLeftPos(new Vector2(0, -30 - 110 * i));
                string pic = "renwu_jiahao_png";
                if (index == i) pic = "renwu_jianhao_png";
                ts.Find("icon").GetComponent<ImgUI>().loadRes(pic);
            }

            Vector2 size = left.GetComponent<RectTransform>().sizeDelta;
            float startY = -110 * index - 30 - 100 - 10;
            Dictionary<string, object> one = (Dictionary<string, object>)list[index];
            JArray arr = (JArray)one["list"];
            for (int i = 0; i < arr.Count; i++)
            {
                string key = "child-" + index + "-" + i;
                JObject tk = (JObject)arr[i];
                GameObject item = gameObjPool.getInstance().get(key, typeof(ImgUI));
                item.transform.SetParent(left.transform, false);
                item.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
                    .setSizePos(new Vector2(size.x, 100), new Vector2(0, -110 * i + startY))
                    .addClk(() =>
                    {
                        if (this.taskItemKey == key)
                        {
                            //双击弹出菜单 自动寻路、瞬间传送、放弃任务（弃用）
                            List<string> ml = new List<string>();
                            ml.Add("瞬间传送");
                            Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                            menu.addCallback((mIndex) =>
                            {
                                JObject ts = face.taskInterface.getTaskFromCache(tk["key"].ToString());
                                mapManager.getInstance().findRoad(tk["key"].ToString(), (int)ts["progressIndex"]);
                                this.freeThisPage();
                            });
                            return;
                        }
                        //选中颜色
                        for (int i = 0; i < left.transform.childCount; i++)
                        {
                            if (left.transform.GetChild(i).name == (key))
                            {
                                left.transform.GetChild(i).GetComponent<ImgUI>().setColor("#076e70");
                            }
                            else
                            {
                                left.transform.GetChild(i).GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor);
                            }
                        }

                        this.taskItemKey = key;

                        string tsKey = tk["key"].ToString();
                        task ts = face.taskInterface.getTask(tsKey);
                        JObject asd = face.taskInterface.getTaskFromCache(tsKey);
                        ts.setProgressIndex(asd);
                        int status = (int)asd["status"];
                        string str = status == 1 ? "可接取" : (status == 2 ? "未完成" : "已完成");
                        string des = "<size=35><color=#000000>" +
                            "任务目的：\n<size=35><color=#77c773>" + ts.getProgressDes() + "</color></size>" +
                            "<size=35><color=#ff0000>（" + str + "）</color></size>\n" +
                            "任务描述：\n<size=35><color=#77c773>" + ts.getTargetDes() + "</color></size>\n" +
                            "任务奖励：\n经验+630\n" +
                            "Lv8 [绑定][无]简易挂坠 x1\n" +
                            "\n</color></size>";
                        //对于一些特殊任务则需要增加描述（如锄奸卫道）
                        if (tsKey.Equals("3272"))
                        {
                            face.activityInterface.viewCjwdTask((res) =>
                            {
                                JArray ls = (JArray)res["list"];
                                des += "<size=35><color=#000000>" +
                                "剩余查明次数：" + res["times"] + "\n" +
                                "墨家门主：" + ls[0] + "\n" +
                                "道家门主：" + ls[1] + "\n" +
                                "阴阳家门主：" + ls[2] + "\n" +
                                 "\n</color></size>";
                                getTaskDes().GetComponent<TextUI>().setText(des);
                            });
                        }
                        else if (tsKey.Equals("3273"))//仗剑除魔
                        {
                            face.activityInterface.viewZjcmMapKey((res) =>
                            {
                                MapData d= face.mapInterface.getMapByKey(res);
                                des += "<size=35><color=#000000>" +
                                "目标所在地图：" + d.name + "\n" +
                                 "\n</color></size>";
                                getTaskDes().GetComponent<TextUI>().setText(des);
                            });
                        }
                        else
                        {
                            getTaskDes().GetComponent<TextUI>().setText(des);
                        }

                    });
                string name;
                if (tk["type"].ToString() == "0") name = "（主）" + tk["name"];
                else if (tk["type"].ToString() == "1") name = "（副）" + tk["name"];
                else if (tk["type"].ToString() == "2") name = "（活）" + tk["name"];
                else name = "（支）" + tk["name"];
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(item.transform, false);
                text.GetComponent<TextUI>().setText(name).setFontSize().setAlign("left").setNoClk()
                    .setSizePos(new Vector2(size.x - 20, 100), new Vector2(10, 0));

            }
            //index之后的item位置向后挪
            startY = -110 * arr.Count + startY;
            for (int i = index + 1; i < list.Count; i++)
            {
                Transform ts = left.transform.Find("item" + i);
                if (ts == null) continue;
                ts.GetComponent<ImgUI>().setLeftPos(new Vector2(0, startY - 110 * (i - index - 1)));
                ts.Find("icon").GetComponent<ImgUI>().loadRes("renwu_jiahao_png");
            }
            DoGet.getInstance().startReqImg();

            //设置scroll大小
            Transform content = this.getStandardPageContent();
            Vector2 size2 = content.GetComponent<RectTransform>().sizeDelta;
            float h = -startY + 110 * (list.Count - index - 1);
            if (h < size2.y) h = size2.y;
           content.Find("left-content").GetComponent<ScrollUI>().updateHeight(h);
        }
        private List<object> getTask()
        {
            List<object> tasks = new List<object>();
            JArray list1 = face.taskInterface.getStartTaskList(0);
            Dictionary<string, object> a1 = new Dictionary<string, object>();
            a1.Add("name", "主线");
            a1.Add("list", list1);
            tasks.Add(a1);

            list1 = face.taskInterface.getStartTaskList(3);
            a1 = new Dictionary<string, object>();
            a1.Add("name", "支线");
            a1.Add("list", list1);
            tasks.Add(a1);

            list1 = face.taskInterface.getStartTaskList(1);
            a1 = new Dictionary<string, object>();
            a1.Add("name", "副本");
            a1.Add("list", list1);
            tasks.Add(a1);

            list1 = face.taskInterface.getStartTaskList(2);
            a1 = new Dictionary<string, object>();
            a1.Add("name", "活动");
            a1.Add("list", list1);
            tasks.Add(a1);

            return tasks;
        }

        public override void init()
        {

        }
    }
}
