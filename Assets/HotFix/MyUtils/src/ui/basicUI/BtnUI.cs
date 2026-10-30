using Assets.HotFix.MyUtils.src.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class BtnUI : ImgUI
    {
        /**添加文字*/
        public BtnUI addText(string str, int fontSize = 40, Vector2 sizeDelta = default, string color = "#804400", string align = "center")
        {
            if (this.transform.Find("tx1") == null)
            {
                RectTransform rc = this.GetComponent<RectTransform>();
                if (sizeDelta == default) sizeDelta = rc.sizeDelta;
                GameObject tx1 = gameObjPool.getInstance().get("tx1", typeof(TextUI));
                tx1.transform.SetParent(this.transform,false);
                tx1.GetComponent<TextUI>().setText(str).setAlign(align).setColor(color)
                    .setFontSize(fontSize).setIsRichText(true)
                .setSizePos(new Vector2(sizeDelta.x - 20, sizeDelta.y), new Vector2((rc.sizeDelta.x - sizeDelta.x) / 2 + 10, -(rc.sizeDelta.y - sizeDelta.y) / 2));
            }
            else
            {
                this.transform.Find("tx1").gameObject.GetComponent<Text>().text = str;
            }
            return this;
        }

        public string getTextStr()
        {
            return this.transform.Find("tx1").gameObject.GetComponent<Text>().text;
        }

        public BtnUI setTextColor(string color = "#ffffff")
        {
            if (this.transform.Find("tx1") == null) return this;
            this.transform.Find("tx1").GetComponent<TextUI>().setColor(color);
            return this;
        }
        public TextUI getTextUI()
        {
            return this.transform.Find("tx1").GetComponent<TextUI>();
        }

    }
}
