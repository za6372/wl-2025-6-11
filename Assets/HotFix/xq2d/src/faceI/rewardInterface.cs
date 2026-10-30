using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
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

    public interface rewardInterface
    {
        public string saveRewards(JArray list);
        public void exchangeGoods(string k, int num, object param);
        public void collectionBox(string npcKey);
        public string getPathByKey(int type);
        public void getHongbaoList(int type, Action<JArray> callback);
        public void sendHongbao(int moneyType, int money, string talk, int type, Action callback);
        public void openHongbao(int type, string Id, Action callback);
        public void findTaskRewards(string taskKey, Action<JArray> callback);
        public JArray goodsToRewardFormat(JArray res);
        public void zhaomuAll(int type, int times, Action<JArray> callback);
        public void compoundSp(string key, int num, Action callback);
    }
    public class rewardInterfaceImpl : rewardInterface
    {
        /**碎片合成*/
        public void compoundSp(string key, int num, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("num", num);
            DoGet.getInstance().sendPost("/rewardService/compoundSp", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(key, num);
                string type = key.Substring(4, 4);
                if (type.Equals("1000"))//宠物
                {
                    JArray list = (JArray)res;
                    foreach (object l in list)
                    {
                        face.petInterface.savePet((JObject)l);
                    }
                }
                else if (type.Equals("1001"))//伙伴
                {
                    JArray list = (JArray)res;

                }
                msgCode.showMsg(898);
                callback();
            });
        }
        /**招募 type0宠物1伙伴*/
        public void zhaomuAll(int type, int times, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("times", times);
            DoGet.getInstance().sendPost("/rewardService/zhaomuAll", dic, (res) =>
            {
                string k = "10000051";
                if (type == 1) k = "10000059";
                face.goodsInterface.cutPlayerGoodsNumByKey(k, times);
                this.saveRewards((JArray)res);
                callback((JArray)res);
            });
        }
        public void findTaskRewards(string taskKey, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", taskKey);
            DoGet.getInstance().sendPost("/rewardService/findTaskRewards", dic, (res) =>
            {
                if (res == null)
                {
                    Debug.Log(taskKey + "未设置奖励");
                }
                else
                {
                    callback((JArray)res);
                }
            });
        }
        /**领取红包*/
        public void openHongbao(int type, string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/rewardService/openHongbao", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(668);
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(891);
                }
                else
                {
                    saveRewards((JArray)res);
                    msgCode.showMsg(892, ((JArray)res)[0]);
                }

                callback();
            });
        }
        /**发送红包*/
        public void sendHongbao(int moneyType, int money, string talk, int type, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("moneyType", moneyType);
            dic.Add("money", money);
            dic.Add("talk", talk);
            DoGet.getInstance().sendPost("/rewardService/sendHongbao", dic, (res) =>
            {
                callback();
            });
        }
        /**获取红包列表*/
        public void getHongbaoList(int type, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/rewardService/getHongbaoList", dic, (list) =>
            {
                callback((JArray)list);
            });
        }
        /**获取奖励途径 
        */
        public string getPathByKey(int type)
        {
            string[] arr = {
                "仙决宝箱","洪荒宝库", "梅老板礼包", "鬼见愁礼包","宠物神技礼包","宠物高级技能礼包","宠物普通技能礼包",
            };
            return arr[type];
        }
        /**npc宝箱*/
        public void collectionBox(string npcKey)
        {
            //根据npcKey区分活动接口
            if (npcKey.Equals("box1000") || npcKey.Equals("box1001") || npcKey.Equals("box1002"))
            {
                openBoxMJXW(npcKey);
            }
            else if (npcKey.Equals("box1003") || npcKey.Equals("box1004") || npcKey.Equals("box1005"))
            {
                openBoxYGMB(npcKey);
            }
            else if (npcKey.Equals("box1006"))
            {
                openBoxBpYanhui();
            }
            else if (npcKey.Equals("box1007"))
            {
                openBoxBpBoss();
            }

        }
        /**开帮派boss宝箱*/
        private void openBoxBpBoss()
        {
            DoGet.getInstance().sendPost("/gangsService/openBossReward", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(882);
                }
                else
                {
                    this.saveRewards((JArray)res);
                }
            });
        }
        /**开帮派宴会宝箱*/
        private void openBoxBpYanhui()
        {
            DoGet.getInstance().sendPost("/gangsService/openBox", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(882);
                }
                else
                {
                    this.saveRewards((JArray)res);
                }
            });
        }
        /**开幽谷秘宝的宝箱*/
        private void openBoxYGMB(string npcKey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("npcKey", npcKey);
            DoGet.getInstance().sendPost("/ygmbService/openBox", dic, (res) =>
            {
                if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(881);
                }
                else if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(882);
                }
                else
                {
                    this.saveRewards((JArray)res);
                }
            });
        }
        /**开摸金校尉的宝箱*/
        private void openBoxMJXW(string npcKey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("npcKey", npcKey);
            DoGet.getInstance().sendPost("/mjxwService/openBox", dic, (res) =>
            {
                if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(881);
                }
                else if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(882);
                }
                else
                {
                    saveRewards((JArray)res);
                }
            });
        }
        /**兑换道具(天晶)*/
        public void exchangeGoods(string k, int num, object param)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(k, num))
            {
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", k);
            dic.Add("params", param);
            DoGet.getInstance().sendPost("/rewardService/exchangeGoods", dic, (res) =>
            {
                if (!res.ToString().Equals("0"))
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(k, num);
                    saveRewards((JArray)res);
                    //textTipUI.getInstance().setText("兑换成功");
                }


            });
        }


        /**将奖励保存 */
        public string saveRewards(JArray list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.Log("未设置奖励");
                return null;
            }
            JArray gs = new JArray();
            string msg = "获得";
            string str0 = "";
            string str1 = "";
            string str2 = " 物品 ";
            string str3 = "";
            JObject role = face.roleInterface.getRole();
            for (int p = 0; p < list.Count; p++)
            {
                if ((int)list[p]["type"] == 0)
                {
                    int exp = (int)list[p]["exp"];
                    int petExp = (int)list[p]["petExp"];
                    bool b = face.roleInterface.addExp(exp, 30);
                    if (b)
                    {
                        //发生升级后才会检测任务的开启
                        face.taskInterface.checkTaskStart(()=> {
                            //刷新任务面板
                            face.taskInterface.noticeUIUpdate(0);
                        });
                    }
                    str0 += "角色经验：" + exp;
                    JObject pet = null;
                    if ((pet = face.petInterface.getIsFightPet()) != null)
                    {
                        face.petInterface.addExp(pet, petExp);
                        str0 += " 宠物经验：" + petExp;
                        if (list[p]["skls"] != null)
                        {
                            JArray skls = (JArray)list[p]["skls"];
                            face.petInterface.addSkillToPet(pet, skls);
                        }
                    }

                }
                else if ((int)list[p]["type"] == 1)
                {
                    if ((int)list[p]["moneyType"] == 0)
                    {
                        str1 += " 元宝：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 1)
                    {
                        str1 += " 银两：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 2)
                    {
                        str1 += " 银票：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 3)
                    {
                        str1 += " 积分：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 4)
                    {
                        str1 += " 武勋：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 5)
                    {
                        str1 += " 帮贡：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 6)
                    {
                        str1 += " 修炼点：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 7)
                    {
                        str1 += " 帮币：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 8)
                    {
                        str1 += " 经脉点数：" + list[p]["money"];
                    }
                    else if ((int)list[p]["moneyType"] == 9)
                    {
                        str1 += " 领地积分：" + list[p]["money"];
                    }
                    face.roleInterface.updateMoney((int)list[p]["money"], (int)list[p]["moneyType"]);
                }
                else if ((int)list[p]["type"] == 2)
                {
                    if (list[p]["goods"] == null)
                    {
                        face.chatInterface.writeSysMsg("背包空间不足，道具被丢弃！");
                        continue;
                    }
                    //返回一个数据库中完整的g
                    face.goodsInterface.savePlayerGoods((JObject)list[p]["goods"]);
                    GoodsDes a = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(list[p]["goods"]["key"].ToString());
                    str2 += "[" + a.name + "] x" + list[p]["goods"]["num"] + "、";
                    gs.Add(list[p]["goods"]);
                }
                else if ((int)list[p]["type"] == 3)
                {
                    face.petInterface.savePet((JObject)list[p]["pet"]);
                    str3 += " 宠物 [" + list[p]["pet"]["nickName"] + "] x1";
                }
                else if ((int)list[p]["type"] == 4)
                {//vip
                    role["attr"]["msg"]["vip"] = (int)role["attr"]["msg"]["vip"] + (int)list[p]["vip"];
                    face.roleInterface.saveRole(role);
                    str3 += " vip经验 +" + list[p]["vip"];
                }
                else if ((int)list[p]["type"] == 5)
                {//声望
                    face.roleInterface.upShengwang((int)list[p]["swExp"]);
                    str3 += " 声望 +" + list[p]["swExp"];
                }
            }

            str2 = str2.Length > 4 ? str2.Substring(0, str2.Length - 1) : "";
            msg += str0 + str1 + str2 + str3;
            face.chatInterface.writeSysMsg(msg);

            return msg;
        }
        /**将物品转化为奖励格式*/
        public JArray goodsToRewardFormat(JArray res)
        {
            JArray arr = new JArray();
            for (int i = 0; i < res.Count; i++)
            {
                JObject obj = (JObject)res[i];

                if ((int)obj["type"] == 0)//经验
                {
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey("10000093");
                    JObject a = new JObject();
                    a.Add("key", gd.key);
                    a.Add("icon", gd.icon);
                    a.Add("num", obj["exp"]);
                    arr.Add(a);
                }
                else if ((int)obj["type"] == 1)//货币
                {
                    string[] hb = { "10000094", "10000095", "10000096", "10000097", "10000098"
                                , "10000099", "10000100", "10000101", "10000102", "10000103"};
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(hb[(int)obj["moneyType"]]);
                    JObject a = new JObject();
                    a.Add("key", gd.key);
                    a.Add("icon", gd.icon);
                    a.Add("num", obj["money"]);
                    arr.Add(a);
                }
                else if ((int)obj["type"] == 2)//道具
                {
                    if (obj["goods"] == null)
                    {
                        continue;
                    }
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(obj["goods"]["key"].ToString());
                    if (gd == null)
                    {
                        Debug.Log(obj["goods"]["key"] + "/道具不存在");
                        continue;
                    }
                    JObject a = new JObject();
                    a.Add("key", gd.key);
                    a.Add("icon", gd.icon);
                    a.Add("num", obj["goods"]["num"]);
                    arr.Add(a);
                }
                else if ((int)obj["type"] == 3)//宠物
                {

                }
                else if ((int)obj["type"] == 4)//vip
                {

                }
            }
            return arr;
        }
    }
}
