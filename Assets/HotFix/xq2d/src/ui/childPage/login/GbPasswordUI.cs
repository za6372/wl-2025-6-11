using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.childPage.login
{
    class GbPasswordUI : PartUI
    {

        public static GbPasswordUI create(Transform parent)
        {
            Vector2 size = new Vector2(800, 600);
            GameObject one = gameObjPool.getInstance().get("GbPasswordUI", typeof(GbPasswordUI));
            GbPasswordUI bs = one.GetComponent<GbPasswordUI>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<GbPasswordUI>(parent).setLeftPos(Vector2.zero);
            bs.draw(size).toReg();

            DoGet.getInstance().startReqImg();
            return bs;
        }


        private void toReg()
        {
            Transform ts = this.transform.Find("content");
            gameObjPool.getInstance().freeChildren(ts.gameObject);

            GameObject username = gameObjPool.getInstance().get("username", typeof(InputUI));
            username.transform.SetParent(ts.transform, false);
            username.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75));
            username.GetComponent<InputUI>().initSetting("请输入账号").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            username.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject old = gameObjPool.getInstance().get("old", typeof(InputUI));
            old.transform.SetParent(ts.transform, false);
            old.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 1));
            old.GetComponent<InputUI>().initSetting("旧密码").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            old.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            GameObject password = gameObjPool.getInstance().get("password", typeof(InputUI));
            password.transform.SetParent(ts.transform, false);
            password.GetComponent<InputUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 2));
            password.GetComponent<InputUI>().initSetting("新密码").addMatchSimple().setContent("").setRoundedCorners(0.5f);
            password.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));

            
            GameObject lg = gameObjPool.getInstance().get("lg", typeof(BtnUI));
            lg.transform.SetParent(ts.transform, false);
            lg.GetComponent<BtnUI>().setSizePos(new Vector2(400, 60), new Vector2(190, -75 - 70 * 5));
            lg.GetComponent<BtnUI>().addText("立即更改", 30).setTextColor("#ffffff").setColor("#15725f").setRoundedCorners(0.5f, "#15725f")
            .addClk(() =>
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", username.GetComponent<InputUI>().getContent());
                dic.Add("old", old.GetComponent<InputUI>().getContent());
                dic.Add("password", password.GetComponent<InputUI>().getContent());
                DoGet.getInstance().sendPost("/manService/gbPassword", dic, (res) =>
                {
                    msgCode.showMsg(200);
                });
            });
            lg.AddComponent<UIOutline>().setSome(new Color32(4, 163, 130, 255), new Vector2(2, -2));


            DoGet.getInstance().startReqImg();
        }
        
        private GbPasswordUI draw(Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setAlpha(0)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero)
                .addClk(() => { this.free(); });

            Vector2 pos = new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f);

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, pos);

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
            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(this.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(pos.x + 20, pos.y - 20));

            return this;
        }
    }
}
