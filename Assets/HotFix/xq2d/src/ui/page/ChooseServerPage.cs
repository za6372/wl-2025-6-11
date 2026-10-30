using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class ChooseServerPage : PageUI
    {
        //是否处于选区 false为选服
        private bool isArea = true;
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("服务器列表");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
                PageUI.create<LoginPage>().drawUI();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            drawK2(content);
            DoGet.getInstance().startReqImg();
        }
        private void drawK1(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, 300), new Vector2(30, -30), content);

            size = k1.getContent().GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(k1.getContent(), false);
            text.GetComponent<TextUI>().setColor(PageUI.PageDefaltColor)
                .setText("默认服务器").setAlign().setFontSize().setFontStyle()
            .setSizePos(new Vector2(size.x - 100, 80), new Vector2(50, -(size.y - 80) / 2 + 80));

            GameObject item = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item.transform.SetParent(k1.getContent(), false);
            item.GetComponent<ImgUI>().setRoundedCorners(1f,"#BCEDE7")
            .setSizePos(new Vector2(size.x - 100, 80), new Vector2(50, -(size.y - 80) / 2))
            .addClk(() =>
            {
                this.freeThisPage();
                PageUI.create<ChooseRolePage>().drawUI();
            });
            item.AddComponent<UIOutline>().setSome(new Color32(0, 215, 255, 255), new Vector2(2, -2));

            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(item.transform, false);
            text.GetComponent<TextUI>().setColor("#0CA894")
                .setText("一服 - 乱世群雄").setAlign().setFontSize(25).setNoClk().setFontStyle()
            .setSizePos(new Vector2(size.x - 100, 80), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
            
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 330 - 60), new Vector2(30, -360), content);

            size = k1.getContent().GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(k1.getContent(), false);
            text.GetComponent<TextUI>().setColor(PageUI.PageDefaltColor)
                .setText("选择区").setAlign().setFontSize().setFontStyle()
            .setSizePos(new Vector2(size.x - 100, 80), new Vector2(50, -50));

            GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
            item.transform.SetParent(k1.getContent(), false);
            item.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 100, 80), new Vector2(50, -50 - 80));

            GameObject pt = gameObjPool.getInstance().get("p1", typeof(ImgUI));
            pt.transform.SetParent(item.transform, false);
            pt.GetComponent<ImgUI>().loadRes("dl_4_png", new Rect(44, 0, 21, 24))
            .setSizePos(new Vector2(70, 80), new Vector2(0, 0));

            pt = gameObjPool.getInstance().get("p2", typeof(ImgUI));
            pt.transform.SetParent(item.transform, false);
            pt.GetComponent<ImgUI>().loadRes("dl_4_png", new Rect(65, 0, 2, 24))
            .setSizePos(new Vector2(size.x - 100 - 160, 80), new Vector2(70, 0));

            pt = gameObjPool.getInstance().get("p3", typeof(ImgUI));
            pt.transform.SetParent(item.transform, false);
            pt.GetComponent<ImgUI>().loadRes("dl_4_png", new Rect(67, 0, 21, 24))
            .setSizePos(new Vector2(70, 80), new Vector2(70 + size.x - 100 - 160, 0));

            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(item.transform, false);
            text.GetComponent<TextUI>().setColor("#C3C3C3").setFontStyle()
                .setText("一区").setAlign().setFontSize(25)
            .setSizePos(new Vector2(size.x - 100, 80), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));

            item.GetComponent<SimpleUI>().addClk( () =>
            {
                handle(item);

            });

        }
        private void handle(GameObject item)
        {
            if (isArea)
            {
                Transform ts = item.transform;
                change(true, ts);
            }
            else
            {
                this.freeThisPage();
                PageUI.create<ChooseRolePage>().drawUI();
            }
        }
        private void change(bool b,Transform ts)
        {
            if (b)
            {
                isArea = false;
                //改变图片
                ts.Find("p1").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(0, 0, 21, 24));

                ts.Find("p2").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(21, 0, 2, 24));

                ts.Find("p3").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(23, 0, 21, 24));

                ts.Find("text").GetComponent<TextUI>().setText("一服 - 乱世群雄");
                ts.parent.transform.Find("text").GetComponent<TextUI>().setText("选择服");
                DoGet.getInstance().startReqImg();
                this.setCancelCallback(() =>
                {
                    change(false, ts);
                });
            }
            else
            {
                this.isArea = true;
                ts.Find("p1").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(44, 0, 21, 24));

                ts.Find("p2").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(65, 0, 2, 24));

                ts.Find("p3").GetComponent<ImgUI>()
                .loadRes("dl_4_png", new Rect(67, 0, 21, 24));

                ts.Find("text").GetComponent<TextUI>().setText("一区");
                ts.parent.transform.Find("text").GetComponent<TextUI>().setText("选择区");
                DoGet.getInstance().startReqImg();
                this.setCancelCallback(() =>
                {
                    this.freeThisPage();
                    PageUI.create<LoginPage>().drawUI();
                });
            }
            
        }


        public override void init()
        {

        }
    }
}
