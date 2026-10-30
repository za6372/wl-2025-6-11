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
    public class SliderUI : SimpleUI
    {
        private Action<float> ValueChangeCheck;
        public override void init()
        {
            this.gameObject.AddComponent<Slider>();
        }
        public SliderUI create()
        {
            GameObject fa = gameObjPool.getInstance().get("Fill Area", typeof(ImgUI));
            fa.transform.SetParent(this.transform, false);
            fa.GetComponent<ImgUI>().setColor(0, 0, 0, 0);
            fa.GetComponent<RectTransform>().anchorMin = new Vector2(0.25f, 0);
            fa.GetComponent<RectTransform>().anchorMax = new Vector2(0.75f, 1);
            fa.GetComponent<RectTransform>().offsetMax = new Vector2(0, -5);//top
            fa.GetComponent<RectTransform>().offsetMin = new Vector2(0, 15);//bottom

            GameObject fi = gameObjPool.getInstance().get("Fill", typeof(ImgUI));
            fi.transform.SetParent(fa.transform, false);
            fi.GetComponent<ImgUI>().setColor(0, 0, 0, 0);
            fi.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            fi.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            fi.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//top
            fi.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//bottom
            fi.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 10);

            GameObject hsa = gameObjPool.getInstance().get("Handle Slide Area", typeof(ImgUI));
            hsa.transform.SetParent(this.transform, false);
            hsa.GetComponent<ImgUI>().setColor(0, 0, 0, 0);
            hsa.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            hsa.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            hsa.GetComponent<RectTransform>().offsetMax = new Vector2(0, -10);//top
            hsa.GetComponent<RectTransform>().offsetMin = new Vector2(0, 10);//bottom

            GameObject hd = gameObjPool.getInstance().get("Handle", typeof(ImgUI));
            hd.transform.SetParent(hsa.transform, false);
            hd.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            hd.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            hd.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//top
            hd.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//bottom
            hd.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20);

            Slider sd = this.gameObject.GetComponent<Slider>();
            sd.direction = Slider.Direction.TopToBottom;
            sd.fillRect = fi.GetComponent<RectTransform>();
            sd.handleRect = hd.GetComponent<RectTransform>();

            sd.onValueChanged.AddListener((p)=> {
                if(this.ValueChangeCheck!=null)
                this.ValueChangeCheck(p);
            });
            return this;
        }
        public SliderUI addValueChangeCheck(Action<float> ac)
        {
            this.ValueChangeCheck = ac;
            return this;
        }
        public SliderUI setHor()
        {
            Slider sd = this.gameObject.GetComponent<Slider>();
            sd.direction = Slider.Direction.LeftToRight;
            Transform fa = this.transform.Find("Fill Area");
            Transform fi = this.transform.Find("Fill Area/Fill");
            Transform hsa = this.transform.Find("Handle Slide Area");
            Transform hd = this.transform.Find("Handle Slide Area/Handle");

            fa.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.25f);
            fa.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.75f);
            fa.GetComponent<RectTransform>().offsetMax = new Vector2(-5, 0);//top
            fa.GetComponent<RectTransform>().offsetMin = new Vector2(15, 0);//bottom

            fi.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            fi.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            fi.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//top
            fi.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//bottom
            fi.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 0);

            hsa.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            hsa.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            hsa.GetComponent<RectTransform>().offsetMax = new Vector2(-10, 0);//top
            hsa.GetComponent<RectTransform>().offsetMin = new Vector2(10, 0);//bottom

            hd.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            hd.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            hd.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//top
            hd.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//bottom
            hd.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 0);
            return this;
        }
        public SliderUI setBgColor(Color32 color)
        {
            Transform hsa = this.transform.Find("Handle Slide Area");
            hsa.GetComponent<Image>().color = color;
            return this;
        }
    }
}
