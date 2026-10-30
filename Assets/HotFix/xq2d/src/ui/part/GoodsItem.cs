using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    /**道具项*/
    class GoodsItem : PartUI
    {
        /**
         type 0道具列表项 1宠物列表项 2商品列表项
         */
        private int type;
        private JArray list;

        public static GoodsItem create(object list, Transform parent, int type = 0)
        {
            Vector2 size = parent.GetComponent<RectTransform>().sizeDelta;
            GameObject one = gameObjPool.getInstance().get("GoodsItem", typeof(GoodsItem));
            GoodsItem bs = one.GetComponent<GoodsItem>();
            bs.setSize(size).putSence<GoodsItem>(parent).setLeftPos(Vector2.zero);
            bs.draw((JArray)list, size, type);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public Transform getItemByIndex(int index)
        {
            GameObject items = this.transform.Find("scroll").GetComponent<ScrollUI>().getContent();
            return items.transform.GetChild(index);
        }
        /**滑动到指定位置*/
        public GoodsItem scrollInpPos(Vector2 inp)
        {
            if (inp == default) return this;
            this.transform.Find("scroll").GetComponent<ScrollUI>().scrollInpPos(inp);
            return this;
        }
        public Vector2 getScrollPos()
        {
            return this.transform.Find("scroll").GetComponent<ScrollUI>().getNowPos();
        }
        private GoodsItem draw(JArray list, Vector2 size, int type)
        {
            this.type = type;
            this.list = list;
            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(this.transform, false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(20, -20));
            scroll.GetComponent<ScrollUI>().initSetting();

            GameObject items = gameObjPool.getInstance().get("items", typeof(SimpleUI));
            scroll.GetComponent<ScrollUI>().setContent(items);

            for (int i = 0; i < list.Count; i++)
            {
                int index = i;

                GameObject tb = gameObjPool.getInstance().get("item" + i, typeof(ImgUI));
                tb.transform.SetParent(items.transform, false);
                tb.GetComponent<ImgUI>().setAlpha(0).setRoundedCorners(0.5f, new Color32(0, 0, 0, 0))//.drawRound(0.2f, size.x - 40, 100)
                    .setSizePos(new Vector2(size.x - 40, 100), new Vector2(0, -110 * i))
                    .addClk(() =>
                    {
                        if (callback != null)
                        {
                            Transform items = this.transform.Find("scroll").GetComponent<ScrollUI>().getContent().transform;
                            for (int t = 0; t < items.transform.childCount; t++)
                            {
                                if (items.transform.GetChild(t).name == "item" + index)
                                {

                                    items.transform.GetChild(t).GetComponent<ImgUI>().setRoundedCorners(0.5f, PageSetting.ListItemColor);//.setColor32(PageSetting.ListItemColor);
                                }
                                else
                                {
                                    items.transform.GetChild(t).GetComponent<ImgUI>().setRoundedCorners(0.5f, new Color32(0, 0, 0, 0));//.setAlpha(0);
                                }
                            }
                            callback(index);
                        }
                    });


                JObject it = (JObject)list[i];

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(tb.transform, false);
                text.GetComponent<TextUI>().setText(getNameByType(it)).setAlign("left").setColor("#000000").setFontSize(PageSetting.FontSize).setFontStyle().setIsRichText()
                    .setSizePos(new Vector2(size.x - 60, 100), new Vector2(10, 0));
                /*Color color = Color.black;
                if (type < 0||type==1)
                {
                    color = Color.white;
                }
                text.AddComponent<UIOutline>().setSome(color, new Vector2(2, -2));*/

                if (type == 0)//物品列表
                {
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText("x" + it["num"]).setAlign("right").setColor("#f63a34").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }
                else if (type == 1)//宠物列表
                {
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText("Lv" + it["lever"]).setAlign("right").setColor("#f63a34").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }
                else if (type == 2)//商品列表
                {
                    text = gameObjPool.getInstance().get("price", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText(it["price"].ToString()).setAlign("right").setColor("#000000").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 170, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                    int imgIndex = 0;
                    if ((int)it["priceType"] == 0) imgIndex = 1;
                    else if ((int)it["priceType"] == 1) imgIndex = 2;
                    else if ((int)it["priceType"] == 2) imgIndex = 3;
                    GameObject tale = gameObjPool.getInstance().get("goldIcon", typeof(ImgUI));
                    tale.transform.SetParent(tb.transform, false);
                    tale.GetComponent<ImgUI>().loadRes("moneyImge_png", new Rect(13 * imgIndex, 0, 13, 13))
                        .setSizePos(new Vector2(50, 50), new Vector2(size.x - 40 - 60, -25));
                }
                else if (type == 3)//帮派列表
                {
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText(it["num"] + "/" + it["sum"]).setAlign("right").setColor("#f63a34").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }
                else if (type == 4)//帮派成员列表
                {
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText((int)it["online"] == 1 ? "在线" : "离线").setAlign("right").setColor("#f63a34").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }
                else if (type == 5)//帮派商品列表
                {
                    int priceType = (int)it["priceType"];
                    string dw = null;
                    if (priceType == 0) dw = "玉帛";
                    else if (priceType == 1) dw = "帮贡";
                    else if (priceType == 2) dw = "军功";
                    else if (priceType == 3) dw = "积分";
                    text = gameObjPool.getInstance().get("price", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText(it["price"].ToString() + dw).setAlign("right").setColor("#000000").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 170, 0));
                }
                else if (type == 6)//邮件列表
                {
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText(strUtils.sToTime((long)it["created"])).setAlign("right").setColor("#198585").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }
                else if (type == 7)//状态列表
                {
                    if (it["key"].ToString().Equals("pk") || it["key"].ToString().Equals("bjb") || it["key"].ToString().Equals("rwzhd") ||
                        it["key"].ToString().Equals("cwzhd") || it["key"].ToString().Equals("ydxc") || it["key"].ToString().Equals("qdxc") ||
                        it["key"].ToString().Equals("blx") || it["key"].ToString().Equals("jml") || it["key"].ToString().Equals("dblq"))
                    {
                        bool isOpen = false;
                        if (it.ContainsKey("isOpen")) isOpen = (int)it["isOpen"] == 1 ? true : false;
                        SwitchUI.create(new Vector2(size.x - 40 - 150, -20), tb.transform).setStatus(isOpen).addCall((b) =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("key", it["key"].ToString());
                            dic.Add("status", b);
                            btnCall(dic);
                        });
                    }
                    else if (it["key"].ToString().Equals("moveSpeed"))
                    {
                        //多档选择
                        JObject st = face.roleInterface.getGameSetting();
                        int ci = 0;
                        if (st.ContainsKey("moveSpeedK")) ci = (int)st["moveSpeedK"] - 1;
                        GearUI.create(new string[] { "x1", "x2", "x3" }, new Vector2(size.x - 40 - 210, -20), tb.transform).choose(ci).addCall((v) =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("key", it["key"].ToString());
                            dic.Add("value", v);
                            btnCall(dic);
                        });
                    }

                }
                else if (type == 8)//跑商
                {
                    string str = null;
                    if (it.ContainsKey("price")) str = it["price"].ToString();
                    else str = "x" + it["num"].ToString();
                    text = gameObjPool.getInstance().get("num", typeof(TextUI));
                    text.transform.SetParent(tb.transform, false);
                    text.GetComponent<TextUI>().setText(str).setAlign("right").setColor("#198585").setFontSize(PageSetting.FontSize).setFontStyle().horiOut()
                        .setSizePos(new Vector2(100, 100), new Vector2(size.x - 40 - 110, 0));
                    //text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                }


            }
            scroll.GetComponent<ScrollUI>().updateHeight(list.Count * 110);
            scroll.GetComponent<ScrollUI>().addTouchMoveScript(() =>
            {
                if (this.scrollTopCall != null) this.scrollTopCall();
            }, () =>
            {
                if (this.scrollBottomCall != null) this.scrollBottomCall();
            }, (vs) =>
            {
                if (this.scrollMoveCall != null) this.scrollMoveCall(vs);
            });
            return this;
        }
        /**根据类型解析名字*/
        private string getNameByType(JObject it)
        {
            if (type == 0)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(it["key"].ToString());
                if (gd == null) return "不存在物品";
                string color = GameAttrConst.getGoodsQualityColor(gd.quality);
                string str = "Lv " + gd.lv + " " + ((int)it["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                if (face.equipInterface.isEquip(gd.key))
                {
                    Equip ep = (Equip)gd;
                    str = ep.getPerfectName(it);
                }
                else if (face.equipInterface.isFaBao(gd.key))
                {
                    color = GameAttrConst.getGoodsQualityColor((int)it["zhuling"]["quality"]);
                    Equip ep = (Equip)gd;
                    str = ep.getPerfectName(it);
                }

                return "<color=" + color + ">" + str + "</color>";
            }
            else if (type == 1)
            {
                int quality = (int)it["quality"];
                string str = "";
                if (quality == 1) str = "一品";
                else if (quality == 2) str = "二品";
                else if (quality == 3) str = "三品";
                else if (quality == 4) str = "四品";

                Pet gd = face.petInterface.getPetDataByKey(it["key"].ToString());
                return ((int)it["isFight"] == 0 ? "" : "[战]") + " " + str + " " + it["nickName"];
            }
            else if (type == 2)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(it["key"].ToString());
                string color = GameAttrConst.getGoodsQualityColor(gd.quality);
                return "<color=" + color + ">" + "Lv " + gd.lv + " " + gd.name + "</color>";
            }
            else if (type == 3)
            {
                return "【" + it["lever"] + "级】 " + it["name"];
            }
            else if (type == 4)
            {
                string job = null;
                if ((int)it["job"] == 0) job = "帮主";
                else if ((int)it["job"] == 1) job = "副帮主";
                else if ((int)it["job"] == 2) job = "左护法";
                else if ((int)it["job"] == 3) job = "右护法";
                else if ((int)it["job"] == 4) job = "精英";
                else job = "帮众";
                return "[" + job + "] Lv" + it["lever"] + " " + it["name"] + " | " + it["bg"];
            }
            else if (type == 5)
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(it["key"].ToString());
                string color = GameAttrConst.getGoodsQualityColor(gd.quality);
                return "<color=" + color + ">" + "Lv " + gd.lv + " " + gd.name + "</color>";
            }
            else if (type == 6)
            {
                string str = it["title"].ToString();
                if (it["isRead"].ToString().Equals("0")) str += " [新]";
                if ((int)it["price"] > 0) str += " [付]";
                if (it["enclosure"] != null && ((JArray)it["enclosure"]).Count > 0)
                {
                    if (it["isObtain"].ToString().Equals("0")) str += " [附/未接]";
                    else str += " [附]";
                }

                if ((int)it["price"] > 0) str = "<color=#0877D6>" + str + "</color>";//付费为蓝色
                else if (it["isRead"].ToString().Equals("1")) str = "<color=#000000>" + str + "</color>";
                else if (it["isRead"].ToString().Equals("0")) str = "<color=#FF0000>" + str + "</color>";//未读取为红色
                return str;
            }
            else if (type == 7)
            {
                string str = GameAttrConst.statusKeyToName(it["key"].ToString());
                if (it.ContainsKey("sy"))
                {
                    str += " / 剩余时间：" + strUtils.nowTimeToEndTime((long)it["created"], (long)it["created"] + (long)it["sy"]);
                }
                return str;
            }
            else if (type == 8)
            {
                it["name"].ToString();
            }
            else if (type == 9)//装备
            {
                Equip gd = (Equip)face.goodsInterface.getGoodsMsgByKey(it["key"].ToString());
                if (gd == null) return "不存在物品";
                string color = GameAttrConst.getGoodsQualityColor(gd.quality);
                string str = gd.getPerfectName(it);
                if (face.equipInterface.isFaBao(gd.key))
                {
                    color = GameAttrConst.getGoodsQualityColor((int)it["zhuling"]["quality"]);
                }
                return "<color=" + color + ">" + str + "</color>";
            }

            return it["name"].ToString();
        }
        private Action<int> callback;
        public GoodsItem addCallback(Action<int> call)
        {
            this.callback = call;
            return this;
        }
        private Action<Dictionary<string, object>> btnCall;
        public GoodsItem addBtnCall(Action<Dictionary<string, object>> btnCall)
        {
            this.btnCall = btnCall;
            return this;
        }
        private Action scrollTopCall;
        private Action scrollBottomCall;
        private Action<Vector2> scrollMoveCall;
        public GoodsItem addScrollTopCall(Action scrollTopCall)
        {
            this.scrollTopCall = scrollTopCall;
            return this;
        }
        public GoodsItem addScrollBottomCall(Action scrollBottomCall)
        {
            this.scrollBottomCall = scrollBottomCall;
            return this;
        }
        public GoodsItem addScrollMoveCall(Action<Vector2> scrollMoveCall)
        {
            this.scrollMoveCall = scrollMoveCall;
            return this;
        }
    }
}
