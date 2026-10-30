using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.Res.script.src.data;
using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class MiniMapChoosePage : PageUI
    {
        public void drawUI(int index)
        {
            this.createStandardPageLayout();
            this.setTitle("");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
                PageUI.createAcPage<MiniMapPage>(PointGet.getIndexPageOfPage()).drawUI();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content, index);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content, int index)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(content, false);
            scroll.GetComponent<ScrollUI>().setSizePos(new Vector2(size.x-20,size.y-20), new Vector2(10, -10));
            scroll.GetComponent<ScrollUI>().initSetting(true, true);

            GameObject sc = gameObjPool.getInstance().get("panel", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(sc);


            draw(index, sc);

            drawDes(content);


        }
        private void drawDes(Transform panel)
        {
            Vector2 sizeDelta = panel.GetComponent<RectTransform>().sizeDelta;
            Vector2 size = new Vector2(sizeDelta.x - 100, 380);

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(panel.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2(50, -(sizeDelta.y - 400)));

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

            //内容区
            GameObject content = gameObjPool.getInstance().get("des", typeof(TextUI));
            content.transform.SetParent(panel.transform, false);
            content.GetComponent<TextUI>().setColor().setAlign("leftTop").setFontSize()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(50 + 20, -(sizeDelta.y - 400 + 20)));
        }
        /**绘制展开的小地图*/
        public void draw(int index, GameObject content)
        {
            List<Vector4> arr = new List<Vector4>();
            List<MiniData> list = MiniMapData.getMiniMapData(index);
            if (list.Count == 0)
            {
                Debug.Log("没有找到任何数据");
                return;
            }
            //计算偏移量
            float maxX = 0, maxY = 0;
            for (int i = 0; i < list.Count; i++)
            {
                MiniData a = list[i];
                if (a.pos.x > maxX) maxX = a.pos.x;
                if (a.pos.y > maxY) maxY = a.pos.y;
            }
            float dx = (ScreenUtils.width - maxX * 80 - 80) / 2f;
            float dy = -(ScreenUtils.height - 90 - 96 - 400 - maxY * 80 - 80) / 2f;
            if (dx < 0) dx = 0;
            if (dy > 0) dy = 0;

            for (int i = 0; i < list.Count; i++)
            {
                MiniData a = list[i];
                GameObject g = gameObjPool.getInstance().get("m_" + i, typeof(ImgUI));
                g.transform.SetParent(content.transform, false);
                ImgUI img = g.GetComponent<ImgUI>();
                //todo:读取图片
                //0城池 1小县 2山 3路 4箭头
                if (a.type == 0)
                {
                    img.loadRes("minimap_png", new Rect(20 * 8, 0, 20, 20));
                }
                else if (a.type == 1)
                {
                    img.loadRes("minimap_png", new Rect(20 * 7, 0, 20, 20));
                }
                else if (a.type == 2)
                {
                    img.loadRes("minimap_png", new Rect(20 * 6, 0, 20, 20));
                }
                else if (a.type == 3)
                {
                    if (a.hori) img.loadRes("minimap_png", new Rect(20 * 5, 0, 20, 20));
                    else img.loadRes("minimap_png", new Rect(20 * 4, 0, 20, 20));
                }
                else if (a.type == 4)
                {
                    if (a.direction == 0) img.loadRes("minimap_png", new Rect(20 * 3, 0, 20, 20));
                    else if (a.direction == 1) img.loadRes("minimap_png", new Rect(20 * 2, 0, 20, 20));
                    else if (a.direction == 2) img.loadRes("minimap_png", new Rect(20 * 1, 0, 20, 20));
                    else if (a.direction == 3) img.loadRes("minimap_png", new Rect(20 * 0, 0, 20, 20));

                }


                img.setSizePos(Vector2.one * 80, new Vector2(dx + a.pos.x * 80, dy - a.pos.y * 80));
                SetGameObj.setScaleByK(g);
                img.addClk(() =>
                {
                    handle(a, dx, dy);
                });

                arr.Add(new Vector4(a.pos.x * 80, a.pos.y * 80, 80, 80));
            }
            //计算总图大小
            Vector2 size = numberUtils.getAllPicGround(arr);
            this.getStandardPageContent().Find("scroll").GetComponent<ScrollUI>()
                .updateHeight(size.y+400).updateWidth(size.x);

            GameObject bg = gameObjPool.getInstance().get("choose", typeof(ImgUI));
            bg.transform.SetParent(content.transform, false);
            bg.GetComponent<ImgUI>().loadRes("s_png").setNoClk()
            .setSizePos(Vector2.one * 80, new Vector2(-2000, 0));
        }
        private string mKey;
        private void handle(MiniData miniData, float dx, float dy)
        {
            if (miniData.type == 0 || miniData.type == 1 || miniData.type == 2)
            {
                if (miniData.mapKey != mKey)
                {
                    this.mKey = miniData.mapKey;
                    //提示出现的怪物
                    MapData md = face.mapInterface.getMapByKey(miniData.mapKey);
                    this.getStandardPageContent().Find("des").GetComponent<TextUI>().setText(md.des);
                    this.setTitle(md.name);

                    this.getStandardPageContent().Find("scroll").GetComponent<ScrollUI>()
                    .getContent().transform.Find("choose").GetComponent<ImgUI>()
                    .setLeftPos(new Vector2(dx + miniData.pos.x * 80, dy - miniData.pos.y * 80));
                    return;
                }
                if (!face.teamInterface.isAllowedTouchMove())
                {
                    return;
                }
                //第二次点击才进入

                mapManager.getInstance().reloadBef(miniData.mapKey);
                this.freeThisPage();


            }
            else if (miniData.type == 4)
            {
                //todo:展开下一张
                this.getStandardPageContent().Find("des").GetComponent<TextUI>().setText("");
                this.setTitle("");
                GameObject sc = gameObjPool.getInstance().get("panel", typeof(SimpleUI));
                this.getStandardPageContent().Find("scroll").GetComponent<ScrollUI>().setContent(sc);
                draw(miniData.minIndex, sc);
                DoGet.getInstance().startReqImg();
            }
        }
        public override void init()
        {

        }
    }
}
