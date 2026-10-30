
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class TextUI : SimpleUI
    {
        private bool isFirst = true;
        public override void init()
        {
            this.gameObject.AddComponent<Text>();
            this.gameObject.layer = 5;
        }
        public string getText()
        {
            return this.GetComponent<Text>().text;
        }
        public TextUI setText(string str)
        {
            if (str.Contains(" "))
            {
                //将空格换成不换行的空格
                str = str.Replace(" ", "\u00A0");
            }
            Text text = this.GetComponent<Text>();
            if (isFirst)
            {
                isFirst = false;
                Action<AssetBundle> fn = (ab) =>
                {
                    Font f = null;
                    if (ab)
                    {
                        f = ab.LoadAsset<Font>("f1.ttf");
                        if (!f)
                        {
                            f = ab.LoadAsset<Font>("f2.ttf");
                        }
                    }

                    if (!f)
                    {
                        //加载默认字体
                        f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    }
                    text.font = f;
                    text.text = str;
                };

                AssetBundle ab = LoadResource.getFromAB("font");
                fn(ab);
            }
            else
            {
                text.text = str;
            }


            return this;
        }
        /**位图字体
         type 1橙色 2蓝色 3绿色 4红色
         */
        public TextUI setBmText(string str, string abName)
        {
            Text text = this.GetComponent<Text>();
            if (isFirst)
            {
                isFirst = false;

                DoGet.getInstance().loadAnyPrefab("font/" + abName, (ab) =>
                 {
                     Font f = null;
                     if (ab) f = ab.LoadAsset<Font>(abName + ".fontsettings");
                     text.font = f;
                     text.text = str;
                 });
            }
            else
            {
                text.text = str;
            }


            return this;
        }
        /**设置颜色*/
        public TextUI setColor(string color = "#ffffff")
        {
            Text text = this.GetComponent<Text>();
            text.color = strUtils.toRGBColor(color);
            return this;
        }
        public TextUI setColor32(Color32 color)
        {
            Text text = this.GetComponent<Text>();
            text.color = color;
            return this;
        }
        /**设置字体大小*/
        public TextUI setFontSize(int size = 30)
        {
            Text text = this.GetComponent<Text>();
            text.fontSize = size;
            return this;
        }
        /**水平溢出*/
        public TextUI horiOut(HorizontalWrapMode horizontalOverflow = HorizontalWrapMode.Overflow)
        {
            Text text = this.GetComponent<Text>();
            text.horizontalOverflow = horizontalOverflow;
            return this;
        }
        /**垂直溢出*/
        public TextUI VertiOut()
        {
            Text text = this.GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return this;
        }
        /**获取富文本的高度*/
        public float getRichHeight()
        {
            Text t = this.transform.GetComponent<Text>();
            RectTransform tRc = t.GetComponent<RectTransform>();
            // width保持不变
            tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tRc.sizeDelta.x);
            // 动态设置height
            tRc.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.preferredHeight);
            return t.preferredHeight;
        }
        /**设置是否为富文本*/
        public TextUI setIsRichText(bool b = true)
        {
            Text text = this.GetComponent<Text>();
            text.supportRichText = b;
            return this;
        }

        /**设置对齐*/
        public TextUI setAlign(string direction = "center")
        {
            Text img = this.GetComponent<Text>();
            switch (direction)
            {
                case "left": img.alignment = TextAnchor.MiddleLeft; break;
                case "right": img.alignment = TextAnchor.MiddleRight; break;
                case "center": img.alignment = TextAnchor.MiddleCenter; break;
                case "leftTop": img.alignment = TextAnchor.UpperLeft; break;
                case "rightBotton": img.alignment = TextAnchor.LowerRight; break;
            }
            return this;
        }
        /**设置可穿透ui进行点击*/
        public TextUI setNoClk()
        {
            this.GetComponent<Text>().raycastTarget = false;
            return this;
        }
        /**设置加粗*/
        public TextUI setFontStyle(FontStyle b = FontStyle.Bold)
        {
            this.GetComponent<Text>().fontStyle = b;
            return this;
        }

        /**设置行间距*/
        public TextUI setLineSpacing(float d = 1)
        {
            this.GetComponent<Text>().lineSpacing = d;
            return this;
        }
    }
}
