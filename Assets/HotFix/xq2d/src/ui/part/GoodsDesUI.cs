using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class GoodsDesUI : PartUI
    {
        private JObject playerGoods;
        private int isShowAll;
        public static GoodsDesUI create(JObject goods, Transform parent)
        {
            return create(goods, parent, 1);
        }
        public static GoodsDesUI create(JObject goods, Transform parent, int isShowAll)
        {
            Vector2 size = new Vector2(800, 600);
            GameObject one = gameObjPool.getInstance().get("GoodsDesUI", typeof(GoodsDesUI));
            GoodsDesUI bs = one.GetComponent<GoodsDesUI>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<GoodsDesUI>(parent).setScreenCenter();
            bs.draw(goods, size, isShowAll);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public Transform getContent()
        {
            return this.transform.Find("kuang/bg/content");
        }
        private Action sureCall;
        public GoodsDesUI addBuyBtn(Action sureCall)
        {
            this.sureCall = sureCall;
            Vector2 size = this.transform.Find("kuang").GetComponent<RectTransform>().sizeDelta;
            Transform bg = this.transform.Find("kuang/bg");

            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(bg.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(0, -(size.y - 100)));
            btn.GetComponent<BtnUI>().addText("取消", 30).setTextColor().loadRes("gy_03_png").addClk(() =>
             {
                 this.free();
             });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bg.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(size.x - 160, -(size.y - 100)));
            btn1.GetComponent<BtnUI>().addText("购买", 30).setTextColor().loadRes("gy_02_png").addClk(() =>
             {
                 if (sureCall != null) sureCall();
                 this.free();
             });
            DoGet.getInstance().startReqImg();
            return this;
        }
        public GoodsDesUI renderText()
        {
            GameObject panel = gameObjPool.getInstance().get("panel", typeof(SimpleUI));
            ScrollUI scroll = getContent().GetComponent<ScrollUI>();
            scroll.GetComponent<ScrollUI>().setContent(panel);


            Vector2 size = panel.GetComponent<RectTransform>().sizeDelta;
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(panel.transform, false);
            text.GetComponent<TextUI>().setFontSize(30).setAlign("leftTop").setColor().setIsRichText(true).setLineSpacing(1.5f).VertiOut()
            .setSizePos(size, Vector2.zero);

            /*float h = text.GetComponent<TextUI>().getRichHeight();
            if (h < 960) h = 960;
            scroll.GetComponent<ScrollUI>().updateHeight(h);*/

            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey((string)playerGoods["key"]);
            string str = "<size=30><color=#FEF3D3>" + gd.des + "\n</color></size>";
            if (playerGoods["key"].ToString().Substring(0, 4).Equals("1002"))
            {
                Skill sk = (Skill)gd;
                str = "<size=30><color=#FEF3D3>" + sk.getSkillDes(1) + "\n</color></size>";
            }
            else if (playerGoods["key"].ToString().Substring(0, 4).Equals("1016"))
            {
                PetEquip sk = (PetEquip)gd;
                str = "<size=30><color=#FEF3D3>" + sk.getDes((long)playerGoods["endTime"]) + "\n</color></size>";
            }

            if (isShowAll == 0)//只展示部分
            {
                text.GetComponent<TextUI>().setText(str);
                return this;
            }

            if (playerGoods["num"] != null)
            {
                str += "<size=30><color=#FEF3D3>" + "数量：" + playerGoods["num"] + "\n</color></size>";
            }
            if (playerGoods["isBind"] != null)
                str += "<size=30><color=#FEF3D3>绑定：" + ((int)playerGoods["isBind"] == 1 ? "是" : "否") + "\n</color></size>";
            if (gd.type == 1)
            {//装备描述
                Equip equip = (Equip)gd;
                str += "<size=30><color=#FEF3D3>" + "职业：" +
                    GameAttrConst.getJobToName(equip.job) + "   部位：" +
                    GameAttrConst.equipPartKeyToName(equip.part) + "\n" +
                    "损坏：" + ((int)playerGoods["isBad"] == 1 ? "是" : "否") + "\n" +
                    "绑定宝石：" + (playerGoods.ContainsKey("bdbs") && (int)playerGoods["bdbs"] == 1 ? "是" : "否") + "\n" +
                    "刻印宝石：" + (playerGoods.ContainsKey("kybs") && (int)playerGoods["kybs"] == 1 ? "是" : "否") + "\n" +
                    "血契：" + (playerGoods.ContainsKey("xqbs") && (int)playerGoods["xqbs"] == 1 ? "是" : "否") + "\n" +
                    "</color></size>";
                if (!face.equipInterface.isFaBao(equip.key) && !face.equipInterface.isHuFu(equip.key))
                {
                    str += "<size=30><color=#FF00FF>" + "基础属性：" + "\n</color></size>";
                    str += listEquipFixedAttr(equip, playerGoods);
                }
                string jlStr = listEquipRandomAttr(playerGoods);
                if (jlStr != null)
                {
                    str += "<size=30><color=#FF00FF>" + "精炼属性：" + "\n</color></size>";
                    str += jlStr;
                }

                if (playerGoods["inlay"] != null)
                {
                    JArray arr = (JArray)playerGoods["inlay"]["list"];
                    if (arr.Count > 0)
                    {
                        str += "<size=30><color=#FF00FF>" + "宝石属性：" + "\n</color></size>";
                        foreach (object a in arr)
                        {
                            JObject aObj = (JObject)a;
                            Baoshi msg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(aObj["key"].ToString());
                            int v = (int)aObj["num"];
                            if (v > msg.prop) v = (int)msg.prop;
                            str += "<size=30><color=#FEF3D3>" + msg.name + " (" + v + "/" + (int)msg.prop + ")\n</color></size>";
                        }

                    }
                }
                if (playerGoods["kxlv"] != null)
                {
                    JObject kxlv = (JObject)playerGoods["kxlv"];
                    str += "<size=30><color=#FF00FF>" + "抗性：" + "\n</color></size>";
                    IEnumerable<JProperty> props = kxlv.Properties();
                    foreach (JProperty p in props)
                    {
                        if ((int)p.Value <= 0) continue;
                        string n = GameAttrConst.propKeyToName(p.Name.Substring(0, 2) + "kx");
                        int kx = 50 * (int)p.Value + (int)Math.Pow(2d, (int)p.Value);
                        str += "<size=30><color=#FEF3D3>" + n + ": +" + kx + " (Lv" + p.Value + ")\n</color></size>";
                    }
                }
                if (playerGoods["czmb"] != null)
                {
                    JObject czmb = (JObject)playerGoods["czmb"];
                    str += "<size=30><color=#FF00FF>" + "橙装模板属性：" + "\n</color></size>";
                    MoBan msg = (MoBan)face.goodsInterface.getGoodsMsgByKey(czmb["key"].ToString());
                    str += "<size=30><color=#FEF3D3>橙装魔力：" + czmb["num"] + "/" + msg.getMaxMoLi() + "\n</color></size>";
                    float k = (float)czmb["num"] / msg.getMaxMoLi();
                    if (k > 1) k = 1f;
                    fightAttr attr = msg.getMaxAttr();
                    PropertyInfo[] properties = attr.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (Convert.ToInt32(property.GetValue(attr)) == 0) continue;
                        string n = GameAttrConst.propKeyToName(property.Name);
                        int kx = Convert.ToInt32(Convert.ToInt32(property.GetValue(attr)) * k);
                        str += "<size=30><color=#FEF3D3>" + n + ": +" + kx + "/" + Convert.ToInt32(property.GetValue(attr)) + "\n</color></size>";
                    }

                }



                if (playerGoods["potential"] != null && (int)playerGoods["potential"]["max"] > 0)
                {
                    str += "<size=30><color=#FF00FF>" + "潜力：" + "\n</color></size>";
                    str += "<size=30><color=#FEF3D3>" + "剩余：" + playerGoods["potential"]["num"] + "/" + playerGoods["potential"]["max"] + "\n</color></size>";
                }
                if (equip.quality == 5 && equip.type == 1)
                {
                    str += "<size=30><color=#FF00FF>" + "套装特效：" + "\n</color></size>";
                    //str += this.equipTx(playerGoods);
                }
                if (playerGoods["skill"] != null)
                {
                    JArray skls = (JArray)playerGoods["skill"];
                    if (skls.Count > 0)
                    {
                        str += "<size=30><color=#FF00FF>" + "特技：" + "\n</color></size>";

                        foreach (object s in skls)
                        {
                            JObject skl = (JObject)s;
                            Skill a = (Skill)face.goodsInterface.getGoodsMsgByKey(skl["key"].ToString());
                            string color = GameAttrConst.getGoodsQualityColor(a.quality);
                            str += "<size=30><color=" + color + ">[" + a.name + "] -> " +
                                a.des + "\n</color></size>";
                        }
                    }
                }

                if (playerGoods["keyin"] != null)
                {
                    JObject keyin = (JObject)playerGoods["keyin"];
                    if (keyin.Count > 0)
                    {
                        str += "<size=30><color=#FF00FF>" + "刻印：" + "\n</color></size>";
                        IEnumerable<JProperty> properties = keyin.Properties();
                        foreach (JProperty item in properties)
                        {
                            if (item.Value == null || item.Value.ToString().Equals("")) continue;
                            GoodsDes a = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(item.Value["key"].ToString());
                            JObject fixedAttr = face.goodsInterface.getKyAttr(item.Value["key"].ToString());
                            //增幅
                            int forgingLv = 0;
                            if (item.Value["forging"] != null)
                            {
                                forgingLv = (int)item.Value["forging"]["lv"];
                            }

                            //按品质给颜色
                            string color = GameAttrConst.getGoodsQualityColor(a.quality);
                            str += "<size=30><color=" + color + ">[" + a.name.Substring(0, 2) + "] +" + forgingLv + " -> " +
                                GameAttrConst.propKeyToName(fixedAttr["k"].ToString()) + " +" +
                                fixedAttr["v"] + " (" + (int)(0.0375f * forgingLv * 100) + "%)" + "\n</color></size>";
                            //TODO:列出随机属性
                        }
                    }
                }
                /*str += "<size=30><color=#F0E68C>" + "雕纹：" + "\n</color></size>";
                str += "<size=30><color=#ffffff>" + "1件：-" + "\n</color></size>";
                str += "<size=30><color=#ffffff>" + "2件：-" + "\n</color></size>";
                str += "<size=30><color=#ffffff>" + "3件：-" + "\n</color></size>";
                str += "<size=30><color=#ffffff>" + "5件：-" + "\n</color></size>";*/
                if (playerGoods["capacity"] != null)
                {
                    str += "<size=30><color=#FF00FF>" + "容量：" + "\n</color></size>";
                    str += "<size=30><color=#FEF3D3>" + "" + playerGoods["capacity"]["num"] + " / " + equip.capacity.sum + "\n</color></size>";
                }

                //法宝等
                /*if (equip.part.Equals("fb") || equip.part.Equals("gf") || equip.part.Equals("hf"))
                {
                    str += "<size=30><color=#804400>" + "经验：" + playerGoods["forging"]["exp"] + " / " + countUtils.fbLvExp((int)playerGoods["forging"]["lv"]) + "\n</color></size>";
                }*/
                if (equip.part.Equals("fb"))
                {
                    str += "<size=30><color=#FF00FF>" + "法宝属性：" + "\n</color></size>";
                    int fgLv = (int)playerGoods["forging"]["lv"];
                    int lv = (int)playerGoods["zhuling"]["lv"];
                    int quality = (int)playerGoods["zhuling"]["quality"];
                    string[] hsType = { "神将化身", "仙灵化身", "禽兽化身" };
                    string[] hrr = { "人物", "宠物", "怪物" };
                    int hs = (int)playerGoods["forging"]["type"];
                    str += "<size=30><color=#FEF3D3>" + "灵气值：" + playerGoods["zhuling"]["lqz"] + "/" + 1000 + "\n</color></size>";
                    str += "<size=30><color=#FEF3D3>" + "熟练度：" + playerGoods["zhuling"]["sld"] + "/" + (1000 * lv + 4000) + "\n</color></size>";
                    str += "<size=30><color=#FEF3D3>" + hsType[hs] + " +" + fgLv + "\n</color></size>";
                    /*str += "<size=30><color=#FEF3D3>" + "增加对" + hrr[hs] + (0.1 * (quality - 1) + 0.01 * quality + 0.005 * fgLv * quality) * 100 +
                        "%的伤害，减少受到" + hrr[hs] + (0.1 * (quality - 1) + 0.01 * quality + 0.003 * fgLv * quality) * 100 + "%的伤害" + "\n</color></size>";*/
                    float addHurt = 0.01f * quality + 0.0107f * fgLv;
                    float cutHurt = 0.00625f * quality + 0.0066875f * fgLv;
                    if (hs == 2)
                    {//怪物需要除以2
                        addHurt /= 2f;
                        cutHurt /= 2f;
                    }
                    str += "<size=30><color=#FEF3D3>" + "增加对" + hrr[hs] + (addHurt * 100f) +
                                            "%的伤害，减少受到" + hrr[hs] + (cutHurt * 100f) + "%的伤害" + "\n</color></size>";
                }
                else if (equip.part.Equals("hf"))
                {
                    str += "<size=30><color=#FF00FF>" + "护符属性：" + "\n</color></size>";
                    int lv = 0;
                    JObject msg = (JObject)face.roleInterface.getRole()["attr"]["msg"];
                    if (msg.ContainsKey("swdj"))
                    {
                        lv = (int)msg["swdj"]["lv"];
                    }
                    Equip a = (Equip)gd;
                    List<fbResult> list = a.getHuFuResult();
                    foreach (fbResult f in list)
                    {
                        str += "<size=30><color=#FEF3D3>" + GameAttrConst.propKeyToName(f.key) + "：" + (int)f.getV(lv) + "/" + (int)f.getMaxV() + "\n</color></size>";
                    }


                }
            }
            else if (strUtils.isMatch(gd.key, "1090([0-9]{4})"))
            {//刻印
                str += "<size=30><color=#FF00FF>" + "基础属性：" + "\n</color></size>";
                JObject fixedAttr = face.goodsInterface.getKyAttr(gd.key);
                int forgingLv = 0;
                if (playerGoods["forging"] != null)
                {
                    forgingLv = (int)playerGoods["forging"]["lv"];
                }

                //按品质给颜色
                str += "<size=30><color=#804400>" +
                    GameAttrConst.propKeyToName(fixedAttr["k"].ToString()) + " +" +
                    fixedAttr["v"] + " (" + (int)float.Parse(0.0375f * forgingLv * 100 + "") + "%)" + "\n</color></size>";
            }

            text.GetComponent<TextUI>().setText(str);
            float h = text.GetComponent<Text>().preferredHeight;
            if (h < size.y) h = size.y;
            scroll.GetComponent<ScrollUI>().updateHeight(h);
            return this;
        }
        /**随机属性*/
        public string listEquipRandomAttr(JObject goods)
        {
            string content = null;
            if (goods["randomAttr"] == null) return content;
            JArray arr = (JArray)goods["randomAttr"];
            for (int i = 0; i < arr.Count; i++)
            {
                JObject a = (JObject)arr[i];
                string k = a["k"].ToString();
                int v = (int)a["v"];
                int max = face.equipInterface.getMaxJingLianAttr(goods["key"].ToString(), k);
                if (v > max) v = max;
                content += "<size=30><color=#FEF3D3>" + GameAttrConst.propKeyToName(k) + "：+" + v + "\n</color></size>";
            }
            return content;
        }
        /**固有属性*/
        public string listEquipFixedAttr(Equip goods, JObject equip)
        {
            string content = "";
            var attr = goods.fixedAttrRule;
            float k0 = 1;
            if (equip.ContainsKey("bdbs") && (int)equip["bdbs"] == 1)
            {
                k0 += 0.1f;
            }
            if (equip.ContainsKey("kybs") && (int)equip["kybs"] == 1)
            {
                k0 += 0.3f;
            }
            if (equip.ContainsKey("xqbs") && (int)equip["xqbs"] == 1)
            {
                k0 += 0.6f;
            }

            switch (goods.part)
            {
                case "wq":
                    {//武器 影响法攻、物攻 fg、wg
                        string wg = strUtils.strToInt(attr.wg * k0).ToString();
                        string fg = strUtils.strToInt(attr.fg * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wg += "(+" + strUtils.strToInt(obj["wg"]) + ")";
                            fg += "(+" + strUtils.strToInt(obj["fg"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物攻：" + wg + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法攻：" + fg + "\n</color></size>";
                        break;
                    }
                case "jb":
                    {//武器 影响法攻、物攻 fg、wg
                        string wg = strUtils.strToInt(attr.wg * k0).ToString();
                        string fg = strUtils.strToInt(attr.fg * k0).ToString();
                        string max_lan = strUtils.strToInt(attr.max_lan).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wg += "(+" + strUtils.strToInt(obj["wg"]) + ")";
                            fg += "(+" + strUtils.strToInt(obj["fg"]) + ")";
                            max_lan += "(+" + strUtils.strToInt(obj["max_lan"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物攻：" + wg + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法攻：" + fg + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "蓝量：" + max_lan + "\n</color></size>";
                        break;
                    }
                case "sz":
                    {//武器 影响法攻、物攻 fg、wg
                        string wg = strUtils.strToInt(attr.wg * k0).ToString();
                        string fg = strUtils.strToInt(attr.fg * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wg += "(+" + strUtils.strToInt(obj["wg"]) + ")";
                            fg += "(+" + strUtils.strToInt(obj["fg"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物攻：" + wg + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法攻：" + fg + "\n</color></size>";
                        break;
                    }
                case "wb":
                    {//武器 影响法攻、物攻 fg、wg
                        string wf = strUtils.strToInt(attr.wf * k0).ToString();
                        string ff = strUtils.strToInt(attr.ff * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wf += "(+" + strUtils.strToInt(obj["wf"]) + ")";
                            ff += "(+" + strUtils.strToInt(obj["ff"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物防：" + wf + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法防：" + ff + "\n</color></size>";
                        break;
                    }
                case "tb":
                    {//武器 影响法攻、物攻 fg、wg
                        string max_xue = strUtils.strToInt(attr.max_xue * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            max_xue += "(+" + strUtils.strToInt(obj["max_xue"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "血量：" + max_xue + "\n</color></size>";
                        break;
                    }
                case "xb":
                    {//武器 影响法攻、物攻 fg、wg
                        string max_xue = strUtils.strToInt(attr.max_xue * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            max_xue += "(+" + strUtils.strToInt(obj["max_xue"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "血量：" + max_xue + "\n</color></size>";
                        break;
                    }
                case "yb":
                    {//武器 影响法攻、物攻 fg、wg
                        string wf = strUtils.strToInt(attr.wf * k0).ToString();
                        string ff = strUtils.strToInt(attr.ff * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wf += "(+" + strUtils.strToInt(obj["wf"]) + ")";
                            ff += "(+" + strUtils.strToInt(obj["ff"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物防：" + wf + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法防：" + ff + "\n</color></size>";
                        break;
                    }
                case "tuib":
                    {//武器 影响法攻、物攻 fg、wg
                        string wf = strUtils.strToInt(attr.wf * k0).ToString();
                        string ff = strUtils.strToInt(attr.ff * k0).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wf += "(+" + strUtils.strToInt(obj["wf"]) + ")";
                            ff += "(+" + strUtils.strToInt(obj["ff"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物防：" + wf + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法防：" + ff + "\n</color></size>";
                        break;
                    }
                case "jiaob":
                    {//武器 影响法攻、物攻 fg、wg
                        string wf = strUtils.strToInt(attr.wf * k0).ToString();
                        string ff = strUtils.strToInt(attr.ff * k0).ToString();
                        string css = strUtils.strToInt(attr.css).ToString();
                        if (equip["forging"] != null && (int)equip["forging"]["lv"] > 0)
                        {
                            JObject obj = face.equipInterface.getAddAttrByForgingLv(equip);
                            wf += "(+" + strUtils.strToInt(obj["wf"]) + ")";
                            ff += "(+" + strUtils.strToInt(obj["ff"]) + ")";
                            css += "(+" + strUtils.strToInt(obj["css"]) + ")";
                        }
                        content += "<size=30><color=#FEF3D3>" + "物防：" + wf + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "法防：" + ff + "\n</color></size>";
                        content += "<size=30><color=#FEF3D3>" + "速度：" + css + "\n</color></size>";
                        break;
                    }
                case "bjb":
                    {
                        string max_xue = strUtils.strToInt(attr.max_xue * k0).ToString();
                        content += "<size=30><color=#FEF3D3>" + "血量：" + max_xue + "\n</color></size>";
                        break;
                    }
            }
            return content;
        }
        private GoodsDesUI draw(JObject goods, Vector2 size, int isShowAll)
        {
            this.playerGoods = goods;
            this.isShowAll = isShowAll;

            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setColor(0, 0, 0, 0f)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0)).addClk(() => { this.free(); });

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2, -(ScreenUtils.height - size.y) / 2));

            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.transform.SetParent(kuang.transform, false);
            bg.GetComponent<ImgUI>().setColor(PageUI.PageDefaltColor)
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(20, -20));
            //左顶
            GameObject bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, 0));
            //顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(9, 1, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, 0));
            //左
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l1_png", new Rect(0, 1, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(0, -20));
            //右顶
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 1, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, 0));
            //右
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l2_png", new Rect(1, 0, 9, 1))
            .setSizePos(new Vector2(20, size.y - 40), new Vector2(size.x - 20, -20));
            //左下
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(0, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(0, -(size.y - 20)));
            //底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l4_png", new Rect(9, 0, 1, 9))
            .setSizePos(new Vector2(size.x - 40, 20), new Vector2(20, -(size.y - 20)));
            //右底
            bian = gameObjPool.getInstance().get("bian", typeof(ImgUI));
            bian.transform.SetParent(kuang.transform, false);
            bian.GetComponent<ImgUI>().loadRes("l3_png", new Rect(1, 0, 9, 9))
            .setSizePos(new Vector2(20, 20), new Vector2(size.x - 20, -(size.y - 20)));

            //内容区

            GameObject scroll = gameObjPool.getInstance().get("content", typeof(ScrollUI));
            scroll.transform.SetParent(bg.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));
            scroll.GetComponent<ScrollUI>().initSetting();


            return this;
        }
    }
}
