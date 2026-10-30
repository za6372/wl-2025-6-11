using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class DiaoYuPage : PageUI
    {
        private int type;
        private int times;
        public static DiaoYuPage create(int type, JObject res, Transform parent)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("DiaoYuPage", typeof(DiaoYuPage));
            DiaoYuPage bs = one.GetComponent<DiaoYuPage>();
            bs.setSize(size).putSence<DiaoYuPage>(parent).setScreenCenter();
            bs.drawUI(type, res);

            DoGet.getInstance().startReqImg();
            return bs;
        }

        public void drawUI(int type, JObject res)
        {
            this.type = type;
            if (type == 0) this.times = (int)res["free_times"];
            else times = (int)res["fufei_times"];

            this.createStandardPageLayout();
            this.setTitle("垂钓");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
            });
            this.setSureCallback(() =>
            {
                startMove();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }

        private void drawK1(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));

            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 60), new Vector2(30, -30), k2.transform);
            draw();
        }


        private void draw()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject tb = gameObjPool.getInstance().get("tiao", typeof(ImgUI));
            tb.transform.SetParent(content, false);
            tb.GetComponent<ImgUI>().loadRes("diaoyu_03_png")
                .setSizePos(new Vector2(44 * 3, 260 * 3), new Vector2((size.x - 44 * 3) / 2f, -(size.y - 260 * 3) / 2f));

            GameObject point = gameObjPool.getInstance().get("point", typeof(ImgUI));
            point.transform.SetParent(tb.transform, false);
            point.GetComponent<ImgUI>().loadRes("diaoyu_02_png")
                .setSizePos(new Vector2(20 * 3, 16 * 3), new Vector2(0, -30 * 3));

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content, false);
            text.GetComponent<TextUI>().setText("剩余次数：" + times)
                .setAlign().setColor("#000000").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(200, 40), new Vector2(20, -20));

            text = gameObjPool.getInstance().get("djs", typeof(TextUI));
            text.transform.SetParent(content, false);
            text.GetComponent<TextUI>().setText("倒计时：10")
                .setAlign().setColor("#000000").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(200, 40), new Vector2(20, -70));

        }
        private bool isStart;
        private ImgUI zhizhen;
        private TextUI djs;
        private Vector2 startAndEndPos;
        private int dir = -1;
        private long endTime;
        private void startMove()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            if (zhizhen == null)
            {
                zhizhen = content.Find("tiao/point").GetComponent<ImgUI>();
                startAndEndPos = new Vector2(zhizhen.transform.localPosition.y, zhizhen.transform.localPosition.y - 560);
                djs = content.Find("djs").GetComponent<TextUI>();
            }
            if (!isStart)
            {
                if (times <= 0)
                {
                    msgCode.showMsg(610);
                    return;
                }
                endTime = strUtils.getMillis()+10*1000;
                isStart = true;
                times--;
                content.Find("text").GetComponent<TextUI>().setText("剩余次数：" + times);
            }
            else
            {
                stop();
            }
        }
        private void stop()
        {
            isStart = false;
            Vector2 vs = zhizhen.transform.localPosition * 1f;
            int index = 0;
            if (vs.y < startAndEndPos.x - 280 + 5 && vs.y > startAndEndPos.x - 280 - 5)
            {
                index = 0;
            }
            else if (vs.y < startAndEndPos.x - 280 + 20 && vs.y > startAndEndPos.x - 280 - 20)
            {
                index = 1;
            }
            else if (vs.y < startAndEndPos.x - 280 + 55 && vs.y > startAndEndPos.x - 280 - 55)
            {
                index = 2;
            }
            else if (vs.y < startAndEndPos.x - 280 + 105 && vs.y > startAndEndPos.x - 280 - 105)
            {
                index = 3;
            }
            else if (vs.y < startAndEndPos.x - 280 + 165 && vs.y > startAndEndPos.x - 280 - 165)
            {
                index = 4;
            }
            else if (vs.y < startAndEndPos.x - 280 + 245 && vs.y > startAndEndPos.x - 280 - 245)
            {
                index = 5;
            }
            else
            {
                index = 6;
            }
            face.activityInterface.putCDProgress(index, this.type, () =>
            {

            });
        }
        public void Update()
        {
            if (!isStart) return;
            int t = (int)((endTime - strUtils.getMillis())/1000f);
            if (t <= 0)
            {
                t = 0;
                djs.setText("倒计时："+t);
                stop();
            }
            else
            {
                djs.setText("倒计时：" + t);
            }
            Vector2 vs = zhizhen.transform.localPosition * 1f;
            if (dir == -1 && vs.y < startAndEndPos.y)
            {
                dir = 1;
            }
            else if (dir == 1 && vs.y > startAndEndPos.x)
            {
                dir = -1;
            }
            zhizhen.transform.localPosition = new Vector2(vs.x, vs.y + 10 * dir);
            //zhizhen.setLeftPos(new Vector2(0, -30 * 3));
        }


        public override void init()
        {

        }
    }
}
