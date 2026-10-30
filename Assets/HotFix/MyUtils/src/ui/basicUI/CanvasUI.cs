using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class CanvasUI : BaseUI
    {
        public CanvasUI init(RenderMode renderMode, int order = 0)
        {
            Canvas cs = this.gameObject.AddComponent<Canvas>();
            this.gameObject.AddComponent<CanvasScaler>();
            this.gameObject.AddComponent<GraphicRaycaster>();
            cs.renderMode = renderMode;
            cs.sortingOrder = order;
            this.gameObject.layer = 5;
            return this;
        }
        /**canvas中再创建一个canvas*/
        public CanvasUI init(bool overrideSorting, int order)
        {
            Canvas cs = this.gameObject.AddComponent<Canvas>();
            this.gameObject.AddComponent<CanvasScaler>();
            this.gameObject.AddComponent<GraphicRaycaster>();
            cs.pixelPerfect = false;
            cs.overrideSorting = overrideSorting;
            cs.sortingOrder = order;
            this.gameObject.layer = 5;
            return this;
        }

        public override void init()
        {

        }
    }
}
