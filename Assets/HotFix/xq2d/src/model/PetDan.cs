using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class PetDan : GoodsDes
    {
        //宝石类型（暴击、物攻。。。）
        public int bsType;
        public int prop;
        public PetDan(string key)
        {
            this.key = key;
        }

        public PetDan getMsg()
        {
            //1011开头
            string key = this.key.Substring(4);
            if (int.Parse(key) >= 30) return null;
            int lv = int.Parse(key.ToCharArray()[2] + "") + 1;
            int type = int.Parse(key.ToCharArray()[3] + "");
            this.lv = lv;
            this.bsType = type;
            string name = getPropName() + (lv < 2 ? "固元丹" : (lv < 3 ? "元魂丹" : "仙元丹"));

            this.init(11, name, "");
            this.quality = lv;
            this.prop = getPetDanAttr(type, lv);
            this.addDes("提升宠物" + prop + "点" + getPropName());
            this.addUseMsg("在【装备】-【镶嵌】界面使用");

            int price = lv < 2 ? 10 : (lv < 3 ? 25 : 50);
            this.addBuyRule(0, price);
            return this;
        }
        /**根据宝石的类型、等级获取其属性 */
        public int getPetDanAttr(int bsType, int lv)
        {
            int k = 0;
            int b = 0;
            switch (bsType)
            {
                //k*1+b=47  k*2+b=77  k=30 b=17  k+17=168
                case 0:
                    {//暴击 
                        k = 6;
                        break;
                    }
                case 1:
                    {//闪避
                        k = 3;
                        break;
                    }
                case 2:
                    {//命中
                        k = 5;
                        break;
                    }
                case 3:
                    {//mp
                        k = 67;
                        break;
                    }
                case 4:
                    {//hp
                        k = 67;
                        break;
                    }
                case 5:
                    {//法防
                        k = 4;
                        break;
                    }
                case 6:
                    {//物防
                        k = 4;
                        break;
                    }
                case 7:
                    {//法攻
                        k = 8;
                        break;
                    }
                case 8:
                    {//物攻
                        k = 8;
                        break;
                    }
                case 9:
                    {//速度
                        k = 2;
                        break;
                    }
            }
            return k * lv + b;
        }
        public string getPropName()
        {
            switch (this.bsType)
            {
                case 0: return "暴击";
                case 1: return "闪避";
                case 2: return "命中";
                case 3: return "魔法";
                case 4: return "生命";
                case 5: return "法防";
                case 6: return "物防";
                case 7: return "法攻";
                case 8: return "物攻";
                case 9: return "速度";
            }
            return null;
        }
        public string getAttrName()
        {
            switch (this.bsType)
            {
                case 0: return "bj";
                case 1: return "sd";
                case 2: return "mz";
                case 3: return "max_lan";
                case 4: return "max_xue";
                case 5: return "ff";
                case 6: return "wf";
                case 7: return "fg";
                case 8: return "wg";
                case 9: return "css";
            }
            return null;
        }
    }
}
