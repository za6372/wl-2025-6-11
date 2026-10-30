using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class TaskGetUI : PartUI
    {
        public static TaskGetUI create()
        {
            Vector2 size = new Vector2(800, 800);
            Transform parent = PointGet.getIndexPage();
            GameObject one = gameObjPool.getInstance().get("TaskGetUI", typeof(TaskGetUI));
            one.GetComponent<TaskGetUI>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<TaskGetUI>(parent).setScreenCenter();
            one.GetComponent<TaskGetUI>().draw(size);
            return one.GetComponent<TaskGetUI>();
        }
        private TaskGetUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setColor(0, 0, 0, 0f)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0));

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2, -(ScreenUtils.height - size.y) / 2));

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(kuang.transform, false);
            bg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(20, -20));
            //左顶
            GameObject bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, 0));
            //顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(9, 1, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, 0));
            //左
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(0, -20));
            //右顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, 0));
            //右
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 0, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(size.x - 20, -20));
            //左下
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(0, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, -(size.y - 20)));
            //底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(9, 0, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, -(size.y - 20)));
            //右底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l3_png", new Rect(1, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, -(size.y - 20)));

            //内容区
            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(bg.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));

            GameObject testText = gameObjPool.getInstance().get("title", typeof(TextUI));
            testText.transform.SetParent(content.transform, false);
            testText.GetComponent<TextUI>().setColor().setAlign().setFontSize(30)
                .setText("").setIsRichText()
                .setSizePos(new Vector2(size.x - 40, 50), new Vector2(0, 0));

            testText = gameObjPool.getInstance().get("des", typeof(TextUI));
            testText.transform.SetParent(content.transform, false);
            testText.GetComponent<TextUI>().setColor().setAlign("leftTop").setFontSize(30)
                .setText("").setIsRichText()
                .setSizePos(new Vector2(size.x - 40, 200), new Vector2(0, -50));




            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(bg.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(0, -700));
            btn.GetComponent<BtnUI>().addText("取消", 30).setTextColor().loadRes("gy_03_png").addClk(() =>
            {
                this.free();
            });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bg.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(640, -700));
            btn1.GetComponent<BtnUI>().addText("确定", 30).setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                callback();
                this.free();
            });

            return this;
        }
        public void setRewards(JArray list)
        {
            Transform ts = this.transform.Find("bg/jllan");
            for (int i = 0; i < list.Count; i++)
            {
                JObject reward = (JObject)list[i];


            }

        }

        public void setTitle(string title)
        {
            this.transform.Find("kuang/bg/content/title").GetComponent<TextUI>().setText(title);
        }
        public void appendContent(string content)
        {
            this.transform.Find("kuang/bg/content/des").GetComponent<TextUI>().setText(content);
        }
        public void setTarget(string target)
        {
            //this.transform.Find("kuang/bg/content/target").GetComponent<TextUI>().setText(target);
        }
        private Action callback;
        public void showGainBtn(Action ac)
        {
            this.callback = ac;
        }
    }
}
