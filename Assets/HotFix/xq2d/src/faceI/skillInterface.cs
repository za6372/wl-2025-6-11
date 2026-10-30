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

    public interface skillInterface
    {
        public JArray getHuobanSkillList(string petKey, string[] filterList);
        public JArray getPetSkillList(string petKey, string[] filterList);
        public JArray getManSkillListToPanel();
        public void learnSkill(string key, Action<JObject> callback);
        public JArray getPetSkillListToPanel(JArray skls);
        public void learnPetSkill(string sklKey, string petId, Action<JObject> callback);
        public void forgetPetSkill(string petId, string key, Action callback);
        public void forgetSkill(int type, int index, Action callback);
        public void openXianJueKeyin(int type, int index, Action callback);
        public JObject getSkl(int type, int index, JArray skls);
        public void sureSkillCover(int sure, int roleType, string key, int index, Action callback);
        public void surePetSkillCover(string petId, int sure, int roleType, string key, int index, Action callback);
    }
    public class skillInterfaceImpl : skillInterface
    {
        /**打通刻印 */
        public void openXianJueKeyin(int type, int index, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if (!face.goodsInterface.isEnoughInPackAndTip("10000164", 6))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/manService/openXianJueKeyin", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000164", 6);
                JArray list = (JArray)r["attr"]["skill"];
                JObject skl = new JObject();
                skl.Add("type", type);
                skl.Add("index", index);
                list.Add(skl);
                face.roleInterface.saveRole(r);
                callback();
                msgCode.showMsg(200);
            });
        }
        /**遗忘技能 */
        public void forgetSkill(int type, int index, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if (!face.goodsInterface.isEnoughInPackAndTip("10000165", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            dic.Add("index", index);
            DoGet.getInstance().sendPost("/manService/forgetSkill", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000165", 1);
                JArray list = (JArray)r["attr"]["skill"];
                JObject temp = getSkl(type, index, list);
                temp.Remove("key");
                temp.Remove("lv");
                face.roleInterface.saveRole(r);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**遗忘技能（测试用，实际上不需要） */
        public void forgetPetSkill(string petId, string key, Action callback)
        {
            JObject r = face.roleInterface.getRole();
            if ((int)r["attr"]["msg"]["gold"] - 10000 < 0)
            {
                msgCode.showMsg(737);
                return;
            }
            /*Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/forgetPetSkill", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 0)
                {
                    msgCode.showMsg(651);
                }
                else
                {
                    r["attr"]["msg"]["gold"] = (int)r["attr"]["msg"]["gold"] - 10000;
                    face.roleInterface.saveRole(r);
                    JArray list = face.petInterface.getPetList();
                    for (int p = 0; p < list.Count; p++)
                    {
                        if (list[p]["Id"].ToString().Equals(petId))
                        {
                            JArray skill = (JArray)list[p]["attr"]["skill"];
                            for (int i = 0; i < skill.Count; i++)
                            {
                                JObject a = (JObject)skill[i];
                                if (a["key"] != null && a["key"].ToString().Equals(key))
                                {

                                    a.Remove("key");
                                    a.Remove("lv");
                                    face.petInterface.savePet((JObject)list[p]);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    msgCode.showMsg(738);
                    callback();
                }
            });*/
        }
        /**学习宠物技能*/
        public void learnPetSkill(string sklKey, string petId, Action<JObject> callback)
        {
            //验证回天书是否在背包
            if (face.goodsInterface.keyIsInCangKu("10000120"))
            {
                msgCode.showMsg(228);
                return;
            }
            if (!face.goodsInterface.isEnoughInPackAndTip(sklKey, 1))
            {
                return;
            }

            JObject pet = face.petInterface.getOneById(petId);
            JArray skls = (JArray)pet["attr"]["skill"];
            for (int p = 0; p < skls.Count; p++)
            {
                if (skls[p]["key"] == null) continue;
                if (skls[p]["key"].ToString().Equals(sklKey))
                {
                    msgCode.showMsg(635);
                    return;
                }
            }
            //悟性是否满足
            Skill msg = (Skill)face.goodsInterface.getGoodsMsgByKey(sklKey);
            if ((int)pet["savvy"] < msg.savvy)
            {
                msgCode.showMsg(963);
                return;
            }
            //是否为禁止学习的技能
            /*if (pet.key == '1003' && goods.key == '3127')
            {
                msgCodeEvent.matchMsgCode({ code: 777 });
                return;
            }*/
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", sklKey);
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/learnSkill", dic, (res) =>
            {
                JObject obj = (JObject)res;
                face.goodsInterface.cutPlayerGoodsNumByKey(sklKey, 1);
                if ((int)obj["sure"] == 1)
                {
                    //弹出回天书的询问框
                    callback(obj);
                    return;
                }
                JObject skl = (JObject)skls[(int)obj["index"]];
                skl["key"] = sklKey;
                skl["lv"] = 1;
                face.petInterface.savePet(pet);

                msgCode.showMsg(636);
                callback(null);

            });

        }
        public JObject getSkl(int type, int index, JArray skls)
        {
            for (int i = 0; i < skls.Count; i++)
            {
                JObject a = (JObject)skls[i];
                if ((a.ContainsKey("type") && (int)a["type"] == type) &&
                    (a.ContainsKey("index") && (int)a["index"] == index)) return a;
            }
            return null;
        }
        public JObject getSkl(string key, JArray skls)
        {
            for (int i = 0; i < skls.Count; i++)
            {
                JObject a = (JObject)skls[i];
                if (a.ContainsKey("key") && a["key"].ToString().Equals(key)) return a;
            }
            return null;
        }
        /**学习人物技能 */
        public void learnSkill(string key, Action<JObject> callback)
        {
            JObject role = face.roleInterface.getRole();
            JArray list = (JArray)role["attr"]["skill"];
            Dictionary<string, object> dic = null;
            Skill skill = (Skill)face.goodsInterface.getGoodsMsgByKey(key);
            //0门派、1天技（4级）、2地技（4级）允许升级 3人技不允许升级 4宠物专属 5宠物普通 6经脉 7装备技能 8武将技能 9生活技能
            if (skill.skillType == 0)
            {
                //如果是门派初始技能，需要先将技能这个key转换成对应的职业技能
                key = GameAttrConst.mpSklToJobSkl(role, key);
                skill = (Skill)face.goodsInterface.getGoodsMsgByKey(key);
                for (int p = 0; p < list.Count; p++)
                {
                    JObject skl = (JObject)list[p];
                    if (skl.ContainsKey("key") && skl["key"].ToString().Equals(key))
                    {
                        //等级是否达到
                        if ((int)role["lever"] < ((int)skl["lv"] + 1) * 4 + 10)
                        {
                            msgCode.showMsg(613);
                            return;
                        }
                        //经验是否足够
                        if ((int)role["attr"]["prop"]["exp"] < 150 * Math.Pow((int)skl["lv"] + 1, 4))
                        {
                            msgCode.showMsg(682);
                            return;
                        }
                        //银票是否足够
                        if ((int)role["attr"]["msg"]["yp"] < 100 * Math.Pow((int)skl["lv"] + 1, 4))
                        {
                            msgCode.showMsg(634);
                            return;
                        }
                        //是否超过最大等级
                        if (skill.isOverLimitLv((int)skl["lv"]))
                        {
                            msgCode.showMsg(656);
                            return;
                        }
                        dic = new Dictionary<string, object>();
                        dic.Add("key", key);
                        DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                        {
                            if (int.Parse(res.ToString()) != 1)
                            {
                                msgCode.showMsg(651);
                                return;
                            }
                            //减去exp、yp
                            role["attr"]["prop"]["exp"] = (int)role["attr"]["prop"]["exp"] - (int)(150 * Math.Pow((int)list[p]["lv"] + 1, 4));
                            face.roleInterface.saveRole(role);
                            face.roleInterface.updateMoney((int)(-100 * Math.Pow((int)list[p]["lv"] + 1, 4)), 2);
                            list[p]["lv"] = (int)list[p]["lv"] + 1;
                            face.roleInterface.updateRoleSkill((JObject)list[p]);
                            msgCode.showMsg(636);
                            callback(null);
                        });

                        return;
                    }
                }
                if ((int)role["lever"] < (1) * 4 + 10)
                {
                    msgCode.showMsg(613);
                    return;
                }
                //经验是否足够
                if ((int)role["attr"]["prop"]["exp"] < 150 * Math.Pow(1, 4))
                {
                    msgCode.showMsg(682);
                    return;
                }
                //银票是否足够
                if ((long)role["attr"]["msg"]["yp"] < 100 * Math.Pow(1, 4))
                {
                    msgCode.showMsg(634);
                    return;
                }
                dic = new Dictionary<string, object>();
                dic.Add("key", key);
                DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                {
                    if (int.Parse(res.ToString()) != 1)
                    {
                        msgCode.showMsg(651);
                        return;
                    }
                    //减去exp、yp
                    role["attr"]["prop"]["exp"] = (int)role["attr"]["prop"]["exp"] - (int)(150 * Math.Pow(1, 4));
                    face.roleInterface.saveRole(role);
                    face.roleInterface.updateMoney((int)(-100 * Math.Pow(1, 4)), 2);
                    JObject item = new JObject();
                    item.Add("key", key);
                    item.Add("lv", 1);
                    face.roleInterface.updateRoleSkill(item);
                    msgCode.showMsg(636);
                    callback(null);
                });


            }
            else if (skill.skillType == 1 || skill.skillType == 2)
            {
                //获取槽位
                JObject item = getSkl(skill.skillType, 0, list);
                //是否开启了槽位
                if (item == null)
                {
                    return;
                }
                //没有打技能的情况
                if (!item.ContainsKey("key"))
                {
                    dic = new Dictionary<string, object>();
                    dic.Add("key", key);
                    DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                    {
                        face.goodsInterface.cutPlayerGoodsNumByKey(key, 1);

                        item.Add("key", key);
                        item.Add("lv", 1);
                        face.roleInterface.saveRole(role);
                        msgCode.showMsg(636);
                        callback(null);
                    });
                }
                else
                {
                    //打了技能但技能的key跟要打的key不一致时
                    if (!item["key"].ToString().Equals(key))
                    {
                        msgCode.showMsg(608);
                        return;
                    }
                    if (!face.goodsInterface.isEnoughInPackAndTip(key, (int)item["lv"] * 2))
                    {
                        return;
                    }
                    //数量不足或已经达到最大等级
                    if ((int)item["lv"] >= 4)
                    {
                        msgCode.showMsg(656);
                        return;
                    }
                    dic = new Dictionary<string, object>();
                    dic.Add("key", key);
                    DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                    {
                        if (int.Parse(res.ToString()) != 1)
                        {
                            msgCode.showMsg(651);
                            return;
                        }
                        face.goodsInterface.cutPlayerGoodsNumByKey(key, (int)item["lv"] * 2);
                        item["lv"] = (int)item["lv"] + 1;
                        face.roleInterface.saveRole(role);
                        msgCode.showMsg(636);
                        callback(null);
                    });
                }
            }

            else if (skill.skillType == 3)
            {
                //验证回天书是否在背包
                if (face.goodsInterface.keyIsInCangKu("10000128"))
                {
                    msgCode.showMsg(228);
                    return;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    if (a.ContainsKey("key") && a["key"].ToString().Equals(key))
                    {
                        msgCode.showMsg(635);
                        return;
                    }
                }
                if (!face.goodsInterface.isEnoughInPackAndTip(key, 1))
                {
                    return;
                }
                dic = new Dictionary<string, object>();
                dic.Add("key", key);
                DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                {
                    JObject obj = (JObject)res;
                    face.goodsInterface.cutPlayerGoodsNumByKey(key, 1);
                    if ((int)obj["sure"] == 1)
                    {
                        //弹出回天书的询问框
                        callback(obj);
                        return;
                    }
                    JObject item = getSkl(3, (int)obj["index"], list);
                    item.Add("key", key);
                    item.Add("lv", 1);
                    face.roleInterface.saveRole(role);
                    msgCode.showMsg(636);
                    callback(null);
                });
            }
            else if (skill.skillType == 6)
            {
                bool b = false;
                int lv = 0;
                int index = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    JObject skl = (JObject)list[i];
                    if (skl.ContainsKey("key") && skl["key"].ToString().Equals(key))
                    {
                        b = true;
                        lv = (int)list[i]["lv"];
                        index = i;
                        break;
                    }
                }
                if (lv >= 15)
                {
                    msgCode.showMsg(656);
                    return;
                }
                int jmPoint = (int)role["attr"]["msg"]["jmPoint"];
                int[] xhn = { 20, 20, 40, 40, 40, 60, 60, 60, 80, 80, 80, 100, 100, 100, 100 };
                int xh = xhn[lv];
                if (jmPoint - xh < 0)
                {
                    msgCode.showMsg(703);
                    return;
                }
                dic = new Dictionary<string, object>();
                dic.Add("key", key);
                DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                {
                    if (int.Parse(res.ToString()) == 0)
                    {
                        msgCode.showMsg(703);
                    }
                    else
                    {
                        role["attr"]["msg"]["jmPoint"] = jmPoint - xh;
                        face.roleInterface.saveRole(role);
                        JObject item = new JObject();
                        item.Add("key", key);
                        item.Add("lv", lv + 1);
                        face.roleInterface.updateRoleSkill(item);
                        msgCode.showMsg(704);
                        callback(null);
                    }
                });
            }
            else if (skill.skillType == 9)
            {
                //找到这个技能
                JObject item = getSkl(key, list);
                if (item == null)
                {
                    //未学习的情况
                    item = new JObject();
                    item.Add("key", key);
                    item.Add("lv", 0);
                }
                int lv = (int)item["lv"];
                int exp = 3000 + 5000 * lv;
                int tale = 1100 + 300 * lv;
                int bg = 0;
                if (lv >= 60) bg = lv * 100;
                if (lv >= 100)
                {
                    msgCode.showMsg(625);
                    return;
                }
                if (!face.roleInterface.isEnoughMoney("tale", -tale))
                {
                    msgCode.showMsg(634);
                    return;
                }
                if ((int)role["attr"]["prop"]["exp"] - exp < 0)
                {
                    msgCode.showMsg(682);
                    return;
                }
                dic = new Dictionary<string, object>();
                dic.Add("key", key);
                DoGet.getInstance().sendPost("/manService/learnSkill", dic, (res) =>
                {
                    role["attr"]["prop"]["exp"] = (int)role["attr"]["prop"]["exp"] - exp;
                    face.roleInterface.saveRole(role);
                    face.roleInterface.updateMoney(-tale, 1);
                    item["lv"] = lv + 1;
                    face.roleInterface.updateRoleSkill(item);
                    msgCode.showMsg(200);
                    callback(null);
                });
            }
        }
        /**确认技能是否被覆盖*/
        public void sureSkillCover(int sure, int roleType, string key, int index, Action callback)
        {
            string xhKey = null;
            if (roleType == 0) xhKey = "10000128";
            else xhKey = "10000120";
            if (sure ==0)
            {
                //点击取消则需要判断回天书是否足够
                if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, 1, true))
                {
                    //不够则自动设置为确认覆盖
                    sure = 1;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sure", sure);
            DoGet.getInstance().sendPost("/manService/sureSkillCover", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == -1)
                {
                    msgCode.showMsg(949);
                }
                else if (int.Parse(res.ToString()) == 0)
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 1);
                    msgCode.showMsg(665);
                }
                else if (int.Parse(res.ToString()) == 1)
                {
                    if (roleType == 0)
                    {
                        JObject role = face.roleInterface.getRole();
                        JArray skls = (JArray)role["attr"]["skill"];
                        JObject item = getSkl(3, index, skls);
                        Debug.Log(item["key"] + "====>" + key);
                        item["key"] = key;
                        item["lv"] = 1;
                        face.roleInterface.saveRole(role);
                        msgCode.showMsg(636);
                    }
                    else
                    {
                        //宠物

                    }

                }
                callback();
            });
        }
        /**确认技能是否被覆盖*/
        public void surePetSkillCover(string petId, int sure, int roleType, string key, int index, Action callback)
        {
            string xhKey = null;
            if (roleType == 0) xhKey = "10000128";
            else xhKey = "10000120";
            if (sure == 0)
            {
                //点击取消则需要判断回天书是否足够
                if (!face.goodsInterface.isEnoughInPackAndTip(xhKey, 1, true))
                {
                    //不够则自动设置为确认覆盖
                    sure = 1;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sure", sure);
            DoGet.getInstance().sendPost("/petService/sureSkillCover", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == -1)
                {
                    msgCode.showMsg(949);
                }
                else if (int.Parse(res.ToString()) == 0)
                {
                    
                    face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 1);
                    msgCode.showMsg(665);
                }
                else if (int.Parse(res.ToString()) == 1)
                {
                    if (roleType == 0)
                    {

                    }
                    else
                    {
                        //宠物
                        JObject pet = face.petInterface.getOneById(petId);
                        JArray skls = (JArray)pet["attr"]["skill"];
                        JObject skl = (JObject)skls[index];
                        skl["key"] = key;
                        skl["lv"] = 1;
                        face.petInterface.savePet(pet);
                        msgCode.showMsg(636);
                    }

                }
                callback();
            });
        }
        /**获取可学习的宠物技能
         skls传入当前宠物学过的技能
         */
        public JArray getPetSkillListToPanel(JArray skls)
        {
            //先从背包获取所有技能道具
            JArray list = face.goodsInterface.getAllGoods();
            JArray arr = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                //判断是否为技能
                int n = int.Parse(((JObject)list[i])["key"].ToString());
                if (n < 3045 || n > 3499) continue;
                Skill g = (Skill)face.goodsInterface.getGoodsMsgByKey(n.ToString());
                if (g == null) continue;
                int skillType = g.skillType;
                if (skillType != 5) continue;

                JObject skl = new JObject();
                skl.Add("key", g.key.ToString());
                skl.Add("isLearn", 0);
                skl.Add("lv", 0);
                skl.Add("skillType", skillType);
                int b = 0;
                foreach (object a in skls)
                {
                    JObject o = (JObject)a;
                    if (o["key"] == null) continue;
                    if (g.key.ToString().Equals(o["key"].ToString()))
                    {
                        b = 1;
                        break;
                    }
                }

                if (b != 0)
                {
                    //已经学习过了
                    continue;
                }
                arr.Add(skl);
            }
            /*JArray arr = new JArray();
            for (int i = 3045; i < 3500; i++)
            {
                skill g = this.getSkillByKey(i.ToString());
                if (g == null) continue;
                int skillType = g.skillType;
                if (skillType != 5) continue;

                JObject skl = new JObject();
                skl.Add("key", g.key.ToString());
                skl.Add("isLearn", 0);
                skl.Add("lv", 0);
                skl.Add("skillType", skillType);
                int b = 0;
                foreach (object a in skls)
                {
                    JObject o = (JObject)a;
                    if (o["key"] == null) continue;
                    if (g.key.ToString().Equals(o["key"].ToString()))
                    {
                        b = 1;
                        break;
                    }
                }

                if (b != 0)
                {
                    //已经学习过了
                    continue;
                }
                //背包中没有
                JObject pg = face.goodsInterface.getPlayerGoodsByKey(g.key);
                if (pg == null) continue;

                arr.Add(skl);
            }*/
            return arr;
        }
        /**获取可学习的人物技能(技能面板)*/
        public JArray getManSkillListToPanel()
        {
            JObject role = face.roleInterface.getRole();
            string job = role["model"].ToString().Split('_')[0];
            JArray skls = (JArray)role["attr"]["skill"];
            JArray arr = new JArray();
            for (int i = 3000; i < 3600; i++)
            {
                Skill g = (Skill)face.goodsInterface.getGoodsMsgByKey(i.ToString());
                if (g == null) continue;
                int skillType = g.skillType;
                if (skillType == 0 || skillType == 1 || skillType == 2 || skillType == 3 || skillType == 6)
                {
                    //非本职业的滤除
                    if (skillType == 0 && !g.job.ToString().Contains(job))
                    {
                        continue;
                    }
                    JObject skl = new JObject();
                    skl.Add("key", g.key.ToString());
                    skl.Add("isLearn", 0);
                    skl.Add("lv", 0);
                    skl.Add("skillType", skillType);

                    int b = 0;
                    foreach (object a in skls)
                    {
                        JObject o = (JObject)a;
                        if (g.key.ToString().Equals(o["key"].ToString()))
                        {
                            if (skillType == 0 && g.isOverLimitLv((int)o["lv"]))
                            {
                                b = 2;//达到最大等级
                            }
                            else if (skillType == 6 && (int)o["lv"] >= 20)
                            {
                                b = 2;//达到最大等级
                            }
                            else if ((skillType == 1 || skillType == 2) && (int)o["lv"] >= 4)
                            {
                                b = 2;//达到最大等级
                            }
                            else
                            {
                                b = 1;//已经学习可以升级
                            }
                            skl.Remove("lv");
                            skl.Add("lv", (int)o["lv"]);
                            break;
                        }
                    }

                    if (b != 0)
                    {
                        //已经学习过了
                        skl.Remove("isLearn");
                        skl.Add("isLearn", b);
                    }


                    //人技学过就不用再放入
                    if (skillType == 3 && b == 1) continue;
                    //过滤背包中没有的
                    if (skillType == 1 || skillType == 2 || skillType == 3)
                    {
                        JObject pg = face.goodsInterface.getPlayerGoodsByKey(g.key);
                        if (pg == null) continue;
                    }
                    if (b != 2)
                    {
                        arr.Add(skl);
                    }
                }
            }
            return arr;
        }
        /**获取伙伴技能*/
        public JArray getHuobanSkillList(string petKey, string[] filterList)
        {
            JArray arr = new JArray();
            for (int i = 3045; i < 3185; i++)
            {
                Skill g = (Skill)face.goodsInterface.getGoodsMsgByKey(i.ToString());
                if (g == null) continue;
                int skillType = g.skillType;
                if (skillType == 4 || skillType == 5)
                {
                    //判断这个专属技能是不是适合
                    if (skillType == 4 && !g.job.Equals(petKey))
                    {
                        continue;
                    }
                    bool b = false;
                    foreach (string p in filterList)
                    {
                        if (p.Equals(g.key))
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b)
                    {
                        continue;
                    }
                    arr.Add(g);
                }
            }
            return arr;
        }
        /**获取宠物技能列表 */
        public JArray getPetSkillList(string petKey, string[] filterList)
        {
            JArray arr = new JArray();
            for (int i = 3045; i < 3185; i++)
            {
                Skill g = (Skill)face.goodsInterface.getGoodsMsgByKey(i.ToString());
                if (g == null) continue;
                int skillType = g.skillType;
                if (skillType == 4 || skillType == 5)
                {
                    //判断这个专属技能是不是适合
                    if (skillType == 4 && !g.job.Equals(petKey))
                    {
                        continue;
                    }
                    bool b = false;
                    foreach (string p in filterList)
                    {
                        if (p.Equals(g.key))
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b)
                    {
                        continue;
                    }
                    arr.Add(g);
                }
            }
            return arr;
        }

    }
}
