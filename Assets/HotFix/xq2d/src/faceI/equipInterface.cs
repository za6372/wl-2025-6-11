using Assets.HotFix.MyUtils.src.common;
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

    public interface equipInterface
    {
        public JArray getZiEquip();
        public JArray getInlayEquip();
        public JArray getGoodEquip();
        public void zhaixiaInlay(string Id, int index, Action callback);
        public JArray getAllEquip();
        public JObject getFbByKey(string key);
        public JObject getAddAttrByForgingLv(JObject equip);
        public int getGoldNum();
        public int getGoldType();
        public int getGoldType(int num);
        public int getShenEquipNum();
        public int getLiangGoldNum();

        public Goods getFbSuipian(string key);
        public bool isShenEquip(string key);
        public JObject getFbHfGfMsgByKey(string key);
        public JArray getForgingEquip();
        public float getBaoshiSucRate(string key);
        public JObject getShenForgingMsg(int lv, float k);
        public JObject getForgingMsg(string key,int lv);
        public void forgingEquip(string baoshiId, string equipId, Action<bool> callback);
        public void inlayBaoshi(string baoshiId, string equipId, Action callback);
        public JArray getRepairEquip();
        public int getFixNum(int lv);
        public void fixEquip(JObject equip, Action callback);
        public void refineEquip(string Id1, string Id2, int index1, int index2, int isUser, Action callback);
        public JArray getZhuQianEquip();
        public JArray getUpGoldEquip();
        public JArray getUpLiangGoldEquip();
        public void joinPotential(int num, string equipId, Action callback);
        public void setPotentialEquip(string equipId, Action callback);
        public void cancelPotentialEquip(Action callback);
        public void getPotentialEquip(Action<string> callback);
        public void kaikong(string equipId, Action callback);
        public void upPetEquip(string Id, Action callback);
        public void downPetEquip(string partKey, Action callback);
        public void goodsToEquip(string Id, Action callback);
        public void upLj(string part, string mbId, Action callback);
        public JArray getKeYinEquip();
        public bool isKeyin(String k);
        public bool isEquip(string key);
        public bool isPetEquip(string key);
        public bool isFaBao(string key);
        public bool isHuFu(string key);
        public void reqEquipToGoods(JObject equip, string part, Action callback);
        public void ky(string Id, string pos, string part, Action callback);
        public void forgingKy(string Id, int lv, Action callback);
        public void unloadKeyin(string part, string pos, Action callback);
        public JArray getLearnSklEquip();
        public void learnEquipSkill(string skillId, string skillKey, string part, Action callback);

        public bool isTaoZhuang(string equipKey);
        public JArray getEquipByJobAndPart(string model, string part);
        public void composeForgingBaoshi(string bsKey, Action callback);
        public void bindEquip(string equipId, Action callback);
        public JArray getBindEquip(bool bdbs);
        public void unBindEquip(string equipId, Action callback);
        public JArray getKeyinEquip(bool kybs);
        public void kyEquip(string equipId, Action callback);
        public void clearInlay(string zbId, int index, Action callback);
        public JArray getJinglianFuEquip(JObject zhuZb);
        public void upGold(string equipId, Action callback);
        public void upKangxing(string equipId, Action callback);
        public JArray getJiaKangEquip();
        public JArray getChengEquip();
        public JArray getMoBan();
        public void zbChongzhu(string equipId, string mbKey, Action callback);
        public void czZhuMo(string equipId, string xhKey, Action callback);
        public void forgingChengEquip(string bsKey, string equipId, Action<bool> callback);
        public void xueqiChengEquip(string equipId, Action callback);
        public void getHf(Action callback);
        public void exchangeShengwang(string Id, Action callback);
        public void upFbLv(Action callback);
        public void xilianFb(int type, Action callback);
        public void zhulingFb(int type, string equipId, Action callback);
        public JArray getZhulingEquip();
        public bool isGoldEquip(string key);
        public int getMaxJingLianAttr(string key, string attrK);
    }

    public class equipInterfaceImpl : equipInterface
    {
        /**
     * 根据key获取最大精炼属性
     */
        public int getMaxJingLianAttr(string key, string attrK)
        {
            JObject attr = getJingLianMaxAttr(key);
            return (int)attr[attrK];
        }
        /**
     * 获取精炼的最大属性
     */
        public JObject getJingLianMaxAttr(String key)
        {
            JObject a = getJingLianBaseAttr(key);
            foreach (JProperty p in a.Properties())
            {
                string k = p.Name.ToString();
                if (countUtils.isBaseProp(k))
                {
                    a[k] = (float)a[k] * 2f;
                }
                else
                {
                    a[k] = (float)a[k] * 2.83f;
                }
            }
            return a;
        }
        /**
     * 获取精炼基础属性
     */
        public JObject getJingLianBaseAttr(string key)
        {
            Equip msg = (Equip)face.goodsInterface.getGoodsMsgByKey(key);
            //每个品质提升20%
            float k = msg.lv * (1 + msg.quality * 0.2f);
            //对于套装还要提升10%
            if (this.isTaoZhuang(key))
            {
                k = k * 1.1f;
            }
            JObject obj = new JObject();
            obj["ll"] = 10 * k / 50;//2
            obj["zl"] = 10 * k / 50;
            obj["nl"] = 10 * k / 50;
            obj["js"] = 10 * k / 50;
            obj["mj"] = 10 * k / 50;

            obj["wg"] = 20 * k / 50;//2.83
            obj["fg"] = 20 * k / 50;
            obj["wf"] = 10 * k / 50;
            obj["ff"] = 10 * k / 50;
            obj["mz"] = 25 * k / 50;
            obj["sd"] = 18 * k / 50;
            obj["bj"] = 19 * k / 50;
            obj["css"] = 20 * k / 50;

            obj["max_xue"] = 60 * k / 50;
            obj["max_lan"] = 60 * k / 50;
            return obj;
        }
        /**获取可注灵的装备*/
        public JArray getZhulingEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                //60级以上
                if ((int)g["pos"] == 0)
                {
                    Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.lv < 60) continue;
                    list.Add(g);
                }
            }
            return list;
        }
        /**法宝注灵*/
        public void zhulingFb(int type, string equipId, Action callback)
        {
            if (type == 0)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000167", 1))
                {
                    return;
                }
            }
            else
            {

            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/zhulingFb", dic, (res) =>
            {
                int exp = 0;
                if (type == 0)
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey("10000167", 1);
                    exp = 250;
                }
                else
                {
                    JObject ep = face.goodsInterface.getPlayerGoodsById(equipId);
                    Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(ep["key"].ToString());
                    exp = (int)(gd.lv * 1.5f);
                    face.goodsInterface.cutPlayerGoodsNum(equipId, 1);
                }

                JObject role = face.roleInterface.getRole();
                JObject fb = (JObject)role["attr"]["equip"]["fb"];
                fb["zhuling"]["lqz"] = (int)fb["zhuling"]["lqz"] + exp;
                if ((int)fb["zhuling"]["lqz"] >= 1000) fb["zhuling"]["lqz"] = 1000;
                fb["zhuling"]["v"] = (int)fb["zhuling"]["v"] + exp;
                if ((int)fb["zhuling"]["v"] >= 1000)
                {
                    fb["zhuling"]["v"] = (int)fb["zhuling"]["v"] - 1000;
                    fb["zhuling"]["sld"] = (int)fb["zhuling"]["sld"] + 250;
                    int lv = (int)fb["zhuling"]["lv"];
                    int maxSld = 1000 * lv + 4000;
                    if ((int)fb["zhuling"]["sld"] >= maxSld)
                    {
                        fb["zhuling"]["lv"] = (int)fb["zhuling"]["lv"] + 1;
                        fb["zhuling"]["sld"] = (int)fb["zhuling"]["sld"] - maxSld;
                        if ((int)fb["zhuling"]["quality"] == 1 && (int)fb["zhuling"]["lv"] > 5)
                        {//蓝品 5个阶
                            fb["zhuling"]["quality"] = (int)fb["zhuling"]["quality"] + 1;
                        }
                        else if ((int)fb["zhuling"]["quality"] == 2 && (int)fb["zhuling"]["lv"] > 10)
                        {//紫品 10个阶
                            fb["zhuling"]["quality"] = (int)fb["zhuling"]["quality"] + 1;
                        }
                        else if ((int)fb["zhuling"]["quality"] == 3 && (int)fb["zhuling"]["lv"] > 15)
                        {//橙品 15个阶
                            fb["zhuling"]["lv"] = (int)fb["zhuling"]["lv"] - 1;
                            fb["zhuling"]["sld"] = maxSld;
                        }
                    }
                }
                face.roleInterface.saveRole(role);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**洗练法宝*/
        public void xilianFb(int type, Action callback)
        {
            JObject role = face.roleInterface.getRole();
            JObject fb = (JObject)role["attr"]["equip"]["fb"];
            if (!face.goodsInterface.isEnoughInPackAndTip("10000166", 1))
            {
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -200))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/packageService/xilianFb", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000166", 1);
                face.roleInterface.updateMoney(-200, 1);
                fb["forging"]["type"] = type;
                face.roleInterface.saveRole(role);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**提升法宝等级*/
        public void upFbLv(Action callback)
        {
            JObject role = face.roleInterface.getRole();
            JObject fb = (JObject)role["attr"]["equip"]["fb"];
            int lv = (int)fb["forging"]["lv"];
            int[] nums = {
                3,3,3,4,5,5,6,8,9,11,
                12,13,15,16,17,19,21,23,26,29,
                32,37,42,48,54,60,64,68,73,77,
                83,90,97,104,111,119,127,135,144,155,
                166,178,191,204,217,232,247,263,279,294,
            };
            int num = nums[lv];
            //int num = ((int)fb["forging"]["lv"] + 1) * 2;
            if (!face.goodsInterface.isEnoughInPackAndTip("10000127", num))
            {
                return;
            }
            if ((int)fb["forging"]["lv"] >= 50)
            {
                msgCode.showMsg(656);
                return;
            }
            DoGet.getInstance().sendPost("/packageService/upFbLv", null, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000127", num);
                if (res.ToString().Equals("1"))
                {
                    fb["forging"]["lv"] = (int)fb["forging"]["lv"] + 1;
                    face.roleInterface.saveRole(role);
                    msgCode.showMsg(200);
                }
                else
                {
                    //失败
                    msgCode.showMsg(954);
                }
                callback();
            });
        }
        /**兑换声望*/
        public void exchangeShengwang(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/packageService/exchangeShengwang", dic, (res) =>
            {
                JObject a = face.goodsInterface.getPlayerGoodsById(Id);
                int n = int.Parse(a["key"].ToString().Substring(8));
                face.goodsInterface.cutPlayerGoodsNum(Id, 1);
                int num = 0;
                if (n < 4) num = 120;
                else if (n < 6) num = 150;
                else if (n < 8) num = 200;
                else if (n < 9) num = 250;
                face.roleInterface.upShengwang(num);
                msgCode.showMsg(952, num);
                callback();
            });
        }
        /**炼制护符*/
        public void getHf(Action callback)
        {
            DoGet.getInstance().sendPost("/packageService/getHf", null, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    //炼制失败，获得60点声望
                    JObject role = face.roleInterface.getRole();
                    JObject swdj = (JObject)role["attr"]["msg"]["swdj"];
                    //计算声望等级提升
                    face.roleInterface.upShengwang(60);
                    msgCode.showMsg(951);
                    return;
                }
                //炼制成功
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**橙装血契*/
        public void xueqiChengEquip(string equipId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000161", 1))
            {
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if ((int)equip["forging"]["lv"] != 20)
            {
                msgCode.showMsg(613);
                return;
            }
            if (!equip.ContainsKey("kybs"))
            {
                msgCode.showMsg(947);
                return;
            }
            if (equip.ContainsKey("xqbs"))
            {
                msgCode.showMsg(946);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            if (ep.quality != 3)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", equipId);
            DoGet.getInstance().sendPost("/packageService/xueqiChengEquip", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000161", 1);
                equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                equip["xqbs"] = 1;
                equip["num"] = 0;//为了不改变原道具数量所以设置为0
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(200);
                callback();
            });

        }
        /**锻造橙装*/
        public void forgingChengEquip(string bsKey, string equipId, Action<bool> callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(bsKey, 1))
            {
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if ((int)equip["forging"]["lv"] >= 20)
            {
                msgCode.showMsg(656);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            if (ep.quality != 3)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }

            equip["num"] = 0;//为了不改变原道具数量所以设置为0
            //float k = this.getBaoshiSucRate(bsKey);
            JObject msg = this.getForgingMsg(bsKey,(int)equip["forging"]["lv"] + 1);
            JObject r = face.roleInterface.getRole();
            if ((int)r["attr"]["msg"]["tale"] - (int)msg["tale"] < 0)
            {
                msgCode.showMsg(634);
                return;
            }
            bool isQing = true;
            if (bsKey.Equals("10000159")) isQing = false;
            else if (bsKey.Equals("10000160")) isQing = true;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("bsKey", bsKey);
            dic.Add("Id", equipId);
            DoGet.getInstance().sendPost("/packageService/forgingChengEquip", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {//锻造失败
                 //缓存银两
                    face.roleInterface.updateMoney(-(int)msg["tale"], 1);
                    //减少宝石数量
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 1);

                    if (isQing)
                    {
                        equip["forging"]["lv"] = (int)equip["forging"]["lv"] - 1;
                    }
                    else
                    {
                        //装备设置损毁
                        equip["isBad"] = 1;
                    }

                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(654);
                }
                else if (res.ToString().Equals("1"))
                {//锻造成功
                 //缓存银两
                    face.roleInterface.updateMoney(-(int)msg["tale"], 1);
                    //减少宝石数量
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 1);

                    //缓存装备
                    equip["forging"]["lv"] = (int)equip["forging"]["lv"] + 1;
                    if ((int)equip["forging"]["lv"] == 3 || (int)equip["forging"]["lv"] == 6 ||
                    (int)equip["forging"]["lv"] == 9 || (int)equip["forging"]["lv"] == 12 ||
                    (int)equip["forging"]["lv"] == 15 || (int)equip["forging"]["lv"] == 18)
                    {
                        int maxNum = 7;
                        if (isKyEquip(equip))
                        {
                            maxNum = 8;
                        }
                        if ((int)equip["inlay"]["num"] < maxNum)
                        {
                            equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                        }
                    }
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(653);
                }

                //刷新
                callback(res.ToString().Equals("0") ? false : true);
            });
        }
        /**橙装注魔*/
        public void czZhuMo(string equipId, string xhKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, 1))
            {
                return;
            }
            //装备是否满足等级、品质、是否已经有模板
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if (!equip.ContainsKey("czmb"))
            {
                msgCode.showMsg(842);
                return;
            }
            JObject a = (JObject)equip["czmb"];
            MoBan mb = (MoBan)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
            if ((int)a["num"] >= mb.getMaxMoLi())
            {
                msgCode.showMsg(945);
                return;
            }

            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            if (ep.quality != 3)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", equipId);
            dic.Add("xhKey", xhKey);
            DoGet.getInstance().sendPost("/packageService/czZhuMo", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 1);
                int n = 10;
                if (xhKey.Equals("10000157")) n = 20;
                else if (xhKey.Equals("10000158")) n = 30;

                a["num"] = (int)a["num"] + n;
                //超过限制时处理
                if ((int)a["num"] > mb.getMaxMoLi())
                {
                    a["num"] = mb.getMaxMoLi();
                }

                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(200);
                //刷新
                callback();
            });

        }
        /**重铸橙装*/
        public void zbChongzhu(string equipId, string mbKey, Action callback)
        {
            //装备是否满足等级、品质
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            if (ep.quality != 3)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }
            //判断职业、部位是否一致
            MoBan mb = (MoBan)face.goodsInterface.getGoodsMsgByKey(mbKey);
            if (!mb.isAllowEquip(equip["key"].ToString()))
            {
                msgCode.showMsg(944);
                return;
            }
            if (!face.goodsInterface.isEnoughInPackAndTip(mbKey, 1))
            {
                return;
            }
            equip["num"] = 0;//为了不改变原道具数量所以设置为0

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", equipId);
            dic.Add("mbKey", mbKey);
            DoGet.getInstance().sendPost("/packageService/zbChongzhu", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(mbKey, 1);

                JObject a = new JObject();
                a.Add("key", mbKey);
                a.Add("num", 0);
                equip["czmb"] = a;
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(200);
                //刷新
                callback();
            });
        }
        /**抗性加护*/
        public void upKangxing(string equipId, Action callback)
        {
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if ((int)equip["forging"]["lv"] < 10)
            {
                msgCode.showMsg(941);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            if (ep.part.Equals("wq"))
            {
                msgCode.showMsg(943);
                return;
            }
            if (ep.quality < 2)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }
            string kxKey = null;
            if (ep.part.Equals("sz") || ep.part.Equals("jiaob")) kxKey = "bjlv";
            else if (ep.part.Equals("wb") || ep.part.Equals("tuib")) kxKey = "hllv";
            else if (ep.part.Equals("jb") || ep.part.Equals("xb")) kxKey = "lxlv";
            else if (ep.part.Equals("tb") || ep.part.Equals("yb")) kxKey = "hslv";
            String xhKey = null;
            if (kxKey.Equals("bjlv")) xhKey = "10000152";
            else if (kxKey.Equals("hllv")) xhKey = "10000153";
            else if (kxKey.Equals("lxlv")) xhKey = "10000154";
            else if (kxKey.Equals("hslv")) xhKey = "10000155";


            int lv = equip.ContainsKey("kxlv") ? (int)equip["kxlv"][kxKey] + 1 : 1;
            int kx = 50 * lv + (int)Math.Pow(2d, lv);
            int num = 6 * lv + (int)Math.Pow(1.5d, lv);
            int tale = 2000 + (lv - 1) * 20000 + (int)Math.Pow(3d, lv);

            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, num))
            {
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -tale))
            {
                msgCode.showMsg(634);
                return;
            }


            equip["num"] = 0;//为了不改变原道具数量所以设置为0

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", equipId);
            DoGet.getInstance().sendPost("/packageService/upKangxing", dic, (res) =>
            {
                face.roleInterface.updateMoney(-tale, 1);
                //减少千年玄铁数量
                face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, num);

                //缓存装备
                if (!equip.ContainsKey("kxlv"))
                {
                    JObject a = new JObject();
                    a.Add("bjlv", 0);
                    a.Add("hllv", 0);
                    a.Add("lxlv", 0);
                    a.Add("hslv", 0);
                    equip["kxlv"] = a;
                }
                equip["kxlv"][kxKey] = int.Parse(res.ToString());
                equip["isBind"] = 1;
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(200);
                //刷新
                callback();
            });
        }
        /**升橙*/
        public void upGold(string equipId, Action callback)
        {
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if ((int)equip["forging"]["lv"] < 10)
            {
                msgCode.showMsg(941);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            string part = ep.part;
            int xhNum = 0;
            if (part.Equals("wq")) xhNum = 9;
            else if (part.Equals("jb")) xhNum = 6;
            else if (part.Equals("sz")) xhNum = 2;
            else if (part.Equals("wb")) xhNum = 4;
            else if (part.Equals("tb")) xhNum = 4;
            else if (part.Equals("xb")) xhNum = 6;
            else if (part.Equals("yb")) xhNum = 3;
            else if (part.Equals("tuib")) xhNum = 3;
            else if (part.Equals("jiaob")) xhNum = 2;
            int tale = 50000 * xhNum;
            if (ep.quality != 2)
            {
                msgCode.showMsg(940);
                return;
            }
            if (ep.lv < 91)
            {
                msgCode.showMsg(942);
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -tale))
            {
                msgCode.showMsg(634);
                return;
            }
            if (!face.goodsInterface.isEnoughInPackAndTip("10000151", xhNum))
            {
                return;
            }

            equip["num"] = 0;//为了不改变原道具数量所以设置为0

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", equipId);
            DoGet.getInstance().sendPost("/packageService/upGold", dic, (res) =>
            {
                face.roleInterface.updateMoney(-tale, 1);
                //减少千年玄铁数量
                face.goodsInterface.cutPlayerGoodsNumByKey("10000151", xhNum);

                //缓存装备
                equip["forging"]["lv"] = (int)equip["forging"]["lv"] + 1;
                equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                if ((int)equip["forging"]["lv"] == 12 || (int)equip["forging"]["lv"] == 15)
                {
                    //未刻印的装备只能达到6，刻印过的装备可以达到7
                    int maxNum = 6;
                    if (isKyEquip(equip))
                    {
                        maxNum = 7;
                    }
                    if ((int)equip["inlay"]["num"] < maxNum)
                    {
                        equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                    }
                }
                equip["isBind"] = 1;
                equip["key"] = res.ToString();
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(200);
                //刷新
                callback();
            });
        }
        /**开启装备刻印*/
        public void kyEquip(string equipId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000150", 1))
            {
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if (isKyEquip(equip))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/kyEquip", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000150", 1);

                equip["isBind"] = 1;
                equip["kybs"] = 1;
                //镶嵌数+1
                equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);

                msgCode.showMsg(200);
                callback();
            });
        }
        private bool isKyEquip(JObject equip)
        {
            if (equip.ContainsKey("kybs") && (int)equip["kybs"] == 1) return true;
            return false;
        }
        private bool isBindEquip(JObject equip)
        {
            if (equip.ContainsKey("bdbs") && (int)equip["bdbs"] == 1) return true;
            return false;
        }
        /**解绑装备*/
        public void unBindEquip(string equipId, Action callback)
        {
            if (!face.roleInterface.isEnoughMoney("tale", -5000))
            {
                msgCode.showMsg(634);
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if (!isBindEquip(equip))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/unBindEquip", dic, (res) =>
            {
                face.roleInterface.updateMoney(-5000, 1);
                equip["isBind"] = 0;
                equip["bdbs"] = 0;
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);

                msgCode.showMsg(200);
                callback();
            });
        }
        /**绑定装备*/
        public void bindEquip(string equipId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000149", 1))
            {
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if (isBindEquip(equip))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/bindEquip", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000149", 1);

                equip["isBind"] = 1;
                equip["bdbs"] = 1;
                equip["num"] = 0;
                face.goodsInterface.savePlayerGoods(equip);

                msgCode.showMsg(200);
                callback();
            });
        }
        /**合成锻造宝石*/
        public void composeForgingBaoshi(string bsKey, Action callback)
        {
            String xhKey = null;
            int tale = 0;
            int needNum = 0;
            if (bsKey.Equals("10000108"))
            {
                xhKey = "10000148";//圣锻需要消耗圣锻碎片
                tale = 2000;
                needNum = 20;
            }
            else
            {
                xhKey = int.Parse(bsKey) - 1 + "";
                tale = (int.Parse(bsKey) - 10000105) * 1000 + 1000;
                needNum = 5;
            }
            if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, needNum))
            {
                return;
            }
            if (!face.roleInterface.isEnoughMoney("tale", -tale))
            {
                msgCode.showMsg(634);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", bsKey);
            DoGet.getInstance().sendPost("/packageService/composeForgingBaoshi", dic, (res) =>
            {
                face.roleInterface.updateMoney(-tale, 1);
                if (res.ToString().Equals("0"))
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 3);
                    msgCode.showMsg(659);
                }
                else
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, needNum);
                    face.rewardInterface.saveRewards((JArray)res);
                    msgCode.showMsg(658);
                }

                callback();
            });
        }
        /**学习装备技能 */
        public void learnEquipSkill(string Id, string skillKey, string part, Action callback)
        {
            //判断是否为亮金或者神装
            JObject r = face.roleInterface.getRole();
            object pa = r["attr"]["equip"][part];
            if (pa.ToString() == "")
            {
                return;
            }
            JObject equip = (JObject)pa;
            if (!"1".Equals(equip["isLj"].ToString()) && !this.isShenEquip(equip["key"].ToString()))
            {
                msgCode.showMsg(846);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("skillKey", skillKey);
            dic.Add("part", part);
            DoGet.getInstance().sendPost("/packageService/learnEquipSkill", dic, (res) =>
            {
                equip["skill"] = (JArray)res;
                face.roleInterface.saveRole(r);
                face.goodsInterface.cutPlayerGoodsNum(Id, 1);
                msgCode.showMsg(636);
                callback();
            });
        }
        /**卸下刻印*/
        public void unloadKeyin(string part, string pos, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if (r["attr"]["equip"][part].ToString() == "")
            {
                return;
            }
            JObject keyin = (JObject)r["attr"]["equip"][part]["keyin"];
            if (keyin[pos] == null || keyin[pos].ToString().Equals(""))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pos", pos);
            dic.Add("part", part);
            DoGet.getInstance().sendPost("/packageService/unloadKeyin", dic, (res) =>
            {
                keyin[pos] = "";
                face.roleInterface.saveRole(r);
                if (((JObject)res)["oldKy"] != null)
                    face.goodsInterface.savePlayerGoods((JObject)((JObject)res)["oldKy"]);
                callback();
            });
        }
        /**增幅刻印 */
        public void forgingKy(string Id, int lv, Action callback)
        {
            if (lv < 50)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("109500", (lv + 1) * 5))
                {
                    return;
                }
                if (!face.goodsInterface.isEnoughInPackAndTip("109501", (lv + 1) * 2))
                {
                    return;
                }

            }
            else
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("109501", (lv + 1) * 5))
                {
                    return;
                }
                if (!face.goodsInterface.isEnoughInPackAndTip("109502", (lv + 1) * 2))
                {
                    return;
                }

            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/packageService/forgingKy", dic, (res) =>
            {
                if (lv < 50)
                {
                    JObject a = face.goodsInterface.getPlayerGoodsByKey("109500");
                    face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), (lv + 1) * 5);
                    a = face.goodsInterface.getPlayerGoodsByKey("109501");
                    face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), (lv + 1) * 2);
                }
                else
                {
                    JObject a = face.goodsInterface.getPlayerGoodsByKey("109501");
                    face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), (lv + 1) * 5);
                    a = face.goodsInterface.getPlayerGoodsByKey("109502");
                    face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), (lv + 1) * 2);
                }
                JObject g = face.goodsInterface.getPlayerGoodsById(Id);
                if (g["forging"] == null)
                {
                    JObject f = new JObject();
                    f.Add("lv", 0);
                    g["forging"] = f;

                }

                g["forging"]["lv"] = lv + (res.ToString().Equals("1") ? 1 : -1);
                if ((int)g["forging"]["lv"] < 0) g["forging"]["lv"] = 0;
                g["num"] = 0;
                face.goodsInterface.savePlayerGoods(g);
                msgCode.showMsg(res.ToString().Equals("1") ? 847 : 848);
                callback();
            });
        }
        /**刻印 */
        public void ky(string Id, string pos, string part, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if (r["attr"]["equip"][part].ToString() == "")
            {
                return;
            }
            JObject equip = (JObject)r["attr"]["equip"][part];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("pos", pos);
            dic.Add("part", part);
            DoGet.getInstance().sendPost("/packageService/keyin", dic, (res) =>
            {
                if (res != null)
                {
                    JObject g = face.goodsInterface.getPlayerGoodsById(Id);
                    if (equip["keyin"] == null)
                    {
                        JObject a = new JObject();
                        a.Add("a", null);
                        a.Add("b", null);
                        a.Add("c", null);
                        a.Add("d", null);
                        a.Add("e", null);
                        equip["keyin"] = a;
                    }
                    JObject b = new JObject();
                    b.Add("key", g["key"].ToString());
                    b.Add("randomAttr", g["randomAttr"]);
                    b.Add("forging", g["forging"]);
                    equip["keyin"][pos] = b;
                    face.roleInterface.saveRole(r);

                    face.goodsInterface.cutPlayerGoodsNum(Id, 1);
                    if (((JObject)res)["oldKy"] != null)
                        face.goodsInterface.savePlayerGoods((JObject)((JObject)res)["oldKy"]);
                    msgCode.showMsg(636);
                    callback();
                }
            });
        }
        public bool isKeyin(String k)
        {
            if (k.Substring(0, 4).Equals("1009"))
            {
                return true;
            }
            return false;
        }

        /**请求卸载装备 */
        public void reqEquipToGoods(JObject equip, string part, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("part", part);
            DoGet.getInstance().sendPost("/packageService/equipToGoods", dic, (res) =>
            {
                equip["Id"] = res.ToString();
                equip["num"] = 1;
                //装备部位置空
                this.equipToGoods(equip, part);
                callback();
            });
        }
        
        /**上宠物装备*/
        public void upPetEquip(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/packageService/upPetEquip", dic, (res) =>
            {
                JObject a = (JObject)res;
                JObject equip = (JObject)a["equip"];

                face.goodsInterface.cutPlayerGoodsNum(Id, 1);
                JObject role= face.roleInterface.getRole();
                role["attr"]["petEquip"] = equip;
                face.roleInterface.saveRole(role);
                if (a.ContainsKey("old"))
                {
                    face.goodsInterface.savePlayerGoods((JObject)a["old"]);
                }
                msgCode.showMsg(200);
                callback();
            });
        }
        /**卸下宠物装备*/
        public void downPetEquip(string partKey, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("partKey", partKey);
            DoGet.getInstance().sendPost("/packageService/downPetEquip", dic, (res) =>
            {
                JObject a = (JObject)res;
                JObject equip = (JObject)a["equip"];
                //将道具放入背包
                JObject role = face.roleInterface.getRole();
                role["attr"]["petEquip"] = equip;
                face.roleInterface.saveRole(role);
                if (a.ContainsKey("old"))
                {
                    face.goodsInterface.savePlayerGoods((JObject)a["old"]);
                }
                msgCode.showMsg(200);
                callback();
            });
        }
        /**物品转装备 */
        public void goodsToEquip(string Id, Action callback)
        {
            JObject goods = face.goodsInterface.getPlayerGoodsById(Id);
            Equip equip = (Equip)face.goodsInterface.getGoodsMsgByKey(goods["key"].ToString());
            if (equip == null)
            {
                return;
            }
            JObject r = face.roleInterface.getRole();
            if ((int)r["lever"] < equip.lv)
            {
                msgCode.showMsg(613);
                return;
            }
            string model = GameAttrConst.getModel(r);

            if (!equip.job.ToString().Equals("*") && (model == null ||
                !equip.job.ToString().Contains(model.Substring(0, 2))))
            {
                msgCode.showMsg(684);
                return;
            }
            string part = equip.part.ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/packageService/goodsToEquip", dic, (res) =>
            {
                if (res != null && res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(651);
                    return;
                }
                if (res != null)
                {
                    //res 返回卸下的装备id
                    JObject map = this.getEquipList();

                    //根据装备部位判断是否需要卸下原装备
                    if (!strUtils.isNull(map[part].ToString()))
                    {
                        //先卸载后装备
                        map[part]["Id"] = res.ToString();
                        map[part]["num"] = 1;
                        //装备部位置空
                        this.equipToGoods((JObject)map[part], part);
                    }
                }
                //装备部位换上新装备
                face.roleInterface.saveRoleAttrEquip(goods, part);
                //缓存
                face.goodsInterface.cutPlayerGoodsNum(Id, 1);
                this.addStatus();
                //通知刷新人物模型（金装效果）
                JObject j = new JObject();
                j.Add("callback", "793");
                j.Add("msg", 1);
                eventsUtils.dispatchEvent("ws", j);
                callback();
            });
        }
        /**补状态(装备时调用一次，每次战斗结束调用一次) */
        private void addStatus()
        {
            JObject r = face.roleInterface.getRole();
            if (r["attr"]["equip"]["bjb"].ToString().Equals("") || (int)r["attr"]["equip"]["bjb"]["capacity"]["num"] <= 0)
            {
                return;
            }
            JObject base0 = countUtils.countProp(r);
            if ((int)r["attr"]["prop"]["xue"] < (int)base0["max_xue"])
            {
                int cut = (int)base0["max_xue"] - (int)r["attr"]["prop"]["xue"];
                if ((int)r["attr"]["equip"]["bjb"]["capacity"]["num"] >= cut)
                {
                    //补满
                    r["attr"]["prop"]["xue"] = (int)r["attr"]["prop"]["xue"] + cut;
                    r["attr"]["equip"]["bjb"]["capacity"]["num"] = (int)r["attr"]["equip"]["bjb"]["capacity"]["num"] - cut;
                }
                else
                {
                    r["attr"]["prop"]["xue"] = (int)r["attr"]["prop"]["xue"] + (int)r["attr"]["equip"]["bjb"]["capacity"]["num"];
                    r["attr"]["equip"]["bjb"]["capacity"]["num"] = 0;
                }
            }
            if ((int)r["attr"]["prop"]["lan"] < (int)base0["max_lan"])
            {
                int cut = (int)base0["max_lan"] - (int)r["attr"]["prop"]["lan"];
                if ((int)r["attr"]["equip"]["bjb"]["capacity"]["num"] >= cut)
                {
                    //补满
                    r["attr"]["prop"]["lan"] = (int)r["attr"]["prop"]["lan"] + cut;
                    r["attr"]["equip"]["bjb"]["capacity"]["num"] = (int)r["attr"]["equip"]["bjb"]["capacity"]["num"] - cut;
                }
                else
                {
                    r["attr"]["prop"]["lan"] = (int)r["attr"]["prop"]["lan"] + (int)r["attr"]["equip"]["bjb"]["capacity"]["num"];
                    r["attr"]["equip"]["bjb"]["capacity"]["num"] = 0;
                }
            }

            JObject pet = face.petInterface.getIsFightPet();
            if (pet != null)
            {
                pet["xrmf"] = face.roleInterface.getXrmf();
                pet["petEquip"] = face.roleInterface.getPetEquip();
                base0 = countUtils.countPetProp(pet);
                //给宠物回血
                if ((int)pet["attr"]["prop"]["xue"] < (int)base0["max_xue"])
                {
                    int cut = (int)base0["max_xue"] - (int)pet["attr"]["prop"]["xue"];
                    if ((int)r["attr"]["equip"]["bjb"]["capacity"]["num"] >= cut)
                    {
                        //补满
                        pet["attr"]["prop"]["xue"] = (int)pet["attr"]["prop"]["xue"] + cut;
                        r["attr"]["equip"]["bjb"]["capacity"]["num"] = (int)r["attr"]["equip"]["bjb"]["capacity"]["num"] - cut;
                    }
                    else
                    {
                        pet["attr"]["prop"]["xue"] = (int)pet["attr"]["prop"]["xue"] + (int)r["attr"]["equip"]["bjb"]["capacity"]["num"];
                        r["attr"]["equip"]["bjb"]["capacity"]["num"] = 0;
                    }
                }
                if ((int)pet["attr"]["prop"]["lan"] < (int)base0["max_lan"])
                {
                    int cut = (int)base0["max_lan"] - (int)pet["attr"]["prop"]["lan"];
                    if ((int)r["attr"]["equip"]["bjb"]["capacity"]["num"] >= cut)
                    {
                        //补满
                        pet["attr"]["prop"]["lan"] = (int)pet["attr"]["prop"]["lan"] + cut;
                        r["attr"]["equip"]["bjb"]["capacity"]["num"] = (int)r["attr"]["equip"]["bjb"]["capacity"]["num"] - cut;
                    }
                    else
                    {
                        pet["attr"]["prop"]["lan"] = (int)pet["attr"]["prop"]["lan"] + (int)r["attr"]["equip"]["bjb"]["capacity"]["num"];
                        r["attr"]["equip"]["bjb"]["capacity"]["num"] = 0;
                    }
                }
            }
            //缓存
            face.roleInterface.saveRole(r);

            //提交宠物属性
            if (pet != null)
            {
                JObject petA = new JObject();
                petA.Add("lan", pet["attr"]["prop"]["lan"]);
                petA.Add("xue", pet["attr"]["prop"]["xue"]);
                face.petInterface.updatePetXueById(petA, pet["Id"].ToString());
            }

            //缓存装备
            face.roleInterface.saveRoleAttrEquip(r["attr"]["equip"]["bjb"], "bjb");
        }
        /**缓存卸载装备 */
        private void equipToGoods(JObject equip, string part)
        {
            JObject map = this.getEquipList();
            map[part] = "";
            //人物装备部位上的移除
            face.roleInterface.saveRoleAttrEquip("", part);
            //将装备放入背包
            face.goodsInterface.equipToPackage(equip);
            //通知刷新人物模型（金装效果）
            JObject j = new JObject();
            j.Add("callback", "793");
            j.Add("msg", 1);
            eventsUtils.dispatchEvent("ws", j);
        }
        /**获取装备列表 */
        public JObject getEquipList()
        {
            return (JObject)(face.roleInterface.getRole()["attr"]["equip"]);
        }
        /**升亮金*/
        public void upLj(string part, string mbId, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("part", part);
            DoGet.getInstance().sendPost("/packageService/upLiangGold", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    face.goodsInterface.cutPlayerGoodsNum(mbId, 1);
                    r["attr"]["equip"][part]["isLj"] = 1;
                    face.roleInterface.saveRole(r);
                    msgCode.showMsg(840);
                    callback();
                }
                else
                {
                    msgCode.showMsg(779);
                }
            });
        }

        public void kaikong(string equipId, Action callback)
        {
            //判断是否有开孔器
            if (!face.goodsInterface.isEnoughInPackAndTip("10000115", 1))
            {
                return;
            }
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if (equip.ContainsKey("inlay") && (int)equip["inlay"]["num"] >= 5)
            {
                msgCode.showMsg(935);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/kaikong", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    JObject g = face.goodsInterface.getPlayerGoodsByKey("10000115");
                    face.goodsInterface.cutPlayerGoodsNum(g["Id"].ToString(), 1);

                    equip["num"] = 0;
                    equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(678);
                    callback();
                }
                else
                {
                    msgCode.showMsg(651);
                }
            });

        }
        /**获取正在练潜的装备 */
        public void getPotentialEquip(Action<string> callback)
        {
            DoGet.getInstance().sendPost("/packageService/getPotentialEquip", null, (res) =>
            {
                callback(res == null ? null : res.ToString());
            });
        }
        /**取消练潜装备 */
        public void cancelPotentialEquip(Action callback)
        {
            DoGet.getInstance().sendPost("/packageService/canclePotentialEquip", null, (res) =>
            {
                msgCode.showMsg(665);
                callback();
            });
        }
        /**设置练潜的装备 */
        public void setPotentialEquip(string equipId, Action callback)
        {
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            JArray list = (JArray)equip["inlay"]["list"];
            //没有镶嵌石头的装备不允许
            if ((int)equip["inlay"]["num"] <= 0 || list.Count <= 0)
            {
                msgCode.showMsg(788);
                return;
            }
            //判断潜力是否已经开发完
            int i = 0;
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                Baoshi msg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                if ((int)msg.prop <= (int)a["num"])
                {
                    i++;
                }
            }
            if (i == list.Count)
            {
                msgCode.showMsg(789);
                return;
            }
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(790);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/setPotentialEquip", dic, (res) =>
            {
                msgCode.showMsg(664);
                callback();
            });
        }
        /**注入潜力 */
        public void joinPotential(int num, string equipId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000114", num))
            {
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("num", num);
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/joinPotential", dic, (res) =>
            {
                JObject g = face.goodsInterface.getPlayerGoodsByKey("10000114");
                face.goodsInterface.cutPlayerGoodsNumByKey(g["key"].ToString(), num);
                JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
                equip["num"] = 0;
                equip["potential"]["max"] = (int)equip["potential"]["max"] + num * 100;
                equip["potential"]["num"] = (int)equip["potential"]["num"] + num * 100;
                face.goodsInterface.savePlayerGoods(equip);
                msgCode.showMsg(662);
                callback();
            });

        }
        /**精炼*/
        public void refineEquip(string Id1, string Id2, int index1, int index2, int isUser, Action callback)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id1", Id1);
            dic.Add("Id2", Id2);
            dic.Add("index1", index1);
            dic.Add("index2", index2);
            dic.Add("isUser", isUser);
            DoGet.getInstance().sendPost("/packageService/jinglian", dic, (res) =>
            {
                if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(827);
                    return;
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(828);
                    return;
                }
                else if (res.ToString().Equals("-3"))
                {
                    msgCode.showMsg(829);
                    return;
                }

                if (isUser == 1)
                {
                    JObject obj = face.goodsInterface.getPlayerGoodsByKey("10000109");
                    face.goodsInterface.cutPlayerGoodsNum(obj["Id"].ToString(), 1);
                }
                face.goodsInterface.cutPlayerGoodsNum(Id2, 1);
                JObject zhuZb = face.goodsInterface.getPlayerGoodsById(Id1);
                zhuZb["randomAttr"] = (JArray)res;
                zhuZb["num"] = 0;
                face.goodsInterface.savePlayerGoods(zhuZb);
                msgCode.showMsg(739);

                callback();
            });
        }
        /**修复装备 */
        public void fixEquip(JObject equip, Action callback)
        {
            int num = this.getFixNum((int)equip["forging"]["lv"]);
            //获取宝石数量
            if (!face.goodsInterface.isEnoughInPackAndTip("10000111", num))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("equipId", equip["Id"]);
            DoGet.getInstance().sendPost("/packageService/fixEquip", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    //减少宝石数量
                    face.goodsInterface.cutPlayerGoodsNumByKey("10000111", num);
                    //装备设置损毁
                    equip["isBad"] = 0;
                    equip["num"] = 0;
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(657);
                    //刷新
                    callback();
                }
                else
                {
                    msgCode.showMsg(651);
                }
            });
        }
        /**跟据损坏装备的锻造等级来获取所需修复宝石数量 */
        public int getFixNum(int lv)
        {
            int num = (int)(Math.Pow(1.3f, lv));
            if (lv >= 15)
            {
                int[] nums = { 42, 56, 74, 92, 110 };
                num = nums[lv - 15];
            }
            return num;
        }
        /**镶嵌宝石*/
        public void inlayBaoshi(string baoshiId, string equipId, Action callback)
        {
            //判断是否有空孔，以及是否损坏
            JObject baoshi = face.goodsInterface.getPlayerGoodsById(baoshiId);
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }

            if (!isFaBao(equip["key"].ToString()))
            {
                if ((int)equip["inlay"]["num"] <= 0 || ((JArray)equip["inlay"]["list"]).Count >= (int)equip["inlay"]["num"])
                {
                    msgCode.showMsg(660);
                    return;
                }
                //装备等级是否达到
                Baoshi msg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(baoshi["key"].ToString());
                int neetLv = face.baoshiInterface.getInlayLv(msg.lv);
                Equip e = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
                if ((int)e.lv < neetLv)
                {
                    msgCode.showMsg(613);
                    return;
                }
            }
            else
            {
                //对于法宝要验证品质是否达到
                if (!equip.ContainsKey("zhuling"))
                {
                    msgCode.showMsg(660);
                    return;
                }
                JObject zhuling = (JObject)equip["zhuling"];
                int quality = (int)zhuling["quality"];
                if (quality < 2)
                {
                    msgCode.showMsg(660);
                    return;
                }
            }

            equip["num"] = 0;//为了不改变原道具数量所以设置为0
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("baoshiId", baoshiId);
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/inlayBaoshi", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    face.goodsInterface.cutPlayerGoodsNum(baoshiId, 1);
                    if (!equip.ContainsKey("inlay"))
                    {
                        JObject zhuling = (JObject)equip["zhuling"];
                        int quality = (int)zhuling["quality"];
                        int num = 0;
                        if (quality == 2) num = 1;
                        else if (quality == 3) num = 2;
                        JObject a = new JObject();
                        a["num"] = num;
                        a["list"] = new JArray();
                        equip["inlay"] = a;
                    }
                    JObject obj = new JObject();
                    obj.Add("key", baoshi["key"].ToString());
                    obj.Add("num", 0);
                    if (isFaBao(equip["key"].ToString()))
                    {
                        Baoshi msg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(baoshi["key"].ToString());
                        obj["num"] = msg.prop;
                    }
                    ((JArray)equip["inlay"]["list"]).Add(obj);
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(661);
                    callback();
                }
                else
                {
                    msgCode.showMsg(651);
                }
            });
        }
        /**摘下镶嵌石（返还宝石）*/
        public void zhaixiaInlay(string Id, int index, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/packageService/zhaixiaInlay", dic, (res) =>
            {
                JObject equip = face.goodsInterface.getPlayerGoodsById(Id);
                //缓存装备
                JArray list = (JArray)equip["inlay"]["list"];
                equip["num"] = 0;
                list.RemoveAt(index);
                face.goodsInterface.savePlayerGoods(equip);
                face.rewardInterface.saveRewards((JArray)res);
                callback();
            });
        }
        /**清理镶嵌石*/
        public void clearInlay(string zbId, int index, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", zbId);
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/packageService/clearInlay", dic, (res) =>
            {
                if (!res.ToString().Equals("0"))
                {
                    JObject equip = face.goodsInterface.getPlayerGoodsById(zbId);
                    //缓存装备
                    JArray list = (JArray)equip["inlay"]["list"];
                    equip["num"] = 0;
                    list.RemoveAt(index);
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(200);
                    callback();
                }
            });
        }
        /**请求锻造
         * 消耗的宝石id跟需要锻造的装备id
         */
        public void forgingEquip(string bsKey, string equipId, Action<bool> callback)
        {
            JObject equip = face.goodsInterface.getPlayerGoodsById(equipId);
            if ((int)equip["isBad"] == 1)
            {
                msgCode.showMsg(655);
                return;
            }
            if ((int)equip["forging"]["lv"] >= 15)
            {
                msgCode.showMsg(656);
                return;
            }
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            /*if (ep.quality > 2)
            {
                msgCode.showMsg(940);
                return;
            }*/

            equip["num"] = 0;//为了不改变原道具数量所以设置为0
            //float k = this.getBaoshiSucRate(bsKey);
            JObject msg = this.getForgingMsg(bsKey,(int)equip["forging"]["lv"] + 1);
            JObject r = face.roleInterface.getRole();
            if ((int)r["attr"]["msg"]["tale"] - (int)msg["tale"] < 0)
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("bsKey", bsKey);
            dic.Add("equipId", equipId);
            DoGet.getInstance().sendPost("/packageService/forgingEquip", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {//锻造失败
                 //缓存银两
                    face.roleInterface.updateMoney(-(int)msg["tale"], 1);
                    //减少宝石数量
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 1);

                    //装备设置损毁
                    equip["isBad"] = 1;
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(654);
                }
                else if (res.ToString().Equals("1"))
                {//锻造成功
                 //缓存银两
                    face.roleInterface.updateMoney(-(int)msg["tale"], 1);
                    //减少宝石数量
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 1);

                    //缓存装备
                    equip["forging"]["lv"] = (int)equip["forging"]["lv"] + 1;
                    if ((int)equip["forging"]["lv"] == 3 || (int)equip["forging"]["lv"] == 6 ||
                    (int)equip["forging"]["lv"] == 9 || (int)equip["forging"]["lv"] == 12 ||
                    (int)equip["forging"]["lv"] == 15)
                    {
                        int maxNum = 5;
                        if (isKyEquip(equip))
                        {
                            maxNum = 6;
                        }
                        if ((int)equip["inlay"]["num"] < maxNum)
                        {
                            equip["inlay"]["num"] = (int)equip["inlay"]["num"] + 1;
                        }
                    }
                    face.goodsInterface.savePlayerGoods(equip);
                    msgCode.showMsg(653);
                }
                else
                {//参数异常
                    msgCode.showMsg(651);
                    return;
                }
                //刷新
                callback(res.ToString().Equals("0") ? false : true);
            });
        }
        /**锻造所需消耗计算
             * lv为需要提升为的锻造等级
             * k附加的成功率
             */
        public JObject getForgingMsg(string baoshiKey, int forgingLv)
        {
            float k = 0f;
            bool isQing = false;
            bool isJin = false;
            if (baoshiKey.Equals("10000104")) k = 0.1f;
            else if (baoshiKey.Equals("10000105")) k = 0.2f;
            else if (baoshiKey.Equals("10000106")) k = 0.3f;
            else if (baoshiKey.Equals("10000107")) k = 0.4f;
            else if (baoshiKey.Equals("10000108")) k = 1f;
            //橙装
            else if (baoshiKey.Equals("10000159"))
            {
                k = 0.5f;
                isJin = true;
            }
            else if (baoshiKey.Equals("10000160"))
            {
                k = 0.5f;
                isQing = true;
            }
            else k = 0f;
            double p = 0d;
            if (k == 1f)
            {
                p = 1d;
            }
            else
            {
                p = Math.Pow(0.9, forgingLv) * (1 + k) + 0.11f;
            }
            if (isJin)
            {
                p = 0.41d;
            }
            if (isQing)
            {
                p = 0.8d - 0.1d * (forgingLv - 15);
            }
            if (p > 1) p = 1d;
            int tale = forgingLv * 1000;
            JObject j = new JObject();
            j.Add("tale", tale);
            j.Add("p", p);
            return j;
        }
        /*public JObject getForgingMsg(int lv, float k)
        {
            //锻造等级越高成功率越低 
            float p;
            if (k == 1)
            {
                p = 1f;//圣锻皇必然成功
            }
            else
            {
                p = (float)Math.Pow(0.9f, lv) * (1 + k) + 0.11f;
            }
            if (p > 1) p = 1;
            int tale = lv * 1000;
            JObject res = new JObject();
            res.Add("p", p);
            res.Add("tale", tale);
            return res;
        }*/
        /**神装锻造概率 */
        public JObject getShenForgingMsg(int lv, float k)
        {
            //锻造等级越高成功率越低 
            float p = (float)Math.Pow(0.9f, lv) * (1 + k);
            p = p - 0.5f;
            if (p > 1)
            {
                p = 1;
            }
            else if (p < 0)
            {
                if (k == 1) p = 0.05f;
                else p = 0;
            }
            if (p > 1) p = 1;
            int tale = lv * 1000 * 2;
            JObject res = new JObject();
            res.Add("p", p);
            res.Add("tale", tale);
            return res;
        }
        /**获取锻造宝石的成功率*/
        public float getBaoshiSucRate(string key)
        {
            float k = 0.1f;
            if (key.Equals("10000104")) k = 0.1f;
            else if (key.Equals("10000105")) k = 0.2f;
            else if (key.Equals("10000106")) k = 0.3f;
            else if (key.Equals("10000107")) k = 0.4f;
            else if (key.Equals("10000108")) k = 1f;
            //橙装
            else if (key.Equals("10000159")) k = 0.5f;
            else if (key.Equals("10000160")) k = 0.5f;
            return k;
        }
        /**获取可修复的装备*/
        public JArray getRepairEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;
                Equip obj = (Equip)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                if ((int)g["pos"] == 0 && obj.type == 1 && (int)g["isBad"] == 1 &&
                    !obj.part.ToString().Equals("bjb") && !obj.part.ToString().Equals("fb") &&
                    !obj.part.ToString().Equals("gf") && !obj.part.ToString().Equals("hf"))
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取未/已刻印的装备*/
        public JArray getKeyinEquip(bool kybs)
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    if (kybs && isKyEquip(g))
                    {
                        list.Add(g);
                    }
                    else if (!kybs && !isKyEquip(g))
                    {
                        list.Add(g);
                    }

                }
            }
            return list;
        }
        /**获取未/已绑定的装备*/
        public JArray getBindEquip(bool bdbs)
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    if (bdbs && isBindEquip(g))
                    {
                        list.Add(g);
                    }
                    else if (!bdbs && !isBindEquip(g))
                    {
                        list.Add(g);
                    }

                }
            }
            return list;
        }
        /**获取精炼的副装备*/
        public JArray getJinglianFuEquip(JObject zhuZb)
        {
            //要求同级别、同品质
            Equip zhu = (Equip)face.goodsInterface.getGoodsMsgByKey(zhuZb["key"].ToString());

            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0 && !g["Id"].ToString().Equals(zhuZb["Id"].ToString()))
                {
                    Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.lv == zhu.lv)
                    {
                        //对于紫品及以上则要求副装也要如此
                        if ((zhu.quality < 2 && gd.quality == zhu.quality) || (zhu.quality >= 2 && gd.quality >= 2))
                        {
                            Debug.Log(gd.lv + "=>" + gd.quality + "=>" + gd.name);
                            list.Add(g);
                        }
                    }
                }
            }
            return list;
        }
        public bool isMoBan(string key)
        {
            if (key.Substring(0, 4).Equals("1010")) return true;
            return false;
        }
        /**获取模板*/
        public JArray getMoBan()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if ((int)g["pos"] == 0 && isMoBan(g["key"].ToString()))
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取橙色装备*/
        public JArray getChengEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                //要求品质紫品及以下，橙跟红需要用到模板
                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0 && (int)g["forging"]["lv"] > 10)
                {
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.quality != 3 || gd.lv < 91) continue;
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取可加抗的装备*/
        public JArray getJiaKangEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                //要求品质紫品及以下，橙跟红需要用到模板
                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0 && (int)g["forging"]["lv"] >= 10)
                {
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.quality < 2 || gd.lv < 91) continue;
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取可锻造的装备*/
        public JArray getInlayEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString()) && !isFaBao(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取好的的装备*/
        public JArray getGoodEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取可注潜的装备*/
        public JArray getZhuQianEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    JArray a = (JArray)g["inlay"]["list"];
                    if (a.Count != 0)
                        list.Add(g);
                }
            }
            return list;
        }
        /**获取可锻造的装备*/
        public JArray getForgingEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                //要求品质紫品及以下，橙跟红需要用到模板
                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    //GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    //if (gd.quality > 2) continue;
                    //只要求锻造等级<=15
                    if (g["forging"] == null || (int)g["forging"]["lv"] <= 15)
                        list.Add(g);
                }
            }
            return list;
        }
        /**获取紫色的装备*/
        public JArray getZiEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;

                //要求品质紫品及以下，橙跟红需要用到模板
                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0)
                {
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                    if (gd.quality != 2 || gd.lv < 91) continue;
                    //只要求锻造等级<=15
                    if (g["forging"] != null && (int)g["forging"]["lv"] > 10)
                        list.Add(g);
                }
            }
            return list;
        }

        /**根据部位跟职业来获取装备*/
        public JArray getEquipByJobAndPart(string model, string part)
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;
                Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                if ((int)g["pos"] == 0 && (int)g["isBad"] == 0 && model.Contains(gd.job) &&
                    (gd.part.Equals(part) || gd.part.Equals("*")))
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取所有装备，包含损坏的*/
        public JArray getAllEquip()
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;
                if ((int)g["pos"] == 0)
                {
                    list.Add(g);
                }
            }
            return list;
        }
        /**获取可精炼的副装备*/
        /*public JArray getJinglianFuEquip(string mainId, string part)
        {
            JArray list = new JArray();
            JArray gs = face.goodsInterface.getAllGoods();
            foreach (object o in gs)
            {
                JObject g = (JObject)o;
                if (!isEquip(g["key"].ToString())) continue;
                Equip obj = (Equip)face.goodsInterface.getGoodsMsgByKey(g["key"].ToString());
                if (!g["Id"].ToString().Equals(mainId) && (int)g["pos"] == 0
                    && (int)g["isBad"] == 0 && obj.part.Equals(part))
                {
                    list.Add(g);
                }
            }
            return list;
        }*/



        /**获取可学习技能的装备*/
        public JArray getLearnSklEquip()
        {
            JObject r = face.roleInterface.getRole();
            JObject equips = (JObject)r["attr"]["equip"];
            JArray list = new JArray();
            IEnumerable<JProperty> properties = equips.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value.ToString().Trim().Equals("")) continue;
                JObject o = (JObject)item.Value;
                Equip msg = (Equip)face.goodsInterface.getGoodsMsgByKey(o["key"].ToString());
                string p = msg.part.ToString();
                if (p != "bjb" && p != "gf" && p != "fb" && p != "hf")
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }
        /**获取刻印的装备*/
        public JArray getKeYinEquip()
        {
            JObject r = face.roleInterface.getRole();
            JObject equips = (JObject)r["attr"]["equip"];

            JArray list = new JArray();
            IEnumerable<JProperty> properties = equips.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value.ToString().Trim().Equals("")) continue;
                JObject o = (JObject)item.Value;
                Equip msg = (Equip)face.goodsInterface.getGoodsMsgByKey(o["key"].ToString());
                string p = msg.part.ToString();
                if (p != "bjb" && p != "gf" && p != "fb" && p != "hf")
                {
                    list.Add(item.Value);
                }

            }

            return list;
        }
        /**获取可升金的装备*/
        public JArray getUpGoldEquip()
        {
            JObject r = face.roleInterface.getRole();
            JObject equips = (JObject)r["attr"]["equip"];

            JArray list = new JArray();
            IEnumerable<JProperty> properties = equips.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value.ToString().Trim().Equals("")) continue;
                JObject o = (JObject)item.Value;
                Equip msg = (Equip)face.goodsInterface.getGoodsMsgByKey(o["key"].ToString());
                if ((int)msg.lv <= 90 ||
                    (o["isGold"] != null && (int)o["isGold"] == 1) ||
                    this.isShenEquip(o["key"].ToString()))
                {
                    continue;
                }
                list.Add(item.Value);
            }

            return list;
        }
        /**获取可升亮金的装备*/
        public JArray getUpLiangGoldEquip()
        {
            JObject r = face.roleInterface.getRole();
            JObject equips = (JObject)r["attr"]["equip"];

            JArray list = new JArray();
            IEnumerable<JProperty> properties = equips.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value.ToString().Trim().Equals("")) continue;
                JObject o = (JObject)item.Value;
                Equip msg = (Equip)face.goodsInterface.getGoodsMsgByKey(o["key"].ToString());
                if ((int)msg.lv <= 90 || o["isGold"] == null ||
                    (o["isLj"] != null && (int)o["isLj"] == 1) ||
                    this.isShenEquip(o["key"].ToString()))
                {
                    continue;
                }
                list.Add(item.Value);
            }
            return list;
        }
        /**由key获得法宝、护符、功法信息 */
        public JObject getFbHfGfMsgByKey(string key)
        {
            JArray names = this.getFbHfGfMsg();
            foreach (object p in names)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key)) return a;
            }
            return null;
        }
        /**活得法宝碎片 */
        public Goods getFbSuipian(string key)
        {
            JObject obj = null;
            JArray list = this.getFbHfGfMsg();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key))
                {
                    obj = a;
                    break;
                }
            }
            if (obj == null)
            {
                return null;
            }
            Goods g = new Goods(key);
            g.init(3, obj["name"] + "碎片", "");
            return g;
        }
        /**获取金装数量 */
        public int getGoldNum()
        {
            JObject r = face.roleInterface.getRole();
            int num = 0;
            JObject a = (JObject)r["attr"]["equip"];
            IEnumerable<JProperty> properties = a.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value == null || item.Value.ToString().Trim().Equals("")) continue;
                if (this.isGoldEquip(item.Value["key"].ToString()))
                {
                    num++;
                }
            }

            return num;
        }
        public int getGoldType()
        {
            return getGoldType(getGoldNum());
        }
        public int getGoldType(int num)
        {
            //0-2 3个档位 -1就是没有橙装效果
            if (num > 0 && num < 4) return 0;
            else if (num >= 4 && num < 9) return 1;
            else if (num >= 9) return 2;
            else return -1;
        }
        /**获取神装数量 */
        public int getShenEquipNum()
        {
            JObject r = face.roleInterface.getRole();
            int num = 0;
            JObject a = (JObject)r["attr"]["equip"];
            IEnumerable<JProperty> properties = a.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value == null || item.Value.ToString().Trim().Equals("")) continue;
                if (this.isShenEquip(item.Value["key"].ToString()))
                {
                    num++;
                }
            }

            return num;
        }
        /**获取亮金数量 */
        public int getLiangGoldNum()
        {
            JObject r = face.roleInterface.getRole();
            int num = 0;
            JObject a = (JObject)r["attr"]["equip"];
            IEnumerable<JProperty> properties = a.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value == null || item.Value.ToString().Trim().Equals("")) continue;
                if (this.isLiangGoldEquip(item.Value["key"].ToString()))
                {
                    num++;
                }
            }

            return num;
        }
        /**是否为橙*/
        public bool isGoldEquip(string key)
        {
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(key);
            if (ep.quality == 3) return true;
            return false;
        }
        /**是否为亮金*/
        public bool isLiangGoldEquip(string key)
        {
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(key);
            if (ep.quality == 4) return true;
            return false;
        }
        /**是否为神装*/
        public bool isShenEquip(string key)
        {
            Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(key);
            if (ep.quality == 5) return true;
            return false;
        }

        public JObject getAddAttrByForgingLv(JObject equip)
        {
            //获取装备信息
            Equip g = (Equip)face.goodsInterface.getGoodsMsgByKey(equip["key"].ToString());
            JObject obj = new JObject();
            float k = (int)g.lv * 0.3f * (int)equip["forging"]["lv"];

            switch (g.part.ToString())
            {
                case "wq":
                    {//武器 影响法攻、物攻 fg、wg
                        obj["wg"] = k;
                        obj["fg"] = k;
                        break;
                    }
                case "jb":
                    {//颈部
                        obj["wg"] = 0.5f * k;
                        obj["fg"] = 0.5f * k;
                        obj["max_lan"] = 3f * k;
                        break;
                    }
                case "sz":
                    {//手指
                        obj["wg"] = 0.8f * k;
                        obj["fg"] = 0.8f * k;
                        break;
                    }
                case "wb":
                    {//腕部
                        obj["wf"] = 0.5f * k;
                        obj["ff"] = 0.5f * k;
                        break;
                    }
                case "tb":
                    {//头部
                        obj["max_xue"] = 7f * k;
                        break;
                    }
                case "xb":
                    {//胸部
                        obj["max_xue"] = 9f * k;
                        break;
                    }
                case "yb":
                    {//腰部
                        obj["wf"] = k / 3.5f;
                        obj["ff"] = k / 3.5f;
                        break;
                    }
                case "tuib":
                    {//腿部
                        obj["wf"] = k / 2.7f;
                        obj["ff"] = k / 2.7f;
                        break;
                    }
                case "jiaob":
                    {//脚部
                        obj["wf"] = k / 2.3f;
                        obj["ff"] = k / 2.3f;
                        obj["css"] = 1.8f * k;
                        break;
                    }
                case "bjb":
                    {

                        break;
                    }
            }
            switch (g.job.ToString())
            {
                case "ms":
                    {
                        //物攻、物防翻倍
                        if (obj["wg"] != null)
                            obj["wg"] = (float)obj["wg"] * 2f;
                        if (obj["wf"] != null)
                            obj["wf"] = (float)obj["wf"] * 2f;
                        break;
                    }
                case "dj":
                    {
                        //物攻、物防翻倍
                        if (obj["wg"] != null)
                            obj["wg"] = (float)obj["wg"] * 2f;
                        if (obj["wf"] != null)
                            obj["wf"] = (float)obj["wf"] * 2f;
                        break;
                    }
                case "qm":
                    {
                        if (obj["fg"] != null)
                            obj["fg"] = (float)obj["fg"] * 2f;
                        if (obj["ff"] != null)
                            obj["ff"] = (float)obj["ff"] * 2f;
                        break;
                    }
                case "ty":
                    {
                        if (obj["fg"] != null)
                            obj["fg"] = (float)obj["fg"] * 2f;
                        if (obj["ff"] != null)
                            obj["ff"] = (float)obj["ff"] * 2f;
                        break;
                    }
                case "ym":
                    {
                        if (obj["wg"] != null)
                            obj["wg"] = (float)obj["wg"] * 2f;
                        if (obj["css"] != null)
                            obj["css"] = (float)obj["css"] * 2f;
                        break;
                    }
                case "lc":
                    {
                        if (obj["wg"] != null)
                            obj["wg"] = (float)obj["wg"] * 2f;
                        if (obj["css"] != null)
                            obj["css"] = (float)obj["css"] * 2f;
                        break;
                    }
            }
            /*IEnumerable<JProperty> properties = obj.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Value != null)
                    obj[item.Name] = (int)item.Value;
            }*/

            return obj;
        }
        public JObject getFbByKey(string key)
        {
            JArray names = this.getFbHfGfMsg();
            foreach (object p in names)
            {
                JObject o = (JObject)p;
                if (o["key"].ToString().Equals(key))
                {
                    return o;
                }
            }
            return null;
        }
        /**判断是否为套装*/
        public bool isTaoZhuang(string equipKey)
        {
            int b = int.Parse(equipKey.Substring(8));
            int taoz = b / (10 * 6 * 9);
            if (taoz == 5) return true;
            return false;
        }
        /**
         * 是否为宠物装备
         */
        public bool isPetEquip(string key)
        {
            if (strUtils.isMatch(key, "1016([0-9]{4})"))
                return true;
            return false;
        }
        /**
         * 是否为装备(不包含法宝此类)
         */
        public bool isEquip(string key)
        {
            if (key.Substring(0, 4).Equals("1001"))
            {
                //0-5是装备5以上就是补给包、法宝等
                if (strUtils.isMatch(key.Substring(4, 4), "100([0-5]{1})"))
                    return true;
            }
            return false;
        }
        public bool isFaBao(string key)
        {
            if (key.Substring(0, 4).Equals("1001"))
            {
                if (key.Substring(4, 4).Equals("1007"))
                    return true;
            }
            return false;
        }
        public bool isHuFu(string key)
        {
            if (key.Substring(0, 4).Equals("1001"))
            {
                if (key.Substring(4, 4).Equals("1008"))
                    return true;
            }
            return false;
        }

        private void addBuyRule(int moneyType, int value, JArray buyList)
        {
            JObject rule = new JObject();
            rule.Add("moneyType", moneyType);
            rule.Add("value", value);
            buyList.Add(rule);
        }

        private JObject getFbObj(string key, string part, string name, string icon,
            string resKey, float k, float b, int limitLv = 100,
            string resKey1 = null, float k1 = 0, float b1 = 0)
        {
            JObject obj = new JObject();
            obj.Add("key", key);
            obj.Add("part", part);
            obj.Add("name", name);
            obj.Add("icon", icon);
            JArray result = new JArray();
            JObject a = new JObject();
            a.Add("key", resKey);
            a.Add("k", k);
            a.Add("b", b);
            a.Add("limitLv", limitLv);
            result.Add(a);
            if (resKey1 != null)
            {
                a = new JObject();
                a.Add("key", resKey1);
                a.Add("k", k1);
                a.Add("b", b1);
                a.Add("limitLv", limitLv);
                result.Add(a);
            }
            obj.Add("result", result);
            return obj;
        }
        /**从法宝护符功法中获取具体信息*/
        private JObject getOneFromFbHfGf(string key)
        {
            JArray list = getFbHfGfMsg();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i]["key"].ToString().Equals(key)) return (JObject)list[i];
            }
            return null;
        }
        private JArray getFbHfGfMsg()
        {
            JArray list = new JArray();
            JObject obj = getFbObj("20070000", "fb", "盘古斧", "prop_fabao_png", "final_hurt_add", 0.01f, 0f);
            list.Add(obj);
            obj = getFbObj("20070001", "fb", "混沌青莲", "prop_fabao_png", "final_cure_add", 0.007f, 0f);
            list.Add(obj);
            obj = getFbObj("20070002", "fb", "诸天庆云", "prop_fabao_png", "wf&ff", 50f, 0f);
            list.Add(obj);
            obj = getFbObj("20070003", "fb", "东皇钟", "prop_fabao_png", "final_hurt_cut", 0.009f, 0f);
            list.Add(obj);
            obj = getFbObj("20070004", "fb", "覆海神刀", "prop_fabao_png", "wg", 70f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070005", "fb", "祝融戟", "prop_fabao_png", "fg", 70f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070006", "fb", "炼妖壶", "prop_fabao_png", "wf", 50f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070007", "fb", "伏羲琴", "prop_fabao_png", "bj", 100f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070008", "fb", "九天息壤", "prop_fabao_png", "ff", 50f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070009", "fb", "盘古幡", "prop_fabao_png", "max_xue", 200f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070010", "fb", "翻天印", "prop_fabao_png", "final_hurt_add", 0.005f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070011", "fb", "紫绶仙衣", "prop_fabao_png", "final_hurt_cut", 0.005f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20070012", "fb", "捆仙锁", "prop_fabao_png", "final_cure_add", 0.005f, 0f, 80);
            list.Add(obj);
            obj = getFbObj("20080000", "hf", "青龙护符", "prop_hufu_png", "bjkx", 80f, 0f, 60);
            list.Add(obj);
            obj = getFbObj("20080001", "hf", "白虎护符", "prop_hufu_png", "lxkx", 80f, 0f, 60);
            list.Add(obj);
            obj = getFbObj("20080002", "hf", "朱雀护符", "prop_hufu_png", "hlkx", 80f, 0f, 60);
            list.Add(obj);
            obj = getFbObj("20080003", "hf", "玄武护符", "prop_hufu_png", "hskx", 80f, 0f, 60);
            list.Add(obj);
            obj = getFbObj("20090000", "gf", "覆神诀", "prop_gongfa_png", "final_wg", 0.008f, 0f, 60, "final_fg", 0.008f, 0f);
            list.Add(obj);
            obj = getFbObj("20090001", "gf", "御体诀", "prop_gongfa_png", "final_wf", 0.008f, 0f, 60, "final_ff", 0.008f, 0f);
            list.Add(obj);

            return list;
        }
    }
}
