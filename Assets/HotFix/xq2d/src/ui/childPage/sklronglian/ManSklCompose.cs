using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.childPage.sklronglian
{
    class ManSklCompose : PageUI
    {
        private Action<JArray> callback;
        private JArray ens = new JArray();
        public static ManSklCompose create(Transform parent)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("ManSklCompose", typeof(ManSklCompose));
            ManSklCompose bs = one.GetComponent<ManSklCompose>();
            bs.setSize(size).putSence<ManSklCompose>(parent).setScreenCenter();
            bs.drawUI();

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("选择技能");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
            });
            this.setSureCallback(() =>
            {
                this.freeThisPage();
                callback(ens);
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }
        public void addCallback(Action<JArray> callback)
        {
            this.callback = callback;
        }

        private int tabIndex;

        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("技能");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                this.tabIndex = mIndex;
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
        private void getOneMsg(string name, string key, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            a["key"] = key;
            list.Add(a);
        }
        private bool isSkl(string k)
        {
            string[] list = {
                "100210030054", "100210030055", "100210030056",
                "100210030057", "100210030058", "100210030059", "100210030060", "100210030061",
                "100210030062", "100210030063", "100210030064", "100210030065", "100210030066", "100210030067",
                "100210030068", "100210030069","100210030071","100210030073"


            };
            foreach (string l in list)
            {
                if (l.Equals(k)) return true;
            }
            return false;
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);

            JArray arr = new JArray();
            JArray list = new JArray();
            JArray all = face.goodsInterface.getAllGoods();
            for (int i = 0; i < all.Count; i++)
            {
                JObject d = (JObject)all[i];
                if (!this.isSkl(d["key"].ToString())) continue;
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(d["key"].ToString());
                string str = "Lv " + gd.lv + " " + ((int)d["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                str += " x" + (d["nowNum"] == null ? 0 : d["nowNum"]);
                getOneMsg(str, d["key"].ToString(), arr);
                list.Add(d);
            }

            GoodsItem gdItem = GoodsItem.create(arr, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject playerGoods = (JObject)list[mIndex];
                int itemIndex = mIndex;
                List<string> ml = new List<string>();
                ml.Add("查看");
                ml.Add("选择数量");

                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((mIndex2) =>
                {
                    string n = ml[mIndex2];
                    if (n.Equals("查看"))
                    {
                        GoodsDesUI.create(playerGoods, this.transform).renderText();

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
                            str += " x" + num;

                            for (int j = 0; j < ens.Count; j++)
                            {
                                if (playerGoods["key"].ToString().Equals(ens[j]["key"].ToString()))
                                {
                                    ens[j]["nowNum"] = num;
                                    gdItem.getItemByIndex(itemIndex).Find("text").GetComponent<TextUI>().setText(str);
                                    return;
                                }
                            }
                            playerGoods["nowNum"] = num;
                            playerGoods["name"] = gd.name;
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
