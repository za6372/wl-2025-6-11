using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    class PageSetting
    {
        //字体标准大小
        public static int FontSize = 40;
        //字体颜色
        public static Color32 FontColor = new Color32(255, 255, 169, 255);

        //页面顶部颜色
        public static bool TopIsJb = true;//是否启用渐变
        public static Color32 TopColor1 = new Color32(1, 112, 114, 255);
        public static Color32 TopColor2 = new Color32(29, 89, 87, 255);//渐变的时候需要使用

        //tab颜色
        public static Color32 TabBgColor = new Color32(0, 0, 0, 255);
        public static Color32 TabItemColor = new Color32(0, 0, 0, 255);
        public static Color32 TabTextColor = new Color32(205, 205, 173, 255);
        public static Color32 TabItemMbColor = new Color32(93, 93, 93, 255);//描边色

        //页面底色
        public static Color32 PgColor = new Color32(187, 223, 206, 255);
        //黑边（介绍栏）
        public static Color32 HbColor = new Color32(33, 52, 66, 255);
        //列表背景色
        public static Color32 ListBgColor = new Color32(222, 231, 206, 255);
        //列表项选中框颜色
        public static Color32 ListItemColor = new Color32(89, 197, 181, 255);

        /**对页面设置的保存*/
        public static void savePage()
        {

        }
        /**保存滤镜的设置*/
        public static void saveLvJing(string ljName,Color color, float mdK)
        {
            JObject a = new JObject();
            a.Add("ljName", ljName);
            a.Add("r", color.r);
            a.Add("g", color.g);
            a.Add("b", color.b);
            a.Add("a", color.a);
            a.Add("mdK", mdK);
            dbHandle.save("page_lvjing", a);
        }
        public static JObject getLvJing()
        {
            JObject a = dbHandle.get<JObject>("page_lvjing");
            return a;
        }
    }
}
