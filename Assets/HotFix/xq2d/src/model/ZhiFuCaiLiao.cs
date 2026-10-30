using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class ZhiFuCaiLiao : GoodsDes
    {
        public ZhiFuCaiLiao(string key)
        {
            this.key = key;
        }
        public ZhiFuCaiLiao getMsg()
        {
            //1015
            string[] arr =
            {
                "槐树叶","丹青","羊毛笔","沁凉墨水","朱砂",
                "紫毫笔","巨熊皮","驼尾笔","辛夷木","狼毫笔",
                "古香墨水","墨玉","天然冻石","兰竹毛笔",
            };
            int index = int.Parse(this.key.Substring(4));
            this.init(15, arr[index], "26123");
            this.addQuality(0)
                    .addDes("用于制作各种神符")
                    .addUseMsg("");
            this.addBuyRule(0, 100);
            return this;
        }
    }
}
