using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    /**档位*/
    class GearUI : PartUI
    {
        public float value;
        public static GearUI create(string[] names, Vector2 pos, Transform parent)
        {
            GameObject one = gameObjPool.getInstance().get("GearUI", typeof(GearUI));
            GearUI bs = one.GetComponent<GearUI>();
            bs.setSize(new Vector2(114 * 3, 41)).putSence<GearUI>(parent).setLeftPos(pos);
            bs.draw(names);
            return bs;
        }
        public GearUI draw(string[] names)
        {
            GameObject jdBg = gameObjPool.getInstance().get("jdBg", typeof(ImgUI));
            jdBg.transform.SetParent(this.transform, false);
            jdBg.GetComponent<ImgUI>().setRoundedCorners(1, "#81B692")
                .setSizePos(new Vector2(200, 60), new Vector2(0, 0));

            GameObject kg = gameObjPool.getInstance().get("kg", typeof(ImgUI));
            kg.transform.SetParent(jdBg.transform, false);
            kg.GetComponent<ImgUI>().setRoundedCorners(1, "#98CEB2")
                .setSizePos(new Vector2(60, 40), new Vector2(10, -10));

            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(jdBg.transform, false);
            text.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setText(names[0])
                .setSizePos(new Vector2(60, 40), new Vector2(10, -10)).addClk(() => { this.clk(0); });
            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(jdBg.transform, false);
            text.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setText(names[1])
                .setSizePos(new Vector2(60, 40), new Vector2(10+60, -10)).addClk(() => { this.clk(1); });
            text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(jdBg.transform, false);
            text.GetComponent<TextUI>().setColor("#ffffff").setAlign().setFontSize(26).setText(names[2])
                .setSizePos(new Vector2(60, 40), new Vector2(10+60*2, -10)).addClk(() => { this.clk(2); });

            return this;
        }
        public GearUI choose(int i)
        {
            this.transform.Find("jdBg/kg").GetComponent<ImgUI>()
                .setSizePos(new Vector2(60, 40), new Vector2(10 + i * 60, -10));
            return this;
        }
        private void clk(int i)
        {
            choose(i);
            ac(i);
        }
        private Action<int> ac;
        public GearUI addCall(Action<int> ac)
        {
            this.ac = ac;
            return this;
        }
    }
}
