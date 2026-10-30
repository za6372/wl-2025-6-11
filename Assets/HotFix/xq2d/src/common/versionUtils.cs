using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.common
{
    class versionUtils
    {
        public static Dictionary<string, object> getVersionMsg()
        {
            List<string> list = new List<string>();
            list.Add("声明：\n" +
                "游戏内容为虚构！下载Q群：836305073 \n " +
                "谨防诈骗，私下买卖道具的注意先联系群主确认该玩家是否可靠，一旦检测到违规玩家的所有关联账号都将做封号处理，请自重~ \n");

            list.Add("2026.5.8 更新内容：\n 群buff的显示乱、回合开始时触发的buff有动画、 \n ");//斗宠、
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("apkVersion", "12.29.5");
            dic.Add("resVersion", "1.2");
            dic.Add("notice", list);
            return dic;
        }
        public static string getUpdateContent()
        {
            string str = "";
            List<string> list = (List<string>) getVersionMsg()["notice"];
            foreach(string s in list)
            {
                str += s;
            }
            return str;
        }
        //锻造 15 最多5孔 如果刻印最多6孔
        //升橙孔+1 6-7孔 锻18 +1 7-8孔 血契9孔

        //若+10就升橙（不刻印）4孔 12+1 5孔 15+1 6孔 18+1 7孔 刻印+1 8孔 血契+1 9孔
    }
}
