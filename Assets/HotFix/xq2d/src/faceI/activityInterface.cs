using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface activityInterface
    {
        public void gainB(Action callback);
        public string[] getLimitTimeAcList();
        public string[] getNoLimitTimeAcList();
        public string[] getZhouAcList();
        public string[] getElseAcList();
        public JObject getAcMsg(string key);
        public JArray keysToObj(string[] arr);
        public List<MultyMenu> getAcMenus(string key, string npcKey, modelMsgBind npc = null);

        public void getHappyProgress(Action<JObject> ac);
        public void putHappyProgress(int isRight);
        public void fightYhmk(string npcKey);
        public void pubZsl(int price, int priceType, string player, Action callback);
        public void gainZsl(JObject obj, Action ac);
        public void getZsl(int pageNum, Action<JObject> callback);
        public void findZslByName(Action<JObject> callback);
        public void getB(Action<JObject> callback);
        public void updateB(Action<int> callback);
        public void uploadB(string key, Action callback);
        public void viewUpB(Action<JArray> callback);
        public void qiangB(string name);
        public void putZPProgress(Action<int> ac);
        public void getZPProgress(Action<JObject> ac);
        public void createFightByWorldBoss();
        public void getMatchJjOpponent(Action<JObject> ac);
        public void pkByName(string name);
        public void updateMatch(Action ac);
        public void signUpWzy(string teamName);
        public void getWzyNameList(int pageNum, Action<object> callback);
        public void signUpWHJX();
        public void toFbMap(string fbKey, string mapKey, int tale);
        public void getMsTask(Action<string> callback);
        public void qiaodaxm();
        public void userGx(string npcKey);
        public void tiaozhanMs(string npcKey);
        public void viewStatus(string npcKey, Action<int> callback);

        public void getJcMsLv(Action<JObject> callback);

        public void createFightByBlxt(string npcKey);
        public void getOrderBzqj(int type, Action<JObject> callback);
        public void getOrderWzy(int pageNum, Action<JObject> callback);
        public void signUpJingji(Action callback);
        public void getJingjiOrder(int pageNum, Action<JObject> callback);
        public void getOrderLv(int pageNum, Action<JObject> callback);
        public void getOrderLvByJob(int type, int pageNum, Action<JObject> callback);
        public void getOrderSez(int type, int pageNum, Action<JObject> callback);
        public void gainJinShouZhi(int type, Action callback);
        public void getSysMsg(Action callback);
        public void getCjwdTask(Action callback);
        public void viewCjwdTask(Action<JObject> callback);
        public void isJianxi(string npcKey, Action callback);
        public void viewCDTimes(int type, Action<JObject> callback);
        public void putCDProgress(int index, int type, Action callback);
        public void viewWzbzMsg(Action<JObject> callback);
        public void gainWzbz(JObject a, Action callback);
        public void qingxiWzbz(JObject a, int isFree, Action callback);
        public void getZjcm(Action callback);
        public void viewZjcmMapKey(Action<string> callback);
        public void intoHhbk(Action callback);
        public void searchHere(int index, Action<int> callback);
        public void intoShengSiMen(Action callback);
        public void chooseShengSiMen(int index, Action<int> callback);
        public void clearSiMen(Action<int> callback);
        public void openBoxFromSheng(int type, Action callback);
        public void intoShenLongMenFromSi(Action callback);
        public void openBoxFromSLM(int type, Action callback);
        public void getCybyTask(Action callback);
        public void getJyfyTask(int type, Action callback);
        public void shoushenji(Action callback);
        public void isHasMonsterByMpbwz(Action<int> callback);
        public void signDsx(Action callback);
        public void gainLLD(Action callback);
        public void composeJmDan(int type, Action callback);
        public void resetJm(Action callback);
        public void getMyFarmMsg(Action<JObject> callback);
        public void touCai(string playerName, string pos, Action callback);
        public void getPlayerFarmMsg(string playerName, Action<JObject> callback);
        public void gainZhongZi(Action callback);
        public void openFarmPos(string pos, Action callback);
        public void zhongzhi(string key, string pos, Action callback);
        public void chanchu(string pos, Action callback);
        public void zhaiqu(string pos, Action callback);
        public void getGangsMemberFarm(Action<JArray> callback);
        public void getMyFriendFarmMsg(Action<JArray> callback);
        public void huilu(Action callback);
        public void getDzfsbOrder(int pageNum, Action<JObject> callback);
        public void getFgLog(Action<JArray> callback);
        public void signBs(Action callback);
        public void recruit(Action callback);
        public void removeMatter(string playerName, Action callback);
        public void cancelBs(Action callback);
        public void apprenticeBs(string playerName, Action callback);
        public void agreeApprentice(string playerName, Action callback);
        public void reqSt(string playerName, Action callback);
        public void agreeBecomeStu(string playerName, Action callback);
        public void getStTask(string npckey, Action ac);
        public void cancelTongjiTask(Action callback);
        public void gainTongjiTask(Action<JObject> callback);
        public void getBiOrder(int i, Action<JObject> callback);
        public void exchangeByShuiJing(int type, Action callback);
        public void getLvGift(int type, Action callback);
        public void getQianDaoGift(Action callback);
        public void chooseSkillGain(int index, Action callback);
        public void petSklTwoToOne(JArray ens, Action callback);
        public void petSklThreeToOne(JArray ens, string key, Action callback);
        public void manSklThreeToOne(JArray ens, string key, Action callback);
        public void gainVipLvGift(int lv, Action callback);
        public void gainLeiJiDangGift(Action callback);
        public void gainLjdXianJueGift(Action callback);
        public void gainLjdGoldGift(Action callback);
        public void exchangeShenShou(int type, Action callback);
        public void exchangeQlfs(Action callback);
        public void getMenPaiTask(string npcKey, Action callback);
        public void gainHolidayGift(Action callback);
        public void exchangeXianDanLiBao(string key, Action callback);
        public void exchangeFbGd(int index, Action callback);
        public void exchangeQNXT(Action callback);
        public void exchangePetEquip(string key, Action callback);
        public void countMoney(Action callback);
        public void getGoldOrder(int type, int pageNum, Action<JObject> callback);
        public void addVipValue(int jf, string playerName, Action callback);
        public void exchangBySyb(string key, int num, Action callback);
        public void dhSfByCaiLiao(string key, Action callback);
        public void useShenFu(string key, Action callback);
        public void exchangByMlcy(Action callback);
        public void sklSpExchange(string xhKey, Action callback);
        public void shenYingExchange(string dhKey, Action callback);
        public void qdGainYuanBao(Action callback);
        public void yxfGainTong(Action callback);
        public void yxfGainShen(Action callback);
        public void openDsx(Action callback);
        public void xianYuanDanExchange(int type, Action callback);
        public void gjPetSklExchange(string dhKey, Action callback);
        public void openWzy(Action callback);
        public void openBz(Action callback);
        public void closeBz(Action callback);
        public void setCont(int type, int index, int isFeng, Action callback);
        public void cancelCont(int type, Action callback);
        public void openXxzd(Action callback);
        public void openXxzdMatch(Action callback);
        public void closeXxzd(Action callback);
        public void getXxzdOrder(int pageNum, Action<JObject> callback);
        public void addGoods(string key, int num, Action callback);
        public void addGoldValue(int type, int num, Action callback);
    }
    class activityInterfaceImpl : activityInterface
    {
        public void addGoods(string key, int num, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/vipService/addGoods", dic, (res) =>
            {
                msgCode.showMsg(200);
                face.rewardInterface.saveRewards((JArray)res);
                callback();
            });
        }
        public void addGoldValue(int type,int num, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/vipService/addGoldValue", dic, (res) =>
            {
                msgCode.showMsg(200);
                face.rewardInterface.saveRewards((JArray)res);
                callback();
            });
        }
        public void getXxzdOrder(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/xxzdService/getOrder", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void openXxzd(Action callback)
        {
            DoGet.getInstance().sendPost("/xxzdService/openAc", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void openXxzdMatch(Action callback)
        {
            DoGet.getInstance().sendPost("/xxzdService/openMatch", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void closeXxzd(Action callback)
        {
            DoGet.getInstance().sendPost("/xxzdService/closeAc", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void cancelCont(int type, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/investService/cancelCont", dic, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void setCont(int type, int index, int isFeng, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("index", index);
            dic.Add("isFeng", isFeng);
            DoGet.getInstance().sendPost("/investService/setCont", dic, (res) =>
            {

                msgCode.showMsg(200);
                callback();
            });
        }
        public void xianYuanDanExchange(int type, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000301", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/rewardService/xianYuanDanExchange", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000301", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gjPetSklExchange(string dhKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000303", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", dhKey);
            DoGet.getInstance().sendPost("/rewardService/gjPetSklExchange", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000303", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void closeBz(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/closeBz", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void openBz(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/openBz", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void openWzy(Action callback)
        {
            DoGet.getInstance().sendPost("/wzyService/openWzy", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void openDsx(Action callback)
        {
            DoGet.getInstance().sendPost("/dsxService/openDsx", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void qdGainYuanBao(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 7)
            {
                msgCode.showMsg(214);
                return;
            }
            DoGet.getInstance().sendPost("/fuliService/qdGainYuanBao", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(699);
                callback();
            });
        }
        public void yxfGainTong(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 7)
            {
                msgCode.showMsg(214);
                return;
            }
            DoGet.getInstance().sendPost("/fuliService/yxfGainTong", null, (res) =>
            {
                JObject rew = (JObject)res;
                if (rew.ContainsKey("ysf"))
                {
                    msgCode.showMsg(226, rew["ysf"]);
                }
                else if (rew.ContainsKey("zong"))
                {
                    msgCode.showMsg(227, rew["zong"]);
                }
                else
                {
                    face.rewardInterface.saveRewards((JArray)rew["list"]);
                    msgCode.showMsg(699);
                }
                callback();
            });
        }
        public void yxfGainShen(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 7)
            {
                msgCode.showMsg(214);
                return;
            }
            DoGet.getInstance().sendPost("/fuliService/yxfGainShen", null, (res) =>
            {
                JObject rew = (JObject)res;
                if (rew.ContainsKey("ysf"))
                {
                    msgCode.showMsg(226, rew["ysf"]);
                }
                else if (rew.ContainsKey("zong"))
                {
                    msgCode.showMsg(227, rew["zong"]);
                }
                else
                {
                    face.rewardInterface.saveRewards((JArray)rew["list"]);
                    msgCode.showMsg(699);
                }
                callback();
            });
        }
        public void shenYingExchange(string dhKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000293", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", dhKey);
            DoGet.getInstance().sendPost("/rewardService/shenYingExchange", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000293", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void sklSpExchange(string xhKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, 50))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", xhKey);
            DoGet.getInstance().sendPost("/rewardService/sklSpExchange", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 50);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangByMlcy(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000284", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/rewardService/exchangByMlcy", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000284", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void useShenFu(string key, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(key, 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/shenfuService/useShenFu", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, 1);
                JObject role = face.roleInterface.getRole();
                role["attr"]["shenfu"] = (JObject)res;
                face.roleInterface.saveRole(role);
                msgCode.showMsg(200);
                eventsUtils.dispatchWsEvent("111", null);
                callback();
            });
        }
        public void dhSfByCaiLiao(string key, Action callback)
        {
            ShenFu sf = (ShenFu)face.goodsInterface.getGoodsMsgByKey(key);
            JArray clList = sf.cailiaoList;
            for (int i = 0; i < clList.Count; i++)
            {
                JObject a = (JObject)clList[i];
                string k = a["key"].ToString();
                int n = (int)a["num"];
                if (!face.goodsInterface.isEnoughInPackAndTip(k, n))
                {
                    return;
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/shenfuService/dhSfByCaiLiao", dic, (res) =>
            {
                for (int i = 0; i < clList.Count; i++)
                {
                    JObject a = (JObject)clList[i];
                    string k = a["key"].ToString();
                    int n = (int)a["num"];
                    face.goodsInterface.cutPlayerGoodsNumByKey(k, n);
                }
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangBySyb(string key, int num, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000180", num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/rewardService/exchangBySyb", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000180", num);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void addVipValue(int jf, string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            dic.Add("jf", jf);
            DoGet.getInstance().sendPost("/vipService/addVipValue", dic, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getGoldOrder(int type, int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/sysService/getGoldOrder", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void countMoney(Action callback)
        {
            DoGet.getInstance().sendPost("/sysService/getGoldMsg", null, (res) =>
            {
                JObject d = (JObject)res;
                string str = "说明：当前-重启时为正数就是出现刷金\n" +
                "重启时元宝：" + d["befGoldSum"] +
                "\n当前元宝：" + d["goldSum"] +
                "\n当前-重启时：" +
                ((long)d["goldSum"] - (long)d["befGoldSum"]) +
                "\n重启时银两：" + d["befTaleSum"] +
                "\n当前银两：" + d["taleSum"] +
                "\n当前-重启时：" +
                ((long)d["taleSum"] - (long)d["befTaleSum"]);
                msgCode.showMsg(201, str);
                callback();
            });
        }
        public void exchangePetEquip(string key, Action callback)
        {
            int xhNum = 1;
            string xhKey = null;
            if (key.Equals("10160000"))
            {
                xhNum = 10; xhKey = "10000234";
            }
            else if (key.Equals("10160001"))
            {
                xhNum = 25; xhKey = "10000234";
            }
            else if (key.Equals("10160002"))
            {
                xhNum = 12; xhKey = "10000235";
            }
            else if (key.Equals("10160003"))
            {
                xhNum = 20; xhKey = "10000235";
            }
            Debug.Log(xhKey + "/" + xhNum);
            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, xhNum))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/rewardService/exchangePetEquip", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, xhNum);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangeQNXT(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000219", 5))
            {
                return;
            }
            DoGet.getInstance().sendPost("/rewardService/exchangeQNXT", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000219", 5);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangeFbGd(int index, Action callback)
        {
            int[] brr =
            {
                10,10,10,10,20,200,500
            };
            if (!face.goodsInterface.isEnoughInPackAndTip("10000236", brr[index]))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/rewardService/exchangeFbGd", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000236", brr[index]);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangeXianDanLiBao(string key, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(key, 3))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/rewardService/exchangeXianDanLiBao", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, 3);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainHolidayGift(Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if ((int)r["lever"] < 90)
            {
                msgCode.showMsg(613);
                return;
            }

            DoGet.getInstance().sendPost("/rewardService/gainHolidayGift", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getMenPaiTask(string npcKey, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            string mp = GameAttrConst.getMenPaiFromModels(r);
            if (mp == null)
            {
                msgCode.showMsg(976);
                return;
            }
            string subNpcKey = null;
            if (mp.Contains("zs")) subNpcKey = "10000457";
            else if (mp.Contains("fs")) subNpcKey = "10000463";
            else if (mp.Contains("fz")) subNpcKey = "10000469";
            if (!npcKey.Equals(subNpcKey))
            {
                msgCode.showMsg(614);
                return;
            }
            DoGet.getInstance().sendPost("/taskService/getMenPaiTask", null, (res) =>
            {
                face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { res.ToString() });
                face.taskInterface.createOneStartTask(res.ToString(), 2);
                msgCode.showMsg(612);
                callback();
            });
        }
        public void exchangeQlfs(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000188", 100))
            {
                return;
            }

            DoGet.getInstance().sendPost("/rewardService/exchangeQlfs", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000188", 100);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangeShenShou(int type, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000000", 20))
            {
                return;
            }
            string xhKey = null;
            if (type == 0) xhKey = "10000284";
            else if (type == 1) xhKey = "10000295";
            else if (type == 2) xhKey = "10000296";
            else if (type == 3) xhKey = "10000297";
            else if (type == 4) xhKey = "10000298";

            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/rewardService/exchangeShenShou", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000000", 20);
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainLjdGoldGift(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 3)
            {
                msgCode.showMsg(214);
                return;
            }

            DoGet.getInstance().sendPost("/vipService/gainLjdGoldGift", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainLjdXianJueGift(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 3)
            {
                msgCode.showMsg(214);
                return;
            }

            DoGet.getInstance().sendPost("/vipService/gainLjdXianJueGift", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainLeiJiDangGift(Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < 3)
            {
                msgCode.showMsg(214);
                return;
            }

            DoGet.getInstance().sendPost("/vipService/gainLeiJiDangGift", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainVipLvGift(int lv, Action callback)
        {
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv < lv)
            {
                msgCode.showMsg(214);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("lv", lv);
            DoGet.getInstance().sendPost("/vipService/gainVipLvGift", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**技能二换一*/
        public void petSklTwoToOne(JArray ens, Action callback)
        {
            int sum = 0;
            JArray list = new JArray();
            for (int i = 0; i < ens.Count; i++)
            {
                JObject en = (JObject)ens[i];
                if ((int)en["nowNum"] <= 0) return;
                if (!face.goodsInterface.isEnoughInPackAndTip(en["key"].ToString(), (int)en["nowNum"]))
                {
                    return;
                }
                JObject a = new JObject();
                a["key"] = en["key"].ToString();
                a["num"] = en["nowNum"].ToString();
                list.Add(a);
                sum += (int)a["num"];
            }
            if (sum != 2)
            {
                msgCode.showMsg(210);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("list", list);
            DoGet.getInstance().sendPost("/rewardService/petSklTwoToOne", dic, (res) =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    JObject en = (JObject)list[i];
                    face.goodsInterface.cutPlayerGoodsNumByKey(en["key"].ToString(), (int)en["num"]);
                }

                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**技能三换一*/
        public void petSklThreeToOne(JArray ens, string key, Action callback)
        {
            int sum = 0;
            JArray list = new JArray();
            for (int i = 0; i < ens.Count; i++)
            {
                JObject en = (JObject)ens[i];
                if ((int)en["nowNum"] <= 0) return;
                if (!face.goodsInterface.isEnoughInPackAndTip(en["key"].ToString(), (int)en["nowNum"]))
                {
                    return;
                }
                JObject a = new JObject();
                a["key"] = en["key"].ToString();
                a["num"] = en["nowNum"].ToString();
                list.Add(a);
                sum += (int)a["num"];
            }
            if (sum != 3)
            {
                msgCode.showMsg(210);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("list", list);
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/rewardService/petSklThreeToOne", dic, (res) =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    JObject en = (JObject)list[i];
                    face.goodsInterface.cutPlayerGoodsNumByKey(en["key"].ToString(), (int)en["num"]);
                }

                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**技能三换一*/
        public void manSklThreeToOne(JArray ens, string key, Action callback)
        {
            int sum = 0;
            JArray list = new JArray();
            for (int i = 0; i < ens.Count; i++)
            {
                JObject en = (JObject)ens[i];
                if ((int)en["nowNum"] <= 0) return;
                if (!face.goodsInterface.isEnoughInPackAndTip(en["key"].ToString(), (int)en["nowNum"]))
                {
                    return;
                }
                JObject a = new JObject();
                a["key"] = en["key"].ToString();
                a["num"] = en["nowNum"].ToString();
                list.Add(a);
                sum += (int)a["num"];
            }
            if (sum != 3)
            {
                msgCode.showMsg(210);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("list", list);
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/rewardService/manSklThreeToOne", dic, (res) =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    JObject en = (JObject)list[i];
                    face.goodsInterface.cutPlayerGoodsNumByKey(en["key"].ToString(), (int)en["num"]);
                }

                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void chooseSkillGain(int index, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000266", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/rewardService/chooseSkillGain", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000266", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getLvGift(int type, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }
            string mp = GameAttrConst.getMenPaiFromModels(r);
            if (mp == null)
            {
                msgCode.showMsg(976);
                return;
            }
            string job = GameAttrConst.getJobFromModels(r);
            if (job == null)
            {
                msgCode.showMsg(1024);
                return;
            }
            if ((type == 0 && lv < 30) || (type == 1 && lv < 40) || (type == 2 && lv < 50) ||
                (type == 3 && lv < 60) || (type == 4 && lv < 70) || (type == 5 && lv < 80) ||
                (type == 6 && lv < 90) || (type == 7 && lv < 100))
            {
                msgCode.showMsg(613);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/openServerAcService/getLvGift", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getQianDaoGift(Action callback)
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }
            string mp = GameAttrConst.getMenPaiFromModels(r);
            if (mp == null)
            {
                msgCode.showMsg(976);
                return;
            }
            string job = GameAttrConst.getJobFromModels(r);
            if (job == null)
            {
                msgCode.showMsg(1024);
                return;
            }
            DoGet.getInstance().sendPost("/openServerAcService/getQianDaoGift", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void exchangeByShuiJing(int type, Action callback)
        {
            int xhNum = 1;
            int num = 1;
            String key = null;
            if (type == 0)
            {//180梦幻水晶兑换潜力符石x1
                xhNum = 180; num = 1; key = "10000114";
            }
            else if (type == 1)
            {//200梦幻水晶兑换造化丹x2
                xhNum = 200; num = 2; key = "10000177";
            }
            else if (type == 2)
            {//300梦幻水晶兑换诱敌香草x3
                xhNum = 300; num = 3; key = "10000184";
            }
            else if (type == 3)
            {//500梦幻水晶兑换无敌药囊x1
                xhNum = 500; num = 3; key = "100110060003";
            }
            if (!face.goodsInterface.isEnoughInPackAndTip("10000199", xhNum))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/dmkjService/exchangeByShuiJing", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000199", xhNum);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getBiOrder(int i, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", i);
            DoGet.getInstance().sendPost("/cbService/getBiOrder", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void cancelTongjiTask(Action callback)
        {
            DoGet.getInstance().sendPost("/prisonService/cancelTask", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainTongjiTask(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/prisonService/gainTask", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void getStTask(string npckey, Action ac)
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            string taskKey = null;
            if (npckey.Equals("10000586") && lv >= 15 && lv < 20) taskKey = "3123";
            else if (npckey.Equals("10000587") && lv >= 20 && lv < 25) taskKey = "3124";
            else if (npckey.Equals("10000588") && lv >= 25 && lv < 30) taskKey = "3125";
            else if (npckey.Equals("10000589") && lv >= 30 && lv < 35) taskKey = "3126";
            else if (npckey.Equals("10000590") && lv >= 35 && lv < 40) taskKey = "3127";
            else if (npckey.Equals("10000591") && lv >= 40 && lv < 45) taskKey = "3128";
            else if (npckey.Equals("10000591") && lv >= 45 && lv < 50) taskKey = "3129";
            if (taskKey == null)
            {
                msgCode.showMsg(1016);
                return;
            }

            DoGet.getInstance().sendPost("/stService/getStTask", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(870);
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(871);
                }
                else
                {
                    //缓存创建的任务
                    JObject progress = new JObject();
                    JObject target = new JObject();
                    target.Add("num", 0);
                    progress.Add("target", target);
                    face.taskInterface.savePlayerTask(res.ToString(), 0, progress, 2);
                }
            });
        }
        public void agreeBecomeStu(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/stService/agreeBecomeStu", dic, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void reqSt(string playerName, Action callback)
        {
            JObject role = face.roleInterface.getRole();
            int lv = (int)role["lever"];
            if (lv < 70)
            {
                msgCode.showMsg(825);
                return;
            }
            if (!role["attr"]["msg"]["stu1"].ToString().Equals("") &&
                !role["attr"]["msg"]["stu2"].ToString().Equals("") &&
                !role["attr"]["msg"]["stu3"].ToString().Equals(""))
            {
                msgCode.showMsg(864);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/stService/reqSt", dic, (res) =>
            {
                msgCode.showMsg(646);
                callback();
            });
        }
        public void agreeApprentice(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/stService/agreeApprentice", dic, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void apprenticeBs(string playerName, Action callback)
        {
            JObject role = face.roleInterface.getRole();
            int lv = (int)role["lever"];
            if (lv < 15 || lv >= 50)
            {
                msgCode.showMsg(825);
                return;
            }
            if (!role["attr"]["msg"]["teacher"].ToString().Equals(""))
            {
                msgCode.showMsg(1014);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/stService/apprentice", dic, (res) =>
            {
                msgCode.showMsg(646);
                callback();
            });
        }
        public void cancelBs(Action callback)
        {
            JObject role = face.roleInterface.getRole();
            int lv = (int)role["lever"];
            if (lv < 15)
            {
                msgCode.showMsg(825);
                return;
            }
            DoGet.getInstance().sendPost("/stService/remBs", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void removeMatter(string playerName, Action callback)
        {
            if (!face.roleInterface.isEnoughMoney("tale", -50000))
            {
                msgCode.showMsg(634);
                return;
            }
            //playerName == null 徒弟解除师傅
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/stService/removeMatter", dic, (res) =>
            {
                face.roleInterface.updateMoney(-50000, 1);
                JObject r = face.roleInterface.getRole();
                if (playerName == null)
                {
                    r["attr"]["msg"]["teacher"] = "";
                }
                else
                {
                    for (int i = 1; i < 4; i++)
                    {
                        if (r["attr"]["msg"]["stu" + i].ToString().Equals(playerName))
                        {
                            r["attr"]["msg"]["stu" + i] = "";
                        }
                    }
                }
                face.roleInterface.saveRole(r);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void recruit(Action callback)
        {
            JObject role = face.roleInterface.getRole();
            int lv = (int)role["lever"];
            if (lv < 70)
            {
                msgCode.showMsg(825);
                return;
            }
            if (!role["attr"]["msg"]["stu1"].ToString().Equals("") &&
                !role["attr"]["msg"]["stu2"].ToString().Equals("") &&
                !role["attr"]["msg"]["stu3"].ToString().Equals(""))
            {
                msgCode.showMsg(864);
                return;
            }
            DoGet.getInstance().sendPost("/stService/recruit", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void signBs(Action callback)
        {
            JObject role = face.roleInterface.getRole();
            int lv = (int)role["lever"];
            if (lv < 15 || lv >= 50)
            {
                msgCode.showMsg(825);
                return;
            }
            if (!role["attr"]["msg"]["teacher"].ToString().Equals(""))
            {
                msgCode.showMsg(1014);
                return;
            }
            DoGet.getInstance().sendPost("/stService/signBs", null, (res) =>
            {
                msgCode.showMsg(866);
                callback();
            });
        }
        public void getFgLog(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/dzfsbService/getFgLog", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        public void getDzfsbOrder(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/dzfsbService/getOrder", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void huilu(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000223", 1)) return;
            DoGet.getInstance().sendPost("/prisonService/huilu", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000223", 1);
                //身份变为平民
                JObject role = face.roleInterface.getRole();
                role["attr"]["msg"]["sez"] = 5;
                face.roleInterface.saveRole(role);
                callback();
            });
        }
        public void getMyFriendFarmMsg(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/farmService/getMyFriendFarmMsg", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        public void getGangsMemberFarm(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/farmService/getGangsMemberFarm", null, (res) =>
            {
                callback((JArray)res);
            });
        }
        public void zhaiqu(string pos, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pos", pos);
            DoGet.getInstance().sendPost("/farmService/zhaiqu", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void chanchu(string pos, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000218", 10))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pos", pos);
            DoGet.getInstance().sendPost("/farmService/chanchu", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000218", 10);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void zhongzhi(string key, string pos, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(key, 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pos", pos);
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/farmService/zhongzhi", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, 1);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void openFarmPos(string pos, Action callback)
        {
            int num = 10;
            if (pos.Equals("p2")) num = 20;
            else if (pos.Equals("p3")) num = 30;
            else if (pos.Equals("p4")) num = 40;
            else if (pos.Equals("p5")) num = 50;
            if (!face.goodsInterface.isEnoughInPackAndTip("10000218", num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pos", pos);
            DoGet.getInstance().sendPost("/farmService/openPos", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000218", num);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void gainZhongZi(Action callback)
        {
            DoGet.getInstance().sendPost("/farmService/gainZhongZi", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(699);
                callback();
            });
        }
        public void getMyFarmMsg(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/farmService/getMyFarmMsg", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void touCai(string playerName, string pos, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            dic.Add("pos", pos);
            DoGet.getInstance().sendPost("/farmService/touCai", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getPlayerFarmMsg(string playerName, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/farmService/getPlayerFarmMsg", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void resetJm(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000129", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/jmService/reset", null, (res) =>
            {
                JObject role = face.roleInterface.getRole();
                JArray skls = (JArray)role["attr"]["skill"];
                for (int i = 0; i < skls.Count; i++)
                {
                    JObject obj = (JObject)skls[i];
                    if (obj.ContainsKey("key") &&
                            obj["key"].ToString().Substring(0, 8).Equals("10021002"))
                    {
                        skls.RemoveAt(i);
                        i--;
                    }
                }
                int old = (int)role["attr"]["msg"]["jmPoint"];
                int point = (int.Parse(res.ToString()));
                if (point + old > 1160)
                {
                    int d = point + old - 1160;
                    point -= d;//将超出部分减去
                }
                role["attr"]["msg"]["jmPoint"] = old + point;
                face.roleInterface.saveRole(role);
                face.goodsInterface.cutPlayerGoodsNumByKey("10000129", 1);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void composeJmDan(int type, Action callback)
        {
            string[] arr = { "10000212", "10000213", "10000214", "10000215", "10000216", "10000217", };
            string key = arr[type];
            int num = 5;
            if (type == 0) num = 5;
            else num = 3;

            if (!face.goodsInterface.isEnoughInPackAndTip(key, num))
            {
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/jmService/compose", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, num);
                JObject a = (JObject)res;
                JArray list = (JArray)a["list"];
                int r = (int)a["r"];
                if (r == 0)
                {
                    msgCode.showMsg(659);
                }
                else
                {
                    msgCode.showMsg(658);
                }
                face.rewardInterface.saveRewards(list);
                callback();
            });
        }
        public void gainLLD(Action callback)
        {
            DoGet.getInstance().sendPost("/jmService/gainLLD", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(699);
                callback();
            });
        }
        public void signDsx(Action callback)
        {
            if ((int)face.roleInterface.getRole()["lever"] < 60)
            {
                msgCode.showMsg(613);
                return;
            }

            DoGet.getInstance().sendPost("/dsxService/sign", null, (res) =>
            {
                msgCode.showMsg(666);
                callback();
            });
        }
        public void isHasMonsterByMpbwz(Action<int> callback)
        {
            JObject r = face.roleInterface.getRole();
            string posKey = r["pos"]["map"].ToString();
            string mp = null;
            if (posKey.Equals("tianshoufeng")) mp = "mj";
            else if (posKey.Equals("mizonglin")) mp = "dj";
            else if (posKey.Equals("taixugu")) mp = "yyj";
            else return;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("mp", mp);
            DoGet.getInstance().sendPost("/mpbwzService/isHasMonster", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });
        }
        public void getJyfyTask(int type, Action callback)
        {
            if (!face.roleInterface.isEnoughMoney("gold", -1000))
            {
                return;
            }
            string taskKey = null;
            if (type == 0) taskKey = "3283";
            else if (type == 1) taskKey = "3284";
            else if (type == 2) taskKey = "3285";
            else if (type == 3) taskKey = "3286";
            else if (type == 4) taskKey = "3287";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/jyfyService/getJyfyTask", dic, (res) =>
            {
                face.roleInterface.updateMoney(-1000, 0);
                face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { taskKey });
                face.taskInterface.createOneStartTask(taskKey, 2);
                msgCode.showMsg(612);
                callback();
            });
        }
        public void shoushenji(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000201", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/cybyService/shoushenji", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000201", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(995);
                callback();
            });
        }
        public void getCybyTask(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000201", 3))
            {
                return;
            }
            DoGet.getInstance().sendPost("/cybyService/getCybyTask", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000201", 3);
                face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { "3282" });
                face.taskInterface.createOneStartTask("3282", 2);
                msgCode.showMsg(612);
                callback();
            });
        }
        public void openBoxFromSLM(int type, Action callback)
        {
            if (type == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000197", 1))
                {
                    return;
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/hhbkService/openBoxFromSLM", dic, (res) =>
            {
                if (type == 1) face.goodsInterface.cutPlayerGoodsNumByKey("10000197", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(991);
                callback();
            });

        }
        public void intoShenLongMenFromSi(Action callback)
        {
            DoGet.getInstance().sendPost("/hhbkService/intoShenLongMenFromSi", null, (res) =>
            {
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(992);
                    return;
                }
                callback();
            });

        }
        public void openBoxFromSheng(int type, Action callback)
        {
            if (type == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000196", 1))
                {
                    return;
                }
            }
            else if (type == 2)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000198", 1))
                {
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/hhbkService/openBoxFromSheng", dic, (res) =>
            {
                if (type == 1) face.goodsInterface.cutPlayerGoodsNumByKey("10000196", 1);
                else if (type == 2) face.goodsInterface.cutPlayerGoodsNumByKey("10000198", 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(991);
                callback();
            });

        }
        public void clearSiMen(Action<int> callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000195", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/hhbkService/clearSiMen", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000195", 1);
                callback(int.Parse(res.ToString()));
            });

        }
        public void chooseShengSiMen(int index, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/hhbkService/chooseShengSiMen", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });

        }
        public void intoShengSiMen(Action callback)
        {
            DoGet.getInstance().sendPost("/hhbkService/intoShengSiMen", null, (res) =>
            {
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(990);
                    return;
                }
                callback();
            });

        }
        public void searchHere(int index, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/hhbkService/searchHere", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });

        }
        public void intoHhbk(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000194", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/hhbkService/intoHhbk", null, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    //交由ws 852 处理
                    callback();
                }
            });

        }
        public void viewZjcmMapKey(Action<string> callback)
        {
            DoGet.getInstance().sendPost("/zjcmService/viewMapKey", null, (res) =>
            {
                callback(res.ToString());
            });

        }
        /**接取仗剑除魔*/
        public void getZjcm(Action callback)
        {
            if (face.taskInterface.isExistTaskFromCacheTaskList("3273"))
            {
                msgCode.showMsg(853);
                return;
            }
            DoGet.getInstance().sendPost("/zjcmService/getTask", null, (res) =>
            {
                face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { "3273" });
                face.taskInterface.createOneStartTask("3273", 2);
                msgCode.showMsg(612);
                callback();
            });

        }
        public void viewWzbzMsg(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/wzycService/viewMsg", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void gainWzbz(JObject a, Action callback)
        {
            if ((int)a["gain_times"] <= 0)
            {
                msgCode.showMsg(854);
                return;
            }
            DoGet.getInstance().sendPost("/wzycService/gain", null, (res) =>
            {
                //生成道具
                face.rewardInterface.saveRewards((JArray)res);
                a["gain_times"] = (int)a["gain_times"] - 1;
                a["type"] = 0;
                a["lv"] = 1;
                msgCode.showMsg(699);
                callback();
            });
        }
        public void qingxiWzbz(JObject a, int isFree, Action callback)
        {
            if (isFree == 1 && (int)a["free_sx_times"] <= 0)
            {
                msgCode.showMsg(854);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("isFree", isFree);
            DoGet.getInstance().sendPost("/wzycService/qingxi", dic, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });

        }
        public void viewCDTimes(int type, Action<JObject> callback)
        {
            if (type == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000179", 1))
                    return;
            }

            DoGet.getInstance().sendPost("/zhuanpanService/viewCDTimes", null, (res) =>
            {
                int times = (int)((JObject)res)["free_times"];
                if (type == 1) times = (int)((JObject)res)["fufei_times"];
                if (times <= 0)
                {
                    msgCode.showMsg(610);
                    return;
                }
                callback((JObject)res);
            });

        }
        public void putCDProgress(int index, int type, Action callback)
        {
            if (type == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000179", 1))
                    return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("index", index);
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/zhuanpanService/putCDProgress", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                if (type == 1)
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey("10000179", 1);
                }
                msgCode.showMsg(984, (int)((JArray)res)[0]["exp"]);
                callback();
            });

        }
        public void isJianxi(string npcKey, Action callback)
        {
            if (face.taskInterface.isExistCommitTask("3272"))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", npcKey);
            DoGet.getInstance().sendPost("/cjwdService/isJianxi", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(983);
                }
                else
                {
                    //触发战斗
                    face.taskInterface.trigger("3272");
                }
                callback();
            });

        }
        public void viewCjwdTask(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/cjwdService/viewCjwdTask", null, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void getCjwdTask(Action callback)
        {
            if (face.taskInterface.isExistCommitTask("3272"))
            {
                msgCode.showMsg(871);
                return;
            }
            if (face.taskInterface.isExistTaskFromCacheTaskList("3272"))
            {
                msgCode.showMsg(853);
                return;
            }
            DoGet.getInstance().sendPost("/cjwdService/getCjwdTask", null, (res) =>
            {
                face.taskInterface.createOneStartTask("3272", 2);
                msgCode.showMsg(612);
                callback();
            });

        }
        public void getSysMsg(Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", 0);
            DoGet.getInstance().sendPost("/onlineService/getSysMsg", dic, (res) =>
            {
                JObject a = (JObject)res;
                msgCode.showMsg(201, "当前在线人数" + a["userNum"]);
                callback();
            });
        }
        public void gainJinShouZhi(int type, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/fuliService/gainJinShouZhi", dic, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });

        }
        /**人气排行*/
        public void getOrderSez(int type, int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/orderService/getOrderSez", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**职业等级排行*/
        public void getOrderLvByJob(int type, int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/orderService/getOrderLvByJob", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**等级排行*/
        public void getOrderLv(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/orderService/getOrderLv", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**擂台排行*/
        public void getJingjiOrder(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/jingjiService/getJingjiOrder", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**报名擂台*/
        public void signUpJingji(Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000185", 1))
            {
                return;
            }
            DoGet.getInstance().sendPost("/jingjiService/signUpJingji", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000185", 1);
                msgCode.showMsg(666);
                callback();
            });
        }
        /**获取武状元排行*/
        public void getOrderWzy(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/wzyService/getJfPh", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**获取百战千军排行*/
        public void getOrderBzqj(int type, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/orderService/getOrderBzqj", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**获取加持等级*/
        public void getJcMsLv(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/yhmkService/getJcMsLv", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**查看魔神状态*/
        public void viewStatus(string npcKey, Action<int> callback)
        {
            int n = 1;
            if (npcKey.Equals("10000485")) n = 1;
            else if (npcKey.Equals("10000486")) n = 2;
            else if (npcKey.Equals("10000487")) n = 3;
            else if (npcKey.Equals("10000488")) n = 4;
            else if (npcKey.Equals("10000489")) n = 5;
            else if (npcKey.Equals("10000490")) n = 6;
            else if (npcKey.Equals("10000491")) n = 7;
            else return;

            string msKey = "yhmkms_" + n;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("msKey", msKey);
            DoGet.getInstance().sendPost("/yhmkService/viewStatus", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });
        }
        /**敲打响木*/
        public void qiaodaxm()
        {
            DoGet.getInstance().sendPost("/yhmkService/qiaodaxm", null, (res) =>
            {
                if (res.ToString().Equals("0"))//没有次数
                {
                    msgCode.showMsg(610);
                }
                else
                {
                    //消耗1000元宝
                    face.roleInterface.updateMoney(-1000, 0);
                    face.rewardInterface.saveRewards((JArray)res);
                }

            });
        }
        /**使用供香*/
        public void userGx(string npcKey)
        {
            JObject a = face.goodsInterface.getInBbOfGoodsByKey("10000143");
            if (a == null)
            {
                msgCode.showMsg(637);
                return;
            }
            int n = 1;
            if (npcKey.Equals("10000485")) n = 1;
            else if (npcKey.Equals("10000486")) n = 2;
            else if (npcKey.Equals("10000487")) n = 3;
            else if (npcKey.Equals("10000488")) n = 4;
            else if (npcKey.Equals("10000489")) n = 5;
            else if (npcKey.Equals("10000490")) n = 6;
            else if (npcKey.Equals("10000491")) n = 7;
            else return;

            string msKey = "yhmkms_" + n;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("msKey", msKey);
            dic.Add("goodsId", a["Id"].ToString());
            DoGet.getInstance().sendPost("/yhmkService/userGx", dic, (res) =>
            {
                if (res.ToString().Equals("0"))//满级
                {
                    msgCode.showMsg(858);
                }
                else if (res.ToString().Equals("-1"))//满级
                {
                    msgCode.showMsg(931);
                }
                else
                {
                    //返回满意度
                    JObject r = (JObject)res;
                    face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), int.Parse(r["need"] + ""));
                    msgCode.showMsg(933, r["myd"] + "/" + r["max_myd"]);
                }

            });
        }
        /**挑战魔神*/
        public void tiaozhanMs(string npcKey)
        {
            int n = 1;
            if (npcKey.Equals("10000485")) n = 1;
            else if (npcKey.Equals("10000486")) n = 2;
            else if (npcKey.Equals("10000487")) n = 3;
            else if (npcKey.Equals("10000488")) n = 4;
            else if (npcKey.Equals("10000489")) n = 5;
            else if (npcKey.Equals("10000490")) n = 6;
            else if (npcKey.Equals("10000491")) n = 7;
            else return;
            string msKey = "yhmkms_" + n;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("msKey", msKey);
            DoGet.getInstance().sendPost("/yhmkService/tiaozhanMs", dic, (res) =>
            {
                if (res.ToString().Equals("0"))//没有次数
                {
                    msgCode.showMsg(858);
                }
                else if (res.ToString().Equals("1"))
                {

                }
                else//满意度未满
                {
                    JObject r = (JObject)res;
                    msgCode.showMsg(932, r["myd"] + "/" + r["max_myd"]);
                }

            });
        }

        /**接取魔神日常任务*/
        public void getMsTask(Action<string> callback)
        {
            if ((int)face.roleInterface.getRole()["lever"] < 60)
            {
                msgCode.showMsg(613);
                return;
            }
            DoGet.getInstance().sendPost("/yhmkService/getMsTask", null, (res) =>
            {
                if (res.ToString().Equals("0"))//没有次数
                {
                    msgCode.showMsg(610);
                }
                else
                {
                    //创建npc
                    string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                    string tk = res.ToString();
                    if ((mapKey.Equals("kgmsd") && int.Parse(tk) != 3265) || (mapKey.Equals("tbmsd") && int.Parse(tk) != 3266) ||
                    (mapKey.Equals("smmsd") && int.Parse(tk) != 3267) || (mapKey.Equals("ssmsd") && int.Parse(tk) != 3268) ||
                    (mapKey.Equals("sheshoumsd") && int.Parse(tk) != 3269) || (mapKey.Equals("fsmsd") && int.Parse(tk) != 3270) ||
                    (mapKey.Equals("bnmsd") && int.Parse(tk) != 3271))
                    {

                    }
                    else
                    {
                        JArray list = new JArray();
                        list.Add(npcManager.getOneData("mshuwei_" + (int.Parse(tk) - 3264), npcManager.getPosData(265f, 300f), null));
                        npcManager.appendNpc(list);
                    }
                    face.taskInterface.createOneStartTask(tk, 2);
                    callback(tk);
                }

            });

        }
        /**跳转副本*/
        public void toFbMap(string fbKey, string mapKey, int tale)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("fbKey", fbKey);
            DoGet.getInstance().sendPost("/fbService/toFbMap", dic, (res) =>
            {
                if (res.ToString().Equals("-1"))//没有次数
                {
                    msgCode.showMsg(610);
                }
                else if (res.ToString().Equals("1"))//创建任务成功
                {
                    face.roleInterface.updateMoney(-tale, 1);
                    mapManager.getInstance().reloadBef(mapKey);
                }
                else if (res.ToString().Equals("2"))//已经存在任务
                {
                    mapManager.getInstance().reloadBef(mapKey);
                }
            });

        }

        /**百战千军*/
        public void signUpWHJX()
        {
            int lv = (int)face.roleInterface.getRole()["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }

            DoGet.getInstance().sendPost("/bzqjService/signUpWHJX", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(667);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(884);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(613);
                }
                else if (res.ToString().Equals("3"))
                {
                    msgCode.showMsg(610);
                }
            });
        }
        /**获取武状元对战名单 */
        public void getWzyNameList(int pageNum, Action<object> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/wzyService/getNameList", dic, (res) =>
            {
                callback(res);
            });

        }
        /**报名武状元 */
        public void signUpWzy(string teamName)
        {
            if (strUtils.isNull(teamName) || teamName.Length > 6)
            {
                msgCode.showMsg(618);
                return;
            }
            //必须先组队
            JObject team = face.teamInterface.getTeam();
            if (team == null)
            {
                msgCode.showMsg(670);
                return;
            }
            JArray list = (JArray)team["list"];
            for (int p = 0; p < list.Count; p++)
            {
                if ((int)list[p]["lever"] < 60)
                {
                    msgCode.showMsg(613);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("teamName", teamName);
            DoGet.getInstance().sendPost("/wzyService/signUp", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(671);
                }
                else if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(666);
                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(670);
                }
                else if (res.ToString().Equals("3"))
                {
                    msgCode.showMsg(672);
                }
                else if (res.ToString().Equals("4"))
                {
                    msgCode.showMsg(673);
                }
                else if (res.ToString().Equals("5"))
                {
                    msgCode.showMsg(674);
                }
                else if (res.ToString().Equals("6"))
                {
                    msgCode.showMsg(613);
                }
            });
        }
        /**竞技场*/
        public void updateMatch(Action ac)
        {
            DoGet.getInstance().sendPost("/pkService/updateMatch", null, (res) =>
            {
                ac();
            });
        }
        public void pkByName(string name)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/pkService/pkByName", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(610);
                }
            });
        }
        public void getMatchJjOpponent(Action<JObject> ac)
        {
            DoGet.getInstance().sendPost("/pkService/getMatchJjOpponent", null, (res) =>
            {
                ac((JObject)res);
            });
        }
        /**世界boss*/
        public void createFightByWorldBoss()
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            if (lv < 30)
            {
                msgCode.showMsg(613);
                return;
            }

            DoGet.getInstance().sendPost("/fightRpcService/createFightByWorldBoss", null, (res) =>
            {

            });
        }
        /**百炼玄塔*/
        public void createFightByBlxt(string npcKey)
        {
            JObject r = face.roleInterface.getRole();
            int lv = (int)r["lever"];
            if (lv < 50)
            {
                msgCode.showMsg(613);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("lever", npcKey.Split("_")[1]);
            DoGet.getInstance().sendPost("/blytService/tz", dic, (res) =>
            {

            });
        }
        /**转盘*/
        public void putZPProgress(Action<int> ac)
        {
            DoGet.getInstance().sendPost("/zhuanpanService/putZPProgress", null, (res) =>
            {
                ac(int.Parse(res.ToString()) - 1);

            });
        }
        public void getZPProgress(Action<JObject> ac)
        {
            DoGet.getInstance().sendPost("/zhuanpanService/getZPProgress", null, (res) =>
            {
                ac((JObject)res);

            });
        }
        /**传壁*/
        public void uploadB(string key, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/cbService/uploadB", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(687);
                }
                else
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(key, 1);

                    msgCode.showMsg(688);
                    callback();
                }
            });

        }
        public void qiangB(string name)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/cbService/qb", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(689);

                }
            });

        }
        public void viewUpB(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/cbService/viewUpB", null, (res) =>
            {
                callback((JArray)res);
            });

        }
        public void getB(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/cbService/getB", null, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void gainB(Action callback)
        {
            DoGet.getInstance().sendPost("/cbService/gainB", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                callback();
            });

        }
        public void updateB(Action<int> callback)
        {
            DoGet.getInstance().sendPost("/cbService/updateB", null, (res) =>
            {
                int type = int.Parse(res.ToString());
                if (type == -1)
                {
                    msgCode.showMsg(610);
                    return;
                }
                callback(type);
            });

        }
        /**查询玩家身上接取的追杀令 */
        public void findZslByName(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/zslService/findZslByName", null, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void gainZsl(JObject obj, Action ac)
        {
            if (!face.roleInterface.isEnoughMoney((int)obj["price_type"] == 0 ? "gold" : "tale", -(int)((int)obj["price"] * 0.1f)))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", obj["Id"]);
            DoGet.getInstance().sendPost("/zslService/gainZsl", dic, (res) =>
            {
                int t = int.Parse(res.ToString());
                if (t == 1)
                {
                    face.roleInterface.updateMoney(-(int)((int)obj["price"] * 0.1f), (int)obj["price_type"]);
                    msgCode.showMsg(784);
                    ac();
                }
                else if (t == -1)
                {
                    msgCode.showMsg(814);
                }
                else if (t == -2)
                {
                    msgCode.showMsg(815);
                }
            });

        }
        public void getZsl(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/zslService/getZsl", dic, (res) =>
            {
                callback((JObject)res);
            });

        }
        /**发布追杀令 */
        public void pubZsl(int price, int priceType, string player, Action callback)
        {
            if (price < 100)
            {
                msgCode.showMsg(796, 100);
                return;
            }
            if (player.Length > 6 || strUtils.isNull(player) || face.roleInterface.getRole()["name"].ToString().Equals(player))
            {
                msgCode.showMsg(879);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("price", price);
            dic.Add("priceType", priceType);
            dic.Add("player", player);
            DoGet.getInstance().sendPost("/zslService/pubZsl", dic, (res) =>
            {
                if (res != null)
                {
                    face.roleInterface.updateMoney(-price, priceType);
                    msgCode.showMsg(811);
                    callback();
                }
            });

        }
        /**永恒魔窟挑战*/
        public void fightYhmk(string npcKey)
        {
            int type = int.Parse(npcKey.Split("_")[1]);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/yhmkService/fightYhmk", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(854);
                }
            });
        }
        /**提交答案*/
        public void putHappyProgress(int isRight)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("isRight", isRight);
            DoGet.getInstance().sendPost("/answerService/putHappyProgress", dic, (res) =>
            {
                if (!res.ToString().Equals("0") && !res.ToString().Equals("1"))
                {
                    face.rewardInterface.saveRewards((JArray)res);

                }
            });
        }
        /**日常答题*/
        public void getHappyProgress(Action<JObject> ac)
        {
            DoGet.getInstance().sendPost("/answerService/getHappyProgress", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    //等级不足
                    msgCode.showMsg(613);
                    return;
                }
                ac((JObject)res);
            });
        }
        /**获取限时*/
        public string[] getLimitTimeAcList()
        {
            string[] arr = { "2001", "2012", "2013", };
            return arr;
        }
        /**获取日常活动y*/
        public string[] getNoLimitTimeAcList()
        {
            string[] arr = { "2002", "2003", "2004", "2006", "2007", "2010", "2011", "2012", "2013", "2015", "2020", "2021", "2022", "2028", "2000", "2030",
                "yuposhi","jlzh", "wzyc", "jyfy", "ssj", "cyby", "zjcm", "dmkj", "mpdsx", "mpbwz", "hlnc", "dzfsb", "lzhf", "cjwd", "qlhd",  };
            return arr;
        }
        /**获取周常活动y*/
        public string[] getZhouAcList()
        {
            string[] arr = { "2004", "2006", "2009" };
            return arr;
        }

        /**节日活动等*/
        public string[] getElseAcList()
        {
            List<string> list = new List<string>();
            for (int i = 20430000; i < 20430014; i++)
            {
                list.Add(i + "");
            }
            return list.ToArray();
        }
        public JArray keysToObj(string[] arr)
        {
            JArray list = new JArray();
            foreach (string a in arr)
            {
                list.Add(getAcMsg(a));
            }
            return list;
        }
        /**获取活动信息*/
        public JObject getAcMsg(string key)
        {
            act task = null;
            switch (key)
            {
                case "2000":
                    {
                        task = new act(key, "大盘投资");
                        task.addAcMsg(-1, 0);
                        task.addDes("每5分钟结算一次。试试今天的运气如何？");
                        task.addReward("110512", -1).addReward("110513", -1);
                        task.addMapKey("m_6", "10000024");
                        break;
                    }
                case "2001":
                    {
                        task = new act(key, "世界答题");
                        task.addAcMsg(1, 0, 20, 0, "限时");
                        task.addDes("每日 （13:00-13:10）世界聊天窗回复正确答案即可获得奖励！");
                        task.addReward("110516", -1);
                        break;
                    }
                case "2002":
                    {
                        task = new act(key, "日常答题");
                        task.addAcMsg(1, 0);
                        task.addDes("一共10题，视答对题数给予奖励！");
                        task.addReward("110516", -1);
                        task.addReward("1004", 1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2003":
                    {
                        //一张大图上寻找任务所需怪物，20环
                        task = new act(key, "天渊");
                        task.addAcMsg(1, 0, 40);
                        task.addDes("一共40环任务，少侠成仙成魔就看这一步了！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2004":
                    {
                        //周六答题，类型：春秋-三国的知识
                        task = new act(key, "百家争鸣");
                        task.addAcMsg(1, 0, 30, 0, "限时");
                        task.addDes("（周六 16:00-18:00）正确回答考官的问题可获得丰厚奖励（共4轮，每轮20题）！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                /*case "2005":
                    {
                        //任务改成寻找机关开启下一层，九层妖塔
                        //改成墓地闯关获得藏宝图，使用会在某个地图上产生一个宝箱，玩家根据提示去寻找
                        task = new act(key, "摸金校尉");
                        task.addAcMsg(1, 0);
                        task.addDes("寻找隐藏在黑暗深处的宝藏！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("110510", -1);
                        break;
                    }*/
                case "2006":
                    {
                        task = new act(key, "门派闯关");
                        task.addAcMsg(1, 0, 60, 1, "限时");
                        task.addDes("（周日 16:00-18:00）挑战每个门派，赢得丰厚奖励！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("110517", -1);

                        break;
                    }
                case "2007":
                    {
                        task = new act(key, "宠物闯关");
                        task.addAcMsg(1, 0);
                        task.addDes("成就神宠之路必先经历炼狱、修罗、混沌的考验，来，干它！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("1122", -1).addReward("1123", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2008":
                    {
                        task = new act(key, "帮派活动");
                        task.addAcMsg(1, 0);
                        task.addDes("各种活动奖励任君选！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("110200", -1)
                            .addReward("110201", -1).addReward("110202", -1);
                        break;
                    }
                /*case "2009":
                    {
                        task = new act(key, "帮派战");
                        task.addAcMsg(1, 0, 20, 1, "限时");
                        task.addDes("（周二 20:00-21:00）获得丰厚奖励！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }*/
                case "2010":
                    {
                        task = new act(key, "英雄擂");
                        task.addAcMsg(1, 0);
                        task.addDes("每日12:00-14:00/20:00-22:00开启，日限制20次，周日按积分结算！");
                        task.addReward("110519", -1);
                        task.addMapKey("m_7", "10000030");
                        break;
                    }
                case "2011":
                    {
                        task = new act(key, "门派跑环");
                        task.addAcMsg(1, 0);
                        task.addDes("一共20环，每5环有额外的道具奖励！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("110510", -1);
                        break;
                    }
                case "2012":
                    {
                        task = new act(key, "世界boss");
                        task.addAcMsg(1, 0, 20, 0, "限时");
                        task.addDes("（周四 19:00-19:10）按单场战斗伤害排名前10发放奖励（前三有几率获得天元丹、宠物技能卷轴、锻皇宝石）！");
                        task.addReward("110520", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2013":
                    {
                        task = new act(key, "天晶石");
                        task.addAcMsg(1, 0, 20, 0, "限时");
                        task.addDes("（周末 20:00-22:00）刷怪可有几率获得天晶石！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000048");
                        break;
                    }
                case "2015":
                    {
                        task = new act(key, "百战千军");//获得积分
                        task.addAcMsg(1, 0, 30, 0, "限时");
                        task.addDes("（每日 12-13、22-23 限5场）挑战各路英雄豪杰，拜将封侯，每周结算一次");
                        task.addReward("110514", -1);
                        task.addMapKey("m_9", "10000056");
                        break;
                    }
                /*case "2016":
                    {
                        task = new act(key, "帮派boss");
                        task.addAcMsg(1, 0, 30, 0);
                        task.addDes("由帮主开启，每日每人仅能挑战一次，消灭boss后会掉落宝箱");
                        task.addReward("110521", -1).addReward("110510", -1);
                        break;
                    }*/
                case "2017":
                    {
                        task = new act(key, "幸运转盘");
                        task.addAcMsg(1, 0);
                        task.addDes("来，看看今天的运势！");
                        task.addReward("110516", -1);
                        break;
                    }
                case "2018":
                    {
                        //组队扫塔、打怪获得额外奖励
                        task = new act(key, "师徒活动");
                        task.addAcMsg(1, 0);
                        task.addDes("师徒组队打怪可获得亲密度，亲密度越高出师奖励越丰厚！");
                        task.addReward("110516", -1).addReward("110513", -1).addReward("110510", -1);
                        break;
                    }
                case "2020":
                    {
                        task = new act(key, "武状元");
                        task.addAcMsg(1, 0, 60, 0, "限时");
                        task.addDes("（周三 11:00-19:50报名 20:00开始）名单公布后参赛队伍需要到赛场准备，5分钟后开始匹配（输的直接淘汰，赢的获得1积分），" +
                        "一轮最多进行10分钟(未结束的双方视为弃权)，一轮结束后会公布下一轮名单。以此类推，直至最后的队伍被选出。大赛结束后将按积分排名给予奖励！");
                        task.addReward("110520", -1);
                        task.addMapKey("m_7", "10000032");
                        break;
                    }
                case "2021":
                    {
                        //需要消耗钥匙解锁任务，闯关完成后开启宝箱
                        task = new act(key, "洪荒宝库");
                        task.addAcMsg(1, 0);
                        task.addDes("洪荒宝库远非想象中那么简单，经多次探索，发现宝库内藏有丰富的宠物技能卷轴，想来又将引起血雨腥风！");
                        task.addReward("110511", -1);
                        task.addMapKey("m_8", "10000046");
                        break;
                    }
                case "2022":
                    {
                        //上传壁，5分钟内接受挑战
                        task = new act(key, "怀璧其罪");
                        task.addAcMsg(1, 0, 30);
                        task.addDes("需达到30级，传送过程中会遭受不同玩家的挑战，用实力守护住心中那一点璀璨！");
                        task.addReward("110514", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2025":
                    {
                        task = new act(key, "江湖追杀令");
                        task.addAcMsg(1, 0);
                        task.addDes("传个白壁都被抢？那就发布追杀令，让有实力的玩家追杀他，在指定时间内完成追杀后给予玩家相应的酬劳（注意失败方会掉落道具）！");
                        task.addReward("110513", -1).addReward("110520", -1);
                        break;
                    }
                case "2027":
                    {
                        task = new act(key, "支线任务");
                        task.addAcMsg(1, 0);
                        task.addDes("主线做完了？行，这里有你想要的经验");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "2028":
                    {
                        task = new act(key, "战魔神");
                        task.addAcMsg(1, 0);
                        task.addDes("魔窟里七位法力无边的魔神正期待战胜它们的英雄，击败它们就可以赐予你魔神的力量！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2029":
                    {
                        task = new act(key, "期货投资");
                        task.addAcMsg(-1, 0);
                        task.addDes("每5分钟结算一次。试试今天的运气如何？");
                        task.addReward("110512", -1).addReward("110513", -1);
                        task.addMapKey("m_6", "10000020");
                        break;
                    }
                case "2030":
                    {
                        task = new act(key, "百炼玄兵塔");
                        task.addAcMsg(1, 0, 30);
                        task.addDes("每是个看守为一层，共七层，战胜每层的boss后有机会得到潜力相关道具！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "2031":
                    {
                        task = new act(key, "副本");
                        task.addAcMsg(1, 0, 50);
                        task.addDes("获取稀有装备！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "2032":
                    {
                        task = new act(key, "过关斩将");//横扫千军（9、18、30、42、57）
                        task.addAcMsg(1, 0, 30);
                        task.addDes("每周限定一次，获取大量银两！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "yuposhi":
                    {
                        task = new act(key, "玉魄石");
                        task.addAcMsg(1, 0, 20, 0, "限时");
                        task.addDes("（每日 18:00-20:00）刷怪可有几率获得天晶石！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000048");
                        break;
                    }
                case "jlzh":
                    {
                        task = new act(key, "聚灵真火");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("在宠物乐园场景使用聚灵道具可获得大量经验，状态持续20分钟，队伍成员越多效果越好，效果只在宠物乐园场景有效！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_6", "10000023");
                        break;
                    }
                case "wzyc":
                    {
                        task = new act(key, "王者遗产");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("领取藏宝图，洗出最高等级、最珍贵的藏宝图吧，15星更有机会产出刻印宝石！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_6", "10000021");
                        break;
                    }
                case "jyfy":
                    {
                        task = new act(key, "监狱风云");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("最近监狱里流窜出一群逃犯，他们身怀异宝，但各个武功高强，凡能将逃犯追捕到案者，官府将把他身上携带的宝物作为奖赏，我的罗盘会帮助少侠定位逃犯，修为越高的逃犯奖励越丰厚！" +
                            "\n太二真人携带[经脉神符] 西门好色携带[玄女宝鉴] 鲁光光携带[天兵帅符] 东方必败携带[魔龙残影] 完颜失色携带[玄姬冰雕]");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_7", "10000033");
                        break;
                    }
                case "ssj":
                    {
                        task = new act(key, "兽神祭");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("直接使用[美人香]便能开启神兽祭，将祭品奉献给天神，会获得天神赐予的奖励一份，有几率获得珍贵奖励。");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_7", "10000033");
                        break;
                    }
                case "cyby":
                    {
                        task = new act(key, "采阴补阳");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("为了祭祀天神，玩家需要找到随机生成的“狐萌萌”，击杀后将天狐精元以及3份美人香交予兽神分身举办祭天仪式，天神大悦之下便会赐予丰厚的奖赏（有1.47%的几率获得兽神的恩赐，此道具100%开出玄晶天狐）。");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_7", "10000033");
                        break;
                    }
                case "zjcm":
                    {
                        task = new act(key, "仗剑除魔");
                        task.addAcMsg(-1, 0, 50);
                        task.addDes("仗剑除魔还世间一份安宁，每日可组队完成20次，零点重置。");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "dmkj":
                    {
                        task = new act(key, "盗梦空间");
                        task.addAcMsg(-1, 0, 30);
                        task.addDes("在空间中击杀怪物，完成收集任务，可领取丰厚的经验，任务不会重置，几率掉落梦幻水晶，可用于兑换其他超值物品，但被偷袭时会被袭击者击落。（血腥之地时段18:30-20:30，几率掉落梦幻珊瑚）");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_7", "10000035");
                        break;
                    }
                case "mpdsx":
                    {
                        task = new act(key, "门派大师兄");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("周一20:00开始按职业进行匹配，需提前进入赛场，胜利方留下，失败方直接淘汰，直至赛场只剩最后一名当前职业的胜利方，即为门派大师兄，当6名大师兄全部选出时才进行结算。");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "mpbwz":
                    {
                        task = new act(key, "门派保卫战");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("每日14:30-16:00，妖魔将入侵门派后山，大家赶紧来保卫家园吧！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "hlnc":
                    {
                        task = new act(key, "欢乐农场");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("（怀璧其罪场景）开垦农田、收获果实、偷菜，尽享欢乐！");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000047");
                        break;
                    }
                case "dzfsb":
                    {
                        task = new act(key, "斗战封神榜");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("究竟谁能再次傲视群雄......");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000057");
                        break;
                    }
                case "lzhf":
                    {
                        task = new act(key, "炼制护符");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("各种抗性护符应有尽有，快来找菩提老祖索要吧");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_8", "10000045");
                        break;
                    }
                case "cjwd":
                    {
                        task = new act(key, "锄奸卫道");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("大陆竟然潜入了内奸，快去找出它是谁吧");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_9", "10000048");
                        break;
                    }
                case "qlhd":
                    {
                        task = new act(key, "潜力黑洞");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("每日21:00-22:00开放双倍练潜，神兵在手天下我有");
                        task.addReward("110516", -1).addReward("110513", -1);
                        task.addMapKey("m_8", "10000040");
                        break;
                    }
                case "bpz":
                    {
                        task = new act(key, "帮派战");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("需帮主、副帮主提前报名，周五19:50-20:20开放，结束后按积分发放奖励");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "bpps":
                    {
                        task = new act(key, "帮派跑商");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("需缴纳20000银两作为本金，云中界-北冥城接取活动");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "fsywt":
                    {
                        task = new act(key, "封赏演武堂");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("提交封赏玉帛方能开启，开启后场景会出现一定数量的猴子，赶走它们能获得碎玉帛");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }
                case "bprw":
                    {
                        task = new act(key, "帮派任务");
                        task.addAcMsg(-1, 0, 60);
                        task.addDes("获得一定帮贡，共十环");
                        task.addReward("110516", -1).addReward("110513", -1);
                        break;
                    }






                //2043xxxx是扩展的其他活动
                case "20430000":
                    {
                        task = new act(key, "神将降临");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("获得绝世神将！");
                        break;
                    }
                case "20430001":
                    {
                        task = new act(key, "全服boss");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("获得绝世法宝！");
                        break;
                    }
                case "20430002":
                    {
                        task = new act(key, "神宠降临");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("获得绝世宠物！");
                        break;
                    }
                case "20430003":
                    {
                        task = new act(key, "魔神降临");
                        task.addAcMsg(10, 0, 1, 0);
                        task.addDes("遗弃的村庄、隐逸村、引龙谷、落雁深林降临魔神（每10分钟刷新一次，每日限制10次），实力强大者可获得魔神装备！");
                        break;
                    }
                case "20430004":
                    {
                        task = new act(key, "春节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430005":
                    {
                        task = new act(key, "元宵节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430006":
                    {
                        task = new act(key, "清明节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430007":
                    {
                        task = new act(key, "端午节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430008":
                    {
                        task = new act(key, "七夕节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430009":
                    {
                        task = new act(key, "中秋节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430010":
                    {
                        task = new act(key, "重阳节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430011":
                    {
                        task = new act(key, "元旦活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430012":
                    {
                        task = new act(key, "劳动节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
                case "20430013":
                    {
                        task = new act(key, "国庆节活动");
                        task.addAcMsg(-1, 0, 1, 1);
                        task.addDes("啦啦啦！");
                        break;
                    }
            }
            if (key.Contains("fb_"))
            {
                string[] arr = { "[50]紫阴古城", "[60]万魂沟壑", "[70]隐月幽谷", "[80]往生殿", "[90]青丘境", "[100]混沌邪灵渊",
                "[英1]赤炼洞窟","[英2]浮游城","[英3]隐龙古城","[英4]魔界之门",};
                string[] mapKeys = { "m_30", "m_41", "m_52", "m_68", "m_77", "m_97", "m_85",
                "m_89","m_94","m_101",};
                string[] npcKeys = { "10000092", "10000145", "10000205", "10000273", "10000319", "10000427", "10000379",
                "10000399","10000413","10000454",};
                int index = int.Parse(key.Split("_")[1]);
                task = new act(key, arr[index]);
                task.addAcMsg(-1, 0, 60);
                task.addDes("每日可完成2次");
                task.addReward("110516", -1).addReward("110513", -1);
                task.addMapKey(mapKeys[index], npcKeys[index]);
            }
            return strUtils.copyJSON<JObject>(task);
        }

        /**获取活动展开菜单*/
        public List<MultyMenu> getAcMenus(string key, string npcKey, modelMsgBind npc = null)
        {
            switch (key)
            {
                case "2000":
                    {
                        MultyMenu m1 = new MultyMenu("买大盘涨(2倍)");
                        MultyMenu m1_1 = new MultyMenu("投资1000银两").bindFnByName(typeof(activityGet), "DaPan", 0, 1, 1000);
                        MultyMenu m1_2 = new MultyMenu("投资10000银两").bindFnByName(typeof(activityGet), "DaPan", 0, 1, 10000);
                        MultyMenu m1_3 = new MultyMenu("投资100000银两").bindFnByName(typeof(activityGet), "DaPan", 0, 1, 100000);
                        MultyMenu m1_4 = new MultyMenu("投资一张龙头金票").bindFnByName(typeof(activityGet), "DaPan", 0, 0, "10000000");
                        MultyMenu m1_5 = new MultyMenu("投资一张龙头银票").bindFnByName(typeof(activityGet), "DaPan", 0, 0, "10000001");
                        MultyMenu m1_6 = new MultyMenu("投资一张龙头小票").bindFnByName(typeof(activityGet), "DaPan", 0, 0, "10000002");
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6);
                        MultyMenu m2 = new MultyMenu("买大盘跌(2倍)");
                        MultyMenu m2_1 = new MultyMenu("投资1000银两").bindFnByName(typeof(activityGet), "DaPan", 1, 1, 1000);
                        MultyMenu m2_2 = new MultyMenu("投资10000银两").bindFnByName(typeof(activityGet), "DaPan", 1, 1, 10000);
                        MultyMenu m2_3 = new MultyMenu("投资100000银两").bindFnByName(typeof(activityGet), "DaPan", 1, 1, 100000);
                        MultyMenu m2_4 = new MultyMenu("投资一张龙头金票").bindFnByName(typeof(activityGet), "DaPan", 1, 0, "10000000");
                        MultyMenu m2_5 = new MultyMenu("投资一张龙头银票").bindFnByName(typeof(activityGet), "DaPan", 1, 0, "10000001");
                        MultyMenu m2_6 = new MultyMenu("投资一张龙头小票").bindFnByName(typeof(activityGet), "DaPan", 1, 0, "10000002");
                        m2.addMenu(m2_1, m2_2, m2_3, m2_4, m2_5, m2_6);
                        MultyMenu m3 = new MultyMenu("买大盘持平(30倍)");
                        MultyMenu m3_1 = new MultyMenu("投资1000银两").bindFnByName(typeof(activityGet), "DaPan", 2, 1, 1000);
                        MultyMenu m3_2 = new MultyMenu("投资10000银两").bindFnByName(typeof(activityGet), "DaPan", 2, 1, 10000);
                        MultyMenu m3_3 = new MultyMenu("投资100000银两").bindFnByName(typeof(activityGet), "DaPan", 2, 1, 100000);
                        MultyMenu m3_4 = new MultyMenu("投资一张龙头金票").bindFnByName(typeof(activityGet), "DaPan", 2, 0, "10000000");
                        MultyMenu m3_5 = new MultyMenu("投资一张龙头银票").bindFnByName(typeof(activityGet), "DaPan", 2, 0, "10000001");
                        MultyMenu m3_6 = new MultyMenu("投资一张龙头小票").bindFnByName(typeof(activityGet), "DaPan", 2, 0, "10000002");
                        m3.addMenu(m3_1, m3_2, m3_3, m3_4, m3_5, m3_6);
                        MultyMenu m4 = new MultyMenu("往期行情").bindFnByName(typeof(activityGet), "GetDaPanHQ");
                        MultyMenu m5 = new MultyMenu("查看当前投资记录").bindFnByName(typeof(activityGet), "GetDaPanTZ");
                        //MultyMenu m6 = new MultyMenu("大盘投资说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        MultyMenu m = new MultyMenu("大盘投资");
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "2002":
                    {
                        MultyMenu m = new MultyMenu("日常答题");
                        MultyMenu m1 = new MultyMenu("开始答题").bindFnByName(typeof(activityGet), "happyAnswer");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2003":
                    {
                        MultyMenu m = new MultyMenu("天渊");
                        MultyMenu m1 = new MultyMenu("挑战天渊").bindFnByName(typeof(activityGet), "toMap", "abyss1");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2003_1":
                    {
                        JObject role = face.roleInterface.getRole();
                        string mapKey = role["pos"]["map"].ToString();
                        MultyMenu m = new MultyMenu("天渊传送");
                        MultyMenu m1 = new MultyMenu("传送到第1层").bindFnByName(typeof(activityGet), "toMap", "abyss1");
                        MultyMenu m2 = new MultyMenu("传送到第5层").bindFnByName(typeof(activityGet), "toMap", "abyss5");
                        MultyMenu m3 = new MultyMenu("传送到第10层").bindFnByName(typeof(activityGet), "toMap", "abyss10");
                        MultyMenu m4 = new MultyMenu("传送到第15层").bindFnByName(typeof(activityGet), "toMap", "abyss15");
                        MultyMenu m5 = new MultyMenu("传送到第20层").bindFnByName(typeof(activityGet), "toMap", "abyss20");
                        MultyMenu m6 = new MultyMenu("传送到第25层").bindFnByName(typeof(activityGet), "toMap", "abyss25");
                        MultyMenu m7 = new MultyMenu("传送到第30层").bindFnByName(typeof(activityGet), "toMap", "abyss30");
                        MultyMenu m8 = new MultyMenu("传送到第35层").bindFnByName(typeof(activityGet), "toMap", "abyss35");
                        MultyMenu m9 = new MultyMenu("传送到第40层").bindFnByName(typeof(activityGet), "toMap", "abyss40");
                        if (!mapKey.Equals("abyss1")) m.addMenu(m1);
                        if (!mapKey.Equals("abyss5")) m.addMenu(m2);
                        if (!mapKey.Equals("abyss10")) m.addMenu(m3);
                        if (!mapKey.Equals("abyss15")) m.addMenu(m4);
                        if (!mapKey.Equals("abyss20")) m.addMenu(m5);
                        if (!mapKey.Equals("abyss25")) m.addMenu(m6);
                        if (!mapKey.Equals("abyss30")) m.addMenu(m7);
                        if (!mapKey.Equals("abyss35")) m.addMenu(m8);
                        if (!mapKey.Equals("abyss40")) m.addMenu(m9);

                        return m.menus;
                    }
                case "2004":
                    {
                        MultyMenu m = new MultyMenu("百家争鸣");
                        MultyMenu m1 = new MultyMenu("前往答题").bindFnByName(typeof(activityGet), "GetDes");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                /*case "2005":
                    {
                        MultyMenu m = new MultyMenu("摸金校尉");
                        MultyMenu m1 = new MultyMenu("进入场景").bindFnByName(typeof(activityGet), "wabao");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }*/
                case "2006":
                    {
                        MultyMenu m = new MultyMenu("门派闯关");
                        MultyMenu m1 = new MultyMenu("前往挑战").bindFnByName(typeof(activityGet), "GetDes");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2007":
                    {
                        MultyMenu m = new MultyMenu("宠物修炼");
                        MultyMenu m1 = new MultyMenu("炼狱之地").bindFnByName(typeof(activityGet), "toMap", "lyzd");
                        MultyMenu m2 = new MultyMenu("修罗之地").bindFnByName(typeof(activityGet), "toMap", "xlzd");
                        MultyMenu m3 = new MultyMenu("混沌之地").bindFnByName(typeof(activityGet), "toMap", "hdzd");
                        MultyMenu m4 = new MultyMenu("兑换宠物装备");
                        MultyMenu m4_1 = new MultyMenu("兑换蓝色防具").bindFnByName(typeof(activityGet), "exchangePetEquip", 0);
                        MultyMenu m4_2 = new MultyMenu("兑换紫色防具").bindFnByName(typeof(activityGet), "exchangePetEquip", 1);
                        MultyMenu m4_3 = new MultyMenu("兑换蓝色饰品").bindFnByName(typeof(activityGet), "exchangePetEquip", 2);
                        MultyMenu m4_4 = new MultyMenu("兑换紫色饰品").bindFnByName(typeof(activityGet), "exchangePetEquip", 3);
                        m4.addMenu(m4_1, m4_2, m4_3, m4_4);
                        m.addMenu(m1, m2, m3, m4);
                        return retMenus(m);
                    }
                case "2007_1":
                    {
                        MultyMenu m = new MultyMenu("宠物修炼");
                        MultyMenu m1 = new MultyMenu("传送到炼狱之地").bindFnByName(typeof(activityGet), "toMap", "lyzd");
                        MultyMenu m2 = new MultyMenu("传送到修罗之地").bindFnByName(typeof(activityGet), "toMap", "xlzd");
                        MultyMenu m3 = new MultyMenu("传送到混沌之地").bindFnByName(typeof(activityGet), "toMap", "hdzd");
                        MultyMenu m4 = new MultyMenu("传送到邯郸").bindFnByName(typeof(activityGet), "toMap", "m_22");
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "2008":
                    {
                        MultyMenu m = new MultyMenu("帮派任务");
                        MultyMenu m1 = new MultyMenu("接取任务").bindFnByName(typeof(activityGet), "GetDes");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2011":
                    {
                        MultyMenu m = new MultyMenu("门派任务");
                        MultyMenu m1 = new MultyMenu("接取任务").bindFnByName(typeof(activityGet), "getMenPaiTask", npcKey);
                        m.addMenu(m1);
                        return retMenus(m);
                    }
                case "2012":
                    {
                        MultyMenu m = new MultyMenu("世界boss");
                        MultyMenu m1 = new MultyMenu("挑战boss").bindFnByName(typeof(activityGet), "pkBoss");
                        MultyMenu m2 = new MultyMenu("伤害排行").bindFnByName(typeof(activityGet), "viewHurtOrder");
                        MultyMenu m3 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3);
                        return retMenus(m);
                    }
                case "2013":
                    {
                        MultyMenu m = new MultyMenu("天晶石收集");
                        MultyMenu m1 = new MultyMenu("兑换奖励").bindFnByName(typeof(activityGet), "openYPSPage", 1);
                        m.addMenu(m1);
                        return retMenus(m);
                    }
                case "2016":
                    {
                        MultyMenu m = new MultyMenu("帮派boss");
                        MultyMenu m1 = new MultyMenu("挑战").bindFnByName(typeof(activityGet), "attackBPBoss");
                        MultyMenu m2 = new MultyMenu("击杀奖励").bindFnByName(typeof(activityGet), "viewBPBossReward");
                        MultyMenu m3 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3);
                        return retMenus(m);
                    }
                /*case "2017":
                    {
                        MultyMenu m = new MultyMenu("幸运转盘");
                        MultyMenu m1 = new MultyMenu("前往").bindFnByName(typeof(activityGet), "zhuanpan");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }*/
                case "2018":
                    {
                        MultyMenu m = new MultyMenu("师徒活动");
                        MultyMenu m1 = new MultyMenu("拜师").bindFnByName(typeof(activityGet), "apprentice");
                        MultyMenu m2 = new MultyMenu("解除关系").bindFnByName(typeof(activityGet), "removeMatter");
                        MultyMenu m3 = new MultyMenu("扫塔").bindFnByName(typeof(activityGet), "toMap", "kongmiao1");
                        MultyMenu m4 = new MultyMenu("出师").bindFnByName(typeof(activityGet), "apprenticeship");
                        MultyMenu m5 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "2020":
                    {
                        MultyMenu m = new MultyMenu("武状元大赛");
                        MultyMenu m1 = new MultyMenu("报名登记").bindFnByName(typeof(activityGet), "OpenWzyShop", 0);
                        MultyMenu m2 = new MultyMenu("前往赛场").bindFnByName(typeof(activityGet), "toMap", "wzy");
                        MultyMenu m3 = new MultyMenu("武状元商城").bindFnByName(typeof(activityGet), "OpenWzyShop", 1);
                        MultyMenu m4 = new MultyMenu("积分榜").bindFnByName(typeof(activityGet), "OpenWzyShop", 2);
                        MultyMenu m5 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "2021":
                    {
                        MultyMenu m = new MultyMenu("洪荒宝库");
                        MultyMenu m1 = new MultyMenu("奖励预览").bindFnByName(typeof(activityGet), "hhbkyl");
                        MultyMenu m2 = new MultyMenu("进入洪荒宝库").bindFnByName(typeof(activityGet), "toHHBK");
                        MultyMenu m3 = new MultyMenu("召唤魔龙").bindFnByName(typeof(activityGet), "GetDes");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "2021_1":
                    {
                        MultyMenu m = new MultyMenu("一层");
                        MultyMenu m1 = new MultyMenu("前往生死门").bindFnByName(typeof(activityGet), "toHShengSiMen");
                        MultyMenu m2 = new MultyMenu("离开洪荒宝库").bindFnByName(typeof(activityGet), "toMap", "m_1");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "2021_2":
                    {
                        MultyMenu m = new MultyMenu("一层");
                        MultyMenu m1 = new MultyMenu("搜索此处").bindFnByName(typeof(activityGet), "HHBKsousuo", npc);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "2021_3":
                    {
                        MultyMenu m = new MultyMenu("二层");
                        MultyMenu m1 = new MultyMenu("生死门说明").bindFnByName(typeof(activityGet), "shengsimenAbout");
                        MultyMenu m2 = new MultyMenu("看清真相").bindFnByName(typeof(activityGet), "remSiMen");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "2021_4":
                    {
                        MultyMenu m = new MultyMenu("二层");
                        MultyMenu m1 = new MultyMenu("生死门传送").bindFnByName(typeof(activityGet), "chooseShengSiMen", npc);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "2021_5":
                    {
                        MultyMenu m = new MultyMenu("二层");
                        MultyMenu m1 = new MultyMenu("前往神龙门").bindFnByName(typeof(activityGet), "siMenToSLM");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "2021_6":
                    {
                        MultyMenu m = new MultyMenu("二层");
                        MultyMenu m1 = new MultyMenu("生门宝藏说明").bindFnByName(typeof(activityGet), "box2About");
                        MultyMenu m2 = new MultyMenu("前往神龙门").bindFnByName(typeof(activityGet), "shengMenToSLM");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "2021_7":
                    {
                        MultyMenu m = new MultyMenu("二层");
                        MultyMenu m1 = new MultyMenu("暴力开箱").bindFnByName(typeof(activityGet), "openHHBKBox2", 0);
                        MultyMenu m2 = new MultyMenu("钥匙开箱").bindFnByName(typeof(activityGet), "openHHBKBox2", 1);
                        MultyMenu m3 = new MultyMenu("再来一罐").bindFnByName(typeof(activityGet), "openHHBKBox2", 2);
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "2021_8":
                    {
                        MultyMenu m = new MultyMenu("三层");
                        MultyMenu m1 = new MultyMenu("神龙后裔的来历").bindFnByName(typeof(activityGet), "box3Laili");
                        MultyMenu m2 = new MultyMenu("神龙门说明").bindFnByName(typeof(activityGet), "box3About");
                        MultyMenu m3 = new MultyMenu("离开洪荒宝库").bindFnByName(typeof(activityGet), "toMap", "m_1");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "2021_9":
                    {
                        MultyMenu m = new MultyMenu("三层");
                        MultyMenu m1 = new MultyMenu("暴力开箱").bindFnByName(typeof(activityGet), "openHHBKBox3", 0);
                        MultyMenu m2 = new MultyMenu("钥匙开箱").bindFnByName(typeof(activityGet), "openHHBKBox3", 1);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "2022":
                    {
                        MultyMenu m = new MultyMenu("怀璧其罪");
                        MultyMenu m1 = new MultyMenu("领取壁").bindFnByName(typeof(activityGet), "viewB");
                        MultyMenu m2 = new MultyMenu("前往").bindFnByName(typeof(activityGet), "toMap", "cbd");
                        MultyMenu m3 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3);
                        return retMenus(m);
                    }
                /*case "2025":
                    {
                        MultyMenu m = new MultyMenu("江湖追杀令");
                        MultyMenu m1 = new MultyMenu("前往").bindFnByName(typeof(activityGet), "publicZSL");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }*/
                case "2027":
                    {
                        MultyMenu m = new MultyMenu("支线任务");
                        MultyMenu m1 = new MultyMenu("前往").bindFnByName(typeof(activityGet), "GetDes");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2028":
                    {
                        MultyMenu m = new MultyMenu("魔神殿");
                        MultyMenu m1 = new MultyMenu("敲打魔神响木").bindFnByName(typeof(activityGet), "qiaodaxm");
                        MultyMenu m2 = new MultyMenu("魔神响木说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        MultyMenu m3 = new MultyMenu("前往魔神窟").bindFnByName(typeof(activityGet), "toMap", "kgmsd");
                        MultyMenu m4 = new MultyMenu("魔神窟说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        MultyMenu m5 = new MultyMenu("查看加持等级").bindFnByName(typeof(activityGet), "viewMsLever", npcKey);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return retMenus(m);
                    }
                case "2028_1":
                    {
                        MultyMenu m = new MultyMenu("战魔神");
                        MultyMenu m1 = new MultyMenu("接取日常任务").bindFnByName(typeof(activityGet), "getMsTask");
                        MultyMenu m2 = new MultyMenu("任务说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "2028_2":
                    {
                        MultyMenu m = new MultyMenu("战魔神");//1级 6供香 3级 42供香 7级 132供香  18
                        MultyMenu m1 = new MultyMenu("使用供香").bindFnByName(typeof(activityGet), "userGx", npcKey);
                        MultyMenu m2 = new MultyMenu("召唤魔神").bindFnByName(typeof(activityGet), "tiaozhanMs", npcKey);
                        MultyMenu m3 = new MultyMenu("查看魔神状态").bindFnByName(typeof(activityGet), "viewMsStatus", npcKey);
                        MultyMenu m4 = new MultyMenu("查看加持等级").bindFnByName(typeof(activityGet), "viewMsLever", npcKey);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "2028_3":
                    {
                        MultyMenu m = new MultyMenu("战魔神");
                        MultyMenu m1 = new MultyMenu("狂攻魔神殿").bindFnByName(typeof(activityGet), "toMap", "kgmsd");
                        MultyMenu m2 = new MultyMenu("铁壁魔神殿").bindFnByName(typeof(activityGet), "toMap", "tbmsd");
                        MultyMenu m3 = new MultyMenu("生命魔神殿").bindFnByName(typeof(activityGet), "toMap", "smmsd");
                        MultyMenu m4 = new MultyMenu("神速魔神殿").bindFnByName(typeof(activityGet), "toMap", "ssmsd");
                        MultyMenu m5 = new MultyMenu("射手魔神殿").bindFnByName(typeof(activityGet), "toMap", "sheshoumsd");
                        MultyMenu m6 = new MultyMenu("法术魔神殿").bindFnByName(typeof(activityGet), "toMap", "fsmsd");
                        MultyMenu m7 = new MultyMenu("暴怒魔神殿").bindFnByName(typeof(activityGet), "toMap", "bnmsd");
                        m.addMenu(m1, m2, m3, m4, m5, m6, m7);
                        return m.menus;
                    }
                case "2029":
                    {
                        MultyMenu m = new MultyMenu("期货投资");
                        string[] arr = {
                        "锻造宝石(3或5倍)","精炼宝石(3或10倍)","镶嵌宝石(3或10倍)","修复宝石(3或10倍)","黑洞陨石(3或10倍)",
                        "天晶石(3或20倍)","玉魄石(3或20倍)","补天玄石(10或40倍)"
                        };
                        for (int i = 0; i < arr.Length; i++)
                        {
                            MultyMenu m1 = new MultyMenu(arr[i]);
                            MultyMenu m1_1 = new MultyMenu("投资1000银两").bindFnByName(typeof(activityGet), "QiHuo", i, 1, 1000);
                            MultyMenu m1_2 = new MultyMenu("投资10000银两").bindFnByName(typeof(activityGet), "QiHuo", i, 1, 10000);
                            MultyMenu m1_3 = new MultyMenu("投资100000银两").bindFnByName(typeof(activityGet), "QiHuo", i, 1, 100000);
                            MultyMenu m1_4 = new MultyMenu("投资一张龙头金票").bindFnByName(typeof(activityGet), "QiHuo", i, 0, "10000000");
                            MultyMenu m1_5 = new MultyMenu("投资一张龙头银票").bindFnByName(typeof(activityGet), "QiHuo", i, 0, "10000001");
                            MultyMenu m1_6 = new MultyMenu("投资一张龙头小票").bindFnByName(typeof(activityGet), "QiHuo", i, 0, "10000002");
                            m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6);
                            m.addMenu(m1);
                        }
                        MultyMenu m4 = new MultyMenu("往期行情").bindFnByName(typeof(activityGet), "GetQiHuoHQ");
                        MultyMenu m5 = new MultyMenu("查看当前投资记录").bindFnByName(typeof(activityGet), "GetQiHuoTZ");
                        m.addMenu(m4, m5);
                        return m.menus;
                    }
                case "2030":
                    {
                        MultyMenu m = new MultyMenu("百炼玄塔");
                        MultyMenu m1 = new MultyMenu("前往").bindFnByName(typeof(activityGet), "toMap", "blxt1");
                        MultyMenu m2 = new MultyMenu("设置练潜").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m3 = new MultyMenu("取消练潜").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m4 = new MultyMenu("注入潜力").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m5 = new MultyMenu("兑换潜力符石").bindFnByName(typeof(activityGet), "exchangeQlfs");
                        m.addMenu(m1, m2, m3, m4, m5);
                        return retMenus(m);
                    }
                case "2030_1":
                    {
                        MultyMenu m = new MultyMenu("百炼玄塔");
                        MultyMenu m1 = new MultyMenu("进入战斗").bindFnByName(typeof(activityGet), "tzBlxt", npcKey);
                        m.addMenu(m1);
                        int index = int.Parse(npcKey.Replace("blxt_", ""));
                        if (index % 10 == 0)
                        {
                            MultyMenu m2 = new MultyMenu("进入下一层").bindFnByName(typeof(activityGet), "toMap", "blxt" + (int)(index / 10 + 1));
                            m.addMenu(m2);
                        }

                        return m.menus;
                    }
                case "zjcm":
                    {
                        MultyMenu m = new MultyMenu("仗剑除魔");
                        MultyMenu m1 = new MultyMenu("接取任务").bindFnByName(typeof(activityGet), "zjcm");
                        m.addMenu(m1);
                        return retMenus(m);
                    }
                case "2032":
                    {
                        MultyMenu m = new MultyMenu("玉魄石收集");
                        MultyMenu m1 = new MultyMenu("兑换奖励").bindFnByName(typeof(activityGet), "openYPSPage", 0);
                        m.addMenu(m1);
                        return retMenus(m);
                    }
                case "2033":
                    {
                        MultyMenu m = new MultyMenu("锄奸卫道");
                        MultyMenu m1 = new MultyMenu("接取任务").bindFnByName(typeof(activityGet), "getCjwd");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return retMenus(m);
                    }
                case "2034":
                    {
                        MultyMenu m = new MultyMenu("垂钓");
                        MultyMenu m1 = new MultyMenu("免费垂钓").bindFnByName(typeof(activityGet), "chuidiao", 0);
                        MultyMenu m2 = new MultyMenu("付费垂钓").bindFnByName(typeof(activityGet), "chuidiao", 1);
                        MultyMenu m3 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3);
                        return retMenus(m);
                    }
                case "2035":
                    {
                        MultyMenu m = new MultyMenu("百战千军");
                        MultyMenu m1 = new MultyMenu("报名参赛").bindFnByName(typeof(activityGet), "OpenBzqj", 0);
                        MultyMenu m2 = new MultyMenu("排行榜").bindFnByName(typeof(activityGet), "OpenBzqj", 1);
                        MultyMenu m3 = new MultyMenu("军功商城").bindFnByName(typeof(activityGet), "OpenBzqj", 2);
                        MultyMenu m4 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "kongmiao":
                    {
                        MultyMenu m = new MultyMenu("孔庙");
                        MultyMenu m1 = new MultyMenu("孔庙清理").bindFnByName(typeof(activityGet), "kmql", npcKey);
                        MultyMenu m2 = new MultyMenu("传送至二层").bindFnByName(typeof(activityGet), "toMap", "kongmiao2");
                        MultyMenu m3 = new MultyMenu("传送至三层").bindFnByName(typeof(activityGet), "toMap", "kongmiao3");
                        MultyMenu m4 = new MultyMenu("传送至四层").bindFnByName(typeof(activityGet), "toMap", "kongmiao4");
                        MultyMenu m5 = new MultyMenu("传送至五层").bindFnByName(typeof(activityGet), "toMap", "kongmiao5");
                        MultyMenu m6 = new MultyMenu("传送至六层").bindFnByName(typeof(activityGet), "toMap", "kongmiao6");
                        m.addMenu(m1, m2, m3, m4, m5, m6);
                        return m.menus;
                    }
                case "nongchang":
                    {
                        MultyMenu m = new MultyMenu("农场");
                        MultyMenu m1 = new MultyMenu("我的农场").bindFnByName(typeof(activityGet), "OpenFarmPage", 0);
                        MultyMenu m2 = new MultyMenu("好友农场").bindFnByName(typeof(activityGet), "OpenFarmPage", 1);
                        MultyMenu m3 = new MultyMenu("帮派成员农场").bindFnByName(typeof(activityGet), "OpenFarmPage", 2);
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "cbdt":
                    {
                        MultyMenu m = new MultyMenu("传壁大厅");
                        MultyMenu m1 = new MultyMenu("打开传壁大厅").bindFnByName(typeof(activityGet), "OpenBiRoom");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "jingmai":
                    {
                        MultyMenu m = new MultyMenu("经脉");
                        MultyMenu m1 = new MultyMenu("修行馈赠").bindFnByName(typeof(activityGet), "OpenJingMai", 0);
                        MultyMenu m2 = new MultyMenu("丹药合成").bindFnByName(typeof(activityGet), "OpenJingMai", 1);
                        MultyMenu m3 = new MultyMenu("提升经脉等级").bindFnByName(typeof(activityGet), "OpenJingMai", 2);
                        MultyMenu m4 = new MultyMenu("易经洗髓").bindFnByName(typeof(activityGet), "OpenJingMai", 2);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "zzfs":
                    {
                        MultyMenu m = new MultyMenu("紫装飞升");
                        MultyMenu m1 = new MultyMenu("抗性加护").bindFnByName(typeof(activityGet), "OpenZzfs", 0);
                        MultyMenu m2 = new MultyMenu("紫装升橙").bindFnByName(typeof(activityGet), "OpenZzfs", 1);
                        MultyMenu m3 = new MultyMenu("重铸橙装").bindFnByName(typeof(activityGet), "OpenZzfs", 2);
                        MultyMenu m4 = new MultyMenu("橙装注魔").bindFnByName(typeof(activityGet), "OpenZzfs", 3);
                        MultyMenu m5 = new MultyMenu("橙装锻造").bindFnByName(typeof(activityGet), "OpenZzfs", 4);
                        MultyMenu m6 = new MultyMenu("橙装血契").bindFnByName(typeof(activityGet), "OpenZzfs", 5);
                        MultyMenu m7 = new MultyMenu("兑换千年玄铁").bindFnByName(typeof(activityGet), "exchangeQNXT");
                        m.addMenu(m1, m2, m3, m4, m5, m6, m7);
                        return m.menus;
                    }
                case "ssfs":
                    {
                        MultyMenu m = new MultyMenu("神兽分身");
                        MultyMenu m1 = new MultyMenu("暂时不开放神宠、5级宝石");
                        m.addMenu(m1);
                        /*MultyMenu m1 = new MultyMenu("监狱风云");
                        MultyMenu m1_1 = new MultyMenu("缉拿太二真人").bindFnByName(typeof(activityGet), "jyfy", 0);
                        MultyMenu m1_2 = new MultyMenu("缉拿西门好色").bindFnByName(typeof(activityGet), "jyfy", 1);
                        MultyMenu m1_3 = new MultyMenu("缉拿鲁光光").bindFnByName(typeof(activityGet), "jyfy", 2);
                        MultyMenu m1_4 = new MultyMenu("缉拿东方必败").bindFnByName(typeof(activityGet), "jyfy", 3);
                        MultyMenu m1_5 = new MultyMenu("缉拿完颜失色").bindFnByName(typeof(activityGet), "jyfy", 4);
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5);
                        MultyMenu m2 = new MultyMenu("兽神祭").bindFnByName(typeof(activityGet), "ssj");
                        MultyMenu m3 = new MultyMenu("采阴补阳").bindFnByName(typeof(activityGet), "cyby");
                        m.addMenu(m1, m2, m3);*/
                        return m.menus;
                    }
                case "ptlz":
                    {
                        MultyMenu m = new MultyMenu("菩提老祖");
                        MultyMenu m1 = new MultyMenu("仙人秘法").bindFnByName(typeof(activityGet), "OpenXianRenMiFa", 0);
                        MultyMenu m2 = new MultyMenu("天地仙诀").bindFnByName(typeof(activityGet), "OpenXianRenMiFa", 1);
                        MultyMenu m3 = new MultyMenu("护体仙符").bindFnByName(typeof(activityGet), "OpenXianRenMiFa", 2);
                        MultyMenu m4 = new MultyMenu("通天法宝").bindFnByName(typeof(activityGet), "OpenXianRenMiFa", 3);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "jls":
                    {
                        MultyMenu m = new MultyMenu("精炼师");
                        MultyMenu m1 = new MultyMenu("卷轴熔炼").bindFnByName(typeof(activityGet), "OpenSkillRL", 0);
                        MultyMenu m2 = new MultyMenu("装备精炼").bindFnByName(typeof(activityGet), "OpenDuanzao", 9);
                        MultyMenu m3 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "qls":
                    {
                        MultyMenu m = new MultyMenu("潜力师");
                        MultyMenu m1 = new MultyMenu("潜力黑洞");
                        MultyMenu m1_1 = new MultyMenu("普通黑洞").bindFnByName(typeof(activityGet), "toMap", "qlhd1");
                        MultyMenu m1_2 = new MultyMenu("英雄黑洞").bindFnByName(typeof(activityGet), "toMap", "qlhd2");
                        MultyMenu m1_3 = new MultyMenu("地狱黑洞").bindFnByName(typeof(activityGet), "toMap", "qlhd3");
                        MultyMenu m1_4 = new MultyMenu("炼狱黑洞").bindFnByName(typeof(activityGet), "toMap", "qlhd4");
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4);
                        MultyMenu m2 = new MultyMenu("设置练潜").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m3 = new MultyMenu("取消练潜").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m4 = new MultyMenu("注入潜力").bindFnByName(typeof(activityGet), "OpenDuanzao", 10);
                        MultyMenu m5 = new MultyMenu("兑换潜力符石").bindFnByName(typeof(activityGet), "exchangeQlfs");
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "xqs":
                    {
                        MultyMenu m = new MultyMenu("镶嵌师");
                        MultyMenu m1 = new MultyMenu("装备打孔").bindFnByName(typeof(activityGet), "OpenDuanzao", 6);
                        MultyMenu m2 = new MultyMenu("镶嵌宝石").bindFnByName(typeof(activityGet), "OpenDuanzao", 7);
                        MultyMenu m3 = new MultyMenu("合成宝石").bindFnByName(typeof(activityGet), "OpenDuanzao", 8);
                        MultyMenu m4 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "shjn":
                    {
                        MultyMenu m = new MultyMenu("生活技能");
                        MultyMenu m1 = new MultyMenu("生活技能学习/升级").bindFnByName(typeof(activityGet), "OpenLiftSkill", 0);
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "bds":
                    {
                        MultyMenu m = new MultyMenu("绑定师");
                        MultyMenu m1 = new MultyMenu("绑定装备").bindFnByName(typeof(activityGet), "OpenDuanzao", 3);
                        MultyMenu m2 = new MultyMenu("解绑装备").bindFnByName(typeof(activityGet), "OpenDuanzao", 4);
                        MultyMenu m3 = new MultyMenu("装备刻印").bindFnByName(typeof(activityGet), "OpenDuanzao", 5);
                        MultyMenu m4 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "dzs":
                    {
                        MultyMenu m = new MultyMenu("锻造师");
                        MultyMenu m1 = new MultyMenu("锻造装备").bindFnByName(typeof(activityGet), "OpenDuanzao", 0);
                        MultyMenu m2 = new MultyMenu("修复装备").bindFnByName(typeof(activityGet), "OpenDuanzao", 1);
                        MultyMenu m3 = new MultyMenu("合成锻造宝石").bindFnByName(typeof(activityGet), "OpenDuanzao", 2);
                        MultyMenu m4 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "douchong":
                    {
                        MultyMenu m = new MultyMenu("斗宠");
                        MultyMenu m1 = new MultyMenu("进入宠物乐园").bindFnByName(typeof(activityGet), "toMap", "cwly");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "jmsr":
                    {
                        MultyMenu m = new MultyMenu("寄卖商人");
                        MultyMenu m1 = new MultyMenu("寄卖商城").bindFnByName(typeof(activityGet), "OpenJiMai", 0);
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "zbsr":
                    {
                        MultyMenu m = new MultyMenu("装备商人");
                        MultyMenu m1 = new MultyMenu("购买装备").bindFnByName(typeof(activityGet), "OpenEquipShop", 0);
                        MultyMenu m2 = new MultyMenu("出售").bindFnByName(typeof(activityGet), "OpenEquipShop", 1);
                        MultyMenu m3 = new MultyMenu("自动出售杂物").bindFnByName(typeof(activityGet), "OpenEquipShop", 1);
                        MultyMenu m4 = new MultyMenu("自动出售白装").bindFnByName(typeof(activityGet), "OpenEquipShop", 1);
                        MultyMenu m5 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "ckgl":
                    {
                        MultyMenu m = new MultyMenu("仓库管理");
                        MultyMenu m1 = new MultyMenu("仓库扩容").bindFnByName(typeof(activityGet), "bbKr", 1);
                        MultyMenu m2 = new MultyMenu("背包扩容").bindFnByName(typeof(activityGet), "bbKr", 0);
                        MultyMenu m3 = new MultyMenu("存入银两").bindFnByName(typeof(activityGet), "showCkTale", 0);
                        MultyMenu m4 = new MultyMenu("取出银两").bindFnByName(typeof(activityGet), "showCkTale", 1);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "wzyc":
                    {
                        MultyMenu m = new MultyMenu("王者遗产");
                        MultyMenu m1 = new MultyMenu("王者遗产").bindFnByName(typeof(activityGet), "showWzyc");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "cwsr":
                    {
                        MultyMenu m = new MultyMenu("宠物商人");
                        MultyMenu m1 = new MultyMenu("兑换神符").bindFnByName(typeof(activityGet), "OpenPetUp", 4);
                        MultyMenu m2 = new MultyMenu("宠物洗点").bindFnByName(typeof(activityGet), "OpenPetUp", 5);
                        MultyMenu m3 = new MultyMenu("宠物放生").bindFnByName(typeof(activityGet), "OpenPetUp", 6);
                        MultyMenu m4 = new MultyMenu("宠物炼化").bindFnByName(typeof(activityGet), "OpenPetUp", 7);
                        MultyMenu m5 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "cwqh":
                    {
                        MultyMenu m = new MultyMenu("宠物强化");
                        MultyMenu m1 = new MultyMenu("宠物重生").bindFnByName(typeof(activityGet), "OpenPetUp", 0);
                        MultyMenu m2 = new MultyMenu("资质强化").bindFnByName(typeof(activityGet), "OpenPetUp", 1);
                        MultyMenu m3 = new MultyMenu("基础强化").bindFnByName(typeof(activityGet), "OpenPetUp", 2);
                        MultyMenu m4 = new MultyMenu("技能强化").bindFnByName(typeof(activityGet), "OpenPetUp", 3);
                        MultyMenu m5 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "phb":
                    {
                        MultyMenu m = new MultyMenu("排行榜");
                        MultyMenu m1 = new MultyMenu("排行榜").bindFnByName(typeof(activityGet), "OpenPaiHang", 0);
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "seb":
                    {
                        MultyMenu m = new MultyMenu("善恶榜");
                        MultyMenu m1 = new MultyMenu("善恶榜").bindFnByName(typeof(activityGet), "OpenSebOrder", 1);
                        MultyMenu m2 = new MultyMenu("通缉任务").bindFnByName(typeof(activityGet), "OpenSebOrder", 0);
                        MultyMenu m3 = new MultyMenu("兑换人气值").bindFnByName(typeof(activityGet), "OpenSebOrder", 2);
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "jrhd":
                    {
                        MultyMenu m = new MultyMenu("节日活动");
                        MultyMenu m1 = new MultyMenu("领取节日礼物").bindFnByName(typeof(activityGet), "gainHolidayGift");
                        MultyMenu m2 = new MultyMenu("秒杀商城").bindFnByName(typeof(activityGet), "OpenMiaoSha", 0);
                        MultyMenu m3 = new MultyMenu("进入婚姻场景").bindFnByName(typeof(activityGet), "toMap", "hunyin");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "bjxs":
                    {
                        MultyMenu m = new MultyMenu("不倦先生");
                        MultyMenu m1 = new MultyMenu("进入孔庙").bindFnByName(typeof(activityGet), "toMap", "kongmiao1");
                        MultyMenu m2 = new MultyMenu("兑换漆黑的毛笔").bindFnByName(typeof(activityGet), "", npcKey);
                        MultyMenu m3 = new MultyMenu("收藏家");
                        MultyMenu m3_1 = new MultyMenu("召唤神宠");
                        m3.addMenu(m3_1);
                        MultyMenu m3_1_1 = new MultyMenu("召唤逆天魔龙（+20金）").bindFnByName(typeof(activityGet), "exchangeShenShou", 0);
                        MultyMenu m3_1_2 = new MultyMenu("召唤惑天玄姬（+20金）").bindFnByName(typeof(activityGet), "exchangeShenShou", 1);
                        MultyMenu m3_1_3 = new MultyMenu("召唤玄女宝鉴（+20金）").bindFnByName(typeof(activityGet), "exchangeShenShou", 2);
                        MultyMenu m3_1_4 = new MultyMenu("召唤天兵帅符（+20金）").bindFnByName(typeof(activityGet), "exchangeShenShou", 3);
                        MultyMenu m3_1_5 = new MultyMenu("召唤玄晶天狐（+20金）").bindFnByName(typeof(activityGet), "exchangeShenShou", 4);
                        m3_1.addMenu(m3_1_1, m3_1_2, m3_1_3, m3_1_4, m3_1_5);

                        MultyMenu m4 = new MultyMenu("神影自选");
                        MultyMenu m4_1 = new MultyMenu("兑换魔龙残影").bindFnByName(typeof(activityGet), "shenYingExchange", 0);
                        MultyMenu m4_2 = new MultyMenu("兑换玄姬冰雕").bindFnByName(typeof(activityGet), "shenYingExchange", 1);
                        MultyMenu m4_3 = new MultyMenu("兑换玄女宝鉴").bindFnByName(typeof(activityGet), "shenYingExchange", 2);
                        MultyMenu m4_4 = new MultyMenu("兑换天兵帅符").bindFnByName(typeof(activityGet), "shenYingExchange", 3);
                        MultyMenu m4_5 = new MultyMenu("兑换兽神残影").bindFnByName(typeof(activityGet), "shenYingExchange", 4);
                        m4.addMenu(m4_1, m4_2, m4_3, m4_4, m4_5);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "shitu":
                    {
                        MultyMenu m = new MultyMenu("师徒");
                        MultyMenu m1 = new MultyMenu("师徒榜").bindFnByName(typeof(activityGet), "OpenShiTu", 1);
                        MultyMenu m2 = new MultyMenu("登记收徒").bindFnByName(typeof(activityGet), "OpenShiTu", 0);
                        MultyMenu m3 = new MultyMenu("登记拜师").bindFnByName(typeof(activityGet), "OpenShiTu", 0);
                        MultyMenu m4 = new MultyMenu("解除师徒关系").bindFnByName(typeof(activityGet), "OpenShiTu", 0);
                        MultyMenu m5 = new MultyMenu("取消登记").bindFnByName(typeof(activityGet), "OpenShiTu", 0);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "yxl":
                    {
                        MultyMenu m = new MultyMenu("英雄擂");
                        MultyMenu m1 = new MultyMenu("参加英雄擂").bindFnByName(typeof(activityGet), "OpenLeiTai", 0);
                        MultyMenu m2 = new MultyMenu("排行").bindFnByName(typeof(activityGet), "OpenLeiTai", 1);
                        MultyMenu m3 = new MultyMenu("积分商城").bindFnByName(typeof(activityGet), "OpenLeiTai", 2);
                        MultyMenu m4 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "", npcKey);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "dmkj":
                    {
                        MultyMenu m = new MultyMenu("盗梦空间");
                        MultyMenu m0 = new MultyMenu("进入血腥之地").bindFnByName(typeof(activityGet), "toMap", "xxzd");
                        MultyMenu m1 = new MultyMenu("进入盗梦空间");

                        MultyMenu m1_1 = new MultyMenu("Lv70-凶兽梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj1");
                        MultyMenu m1_2 = new MultyMenu("Lv75-恶鬼梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj2");
                        MultyMenu m1_3 = new MultyMenu("Lv80-修罗梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj3");
                        MultyMenu m1_4 = new MultyMenu("Lv85-噬人梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj4");
                        MultyMenu m1_5 = new MultyMenu("Lv90-腐地梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj5");
                        MultyMenu m1_6 = new MultyMenu("Lv95-焚天梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj6");
                        MultyMenu m1_7 = new MultyMenu("Lv100-十绝梦境").bindFnByName(typeof(activityGet), "toMap", "dmkj7");
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6, m1_7);

                        MultyMenu m2 = new MultyMenu("盗梦商城").bindFnByName(typeof(activityGet), "OpenDmkjShop", 0);
                        MultyMenu m3 = new MultyMenu("出售单件盗梦装备").bindFnByName(typeof(activityGet), "OpenDmkjShop", 2);
                        MultyMenu m4 = new MultyMenu("出售全部盗梦装备").bindFnByName(typeof(activityGet), "OpenDmkjShop", 2);
                        m.addMenu(m0, m1, m2, m3, m4);
                        return m.menus;
                    }
                case "dmkj_1":
                    {
                        MultyMenu m = new MultyMenu("盗梦空间");
                        MultyMenu m1 = new MultyMenu("兑换奖励").bindFnByName(typeof(activityGet), "OpenDmkjShop", 0);
                        MultyMenu m2 = new MultyMenu("离开空间").bindFnByName(typeof(activityGet), "toMap", "m_1");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "bpgl":
                    {
                        MultyMenu m = new MultyMenu("帮派管理");
                        MultyMenu m1 = new MultyMenu("回到帮派").bindFnByName(typeof(activityGet), "toMap", "gangs");
                        MultyMenu m2 = new MultyMenu("帮派列表").bindFnByName(typeof(activityGet), "bpList");
                        MultyMenu m3 = new MultyMenu("创建帮派").bindFnByName(typeof(activityGet), "createBp");
                        MultyMenu m4 = new MultyMenu("解散帮派").bindFnByName(typeof(activityGet), "bpJieSan");
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "dzfsb":
                    {
                        MultyMenu m = new MultyMenu("斗战封神榜");
                        MultyMenu m1 = new MultyMenu("封神榜").bindFnByName(typeof(activityGet), "OpenDzfsbPage", 0);
                        MultyMenu m2 = new MultyMenu("锻体健骨").bindFnByName(typeof(activityGet), "", npcKey);
                        MultyMenu m3 = new MultyMenu("仙元丹自选");
                        MultyMenu m3_1 = new MultyMenu("暴击仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 0);
                        MultyMenu m3_2 = new MultyMenu("闪避仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 1);
                        MultyMenu m3_3 = new MultyMenu("命中仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 2);
                        MultyMenu m3_4 = new MultyMenu("法术仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 3);
                        MultyMenu m3_5 = new MultyMenu("生命仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 4);
                        MultyMenu m3_6 = new MultyMenu("法防仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 5);
                        MultyMenu m3_7 = new MultyMenu("物防仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 6);
                        MultyMenu m3_8 = new MultyMenu("法攻仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 7);
                        MultyMenu m3_9 = new MultyMenu("物攻仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 8);
                        MultyMenu m3_10 = new MultyMenu("速度仙元丹").bindFnByName(typeof(activityGet), "xianYuanDanExchange", 9);
                        m3.addMenu(m3_1, m3_2, m3_3, m3_4, m3_5, m3_6, m3_7, m3_8, m3_9, m3_10);
                        //MultyMenu m4 = new MultyMenu("高级宠技自选").bindFnByName(typeof(activityGet), "gjPetSklExchange");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "jycs":
                    {
                        MultyMenu m = new MultyMenu("监狱传送");
                        MultyMenu m1 = new MultyMenu("监狱传送").bindFnByName(typeof(activityGet), "toMap", "prison");
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "", npcKey);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "mpjnStudy":
                    {
                        MultyMenu m = new MultyMenu("分堂技能");
                        MultyMenu m1 = new MultyMenu("技能学习及升级").bindFnByName(typeof(activityGet), "learnJobSkill", npcKey);
                        MultyMenu m2 = new MultyMenu("加入分堂").bindFnByName(typeof(activityGet), "joinFenTang", npcKey);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "joinMenPai":
                    {
                        MultyMenu m = new MultyMenu("加入门派");
                        MultyMenu m1 = new MultyMenu("技能学习及升级").bindFnByName(typeof(activityGet), "learnMpSkill", npcKey);
                        MultyMenu m2 = new MultyMenu("加入门派").bindFnByName(typeof(activityGet), "joinMenPai", npcKey);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "fb":
                    {
                        MultyMenu m = new MultyMenu("副本入口");
                        MultyMenu m1 = new MultyMenu("进入副本").bindFnByName(typeof(activityGet), "toFuben", npcKey);
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        if (npcKey.Equals("10000379") || npcKey.Equals("10000399") || npcKey.Equals("10000413") || npcKey.Equals("10000454"))
                        {
                            MultyMenu m3 = new MultyMenu("名匠之魂兑换道具");
                            MultyMenu m3_1 = new MultyMenu("10个兑换暴击抵抗玉石").bindFnByName(typeof(activityGet), "exchangeFbGd", 0);
                            MultyMenu m3_2 = new MultyMenu("10个兑换睡眠抵抗玉石").bindFnByName(typeof(activityGet), "exchangeFbGd", 1);
                            MultyMenu m3_3 = new MultyMenu("10个兑换混乱抵抗玉石").bindFnByName(typeof(activityGet), "exchangeFbGd", 2);
                            MultyMenu m3_4 = new MultyMenu("10个兑换流血抵抗玉石").bindFnByName(typeof(activityGet), "exchangeFbGd", 3);
                            MultyMenu m3_5 = new MultyMenu("20个兑换名匠石磨").bindFnByName(typeof(activityGet), "exchangeFbGd", 4);
                            MultyMenu m3_6 = new MultyMenu("200个兑换千年玄铁").bindFnByName(typeof(activityGet), "exchangeFbGd", 5);
                            MultyMenu m3_7 = new MultyMenu("500个兑换血契之石").bindFnByName(typeof(activityGet), "exchangeFbGd", 6);
                            m3.addMenu(m3_1, m3_2, m3_3, m3_4, m3_5, m3_6, m3_7);
                            m.addMenu(m3);
                            MultyMenu m4 = new MultyMenu("魔龙残影兑换名匠之魂").bindFnByName(typeof(activityGet), "exchangByMlcy");
                            m.addMenu(m4);

                        }

                        return m.menus;
                    }
                case "fb_yzj":
                    {
                        MultyMenu m = new MultyMenu("隐藏副本");
                        MultyMenu m1 = new MultyMenu("进入隐藏副本").bindFnByName(typeof(activityGet), "toFuben", npcKey);
                        MultyMenu m2 = new MultyMenu("说明").bindFnByName(typeof(activityGet), "GetDes", key);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "bpnw":
                    {
                        MultyMenu m = new MultyMenu("帮派内务");
                        MultyMenu m1 = new MultyMenu("帮派建设");

                        MultyMenu m1_1 = new MultyMenu("商店建设");
                        MultyMenu m1_1_1 = new MultyMenu("提交炼神木").bindFnByName(typeof(activityGet), "putGP", "shop", "10000172");
                        MultyMenu m1_1_2 = new MultyMenu("提交玄铁矿").bindFnByName(typeof(activityGet), "putGP", "shop", "10000173");
                        m1_1.addMenu(m1_1_1, m1_1_2);
                        MultyMenu m1_2 = new MultyMenu("建筑建设");
                        MultyMenu m1_2_1 = new MultyMenu("提交炼神木").bindFnByName(typeof(activityGet), "putGP", "tec", "10000172");
                        MultyMenu m1_2_2 = new MultyMenu("提交玄铁矿").bindFnByName(typeof(activityGet), "putGP", "tec", "10000173");
                        m1_2.addMenu(m1_2_1, m1_2_2);
                        MultyMenu m1_3 = new MultyMenu("人才建设");
                        MultyMenu m1_3_1 = new MultyMenu("提交炼神木").bindFnByName(typeof(activityGet), "putGP", "solicit", "10000172");
                        MultyMenu m1_3_2 = new MultyMenu("提交玄铁矿").bindFnByName(typeof(activityGet), "putGP", "solicit", "10000173");
                        m1_3.addMenu(m1_3_1, m1_3_2);
                        MultyMenu m1_4 = new MultyMenu("技能建设");
                        MultyMenu m1_4_1 = new MultyMenu("提交炼神木").bindFnByName(typeof(activityGet), "putGP", "book", "10000172");
                        MultyMenu m1_4_2 = new MultyMenu("提交玄铁矿").bindFnByName(typeof(activityGet), "putGP", "book", "10000173");
                        m1_4.addMenu(m1_4_1, m1_4_2);
                        MultyMenu m1_5 = new MultyMenu("秘法建设");
                        MultyMenu m1_5_1 = new MultyMenu("提交炼神木").bindFnByName(typeof(activityGet), "putGP", "mifa", "10000172");
                        MultyMenu m1_5_2 = new MultyMenu("提交玄铁矿").bindFnByName(typeof(activityGet), "putGP", "mifa", "10000173");
                        m1_5.addMenu(m1_5_1, m1_5_2);

                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5);

                        MultyMenu m2 = new MultyMenu("接取帮派任务").bindFnByName(typeof(activityGet), "getBpTask");
                        MultyMenu m3 = new MultyMenu("捐献建设金").bindFnByName(typeof(activityGet), "putBpMoney");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "kaifu":
                    {
                        MultyMenu m = new MultyMenu("开服活动");
                        MultyMenu m1 = new MultyMenu("等级礼包");

                        MultyMenu m1_1 = new MultyMenu("领取30级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 0);
                        MultyMenu m1_2 = new MultyMenu("领取40级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 1);
                        MultyMenu m1_3 = new MultyMenu("领取50级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 2);
                        MultyMenu m1_4 = new MultyMenu("领取60级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 3);
                        MultyMenu m1_5 = new MultyMenu("领取70级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 4);
                        MultyMenu m1_6 = new MultyMenu("领取80级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 5);
                        MultyMenu m1_7 = new MultyMenu("领取90级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 6);
                        MultyMenu m1_8 = new MultyMenu("领取100级礼包").bindFnByName(typeof(activityGet), "gainOpenServerLvGift", 7);
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6, m1_7, m1_8);

                        MultyMenu m2 = new MultyMenu("领取助力升级礼").bindFnByName(typeof(activityGet), "gainOpenServerZhuLiGift");
                        MultyMenu m3 = new MultyMenu("冲刺等级排名赛").bindFnByName(typeof(activityGet), "aboutOpenServerAc");
                        m.addMenu(m1, m2, m3);
                        return m.menus;
                    }
                case "shenjiduihuan":
                    {
                        MultyMenu m = new MultyMenu("神技兑换");
                        MultyMenu m1 = new MultyMenu("神技兑换");

                        MultyMenu m1_1 = new MultyMenu("血魔缚命").bindFnByName(typeof(activityGet), "chooseSkillGain", 0);
                        MultyMenu m1_2 = new MultyMenu("仙术回血").bindFnByName(typeof(activityGet), "chooseSkillGain", 1);
                        MultyMenu m1_3 = new MultyMenu("浴火重生").bindFnByName(typeof(activityGet), "chooseSkillGain", 2);
                        MultyMenu m1_4 = new MultyMenu("裂骨鬼刃").bindFnByName(typeof(activityGet), "chooseSkillGain", 3);
                        MultyMenu m1_5 = new MultyMenu("踏雪无痕").bindFnByName(typeof(activityGet), "chooseSkillGain", 4);
                        MultyMenu m1_6 = new MultyMenu("玄心破霄").bindFnByName(typeof(activityGet), "chooseSkillGain", 5);
                        MultyMenu m1_7 = new MultyMenu("天赐木灵").bindFnByName(typeof(activityGet), "chooseSkillGain", 6);
                        MultyMenu m1_8 = new MultyMenu("天赐火灵").bindFnByName(typeof(activityGet), "chooseSkillGain", 7);
                        MultyMenu m1_9 = new MultyMenu("天赐土灵").bindFnByName(typeof(activityGet), "chooseSkillGain", 8);
                        MultyMenu m1_10 = new MultyMenu("天赐金灵").bindFnByName(typeof(activityGet), "chooseSkillGain", 9);
                        MultyMenu m1_11 = new MultyMenu("天赐水灵").bindFnByName(typeof(activityGet), "chooseSkillGain", 10);

                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6, m1_7, m1_8, m1_9, m1_10, m1_11);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "vipGift":
                    {
                        MultyMenu m = new MultyMenu("特权礼包");
                        MultyMenu m1 = new MultyMenu("特权礼包");

                        MultyMenu m1_1 = new MultyMenu("领取v1礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 1);
                        MultyMenu m1_2 = new MultyMenu("领取v2礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 2);
                        MultyMenu m1_3 = new MultyMenu("领取v3礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 3);
                        MultyMenu m1_4 = new MultyMenu("领取v4礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 4);
                        MultyMenu m1_5 = new MultyMenu("领取v5礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 5);
                        MultyMenu m1_6 = new MultyMenu("领取v6礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 6);
                        MultyMenu m1_7 = new MultyMenu("领取v7礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 7);
                        MultyMenu m1_8 = new MultyMenu("领取v8礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 8);
                        MultyMenu m1_9 = new MultyMenu("领取v9礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 9);
                        MultyMenu m1_10 = new MultyMenu("领取v10礼包").bindFnByName(typeof(activityGet), "gainVipLvGift", 10);
                        m1.addMenu(m1_1, m1_2, m1_3, m1_4, m1_5, m1_6, m1_7, m1_8, m1_9, m1_10);
                        //MultyMenu m2 = new MultyMenu("领取积分礼包").bindFnByName(typeof(activityGet), "gainLeiJiDangGift");
                        //MultyMenu m2 = new MultyMenu("领取金票").bindFnByName(typeof(activityGet), "gainLjdGoldGift");
                        //MultyMenu m3 = new MultyMenu("领取仙决/强装").bindFnByName(typeof(activityGet), "gainLjdXianJueGift");
                        MultyMenu m2 = new MultyMenu("技能碎片兑换");
                        MultyMenu m2_1 = new MultyMenu("兑换仙决宝箱").bindFnByName(typeof(activityGet), "sklSpExchange", 1);
                        MultyMenu m2_2 = new MultyMenu("兑换一本天地技").bindFnByName(typeof(activityGet), "sklSpExchange", 2);
                        MultyMenu m2_3 = new MultyMenu("兑换一本宠技").bindFnByName(typeof(activityGet), "sklSpExchange", 3);
                        m2.addMenu(m2_1, m2_2, m2_3);

                        MultyMenu m3 = new MultyMenu("特权印象分领神宠").bindFnByName(typeof(activityGet), "qdGainYuanBao", 1);
                        MultyMenu m4 = new MultyMenu("特权印象分领帝童").bindFnByName(typeof(activityGet), "qdGainYuanBao", 2);
                        MultyMenu m5 = new MultyMenu("特权签到领元宝").bindFnByName(typeof(activityGet), "qdGainYuanBao", 3);
                        m.addMenu(m1, m2, m3, m4, m5);
                        return m.menus;
                    }
                case "bphf":
                    {
                        MultyMenu m = new MultyMenu("帮派护法");
                        MultyMenu m1 = new MultyMenu("帮战报名").bindFnByName(typeof(activityGet), "OpenBpAc", 0);
                        MultyMenu m2 = new MultyMenu("帮战排行").bindFnByName(typeof(activityGet), "OpenBpOrder", 0);
                        MultyMenu m3 = new MultyMenu("封赏演武堂").bindFnByName(typeof(activityGet), "OpenBpAc", 1);
                        MultyMenu m4 = new MultyMenu("碎玉帛兑换").bindFnByName(typeof(activityGet), "OpenBpAc", 2);
                        m.addMenu(m1, m2, m3, m4);
                        return m.menus;
                    }
                case "flds":
                    {
                        MultyMenu m = new MultyMenu("福利大使");
                        MultyMenu m1 = new MultyMenu("传璧奖励").bindFnByName(typeof(activityGet), "OpenBpOrder", 1);
                        MultyMenu m2 = new MultyMenu("帮贡商店").bindFnByName(typeof(activityGet), "OpenBpAc", 2);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "yfzl":
                    {
                        MultyMenu m = new MultyMenu("研发长老");
                        MultyMenu m1 = new MultyMenu("学习生活技能").bindFnByName(typeof(activityGet), "OpenLiftSkill", 0);
                        MultyMenu m2 = new MultyMenu("研发生活技能").bindFnByName(typeof(activityGet), "OpenLiftSkill", 1);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "shxb":
                    {
                        MultyMenu m = new MultyMenu("守护玄碑");
                        MultyMenu m1 = new MultyMenu("帮派buff");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "jstz":
                    {
                        MultyMenu m = new MultyMenu("建设堂主");
                        MultyMenu m1 = new MultyMenu("帮派升级").bindFnByName(typeof(activityGet), "upLvBp");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "bgzl":
                    {
                        MultyMenu m = new MultyMenu("帮贡长老");
                        MultyMenu m1 = new MultyMenu("贡献度查看").bindFnByName(typeof(activityGet), "OpenBpOrder", 2);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "cszl":
                    {
                        MultyMenu m = new MultyMenu("传送长老");
                        MultyMenu m1 = new MultyMenu("传送到帮战场景").bindFnByName(typeof(activityGet), "toMap", "bz");
                        MultyMenu m2 = new MultyMenu("传送到云中界").bindFnByName(typeof(activityGet), "toMap", "yzj_1");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "houzi":
                    {
                        MultyMenu m = new MultyMenu("猴子");
                        MultyMenu m1 = new MultyMenu("揍它").bindFnByName(typeof(activityGet), "attackMonkey");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "houshanshouwei":
                    {
                        string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                        if (mapKey.Equals("txzf")) mapKey = "tianshoufeng";
                        else if (mapKey.Equals("txzl")) mapKey = "mizonglin";
                        else if (mapKey.Equals("txzg")) mapKey = "taixugu";
                        MultyMenu m = new MultyMenu("后山守卫");
                        MultyMenu m1 = new MultyMenu("前往禁地").bindFnByName(typeof(activityGet), "toMap", mapKey);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "jldh":
                    {
                        MultyMenu m = new MultyMenu("聚灵兑换");
                        MultyMenu m1 = new MultyMenu("兑换仙石").bindFnByName(typeof(activityGet), "openJuLingPage");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "chuansong":
                    {
                        MultyMenu m = new MultyMenu("传送");
                        MultyMenu m1 = new MultyMenu("传送云中界").bindFnByName(typeof(activityGet), "toMap", "yzj_1");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "paoshang_task":
                    {
                        MultyMenu m = new MultyMenu("跑商任务");
                        MultyMenu m1 = new MultyMenu("开始跑商").bindFnByName(typeof(activityGet), "startPs");
                        MultyMenu m2 = new MultyMenu("完成跑商").bindFnByName(typeof(activityGet), "compilePs");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "paoshang_buy":
                    {
                        MultyMenu m = new MultyMenu("跑商任务");
                        MultyMenu m1 = new MultyMenu("购买货物").bindFnByName(typeof(activityGet), "openPsShop", 0, npcKey);
                        MultyMenu m2 = new MultyMenu("出售货物").bindFnByName(typeof(activityGet), "openPsShop", 0, npcKey);
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "bz_box":
                    {
                        MultyMenu m = new MultyMenu("帮战箱子");
                        MultyMenu m1 = new MultyMenu("抢占").bindFnByName(typeof(activityGet), "zhanlingBzBox", npcKey);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "bz_npc":
                    {
                        MultyMenu m = new MultyMenu("帮战npc");
                        MultyMenu m1 = new MultyMenu("积分排行").bindFnByName(typeof(activityGet), "bzJfOrder");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "mpdsx":
                    {
                        MultyMenu m = new MultyMenu("门派大师兄");
                        MultyMenu m1 = new MultyMenu("报名门派大师兄").bindFnByName(typeof(activityGet), "signDsx");
                        MultyMenu m2 = new MultyMenu("进入赛场").bindFnByName(typeof(activityGet), "toMap", "dsx");
                        m.addMenu(m1, m2);
                        return m.menus;
                    }
                case "prison":
                    {
                        MultyMenu m = new MultyMenu("监狱");
                        MultyMenu m1 = new MultyMenu("贿赂").bindFnByName(typeof(activityGet), "huilu");
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "toMenPai":
                    {
                        MultyMenu m = new MultyMenu("传送");
                        MultyMenu m1 = new MultyMenu("传送至门派").bindFnByName(typeof(activityGet), "toMpMap", npcKey);
                        m.addMenu(m1);
                        return m.menus;
                    }
                case "xxzd":
                    {
                        MultyMenu m = new MultyMenu("血腥之地");
                        MultyMenu m1 = new MultyMenu("血腥徽章收集").bindFnByName(typeof(activityGet), "xxzdJfOrder");
                        m.addMenu(m1);
                        return m.menus;
                    }
            }
            return null;
        }
        //90级 青丘境
        //100级副本 混沌邪灵渊
        //95 赤炼洞窟
        //95 浮游城
        //100 隐龙古城
        //100 魔界之门
        private List<MultyMenu> retMenus(params MultyMenu[] ms)
        {
            return new List<MultyMenu>(ms);
        }
    }
    public class MultyMenu
    {
        public string name;
        public List<MultyMenu> menus;

        public MultyMenu(string name)
        {
            this.name = name;
        }
        public MultyMenu addMenu(params MultyMenu[] childs)
        {
            if (menus == null) menus = new List<MultyMenu>();
            menus.AddRange(childs);
            return this;
        }
        public Type type;
        public string fnName;
        public object[] ps;
        public MultyMenu bindFnByName(Type type, string fnName, params object[] ps)
        {
            this.type = type;
            this.fnName = fnName;
            this.ps = ps;
            return this;
        }
    }
    class act
    {
        public string key;
        public string name;
        //活动信息
        public JObject acMsg;
        //奖励
        public JArray rewards;
        //子菜单
        public List<MultyMenu> menus;
        //位置信息
        public JObject posMsg;
        public act(string key, string name)
        {
            this.key = key;
            this.name = name;
        }
        public act addMenu(params MultyMenu[] childs)
        {
            if (menus == null) menus = new List<MultyMenu>();
            menus.AddRange(childs);
            return this;
        }
        /**添加活动信息*/
        public act addAcMsg(int times, int jf, int limitLv = 20, int multy = 0, string openTime = "全天（凌晨0点刷新）")
        {
            JObject item = new JObject();
            item.Add("times", times);//-1无限
            item.Add("jf", jf);//活跃度
            item.Add("openTime", openTime);//开放时间
            item.Add("limitLv", limitLv);//限制等级
            item.Add("multy", multy);//0单人1多人
            this.acMsg = item;
            return this;
        }
        /**放置奖励 num=-1为任意*/
        public act addReward(string key, int num)
        {
            if (this.rewards == null) this.rewards = new JArray();
            JObject a = new JObject();
            a.Add("key", key);
            a.Add("num", num);
            this.rewards.Add(a);
            return this;
        }
        /**任务详情*/
        public act addDes(string des = "暂无活动详情")
        {
            this.acMsg.Add("des", des);
            return this;
        }
        public act addMapKey(string mapKey, string npcKey)
        {
            this.posMsg = new JObject();
            this.posMsg.Add("mapKey", mapKey);
            this.posMsg.Add("npcKey", npcKey);
            return this;
        }
    }
}
