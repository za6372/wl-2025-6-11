using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class TextTipUI : PartUI
    {
        public static TextTipUI create()
        {
            Vector2 pos = new Vector2(ScreenUtils.width / 4, -ScreenUtils.height / 2 + 46);
            GameObject one = gameObjPool.getInstance().get("TextTipUI", typeof(TextTipUI));
            TextTipUI bs = one.GetComponent<TextTipUI>();
            bs.setSize(new Vector2(ScreenUtils.width / 2, 46)).putSence<TextTipUI>(PointGet.getTipCanvas()).setLeftPos(pos);
            return bs;
        }
        public TextTipUI draw(string content)
        {
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(this.transform, false);
            bg.GetComponent<ImgUI>().setColor(0,0,0,0.8f)
                .setSizePos(new Vector2(ScreenUtils.width / 2, 46), new Vector2(0, 0));
            GameObject nameStr = gameObjPool.getInstance().get("text", typeof(TextUI));
            nameStr.transform.SetParent(bg.transform, false);
            nameStr.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setIsRichText().setText(content)
                .setSizePos(new Vector2(ScreenUtils.width / 2, 46), new Vector2(0, 0));
            DoGet.getInstance().startReqImg();

            timeManager.getTimeManageOne().putDelayTask(() => { this.free(); }, 3000);
            return this;
        }
        /**多组文字向上浮动*/
        public TextTipUI draw(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
                bg.transform.SetParent(this.transform, false);
                bg.GetComponent<ImgUI>().setColor(0, 0, 0, 0.8f)
                    .setSizePos(new Vector2(ScreenUtils.width / 2, 46), new Vector2(0, i * 56));
                GameObject nameStr = gameObjPool.getInstance().get("text", typeof(TextUI));
                nameStr.transform.SetParent(bg.transform, false);
                nameStr.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setIsRichText().setText(list[i])
                    .setSizePos(new Vector2(ScreenUtils.width / 2, 46), new Vector2(0, 0));
            }

            DoGet.getInstance().startReqImg();

            timeManager.getTimeManageOne().putDelayTask(() => { this.free(); }, 2000);
            return this;
        }
        /**显示任务完成*/

        public TextTipUI showTaskFinish()
        {
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(this.transform, false);
            bg.GetComponent<ImgUI>().loadRes("_LocRenWuWanCheng_png")
                .setSizePos(new Vector2(306, 90f), new Vector2(ScreenUtils.width / 4 - 306 / 2f, 290));
            DoGet.getInstance().startReqImg();
            timeManager.getTimeManageOne().putDelayTask(() => { this.free(); }, 2000);
            return this;
        }

    }
}
