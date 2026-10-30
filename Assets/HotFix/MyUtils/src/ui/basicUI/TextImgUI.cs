using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
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
    /**文字图片（如倒计时、伤害等显示）*/
    public class TextImgUI : SimpleUI
    {
        private Sprite sprite;
        private string text;
        private bool isUpdate;

        private string rep;//"0123456789"
        private Vector2Int onePngSize;
        private int scaleH;
        List<GameObject> items = new List<GameObject>();
        public TextImgUI setBase(Vector2Int onePngSize, string rep, int scaleH)
        {
            this.rep = rep;
            this.onePngSize = onePngSize;
            this.scaleH = scaleH;
            return this;
        }
        public TextImgUI loadRes(string png)
        {
            //先获得图片
            GameObject img = gameObjPool.getInstance().get("img", typeof(ImgUI));
            img.GetComponent<ImgUI>().loadRes(png).addCallback((sp) =>
            {
                sprite = sp;
                SetGameObj.free(img);
            });
            DoGet.getInstance().startReqImg();
            return this;
        }
        public TextImgUI setText(string text)
        {
            this.text = text;
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            char[] arr = text.ToCharArray();
            int w = onePngSize.x;
            int h = onePngSize.y;
            float k = scaleH / h;
            h = scaleH;
            w = (int)(k * w);
            int len = items.Count;
            float x = (size.x - (w * arr.Length)) / 2;
            foreach(GameObject it in items)
            {
                it.SetActive(false);
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (i < len)
                {
                    items[i].SetActive(false);
                    items[i].GetComponent<ImgUI>()
                    .setSizePos(new Vector2(w, h), new Vector2(x + w * i, 0));
                }
                else
                {
                    GameObject img = gameObjPool.getInstance().get("i" + i, typeof(ImgUI));
                    img.transform.SetParent(this.transform, false);
                    img.GetComponent<ImgUI>()
                    .setSizePos(new Vector2(w, h), new Vector2(x + w * i, 0));
                    img.SetActive(false);
                    items.Add(img);
                }

            }
            isUpdate = true;
            return this;
        }
        /**每格有不同定义、像素时需要重写*/
        public void renderText()
        {
            //这里按11像素为一格
            int w = onePngSize.x;
            int h = onePngSize.y;
            char[] arr0 = rep.ToCharArray();
            char[] arr = text.ToCharArray();
            List<int> pos = new List<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr0.Length; j++)
                {
                    if (arr[i].ToString().Equals(arr0[j].ToString()))
                    {
                        pos.Add(j);
                        break;
                    }
                }
            }
            for (int i = 0; i < pos.Count; i++)
            {

                Sprite sp = Sprite.Create(sprite.texture, new Rect(sprite.rect.x+ pos[i] * w, sprite.rect.y + 0, w, h), new Vector2(0.5f, 0.5f));
                items[i].GetComponent<Image>().sprite = sp;
                items[i].SetActive(true);
            }

        }
        public void LateUpdate()
        {
            if (text != null && sprite != null && isUpdate)
            {
                isUpdate = false;
                renderText();
            }
        }
        public override void init()
        {
            this.gameObject.layer = 5;
        }
    }
}
