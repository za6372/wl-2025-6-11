using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class CangBaoTu : GoodsDes
    {
        public int cbtType;
        public int cbtLv;
        public CangBaoTu(string key)
        {
            this.key = key;
        }
        public CangBaoTu getMsg()
        {
            //1012开头 0000开始
            //每级是4种品质，共15级，共计4*15=60
            string key = this.key.Substring(4);
            if (int.Parse(key) >= 60) return null;
            this.cbtLv = int.Parse(key) / 4 + 1;
            this.cbtType = int.Parse(key) % 4;
            string[] ns = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", };
            string[] ls = { "精致", "名贵", "珍稀", "绝世" };
            string name = ns[cbtLv - 1] + "星" + ls[cbtType] + "藏宝图";

            this.init(12, name, "");

            this.quality = cbtType;

            this.addDes("使用后将触发神奇的事件");
            this.addUseMsg("");

            return this;
        }
        /**通过品质跟星级来确定藏宝图的key*/
        public string getKey(int cbtType, int cbtLv)
        {
            //14 2 4
            int n = (cbtLv - 1) * 4 + cbtType;
            string str = n + "";
            if (n < 10) str = "000" + str;
            else if (n < 100) str = "00" + str;
            else if (n < 1000) str = "0" + str;

            return "1012" + str;
        }
    }
}
