using Assets.HotFix.MyUtils.src.color;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.duanzao;
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
    class LvJingPage : PageUI
    {
        public LvJingPage drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("滤镜设置");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
            return this;

        }

        public void clkTab(int index)
        {
            Transform content = this.getStandardPageContent();
            content.Find("Tab").GetComponent<Tab>().clkDefault(index);
        }

        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>() {
            "滤镜",
            };

            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);

            //tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));
            //黑色部分
            GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
            hb.transform.SetParent(k2.transform, false);
            hb.GetComponent<ImgUI>().setColor32(PageSetting.HbColor)
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);
            //列表部分
            //bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform k2 = this.getStandardPageContent().Find("k2");
            if (k2.Find("bgStyle1") != null)
                gameObjPool.getInstance().free(k2.Find("bgStyle1").gameObject);
            if (k2.Find("hb") != null)
            {
                gameObjPool.getInstance().freeChildren(k2.Find("hb").gameObject);
            }
            if (index == 0)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 400), new Vector2(30, -30 - 400), k2.transform);
                draw0();
            }




        }

        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");

            Vector2 hbSize = hb.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(hb.transform, false);
            text.GetComponent<TextUI>().setText("模糊效果需设置透明度为100%才有效。")
                .setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle().setIsRichText()
                .setSizePos(new Vector2(hbSize.x - 40, hbSize.y - 40), new Vector2(20, -20));

            string[] m2 = { "液晶效果", "模糊效果", };
            JArray list = new JArray();
            for (int i = 0; i < m2.Length; i++)
            {
                JObject a = new JObject();
                a.Add("name", m2[i]);
                list.Add(a);
            }
            gameObjPool.getInstance().freeChildren(content.gameObject);
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                Dialog dialog = Dialog.create(new Vector2(800, 600), this.transform);
                Transform ts = dialog.getContent();
                drawColorPanel(mIndex,ts);

                DoGet.getInstance().startReqImg();


            });
        }
        private void loadLvJing(string ljName,Color color1, float mdK)
        {
            Transform bg = PointGet.getC2dFilterCanvas().Find("yejing");
            bg.gameObject.SetActive(true);
            bg.GetComponent<Image>().material = null;
            DoGet.getInstance().loadAnyPrefab("shaders", (ab) =>
            {

                Material mat = new Material(ab.LoadAsset<Shader>(ljName+".shader"));
                if (ljName.Equals("GridShader"))
                {
                    mat.SetFloat("_GridSize", 100f + 900f * mdK);
                    mat.SetColor("_Color1", color1);
                    mat.SetColor("_Color2", new Color(1, 1, 1, 0));
                }else if (ljName.Equals("GaussianBlurMask"))
                {
                    bg.GetComponent<Image>().color = color1;
                    mat.SetFloat("_BlurSize", mdK*10f);
                }
                
                bg.GetComponent<Image>().material = mat;
            });
        }
        /**颜色选择器面板*/
        private void drawColorPanel(int type,Transform ts)
        {
            string ljName = null;
            if (type == 0) ljName = "GridShader";
            else if (type == 1) ljName = "GaussianBlurMask";

            GameObject Panel = gameObjPool.getInstance().get("Panel", typeof(ImgUI));
            Panel.transform.SetParent(ts.transform, false);
            Panel.GetComponent<ImgUI>().setColor(0, 0, 0, 0)
                .setSizePos(new Vector2(256, 256), new Vector2(200, 0));

            GameObject ColorPanel = gameObjPool.getInstance().get("ColorPanel", typeof(RawImage));
            ColorPanel.transform.SetParent(Panel.transform, false);
            ColorPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 256);
            ColorPanel.transform.localPosition = new Vector2(-200, 0);

            GameObject ColorCircle = gameObjPool.getInstance().get("ColorCircle", typeof(RawImage));
            ColorCircle.transform.SetParent(ColorPanel.transform, false);
            ColorCircle.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            ColorCircle.GetComponent<RectTransform>().anchorMax = Vector2.zero;
            ColorCircle.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
            ColorCircle.transform.localPosition = new Vector2(-127, 127);

            GameObject ColorRgb = gameObjPool.getInstance().get("ColorRgb", typeof(RawImage));
            ColorRgb.transform.SetParent(Panel.transform, false);
            ColorRgb.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 256);
            ColorRgb.transform.localPosition = Vector2.zero;

            GameObject slider = gameObjPool.getInstance().get("mySlider", typeof(SliderUI));
            slider.transform.SetParent(ColorRgb.transform, false);
            slider.GetComponent<SliderUI>().create();
            slider.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 256);
            slider.transform.localPosition = Vector2.zero;

            GameObject ColorShow = gameObjPool.getInstance().get("ColorShow", typeof(ImgUI));
            ColorShow.transform.SetParent(Panel.transform, false);
            ColorShow.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            ColorShow.transform.localPosition = new Vector2(100, 0);

            ColorPanel colorPanel = ColorPanel.AddComponent<ColorPanel>();
            colorPanel.ri = ColorPanel.GetComponent<RawImage>();
            colorPanel.circleRect = ColorCircle.GetComponent<RectTransform>();
            ColorCircle.AddComponent<ColorCircle>();
            ColorRgb.AddComponent<ColorRGB>().ri = ColorRgb.GetComponent<RawImage>();

            ColorManager colorManager = Panel.AddComponent<ColorManager>();
            colorManager.sliderCRGB = slider.GetComponent<Slider>();
            colorManager.colorShow = ColorShow.GetComponent<Image>();

            //调整透明度
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(Panel.transform, false);
            text.GetComponent<TextUI>().setText("0%").setAlign("left").setColor("#FFEB00").setFontSize(26).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 30), new Vector2(-198 + 266, -266));

            JObject msg= PageSetting.getLvJing();
            //todo:将设置进行回显

            float a = 0;
            float mdK = 0;
            Action ac = () =>
            {
                Color color = ColorShow.GetComponent<Image>().color;
                color.a = a;
                loadLvJing(ljName,color, mdK);
            };
            GameObject touming = gameObjPool.getInstance().get("touming", typeof(SliderUI));
            touming.transform.SetParent(Panel.transform, false);
            touming.GetComponent<SliderUI>().create().setHor().setBgColor(new Color32(0, 125, 0, 255));
            touming.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 30);
            touming.GetComponent<SliderUI>().setSizePos(new Vector2(256, 30), new Vector2(-198, -266));
            touming.GetComponent<SliderUI>().addValueChangeCheck((k) =>
            {
                text.GetComponent<TextUI>().setText((int)(k * 100) + "%");
                a = k;
                ac();
            });
            touming.GetComponent<Slider>().value = 0.3f;


            GameObject midu = gameObjPool.getInstance().get("midu", typeof(SliderUI));
            midu.transform.SetParent(Panel.transform, false);
            midu.GetComponent<SliderUI>().create().setHor().setBgColor(new Color32(0, 125, 0, 255));
            midu.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 30);
            midu.GetComponent<SliderUI>().setSizePos(new Vector2(256, 30), new Vector2(-198, -336));
            midu.GetComponent<SliderUI>().addValueChangeCheck((k) =>
            {
                mdK = k;
                ac();
            });

            GameObject yulan = gameObjPool.getInstance().get("yulan", typeof(BtnUI));
            yulan.transform.SetParent(Panel.transform, false);
            yulan.GetComponent<BtnUI>()
            .setSizePos(new Vector2(120, 60), new Vector2(-198, -430));
            yulan.GetComponent<BtnUI>().addText("启用", 30).setTextColor().loadRes("gy_02_png")
                .addClk(() =>
                {
                    Transform bg = PointGet.getC2dFilterCanvas().Find("yejing");
                    bg.gameObject.SetActive(true);
                    //将颜色缓存
                    Color color = ColorShow.GetComponent<Image>().color;
                    color.a = a;
                    PageSetting.saveLvJing(ljName,color, mdK);
                });
            yulan = gameObjPool.getInstance().get("yulan", typeof(BtnUI));
            yulan.transform.SetParent(Panel.transform, false);
            yulan.GetComponent<BtnUI>()
            .setSizePos(new Vector2(120, 60), new Vector2(-198+130, -430));
            yulan.GetComponent<BtnUI>().addText("关闭", 30).setTextColor().loadRes("gy_02_png")
                .addClk(() =>
                {
                    Transform bg = PointGet.getC2dFilterCanvas().Find("yejing");
                    bg.GetComponent<Image>().color = Color.white;
                    bg.gameObject.SetActive(false);
                });
        }
        private void drawBtn(string str, Transform ts, Vector2 pos, Action ac)
        {
            GameObject tb = gameObjPool.getInstance().get("tb", typeof(ImgUI));
            tb.transform.SetParent(ts);
            tb.GetComponent<ImgUI>().setRoundedCorners(1, "#005D5F")
                .setSizePos(new Vector2(120, 60), pos)
                .addClk(ac);
            tb.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(2, -2));
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(tb.transform);
            text.GetComponent<TextUI>().setText(str).setAlign().setColor("#ffffff").setFontSize(30).setNoClk().setFontStyle()
                .setSizePos(new Vector2(120, 60), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));
        }
        public override void init()
        {

        }
    }
}
