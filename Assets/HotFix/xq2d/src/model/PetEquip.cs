using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class PetEquip : GoodsDes
    {
        public petFixedAttrRule attr;
        //武器、防具、饰品
        public int part;
        //过期时间
        public long endTime;
        public PetEquip(string key)
        {
            this.key = key;
        }
        public string getDes(long endTime)
        {
            int index = int.Parse(this.key.Substring(4));
            string[] dess = {
                "耐力+88\n元灵幽草x10兑换","耐力+144\n元灵幽草x25兑换",
                "暴击+660 命中+330\n元灵幽花x12兑换","暴击+961 命中+481\n元灵幽花x20兑换",
            };
            string des = dess[index] + "\n剩余时间：" + strUtils.nowTimeToEndTime(strUtils.getMillis(), endTime);
            return des;
        }
        public PetEquip getMsg()
        {
            //1016
            int index = int.Parse(this.key.Substring(4));
            string[] names = { "苍穹仙衣", "紫霞仙衣", "宁心戒指", "勇气项链", };
            int[] lvs = { 40, 60, 60, 60, };
            int[] qua = { 1, 2, 1, 2, };
            int[] parts = { 1, 1, 2, 2 };
            this.part = parts[index];
            
            this.init(13, names[index], "27621").addQuality(qua[index]).addLv(lvs[index]);
            
            attr = new petFixedAttrRule();
            if (index == 0)
            {
                attr.nl = 88;
            }
            else if (index == 1)
            {
                attr.nl = 144;
            }
            else if (index == 2)
            {
                attr.bj = 660;
                attr.mz = 330;
            }
            else if (index == 3)
            {
                attr.bj = 961;
                attr.mz = 481;
            }
            this.addUseMsg("");

            return this;
        }
    }
}
