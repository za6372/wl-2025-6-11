using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.ui.part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.basicUI
{
    /**页面级*/
    public abstract class PageUI : BaseUI
    {
        
        //页面默认背景颜色
        public static string PageDefaltColor = "#005d5f";

        //标准页面布局
        public void createStandardPageLayout()
        {
            GameObject top = gameObjPool.getInstance().get("Top", typeof(SimpleUI));
            top.transform.SetParent(this.transform, false);
            top.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 90), new Vector2(0, 0));
            drawTop(top.transform);

            GameObject center = gameObjPool.getInstance().get("Center", typeof(SimpleUI));
            center.transform.SetParent(this.transform, false);
            center.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height - 90 - 94), new Vector2(0, -90));
            drawCenter(center.transform);

            GameObject bottom = gameObjPool.getInstance().get("Bottom", typeof(SimpleUI));
            bottom.transform.SetParent(this.transform,false);
            bottom.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 96), new Vector2(0, -(ScreenUtils.height - 96)));
            drawBottom(bottom.transform);
        }
        /**获取标准布局的内容区*/
        public Transform getStandardPageContent()
        {
            return this.transform.Find("Center/content");
        }
        /**刷新含Tab的页面*/
        public void updateTabPage(int i)
        {
            this.getStandardPageContent().Find("Tab").GetComponent<Tab>().clkDefault(i);
        }
        //取消回调
        private Action cancelCallback;
        public void setCancelCallback(Action cancelCallback)
        {
            this.cancelCallback = cancelCallback;
            this.transform.Find("Bottom/cancel").gameObject.SetActive(true);
        }
        private Action sureCallback;
        public void setSureCallback(Action sureCallback)
        {
            this.sureCallback = sureCallback;
            this.transform.Find("Bottom/sure").gameObject.SetActive(true);
        }
        /**设置标准布局的标题*/
        public void setTitle(string str)
        {
            this.transform.Find("Top/title").GetComponent<TextUI>().setText(str);
        }

        private void drawCenter(Transform ts)
        {
            float w = ScreenUtils.width;
            float h = ScreenUtils.height - 90 - 94;
            GameObject center = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            center.transform.SetParent(ts, false);
            center.GetComponent<ImgUI>().setColor32(PageSetting.PgColor)
                .setSizePos(new Vector2(w, h), new Vector2(0, 0));

            GameObject k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_03_png", new Rect(0, 0, 7, 12))
                .setSizePos(new Vector2(14, 24), new Vector2(0, 0));

            k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_03_png", new Rect(7, 0, 1, 12))
                .setSizePos(new Vector2(w - 28, 24), new Vector2(14, 0));

            k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_04_png", new Rect(1, 0, 7, 12))
                .setSizePos(new Vector2(14, 24), new Vector2(w - 14, 0));
            //下方
            k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_03_png", new Rect(0, 0, 7, 12))
                .setSizePos(new Vector2(14, 24), new Vector2(0, -(h - 24)));
            k.transform.rotation = Quaternion.Euler(180, 0, 0);

            k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_03_png", new Rect(7, 0, 1, 12))
                .setSizePos(new Vector2(w - 28, 24), new Vector2(14, -(h - 24)));
            k.transform.rotation = Quaternion.Euler(180, 0, 0);

            k = gameObjPool.getInstance().get("k", typeof(ImgUI));
            k.transform.SetParent(center.transform, false);
            k.GetComponent<ImgUI>().loadRes("ui_04_png", new Rect(1, 0, 7, 12))
                .setSizePos(new Vector2(14, 24), new Vector2(w - 14, -(h - 24)));
            k.transform.rotation = Quaternion.Euler(180, 0, 0);

            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(ts, false);
            content.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(w, h), new Vector2(0, 0));

        }

        private void drawBottom(Transform ts)
        {
            GameObject img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(ScreenUtils.width, 96), new Vector2(0, 0));

            GameObject cancel = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            cancel.transform.SetParent(ts, false);
            cancel.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(ScreenUtils.width - 168, 0));
            cancel.GetComponent<BtnUI>().addText("取消").setTextColor().loadRes("gy_02_png")
                .addClk(() =>
                {
                    if (cancelCallback != null) cancelCallback();
                });
            cancel.GetComponent<BtnUI>().getTextUI().gameObject
                .AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));
            cancel.SetActive(false);

            GameObject sure = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            sure.transform.SetParent(ts, false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("确定").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    if (sureCallback != null) sureCallback();
                });
            sure.GetComponent<BtnUI>().getTextUI().gameObject
                .AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));
            sure.SetActive(false);
        }
        private void drawTop(Transform ts)
        {
            GameObject img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().setColor32(PageSetting.TopColor1)
            .setSizePos(new Vector2(ScreenUtils.width, 90), new Vector2(0, 0));
            GradientDefinded gd = img.AddComponent<GradientDefinded>();
            gd.dir = Dir.Vertical;
            //gd.range = 0.8f;
            gd.color1 = PageSetting.TopColor1;
            gd.color2 = PageSetting.TopColor2;
            gd.toUpdate();

            img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().loadRes("ui_01_png", new Rect(0, 0, 38, 20))
            .setSizePos(new Vector2(171, 90), new Vector2(ScreenUtils.width / 2 - 100 - 171, 0));

            img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().loadRes("ui_01_png", new Rect(38, 0, 8, 20))
            .setSizePos(new Vector2(200, 90), new Vector2(ScreenUtils.width / 2 - 100, 0));

            img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().loadRes("ui_02_png", new Rect(8, 0, 38, 20))
            .setSizePos(new Vector2(171, 90), new Vector2(ScreenUtils.width / 2 + 100, 0));

            GameObject title = gameObjPool.getInstance().get("title", typeof(TextUI));
            title.transform.SetParent(ts, false);
            title.GetComponent<TextUI>().setColor().setAlign().setFontSize(30).setText("标题")
            .setSizePos(new Vector2(ScreenUtils.width, 90), new Vector2(0, 0));
            title.AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));
        }






        /**创建页面，只能是登录页、选服、创角、选角、主页使用，活动页不允许使用*/
        public static T create<T>()
        {
            Type tp = typeof(T);
            GameObject one = gameObjPool.getInstance().get(tp.Name, tp);
            PageUI pg = one.GetComponent<PageUI>();
            pg.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height));
            Transform ts = PointGet.getC2dCanvas();
            pg.putSence<PageUI>(ts);
            pg.setLayerCenter();
            //pg.setLeftPos(SetGameObj.getCenterPos());
            return one.GetComponent<T>();
        }
        /**活动页使用*/
        public static T createAcPage<T>(Transform parent)
        {
            Type tp = typeof(T);
            GameObject one = gameObjPool.getInstance().get(tp.Name, tp);
            PageUI pg = one.GetComponent<PageUI>();
            pg.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height));
            pg.putSence<PageUI>(parent);
            pg.setScreenCenter();
            return one.GetComponent<T>();
        }
        /**设置父级*/
        public void SetParent(Transform parent)
        {
            this.gameObject.transform.SetParent(parent);
        }

        public void freeC2dCanvas()
        {
            GameObject canvas = PointGet.getC2dCanvas().gameObject;
            gameObjPool.getInstance().freeChildren(canvas);
        }
        /**当前页放入空闲区*/
        public void freeThisPage()
        {
            gameObjPool.getInstance().free(this.gameObject);
        }



    }
}
