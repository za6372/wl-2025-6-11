using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{
    public class countUtils
    {
        
        /**计算宠物属性*/
        public static JObject countPetProp(JObject r)
        {
            //Debug.Log(r);
            JObject baseAttr = new JObject();
            baseAttr["ll"] = (float)r["baseProp"]["ll"];
            baseAttr["zl"] = (float)r["baseProp"]["zl"];
            baseAttr["mj"] = (float)r["baseProp"]["mj"];
            baseAttr["nl"] = (float)r["baseProp"]["nl"];
            baseAttr["js"] = (float)r["baseProp"]["js"];


            if (r.ContainsKey("petEquip") && !strUtils.isNull(r["petEquip"]))
            {
                JObject zb = (JObject)r["petEquip"];
                IEnumerable<JProperty> properties = zb.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || strUtils.getMillis() > (long)playerGoods["endTime"] || !face.equipInterface.isPetEquip(playerGoods["key"].ToString())) continue;
                    PetEquip ep = (PetEquip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JObject equip = strUtils.copyJSON<JObject>(ep);
                    JObject fixedAttrRule = (JObject)equip["attr"];
                    if (fixedAttrRule != null)
                    {
                        IEnumerable<JProperty> fixedAttrRuleProp = fixedAttrRule.Properties();
                        foreach (JProperty a in fixedAttrRuleProp)
                        {
                            string attr = a.Name.ToString();
                            if (!isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value;
                        }
                    }
                }
            }



            baseAttr["xue"] = (float)r["attr"]["prop"]["xue"];
            baseAttr["lan"] = (float)r["attr"]["prop"]["lan"];
            baseAttr["exp"] = (float)r["attr"]["prop"]["exp"];
            baseAttr["max_xue"] = (Math.Pow(1.6f, (float)r["growLv"]) * 400 + (float)r["lever"] * 20 + (float)r["lever"] * 80 * (1 + (float)baseAttr["nl"] / 2000) * (1 + (float)r["qualityValue"]["max_xue"] / 50000)
                + ((float)baseAttr["nl"]) * 3 * (float)r["qualityValue"]["max_xue"]
                / 1000 + ((float)baseAttr["js"]) * 3 * (float)r["qualityValue"]["max_xue"] / 1000);
            baseAttr["max_lan"] = ((float)r["qualityValue"]["max_lan"] / 2) + (float)r["lever"] * 20 + (50 + ((float)baseAttr["js"]) * 5 * (float)r["qualityValue"]["max_lan"] / 1000);
            //经验公式
            //baseAttr.max_exp = (lever * 100 + 1.03 * Math.pow(5, 0.1 * lever) );
            baseAttr["max_exp"] = (float)r["lever"] * 100f + 2f * Math.Pow(5f, 0.09f * (float)r["lever"]);
            baseAttr["wg"] = Math.Pow(1.4f, (float)r["growLv"]) * 100 + (float)r["lever"] * 10f + (float)r["lever"] * 8f * (1f + (float)r["qualityValue"]["wg"] / 10000f)
                + ((float)baseAttr["ll"]) * 1.3f * (float)r["qualityValue"]["wg"] / 4000f;
            baseAttr["fg"] = Math.Pow(1.4f, (float)r["growLv"]) * 100 + (float)r["lever"] * 10 + (float)r["lever"] * 8 * (1 + (float)r["qualityValue"]["fg"] / 10000)
                + ((float)baseAttr["zl"]) * 1.3f * (float)r["qualityValue"]["fg"] / 4000;
            baseAttr["wf"] = Math.Pow(1.3f, (float)r["growLv"]) * 50 + (float)r["lever"] * 5f + (float)r["lever"] * 6 * (1 + (float)r["qualityValue"]["wf"] / 10000)
                + ((float)baseAttr["nl"]) * 0.8f * (float)r["qualityValue"]["wf"] / 4000;
            baseAttr["ff"] = Math.Pow(1.3f, (float)r["growLv"]) * 50 + (float)r["lever"] * 5f + (float)r["lever"] * 6 * (1 + (float)r["qualityValue"]["ff"] / 10000)
                + ((float)baseAttr["nl"]) * 0.8f * (float)r["qualityValue"]["ff"] / 4000;
            baseAttr["mz"] = Math.Pow(1.4f, (float)r["growLv"]) * 30 + (float)r["lever"] * 8f + (float)r["lever"] * 8 * (1 + (float)r["qualityValue"]["mz"] / 10000)
                + ((float)baseAttr["ll"]) * 1.2f * (float)r["qualityValue"]["mz"] / 4000;
            baseAttr["sd"] = Math.Pow(1.4f, (float)r["growLv"]) * 15 + (float)r["lever"] * 3f + (float)r["lever"] * 3 * (1 + (float)r["qualityValue"]["sd"] / 10000)
                + ((float)baseAttr["mj"]) * 0.6f * (float)r["qualityValue"]["sd"] / 4000;
            baseAttr["bj"] = Math.Pow(1.4f, (float)r["growLv"]) * 20 + (float)r["lever"] * 5f + (float)r["lever"] * 5 * (1 + (float)r["qualityValue"]["bj"] / 10000)
                + ((float)baseAttr["zl"]) * 2 * (float)r["qualityValue"]["bj"] / 4000;
            baseAttr["css"] = Math.Pow(1.4f, (float)r["growLv"]) * 20 + (float)r["lever"] * 5f + (float)r["lever"] * 10 * (1 + (float)r["qualityValue"]["css"] / 10000)
                + ((float)baseAttr["mj"]) * 1.5f * (float)r["qualityValue"]["css"] / 4000;
            baseAttr["lxkx"] = 0;
            baseAttr["bjkx"] = 0;
            baseAttr["hlkx"] = 0;
            baseAttr["hskx"] = 0;

            if (r.ContainsKey("petEquip") && !strUtils.isNull(r["petEquip"]))
            {
                JObject zb = (JObject)r["petEquip"];
                IEnumerable<JProperty> properties = zb.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || strUtils.getMillis() > (long)playerGoods["endTime"] || !face.equipInterface.isPetEquip(playerGoods["key"].ToString())) continue;
                    PetEquip ep = (PetEquip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JObject equip = strUtils.copyJSON<JObject>(ep);
                    JObject fixedAttrRule = (JObject)equip["attr"];
                    if (fixedAttrRule != null)
                    {
                        IEnumerable<JProperty> fixedAttrRuleProp = fixedAttrRule.Properties();
                        foreach (JProperty a in fixedAttrRuleProp)
                        {
                            string attr = a.Name.ToString();
                            if (isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value;
                        }
                    }
                }
            }
            if (r.ContainsKey("xrmf") && !strUtils.isNull(r["xrmf"]))
            {
                JObject xrmf = (JObject)r["xrmf"];
                int xfLv = (int)xrmf["xfLv"];
                float d = (float)(xfLv * 20f + Math.Pow(1.41f, xfLv) / 3f);
                baseAttr["max_xue"] = (float)baseAttr["max_xue"] + d;
                //因为仙人秘法这个属性仅参与计算，计算完要删除
                r.Remove("xrmf");
            }
            //计算属性丹
            JObject danAttr = (JObject)r["danAttr"];
            if (danAttr != null)
            {
                JObject d1 = (JObject)danAttr["d1"];
                JObject d2 = (JObject)danAttr["d2"];
                JObject d3 = (JObject)danAttr["d3"];
                foreach (JProperty k in d1.Properties())
                {
                    int max = face.petInterface.getMaxPetDanAttr(k.Name, 1);
                    int v = (int)d1[k.Name];
                    if (v > max) v = max;
                    baseAttr[k.Name] = (float)baseAttr[k.Name] + v;
                }
                foreach (JProperty k in d2.Properties())
                {
                    int max = face.petInterface.getMaxPetDanAttr(k.Name, 2);
                    int v = (int)d2[k.Name];
                    if (v > max) v = max;
                    baseAttr[k.Name] = (float)baseAttr[k.Name] + v;
                }
                foreach (JProperty k in d3.Properties())
                {
                    int max = face.petInterface.getMaxPetDanAttr(k.Name, 3);
                    int v = (int)d3[k.Name];
                    if (v > max) v = max;
                    baseAttr[k.Name] = (float)baseAttr[k.Name] + v;
                }
            }
            //技能提升的属性
            JArray jn = (JArray)r["attr"]["skill"];
            if (jn != null)
            {
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || obj["key"] == null) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    JArray attrs = skl.getLearnRuleAttrs();
                    if (attrs == null) continue;
                    for (int i = 0; i < attrs.Count; i++)
                    {
                        JObject aa = (JObject)attrs[i];
                        if ((int)aa["valueType"] == 1) continue;
                        string attr = aa["name"].ToString();
                        //去掉是5个基础属性的
                        if (isBaseProp(attr)) continue;
                        float v = skl.getLearnResultRuleCount((int)obj["lv"], aa);
                        baseAttr[attr] = (float)baseAttr[attr] + v;
                    }
                }

            }
            //由某个属性加持另一个属性
            if (jn != null)
            {
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || obj["key"] == null) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    if (skl.aToBAttrIsNull()) continue;
                    if (skl.aToBAttrBKeyIsEquals("wf&ff"))
                    {
                        float t = skl.countAToBAttr(baseAttr);
                        baseAttr["wf"] = (float)baseAttr["wf"] + t;
                        baseAttr["ff"] = (float)baseAttr["ff"] + t;
                    }
                    else
                    {
                        string k = skl.getAToBAttrKey();
                        baseAttr[k] = (float)baseAttr[k] + skl.countAToBAttr(baseAttr);
                    }

                }
            }
            /*Debug.Log("==============1");
            Debug.Log(baseAttr);*/
            //至此固定的属性基数已经计算完成，开始计算增益
            string[] rateKeys = {"max_xue", "max_lan",
                "wg", "fg", "wf", "ff",
                "mz", "sd", "bj", "css",
                "bjkx", "hskx", "hlkx", "lxkx"};
            //存储每个属性的增益比例
            JObject rateMap = new JObject();
            foreach (string k in rateKeys)
            {
                rateMap[k] = 1f;
            }

            if (jn != null)
            {
                //JObject copyBase = strUtils.copyJSON<JObject>(baseAttr);
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || obj["key"] == null) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    JArray attrs = skl.getLearnRuleAttrs();
                    if (attrs == null) continue;
                    for (int i = 0; i < attrs.Count; i++)
                    {
                        JObject aa = (JObject)attrs[i];
                        if ((int)aa["valueType"] != 1) continue;
                        string attr = aa["name"].ToString();
                        if (isBaseProp(attr)) continue;
                        /*float v = skl.getLearnResultRuleCountByK((int)obj["lv"], aa, copyBase);
                        baseAttr[attr] = (float)baseAttr[attr] + v;*/
                        rateMap[attr] = (float)rateMap[attr] +
                            skl.getLearnResultRuleK((int)obj["lv"], aa);
                    }
                }
            }
            foreach (string p in rateKeys)
            {
                baseAttr[p] = (float)baseAttr[p] * (float)rateMap[p];
            }
            /*Debug.Log("==============2");
            Debug.Log(rateMap);
            Debug.Log("==============3");
            Debug.Log(baseAttr);*/
            baseAttr["xue"] = (float)baseAttr["xue"] > (float)baseAttr["max_xue"] ? (float)baseAttr["max_xue"] : (float)baseAttr["xue"];
            baseAttr["lan"] = (float)baseAttr["lan"] > (float)baseAttr["max_lan"] ? (float)baseAttr["max_lan"] : (float)baseAttr["lan"];
            IEnumerable<JProperty> bs = baseAttr.Properties();
            foreach (JProperty a in bs)
            {
                float v = (float)a.Value;
                if (v < 0) v = 0;
                baseAttr[a.Name] = v;
            }
            return baseAttr;
        }
       
       
        public static JObject countProp(JObject role)
        {
            int lever = (int)role["lever"];
            JObject baseAttr = new JObject();
            baseAttr.Add("ll", lever);
            baseAttr.Add("zl", lever);
            baseAttr.Add("mj", lever);
            baseAttr.Add("nl", lever);
            baseAttr.Add("js", lever);
            JObject zb = (JObject)role["attr"]["equip"];
            JArray jn = (JArray)role["attr"]["skill"];
            JObject msg = (JObject)role["attr"]["msg"];
            JObject msLv = (JObject)role["attr"]["msLv"];
            JObject shenfu = null;
            if (!strUtils.isNull(role["attr"]["shenfu"]))
            {
                shenfu = (JObject)role["attr"]["shenfu"];
            }
            //Debug.Log(msLv);
            if (zb != null)
            {
                IEnumerable<JProperty> properties = zb.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JObject equip = strUtils.copyJSON<JObject>(ep);
                    JObject fixedAttrRule = (JObject)equip["fixedAttrRule"];
                    if (fixedAttrRule != null)
                    {
                        IEnumerable<JProperty> fixedAttrRuleProp = fixedAttrRule.Properties();
                        foreach (JProperty a in fixedAttrRuleProp)
                        {
                            string attr = a.Name.ToString();
                            if (!isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value;
                        }
                    }
                    JArray randomAttr = (JArray)playerGoods["randomAttr"];
                    if (randomAttr != null)
                    {
                        for (int i = 0; i < randomAttr.Count; i++)
                        {
                            JObject o = (JObject)randomAttr[i];
                            string e = o["k"].ToString();
                            if (!isBaseProp(e))
                            {
                                continue;
                            }
                            float v = (float)o["v"];
                            int max = face.equipInterface.getMaxJingLianAttr(playerGoods["key"].ToString(), e);
                            if (v > max) v = max;
                            baseAttr[e] = (float)baseAttr[e] + v;
                        }
                    }
                    //计算装备刻印加成
                    if (playerGoods["keyin"] != null)
                    {
                        IEnumerable<JProperty> prop = ((JObject)playerGoods["keyin"]).Properties();
                        foreach (JProperty a in prop)
                        {
                            if (a.Value == null || a.Value.ToString().Equals("")) continue;
                            JObject o = (JObject)a.Value;
                            int forgingLv = 0;
                            if (o["forging"] != null)
                            {
                                forgingLv = (int)o["forging"]["lv"];
                            }
                            JObject attr = face.goodsInterface.getKyAttr(o["key"].ToString());
                            string kN = attr["k"].ToString();
                            if (!isBaseProp(kN)) continue;
                            baseAttr[kN] = (float)baseAttr[kN] + (float)attr["v"] + (float)attr["v"] * 0.0375f * forgingLv;
                        }
                    }
                }
            }
            //技能属性
            if (jn != null)
            {
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || !obj.ContainsKey("key")) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    if (skl == null) continue;
                    JArray attrs = skl.getLearnRuleAttrs();
                    if (attrs == null)
                    {
                        Debug.Log("attrs不能为null");
                        continue;
                    }
                    foreach (object a in attrs)
                    {
                        JObject aa = (JObject)a;
                        if ((int)aa["valueType"] == 1) continue;
                        string attr = aa["name"].ToString();
                        if (!isBaseProp(attr)) continue;
                        float v = skl.getLearnResultRuleCount((int)obj["lv"], aa);
                        baseAttr[attr] = (float)baseAttr[attr] + v;
                    }
                }
            }
            //先把5个基础属性的算完
            baseAttr["max_xue"] = (float)(100 + (float)(baseAttr["nl"]) * 5 + (float)(baseAttr["js"]) * 3 + (lever - 1) * 50);
            baseAttr["max_lan"] = (float)(50 + (float)(baseAttr["js"]) * 5 + (lever - 1) * 5);
            //经验公式
            //baseAttr[max_exp = (float)(lever * 100 + 1.03 * Math.pow(5, 0.1 * lever) );
            baseAttr["max_exp"] = (float)getExpData(lever);
            baseAttr["wg"] = (float)(20 + (float)(baseAttr["ll"]) * 1.3 + lever * 2);
            baseAttr["fg"] = (float)(20 + (float)(baseAttr["zl"]) * 1.3 + lever * 2);
            baseAttr["wf"] = (float)(2 + (float)(baseAttr["nl"]) * 0.8 + lever * 1);
            baseAttr["ff"] = (float)(2 + (float)(baseAttr["nl"]) * 0.8 + lever * 1);
            baseAttr["mz"] = (float)((float)(baseAttr["ll"]) * 1.5 + lever * 7f);
            baseAttr["sd"] = (float)((float)(baseAttr["mj"]) * 1.2 + lever * 2);
            baseAttr["bj"] = (float)((float)(baseAttr["zl"]) * 1 + lever * 2);
            baseAttr["css"] = (float)(9 + (float)(baseAttr["mj"]) * 1.5 + lever * 1);
            //四个抗性
            baseAttr["lxkx"] = 0;
            baseAttr["bjkx"] = 0;
            baseAttr["hlkx"] = 0;
            baseAttr["hskx"] = 0;

            /*Debug.Log("===========1");
            Debug.Log(baseAttr);*/
            //计算星级属性
            JObject starAttr = countStarAttr(1, role["model"].ToString());
            IEnumerable<JProperty> starAttrProp = starAttr.Properties();
            foreach (JProperty item in starAttrProp)
            {
                baseAttr[item.Name] = (float)baseAttr[item.Name] + (float)item.Value * (int)role["attr"]["star"]["num"];
            }

            //加上当前等级所点亮的属性
            JObject starAttr2 = countStarAttr((int)role["attr"]["star"]["num"] + 1, role["model"].ToString());
            IEnumerable<JProperty> starAttr2Prop = ((JObject)role["attr"]["star"]["attr"]).Properties();
            foreach (JProperty item in starAttr2Prop)
            {
                if ((int)item.Value == 1)
                {
                    baseAttr[item.Name] = (float)baseAttr[item.Name] + (float)starAttr2[item.Name];
                }
            }

            /*Debug.Log("===========2");
            Debug.Log(baseAttr);*/

            if (msg.ContainsKey("xrmf"))
            {
                JObject xrmf = (JObject)msg["xrmf"];
                int xrLv = (int)xrmf["xrLv"];
                float d = (float)(xrLv * 20f + Math.Pow(1.41f, xrLv) / 3f);
                baseAttr["max_xue"] = (float)baseAttr["max_xue"] + d;
            }

            /*Debug.Log("===========3");
            Debug.Log(baseAttr);*/

            //再计算其余衍生的属性
            if (zb != null)
            {
                IEnumerable<JProperty> properties = zb.Properties();
                //固定属性
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JObject equip = strUtils.copyJSON<JObject>(ep);
                    JObject fixedAttrRule = (JObject)equip["fixedAttrRule"];
                    if (fixedAttrRule != null)
                    {
                        IEnumerable<JProperty> fixedAttrRuleProp = fixedAttrRule.Properties();
                        foreach (JProperty a in fixedAttrRuleProp)
                        {
                            string attr = a.Name.ToString();
                            if (isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            float k0 = 1f;
                            //对于有绑定宝石的装备基础属性增加10%
                            if (playerGoods.ContainsKey("bdbs") &&
                                    (int)playerGoods["bdbs"] == 1)
                            {
                                k0 += 0.1f;
                            }
                            //对于有刻印宝石的装备基础属性增加30%
                            if (playerGoods.ContainsKey("kybs") &&
                                    (int)playerGoods["kybs"] == 1)
                            {
                                k0 += 0.3f;
                            }
                            //对于有血契宝石的装备基础属性增加60%
                            if (playerGoods.ContainsKey("xqbs") &&
                                    (int)playerGoods["xqbs"] == 1)
                            {
                                k0 += 0.6f;
                            }
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value * k0;
                        }
                    }
                }
                /*Debug.Log("===========固定属性");
                Debug.Log(baseAttr);*/
                //随机属性
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JArray randomAttr = (JArray)playerGoods["randomAttr"];
                    if (randomAttr != null)
                    {
                        for (int i = 0; i < randomAttr.Count; i++)
                        {
                            JObject o = (JObject)randomAttr[i];
                            string e = o["k"].ToString();
                            if (isBaseProp(e))
                            {
                                continue;
                            }
                            float v = (float)o["v"];
                            int max = face.equipInterface.getMaxJingLianAttr(playerGoods["key"].ToString(), e);
                            if (v > max) v = max;
                            baseAttr[e] = (float)baseAttr[e] + v;
                        }

                    }
                }
                /*Debug.Log("===========随机属性");
                Debug.Log(baseAttr);*/
                //锻造加成
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    if (playerGoods["forging"] != null && (int)playerGoods["forging"]["lv"] > 0)
                    {
                        JObject obj = face.equipInterface.getAddAttrByForgingLv(playerGoods);
                        IEnumerable<JProperty> prop = obj.Properties();
                        foreach (JProperty a in prop)
                        {
                            if (a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[a.Name] = (float)baseAttr[a.Name] + (float)a.Value;
                        }
                    }
                }
                /* Debug.Log("===========锻造加成");
                 Debug.Log(baseAttr);*/
                //镶嵌加成
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    if (playerGoods["inlay"] != null && (int)playerGoods["inlay"]["num"] > 0 &&
                        ((JArray)playerGoods["inlay"]["list"]).Count > 0)
                    {
                        JArray list = (JArray)playerGoods["inlay"]["list"];
                        foreach (object p in list)
                        {
                            JObject a = (JObject)p;
                            Baoshi bsMsg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            string kn = bsMsg.getAttrName();
                            float max = bsMsg.getBaoshiAttr();
                            float v = (float)a["num"];
                            if (v > max) v = max;
                            baseAttr[kn] = (float)baseAttr[kn] + v;
                        }
                    }
                }
                /*Debug.Log("===========镶嵌加成");
                Debug.Log(baseAttr);*/
                //注魔加成
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    if (playerGoods.ContainsKey("czmb"))
                    {
                        JObject czmb = (JObject)playerGoods["czmb"];
                        MoBan mbMsg = (MoBan)face.goodsInterface.getGoodsMsgByKey(czmb["key"].ToString());
                        float k0 = (float)czmb["num"] / mbMsg.getMaxMoLi();
                        if (k0 > 1) k0 = 1f;
                        fightAttr attr = mbMsg.getMaxAttr();
                        PropertyInfo[] props = attr.GetType().GetProperties();
                        foreach (PropertyInfo property in props)
                        {
                            /*if (Convert.ToInt32(property.GetValue(attr)) == 0) continue;
                            int kx = Convert.ToInt32(Convert.ToInt32(property.GetValue(attr)) * k0);*/
                            if ((float)property.GetValue(attr) == 0) continue;
                            float kx = (float)property.GetValue(attr) * k0;
                            baseAttr[property.Name] = (float)baseAttr[property.Name] + kx;
                        }
                    }
                }
                /*Debug.Log("===========注魔加成");
                Debug.Log(baseAttr);*/
                //抗性加护
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    if (playerGoods.ContainsKey("kxlv"))
                    {
                        JObject kxlv = (JObject)playerGoods["kxlv"];
                        IEnumerable<JProperty> prop = kxlv.Properties();
                        foreach (JProperty a in prop)
                        {
                            int lv = (int)a.Value;
                            int kx = 50 * lv + (int)Math.Pow(2d, lv);
                            string attrName = a.Name.ToString().Substring(0, 2) + "kx";
                            baseAttr[attrName] = (float)baseAttr[attrName] + kx;
                        }
                    }
                }
                /*Debug.Log("===========抗性加护");
                Debug.Log(baseAttr);*/
            }
            /*if (zb != null)
            {
                IEnumerable<JProperty> properties = zb.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)item.Value;
                    //只计算5个基础属性,只有随机属性才会出现这5个基础属性
                    if (playerGoods == null || (int)playerGoods["isBad"] == 1 || !face.equipInterface.isEquip(playerGoods["key"].ToString())) continue;
                    Equip ep = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                    if (ep == null) continue;
                    JObject equip = strUtils.copyJSON<JObject>(ep);

                    JObject fixedAttrRule = (JObject)equip["fixedAttrRule"];
                    if (fixedAttrRule != null)
                    {
                        IEnumerable<JProperty> fixedAttrRuleProp = fixedAttrRule.Properties();
                        foreach (JProperty a in fixedAttrRuleProp)
                        {
                            string attr = a.Name.ToString();
                            if (isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            float k0 = 1f;
                            //对于有绑定宝石的装备基础属性增加10%
                            if (playerGoods.ContainsKey("bdbs") &&
                                    (int)playerGoods["bdbs"] == 1)
                            {
                                k0 += 0.1f;
                            }
                            //对于有刻印宝石的装备基础属性增加30%
                            if (playerGoods.ContainsKey("kybs") &&
                                    (int)playerGoods["kybs"] == 1)
                            {
                                k0 += 0.3f;
                            }
                            //对于有血契宝石的装备基础属性增加60%
                            if (playerGoods.ContainsKey("xqbs") &&
                                    (int)playerGoods["xqbs"] == 1)
                            {
                                k0 += 0.6f;
                            }
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value * k0;
                        }
                    }

                    JObject randomAttr = (JObject)playerGoods["randomAttr"];
                    if (randomAttr != null)
                    {
                        IEnumerable<JProperty> randomAttrProp = randomAttr.Properties();
                        foreach (JProperty a in randomAttrProp)
                        {
                            string attr = a.Name.ToString();
                            if (isBaseProp(attr) || a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[attr] = (float)baseAttr[attr] + (float)a.Value;
                        }
                    }

                    //计算锻造的加成
                    if (playerGoods["forging"] != null && (int)playerGoods["forging"]["lv"] > 0)
                    {
                        JObject obj = face.equipInterface.getAddAttrByForgingLv(playerGoods);
                        IEnumerable<JProperty> prop = obj.Properties();
                        foreach (JProperty a in prop)
                        {
                            if (a.Value == null || (float)a.Value == 0) continue;
                            baseAttr[a.Name] = (float)baseAttr[a.Name] + (float)a.Value;
                        }
                    }
                    //计算镶嵌加成
                    if (playerGoods["inlay"] != null && (int)playerGoods["inlay"]["num"] > 0 &&
                        ((JArray)playerGoods["inlay"]["list"]).Count > 0)
                    {
                        JArray list = (JArray)playerGoods["inlay"]["list"];
                        foreach (object p in list)
                        {
                            JObject a = (JObject)p;
                            Baoshi bsMsg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            string kn = bsMsg.getAttrName();
                            baseAttr[kn] = (float)baseAttr[kn] + (int)a["num"];
                        }
                    }
                    //装备注魔加成{"key":"10100012","num":10}
                    if (playerGoods.ContainsKey("czmb"))
                    {
                        JObject czmb = (JObject)playerGoods["czmb"];
                        MoBan mbMsg = (MoBan)face.goodsInterface.getGoodsMsgByKey(czmb["key"].ToString());
                        float k0 = (float)czmb["num"] / mbMsg.getMaxMoLi();
                        if (k0 > 1) k0 = 1f;
                        fightAttr attr = mbMsg.getMaxAttr();
                        PropertyInfo[] props = attr.GetType().GetProperties();
                        foreach (PropertyInfo property in props)
                        {
                            if (Convert.ToInt32(property.GetValue(attr)) == 0) continue;
                            int kx = Convert.ToInt32(Convert.ToInt32(property.GetValue(attr)) * k0);
                            baseAttr[property.Name] = (float)baseAttr[property.Name] + kx;
                        }
                    }
                    //抗性加护 （高级属性的累计）
                    if (playerGoods.ContainsKey("kxlv"))
                    {
                        JObject kxlv = (JObject)playerGoods["kxlv"];
                        IEnumerable<JProperty> prop = kxlv.Properties();
                        foreach (JProperty a in prop)
                        {
                            int lv = (int)a.Value;
                            int kx = 50 * lv + (int)Math.Pow(2d, lv);
                            string attrName = a.Name.ToString().Substring(0, 2) + "kx";
                            baseAttr[attrName] = (float)baseAttr[attrName] + kx;
                        }
                    }
                    //计算装备技能加成
                    if (playerGoods["skill"] != null && ((JArray)playerGoods["skill"]).Count > 0)
                    {
                        JObject copyBase = strUtils.copyJSON<JObject>(baseAttr);
                        JArray skls = (JArray)playerGoods["skill"];
                        foreach (object p in skls)
                        {
                            if (p == null) continue;
                            JObject a = (JObject)p;
                            Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            JArray attrs = skl.getLearnRuleAttrs();
                            foreach (object i in attrs)
                            {
                                JObject aa = (JObject)i;
                                string attr = aa["name"].ToString();
                                if (isBaseProp(attr)) continue;
                                float v = skl.getLearnResultRuleCount((int)a["lv"], aa);
                                baseAttr[attr] = (float)baseAttr[attr] + v;
                            }
                        }
                    }
                    //计算装备刻印加成
                    if (playerGoods["keyin"] != null)
                    {
                        IEnumerable<JProperty> prop = ((JObject)playerGoods["keyin"]).Properties();
                        foreach (JProperty a in prop)
                        {
                            if (a.Value == null || a.Value.ToString().Equals("")) continue;
                            JObject o = (JObject)a.Value;
                            int forgingLv = 0;
                            if (o["forging"] != null)
                            {
                                forgingLv = (int)o["forging"]["lv"];
                            }
                            JObject attr = face.goodsInterface.getKyAttr(o["key"].ToString());
                            string kN = attr["k"].ToString();
                            if (isBaseProp(kN)) continue;
                            baseAttr[kN] = (float)baseAttr[kN] + (float)attr["v"] + (float)attr["v"] * 0.0375f * forgingLv;
                        }
                    }
                }

            }*/
            /*Debug.Log("===========3.1");
            Debug.Log(baseAttr);*/
            //法宝只看镶嵌孔，对人物、宠物、怪物三者额外的伤害跟减免交由战斗中计算
            if (zb != null && !zb["fb"].ToString().Equals(""))
            {
                JObject fb = (JObject)zb["fb"];
                //string k = fb["key"].ToString();
                //JObject fbMsg = face.equipInterface.getFbByKey(k);
                if (fb["inlay"] != null &&
                        (int)fb["inlay"]["num"] > 0 &&
                        ((JArray)fb["inlay"]["list"]).Count() > 0)
                {
                    JArray list = (JArray)fb["inlay"]["list"];
                    foreach (object o in list)
                    {
                        JObject obj = (JObject)o;
                        Baoshi gd = (Baoshi)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                        string attr = gd.getAttrName();
                        float max = gd.getBaoshiAttr();
                        float v = (float)obj["num"];
                        if (v > max) v = max;
                        baseAttr[attr] = (float)baseAttr[attr] + v;
                    }
                }
            }
            /*Debug.Log("===========3.2");
            Debug.Log(baseAttr);*/
            if (zb != null && !zb["hf"].ToString().Equals(""))
            {
                JObject hf = (JObject)zb["hf"];
                string k = hf["key"].ToString();
                //需要声望等级
                int lv = 0;
                if (msg.ContainsKey("swdj"))
                {
                    lv = (int)msg["swdj"]["lv"];
                }
                Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(hf["key"].ToString());
                List<fbResult> list = gd.getHuFuResult();
                foreach (fbResult f in list)
                {
                    baseAttr[f.key] = (float)baseAttr[f.key] + f.getV(lv);
                }
            }
            /*Debug.Log("===========3.3");
            Debug.Log(baseAttr);*/
            //功法计算最终属性 wf\": 1789.2,\r\n  \"ff\": 2867.4,
            if (zb != null && !zb["gf"].ToString().Equals(""))
            {
                JObject gf = (JObject)zb["gf"];
                string k = gf["key"].ToString();
                JObject fbMsg = face.equipInterface.getFbHfGfMsgByKey(k);
                JArray list = (JArray)fbMsg["result"];
                foreach (object i in list)
                {
                    JObject f = (JObject)i;
                    if (f["key"].ToString().Contains("final_"))
                    {
                        string key = f["key"].ToString().Split('_')[1];
                        baseAttr[key] = (float)baseAttr[key] * (1 + ((float)f["k"] * (float)gf["forging"]["lv"] + (float)f["b"]));
                    }
                }
            }
            /*Debug.Log("===========4");
            Debug.Log(baseAttr);*/
            //技能属性 
            if (jn != null)
            {
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || !obj.ContainsKey("key")) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    JArray attrs = skl.getLearnRuleAttrs();
                    if (attrs == null)
                    {
                        Debug.Log("attrs不能为null");
                        continue;
                    }
                    foreach (object a in attrs)
                    {
                        JObject aa = (JObject)a;
                        if ((int)aa["valueType"] == 1) continue;
                        string attr = aa["name"].ToString();
                        if (isBaseProp(attr)) continue;
                        float v = skl.getLearnResultRuleCount((int)obj["lv"], aa);
                        baseAttr[attr] = (float)baseAttr[attr] + v;
                    }
                }

            }
            /*Debug.Log("===========5");
            Debug.Log(baseAttr);*/
            //至此固定的属性基数已经计算完成，开始计算增益
            string[] rateKeys = {"max_xue", "max_lan",
                "wg", "fg", "wf", "ff",
                "mz", "sd", "bj", "css",
                "bjkx", "hskx", "hlkx", "lxkx"};
            //存储每个属性的增益比例
            JObject rateMap = new JObject();
            foreach (string k in rateKeys)
            {
                rateMap[k] = 1f;
            }
            if (jn != null)
            {
                //JObject copyBase = strUtils.copyJSON<JObject>(baseAttr);
                foreach (object p in jn)
                {
                    JObject obj = (JObject)p;
                    if (obj == null || !obj.ContainsKey("key")) continue;
                    Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                    JArray attrs = skl.getLearnRuleAttrs();
                    if (attrs == null) continue;
                    foreach (object a in attrs)
                    {
                        JObject aa = (JObject)a;
                        if ((int)aa["valueType"] != 1) continue;
                        string attr = aa["name"].ToString();
                        if (isBaseProp(attr)) continue;
                        //float v = skl.getLearnResultRuleCountByK((int)obj["lv"], aa, copyBase);
                        //baseAttr[attr] = (float)baseAttr[attr] + v;
                        rateMap[attr] = (float)rateMap[attr] +
                            skl.getLearnResultRuleK((int)obj["lv"], aa);
                    }
                }
            }
            /*Debug.Log("===========rateMap1");
            Debug.Log(rateMap);*/


            /*Debug.Log("===========6");
            Debug.Log(baseAttr);*/
            //橙装加成（提升所有高级属性）
            if (zb != null)
            {
                //橙装加成 1->0% 2->2% 3->4% 4->7% 5->10% 6->14% 7->18% 8->25% 9->35%
                int num = 0;
                float rate = 1f;
                IEnumerable<JProperty> pp = zb.Properties();
                foreach (JProperty a in pp)
                {
                    if (a.Value.ToString().Equals("")) continue;
                    JObject playerGoods = (JObject)a.Value;
                    if (face.equipInterface.isEquip(playerGoods["key"].ToString()) &&
                        face.equipInterface.isGoldEquip(playerGoods["key"].ToString()))
                    {
                        num++;
                    }
                }

                float[] rates = { 0f, 0f, 0.02f, 0.04f, 0.07f, 0.1f, 0.14f, 0.18f, 0.25f, 0.35f, };
                rate = rates[num];
                //全属性增加rate
                foreach (string p in rateKeys)
                {
                    //baseAttr[p] = (float)baseAttr[p] * rate;
                    if (p.Equals("bjkx") || p.Equals("hskx") || p.Equals("hlkx") || p.Equals("lxkx"))
                    {
                        rateMap[p] = (float)rateMap[p] + rate;
                    }
                    else if (p.Equals("max_xue"))
                    {
                        rateMap[p] = (float)rateMap[p] + rate * 16f;
                    }
                    else if (p.Equals("wf") || p.Equals("ff"))
                    {
                        rateMap[p] = (float)rateMap[p] + rate * 2f;
                    }
                    else
                    {
                        rateMap[p] = (float)rateMap[p] + 0.01f * num;
                    }

                }
            }
            /*Debug.Log("===========rateMap2");
            Debug.Log(rateMap);*/
            /*Debug.Log("===========7");
            Debug.Log(baseAttr);*/
            if (msLv != null)
            {
                //Debug.Log(msLv);
                IEnumerable<JProperty> prop = msLv.Properties();
                foreach (JProperty a in prop)
                {
                    //Debug.Log(a.Name.ToString());
                    int lv = (int)a.Value;
                    if (a.Name.ToString().Equals("lv1"))
                    {//攻击
                        rateMap["wg"] = (float)rateMap["wg"] + 0.03f * lv;
                        rateMap["fg"] = (float)rateMap["fg"] + 0.03f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv2"))
                    {//防御
                        rateMap["wf"] = (float)rateMap["wf"] + 0.03f * lv;
                        rateMap["ff"] = (float)rateMap["ff"] + 0.03f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv3"))
                    {//生命
                        rateMap["max_xue"] = (float)rateMap["max_xue"] + 0.03f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv4"))
                    {//速度
                        rateMap["css"] = (float)rateMap["css"] + 0.05f * lv;
                        rateMap["sd"] = (float)rateMap["sd"] + 0.02f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv5"))
                    {//命中
                        rateMap["mz"] = (float)rateMap["mz"] + 0.03f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv6"))
                    {//魔法
                        rateMap["max_lan"] = (float)rateMap["max_lan"] + 0.03f * lv;
                    }
                    else if (a.Name.ToString().Equals("lv7"))
                    {//暴击
                        rateMap["bj"] = (float)rateMap["bj"] + 0.03f * lv;
                    }
                }
            }
            if (shenfu != null && strUtils.getMillis() < (long)shenfu["end"])
            {
                ShenFu sf = (ShenFu)face.goodsInterface.getGoodsMsgByKey(shenfu["sf_key"].ToString());
                JArray attrs = sf.getLearnRuleAttrs();
                if (attrs != null)
                {
                    foreach (object a in attrs)
                    {
                        JObject aa = (JObject)a;
                        if ((int)aa["valueType"] != 1) continue;
                        string attr = aa["name"].ToString();
                        if (isBaseProp(attr)) continue;
                        rateMap[attr] = (float)rateMap[attr] +
                            sf.getLearnResultRuleK(aa);
                    }
                }

            }
            /*Debug.Log("===========rateMap3");
            Debug.Log(rateMap);*/
            foreach (string p in rateKeys)
            {
                baseAttr[p] = (float)baseAttr[p] * (float)rateMap[p];
            }

            baseAttr["xue"] = (float)role["attr"]["prop"]["xue"] > (float)baseAttr["max_xue"] ? (float)baseAttr["max_xue"] : (float)role["attr"]["prop"]["xue"];
            baseAttr["lan"] = (float)role["attr"]["prop"]["lan"] > (float)baseAttr["max_lan"] ? (float)baseAttr["max_lan"] : (float)role["attr"]["prop"]["lan"];
            baseAttr["exp"] = (float)role["attr"]["prop"]["exp"];
            //转整数 wf\": 1935.19861,\r\n  \"ff\": 2867.4
            IEnumerable<JProperty> bs = baseAttr.Properties();
            foreach (JProperty a in bs)
            {
                float v = (float)a.Value;
                if (v < 0) v = 0;
                baseAttr[a.Name] = (float)a.Value;
            }
            /*Debug.Log("===========8");
            Debug.Log(baseAttr);*/
            return baseAttr;
        }
        /**获取等级所需经验*/
        public static Double getExpData(int lever)
        {
            return lever * 100 + 2 * Math.Pow(5, 0.1d * lever);
        }

        /**计算人物星级属性 */
        public static JObject countStarAttr(int lv, string model)
        {
            JObject obj = new JObject();
            obj.Add("wg", 30f * lv);
            obj.Add("fg", 30f * lv);
            obj.Add("max_xue", 200f * lv);
            obj.Add("wf", 20f * lv);
            obj.Add("ff", 20f * lv);
            obj.Add("max_lan", 100f * lv);
            obj.Add("bj", 20f * lv);
            obj.Add("mz", 30f * lv);
            obj.Add("css", 10f * lv);
            obj.Add("sd", 15f * lv);

            if (model.Contains("ms_"))
            {
                obj["wg"] = 60f * lv;
                obj["mz"] = 40f * lv;
            }
            else if (model.Contains("dj_"))
            {
                obj["max_xue"] = 300f * lv;
                obj["wf"] = 40f * lv;
                obj["ff"] = 40f * lv;
            }
            else if (model.Contains("qm_"))
            {
                obj["fg"] = 60f * lv;
                obj["bj"] = 40f * lv;
            }
            else if (model.Contains("ty_"))
            {
                obj["fg"] = 40f * lv;
                obj["bj"] = 40f * lv;
                obj["wf"] = 30f * lv;
                obj["ff"] = 30f * lv;
            }
            else if (model.Contains("ym_"))
            {
                obj["wg"] = 60f * lv;
                obj["css"] = 30f * lv;
                obj["sd"] = 30f * lv;
            }
            else if (model.Contains("lc_"))
            {
                obj["css"] = 30f * lv;
                obj["sd"] = 30f * lv;
                obj["wf"] = 30f * lv;
                obj["ff"] = 30f * lv;
            }
            return obj;
        }
        /**
        * 判断是否为基础属性
        */
        public static bool isBaseProp(string v)
        {
            string[] obj = {
            "ll", "nl", "js", "zl", "mj"
        };
            foreach (string a in obj)
            {
                if (a.Equals(v)) return true;
            }
            return false;
        }
    }
}
