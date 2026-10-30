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

namespace Assets.HotFix.xq2d.src.ui.childPage.talk
{
    class AddChatEnsPage : PageUI
    {
        private Action<JObject> callback;
        private JObject en;
        public static AddChatEnsPage create(Transform parent)
        {
            Vector2 size = new Vector2(ScreenUtils.width, ScreenUtils.height);
            GameObject one = gameObjPool.getInstance().get("AddChatEnsPage", typeof(AddChatEnsPage));
            AddChatEnsPage bs = one.GetComponent<AddChatEnsPage>();
            bs.setSize(size).putSence<AddChatEnsPage>(parent).setScreenCenter();
            bs.drawUI();

            DoGet.getInstance().startReqImg();
            return bs;
        }
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("添加附件");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();
            });
            this.setSureCallback(() =>
            {
                if (en == null)
                {
                    msgCode.showMsg(861);
                    return;
                }
                this.freeThisPage();
                callback(en) ;
            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }
        public void addCallback(Action<JObject> callback)
        {
            this.callback = callback;
        }

        private int tabIndex;

        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("道具");
            ml.Add("宠物");
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

            JArray arr = new JArray();
            JArray list = null;
            if (index == 0)
            {
                list = face.goodsInterface.getEnclosure();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject d = (JObject)list[i];
                    GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(d["key"].ToString());
                    string str = "Lv " + gd.lv + " " + ((int)d["isBind"] == 0 ? "" : "[绑定]") + " " + gd.name;
                    if (d["forging"] != null && (int)d["forging"]["lv"] > 0) str += " +" + d["forging"]["lv"];
                    getOneMsg(str, arr);
                }
            }
            else
            {
                list = face.petInterface.getPetList();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject d = (JObject)list[i];
                    string str = "Lv " + d["lv"] + " " + d["nickName"];
                    getOneMsg(str, arr);
                }
            }


            GoodsItem gdItem = GoodsItem.create(arr, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                JObject playerGoods = (JObject)list[mIndex];
                int itemIndex = mIndex;
                List<string> ml = new List<string>();
                ml.Add("查看");
                ml.Add("选择");

                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((mIndex) =>
                {
                    if (mIndex == 0)
                    {
                        GoodsDesUI.create(playerGoods, this.transform).renderText();
                    }
                    else
                    {
                        this.en = new JObject();
                        this.en["type"] = tabIndex;//0道具1宠物
                        this.en["Id"] = playerGoods["Id"].ToString();
                        this.en["key"] = playerGoods["key"].ToString();
                        if (tabIndex == 0)
                        {
                            GoodsDes ms=(GoodsDes) face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                            this.en["name"] = ms.name;
                        }
                        else
                        {
                            this.en["name"] = playerGoods["nickName"].ToString();
                        }
                        this.freeThisPage();
                        callback(this.en);
                    }
                });
            });
        }
        
        public override void init()
        {

        }
    }
}
