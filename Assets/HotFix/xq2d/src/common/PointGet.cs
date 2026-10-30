using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    /**节点获取*/
    class PointGet 
    {
        public static Transform getMapLayer()
        {
            return getC2dCanvas().GetChild(0).Find("mapLayer");
        }
        public static Transform getModelLayer()
        {
            return getC2dCanvas().GetChild(0).Find("modelLayer");
        }
        public static Transform getTouchLayer()
        {
            return getC2dCanvas().GetChild(0).Find("touchLayer");
        }
        public static Transform getPartsLayer()
        {
            return getC2dCanvas().GetChild(0).Find("partsLayer");
        }
        public static Transform getNowAcPage()
        {
            return getIndexPage().Find("Page").GetChild(0);
        }
        public static Transform getAcPage<T>()
        {
            Type t = typeof(T);
            Transform ts= getIndexPage();
            if (ts == null) return null;
            return ts.Find("Page/" + t.Name);
        }
        public static Transform getIndexPageOfPage()
        {
            return getIndexPage().Find("Page");
        }
        public static Transform getIndexPage()
        {
            return getC2dCanvas().Find("IndexPage");
        }
        public static Transform getFightPage()
        {
            return getC2dCanvas().Find("FightPage");
        }
        public static Transform getFightPageOfPage()
        {
            return getFightPage().Find("pageLayer");
        }
        public static Transform getTipCanvas()
        {
            Transform ts = getC2d().Find("tipCanvas");
            if (ts.Find("tipView") == null)
            {
                //需要一个组件来使提示框居中
                GameObject fc = gameObjPool.getInstance().get("tipView", typeof(SimpleUI));
                fc.transform.SetParent(ts, false);
                fc.GetComponent<SimpleUI>().setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).setLayerCenter();
            }
            return ts.Find("tipView");
        }
        //怎么去确定netUtils是在哪个活动页上？
        public static Transform getC2dCanvas()
        {
            return getC2d().Find("canvas");
        }
        public static Transform getC2dFilterCanvas()
        {
            return getC2d().Find("filterCanvas");
        }
        public static Transform getC2d()
        {
            return GameWindowHandle.getOne().getNowActivity().Find("camera2d");
        }
        public static Transform getControlPoint()
        {
            return getMapPoint().Find("control_player");
        }
        public static Transform getControlPetPoint()
        {
            return getControlPoint().Find("pet");
        }
        /**获取Map节点*/
        public static Transform getMapPoint()
        {
            return getC2dCanvas().GetChild(0).Find("Map");
        }
        /**地图底图节点（存放建筑、npc，方便地图整体移动）*/
        public static Transform getMapMapePoint()
        {
            return getMapPoint().Find("mape");
        }


    }
}
