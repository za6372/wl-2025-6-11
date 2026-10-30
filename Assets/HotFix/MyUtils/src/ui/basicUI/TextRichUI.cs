using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    /**自定义富文本（代替TextMeshProUGUI，因为这玩意的字体文件太大）
     表情图片命名 face_1、face_2
     要求文字中不能包含标签，否则表情位置会出现问题
     */
    public class TextRichUI : SimpleUI, IPointerClickHandler
    {
        private string str;
        //表情的偏移量
        private Vector2 facePy;
        private List<RichTextImage> rtImageList = new List<RichTextImage>();
        private List<string> imageIds = new List<string>();
        private List<int> rIndexs = new List<int>();
        public override void init()
        {
            this.gameObject.AddComponent<Text>();
            this.gameObject.layer = 5;
        }
        /**设置可穿透ui进行点击*/
        public TextRichUI setNoClk()
        {
            this.GetComponent<Text>().raycastTarget = false;
            return this;
        }
        /**设置加粗*/
        public TextRichUI setFontStyle(FontStyle b = FontStyle.Bold)
        {
            this.GetComponent<Text>().fontStyle = b;
            return this;
        }
        /**要求宽是固定并且竖直方向允许文字超出*/
        public float getTextHeight()
        {
            Text text = this.GetComponent<Text>();
            return text.preferredHeight;
        }
        public float getTextWidth()
        {
            Text text = this.GetComponent<Text>();
            return text.preferredWidth;
        }
        public TextRichUI setFacePy(Vector2 py)
        {
            this.facePy = py;
            return this;
        }
        /**设置颜色*/
        public TextRichUI setColor(string color = "#ffffff")
        {
            Text text = this.GetComponent<Text>();
            text.color = strUtils.toRGBColor(color);
            return this;
        }
        public TextRichUI setTextValue(string content, int fontSize, JObject en)
        {
            if (strUtils.isNull(content)) content = "";


            if (content.Contains(" "))
            {
                //将空格换成不换行的空格
                content = content.Replace(" ", "\u00A0");
            }

            gameObjPool.getInstance().freeChildren(this.transform.gameObject);
            //清除之前创建的 
            this.rtImageList.Clear();
            imageIds.Clear();
            rIndexs.Clear();

            Text text = this.GetComponent<Text>();
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = fontSize;

            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            //使用正则来替换需要,假设图片id 1 - 99
            string regex = @"#[0-9]{2}";//"[#[0-9]{1}[0-3]{0,1}]";
            //有图片的地方需要用同字体大小的中文丶来替代，图片的大小是字体的大小
            string replaceStr = "<size=" + text.fontSize + ">圞</size>";
            //匹配的返回成一个数组
            MatchCollection matches = new Regex(regex).Matches(content);
            //Debug.Log(matches.Count);
            if (matches.Count > 0)
            {
                int realIndex = 0;
                int prevLen = 0;
                foreach (Match match in matches)
                {
                    this.imageIds.Add(match.Value);

                    //减去上个位置的长度
                    realIndex = match.Index - prevLen;
                    //累计长度
                    prevLen += match.Length - 1;

                    rIndexs.Add(realIndex);
                    //Debug.Log("匹配：" + realIndex + "=>" + match.Value+"=>"+match.Index);
                }
            }
            //获得替换后的文本
            this.str = Regex.Replace(content, regex, replaceStr);
            if (en != null) this.str += "<color=#FFF100>[<i>" + en["name"] + "</i>]</color>";
            text.text = this.str;
            this.calculatePosition();
            return this;
        }
        /**需要设置font和text才会调用这里*/
        /*protected override void OnPopulateMesh(VertexHelper toFill)
        {
            Debug.Log(1688);
            Debug.Log(this.str);
        }*/
        public void OnPointerClick(PointerEventData eventData)
        {

        }
        //计算位置
        private void calculatePosition()
        {
            


            //对文本的 linesArr (私有属性)进行访问，以计算出需要显示的图片的位置并设置
            Text text = this.GetComponent<Text>();
            RectTransform rectTransform = text.transform.GetComponent<RectTransform>();
            Vector2 extents = rectTransform.rect.size;
            TextGenerationSettings settings = text.GetGenerationSettings(extents);
            text.cachedTextGenerator.Populate(text.text, settings);
            Rect inputRect = rectTransform.rect;
            // 获取text的alignment anchor 
            Vector2 textAnchorPivot = Text.GetTextAnchorPivot(text.alignment);
            Vector2 refPoint = Vector2.zero;
            refPoint.x = Mathf.Lerp(inputRect.xMin, inputRect.xMax, textAnchorPivot.x);
            refPoint.y = Mathf.Lerp(inputRect.yMin, inputRect.yMax, textAnchorPivot.y);
            // 确定要偏移网格的像素。
            Vector2 roundingOffset = text.PixelAdjustPoint(refPoint) - refPoint;
            // 将偏移应用于顶点
            IList<UIVertex> verts = text.cachedTextGenerator.verts;
            float unitsPerPixel = 1 / text.pixelsPerUnit;
            //最后4节总是一条新的线
            int vertCount = verts.Count;
            //Debug.Log("roundingOffset:" + roundingOffset);
            //Debug.Log("vertCount:" + vertCount);

            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                UIVertex vx = verts[i];
                vx.position *= unitsPerPixel;
                vx.position.x += roundingOffset.x;
                vx.position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                {
                    //拿到解析后每个字符串的位置
                    list.Add(vx.position);
                    //Debug.Log(vx.position);
                }
            }
            //Debug.Log("位置数：" + list.Count);
            //遍历每个字符的位置
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 pos = list[i];
                //遍历表情所在的真实索引位置
                for (int j = 0; j < rIndexs.Count; j++)
                {
                    if (i == rIndexs[j])
                    {
                        //Debug.Log("表情：" + i);
                        //在该位置放置表情
                        rtImageList.Add(new RichTextImage(imageIds[j], pos.x + facePy.x, pos.y + facePy.y, text.fontSize, this.transform));
                    }
                }
            }



            /*this['linesArr'].forEach((item, lines) =>
            {
                let x = dx;
                y += item.height;
                for (let i = 0; i < item.elements.length; i++)
                {
                    let element: egret.IWTextElement = item.elements[i]
                            x += element.width;
                    if (element.text == '丶')
                    {
                        let idStr = imgIds[index++].match(/[0 - 9] +/);
                        this.rtImageList.push(new RichTextImage(this.parent, idStr[0], x - this.fontSize, y - this.fontSize, this.fontSize));
                    }
                }
            })*/
        }





    }
    class RichTextImage
    {
        private string id;
        private float x;
        private float y;
        private string source;
        private int size;
        private Transform parent;

        public RichTextImage(string id, float x, float y, int size, Transform parent)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.size = size;
            this.source = "face_" + int.Parse(id.Replace("#", "")) + "_png";
            this.parent = parent;
            this.LoadImage();
        }
        public void LoadImage()
        {
            //获取表情
            GameObject kuang = gameObjPool.getInstance().get("bq", typeof(ImgUI));
            kuang.transform.SetParent(parent, false);
            kuang.GetComponent<ImgUI>().loadRes(this.source)
                .setSize(new Vector2(this.size, this.size)).setPos(new Vector2(this.x + this.size / 2f, this.y + this.size / 2f));
        }
        
    }
}
