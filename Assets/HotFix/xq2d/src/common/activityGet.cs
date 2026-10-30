using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.activityPage;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.duanzao;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    public class activityGet
    {
        public static void xxzdJfOrder()
        {
            PageUI.createAcPage<XxzdShopPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(0);
        }
        public static void xianYuanDanExchange(int type)
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("兑换需要消耗仙元丹自选x1，是否继续？", () =>
            {
                face.activityInterface.xianYuanDanExchange(type, () => { });
            }, () => { });
        }
        public static void gjPetSklExchange()
        {
            List<string> m2 = new List<string>() {
                    "100210010265", "100210010266", "100210010267", "100210010268", "100210010269",
                "100210010270", "100210010271", "100210010272", "100210010273", "100210010276",
                "100210010277", "100210010278", "100210010279", "100210010280", "100210010281",
                "100210010282", "100210010283", "100210010284", "100210010287",
                "100210010288", "100210010289", "100210010291", "100210010294",
                "100210010295", "100210010296", "100210010298", "100210010308",
                "100210010309", "100210010310", "100210010311", "100210010312", "100210010313",
                "100210010180","100210010183",
            };
            List<string> ml = new List<string>();
            for (int i = 0; i < m2.Count; i++)
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[i]);
                ml.Add(skl.name);
            }
            Menu menu = Menu.create(ml, PointGet.getIndexPage());
            menu.addCallback((mIndex) =>
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[mIndex]);
                
                MsgSureUI.create(PointGet.getTipCanvas()).show("兑换"+skl.name+ "，需要消耗高级宠技自选x1，是否继续？" , () =>
                {
                    face.activityInterface.gjPetSklExchange(m2[mIndex], () => { });
                }, () => { });
            });
        }
        public static void qdGainYuanBao(int type)
        {
            if (type == 1)//领取神
            {
                face.activityInterface.yxfGainShen(() => { });
            }
            else if (type == 2)//领取童
            {
                face.activityInterface.yxfGainTong(() => { });
            }
            else if (type == 3)//领取元宝
            {
                face.activityInterface.qdGainYuanBao(() => { });
            }
        }
        public static void shenYingExchange(int type)
        {
            string dhKey = null;
            string[] arr = { "10000284", "10000295", "10000296", "10000297", "10000298" };
            dhKey = arr[type];
            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(dhKey);
            //碎片兑换技能
            string str = "兑换" + gd.name + "需消耗神影自选x1，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.shenYingExchange(dhKey, () => { });
            }, () => { });
        }
        public static void sklSpExchange(int type)
        {
            string xhKey = null;
            if (type == 1) xhKey = "10000289";
            else if (type == 2) xhKey = "10000290";
            else if (type == 3) xhKey = "10000291";
            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(xhKey);
            //碎片兑换技能
            string str = "需消耗" + gd.name + "碎片x50，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.sklSpExchange(xhKey, () => { });
            }, () => { });
        }
        public static void exchangByMlcy()
        {
            string str = "需消耗魔龙残影x1兑换名匠之魂1000，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.exchangByMlcy(() => { });
            }, () => { });
        }
        public static void exchangePetEquip(int index)
        {
            int xhNum = 1;
            string xhKey = null;
            string dhKey = null;
            if (index == 0)
            {
                xhNum = 10; xhKey = "10000234"; dhKey = "10160000";
            }
            else if (index == 1)
            {
                xhNum = 25; xhKey = "10000234"; dhKey = "10160001";
            }
            else if (index == 2)
            {
                xhNum = 12; xhKey = "10000235"; dhKey = "10160002";
            }
            else if (index == 3)
            {
                xhNum = 20; xhKey = "10000235"; dhKey = "10160003";
            }
            GoodsDes xhGd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(xhKey);
            GoodsDes dhGd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(dhKey);
            string str = "需消耗" + xhGd.name + "x" + xhNum + "兑换" + dhGd.name + "x1，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.exchangePetEquip(dhKey, () => { });
            }, () => { });
        }
        public static void exchangeQNXT()
        {
            string str = "需消耗玄铁残骸x5，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.exchangeQNXT(() => { });
            }, () => { });
        }
        public static void exchangeFbGd(int index)
        {
            face.activityInterface.exchangeFbGd(index, () => { });
        }
        public static void gainHolidayGift()
        {
            face.activityInterface.gainHolidayGift(() => { });
        }
        public static void getMenPaiTask(string npcKey)
        {
            face.activityInterface.getMenPaiTask(npcKey, () => { });
        }
        public static void exchangeQlfs()
        {
            string str = "即将兑换潜力符石x1，需消耗黑洞陨石x100，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.exchangeQlfs(() => { });
            }, () => { });
        }
        public static void exchangeShenShou(int type)
        {
            string name = null;
            string xhName = null;
            if (type == 0) { name = "魔龙召唤令"; xhName = "魔龙残影"; }
            else if (type == 1) { name = "玄姬召唤令"; xhName = "玄姬冰雕"; }
            else if (type == 2) { name = "玄女召唤令"; xhName = "玄女宝鉴"; }
            else if (type == 3) { name = "天兵召唤令"; xhName = "天兵帅符"; }
            else if (type == 4) { name = "天狐召唤令"; xhName = "兽神残影"; }

            string str = "即将兑换" + name + "需消耗" + xhName + "x1和龙头金票x20，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.exchangeShenShou(type, () => { });
            }, () => { });
        }
        public static void gainLjdGoldGift()
        {
            face.activityInterface.gainLjdGoldGift(() => { });
        }
        public static void gainLjdXianJueGift()
        {
            face.activityInterface.gainLjdXianJueGift(() => { });
        }
        public static void gainLeiJiDangGift()
        {
            face.activityInterface.gainLeiJiDangGift(() => { });
        }
        public static void gainVipLvGift(int lv)
        {
            face.activityInterface.gainVipLvGift(lv, () => { });
        }
        public static void chooseSkillGain(int index)
        {
            string[] arr = {
                "100210010286", "100210010292", "100210010293",
                "100210010297", "100210010274", "100210010275",
                "100210010299", "100210010300",
                "100210010301", "100210010302", "100210010303",
             };
            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(arr[index]);
            string str = "即将兑换" + gd.name + "，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.chooseSkillGain(index, () => { });
            }, () => { });


        }
        /**等级礼包*/
        public static void gainOpenServerLvGift(int type)
        {
            face.activityInterface.getLvGift(type, () => { });
        }
        public static void gainOpenServerZhuLiGift()
        {
            face.activityInterface.getQianDaoGift(() => { });
        }
        public static void aboutOpenServerAc()
        {
            string str = "2月12-2月18 23:50 " +
                "冲刺等级排名活动，活动结束后按等级排名发放奖励," +
                "第一名 刻印宝石x1、仙决宝箱x1、龙头金票x5、高级宠物蛋x1," +
                "第二名 人物刻印卷轴x5、龙头金票x2、高级宠物蛋x1," +
                "第三名 人物刻印卷轴x2、龙头金票x2、高级宠物蛋x1," +
                "前10名 锻造宝石x20、龙头银票x1," +
                "前50名 初锻x20、龙头小票x1,"
                ;
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {


            }, () => { });
        }
        /**传送到门派*/
        public static void toMpMap(string npcKey)
        {
            string mapKey = null;
            if (npcKey.Equals("10000009")) mapKey = "txzf";
            else if (npcKey.Equals("10000007")) mapKey = "txzl";
            else if (npcKey.Equals("10000008")) mapKey = "txzg";
            toMap(mapKey);
        }
        public static void createBp()
        {
            UseNumInputUI.create(PointGet.getIndexPageOfPage()).renderText("请输入帮派名").addTextCallback((bpName) =>
            {
                face.gangsInterface.create(bpName, () => { });
            });
        }
        /**解散帮派*/
        public static void bpJieSan()
        {
            string str = "只有帮主自己一人时才可解散帮派，还有其他成员则需要先转让帮主，然后才能主动退出！";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
           {
               face.gangsInterface.existGangs(() =>
               {
                   toMap("m_1");
               });
           }, () => { });
        }
        public static void bpNames()
        {
            GangsMembersPage pg = PageUI.createAcPage<GangsMembersPage>(PointGet.getIndexPageOfPage());
            pg.drawUI();
            pg.clkTab(1);
        }
        public static void bpMsg()
        {
            GangsMembersPage pg = PageUI.createAcPage<GangsMembersPage>(PointGet.getIndexPageOfPage());
            pg.drawUI();
            pg.clkTab(0);
        }
        public static void bpReq()
        {
            GangsMembersPage pg = PageUI.createAcPage<GangsMembersPage>(PointGet.getIndexPageOfPage());
            pg.drawUI();
            pg.clkTab(2);
        }

        public static void bpList()
        {
            PageUI.createAcPage<GangsListPage>(PointGet.getIndexPageOfPage()).drawUI();
        }
        public static void kmql(string npcKey)
        {
            face.activityInterface.getStTask(npcKey, () => { });
        }
        public static void OpenDzfsbPage(int index)
        {
            PageUI.createAcPage<DzfsbPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void huilu()
        {
            string str = "提前出狱需要消耗翡翠金叶x1";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
            {
                face.activityInterface.huilu(() =>
                {
                    toMap("m_1");
                });
            }, () => { });
        }
        public static void OpenFarmPage(int index)
        {
            PageUI.createAcPage<FarmPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void getBpTask()
        {
            face.gangsInterface.getBpTask(() => { });
        }
        public static void signDsx()
        {
            face.activityInterface.signDsx(() => { });
        }
        public static void bzJfOrder()
        {
            PageUI.createAcPage<BzOrderPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(0);
        }
        public static void zhanlingBzBox(string npcKey)
        {
            face.gangsInterface.getBzBox(npcKey, (res) =>
            {
                string str = "该宝箱已被玩家" + res["playerName"] + "占领，是否抢夺？";
                if (res["playerName"] == null) str = "该宝箱未被占领，是否需要占领它？";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
               {
                   face.gangsInterface.createFightByBz(res["bzId"].ToString(), res["boxKey"].ToString());
               }, () => { });
            });
        }
        public static void openPsShop(int index, string npcKey)
        {
            string areaKey = null;
            if (npcKey.Equals("10000614")) areaKey = "p1";
            else if (npcKey.Equals("10000615")) areaKey = "p2";
            else if (npcKey.Equals("10000617")) areaKey = "p3";
            else if (npcKey.Equals("10000618")) areaKey = "p4";
            PageUI.createAcPage<PsShopPage>(PointGet.getIndexPageOfPage()).drawUI(areaKey).clkTab(index);
        }
        public static void compilePs()
        {
            face.gangsInterface.compilePs();
        }
        public static void startPs()
        {
            string str = "开启跑商任务需要消耗20000银两，同时扣除一次帮派跑商次数，在四个城中游走，通过商人购买或出售货物来赚取差价，完成后目标额度后可到这里换取40000银两";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
            {
                face.gangsInterface.getPsTask(() =>
                {

                });
            }, () => { });
        }

        public static void openHHBKBox3(int type)
        {
            string str = "暴力开箱会得到一些废渣";
            if (type == 1) str = "需要消耗一把 迷宫铜之钥匙";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
            {
                face.activityInterface.openBoxFromSLM(type, () =>
                {

                });
            }, () => { });


        }
        public static void box3Laili()
        {
            string str = "三千年的等待，当年一战已经如流水般逝去，巨龙一族的尊严也不容你等小辈践踏！靠劳资恶龙咆哮......啊呜...啊呜...";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {

            }, () => { });
        }
        public static void box3About()
        {
            string str = "击败龙神后裔你就可以选择一个宝箱开启，暴力开箱只能拿到废渣，钥匙开箱能拿到完整的宝藏！";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {

            }, () => { });
        }
        public static void shengMenToSLM()
        {
            toMap("hhbk_3");
        }
        /**由死门进入神龙门*/
        public static void siMenToSLM()
        {
            //需要将所有怪物消灭
            face.activityInterface.intoShenLongMenFromSi(() =>
            {
                toMap("hhbk_3");
            });

        }
        public static void openHHBKBox2(int type)
        {
            string str = "暴力开箱会得到一些废渣";
            if (type == 1) str = "需要消耗一把 迷宫铁之钥匙";
            else if (type == 2) str = "需要消耗 再来一罐 x1";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
              {
                  face.activityInterface.openBoxFromSheng(type, () =>
                  {

                  });
              }, () => { });


        }
        public static void box2About()
        {
            string str = "你可以选择两个宝箱开启，暴力开箱只能拿到废渣，钥匙开箱能拿到完整的宝藏！";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {

            }, () => { });
        }
        public static void remSiMen()
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("需要是否消耗一面明心镜，是否继续？", () =>
            {
                face.activityInterface.clearSiMen((index) =>
                {
                    //npcManager.clearNpcByKeyExpIndex("10000605", index);
                    string mapKey = "hhbk_2_2";
                    toMap(mapKey);
                });
            }, () => { });
        }
        public static void shengsimenAbout()
        {
            string str = "顾名思义，你的抉择将决定是无穷的宝藏还是穷凶极恶的野兽，生门内藏有宝藏等你开启，而死门会有怪物等你，你可以使用明心镜来消除所有死门！";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {

            }, () => { });
        }
        public static void chooseShengSiMen(modelMsgBind npc)
        {
            //进入生门或者死门
            int index = npcManager.getIndexFromNpc(npc);

            face.activityInterface.chooseShengSiMen(index, (r) =>
            {
                string mapKey = "hhbk_2_1";
                if (r == 1) mapKey = "hhbk_2_2";
                toMap(mapKey);
            });

        }
        /**洪荒宝库-搜索此处*/
        public static void HHBKsousuo(modelMsgBind npc)
        {
            //搜索后弹出
            int index = npcManager.getIndexFromNpc(npc);
            face.activityInterface.searchHere(index, (r) =>
            {
                string str = "此处空荡无比，且去其他地方寻一番机缘吧！";
                if (r == 1) str = "鸿运当头，机缘天降，你终于在石缝种寻找到一把迷宫钥匙！";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                {

                }, () => { });
            });
        }
        public static void toHShengSiMen()
        {
            face.activityInterface.intoShengSiMen(() => { toMap("hhbk_2"); });
        }
        public static void toHHBK()
        {
            //最多能待半个小时
            //集齐3把钥匙才能进入下一层，第二层是生死门入口，1生14死，选对进入生门，否则进入死门，死门需要击败10个怪物才能进入下一层，
            MsgSureUI.create(PointGet.getTipCanvas()).show("需要是否消耗一枚虚幻之石，是否继续？", () =>
            {
                face.activityInterface.intoHhbk(() => { });
            }, () => { });
        }
        /**宝库预览*/
        public static void hhbkyl()
        {
            string str = "洪荒一现魔龙出，高阶宠技数无穷！\n疗血（每回合恢复7%生命）仙灵命术（提高10%生命上限）真灵返血（受到暴击后恢复20%生命）魔龙残影（召唤逆天魔龙）";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {

            }, () => { });
        }
        public static void zjcm()
        {
            //接取仗剑除魔的任务
            face.activityInterface.getZjcm(() => { });
        }
        public static void jyfy(int type)
        {
            string str = "参加活动需要消耗1000元宝，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.getJyfyTask(type, () => { });
            }, () => { });
        }
        public static void ssj()
        {
            string str = "参加活动需要消耗美人香 x1，是否继续？";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                face.activityInterface.shoushenji(() => { });
            }, () => { });
        }
        public static void cyby()
        {
            string str = "天界的神仙需要纯阴的精华炼制仙丹，需要凡间的美人香和天狐精元，现在就派你去青阳城郊击杀狐萌萌，获取体内的天狐精元并交给我，速去速回。";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
            {
                str = "接取任务需要消耗美人香 x3，是否继续？";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                {
                    face.activityInterface.getCybyTask(() => { });
                }, () => { });
            }, () => { });
        }
        public static void showCkTale(int type)
        {
            JObject a = face.goodsInterface.getBBNAndCKN();
            string str = "当前存储的银两共计：" + a["tale"];
            //显示输入框
            UseNumInputUI.create(PointGet.getIndexPageOfPage()).renderText(str).addCallback((num) =>
            {
                if (type == 0)//存入
                {
                    face.goodsInterface.cunchuTale(num, () => { });
                }
                else//取出
                {
                    face.goodsInterface.quchuTale(num, () => { });
                }
            });
        }
        public static void bbKr(int type)
        {
            string str = "扩充背包需要消耗200元宝，总格数+1";
            if (type == 1) str = "扩充仓库需要消耗20000银两，总格数+1";
            MsgSureUI.create(PointGet.getTipCanvas()).show(str + "，是否继续？", () =>
              {
                  face.goodsInterface.bbKr(type, () => { });
              }, () => { });

        }
        public static void showWzyc()
        {
            face.activityInterface.viewWzbzMsg((res) =>
            {
                List<string> m2 = new List<string>() { "领取", "清洗", };
                string[] ns = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", };
                string[] ls = { "精致", "名贵", "珍稀", "绝世" };
                string str = ns[(int)res["lv"] - 1] + "星";
                int type = (int)res["type"];
                str += ls[type] + "藏宝图";
                BaoshiChooseMenuUI menu2 = BaoshiChooseMenuUI.create(m2, PointGet.getIndexPage()).setDes("已领取：" + (3 - (int)res["gain_times"]) + "/3，已清洗：" + res["sx_times"] + "次\n" +
                    "是否清洗 " + str);
                menu2.addCallback((mIndex2) =>
                {
                    //精致、名贵、珍稀、绝世，每日可领取3次，每次领取后需还原成1星，清洗后改变品质跟星级，最高15星，
                    if (mIndex2 == 0)
                    {
                        face.activityInterface.gainWzbz(res, () =>
                        {

                        });
                    }
                    else if (mIndex2 == 1)
                    {
                        face.activityInterface.qingxiWzbz(res, 1, () =>
                        {
                            showWzyc();
                        });
                    }
                });
            });

        }
        public static void openYPSPage(int index)
        {
            PageUI.createAcPage<YuPoShiPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void openJuLingPage()
        {
            PageUI.createAcPage<JuLingPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(0);
        }
        /**垂钓*/
        public static void chuidiao(int type)
        {
            face.activityInterface.viewCDTimes(type, (res) =>
            {
                DiaoYuPage.create(type, res, PointGet.getIndexPageOfPage());
            });
        }
        /**接取锄奸卫道*/
        public static void getCjwd()
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("除魔军传来消息，有奸细潜入寻秦大地，欲将九州各门派逐一铲除，请少侠速去查明奸细身份。三大门主天赋神通，" +
                "早已察觉九州异动，你可到他处寻获线索！", () =>
            {
                //直接接取任务
                face.activityInterface.getCjwdTask(() => { });
            }, () => { });
        }
        /**加入分堂*/
        public static void joinFenTang(string npcKey)
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("即将加入分堂，是否继续？", () =>
            {
                face.roleInterface.joinFenTang(npcKey);
            }, () => { });
        }
        public static void joinMenPai(string npcKey)
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("即将加入门派，是否继续？", () =>
            {
                face.roleInterface.joinMenPai(npcKey);
            }, () => { });
        }
        public static void OpenSebOrder(int index)
        {
            PageUI.createAcPage<ShanErPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void attackMonkey()
        {
            face.gangsInterface.attackMonkey(() => { });
        }
        public static void upLvBp()
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("升级帮派需要商店、建筑、人才、技能、秘法各项建设度达到当前等级对应的贡献度最大值，并且消耗20000建设金，是否继续？", () =>
            {
                face.gangsInterface.upLvGangs(() => { });
            }, () => { });
        }
        public static void OpenBpOrder(int index)
        {
            PageUI.createAcPage<GangsOrderPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenBpAc(int index)
        {
            PageUI.createAcPage<GangsAcPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void putBpMoney()
        {
            UseNumInputUI.create(PointGet.getIndexPageOfPage()).addCallback((num) =>
            {
                face.gangsInterface.putBpMoney(num, () => { });
            });
        }
        public static void putGP(string buildKey, string gpKey)
        {
            UseNumInputUI.create(PointGet.getIndexPageOfPage()).addCallback((num) =>
            {
                face.gangsInterface.putGP(buildKey, gpKey, num, () => { });
            });
        }
        public static void OpenJingMai(int index)
        {
            PageUI.createAcPage<JingMaiPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenZzfs(int index)
        {
            PageUI.createAcPage<ZiZhuangUpPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenBzqj(int index)
        {
            PageUI.createAcPage<BzqjPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenPaiHang(int index)
        {
            PageUI.createAcPage<PaiHangPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenDmkjShop(int index)
        {
            PageUI.createAcPage<DMKJShopPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenLeiTai(int index)
        {
            PageUI.createAcPage<LeiTaiPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenWzyShop(int index)
        {
            PageUI.createAcPage<WzyShopPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenShiTu(int index)
        {
            PageUI.createAcPage<ShiTuPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenMiaoSha(int index)
        {
            PageUI.createAcPage<MiaoShaShopPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenPetUp(int index)
        {
            PageUI.createAcPage<PetUpPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenEquipShop(int index)
        {
            PageUI.createAcPage<EquipShopPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenJiMai(int index)
        {
            PageUI.createAcPage<JiMaiPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        public static void OpenXianRenMiFa(int index)
        {
            PageUI.createAcPage<XianRenMiFaPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        /**打开技能熔炼面板*/
        public static void OpenSkillRL(int index)
        {
            PageUI.createAcPage<SkillRongLianPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        /**打开生活技能面板*/
        public static void OpenLiftSkill(int index)
        {
            PageUI.createAcPage<LiftSkillPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        /**打开锻造面板*/
        public static void OpenDuanzao(int index)
        {
            PageUI.createAcPage<DuanzaoEquipPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(index);
        }
        /**传壁大厅*/
        public static void OpenBiRoom()
        {
            PageUI.createAcPage<ChuanBiRoomPage>(PointGet.getIndexPageOfPage()).drawUI();
        }
        /**领壁页面*/
        public static void viewB()
        {
            Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());

            Transform content = dialog.getContent();
            //提示当前壁的品质，刷新、领取按钮
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(content.transform, false);
            text.GetComponent<TextUI>().setText("").setFontSize(30).setAlign().setColor().setIsRichText(true)
            .setSizePos(new Vector2(760, 200), Vector2.zero);
            Action<int> ac = (lv) =>
            {
                string str = "";
                if (lv == 1) str = "白壁";
                else if (lv == 2) str = "蓝壁";
                else if (lv == 3) str = "紫壁";
                else if (lv == 4) str = "橙壁";
                text.GetComponent<TextUI>().setText("当前壁为" + str);
            };
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(content.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(430, -500));
            btn1.GetComponent<BtnUI>().addText("刷新", 30).setTextColor().loadRes("gy_02_png", new Vector4(10, 10, 10, 10)).addClk(() =>
            {
                face.activityInterface.updateB((lv) =>
                {
                    ac(lv);
                });
            });
            btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(content.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(160, 60), new Vector2(600, -500));
            btn1.GetComponent<BtnUI>().addText("领取", 30).setTextColor().loadRes("gy_02_png", new Vector4(10, 10, 10, 10)).addClk(() =>
            {
                face.activityInterface.gainB(() => { dialog.free(); });
            });

            face.activityInterface.getB((res) =>
            {
                //1白2蓝3紫4橙
                int lv = (int)res["lv"];
                ac(lv);
            });


            DoGet.getInstance().startReqImg();
        }
        /**百炼玄塔触发战斗*/
        public static void tzBlxt(string npcKey)
        {
            //blxt_
            face.activityInterface.createFightByBlxt(npcKey);
        }
        /**显示可学习的门派技能*/
        public static void learnMpSkill(string npcKey)
        {
            JObject role = face.roleInterface.getRole();
            string mp = GameAttrConst.getMenPaiFromModels(role);
            if (mp == null)
            {
                msgCode.showMsg(976);
                return;
            }
            List<string> m2 = null;
            if (mp.Contains("zs") && npcKey.Equals("10000457"))
            {
                m2 = new List<string>() {
                   "100210000001", "100210000040", "100210000041",
                };
            }
            else if (mp.Contains("fs") && npcKey.Equals("10000463"))
            {
                m2 = new List<string>() {
                    "100210000014","100210000042", "100210000043",
                };
            }
            else if (mp.Contains("fz") && npcKey.Equals("10000469"))
            {
                m2 = new List<string>() {
                    "100210000027","100210000044", "100210000045",
                };
            }
            List<string> ml = new List<string>();
            for (int i = 0; i < m2.Count; i++)
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[i]);
                ml.Add(skl.name);
            }
            Menu menu = Menu.create(ml, PointGet.getIndexPage());
            menu.addCallback((mIndex) =>
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[mIndex]);
                JObject role = face.roleInterface.getRole();
                JArray skls = (JArray)role["attr"]["skill"];
                int lv = 0;
                for (int i = 0; i < skls.Count; i++)
                {
                    JObject s = (JObject)skls[i];
                    if (s.ContainsKey("key") && skl.key.Equals(s["key"].ToString()))
                    {
                        lv = (int)s["lv"];
                        break;
                    }
                }
                MsgSureUI.create(PointGet.getTipCanvas()).show(skl.getSkillDes(lv + 1), () =>
                {
                    //确认框，提示学习成本
                    int exp = (int)(150 * Math.Pow(lv + 1, 4));
                    int yp = (int)(100 * Math.Pow(lv + 1, 4));
                    MsgSureUI.create(PointGet.getTipCanvas()).show("学习/升级此技能需要消耗经验" + exp + "，银票" + yp + "。是否继续？", () =>
                    {
                        face.skillInterface.learnSkill(skl.key, (res) =>
                        {

                        });

                    }, () => { });
                }, () => { });
            });
        }
        /**显示可学习的职业技能*/
        public static void learnJobSkill(string npcKey)
        {
            //先判断是否有职业
            string job = face.roleInterface.getJob();
            if (job == null)
            {
                msgCode.showMsg(930);
                return;
            }

            List<string> m2 = null;
            if (job.Contains("ms") && npcKey.Equals("10000458"))
            {
                m2 = new List<string>() {
                    "100210000001", "100210000002", "100210000003", "100210000004", "100210000005", "100210000006", "100210000007",
                };
            }
            else if (job.Contains("dj") && npcKey.Equals("10000459"))
            {
                m2 = new List<string>() {
                    "100210000001", "100210000008", "100210000009", "100210000010", "100210000011", "100210000012", "100210000013",
                };
            }
            else if (job.Contains("qm") && npcKey.Equals("10000464"))
            {
                m2 = new List<string>() {
                    "100210000014", "100210000015", "100210000016", "100210000017", "100210000018", "100210000019", "100210000020",
                };
            }
            else if (job.Contains("ty") && npcKey.Equals("10000465"))
            {
                m2 = new List<string>() {
                    "100210000014", "100210000021", "100210000022", "100210000023", "100210000024", "100210000025", "100210000026",
                };
            }
            else if (job.Contains("ym") && npcKey.Equals("10000471"))
            {
                m2 = new List<string>() {
                    "100210000027", "100210000028", "100210000029", "100210000030", "100210000031", "100210000032", "100210000033",
                };
            }
            else if (job.Contains("lc") && npcKey.Equals("10000470"))
            {
                m2 = new List<string>() {
                    "100210000027", "100210000034", "100210000035", "100210000036", "100210000037", "100210000038", "100210000039",
                };
            }
            else
            {
                msgCode.showMsg(930);
                return;
            }

            List<string> ml = new List<string>();
            for (int i = 0; i < m2.Count; i++)
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[i]);
                ml.Add(skl.name);
            }
            Menu menu = Menu.create(ml, PointGet.getIndexPage());
            menu.addCallback((mIndex) =>
            {
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(m2[mIndex]);
                JObject role = face.roleInterface.getRole();
                JArray skls = (JArray)role["attr"]["skill"];
                int lv = 0;
                for (int i = 0; i < skls.Count; i++)
                {
                    JObject s = (JObject)skls[i];
                    if (s.ContainsKey("key") && skl.key.Equals(s["key"].ToString()))
                    {
                        lv = (int)s["lv"];
                        break;
                    }
                }
                MsgSureUI.create(PointGet.getTipCanvas()).show(skl.getSkillDes(lv + 1), () =>
                  {
                      //确认框，提示学习成本
                      int exp = (int)(150 * Math.Pow(lv + 1, 4));
                      int yp = (int)(100 * Math.Pow(lv + 1, 4));
                      MsgSureUI.create(PointGet.getTipCanvas()).show("学习/升级此技能需要消耗经验" + exp + "，银票" + yp + "。是否继续？", () =>
                            {
                                face.skillInterface.learnSkill(skl.key, (res) =>
                                {

                                });

                            }, () => { });
                  }, () => { });
            });
        }

        public static void toMap(string key)
        {
            mapManager.getInstance().reloadBef(key);
        }
        public static void GetDes(string key)
        {
            Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
            dialog.renderText(face.activityInterface.getAcMsg(key)["des"].ToString());
            DoGet.getInstance().startReqImg();
        }
        public static void GetDaPanHQ()
        {
            face.goodsInterface.GetDaPanHQ((list) =>
            {
                string str = "";
                for (int k = 0; k < list.Count; k++)
                {
                    str += list[k].ToString() + "\n";
                }
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
                dialog.renderText(str);
                DoGet.getInstance().startReqImg();

            });
        }
        public static void GetDaPanTZ()
        {
            face.goodsInterface.GetDaPanTZ((list) =>
            {
                string str = "";
                for (int k = 0; k < list.Count; k++)
                {
                    str += list[k].ToString() + "\n";
                }
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
                dialog.renderText(str);
                DoGet.getInstance().startReqImg();
            });
        }
        public static void DaPan(int bet, int moneyType, object value)
        {
            JObject dapanMsg = new JObject();
            dapanMsg["bet"] = bet;
            dapanMsg["moneyType"] = moneyType;
            dapanMsg["value"] = value.ToString();
            MsgSureUI.create(PointGet.getTipCanvas()).show("即将投资，是否继续？", () =>
            {
                face.goodsInterface.DaPan(dapanMsg, () =>
                {

                });
            }, () => { });

        }
        public static void GetQiHuoHQ()
        {
            face.goodsInterface.GetQiHuoHQ((list) =>
            {
                string str = "";
                for (int k = 0; k < list.Count; k++)
                {
                    str += list[k].ToString() + "\n";
                }
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
                dialog.renderText(str);
                DoGet.getInstance().startReqImg();

            });
        }
        public static void GetQiHuoTZ()
        {
            face.goodsInterface.GetQiHuoTZ((list) =>
            {
                string str = "";
                for (int k = 0; k < list.Count; k++)
                {
                    str += list[k].ToString() + "\n";
                }
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
                dialog.renderText(str);
                DoGet.getInstance().startReqImg();
            });
        }
        public static void QiHuo(int bet, int moneyType, object value)
        {
            JObject dapanMsg = new JObject();
            dapanMsg["bet"] = bet;
            dapanMsg["moneyType"] = moneyType;
            dapanMsg["value"] = value.ToString();
            MsgSureUI.create(PointGet.getTipCanvas()).show("即将投资，是否继续？", () =>
            {
                face.goodsInterface.QiHuo(dapanMsg, () =>
                {

                });
            }, () => { });

        }
        public static void happyAnswer()
        {
            DayAnswerPage.create(PointGet.getIndexPage());
        }

        public static void toFuben(string npcKey)
        {
            string fbKey = null;
            string mapKey = null;
            int tale = 0;
            if (npcKey.Equals("10000092"))
            {
                fbKey = "fb50";
                mapKey = "fb50_1";
                tale = 1000;
            }
            else if (npcKey.Equals("10000145"))
            {
                fbKey = "fb60";
                mapKey = "fb60_1";
                tale = 2000;
            }
            else if (npcKey.Equals("10000205"))
            {
                fbKey = "fb70";
                mapKey = "fb70_1";
                tale = 4000;
            }
            else if (npcKey.Equals("10000273"))
            {
                fbKey = "fb80";
                mapKey = "fb80_1";
                tale = 6000;
            }
            else if (npcKey.Equals("10000319"))
            {
                fbKey = "fb90";
                mapKey = "fb90_1";
                tale = 8000;
            }
            else if (npcKey.Equals("10000427"))
            {
                fbKey = "fb100";
                mapKey = "fb100_1";
                tale = 10000;
            }
            else if (npcKey.Equals("10000379"))//95
            {
                fbKey = "fbls";
                mapKey = "fbls_1";
                tale = 15000;
            }
            else if (npcKey.Equals("10000399"))
            {
                fbKey = "fbhh";
                mapKey = "fbhh_1";
                tale = 15000;
            }
            else if (npcKey.Equals("10000413"))
            {
                fbKey = "fbys";
                mapKey = "fbys_1";
                tale = 20000;
            }
            else if (npcKey.Equals("10000454"))
            {
                fbKey = "fbzx";
                mapKey = "fbzx_1";
                tale = 20000;
            }
            else if (npcKey.Equals("10000555"))//隐藏本 云之境
            {
                fbKey = "fb_yzj";
                mapKey = "fb_yzj_1";
                tale = 0;
            }

            //face.activityInterface.toFbMap(fbKey, mapKey, tale);
            mapManager.getInstance().reloadBef(mapKey);
        }
        public static void createFightByWorldBoss()
        {
            face.activityInterface.createFightByWorldBoss();
        }
        public static void fightYhmk(string npcKey)
        {
            face.activityInterface.fightYhmk(npcKey);
        }
        public static void getMsTask()
        {
            face.activityInterface.getMsTask((taskKey) =>
            {
                task ts = face.taskInterface.getTask(taskKey);
                Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
                dialog.renderText(ts.getProgressDes());
                DoGet.getInstance().startReqImg();
            });
        }
        public static void qiaodaxm()
        {
            MsgSureUI.create(PointGet.getTipCanvas()).show("敲打响木需要消耗1000元宝，是否继续？", () =>
            {
                face.activityInterface.qiaodaxm();
            }, () => { });

        }
        public static void viewMsStatus(string npcKey)
        {
            JObject res = (JObject)face.roleInterface.getRole()["attr"]["msLv"];
            int lv = 0;
            int n = 1;
            List<string> ss = new List<string>();
            List<int> vs = new List<int>();
            string k = null;
            if (npcKey.Equals("10000485")) { n = 1; ss.Add("攻击"); k = "lv1"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            else if (npcKey.Equals("10000486")) { n = 2; ss.Add("防御"); k = "lv2"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            else if (npcKey.Equals("10000487")) { n = 3; ss.Add("生命"); k = "lv3"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            else if (npcKey.Equals("10000488")) { n = 4; ss.Add("速度"); ss.Add("闪避"); k = "lv4"; lv = (int)res[k]; vs.Add(5 * (lv + 1)); vs.Add(2 * (lv + 1)); }
            else if (npcKey.Equals("10000489")) { n = 5; ss.Add("命中"); k = "lv5"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            else if (npcKey.Equals("10000490")) { n = 6; ss.Add("魔法"); k = "lv6"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            else if (npcKey.Equals("10000491")) { n = 7; ss.Add("暴击"); k = "lv7"; lv = (int)res[k]; vs.Add(3 * (lv + 1)); }
            string[] arr =
            {
                "狂攻","铁壁","生命","神速","射手","法术","暴怒",
            };

            string str = "当前" + arr[n - 1] + "魔神等级为" + (lv + 1) + "级\n";
            str += arr[n - 1] + "加持 Lv" + (lv + 1) + "\n";
            for (int i = 0; i < ss.Count; i++)
            {
                str += ss[i] + "加持" + vs[i] + "%" + "\n";
            }


            Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
            dialog.renderText(str);
            DoGet.getInstance().startReqImg();

            /*face.activityInterface.viewStatus(npcKey, (lv) =>
            {
                
            });*/
        }
        public static void viewMsLever(string npcKey)
        {
            JObject res = (JObject)face.roleInterface.getRole()["attr"]["msLv"];
            string str = "Lv" + res["lv1"] + " 狂攻魔神 （攻击加持" + ((int)res["lv1"] * 3) + "%）\n";
            str += "Lv" + res["lv2"] + " 铁壁魔神 （防御加持" + ((int)res["lv2"] * 3) + "%）\n";
            str += "Lv" + res["lv3"] + " 生命魔神 （生命加持" + ((int)res["lv3"] * 3) + "%）\n";
            str += "Lv" + res["lv4"] + " 神速魔神 （速度加持" + ((int)res["lv4"] * 5) + "%，闪避加持" + ((int)res["lv4"] * 2) + "%）\n";
            str += "Lv" + res["lv5"] + " 射手魔神 （命中加持" + ((int)res["lv5"] * 3) + "%）\n";
            str += "Lv" + res["lv6"] + " 法术魔神 （魔法加持" + ((int)res["lv6"] * 3) + "%）\n";
            str += "Lv" + res["lv7"] + " 暴怒魔神 （暴击加持" + ((int)res["lv7"] * 3) + "%）\n";

            Dialog dialog = Dialog.create(new Vector2(800, 600), PointGet.getIndexPage());
            dialog.renderText(str);
            DoGet.getInstance().startReqImg();
            /*face.activityInterface.getJcMsLv((res) =>
            {
               
            });*/
        }
        public static void userGx(string npcKey)
        {
            face.activityInterface.userGx(npcKey);
        }
        public static void tiaozhanMs(string npcKey)
        {
            face.activityInterface.tiaozhanMs(npcKey);
        }
        /**接取门派任务
         */
        /*public static void getMenPaiTask()
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            if (lv < 20)
            {
                msgCode.showMsg(613);
                return;
            }
            //起始任务3100
            face.taskInterface.getHDTask("3100", (res) =>
            {
                JObject msg = (JObject)res;
                JArray createList = (JArray)msg["createList"];
                foreach (object o in createList)
                {
                    JObject a = (JObject)o;
                    JObject progress = new JObject();
                    JObject target = new JObject();
                    target.Add("num", 0);
                    progress.Add("target", target);
                    face.taskInterface.savePlayerTask(a["key"].ToString(), 0, progress, 2);//自动接取
                    int k = int.Parse(a["key"].ToString());
                    if (k >= 3110 & k < 3115)
                        face.taskInterface.trigger(a["key"].ToString());
                }

            });
        }*/
    }
}
