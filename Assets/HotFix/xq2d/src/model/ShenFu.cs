using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    /**神符*/
    class ShenFu : GoodsDes
    {
        private shenfuResultRule shenfuResultRule;
        public JArray cailiaoList;
        //对应的宠物key
        public string petKey;
        public ShenFu(string key)
        {
            this.key = key;
        }
        public ShenFu addPetKey(string petKey)
        {
            this.petKey = petKey;
            return this;
        }
        public ShenFu putshenfuResultRule(string name, float k, float b, int valueType)
        {
            if (this.shenfuResultRule == null || this.shenfuResultRule.attrs == null)
            {
                this.shenfuResultRule = new shenfuResultRule(new JArray());
            }
            JObject o = new JObject();
            o.Add("name", name);
            o.Add("k", k);
            o.Add("b", b);
            o.Add("valueType", valueType);//0固定值1原值的倍数
            JArray list = this.shenfuResultRule.attrs;
            list.Add(o);
            return this;
        }
        public JArray getLearnRuleAttrs()
        {
            if (this.shenfuResultRule != null)
                return this.shenfuResultRule.attrs;
            return null;
        }
        public float getLearnResultRuleK(JObject obj)
        {
            if (this.shenfuResultRule != null)
                return this.shenfuResultRule.count(1, obj);
            return 0;
        }
        public ShenFu putCaiLiaoList(string k, int num)
        {
            if (cailiaoList == null) cailiaoList = new JArray();
            JObject a = new JObject();
            a.Add("key", k);
            a.Add("num", num);
            cailiaoList.Add(a);
            return this;
        }
        public ShenFu getMsg()
        {
            if (this.key.Equals("10140000"))
            {
                this.init(14, "暗影妖狼之符", "26123").addQuality(0)
                    .addDes("封印了妖狼一族的力量的神符，可提升8%物攻、出手速，持续15分钟。\n可用暗影妖狼兑换\n制符要求：消耗10活力，槐树叶x2，羊毛笔x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.08f, 1).putshenfuResultRule("fg", 0, 0.08f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150002", 3);
                this.addPetKey("1145");
            }
            else if (this.key.Equals("10140001"))
            {
                this.init(14, "妖焰虫之符", "26123").addQuality(0)
                    .addDes("封印了虫族的力量的神符，可提升4%物攻、12%暴击，持续15分钟。\n可用妖焰虫兑换\n制符要求：消耗10活力，槐树叶x2，丹青x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.04f, 1).putshenfuResultRule("bj", 0, 0.12f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150001", 3);
                this.addPetKey("1187");
            }
            else if (this.key.Equals("10140002"))
            {
                this.init(14, "破军猿王之符", "26123").addQuality(0)
                    .addDes("封印了巨猿一族的力量的神符，可提升14%物攻、出手速，持续15分钟。\n可用破军猿王兑换\n制符要求：消耗10活力，巨熊皮x2，驼尾笔x4")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.14f, 1).putshenfuResultRule("css", 0, 0.14f, 1);
                this.putCaiLiaoList("10150006", 2).putCaiLiaoList("10150007", 4);
                this.addPetKey("1140");
            }
            else if (this.key.Equals("10140003"))
            {
                this.init(14, "天鹰玄兽之符", "26123").addQuality(0)
                    .addDes("封印了天鹰一族的力量的神符，可提升7%物攻、21%暴击，持续15分钟。\n可用天鹰玄兽兑换\n制符要求：消耗10活力，沁凉墨水x4，巨熊皮x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.07f, 1).putshenfuResultRule("bj", 0, 0.21f, 1);
                this.putCaiLiaoList("10150003", 4).putCaiLiaoList("10150006", 2);
                this.addPetKey("1190");
            }
            else if (this.key.Equals("10140004"))
            {
                this.init(14, "霸荒战狼之符", "26123").addQuality(0)
                    .addDes("封印了战狼一族的力量的神符，可提升20%物攻、20%出手速，持续15分钟。\n可用霸荒战狼兑换\n制符要求：消耗10活力，辛夷木x2，狼毫笔x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.2f, 1).putshenfuResultRule("css", 0, 0.2f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150009", 6);
                this.addPetKey("1176");
            }
            else if (this.key.Equals("10140005"))
            {
                this.init(14, "巨掌黑熊之符", "26123").addQuality(0)
                    .addDes("封印了黑熊一族的力量的神符，可提升10%物攻、30%暴击，持续15分钟。\n可用巨掌黑熊兑换\n制符要求：消耗10活力，辛夷木x2，古香墨水x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.1f, 1).putshenfuResultRule("bj", 0, 0.3f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150010", 6);
                this.addPetKey("1076");
            }
            else if (this.key.Equals("10140006"))
            {
                this.init(14, "镇川巨熊之符", "26123").addQuality(0)
                    .addDes("封印了巨熊一族的力量的神符，可提升26%物攻、出手速，持续15分钟。\n可用震川巨熊兑换\n制符要求：消耗10活力，紫毫笔x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.26f, 1).putshenfuResultRule("css", 0, 0.26f, 1);
                this.putCaiLiaoList("10150005", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1152");
            }
            else if (this.key.Equals("10140007"))
            {
                this.init(14, "龙蛟蛟之符", "26123").addQuality(0)
                    .addDes("封印了龙族的力量的神符，可提升13%物攻、39%暴击，持续15分钟。\n可用龙蛟蛟兑换\n制符要求：消耗10活力，朱砂x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.13f, 1).putshenfuResultRule("bj", 0, 0.39f, 1);
                this.putCaiLiaoList("10150004", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1034");
            }
            else if (this.key.Equals("10140008"))
            {
                this.init(14, "真阳火凤之符", "26123").addQuality(0)
                    .addDes("封印了火凤一族的力量的神符，可提升30%物攻、出手速，持续15分钟。\n可用真阳火凤兑换\n制符要求：消耗10活力，天然冻石x2，兰竹毛笔x12")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.3f, 1).putshenfuResultRule("css", 0, 0.3f, 1);
                this.putCaiLiaoList("10150012", 2).putCaiLiaoList("10150013", 12);
                this.addPetKey("1201");
            }
            //人皇
            else if (this.key.Equals("10140009"))
            {
                this.init(14, "双刀大盗之符", "26123").addQuality(0)
                    .addDes("封印了双刀一族的力量的神符，可提升4%生命、12%闪避，持续15分钟。\n可用双刀大盗兑换\n制符要求：消耗10活力，槐树叶x2，羊毛笔x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("max_xue", 0, 0.04f, 1).putshenfuResultRule("sd", 0, 0.12f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150002", 3);
                this.addPetKey("1059");
            }
            else if (this.key.Equals("10140010"))
            {
                this.init(14, "太阴斧魔之符", "26123").addQuality(0)
                    .addDes("封印了太阴一族的力量的神符，可提升8%防御、闪避，持续15分钟。\n可用太阴斧魔兑换\n制符要求：消耗10活力，槐树叶x2，丹青x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wf", 0, 0.08f, 1).putshenfuResultRule("ff", 0, 0.08f, 1).putshenfuResultRule("sd", 0, 0.08f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150001", 3);
                this.addPetKey("1204");
            }
            else if (this.key.Equals("10140011"))
            {
                this.init(14, "地魔统领之符", "26123").addQuality(0)
                    .addDes("封印了地魔一族的力量的神符，可提升7%生命、21%闪避，持续15分钟。\n可用地魔统领兑换\n制符要求：消耗10活力，巨熊皮x2，驼尾笔x4")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("max_xue", 0, 0.07f, 1).putshenfuResultRule("sd", 0, 0.21f, 1);
                this.putCaiLiaoList("10150006", 2).putCaiLiaoList("10150007", 4);
                this.addPetKey("1208");
            }
            else if (this.key.Equals("10140012"))
            {
                this.init(14, "开荒兽人之符", "26123").addQuality(0)
                    .addDes("封印了开荒一族的力量的神符，可提升14%防御、闪避，持续15分钟。\n可用开荒兽人兑换\n制符要求：消耗10活力，沁凉墨水x4，巨熊皮x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wf", 0, 0.14f, 1).putshenfuResultRule("ff", 0, 0.14f, 1).putshenfuResultRule("sd", 0, 0.14f, 1);
                this.putCaiLiaoList("10150003", 4).putCaiLiaoList("10150006", 2);
                this.addPetKey("1171");
            }
            else if (this.key.Equals("10140013"))
            {
                this.init(14, "兽神统领之符", "26123").addQuality(0)
                    .addDes("封印了兽神一族的力量的神符，可提升10%生命、30%闪避，持续15分钟。\n可用兽神统领兑换\n制符要求：消耗10活力，辛夷木x2，狼毫笔x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("max_xue", 0, 0.1f, 1).putshenfuResultRule("sd", 0, 0.3f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150009", 6);
                this.addPetKey("1212");
            }
            else if (this.key.Equals("10140014"))
            {
                this.init(14, "射日飞骑之符", "26123").addQuality(0)
                    .addDes("封印了射日一族的力量的神符，可提升20%防御、20%闪避，持续15分钟。\n可用射日飞骑兑换\n制符要求：消耗10活力，辛夷木x2，古香墨水x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wf", 0, 0.2f, 1).putshenfuResultRule("ff", 0, 0.2f, 1).putshenfuResultRule("sd", 0, 0.2f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150010", 6);
                this.addPetKey("1196");
            }
            else if (this.key.Equals("10140015"))
            {
                this.init(14, "裂地将军之符", "26123").addQuality(0)
                    .addDes("封印了裂地一族的力量的神符，可提升13%生命、39%闪避，持续15分钟。\n可用裂地将军兑换\n制符要求：消耗10活力，紫毫笔x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("max_xue", 0, 0.13f, 1).putshenfuResultRule("sd", 0, 0.39f, 1);
                this.putCaiLiaoList("10150005", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1158");
            }
            else if (this.key.Equals("10140016"))
            {
                this.init(14, "震天将军之符", "26123").addQuality(0)
                    .addDes("封印了震天一族的力量的神符，可提升26%防御、闪避，持续15分钟。\n可用震天将军兑换\n制符要求：消耗10活力，朱砂x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wf", 0, 0.26f, 1).putshenfuResultRule("ff", 0, 0.26f, 1).putshenfuResultRule("sd", 0, 0.26f, 1);
                this.putCaiLiaoList("10150004", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1167");
            }
            else if (this.key.Equals("10140017"))
            {
                this.init(14, "枯魂枪客之符", "26123").addQuality(0)
                    .addDes("封印了枪客一族的力量的神符，可提升15%生命、45%闪避，持续15分钟。\n可用枯魂枪客兑换\n制符要求：消耗10活力，天然冻石x2，兰竹毛笔x12")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("max_xue", 0, 0.15f, 1).putshenfuResultRule("sd", 0, 0.45f, 1);
                this.putCaiLiaoList("10150012", 2).putCaiLiaoList("10150013", 12);
                this.addPetKey("1179");
            }
            //仙魔
            else if (this.key.Equals("10140018"))
            {
                this.init(14, "古皇兽之符", "26123").addQuality(0)
                    .addDes("封印了野兽一族的力量的神符，可提升8%法攻、出手速，持续15分钟。\n可用古皇兽兑换\n制符要求：消耗10活力，槐树叶x2，羊毛笔x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.08f, 1).putshenfuResultRule("css", 0, 0.08f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150002", 3);
                this.addPetKey("1188");
            }
            else if (this.key.Equals("10140019"))
            {
                this.init(14, "陨星妖灵之符", "26123").addQuality(0)
                    .addDes("封印了妖灵一族的力量的神符，可提升4%法攻、12%暴击，持续15分钟。\n可用陨星妖灵兑换\n制符要求：消耗10活力，槐树叶x2，丹青x3")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.08f, 1).putshenfuResultRule("css", 0, 0.08f, 1);
                this.putCaiLiaoList("10150000", 2).putCaiLiaoList("10150001", 3);
                this.addPetKey("1173");
            }
            else if (this.key.Equals("10140020"))
            {
                this.init(14, "青莲竹妖之符", "26123").addQuality(0)
                    .addDes("封印了竹妖一族的力量的神符，可提升14%法攻、出手速，持续15分钟。\n可用青莲竹妖兑换\n制符要求：消耗10活力，巨熊皮x2，驼尾笔x4")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.08f, 1).putshenfuResultRule("css", 0, 0.08f, 1);
                this.putCaiLiaoList("10150006", 2).putCaiLiaoList("10150007", 4);
                this.addPetKey("1148");
            }
            else if (this.key.Equals("10140021"))
            {
                this.init(14, "玄幽魔匠之符", "26123").addQuality(0)
                    .addDes("封印了魔匠一族的力量的神符，可提升7%法攻、21%暴击，持续15分钟。\n可用玄幽魔匠兑换\n制符要求：消耗10活力，沁凉墨水x4，巨熊皮x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.07f, 1).putshenfuResultRule("bj", 0, 0.21f, 1);
                this.putCaiLiaoList("10150003", 4).putCaiLiaoList("10150006", 2);
                this.addPetKey("1161");
            }
            else if (this.key.Equals("10140022"))
            {
                this.init(14, "覆海藤之符", "26123").addQuality(0)
                    .addDes("封印了海藤一族的力量的神符，可提升20%法攻、出手速，持续15分钟。\n可用覆海藤兑换\n制符要求：消耗10活力，辛夷木x2，狼毫笔x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.2f, 1).putshenfuResultRule("css", 0, 0.2f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150009", 6);
                this.addPetKey("1185");
            }
            else if (this.key.Equals("10140023"))
            {
                this.init(14, "古炽灵之符", "26123").addQuality(0)
                    .addDes("封印了炽灵一族的力量的神符，可提升10%法攻、30%暴击，持续15分钟。\n可用古炽灵兑换\n制符要求：消耗10活力，辛夷木x2，古香墨水x6")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.1f, 1).putshenfuResultRule("bj", 0, 0.3f, 1);
                this.putCaiLiaoList("10150008", 2).putCaiLiaoList("10150009", 10);
                this.addPetKey("1063");
            }
            else if (this.key.Equals("10140024"))
            {
                this.init(14, "化阴魔尸之符", "26123").addQuality(0)
                    .addDes("封印了魔尸一族的力量的神符，可提升26%法攻、出手速，持续15分钟。\n可用化阴魔尸兑换\n制符要求：消耗10活力，紫毫笔x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.26f, 1).putshenfuResultRule("css", 0, 0.26f, 1);
                this.putCaiLiaoList("10150005", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1206");
            }
            else if (this.key.Equals("10140025"))
            {
                this.init(14, "枯煞木灵之符", "26123").addQuality(0)
                    .addDes("封印了木灵一族的力量的神符，可提升13%法攻、39暴击，持续15分钟。\n可用枯煞木灵兑换\n制符要求：消耗10活力，朱砂x8，墨玉x2")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.13f, 1).putshenfuResultRule("bj", 0, 0.39f, 1);
                this.putCaiLiaoList("10150004", 8).putCaiLiaoList("10150011", 2);
                this.addPetKey("1124");
            }
            else if (this.key.Equals("10140026"))
            {
                this.init(14, "玄影妖灵之符", "26123").addQuality(0)
                    .addDes("封印了妖灵一族的力量的神符，可提升30%法攻、出手速，持续15分钟。\n可用玄影妖灵兑换\n制符要求：消耗10活力，天然冻石x2，兰竹毛笔x12")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("fg", 0, 0.3f, 1).putshenfuResultRule("css", 0, 0.3f, 1);
                this.putCaiLiaoList("10150012", 2).putCaiLiaoList("10150013", 12);
                this.addPetKey("1142");
            }
            //天神
            else if (this.key.Equals("10140027"))
            {
                this.init(14, "九天玄女之符", "26123").addQuality(0)
                    .addDes("封印了玄女一族的力量的神符，可提升5%暴击、20%攻击，持续60分钟。\n可用九天玄女兑换\n")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("bj", 0, 0.05f, 1).putshenfuResultRule("wg", 0, 0.2f, 1).putshenfuResultRule("fg", 0, 0.2f, 1);
                this.addPetKey("1224");
            }
            else if (this.key.Equals("10140028"))
            {
                this.init(14, "龙翔天兵之符", "26123").addQuality(0)
                    .addDes("封印了天兵一族的力量的神符，可提升20%暴击、5%攻击，持续60分钟。\n可用龙翔天兵兑换\n")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("bj", 0, 0.2f, 1).putshenfuResultRule("wg", 0, 0.05f, 1).putshenfuResultRule("fg", 0, 0.05f, 1);
                this.addPetKey("1225");
            }
            else if (this.key.Equals("10140029"))
            {
                this.init(14, "逆天魔龙之符", "26123").addQuality(0)
                    .addDes("封印了魔龙一族的力量的神符，可提升10%暴击、命中、5%闪避，持续60分钟。\n可用逆天魔龙兑换\n")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("bj", 0, 0.1f, 1).putshenfuResultRule("mz", 0, 0.1f, 1).putshenfuResultRule("sd", 0, 0.05f, 1);
                this.addPetKey("1221");
            }
            else if (this.key.Equals("10140030"))
            {
                this.init(14, "惑天玄姬之符", "26123").addQuality(0)
                    .addDes("封印了玄姬一族的力量的神符，可提升5%暴击、命中、10%闪避，持续60分钟。\n可用惑天玄姬兑换\n")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("bj", 0, 0.05f, 1).putshenfuResultRule("mz", 0, 0.1f, 1).putshenfuResultRule("sd", 0, 0.1f, 1);
                this.addPetKey("1222");
            }
            else if (this.key.Equals("10140031"))
            {
                this.init(14, "玄晶天狐之符", "26123").addQuality(0)
                    .addDes("封印了天狐一族的力量的神符，全属性提升10%，持续60分钟。\n可用玄晶天狐兑换\n")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
                this.putshenfuResultRule("wg", 0, 0.1f, 1).putshenfuResultRule("fg", 0, 0.1f, 1)
                    .putshenfuResultRule("wf", 0, 0.1f, 1).putshenfuResultRule("ff", 0, 0.1f, 1)
                    .putshenfuResultRule("css", 0, 0.1f, 1).putshenfuResultRule("max_xue", 0, 0.1f, 1)
                    .putshenfuResultRule("max_lan", 0, 0.1f, 1).putshenfuResultRule("bj", 0, 0.1f, 1)
                    .putshenfuResultRule("mz", 0, 0.1f, 1).putshenfuResultRule("sd", 0, 0.1f, 1);
                this.addPetKey("1220");
            }


            return this;
        }

    }
    class shenfuResultRule
    {
        //属性名数组{name, k, b, valueType}
        public JArray attrs;
        public shenfuResultRule(JArray attrs)
        {
            this.attrs = attrs;
        }
        public JObject getResult(int lv)
        {
            //如杀气决，固定力量的基础数是36，耐力12，
            //每个技能的b、k是固定的，只有lv能改变
            JObject res = new JObject();
            foreach (object o in this.attrs)
            {
                JObject obj = (JObject)o;
                res.Add(obj["name"].ToString(), (float)obj["b"] + (float)obj["k"] * lv);
            }


            return res;
        }
        public float count(int lv, JObject obj)
        {
            return (float)obj["b"] + (float)obj["k"] * lv;
        }
        public int countNItemSum(int lv, JObject obj)
        {
            return (int)((((float)obj["b"] + (float)obj["k"] * 1) + ((float)obj["b"] + (float)obj["k"] * lv)) / 2f * lv);
        }
        public int countByK(int lv, JObject obj, JObject attr)
        {
            float k = (float)obj["k"] * lv + (float)obj["b"];
            float value = (float)attr[obj["name"].ToString()] * k;
            return (int)(value);
        }
    }
}
