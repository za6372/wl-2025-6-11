using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    class Equip : GoodsDes
    {
        public string part;
        public string job;
        public int partIndex;
        public int jobIndex;
        //固定、随机属性规则
        public fixedAttrRule fixedAttrRule;
        public randomAttrRule randomAttrRule;
        public capacity capacity;
        public Equip(string key)
        {
            this.key = key;
        }
        public Equip addPartAndJob(string part, string job)
        {
            this.part = part;
            this.job = job;
            return this;
        }
        /**以一定格式输出装备名*/
        public string getPerfectName(JObject obj)
        {
            string name = null;
            
            if (obj.ContainsKey("czmb"))
            {
                //将名字变为模板名
                MoBan gd =(MoBan) face.goodsInterface.getGoodsMsgByKey(obj["czmb"]["key"].ToString());
                this.name = gd.simpleName;
            }
            if(int.Parse(this.key.Substring(4,4))>=1000&& int.Parse(this.key.Substring(4, 4)) < 1006)
            {
                name = "Lv" + this.lv + " " +
                    (obj.ContainsKey("xqbs") ? "[契]" : (obj.ContainsKey("kybs") ? "[刻]" : (obj.ContainsKey("isBind") &&(int)obj["isBind"] == 1 ? "[绑]" : ""))) +
                    ((int)obj["isBad"] == 1 ? "[损]" : "") +
                    " [" + GameAttrConst.getJobToSimpleName(this.job).Substring(0, 1) + "] " + this.name + " " +
                    (((int)obj["forging"]["lv"] > 0 ? "+" + obj["forging"]["lv"] : "") +
                    (obj.ContainsKey("inlay") && (int)obj["inlay"]["num"] > 0 ? "[" + obj["inlay"]["num"] + "]" : ""));
            }else if (int.Parse(this.key.Substring(4, 4)) == 1007)
            {
                name = "Lv" + this.lv + " " +
                    " [法宝] " + this.name + " " +
                    "[" + obj["zhuling"]["lv"] + "]阶" +
                    (obj.ContainsKey("inlay") && (int)obj["inlay"]["num"] > 0 ? "[" + obj["inlay"]["num"] + "]" : "");
            }
            else
            {
                name = "Lv" + this.lv + " " + this.name;
            }
            
            return name;
        }
        public string getSimpleName()
        {
            return "Lv" + this.lv  + " [" + GameAttrConst.getJobToSimpleName(this.job) + "] " + this.name;
        }
        public Equip getMsg()
        {
            string[] jobs = {"ms", "dj", "qm", "ty", "ym", "lc", "*",
                "ms/dj/zs", "qm/ty/fs", "ym/lc/fz"};
            string[] parts = { "wq", "jb", "sz", "wb", "tb", "xb", "yb", "tuib", "jiaob", "bjb",
            "fb", "hf", "gf" };
            int[] lvs = { 10, 8, 2, 6, 4, 10, 3, 7, 1 };
            string[] pics = {
                        "2036", "7000", "5000",
                        "2001", "4300", "1001",
                        "4310", "4313", "4001",
                        "22973","22973","22973","22973",
                    };
            //1001开头
            string equipKey = this.key.Substring(4);
            //9个部位 6职业 6*9=54件 5种散装 共5*54=270种 10个等级共2700种 1种套装54件10等级共540种
            //9部位6职业10等级5散装1套装=9*6*10*6=3240 * 6品质=19440种

            //排法：先按一套散装分成6份，一份中分10个等级，每级中分6个职业，每个职业中分9个部位
            int a = int.Parse(equipKey.Substring(0, 4));
            int b = int.Parse(equipKey.Substring(4));
            /*int part = b % 9;//分9份确定部位
            int job = (b / 9) % 6;//按9个为一组分，每组6个职业，确定所在职业
            int lv = (b / (9 * 6)) % 10 * 10 + lvs[part];//按54个为一组分，每组10个等级，确定所在等级
            int taoz = b / (10 * 6 * 9);//按540个为一组分，确定散装
            int quality = a - 1000;//白装0 蓝装1 紫装2 橙装3 金装4 神装5
*/
            int part = 0;
            int job = 0;
            int lv = 0;
            int taoz = 0;
            int quality = a - 1000;

            string name = null;
            if (a >= 1000 && a < 1006)
            {
                if (b < 360)
                {//wq
                    part = 0;
                    job = b % 6;//6个职业，确定所在职业
                    lv = (b / 6) % 10 * 10 + lvs[part];//6个职业，又每组10个等级，确定所在等级
                    taoz = b / (10 * 6);//确定散装
                }
                else if (b < 360 + 60 * 2)
                {//jb/sz
                    int index = (b - 360) / 60;
                    part = 1 + index;
                    job = 6;
                    lv = (b - 360 - 60 * index) % 10 * 10 + lvs[part];//10个等级，确定所在等级
                    taoz = (b - 360 - 60 * index) / 10;//确定散装
                }
                else if (b < 360 + 60 * 2 + 180 * 6)
                {//wb
                    int index = (b - (360 + 60 * 2)) / 180;
                    part = 3 + index;
                    job = 7 + (b - 360 - 60 * 2 - 180 * index) % 3;//相当于只有3个职业
                    lv = ((b - 360 - 60 * 2 - 180 * index) / 3) % 10 * 10 + lvs[part];//3职业10个等级，确定所在等级
                    taoz = (b - 360 - 60 * 2 - 180 * index) / (10 * 3);//确定散装
                }
                //装备
                name = getEquipName(a, b);
                //按职业命名
                string[] wqStr = { "斧", "盾", "琴", "筝", "刃", "刺", "剑" };
                string[] elseStr = { "玉坠", "戒指", "玉镯", "头冠", "铠甲", "束腰", "护腿", "长靴" };
                if (part == 0) name += wqStr[job];
                else name += elseStr[part - 1];
            }
            else if (a == 1006)
            {
                lv = 1;
                part = 9;
                job = 6;
                //药囊
                if (b == 0)
                {
                    quality = 0;
                    name = "小药囊";
                    this.addBuyRule(1, 100);//10000容量
                    this.addBuyRule(2, 500);//10000容量
                    this.addDes("拥有10000气血容量");
                }
                else if (b == 1)
                {
                    quality = 1;
                    name = "大药囊";
                    this.addBuyRule(1, 5000);//150000
                    this.addDes("拥有150000气血容量");
                }
                else if (b == 2)
                {
                    quality = 2;
                    name = "超级药囊";
                    this.addBuyRule(0, 100);//200000
                    this.addDes("拥有200000气血容量");
                    this.addBuyRule(0, 100);
                }
                else if (b == 3)
                {
                    quality = 3;
                    name = "无敌药囊";
                    this.addBuyRule(0, 1000);//3000000
                    this.addDes("拥有3000000气血容量");
                    this.addBuyRule(0, 1000);
                }
            }
            //法宝1007 护符1008 功法1009
            else if (a == 1007)
            {
                lv = 60;
                part = 10;
                job = 6;
                quality = 1;
                name = "翻天印";
                this.addBuyRule(0, 1);
                this.addDes("");
            }
            else if (a == 1008)
            {
                lv = 60;
                part = 11;
                job = 6;
                if(b<4) quality = 1;
                else if (b < 8) quality = 2;
                else if (b < 9) quality = 3;
                //按1400 声望100级的比例来算
                //白色有4种（分别对应四个抗性100）蓝色两种（前两跟后两300）紫色两种（前三后三，900上限）橙色1种（四抗 1400上限）
                string[] names = { "瓜皮", "蛇皮", "毛线", "屌丝", "子鼠", "寅牛", "青龙", "白虎", "麒麟" };
                name = names[b] + "护符";                
                this.addDes("护符发挥的抗性潜能与自身的声望等级有关");
                
            }
            else if (a == 1009)
            {

            }
            else return null;
            this.init(1, name, pics[part]).addLv(lv);
            this.part = parts[part];
            this.job = jobs[job];
            this.partIndex = part;
            this.jobIndex = job;
            this.quality = quality;
            this.isBad = 0;
            this.fixedAttrRule = new fixedAttrRule(this.key, this.part, this.lv, this.quality, this.job);
            this.randomAttrRule = new randomAttrRule(this.lv, this.quality);
            if (this.part.Equals("bjb"))
            {
                //根据品质分配
                int sum = 0;
                switch (this.quality)
                {
                    case 0:
                        {
                            sum = 10000;
                            break;
                        }
                    case 1:
                        {
                            sum = 150000;
                            break;
                        }
                    case 2:
                        {
                            sum = 200000;
                            break;
                        }
                    case 3:
                        {
                            sum = 3000000;
                            break;
                        }
                    case 4:
                        {
                            sum = 5000000;
                            break;
                        }
                    case 5:
                        {
                            sum = 10000000;
                            break;
                        }
                }
                this.capacity = new capacity(sum, sum);

            }
            return this;
        }
        /**获取护符属性*/
        public List<fbResult> getHuFuResult()
        {
            List<fbResult> list = new List<fbResult>();
            if (key.Equals("100110080000"))
            {
                list.Add(new fbResult("bjkx", 100f / 20, 0f, 20));
            }
            else if (key.Equals("100110080001"))
            {
                list.Add(new fbResult("hlkx", 100f / 20, 0f, 20));
            }
            else if (key.Equals("100110080002"))
            {
                list.Add(new fbResult("hskx", 100f / 20, 0f, 20));
            }
            else if (key.Equals("100110080003"))
            {
                list.Add(new fbResult("lxkx", 100f / 20, 0f, 20));
            }
            else if (key.Equals("100110080004"))
            {
                list.Add(new fbResult("bjkx", 300f / 40, 0f, 40));
                list.Add(new fbResult("hlkx", 300f / 40, 0f, 40));
            }
            else if (key.Equals("100110080005"))
            {
                list.Add(new fbResult("hskx", 300f / 40, 0f, 40));
                list.Add(new fbResult("lxkx", 300f / 40, 0f, 40));
            }
            else if (key.Equals("100110080006"))
            {
                list.Add(new fbResult("bjkx", 900f / 70, 0f, 70));
                list.Add(new fbResult("hlkx", 900f / 70, 0f, 70));
                list.Add(new fbResult("hskx", 900f / 70, 0f, 70));
            }
            else if (key.Equals("100110080007"))
            {
                list.Add(new fbResult("lxkx", 900f / 70, 0f, 70));
                list.Add(new fbResult("hlkx", 900f / 70, 0f, 70));
                list.Add(new fbResult("hskx", 900f / 70, 0f, 70));
            }
            else if (key.Equals("100110080008"))
            {
                list.Add(new fbResult("bjkx", 1400f / 100, 0f, 100));
                list.Add(new fbResult("lxkx", 1400f / 100, 0f, 100));
                list.Add(new fbResult("hlkx", 1400f / 100, 0f, 100));
                list.Add(new fbResult("hskx", 1400f / 100, 0f, 100));
            }
            return list;
        }
        /**
            a:key的前4位 品质
            b:key的后4位 9部位6职业10等级5散装1套装
         */
        private string getEquipName(int a, int b)
        {
            //两两组合
            int quanlity = a - 1000;
            int startCodePoint = '\u4e00';   // 起始Unicode码点
                                             //int endCodePoint = '\u9fa5';      // 结束Unicode码点
                                             //一共20901个汉字
            string str = (char)(startCodePoint + b * 2 + (quanlity + 1) * 100) + "" +
                (char)(startCodePoint + b + (1000 / (quanlity + 1)));

            //int part = b % 9;//分9份确定部位
            //int job = (b / 9) % 6;//按9个为一组分，每组6个职业，确定所在职业
            //int lv = (b / (9 * 6)) % 10;
            //int taoz = b / (10 * 6 * 9);

            //3239=>8/5/9/5
            //((int)(b/9))*9+part=b  b=part*9/8
            //return str +"=>"+b+"=>"+part+"/"+job+"/"+lv+"/"+taoz;

            int taoz = b / (10 * 6 * 9);
            if (taoz == 5) str = "[套]" + str;
            return str;
        }
    }
}
