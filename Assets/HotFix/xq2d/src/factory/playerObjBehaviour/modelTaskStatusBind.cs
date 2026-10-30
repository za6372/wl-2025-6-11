using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.factory.playerObjBehaviour
{
    class modelTaskStatusBind : MonoBehaviour
    {
        public void remTaskIcon()
        {
            Transform ts = this.transform.Find("taskIcon");
            if (ts != null)
                gameObjPool.getInstance().free(ts.gameObject);
        }
        public void addTaskIcon(int status)
        {
            string icon = null;
            Vector2 size = new Vector2(46, 13);
            //0未开启 1已开启（可接取） 2进行中 3可提交
            if (status == 1)
            {
                icon = "m1_png";
                size.y = 15;
            }
            else if (status == 2) icon = "m3_png";
            else if (status == 3) icon = "m2_png";

           

            if (this.transform.Find("taskIcon") == null)
            {
                GameObject qhBg = gameObjPool.getInstance().get("taskIcon", typeof(ImgUI));
                qhBg.transform.SetParent(this.transform, false);
                qhBg.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x * 4, size.y * 4);
                qhBg.transform.localPosition = new Vector2(0, this.transform.localScale.x == 1 ? 210 : 62);
                qhBg.transform.localScale = new Vector2(1f / this.transform.localScale.x, 1f / this.transform.localScale.y);
            }
            this.transform.Find("taskIcon").GetComponent<ImgUI>().loadRes(icon);
        }
    }
}
