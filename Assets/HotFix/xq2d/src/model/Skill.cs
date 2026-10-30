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
    /**技能
     每次释放技能时回带上技能key，通过技能key找到相关的特效配置（按动画阶段分，起始点、打击点）
     */
    public class Skill : GoodsDes
    {
        //技能类型 0门派、1天技（4级）、2地技（4级）允许升级 3人技不允许升级 4宠物专属 5宠物普通 6经脉 7装备技能 8武将技能 9生活技能 决定槽位
        public int skillType;
        //0主动、1被动 ========>决定战斗中是否可以选取
        public int triggerType;
        //目标对象 0自己、1己方、2敌方、3除了自己以外的友方 决定可以选取的目标
        public int target;
        //目标数量 用于技能展示
        private targetNumRule targetNumRule;
        //职业 决定门派技能学习。宠物时为宠物的key
        public string job;
        //技能升级规则(升级所需道具数量)
        private leverRule leverRule;
        //技能学习规则（学习后加成属性）
        private learnResultRule learnResultRule;
        //发动的前提条件
        private triggerCondition triggerCondition;
        //冷却规则
        private coolingRule coolingRule;
        //技能点
        JArray skillItems;
        //由某个属性加持另一个属性
        aToBAttr aToBAttr;
        //技能绑定的am
        private amObj amObj;
        //悟性
        public int savvy = 80;
        //计算方式 0线性 1n项和
        public int countWay = 0;

        public int maxLv = 1;

        //技能特效配置可有可无，为null就不播放
        private List<AmMatchEffct> efList = new List<AmMatchEffct>();
        public Skill(string key)
        {
            this.key = key;
        }
        public Skill addSavvy(int savvy)
        {
            this.savvy = savvy;
            return this;
        }
        public Skill putStartEffect(string effectKey, float dy = 1)
        {
            efList.Add(new AmMatchEffct(effectKey, 0, 0, dy));
            return this;
        }
        public Skill putHitEffect(string effectKey, float dy = 0)
        {
            efList.Add(new AmMatchEffct(effectKey, 0, 1, dy));
            return this;
        }
        public Skill putMoveEffect(string startEffectKey, string endEffectKey)
        {
            if (startEffectKey != null) efList.Add(new AmMatchEffct(startEffectKey, 1, 0, 1));
            if (endEffectKey != null) efList.Add(new AmMatchEffct(endEffectKey, 1, 1, 1));
            return this;
        }
        public List<AmMatchEffct> getStartEffect()
        {
            return getEffect(0, 0);
        }
        public List<AmMatchEffct> getHitEffect()
        {
            return getEffect(0, 1);
        }
        public List<AmMatchEffct> getMoveStartEffect()
        {
            return getEffect(1, 0);
        }
        public List<AmMatchEffct> getMoveEndEffect()
        {
            return getEffect(1, 1);
        }
        private List<AmMatchEffct> getEffect(int type, int point)
        {
            List<AmMatchEffct> al = new List<AmMatchEffct>();
            for (int i = 0; i < efList.Count; i++)
            {
                if (efList[i].type == type && efList[i].point == point) al.Add(efList[i]);
            }
            return al;
        }
        /**
          skillType 技能类型 0门派、1天技（4级）、2地技（4级）允许升级 3人技不允许升级 4宠物专属 5宠物普通 6经脉 7装备技能 8武将技能 9生活技能 决定槽位
        triggerType 0主动、1被动 ========>决定战斗中是否可以选取
        target 目标对象 0自己、1己方、2敌方、3除了自己以外的友方 决定可以选取的目标
          */
        public Skill bindSkillData(int skillType, int triggerType, int target, string job)
        {
            this.skillType = skillType;
            this.triggerType = triggerType;
            this.target = target;
            this.job = job;
            return this;
        }
        /**注意：设置等级之后，拿到的数据才是此等级下对应的数据 */
        public void bindLv(int lv)
        {
            this.lv = lv;
        }

        public void bindTargetNumRule(float k, float b)
        {
            this.targetNumRule = new targetNumRule(k, b);
        }
        /**设置计算方式*/
        public Skill putCountWay(int countWay)
        {
            this.countWay = countWay;
            return this;
        }
        public Skill putLearnResultRule(string name, float k, float b, int valueType)
        {
            if (this.learnResultRule == null || this.learnResultRule.attrs == null)
            {
                this.learnResultRule = new learnResultRule(new JArray());
            }
            JObject o = new JObject();
            o.Add("name", name);
            o.Add("k", k);
            o.Add("b", b);
            o.Add("valueType", valueType);//0固定值1原值的倍数
            JArray list = this.learnResultRule.attrs;
            list.Add(o);
            return this;
        }
        public JObject getLearnResult(int lv = 1)
        {
            if (this.learnResultRule != null)
                return this.learnResultRule.getResult(lv);
            return null;
        }
        public JArray getLearnRuleAttrs()
        {
            if (this.learnResultRule != null)
                return this.learnResultRule.attrs;
            return null;
        }
        public float getLearnResultRuleCount(int lv, JObject obj)
        {
            if (this.learnResultRule != null)
            {
                if (this.countWay == 0) return this.learnResultRule.count(lv, obj);
                else if (this.countWay == 1) return this.learnResultRule.countNItemSum(lv, obj);
            }

            return 0;
        }
        public float getLearnResultRuleCountByK(int lv, JObject obj, JObject attr)
        {
            if (this.learnResultRule != null)
                return this.learnResultRule.countByK(lv, obj, attr);
            return 0;
        }
        public float getLearnResultRuleK(int lv, JObject obj)
        {
            if (this.learnResultRule != null)
                return this.learnResultRule.count(lv, obj);
            return 0;
        }
        //hp、mp
        public void bindTriggerRule(float k0, float b0, float k1, float b1)
        {
            this.triggerCondition = new triggerCondition(k0, b0, k1, b1);
        }
        public Skill bindLeverRule(int maxLv)
        {
            this.leverRule = new leverRule(maxLv);
            return this;
        }
        public Skill addMaxLv(int max)
        {
            this.maxLv = max;
            return this;
        }
        public bool isOverLimitLv(int lv)
        {
            //if (lv >= this.leverRule.maxLv) return true;
            if (lv >= this.maxLv) return true;
            return false;
        }
        public Skill bindExpRule(float k, float b)
        {
            this.leverRule.bindExpRule(k, b);
            return this;
        }
        public Skill bindYpRule(float k, float b)
        {
            this.leverRule.bindYpRule(k, b);
            return this;
        }
        public Skill bindLvRule(float k, float b)
        {
            this.leverRule.bindYpRule(k, b);
            return this;
        }

        public Skill bindCoolingRule(float k, float b)
        {
            this.coolingRule = new coolingRule(k, b);
            return this;
        }
        public void bindAToBAttrRule(string aKey, float k, string bKey)
        {
            this.aToBAttr = new aToBAttr(aKey, k, bKey);

        }
        public bool aToBAttrIsNull()
        {
            if (this.aToBAttr == null) return true;
            return false;
        }
        public bool aToBAttrBKeyIsEquals(string k)
        {
            if (this.aToBAttr.bKey.Equals(k)) return true;
            return false;
        }
        public float countAToBAttr(JObject attr)
        {
            return (float)attr[this.aToBAttr.aKey] * this.aToBAttr.k;
        }
        public string getAToBAttrKey()
        {
            return this.aToBAttr.bKey;
        }
        public Skill getMsg()
        {
            //"1002([0-9]{8})"
            string key = this.key.Substring(4);
            //1000开头为职业技能 1001为宠物技能 1002为经脉技能 1003为仙绝技能 1004为伙伴技能 1005为怪物技能 1006为生活技能
            //技能文件所在位置
            //E:\wanlingshanhai\NirvanaClient1\Assets\Game\Lua\config\auto\single_skill_auto.lua
            //在里面搜索技能名称，找到asset_id_2对应的配置文件名，将文件名复制到unity搜索栏找到配置文件，查看绑定的特效
            if (key.Substring(0, 4).Equals("1000"))
            {
                this.initJobSkill(key);
            }
            //宠物技能
            else if (key.Substring(0, 4).Equals("1001"))
            {
                this.initPetSkill(key);
            }
            else if (key.Substring(0, 4).Equals("1002"))
            {
                this.initJingMaiSkill(key);
            }
            else if (key.Substring(0, 4).Equals("1003"))
            {
                this.initXianJueSkill(key);
            }
            else if (key.Substring(0, 4).Equals("1004"))
            {
                this.initHuoBanSkill(key);
            }
            else if (key.Substring(0, 4).Equals("1006"))
            {
                this.initLifeSkill(key);
            }

            //this.addDes(this.getSkillDes(1));

            return this;
        }


        private void initHuoBanSkill(string key)
        {

            if (key.Equals("10040000"))//默认打击特效
            {
                this.init(2, "", "");
                this.putHitEffect("10045525");
            }




        }
        private void initLifeSkill(string key)
        {
            //30, 10, 5, 5, 6, 6, 30, 23, 12, 5
            //"玄黄诀","冲虚诀","武宗诀","紫微诀","金刚诀","凝神诀","鹰眼诀","飘渺诀","破杀诀","绝尘诀",

            if (key.Equals("10060000"))
            {
                this.init(2, "玄黄诀", "s7122");
                this.addDes("增加30点生命");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("max_xue", 30, 0, 0);
            }
            else if (key.Equals("10060001"))
            {
                this.init(2, "冲虚诀", "s7122");
                this.addDes("增加10点法力");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("max_lan", 10, 0, 0);
            }
            else if (key.Equals("10060002"))
            {
                this.init(2, "武宗诀", "s7122");
                this.addDes("增加5点物攻");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("wg", 5, 0, 0);
            }
            else if (key.Equals("10060003"))
            {
                this.init(2, "紫微诀", "s7122");
                this.addDes("增加5点法攻");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("fg", 5, 0, 0);
            }
            else if (key.Equals("10060004"))
            {
                this.init(2, "金刚诀", "s7122");
                this.addDes("增加6点物防");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("wf", 6, 0, 0);
            }
            else if (key.Equals("10060005"))
            {
                this.init(2, "凝神诀", "s7122");
                this.addDes("增加6点法防");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("ff", 6, 0, 0);
            }
            else if (key.Equals("10060006"))
            {
                this.init(2, "鹰眼诀", "s7122");
                this.addDes("增加30点法命中");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("mz", 30, 0, 0);
            }
            else if (key.Equals("10060007"))
            {
                this.init(2, "飘渺诀", "s7122");
                this.addDes("增加23点闪避");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("sd", 23, 0f, 0);
            }
            else if (key.Equals("10060008"))
            {
                this.init(2, "破杀诀", "s7122");
                this.addDes("增加12点暴击");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("bj", 12, 0f, 0);
            }
            else if (key.Equals("10060009"))
            {
                this.init(2, "绝尘诀", "s7122");
                this.addDes("增加5点出手速");
                this.bindSkillData(9, 1, 0, "*").putLearnResultRule("css", 5, 0f, 0);
            }
            //"养生诀","疾风步","人品爆发","宠物密语","捆仙诀","制符","烹饪","炼药",
            else if (key.Equals("10060010"))
            {
                this.init(2, "养生诀", "s7122");
                this.addDes("增加5点活力");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060011"))
            {
                this.init(2, "疾风步", "s7122");
                this.addDes("增加1%逃跑成功率");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060012"))
            {
                this.init(2, "人品爆发", "s7122");
                this.addDes("增加4%变异宠物出现率");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060013"))
            {
                this.init(2, "宠物密语", "s7122");
                this.addDes("增加1%捕捉成功率");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060014"))
            {
                this.init(2, "捆仙诀", "s7122");
                this.addDes("减少对方1%逃跑成功率");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060015"))
            {
                this.init(2, "制符", "s7122");
                this.addDes("可炼制当前等级以内的符");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060016"))
            {
                this.init(2, "烹饪", "s7122");
                this.addDes("可烹饪当前等级以内的食物");
                this.bindSkillData(9, 1, 0, "*");
            }
            else if (key.Equals("10060017"))
            {
                this.init(2, "炼药", "s7122");
                this.addDes("可制作当前等级以内的丹药");
                this.bindSkillData(9, 1, 0, "*");
            }
        }
        private void initXianJueSkill(string key)
        {

            //===========寻秦部分=================
            if (key.Equals("10030027"))
            {
                this.init(2, "凝神静息", "s7122");//天技 4级效果
                this.addDes("受到的治疗效果提升%s%%，同时命中上升15%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.15f, 1);
                this.addLineRule(0.06f, 0);
            }
            else if (key.Equals("10030028"))
            {
                this.init(2, "绝地反击", "s7122");//地技 4级效果
                this.addDes("生命低于50%时，雷霆震击的反击概率提高%s%%，同时出手速增加15%");
                this.bindSkillData(2, 1, 0, "*").putLearnResultRule("css", 0, 0.15f, 1);
                this.addLineRule(0.03f, 0);
            }
            else if (key.Equals("10030029"))
            {
                this.init(2, "如风迅击", "s7122");//人技
                this.addDes("攻击增加13%，同时暴击下降6%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.13f, 1).putLearnResultRule("fg", 0, 0.13f, 1).putLearnResultRule("bj", 0, -0.06f, 1);
            }
            else if (key.Equals("10030030"))
            {
                this.init(2, "无极淬体", "s7122");//人技
                this.addDes("生命提高10%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1);
            }
            else if (key.Equals("10030031"))
            {
                this.init(2, "八玄锻脉", "s7122");//人技
                this.addDes("生命提高13%，暴击降低5%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.13f, 1).putLearnResultRule("bj", 0, -0.05f, 1);
            }
            else if (key.Equals("10030032"))
            {
                this.init(2, "金身硬化", "s7122");//人技
                this.addDes("生命提高8%，命中提升20%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.08f, 1).putLearnResultRule("mz", 0, 0.2f, 1);
            }
            else if (key.Equals("10030033"))
            {
                this.init(2, "集思凝力", "s7122");//人技
                this.addDes("人物攻击提升10%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1).putLearnResultRule("fg", 0, 0.1f, 1);
            }
            else if (key.Equals("10030034"))
            {
                this.init(2, "精金血刃", "s7122");//人技
                this.addDes("增加8%的攻击，命中提升20%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.08f, 1).putLearnResultRule("fg", 0, 0.08f, 1).putLearnResultRule("mz", 0, 0.2f, 1);
            }
            else if (key.Equals("10030035"))
            {
                this.init(2, "冰凌锋锐", "s7122");//人技
                this.addDes("增加6%的攻击，伤害提升12%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.06f, 1).putLearnResultRule("fg", 0, 0.06f, 1);
            }
            else if (key.Equals("10030036"))
            {
                this.init(2, "回光反照", "s7122");//人技
                this.addDes("受到致死伤害时，无敌一回合，并爆发巨大潜力");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030037"))
            {
                this.init(2, "强烈腐蚀", "s7122");//人技
                this.addDes("命中敌人后降低对方35%防御，持续2回合");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030038"))
            {
                this.init(2, "勇士之怒", "s7122");//天技 1级效果
                this.addDes("暴击后伤害提升至%s%%，同时命中提升15%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.15f, 1);
                this.addLineRule(0.1f, 2);
            }
            else if (key.Equals("10030039"))
            {
                this.init(2, "邪神附体", "s7122");//地技 1级效果
                this.addDes("每暴击一次暴击率增加%s%%，最多%s%%，同时出手速上升15%");
                this.bindSkillData(2, 1, 0, "*").putLearnResultRule("css", 0, 0.15f, 1);
                this.addLineRule(0.01f, 0).addLineRule(0.01f, 0);
            }
            else if (key.Equals("10030040"))
            {
                this.init(2, "嗜血狂攻", "s7122");//人技
                this.addDes("增加15%的攻击，同时降低5%防御和10%命中");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.15f, 1).putLearnResultRule("fg", 0, 0.15f, 1)
                    .putLearnResultRule("wf", 0, -0.05f, 1).putLearnResultRule("ff", 0, -0.05f, 1).putLearnResultRule("mz", 0, -0.1f, 1);
            }
            else if (key.Equals("10030041"))
            {
                this.init(2, "仙灵庇佑", "s7122");//天技
                this.addDes("打出暴击时，恢复%s%%的生命，同时命中上升15%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.15f, 1);
                this.addLineRule(0.01f, 0);
            }
            else if (key.Equals("10030042"))
            {
                this.init(2, "聚精会神", "s7122");//天技 1级
                this.addDes("命中上升%s%%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.05f, 1);
                this.addLineRule(0.05f, 0);
            }
            else if (key.Equals("10030043"))
            {
                this.init(2, "舍命七伤", "s7122");//人技
                this.addDes("增加30%攻击，同时降低60%闪避");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.3f, 1).putLearnResultRule("fg", 0, 0.3f, 1)
                    .putLearnResultRule("sd", 0, -0.6f, 1);
            }
            else if (key.Equals("10030044"))
            {
                this.init(2, "玄火炼骨", "s7122");//人技
                this.addDes("人物生命提升26%，防御降低3%，闪避降低5%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.26f, 1).putLearnResultRule("wf", 0, -0.03f, 1)
                    .putLearnResultRule("ff", 0, -0.03f, 1).putLearnResultRule("sd", 0, -0.05f, 1);
            }
            else if (key.Equals("10030045"))
            {
                this.init(2, "孤身铁胆", "s7122");//人技
                this.addDes("人物生命提升15%，闪避降低20%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.15f, 1).putLearnResultRule("sd", 0, -0.2f, 1);
            }
            else if (key.Equals("10030046"))
            {
                this.init(2, "混元诱杀", "s7122");//人技
                this.addDes("当目标被援护时伤害提升至200%");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030047"))
            {
                this.init(2, "惊恐噩梦", "s7122");//人技
                this.addDes("命中目标后敌方有25%的几率进入噩梦状态（释放技能时蓝消耗增加至400%，持续2回合）");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030048"))
            {
                this.init(2, "雷霆震击", "s7122");//天技 1级
                this.addDes("受到伤害时有%s%%的几率进行反击，同时人物命中上升15%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.15f, 1);
                this.addLineRule(0.03f, 0);
            }
            else if (key.Equals("10030049"))
            {
                this.init(2, "血腥愤怒", "s7122");//人技
                this.addDes("未命中敌人则增加15%攻击，持续2回合");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030050"))
            {
                this.init(2, "巨力碾压", "s7122");//人技
                this.addDes("50%几率打出碾压效果，伤害提升20%");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030051"))
            {
                this.init(2, "乘胜追击", "s7122");//天技 3
                this.addDes("攻击目标时可能会打出残废效果（持续伤害，持续2回合，%s%%几率触发），同时人物命中上升15%");
                this.bindSkillData(1, 1, 0, "*").putLearnResultRule("mz", 0, 0.15f, 1);
                this.addLineRule(0.1f, 0);
            }
            else if (key.Equals("10030052"))
            {
                this.init(2, "恃强凌弱", "s7122");//地技 3
                this.addDes("对残废状态下的目标伤害增加%s%%，同时出手速上升15%");
                this.bindSkillData(2, 1, 0, "*").putLearnResultRule("css", 0, 0.15f, 1);
                this.addLineRule(0.06f, 0);
            }
            else if (key.Equals("10030053"))
            {
                this.init(2, "飞驰电挚", "s7122");//天技 1
                this.addDes("出手速上升%s%%");
                this.bindSkillData(2, 1, 0, "*").putLearnResultRule("css", 0, 0.05f, 1);
                this.addLineRule(0.05f, 0);
            }
            //二字
            else if (key.Equals("10030054"))
            {
                this.init(2, "迅击", "s7122");//人技
                this.addDes("攻击增加8%，同时暴击下降3%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.08f, 1).putLearnResultRule("fg", 0, 0.08f, 1).putLearnResultRule("bj", 0, -0.03f, 1);
            }
            else if (key.Equals("10030055"))
            {
                this.init(2, "淬体", "s7122");//人技
                this.addDes("生命提高8%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.08f, 1);
            }
            else if (key.Equals("10030056"))
            {
                this.init(2, "锻脉", "s7122");//人技
                this.addDes("生命提高10%，暴击降低3%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1).putLearnResultRule("bj", 0, -0.03f, 1);
            }
            else if (key.Equals("10030057"))
            {
                this.init(2, "硬化", "s7122");//人技
                this.addDes("生命提高6%，命中提升15%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.06f, 1).putLearnResultRule("mz", 0, 0.15f, 1);
            }
            else if (key.Equals("10030058"))
            {
                this.init(2, "凝力", "s7122");//人技
                this.addDes("人物攻击提升6%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.06f, 1).putLearnResultRule("fg", 0, 0.06f, 1);
            }
            else if (key.Equals("10030059"))
            {
                this.init(2, "血刃", "s7122");//人技
                this.addDes("增加4%的攻击，命中提升15%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.04f, 1).putLearnResultRule("fg", 0, 0.04f, 1).putLearnResultRule("mz", 0, 0.15f, 1);
            }
            else if (key.Equals("10030060"))
            {
                this.init(2, "锋锐", "s7122");//人技
                this.addDes("增加3%的攻击，伤害提升6%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.03f, 1).putLearnResultRule("fg", 0, 0.03f, 1);
            }
            else if (key.Equals("10030061"))
            {
                this.init(2, "腐蚀", "s7122");//人技
                this.addDes("命中敌人后降低对方15%防御，持续2回合");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030062"))
            {
                this.init(2, "狂攻", "s7122");//人技
                this.addDes("增加10%的攻击，同时降低3%防御和5%命中");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1).putLearnResultRule("fg", 0, 0.1f, 1)
                    .putLearnResultRule("wf", 0, -0.03f, 1).putLearnResultRule("ff", 0, -0.03f, 1).putLearnResultRule("mz", 0, -0.05f, 1);
            }
            else if (key.Equals("10030063"))
            {
                this.init(2, "七伤", "s7122");//人技
                this.addDes("增加15%攻击，同时降低30%闪避");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("wg", 0, 0.15f, 1).putLearnResultRule("fg", 0, 0.15f, 1)
                    .putLearnResultRule("sd", 0, -0.3f, 1);
            }
            else if (key.Equals("10030064"))
            {
                this.init(2, "炼骨", "s7122");//人技
                this.addDes("人物生命提升13%，防御降低3%，闪避降低5%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.13f, 1).putLearnResultRule("wf", 0, -0.03f, 1)
                    .putLearnResultRule("ff", 0, -0.03f, 1).putLearnResultRule("sd", 0, -0.05f, 1);
            }
            else if (key.Equals("10030065"))
            {
                this.init(2, "铁胆", "s7122");//人技
                this.addDes("人物生命提升8%，闪避降低10%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.08f, 1).putLearnResultRule("sd", 0, -0.1f, 1);
            }
            else if (key.Equals("10030066"))
            {
                this.init(2, "诱杀", "s7122");//人技
                this.addDes("当目标被援护时伤害提升至150%");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030067"))
            {
                this.init(2, "噩梦", "s7122");//人技
                this.addDes("命中目标后敌方有10%的几率进入噩梦状态（释放技能时蓝消耗增加至400%，持续2回合）");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030068"))
            {
                this.init(2, "愤怒", "s7122");//人技
                this.addDes("未命中敌人则增加8%攻击，持续2回合");
                this.bindSkillData(3, 1, 0, "*");
            }
            else if (key.Equals("10030069"))
            {
                this.init(2, "碾压", "s7122");//人技
                this.addDes("50%几率打出碾压效果，伤害提升10%");
                this.bindSkillData(3, 1, 0, "*");
            }
            //===新增人技====
            else if (key.Equals("10030070"))
            {
                this.init(2, "游龙闪击", "s7122");//人技
                this.addDes("人物闪避提升16%，命中增加6%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("sd", 0, 0.16f, 1).putLearnResultRule("mz", 0, 0.06f, 1);
            }
            else if (key.Equals("10030071"))
            {
                this.init(2, "游龙", "s7122");//人技
                this.addDes("人物闪避提升12%，命中增加4%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("sd", 0, 0.12f, 1).putLearnResultRule("mz", 0, 0.04f, 1);
            }
            else if (key.Equals("10030072"))
            {
                this.init(2, "轻影锐攻", "s7122");//人技
                this.addDes("人物闪避提升10%，攻击增加8%，速度增加2%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("sd", 0, 0.10f, 1).putLearnResultRule("wg", 0, 0.08f, 1)
                    .putLearnResultRule("fg", 0, 0.08f, 1).putLearnResultRule("css", 0, 0.02f, 1);
            }
            else if (key.Equals("10030073"))
            {
                this.init(2, "轻影", "s7122");//人技
                this.addDes("人物闪避提升8%，攻击增加6%，速度增加2%");
                this.bindSkillData(3, 1, 0, "*").putLearnResultRule("sd", 0, 0.08f, 1).putLearnResultRule("wg", 0, 0.06f, 1)
                    .putLearnResultRule("fg", 0, 0.06f, 1).putLearnResultRule("css", 0, 0.02f, 1);
            }


            else
            {
                Debug.Log("仙绝未存在");
                this.init(2, "", "s7122");
                this.addDes("未定义");
            }
            this.addBuyRule(0, 1);
        }
        private void initJingMaiSkill(string key)
        {
            if (key.Equals("10020000"))
            {
                this.init(2, "青木长生", "s850");
                this.addDes("增加%s%点耐力");
                this.bindSkillData(6, 1, 0, "*").putLearnResultRule("nl", 4, 36, 0).putCountWay(1);//(36+4*lv+40)/2*lv
                this.addLineRule(0.04f, 0.36f);
            }
            else if (key.Equals("10020001"))
            {
                this.init(2, "赤火蛮力", "s837");
                this.addDes("增加%s%点力量");
                this.bindSkillData(6, 1, 0, "*").putLearnResultRule("ll", 4, 36, 0).putCountWay(1);
                this.addLineRule(0.04f, 0.36f);
            }
            else if (key.Equals("10020002"))
            {
                this.init(2, "精金疾风", "s845");
                this.addDes("增加%s%点敏捷");
                this.bindSkillData(6, 1, 0, "*").putLearnResultRule("mj", 4, 36, 0).putCountWay(1);
                this.addLineRule(0.04f, 0.36f);
            }
            else if (key.Equals("10020003"))
            {
                this.init(2, "玄水明智", "s849");
                this.addDes("增加%s%点智力");
                this.bindSkillData(6, 1, 0, "*").putLearnResultRule("zl", 4, 36, 0).putCountWay(1);
                this.addLineRule(0.04f, 0.36f);
            }
            else if (key.Equals("10020004"))
            {
                this.init(2, "戊土精元", "s858");
                this.addDes("增加%s%点精神");
                this.bindSkillData(6, 1, 0, "*").putLearnResultRule("js", 4, 36, 0).putCountWay(1);
                this.addLineRule(0.04f, 0.36f);
            }
        }

        private void initJobSkill(string key)
        {
            if (key.Equals("10000000"))//默认打击特效
            {
                this.init(2, "", "");
                this.putHitEffect("10045525");
            }
            else if (key.Equals("10000001"))
            {
                this.init(2, "墨家心法", "s100");
                this.addDes("提升力量%s%点，耐力%s%点");
                this.bindSkillData(0, 1, 0, "ms/dj/zs").putLearnResultRule("ll", 16, 0, 0).putLearnResultRule("nl", 16, 0, 0);
                this.addLineRule(0.16f, 0).addLineRule(0.16f, 0);
                this.addMaxLv(2);
            }
            else if (key.Equals("10000002"))
            {
                this.init(2, "猛虎纵", "s101");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "并有100%的几率造成每回合以目标所受伤害30%的流血效果，持续3回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物攻%s%点");
                this.bindSkillData(0, 0, 2, "ms").setIsNear(true).putLearnResultRule("wg", 3, 0, 0);
                this.addLineRule(0.03f, 0.78f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000003"))
            {
                this.init(2, "破天吼", "s103");
                this.addDes("使单体受到额外%s%%的伤害，并降低其受到治疗效果的30%，持续4回合。冷却1回合。" +
                    "需要消耗%s%点mp。" +
                            "学习此技能永久提升血量%s%点");
                this.bindSkillData(0, 0, 2, "ms").putLearnResultRule("max_xue", 6, 100, 0).bindCoolingRule(0f, 2f);
                this.addLineRule(0.06f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.06f, 1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000004"))
            {
                this.init(2, "杀气决", "s106");
                this.addDes("提升力量%s%点，耐力%s%点");
                this.bindSkillData(0, 1, 0, "ms").putLearnResultRule("ll", 36, 0, 0).putLearnResultRule("nl", 12, 0, 0);
                this.addLineRule(0.36f, 0).addLineRule(0.12f, 0);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000005"))
            {
                this.init(2, "连斩", "s112");
                this.addDes("连续攻击目标3次。第一次以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "第二次、第三次伤害依次递减。" + "使用后自身受伤提高30%，" +
                            "持续1回合，并且冷却1回合" +
                            "需要消耗%s%%点hp。" +
                            "学习此技能永久提升法防%s%点");
                this.bindSkillData(0, 0, 2, "ms").setIsNear(true).putLearnResultRule("ff", 3, 0, 0).bindCoolingRule(0f, 2f);
                this.addLineRule(0.03f, 1.02f).addLineRule(0.2f, 2f).addLineRule(0f, 0.03f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000006"))
            {
                this.init(2, "血祭", "s107");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "对流血状态下的目标攻击扩大%s%%,附加%s%点攻击。" +
                            "需要消耗%s%%点hp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ms").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.5f).addLineRule(0.2f, 2f).addLineRule(0.03f, 1.97f).addLineRule(0.4f, 4f)
                    .addLineRule(0f, 0.03f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000007"))
            {
                this.init(2, "狂龙吼", "s113");
                this.addDes("牺牲自身80%的防御" + "，增加%s%%的攻击伤害，持续2回合。冷却1回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升暴击%s%点");
                this.bindSkillData(0, 0, 0, "ms").putLearnResultRule("bj", 6, 20, 0).bindCoolingRule(0f, 2f); ;
                this.addLineRule(0.05f, 0.55f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0.2f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000008"))
            {
                this.init(2, "猛虎纵", "s101");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物攻%s%点");
                this.bindSkillData(0, 0, 2, "dj").setIsNear(true).putLearnResultRule("wg", 3, 0, 0);
                this.addLineRule(0.03f, 0.78f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000009"))
            {
                this.init(2, "破天吼", "s103");
                this.addDes("使敌方全体受到额外%s%%的物理伤害，持续4回合。冷却1回合。" +
                    "需要消耗%s%点mp。" +
                            "学习此技能永久提升生命%s%点");
                this.bindSkillData(0, 0, 2, "dj").putLearnResultRule("max_xue", 6, 100, 0).bindCoolingRule(0f, 2f); ;
                this.addLineRule(0.01f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.06f, 1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000010"))
            {
                this.init(2, "运气决", "s105");
                this.addDes("提升力量%s%点，耐力%s%点");
                this.bindSkillData(0, 1, 0, "dj").putLearnResultRule("ll", 12, 0, 0).putLearnResultRule("nl", 36, 0, 0);
                this.addLineRule(0.12f, 0).addLineRule(0.36f, 0);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000011"))
            {
                this.init(2, "破甲", "s111");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成伤害，" +
                    "同时减少其%s%%防御，额外承受%s%%伤害。" +
                            "持续4回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物攻%s%点");
                this.bindSkillData(0, 0, 2, "dj").setIsNear(true).putLearnResultRule("wg", 3, 0, 0);
                this.addLineRule(0.015f, 0.2f).addLineRule(0.2f, 2f).addLineRule(0.01f, 0.04f).addLineRule(0.01f, 0.05f)
                    .addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000012"))
            {
                this.init(2, "战魂歌", "s108");
                this.addDes("为己方全体增加%s%%伤害，" +
                            "并降低全体%s%%所受伤害，持续3回合，冷却3回合，" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物攻%s%点");
                this.bindSkillData(0, 0, 1, "dj").putLearnResultRule("wg", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.01f, 0.1f).addLineRule(0.02f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000013"))
            {
                this.init(2, "挫骨", "s116");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                    "并把造成伤害的30%转化成自己的生命。冷却1回合，" +
                            "需要消耗%s%点hp。" +
                            "学习此技能永久提升物攻%s%点");
                this.bindSkillData(0, 0, 2, "dj").setIsNear(true).putLearnResultRule("wg", 3, 0, 0).bindCoolingRule(0f, 2f);
                this.addLineRule(0.03f, 0.93f).addLineRule(0.2f, 2f).addLineRule(0f, 0.03f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000014"))
            {
                this.init(2, "道家心法", "s114");
                this.addDes("提升智力%s%点，精神%s%点");
                this.bindSkillData(0, 1, 0, "qm/ty/fz").putLearnResultRule("zl", 16, 0, 0).putLearnResultRule("js", 16, 0, 0);
                this.addLineRule(0.16f, 0f).addLineRule(0.16f, 0f);
                this.addMaxLv(2);
            }
            else if (key.Equals("10000015"))
            {
                this.init(2, "荡魔曲", "s129");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升法攻%s%点");
                this.bindSkillData(0, 0, 2, "qm").putLearnResultRule("fg", 3, 0, 0);
                this.addLineRule(0.02f, 1.05f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000016"))
            {
                this.init(2, "灵脉曲", "s124");
                this.addDes("解除一个友方中毒、流血、蛊（混乱、定身、睡眠）、魔化、诅咒等状态," +
                            "需要消耗%s%点mp。冷却1回合。" +
                            "学习此技能永久提升血量%s%点");
                this.bindSkillData(0, 0, 1, "qm").putLearnResultRule("max_xue", 6, 100, 0).bindCoolingRule(0f, 2f);
                this.addLineRule(0.05f, 0.1f).addLineRule(0.06f, 1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000017"))
            {
                this.init(2, "琴魔心法", "s115");
                this.addDes("提升智力%s%点，精神%s%点");
                this.bindSkillData(0, 1, 0, "qm").putLearnResultRule("zl", 36, 0, 0).putLearnResultRule("js", 12, 0, 0);
                this.addLineRule(0.36f, 0f).addLineRule(0.12f, 0f);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000018"))
            {
                this.init(2, "魔魂曲", "s126");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方多个目标造成伤害（数量随技能等级提升），" +
                            "100%几率造成魔化效果（魔化持续2回合）。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升血量%s%点");
                this.bindSkillData(0, 0, 2, "qm").putLearnResultRule("max_xue", 6, 100, 0);
                this.addLineRule(0.015f, 0.38f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.06f, 1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000019"))
            {
                this.init(2, "夺魂曲", "s127");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "对魔化状态下的目标攻击扩大%s%%,附加%s%点攻击。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升法攻%s%点");
                this.bindSkillData(0, 0, 2, "qm").putLearnResultRule("fg", 3, 0, 0);
                this.addLineRule(0.03f, 0.52f).addLineRule(0.2f, 2f).addLineRule(0.03f, 1.97f).addLineRule(0.4f, 4f)
                    .addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000020"))
            {
                this.init(2, "缚灵曲", "s128");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "并降低%s%%出手速、闪避（有效2回合）。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "qm").putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.3f).addLineRule(0.2f, 2f).addLineRule(0.02f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000021"))
            {
                this.init(2, "荡魔曲", "s129");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ty").putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.02f, 1.05f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000022"))
            {
                this.init(2, "灵脉曲", "s124");
                this.addDes("解除友方一个中毒、诅咒、蛊（混乱、定身、睡眠）状态。同时降低%s%%所受伤害（减伤持续2回合），" +
                            "需要消耗%s%点mp。冷却1回合。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 1, "ty").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 2f);
                this.addLineRule(0.03f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000023"))
            {
                this.init(2, "天音心法", "s135");
                this.addDes("提升智力%s%点，精神%s%点");
                this.bindSkillData(0, 1, 0, "ty").putLearnResultRule("zl", 12, 0, 0).putLearnResultRule("js", 36, 0, 0);
                this.addLineRule(0.12f, 0f).addLineRule(0.32f, 0f);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000024"))
            {
                this.init(2, "养生曲", "s102");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点，对友方单体生命进行恢复。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升法防%s%点");
                this.bindSkillData(0, 0, 1, "ty").putLearnResultRule("ff", 3, 0, 0);
                this.addLineRule(0.03f, 0.8f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000025"))
            {
                this.init(2, "天籁曲", "s183");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点，对友方全体生命进行恢复。" +
                            "随后的4回合以该值的35%恢复生命。可叠加3层。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升法防%s%点");
                this.bindSkillData(0, 0, 1, "ty").putLearnResultRule("ff", 3, 0, 0);
                this.addLineRule(0.015f, 0.3f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000026"))
            {
                this.init(2, "返魂曲", "s187");
                this.addDes("将一个友方目标复活，并恢复%s%%生命。" +
                            "需要消耗%s%点mp。冷却3回合。" +
                            "学习此技能永久提升法攻%s%点");
                this.bindSkillData(0, 0, 1, "ty").putLearnResultRule("fg", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.04f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000027"))
            {
                this.init(2, "阴阳家心法", "s196");
                this.addDes("提升力量%s%点，敏捷%s%点");
                this.bindSkillData(0, 1, 0, "ym/lc/fz").putLearnResultRule("ll", 16, 0, 0).putLearnResultRule("mj", 16, 0, 0);
                this.addLineRule(0.16f, 0f).addLineRule(0.16f, 0f);
                this.addMaxLv(2);
            }
            else if (key.Equals("10000028"))
            {
                this.init(2, "幽冥经", "s205");
                this.addDes("提升力量%s%点，敏捷%s%点");
                this.bindSkillData(0, 1, 0, "ym").putLearnResultRule("ll", 36, 0, 0).putLearnResultRule("mj", 12, 0, 0);
                this.addLineRule(0.36f, 0f).addLineRule(0.12f, 0f);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000029"))
            {
                this.init(2, "淬毒", "s303");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害（自身恢复该伤害10%的血量）。附带中毒效果，每回合按该伤害的100%持续减血。持续2回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ym").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.15f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000030"))
            {
                this.init(2, "扬沙", "s323");
                this.addDes("降低敌方单体%s%%命中。冷却3回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ym").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.03f, 0.05f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000031"))
            {
                this.init(2, "影袭", "s321");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，并附加目标最大血量5%的伤害。" +
                            "%s%%概率使敌方当前回合无法行动（持续1回合）。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ym").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.83f).addLineRule(0.2f, 2f).addLineRule(0.01f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000032"))
            {
                this.init(2, "断脉", "s324");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "对中毒状态下的目标攻击扩大%s%%,附加%s%点攻击。" +
                            "目标有%s%%概率混乱（持续1回合）。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "ym").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.3f).addLineRule(0.2f, 2f).addLineRule(0.03f, 1.97f).addLineRule(0.4f, 4f)
                    .addLineRule(0.01f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000033"))
            {
                this.init(2, "凝钢决", "s391");
                this.addDes("减少50%所受伤害，免疫混乱、昏睡（2回合有效）。冷却2回合。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 0, "ym").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 3f);
                this.addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000034"))
            {
                this.init(2, "罗刹经", "s234");
                this.addDes("提升力量%s%点，敏捷%s%点");
                this.bindSkillData(0, 1, 0, "lc").putLearnResultRule("ll", 12, 0, 0).putLearnResultRule("mj", 36, 0, 0);
                this.addLineRule(0.12f, 0f).addLineRule(0.36f, 0f);
                this.addMaxLv(8);
            }
            else if (key.Equals("10000035"))
            {
                this.init(2, "淬毒", "s303");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，每回合按该伤害的100%持续减血。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "lc").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.15f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000036"))
            {
                this.init(2, "扬沙", "s323");
                this.addDes("降低敌方单体%s%%命中。" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点。冷却3回合。");
                this.bindSkillData(0, 0, 2, "lc").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.04f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000037"))
            {
                this.init(2, "巫蛊傀儡", "s590");
                this.addDes("%s%%几率对方无法行动。" +
                            "需要消耗%s%点mp。冷却3回合。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "lc").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.04f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000038"))
            {
                this.init(2, "迷神蛊", "s407");
                this.addDes("%s%%几率造成混乱，对方伤害降低50%，随机攻击附近目标。" +
                            "需要消耗%s%点mp。冷却3回合。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "lc").putLearnResultRule("wf", 3, 0, 0).bindCoolingRule(0f, 4f);
                this.addLineRule(0.04f, 0.2f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000039"))
            {
                this.init(2, "碎魂", "s412");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，对于处于中毒状态的目标伤害扩大至%s%%附加%s%点攻击，。" +
                            "%s%%几率进入昏睡（无法行动，两回合，受到攻击时状态解除），" +
                            "需要消耗%s%点mp。" +
                            "学习此技能永久提升物防%s%点");
                this.bindSkillData(0, 0, 2, "lc").setIsNear(true).putLearnResultRule("wf", 3, 0, 0);
                this.addLineRule(0.03f, 0.3f).addLineRule(0.2f, 2f).addLineRule(0.03f, 1.97f).addLineRule(0.4f, 4f)
                     .addLineRule(0.02f, 0.1f).addLineRule(0.05f, 0.1f).addLineRule(0.03f, 0f);
                this.addMaxLv(20);
            }
            //6个门派形象时的门派技能
            else if (key.Equals("10000040"))
            {
                this.init(2, "猛虎纵", "s101");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "需要消耗%s%点mp。");
                this.bindSkillData(0, 0, 2, "zs").setIsNear(true);
                this.addLineRule(0.03f, 0.78f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000041"))
            {
                this.init(2, "破天吼", "s103");
                this.addDes("使单体受到额外%s%%的伤害，持续4回合。冷却1回合。" +
                    "需要消耗%s%点mp。冷却1回合");
                this.bindSkillData(0, 0, 2, "zs").bindCoolingRule(0f, 2f);
                this.addLineRule(0.06f, 0.1f).addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000042"))
            {
                this.init(2, "荡魔曲", "s129");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害，" +
                            "需要消耗%s%点mp。");
                this.bindSkillData(0, 0, 2, "fs");
                this.addLineRule(0.02f, 1.05f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000043"))
            {
                this.init(2, "灵脉曲", "s124");
                this.addDes("解除一个友方中毒、流血等状态," +
                            "需要消耗%s%点mp。冷却1回合");
                this.bindSkillData(0, 0, 1, "fs").bindCoolingRule(0f, 2f);
                this.addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000044"))
            {
                this.init(2, "淬毒", "s303");
                this.addDes("以自身攻击的%s%%为基础，附加%s%点攻击，对敌方造成单体伤害。附带中毒效果，每回合按该伤害的100%持续减血。持续2回合。" +
                            "需要消耗%s%点mp。");
                this.bindSkillData(0, 0, 2, "fz").setIsNear(true);
                this.addLineRule(0.03f, 0.15f).addLineRule(0.2f, 2f).addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000045"))
            {
                this.init(2, "扬沙", "s323");
                this.addDes("降低敌方单体%s%%命中。" +
                            "需要消耗%s%点mp。冷却3回合。");
                this.bindSkillData(0, 0, 2, "fz").bindCoolingRule(0f, 4f);
                this.addLineRule(0.03f, 0.05f).addLineRule(0.05f, 0.1f);
                this.addMaxLv(20);
            }
            else if (key.Equals("10000046"))
            {
                this.init(2, "防御", "s323");
                this.addDes("降低15%受伤");
                this.bindSkillData(0, 0, 2, "*");
            }
        }
        //近战、远程(默认)
        public bool isNear = false;
        public Skill setIsNear(bool isNear)
        {
            this.isNear = isNear;
            return this;
        }
        public List<fixedRule> LineRules;
        public Skill addLineRule(float k0, float b0)
        {
            if (this.LineRules == null) this.LineRules = new List<fixedRule>();
            this.LineRules.Add(new fixedRule(k0, b0));
            return this;
        }
        public string getSkillDes(int lv)
        {
            string str = this.des + "";
            if (this.LineRules == null)
            {
                return str;
            }
            for (int i = 0; i < this.LineRules.Count; i++)
            {
                fixedRule r = this.LineRules[i];
                int index = str.IndexOf("%s%");
                //Debug.Log(index + "=>" + str);
                float res = 0;
                if (this.countWay == 0) res = r.getV(lv);
                else if (this.countWay == 1) res = r.getNItemSum(lv);
                str = str.Substring(0, index) + Convert.ToInt32(res * 100f).ToString() + str.Substring(index + 3);
            }
            return str;
        }
        private void initPetSkill(string key)
        {
            if (key.Equals("10010000"))//默认打击特效
            {
                this.init(2, "普攻", "1");
                this.putHitEffect("10045525");
            }


            else if (key.Equals("10010178"))
            {
                this.init(2, "狂舞乱击", "s4050");
                this.addDes("以自身105%的攻击随机一个敌方目标进行攻击，命中则必然暴击，未命中则增加25%攻击。");
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010179"))
            {
                this.init(2, "轰天狂雷", "s4050");
                this.addDes("对6个目标造成80%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010180"))//落羽
            {
                this.init(2, "真灵火攻", "s4050");
                this.addDes("对目标造成160%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010181"))//青玄剑灵
            {
                this.init(2, "真魔血破", "s4050");
                this.addDes("对同一排的敌方目标造成90%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010182"))//太虚之魂
            {
                this.init(2, "烈焰焚野", "s4050");
                this.addDes("随机对敌方3个目标造成80%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010183"))//幽玄魔俑
            {
                this.init(2, "坚盾援护", "s4050");
                this.addDes("（遭受单体攻击时）当前回合替一名友方目标承受伤害，自身承受80%，友方受30%，最多生效2次");
                this.bindSkillData(5, 0, 1, "*").setIsNear(true);
            }
            else if (key.Equals("10010184"))//
            {
                this.init(2, "神鬼俱焚", "s4050");
                this.addDes("命中后20%几率双倍伤害");
                this.bindSkillData(4, 1, 2, "1225");
            }
            else if (key.Equals("10010185"))
            {
                this.init(2, "双龙逆天", "s4050");
                this.addDes("拥有此技能将提高50%暴击，以自身攻击力的180%斩向敌人单体，如果暴击,则下回合增加15%的攻击，可以叠加3次，持续3回合");
                this.bindSkillData(4, 0, 2, "1225").putLearnResultRule("bj", 0, 0.5f, 1).setIsNear(true);
            }
            else if (key.Equals("10010186"))
            {
                this.init(2, "仙魔真诀", "s4050");
                this.addDes("自身每受伤一次，伤害减免增加10%，可叠加4次");
                this.bindSkillData(4, 1, 0, "1225");
            }
            else if (key.Equals("10010187"))
            {
                this.init(2, "真武御体", "s4050");
                this.addDes("给自己加持血盾，当受到攻击时，把伤害转化成治疗吸收，盾当前回合有效且生效2次，冷却5回合");
                this.bindSkillData(4, 0, 0, "1225");
            }
            else if (key.Equals("10010188"))
            {
                this.init(2, "乾坤逆转", "s4050");
                this.addDes("消耗15%的最大血量（当前血量若超出最大血量则扣除）按150%的比例转为物攻，同时以物攻的110%对敌方单个目标造成伤害，冷却5回合");
                this.bindSkillData(4, 0, 2, "1221");
            }
            else if (key.Equals("10010189"))
            {
                this.init(2, "万魔归宗", "s4050");
                this.addDes("当自身生命低于30%时，将提升自身20%的攻击");
                this.bindSkillData(4, 1, 0, "1221");
            }
            else if (key.Equals("10010190"))
            {
                this.init(2, "邪龙附体", "s4050");
                this.addDes("死亡时为主人增加20%的攻击力");
                this.bindSkillData(4, 1, 1, "1221");
            }
            else if (key.Equals("10010191"))
            {
                this.init(2, "碎元剑诀", "s4050");
                this.addDes("攻击会减少对方5%的生命上限，以攻击的200%对目标造成伤害，目标损失的上限会给目标造成额外伤害");
                this.bindSkillData(4, 0, 2, "1222");
            }
            else if (key.Equals("10010192"))
            {
                this.init(2, "绝杀咒印", "s4050");
                this.addDes("致死时无敌一回合，伤害增加50%");
                this.bindSkillData(4, 1, 0, "1222");
            }
            else if (key.Equals("10010193"))
            {
                this.init(2, "万剑归宗", "s4050");
                this.addDes("伤害额外提升30%");
                this.bindSkillData(4, 1, 0, "1222");
            }
            else if (key.Equals("10010194"))
            {
                this.init(2, "天魔噬魂", "s4050");
                this.addDes("以80%的攻击给所有目标造成伤害，造成伤害后防御降低30%");
                this.bindSkillData(4, 0, 2, "1224");
            }
            else if (key.Equals("10010195"))
            {
                this.init(2, "移魂转命", "s4050");
                this.addDes("致死时100%触发不死，不死触发后恢复全部生命并增加20%法攻");
                this.bindSkillData(4, 1, 0, "1224");
            }
            else if (key.Equals("10010196"))
            {
                this.init(2, "九天魔甲", "s4050");
                this.addDes("减少40%受伤，拥有此技能闪避增加20%");
                this.bindSkillData(4, 1, 0, "1224");
                this.putLearnResultRule("sd", 0, 0.2f, 1);
            }
            else if (key.Equals("10010197"))
            {
                this.init(2, "蛇啸焚天", "s4050");
                this.addDes("xxx");
            }
            else if (key.Equals("10010198"))
            {
                this.init(2, "蛊心定影", "s4050");
                this.addDes("xxx");
            }
            else if (key.Equals("10010199"))
            {
                this.init(2, "天蛇噬血", "s4050");
                this.addDes("xxx");
            }
            else if (key.Equals("10010200"))
            {
                this.init(2, "天火燎原", "s4050");
                this.addDes("消耗身上的霸体给敌方造成400%的法术伤害，每消耗一层霸体伤害提升10%，冷却5回合");
                this.bindSkillData(4, 0, 2, "1226");
            }
            else if (key.Equals("10010201"))
            {
                this.init(2, "混元霸体", "s4050");
                this.addDes("每回合获得1层霸体，受伤时也获得1层，每获得一次增加10%的减伤，持续5回合，可叠加5次，受到致死伤害时立即恢复60%的血量（该效果一场只能触发一次）");
                this.bindSkillData(4, 1, 0, "1226");
            }
            else if (key.Equals("10010202"))
            {
                this.init(2, "都天炎爆", "s4050");
                this.addDes("对目标造成250%的法术伤害，并使其进入天罚状态（受伤增加10%，攻击下降10%，持续2回合，叠加2次），同时给自己加1层霸体");
                this.bindSkillData(4, 0, 2, "1226");
            }
            else if (key.Equals("10010203"))
            {
                this.init(2, "九域神威", "s4050");
                this.addDes("每受到一次伤害，攻击增加10%攻击，效果持续3回合，可叠加10次");
                this.bindSkillData(4, 1, 0, "1226");
            }
            else if (key.Equals("10010204"))
            {
                this.init(2, "烈战狂歌", "s4050");
                this.addDes("开场立即获得5层霸体，拥有此技能攻击增加30%");
                this.bindSkillData(4, 1, 0, "1226")
                    .putLearnResultRule("wg", 0f, 0.3f, 1).putLearnResultRule("fg", 0f, 0.3f, 1);
            }
            else if (key.Equals("10010205"))
            {
                this.init(2, "神龙摆尾", "s4050");
                this.addDes("对目标造成250%的攻击伤害，同时使对方进入恐惧状态，降低物理防御（叠加3层）");
                this.bindSkillData(4, 0, 2, "1227");
            }
            else if (key.Equals("10010206"))
            {
                this.init(2, "龙族本能", "s4050");
                this.addDes("对于处于恐惧状态下的目标，伤害提升30%，拥有此技能攻击提升35%");
                this.bindSkillData(4, 1, 0, "1227")
                    .putLearnResultRule("wg", 0f, 0.35f, 1).putLearnResultRule("fg", 0f, 0.35f, 1);
            }
            else if (key.Equals("10010207"))
            {
                this.init(2, "亢龙有悔", "s4050");
                this.addDes("对目标造成400%的攻击伤害，并在下回合燃烧目标魔法，冷却5回合");
                this.bindSkillData(4, 0, 2, "1227");
            }
            else if (key.Equals("10010208"))
            {
                this.init(2, "神龙祝福", "s4050");
                this.addDes("致死时恢复60%生命，一场战斗仅生效一次，每次攻击有15%几率造成双倍伤害");
                this.bindSkillData(4, 1, 0, "1227");
            }
            else if (key.Equals("10010209"))
            {
                this.init(2, "龙鳞护体", "s4050");
                this.addDes("降低60%受伤，死亡后降低所有敌方目标物理防御，一场战斗仅生效一次");
                this.bindSkillData(4, 1, 0, "1227");
            }

            //=========寻秦部分的技能============
            else if (key.Equals("10010210"))
            {
                this.init(2, "铁命", "s4050");
                this.addDes("当自身生命低于30%时，降低自身所受伤害的20%");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010211"))
            {
                this.init(2, "血杀", "s4050");
                this.addDes("增加10%命中，以130%的攻击对目标造成伤害，自身会额外遭受40%的伤害");//20 170 20
                this.bindSkillData(5, 0, 2, "*").putLearnResultRule("mz", 0, 0.1f, 1).setIsNear(true);
            }
            else if (key.Equals("10010212"))
            {
                this.init(2, "焚野", "s4050");
                this.addDes("随机对敌方3个目标造成60%的攻击伤害");//80
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010213"))
            {
                this.init(2, "战魂", "s4050");
                this.addDes("以180%的攻击对目标造成伤害，战魂会让下回合的伤害提高10%，冷却3回合");//220 25
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0, 4);
            }
            else if (key.Equals("10010214"))
            {
                this.init(2, "忘我", "s4050");
                this.addDes("攻击时有20%几率增加20%的攻击，拥有此技能后闪避降低30%，防御降低40%");//50 35 20 30
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, -0.3f, 1)
                    .putLearnResultRule("wf", 0, -0.4f, 1).putLearnResultRule("ff", 0, -0.4f, 1);
            }
            else if (key.Equals("10010215"))
            {
                this.init(2, "猛力", "s4050");
                this.addDes("增加8%攻击，拥有此技能后闪避降低40%，防御降低35%");//15 30 25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.08f, 1).putLearnResultRule("fg", 0, 0.08f, 1).putLearnResultRule("sd", 0, -0.4f, 1)
                    .putLearnResultRule("wf", 0, -0.35f, 1).putLearnResultRule("ff", 0, -0.35f, 1);
            }
            else if (key.Equals("10010216"))
            {
                this.init(2, "背刺", "s4050");
                this.addDes("当前回合反弹1次伤害，且自身不受伤害，冷却4回合");
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 5f);
            }
            else if (key.Equals("10010217"))
            {
                this.init(2, "猛攻", "s4050");
                this.addDes("增加10%攻击，拥有此技能后防御降低50%");//20 40
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1).putLearnResultRule("fg", 0, 0.1f, 1)
                    .putLearnResultRule("wf", 0, -0.5f, 1).putLearnResultRule("ff", 0, -0.5f, 1);
            }
            else if (key.Equals("10010218"))
            {
                this.init(2, "回灵", "s4050");
                this.addDes("每回合自动恢复5%的蓝量");//10
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010219"))
            {
                this.init(2, "护体", "s4050");
                this.addDes("降低自身15%所受伤害，持续2回合，冷却2回合");//30
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 3f); ;
            }
            else if (key.Equals("10010220"))
            {
                this.init(2, "绝影", "s4050");
                this.addDes("提高15%攻击和10%闪避");//30 20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.15f, 1).putLearnResultRule("fg", 0, 0.15f, 1)
                    .putLearnResultRule("sd", 0, 0.1f, 1);
            }
            else if (key.Equals("10010221"))
            {
                this.init(2, "玄心", "s4050");
                this.addDes("提高15%攻击和10%命中");//30 20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.15f, 1).putLearnResultRule("fg", 0, 0.15f, 1)
                    .putLearnResultRule("mz", 0, 0.1f, 1);
            }
            else if (key.Equals("10010222"))
            {
                this.init(2, "狂暴", "s4050");
                this.addDes("提高10%暴击，同时降低15%的生命上限");//20 15
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.1f, 1).putLearnResultRule("max_xue", 0, -0.15f, 1);
            }
            else if (key.Equals("10010223"))
            {
                this.init(2, "体术", "s4050");
                this.addDes("提高12%的生命上限，同时降低8%的防御");//22 4
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1)
                    .putLearnResultRule("wf", 0, -0.08f, 1).putLearnResultRule("ff", 0, -0.08f, 1);
            }
            else if (key.Equals("10010224"))
            {
                this.init(2, "血击", "s4050");
                this.addDes("生命低于30%时，反击概率增加25%");//40
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010225"))
            {
                this.init(2, "刚体", "s4050");
                this.addDes("降低80%所受的伤害，冷却4回合");//90
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 5f);
            }
            else if (key.Equals("10010226"))
            {
                this.init(2, "返血", "s4050");
                this.addDes("受到暴击后恢复自身生命上限10%的血量，冷却3回合");//20
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010227"))
            {
                this.init(2, "不死", "s4050");
                this.addDes("50%几率受到致死伤害时保留1点血，仅触发1次");//100
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010228"))
            {
                this.init(2, "狂化", "s4050");
                this.addDes("提高8%的暴击，同时降低30%防御");//15
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.08f, 1)
                    .putLearnResultRule("wf", 0, -0.3f, 1).putLearnResultRule("ff", 0, -0.3f, 1);
            }
            else if (key.Equals("10010229"))
            {
                this.init(2, "反扑", "s4050");
                this.addDes("生命低于30%时，暴击提升15%");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010230"))
            {
                this.init(2, "反震", "s4050");
                this.addDes("受伤时有10%几率将50%伤害反震给对方");//20 
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010231"))
            {
                this.init(2, "沉甲", "s4050");
                this.addDes("增加12%防御，同时降低20%闪避");//25 20 
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, -0.2f, 1)
                    .putLearnResultRule("wf", 0, 0.12f, 1).putLearnResultRule("ff", 0, 0.12f, 1);
            }
            else if (key.Equals("10010232"))
            {
                this.init(2, "缚命", "s4050");
                this.addDes("对带有回血buff的目标伤害增加30%");//60
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010233"))
            {
                this.init(2, "奔雷", "s4050");
                this.addDes("提高8%的暴击和5%的速度");//15 10
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.08f, 1).putLearnResultRule("css", 0, 0.05f, 1);
            }
            else if (key.Equals("10010234"))
            {
                this.init(2, "爆斩", "s4050");
                this.addDes("暴击后伤害增加10%（2回合有效）");//20
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010235"))
            {
                this.init(2, "神速", "s4050");
                this.addDes("提高15%的手速跟15%的命中，同时防御降低35%，闪避降低30%");//25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("css", 0, 0.15f, 1).putLearnResultRule("mz", 0, 0.15f, 1)
                    .putLearnResultRule("wf", 0, -0.35f, 1).putLearnResultRule("ff", 0, -0.35f, 1).putLearnResultRule("sd", 0, -0.3f, 1);
            }
            else if (key.Equals("10010236"))
            {
                this.init(2, "体魄", "s4050");
                this.addDes("提高10%的生命上限，同时降低20%的攻击");//25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1).putLearnResultRule("wg", 0, -0.2f, 1)
                    .putLearnResultRule("fg", 0, -0.2f, 1);
            }
            else if (key.Equals("10010237"))
            {
                this.init(2, "残影", "s4050");
                this.addDes("增加20%的闪避");//50
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, 0.2f, 1);
            }
            else if (key.Equals("10010238"))
            {
                this.init(2, "疗血", "s4050");
                this.addDes("每回合恢复最大生命6%的血量");//10
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010239"))
            {
                this.init(2, "重生", "s4050");
                this.addDes("死亡后有20%几率以70%生命复活，一场战斗仅一次");//100
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010240"))
            {
                this.init(2, "吸血", "s4050");
                this.addDes("以100%的攻击对目标造成伤害，并吸取该伤害的70%恢复血量（冷却1回合）");//100
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0f, 2f).setIsNear(true);
            }
            else if (key.Equals("10010241"))
            {
                this.init(2, "石肤", "s4050");
                this.addDes("防御提升15%");//20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wf", 0, 0.15f, 1).putLearnResultRule("ff", 0, 0.15f, 1);
            }
            else if (key.Equals("10010242"))
            {
                this.init(2, "命术", "s4050");
                this.addDes("提升7%生命上限");//10
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.07f, 1);
            }
            else if (key.Equals("10010243"))
            {
                this.init(2, "裂骨", "s4050");//裂骨鬼刃
                this.addDes("对敌方造成攻击伤害时，对方每回合以该伤害的8%持续掉血（持续2回合）");//15
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010244"))
            {
                this.init(2, "反击", "s4050");
                this.addDes("被攻击时有15%几率反击对方");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            //todo:
            else if (key.Equals("10010245"))
            {
                this.init(2, "木灵", "s4050");
                this.addDes("增加10%生命，同时被治疗效果增加10%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1);
            }
            else if (key.Equals("10010246"))
            {
                this.init(2, "火灵", "s4050");
                this.addDes("增加10%攻击，同时造成伤害后攻击提升10%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1)
                        .putLearnResultRule("fg", 0, 0.1f, 1);
            }
            else if (key.Equals("10010247"))
            {
                this.init(2, "土灵", "s4050");
                this.addDes("增加10%防御，同时受到伤害时防御增加10%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wf", 0, 0.1f, 1)
                        .putLearnResultRule("ff", 0, 0.1f, 1);
            }
            else if (key.Equals("10010248"))
            {
                this.init(2, "金灵", "s4050");
                this.addDes("增加10%闪避，同时造成伤害后降低对方10%攻击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, 0.1f, 1);
            }
            else if (key.Equals("10010249"))
            {
                this.init(2, "水灵", "s4050");
                this.addDes("增加10%暴击，同时暴击后暴击提升10%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.1f, 1);
            }
            else if (key.Equals("10010250"))
            {
                this.init(2, "迅捷", "s4050");
                this.addDes("增加10%出手速，降低40%防御");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("css", 0, 0.1f, 1)
                        .putLearnResultRule("wf", 0, -0.4f, 1)
                        .putLearnResultRule("ff", 0, -0.4f, 1);
            }
            else if (key.Equals("10010251"))
            {
                this.init(2, "厚甲", "s4050");
                this.addDes("增加10%防御，降低10%生命");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, -0.1f, 1)
                        .putLearnResultRule("wf", 0, 0.1f, 1)
                        .putLearnResultRule("ff", 0, 0.1f, 1);
            }
            else if (key.Equals("10010252"))
            {
                this.init(2, "鲁莽", "s4050");
                this.addDes("增加10%暴击，降低20%命中");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.1f, 1)
                        .putLearnResultRule("mz", 0, -0.2f, 1);
            }
            else if (key.Equals("10010253"))
            {
                this.init(2, "援护", "s4050");
                this.addDes("（遭受单体攻击时）当前回合替一名友方目标承受伤害，自身承受80%，友方受30%，最多生效1次");
                this.bindSkillData(5, 0, 1, "*");
            }
            else if (key.Equals("10010254"))
            {
                this.init(2, "灵噬", "s4050");
                this.addDes("以攻击的40%燃烧敌方魔法，冷却2回合");
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0f, 3f);
            }
            else if (key.Equals("10010255"))
            {
                this.init(2, "强攻", "s4050");
                this.addDes("增加10%攻击，降低15%生命");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1)
                        .putLearnResultRule("fg", 0, 0.1f, 1)
                        .putLearnResultRule("max_xue", 0, -0.15f, 1);
            }
            else if (key.Equals("10010256"))
            {
                this.init(2, "暴怒", "s4050");
                this.addDes("增加4%暴击，造成伤害后增加2%暴击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.04f, 1);
            }
            else if (key.Equals("10010257"))
            {
                this.init(2, "乱击", "s4050");
                this.addDes("以自身85%的攻击随机一个敌方目标进行攻击，命中则必然暴击，未命中则增加15%攻击。");
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010258"))
            {
                this.init(2, "狂雷", "s4050");
                this.addDes("对6个目标造成60%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010259"))
            {
                this.init(2, "火攻", "s4050");
                this.addDes("对目标造成120%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010260"))
            {
                this.init(2, "血破", "s4050");
                this.addDes("对同一排的敌方目标造成60%的攻击伤害");
                this.bindSkillData(5, 0, 2, "*").setIsNear(true);
            }
            else if (key.Equals("10010261"))
            {
                this.init(2, "嗜血", "s4050");
                this.addDes("增加4%攻击，造成伤害后增加2%攻击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.04f, 1)
                        .putLearnResultRule("fg", 0, 0.04f, 1);
            }
            else if (key.Equals("10010262"))
            {
                this.init(2, "铁骨", "s4050");
                this.addDes("受到的伤害降低8%");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010263"))
            {
                this.init(2, "锐爪", "s4050");
                this.addDes("增加5%攻击%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.05f, 1)
                        .putLearnResultRule("fg", 0, 0.05f, 1);
            }
            else if (key.Equals("10010264"))
            {
                this.init(2, "精准", "s4050");
                this.addDes("增加5%命中%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("mz", 0, 0.05f, 1);
            }
            //======四字==========
            else if (key.Equals("10010265"))
            {
                this.init(2, "真魔铁命", "s4050");
                this.addDes("当自身生命低于30%时，降低自身所受伤害的30%");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010266"))
            {
                this.init(2, "真魔血杀", "s4050");
                this.addDes("增加20%命中，以170%的攻击对目标造成伤害，自身会额外遭受20%的伤害");//20 170 20
                this.bindSkillData(5, 0, 2, "*").putLearnResultRule("mz", 0, 0.2f, 1).setIsNear(true);
            }
            else if (key.Equals("10010267"))
            {
                this.init(2, "灭世战魂", "s4050");
                this.addDes("以220%的攻击对目标造成伤害，战魂会让下回合的伤害提高25%，冷却3回合");//220 25
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0, 4);
            }
            else if (key.Equals("10010268"))
            {
                this.init(2, "忘我强攻", "s4050");
                this.addDes("攻击时有50%几率增加35%的攻击，拥有此技能后闪避降低20%，防御降低30%");//50 35 20 30
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, -0.2f, 1)
                        .putLearnResultRule("wf", 0, -0.3f, 1)
                        .putLearnResultRule("ff", 0, -0.3f, 1);
            }
            else if (key.Equals("10010269"))
            {
                this.init(2, "摧山猛力", "s4050");
                this.addDes("增加15%攻击，拥有此技能后闪避降低30%，防御降低25%");//15 30 25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.15f, 1)
                        .putLearnResultRule("fg", 0, 0.15f, 1)
                        .putLearnResultRule("sd", 0, -0.3f, 1)
                        .putLearnResultRule("wf", 0, -0.25f, 1)
                        .putLearnResultRule("ff", 0, -0.25f, 1);
            }
            else if (key.Equals("10010270"))
            {
                this.init(2, "连环背刺", "s4050");
                this.addDes("当前回合反弹2次伤害，且自身不受伤害，冷却4回合");
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 5f);
            }
            else if (key.Equals("10010271"))
            {
                this.init(2, "天生猛攻", "s4050");
                this.addDes("增加20%攻击，拥有此技能后防御降低40%");//20 40
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.2f, 1)
                        .putLearnResultRule("fg", 0, 0.2f, 1)
                        .putLearnResultRule("wf", 0, -0.4f, 1)
                        .putLearnResultRule("ff", 0, -0.4f, 1);
            }
            else if (key.Equals("10010272"))
            {
                this.init(2, "仙术回灵", "s4050");
                this.addDes("每回合自动恢复10%的蓝量");//10
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010273"))
            {
                this.init(2, "神盾护体", "s4050");
                this.addDes("降低自身30%所受伤害，持续2回合，冷却2回合");//30
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 3f); ;
            }
            else if (key.Equals("10010274"))
            {
                this.init(2, "踏雪绝影", "s4050");
                this.addDes("提高30%攻击和20%闪避");//30 20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.3f, 1)
                        .putLearnResultRule("fg", 0, 0.3f, 1)
                        .putLearnResultRule("sd", 0, 0.2f, 1);
            }
            else if (key.Equals("10010275"))
            {
                this.init(2, "玄心破霄", "s4050");
                this.addDes("提高30%攻击和20%命中");//30 20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.3f, 1)
                        .putLearnResultRule("fg", 0, 0.3f, 1)
                        .putLearnResultRule("mz", 0, 0.2f, 1);
            }
            else if (key.Equals("10010276"))
            {
                this.init(2, "天生狂暴", "s4050");
                this.addDes("提高20%暴击，同时降低15%的生命上限");//20 15
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.2f, 1)
                        .putLearnResultRule("max_xue", 0, -0.15f, 1);
            }
            else if (key.Equals("10010277"))
            {
                this.init(2, "真魔体术", "s4050");
                this.addDes("提高22%的生命上限，同时降低4%的防御");//22 4
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.22f, 1)
                        .putLearnResultRule("wf", 0, -0.04f, 1)
                        .putLearnResultRule("ff", 0, -0.04f, 1);
            }
            else if (key.Equals("10010278"))
            {
                this.init(2, "绝命血击", "s4050");
                this.addDes("生命低于30%时，反击概率增加35%");//40
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010279"))
            {
                this.init(2, "天魔刚体", "s4050");
                this.addDes("降低90%所受的伤害，冷却4回合");//90
                this.bindSkillData(5, 0, 0, "*").bindCoolingRule(0f, 5f);
            }
            else if (key.Equals("10010280"))
            {
                this.init(2, "真灵返血", "s4050");
                this.addDes("受到暴击后恢复自身生命上限20%的血量，冷却3回合");//20
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010281"))
            {
                this.init(2, "天命不死", "s4050");
                this.addDes("100%几率受到致死伤害时保留1点血，仅触发1次");//100
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010282"))
            {
                this.init(2, "真魔狂化", "s4050");
                this.addDes("提高15%的暴击，同时降低30%防御");//15
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.15f, 1)
                        .putLearnResultRule("wf", 0, -0.3f, 1)
                        .putLearnResultRule("ff", 0, -0.3f, 1);
            }
            else if (key.Equals("10010283"))
            {
                this.init(2, "绝境反扑", "s4050");
                this.addDes("生命低于30%时，暴击提升30%");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010284"))
            {
                this.init(2, "霸气反震", "s4050");
                this.addDes("受伤时有20%几率将50%伤害反震给对方");//20 
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010285"))
            {
                this.init(2, "天生沉甲", "s4050");
                this.addDes("增加25%防御，同时降低20%闪避");//25 20 
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wf", 0, 0.25f, 1)
                        .putLearnResultRule("ff", 0, 0.25f, 1)
                        .putLearnResultRule("sd", 0, -0.2f, 1);
            }
            else if (key.Equals("10010286"))
            {
                this.init(2, "血魔缚命", "s4050");
                this.addDes("对带有回血buff的目标伤害增加60%");//60
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010287"))
            {
                this.init(2, "疾电奔雷", "s4050");
                this.addDes("提高15%的暴击和10%的速度");//15 10
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.15f, 1)
                        .putLearnResultRule("css", 0, 0.1f, 1);
            }
            else if (key.Equals("10010288"))
            {
                this.init(2, "狂乱爆斩", "s4050");
                this.addDes("暴击后伤害增加20%（2回合有效）");//20
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010289"))
            {
                this.init(2, "飞影神速", "s4050");
                this.addDes("提高25%的手速跟25%的命中，同时防御降低35%，闪避降低30%");//25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("css", 0, 0.25f, 1)
                        .putLearnResultRule("mz", 0, 0.25f, 1)
                        .putLearnResultRule("wf", 0, -0.35f, 1)
                        .putLearnResultRule("ff", 0, -0.35f, 1)
                        .putLearnResultRule("sd", 0, -0.3f, 1);
            }
            else if (key.Equals("10010290"))
            {
                this.init(2, "天生体魄", "s4050");
                this.addDes("提高20%的生命上限，同时降低20%的攻击");//25
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.2f, 1)
                        .putLearnResultRule("wg", 0, -0.2f, 1)
                        .putLearnResultRule("fg", 0, -0.2f, 1);
            }
            else if (key.Equals("10010291"))
            {
                this.init(2, "逐风残影", "s4050");
                this.addDes("增加50%的闪避");//50
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, 0.5f, 1);
            }
            else if (key.Equals("10010292"))
            {
                this.init(2, "仙术回血", "s4050");
                this.addDes("每回合恢复最大生命10%的血量");//10
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010293"))
            {
                this.init(2, "浴火重生", "s4050");
                this.addDes("死亡后有50%几率以100%生命复活，一场战斗仅一次");//100
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010294"))
            {
                this.init(2, "真魔吸血", "s4050");
                this.addDes("以100%的攻击对目标造成伤害，并吸取该伤害的100%恢复血量（冷却1回合）");//100
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0f, 2f).setIsNear(true);
            }
            else if (key.Equals("10010295"))
            {
                this.init(2, "坚壁石肤", "s4050");
                this.addDes("防御提升20%");//20
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wf", 0, 0.2f, 1)
                        .putLearnResultRule("ff", 0, 0.2f, 1);
            }
            else if (key.Equals("10010296"))
            {
                this.init(2, "仙灵命术", "s4050");
                this.addDes("提升10%生命上限");//10
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.1f, 1);
            }
            else if (key.Equals("10010297"))
            {
                this.init(2, "裂骨鬼刃", "s4050");//裂骨鬼刃
                this.addDes("对敌方造成攻击伤害时，对方每回合以该伤害的15%持续掉血（持续2回合）");//15
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010298"))
            {
                this.init(2, "强势反击", "s4050");
                this.addDes("被攻击时有30%几率反击对方");//30
                this.bindSkillData(5, 1, 0, "*");
            }
            //todo:
            else if (key.Equals("10010299"))
            {
                this.init(2, "天赐木灵", "s4050");
                this.addDes("增加20%生命，同时被治疗效果增加20%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, 0.2f, 1);
            }
            else if (key.Equals("10010300"))
            {
                this.init(2, "天赐火灵", "s4050");
                this.addDes("增加20%攻击，同时造成伤害后攻击提升20%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.2f, 1)
                        .putLearnResultRule("fg", 0, 0.2f, 1);
            }
            else if (key.Equals("10010301"))
            {
                this.init(2, "天赐土灵", "s4050");
                this.addDes("增加20%防御，同时受到伤害时防御增加20%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wf", 0, 0.2f, 1)
                        .putLearnResultRule("ff", 0, 0.2f, 1);
            }
            else if (key.Equals("10010302"))
            {
                this.init(2, "天赐金灵", "s4050");
                this.addDes("增加20%闪避，同时造成伤害后降低对方20%攻击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("sd", 0, 0.2f, 1);
            }
            else if (key.Equals("10010303"))
            {
                this.init(2, "天赐水灵", "s4050");
                this.addDes("增加20%暴击，同时暴击后暴击提升20%");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.2f, 1);
            }
            else if (key.Equals("10010304"))
            {
                this.init(2, "天生迅捷", "s4050");
                this.addDes("增加25%出手速，降低40%防御");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("css", 0, 0.25f, 1)
                        .putLearnResultRule("wf", 0, -0.4f, 1)
                        .putLearnResultRule("ff", 0, -0.4f, 1);
            }
            else if (key.Equals("10010305"))
            {
                this.init(2, "天生厚甲", "s4050");
                this.addDes("增加25%防御，降低10%生命");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("max_xue", 0, -0.1f, 1)
                        .putLearnResultRule("wf", 0, 0.25f, 1)
                        .putLearnResultRule("ff", 0, 0.25f, 1);
            }
            else if (key.Equals("10010306"))
            {
                this.init(2, "天生鲁莽", "s4050");
                this.addDes("增加20%暴击，降低20%命中");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.2f, 1)
                        .putLearnResultRule("mz", 0, -0.2f, 1);
            }
            else if (key.Equals("10010307"))
            {
                this.init(2, "妙法灵噬", "s4050");
                this.addDes("以攻击的80%燃烧敌方魔法，冷却2回合");
                this.bindSkillData(5, 0, 2, "*").bindCoolingRule(0, 3);
            }
            else if (key.Equals("10010308"))
            {
                this.init(2, "天生强攻", "s4050");
                this.addDes("增加20%攻击，降低15%生命");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.2f, 1)
                        .putLearnResultRule("fg", 0, 0.2f, 1)
                        .putLearnResultRule("max_xue", 0, -0.15f, 1);
            }
            else if (key.Equals("10010309"))
            {
                this.init(2, "真魔暴怒", "s4050");
                this.addDes("增加8%暴击，造成伤害后增加5%暴击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("bj", 0, 0.08f, 1);
            }
            else if (key.Equals("10010310"))
            {
                this.init(2, "真魔嗜血", "s4050");
                this.addDes("增加8%攻击，造成伤害后增加5%攻击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.08f, 1)
                        .putLearnResultRule("fg", 0, 0.08f, 1);
            }
            else if (key.Equals("10010311"))
            {
                this.init(2, "铜皮铁骨", "s4050");
                this.addDes("受到的伤害降低10%");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010312"))
            {
                this.init(2, "裂空锐爪", "s4050");
                this.addDes("增加10%攻击");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("wg", 0, 0.1f, 1)
                        .putLearnResultRule("fg", 0, 0.1f, 1);
            }
            else if (key.Equals("10010313"))
            {
                this.init(2, "神目精准", "s4050");
                this.addDes("增加10%命中");
                this.bindSkillData(5, 1, 0, "*").putLearnResultRule("mz", 0, 0.1f, 1);
            }
            //====怪物技能====
            else if (key.Equals("10010314"))
            {
                this.init(2, "魔神降临", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010315"))
            {
                this.init(2, "天下无双", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010316"))
            {
                this.init(2, "永恒背刺", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010317"))
            {
                this.init(2, "风雷之力", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010318"))
            {
                this.init(2, "魔神之力", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010319"))
            {
                this.init(2, "凶残", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010320"))
            {
                this.init(2, "免伤", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010321"))
            {
                this.init(2, "神避", "s4050");
                this.addDes("");
                this.bindSkillData(5, 1, 0, "*");
            }
            else if (key.Equals("10010322"))
            {
                this.init(2, "奇术定身", "s4050");
                this.addDes("40%几率使目标定身，持续1回合");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010323"))
            {
                this.init(2, "奇术混乱", "s4050");
                this.addDes("40%几率使目标混乱，持续1回合");
                this.bindSkillData(5, 0, 2, "*");
            }
            else if (key.Equals("10010324"))
            {
                this.init(2, "奇术睡眠", "s4050");
                this.addDes("40%几率使目标睡眠，持续1回合");
                this.bindSkillData(5, 0, 2, "*");
            }
            //狐狸
            else if (key.Equals("10010325"))//
            {
                this.init(2, "灵风咒", "s4050");
                this.addDes("以180%的攻击对目标造成伤害，命中目标后目标闪避将降低20%，有效两回合");
                this.bindSkillData(4, 0, 2, "1220");
            }
            else if (key.Equals("10010326"))
            {
                this.init(2, "幻影", "s4050");
                this.addDes("减少40%受伤");
                this.bindSkillData(4, 1, 0, "1220");
            }
            else if (key.Equals("10010327"))
            {
                this.init(2, "灵动", "s4050");
                this.addDes("为所有队友增加10%闪避，叠加3次，两回合有效");
                this.bindSkillData(4, 0, 1, "1220").putLearnResultRule("sd", 0, 0.15f, 1);
            }
            else if (key.Equals("10010328"))
            {
                this.init(2, "幻身", "s4050");
                this.addDes("自身每次闪避法攻增加20%，拥有此技能闪避增加20%");
                this.bindSkillData(4, 1, 0, "1220");
                this.putLearnResultRule("sd", 0, 0.2f, 1);
            }
            else if (key.Equals("10010329"))
            {
                this.init(2, "破天一剑", "s4050");
                this.addDes("对援护的目标造成3倍伤害");
                this.bindSkillData(4, 0, 2, "1222");
            }
            else
            {
                Debug.Log("未定义技能");
                this.init(2, "", "s4050");
                this.addDes("未定义");
            }
            this.addBuyRule(0, 1);
        }
    }
    public class AmMatchEffct
    {
        public string key;
        //类型 0静止特效 1移动特效
        public int type;
        //0起始点 1打击点 （动画skill就播放起始特效，attack播放打击特效）
        public int point;
        //特效所在目标的位置 起始特效一般在中心（+1），打击特效在脚底（0）
        public float dy;
        public AmMatchEffct(string key, int type, int point, float dy)
        {
            this.key = key;
            this.type = type;
            this.point = point;
            this.dy = dy;
        }
    }
    class coolingRule : fixedRule
    {
        public coolingRule(float k, float b) : base(k, b)
        {
        }

    }
    class triggerCondition
    {
        public fixedRule mp;
        public fixedRule hp;
        public triggerCondition(float k0, float b0, float k1, float b1)
        {
            this.hp = new fixedRule(k0, b0);
            this.mp = new fixedRule(k1, b1);
        }
        public float getMpValue(int lv)
        {
            return this.mp.k * lv + this.mp.b;
        }
        public float getHpValue(int lv)
        {
            return this.hp.k * lv + this.hp.b;
        }
    }
    class learnResultRule
    {
        //属性名数组{name, k, b, valueType}
        public JArray attrs;
        public learnResultRule(JArray attrs)
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
    class leverRule
    {
        //技能最大等级
        public int maxLv;
        public fixedRule expRule;
        public fixedRule ypRule;
        public fixedRule lvRule;
        public fixedRule numRule;
        public leverRule(int maxLv)
        {
            this.maxLv = maxLv;
        }
        /**当需要数量作为升级条件时绑定 */
        public leverRule bindNumRule(float k, float b)
        {
            this.numRule = new fixedRule(k, b);

            return this;
        }
        public leverRule bindExpRule(float k, float b)
        {
            this.expRule = new fixedRule(k, b);

            return this;
        }
        public leverRule bindYpRule(float k, float b)
        {
            this.ypRule = new fixedRule(k, b);

            return this;
        }
        public leverRule bindLvRule(float k, float b)
        {
            this.lvRule = new fixedRule(k, b);

            return this;
        }
        public JObject getCount(int lv)
        {
            JObject obj = new JObject();
            obj.Add("exp", this.expRule.k * lv + this.expRule.b);
            obj.Add("yp", this.ypRule.k * lv + this.ypRule.b);
            obj.Add("lv", this.lvRule.k * lv + this.lvRule.b);
            return obj;
        }
        /**是否达到升级条件 (门派)
         * lv为技能将要提升的等级
        */
        public bool isAllowedLvMP(int lv)
        {
            JObject r = face.roleInterface.getRole();
            JObject condition = this.getCount(lv);
            bool b = true;
            if ((int)condition["exp"] > (int)r["attr"]["prop"]["exp"] ||
                (int)condition["lv"] > (int)r["lever"] ||
                (int)condition["yp"] > (int)r["attr"]["msg"]["yp"])
            {
                b = false;
            }
            return b;
        }
        /**是否达到条件（消耗技能卷轴数量） */
        public bool isAllowedLvByNum(int lv, int num)
        {
            if (this.getNum(lv) > num)
            {
                return false;
            }
            return true;
        }
        public int getNum(int lv)
        {
            return (int)(this.numRule.k * lv + this.numRule.b);
        }
        /**是否达到最大等级 */
        public bool isMaxLv(int lv)
        {
            return this.maxLv < lv;
        }
    }

    class targetNumRule : fixedRule
    {
        public targetNumRule(float k, float b) : base(k, b)
        {

        }
        public int getNum(int lv)
        {
            int num = (int)(this.k * lv + this.b);
            if (num > 6) num = 6;
            return num;
        }
    }
    public class fixedRule
    {
        public float k;
        public float b;
        //值类型 0固定值 1百分比
        public int valueType;

        public fixedRule(float k, float b)
        {
            this.k = k;
            this.b = b;
        }

        public fixedRule(float k, float b, int valueType) : this(k, b)
        {
            this.valueType = valueType;
        }

        public float getV(float x)
        {
            return this.k * x + this.b;

        }
        /**求n项和*/
        public float getNItemSum(int x)
        {
            float sum = 0;
            for (int i = 1; i <= x; i++)
            {
                sum += (this.k * i + this.b);
            }
            return sum;
        }
    }
    class aToBAttr
    {
        public string aKey;
        public float k;
        public string bKey;

        public aToBAttr(string aKey, float k, string bKey)
        {
            this.aKey = aKey;
            this.k = k;
            this.bKey = bKey;
        }
    }
    class amObj
    {
        public string otherKey;
        public string boomKey;
        public string lzYcKey;//吟唱
        public string lzBoomKey;
        //技能移动过程中粒子key 为null就是不需要
        public string moveLzKey;


        public amObj(string otherKey, string boomKey, string lzYcKey, string lzBoomKey, string moveLzKey)
        {
            this.otherKey = otherKey;
            this.lzYcKey = lzYcKey;
            this.boomKey = boomKey;
            this.lzBoomKey = lzBoomKey;
            this.moveLzKey = moveLzKey;
        }
    }


}
