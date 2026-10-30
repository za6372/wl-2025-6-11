using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.part
{
    /**背景样式1，白底加四个角*/
    class bgStyle1 : PartUI
    {
        public static bgStyle1 create(Vector2 size,Vector2 pos, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("bgStyle1", typeof(bgStyle1));
            bgStyle1 bs=one.GetComponent<bgStyle1>();
            bs.setSize(size).putSence<bgStyle1>(parent).setLeftPos(pos);
            bs.draw(size);


            return bs;
        }
        public void setCenterBg(string pic)
        {
            this.transform.Find("kuang/bg").GetComponent<ImgUI>().setColor("#ffffff").loadRes(pic);
        }
        private bgStyle1 draw(Vector2 size)
        {
            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, Vector2.zero);

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(kuang.transform, false);
            bg.GetComponent<ImgUI>().setColor32(PageSetting.ListBgColor)
            .setSizePos(size, Vector2.zero);
            bg.AddComponent<UIOutline>().setSome(new Color32(185, 197, 180, 255), new Vector2(1, 1));

            GameObject bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("06_png")
            .setSizePos(new Vector2(50, 50), new Vector2(0, 0));

            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("07_png")
            .setSizePos(new Vector2(50, 50), new Vector2(size.x-50, 0));

            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("08_png")
            .setSizePos(new Vector2(50, 50), new Vector2(0, -(size.y-50)));

            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("09_png")
            .setSizePos(new Vector2(50, 50), new Vector2(size.x-50, -(size.y - 50)));

            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(this.transform, false);
            content.GetComponent<SimpleUI>()
                .setSizePos(size, Vector2.zero);

            return this;
        }
        public Transform getContent()
        {
            return this.transform.Find("content");
        }
        
    }
}
