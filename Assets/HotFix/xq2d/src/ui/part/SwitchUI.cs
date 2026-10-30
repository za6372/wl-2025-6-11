using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class SwitchUI : PartUI
    {
        private bool isOpen;
        public static SwitchUI create(Vector2 pos, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("SwitchUI", typeof(SwitchUI));
            SwitchUI bs = one.GetComponent<SwitchUI>();
            bs.setSize(new Vector2(94, 41)).putSence<SwitchUI>(parent).setLeftPos(pos);
            bs.draw();
            return bs;
        }
        public SwitchUI draw()
        {
            //KaiGuan1
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(this.transform, false);
            bg.GetComponent<ImgUI>().setRoundedCorners(1, "#81B692")
                .setSizePos(new Vector2(140, 60), new Vector2(0, 0)).addClk(() =>
                {
                    setStatus(!this.isOpen);
                    ac(this.isOpen);
                });
            //bg.AddComponent<UIOutline>().setSome(new Color32(1, 1, 1, 1), new Vector2(2, -2));

            GameObject mark = gameObjPool.getInstance().get("mark", typeof(ImgUI));
            mark.transform.SetParent(bg.transform, false);
            mark.GetComponent<ImgUI>().setRoundedCorners(1, "#98CEB2")
                .setSizePos(new Vector2(60, 40), new Vector2(10, -10));

            GameObject nameStr = gameObjPool.getInstance().get("text", typeof(TextUI));
            nameStr.transform.SetParent(bg.transform, false);
            nameStr.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setIsRichText().setText("开")
                .setSizePos(new Vector2(60, 40), new Vector2(10, -10));

            return this;
        }
        public SwitchUI setStatus(bool isOpen)
        {
            this.isOpen = isOpen;
            if (isOpen)
            {
                //this.transform.Find("bg").GetComponent<ImgUI>().setRoundedCorners(1, "#98CEB2");
                this.transform.Find("bg/mark").GetComponent<ImgUI>()
                     .setSizePos(new Vector2(60, 40), new Vector2(70, -10));
                this.transform.Find("bg/text").GetComponent<TextUI>().setText("开")
                    .setSizePos(new Vector2(60, 40), new Vector2(10, -10));
            }
            else
            {
                //this.transform.Find("bg").GetComponent<ImgUI>().setRoundedCorners(1, "#98CEB2", 0);
                this.transform.Find("bg/mark").GetComponent<ImgUI>()
                     .setSizePos(new Vector2(60, 40), new Vector2(10, -10));
                this.transform.Find("bg/text").GetComponent<TextUI>().setText("关")
                    .setSizePos(new Vector2(60, 40), new Vector2(70, -10));

            }
            //DoGet.getInstance().startReqImg();

            return this;
        }
        private Action<bool> ac;
        public SwitchUI addCall(Action<bool> ac)
        {
            this.ac = ac;
            return this;
        }
    }
}
