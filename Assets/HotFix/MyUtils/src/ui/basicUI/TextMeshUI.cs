
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class TextMeshUI : SimpleUI
    {
        //表示第一次执行
        private bool isFirst = true;
        public override void init()
        {
            if (!this.gameObject.GetComponent<RectTransform>())
                this.gameObject.AddComponent<RectTransform>();
            this.gameObject.AddComponent<CanvasRenderer>();
            this.gameObject.AddComponent<TextMeshProUGUI>();
            //this.gameObject.AddComponent<TMP_SubMeshUI>();
        }
        
        private TextMeshUI setText(string str, Action ac)
        {
            TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
            if (isFirst)
            {
                isFirst = false;
                Action<AssetBundle> fn = (ab) =>
                {
                    if (ab != null)
                    {
                        TMP_FontAsset f = ab.LoadAsset<TMP_FontAsset>("f2 SDF.asset");
                        text.font = f;
                    }
                    else
                    {

                    }
;                    
                    text.text = str;
                    text.enableWordWrapping = true;
                    ac();
                };
                DoGet.getInstance().loadAnyPrefab("fontsys/f2", fn);
            }
            else
            {
                text.text = str;
                ac();
            }
            return this;
        }
        /**设置颜色*/
        public TextMeshUI setColor(string color = "#804400")
        {
            TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
            text.color = strUtils.toRGBColor(color);
            return this;
        }
        /**设置字体大小*/
        public TextMeshUI setFontSize(int size)
        {
            TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
            text.fontSize = size;
            return this;
        }
        /**设置对齐*/
        public TextMeshUI setAlign(string direction)
        {
            TextMeshProUGUI img = this.GetComponent<TextMeshProUGUI>();
            switch (direction)
            {
                case "left": img.alignment = TextAlignmentOptions.Left; break;
                case "right": img.alignment = TextAlignmentOptions.Right; break;
                case "center": img.alignment = TextAlignmentOptions.Center; break;
                case "leftTop": img.alignment = TextAlignmentOptions.TopLeft; break;
                case "leftCenter": img.alignment = TextAlignmentOptions.MidlineLeft; break;

            }
            return this;
        }
        /**设置间隔*/
        public TextMeshUI setSpace(float character, float line)
        {
            TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
            text.lineSpacing = line;
            text.characterSpacing = character;
            return this;
        }
        /**渲染表情*/
        public TextMeshUI renderFace(string str, Action ac)
        {
            //将#00...的替换成<sprite="Bq" index=0>
            for (int i = 0; i < 20; i++)
            {
                string a = "#" + i;
                if (i < 10) a = "#0" + i;
                string re = "<sprite=\"Bq\" index=" + i + ">";
                str = str.Replace(a, re);
            }
            this.setText(str, ac);
            return this;
        }
        public TextMeshUI renderFace2(string str, Action ac)
        {
            this.setText(str, ac);
            return this;
        }
    }
}
