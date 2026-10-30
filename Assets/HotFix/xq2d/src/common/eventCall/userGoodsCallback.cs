using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common.eventCall
{
    class userGoodsCallback
    {
        private static userGoodsCallback ws;
        public static userGoodsCallback getInstance()
        {
            if (ws == null) ws = new userGoodsCallback();
            return ws;
        }
        public void call(JToken obj)
        {
            string key = obj["key"].ToString();
            object msg = obj["msg"];
            JObject a = (JObject)msg;
            if (key.Equals("10000000") || key.Equals("10000001") || key.Equals("10000002"))
            {
                this.LTPResult(key, (int)a["num"]);
            }
            else if (key.Equals("10000029") || key.Equals("10000030") || key.Equals("10000031") || key.Equals("10000032"))
            {
                this.eatDan(key, (JArray)a["res"], (int)a["num"]);
            }
            else if (key.Equals("10000133") || key.Equals("10000134") || key.Equals("10000135") ||
               key.Equals("10000136") || key.Equals("10000137") || key.Equals("10000138") || key.Equals("10000139") ||
               key.Equals("10000140") || key.Equals("10000141") || key.Equals("10000142") || key.Equals("10000233"))
            {
                this.GetPet((JObject)a["res"]);
            }
            else if (key.Equals("10000125") || key.Equals("10000126") || key.Equals("10000181") || key.Equals("10000182"))
            {
                this.juling((int)a["res"]);
            }
            else if (key.Equals("10000177") || key.Equals("10000178") || key.Equals("10000183") || key.Equals("10000184") || key.Equals("10000208")
                || key.Equals("10000209") || key.Equals("10000210"))
            {
                this.updateStatusLan(key, (JObject)a["res"]);
            }
            else if (key.Equals("10000213") || key.Equals("10000214") || key.Equals("10000215") || key.Equals("10000216") || key.Equals("10000217"))
            {
                this.userJmDan(key, (int)a["res"]);
            }
            else if (key.Equals("10000187"))
            {
                this.useYangShan(key, (int)a["res"]);
            }
            else if (strUtils.isMatch(key, "1012([0-9]{4})"))
            {
                this.useWzyc(key, (JObject)a["res"]);
            }
            else if (key.Equals("10000112") || key.Equals("10000175") || key.Equals("10000176") || key.Equals("10000112") || key.Equals("10000189")
                || key.Equals("10000201") || key.Equals("10000220") || key.Equals("10000257") || key.Equals("10000258") || key.Equals("10000259") ||
                 key.Equals("10000260") || key.Equals("10000261") || key.Equals("10000262") || key.Equals("10000263") || key.Equals("10000264") || key.Equals("10000265")
                 || key.Equals("10000267") || key.Equals("10000268") || key.Equals("10000269") || key.Equals("10000270") || key.Equals("10000271") || key.Equals("10000272")
                 || key.Equals("10000273") || key.Equals("10000274") || key.Equals("10000275") || key.Equals("10000276") || key.Equals("10000277") || key.Equals("10000278")
                 || key.Equals("10000279") || key.Equals("10000280") || key.Equals("10000281") || key.Equals("10000282") || key.Equals("10000283") || key.Equals("10000285")
                 || key.Equals("10000286") || key.Equals("10000237") || key.Equals("10000238") || key.Equals("10000239") || key.Equals("10000240")
                 || key.Equals("10000241") || key.Equals("10000242") || key.Equals("10000243") || key.Equals("10000244") || key.Equals("10000245")
                 || key.Equals("10000246") || key.Equals("10000288") || key.Equals("10000292") || key.Equals("10000294") || key.Equals("10000302")
                 )
            {
                openBox(key, (JArray)a["res"]);
            }

        }

        private void useWzyc(string key, JObject res)
        {
            //清理任务、创建任务
            string[] tasks = { "3288", "3289", "3290" };
            face.taskInterface.remTaskAndOverTaskFromCache(tasks);
            face.taskInterface.createOneStartTask(res["key"].ToString(), 2);
        }
        private void useYangShan(string key, int res)
        {
            JObject role = face.roleInterface.getRole();
            role["attr"]["msg"]["sez"] = (int)role["attr"]["msg"]["sez"] + 1;
            face.roleInterface.saveRole(role);
            msgCode.showMsg(1013, 1);
        }
        /**使用紫云丹 */
        public void userJmDan(string key, int res)
        {
            if (res == 1)
            {
                int num = 0;
                if (key.Equals("10000213"))
                {
                    num = 20;
                }
                else if (key.Equals("10000214"))
                {
                    num = 40;
                }
                else if (key.Equals("10000215"))
                {
                    num = 60;
                }
                else if (key.Equals("10000216"))
                {
                    num = 80;
                }
                else if (key.Equals("10000217"))
                {
                    num = 100;
                }
                JObject r = face.roleInterface.getRole();
                JArray skls = (JArray)r["attr"]["skill"];
                int xh = 0;
                for (int i = 0; i < skls.Count; i++)
                {
                    JObject skl = (JObject)skls[i];
                    if (skl.ContainsKey("key") && skl["key"].ToString().Substring(0, 8).Equals("10021002"))
                    {
                        int lv = (int)skl[i]["lv"];
                        xh += countPointByLv(lv);
                    }
                }
                int point = (int)r["attr"]["msg"]["jmPoint"] + xh;
                if (point + num > 1160)
                {
                    num = 1160 - point;
                }
                r["attr"]["msg"]["jmPoint"] = (int)r["attr"]["msg"]["jmPoint"] + num;
                face.roleInterface.saveRole(r);
                msgCode.showMsg(629);
            }
            else if (res == -1)
            {
                msgCode.showMsg(1012);
            }
            else
            {
                msgCode.showMsg(0);
            }
        }
        private int countPointByLv(int lv)
        {
            //每个等级需要消耗的点数
            int[] xhn = { 20, 20, 40, 40, 40, 60, 60, 60, 80, 80, 80, 100, 100, 100, 100 };
            int sum = 0;
            for (int i = 0; i < lv; i++)
            {
                sum += xhn[i];
            }
            return sum;
        }
        /**诱敌香草*/
        private void updateStatusLan(string key, JObject res)
        {
            if (res == null)
            {
                msgCode.showMsg(639);
                return;
            }
            string attrKey = null;
            if (key.Equals("10000177")) attrKey = "rwzhd";
            else if (key.Equals("10000178")) attrKey = "cwzhd";
            else if (key.Equals("10000183")) attrKey = "qdxc";
            else if (key.Equals("10000184")) attrKey = "ydxc";
            else if (key.Equals("10000208")) attrKey = "blx";
            else if (key.Equals("10000209")) attrKey = "jml";
            else if (key.Equals("10000210")) attrKey = "dblq";
            JObject role = face.roleInterface.getRole();
            JObject status = (JObject)role["attr"]["status"];

            status[attrKey] = res;
            face.roleInterface.saveRole(role);
        }
        private void juling(int res)
        {
            if (res == 1)
            {
                msgCode.showMsg(986);
            }
            else if (res == 0)
            {
                msgCode.showMsg(0);
            }
            else if (res == -1)
            {
                msgCode.showMsg(610);
            }
        }

        /**扩展突破技能槽 */
        public void expSkills(string key, object res)
        {
            if (res != null)
            {
                JArray list = face.petInterface.getPetList();
                for (int p = 0; p < list.Count; p++)
                {
                    if ((int)list[p]["isFight"] == 1)
                    {
                        JArray al = (JArray)list[p]["attr"]["skill"];
                        JObject a = new JObject();
                        a.Add("isOpen", 1);
                        al.Add(a);
                        face.petInterface.initPetList(list);
                        msgCode.showMsg(629);
                        break;
                    }
                }
            }
        }



        /**通用使用回调 */
        public void commonUser(string key, int res)
        {
            if (res == 1)
            {
                msgCode.showMsg(629);
            }
        }

        /**开启宝箱 */
        public void openBox(string key, JArray res)
        {
            //eventsUtils.dispatchWsEvent("10000", res);
            //msgCode.showMsg(629);
            string str = face.rewardInterface.saveRewards(res);
            TextTipUI.create().draw(str);
        }
        /**宠物经验丹 */
        public bool eatDan(string key, JArray res, int num)
        {
            JObject pet = face.petInterface.getIsFightPet();
            if (pet != null)
            {
                if ((int)pet["lever"] - (int)face.roleInterface.getRole()["lever"] > 10 || (int)pet["lever"] >= 110)
                {
                    msgCode.showMsg(631);
                    return false;
                }
                int exp = 1000;
                if (key.Equals("10000030")) exp = 10000;
                else if (key.Equals("10000031")) exp = 50000;
                else if (key.Equals("10000032")) exp = 200000;
                face.petInterface.addExp(pet, exp * num);
                msgCode.showMsg(629);
                //遍历返回的领悟技能
                eventsUtils.dispatchWsEvent("824", res);
            }
            else
            {
                msgCode.showMsg(628);
                return false;
            }
            return true;
        }
        /**获得称号 */
        public void getCh(string key, string res)
        {
            if (res.Equals("1"))
            {
                int i = int.Parse(key.Substring(4)) + 1;
                face.roleInterface.saveCh("a" + i);
                msgCode.showMsg(895);
            }
            else
            {
                msgCode.showMsg(894);
            }
        }
        /**龙头票的使用效果 
             * 按比例转换属性
            */
        public void LTPResult(string key, int num)
        {
            int gold = 0;
            if (key.Equals("10000000")) gold = 1000;
            else if (key.Equals("10000001")) gold = 500;
            else if (key.Equals("10000002")) gold = 100;

            face.roleInterface.updateMoney(gold * num, 0);
            msgCode.showMsg(629);
            //刷新背包银两
            //PointGet.getAcPage<PackagePage>().GetComponent<PackagePage>().updateHb();
        }
        /**宠物重生 */
        /*public void reLive(string key, object res)
        {
            if (res.ToString().Equals("0"))
            {
                msgCode.showMsg(0);
                return;
            }
            JObject pet = face.petInterface.getIsFightPet();
            if (pet != null)
            {
                face.petInterface.reLive(int.Parse(res.ToString()));
                msgCode.showMsg(629);
            }
            else
            {
                msgCode.showMsg(628);
            }
        }*/
        /**获得宠物 */
        public void GetPet(JObject res)
        {
            face.petInterface.savePet(res);
            msgCode.showMsg(860, res["nickName"].ToString());
        }



    }
}
