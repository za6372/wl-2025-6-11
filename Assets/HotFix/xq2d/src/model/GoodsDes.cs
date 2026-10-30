using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    /**描述类，物品、技能等的父类*/
    public class GoodsDes
    {
        public string Id;
        public string key;
        public string name;
        public string des;
        //使用提示
        public string useMsg;
        //类型 0无分类（8位）、1装备类（12位）、2技能类（12位）、3宝石类（8位）、
        //4碎片类（8位）、5药草类（8位）、6活动类（8位）、7任务道具类（8位）、8技能书（12位）
        public int type;
        //图片
        public string icon;
        //等级 仅展示，不是使用的条件
        public int lv = 1;
        //品质 白0蓝1紫2橙3金4红5
        public int quality;
        public int isBad;
        //物品保护 物品是否可拾取、交易、丢弃等规则
        public JArray buyList = new JArray();
        //是否允许直接使用
        public bool isAllowdUse;


        public GoodsDes init(int type, string name, string icon)
        {
            this.type = type;
            this.name = name;
            this.icon = icon + "_png";
            return this;
        }
        public GoodsDes addIsAllowdUse(bool isAllowdUse)
        {
            this.isAllowdUse = isAllowdUse;
            return this;
        }
        public GoodsDes addLv(int lv)
        {
            this.lv = lv;
            return this;
        }
        public GoodsDes addQuality(int quality)
        {
            this.quality = quality;
            return this;
        }
        public GoodsDes addDes(string des)
        {
            this.des = des;
            return this;
        }
        public GoodsDes addUseMsg(string useMsg)
        {
            this.useMsg = useMsg;
            return this;
        }
        /**添加交易规则
         moneyType 0元宝 1银两 2银票 3竞技积分 4武勋值 5帮贡 6修炼点 7帮币 8经脉点
         */
        public GoodsDes addBuyRule(int moneyType, int value)
        {
            JObject rule = new JObject();
            rule.Add("moneyType", moneyType);
            rule.Add("value", value);
            this.buyList.Add(rule);
            return this;
        }
        public int getPrice(int moneyType)
        {
            for (int i = 0; i < this.buyList.Count; i++)
            {
                if ((int)this.buyList[i]["moneyType"] == moneyType) return (int)this.buyList[i]["value"];
            }
            return 0;
        }
    }
}
