using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    public class Pet : ModelMsg
    {
        public int lever = 1;
        //public string model;
        public int type;
        public string icon;
        //昵称
        public string nickName;
        //成8后的名字
        public string x8Name;
        //品质 (1-4品) 
        //决定设置的资质范围比例 （1品按设置的100%-90%取，2品为90%-80%取） 
        //决定技能悟性（1品按100的100%-90%取）
        public int quality;
        //忠诚度 0-1000每次战斗失败-100 ，达到0时不允许出战
        public int loyal;
        //出战等级
        public int fightLv;
        //技能悟性
        public int savvy;
        //资质 (设定的值，最大)
        public JObject zizhi;
        //成长率 确定具体多少资质。品质决定的上限*成长率=具体资质。 1200是满资质，最低1000，在这范围取一个数来获得比率
        public int grow;
        //技能列表 {key，lever，isOpen}
        //public skillKeyList;

        //因为宠物可以分配加点数的，所以需要保存
        public JObject baseProp;
        //具体资质
        public int qualityValue;
        //可分配属性点
        public int propPoint;
        //是否出战
        public int isFight;
        //成长度
        public int growValue;
        //成长等级
        public int growLv;
        //突破等级
        public int growBreachLv;
        //每日吃宠
        public int eatPet;
        public JObject attr;

        //动画对应的key,通过这个key去amManager中取动画信息
        public string pwdId;
        public string x8pwdId;
        public Pet(string key)
        {
            this.key = key;
            this.autoFightLv();
        }
        public string getNpcPicPath()
        {
            if (changguiPwds == null) return null;
            string str = "";
            foreach (string p in changguiPwds)
            {
                str += p + ",";
            }
            str += changguiAef;
            return str;
        }

        public Pet addPwdId(string pwdId)
        {
            this.pwdId = pwdId;
            this.x8pwdId = pwdId;
            return this;
        }
        public Pet addX8PwdId(string x8pwdId)
        {
            this.x8pwdId = x8pwdId;
            return this;
        }
        /**获取待机的pwd*/
        public string getIdlePwd(int growBreachLv)
        {
            if (growBreachLv == 2) return this.x8pwdId;
            return this.pwdId;
        }
        public string getIdlePwd(bool isX8)
        {
            if (isX8) return this.x8pwdId;
            return this.pwdId;
        }
        public Pet addFightLv(int fightLv)
        {
            this.fightLv = fightLv;
            return this;
        }
        /**
         * 自动计算出战等级
         */
        public Pet autoFightLv()
        {
            int index = int.Parse(this.key) - 1000;
            int n = (int)(index / 214f * 100f);
            if (n > 100) n = 100;
            else if (n < 1) n = 1;
            this.fightLv = n;
            return this;
        }
        public Pet init(string name,int type=0, string prefabCode = "651")
        {
            this.name = name;
            this.x8Name = name;
            this.type = type;
            //this.headIcon = headIcon + "_png";
            this.prefabCode = prefabCode;

            this.attr = new JObject();
            JObject prop = new JObject();
            prop.Add("xue", 0);
            prop.Add("lan", 0);
            prop.Add("exp", 0);
            this.attr.Add("prop", prop);
            this.attr.Add("skill", new JArray());
            this.zizhi = new JObject();
            this.propPoint = 0;
            this.isFight = 0;
            this.growValue = 0;
            this.growLv = 0;
            this.eatPet = 0;
            //this.autoAddQualityByK(1, 1);
            int index = int.Parse(key) - 1000;
            
             if (index == 157)
                this.bindQualityGroundStruct(2533, 1899, 2111, 1688, 2955, 2533, 1474, 1474, 2955, 1072);
            else if (index == 175)
                this.bindQualityGroundStruct(2426, 1985, 2867, 1985, 2646, 2205, 1260, 1540, 2426, 1960);
            else if (index == 176)
                this.bindQualityGroundStruct(2646, 2205, 3087, 1985, 2426, 1985, 1960, 1540, 2205, 1260);
            else if (index == 177)
                this.bindQualityGroundStruct(2205, 2205, 1985, 2867, 2205, 2646, 1960, 1260, 2646, 1400);
            else if (index == 178)
                this.bindQualityGroundStruct(2205, 2646, 1985, 3087, 2205, 2648, 1960, 1400, 2205, 1120);
            else if (index == 179)
                this.bindQualityGroundStruct(2867, 1985, 2646, 1985, 2426, 2205, 1400, 1960, 2426, 1400);
            else if (index == 183)
                this.bindQualityGroundStruct(2443, 2221, 2665, 1999, 2665, 2443, 1833, 1551, 2443, 1269);
            else if (index == 184)
                this.bindQualityGroundStruct(1999, 2443, 1999, 2887, 2443, 2887, 1833, 1551, 2221, 1269);
            else if (index == 196)
                this.bindQualityGroundStruct(2948, 2268, 2722, 2041, 2722, 2268, 2016, 1152, 2495, 1440);
            else if (index == 197)
                this.bindQualityGroundStruct(2495, 2268, 3175, 1814, 2722, 2268, 2016, 1152, 2495, 1584);
            else if (index == 201)
                this.bindQualityGroundStruct(2070, 2529, 2300, 2759, 2529, 2989, 1606, 1898, 2300, 1314);
            else if (index == 202)
                this.bindQualityGroundStruct(2529, 2070, 3219, 2300, 2300, 2759, 1606, 1314, 2300, 1898);
            else if (index == 203)
                this.bindQualityGroundStruct(2300, 2529, 2070, 2989, 2300, 2759, 2044, 1460, 2529, 1314);
            else if (index == 204)
                this.bindQualityGroundStruct(2547, 2084, 3010, 2315, 2778, 2315, 1617, 1470, 3010, 1470);
            else if (index == 208)
                this.bindQualityGroundStruct(2564, 2098, 3263, 2098, 2797, 2331, 1332, 1628, 2564, 1924);
            else if (index == 210)
                this.bindQualityGroundStruct(2347, 2816, 2112, 3285, 2347, 2816, 2086, 1490, 2347, 1192);
            else if (index == 211)
                this.bindQualityGroundStruct(2581, 2112, 3051, 2112, 2816, 2581, 2086, 1639, 2347, 1341);
            else if (index == 212)
                this.bindQualityGroundStruct(3071, 2363, 2599, 2126, 2835, 2599, 1200, 1800, 2835, 1650);
            else if (index == 213)
                this.bindQualityGroundStruct(2126, 2599, 2126, 3071, 2363, 2835, 2100, 1500, 2835, 1350);
            else if (index == 214)
                this.bindQualityGroundStruct(2599, 2126, 3071, 2126, 3071, 2599, 1650, 1500, 3308, 1200);
           


            else if (index == 215)
                this.bindQualityGroundStruct(2000, 2280, 2600, 1500, 2000, 1700, 1500, 1100, 1400, 1200);
            else if (index == 216)
                this.bindQualityGroundStruct(1500, 2780, 1570, 2450, 1750, 2100, 1100, 888, 2100, 1550);
            else if (index == 217)
                this.bindQualityGroundStruct(1400, 1600, 1900, 1150, 1530, 1270, 890, 810, 1020, 1130);
            else if (index == 218)
                this.bindQualityGroundStruct(1570, 2790, 1570, 2450, 1750, 2100, 1550, 999, 1750, 1220);
            else if (index == 219)
                this.bindQualityGroundStruct(1150, 2010, 1150, 1910, 1280, 1530, 890, 810, 1020, 1130);
            else if (index == 220)
                this.bindQualityGroundStruct(2460, 2300, 2100, 3200, 2200, 2800, 1600, 1840, 3000, 1880);
            else if (index == 221)//龙
                this.bindQualityGroundStruct(2887, 1999, 2221, 1777, 2887, 2665, 1551, 1269, 3331, 1269);
            else if (index == 222)//未输入
                this.bindQualityGroundStruct(2550, 2000, 3400, 2200, 3000, 2800, 2200, 1400, 3000, 1700);
            else if (index == 223)//未输入
                this.bindQualityGroundStruct(2800, 2300, 2220, 3500, 2800, 2700, 2200, 2000, 3200, 2300);
            else if (index == 224)
                this.bindQualityGroundStruct(2670, 2220, 2000, 3110, 2000, 2890, 1560, 1840, 2670, 1980);
            else if (index == 225)
                this.bindQualityGroundStruct(2450, 2000, 3110, 2000, 2900, 2000, 1980, 1410, 2450, 1560);
            else if (index == 226)//未输入
                this.bindQualityGroundStruct(3050, 2500, 2220, 3900, 2850, 3000, 2200, 1300, 3600, 2800);
            else if (index == 227)
                this.bindQualityGroundStruct(2990, 2220, 3910, 2220, 2180, 3310, 2330, 1410, 3780, 2160);
            else if (index == 228)
                this.bindQualityGroundStruct(2270, 2280, 1750, 1400, 2280, 2100, 1220, 1000, 2630, 1000);
            else
            {
                this.autoAddQualityByK(1f + index / 80f, 1f + index / 80f * 0.7f);
            }

            return this;
        }
        public Pet setX8Name(string x8Name)
        {
            this.x8Name = x8Name;
            return this;
        }
        /**获取种类名*/
        public string getTypeName(int growBreachLv)
        {
            if (growBreachLv == 2) return this.x8Name;
            return this.name;
        }

        /**获取成长率的范围*/
        public JObject getGrowGround(int quality)
        {
            JObject a = new JObject();
            if (quality == 1)
            {
                a.Add("max", 1200);
                a.Add("min", 1000);
            }
            else if (quality == 2)
            {
                a.Add("max", 1000);
                a.Add("min", 800);
            }
            else
            {
                a.Add("max", 800);
                a.Add("min", 600);
            }
            return a;
        }
        /**
         * 获取资质范围
         */
        private JObject getRealQualityGround(int quality)
        {
            JObject res = new JObject();
            IEnumerable<JProperty> ps = this.zizhi.Properties();
            foreach (JProperty p in ps)
            {
                string k = p.Name;
                int q = (int)p.Value;
                float min = q * (1 - quality * 0.2f);
                float max = q * (1 - (quality - 1) * 0.2f);
                JObject m = new JObject();
                m.Add("min", min);
                m.Add("max", max);
                res.Add(k, m);
            }
            return res;
        }
        public JObject getRealQualityGround(int quality, float grow, int growLv)
        {
            JObject groundMap = getRealQualityGround(quality);
            IEnumerable<JProperty> ps = groundMap.Properties();
            foreach (JProperty p in ps)
            {
                string k = p.Name;

                JObject limit = (JObject)p.Value;
                int max = (int)limit["max"];
                int real = (int)(max + (max / 10 * grow / 1000 * (0.8f + 0.1f * growLv)) * growLv / 2.5f);
                limit["max"] = real;
            }
            return groundMap;
        }
        /**由系数来生成资质数据
         k0影响血量、蓝
        k1影响攻击
         */
        public Pet autoAddQualityByK(float k0, float k1)
        {
            int wg, fg;
            if (this.type == 0)
            {
                wg = (int)(800 * k1);
                fg = wg / 2;
            }
            else
            {
                fg = (int)(800 * k1);
                wg = fg / 2;
            }


            this.bindQualityGroundStruct((int)(800 * k0), (int)(500 * k0), wg, fg,
                (int)(wg / 1.5), (int)(fg / 1.5), (int)(wg / 1.6), (int)(200 * k0), (int)(300 * k0), (int)(fg / 1.8));
            return this;
        }
        public void bindQualityGroundStruct(int max_xue, int max_lan, int wg, int fg, int wf, int ff, int mz, int sd, int css, int bj)
        {
            this.zizhi["max_xue"] = max_xue;
            this.zizhi["max_lan"] = max_lan;
            this.zizhi["wg"] = wg;
            this.zizhi["fg"] = fg;
            this.zizhi["wf"] = wf;
            this.zizhi["ff"] = ff;
            this.zizhi["mz"] = mz;
            this.zizhi["sd"] = sd;
            this.zizhi["css"] = css;
            this.zizhi["bj"] = bj;
        }
    }
}
