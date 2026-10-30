using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.childPage.email
{
    class AddEnsPage : PageUI
    {
        private JObject email;
        private Action callback;
        public static AddEnsPage create(JObject email, Transform parent)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("AddEnsPage", typeof(AddEnsPage));
            AddEnsPage bs = one.GetComponent<AddEnsPage>();
            bs.setSize(size).putSence<AddEnsPage>(parent).setScreenCenter();
            bs.drawUI(email);

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public void drawUI(JObject email)
        {
            this.email = email;
            this.createStandardPageLayout();
            this.setTitle("添加附件");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
            });
            this.setSureCallback(() =>
            {
                this.freeThisPage();
                callback();
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }
        public void addCallback(Action callback)
        {
            this.callback = callback;
        }



        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("道具");
            ml.Add("宠物");
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

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 60), new Vector2(30, -30), k2.transform);
        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);

            JArray ens = (JArray)this.email["enclosure"];

            JArray arr = new JArray();
            JArray list = null;
            if (index == 0)
            {
                list = face.goodsInterface.getEnclosure();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject d = (JObject)list[i];
                    for (int j = 0; j < ens.Count; j++)
                    {
                        if ((int)d["enType"] == 0 && d["Id"].ToString().Equals(ens[j]["Id"].ToString()))
                        {
                            d["nowNum"] = ens[j]["nowNum"];
                            break;
                        }
                    }

                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(d["key"].ToString());
                    string str = "Lv " + gd.lv + " " + ((int)d["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                    if (d["forging"] != null && (int)d["forging"]["lv"] > 0) str += " +" + d["forging"]["lv"];
                    str += " x" + d["nowNum"];
                    getOneMsg(str, arr);
                }
            }
            else
            {
                list = face.petInterface.getEnclosure();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject d = (JObject)list[i];
                    for (int j = 0; j < ens.Count; j++)
                    {
                        if ((int)d["enType"] == 1 && d["Id"].ToString().Equals(ens[j]["Id"].ToString()))
                        {
                            d["nowNum"] = ens[j]["nowNum"];
                            break;
                        }
                    }
                    Pet gd = face.petInterface.getPetDataByKey(d["key"].ToString());
                    string str = "Lv " + d["lever"] + " 成" + d["growLv"] + " " + gd.name + " x" + d["nowNum"];
                    getOneMsg(str, arr);
                }
            }


            GoodsItem gdItem = GoodsItem.create(arr, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject playerGoods = (JObject)list[mIndex];
                int itemIndex = mIndex;
                List<string> ml = new List<string>();
                if (index == 0)
                {
                    ml.Add("查看");
                    ml.Add("选择数量");
                }
                else
                {
                    ml.Add("查看");
                    ml.Add("确认选择");
                }

                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((mIndex2) =>
                {
                    string n = ml[mIndex2];
                    if (n.Equals("查看"))
                    {
                        if (index == 0) { GoodsDesUI.create(playerGoods, this.transform).renderText(); }
                        else
                        {
                            PageUI.createAcPage<PetPage>(this.transform).drawUI(false, playerGoods);
                        }

                    }
                    else if (n.Equals("确认选择"))
                    {
                        Pet gd = face.petInterface.getPetDataByKey(playerGoods["key"].ToString());
                        string str = "Lv " + playerGoods["lever"] + " 成" + playerGoods["growLv"] + " " + gd.name + " x1";
                        for (int j = 0; j < ens.Count; j++)
                        {
                            if ((int)ens[j]["enType"] == 1 && playerGoods["Id"].ToString().Equals(ens[j]["Id"].ToString()))
                            {
                                ens[j]["nowNum"] = 1;
                                gdItem.getItemByIndex(itemIndex).Find("text").GetComponent<TextUI>().setText(str);
                                return;
                            }
                        }
                        playerGoods["nowNum"] = 1;
                        ens.Add(playerGoods);
                        //刷新item的数量
                        gdItem.getItemByIndex(itemIndex).Find("text").GetComponent<TextUI>().setText(str);
                    }
                    else if (n.Equals("选择数量"))
                    {
                        UseNumInputUI.create(this.transform).addCallback((num) =>
                        {
                            if (ens.Count >= 3)
                            {
                                msgCode.showMsg(786);
                                return;
                            }
                            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                            string str = "Lv " + gd.lv + " " + ((int)playerGoods["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                            if (playerGoods["forging"] != null && (int)playerGoods["forging"]["lv"] > 0) str += " +" + playerGoods["forging"]["lv"];
                            str += " x" + num;

                            for (int j = 0; j < ens.Count; j++)
                            {
                                if ((int)ens[j]["enType"] == 0 && playerGoods["Id"].ToString().Equals(ens[j]["Id"].ToString()))
                                {
                                    ens[j]["nowNum"] = num;
                                    gdItem.getItemByIndex(itemIndex).Find("text").GetComponent<TextUI>().setText(str);
                                    return;
                                }
                            }
                            playerGoods["nowNum"] = num;
                            ens.Add(playerGoods);
                            //刷新item的数量
                            gdItem.getItemByIndex(itemIndex).Find("text").GetComponent<TextUI>().setText(str);
                        });
                    }
                });
            });
        }
        public override void init()
        {

        }
    }
}
