using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.Res.script.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class LoadingUI : PartUI
    {
        private static GameObject one;
        private long oldTime;
        public static LoadingUI getOne()
        {
            if (one == null || one.GetComponent<LoadingUI>() == null)
            {
                one = create();
            }
            return one.GetComponent<LoadingUI>();
        }
        private static GameObject create()
        {
            GameObject one = gameObjPool.getInstance().get("LoadingUI", typeof(LoadingUI));
            one.GetComponent<LoadingUI>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<LoadingUI>(PointGet.getTipCanvas()).setScreenCenter();
            one.GetComponent<LoadingUI>().draw().addClk(() => { });
            return one;
        }
        private LoadingUI draw()
        {
            GameObject bg = gameObjPool.getInstance().get("lgBg", typeof(ImgUI));
            bg.transform.SetParent(this.transform, false);
            bg.GetComponent<ImgUI>().setColor("#000000").setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);

            GameObject am = readStartAm.readLoadingAm((am) =>
            {

            });
            am.transform.SetParent(this.transform, false);
            am.GetComponent<SimpleUI>().setPos(Vector2.zero);

            float k = (ScreenUtils.width - 100) / 230f;
            //提示文字
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(bg.transform, false);
            text.GetComponent<TextUI>().setText("xxx")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText().horiOut()
                .setSizePos(new Vector2(200, 40), new Vector2(50, -ScreenUtils.height + 250));

            GameObject bian = gameObjPool.getInstance().get("tiao", typeof(ImgUI));
            bian.transform.SetParent(bg.transform, false);
            bian.GetComponent<ImgUI>().setRoundedCorners(0.5f, "#333333")
            .setSizePos(new Vector2(230 * k, 4 * k), new Vector2(50, -ScreenUtils.height + 200));
            bian.AddComponent<UIOutline>().setSome(new Color32(170, 124, 38, 255), new Vector2(2, -2));

            GameObject jd = gameObjPool.getInstance().get("jd", typeof(ImgUI));
            jd.transform.SetParent(bian.transform, false);
            jd.GetComponent<ImgUI>()//.setRoundedCorners(0.5f, "#FFB530")
            .setSizePos(new Vector2(230 * k, 4 * k), new Vector2(2, -2));
            GradientDefinded gd = jd.AddComponent<GradientDefinded>();
            gd.color1 = new Color32(171, 255, 0, 255);
            gd.color2 = new Color32(0, 112, 13, 255);

            GameObject chongzai = gameObjPool.getInstance().get("chongzai", typeof(TextUI));
            chongzai.transform.SetParent(bg.transform, false);
            chongzai.GetComponent<TextUI>().setText("失败点我重载")
                .setAlign().setColor("#FF5600").setFontSize(30).setFontStyle().setIsRichText().horiOut()
                .setSizePos(new Vector2(200, 40), new Vector2(ScreenUtils.width - 300, -ScreenUtils.height + 250)).addClk(() =>
                {
                    chongzai.SetActive(false);
                    //超时没加载完则重新加载
                    this.free();
                    LoadingUI.getOne().updateJd(0, "加载失败，尝试重新载入");
                    mapManager.getInstance().reloadBef();
                    timeManager.getTimeManageOne().putDelayTask(() =>
                    {
                        if (chongzai != null)
                        {
                            chongzai.SetActive(true);
                        }
                    }, 3000);

                });
            chongzai.SetActive(false);
            timeManager.getTimeManageOne().putDelayTask(() =>
            {
                if (chongzai != null)
                {
                    chongzai.SetActive(true);
                }
            },3000);

            DoGet.getInstance().startReqImg();

            updateJd(0, "");
            return this;
        }
        public void updateJd(float rate, string tip)
        {
            this.oldTime = strUtils.getMillis();
            float k = (ScreenUtils.width - 100) / 230f;
            Transform ts = this.transform.Find("lgBg/tiao/jd");
            if (ts == null) return;
            this.transform.Find("lgBg/text").GetComponent<TextUI>().setText(tip);
            ts.GetComponent<ImgUI>()
                .setSizePos(new Vector2(230 * k * rate, 4 * k), new Vector2(0, 0));
        }

        public new void free()
        {
            one = null;
            base.free();
        }
    }
}
