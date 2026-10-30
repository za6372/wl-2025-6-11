using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    class ZhongZi : GoodsDes
    {
        public ZhongZi(string key)
        {
            this.key = key;
        }
        public ZhongZi getMsg()
        {
            //1013
            int key = int.Parse(this.key.Substring(4));
            string[] names = { "玄铁矿", "炼神木", "锄头", "槐树叶", "银票", "丹青", "羊毛笔" };
            string[] lvs = { "初级", "中级", "高级" };
            int[] tm = { 3, 10, 24 };
            int[] prices = { 30, 90, 240 };
            int index = key / 3;
            int lv = key % 3;
            this.init(13, lvs[lv]+names[index]+"种子", "27621").addQuality(lv);
            this.addDes(names[index]+"留下来的种子，成长时长为"+tm[lv]+"小时");

            this.addUseMsg("");
            this.addBuyRule(0, 1);
            this.addBuyRule(11, prices[lv]);
            return this;
        }
    }
}
