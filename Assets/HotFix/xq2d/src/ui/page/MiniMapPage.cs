using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.Res.script.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class MiniMapPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("世界地图");
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
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            //225*259
            float w = size.x - 60;
            float h = 259 / 225 * w;
            bgStyle1 k1 = bgStyle1.create(new Vector2(w, h), new Vector2(30, -(size.y - h) / 2), content);
            k1.setCenterBg("mini_map_png");

            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            items.transform.SetParent(content.transform,false);
            items.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(w, h), new Vector2(30, -(size.y - h) / 2));

            size = items.GetComponent<RectTransform>().sizeDelta;

            string[] names = {
            "太虚之峰","咸阳城","太虚之林","恒山村","天元境","安邑县","汉中郡","三秦地",
            "邯郸郡","吴中城","青阳境","太虚之谷","酆都城",
            };
            Vector2[] sizes =
            {
                new Vector2(92,92),new Vector2(120,112),new Vector2(92,92),
                new Vector2(84,84),new Vector2(84,84),new Vector2(84,84),
                new Vector2(120,112),new Vector2(84,84),new Vector2(120,112),
                new Vector2(120,112),new Vector2(84,84),new Vector2(88,84),
                new Vector2(120,112)
            };
            Vector2[] poss =
            {
                new Vector2(156,-60),new Vector2(344,-150),new Vector2(645,-100),
                new Vector2(37,-320),new Vector2(369,-322),new Vector2(744,-318),
                new Vector2(19,-483),new Vector2(163,-556),new Vector2(336,-496),
                new Vector2(713,-516),new Vector2(351,-682),new Vector2(156,-844),
                new Vector2(338,-845)
            };
            string[] pics = {
            "zf1","1B","zl1","2B","2B","2B","1B","2B","1B","1B","zg1","2B","1B",
            };
            //以上点是基于960宽设定
            float kx = size.x / (960 - 60);
            float ky = size.y / (1094 - 60);
            for (int i = 0; i < names.Length; i++)
            {
                int index = i;
                GameObject item = gameObjPool.getInstance().get("i" + (i + 1), typeof(ImgUI));
                item.transform.SetParent(items.transform, false);
                item.GetComponent<ImgUI>().loadRes(pics[i] + "_png")
                    .setSizePos(sizes[i], new Vector2(poss[i].x * kx, poss[i].y * ky))
                    .addClk(() =>
                    {
                        handle(index);
                    });
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(item.transform, false);
                text.GetComponent<TextUI>().setAlign().setColor("#D9E49D").setFontSize().setFontStyle().setText(names[i])
                    .setSizePos(new Vector2(160, 60), new Vector2((sizes[i].x - 160) / 2, -84));
            }

            GameObject jt = gameObjPool.getInstance().get("jt" , typeof(ImgUI));
            jt.transform.SetParent(items.transform, false);
            jt.GetComponent<ImgUI>().loadRes("map_p_png")
                .setSizePos(new Vector2(44, 88), new Vector2(58 * kx, -251 * ky));
        }
        private void handle(int index)
        {
            if (index == 0 || index == 2 || index == 11)
            {
                if (!face.teamInterface.isAllowedTouchMove())
                {
                    msgCode.showMsg(1026);
                    return;
                }
                //进入主峰图
                if (index == 0) mapManager.getInstance().reloadBef("txzf");
                else if (index == 2) mapManager.getInstance().reloadBef("txzl");
                else if (index == 11) mapManager.getInstance().reloadBef("txzg");
                this.freeThisPage();
            }
            else
            {
                //展开小地图
                this.freeThisPage();
                //显示展开的地图并渲染
                PageUI.createAcPage<MiniMapChoosePage>(PointGet.getIndexPageOfPage()).drawUI(index);

            }
        }
        public override void init()
        {

        }
    }
}
