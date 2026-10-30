using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.xq2d.src.factory.manager;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class PetPage : PageUI
    {
        private JArray pets;
        private JObject petData;//出战宠物
        private JObject old;
        private bool isMe;
        private JObject xrmf;
        private JObject petEquip;
        public void drawUI(bool isMe, JObject data = null)
        {
            this.isMe = isMe;
            this.createStandardPageLayout();
            this.setTitle("宠物");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            if (isMe)
            {
                xrmf = face.roleInterface.getXrmf();
                petEquip = face.roleInterface.getPetEquip();
            }
            Transform content = this.getStandardPageContent();
            if (data == null)
            {
                pets = face.petInterface.getPetList();
                for (int i = 0; i < pets.Count; i++)
                {
                    if (pets[i]["isFight"].ToString().Equals("1"))
                    {
                        this.setPetData((JObject)pets[i]);
                        break;
                    }
                }
                drawK1(content);
                DoGet.getInstance().startReqImg();
            }
            else
            {
                pets = new JArray();
                pets.Add(data);
                if (!isMe&& data!=null)
                {
                    xrmf = (JObject)data["xrmf"];
                    petEquip = (JObject)data["petEquip"];
                }
                this.setPetData(data);
                drawK1(content);
                DoGet.getInstance().startReqImg();
            }
        }
        private Action sureCall;
        public PetPage addBuyBtn(Action sureCall)
        {
            this.sureCall = sureCall;
            Transform bt = this.transform.Find("Bottom");

            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bt.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            btn1.GetComponent<BtnUI>().addText("购买", 30).setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                if (sureCall != null) sureCall();
                this.free();
            });
            btn1.GetComponent<BtnUI>().getTextUI().gameObject
                .AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));

            DoGet.getInstance().startReqImg();
            return this;
        }

        public void updateWindow()
        {
            Tab tab = this.getStandardPageContent().Find("Tab").GetComponent<Tab>();
            tab.clkDefault(tab.chooseIndex);
        }

        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("列表");
            ml.Add("能力");
            ml.Add("属性");
            ml.Add("资质");
            ml.Add("技能");
            

            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);
            tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));

            //黑色部分
            GameObject hb = gameObjPool.getInstance().get("hb", typeof(ImgUI));
            hb.transform.SetParent(k2.transform, false);
            hb.GetComponent<ImgUI>().setColor32(PageSetting.HbColor)
                .setSizePos(new Vector2(size.x, 400), Vector2.zero);


            GameObject sure = gameObjPool.getInstance().get("save", typeof(BtnUI));
            sure.transform.SetParent(this.transform.Find("Bottom"), false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("保存").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    saveAttrPoint();
                });
            sure.SetActive(false);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Vector2 size = this.getStandardPageContent().GetComponent<RectTransform>().sizeDelta;
            Transform k2 = this.getStandardPageContent().Find("k2");
            if (k2.Find("bgStyle1") != null)
                gameObjPool.getInstance().free(k2.Find("bgStyle1").gameObject);
            if (k2.Find("hb") != null)
            {
                gameObjPool.getInstance().freeChildren(k2.Find("hb").gameObject);
            }
            this.transform.Find("Bottom/save").gameObject.SetActive(false);
            if (index == 0)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 100), Vector2.zero);

                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 100), new Vector2(30, -30 - 100), k2.transform);
                Transform hb = k2.Find("hb");
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(hb.transform, false);
                text.GetComponent<TextUI>().setText("携带数量:1/10").setAlign().setColor32(PageSetting.FontColor).setFontSize(30).setFontStyle()
                    .setSizePos(new Vector2(size.x, 100), Vector2.zero);
                //text.AddComponent<UIOutline>().setSome(new Color32(0, 147, 150, 255), new Vector2(1, -1));

                this.drawPetList();
            }
            else if (index == 1)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 500), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 500), new Vector2(30, -30 - 500), k2.transform);

                this.drawNl();
            }
            else if (index == 2)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.drawAttr();
                if (isMe)
                    this.transform.Find("Bottom/save").gameObject.SetActive(true);
            }
            else if (index == 3)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.drawZizhi();
            }
            else if (index == 4)
            {
                this.getStandardPageContent().Find("k2/hb").GetComponent<ImgUI>()
                .setSizePos(new Vector2(size.x, 0), Vector2.zero);
                bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160 - 0), new Vector2(30, -30 - 0), k2.transform);

                this.drawSkill();
            }
            
        }
        private void setGetNum(int num, int sum)
        {
            this.getStandardPageContent().Find("k2/hb").GetChild(0).GetComponent<TextUI>().setText("携带数量:" + num + "/" + sum);
        }
        private bool isNoFight()
        {
            if (petData == null) return true;
            return false;
        }
        private void drawSkill()
        {
            if (isNoFight()) return;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JArray skls = (JArray)petData["attr"]["skill"];
            JArray list = new JArray();
            for (int i = 0; i < skls.Count; i++)
            {
                JObject skl = (JObject)skls[i];
                string str = null;
                if ((int)skl["isOpen"] != 1)
                {
                    str = "未开启";
                }
                else if (skl["key"] == null)
                {
                    str = "普通技能槽";
                }
                else
                {
                    Skill a = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                    str = a.name + " (" + (a.triggerType == 0 ? "主动" : "被动") + ")";
                }
                getOneMsg(str, list);
            }
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject a = (JObject)skls[mIndex];
                if (!a.ContainsKey("key")) return;
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getTipCanvas());
                dialog.renderText(skl.getSkillDes((int)a["lv"]), "center");
                DoGet.getInstance().startReqImg();
            });
        }
        private void drawZizhi()
        {
            if (isNoFight()) return;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            Pet data = face.petInterface.getPetDataByKey(petData["key"].ToString());
            JObject qualityValue = (JObject)petData["qualityValue"];
            //资质受品质、成长等影响，所以不能直接使用zizhi属性
            JObject now = data.getRealQualityGround((int)petData["quality"], (float)petData["grow"], (int)petData["growLv"]);
            JArray list = new JArray();
            string[] arr = { "xue", "lan", "wg", "fg", "wf", "ff", "mz", "sd", "bj", "css" };
            for (int i = 0; i < arr.Length; i++)
            {
                string key = arr[i];
                string str = GameAttrConst.propKeyToName(key + "_zz");
                if (key.Equals("xue")) key = "max_xue";
                else if (key.Equals("lan")) key = "max_lan";
                str = str + ":" + qualityValue[key] + "/" + now[key]["max"];
                getOneMsg(str, list);
            }
            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {

            });
        }
        private void drawAttr()
        {
            if (isNoFight()) return;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();

            petData["xrmf"]= xrmf;
            petData["petEquip"] = petEquip;
            JObject prop = countUtils.countPetProp(petData);

            if (true)
            {
                GameObject left = gameObjPool.getInstance().get("left", typeof(ImgUI));
                left.transform.SetParent(content.transform, false);
                left.GetComponent<ImgUI>().setRoundedCorners(0.1f, "#ADD6C2")
                    .setSizePos(new Vector2(420, 650), new Vector2(10, -50));
                left.AddComponent<UIOutline>().setSome(new Color32(102, 169, 139, 255), new Vector2(2, -2));

                GameObject point = gameObjPool.getInstance().get("sy", typeof(TextUI));
                point.transform.SetParent(content.transform, false);
                point.GetComponent<TextUI>().setText("剩余属性点 " + this.petData["propPoint"]).setAlign("left").setColor("#0CA894").setFontSize(40).setFontStyle().horiOut()
                    .setSizePos(new Vector2(400, 100), new Vector2(20, -50));
                //point.AddComponent<UIOutline>().setSome(Color.white, new Vector2(1, -1));

                string[] arr = { "ll", "mj", "zl", "nl", "js" };
                for (int i = 0; i < arr.Length; i++)
                {
                    string k = arr[i];
                    int index = i;
                    GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
                    item.transform.SetParent(left.transform, false);
                    item.GetComponent<SimpleUI>()
                        .setSizePos(new Vector2(420, 100), new Vector2(0, -i * 110 - 100));

                    GameObject attrName = gameObjPool.getInstance().get("attrName", typeof(TextUI));
                    attrName.transform.SetParent(item.transform, false);
                    attrName.GetComponent<TextUI>().setText(GameAttrConst.propKeyToName(k)).setAlign().setColor("#0CA894").setFontSize(40).setFontStyle()
                        .setSizePos(new Vector2(120, 100), new Vector2(0, 0));
                    //attrName.AddComponent<UIOutline>().setSome(Color.white, new Vector2(1, -1));

                    GameObject attrValue = gameObjPool.getInstance().get("attrValue", typeof(TextUI));
                    attrValue.transform.SetParent(item.transform, false);
                    attrValue.GetComponent<TextUI>().setText(this.petData["baseProp"][k].ToString()).setAlign("left").setColor("#0CA894").setFontSize(35).setFontStyle().horiOut()
                        .setSizePos(new Vector2(80, 100), new Vector2(120, 0));
                    //attrValue.AddComponent<UIOutline>().setSome(Color.white, new Vector2(1, -1));

                    GameObject cut = gameObjPool.getInstance().get("cut", typeof(BtnUI));
                    cut.transform.SetParent(item.transform, false);
                    cut.GetComponent<BtnUI>().setRoundedCorners(1, "#0CA894")
                        .setSizePos(new Vector2(80, 80), new Vector2(220, -10));
                    cut.GetComponent<BtnUI>().addText("-", 36).setTextColor("#ffffff")
                    .addLongClk(() =>
                    {
                        changeAttrPoint(-1, arr, index);
                    });
                    cut.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));

                    GameObject add = gameObjPool.getInstance().get("add", typeof(BtnUI));
                    add.transform.SetParent(item.transform, false);
                    add.GetComponent<BtnUI>().setRoundedCorners(1, "#0CA894")
                        .setSizePos(new Vector2(80, 80), new Vector2(220 + 90, -10));
                    add.GetComponent<BtnUI>().addText("+", 36).setTextColor("#ffffff")
                    .addLongClk(() =>
                    {
                        changeAttrPoint(1, arr, index);
                    });
                    add.AddComponent<UIOutline>().setSome(new Color32(134, 192, 185, 255), new Vector2(2, -2));
                }
            }
            if (true)
            {
                string[] arr = { "max_xue", "max_lan", "wg", "fg", "wf", "ff", "mz", "sd", "bj", "css" };

                GameObject right = gameObjPool.getInstance().get("right", typeof(ImgUI));
                right.transform.SetParent(content.transform, false);
                right.GetComponent<ImgUI>().setRoundedCorners(0.1f, "#ADD6C2")
                    .setSizePos(new Vector2(450, 90 * arr.Length), new Vector2(440, -50));
                right.AddComponent<UIOutline>().setSome(new Color32(102, 169, 139, 255), new Vector2(2, -2));
                //读取吃丹的属性
                JObject danAttr = null;
                if (petData.ContainsKey("danAttr"))
                {
                    danAttr = (JObject)petData["danAttr"];
                }
                for (int i = 0; i < arr.Length; i++)
                {
                    string key = arr[i];
                    GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
                    item.transform.SetParent(right.transform, false);
                    item.GetComponent<SimpleUI>()
                        .setSizePos(new Vector2(430, 80), new Vector2(10, -i * 90));
                    GameObject text = gameObjPool.getInstance().get("attrName", typeof(TextUI));
                    text.transform.SetParent(item.transform, false);
                    text.GetComponent<TextUI>().setColor("#0CA894").setAlign("left").setFontSize(35).setText(GameAttrConst.propKeyToName(key)).setFontStyle()
                        .setSizePos(new Vector2(80, 80), new Vector2(0, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(1, -1));
                    string v = ((int)prop[key]).ToString();
                    if (danAttr != null)
                    {
                        int max1 = face.petInterface.getMaxPetDanAttr(key, 1);
                        int max2 = face.petInterface.getMaxPetDanAttr(key, 2);
                        int max3 = face.petInterface.getMaxPetDanAttr(key, 3);
                        int v1 = (int)danAttr["d1"][key];
                        if (v1 > max1) v1 = max1;
                        int v2 = (int)danAttr["d2"][key];
                        if (v2 > max2) v2 = max2;
                        int v3 = (int)danAttr["d3"][key];
                        if (v3 > max3) v3 = max3;
                        v += " (" + ("<color=#1662fa>" + v1 + "</color>/") +
                            ("<color=#d03cd3>" + v2 + "</color>/") +
                            ("<color=#f9b024>" + v3 + "</color>") + ")";
                    }
                    GameObject ap = gameObjPool.getInstance().get("attrValue", typeof(TextUI));
                    ap.transform.SetParent(item.transform, false);
                    ap.GetComponent<TextUI>().setColor("#0CA894").setAlign("left").setFontSize(30).setFontStyle().horiOut()
                        .setText(v)
                        .setSizePos(new Vector2(300, 80), new Vector2(80, 0));
                    //ap.AddComponent<UIOutline>().setSome(Color.white, new Vector2(1, -1));
                }
            }

        }

        private void changeAttrPoint(int addPoint, string[] arr, int index)
        {
            if (!isMe) return;

            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();

            string key = arr[index];
            int lv = (int)petData["lever"];
            if ((addPoint == 1) && ((int)petData["propPoint"] <= 0)) return;
            else if ((addPoint == -1) && ((int)petData["baseProp"][key] <= 5 ||
                        (int)petData["baseProp"][key] <= (int)old["baseProp"][key])) return;
            //可分配点数--，长按时
            petData["propPoint"] = (int)petData["propPoint"] - addPoint;
            content.Find("sy").GetComponent<TextUI>().setText("剩余属性点 " + this.petData["propPoint"]);
            petData["baseProp"][key] = (int)petData["baseProp"][key] + addPoint;
            content.Find("left").GetChild(index).Find("attrValue").GetComponent<TextUI>()
                .setText(petData["baseProp"][key].ToString());

            petData["xrmf"] = xrmf;
            petData["petEquip"] = petEquip;
            JObject obj = countUtils.countPetProp(petData);
            //读取吃丹的属性
            JObject danAttr = null;
            if (petData.ContainsKey("danAttr"))
            {
                danAttr = (JObject)petData["danAttr"];
            }
            //刷新属性、血量面板
            string[] list = { "max_xue", "max_lan", "wg", "fg", "wf", "ff", "mz", "sd", "bj", "css" };
            for (int i = 0; i < list.Length; i++)
            {
                string k = list[i];
                string v = ((int)obj[k]).ToString();
                if (danAttr != null)
                {
                    v += " (" + ("<color=#1662fa>" + danAttr["d1"][k] + "</color>/") +
                        ("<color=#d03cd3>" + danAttr["d2"][k] + "</color>/") +
                        ("<color=#f9b024>" + danAttr["d3"][k] + "</color>") + ")";
                }
                content.Find("right").GetChild(i).Find("attrValue").GetComponent<TextUI>()
                .setText(v);
            }

        }
        private void saveAttrPoint()
        {
            if (!isMe) return;
            //判断新数据与旧数据是否发生改变
            JObject baseProp = (JObject)petData["baseProp"];
            bool isChange = false;
            IEnumerable<JProperty> ps = baseProp.Properties();
            foreach (JProperty p in ps)
            {
                string k = p.Name;
                if ((int)old["baseProp"][k] != (int)p.Value)
                {
                    isChange = true;
                    break;
                }
            }
            if (isChange)
            {
                //上传
                face.petInterface.updatePetPropPointAndBaseAttr((int)petData["propPoint"], baseProp, petData["Id"].ToString(), () =>
                {
                    this.old = strUtils.copyJSON<JObject>(this.petData);
                    msgCode.showMsg(200);
                });
            }
        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        private void drawNl()
        {
            if (isNoFight()) return;
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            Pet pet = face.petInterface.getPetDataByKey(this.petData["key"].ToString());
            JArray list = new JArray();
            getOneMsg("宠物忠诚度:1000/1000", list);
            getOneMsg("成长度:" + petData["growValue"] + "/1000", list);
            getOneMsg("成长等级:" + petData["growLv"], list);
            getOneMsg("品质:" + petData["quality"] + "品", list);
            getOneMsg("种类:" + pet.getTypeName((int)petData["growBreachLv"]), list);
            getOneMsg("出战等级:" + pet.fightLv, list);
            getOneMsg("技能悟性:" + petData["savvy"], list);
            getOneMsg("成长率:" + petData["grow"], list);
            getOneMsg("攻击类型:" + (petData["attr"]["prop"]["type"].ToString().Equals("0") ? "物理" : "法术"), list);

            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {

            });

            //属性页黑色部分
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            petData["xrmf"] = xrmf;
            petData["petEquip"] = petEquip;
            JObject prop = countUtils.countPetProp(petData);

            string pwd = pet.getIdlePwd((int)petData["growBreachLv"]);
            AmModelMsg am = amManager.getAmByKey(pwd);
            string[] aefs = { am.changguiAef };
            string[] pwds = am.changguiPwds;
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readModel.drawGameObj(hb, pwds, aefs);

                GameObject g = list[0].gameObject;
                g.transform.SetParent(hb, false);
                int len = g.transform.Find("frames").childCount;
                g.GetComponent<AniRuntime>().frameInterval = 0.25f;
                g.GetComponent<AniRuntime>().play(len / 2, len);

                g.transform.localPosition = new Vector2(-ScreenUtils.width / 2f + 200, -150);
                g.transform.localScale = Vector2.one * 5;

                GameObject lv = gameObjPool.getInstance().get("lv", typeof(ImgUI));
                lv.transform.SetParent(hb.transform, false);
                lv.GetComponent<ImgUI>().loadRes("jl04_png")
                    .setSizePos(new Vector2(33.4f, 26), new Vector2(30, -30));
                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(lv.transform, false);
                text.GetComponent<TextUI>().setText(petData["lever"].ToString()).setAlign("left").setColor("#FFEB00").setFontSize(26).setNoClk().setFontStyle()
                    .setSizePos(new Vector2(120, 30), new Vector2(36, 2));
                text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(1, -1));

                DoGet.getInstance().startReqImg();
            }, all);




            GameObject hp = gameObjPool.getInstance().get("hp", typeof(ImgUI));
            hp.transform.SetParent(hb.transform, false);
            hp.GetComponent<ImgUI>().loadRes("js01_png")
                .setSizePos(new Vector2(60, 36), new Vector2(400, -80));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(ImgUI));
            mp.transform.SetParent(hb.transform, false);
            mp.GetComponent<ImgUI>().loadRes("js02_png")
                .setSizePos(new Vector2(60, 36), new Vector2(400, -80 - 100));
            drawTiao(mp, 1, prop);

            GameObject exp = gameObjPool.getInstance().get("exp", typeof(ImgUI));
            exp.transform.SetParent(hb.transform, false);
            exp.GetComponent<ImgUI>().loadRes("js03_png")
                .setSizePos(new Vector2(60, 36), new Vector2(400, -80 - 200));
            drawTiao(exp, 2, prop);

        }

        private void drawTiao(GameObject attr, int i, JObject prop)
        {
            GameObject item = gameObjPool.getInstance().get("item", typeof(SimpleUI));
            item.transform.SetParent(attr.transform, false);
            item.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(320, 20), new Vector2(0, 0));
            string k1 = "xue";
            string k2 = "max_xue";
            Color32 color = new Color32(255, 16, 0, 255);
            Color32 color1 = new Color32(172, 11, 0, 255);
            if (i == 1)
            {
                k1 = "lan";
                k2 = "max_lan";
                color = new Color32(0, 174, 255, 255);
                color1 = new Color32(2, 140, 205, 255);
            }
            else if (i == 2)
            {
                k1 = "exp";
                k2 = "max_exp";
                color = new Color32(255, 183, 0, 255);
                color1 = new Color32(192, 137, 0, 255);
            }

            GameObject jdBg = gameObjPool.getInstance().get("jdBg", typeof(ImgUI));
            jdBg.transform.SetParent(item.transform, false);
            jdBg.GetComponent<ImgUI>().setColor("#555555")
                .setSizePos(new Vector2(320, 12), new Vector2(80, -21));
            float rate = (float)prop[k1] / (float)prop[k2];
            if (rate > 1) rate = 1;
            GameObject jdt = gameObjPool.getInstance().get("jdt", typeof(ImgUI));
            jdt.transform.SetParent(jdBg.transform, false);
            jdt.GetComponent<ImgUI>()
                .setSizePos(new Vector2(314 * rate, 6), new Vector2(3, -3));
            GradientDefinded gd = jdt.AddComponent<GradientDefinded>();
            gd.color1 = color;
            gd.color2 = color1;


            GameObject exp = gameObjPool.getInstance().get("str", typeof(TextUI));
            exp.transform.SetParent(jdBg.transform, false);
            exp.GetComponent<TextUI>().setColor("#ffffff").setAlign("right").setFontSize(24).setText((int)prop[k1] + "/" + (int)prop[k2])
                .setSizePos(new Vector2(320, 30), new Vector2(0, 30));
        }

        private void setPetData(JObject petData)
        {
            this.petData = petData;
            if (petData != null)
            {
                this.setTitle(petData["nickName"].ToString());
                
            }
            this.old = strUtils.copyJSON<JObject>(this.petData);
        }
        private void drawPetList()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();

            setGetNum(pets.Count, 20);

            GoodsItem gdItem = GoodsItem.create(pets, content.transform, 1);
            gdItem.addCallback((mIndex) =>
            {
                if (!isMe) return;
                JObject mPet = (JObject)pets[mIndex];
                int isFight = (int)mPet["isFight"];
                List<string> ml = new List<string>();
                if (isFight != 1) ml.Add("出战");
                if (isFight == 1) ml.Add("休息");
                if (isFight != 1) ml.Add("查看");
                ml.Add("改名");
                if (isFight != 1) ml.Add("抛弃");
                Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                menu.addCallback((mIndex) =>
                {
                    handle(ml[mIndex], mPet);

                });
                DoGet.getInstance().startReqImg();
            });
        }
        private void updateCache()
        {
            this.pets = face.petInterface.getPetList();
            this.updateWindow();
            this.setPetData(face.petInterface.getIsFightPet());
            PointGet.getIndexPage().GetComponent<IndexPage>().updatePetHeadIcon();
        }
        private void handle(string str, JObject pet)
        {
            switch (str)
            {
                case "出战":
                    {
                        face.petInterface.updateIsFightById(1, pet["Id"].ToString(), () =>
                        {
                            updateCache();
                        });
                        break;
                    }
                case "休息":
                    {
                        face.petInterface.updateIsFightById(0, pet["Id"].ToString(), () =>
                        {
                            updateCache();
                        });
                        break;
                    }
                case "查看":
                    {
                        PageUI.createAcPage<PetPage>(this.transform).drawUI(false, pet);
                        break;
                    }
                case "改名":
                    {
                        UseNumInputUI.create(PointGet.getIndexPageOfPage()).renderText("请输入宠物昵称").addTextCallback((bpName) =>
                        {
                            MsgSureUI.create(PointGet.getTipCanvas()).show("需要消耗宠物更名卡 x1，是否继续？", () =>
                            {
                                face.petInterface.setPetName(pet["Id"].ToString(), bpName, () =>
                                {
                                    updateCache();
                                });
                            }, () => { });
                        });
                        break;
                    }
                case "抛弃":
                    {
                        MsgSureUI.create(PointGet.getTipCanvas()).show("确定要抛弃它么？", () =>
                        {
                            face.petInterface.abandonPet(pet["Id"].ToString(), () =>
                            {
                                updateCache();
                            });
                        }, () => { });
                        break;
                    }
            }
        }
        
        public override void init()
        {

        }
    }
}
