using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface gangsInterface
    {
        public void reqInto(string Id);
        public void create(string gangsName, Action callback);
        public void getBpMsg(Action<JObject> callback);
        public string getGangsAcDes(int i);
        public void changeJob(string playerName, string job, Action callback);
        public void tichuGangs(string playerName, Action callback);
        public void existGangs(Action callback);
        public void agreeReq(string playerName, Action callback);
        public void rejectReq(string playerName, Action callback);
        public void gainDayRewards(Action callback);
        public void isGainDayRewards(Action<JObject> callback);
        public void putGP(string buildKey, string key, int num, Action callback);
        public void getBuildV(Action<JObject> callback);
        public void upLvGangs(Action callback);
        public void openBPActivity(int type);
        public void getPsTask(Action callback);
        public void getPsGoods(string key, Action<JObject> callback);
        public void getMyPsGoods(Action<JObject> callback);
        public void buyPsGoods(string areaKey, string key, int num, Action<int> callback);
        public void getPsGoodsPrice(string areaKey, string key, Action<int> callback);
        public void salePsGoods(string key, int num, Action<int> callback);
        public void compilePs();
        public void signUpBz(Action ac);

        public void getBzBox(string npcKey, Action<JObject> callback);
        public void createFightByBz(string bzId, string boxKey);
        public void getSignUpBzOfGangs(Action<JArray> ac);
        public void getPkOfGangs(Action<JArray> ac);
        public void getExchangeShop(Action<JArray> callback);
        public void jfExchange(string key, int price, Action<JArray> callback);
        public void getFarm(Action<JObject> callback);
        public void plant(string zzKey, string ytKey, int itemIndex, Action callback);
        public void gainZw(string ytKey, int itemIndex, Action callback);
        public void remZw(string ytKey, int itemIndex, Action callback);
        public void viewList(int pageNum, Action<JObject> callback);
        public void viewMemberList(int pageNum, Action<JObject> callback);
        public void viewBPMsg(string Id, Action<JObject> callback);
        public void viewReqList(int pageNum, Action<JObject> callback);
        public void putBpMoney(int num, Action callback);
        public void attackMonkey(Action callback);
        public void getBzMbJf(Action<JArray> ac);
        public void getBpTask(Action callback);
    }
    public class gangsInterfaceImpl : gangsInterface
    {
        public void getBpTask(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/getBpTask", null, (res) =>
            {
                face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { res.ToString() });
                face.taskInterface.createOneStartTask(res.ToString(), 2);
                msgCode.showMsg(612);
                callback();
            });
        }
        public void attackMonkey(Action callback)
        {
            DoGet.getInstance().sendPost("/fightRpcService/attackMonkey", null, (res) =>
            {
                if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(971);
                    return;
                }
                callback();
            });
        }
        public void viewReqList(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/gangsService/viewReqList", dic, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void viewBPMsg(string Id, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/gangsService/viewBPMsg", dic, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void viewMemberList(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/gangsService/viewMemberList", dic, (res) =>
            {
                callback((JObject)res);
            });

        }
        public void viewList(int pageNum, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pageNum", pageNum);
            DoGet.getInstance().sendPost("/gangsService/viewList", dic, (res) =>
            {
                callback((JObject)res);
            });

        }
        /**铲除*/
        public void remZw(string ytKey, int itemIndex, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ytKey", ytKey);
            dic.Add("index", itemIndex);
            DoGet.getInstance().sendPost("/gangsService/remZw", dic, (res) =>
            {

                if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(922);
                    return;
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(923);
                    return;
                }
                callback();
            });

        }
        /**摘取*/
        public void gainZw(string ytKey, int itemIndex, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ytKey", ytKey);
            dic.Add("index", itemIndex);
            DoGet.getInstance().sendPost("/gangsService/gainZw", dic, (res) =>
            {

                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(918);
                    return;
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(920);
                    return;
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(921);
                    return;
                }
                callback();
            });

        }
        public void plant(string zzKey, string ytKey, int itemIndex, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("zzKey", zzKey);
            dic.Add("ytKey", ytKey);
            dic.Add("index", itemIndex);
            DoGet.getInstance().sendPost("/gangsService/plant", dic, (res) =>
            {

                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(918);
                    return;
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(919);
                    return;
                }
                face.goodsInterface.cutPlayerGoodsNumByKey(zzKey, 1);
                callback();
            });

        }
        /**获取农场作物*/
        public void getFarm(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/gangsService/getFarm", null, (res) =>
            {
                callback((JObject)res);
            });

        }
        /**获取可兑换的商品*/
        public void getExchangeShop(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/gangsService/getExchangeShop", null, (res) =>
            {
                callback((JArray)res);
            });

        }
        /**兑换*/
        public void jfExchange(string key, int price, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/gangsService/jfExchange", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("1188", price);
                face.rewardInterface.saveRewards((JArray)res);
                callback((JArray)res);
            });

        }
        public void getBzMbJf(Action<JArray> ac)
        {
            DoGet.getInstance().sendPost("/gangsService/getBzMbJf", null, (res) =>
            {
                ac((JArray)res);
            });
        }
        public void getPkOfGangs(Action<JArray> ac)
        {
            DoGet.getInstance().sendPost("/gangsService/getPkOfGangs", null, (res) =>
            {
                ac((JArray)res);
            });
        }
        public void getSignUpBzOfGangs(Action<JArray> ac)
        {
            DoGet.getInstance().sendPost("/gangsService/getSignUpBzOfGangs", null, (res) =>
            {
                ac((JArray)res);
            });
        }
        /**帮战抢夺*/
        public void createFightByBz(string bzId, string boxKey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("bzId", bzId);
            dic.Add("boxKey", boxKey);
            DoGet.getInstance().sendPost("/fightRpcService/createFightByBz", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {

                }
                else if (res.ToString().Equals("2"))
                {
                    msgCode.showMsg(681);
                }
                else
                {
                    msgCode.showMsg(0);
                }
            });
        }
        /**拾取帮战宝箱或获取宝箱信息*/
        public void getBzBox(string npcKey, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("npcKey", npcKey);
            DoGet.getInstance().sendPost("/gangsService/getBzBox", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {

                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(882);
                }
                else
                {
                    callback((JObject)res);
                }
            });
        }
        /**报名帮战*/
        public void signUpBz(Action ac)
        {
            DoGet.getInstance().sendPost("/gangsService/signUpBz", null, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(666);
                    ac();
                }
                else
                {
                    msgCode.showMsg(667);
                }
            });
        }
        /**提交跑商任务*/
        public void compilePs()
        {
            DoGet.getInstance().sendPost("/gangsService/compilePs", null, (res) =>
            {
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(999);
            });
        }
        /**出售跑商物品*/
        public void salePsGoods(string key, int num, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/gangsService/salePsGoods", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });
        }
        /**获取跑商物品的价格*/
        public void getPsGoodsPrice(string areaKey, string key, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("areaKey", areaKey);
            DoGet.getInstance().sendPost("/gangsService/getPsGoodsPrice", dic, (res) =>
            {
                callback(int.Parse(res.ToString()));
            });
        }
        /**购买跑商物品*/
        public void buyPsGoods(string areaKey, string key, int num, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("areaKey", areaKey);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/gangsService/buyPsGoods", dic, (res) =>
            {
                msgCode.showMsg(692);
                callback(int.Parse(res.ToString()));
            });
        }
        /**获取已经购买的物品*/
        public void getMyPsGoods(Action<JObject> callback)
        {

            DoGet.getInstance().sendPost("/gangsService/getMyPsGoods", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**获取跑商出售物品
         key p1-4
         */
        public void getPsGoods(string key, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/gangsService/getPsGoods", dic, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**开启帮派boss*/
        public void getPsTask(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/getPsTask", null, (res) =>
            {
                face.roleInterface.updateMoney(-20000, 1);
                msgCode.showMsg(612);
                callback();
            });
        }
        /**开启帮派boss*/
        public void openBPActivity(int type)
        {
            if (type == 1)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000174", 1))
                {
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/gangsService/openBPActivity", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(888);
                }
                else
                {
                    msgCode.showMsg(889);
                }
            });
        }
        /**升级帮派*/
        public void upLvGangs(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/upLvGangs", null, (res) =>
            {
                msgCode.showMsg(200);
                callback();
            });
        }
        public void getBuildV(Action<JObject> callback)
        {

            DoGet.getInstance().sendPost("/gangsService/getBuildV", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        public void putBpMoney(int num, Action callback)
        {
            if (num < 1000)
            {
                msgCode.showMsg(968);
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -num))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/gangsService/putBpMoney", dic, (res) =>
            {
                face.roleInterface.updateMoney(-num, 1);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(967, num);
                callback();
            });
        }
        /**贡献*/
        public void putGP(string buildKey, string key, int num, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(key, num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("buildKey", buildKey);
            dic.Add("key", key);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/gangsService/putGP", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, num);
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(965, num);
                callback();
            });
        }
        public void isGainDayRewards(Action<JObject> callback)
        {

            DoGet.getInstance().sendPost("/gangsService/isGainDayRewards", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**领取每日奖励*/
        public void gainDayRewards(Action callback)
        {

            DoGet.getInstance().sendPost("/gangsService/gainDayRewards", null, (res) =>
            {
                msgCode.showMsg(200);
                face.rewardInterface.saveRewards((JArray)res);
                callback();

            });
        }
        /**同意申请*/
        public void agreeReq(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/gangsService/agreeReq", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(200);
                    callback();
                }

            });
        }
        /**拒绝申请*/
        public void rejectReq(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/gangsService/rejectReq", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(200);
                    callback();
                }

            });
        }
        /**踢出帮派*/
        public void tichuGangs(string playerName, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            DoGet.getInstance().sendPost("/gangsService/tichuGangs", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(200);
                    callback();
                }

            });
        }
        /**主动离开帮派*/
        public void existGangs(Action callback)
        {
            DoGet.getInstance().sendPost("/gangsService/existGangs", null, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    msgCode.showMsg(200);
                    callback();
                }

            });
        }
        /**更改职位*/
        public void changeJob(string playerName, string job, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", playerName);
            dic.Add("job", job);
            DoGet.getInstance().sendPost("/gangsService/changeJob", dic, (res) =>
            {

                //刷新ui
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(649);
                }
                else if (int.Parse(res.ToString()) == 1)
                {
                    msgCode.showMsg(200);
                    callback();
                }
                else
                {
                    msgCode.showMsg(805);

                }
            });
        }
        /**获取帮派信息*/
        public void getBpMsg(Action<JObject> callback)
        {
            DoGet.getInstance().sendPost("/gangsService/getBpMsg", null, (res) =>
            {
                callback((JObject)res);
            });
        }
        /**创建帮派 */
        public void create(string gangsName, Action callback)
        {
            if (!strUtils.isEnoughLen(gangsName, 1, 6))
            {
                msgCode.showMsg(618);
                return;
            }
            //判断是否已经加入帮派
            JObject r = face.roleInterface.getRole();
            if (!r["attr"]["msg"]["bp"].ToString().Equals(""))
            {
                msgCode.showMsg(643);
                return;
            }

            //是否钱足够
            if (!face.roleInterface.isEnoughMoney("tale", -20000))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("gangsName", gangsName);
            DoGet.getInstance().sendPost("/gangsService/create", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(644);
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(767);
                }
                else
                {
                    JObject o = (JObject)res;
                    JObject r = face.roleInterface.getRole();
                    r["attr"]["msg"]["tale"] = (int)r["attr"]["msg"]["tale"] - 20000;
                    JObject bp = new JObject();
                    bp.Add("Id", o["Id"].ToString());
                    bp.Add("name", o["name"].ToString());
                    r["attr"]["msg"]["bp"] = bp;
                    face.roleInterface.saveRole(r);
                    msgCode.showMsg(200);
                    callback();
                }
            });

        }
        /**申请加入帮派 */
        public void reqInto(string Id)
        {
            JObject r = face.roleInterface.getRole();
            if (!r["attr"]["msg"]["bp"].ToString().Equals(""))
            {
                msgCode.showMsg(643);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/gangsService/reqInto", dic, (res) =>
            {

                if (int.Parse(res.ToString()) == 1)
                {
                    msgCode.showMsg(646);
                }
                else if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(647);
                }
                else if (int.Parse(res.ToString()) == -1)
                {
                    msgCode.showMsg(767);
                }

            });

        }
        /**获取活动描述*/
        public string getGangsAcDes(int i)
        {
            string[] arr =
            {
            "帮派秘境结束后开启。玩家各自出价竞争所需道具。",
            "",
            "帮贡兑换道具",
            "提升属性",
            "获得经验、帮贡",
            "周三20:00-21:00，帮派场景中每5分钟刷新一次宝箱（每场限领取一次）",
            "需帮主（副帮主）开启，每日每人限打一次（0点刷新）",
            "需帮主报名，参加帮战获得帮贡",
            "击退强盗获得帮贡",
            "每周开启一次",
            "参与跑商获得大量银两、帮贡，每日领取次数可随帮派等级增加",
        };
            return arr[i];
        }
    }
}
