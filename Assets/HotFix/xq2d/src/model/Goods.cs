using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    /**可在背包中直接使用的道具*/
    public class Goods : GoodsDes
    {
        public Goods(string key)
        {
            this.key = key;
        }
        public Goods getMsg()
        {
            if (this.key.Equals("10000000"))
            {
                this.init(0, "龙头金票", "prop_1000").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后可获得1000元宝");
                this.addBuyRule(0, 1000);
            }
            else if (this.key.Equals("10000001"))
            {
                this.init(0, "龙头银票", "prop_1001").addQuality(1).addIsAllowdUse(true)
                    .addDes("使用后可获得500元宝");
                this.addBuyRule(0, 500);
            }
            else if (this.key.Equals("10000002"))
            {
                this.init(0, "龙头小票", "prop_1002").addQuality(0).addIsAllowdUse(true)
                    .addDes("使用后可获得100元宝");
                this.addBuyRule(0, 100);
            }
            
            else if (this.key.Equals("10000004"))
            {
                this.init(0, "宠物更名卡", "26058").addQuality(2)
                    .addDes("更改宠物名称所需的道具")
                    .addUseMsg("在【宠物】-【改名】中使用");
                this.addBuyRule(1, 1000);
            }
            


            
            else if (this.key.Equals("10000029"))
            {
                this.init(0, "初级宠物口粮", "26391").addQuality(0).addIsAllowdUse(true)
                    .addDes("使用后为指定宠物增加1000经验")
                    .addUseMsg("在【宠物】界面使用");
                this.addBuyRule(1, 2000);
            }
            else if (this.key.Equals("10000030"))
            {
                this.init(0, "中级宠物口粮", "26392").addQuality(1).addIsAllowdUse(true)
                    .addDes("使用后为指定宠物增加10000经验")
                    .addUseMsg("在【宠物】界面使用");
                this.addBuyRule(1, 20000);
            }
            else if (this.key.Equals("10000031"))
            {
                this.init(0, "高级宠物口粮", "26393").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后为指定宠物增加50000经验")
                    .addUseMsg("在【宠物】界面使用");
                this.addBuyRule(0, 100);
            }
            else if (this.key.Equals("10000032"))
            {
                this.init(0, "超级宠物口粮", "26394").addQuality(3).addIsAllowdUse(true)
                    .addDes("使用后为指定宠物增加200000经验")
                    .addUseMsg("在【宠物】界面使用");
                this.addBuyRule(0, 400);
            }
            
            else if (this.key.Equals("10000053"))
            {
                this.init(0, "异兽之魂", "27187").addQuality(0)
                    .addDes("异兽遗落世间精魂，收集99个异兽之魂可至神兽使者处兑换异兽")
                    .addUseMsg("唤灵");
                
            }
            else if (this.key.Equals("10000054"))
            {
                this.init(0, "神兽之魂", "27187").addQuality(0)
                    .addDes("神兽遗落世间精魂，收集99个神兽之魂可至神兽使者处兑换神兽，或于限时运营活动兑换神兽")
                    .addUseMsg("神兽碎片");

            }
            
            else if (this.key.Equals("10000093"))
            {
                this.init(0, "经验", "65535").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000094"))
            {
                this.init(0, "元宝", "65534").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000095"))
            {
                this.init(0, "银两", "yinliang").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000096"))
            {
                this.init(0, "银票", "65532").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000097"))
            {
                this.init(0, "积分", "65537").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000098"))
            {
                this.init(0, "武勋", "27540").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000099"))
            {
                this.init(0, "帮贡", "65536").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000100"))
            {
                this.init(0, "修炼点", "27575").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000101"))
            {
                this.init(0, "帮币", "27842").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000102"))
            {
                this.init(0, "经脉点数", "65607").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000103"))
            {
                this.init(0, "领地积分", "56447").addQuality(0)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000104"))
            {
                this.init(0, "初锻宝石", "26459").addQuality(0)
                    .addDes("有着荧荧幽光的灵石，在黑暗中散发着煞气，是强化装备的材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(11, 30);
            }
            else if (this.key.Equals("10000105"))
            {
                this.init(0, "锻造宝石", "26459").addQuality(1)
                    .addDes("有着荧荧幽光的灵石，在黑暗中散发着煞气，是强化装备的材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 80).addBuyRule(3, 120).addBuyRule(5, 420).addBuyRule(11, 90);
            }
            else if (this.key.Equals("10000106"))
            {
                this.init(0, "精锻宝石", "26459").addQuality(2)
                    .addDes("有着荧荧幽光的灵石，在黑暗中散发着煞气，是强化装备的材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(11, 270);
            }
            else if (this.key.Equals("10000107"))
            {
                this.init(0, "锻皇宝石", "26459").addQuality(3)
                    .addDes("有着荧荧幽光的灵石，在黑暗中散发着煞气，是强化装备的材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 500).addBuyRule(3, 750).addBuyRule(11, 810);
            }
            else if (this.key.Equals("10000108"))
            {
                this.init(0, "圣锻宝石", "26459").addQuality(4)
                    .addDes("有着荧荧幽光的灵石，在黑暗中散发着煞气，是强化装备的材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(11, 6000);
            }
            else if (this.key.Equals("10000109"))
            {
                this.init(0, "精炼宝石", "27322").addQuality(2)
                    .addDes("精炼装备的材料")
                    .addUseMsg("在【装备】-【精炼】界面使用");
                this.addBuyRule(0, 100).addBuyRule(3, 75);
            }
            else if (this.key.Equals("10000110"))
            {
                this.init(0, "特效石", "27850").addQuality(2)
                    .addDes("能给装备附加特殊力量的道具，使用后能给装备刷新特效")
                    .addUseMsg("在【装备】-【特效】界面使用");
            }
            else if (this.key.Equals("10000111"))
            {
                this.init(0, "修复宝石", "26269").addQuality(2)
                    .addDes("能给装备修复损坏的道具")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 300).addBuyRule(3, 450);
            }
            else if (this.key.Equals("10000112"))
            {
                this.init(0, "仙决宝箱", "26369").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后获得一本仙决（无天地仙绝，仅人技）")
                    .addUseMsg("");
                this.addBuyRule(0, 4000);
            }
            else if (this.key.Equals("10000113"))
            {
                this.init(0, "资质重置丹", "26402").addQuality(2)
                    .addDes("重置宠物的资质")
                    .addUseMsg("在【宠物】-【资质】界面使用");
                this.addBuyRule(0, 800);
            }
            else if (this.key.Equals("10000114"))
            {
                this.init(0, "潜力符石", "27850").addQuality(2)
                    .addDes("注入装备，增加装备潜力")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 250).addBuyRule(3, 375);
            }
            else if (this.key.Equals("10000115"))
            {
                this.init(0, "打孔器", "56012").addQuality(2)
                    .addDes("增加装备的镶嵌孔")
                    .addUseMsg("在【装备】-【镶嵌】界面使用");
                this.addBuyRule(0, 1500).addBuyRule(3, 2250);
            }
            /*else if (this.key.Equals("10000116"))
            {
                this.init(0, "轻锻宝石", "26414").addQuality(3)
                    .addDes("是强化+15装备的必要材料，成功升1级，失败则掉1级，装备不会损毁")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 18000);
            }*/
            /*else if (this.key.Equals("10000117"))
            {
                this.init(0, "金锻皇", "26413").addQuality(2)
                    .addDes("是强化+15装备的必要材料")
                    .addUseMsg("在【装备】-【强化】界面使用");
                this.addBuyRule(0, 2000);
            }*/
            else if (this.key.Equals("10000118"))
            {
                this.init(0, "仙灵悟性丹", "27600").addQuality(2)
                    .addDes("使用可提升宠物1-3点技能悟性，但对悟性大于150的毫无作用可言")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000119"))
            {
                this.init(0, "神通悟性丹", "27600").addQuality(3)
                    .addDes("使用可提升宠物2-4点技能悟性")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 500);
            }
            else if (this.key.Equals("10000120"))
            {
                this.init(0, "宠物回天书", "27726").addQuality(2)
                    .addDes("学习新技能不满意当前覆盖的技能位置时，使用该道具可取消学习")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000121"))
            {
                this.init(0, "宠物进化石", "27726").addQuality(3)
                    .addDes("宠物成长等级达到8时使用")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 2000).addBuyRule(11, 3000);
            }
            else if (this.key.Equals("10000122"))
            {
                this.init(0, "人参果", "56004").addQuality(2)
                    .addDes("宠物成长等级达到6时使用")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 1000).addBuyRule(11, 3000);
            }
            else if (this.key.Equals("10000123"))
            {
                this.init(0, "宠物天启卷轴", "27612").addQuality(2)
                    .addDes("用于解锁宠物技能位置")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000124"))
            {
                this.init(0, "宠物重生丹", "27598").addQuality(2)
                    .addDes("使用后宠物所有属性会被重置")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000125"))
            {
                this.init(0, "聚灵石", "27803").addQuality(0).addIsAllowdUse(true)
                    .addDes("可在宠物乐园聚灵20分钟，使自己和出战的宠物获得经验（每日限使用3次）")
                    .addUseMsg("");
                this.addBuyRule(0, 20);
                this.addBuyRule(11, 30);
            }
            else if (this.key.Equals("10000126"))
            {
                this.init(0, "天元精髓", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("可在宠物乐园聚灵20分钟，使自己和出战的宠物获得大量经验（每日限使用3次）")
                    .addUseMsg("");
                this.addBuyRule(0, 430);
                this.addBuyRule(11, 810);
            }
            else if (this.key.Equals("10000127"))
            {
                this.init(0, "法宝强化符", "27612").addQuality(3)
                    .addDes("用于强化法宝")
                    .addUseMsg("");
                this.addBuyRule(0, 100).addBuyRule(11, 200);
            }
            else if (this.key.Equals("10000128"))
            {
                this.init(0, "角色回天书", "27726").addQuality(3)
                    .addDes("学习新技能不满意当前覆盖的技能位置时，使用该道具可取消学习")
                    .addUseMsg("在【角色】-【技能】界面使用");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000129"))
            {
                this.init(0, "易筋经", "27726").addQuality(3)
                    .addDes("重置经脉")
                    .addUseMsg("在【角色】-【技能】界面使用");
                this.addBuyRule(0, 1000);
            }
            else if (this.key.Equals("10000130"))
            {
                this.init(0, "明心悟性丹", "27600").addQuality(1)
                    .addDes("使用可提升宠物0-2点技能悟性，但对悟性大于100的毫无作用可言")
                    .addUseMsg("在【宠物】-【强化】界面使用");
                this.addBuyRule(1, 2500);
            }
            /*else if (this.key.Equals("10000131"))
            {
                this.init(0, "灵力丹", "27602").addQuality(1)
                    .addDes("合成紫云丹的道具")
                    .addUseMsg("");
                this.addBuyRule(0, 1000);
            }
            else if (this.key.Equals("10000132"))
            {
                this.init(0, "紫云丹", "27601").addQuality(2)
                    .addDes("使用后增加25经脉属性点")
                    .addUseMsg("");
            }*/
            else if (this.key.Equals("10000133"))
            {
                this.init(0, "魔龙召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 逆天魔龙")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000134"))
            {
                this.init(0, "玄姬召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 惑天玄姬")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000135"))
            {
                this.init(0, "小青召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 小青娘子")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000136"))
            {
                this.init(0, "玄女召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 九天玄女")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000137"))
            {
                this.init(0, "天兵召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 龙翔天兵")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000138"))
            {
                this.init(0, "帝君召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 龙翔帝君")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000139"))
            {
                this.init(0, "青童召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得神宠 青龙仙童")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000140"))
            {
                this.init(0, "中级宠物蛋", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("带着蓝色斑纹的神秘宠物蛋，拿在手里有种温热的感觉。")
                    .addUseMsg("");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000141"))
            {
                this.init(0, "高级宠物蛋", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("带着红色斑纹的神秘宠物蛋，拿在手里有种强大的感觉。")
                    .addUseMsg("");
                this.addBuyRule(0, 1000);
            }
            else if (this.key.Equals("10000142"))
            {
                this.init(0, "天狐召唤令", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("使用后获得稀宠 玄晶天狐")
                    .addUseMsg("");
                this.addBuyRule(0, 1);
            }
            else if (this.key.Equals("10000143"))
            {
                this.init(0, "供香", "27601").addQuality(3)
                    .addDes("用于祭拜魔神，可以提高魔神满意度")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000144"))
            {
                this.init(0, "白壁", "27601").addQuality(0)
                    .addDes("用于怀璧其罪活动")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000145"))
            {
                this.init(0, "蓝壁", "27601").addQuality(1)
                    .addDes("用于怀璧其罪活动")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000146"))
            {
                this.init(0, "紫壁", "27601").addQuality(2)
                    .addDes("用于怀璧其罪活动")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000147"))
            {
                this.init(0, "橙壁", "27601").addQuality(3)
                    .addDes("用于怀璧其罪活动")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000148"))
            {
                this.init(0, "圣锻碎片", "27601").addQuality(3)
                    .addDes("消耗20个可以合成一个圣锻宝石")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000149"))
            {
                this.init(0, "绑定宝石", "27601").addQuality(2)
                    .addDes("将装备绑定")
                    .addUseMsg("");
                this.addBuyRule(0, 250).addBuyRule(3, 375).addBuyRule(11, 800);
            }
            else if (this.key.Equals("10000150"))
            {
                this.init(0, "刻印宝石", "27601").addQuality(2)
                    .addDes("用于开启装备刻印")
                    .addUseMsg("");
                this.addBuyRule(0, 4000).addBuyRule(3, 6000).addBuyRule(11, 2500);
            }
            else if (this.key.Equals("10000151"))
            {
                this.init(0, "千年玄铁", "27601").addQuality(3)
                    .addDes("用于紫装升橙")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000152"))
            {
                this.init(0, "暴击抵抗玉石", "27601").addQuality(3)
                    .addDes("用于提升暴击抗性")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000153"))
            {
                this.init(0, "混乱抵抗玉石", "27601").addQuality(3)
                    .addDes("用于提升混乱抗性")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000154"))
            {
                this.init(0, "流血抵抗玉石", "27601").addQuality(3)
                    .addDes("用于提升流血抗性")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000155"))
            {
                this.init(0, "睡眠抵抗玉石", "27601").addQuality(3)
                    .addDes("用于提升睡眠抗性")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000156"))
            {
                this.init(0, "磨刀石", "27601").addQuality(1)
                    .addDes("提升橙装10点魔力")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000157"))
            {
                this.init(0, "名匠石磨", "27601").addQuality(2)
                    .addDes("提升橙装20点魔力")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000158"))
            {
                this.init(0, "百炼玄石", "27601").addQuality(3)
                    .addDes("提升橙装30点魔力")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000159"))
            {
                this.init(0, "金锻皇", "27601").addQuality(2)
                    .addDes("用于15星及以上的橙装锻造")
                    .addUseMsg("");
                this.addBuyRule(0, 2000);
            }
            else if (this.key.Equals("10000160"))
            {
                this.init(0, "轻锻宝石", "27601").addQuality(3)
                    .addDes("用于橙装锻造，失败则降1星而不会损坏")
                    .addUseMsg("");
                this.addBuyRule(0, 18000);
                this.addBuyRule(11, 8000);
            }
            else if (this.key.Equals("10000161"))
            {
                this.init(0, "血契之石", "27601").addQuality(3)
                    .addDes("用于装备血契")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000162"))
            {
                this.init(0, "仙人神水", "27601").addQuality(3)
                    .addDes("用于提升仙人模式")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
            }
            else if (this.key.Equals("10000163"))
            {
                this.init(0, "通灵神水", "27601").addQuality(3)
                    .addDes("用于提升仙法通灵")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
            }
            else if (this.key.Equals("10000164"))
            {
                this.init(0, "人物刻印卷轴", "27601").addQuality(3)
                    .addDes("用于开启仙诀的人物刻印")
                    .addUseMsg("");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000165"))
            {
                this.init(0, "技能遗忘卷轴", "27601").addQuality(2)
                    .addDes("用于遗忘仙诀、宠物技能")
                    .addUseMsg("");
                this.addBuyRule(0, 250);
            }
            else if (this.key.Equals("10000166"))
            {
                this.init(0, "法宝洗练符", "27601").addQuality(2)
                    .addDes("用于洗练法宝")
                    .addUseMsg("");
                this.addBuyRule(0, 50);
            }
            else if (this.key.Equals("10000167"))
            {
                this.init(0, "注灵珍露", "27601").addQuality(2)
                    .addDes("用于法宝注灵")
                    .addUseMsg("");
                this.addBuyRule(0, 250).addBuyRule(11, 400);
            }
            else if (this.key.Equals("10000168"))
            {
                this.init(0, "低级天命逆转散", "27601").addQuality(1)
                    .addDes("将宠物的1点天生属性转化为可分配点")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000169"))
            {
                this.init(0, "中级天命逆转散", "27601").addQuality(2)
                    .addDes("将宠物的2点天生属性转化为可分配点")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000170"))
            {
                this.init(0, "高级天命逆转散", "27601").addQuality(3)
                    .addDes("将宠物的3点天生属性转化为可分配点")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000171"))
            {
                this.init(0, "易经洗髓丹", "27601").addQuality(3)
                    .addDes("重置宠物属性点")
                    .addUseMsg("");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000172"))
            {
                this.init(0, "炼神木", "27601").addQuality(2)
                    .addDes("帮贡材料，提交后获得1点建设度和帮贡")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000173"))
            {
                this.init(0, "玄铁矿", "27601").addQuality(2)
                    .addDes("帮贡材料，提交后获得1点建设度和帮贡")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000174"))
            {
                this.init(0, "封赏玉帛", "27601").addQuality(2)
                    .addDes("用于举办封赏演武堂")
                    .addUseMsg("");
                this.addBuyRule(0, 500).addBuyRule(5, 1500);
            }
            else if (this.key.Equals("10000175"))
            {
                this.init(0, "汇通金券", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("可兑换10000银两")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000176"))
            {
                this.init(0, "汇通银券", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("可兑换1000银两")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000177"))
            {
                this.init(0, "造化丹", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时人物双倍经验")
                    .addUseMsg("");
                this.addBuyRule(0, 90).addBuyRule(11, 100);
            }
            else if (this.key.Equals("10000178"))
            {
                this.init(0, "宠物造化丹", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时宠物双倍经验")
                    .addUseMsg("");
                this.addBuyRule(0, 100).addBuyRule(5, 400);
            }
            else if (this.key.Equals("10000179"))
            {
                this.init(0, "太公鱼钩", "27601").addQuality(2)
                    .addDes("用于钓鱼")
                    .addUseMsg("");
                this.addBuyRule(0, 80).addBuyRule(5, 320);
            }
            else if (this.key.Equals("10000180"))
            {
                this.init(0, "碎玉帛", "27601").addQuality(2)
                    .addDes("用于封赏演武堂兑换道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000181"))
            {
                this.init(0, "化婴玉", "27803").addQuality(1).addIsAllowdUse(true)
                    .addDes("可在宠物乐园聚灵20分钟，使自己和出战的宠物获得比聚灵石更多的经验（每日限使用3次）")
                    .addUseMsg("");
                this.addBuyRule(11, 90);
            }
            else if (this.key.Equals("10000182"))
            {
                this.init(0, "炼神珠", "27802").addQuality(2).addIsAllowdUse(true)
                    .addDes("可在宠物乐园聚灵20分钟，使自己和出战的宠物获得比炼神珠更多的经验（每日限使用3次）")
                    .addUseMsg("");
                this.addBuyRule(11, 270);
            }
            else if (this.key.Equals("10000183"))
            {
                this.init(0, "驱敌香草", "27802").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时野外不遇怪")
                    .addUseMsg("");
                this.addBuyRule(0, 120).addBuyRule(11, 50);
            }
            else if (this.key.Equals("10000184"))
            {
                this.init(0, "诱敌香草", "27802").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时野外原地遇怪")
                    .addUseMsg("");
                this.addBuyRule(0, 120).addBuyRule(11, 200);
            }
            else if (this.key.Equals("10000185"))
            {
                this.init(0, "英雄擂台入场券", "27802").addQuality(2)
                    .addDes("用于参加英雄擂")
                    .addUseMsg("");
                this.addBuyRule(2, 200);
            }
            else if (this.key.Equals("10000186"))
            {
                this.init(0, "积德令", "27802").addQuality(2)
                    .addDes("每60个可获得1点人气值")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000187"))
            {
                this.init(0, "扬善令牌", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("可获得1点人气值")
                    .addUseMsg("");
                this.addBuyRule(0, 500);
            }
            else if (this.key.Equals("10000188"))
            {
                this.init(0, "黑洞陨石", "27802").addQuality(2)
                    .addDes("每100个可合成1个潜力符石")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000189"))
            {
                this.init(0, "福神的礼袋", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("使用后可获得神秘奖励")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000190"))
            {
                this.init(0, "天晶石", "27802").addQuality(1)
                    .addDes("兑换经验和道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000191"))
            {
                this.init(0, "玉魄石", "27802").addQuality(1)
                    .addDes("兑换经验和道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000192"))
            {
                this.init(0, "魔精", "27802").addQuality(2)
                    .addDes("兑换经验和道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000193"))
            {
                this.init(0, "补天玄石", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后有概率获得神宠天狐")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000194"))
            {
                this.init(0, "虚幻之石", "27802").addQuality(2)
                    .addDes("进入洪荒宝库所需道具")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
            }
            else if (this.key.Equals("10000195"))
            {
                this.init(0, "明心镜", "27802").addQuality(2)
                    .addDes("洪荒宝库看破生死门的道具，消除9个死门")
                    .addUseMsg("");
                this.addBuyRule(0, 100);
            }
            else if (this.key.Equals("10000196"))
            {
                this.init(0, "迷宫铁之钥匙", "27802").addQuality(1)
                    .addDes("用来打开洪荒宝库第二层宝箱")
                    .addUseMsg("");
                this.addBuyRule(0, 300);
            }
            else if (this.key.Equals("10000197"))
            {
                this.init(0, "迷宫铜之钥匙", "27802").addQuality(2)
                    .addDes("用来打开洪荒宝库第三层宝箱")
                    .addUseMsg("");
                this.addBuyRule(0, 1000);
            }
            else if (this.key.Equals("10000198"))
            {
                this.init(0, "再来一罐", "27802").addQuality(2)
                    .addDes("用来打开洪荒宝库第二层宝箱")
                    .addUseMsg("");
                this.addBuyRule(0, 300);
            }
            else if (this.key.Equals("10000199"))
            {
                this.init(0, "梦幻水晶", "27802").addQuality(1)
                    .addDes("用来兑换盗梦空间的奖励")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000200"))
            {
                this.init(0, "未定义物品", "27802").addQuality(1)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000201"))
            {
                this.init(0, "美人香", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("用于兽神祭，或者直接使用")
                    .addUseMsg("");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000202"))
            {
                this.init(0, "兽神大礼", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后能获得丰厚大奖")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000203"))
            {
                this.init(0, "仙人宝盒", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("概率获得初级经脉神符")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000204"))
            {
                this.init(0, "珍珠宝盒", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("概率获得玄女宝鉴")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000205"))
            {
                this.init(0, "机关宝盒", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("概率获得天兵帅符")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000206"))
            {
                this.init(0, "鬼刹宝盒", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("概率获得魔龙残影")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000207"))
            {
                this.init(0, "武圣宝盒", "27802").addQuality(3).addIsAllowdUse(true)
                    .addDes("概率获得玄姬冰雕")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000208"))
            {
                this.init(0, "百里香", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时野外每次遇怪都是10只")
                    .addUseMsg("");
                this.addBuyRule(0, 180);
            }
            else if (this.key.Equals("10000209"))
            {
                this.init(0, "聚魔铃", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时野外每次遇怪都是10只精英")
                    .addUseMsg("");
                this.addBuyRule(0, 460);
            }
            else if (this.key.Equals("10000210"))
            {
                this.init(0, "双倍潜力激发", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("持续一小时获得双倍练潜")
                    .addUseMsg("");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000211"))
            {
                this.init(0, "两仪八卦", "27601").addQuality(2)
                    .addDes("可到门主处兑换经验和银两")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000212"))
            {
                this.init(0, "修行药匣", "27601").addQuality(1)
                    .addDes("每5个可兑换一个[初]天元丹")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000213"))
            {
                this.init(0, "[初]天元丹", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("服用后可获得20点真元点，获得40点后无法继续使用")
                    .addUseMsg("");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000214"))
            {
                this.init(0, "[中]天元丹", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("服用后可获得40点真元点，获得200点后无法继续使用")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000215"))
            {
                this.init(0, "[高]天元丹", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("服用后可获得60点真元点，获得440点后无法继续使用")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000216"))
            {
                this.init(0, "[超]天元丹", "27601").addQuality(4).addIsAllowdUse(true)
                    .addDes("服用后可获得80点真元点，获得760点后无法继续使用")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000217"))
            {
                this.init(0, "[圣]天元丹", "27601").addQuality(5).addIsAllowdUse(true)
                    .addDes("服用后可获得100点真元点，获得1160点后无法继续使用")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000218"))
            {
                this.init(0, "锄头", "27601").addQuality(0)
                    .addDes("用于开垦农田")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000219"))
            {
                this.init(0, "玄铁残骸", "27601").addQuality(2)
                    .addDes("每5个兑换1玄铁")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000220"))
            {
                this.init(0, "银票", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("使用后可获得1000银票")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000221"))
            {
                /* this.init(0, "丹青", "27601").addQuality(0)
                     .addDes("用于制作各种神符")
                     .addUseMsg("");
                 this.addBuyRule(0, 200);*/
                return null;
            }
            else if (this.key.Equals("10000222"))
            {
                /*this.init(0, "羊毛笔", "27601").addQuality(0)
                    .addDes("用于制作各种神符")
                    .addUseMsg("");
                this.addBuyRule(0, 200);*/
                return null;
            }
            else if (this.key.Equals("10000223"))
            {
                this.init(0, "翡翠金叶", "27601").addQuality(2)
                    .addDes("用于贿赂监狱看守")
                    .addUseMsg("");
                this.addBuyRule(0, 500);
            }
            else if (this.key.Equals("10000224"))
            {
                this.init(0, "直捣黄龙", "27601").addQuality(2)
                    .addDes("用于斗战封神榜挑战")
                    .addUseMsg("");
                this.addBuyRule(0, 200);
            }
            else if (this.key.Equals("10000225"))
            {
                this.init(0, "基础出师礼", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000226"))
            {
                this.init(0, "普通出师礼", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000227"))
            {
                this.init(0, "高级出师礼", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000228"))
            {
                this.init(0, "特级出师礼", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000229"))
            {
                this.init(0, "基础严师礼", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000230"))
            {
                this.init(0, "普通严师礼", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000231"))
            {
                this.init(0, "高级严师礼", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000232"))
            {
                this.init(0, "特级严师礼", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后可获得一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000233"))
            {
                this.init(0, "封妖石", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可获得赤翼蝠")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000234"))
            {
                this.init(0, "元灵幽草", "27601").addQuality(0)
                    .addDes("宠物闯关活动获取，兑换宠物防具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000235"))
            {
                this.init(0, "元灵幽花", "27601").addQuality(0)
                    .addDes("宠物闯关活动获取，兑换宠物防具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000236"))
            {
                this.init(0, "名匠之魂", "27601").addQuality(2)
                    .addDes("可到英雄副本兑换相关道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000237"))
            {
                this.init(0, "暴击仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000238"))
            {
                this.init(0, "闪避仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000239"))
            {
                this.init(0, "命中仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000240"))
            {
                this.init(0, "法术仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000241"))
            {
                this.init(0, "生命仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000242"))
            {
                this.init(0, "法防仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000243"))
            {
                this.init(0, "物防仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000244"))
            {
                this.init(0, "法攻仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000245"))
            {
                this.init(0, "物攻仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000246"))
            {
                this.init(0, "速度仙丹礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得固元、元魂、仙元丹各x1")
                    .addUseMsg("");
            }

            else if (this.key.Equals("10000247"))
            {
                this.init(0, "暴击仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000248"))
            {
                this.init(0, "闪避仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000249"))
            {
                this.init(0, "命中仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000250"))
            {
                this.init(0, "法术仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000251"))
            {
                this.init(0, "生命仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000252"))
            {
                this.init(0, "法防仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000253"))
            {
                this.init(0, "物防仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000254"))
            {
                this.init(0, "法攻仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000255"))
            {
                this.init(0, "物攻仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000256"))
            {
                this.init(0, "速度仙丹礼包碎片", "27601").addQuality(3)
                    .addDes("满3个可兑换仙丹礼包")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000257"))
            {
                this.init(0, "梅老板礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得 锻造宝石x198、中级天元丹x18、人物刻印卷轴x36、修复宝石x398、供香x868（全绑定，活动期间限购一次）")
                    .addUseMsg("");
                this.addBuyRule(0, 120000);
            }
            else if (this.key.Equals("10000258"))
            {
                this.init(0, "鬼见愁礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得 高级宠物口粮x98、天元精髓x98、51-60级橙品套装（全绑定，需选择职业后方可使用，活动期间限购一次）")
                    .addUseMsg("");
                this.addBuyRule(0, 10000);
            }
            else if (this.key.Equals("10000259"))
            {
                this.init(0, "储钱罐", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得随机一份 50000、100000、150000银两")
                    .addUseMsg("");
                this.addBuyRule(0, 10000);
            }
            else if (this.key.Equals("10000260"))
            {
                this.init(0, "宠物神技礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得宠物神技一份")
                    .addUseMsg("");
                this.addBuyRule(0, 15000);
            }
            else if (this.key.Equals("10000261"))
            {
                this.init(0, "宠物高级技能礼包", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后获得宠物高级技能一份")
                    .addUseMsg("");
                this.addBuyRule(0, 5000);
            }
            else if (this.key.Equals("10000262"))
            {
                this.init(0, "宠物普通技能礼包", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后获得宠物普通技能一份")
                    .addUseMsg("");
                this.addBuyRule(0, 2000);
            }
            else if (this.key.Equals("10000263"))
            {
                this.init(0, "百变种子", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后获得高级种子一枚")
                    .addUseMsg("");
                this.addBuyRule(0, 20);
            }
            else if (this.key.Equals("10000264"))
            {
                this.init(0, "锻造礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得锻造宝石x100，锻皇x50，修复x200，银两一百万")
                    .addUseMsg("");
                this.addBuyRule(0, 80000);
            }
            else if (this.key.Equals("10000265"))
            {
                this.init(0, "魔龙礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得魔龙召唤令x1、自选神技礼盒x2、宠物高级技能礼包x4")
                    .addUseMsg("");
                this.addBuyRule(0, 100000);
            }
            else if (this.key.Equals("10000266"))
            {
                this.init(0, "自选神技礼盒", "27601").addQuality(3)
                    .addDes("于节日大使处选择一个神技兑换")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000267"))
            {
                this.init(0, "黑铁宝箱", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000268"))
            {
                this.init(0, "青铜宝箱", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000269"))
            {
                this.init(0, "白银宝箱", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000270"))
            {
                this.init(0, "黄金宝箱", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000271"))
            {
                this.init(0, "钻石宝箱", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000272"))
            {
                this.init(0, "v1礼包", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000273"))
            {
                this.init(0, "v2礼包", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000274"))
            {
                this.init(0, "v3礼包", "27601").addQuality(0).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000275"))
            {
                this.init(0, "v4礼包", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000276"))
            {
                this.init(0, "v5礼包", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000277"))
            {
                this.init(0, "v6礼包", "27601").addQuality(1).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000278"))
            {
                this.init(0, "v7礼包", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000279"))
            {
                this.init(0, "v8礼包", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000280"))
            {
                this.init(0, "v9礼包", "27601").addQuality(2).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000281"))
            {
                this.init(0, "v10礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后可得到一些道具")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000282"))
            {
                this.init(0, "人物技能箱", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("随机一本人技")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000283"))
            {
                this.init(0, "宠物物技能箱", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("随机一本宠技")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000284"))
            {
                this.init(0, "魔龙残影", "27601").addQuality(2)
                    .addDes("魔龙残影需另加龙头金票x20在收藏家处兑换逆天魔龙一只")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000285"))
            {
                this.init(0, "修复礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得修复宝石x398")
                    .addUseMsg("");
                this.addBuyRule(0, 50000);
            }
            else if (this.key.Equals("10000286"))
            {
                this.init(0, "供香礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后获得供香x868")
                    .addUseMsg("");
                this.addBuyRule(0, 50000);
            }
            else if (this.key.Equals("10000287"))
            {
                this.init(0, "神兽碎片", "27601").addQuality(3)
                    .addDes("满100个可自选兑换一神宠")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000288"))
            {
                this.init(0, "强装礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000289"))
            {
                this.init(0, "仙决碎片", "27601").addQuality(3)
                    .addDes("满50个可兑换仙决宝箱")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000290"))
            {
                this.init(0, "天地技碎片", "27601").addQuality(3)
                    .addDes("满50个可随机开一本天地技能")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000291"))
            {
                this.init(0, "宠物技能碎片", "27601").addQuality(3)
                    .addDes("满50个可随机开一本宠物技能")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000292"))
            {
                this.init(0, "天地仙决箱", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("打开后随机一本天地仙决")
                    .addUseMsg("");
                this.addBuyRule(0, 4000);
            }
            else if (this.key.Equals("10000293"))
            {
                this.init(0, "神影自选", "27601").addQuality(3)
                    .addDes("（不倦先生处）魔龙残影、玄姬冰雕、玄女宝鉴、天兵帅符、兽神残影，自选其一")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000294"))
            {
                this.init(0, "嘎嘎香礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("有几率开出神影自选、宠物神技礼包、宠物高级技能礼包、修复、供香等")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000295"))
            {
                this.init(0, "玄姬冰雕", "27601").addQuality(3)
                    .addDes("需另加龙头金票x20在收藏家处兑换惑天玄姬一只")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000296"))
            {
                this.init(0, "玄女宝鉴", "27601").addQuality(3)
                    .addDes("需另加龙头金票x20在收藏家处兑换玄女宝鉴一只")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000297"))
            {
                this.init(0, "天兵帅符", "27601").addQuality(3)
                    .addDes("需另加龙头金票x20在收藏家处兑换龙翔天兵一只")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000298"))
            {
                this.init(0, "兽神残影", "27601").addQuality(3)
                    .addDes("需另加龙头金票x20在收藏家处兑换玄晶天狐一只")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000299"))
            {
                this.init(0, "童帝二选一", "27601").addQuality(3)
                    .addDes("童帝二选一（不倦先生处）")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000300"))
            {
                this.init(0, "神宠五选一", "27601").addQuality(3)
                    .addDes("神宠五选一（不倦先生处）")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000301"))
            {
                this.init(0, "仙元丹自选", "27601").addQuality(3)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000302"))
            {
                this.init(0, "小黄毛礼包", "27601").addQuality(3).addIsAllowdUse(true)
                    .addDes("")
                    .addUseMsg("");
            }
            else if (this.key.Equals("10000303"))
            {
                this.init(0, "高级宠技自选", "27601").addQuality(3)
                    .addDes("")
                    .addUseMsg("");
            }
            /*else if (this.key.Equals("10000304"))
            {
                this.init(0, "血腥徽章", "27802").addQuality(2)
                    .addDes("用来兑换血腥之地的奖励")
                    .addUseMsg("");
            }*/







            else return null;

            return this;
        }

    }
}
