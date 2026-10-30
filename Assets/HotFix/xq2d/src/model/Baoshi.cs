using Assets.HotFix.xq2d.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class Baoshi : GoodsDes
    {
        public string[] parts;
        //宝石类型（暴击、物攻。。。）
        public int bsType;
        public float prop;
        public Baoshi(string key)
        {
            this.key = key;
        }
        public Baoshi getMsg()
        {
            //1003开头 0000开始
            //1级0-9 二级10-19 三级20-29 四级30-39 五级40-49
            string key = this.key.Substring(4);
            if (int.Parse(key) >= 50) return null;
            int lv = int.Parse(key.ToCharArray()[2] + "") + 1;
            int type = int.Parse(key.ToCharArray()[3] + "");
            this.lv = lv;
            this.bsType = type;
            string name = lv + "级" + getPropName() + "镶嵌石";

            this.init(3, name, getIcon(type));

            this.quality = lv - 1;
            this.parts = getBaoshiPart(type);
            this.prop = getBaoshiAttr();
            string str = "";
            for(int i = 0; i < parts.Length; i++)
            {
                str+=GameAttrConst.equipPartKeyToName(parts[i])+"/";
            }
            this.addDes("可镶嵌在装备"+str.Substring(0,str.Length-1)+"上，提升装备"+ prop + "点" + getPropName() + "属性");
            this.addUseMsg("在【装备】-【镶嵌】界面使用");
            if (this.lv == 2)
            {
                this.addBuyRule(5, 350);
            }
            if (this.lv == 3)
            {
                this.addBuyRule(0, 150).addBuyRule(3, 225);
            }
            
            return this;
        }
        private string getIcon(int bsType)
        {
            //26239
            if (bsType == 0) return "26239";
            else if (bsType == 1) return "26249";
            else if (bsType == 2) return "26259";
            else if (bsType == 3) return "26269";
            else if (bsType == 4) return "26279";
            else if (bsType == 5) return "26289";
            else if (bsType == 6) return "26299";
            else if (bsType == 7) return "26351";
            else if (bsType == 8) return "26389";
            else if (bsType == 9) return "26462";

            return null;
        }
        /**根据宝石的类型获取其能镶嵌的部位 */
        public string[] getBaoshiPart(int bsType)
        {
            switch (bsType)
            {
                case 0: return new string[] { "wq", "sz", "jb" };//暴击
                case 1: return new string[] { "tb", "yb" };//闪避
                case 2: return new string[] { "tb", "wb", "sz" };//命中
                case 3: return new string[] { "jb", "jiaob" };//mp
                case 4: return new string[] { "xb", "tuib" };//hp
                case 5: return new string[] { "xb", "tuib", "yb" };//法防
                case 6: return new string[] { "xb", "tuib", "yb" };//物防
                case 7: return new string[] { "wq", "wb", "sz" };//法攻
                case 8: return new string[] { "wq", "wb", "sz" };//物攻
                case 9: return new string[] { "jiaob" };//速度
            }
            return null;
        }

        /**根据宝石的类型、等级获取其属性 */
        public float getBaoshiAttr()
        {
            float k = 0;
            float b = (float)Math.Pow(3, lv - 1);
            switch (this.bsType)
            {
                //k*1+b=47  k*2+b=77  k=30 b=17  k+17=168
                case 0:
                    {//暴击 
                        k = 40;
                        break;
                    }
                case 1:
                    {//闪避
                        k = 20;
                        break;
                    }
                case 2:
                    {//命中
                        k = 40;
                        break;
                    }
                case 3:
                    {//mp
                        k = 80;
                        break;
                    }
                case 4:
                    {//hp
                        k = 100;
                        break;
                    }
                case 5:
                    {//法防
                        k = 20;
                        break;
                    }
                case 6:
                    {//物防
                        k = 20;
                        break;
                    }
                case 7:
                    {//法攻
                        k = 24;
                        break;
                    }
                case 8:
                    {//物攻
                        k = 24;
                        break;
                    }
                case 9:
                    {//速度
                        k = 40;
                        break;
                    }
            }
            return k * (this.lv-1) + b;
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
