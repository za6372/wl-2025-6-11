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
    public interface petInterface
    {
        public JArray getFuPetList(string mainPeId);
        public JArray getPetList();
        public void initPetList(JArray petList);
        public Pet getPetDataByKey(string key);
        public JObject getIsFightPet();
        public void addExp(JObject pet, int exp);
        public void savePet(JObject pet);
        public bool isOverNumLimit();
        public List<Pet> getAllPetList();
        public JObject getOneById(string Id);
        public void updatePetSklCache(string petId, int index, string sklKey);
        public void addCao(string petId, Action callback);
        public void updatePetPropPointAndBaseAttr(int propPoint, JObject baseProp, string Id, Action callback);
        public void updatePetXueById(JObject attrMap, string Id);
        public void abandonPet(string Id, Action callback);
        public void updateIsFightById(int isFight, string Id, Action callback);
        public void getPlayerPet(string name, Action<JArray> callback);
        public void getPlayerPetById(string Id, string name, Action<JArray> callback);
        public void reZizhi(string petId, Action callback);
        public void reLive(string Id, int type, Action ac);
        public void addSkillToPet(JObject pet, JArray skls);
        public void remFromCache(string Id);
        public JObject getRealQualityGround(JObject zizhi, int quality);
        public void setPetName(string petId, string petName, Action callback);
        public JObject getPetByIdFromCache(string Id);
        public void eatDan(string petId, string danKey, int times, Action callback);
        public void resetAttrPoint(string petId, Action callback);
        public void eatPet(string petId, string fuId, Action callback);
        public void petQiangHua(string petId, Action callback);
        public Pet getPetByPrefabCode(string prefabCode);
        public void breach(string petId, Action callback);
        public void evolution(string petId, Action callback);
        public void eatPetDan(string petId, string danKey, Action callback);
        public void eatPetNzs(string petId, string danKey, string attrKey, Action callback);
        public void eatPetWxd(string petId, string danKey, Action callback);
        public void resetPoint(string petId, Action callback);
        public void fangshengPet(string petId, Action callback);
        public void lianhuaPet(string petId, Action callback);
        public bool isWaiXianPet(string key);
        public int getMaxPetDanAttr(string key, int lv);
        public JArray getEnclosure();
        public void forgetPetSkill(string petId, string sklKey, Action ac);
        public int getQiangHuaZhi(int growLv);

    }
    public class petInterfaceImpl : petInterface
    {

        public bool isAllowedQiangHua(int growLv)
        {
            //v5允许成6 v6允许成7\8 v7允许成9 v8允许成10
            int vipLv = face.roleInterface.getVipLv();
            if (vipLv == 0) return false;
            //成5以上进行强化则需要达到v5
            if (vipLv == 1 && growLv > 1) return false;
            else if (vipLv == 2 && growLv > 2) return false;
            else if (vipLv == 3 && growLv > 3) return false;
            else if (vipLv == 4 && growLv > 4) return false;
            else if (vipLv == 5 && growLv > 5) return false;
            else if (vipLv == 6 && growLv > 7) return false;
            else if (vipLv == 7 && growLv > 8) return false;
            return true;
        }
        public int getQiangHuaZhi(int growLv)
        {
            int vipLv = face.roleInterface.getVipLv();

            if (vipLv == 0) return 0;
            if (growLv < 5)
            {
                return 200;
            }
            else if (growLv == 5)
            {//成5的 v7以上一次增加200成长值，以下增加100
                if (vipLv < 7) return 100;
                return 200;
            }
            else if (growLv == 6) return 50;
            else if (growLv == 7) return 25;
            else if (growLv == 8) return 15;
            else if (growLv == 9) return 10;
            else if (growLv == 10) return 0;
            return 0;
        }
        /**遗忘技能*/
        public void forgetPetSkill(string petId, string sklKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000165", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            dic.Add("key", sklKey);
            DoGet.getInstance().sendPost("/petService/forgetPetSkill", dic, (res) =>
            {
                JObject pet = getOneById(petId);
                JArray skls = (JArray)pet["attr"]["skill"];
                for (int i = 0; i < skls.Count; i++)
                {
                    JObject skl = (JObject)skls[i];
                    if (!skl.ContainsKey("key") || !skl["key"].ToString().Equals(sklKey)) continue;
                    skl.Remove("key");
                    skl.Remove("lv");
                    break;
                }
                savePet(pet);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**是否宠物外显*/
        public bool isWaiXianPet(string key)
        {
            if (int.Parse(key) >= 1215 && int.Parse(key) < 1229)
            {
                return true;
            }
            return false;
        }
        /**炼化*/
        public void lianhuaPet(string petId, Action callback)
        {
            JObject pet = getOneById(petId);
            if ((int)pet["lever"] < 40)
            {
                msgCode.showMsg(613);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/lianhuaPet", dic, (res) =>
            {
                this.remFromCache(petId);
                //获得口粮
                face.rewardInterface.saveRewards((JArray)res);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**放生*/
        public void fangshengPet(string petId, Action callback)
        {
            JObject pet = getOneById(petId);
            if ((int)pet["lever"] < 40)
            {
                msgCode.showMsg(613);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/fangshengPet", dic, (res) =>
            {
                this.remFromCache(petId);
                //增加人气值
                JObject role = face.roleInterface.getRole();
                role["attr"]["msg"]["sez"] = (int)role["attr"]["msg"]["sez"] + int.Parse(res.ToString());
                face.roleInterface.saveRole(role);
                msgCode.showMsg(964, res);
                callback();
            });
        }
        /**洗点，属性重置*/
        public void resetPoint(string petId, Action callback)
        {
            JObject pet = getOneById(petId);
            if ((int)pet["lever"] >= 50)
            {
                if (!face.goodsInterface.isEnoughInPackAndTip("10000171", 1))
                {
                    return;
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/resetPoint", dic, (res) =>
            {
                if ((int)pet["lever"] >= 50)
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey("10000171", 1);
                }
                int lv = (int)pet["lever"];
                int oldPropPoint = (int)pet["propPoint"];
                int sum = 0;//累计转出属性
                JObject baseProp = (JObject)pet["baseProp"];
                foreach (JProperty p in baseProp.Properties())
                {
                    if ((int)baseProp[p.Name] > 5 + (lv - 1))
                    {
                        sum += ((int)baseProp[p.Name] - (5 + (lv - 1)));
                        baseProp[p.Name] = 5 + (lv - 1);
                    }
                }
                int point = oldPropPoint + sum;
                /*if ((int)pet["growBreachLv"] >= 1)
                {//突破则多50
                    point += 50;
                }*/
                pet["propPoint"] = point;
                this.savePet(pet);
                msgCode.showMsg(200);
                callback();
            });
        }
        /**吃悟性丹*/
        public void eatPetWxd(string petId, string danKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(danKey, 1))
            {
                return;
            }
            JObject pet = getOneById(petId);
            int savvy = (int)pet["savvy"];
            if (savvy >= 200 || (savvy >= 100 && danKey.Equals("10000130")) ||
                    (savvy >= 150 && danKey.Equals("10000118")))
            {
                msgCode.showMsg(961);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            dic.Add("danKey", danKey);
            DoGet.getInstance().sendPost("/petService/eatPetWxd", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(danKey, 1);
                pet["savvy"] = (int)pet["savvy"] + int.Parse(res.ToString());
                this.savePet(pet);
                msgCode.showMsg(962, res);
                callback();

            });
        }
        public void eatPetNzs(string petId, string danKey, string attrKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(danKey, 1))
            {
                return;
            }
            JObject pet = getOneById(petId);
            JObject baseProp = (JObject)pet["baseProp"];
            if ((int)baseProp[attrKey] <= 5)
            {
                msgCode.showMsg(960);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            dic.Add("danKey", danKey);
            dic.Add("attrKey", attrKey);
            DoGet.getInstance().sendPost("/petService/eatPetNzs", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(danKey, 1);
                int num = 0;
                if (danKey.Equals("10000168")) num = 1;
                else if (danKey.Equals("10000169")) num = 2;
                else num = 3;
                int sy = (int)baseProp[attrKey] - num;
                baseProp[attrKey] = sy;
                if (sy < 5) baseProp[attrKey] = 5;
                int point = num - (sy < 5 ? (5 - sy) : 0);
                pet["propPoint"] = (int)pet["propPoint"] + point;
                savePet(pet);
                msgCode.showMsg(200);
                callback();
            });

        }
        public int getMaxPetDanAttr(string key, int lv)
        {
            string[] attrs = { "bj", "sd", "mz", "max_lan", "max_xue", "ff", "wf", "fg", "wg", "css", };
            int[] nums = { 50, 100, 200 };
            int type = 0;
            for (int i = 0; i < attrs.Length; i++)
            {
                if (attrs[i].Equals(key))
                {
                    type = i; break;
                }
            }
            int[] arr = { 6, 3, 5, 67, 67, 4, 4, 8, 8, 2, };
            return arr[type] * nums[lv - 1] * lv;
        }
        public void eatPetDan(string petId, string danKey, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(danKey, 1))
            {
                return;
            }
            JObject pet = getOneById(petId);
            //是否吃满了
            JObject danAttr = (JObject)pet["danAttr"];
            int type = int.Parse(danKey.Substring(4).ToCharArray()[3] + "");
            int lv = int.Parse(danKey.Substring(4).ToCharArray()[2] + "") + 1;
            PetDan petDan = (PetDan)face.goodsInterface.getGoodsMsgByKey(danKey);
            int p = petDan.prop;
            string[] attrs = { "bj", "sd", "mz", "max_lan", "max_xue", "ff", "wf", "fg", "wg", "css", };
            string attrKey = attrs[type];
            if (lv == 1)
            {//固元丹
                JObject d1 = (JObject)danAttr["d1"];
                if ((int)d1[attrKey] >= 50 * p)
                {
                    msgCode.showMsg(959);
                    return;
                }
            }
            else if (lv == 2)
            {
                JObject d2 = (JObject)danAttr["d2"];
                if ((int)d2[attrKey] >= 100 * p)
                {
                    msgCode.showMsg(959);
                    return;
                }
            }
            else if (lv == 3)
            {
                JObject d3 = (JObject)danAttr["d3"];
                if ((int)d3[attrKey] >= 200 * p)
                {
                    msgCode.showMsg(959);
                    return;
                }
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            dic.Add("danKey", danKey);
            DoGet.getInstance().sendPost("/petService/eatPetDan", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(danKey, 1);
                if (lv == 1)
                {//固元丹
                    JObject d1 = (JObject)danAttr["d1"];
                    d1[attrKey] = (int)d1[attrKey] + p;
                    if ((int)d1[attrKey] >= 50 * p)
                    {
                        d1[attrKey] = 50 * p;
                    }
                }
                else if (lv == 2)
                {
                    JObject d2 = (JObject)danAttr["d2"];
                    d2[attrKey] = (int)d2[attrKey] + p;
                    if ((int)d2[attrKey] >= 100 * p)
                    {
                        d2[attrKey] = 100 * p;
                    }
                }
                else if (lv == 3)
                {
                    JObject d3 = (JObject)danAttr["d3"];
                    d3[attrKey] = (int)d3[attrKey] + p;
                    if ((int)d3[attrKey] >= 200 * p)
                    {
                        d3[attrKey] = 200 * p;
                    }
                }
                savePet(pet);
                msgCode.showMsg(200);
                callback();
            });
        }
        public void evolution(string petId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000121", 1))
            {
                return;
            }
            JObject pet = getOneById(petId);
            if ((int)pet["lever"] < 60 || (int)pet["growLv"] != 8 || (int)pet["growBreachLv"] != 1)
            {
                msgCode.showMsg(746);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/evolution", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000121", 1);
                pet["growBreachLv"] = 2;
                JObject pos = new JObject();
                pos["isOpen"] = 1;
                JArray skls = (JArray)pet["attr"]["skill"];
                skls.Add(pos);
                savePet(pet);
                msgCode.showMsg(748);
                callback();
            });
        }
        public void breach(string petId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000122", 5))
            {
                return;
            }
            JObject pet = getOneById(petId);
            if ((int)pet["lever"] < 60 || (int)pet["growLv"] != 6 || (int)pet["growBreachLv"] != 0)
            {
                msgCode.showMsg(745);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/breach", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000122", 5);
                pet["growBreachLv"] = 1;
                pet["propPoint"] = (int)pet["propPoint"] + 50;
                savePet(pet);
                msgCode.showMsg(747);
                callback();
            });
        }
        /**宠物强化*/
        public void petQiangHua(string petId, Action callback)
        {
            JObject mainPet = getOneById(petId);
            if (!this.isAllowedQiangHua((int)mainPet["growLv"]))
            {
                msgCode.showMsg(212);
                return;
            }
            if ((int)mainPet["lever"] < 60)
            {
                msgCode.showMsg(825);
                return;
            }
            int growLv1 = (int)mainPet["growLv"];
            int growBreachLv1 = (int)mainPet["growBreachLv"];
            if (growLv1 >= 10)
            {
                msgCode.showMsg(740);
                return;
            }
            if (growLv1 >= 6 && growBreachLv1 < 1)
            {
                msgCode.showMsg(742);
                return;
            }
            if (growLv1 >= 8 && growBreachLv1 < 2)
            {
                msgCode.showMsg(743);
                return;
            }
            if (!face.roleInterface.isEnoughMoney("gold", -500))
            {
                msgCode.showMsg(634);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("petId", petId);
            DoGet.getInstance().sendPost("/petService/petQiangHua", dic, (res) =>
            {
                face.roleInterface.updateMoney(-500, 0);
                JObject a = (JObject)res;
                string str = "";
                if (a["growValue"] != null)
                {
                    mainPet["growValue"] = a["growValue"];
                    str = "成长值增加 " + a["growValue"] + "/1000";
                }
                if (a["growLv"] != null)
                {
                    mainPet["growLv"] = a["growLv"];
                    str = "成长等级提升 " + a["growLv"] + "/10\n";
                }
                if (a["qualityValue"] != null)
                {
                    mainPet["qualityValue"] = a["qualityValue"];
                    str += "资质提升\n";
                    JObject qualityValue = (JObject)a["qualityValue"];
                    foreach (JProperty p in qualityValue.Properties())
                    {
                        str += GameAttrConst.propKeyToName(p.Name.ToString()) + " " + p.Value + "\n";
                    }
                }
                msgCode.showMsg(201, str);

                savePet(mainPet);
                callback();
            });

        }
        public void eatPet(string petId, string fuId, Action callback)
        {
            JObject mainPet = getOneById(petId);
            JObject fuPet = getOneById(fuId);
            //主宠最低60级，副宠最低40级
            if ((int)mainPet["lever"] < 60 || (int)fuPet["lever"] < 40)
            {
                msgCode.showMsg(825);
                return;
            }
            int growLv1 = (int)mainPet["growLv"];
            if (growLv1 >= 5)
            {
                //成5以上需要vip
                if (!this.isAllowedQiangHua((int)mainPet["growLv"]))
                {
                    msgCode.showMsg(212);
                    return;
                }
            }
            int growLv2 = (int)fuPet["growLv"];
            int growBreachLv1 = (int)mainPet["growBreachLv"];
            if (growLv1 >= 10)
            {
                msgCode.showMsg(740);
                return;
            }
            if (growLv1 - growLv2 > 2)
            {
                msgCode.showMsg(741);
                return;
            }
            if (growLv1 >= 6 && growBreachLv1 < 1)
            {
                msgCode.showMsg(742);
                return;
            }
            if (growLv1 >= 8 && growBreachLv1 < 2)
            {
                msgCode.showMsg(743);
                return;
            }
            if ((int)mainPet["eatPet"] >= 10)
            {
                msgCode.showMsg(826);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("fuId", fuId);
            dic.Add("petId", petId);
            DoGet.getInstance().sendPost("/petService/eatPet", dic, (res) =>
            {
                JObject a = (JObject)res;

                //吞噬同等级获得200成长度，吞噬高1等级的就+500，2等级以上的就+1k
                /* int value = 0;
                 if (growLv2 - growLv1 == 0)
                 {
                     value = 200;
                 }
                 else if (growLv2 - growLv1 == 1)
                 {
                     value = 500;
                 }
                 else if (growLv2 - growLv1 >= 2)
                 {
                     value = 1000;
                 }
                 else if (growLv2 - growLv1 == -1)
                 {
                     value = 10;
                 }
                 else if (growLv2 - growLv1 == -2)
                 {
                     value = 1;
                 }
                 mainPet["growValue"] = (int)mainPet["growValue"] + value;
                 if ((int)mainPet["growValue"] >= 1000)
                 {
                     mainPet["growValue"] = 0;
                     mainPet["growLv"] = growLv1 + 1;
                     msgCode.showMsg(958);
                 }
                 else
                 {
                     msgCode.showMsg(957, value);
                 }*/
                string str = "";
                if (a["growValue"] != null)
                {
                    mainPet["growValue"] = a["growValue"];
                    str = "成长值增加 " + a["growValue"] + "/1000";
                }
                if (a["growLv"] != null)
                {
                    mainPet["growLv"] = a["growLv"];
                    str = "成长等级提升 " + a["growLv"] + "/10\n";
                }
                if (a["qualityValue"] != null)
                {
                    mainPet["qualityValue"] = a["qualityValue"];
                    str += "资质提升\n";
                    JObject qualityValue = (JObject)a["qualityValue"];
                    foreach (JProperty p in qualityValue.Properties())
                    {
                        str += GameAttrConst.propKeyToName(p.Name.ToString()) + " " + p.Value + "\n";
                    }
                }
                msgCode.showMsg(201, str);

                savePet(mainPet);
                this.remFromCache(fuId);
                callback();
            });
        }

        private int countExpFromALvToBLv(int aLv, int bLv)
        {
            if (aLv >= bLv) return 0;
            int max_exp = 0;
            for (int i = aLv; i <= bLv; i++)
            {
                double jy = i * 100 + 2 * Math.Pow(5, 0.1 * i);
                max_exp += (int)jy;
            }
            return max_exp;
        }
        public void eatDan(string petId, string danKey, int times, Action callback)
        {
            JObject pet = this.getPetByIdFromCache(petId);

            if ((int)pet["lever"] - (int)face.roleInterface.getRole()["lever"] > 10 || (int)pet["lever"] >= 110)
            {
                msgCode.showMsg(631);
                return;
            }
            int jy = 1000;
            if (danKey.Equals("10000030")) jy = 10000;
            else if (danKey.Equals("10000031")) jy = 50000;
            else if (danKey.Equals("10000032")) jy = 200000;
            JObject dan = face.goodsInterface.getPlayerGoodsByKey(danKey);
            if (dan == null)
            {
                msgCode.showMsg(637);
                return;
            }
            int sum = (int)dan["num"];
            int n = 0;
            if (times == -1)
            {//一键升级
                int roleLv = (int)face.roleInterface.getRole()["lever"];
                int petLv = (int)pet["lever"];
                //角色等级+10为最大等级，计算宠物等级与最大等级的差距，计算所需经验
                int exp = countExpFromALvToBLv(petLv, roleLv + 10);
                if (exp == 0)
                {
                    msgCode.showMsg(631);
                    return;
                }
                int nowExp = (int)pet["attr"]["prop"]["exp"];
                exp = exp - nowExp;
                //所需经验/jy=所需口粮数量，判断所需口粮数量是否超过拥有的口粮数
                int num = exp / jy == 0 ? (exp / jy) : (exp / jy + 1);
                //超过则全部使用，小于则拥有的减去所需的
                if (sum - num >= 0) n = num;
                else n = sum;
            }
            else
            {//使用1次
                n = 1;
            }

            if (!face.goodsInterface.isEnoughInPackAndTip(danKey, n))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("danKey", danKey);
            dic.Add("petId", petId);
            dic.Add("times", times);
            DoGet.getInstance().sendPost("/petService/eatDan", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey(danKey, n);
                this.addExp(pet, jy * n);
                msgCode.showMsg(629);
                //遍历返回的领悟技能
                this.addSkillToPet(pet, (JArray)res);

                callback();
            });
        }
        public JObject getPetByIdFromCache(string Id)
        {
            JArray list = this.getPetList();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString().Equals(Id))
                {
                    JObject pet = (JObject)list[p];
                    return pet;
                }
            }
            return null;
        }
        public void setPetName(string petId, string petName, Action callback)
        {
            if (strUtils.isNull(petName))
            {
                msgCode.showMsg(618);
                return;
            }
            //是否存在改名卡
            if (!face.goodsInterface.isEnoughInPackAndTip("10000004", 1))
            {
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("petName", petName);
            dic.Add("petId", petId);
            DoGet.getInstance().sendPost("/petService/setPetName", dic, (res) =>
            {
                JObject pet = this.getPetByIdFromCache(petId);
                pet["nickName"] = petName;
                this.savePet(pet);

                callback();
            });


        }
        /**获取资质范围,这个品质会影响资质上限*/
        public JObject getRealQualityGround(JObject zizhi, int quality)
        {
            JObject res = new JObject();
            //根据品质来生成最大最小资质
            IEnumerable<JProperty> ps = zizhi.Properties();
            foreach (JProperty p in ps)
            {
                string k = p.Name;
                float q = (float)p.Value;

                //假设品质为1 最小值为原始资质的80%，最大值为原始资质
                float min = q * (1 - quality * 0.2f);
                float max = q * (1 - (quality - 1) * 0.2f);
                JObject m = new JObject();
                m.Add("min", min);
                m.Add("max", max);
                res.Add(k, m);
            }
            return res;
        }
        /**缓存领悟技能 */
        public void addSkillToPet(JObject pet, JArray skls)
        {
            if (skls == null || skls.Count == 0) return;
            int index = 0;
            JArray sklList = (JArray)pet["attr"]["skill"];
            foreach (object i in sklList)
            {
                if (index >= skls.Count) break;
                JObject a = (JObject)i;
                if (a["key"] == null && (int)a["isOpen"] == 1)
                {
                    a["key"] = skls[index];
                    a["lv"] = 1;

                    index++;
                }
            }
            this.savePet(pet);

            string str = "";
            foreach (object s in skls)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(s.ToString());
                str += gd.name + "、";
            }
            str = str.Substring(0, str.Length - 1);

            face.chatInterface.writeSysMsg(pet["nickName"] + " 领悟了技能 " + str + "。");
        }

        /**重置属性点 */
        public void resetAttrPoint(string petId, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/resetAttrPoint", dic, (res) =>
            {
                JObject pet = this.getOneById(petId);
                if (pet != null)
                {
                    //只有属性点、等级重置，其他不变
                    pet["propPoint"] = ((int)pet["lever"] - 1) * 5;
                    JObject baseProp = new JObject();
                    baseProp.Add("ll", 5);
                    baseProp.Add("nl", 5);
                    baseProp.Add("js", 5);
                    baseProp.Add("mj", 5);
                    baseProp.Add("zl", 5);
                    pet["baseProp"] = baseProp;

                    pet["xrmf"] = face.roleInterface.getXrmf();
                    pet["petEquip"] = face.roleInterface.getPetEquip();
                    JObject attr = countUtils.countPetProp(pet);
                    if ((int)pet["attr"]["prop"]["xue"] > (int)attr["max_xue"])
                    {
                        pet["attr"]["prop"]["xue"] = (int)attr["max_xue"];
                    }
                    if ((int)pet["attr"]["prop"]["lan"] > (int)attr["lan"])
                    {
                        pet["attr"]["prop"]["lan"] = (int)attr["lan"];
                    }

                    this.savePet(pet);
                }
                callback();
            });

        }
        public void reLive(string Id, int type, Action ac)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000124", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/petService/reLive", dic, (res) =>
            {
                JObject obj = (JObject)res;
                JArray pets = getPetList();
                for (int i = 0; i < pets.Count; i++)
                {
                    if (pets[i]["Id"].ToString().Equals(Id))
                    {
                        JObject pet = (JObject)pets[i];
                        //只有属性点、等级重置，其他不变
                        pet["lever"] = 1;
                        pet["propPoint"] = 0;
                        JObject baseProp = new JObject();
                        baseProp.Add("ll", 5);
                        baseProp.Add("nl", 5);
                        baseProp.Add("js", 5);
                        baseProp.Add("mj", 5);
                        baseProp.Add("zl", 5);
                        pet["baseProp"] = baseProp;
                        //重置成长率
                        pet["grow"] = int.Parse(obj["grow"].ToString());
                        pet["growValue"] = 0;
                        pet["growLv"] = 0;
                        pet["growBreachLv"] = 0;
                        //重置吃的属性丹
                        JObject danAttr = (JObject)pet["danAttr"];
                        JObject d1 = (JObject)danAttr["d1"];//固元
                        JObject d2 = (JObject)danAttr["d2"];//元魂
                        JObject d3 = (JObject)danAttr["d3"];//仙元
                        string[] attrs = { "bj", "sd", "mz", "max_lan", "max_xue", "ff", "wf", "fg", "wg", "css", };
                        foreach (string a in attrs)
                        {
                            d1[a] = 0;
                            d2[a] = 0;
                            d3[a] = 0;
                        }
                        //重置资质
                        pet["qualityValue"] = obj["qualityValue"];

                        if (type == 1)
                        {
                            JArray skls = (JArray)pet["attr"]["skill"];
                            for (int j = 0; j < skls.Count; j++)
                            {
                                JObject s = (JObject)skls[j];
                                if (j == 0) continue;
                                if (s.ContainsKey("key")) s.Remove("key");
                                if (s.ContainsKey("lv")) s.Remove("lv");
                            }
                        }

                        pet["xrmf"] = face.roleInterface.getXrmf();
                        pet["petEquip"] = face.roleInterface.getPetEquip();
                        JObject attr = countUtils.countPetProp(pet);
                        if ((int)pet["attr"]["prop"]["xue"] > (int)attr["max_xue"])
                        {
                            pet["attr"]["prop"]["xue"] = (int)attr["max_xue"];
                        }
                        if ((int)pet["attr"]["prop"]["lan"] > (int)attr["lan"])
                        {
                            pet["attr"]["prop"]["lan"] = (int)attr["lan"];
                        }
                        pet["attr"]["prop"]["exp"] = 0;

                        this.savePet(pet);

                        string str = "资质重置\n";
                        JObject qualityValue = (JObject)pet["qualityValue"];
                        foreach (JProperty p in qualityValue.Properties())
                        {
                            str += GameAttrConst.propKeyToName(p.Name.ToString()) + " " + p.Value + "\n";
                        }
                        msgCode.showMsg(201, str);

                        ac();
                    }
                }
            });
        }
        /*public void reLive(int grow)
        {
            JObject pet = this.getIsFightPet();
            if (pet != null)
            {
                //只有属性点、等级重置，其他不变
                pet["lever"] = 1;
                pet["propPoint"] = 0;
                JObject baseProp = new JObject();
                baseProp.Add("ll", 5);
                baseProp.Add("nl", 5);
                baseProp.Add("js", 5);
                baseProp.Add("mj", 5);
                baseProp.Add("zl", 5);
                pet["baseProp"] = baseProp;

                JObject attr = countUtils.countPetProp(pet);
                if ((int)pet["attr"]["prop"]["xue"] > (int)attr["max_xue"])
                {
                    pet["attr"]["prop"]["xue"] = (int)attr["max_xue"];
                }
                if ((int)pet["attr"]["prop"]["lan"] > (int)attr["lan"])
                {
                    pet["attr"]["prop"]["lan"] = (int)attr["lan"];
                }
                pet["attr"]["prop"]["exp"] = 0;
                pet["grow"] = grow;
                this.savePet(pet);
            }
        }*/
        /**重置资质 */
        public void reZizhi(string petId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000113", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/reZizhi", dic, (res) =>
            {
                face.goodsInterface.cutPlayerGoodsNumByKey("10000113", 1);
                JObject pet = this.getOneById(petId);
                pet["qualityValue"] = (JObject)res;
                this.savePet(pet);
                string str = "资质重置\n";
                JObject qualityValue = (JObject)pet["qualityValue"];
                foreach (JProperty p in qualityValue.Properties())
                {
                    str += GameAttrConst.propKeyToName(p.Name.ToString()) + " " + p.Value + "\n";
                }
                msgCode.showMsg(201, str);
                callback();
            });
        }

        public void getPlayerPetById(string Id, string name, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/petService/getPlayerPetById", dic, (res) =>
            {
                this.filterPetAttr((JArray)res);
                callback((JArray)res);
            });


        }
        /**获取其他玩家的宠物 */
        public void getPlayerPet(string name, Action<JArray> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/petService/getPlayerPet", dic, (res) =>
            {
                this.filterPetAttr((JArray)res);
                callback((JArray)res);
            });


        }
        /**更新出战状态 */
        public void updateIsFightById(int isFight, string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("isFight", isFight);
            DoGet.getInstance().sendPost("/petService/updateIsFightById", dic, (res) =>
            {
                JArray list = this.getPetList();
                //isFight为1时，其他宠物的isFight=0，为0时其他的不用变
                if (isFight == 1)
                {
                    foreach (object p in list)
                    {
                        JObject obj = (JObject)p;
                        if (!obj["Id"].ToString().Equals(Id))
                        {
                            obj["isFight"] = 0;
                        }
                        else
                        {
                            obj["isFight"] = 1;
                        }
                    }
                    this.initPetList(list);
                }
                else
                {
                    foreach (object p in list)
                    {
                        JObject obj = (JObject)p;
                        if (obj["Id"].ToString().Equals(Id))
                        {
                            obj["isFight"] = 0;
                            this.savePet(obj);
                            break;
                        }
                    }
                }
                //todo:刷新外观(显宠) ws793
                eventsUtils.dispatchWsEvent("793", null);
                //model.eventListener.dispatchEvent('ws', { callback: '793', msg: 3 });
                callback();
            });

        }
        /**宠物放生 */
        public void abandonPet(string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/petService/abandonPet", dic, (res) =>
            {
                this.remFromCache(Id);
                callback();
            });

        }
        public void remFromCache(string Id)
        {
            JArray list = this.getPetList();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["Id"].ToString().Equals(Id))
                {
                    list.Remove(a);
                    break;
                }
            }
            this.initPetList(list);
        }
        /**由id修改指定宠物的某些属性数据。如：血量等 */
        public void updatePetXueById(JObject attrMap, string Id)
        {
            JArray list = this.getPetList();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString().Equals(Id))
                {
                    //缓存
                    IEnumerable<JProperty> properties = attrMap.Properties();
                    foreach (JProperty item in properties)
                    {
                        list[p]["attr"]["prop"][item.Name.ToString()] = item.Value;
                    }

                    this.savePet((JObject)list[p]);
                    break;
                }
            }
        }
        public void updatePetPropPointAndBaseAttr(int propPoint, JObject baseProp, string Id, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            dic.Add("propPoint", propPoint);
            dic.Add("baseProp", baseProp);
            DoGet.getInstance().sendPost("/petService/updatePetPropPointAndBaseAttr", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 1)
                {
                    JArray list = this.getPetList();
                    for (int p = 0; p < list.Count; p++)
                    {
                        if (list[p]["Id"].ToString().Equals(Id))
                        {
                            list[p]["propPoint"] = propPoint;
                            list[p]["baseProp"] = baseProp;
                            this.savePet((JObject)list[p]);
                            callback();
                            break;
                        }
                    }
                }
                else
                {
                    msgCode.showMsg(651);
                }
            });
        }
        /**开启技能槽*/
        public void addCao(string petId, Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip("10000123", 1))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", petId);
            DoGet.getInstance().sendPost("/petService/addCao", dic, (res) =>
            {
                if (int.Parse(res.ToString()) != 0)
                {
                    JObject pet = this.getOneById(petId);
                    JArray skls = (JArray)pet["attr"]["skill"];
                    for (int p = 0; p < skls.Count; p++)
                    {
                        JObject s = (JObject)skls[p];
                        if ((int)s["isOpen"] == 0)
                        {
                            s["isOpen"] = 1;
                            face.goodsInterface.cutPlayerGoodsNumByKey("10000123", 1);
                            this.savePet(pet);
                            msgCode.showMsg(686);
                            callback();
                            break;
                        }
                    }
                }
                else
                {
                    msgCode.showMsg(639);
                }
            });
        }
        /**更新技能缓存 */
        public void updatePetSklCache(string petId, int index, string sklKey)
        {
            JArray list = this.getPetList();
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString().Equals(petId))
                {
                    JObject pet = (JObject)list[p];
                    JArray sklList = (JArray)pet["attr"]["skill"];
                    JObject s = (JObject)sklList[index];
                    s.Remove("key");
                    s.Remove("lv");
                    s.Add("key", sklKey);
                    s.Add("lv", 1);

                    this.savePet(pet);
                    break;
                }
            }
        }
        /**判断是否超出数量限制 */
        public bool isOverNumLimit()
        {
            JArray list = this.getPetList();
            if (list.Count >= 20) return true;
            return false;
        }
        /**给宠物添加经验,并判断是否升级 */
        public void addExp(JObject pet, int exp)
        {
            if (pet == null) return;
            JObject role = face.roleInterface.getRole();
            int oldLv = (int)pet["lever"];
            if (oldLv - (int)role["lever"] > 10)
            {
                return;
            }
            //缓存
            JObject obj = new JObject();
            obj.Add("inputExp", (int)pet["attr"]["prop"]["exp"] + exp);
            obj.Add("lever", oldLv);
            obj.Add("propPoint", pet["propPoint"]);
            obj.Add("baseProp", pet["baseProp"]);
            //不能超过玩家等级10级
            this.countLv(obj, (int)role["lever"] + 10);
            pet["attr"]["prop"]["exp"] = obj["inputExp"];
            pet["lever"] = obj["lever"];
            pet["propPoint"] = obj["propPoint"];
            pet["baseProp"] = obj["baseProp"];
            string sklKey;
            //发生升级的情况，几率领悟技能。与悟性有关
            if (oldLv != (int)pet["lever"])
            {
                int r = strUtils.getRandom(1, 100);
                string str = "";
                //通知宠物升级
                face.chatInterface.writeSysMsg("恭喜您的宠物[" + pet["nickName"] + "]荣升" + pet["lever"] +
                    "级！获得" + ((int)pet["lever"] - oldLv) * 5 + "点属性点。" + str);
                //升级后将血量、蓝量补足
                pet["xrmf"] = face.roleInterface.getXrmf();
                pet["petEquip"] = face.roleInterface.getPetEquip();
                JObject map = countUtils.countPetProp(pet);
                pet["attr"]["prop"]["xue"] = map["max_xue"];
                pet["attr"]["prop"]["lan"] = map["max_lan"];
            }
            this.savePet(pet);
        }
        private void countLv(JObject obj, int limitLv)
        {
            int lv = (int)obj["lever"];
            int inputExp = (int)obj["inputExp"];
            int propPoint = (int)obj["propPoint"];
            int max_exp = lv * 100 + 2 * (int)Math.Pow(5, 0.09f * lv);
            if (lv >= limitLv)
            {
                if (inputExp >= max_exp * 5)
                {
                    obj["inputExp"] = max_exp * 5;
                }
                return;
            }

            if (inputExp >= max_exp)
            {
                inputExp -= max_exp;
                lv += 1;
                propPoint += 5;
                obj["inputExp"] = inputExp;
                obj["lever"] = lv;
                obj["propPoint"] = propPoint;
                //基础属性也要+1，因为逆转散需要
                JObject baseProp = (JObject)obj["baseProp"];
                IEnumerable<JProperty> ps = baseProp.Properties();
                foreach (JProperty p in ps)
                {
                    baseProp[p.Name] = (int)baseProp[p.Name] + 1;
                }
                this.countLv(obj, limitLv);
            }
        }
        /**保存宠物数据(全部需要保存的数据) */
        public void savePet(JObject pet)
        {
            JArray list = getPetList();
            JObject obj = new JObject();
            obj.Add("Id", pet["Id"]);
            obj.Add("key", pet["key"]);
            obj.Add("lever", pet["lever"]);
            obj.Add("model", pet["model"]);
            obj.Add("nickName", pet["nickName"]);
            obj.Add("quality", pet["quality"]);
            obj.Add("loyal", pet["loyal"]);
            obj.Add("savvy", pet["savvy"]);
            obj.Add("grow", pet["grow"]);
            JObject attr = new JObject();
            attr.Add("prop", pet["attr"]["prop"]);
            attr.Add("skill", pet["attr"]["skill"]);
            obj.Add("attr", attr);
            obj.Add("baseProp", pet["baseProp"]);
            obj.Add("qualityValue", pet["qualityValue"]);
            obj.Add("propPoint", pet["propPoint"]);
            obj.Add("isFight", pet["isFight"]);
            obj.Add("growLv", pet["growLv"]);
            obj.Add("growValue", pet["growValue"]);
            obj.Add("growBreachLv", pet["growBreachLv"]);
            obj.Add("danAttr", pet["danAttr"]);
            obj.Add("eatPet", pet["eatPet"]);

            bool b = false;
            for (int p = 0; p < list.Count; p++)
            {
                if (list[p]["Id"].ToString().Equals(obj["Id"].ToString()))
                {
                    b = true;
                    list[p] = obj;
                    break;
                }
            }
            //从宠物数据中提取需要进行保存的属性
            if (!b)
            {
                list.Add(obj);
            }

            dbHandle.save("petList", list);
        }


        public JObject getIsFightPet()
        {
            JArray list = getPetList();
            foreach (object o in list)
            {
                JObject obj = (JObject)o;
                if ((int)obj["isFight"] == 1) return obj;
            }
            return null;
        }
        public JObject getOneById(string Id)
        {
            JArray ls = this.getPetList();
            for (int i = 0; i < ls.Count; i++)
            {
                JObject a = (JObject)ls[i];
                if (a["Id"].ToString().Equals(Id)) return a;
            }
            return null;
        }
        /**获取附件宠物列表 */
        public JArray getEnclosure()
        {
            JArray list = getPetList();
            for (int i = 0; i < list.Count; i++)
            {
                JObject pet = (JObject)list[i];
                if ((int)pet["isFight"] == 1 || (int)pet["growLv"] >= 6)
                {
                    list.RemoveAt(i);
                    i--;
                }
                pet.Add("nowNum", 0);
                pet.Add("enType", 1);
            }
            return list;
        }
        /**获取副宠列表 */
        public JArray getFuPetList(string mainPeId)
        {
            JArray list = getPetList();
            for (int i = 0; i < list.Count; i++)
            {
                JObject pet = (JObject)list[i];
                if ((int)pet["isFight"] == 1 || pet["Id"].ToString().Equals(mainPeId))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
            return list;
        }
        /**获取宠物列表 */
        public JArray getPetList()
        {
            //填补不需要保存的信息
            JArray list = dbHandle.get<JArray>("petList");
            this.filterPetAttr(list);
            return list;
        }
        /**过滤一些属性 */
        private void filterPetAttr(JArray list)
        {
            for (int p = 0; p < list.Count; p++)
            {
                Pet a = this.getPetDataByKey(list[p]["key"].ToString());
                if (a == null) continue;
                list[p]["key"] = a.key;
                list[p]["fightLv"] = a.fightLv;
                list[p]["zizhi"] = a.zizhi;
                list[p]["name"] = a.name;
                a.attr["prop"]["xue"] = list[p]["attr"]["prop"]["xue"];
                a.attr["prop"]["lan"] = list[p]["attr"]["prop"]["lan"];
                a.attr["prop"]["exp"] = list[p]["attr"]["prop"]["exp"];
                a.attr["prop"]["type"] = list[p]["attr"]["prop"]["type"];
                a.attr["skill"] = list[p]["attr"]["skill"];
                list[p]["attr"] = a.attr;
            }
        }

        public void initPetList(JArray petList)
        {
            dbHandle.save("petList", petList);
        }
        public List<Pet> getAllPetList()
        {
            List<Pet> arr = new List<Pet>();
            for (int i = 1000; i < 1250; i++)
            {
                Pet a = getPetDataByKey(i.ToString());
                if (a == null) continue;
                arr.Add(a);
            }
            return arr;
        }

        /**按PrefabCode来获取对应的pet*/
        public Pet getPetByPrefabCode(string prefabCode)
        {
            List<Pet> list = getAllPetList();
            foreach (Pet p in list)
            {
                if (p.prefabCode.Equals(prefabCode)) return p;
            }
            return null;
        }
        public Pet getPetDataByKey(string key)
        {
            Pet pet = new Pet(key);
            switch (key)
            {
                case "1000":
                    {
                        pet.init("赤翼蝠").addPwdId("10002");
                        return pet;
                    }
                case "1001":
                    {
                        pet.init("狼蛛", 1).addPwdId("20005");
                        return pet;
                    }
                case "1002":
                    {
                        pet.init("棕毛土狼").addPwdId("20004");
                        return pet;
                    }
                case "1003":
                    {
                        pet.init("利爪幼虎").addPwdId("60007");
                        return pet;
                    }
                case "1004":
                    {
                        pet.init("食尸虫", 1).addPwdId("30009");
                        return pet;
                    }
                case "1005":
                    {
                        pet.init("双头毒蛇", 1).addPwdId("20002");
                        return pet;
                    }
                case "1006":
                    {
                        pet.init("黑衣山贼").addPwdId("70007");
                        return pet;
                    }
                case "1007":
                    {
                        pet.init("焰火虫", 1).addPwdId("80008");
                        return pet;
                    }
                case "1008":
                    {
                        pet.init("异变尸煞").addPwdId("20003");
                        return pet;
                    }
                case "1009":
                    {
                        pet.init("铁钩手").addPwdId("100001");
                        return pet;
                    }
                case "1010":
                    {
                        pet.init("山野顽猴").addPwdId("xz08");
                        return pet;
                    }
                case "1011":
                    {
                        pet.init("赤眼霜狼", 1).addPwdId("60002");
                        return pet;
                    }
                case "1012":
                    {
                        pet.init("金仓鼠").addPwdId("30007");
                        return pet;
                    }
                case "1013":
                    {
                        pet.init("苍鹰").addPwdId("30010");
                        return pet;
                    }
                case "1014":
                    {
                        pet.init("赤灵", 1).addPwdId("1014");
                        return pet;
                    }
                case "1015":
                    {
                        pet.init("江鳇鱼").addPwdId("80002");
                        return pet;
                    }
                case "1016":
                    {
                        pet.init("狂暴兽人").addPwdId("100003");
                        return pet;
                    }
                case "1017":
                    {
                        pet.init("草藤妖", 1).addPwdId("30005");
                        return pet;
                    }
                case "1018":
                    {
                        pet.init("轻骑兵").addPwdId("90002");
                        return pet;
                    }
                case "1019":
                    {
                        pet.init("恶犬", 1).addPwdId("30004");
                        return pet;
                    }
                case "1020":
                    {
                        pet.init("大刀兵").addPwdId("sc05");
                        return pet;
                    }
                case "1021":
                    {
                        pet.init("崩角牛").addPwdId("80007");
                        return pet;
                    }
                case "1022":
                    {
                        pet.init("青竹怪", 1).addPwdId("40001");
                        return pet;
                    }
                case "1023":
                    {
                        pet.init("吸血蝙蝠").addPwdId("10002");
                        return pet;
                    }
                case "1024":
                    {
                        pet.init("懒熊").addPwdId("80003");
                        return pet;
                    }
                case "1025":
                    {
                        pet.init("湛岚犬").addPwdId("1025");
                        return pet;
                    }
                case "1026":
                    {
                        pet.init("树精", 1).addPwdId("50005");
                        return pet;
                    }
                case "1027":
                    {
                        pet.init("食人狼", 1).addPwdId("80006");
                        return pet;
                    }
                case "1028":
                    {
                        pet.init("苍狼").addPwdId("50003");
                        return pet;
                    }
                case "1029":
                    {
                        pet.init("鹰狮", 1).addPwdId("30011");
                        return pet;
                    }
                case "1030":
                    {
                        pet.init("蜥蜴", 1).addPwdId("70001");
                        return pet;
                    }
                case "1031":
                    {
                        pet.init("红羽凶鹰", 1).addPwdId("60012");
                        return pet;
                    }
                case "1032":
                    {
                        pet.init("青岩兽", 1).addPwdId("60013");
                        return pet;
                    }
                case "1033":
                    {
                        pet.init("狩猎者").addPwdId("30001");
                        return pet;
                    }
                case "1034":
                    {
                        pet.init("龙蛟蛟", 1).addPwdId("90003");
                        return pet;
                    }
                case "1035":
                    {
                        pet.init("古牙兽", 1).addPwdId("xz07");
                        return pet;
                    }
                case "1036":
                    {
                        pet.init("斧兵").addPwdId("sc01");
                        return pet;
                    }
                case "1037":
                    {
                        pet.init("铁锤兵").addPwdId("40008");
                        return pet;
                    }
                case "1038":
                    {
                        pet.init("铁骑枪兵").addPwdId("90002");
                        return pet;
                    }
                case "1039":
                    {
                        pet.init("散仙", 1).addPwdId("30012");
                        return pet;
                    }
                case "1040":
                    {
                        pet.init("幽魂").addPwdId("60010");
                        return pet;
                    }
                case "1041":
                    {
                        pet.init("泥石兵俑", 1).addPwdId("50004");
                        return pet;
                    }
                case "1042":
                    {
                        pet.init("盾甲兵").addPwdId("100005");
                        return pet;
                    }
                case "1043":
                    {
                        pet.init("熔骨血尸", 1).addPwdId("60009");
                        return pet;
                    }
                case "1044":
                    {
                        pet.init("开山力士").addPwdId("100006");
                        return pet;
                    }
                case "1045":
                    {
                        pet.init("恶灵", 1).addPwdId("40010");
                        return pet;
                    }
                case "1046":
                    {
                        pet.init("摄魂使者", 1).addPwdId("100004");
                        return pet;
                    }
                case "1047":
                    {
                        pet.init("山贼哨兵").addPwdId("70003");
                        return pet;
                    }
                case "1048":
                    {
                        pet.init("黑煞甲士").addPwdId("20003");
                        return pet;
                    }
                case "1049":
                    {
                        pet.init("赤炎甲虫", 1).addPwdId("80008");
                        return pet;
                    }
                case "1050":
                    {
                        pet.init("积怨行尸").addPwdId("50008");
                        return pet;
                    }
                case "1051":
                    {
                        pet.init("黑风狼").addPwdId("20004");
                        return pet;
                    }
                case "1052":
                    {
                        pet.init("阴魁猴").addPwdId("xz08");
                        return pet;
                    }
                case "1053":
                    {
                        pet.init("木精", 1).addPwdId("50002");
                        return pet;
                    }
                case "1054":
                    {
                        pet.init("利爪猛虎").addPwdId("60007");
                        return pet;
                    }
                case "1055":
                    {
                        pet.init("褐甲蜥蜴", 1).addPwdId("70001");
                        return pet;
                    }
                case "1056":
                    {
                        pet.init("黑寡妇", 1).addPwdId("20005");
                        return pet;
                    }
                case "1057":
                    {
                        pet.init("擎斧恶汉").addPwdId("70007");
                        return pet;
                    }
                case "1058":
                    {
                        pet.init("擎钩先锋").addPwdId("100001");
                        return pet;
                    }
                case "1059":
                    {
                        pet.init("双刀大盗").addPwdId("70006");
                        return pet;
                    }
                case "1060":
                    {
                        pet.init("绿食怪", 1).addPwdId("30005");
                        return pet;
                    }
                case "1061":
                    {
                        pet.init("沙虫", 1).addPwdId("30009");
                        return pet;
                    }
                case "1062":
                    {
                        pet.init("猎命鹫", 1).addPwdId("30011");
                        return pet;
                    }
                case "1063":
                    {
                        pet.init("古炽灵", 1).addPwdId("1014");
                        return pet;
                    }
                case "1064":
                    {
                        pet.init("千年树妖", 1).addPwdId("50005");
                        return pet;
                    }
                case "1065":
                    {
                        pet.init("魔怨雪狼", 1).addPwdId("60002");
                        return pet;
                    }
                case "1066":
                    {
                        pet.init("幼鳞鳇鱼").addPwdId("80002");
                        return pet;
                    }
                case "1067":
                    {
                        pet.init("盘蛟兽", 1).addPwdId("90003");
                        return pet;
                    }
                case "1068":
                    {
                        pet.init("吸血妖木", 1).addPwdId("50002");
                        return pet;
                    }
                case "1069":
                    {
                        pet.init("叱炎犬", 1).addPwdId("xz18");
                        return pet;
                    }
                case "1070":
                    {
                        pet.init("恶浪蛟", 1).addPwdId("90003");
                        return pet;
                    }
                case "1071":
                    {
                        pet.init("荒野僵尸").addPwdId("50008");
                        return pet;
                    }
                case "1072":
                    {
                        pet.init("伴生妖蛇", 1).addPwdId("20002");
                        return pet;
                    }
                case "1073":
                    {
                        pet.init("火帘鹰", 1).addPwdId("60012");
                        return pet;
                    }
                case "1074":
                    {
                        pet.init("紫魂使魔").addPwdId("60010");
                        return pet;
                    }
                case "1075":
                    {
                        pet.init("山越兽人").addPwdId("100003");
                        return pet;
                    }
                case "1076":
                    {
                        pet.init("巨掌黑熊").addPwdId("80003");
                        return pet;
                    }
                case "1077":
                    {
                        pet.init("破劫半仙", 1).addPwdId("30012");
                        return pet;
                    }
                case "1078":
                    {
                        pet.init("弓骑兵", 1).addPwdId("30003");
                        return pet;
                    }
                case "1079":
                    {
                        pet.init("冲锋斧手").addPwdId("70007");
                        return pet;
                    }
                case "1080":
                    {
                        pet.init("蓝魔").addPwdId("60010");
                        return pet;
                    }
                case "1081":
                    {
                        pet.init("虚魂犬").addPwdId("1025");
                        return pet;
                    }
                case "1082":
                    {
                        pet.init("震岳荒兽", 1).addPwdId("80007");
                        return pet;
                    }
                case "1083":
                    {
                        pet.init("紫命玄魄").addPwdId("60010");
                        return pet;
                    }
                case "1084":
                    {
                        pet.init("纳灵竹妖", 1).addPwdId("40001");
                        return pet;
                    }
                case "1085":
                    {
                        pet.init("白首兽", 1).addPwdId("30011");
                        return pet;
                    }
                case "1086":
                    {
                        pet.init("藤甲射手").addPwdId("30001");
                        return pet;
                    }
                case "1087":
                    {
                        pet.init("啮齿鼠").addPwdId("xz17");
                        return pet;
                    }
                case "1088":
                    {
                        pet.init("狼人战士", 1).addPwdId("80006");
                        return pet;
                    }
                case "1089":
                    {
                        pet.init("飞廉骑兵").addPwdId("90002");
                        return pet;
                    }
                case "1090":
                    {
                        pet.init("大刀护卫").addPwdId("sc05");
                        return pet;
                    }
                case "1091":
                    {
                        pet.init("业火狼人", 1).addPwdId("80006");
                        return pet;
                    }
                case "1092":
                    {
                        pet.init("亡命逃兵").addPwdId("70003");
                        return pet;
                    }
                case "1093":
                    {
                        pet.init("飞羽死士", 1).addPwdId("50004");
                        return pet;
                    }
                case "1094":
                    {
                        pet.init("凶牙血蝠").addPwdId("10002");
                        return pet;
                    }
                case "1095":
                    {
                        pet.init("长毛猛犸", 1).addPwdId("60006");
                        return pet;
                    }
                case "1096":
                    {
                        pet.init("血魄炼尸", 1).addPwdId("60009");
                        return pet;
                    }
                case "1097":
                    {
                        pet.init("吸魄魔蛛", 1).addPwdId("20005");
                        return pet;
                    }
                case "1098":
                    {
                        pet.init("啸冥犬").addPwdId("1025");
                        return pet;
                    }
                case "1099":
                    {
                        pet.init("巨斧死士").addPwdId("100006");
                        return pet;
                    }
                case "1100":
                    {
                        pet.init("冷血刀客").addPwdId("70006");
                        return pet;
                    }
                case "1101":
                    {
                        pet.init("嗜血狂鹰", 1).addPwdId("60012");
                        return pet;
                    }
                case "1102":
                    {
                        pet.init("丧魂魔将").addPwdId("20003");
                        return pet;
                    }
                case "1103":
                    {
                        pet.init("幽冥之狼", 1).addPwdId("80006");
                        return pet;
                    }
                case "1104":
                    {
                        pet.init("赤瞳魔俑", 1).addPwdId("50004");
                        return pet;
                    }
                case "1105":
                    {
                        pet.init("冥府守卫").addPwdId("100005");
                        return pet;
                    }
                case "1106":
                    {
                        pet.init("阴风豹").addPwdId("60007");
                        return pet;
                    }
                case "1107":
                    {
                        pet.init("夺命将军").addPwdId("100006");
                        return pet;
                    }
                case "1108":
                    {
                        pet.init("吸魂木妖", 1).addPwdId("50002");
                        return pet;
                    }
                case "1109":
                    {
                        pet.init("夺魄护卫").addPwdId("100005");
                        return pet;
                    }
                case "1110":
                    {
                        pet.init("阴火虫", 1).addPwdId("xz09");
                        return pet;
                    }
                case "1111":
                    {
                        pet.init("游荡孤魂").addPwdId("50008");
                        return pet;
                    }
                case "1112":
                    {
                        pet.init("青炎妖狼", 1).addPwdId("80006");
                        return pet;
                    }
                case "1113":
                    {
                        pet.init("巨灵守卫").addPwdId("100005");
                        return pet;
                    }
                case "1114":
                    {
                        pet.init("引路使者", 1).addPwdId("100004");
                        return pet;
                    }
                case "1115":
                    {
                        pet.init("白魔猿").addPwdId("60003");
                        return pet;
                    }
                case "1116":
                    {
                        pet.init("玄魄妖", 1).addPwdId("40001");
                        return pet;
                    }
                case "1117":
                    {
                        pet.init("丧魂魔尸").addPwdId("50008");
                        return pet;
                    }
                case "1118":
                    {
                        pet.init("地狱犬", 1).addPwdId("xz18");
                        return pet;
                    }
                case "1119":
                    {
                        pet.init("般涅雏凤", 1).addPwdId("60012");
                        return pet;
                    }
                case "1120":
                    {
                        pet.init("恋尘阴灵").addPwdId("60010");
                        return pet;
                    }
                case "1121":
                    {
                        pet.init("毒尸怪").addPwdId("50008");
                        return pet;
                    }
                case "1122":
                    {
                        pet.init("阴阳界灵", 1).addPwdId("xz15");
                        return pet;
                    }
                case "1123":
                    {
                        pet.init("幽玄枪客").addPwdId("70003");
                        return pet;
                    }
                case "1124":
                    {
                        pet.init("枯煞木灵", 1).addPwdId("50002");
                        return pet;
                    }
                case "1125":
                    {
                        pet.init("白苍魔狼", 1).addPwdId("60002");
                        return pet;
                    }
                case "1126":
                    {
                        pet.init("奈河守将").addPwdId("100005");
                        return pet;
                    }
                case "1127":
                    {
                        pet.init("厌世花", 1).addPwdId("xz14");
                        return pet;
                    }
                case "1128":
                    {
                        pet.init("黑魇兽", 1).addPwdId("80007");
                        return pet;
                    }
                case "1129":
                    {
                        pet.init("吞魂兽").addPwdId("1025");
                        return pet;
                    }
                case "1130":
                    {
                        pet.init("阴冥护卫").addPwdId("100005");
                        return pet;
                    }
                case "1131":
                    {
                        pet.init("幽蓝匠魂").addPwdId("60010");
                        return pet;
                    }
                case "1132":
                    {
                        pet.init("守魂兽", 1).addPwdId("xz05");
                        return pet;
                    }
                case "1133":
                    {
                        pet.init("冥仙", 1).addPwdId("100004");
                        return pet;
                    }
                case "1134":
                    {
                        pet.init("通灵鼠").addPwdId("xz17");
                        return pet;
                    }
                case "1135":
                    {
                        pet.init("通玄鳇鱼").addPwdId("80002");
                        return pet;
                    }
                case "1136":
                    {
                        pet.init("天煞老妖").addPwdId("20003");
                        return pet;
                    }
                case "1137":
                    {
                        pet.init("化梦犬").addPwdId("1025");
                        return pet;
                    }
                case "1138":
                    {
                        pet.init("沙化蜥蜴", 1).addPwdId("70001");
                        return pet;
                    }

                case "1139":
                    {
                        pet.init("天刀护卫").addPwdId("sc05");
                        return pet;
                    }
                case "1140":
                    {
                        pet.init("破军猿王").addPwdId("xz08");
                        return pet;
                    }
                case "1141":
                    {
                        pet.init("苍刑飞骑").addPwdId("90002");
                        return pet;
                    }
                case "1142":
                    {
                        pet.init("玄影妖灵").addPwdId("xz15");
                        return pet;
                    }
                case "1143":
                    {
                        pet.init("诛灵天鹰", 1).addPwdId("60012");
                        return pet;
                    }
                case "1144":
                    {
                        pet.init("阴阳玄蛇", 1).addPwdId("20002");
                        return pet;
                    }
                case "1145":
                    {
                        pet.init("暗影妖狼").addPwdId("20004");
                        return pet;
                    }
                case "1146":
                    {
                        pet.init("勾陈古树", 1).addPwdId("50005");
                        return pet;
                    }
                case "1147":
                    {
                        pet.init("龙胆将军").addPwdId("100006");
                        return pet;
                    }
                case "1148":
                    {
                        pet.init("青莲竹妖", 1).addPwdId("40001");
                        return pet;
                    }
                case "1149":
                    {
                        pet.init("龙血鳇鱼").addPwdId("80002");
                        return pet;
                    }
                case "1150":
                    {
                        pet.init("沧浪妖狼", 1).addPwdId("xz05");
                        return pet;
                    }
                case "1151":
                    {
                        pet.init("翻江藤", 1).addPwdId("xz14");
                        return pet;
                    }
                case "1152":
                    {
                        pet.init("镇川巨熊").addPwdId("xz10");
                        return pet;
                    }
                case "1153":
                    {
                        pet.init("赤影妖蝠").addPwdId("10002");
                        return pet;
                    }
                case "1154":
                    {
                        pet.init("盘丝玄蛛", 1).addPwdId("20005");
                        return pet;
                    }
                case "1155":
                    {
                        pet.init("龙爪凶狼").addPwdId("50003");
                        return pet;
                    }
                case "1156":
                    {
                        pet.init("穿天弩手").addPwdId("30001");
                        return pet;
                    }
                case "1157":
                    {
                        pet.init("荡岳妖熊").addPwdId("xz10");
                        return pet;
                    }
                case "1158":
                    {
                        pet.init("裂地将军").addPwdId("100006");
                        return pet;
                    }
                case "1159":
                    {
                        pet.init("擎山兽人").addPwdId("100003");
                        return pet;
                    }
                case "1160":
                    {
                        pet.init("混世散仙", 1).addPwdId("30012");
                        return pet;
                    }
                case "1161":
                    {
                        pet.init("玄幽魔匠").addPwdId("60010");
                        return pet;
                    }
                case "1162":
                    {
                        pet.init("遁甲卫士").addPwdId("100005");
                        return pet;
                    }
                case "1163":
                    {
                        pet.init("道化枪兵").addPwdId("70003");
                        return pet;
                    }
                case "1164":
                    {
                        pet.init("千幻音蝠").addPwdId("xz02");
                        return pet;
                    }
                case "1165":
                    {
                        pet.init("不朽木灵", 1).addPwdId("50002");
                        return pet;
                    }
                case "1166":
                    {
                        pet.init("沧澜兽", 1).addPwdId("xz11");
                        return pet;
                    }
                case "1167":
                    {
                        pet.init("震天将军").addPwdId("bs07");
                        return pet;
                    }
                case "1168":
                    {
                        pet.init("虎魄将军").addPwdId("100006");
                        return pet;
                    }
                case "1169":
                    {
                        pet.init("离火蛟", 1).addPwdId("90003");
                        return pet;
                    }
                case "1170":
                    {
                        pet.init("钩玄统领").addPwdId("100001");
                        return pet;
                    }
                case "1171":
                    {
                        pet.init("开荒兽人").addPwdId("100003");
                        return pet;
                    }
                case "1172":
                    {
                        pet.init("七绝斧手").addPwdId("70007");
                        return pet;
                    }
                case "1173":
                    {
                        pet.init("陨星妖灵", 1).addPwdId("xz11");
                        return pet;
                    }
                case "1174":
                    {
                        pet.init("破岩天蛇", 1).addPwdId("20002");
                        return pet;
                    }
                case "1175":
                    {
                        pet.init("追风魔豹").addPwdId("60007");
                        return pet;
                    }
                case "1176":
                    {
                        pet.init("霸荒战狼").addPwdId("xz06");
                        return pet;
                    }
                case "1177":
                    {
                        pet.init("狂骨血魔", 1).addPwdId("60009");
                        return pet;
                    }
                case "1178":
                    {
                        pet.init("不灭炽灵", 1).addPwdId("1014");
                        return pet;
                    }
                case "1179":
                    {
                        pet.init("枯魂枪客").addPwdId("70003");
                        return pet;
                    }
                case "1180":
                    {
                        pet.init("噬川虫", 1).addPwdId("30009");
                        return pet;
                    }
                case "1181":
                    {
                        pet.init("流星猎手").addPwdId("30001");
                        return pet;
                    }
                case "1182":
                    {
                        pet.init("穿江巨蜥", 1).addPwdId("70001");
                        return pet;
                    }
                case "1183":
                    {
                        pet.init("洞天鼠").addPwdId("xz17");
                        return pet;
                    }
                case "1184":
                    {
                        pet.init("迷幻影狼", 1).addPwdId("xz05");
                        return pet;
                    }
                case "1185":
                    {
                        pet.init("覆海藤", 1).addPwdId("xz14");
                        return pet;
                    }
                case "1186":
                    {
                        pet.init("不死秦俑", 1).addPwdId("50004");
                        return pet;
                    }
                case "1187":
                    {
                        pet.init("妖焰虫", 1).addPwdId("xz09");
                        return pet;
                    }
                case "1188":
                    {
                        pet.init("古皇兽", 1).addPwdId("60013");
                        return pet;
                    }
                case "1189":
                    {
                        pet.init("风原妖狼", 1).addPwdId("xz05");
                        return pet;
                    }
                case "1190":
                    {
                        pet.init("天鹰玄兽", 1).addPwdId("60012");
                        return pet;
                    }
                case "1191":
                    {
                        pet.init("惊鸿神鹰", 1).addPwdId("30011");
                        return pet;
                    }
                case "1192":
                    {
                        pet.init("太炎巨蜥", 1).addPwdId("70001");
                        return pet;
                    }
                case "1193":
                    {
                        pet.init("破虚兽").addPwdId("1025");
                        return pet;
                    }
                case "1194":
                    {
                        pet.init("震苍兽", 1).addPwdId("xz07");
                        return pet;
                    }
                case "1195":
                    {
                        pet.init("盘龙兽", 1).addPwdId("xz03");
                        return pet;
                    }
                case "1196":
                    {
                        pet.init("射日飞骑", 1).addPwdId("30003");
                        return pet;
                    }
                case "1197":
                    {
                        pet.init("斩月铁骑").addPwdId("90002"); ;
                        return pet;
                    }
                case "1198":
                    {
                        pet.init("尸煞妖王").addPwdId("20003");
                        return pet;
                    }
                case "1199":
                    {
                        pet.init("两仪蛟", 1).addPwdId("90003");
                        return pet;
                    }
                case "1200":
                    {
                        pet.init("太乙散仙", 1).addPwdId("30012");
                        return pet;
                    }
                case "1201":
                    {
                        pet.init("真阳火凤", 1).addPwdId("60012");
                        return pet;
                    }
                case "1202":
                    {
                        pet.init("少阳兽", 1).addPwdId("60013");
                        return pet;
                    }
                case "1203":
                    {
                        pet.init("虚阳古木", 1).addPwdId("50002");
                        return pet;
                    }
                case "1204":
                    {
                        pet.init("太阴斧魔").addPwdId("70007");
                        return pet;
                    }
                case "1205":
                    {
                        pet.init("玄阴古兽", 1).addPwdId("xz11");
                        return pet;
                    }
                case "1206":
                    {
                        pet.init("化阴魔尸").addPwdId("50008");
                        return pet;
                    }
                case "1207":
                    {
                        pet.init("天煞统领").addPwdId("100006");
                        return pet;
                    }
                case "1208":
                    {
                        pet.init("地魔统领").addPwdId("100005");
                        return pet;
                    }
                case "1209":
                    {
                        pet.init("乾坤箭俑", 1).addPwdId("50004");
                        return pet;
                    }
                case "1210":
                    {
                        pet.init("道玄赤灵").addPwdId("xz15");
                        return pet;
                    }
                case "1211":
                    {
                        pet.init("无双刀客").addPwdId("70006");
                        return pet;
                    }
                case "1212":
                    {
                        pet.init("兽神统领").addPwdId("sc05");
                        return pet;
                    }
                case "1213":
                    {
                        pet.init("太虚妖龙", 1).addPwdId("xz03");
                        return pet;
                    }
                case "1214":
                    {
                        pet.init("劈天霸王").addPwdId("100006");
                        return pet;
                    }
                case "1215":
                    {
                        pet.init("剑圣").addFightLv(1).addPwdId("1214");
                        return pet;
                    }
                case "1216":
                    {
                        pet.init("梦瑶仙子", 1).addFightLv(1).addPwdId("1215");
                        return pet;
                    }
                case "1217":
                    {
                        pet.init("青玄剑灵").addFightLv(1).addPwdId("60005");
                        return pet;
                    }
                case "1218":
                    {
                        pet.init("落羽仙子", 1).addFightLv(1).addPwdId("1217");
                        return pet;
                    }
                case "1219":
                    {
                        pet.init("太虚之魂", 1).addFightLv(1).addPwdId("35932");
                        return pet;
                    }
                case "1220":
                    {
                        pet.init("玄晶天狐", 1).addFightLv(1).addPwdId("bs20").setX8Name("九尾仙狐").addX8PwdId("bs20_1");
                        return pet;
                    }
                case "1221":
                    {
                        pet.init("逆天魔龙").addFightLv(1).addPwdId("1220").setX8Name("灭天魔龙").addX8PwdId("1220_1");
                        return pet;
                    }
                case "1222":
                    {
                        pet.init("惑天玄姬").addFightLv(1).addPwdId("1221").setX8Name("祸天狐仙").addX8PwdId("1221_1"); ;
                        return pet;
                    }
                case "1223":
                    {
                        pet.init("小青娘子", 1).addFightLv(1).addPwdId("cw_sj").setX8Name("千年青蛇").addX8PwdId("cw_sj_1"); ;
                        return pet;
                    }
                case "1224":
                    {
                        pet.init("九天玄女", 1).addFightLv(1).addPwdId("1223").setX8Name("九天圣女").addX8PwdId("1223_1"); ;
                        return pet;
                    }
                case "1225":
                    {
                        pet.init("龙翔天兵").addFightLv(1).addPwdId("1224").setX8Name("龙翔神将").addX8PwdId("1224_1"); ;
                        return pet;
                    }
                case "1226":
                    {
                        pet.init("龙翔帝君", 1).addFightLv(1).addPwdId("1225").setX8Name("龙翔大帝").addX8PwdId("1225_1"); ;
                        return pet;
                    }
                case "1227":
                    {
                        pet.init("青龙仙童").addFightLv(1).addPwdId("cw_xlr").setX8Name("苍龙仙童").addX8PwdId("cw_xlr_1"); ;
                        return pet;
                    }
                case "1228":
                    {
                        pet.init("幽玄魔俑").addFightLv(1).addPwdId("1227");
                        return pet;
                    }

            }
            return null;
        }
    }
}
