using Assets.HotFix.MyUtils.src.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.InputField;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class InputUI :ImgUI
    {
        private string reg;

        public override void init()
        {
            this.gameObject.AddComponent<Image>();
            this.gameObject.AddComponent<InputField>();
            this.gameObject.layer = 5;
        }

        /**初始化设置
     * 注意：使用前需要先设置ui的大小
     * **/
        public InputUI initSetting(string placeholder, string textColor = "#333", int fontSize = 35, string plColor = "#D9D9D9", string align = "left")
        {
            InputField input = this.GetComponent<InputField>();
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;
            //内部文字小四边距5
            size.x -= 10;
            size.y -= 10;
            //todo:注意：当输入时会创建一个Input Caret（光标）对象，这个是不需要被对象池管理的
            if (!input.placeholder)
            {
                GameObject tx = gameObjPool.getInstance().get("placeholder", typeof(TextUI));
                tx.transform.SetParent(this.transform,false);
                tx.GetComponent<TextUI>().setText(placeholder).setFontSize(fontSize)
                    .setColor(plColor).setIsRichText(false).setAlign(align).setSize(size).setPos(Vector2.zero);
                input.placeholder = tx.GetComponent<Text>();
            }
            if (!input.textComponent)
            {
                GameObject tx = gameObjPool.getInstance().get("textComponent", typeof(TextUI));
                tx.transform.SetParent(this.transform, false);
                tx.GetComponent<TextUI>().setText("").setFontSize(fontSize)
                    .setColor(textColor).setIsRichText(false).setAlign(align).horiOut(HorizontalWrapMode.Wrap)
                    .setSize(size).setPos(Vector2.zero);
                input.textComponent = tx.GetComponent<Text>();
            }
            return this;
        }
        public InputUI setMulRow()
        {
            InputField input = this.GetComponent<InputField>();
            input.inputType = InputType.Standard;
            input.lineType = LineType.MultiLineNewline;
            return this;
        }
        public InputUI setIsInput(bool b)
        {
            InputField input = this.GetComponent<InputField>();
            input.enabled = b;
            return this;
        }

        public InputUI setNoBgColor()
        {
            Image img = this.GetComponent<Image>();
            img.color = new Color(1, 1, 1, 0);
            return this;
        }
        /**设置输入内容*/
        public InputUI setContent(string str)
        {
            InputField input = this.GetComponent<InputField>();
            input.text = str;
            input.textComponent.text = str;
            return this;
        }
        /**追加文本*/
        public InputUI appendContent(string str)
        {
            InputField input = this.GetComponent<InputField>();
            input.text = input.text + str;
            return this;
        }
        /**获取输入框内容*/
        public string getContent()
        {
            return this.GetComponent<InputField>().text;
        }
        /**基本匹配*/
        public InputUI addMatchSimple()
        {
            this.reg = @"[A-Za-z0-9]{6,12}$";
            return this;
        }
        /**邮件匹配*/
        public InputUI addMatchEmail()
        {
            this.reg = @"\w+@\w+(\.\w+)+$";
            return this;
        }
        /**中英文数字*/
        public InputUI addMatchChinese(int min = 1, int max = 6)
        {
            this.reg = @"[\u4e00-\u9fa5_a-zA-Z0-9_]{" + min + "," + max + "}$";
            return this;
        }
        public InputUI addMatchNum(int min = 1, int max = 9)
        {

            this.reg = @"[0-9]{" + min + "," + max + "}$";
            return this;
        }
        public InputUI addMatch(string reg = @"[A-Za-z0-9]*$")
        {
            this.reg = reg;
            return this;
        }
        /**是否通过匹配*/
        public bool isMatch()
        {
            string str = this.getContent();

            if (this.reg != null)
            {
                Regex regex = new Regex("^" + this.reg);
                if (!regex.IsMatch(str))
                {
                    return false;
                }
            }

            return true;
        }
        /**对文本的基本处理*/
        public InputUI addHandle(string defaultStr)
        {
            addChange((str) =>
            {
                if (!isMatch()) str = defaultStr;
                setContent(str);
            });
            return this;
        }
        /**文本改变事件*/
        public InputUI addChange(Action<string> callback)
        {
            this.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
            this.GetComponent<InputField>().onValueChanged.AddListener((e) =>
            {
                callback(this.getContent());
            });
            return this;
        }
        public InputUI addEndEdit(Action<string> callback)
        {
            this.GetComponent<InputField>().onEndEdit.RemoveAllListeners();
            this.GetComponent<InputField>().onEndEdit.AddListener((e) =>
            {
                callback(this.getContent());
            });
            return this;
        }
        
    }
}
