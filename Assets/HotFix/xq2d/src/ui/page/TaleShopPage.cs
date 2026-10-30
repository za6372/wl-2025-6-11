using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
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

namespace Assets.HotFix.xq2d.src.ui.page
{
    class TaleShopPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("银两商城");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("银两");
            ml.Add("银票");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                drawTale();
                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);

            tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform,false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));

            //黑色部分
            GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
            hb.transform.SetParent(k2.transform, false);
            hb.GetComponent<ImgUI>().setColor32(PageSetting.HbColor)
                .setSizePos(new Vector2(size.x, 200), Vector2.zero);
            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 200 - 60), new Vector2(30, -230), k2.transform);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            JArray list = null;
            if (index == 0)
            {
                list = face.goodsInterface.getTaleShop();
            }
            else
            {
                list = face.goodsInterface.getYpShop();
            }
            
            GoodsItem gdItem = GoodsItem.create(list, content.transform,2);
            gdItem.addCallback((mIndex) =>
            {
                JObject qp = (JObject)list[mIndex];
                List<string> ml = new List<string>();
                ml.Add("购买");
                ml.Add("查看");
                Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                menu.addCallback((mIndex) =>
                {
                    if (mIndex == 0)
                    {
                        UseNumInputUI.create(this.transform).addCallback((num) =>
                        {
                            int priceType = 1;
                            if (index == 1) priceType = 2;
                            face.goodsInterface.buyGoods(qp["key"].ToString(), num, priceType, null, (b) => { this.updateHb(); });
                        });
                    }
                    else
                    {
                        GoodsDesUI.create(qp, this.transform, 0).renderText();
                    }
                });
            });
        }
        public void updateHb()
        {
            JObject role = face.roleInterface.getRole();
            JObject bb = face.goodsInterface.getBBNAndCKN();
            JArray all = face.goodsInterface.getAllGoods();
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            hb.Find("tale").GetComponent<TextUI>().setText("银两：" + role["attr"]["msg"]["tale"]);
            hb.Find("yp").GetComponent<TextUI>().setText("银票：" + role["attr"]["msg"]["yp"]);
            hb.Find("packNum").GetComponent<TextUI>().setText("" + all.Count + " / " + bb["bbn"] + "");
        }
        /**在黑色部分展示银两*/
        private void drawTale()
        {
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            gameObjPool.getInstance().freeChildren(hb.gameObject);

            Vector2 size = hb.GetComponent<RectTransform>().sizeDelta;
            JObject role = face.roleInterface.getRole();
            JArray bb = face.goodsInterface.getAllGoods();
            JObject nk = face.goodsInterface.getBBNAndCKN();

            GameObject tale = gameObjPool.getInstance().get("tale", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("银两：" + role["attr"]["msg"]["tale"]).setAlign("left").setFontSize().horiOut()
                .setSizePos(new Vector2(200, 100), new Vector2(100, 0));

            tale = gameObjPool.getInstance().get("taleIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("moneyImge_png", new Rect(13 * 2, 0, 13, 13))
                .setSizePos(new Vector2(50, 50), new Vector2(25, -25));

            tale = gameObjPool.getInstance().get("yp", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("银票：" + role["attr"]["msg"]["yp"]).setAlign("left").setFontSize().horiOut()
                .setSizePos(new Vector2(200, 100), new Vector2(100, -100));

            tale = gameObjPool.getInstance().get("goldIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("moneyImge_png", new Rect(13 * 3, 0, 13, 13))
                .setSizePos(new Vector2(50, 50), new Vector2(25, -100 - 25));

            tale = gameObjPool.getInstance().get("packIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("packCount_png", new Rect(0, 0, 22, 19))
                .setSizePos(new Vector2(115, 100), new Vector2(size.x - 125 - 15, -10));

            tale = gameObjPool.getInstance().get("packNum", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("(" + bb.Count + "/" + nk["bbn"] + ")").setAlign().setFontSize()
                .setSizePos(new Vector2(300, 100), new Vector2(size.x - 125-100, -100));
        }
        public override void init()
        {

        }
    }
}
