using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class DayAnswerPage : PageUI
    {

        public static DayAnswerPage create(Transform parent)
        {
            Vector2 size = new Vector2(800, 600);
            GameObject one = gameObjPool.getInstance().get("DayAnswerPage", typeof(DayAnswerPage));
            one.GetComponent<DayAnswerPage>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<DayAnswerPage>(parent).setScreenCenter();
            one.GetComponent<DayAnswerPage>().draw(size).renderData();
            DoGet.getInstance().startReqImg();
            return one.GetComponent<DayAnswerPage>();
        }

        private DayAnswerPage draw(Vector2 size)
        {
            GameObject bg0 = gameObjPool.getInstance().get("bg0", typeof(ImgUI));
            bg0.transform.SetParent(this.transform, false);
            bg0.GetComponent<ImgUI>().setColor(0, 0, 0, 0)
            .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero).addClk(() => { this.freeThisPage(); });

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f));

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
            bian.transform.SetParent(kuang.transform, false);
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


            GameObject title = gameObjPool.getInstance().get("title", typeof(TextUI));
            title.transform.SetParent(bg.transform, false);
            title.GetComponent<TextUI>().setColor("#FFFFFF").setAlign("left").setFontSize(35)
                .setText("1.题目").VertiOut()
                .setSizePos(new Vector2(size.x - 40, 80), new Vector2(0, 0));

            for (int i = 0; i < 4; i++)
            {
                int index = i;
                GameObject text = gameObjPool.getInstance().get("btn", typeof(TextUI));
                text.transform.SetParent(bg.transform, false);
                text.GetComponent<TextUI>().setColor("#FFFFFF").setAlign("left").setFontSize(30)
                    .setText("选项" + i).horiOut()
                    .setSizePos(new Vector2(600, 100), new Vector2(0, -150 - 100 * i)).addClk(() => { this.choose(index + 1); });
            }


            /*GameObject right = gameObjPool.getInstance().get("right", typeof(TextUI));
            right.transform.SetParent(bg.transform, false);
            right.GetComponent<TextUI>().setColor("#5E270F").setAlign().setFontSize(30)
                .setText("准确率")
                .setSizePos(new Vector2(300, 80), new Vector2(x + (w - 800) - 250, -h + 380));*/

            this.setVisible(false);



            return this;
        }

        private JArray question;
        private int num = 0;
        private int right = 0;
        public DayAnswerPage renderData()
        {
            face.activityInterface.getHappyProgress((a) =>
            {
                num = (int)a["num"];
                right = (int)a["right_num"];
                this.question = (JArray)a["list"];

                if (num >= 10 || right >= 10)
                {
                    //提示今日已完成
                    this.transform.Find("kuang/bg/title").gameObject.GetComponent<TextUI>().setText("今日答题已完成，请明日再来");
                    return;
                }

                showQues();
                this.setVisible(true);
            });
            return this;
        }
        private void showQues()
        {
            //this.transform.Find("bg/right").GetComponent<TextUI>().setText("准确率：" + right + "/10");
            JObject que = (JObject)this.question[num];
            this.transform.Find("kuang/bg/title").GetComponent<TextUI>().setText("第" + (num + 1) + "题 " + que["question"]);
            JArray ans = (JArray)que["choose"];
            int index = 0;
            Transform bg = this.transform.Find("kuang/bg");
            for (int i = 0; i < bg.childCount; i++)
            {
                Transform ts = bg.GetChild(i);
                if (!ts.name.Equals("btn")) continue;
                ts.GetComponent<TextUI>().setText(ans[index].ToString());
                index++;
            }
            this.setClk(true);
        }
        private void choose(int chi)
        {
            this.setClk(false);
            //对比答案是否正确
            JObject que = (JObject)this.question[num];
            int answer = (int)que["answer"];
            string str = "回答错误！";
            int isRight = 0;
            if (chi == answer)
            {
                str = "回答正确！";
                isRight = 1;
            }

            TextTipUI.create().draw(str);
            timeManager.getTimeManageOne().putDelayTask(() =>
            {
                //切换下一题
                this.num++;
                if (chi == answer)
                    this.right++;
                if (num < question.Count)
                    this.showQues();
            }, 2000);

            //提交题号跟答案
            face.activityInterface.putHappyProgress(isRight);


        }
        /**设置答案是否允许点击*/
        private void setClk(bool b)
        {
            Transform bg = this.transform.Find("kuang/bg");
            for (int i = 0; i < bg.childCount; i++)
            {
                Transform ts = bg.GetChild(i);
                if (!ts.name.Equals("btn")) continue;
                ts.GetComponent<Button>().interactable = b;
            }
        }
        /**显示组件*/
        private void setVisible(bool b)
        {
            Transform bg = this.transform.Find("kuang/bg");
            foreach (Transform child in bg)
            {
                if (child.name.Equals("btn") || child.name.Equals("right"))
                    child.gameObject.SetActive(b);
            }
        }
        public override void init()
        {

        }
    }
}
