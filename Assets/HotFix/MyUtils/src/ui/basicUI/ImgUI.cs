using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class ImgUI : SimpleUI
    {

        public override void init()
        {
            this.gameObject.AddComponent<Image>();
            this.gameObject.layer = 5;

        }

        

        /**图片加载完成后回调*/
        public Action<Sprite> callback;
        public void call(Sprite sp)
        {
            if (callback != null)
            {
                callback(sp);
                return;
            }
            this.gameObject.GetComponent<Image>().sprite = sp;
        }
        /**注意先添加回调再loadRes*/
        public ImgUI addCallback(Action<Sprite> callback)
        {
            this.callback = callback;
            return this;
        }
        /**设置可穿透ui进行点击*/
        public ImgUI setNoClk()
        {
            this.GetComponent<Image>().raycastTarget = false;
            return this;
        }
        
        
        
        /**载入图片
         注意：rect x,y,w,h 原点在左下角，而不是右上角（截取时）
         切分拉伸时：左、上、右、下
         */
        public ImgUI loadRes(string path)
        {
            return loadRes(path, default, default, Image.Type.Simple);
        }
        public ImgUI loadRes(string path, Image.Type type)
        {
            return loadRes(path, default, default, type);
        }
        public ImgUI loadRes(string path, Rect rect)
        {
            return loadRes(path, rect, default, Image.Type.Simple);
        }
        public ImgUI loadRes(string path, Vector4 border)
        {
            return loadRes(path, default, border, Image.Type.Sliced);
        }

        private ImgUI loadRes(string path, Rect rect, Vector4 border, Image.Type type)
        {
            addMsg(path, rect, border);
            //动态加载贴图赋值给Image
            Image img = this.GetComponent<Image>();
            img.type = type;
            //先收集需要加载的信息
            DoGet.getInstance().collectionImgMsg(this, path);

            return this;
        }
        public string path;
        //显示纹理的矩形部分
        public Rect rect;
        //切分拉伸的部分
        public Vector4 border;
        public picCollection collection;
        public class picCollection
        {
            public string sheetName;
            public string sheetKey;
            public Vector4 rect;
        }
        private void addMsg(string path, Rect rect, Vector4 border)
        {
            this.path = path;
            this.rect = rect;
            this.border = border;
        }

        /**设置可见度*/
        public ImgUI setAlpha(float k)
        {
            return this.setColor(1, 1, 1, k);
        }
        /**设置颜色*/
        public ImgUI setColor(float r, float g, float b, float k)
        {
            this.GetComponent<Image>().color = new Color(r, g, b, k);
            return this;
        }
        public ImgUI setColor32(byte r, byte g, byte b, byte a)
        {
            this.GetComponent<Image>().color = new Color32(r, g, b, a);
            return this;
        }
        public ImgUI setColor32(Color32 color)
        {
            this.GetComponent<Image>().color = color;
            return this;
        }

        public ImgUI setColor(string color = "#ffffff")
        {
            this.GetComponent<Image>().color = strUtils.toRGBColor(color);
            return this;
        }
        
        

        public ImgUI setRoundedCorners(float k, string color = "#ffffff", float alphy = 1f)
        {
            SetGameObj.remComponent(this.GetComponent<Image>());
            RoundedCorners rd = this.gameObject.GetComponent<RoundedCorners>();
            if (rd == null)
            {
                rd = this.gameObject.AddComponent<RoundedCorners>();
            }
            
            rd.radius = k;
            Color c = strUtils.toRGBColor(color);
            c.a = alphy;
            rd.color = c;
            rd.SetVerticesDirty();
            return this;
        }
        public ImgUI setRoundedCorners(float k, Color32 color)
        {
            SetGameObj.remComponent(this.GetComponent<Image>());
            RoundedCorners rd = this.gameObject.GetComponent<RoundedCorners>();
            if (rd == null)
            {
                rd = this.gameObject.AddComponent<RoundedCorners>();
            }

            rd.radius = k;
            rd.color = color;
            rd.SetVerticesDirty();
            return this;
        }

    }
}
