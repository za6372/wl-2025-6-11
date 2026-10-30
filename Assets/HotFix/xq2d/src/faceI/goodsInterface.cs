using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{

    public interface goodsInterface
    {
        public int getChaZhiInPack(string key, int sum);
        public JObject getBBNAndCKN();
        public bool isOverBBSum(int addNum);
        public bool isEnoughInPackAndTip(string key, int sum, bool isTip = true);
        public JArray getAllGoods();
        public JArray getNoWarehouseAnyTypeGoods(string prev);
        public JArray getCkGoods();
        public void initGoods(JToken gMsg);
        public object getGoodsMsgByKey(string key);
        public JObject getPlayerGoodsById(string Id);
        public JObject getPlayerGoodsByKey(string key);
        public JObject getKyAttr(string key);
        public JArray getAllowBuyGoodsList(int type);
        public void savePlayerGoods(JObject obj);
        public JArray getBi();
        public void cutPlayerGoodsNum(string Id, int num);
        public void cutPlayerGoodsNumByKey(String key, int num);
        public void userGoods(JObject target, int num, Action callback = null);
        public void userGoods(string gdKey, int num, Action callback = null);
        public void copyGoods(JObject playerGoods);
        public JArray getEnclosure();
        public void buyGoods(string key, int num, int moneyType, string zkKey, Action<bool> callback);
        public JArray getForingBaoshi();
        public void equipToPackage(JObject equip);
        public JArray getKeyin();
        public void putWarehouse(string Id, Action callback);
        public void reqGiveUpGoods(string Id, Action callback);
        public void getWarehouse(string Id, Action callback);
        public JArray getEquipSkill();
        public void DaPan(JObject msg, Action callback);
        public void GetDaPanHQ(Action<JArray> callback);
        public void GetDaPanTZ(Action<JArray> callback);
        public void GetQiHuoTZ(Action<JArray> callback);
        public void GetQiHuoHQ(Action<JArray> callback);
        public void QiHuo(JObject msg, Action callback);
        public void findJingPai(int pageNum, Action<JObject> callback);
        public void findJingPaiByName(Action<JArray> callback);
        public void pubJingPai(JObject a, Action callback);
        public void getGoodsById(string Id, string businessName, Action<JObject> callback);

        public void offerJingpai(JObject playerGoods, string price, Action callback);
        public void findJPByName(Action<JObject> callback);
        public void agreeOffer(string name, Action callback);
        public void findGrounding(int type, string keywords, int pageNum, Action<JObject> callback);
        public void findGroundingBySelf(Action<JArray> callback);
        public void grounding(string id, int num, int price, int priceType, int gdType, int qx, Action callback);
        public void undercarriage(string Id, Action callback);
        public void buyGrounding(string Id, string businessName, int num, int price, int priceType, Action callback);
        public void undercarriageJingpai(string Id, Action callback);
        public void saveQiangGouGoods(JObject a);
        public JObject getQianggouGoods();
        public void reqRushToBuy(string key, Action callback);
        public JArray getWjSpFromBB();
        public JArray getZhongzi();
        public void TestCreateGoods(string key, int num);
        public JArray getGoldShop(int index);

        public JArray getTaleShop();

        public JArray getYpShop();
        public string getEquipKey(int quality, int part, int job, int lv, int taoz);

        public JObject getInBbOfGoodsByKey(string key);
        public JArray getXianjue(int type);
        public JArray getAllHufu();
        public JArray getEquipShop(int type);
        public void findGroundingDes(string Id, Action<JObject> callback);
        public JArray getPetDan();
        public JArray getPetSkillBook();
        public void getPlayerGoodsOrPetMsgById(string Id, int type, string playerName, Action<JObject> callback);
        public void exchangeJuling(int type, Action callback);
        public void exchangeYuPo(int type, Action callback);
        public void exchangeTianJing(int type, Action callback);
        public void bbKr(int type, Action callback);
        public void cunchuTale(int tale, Action callback);
        public void quchuTale(int tale, Action callback);
        public JArray getZhongZi();
        public void saleBaiEquip(Action callback);
        public bool keyIsInCangKu(string gdKey);
    }
    public class goodsInterfaceImpl : goodsInterface
    {
        /**某个道具是否在仓库*/
        public bool keyIsInCangKu(string gdKey)
        {
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                string key = obj["key"].ToString();
                if (obj["pos"].ToString().Equals("1") && gdKey.Equals(key)) return true;
            }
            return false;
        }
        public void saleBaiEquip(Action callback)
        {
            //判断是否存在白装
            int sum = 0;
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                string key = obj["key"].ToString();
                if (obj["pos"].ToString().Equals("1") || !face.equipInterface.isEquip(key)) continue;
                Equip ep = (Equip)getGoodsMsgByKey(key);
                if (ep.quality == 0)
                {
                    sum++;
                }
            }
            if (sum == 0)
            {
                msgCode.showMsg(209);
                return;
            }

            DoGet.getInstance().sendPost("/packageService/saleBaiEquip", null, (res) =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = (JObject)list[i];
                    string key = obj["key"].ToString();
                    if (!face.equipInterface.isEquip(key)) continue;
                    Equip ep = (Equip)getGoodsMsgByKey(key);
                    if (ep.quality == 0)
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
                saveGoods(list);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public JArray getZhongZi()
        {
            JArray arr = new JArray();
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if ((int)obj["pos"] != 0) continue;
                if (obj["key"].ToString().Substring(0, 4).Equals("1013"))
                {
                    arr.Add(obj);
                }
            }
            return arr;
        }
        public void quchuTale(int tale, Action callback)
        {
            if (tale <= 0)
            {
                msgCode.showMsg(1002);
                return;
            }
            JObject a = getBBNAndCKN();
            int num = (int)a["tale"] - tale;
            if (num < 0)
            {
                msgCode.showMsg(1002);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("tale", tale);
            DoGet.getInstance().sendPost("/packageService/quchuTale", dic, (res) =>
            {
                face.roleInterface.updateMoney(tale, 1);
                saveBbnOrCknOrTale(2, num);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void cunchuTale(int tale, Action callback)
        {
            if (tale <= 0)
            {
                msgCode.showMsg(1002);
                return;
            }
            JObject a = getBBNAndCKN();
            int lv = (int)face.roleInterface.getRole()["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }
            long max = (long)(10000000000f * (lv / 100f));
            int num = tale + (int)a["tale"];
            if (num > max)
            {
                msgCode.showMsg(989, max);
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -num))
            {
                msgCode.showMsg(634);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("tale", tale);
            DoGet.getInstance().sendPost("/packageService/cunchuTale", dic, (res) =>
            {
                face.roleInterface.updateMoney(-tale, 1);
                saveBbnOrCknOrTale(2, num);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**背包扩容*/
        public void bbKr(int type, Action callback)
        {
            JObject nk = this.getBBNAndCKN();
            if (type == 0 && (int)nk["bbn"] >= 400)
            {
                msgCode.showMsg(988);
                return;
            }
            else if (type == 1 && (int)nk["ckn"] >= 400)
            {
                msgCode.showMsg(988);
                return;
            }
            if (type == 0 && !face.roleInterface.isEnoughMoney("gold", -200))
            {
                msgCode.showMsg(634);
                return;
            }
            else if (type == 1 && !face.roleInterface.isEnoughMoney("tale", -20000))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/packageService/bbKr", dic, (res) =>
            {
                JObject a = getBBNAndCKN();
                int num = 0;
                if (type == 0)
                {
                    face.roleInterface.updateMoney(-200, 0);
                    num = (int)a["bbn"] + 1;
                }
                else
                {
                    face.roleInterface.updateMoney(-20000, 1);
                    num = (int)a["ckn"] + 1;
                }
                //保存背包/仓库数量
                saveBbnOrCknOrTale(type, num);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**兑换天晶*/
        public void exchangeTianJing(int type, Action callback)
        {
            string xhKey = null;
            int num = 3;
            if (type == 0)
            {
                xhKey = "10000190";
                num = 5;
            }
            else if (type == 1)
            {
                xhKey = "10000190";
                num = 10;
            }

            if (!this.isEnoughInPackAndTip(xhKey, num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/tjypService/exchangeTianJing", dic, (res) =>
            {
                this.cutPlayerGoodsNumByKey(xhKey, num);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(838);
                callback();
            });
        }
        /**兑换玉魄石*/
        public void exchangeYuPo(int type, Action callback)
        {
            List<string> xhKeys = new List<string>();
            List<int> nums = new List<int>();
            int exp = 0;
            int xhTale = 0;
            String dhKey = null;
            if (type == 0)
            {
                xhKeys.Add("10000191");
                nums.Add(5);
                exp = 43424;
            }
            else if (type == 1)
            {
                xhKeys.Add("10000191");
                nums.Add(10);
                exp = 43424 * 2;
            }
            else if (type == 2)
            {
                xhKeys.Add("10000191");
                nums.Add(30);
                exp = 43424 * 6;
            }
            else if (type == 3)
            {
                xhKeys.Add("10000191");
                xhKeys.Add("10000002");
                nums.Add(1);
                nums.Add(1);
                xhTale = 8000;
                dhKey = "10000192";
            }
            else if (type == 4)
            {
                xhKeys.Add("10000191");
                xhKeys.Add("10000192");
                nums.Add(99);
                nums.Add(12);
                dhKey = "10000193";
            }
            else return;
            if (xhKeys.Count() > 0)
            {
                for (int i = 0; i < xhKeys.Count(); i++)
                {
                    if (!face.goodsInterface.isEnoughInPackAndTip(xhKeys[i], nums[i]))
                    {
                        return;
                    }
                }
            }
            if (xhTale > 0)
            {
                if (!face.roleInterface.isEnoughMoney("tale", -xhTale))
                {
                    msgCode.showMsg(634);
                    return;
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/tjypService/exchangeYuPo", dic, (res) =>
            {
                if (xhKeys.Count() > 0)
                {
                    for (int i = 0; i < xhKeys.Count(); i++)
                    {
                        face.goodsInterface.cutPlayerGoodsNumByKey(xhKeys[i], nums[i]);
                    }
                }
                if (xhTale > 0)
                {
                    face.roleInterface.updateMoney(-xhTale, 1);
                }

                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(838);
                callback();
            });
        }
        /**兑换聚灵道具*/
        public void exchangeJuling(int type, Action callback)
        {
            string xhKey = null;
            string dhKey = null;
            int num = 3;
            if (type == 0)
            {
                xhKey = "10000125";
                dhKey = "10000181";
                num = 3;
            }
            else if (type == 1)
            {
                xhKey = "10000181";
                dhKey = "10000182";
                num = 3;
            }
            else if (type == 2)
            {
                xhKey = "10000182";
                dhKey = "10000126";
                num = 3;
            }
            else if (type == 3)
            {
                xhKey = "10000125";
                dhKey = "10000126";
                num = 27;
            }
            else if (type == 4)
            {
                xhKey = "10000125";
                dhKey = "10000182";
                num = 9;
            }
            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/julingService/exchangeJuling", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, num);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(838);
                callback();
            });
        }
        /**查看道具信息*/
        public void getPlayerGoodsOrPetMsgById(string Id, int type, string playerName, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("type", type);
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/packageService/getPlayerGoodsOrPetMsgById", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**获取背包中的宠物技能书*/
        public JArray getPetSkillBook()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if ((int)g["pos"] != 0 || !g["key"].ToString().Substring(0, 8).Equals("10021001")) continue;
                list.Add(g);
            }
            return list;
        }
        /**获取背包中的宠物丹*/
        public JArray getPetDan()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if ((int)g["pos"] != 0 || !g["key"].ToString().Substring(0, 4).Equals("1011")) continue;
                list.Add(g);
            }
            return list;
        }
        /**获取背包中的仙绝*/
        public JArray getAllHufu()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                //仙绝前缀
                if (!g["key"].ToString().Substring(0, 8).Equals("10011008")) continue;

                int num = int.Parse(g["key"].ToString().Substring(8));
                if ((int)g["pos"] == 0 && num < 9)
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取背包中的仙绝*/
        public JArray getXianjue(int type)
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                //仙绝前缀
                if (!g["key"].ToString().Substring(0, 8).Equals("10021003")) continue;

                int num = int.Parse(g["key"].ToString().Substring(8));
                if ((int)g["pos"] == 0 && num < 100)
                {
                    Skill gd = (Skill)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.skillType != type) continue;
                    list.Add(g);
                }
            }
            return list;
        }
        /**
         * 根据职业、等级、部位、品质获取装备（不包含药囊、法宝等）
         * part "wq", "jb", "sz", "wb", "tb", "xb", "yb", "tuib", "jiaob"
         * job "ms", "dj", "qm", "ty", "ym", "lc",
         * quality 0白、1蓝、2紫、3金、4亮金、5红
         * lv 0=>1-10 1=>11-20 2=>21-30 3=>31-40...
         * taoz 0|1|2|3|4|（散装）  5（套装）
         */
        public string getEquipKey(int quality, int part, int job, int lv, int taoz)
        {
            //装备的标志
            String bz = "1001";
            //前四位
            int a = strUtils.getRandom(1000, 1006);
            if (quality != -1) a = 1000 + quality;
            //首先按部位来确定划分的区间
            int start = 0;
            int end = 360 + 60 * 2 + 180 * 6;//区间的极限
            if (part == -1)
            {
                //部位必须要存在，因为部位关系到装备的职业，如武器只能是6个职业，戒指无职业
                part = strUtils.getRandom(0, 9);
            }
            if (part == 0) end = 360;
            else if (part == 1 || part == 2)
            {
                start = 360 + 60 * (part - 1);
                end = 360 + 60 * part;
            }
            else
            {
                start = 360 + 60 * 2 + 180 * (part - 3);
                end = 360 + 60 * 2 + 180 * (part - 2);
            }
            List<int> list = new List<int>();
            for (int i = start; i < end; i++)
            {
                list.Add(i);
            }
            //在此区间中筛选出该职业对应的数
            if (job != -1)
            {
                if (part == 0)
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if ((list[i] - start) % 6 != job)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else if (part == 1 || part == 2)
                {
                    //项链、戒指都是无职业一种，所以不用筛选
                }
                else
                {
                    //传入的职业为标准职业时，需要处理
                    if (job == 0 || job == 1) job = 7;
                    else if (job == 2 || job == 3) job = 8;
                    else if (job == 4 || job == 5) job = 9;
                    else if (job == 6) job = strUtils.getRandom(7, 10);
                    if (job < 7)
                    {
                        Debug.Log("手指之后的部位job需要+7来表示墨、道、阳");
                    }
                    //7 + (b - 360 - 60 * 2 - 180 * index) % 3;
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (7 + (list[i] - start) % 3 != job)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            //在此区间找等级一致的
            if (lv != -1)
            {
                if (part == 0)
                {
                    //lv = (b / 6) % 10 * 10 + lvs[part];
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (((list[i] - start) / 6) % 10 != lv)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else if (part == 1 || part == 2)
                {
                    //(b - 360 - 60 * index) % 10 * 10 + lvs[part];
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if ((list[i] - start) % 10 != lv)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    //((b - 360 - 60 * 2 - 180 * index) / 3) % 10 * 10 + lvs[part];
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (((list[i] - start) / 3) % 10 != lv)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            if (taoz != -1)
            {
                if (part == 0)
                {
                    //b / (10 * 6)
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if ((list[i] - start) / (10 * 6) != taoz)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else if (part == 1 || part == 2)
                {
                    //(b - 360 - 60 * index) / 10;
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if ((list[i] - start) / 10 != taoz)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    //(b - 360 - 60 * 2 - 180 * index) / (10 * 3);
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if ((list[i] - start) / (10 * 3) != taoz)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            String c = null;
            if (list.Count() == 1)
            {
                //完全确定的装备
                c = list[0] + "";
            }
            else
            {
                //需要随机
                c = list[strUtils.getRandom(0, list.Count())] + "";
            }
            //不满4位需要在前面补0
            if (c.Length < 4)
            {
                int len = 4 - c.Length;
                for (int i = 0; i < len; i++)
                {
                    c = "0" + c;
                }
            }
            return bz + a + c;
        }

        private JArray shopKeyToArr(List<string> krr, int type)
        {
            JArray arr = new JArray();
            for (int i = 0; i < krr.Count; i++)
            {
                GoodsDes g = (GoodsDes)this.getGoodsMsgByKey(krr[i]);
                JArray list = g.buyList;
                JObject item = new JObject();
                item.Add("key", g.key.ToString());
                item.Add("name", g.name.ToString());
                item.Add("des", g.des);
                item.Add("priceType", type);
                item.Add("icon", g.icon);
                item.Add("num", 1);
                item.Add("quality", g.quality);
                for (int j = 0; j < list.Count; j++)
                {
                    if ((int)list[j]["moneyType"] == type)
                    {
                        item.Add("price", (int)list[j]["value"]);
                        arr.Add(item);
                        break;
                    }
                }
            }

            return arr;
        }
        public JArray getEquipShop(int type)
        {
            JArray list = new JArray();
            string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
            if (mapKey.Equals("m_6"))
            {
                if (type == 0)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,0,0,0),getEquipKey(0,0,0,1,0),getEquipKey(0,0,0,2,0),
                    getEquipKey(0,0,1,0,0),getEquipKey(0,0,1,1,0),getEquipKey(0,0,1,2,0),
                    getEquipKey(0,1,0,0,0),getEquipKey(0,1,0,1,0),getEquipKey(0,1,0,2,0),
                    getEquipKey(0,2,0,0,0),getEquipKey(0,2,0,1,0),getEquipKey(0,2,0,2,0),
                    getEquipKey(0,3,0,0,0),getEquipKey(0,3,0,1,0),getEquipKey(0,3,0,2,0),
                    getEquipKey(0,4,0,0,0),getEquipKey(0,4,0,1,0),getEquipKey(0,4,0,2,0),
                    getEquipKey(0,5,0,0,0),getEquipKey(0,5,0,1,0),getEquipKey(0,5,0,2,0),
                    getEquipKey(0,6,0,0,0),getEquipKey(0,6,0,1,0),getEquipKey(0,6,0,2,0),
                    getEquipKey(0,7,0,0,0),getEquipKey(0,7,0,1,0),getEquipKey(0,7,0,2,0),
                    getEquipKey(0,8,0,0,0),getEquipKey(0,8,0,1,0),getEquipKey(0,8,0,2,0),
                    });
                }
                else if (type == 1)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,2,0,0),getEquipKey(0,0,2,1,0),getEquipKey(0,0,2,2,0),
                    getEquipKey(0,0,3,0,0),getEquipKey(0,0,3,1,0),getEquipKey(0,0,3,2,0),
                    getEquipKey(0,1,0,0,0),getEquipKey(0,1,0,1,0),getEquipKey(0,1,0,2,0),
                    getEquipKey(0,2,0,0,0),getEquipKey(0,2,0,1,0),getEquipKey(0,2,0,2,0),
                    getEquipKey(0,3,2,0,0),getEquipKey(0,3,2,1,0),getEquipKey(0,3,2,2,0),
                    getEquipKey(0,4,2,0,0),getEquipKey(0,4,2,1,0),getEquipKey(0,4,2,2,0),
                    getEquipKey(0,5,2,0,0),getEquipKey(0,5,2,1,0),getEquipKey(0,5,2,2,0),
                    getEquipKey(0,6,2,0,0),getEquipKey(0,6,2,1,0),getEquipKey(0,6,2,2,0),
                    getEquipKey(0,7,2,0,0),getEquipKey(0,7,2,1,0),getEquipKey(0,7,2,2,0),
                    getEquipKey(0,8,2,0,0),getEquipKey(0,8,2,1,0),getEquipKey(0,8,2,2,0),
                    });
                }
                else if (type == 2)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,4,0,0),getEquipKey(0,0,4,1,0),getEquipKey(0,0,4,2,0),
                    getEquipKey(0,0,4,0,0),getEquipKey(0,0,5,1,0),getEquipKey(0,0,5,2,0),
                    getEquipKey(0,1,0,0,0),getEquipKey(0,1,0,1,0),getEquipKey(0,1,0,2,0),
                    getEquipKey(0,2,0,0,0),getEquipKey(0,2,0,1,0),getEquipKey(0,2,0,2,0),
                    getEquipKey(0,3,4,0,0),getEquipKey(0,3,4,1,0),getEquipKey(0,3,4,2,0),
                    getEquipKey(0,4,4,0,0),getEquipKey(0,4,4,1,0),getEquipKey(0,4,4,2,0),
                    getEquipKey(0,5,4,0,0),getEquipKey(0,5,4,1,0),getEquipKey(0,5,4,2,0),
                    getEquipKey(0,6,4,0,0),getEquipKey(0,6,4,1,0),getEquipKey(0,6,4,2,0),
                    getEquipKey(0,7,4,0,0),getEquipKey(0,7,4,1,0),getEquipKey(0,7,4,2,0),
                    getEquipKey(0,8,4,0,0),getEquipKey(0,8,4,1,0),getEquipKey(0,8,4,2,0),
                    });
                }
            }
            else if (mapKey.Equals("m_19"))
            {
                if (type == 0)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,0,3,0),getEquipKey(0,0,0,4,0),
                    getEquipKey(0,0,1,3,0),getEquipKey(0,0,1,4,0),
                    getEquipKey(0,1,0,3,0),getEquipKey(0,1,0,4,0),
                    getEquipKey(0,2,0,3,0),getEquipKey(0,2,0,4,0),
                    getEquipKey(0,3,0,3,0),getEquipKey(0,3,0,4,0),
                    getEquipKey(0,4,0,3,0),getEquipKey(0,4,0,4,0),
                    getEquipKey(0,5,0,3,0),getEquipKey(0,5,0,4,0),
                    getEquipKey(0,6,0,3,0),getEquipKey(0,6,0,4,0),
                    getEquipKey(0,7,0,3,0),getEquipKey(0,7,0,4,0),
                    getEquipKey(0,8,0,3,0),getEquipKey(0,8,0,4,0),
                    });
                }
                else if (type == 1)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,2,3,0),getEquipKey(0,0,2,4,0),
                    getEquipKey(0,0,3,3,0),getEquipKey(0,0,3,4,0),
                    getEquipKey(0,1,0,3,0),getEquipKey(0,1,0,4,0),
                    getEquipKey(0,2,0,3,0),getEquipKey(0,2,0,4,0),
                    getEquipKey(0,3,2,3,0),getEquipKey(0,3,2,4,0),
                    getEquipKey(0,4,2,3,0),getEquipKey(0,4,2,4,0),
                    getEquipKey(0,5,2,3,0),getEquipKey(0,5,2,4,0),
                    getEquipKey(0,6,2,3,0),getEquipKey(0,6,2,4,0),
                    getEquipKey(0,7,2,3,0),getEquipKey(0,7,2,4,0),
                    getEquipKey(0,8,2,3,0),getEquipKey(0,8,2,4,0),
                    });
                }
                else if (type == 2)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,4,3,0),getEquipKey(0,0,4,4,0),
                    getEquipKey(0,0,4,3,0),getEquipKey(0,0,5,4,0),
                    getEquipKey(0,1,0,3,0),getEquipKey(0,1,0,4,0),
                    getEquipKey(0,2,0,3,0),getEquipKey(0,2,0,4,0),
                    getEquipKey(0,3,4,3,0),getEquipKey(0,3,4,4,0),
                    getEquipKey(0,4,4,3,0),getEquipKey(0,4,4,4,0),
                    getEquipKey(0,5,4,3,0),getEquipKey(0,5,4,4,0),
                    getEquipKey(0,6,4,3,0),getEquipKey(0,6,4,4,0),
                    getEquipKey(0,7,4,3,0),getEquipKey(0,7,4,4,0),
                    getEquipKey(0,8,4,3,0),getEquipKey(0,8,4,4,0),
                    });
                }
            }
            else if (mapKey.Equals("m_35"))
            {
                if (type == 0)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,0,5,0),getEquipKey(0,0,0,6,0),
                    getEquipKey(0,0,1,5,0),getEquipKey(0,0,1,6,0),
                    getEquipKey(0,1,0,5,0),getEquipKey(0,1,0,6,0),
                    getEquipKey(0,2,0,5,0),getEquipKey(0,2,0,6,0),
                    getEquipKey(0,3,0,5,0),getEquipKey(0,3,0,6,0),
                    getEquipKey(0,4,0,5,0),getEquipKey(0,4,0,6,0),
                    getEquipKey(0,5,0,5,0),getEquipKey(0,5,0,6,0),
                    getEquipKey(0,6,0,5,0),getEquipKey(0,6,0,6,0),
                    getEquipKey(0,7,0,5,0),getEquipKey(0,7,0,6,0),
                    getEquipKey(0,8,0,5,0),getEquipKey(0,8,0,6,0),
                    });
                }
                else if (type == 1)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,2,5,0),getEquipKey(0,0,2,6,0),
                    getEquipKey(0,0,3,5,0),getEquipKey(0,0,3,6,0),
                    getEquipKey(0,1,0,5,0),getEquipKey(0,1,0,6,0),
                    getEquipKey(0,2,0,5,0),getEquipKey(0,2,0,6,0),
                    getEquipKey(0,3,2,5,0),getEquipKey(0,3,2,6,0),
                    getEquipKey(0,4,2,5,0),getEquipKey(0,4,2,6,0),
                    getEquipKey(0,5,2,5,0),getEquipKey(0,5,2,6,0),
                    getEquipKey(0,6,2,5,0),getEquipKey(0,6,2,6,0),
                    getEquipKey(0,7,2,5,0),getEquipKey(0,7,2,6,0),
                    getEquipKey(0,8,2,5,0),getEquipKey(0,8,2,6,0),
                    });
                }
                else if (type == 2)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,4,5,0),getEquipKey(0,0,4,6,0),
                    getEquipKey(0,0,4,5,0),getEquipKey(0,0,5,6,0),
                    getEquipKey(0,1,0,5,0),getEquipKey(0,1,0,6,0),
                    getEquipKey(0,2,0,5,0),getEquipKey(0,2,0,6,0),
                    getEquipKey(0,3,4,5,0),getEquipKey(0,3,4,6,0),
                    getEquipKey(0,4,4,5,0),getEquipKey(0,4,4,6,0),
                    getEquipKey(0,5,4,5,0),getEquipKey(0,5,4,6,0),
                    getEquipKey(0,6,4,5,0),getEquipKey(0,6,4,6,0),
                    getEquipKey(0,7,4,5,0),getEquipKey(0,7,4,6,0),
                    getEquipKey(0,8,4,5,0),getEquipKey(0,8,4,6,0),
                    });
                }
            }
            else if (mapKey.Equals("m_56"))
            {
                if (type == 0)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,0,7,0),getEquipKey(0,0,0,8,0),
                    getEquipKey(0,0,1,7,0),getEquipKey(0,0,1,8,0),
                    getEquipKey(0,1,0,7,0),getEquipKey(0,1,0,8,0),
                    getEquipKey(0,2,0,7,0),getEquipKey(0,2,0,8,0),
                    getEquipKey(0,3,0,7,0),getEquipKey(0,3,0,8,0),
                    getEquipKey(0,4,0,7,0),getEquipKey(0,4,0,8,0),
                    getEquipKey(0,5,0,7,0),getEquipKey(0,5,0,8,0),
                    getEquipKey(0,6,0,7,0),getEquipKey(0,6,0,8,0),
                    getEquipKey(0,7,0,7,0),getEquipKey(0,7,0,8,0),
                    getEquipKey(0,8,0,7,0),getEquipKey(0,8,0,8,0),
                    });
                }
                else if (type == 1)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,2,7,0),getEquipKey(0,0,2,8,0),
                    getEquipKey(0,0,3,7,0),getEquipKey(0,0,3,8,0),
                    getEquipKey(0,1,0,7,0),getEquipKey(0,1,0,8,0),
                    getEquipKey(0,2,0,7,0),getEquipKey(0,2,0,8,0),
                    getEquipKey(0,3,2,7,0),getEquipKey(0,3,2,8,0),
                    getEquipKey(0,4,2,7,0),getEquipKey(0,4,2,8,0),
                    getEquipKey(0,5,2,7,0),getEquipKey(0,5,2,8,0),
                    getEquipKey(0,6,2,7,0),getEquipKey(0,6,2,8,0),
                    getEquipKey(0,7,2,7,0),getEquipKey(0,7,2,8,0),
                    getEquipKey(0,8,2,7,0),getEquipKey(0,8,2,8,0),
                    });
                }
                else if (type == 2)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,4,7,0),getEquipKey(0,0,4,8,0),
                    getEquipKey(0,0,4,7,0),getEquipKey(0,0,5,8,0),
                    getEquipKey(0,1,0,7,0),getEquipKey(0,1,0,8,0),
                    getEquipKey(0,2,0,7,0),getEquipKey(0,2,0,8,0),
                    getEquipKey(0,3,4,7,0),getEquipKey(0,3,4,8,0),
                    getEquipKey(0,4,4,7,0),getEquipKey(0,4,4,8,0),
                    getEquipKey(0,5,4,7,0),getEquipKey(0,5,4,8,0),
                    getEquipKey(0,6,4,7,0),getEquipKey(0,6,4,8,0),
                    getEquipKey(0,7,4,7,0),getEquipKey(0,7,4,8,0),
                    getEquipKey(0,8,4,7,0),getEquipKey(0,8,4,8,0),
                    });
                }
            }
            else if (mapKey.Equals("m_90"))
            {
                if (type == 0)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,0,9,0),
                    getEquipKey(0,0,1,9,0),
                    getEquipKey(0,1,0,9,0),
                    getEquipKey(0,2,0,9,0),
                    getEquipKey(0,3,0,9,0),
                    getEquipKey(0,4,0,9,0),
                    getEquipKey(0,5,0,9,0),
                    getEquipKey(0,6,0,9,0),
                    getEquipKey(0,7,0,9,0),
                    getEquipKey(0,8,0,9,0),
                    });
                }
                else if (type == 1)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,2,9,0),
                    getEquipKey(0,0,3,9,0),
                    getEquipKey(0,1,0,9,0),
                    getEquipKey(0,2,0,9,0),
                    getEquipKey(0,3,2,9,0),
                    getEquipKey(0,4,2,9,0),
                    getEquipKey(0,5,2,9,0),
                    getEquipKey(0,6,2,9,0),
                    getEquipKey(0,7,2,9,0),
                    getEquipKey(0,8,2,9,0),
                    });
                }
                else if (type == 2)
                {
                    list.Merge(new List<string> {
                    getEquipKey(0,0,4,9,0),
                    getEquipKey(0,0,4,9,0),
                    getEquipKey(0,1,0,9,0),
                    getEquipKey(0,2,0,9,0),
                    getEquipKey(0,3,4,9,0),
                    getEquipKey(0,4,4,9,0),
                    getEquipKey(0,5,4,9,0),
                    getEquipKey(0,6,4,9,0),
                    getEquipKey(0,7,4,9,0),
                    getEquipKey(0,8,4,9,0),
                    });
                }
            }
            return list;
        }
        /**获取元宝商品*/
        public JArray getGoldShop(int index)
        {
            List<string> list = new List<string>();
            if (index == 0)
            {
                string[] arr =
                {
                    "10000000","10000001","10000002",
                    "10000125","10000126","10000127","10000128","10000129",

                    "10000160","10000162","10000163","10000164",
                    "10000165","10000166","10000167","10000174","10000177","10000179","10000183",
                    "10000184", "10000185","10000187","10000194","10000195","10000196","10000197", "10000198", "10000201","10000208","10000209","10000210",
                    "10000213","10000223",
                    "10000224","10000263",

                };
                list.AddRange(arr);
            }
            else if (index == 1)
            {
                list.AddRange(new List<string>() { "10000112", "10000257", "10000258", "10000260",
                    "10000261", "10000262", "10000265","10000285","10000286","10000292", });
            }
            else if (index == 2)
            {
                list.AddRange(new List<string>() { "10000105","10000107",
                    "10000109","10000111","10000114","10000115","10000149","10000150","10000159",

                    "100110060002", "100110060003", });
                //3级镶嵌石
                for (int i = 10030020; i < 10030030; i++)
                {
                    list.Add(i.ToString());
                }

            }
            else if (index == 3)
            {
                list.AddRange(new List<string>() { "10000031", "10000032","10000113","10000118","10000119","10000120","10000121","10000122","10000123",
                    "10000124","10000130","10000140","10000141","10000171","10000178", });

                //宠物丹
                /*for (int i = 10110000; i < 10110030; i++)
                {
                    list.Add(i.ToString());
                }*/
                //仙绝技能
                /*for (int i = 10030027; i < 10030054; i++)
                {
                    list.Add("1002" + i.ToString());
                }
                //宠物技能
                for (int i = 10010178; i < 10010183; i++)
                {
                    list.Add("1002" + i.ToString());
                }
                for (int i = 10010210; i < 10010314; i++)
                {
                    list.Add("1002" + i.ToString());
                }*/

            }




            return shopKeyToArr(list, 0);
        }
        /**获取银两商品*/
        public JArray getTaleShop()
        {
            List<string> list = new List<string>()
            {
                "10000004","10000029","10000030",
                "100110060001"
            };
            return shopKeyToArr(list, 1);
        }
        /**获取银票商品*/
        public JArray getYpShop()
        {
            List<string> list = new List<string>()
            {
                "100110060000","10000185",
            };
            return shopKeyToArr(list, 2);
        }
        public void TestCreateGoods(string key, int num)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/packageService/TestCreateGoods", dic, (res) =>
            {

            });
        }
        /**获取种子*/
        public JArray getZhongzi()
        {
            JArray list = new JArray();
            JArray gs = this.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                GoodsDes obj = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                if ((int)g["pos"] == 0 && strUtils.isMatch(g["key"].ToString(), "1199([0-9]{4})"))
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**从背包中获取武将碎片*/
        public JArray getWjSpFromBB()
        {
            JArray al = new JArray();
            JArray list = this.getAllGoods();
            for (int i = 0; i < list.Count; i++)
            {
                JObject a = (JObject)list[i];
                if (strUtils.isMatch(a["key"].ToString(), "1150([0-9]{4})"))
                {
                    a.Add("nowNum", 0);
                    al.Add(a);
                }
            }
            return al;
        }
        public void reqRushToBuy(string key, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/shopService/reqRushToBuy", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    msgCode.showMsg(903);
                }
                else
                {
                    msgCode.showMsg(668);
                }
                //不论是否购买到都要标记已被购买
                JObject a = getQianggouGoods();
                a[key]["isBuy"] = 1;
                saveQiangGouGoods(a);
                callback();
            });
        }
        public JObject getQianggouGoods()
        {
            return dbHandle.get<JObject>("qianggou");
        }
        /**缓存抢购的商品*/
        public void saveQiangGouGoods(JObject a)
        {
            dbHandle.save("qianggou", a);
        }
        /**购买上架商品*/
        public void buyGrounding(string Id, string businessName, int num, int price, int priceType, Action callback)
        {
            if (num <= 0) return;
            if (!face.roleInterface.isEnoughMoney(priceType == 0 ? "gold" : "tale", -num * price))
            {
                msgCode.showMsg(634);
                return;
            }
            JObject role = face.roleInterface.getRole();
            if (businessName.Equals(role["name"].ToString()))
            {
                msgCode.showMsg(204);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("num", num);

            DoGet.getInstance().sendPost("/jimaiService/buyGrounding", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    face.roleInterface.updateMoney(-num * price, priceType);
                    msgCode.showMsg(692);
                    callback();
                }
                else
                {
                    msgCode.showMsg(693);
                }

            });
        }
        /**下架商品*/
        public void undercarriage(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);

            DoGet.getInstance().sendPost("/jimaiService/undercarriage", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    msgCode.showMsg(787);
                    callback();
                }
                else
                {
                    msgCode.showMsg(697);
                }

            });
        }
        /**上架商品*/
        public void grounding(string id, int num, int price, int priceType, int gdType, int qx, Action callback)
        {
            if (num <= 0)
            {
                msgCode.showMsg(956);
                return;
            }
            int tale = Convert.ToInt32(num * price * 0.01f * (qx + 1));
            if (tale <= 0) tale = 1;
            if (!face.roleInterface.isEnoughMoney("tale", -tale))
            {
                msgCode.showMsg(634);
                return;
            }
            string keywords = null;
            if (gdType == 0)
            {
                JObject a = getPlayerGoodsById(id);
                if ((int)a["num"] < num)
                {
                    msgCode.showMsg(637);
                    return;
                }
                if (face.emailInterface.isNoAllowedSend(a))
                {
                    msgCode.showMsg(691);
                    return;
                }
                keywords = ((GoodsDes)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString())).name;
            }
            else
            {
                JObject pet = face.petInterface.getOneById(id);
                if ((int)pet["growLv"] >= 6)
                {
                    msgCode.showMsg(691);
                    return;
                }
                keywords = face.petInterface.getPetDataByKey(pet["key"].ToString()).name;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", id);
            dic.Add("num", num);
            dic.Add("price", price);
            dic.Add("priceType", priceType);
            dic.Add("gdType", gdType);
            dic.Add("keywords", keywords);
            dic.Add("qx", qx);

            DoGet.getInstance().sendPost("/jimaiService/grounding", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    face.roleInterface.updateMoney(-tale, 1);
                    if (gdType == 0)
                    {
                        face.goodsInterface.cutPlayerGoodsNum(id, num);
                    }
                    else
                    {
                        face.petInterface.remFromCache(id);
                    }
                    msgCode.showMsg(690);
                    callback();
                }
                else
                {
                    msgCode.showMsg(691);
                }

            });
        }
        public void findGroundingDes(string Id, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/shopService/findGroundingDes", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(787);
                    return;
                }
                callback((JObject)res);
            });
        }
        /**获取已经上架的商品*/
        public void findGroundingBySelf(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/jimaiService/findGroundingBySelf", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**获取竞拍列表*/
        public void findGrounding(int type, string keywords, int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            dic.Add("type", type);
            dic.Add("keywords", keywords);
            DoGet.getInstance().sendPost("/jimaiService/findGrounding", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**接受出价*/
        public void agreeOffer(string name, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("name", name);
            DoGet.getInstance().sendPost("/shopService/agreeOffer", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(805);
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(806);
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(807);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(808);
                    callback();
                }


            });
        }
        /**下架竞拍商品*/
        public void undercarriageJingpai(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);

            DoGet.getInstance().sendPost("/shopService/undercarriageJingpai", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    msgCode.showMsg(787);
                    callback();
                }
                else
                {
                    msgCode.showMsg(697);
                }

            });
        }
        /**查询竞拍中的商品*/
        public void findJPByName(Action<JObject> callback)
        {

            DoGet.getInstance().sendPost("/shopService/findJPByName", null, (res) =>
            {
                if (res == null || ((JArray)res).Count == 0) return;
                JObject gs = (JObject)(((JArray)res)[0]["offers"]);

                callback(gs);

            });
        }
        /**出价*/
        public void offerJingpai(JObject playerGoods, string price, Action callback)
        {
            JObject ls = (JObject)playerGoods["offers"];
            int a = 0;
            foreach (JProperty property in ls.Properties())
            {
                if ((int)property.Value > a)
                {
                    a = (int)property.Value;
                }
            }
            //判断余额是否充足
            JObject r = face.roleInterface.getRole();

            int offerPrice = int.Parse(price);
            if (strUtils.isNull(price) || offerPrice <= a)
            {
                msgCode.showMsg(800);
                return;
            }
            if ((int)playerGoods["priceType"] == 0)
            {
                if ((int)r["attr"]["msg"]["gold"] - offerPrice < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            else
            {
                if ((int)r["attr"]["msg"]["tale"] - offerPrice < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", playerGoods["Id"].ToString());
            dic.Add("offer", offerPrice);
            DoGet.getInstance().sendPost("/shopService/offerJingpai", dic, (res) =>
            {
                if (res.ToString() == "1")
                {
                    msgCode.showMsg(801);
                    callback();
                }
                else
                {
                    msgCode.showMsg(802);
                }


            });
        }
        /**查询商家物品 */
        public void getGoodsById(string Id, string businessName, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("businessName", businessName);
            DoGet.getInstance().sendPost("/packageService/getGoodsById", dic, (res) =>
            {
                if (res == null) return;
                callback((JObject)res);

            });
        }
        /**发布竞拍*/
        public void pubJingPai(JObject a, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", a["Id"]);
            dic.Add("num", a["num"]);
            dic.Add("price", a["price"]);
            dic.Add("priceType", a["priceType"]);
            dic.Add("jptype", a["jptype"]);
            DoGet.getInstance().sendPost("/shopService/pubJingPai", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(690);
                    callback();
                }
                else
                {
                    msgCode.showMsg(691);
                }
            });
        }
        /**查询正在参与竞拍中的商品*/
        public void findJingPaiByName(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/shopService/findJingPaiByName", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**获取竞拍列表*/
        public void findJingPai(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/shopService/findJingPai", dic, (res) =>
            {
                callback((JObject)res);
            });
        }

        /**查看期货当前投资记录*/
        public void GetQiHuoTZ(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/investService/getQihuoTZ", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**获取期货往期行情 */
        public void GetQiHuoHQ(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/investService/getQihuoHQ", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**0锻造宝石1精炼宝石2镶嵌宝石3修复宝石4黑洞陨石5天晶石6圣锻皇宝石*/
        public void QiHuo(JObject msg, Action callback)
        {
            if ((int)face.roleInterface.getRole()["lever"] < 30)
            {
                msgCode.showMsg(613);
                return;
            }
            int bet = (int)msg["bet"];
            int moneyType = (int)msg["moneyType"];
            string value = msg["value"].ToString();
            if (moneyType == 0)
            {
                JObject g = getPlayerGoodsByKey(value);
                if (g == null || (int)g["num"] <= 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            else
            {
                int tale = (int)face.roleInterface.getRole()["attr"]["msg"]["tale"];
                if (tale - int.Parse(value) < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("bet", bet);
            dic.Add("moneyType", moneyType);
            dic.Add("value", value);
            DoGet.getInstance().sendPost("/investService/investQihuo", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    if (moneyType == 0)
                    {
                        cutPlayerGoodsNumByKey(value + "", 1);
                    }
                    else
                    {
                        face.roleInterface.updateMoney(-int.Parse(value), 1);
                    }
                    msgCode.showMsg(700);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(701);
                }
                else
                {
                    msgCode.showMsg(696);
                }
                callback();
            });
        }
        /**查看大盘当前投资记录*/
        public void GetDaPanTZ(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/investService/getDapanTZ", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**获取大盘往期行情 */
        public void GetDaPanHQ(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/investService/getDapanHQ", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        /**大盘投资*/
        public void DaPan(JObject msg, Action callback)
        {
            if ((int)face.roleInterface.getRole()["lever"] < 30)
            {
                msgCode.showMsg(613);
                return;
            }
            int bet = (int)msg["bet"];
            int moneyType = (int)msg["moneyType"];
            string value = msg["value"].ToString();
            if (moneyType == 0)
            {
                JObject g = getPlayerGoodsByKey(value);
                if (g == null || (int)g["num"] <= 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            else
            {
                int tale = (int)face.roleInterface.getRole()["attr"]["msg"]["tale"];
                if (tale - int.Parse(value) < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("bet", bet);
            dic.Add("moneyType", moneyType);
            dic.Add("value", value);
            DoGet.getInstance().sendPost("/investService/investDapan", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    if (moneyType == 0)
                    {
                        cutPlayerGoodsNumByKey(value + "", 1);
                    }
                    else
                    {
                        face.roleInterface.updateMoney(-int.Parse(value), 1);
                    }
                    msgCode.showMsg(700);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(701);
                }
                else
                {
                    msgCode.showMsg(696);
                }
                callback();
            });
        }
        /**从仓库取出 */
        public void getWarehouse(string Id, Action callback)
        {
            JArray list = this.getGoodsFromCache();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString() == Id)
                {
                    list[p]["pos"] = 0;
                    list[p]["num"] = 0;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Id", Id);
                    DoGet.getInstance().sendPost("/packageService/opWarehouse", dic, (res) =>
                    {
                        this.savePlayerGoods((JObject)list[p]);
                        callback();
                    });
                    break;
                }
            }
        }
        /**请求丢弃道具 */
        public void reqGiveUpGoods(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/packageService/giveUpGoods", dic, (res) =>
            {
                JArray list = this.getGoodsFromCache();
                for (int p = 0; p < list.Count; p++)
                {
                    if (list[p]["Id"].ToString() == Id)
                    {
                        this.cutPlayerGoodsNum(Id, (int)list[p]["num"]);
                        break;
                    }
                }
                callback();
            });
        }
        /**放入仓库 */
        public void putWarehouse(string Id, Action callback)
        {
            JObject a = face.goodsInterface.getBBNAndCKN();
            //判断仓库是否满20个
            if (this.getCkGoods().Count >= (int)a["ckn"])
            {
                msgCode.showMsg(830);
                return;
            }
            JArray list = this.getGoodsFromCache();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString() == Id)
                {
                    list[p]["pos"] = 1;
                    list[p]["num"] = 0;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Id", Id);
                    DoGet.getInstance().sendPost("/packageService/opWarehouse", dic, (res) =>
                    {
                        this.savePlayerGoods((JObject)list[p]);
                        callback();
                    });
                    break;
                }
            }
        }
        /**从背包中获取装备技能**/
        public JArray getEquipSkill()
        {
            JArray list = new JArray();
            JArray gs = this.getAllGoods();
            for (int g = 0; g < gs.Count; g++)
            {
                int k = (int)gs[g]["key"];
                if (k >= 3600 && k < 3609)
                {
                    Goods a = (Goods)this.getGoodsMsgByKey(gs[g]["key"].ToString());
                    gs[g]["name"] = a.name.ToString();
                    list.Add(gs[g]);
                }
            }
            return list;
        }
        /**从背包中获取刻印**/
        public JArray getKeyin()
        {
            JArray list = new JArray();
            JArray gs = this.getAllGoods();
            for (int g = 0; g < gs.Count; g++)
            {
                int k = (int)gs[g]["key"];
                if (k >= 10900000 && k < 10900057)
                    list.Add(gs[g]);
            }
            return list;
        }
        /**将装备放入背包 */
        public void equipToPackage(JObject equip)
        {
            /*JArray list = this.getGoodsFromCache();
            list.Add(equip);
            dbHandle.save("playerGoods", list);*/
            this.savePlayerGoods(equip);
        }
        /**获取锻造宝石*/
        public JArray getForingBaoshi()
        {
            JArray arr = new JArray();
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["key"].ToString().Equals("1008"))
                {
                    JObject a = new JObject();
                    a.Add("name", "初锻宝石");
                    a.Add("key", obj["key"].ToString());
                    arr.Add(a);
                }
                else if (obj["key"].ToString().Equals("1009"))
                {
                    JObject a = new JObject();
                    a.Add("name", "锻造宝石");
                    a.Add("key", obj["key"].ToString());
                    arr.Add(a);
                }
                else if (obj["key"].ToString().Equals("1010"))
                {
                    JObject a = new JObject();
                    a.Add("name", "精锻宝石");
                    a.Add("key", obj["key"].ToString());
                    arr.Add(a);
                }
                else if (obj["key"].ToString().Equals("1011"))
                {
                    JObject a = new JObject();
                    a.Add("name", "锻皇宝石");
                    a.Add("key", obj["key"].ToString());
                    arr.Add(a);
                }
                else if (obj["key"].ToString().Equals("1012"))
                {
                    JObject a = new JObject();
                    a.Add("name", "圣锻皇宝石");
                    a.Add("key", obj["key"].ToString());
                    arr.Add(a);
                }
            }
            return arr;
        }

        /**购买商品 */
        public void buyGoods(string key, int num, int moneyType, string zkKey, Action<bool> callback)
        {
            if (num <= 0)
            {
                callback(false);
                return;
            }
            //Debug.Log(moneyType);
            GoodsDes g = (GoodsDes)this.getGoodsMsgByKey(key);
            int singlePrice = 0;
            if (g.buyList != null)
            {
                singlePrice = g.getPrice(moneyType);
            }
            //Debug.Log(singlePrice);
            //JArray pos = (JArray)g["rule"]["buyRule"]["pos"];
            string k = null;
            if (moneyType == 0)
            {
                k = "gold";
            }
            else if (moneyType == 1)
            {
                k = "tale";
            }
            else if (moneyType == 2)
            {
                k = "yp";
            }
            else if (moneyType == 3)
            {
                k = "jf";
            }
            else if (moneyType == 4)
            {
                k = "wxz";
            }
            else if (moneyType == 5)
            {
                k = "bg";
            }
            else if (moneyType == 11)
            {
                k = "jungong";
            }
            else
            {
                Debug.Log("不存在的货币类型" + moneyType);
                return;
            }
            JObject r = face.roleInterface.getRole();
            float zkK = 1;
            if (zkKey != null)
            {
                JObject a = face.goodsInterface.getPlayerGoodsByKey(zkKey);
                this.cutPlayerGoodsNum(a["Id"].ToString(), 1);
                zkK = ((int)(zkKey.ToCharArray()[3]) + 1) * 0.1f;
            }
            Debug.Log("支付前：" + r["attr"]["msg"][k]);
            Debug.Log("单价：" + singlePrice);
            long v = (long)r["attr"]["msg"][k] - (long)(num * singlePrice * zkK);
            Debug.Log("总价价：" + num * singlePrice * zkK);
            Debug.Log("支付后：" + v);
            if (v < 0)
            {
                msgCode.showMsg(634);
                callback(false);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("num", num);
            dic.Add("moneyType", moneyType);
            dic.Add("zkKey", zkKey);
            DoGet.getInstance().sendPost("/shopService/buyGoods", dic, (res) =>
            {
                if (res != null)
                {
                    face.roleInterface.updateMoney(-(int)(num * singlePrice * zkK), moneyType);
                    face.rewardInterface.saveRewards((JArray)res);
                    msgCode.showMsg(692);
                    callback(true);
                }
                else
                {
                    msgCode.showMsg(640);
                    callback(false);
                }
            });
        }
        /**获取可邮的附件*/
        public JArray getEnclosure()
        {
            JArray list = this.getAllGoods();
            JArray arr = new JArray();
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (face.emailInterface.isNoAllowedSend(a))
                {
                    continue;
                }
                a.Add("nowNum", 0);
                a.Add("enType", 0);
                arr.Add(a);
            }
            return arr;
        }
        /**复制道具,邮件附加,要求传入的一定都有Id */
        public void copyGoods(JObject playerGoods)
        {
            JObject nk = this.getBBNAndCKN();
            if (this.getAllGoods().Count >= (int)nk["bbn"])
            {
                msgCode.showMsg(640);
                return;
            }
            this.savePlayerGoods(playerGoods);
        }
        private bool isAllowedUse(string key)
        {
            if (strUtils.isMatch(key, "1012([0-9]{4})"))
            {//王者遗产藏宝图
                return true;
            }
            string[] arr = new string[]{

                "10000000", "10000001", "10000002",
                "10000029", "10000030", "10000031", "10000032",
                "10000112", "10000125", "10000126",
                "10000133", "10000134", "10000135", "10000136", "10000137", "10000138", "10000139", "10000140", "10000141","10000142",
                "10000175", "10000176", "10000177", "10000178", "10000181", "10000182", "10000183", "10000184", "10000187", "10000189",
                "10000201", "10000208", "10000209", "10000210",
                "10000213", "10000214", "10000215", "10000216", "10000217", "10000220","10000233",
                "10000237","10000238","10000239","10000240", "10000241",
                "10000242","10000243","10000244","10000245","10000246",
                "10000257", "10000258", "10000259", "10000260", "10000261", "10000262", "10000263","10000264","10000265",
                "10000267","10000268","10000269","10000270","10000271","10000272","10000273","10000274","10000275",
                "10000276","10000277","10000278","10000279","10000280","10000281","10000282","10000283","10000288",
                "10000285","10000286","10000292","10000294","10000302",
            };
            foreach (string a in arr)
            {
                if (a.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        /**使用道具 */
        public void userGoods(string gdKey, int num, Action callback = null)
        {
            JObject g = face.goodsInterface.getPlayerGoodsByKey(gdKey);
            if (g == null)
            {
                msgCode.showMsg(637);
                return;
            }
            this.userGoods(g, num, callback);
        }
        public void userGoods(JObject target, int num, Action callback = null)
        {
            if (num <= 0) return;
            if (target["key"] == null)
            {
                Debug.Log("需要key");
                return;
            }
            if (!isAllowedUse(target["key"].ToString()))
            {
                msgCode.showMsg(1023);
                Debug.Log("该道具不允许使用");
                return;
            }
            //背包是否已满
            if (this.isOverSum())
            {
                msgCode.showMsg(640);
                return;
            }
            //验证数量（注意绑定和非绑定的数量要合在一起）
            if (!isEnoughInPackAndTip(target["key"].ToString(), num))
            {
                return;
            }
            //宠物口粮需要验证
            if (target["key"].ToString().Equals("10000029") || target["key"].ToString().Equals("10000030") ||
                target["key"].ToString().Equals("10000031") || target["key"].ToString().Equals("10000032"))
            {
                JObject pet = face.petInterface.getIsFightPet();
                if (pet == null || ((int)pet["growLv"] < 6 && (int)pet["lever"] >= 100) || (int)pet["lever"] >= 110)
                {
                    if (pet == null) msgCode.showMsg(628);
                    else msgCode.showMsg(656);
                    return;
                }
            }
            //判断是否在某个场景
            if (target["key"].ToString().Equals("10000125") || target["key"].ToString().Equals("10000126") ||
                target["key"].ToString().Equals("10000181") || target["key"].ToString().Equals("10000182"))
            {
                if (!face.roleInterface.isInMap("cwly"))
                {
                    msgCode.showMsg(987);
                    return;//宠物乐园
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", target["Id"].ToString());
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/packageService/userGoods", dic, (res) =>
            {
                if (res != null)
                {
                    Action ac = () =>
                    {
                        JObject j = new JObject();
                        j.Add("key", target["key"].ToString());
                        JObject msg = new JObject();
                        msg.Add("res", (JToken)res);
                        msg.Add("num", num);
                        j.Add("msg", msg);
                        //todo:很多道具的回调未完善
                        eventsUtils.dispatchEvent("userGoods", j);
                        if (callback != null) callback();
                    };
                    //存在未使用成功的情况直接调用使用结果（r<=0）
                    if (strUtils.isNumber(res))
                    {
                        if (int.Parse(res.ToString()) <= 0)
                        {
                            ac();
                            return;
                        }
                    }
                    this.cutPlayerGoodsNum(target["Id"].ToString(), num);
                    ac();

                }
                else
                {
                    msgCode.showMsg(639);
                }
            });
        }
        /**是否超出背包数量
         addNum:额外加addNum个，看是否超出背包数量
         */
        public bool isOverBBSum(int addNum)
        {
            JArray list = this.getGoodsFromCache();
            int bb = 0;
            int ck = 0;
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if ((int)a["pos"] == 1)
                {
                    ck += 1;
                }
                else
                {
                    bb += 1;
                }
            }
            JObject nk = this.getBBNAndCKN();
            if ((bb + addNum) >= (int)nk["bbn"])
            {
                return true;
            }
            return false;
        }
        /**是否超过限制的100个道具 */
        public bool isOverSum()
        {
            JArray list = this.getGoodsFromCache();
            int bb = 0;
            int ck = 0;
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if ((int)a["pos"] == 1)
                {
                    ck += 1;
                }
                else
                {
                    bb += 1;
                }
            }
            JObject nk = this.getBBNAndCKN();
            if (bb >= (int)nk["bbn"] || (bb + ck) >= ((int)nk["bbn"] + (int)nk["ckn"]))
            {
                return true;
            }
            return false;
        }

        /**减少道具数量 （只会消耗id相同的道具）*/
        public void cutPlayerGoodsNum(string Id, int num)
        {
            JObject obj = new JObject();
            obj.Add("Id", Id);
            obj.Add("num", -num);
            this.savePlayerGoods(obj);
        }
        /**使用这个会先消耗绑定的道具，然后再消耗非绑定的道具*/
        public void cutPlayerGoodsNumByKey(String key, int num)
        {
            JObject obj = new JObject();
            obj.Add("key", key);
            obj.Add("num", -num);
            this.savePlayerGoods(obj);
        }
        /**获取背包中壁*/
        public JArray getBi()
        {
            JArray list = this.getGoodsFromCache();
            JArray aList = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["key"].ToString().Equals("10000144") || obj["key"].ToString().Equals("10000145") ||
                    obj["key"].ToString().Equals("10000146") || obj["key"].ToString().Equals("10000147"))
                {
                    aList.Add(obj);
                }
            }
            return aList;
        }
        /**缓存玩家物品
         * 对于非装备类型会先消耗绑定的物品
         */
        public void savePlayerGoods(JObject goods)
        {
            //出现key相同但Id不相同的情况
            JArray js = this.getGoodsFromCache();
            //1.先判断是否为消耗道具
            if ((int)goods["num"] < 0)
            {
                int need = (int)goods["num"];
                //装备类型道具不允许叠加
                bool isEquipType = false;
                if (goods["Id"] != null)
                {
                    for (int i = 0; i < js.Count; i++)
                    {
                        JObject j = (JObject)js[i];
                        string key = j["key"].ToString();
                        if ((j["Id"].ToString().Equals(goods["Id"].ToString())))
                        {
                            if (face.equipInterface.isEquip(key) ||
                                face.equipInterface.isKeyin(key) ||
                                face.equipInterface.isPetEquip(key))
                            {
                                isEquipType = true;
                            }

                            //假如删除的不是一件装备，传入的是id，而后面的判断，就会导致有问题 
                            if (goods["key"] == null)
                            {
                                goods.Add("key", j["key"]);
                            }
                            break;
                        }
                    }
                }
                //对于装备、刻印需要id，其他则只需要key
                if (isEquipType)
                {
                    for (int i = 0; i < js.Count; i++)
                    {
                        JObject j = (JObject)js[i];
                        if (j["Id"].ToString().Equals(goods["Id"].ToString()))
                        {
                            int n = (int)j["num"] + (int)goods["num"];
                            j["num"] = n;
                            if ((int)j["num"] < 0)
                            {
                                Debug.Log("装备类型的道具数量不够");
                                return;
                            }
                            else if ((int)j["num"] == 0)
                            {
                                js.RemoveAt(i);
                            }
                            break;
                        }
                    }
                    dbHandle.save("playerGoods", js);
                    this.noticeUpdateUI();
                    return;
                }
                //非装备类型的处理（分有id和有key）
                //对于有id的只会消耗该id的道具，对于有key的先消耗绑定后消耗未绑定的道具
                if (goods["Id"] != null)
                {
                    for (int i = 0; i < js.Count; i++)
                    {
                        JObject j = (JObject)js[i];
                        string Id = j["Id"].ToString();
                        int num = (int)j["num"];
                        //Debug.Log(goods);
                        if (Id.Equals(goods["Id"].ToString()))
                        {
                            //当num<0说明id对应的不够，当num=0说明刚好消耗完绑定的
                            num = num + (int)goods["num"];
                            j["num"] = num;
                            if (num <= 0)
                            {
                                //消耗完毕或者不够时就要移除
                                js.RemoveAt(i);
                                need = num;
                            }
                            else
                            {
                                //足够消耗
                                need = 0;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    //先消耗绑定的
                    for (int i = 0; i < js.Count; i++)
                    {
                        JObject j = (JObject)js[i];
                        int isBind = (int)j["isBind"];
                        string key = j["key"].ToString();
                        int num = (int)j["num"];
                        //Debug.Log(goods);
                        if (key.Equals(goods["key"].ToString()) && isBind == 1)
                        {
                            //当num<0说明绑定的不够，仍需要消耗非绑定的。当num=0说明刚好消耗完绑定的
                            num = num + (int)goods["num"];
                            j["num"] = num;
                            if (num <= 0)
                            {
                                //消耗完毕或者不够时就要移除
                                js.RemoveAt(i);
                                need = num;
                            }
                            else
                            {
                                //足够消耗
                                need = 0;
                            }
                            break;
                        }
                    }
                    //再消耗不绑定的
                    if (need < 0)
                    {
                        for (int i = 0; i < js.Count; i++)
                        {
                            JObject j = (JObject)js[i];
                            int isBind = (int)j["isBind"];
                            string key = j["key"].ToString();
                            int num = (int)j["num"];
                            if (key.Equals(goods["key"].ToString()) && isBind == 0)
                            {
                                num = num + need;
                                j["num"] = num;
                                if (num <= 0)
                                {
                                    //消耗完毕或者不够时就要移除
                                    js.RemoveAt(i);
                                    need = num;
                                }
                                else
                                {
                                    //足够消耗
                                    need = 0;
                                }
                                break;
                            }
                        }
                    }
                }

                if (need == 0)
                {
                    //能被消耗
                    dbHandle.save("playerGoods", js);
                    //通知刷新背包
                    this.noticeUpdateUI();
                    return;
                }
                else
                {
                    //不足数量
                    Debug.Log("没有足够数量");
                    return;
                }
            }
            //2.其他转移、创建的情况
            if (goods["Id"] == null || goods["isBind"] == null)
            {
                Debug.Log("没有Id、isBind属性");
                return;
            }
            //几种情况：
            // 1.创建（背包内有的就会沿用【由key跟isBind选取，这样就保证isBind会有两个不同的id】）
            // 2.查找后修改部分属性（id跟背包道具的一致）
            bool b = false;
            for (int i = 0; i < js.Count; i++)
            {
                JObject j = (JObject)js[i];
                //因绑定和非绑定的id传入之前已经区分开了，所以直接用id判断
                if (j["Id"].ToString().Equals(goods["Id"].ToString()))
                {
                    //注意：修改一下属性时，最后需要设置数量为0，否则会导致叠加
                    int n = (int)j["num"] + (int)goods["num"];
                    j["num"] = n;
                    //当获得新道具时可能把原先存在仓库的道具给置入背包
                    if (goods["pos"] != null && (int)goods["num"] == 0)
                    {
                        //只有数量为0，纯属仓库背包相互转移时才按道具的pos属性，否则不变
                        j["pos"] = goods["pos"];
                    }
                    //保存损毁
                    if (goods["isBad"] != null &&
                            (int)j["isBad"] != (int)goods["isBad"])
                    {
                        j["isBad"] = goods["isBad"];
                    }
                    if (goods["forging"] != null)
                    {
                        j["forging"] = goods["forging"];
                    }
                    if (goods["inlay"] != null)
                    {
                        j["inlay"] = goods["inlay"];
                    }
                    if (goods["potential"] != null)
                    {
                        j["potential"] = goods["potential"];
                    }
                    if (goods["randomAttr"] != null)
                    {
                        j["randomAttr"] = goods["randomAttr"];
                    }
                    if (goods["isBind"] != null)
                    {
                        j["isBind"] = goods["isBind"];
                    }
                    if (goods["bdbs"] != null)
                    {
                        j["bdbs"] = goods["bdbs"];
                    }
                    if (goods["kybs"] != null)
                    {
                        j["kybs"] = goods["kybs"];
                    }
                    if (goods["key"] != null)
                    {
                        j["key"] = goods["key"];
                    }
                    if (goods["kxlv"] != null)
                    {
                        j["kxlv"] = goods["kxlv"];
                    }
                    if (goods["czmb"] != null)
                    {
                        j["czmb"] = goods["czmb"];
                    }
                    if (goods["xqbs"] != null)
                    {
                        j["xqbs"] = goods["xqbs"];
                    }
                    b = true;
                    break;
                }
            }
            if (!b)
            {
                //不存在时就直接添加
                js.Add(goods);
            }
            dbHandle.save("playerGoods", js);
            //通知刷新背包
            this.noticeUpdateUI();
        }
        private void saveGoods(JArray js)
        {
            dbHandle.save("playerGoods", js);
            //通知刷新背包
            this.noticeUpdateUI();
        }
        public void noticeUpdateUI()
        {
            JObject j = new JObject();
            j.Add("callback", "818");
            eventsUtils.dispatchEvent("ws", j);
        }
        /**获取刻印基础属性 */
        public JObject getKyAttr(string key)
        {
            //1力量、2敏捷、3耐力、4智力、5精神、6血量、7蓝量、8物攻、9法攻、10物防、11法防、
            //12命中、13闪避、14暴击、15速度、16爆抗、17血抗、18混抗、19昏抗
            int i = int.Parse(key.Substring(4));
            string[] arr = {
        "ll", "mj", "nl", "zl", "js", "max_xue", "max_lan", "wg", "fg", "wf", "ff",
        "mz", "sd", "bj", "css", "bjkx", "lxkx", "hlkx", "hskx"
        };
            int[] brr = {
        30, 15, 30, 30, 60, 200, 200, 60, 60, 80, 80,
        50, 30, 60, 30, 100, 100, 100, 100
        };
            JObject obj = new JObject();
            if (i >= 19 && i < 38)
            {
                obj.Add("k", arr[i - 19]);
                obj.Add("v", brr[i - 19] * 1.5f);
            }
            else if (i >= 38 && i < 57)
            {
                obj.Add("k", arr[i - 38]);
                obj.Add("v", brr[i - 38] * 2f);
            }
            else
            {
                obj.Add("k", arr[i]);
                obj.Add("v", brr[i]);
            }
            return obj;
        }
        public JObject getPlayerGoodsById(string Id)
        {
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if ((obj["Id"] + "").Equals(Id))
                {
                    return obj;
                }
            }
            return null;
        }
        public JObject getPlayerGoodsByKey(string key)
        {
            JArray list = this.getGoodsFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if ((obj["key"] + "").Equals(key))
                {
                    return obj;
                }
            }
            return null;
        }
        /**带提示的验证*/
        public bool isEnoughInPackAndTip(string key, int sum, bool isTip = true)
        {
            if (isEnoughInPack(key, sum)) return true;
            if (isTip) msgCode.showMsg(637);
            return false;
        }
        /**获取距离所需物品的数量的差值*/
        public int getChaZhiInPack(string key, int sum)
        {
            JArray list = this.getAllGoods();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["key"].ToString() == key)
                {
                    num += (int)obj["num"];
                }
            }
            if (num >= sum) return 0;//足够
            return sum - num;//不足，返回相差多少
        }
        /**判断道具在背包中的数量是否足够（非仓库）,如果是使用id的就不需要统计绑定和非绑定的数量*/
        private bool isEnoughInPack(string key, int sum)
        {
            JArray list = this.getAllGoods();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["key"].ToString() == key)
                {
                    num += (int)obj["num"];
                }
            }
            if (num >= sum) return true;
            return false;
        }
        /**获取背包中所有的道具*/
        public JArray getAllGoods()
        {
            JArray list = this.getGoodsFromCache();
            JArray aList = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["pos"].ToString().Equals("0"))
                {
                    aList.Add(obj);
                }
            }
            return aList;
        }
        /**由key获取在背包中的物品*/
        public JObject getInBbOfGoodsByKey(string key)
        {
            JArray list = this.getGoodsFromCache();
            JArray aList = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["pos"].ToString().Equals("0") && key.Equals(obj["key"].ToString()))
                {
                    return obj;
                }
            }
            return null;
        }
        /**
         prev取key的前四位来确定道具类型，如1001表示装备
         */
        public JArray getNoWarehouseAnyTypeGoods(string prev)
        {
            JArray list = this.getGoodsFromCache();
            JArray aList = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (obj["pos"].ToString().Equals("0") && obj["key"].ToString().Substring(0, 4).Equals(prev))
                {
                    aList.Add(obj);
                }
            }
            return aList;
        }

        public JArray getCkGoods()
        {
            JArray list = this.getGoodsFromCache();
            JArray aList = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = (JObject)list[i];
                if (int.Parse(obj["pos"] + "") == 1)
                {
                    aList.Add(obj);
                }
            }
            return aList;
        }
        private JArray getGoodsFromCache()
        {
            JArray list = dbHandle.get<JArray>("playerGoods");
            if (list == null) return new JArray();
            return list;
        }
        public void initGoods(JToken gMsg)
        {
            dbHandle.save("playerGoods", gMsg["package"]);
            JObject a = new JObject();
            a.Add("bbn", gMsg["bbn"]);
            a.Add("ckn", gMsg["ckn"]);
            a.Add("tale", gMsg["tale"]);
            dbHandle.save("playerBbn&Ckn", a);
        }
        public void saveBbnOrCknOrTale(int type, int num)
        {
            string key = null;
            if (type == 0) key = "bbn";
            else if (type == 1) key = "ckn";
            else if (type == 2) key = "tale";
            JObject a = getBBNAndCKN();
            a[key] = num;
            saveBBNAndCKN(a);
        }
        public JObject getBBNAndCKN()
        {
            return dbHandle.get<JObject>("playerBbn&Ckn");
        }
        public void saveBBNAndCKN(JObject d)
        {
            dbHandle.save("playerBbn&Ckn", d);
        }
        /**获取道具*/
        public JArray getAllowBuyGoodsList(int type)
        {
            int[] krr = { };
            //为了更快查询，对出售的道具设置索引
            if (type == 0)
            {
                List<int> al = new List<int>();
                for (int i = 10000000; i < 10000100; i++)
                {
                    if (getGoodsMsgByKey(i + "") == null) break;
                    al.Add(i);
                }
                krr = al.ToArray();
            }
            else if (type == 1)
            {
                int[] arr0 = {
                1004,1033,1130,20060001
            };
                krr = arr0;
            }
            else if (type == 2)
            {
                int[] arr0 = {
                1016,20060000
            };
                krr = arr0;
            }
            else if (type == 3)
            {
                int[] arr0 = {
                1008
            };
                //一级镶嵌
                int[] arr1 = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    arr1[i] = 4000 + i;
                }
                krr = strUtils.concatArr(arr0, arr1);
            }
            else if (type == 4)
            {
                int[] arr0 = {
                1042
            };
                krr = arr0;
            }
            else if (type == 5)//帮贡
            {
                List<int> al = new List<int>();
                al.Add(1013);
                al.Add(1009);
                al.Add(1010);
                al.Add(1011);
                for (int i = 11990000; i < 11990036; i++)
                {
                    al.Add(i);
                }
                krr = al.ToArray();
            }



            JArray arr = new JArray();
            for (int i = 0; i < krr.Length; i++)
            {
                Goods g = (Goods)this.getGoodsMsgByKey(krr[i] + "");
                JArray list = g.buyList;
                JObject item = new JObject();
                item.Add("key", g.key.ToString());
                item.Add("name", g.name.ToString());
                item.Add("des", g.des);
                item.Add("priceType", type);
                item.Add("icon", g.icon);
                item.Add("num", 1);
                item.Add("quality", g.quality);
                for (int j = 0; j < list.Count; j++)
                {
                    if ((int)list[j]["moneyType"] == type)
                    {
                        item.Add("price", (int)list[j]["value"]);
                        arr.Add(item);
                        break;
                    }
                }
            }

            return arr;
        }
        
        public object getGoodsMsgByKey(string key)
        {
            if (strUtils.isMatch(key, "1000([0-9]{4})"))
            {
                Goods res = new Goods(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1001([0-9]{8})"))
            {
                Equip res = new Equip(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1002([0-9]{8})"))
            {
                Skill res = new Skill(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1003([0-9]{4})"))
            {
                Baoshi res = new Baoshi(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1004([0-9]{8})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1005([0-9]{4})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1006([0-9]{4})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1007([0-9]{4})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1008([0-9]{8})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1009([0-9]{8})"))
            {
                return null;
            }
            else if (strUtils.isMatch(key, "1010([0-9]{4})"))
            {
                MoBan res = new MoBan(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1011([0-9]{4})"))
            {
                PetDan res = new PetDan(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1012([0-9]{4})"))
            {
                CangBaoTu res = new CangBaoTu(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1013([0-9]{4})"))
            {
                ZhongZi res = new ZhongZi(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1014([0-9]{4})"))
            {
                ShenFu res = new ShenFu(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1015([0-9]{4})"))
            {
                ZhiFuCaiLiao res = new ZhiFuCaiLiao(key);
                return res.getMsg();
            }
            else if (strUtils.isMatch(key, "1016([0-9]{4})"))
            {
                PetEquip res = new PetEquip(key);
                return res.getMsg();
            }
            return null;
        }
    }
}
