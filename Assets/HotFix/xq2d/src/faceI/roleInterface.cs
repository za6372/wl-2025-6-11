using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.Res.script.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.model;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface roleInterface
    {
        public JObject getRole();
        public bool saveRole(JToken role);
        public void saveRoleAttr(JToken attr);
        public void saveUser(string username, string password);
        public JObject getUser();
        public void saveRolePos(string mapKey);
        public void uploadRolePos();
        void initRolePos(string mapKey = "m_1", JObject pos = null);
        public void isUploadPos(Vector3 pos);

        public void loadData(Action ac);
        //public void savePosToCache(string mapKey, float x, float y, float z);
        public bool addExp(int exp, int limitLv);
        public void updateMoney(int v, int type);
        public bool isInMap(string mapKey);
        public bool isInNoTeamMap();
        public bool isInNoPkMap();

        public void updateRoleSkill(JObject skl);
        public bool isEnoughMoney(string key, int price);
        public void saveRoleAttrEquip(object equip, string part);
        public void createRole(string name, string model, Action<object> callback);
        public void getPlayerBaseMsg(string name, Action<JObject> callback);
        public void getPlayerMsg(string name, Action<JObject> callback);
        public void initCh(JArray titles);
        public JArray getCh();
        public void saveCh(string key);
        public bool isExistCh(string key);
        public void changeTitle(string key, Action callback);
        public void initWings(JArray wings);
        public JArray getWings();
        public void saveWings(string key);
        public void changeWings(string key, Action callback);
        public bool isExistWings(string key);
        public void initZuoqi(JArray zuoqi);
        public JArray getZuoqi();
        public void saveZuoqi(string key);
        public bool isExistZuoqi(string key);
        public void clkStarAttr(string key, Action callback);
        public void realmBreak(Action callback);
        public string[] getTitleNames();
        public string getJob();
        public void upXrmf(int type, Action callback);
        public void upShengwang(int exp);
        public void setStatusPk(int isOpen, Action callback);
        public void setStatusBjb(int isOpen, Action callback);
        public void setStatusLan(string key, int isOpen, Action<int> callback);
        public void saveGameSetting(JObject gs);
        public JObject getGameSetting();
        public float getMoveSpeed();
        public void joinMenPai(string npcKey);
        public void joinFenTang(string npcKey);
        public void changeModel(int type, Action ac);
        public void addLvByBtn(Action ac);
        public bool isAllowedUpLv();
        public JObject getXrmf();
        public JObject getPetEquip();
        public bool isUseQdxc();
        public void clearOverTimeStatus(JArray list);
        public void updateStatus(JObject status0);
        public int getVipLv();
        public int getVipLv(int exp);
    }
    class roleInterfaceImpl : roleInterface
    {
        public int getVipLv()
        {
            JObject role = this.getRole();
            return getVipLv((int)role["attr"]["msg"]["vip"]);
        }
        public int getVipLv(int exp)
        {
            //累计的成长度
            int[] arr = {
                100, 2000, 5000, 10000, 20000,
                50000, 100000, 300000, 500000, 1000000
        };
            int sum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (exp >= arr[i]) sum++;
            }
            return sum;
        }
        public JObject getXrmf()
        {
            JObject r = this.getRole();
            return (JObject)r["attr"]["msg"]["xrmf"];
        }
        public JObject getPetEquip()
        {
            JObject r = this.getRole();
            return (JObject)r["attr"]["petEquip"];
        }
        /**更换角色形象*/
        public void changeModel(int type, Action ac)
        {
            JObject r = face.roleInterface.getRole();
            JArray models = strUtils.strToJSONObj<JArray>(r["models"].ToString());
            if (type >= models.Count)
            {
                msgCode.showMsg(977);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/manService/changeModel", dic, (res) =>
            {
                r["model"] = models[type];
                this.saveRole(r);
                msgCode.showMsg(200);
                ac();
            });
        }
        public void joinFenTang(string npcKey)
        {
            JObject r = face.roleInterface.getRole();
            if ((int)r["lever"] < 30)
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
            if (job != null)
            {
                msgCode.showMsg(974);
                return;
            }

            JArray models = strUtils.strToJSONObj<JArray>(r["models"].ToString());
            string sex = GameAttrConst.getSexFromModels(r);
            string md = null;
            int type = 0;
            if (npcKey.Equals("10000458"))
            {
                md = "ms";
                type = 0;
            }
            else if (npcKey.Equals("10000459"))
            {
                md = "dj";
                type = 1;
            }
            else if (npcKey.Equals("10000464"))
            {
                md = "qm";
                type = 2;
            }
            else if (npcKey.Equals("10000465"))
            {
                md = "ty";
                type = 3;
            }
            else if (npcKey.Equals("10000471"))
            {
                md = "ym";
                type = 4;
            }
            else if (npcKey.Equals("10000470"))
            {
                md = "lc";
                type = 5;
            }
            md += "_" + sex;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/manService/joinFenTang", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    models.Add(md);
                    r["models"] = models.ToString();
                    JArray skls = (JArray)r["attr"]["skill"];
                    for (int i = 0; i < skls.Count(); i++)
                    {
                        JObject skl = (JObject)skls[i];
                        if (!skl.ContainsKey("key")) continue;
                        string sklKey = skl["key"].ToString();
                        if (GameAttrConst.isMenPaiSklKey(sklKey))
                        {
                            //直接覆盖旧的技能
                            String newSklKey = GameAttrConst.mpSklToJobSkl(md, sklKey);
                            skl["key"] = newSklKey;
                        }
                    }
                    this.saveRole(r);
                    msgCode.showMsg(975);
                }
            });
        }
        public void joinMenPai(string npcKey)
        {
            JObject r = face.roleInterface.getRole();
            if ((int)r["lever"] < 15)
            {
                msgCode.showMsg(613);
                return;
            }
            string mp = GameAttrConst.getMenPaiFromModels(r);
            if (mp != null)
            {
                msgCode.showMsg(972);
                return;
            }

            JArray models = strUtils.strToJSONObj<JArray>(r["models"].ToString());
            string sex = GameAttrConst.getSexFromModels(r);
            string md = null;
            int type = 0;
            if (npcKey.Equals("10000457"))
            {
                md = "zs";
                type = 0;
            }
            else if (npcKey.Equals("10000463"))
            {
                md = "fs";
                type = 1;
            }
            else if (npcKey.Equals("10000469"))
            {
                md = "fz";
                type = 2;
            }
            md += "_" + sex;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/manService/joinMenPai", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    models.Add(md);
                    r["models"] = models.ToString();
                    this.saveRole(r);
                    msgCode.showMsg(973);
                }
            });
        }
        public float getMoveSpeed()
        {
            JObject a = getGameSetting();
            if (a.ContainsKey("moveSpeedK")) return 300f * (float)a["moveSpeedK"];
            return 300f;
        }

        public void saveGameSetting(JObject gs)
        {
            dbHandle.save("GameSetting", gs);
        }
        public JObject getGameSetting()
        {
            JObject a = dbHandle.get<JObject>("GameSetting");
            if (a == null) return new JObject();
            return a;
        }
        /**关闭过期的状态*/
        public void clearOverTimeStatus(JArray list)
        {
            JObject role = getRole();
            JObject status = (JObject)role["attr"]["status"];
            for (int i = 0; i < list.Count; i++)
            {
                string k = list[i].ToString();
                if (status.ContainsKey(k))
                {
                    status[k]["isOpen"] = 0;
                    JObject a = (JObject)status[k];
                    a.Remove("sy");
                }
            }
            saveRole(role);
        }
        public void updateStatus(JObject status0)
        {
            JObject role = getRole();
            JObject status = (JObject)role["attr"]["status"];
            foreach(JProperty p in status0.Properties())
            {
                string k = p.Name.ToString();
                JObject a = (JObject)status[k];
                JObject b=(JObject)status0[k];
                if (b.ContainsKey("sy") && a.ContainsKey("sy"))
                {
                    a["sy"] = b["sy"];
                }
            }

            saveRole(role);
        }
        /**是否正在使用驱敌香草*/
        public bool isUseQdxc()
        {
            JObject role = getRole();
            JObject status = (JObject)role["attr"]["status"];
            if (status.ContainsKey("qdxc") && (int)status["qdxc"]["isOpen"] == 1) return true;
            return false;
        }
        /**境界突破*/
        public void setStatusLan(string key, int isOpen, Action<int> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            dic.Add("isOpen", isOpen);
            DoGet.getInstance().sendPost("/manService/setStatusLan", dic, (res) =>
            {

                if (res.ToString().Equals("-1"))
                {
                    callback(-1);
                }
                else
                {
                    JObject role = getRole();
                    JObject status = (JObject)role["attr"]["status"];
                    status[key] = (JObject)res;
                    saveRole(role);
                    callback(1);
                }
            });
        }
        /**境界突破*/
        public void setStatusPk(int isOpen, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("isOpen", isOpen);
            DoGet.getInstance().sendPost("/manService/setStatusPk", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    JObject role = getRole();
                    role["attr"]["status"]["pk"]["isOpen"] = isOpen;
                    saveRole(role);
                    callback();
                }
            });
        }
        /**境界突破*/
        public void setStatusBjb(int isOpen, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("isOpen", isOpen);
            DoGet.getInstance().sendPost("/manService/setStatusBjb", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    JObject role = getRole();
                    role["attr"]["status"]["bjb"]["isOpen"] = isOpen;
                    saveRole(role);
                    callback();
                }
            });
        }
        /**提升声望*/
        public void upShengwang(int exp)
        {
            JObject role = getRole();
            JObject swdj = (JObject)role["attr"]["msg"]["swdj"];
            upShengwangCount(swdj, exp);
            saveRole(role);
        }
        private JObject upShengwangCount(JObject swdj, int exp)
        {
            int lv = (int)swdj["lv"] + 1;
            int old = (int)swdj["exp"];
            int max = (int)(lv * 100 + 2 * Math.Pow(5, 0.1 * lv));
            int d = exp + old - max;
            if (d >= 0)
            {
                //发生升级
                swdj["lv"] = lv;
                swdj["exp"] = 0;
                upShengwangCount(swdj, d);
            }
            else
            {
                //未发生升级
                swdj["exp"] = exp + old;
            }
            return swdj;
        }
        /**提升仙人秘法*/
        public void upXrmf(int type, Action callback)
        {
            JObject role = face.roleInterface.getRole();
            JObject xrmf = (JObject)role["attr"]["msg"]["xrmf"];
            int lv = 0;
            string xhKey = "10000162";
            if (type == 0) lv = (int)xrmf["xrLv"] + 1;
            else
            {
                lv = (int)xrmf["xfLv"] + 1;
                xhKey = "10000163";
            }
            if (lv > 30)
            {
                msgCode.showMsg(656);
                return;
            }
            int tale = 0;
            int xhNum = 0;
            if (type == 0)
            {
                tale = lv * 1000;
                int[] xhNums = { 4, 5, 6, 7, 9, 11, 13, 15, 17, 27, 33, 39, 45, 51, 61, 71, 81, 91, 101, 125 };
                if (lv > 10)
                {
                    xhNum = xhNums[lv - 11];
                    if (lv > 11) tale = (lv - 10) * 10000;
                }
            }
            else
            {
                tale = lv * 1000;
                if (lv > 10) xhNum = lv - 10;
            }
            if (xhNum > 0 && !face.goodsInterface.isEnoughInPackAndTip(xhKey, xhNum))
            {
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/manService/upXrmf", dic, (res) =>
            {
                if (xhNum > 0) face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, xhNum);
                if (res.ToString().Equals("1"))
                {
                    if (type == 0) role["attr"]["msg"]["xrmf"]["xrLv"] = lv;
                    else role["attr"]["msg"]["xrmf"]["xfLv"] = lv;
                    msgCode.showMsg(200);
                }
                else
                {
                    msgCode.showMsg(948);
                }
                saveRole(role);
                updateMoney(-tale, 1);
                callback();
            });
        }
        public string getJob()
        {
            JObject role = getRole();
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            for (int i = 0; i < models.Count; i++)
            {
                string jb = models[i].ToString();
                if (jb.Contains("ms_") || jb.Contains("dj_") || jb.Contains("qm_") || jb.Contains("ty_") || jb.Contains("ym_") || jb.Contains("lc_")) return jb;
            }
            return null;
        }
        /**获取称号名字*/
        public string[] getTitleNames()
        {
            return new string[]{
                "初露峥嵘","勤劳小蜜蜂","锋芒毕露","独孤求败","技冠群雄","天下第一",
            };
        }
        /**境界突破*/
        public void realmBreak(Action callback)
        {
            JObject role = getRole();
            if ((int)role["lever"] < 100)
            {
                msgCode.showMsg(915);
                return;
            }
            int realmLv = (int)role["attr"]["msg"]["realmLv"];
            JObject gd = face.goodsInterface.getPlayerGoodsByKey("1155");
            int num = 0;
            if (gd != null) num = (int)gd["num"];
            if (num < 9 * (realmLv + 1))
            {
                msgCode.showMsg(637);
                return;
            }
            DoGet.getInstance().sendPost("/manService/realmBreak", null, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey("1155", 9 * (realmLv + 1));
                    role["attr"]["msg"]["realmLv"] = realmLv + 1;
                    saveRole(role);
                    msgCode.showMsg(916);
                    callback();
                }
            });
        }
        /**点亮人物魔神星级*/
        public void clkStarAttr(string key, Action callback)
        {
            //判断所需材料是否充足
            string gk = this.attrKeyToGoodsKey(key);
            JObject g = face.goodsInterface.getPlayerGoodsByKey(gk);
            GoodsDes goods = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(gk);
            JObject r = face.roleInterface.getRole();
            int n = 10 + 10 * (int)r["attr"]["star"]["num"];
            if (g == null || (int)g["num"] < n)
            {
                msgCode.showMsg(856, goods.name + " x" + n);
                return;
            }

            if ((int)r["attr"]["star"]["num"] >= 12)
            {
                msgCode.showMsg(858);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/manService/clkStarAttr", dic, (res) =>
            {
                //缓存
                if (res.ToString().Equals("1"))
                {
                    //r["attr"]["star"]["num"] = (int)r["attr"]["star"]["num"] + 1;
                    face.goodsInterface.cutPlayerGoodsNum(g["Id"].ToString(), n);
                    r["attr"]["star"]["attr"][key] = 1;
                    bool b = true;
                    JObject attr = (JObject)r["attr"]["star"]["attr"];
                    IEnumerable<JProperty> props = attr.Properties();
                    foreach (JProperty p in props)
                    {
                        if ((int)p.Value == 0)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (!b)
                    {
                        //未全部点亮
                        r["attr"]["star"]["attr"] = attr;
                    }
                    else
                    {
                        //全部点亮了
                        r["attr"]["star"]["num"] = (int)r["attr"]["star"]["num"] + 1;
                        IEnumerable<JProperty> props1 = attr.Properties();
                        foreach (JProperty p in props1)
                        {
                            attr[p.Value] = 0;
                        }
                        r["attr"]["star"]["attr"] = attr;
                    }
                    saveRole(r);

                    callback();
                }

            });

        }
        private string attrKeyToGoodsKey(string key)
        {
            switch (key)
            {
                case "wg":
                    return "111000";
                case "fg":
                    return "111001";
                case "max_xue":
                    return "111002";
                case "wf":
                    return "111003";
                case "ff":
                    return "111004";
                case "max_lan":
                    return "111005";
                case "bj":
                    return "111006";
                case "mz":
                    return "111007";
                case "css":
                    return "111008";
                case "sd":
                    return "111009";
            }
            return null;
        }
        //待上传的位置对象
        private JObject posObj = null;
        public void initRolePos(string mapKey = "m_1", JObject pos = null)
        {
            this.posObj = new JObject();
            this.posObj["isUp"] = 0;
            this.posObj["time"] = strUtils.getMillis();
            this.posObj["map"] = mapKey;
            if (pos == null) this.posObj["pos"] = face.mapInterface.getMapByKey(mapKey).manPos;
            else this.posObj["pos"] = pos;


        }
        /**保存玩家的位置 */
        public void saveRolePos(string mapKey)
        {
            this.posObj["map"] = mapKey;
        }
        /**位置发生变动时调用*/
        public void isUploadPos(Vector3 v3)
        {
            //这个位置是相对于mape的位置
            this.posObj["isUp"] = 0;
            JObject pos = new JObject();
            pos.Add("x", v3.x);
            pos.Add("y", v3.y);
            pos.Add("z", v3.z);
            //放入内存中，等待线程调用上传时使用
            this.posObj["pos"] = pos;
            //缓存到本地
            this.savePosToCache(this.posObj["map"].ToString(), v3.x, v3.y, v3.z);
        }
        /**上传人物新位置到服务器 */
        public void uploadRolePos()
        {

            long now = strUtils.getMillis();
            if ((int)this.posObj["isUp"] == 0
                && now - (long)this.posObj["time"] >= 5000)
            {
                this.posObj["isUp"] = 1;
                this.posObj["time"] = strUtils.getMillis();
                //获取控制者位置
                JObject msg = new JObject();
                msg.Add("map", this.posObj["map"]);
                msg.Add("pos", this.posObj["pos"]);

                DoGet.getInstance().sendWs("/onlineService/syncPos", msg);
            }
        }
        /**获取玩家基本信息*/
        public void getPlayerBaseMsg(string name, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/manService/getPlayerBaseMsg", dic, (res) =>
            {

                callback((JObject)res);
            });
        }
        public void getPlayerMsg(string name, Action<JObject> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("playerName", name);
            DoGet.getInstance().sendPost("/manService/getPlayerMsg", dic, (res) =>
            {
                JObject r = (JObject)res;
                JObject msg = new JObject();
                //去掉lv
                msg["lever"] = r["lever"];
                msg["model"] = r["model"];
                msg["models"] = r["models"];
                msg["name"] = name;
                msg["attr"] = new JObject();
                msg["attr"]["equip"] = r["equip"];
                msg["attr"]["msg"] = r["msg"];
                msg["attr"]["prop"] = r["prop"];
                msg["attr"]["skill"] = r["skill"];
                msg["attr"]["star"] = r["star"];
                msg["attr"]["msLv"] = r["msLv"];
                msg["attr"]["shenfu"] = r["shenfu"];
                msg["attr"]["petEquip"] = r["petEquip"];

                callback(msg);
            });
        }
        /**创建角色 */
        public void createRole(string name, string model, Action<object> callback)
        {
            //判断是否昵称被限制
            if (!strUtils.isEnoughLen(name, 1, 6))
            {
                msgCode.showMsg(618);
                return;
            }
            JObject user = this.getUser();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("username", user["username"].ToString());
            dic.Add("name", name);
            dic.Add("model", model);
            dic.Add("projRootDir", sysUtils.projRootDir);
            DoGet.getInstance().sendPost("/loginService/createRole", dic, (res) =>
            {
                callback(res);
            });

        }
        /**保存角色装备 */
        public void saveRoleAttrEquip(object equip, string part)
        {
            JObject r = this.getRole();
            if (equip.Equals("")) r["attr"]["equip"][part] = (string)equip;
            else r["attr"]["equip"][part] = (JObject)equip;
            //缓存角色属性，不上传
            this.saveRoleAttr(r["attr"]);
            //更新人物属性页
            this.noticeUpdateUI();
        }
        /**是否在指定地图*/
        public bool isInMap(string mapKey)
        {
            JObject r = this.getRole();
            if (r["pos"]["map"].ToString().Equals(mapKey))
            {
                return true;
            }
            return false;
        }
        /**不允许pk的地图（点击玩家弹出的菜单中不会出现pk等战斗菜单项）*/
        public bool isInNoPkMap()
        {
            JObject r = this.getRole();
            string k = r["pos"]["map"].ToString();
            if (k.Equals("abyss1") || k.Equals("bz") || k.Equals("cbd") || k.Equals("dsx") || k.Equals("gangs") ||
                k.Equals("kongmiao") || k.Equals("md1") || k.Equals("petxl") || k.Equals("ps") ||
                k.Equals("smbz") || k.Equals("wzy") || k.Equals("yhmk"))
            {
                return true;
            }
            return false;
        }
        /**不允许组队的场景*/
        public bool isInNoTeamMap()
        {
            JObject r = this.getRole();
            string k = r["pos"]["map"].ToString();
            if (k.Equals("abyss1") || k.Equals("bz") || k.Equals("cbd") || k.Equals("dsx") ||
                k.Equals("kongmiao") || k.Equals("md1") || k.Equals("petxl") ||
                k.Equals("smbz") || k.Equals("wzy"))
            {
                return true;
            }
            return false;
        }
        /**缓存人物技能 */
        public void updateRoleSkill(JObject skl)
        {
            JObject role = this.getRole();
            JArray list = (JArray)role["attr"]["skill"];
            for (int p = 0; p < list.Count; p++)
            {
                JObject obj = (JObject)list[p];
                if (obj.ContainsKey("key") && obj["key"].ToString().Equals(skl["key"].ToString()))
                {
                    IEnumerable<JProperty> ps = skl.Properties();
                    foreach (JProperty a in ps)
                    {
                        list[p][a.Name] = a.Value;
                    }
                    this.saveRole(role);
                    return;
                }
            }
            list.Add(skl);
            this.saveRole(role);
        }
        /**载入角色数据*/
        public void loadData(Action ac)
        {
            DoGet doGet = DoGet.getInstance();
            doGet.sendPost("/manService/loadMsg", null, (res) =>
            {
                //Debug.Log(res);
                JObject data = (JObject)res;
                face.roleInterface.saveRoleAttr(data["manAttr"]);
                face.taskInterface.initTaskList((JArray)data["taskMap"]["playerTask"],
                    (JArray)data["taskMap"]["overTask"], (JArray)data["taskMap"]["taskIndex"]);
                face.playerInterface.initPlayerPos();
                face.goodsInterface.initGoods(data["goods"]);
                face.emailInterface.initEmailList((JArray)data["emails"]);
                face.teamInterface.initTeam();
                face.petInterface.initPetList((JArray)data["petList"]);
                //face.huobanInterface.initHuobanList((JArray)data["huobanList"]);
                face.msgInterface.init();
                face.friendInterface.initFriendList((JArray)data["friendList"]);
                face.roleInterface.initCh((JArray)data["titles"]);
                face.roleInterface.initWings((JArray)data["wings"]);
                face.roleInterface.initZuoqi((JArray)data["zuoqi"]);
                face.goodsInterface.saveQiangGouGoods(new JObject());

                ac();

            });
        }
        /**获取可装备的称号 */
        public JArray getCh()
        {
            return dbHandle.get<JArray>("myCh");
        }
        /**切换称号*/
        public void changeTitle(string key, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/manService/changeTitle", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    JObject r = this.getRole();
                    r["attr"]["msg"]["ch"] = key;
                    this.saveRole(r);
                    callback();
                }
                else
                {
                    msgCode.showMsg(761);
                }
            });
        }
        /**是否存在该称号*/
        public bool isExistCh(string key)
        {
            JArray arr = this.getCh();
            foreach (object p in arr)
            {
                if (p.ToString().Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        /**保存称号 */
        public void saveCh(string key)
        {
            if (isExistCh(key)) return;
            JArray arr = this.getCh();
            arr.Add(key);
            this.initCh(arr);
        }
        /**初始化拥有的称号 */
        public void initCh(JArray titles)
        {
            dbHandle.save("myCh", titles);
        }
        /**初始化翅膀 */
        public void initWings(JArray wings)
        {
            dbHandle.save("myWings", wings);
        }
        public JArray getWings()
        {
            return dbHandle.get<JArray>("myWings");
        }
        public bool isExistWings(string key)
        {
            JArray arr = this.getWings();
            foreach (object p in arr)
            {
                if (p.ToString().Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        /**切换翅膀*/
        public void changeWings(string key, Action callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/manService/changeWings", dic, (res) =>
            {
                if (res.ToString().Equals("1"))
                {
                    JObject r = this.getRole();
                    r["attr"]["msg"]["wings"] = key;
                    this.saveRole(r);
                    callback();
                }
                else
                {
                    msgCode.showMsg(761);
                }
            });
        }
        public void saveWings(string key)
        {
            if (isExistWings(key)) return;
            JArray arr = this.getWings();

            arr.Add(key);
            this.initWings(arr);
        }
        /**初始化坐骑 */
        public void initZuoqi(JArray zuoqi)
        {
            dbHandle.save("myZuoqi", zuoqi);
        }
        public JArray getZuoqi()
        {
            return dbHandle.get<JArray>("myZuoqi");
        }
        public bool isExistZuoqi(string key)
        {
            JArray arr = this.getZuoqi();
            foreach (object p in arr)
            {
                if (p.ToString().Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        public void saveZuoqi(string key)
        {
            if (isExistZuoqi(key)) return;
            JArray arr = this.getZuoqi();

            arr.Add(key);
            this.initZuoqi(arr);
        }
        /**判断金额是否充足（需要传入负值）*/
        public bool isEnoughMoney(string key, int price)
        {
            if (price > 0) { Debug.Log("值需要负数"); return false; }
            JObject role = this.getRole();
            if ((int)role["attr"]["msg"][key] + price >= 0) return true;
            return false;
        }
        private string getMoneyType(int i)
        {
            string k = null;
            if (i == 0) k = "gold";
            else if (i == 1) k = "tale";
            else if (i == 2) k = "yp";
            else if (i == 3) k = "jf";
            else if (i == 4) k = "wxz";
            else if (i == 5) k = "bg";
            else if (i == 6) k = "xld";
            else if (i == 7) k = "bb";
            else if (i == 8) k = "jmPoint";
            return k;
        }
        /**按原来的值进行累计的 */
        public void updateMoney(int v, int i)
        {
            JObject role = this.getRole();
            string k = getMoneyType(i);
            long sum = (long)role["attr"]["msg"][k] + v;
            if (sum > int.MaxValue)
            {
                sum = int.MaxValue;
                msgCode.showMsg(202);
            }
            role["attr"]["msg"][k] = sum;
            this.saveRole(role);
            //刷新人物属性页
            this.noticeUpdateUI();
            //刷新背包
            JObject j = new JObject();
            j.Add("callback", "818");
            eventsUtils.dispatchEvent("ws", j);
        }
        
        /**增加角色经验 */
        public bool addExp(int exp, int limitLv)
        {
            //增加经验
            JObject role = getRole();
            this.upLv(exp, limitLv);
            JObject role2 = getRole();
            if ((int)role["lever"] < (int)role2["lever"])
            {
                int n = (int)role2["lever"] - (int)role["lever"];
                face.chatInterface.writeSysMsg("恭喜您荣升" + role2["lever"] + "级！力量、智力、耐力、精神、敏捷+" + n);
                return true;
            }
            this.noticeUpdateUI();
            return false;
        }
        /**是否满足升级*/
        public bool isAllowedUpLv()
        {
            JObject r = this.getRole();
            int oldLv = (int)r["lever"];
            int max_exp = (int)getExpData(oldLv);
            JObject prop = (JObject)r["attr"]["prop"];
            if ((int)prop["exp"] < max_exp) return false;
            return true;
        }
        /**点击升级按钮*/
        public void addLvByBtn(Action ac)
        {
            JObject r = this.getRole();
            int oldLv = (int)r["lever"];
            if (oldLv < 30) return;
            int max_exp = (int)getExpData(oldLv);
            JObject prop = (JObject)r["attr"]["prop"];
            if ((int)prop["exp"] < max_exp) return;
            //突破任务是否完成
            string taskKey = null;
            if (oldLv == 49) taskKey = "1112";
            else if (oldLv == 59) taskKey = "1113";
            else if (oldLv == 69) taskKey = "1114";
            else if (oldLv == 79) taskKey = "1115";
            else if (oldLv == 89) taskKey = "1116";
            else if (oldLv == 99) taskKey = "1117";
            if (taskKey != null)
            {
                //判断突破任务是否已经完成
                if (!face.taskInterface.isExistCommitTask(taskKey))
                {
                    msgCode.showMsg(978);
                    return;
                }
            }

            DoGet.getInstance().sendPost("/manService/addLvByBtn", null, (res) =>
            {
                if (addExp(0, 100))
                {
                    //发生升级后才会检测任务的开启
                    face.taskInterface.checkTaskStart(()=> {
                        //刷新任务面板
                        face.taskInterface.noticeUIUpdate(0);
                    });
                    
                }
                ac();
            });
        }
        /**
         * 获取当前等级提升到下一级所需经验
         */
        private double getExpData(int lever)
        {
            return lever * 100 + 2 * Math.Pow(5, 0.1 * lever);
        }
        /**
         * 升级计算
         */
        public void countLv(JObject j)
        {
            int lv = (int)j["lever"];
            if (lv >= 100) return;
            int inputExp = (int)j["inputExp"];
            double jy = getExpData(lv);
            int max_exp = (int)jy;
            if (inputExp >= max_exp)
            {
                inputExp -= max_exp;
                lv += 1;
                j["inputExp"] = inputExp;
                j["lever"] = lv;
                this.countLv(j);
            }

        }
        public bool upLv(int exp, int limitLv)
        {
            JObject item = this.getRole();
            int oldLv = (int)item["lever"];

            JObject prop = (JObject)item["attr"]["prop"];

            JObject temp = new JObject();
            temp["inputExp"] = (int)prop["exp"] + exp;
            temp["lever"] = oldLv;
            //低于30级才直接计算等级，否则直接加经验（30-100需要按升级按钮来升级）
            if (oldLv < limitLv)
            {
                countLv(temp);
            }
            else
            {
                //经验不能超过当前等级的极限的5倍
                int max_exp = (int)getExpData(oldLv) * 5;
                if ((int)temp["inputExp"] > max_exp)
                {
                    temp["inputExp"] = max_exp;
                }
            }

            //50-100都需要完成突破任务  最多累计当前未突破等级的5倍
            int newLv = (int)temp["lever"];
            if (newLv >= 50)
            {
                String taskKey = null;
                int limit = 49;
                if (oldLv < 50 && newLv >= 50)
                {
                    taskKey = "1112";
                    limit = 49;
                }
                else if (oldLv < 60 && newLv >= 60)
                {
                    taskKey = "1113";
                    limit = 59;
                }
                else if (oldLv < 70 && newLv >= 70)
                {
                    taskKey = "1114";
                    limit = 69;
                }
                else if (oldLv < 80 && newLv >= 80)
                {
                    taskKey = "1115";
                    limit = 79;
                }
                else if (oldLv < 90 && newLv >= 90)
                {
                    taskKey = "1116";
                    limit = 89;
                }
                else if (oldLv < 100 && newLv >= 100)
                {
                    taskKey = "1117";
                    limit = 99;
                }
                if (taskKey != null)
                {
                    //判断突破任务是否已经完成
                    if (face.taskInterface.isExistCommitTask(taskKey))
                    {
                        //对于完成的等级就往上提升
                    }
                    else
                    {
                        //未完成的就阻止等级提升
                        //距离突破等级相差的等级
                        int disLv = newLv - (limit + 1);
                        int sum = 0;
                        //如：48升51，disLv=1 跨越了50级，所以计算50所需
                        double jy;
                        int max_exp;
                        for (int i = 0; i < disLv; i++)
                        {
                            //计算跨级累计
                            jy = getExpData(limit + i + 1);
                            max_exp = (int)jy;
                            sum += max_exp;
                        }
                        sum += (int)temp["inputExp"];
                        //当前等级的极限exp
                        jy = getExpData(limit);
                        max_exp = (int)jy;
                        //总经验=当前突破等级所需经验+跨级经验累计
                        sum += max_exp;
                        if (sum > max_exp * 5)
                            sum = max_exp * 5;
                        //等级不变，经验累计
                        temp["inputExp"] = sum;
                        temp["lever"] = limit;
                    }
                }
            }
            prop["exp"] = (int)temp["inputExp"];
            newLv = (int)temp["lever"];
            bool b = false;

            //发生升级
            if (oldLv < newLv)
            {
                item["lever"] = newLv;
                //获取新等级数据
                JObject baseAttr = countUtils.countProp(item);
                //升级后xue、lan补满
                item["attr"]["prop"]["xue"] = baseAttr["max_xue"];
                item["attr"]["prop"]["lan"] = baseAttr["max_lan"];
                b = true;
            }
            this.saveRole(item);
            return b;
        }

        public void noticeUpdateUI()
        {
            JObject j = new JObject();
            j.Add("callback", "820");
            eventsUtils.dispatchEvent("ws", j);
        }
        /**将位置信息保存到本地*/
        private void savePosToCache(string mapKey, float x, float y, float z)
        {
            JObject r = getRole();
            if (r["pos"] == null)
            {
                r["pos"] = new JObject();
            }
            if (mapKey != null)
                r["pos"]["map"] = mapKey;
            JObject pos = new JObject();
            pos.Add("x", x);
            pos.Add("y", y);
            pos.Add("z", z);
            r["pos"]["pos"] = pos;

            saveRole(r);
        }


        /**保存角色属性 */
        public void saveRoleAttr(JToken attr)
        {
            JObject r = this.getRole();
            //Debug.Log(r);
            r["attr"] = attr;
            this.saveRole(r);
        }
        /**保存角色信息 */
        public bool saveRole(JToken role)
        {
            return dbHandle.save("role", role);
        }
        /**获取角色信息 */
        public JObject getRole()
        {
            var jb = dbHandle.get<JObject>("role");
            if (jb == null)
            {
                jb = new JObject();
            }
            if (jb != null && jb["pos"] == null)
            {
                JObject j = new JObject();
                j.Add("x", 400f);
                j.Add("y", 400f);
                jb["pos"] = j;
            }
            if (jb != null && jb["attr"] == null)
            {
                jb["attr"] = new JObject();
            }
            return jb;
        }
        public void saveUser(string username, string password)
        {
            dbHandle.save("user", new user(username, password));
        }
        public JObject getUser()
        {
            return dbHandle.get<JObject>("user");
        }

    }



    class user
    {
        public string Id;
        public string username;
        public string password;
        public string created;
        public user(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

    }
}
