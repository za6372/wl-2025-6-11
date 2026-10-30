using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    public class MoBan : GoodsDes
    {
        public int partIndex;
        public int jobIndex;
        public int lvIndex;
        public string simpleName;
        public MoBan(string key)
        {
            this.key = key;
        }

        public MoBan getMsg()
        {
            //1010开头
            string key = this.key.Substring(4);
            int b = int.Parse(key);
            int part = 0;
            int job = 0;
            int lv = 0;
            //6职业，3等级魔板 9个部位 3*6*9=162种
            //武器 1部位6职业3等级 1*6*3
            //项链、戒指 1部位1职业3等级 1*1*3
            //其他 1部位3职业3等级 1*3*3
            if (b < 18)
            {
                part = 0;
                job = b % 6;
                lv = (b / 6) % 3;
            }
            else if (b < 18 + 3 * 2)
            {
                int index = (b - 18) / 3;
                part = 1 + index;
                job = 6;
                lv = ((b - (18 + 3 * index))) % 3;
            }
            else if (b < 18 + 3 * 2 + 9 * 6)
            {
                int index = (b - (18 + 3 * 2)) / 9;
                part = 3 + index;
                job = 7 + (b - (18 + 3 * 2 + 9 * index)) % 3;
                lv = ((b - (18 + 3 * 2 + 9 * index)) / 3) % 3;
            }
            //Debug.Log(part + "=>" + job + "=>" + lv + "=>" + this.key);
            string[] jobs = {"ms", "dj", "qm", "ty", "ym", "lc", "*",
                "ms/dj", "qm/ty", "ym/lc"};
            string[] arr0 = { "褪竭", "良品", "卓越" };
            string[] arr1 = { "破军", "巨门", "天机", "天相", "七杀", "廉贞", "太阳", "贪狼", "紫微", "太阴", };
            string[] wqStr = { "斧", "盾", "琴", "筝", "刃", "刺", "剑" };
            string[] elseStr = { "玉坠", "戒指", "玉镯", "头冠", "铠甲", "束腰", "护腿", "长靴" };
            int[] lvs = { 3, 6, 10 };
            name = "[" + GameAttrConst.getJobToSimpleName(jobs[job]).Substring(0,1) + "] " + arr0[lv] + arr1[job];
            if (part == 0) name += wqStr[job];
            else name += elseStr[part - 1];
            name += "模板";
            this.lv = lvs[lv];
            //Debug.Log(name);
            int quality = 3;
            this.partIndex = part;
            this.jobIndex = job;
            this.lvIndex = lv;
            this.init(10, name, "27542").addQuality(quality);
            this.des = this.setDes();
            this.simpleName = arr0[lv] + arr1[job] + (part == 0 ? wqStr[job] : elseStr[part - 1]);
            this.addBuyRule(0, 1);
            return this;
        }

        /**判断跟装备是否属于同一职业、部位*/
        public bool isAllowEquip(string epKey)
        {
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(epKey);
            if (ep.partIndex == this.partIndex && ep.jobIndex == this.jobIndex) return true;

            return false;
        }
        //todo:对每个魔板属性加成的描述，每个魔板对应的魔力上限

        //墨家10级 手链（生命1460，物攻328 魔力1590）帽子（法防534，生命860 魔力1590）腰带（生命860，物防534 魔力1590）护腿（生命1460，物攻328 魔力1590）
        //道家10级 武器（法攻1374 魔力1590） 项链（出手速451，物攻685，法攻685 魔力1590） 戒指（生命2846 魔力1590）手链（生命1460，魔法422 魔力1590） 帽子（法防410，生命1460 魔力1590）
        //衣服（生命4775 魔力1590）腰带（生命1460，物防410 魔力1590）护腿（生命1460，魔法422 魔力1590） 鞋子（出手速764，魔法284 魔力1590）
        //阴阳10级 手链（生命1840，物攻225 魔力1590）帽子（出手速218，法防584 魔力1590）

        //3、6、10
        //魔力有120、570、1130、1590

        private string setDes()
        {
            fightAttr attr = getMaxAttr();
            PropertyInfo[] properties = attr.GetType().GetProperties();
            string sb = "";
            foreach (PropertyInfo property in properties)
            {
                // Debug.Log ($"Property Name: {property.Name}, Property Value: {property.GetValue(attr)}");
                if (Convert.ToInt32(property.GetValue(attr)) == 0) continue;

                sb += GameAttrConst.propKeyToName(property.Name) + Convert.ToInt32(property.GetValue(attr)) + "、";
            }
            string str = "魔力等级上限" + lv + "级，魔力注满时增加属性：" + sb.Substring(0, sb.Length - 1);
            return str;
        }
        public int getMaxMoLi()
        {
            int ml = 570;
            if (lvIndex == 1) ml = 1130;
            else if (lvIndex == 2) ml = 1590;
            return ml;
        }
        public fightAttr getMaxAttr()
        {
            fightAttr attr = new fightAttr();
            if (partIndex == 0)//按职业分
            {
                if (jobIndex == 2 || jobIndex == 3) attr.fg = 1374;
                else attr.wg = 1374;
            }
            else if (partIndex == 1)//不区分职业、门派
            {
                attr.wg = 685; attr.fg = 685; attr.css = 451;
            }
            else if (partIndex == 2)//不区分职业、门派
            {
                attr.max_xue = 2846;
            }
            else if (partIndex == 3)//3及以上是按门派分
            {
                if (jobIndex == 7)
                {
                    attr.max_xue = 1460;
                    attr.wg = 328;
                }
                else if (jobIndex == 8)
                {
                    attr.max_xue = 1460;
                    attr.fg = 328;
                }
                else
                {
                    attr.max_xue = 1840;
                    attr.wg = 225;
                }
            }
            else if (partIndex == 4)
            {
                if (jobIndex ==7)
                {
                    attr.max_xue = 860;
                    attr.ff = 534;
                }
                else if (jobIndex == 8)
                {
                    attr.max_xue = 1460;
                    attr.ff = 410;
                }
                else
                {
                    attr.css = 218;
                    attr.ff = 584;
                }
            }
            else if (partIndex == 5)
            {
                attr.max_xue = 4775;
            }
            else if (partIndex == 6)
            {
                attr.max_xue = 860;
                attr.wf = 534;
                if (jobIndex == 8)
                {
                    attr.max_xue = 1460;
                    attr.wf = 410;
                }
            }
            else if (partIndex == 7)
            {
                attr.max_xue = 1460;
                attr.wf = 328;
                if (jobIndex == 8)
                {
                    attr.max_xue = 1460;
                    attr.max_lan = 422;
                }
            }
            else if (partIndex == 8)
            {
                attr.css = 764;
                attr.max_lan = 284;
            }
            float k = 0.3f;
            if (lvIndex == 1) k = 0.6f;
            else if (lvIndex == 2) k = 1f;
            PropertyInfo[] properties = attr.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                // Debug.Log($"Property Name: {property.Name}, Property Value: {property.GetValue(attr)}");
                float v = (float)property.GetValue(attr) * k;
                property.SetValue(attr, v);
            }
            return attr;
        }
    }
}
