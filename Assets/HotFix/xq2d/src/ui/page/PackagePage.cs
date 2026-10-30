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
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class PackagePage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("背包");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);


        }



        private void drawK1(Transform content)
        {

            //选项卡
            List<string> ml = new List<string>();
            ml.Add("全部");
            ml.Add("装备");
            ml.Add("宠物");
            ml.Add("神符");
            ml.Add("任务");
            ml.Add("仓库");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                if (mIndex < 5)
                {
                    drawTale();
                }
                else
                {
                    drawWare();
                }
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
                .setSizePos(new Vector2(size.x, 200), Vector2.zero);
            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 200 - 60), new Vector2(30, -230), k2.transform);

        }
        private JArray sortToList(JArray list)
        {
            JArray temp = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject a = (JObject)list[i];
                if (a["key"].ToString().Equals("10000000") || a["key"].ToString().Equals("10000001") || a["key"].ToString().Equals("10000002"))
                {
                    list.RemoveAt(i);
                    temp.Add(a);
                    i--;
                }
            }
            List<JObject> al = list.OrderByDescending(x =>
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(x["key"].ToString());
                if (gd == null)
                {
                    Debug.Log(x);
                    return 0;
                }
                return gd.quality;
            }).ThenByDescending(x =>
            {
                GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(x["key"].ToString());
                if (gd == null) return 0;
                return gd.lv;
            }).Select(x => JObject.FromObject(x)).ToList();

            temp.Merge(new JArray(al));

            return temp;
        }
        //列表旧位置
        private Vector2 scrollOldPos;
        /**绘制列表*/
        public void drawList(int index)
        {
            JArray list = null;
            if (index == 0)//所有
            {
                list = face.goodsInterface.getAllGoods();
                //对道具进行排序 龙头票在前，其他按品阶来
                list = sortToList(list);
            }
            else if (index == 1)//装备
            {
                list = face.goodsInterface.getNoWarehouseAnyTypeGoods("1001");
                //对道具进行排序 龙头票在前，其他按品阶来
                list = sortToList(list);
            }
            else if (index == 2)//宠物
            {
                list = new JArray();
            }
            else if (index == 3)//神符
            {
                list = new JArray();
            }
            else if (index == 4)//任务
            {
                list = new JArray();
            }
            else if (index == 5)//仓库
            {
                list = face.goodsInterface.getCkGoods();
                //对道具进行排序 龙头票在前，其他按品阶来
                list = sortToList(list);
            }
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);

            GoodsItem gdItem = GoodsItem.create(list, content.transform);
            gdItem.addCallback((mIndex) =>
            {
                JObject playerGoods = (JObject)list[mIndex];

                List<string> ml = new List<string>();
                ml.Add("查看");
                string key = playerGoods["key"].ToString();
                string sub = key.Substring(0, 4);
                if (playerGoods["pos"].ToString().Equals("0"))
                {
                    if (sub.Equals("1000"))
                    {
                        GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(key);
                        if (gd != null && gd.isAllowdUse)
                        {
                            ml.Add("使用");
                        }
                    }
                    if (sub.Equals("1012"))
                    {
                        ml.Add("使用");
                    }else if (sub.Equals("1014"))
                    {
                        ml.Add("变身");
                    }
                    else if (sub.Equals("1001")|| sub.Equals("1016"))
                    {
                        ml.Add("装备");
                        ml.Add("比较");
                    }
                    else if (sub.Equals("1004") && key.Substring(4, 4).Equals("1000")) ml.Add("合成");
                    ml.Add("存入仓库");
                    ml.Add("丢弃");
                }
                else if (playerGoods["pos"].ToString().Equals("1"))
                {
                    ml.Add("取出");
                }

                ml.Add("一键出售");
                Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                menu.addCallback((mIndex) =>
                {
                    //缓存scroll位置
                    scrollOldPos = gdItem.getScrollPos();
                    this.handle(playerGoods, ml[mIndex]);
                });
            });
            gdItem.scrollInpPos(scrollOldPos);
        }
        public void updateWindow()
        {
            Tab tab = this.getStandardPageContent().Find("Tab").GetComponent<Tab>();
            tab.clkDefault(tab.chooseIndex);
        }
        /**
         * 是否允许使用多个数量
         */
        private bool isAllowedUseMulNum(String k)
        {
            //金票和经验丹才允许超过数量1
            if (k.Equals("10000000") || k.Equals("10000001") || k.Equals("10000002") ||
                k.Equals("10000029") || k.Equals("10000030") || k.Equals("10000031") || k.Equals("10000032")
                || k.Equals("10000175") || k.Equals("10000176")
                || k.Equals("10000220")
                )
            {
                return true;
            }
            return false;
        }
        private void handle(JObject playerGoods, string str)
        {
            if (str.Equals("使用"))
            {
                string key = playerGoods["key"].ToString();
                //使用经验丹、龙票等允许一次使用多个
                if (isAllowedUseMulNum(key))
                {
                    //弹出数量输入面板
                    UseNumInputUI.create(this.transform).addCallback((num) =>
                    {
                        face.goodsInterface.userGoods(playerGoods, num);
                    });
                    return;
                }
                //todo:道具跳转指定界面

                face.goodsInterface.userGoods(playerGoods, 1);
            }
            else if (str.Equals("变身"))
            {
                string key = playerGoods["key"].ToString();
                MsgSureUI.create(this.transform).show("若存在神符效果将会被替换，是否继续？", () =>
                {
                    face.activityInterface.useShenFu(key, () => { });
                }, () => { });
            }
            else if (str.Equals("装备"))
            {
                if (strUtils.isMatch(playerGoods["key"].ToString(),"1001([0-9]{8})"))
                {
                    face.equipInterface.goodsToEquip(playerGoods["Id"].ToString(), () =>
                    {
                        //TODO:刷新

                    });
                }
                else if (face.equipInterface.isPetEquip(playerGoods["key"].ToString()))
                {
                    face.equipInterface.upPetEquip(playerGoods["Id"].ToString(), () =>
                    {
                        //TODO:刷新

                    });
                }
            }
            else if (str.Equals("合成"))
            {
                //SuipianCompoundUI.create(playerGoods, PointGet.getAcPage<PackagePage>());
            }
            else if (str.Equals("存入仓库"))
            {
                face.goodsInterface.putWarehouse(playerGoods["Id"].ToString(), () =>
                {

                });
            }
            else if (str.Equals("丢弃"))
            {

                MsgSureUI.create(this.transform).show("将要丢弃该物品，是否继续？", () =>
                {
                    face.goodsInterface.reqGiveUpGoods(playerGoods["Id"].ToString(), () =>
                    {

                    });
                }, () => { });
            }
            else if (str.Equals("取出"))
            {
                face.goodsInterface.getWarehouse(playerGoods["Id"].ToString(), () =>
                {

                });
            }
            else if (str.Equals("卸下"))
            {
                Equip g = (Equip)face.goodsInterface.getGoodsMsgByKey(playerGoods["key"].ToString());
                face.equipInterface.reqEquipToGoods(playerGoods, g.part, () =>
                {

                });
            }
            else if (str.Equals("查看"))
            {
                GoodsDesUI.create(playerGoods, this.transform).renderText();
            }
            else if (str.Equals("一键出售"))
            {
                MsgSureUI.create(this.transform).show("将出售背包中所有白装，是否继续？", () =>
                {
                    face.goodsInterface.saleBaiEquip(() =>
                    {

                    });
                }, () => { });
            }


        }
        public void updateHb()
        {
            JObject role = face.roleInterface.getRole();
            JObject bb = face.goodsInterface.getBBNAndCKN();
            JArray all = face.goodsInterface.getAllGoods();
            Transform hb = this.getStandardPageContent().Find("k2/hb");
            hb.Find("tale").GetComponent<TextUI>().setText("银两 " + role["attr"]["msg"]["tale"]);
            hb.Find("gold").GetComponent<TextUI>().setText("元宝 " + role["attr"]["msg"]["gold"]);
            hb.Find("packNum").GetComponent<TextUI>().setText("" + all.Count + " / " + bb["bbn"] + "");
        }
        /**在黑色部分展示银两*/
        private void drawTale()
        {
            JObject role = face.roleInterface.getRole();
            JObject bb = face.goodsInterface.getBBNAndCKN();
            JArray all = face.goodsInterface.getAllGoods();

            Transform hb = this.getStandardPageContent().Find("k2/hb");
            gameObjPool.getInstance().freeChildren(hb.gameObject);

            Vector2 size = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject tale = gameObjPool.getInstance().get("tale", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("银两 " + role["attr"]["msg"]["tale"]).setAlign("left").setFontSize().horiOut().setFontStyle()
                .setSizePos(new Vector2(400, 100), new Vector2(100, 0));
            //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            tale = gameObjPool.getInstance().get("taleIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("moneyImge_png", new Rect(13 * 2, 0, 13, 13))
                .setSizePos(new Vector2(50, 50), new Vector2(25, -25));

            tale = gameObjPool.getInstance().get("gold", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("元宝 " + role["attr"]["msg"]["gold"]).setAlign("left").setFontSize().horiOut().setFontStyle()
                .setSizePos(new Vector2(400, 100), new Vector2(100, -100));
            //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            tale = gameObjPool.getInstance().get("goldIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("moneyImge_png", new Rect(13 * 1, 0, 13, 13))
                .setSizePos(new Vector2(50, 50), new Vector2(25, -100 - 25));

            tale = gameObjPool.getInstance().get("packIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("packCount_png", new Rect(0, 0, 22, 19))
                .setSizePos(new Vector2(115, 100), new Vector2(size.x - 125 - 30, -10));

            tale = gameObjPool.getInstance().get("packNum", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("" + all.Count + " / " + bb["bbn"] + "").setAlign().setFontSize().setFontStyle().horiOut()
                .setSizePos(new Vector2(200, 100), new Vector2(size.x - 200, -100));
            //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
        }
        /**在黑色部分展示仓库*/
        private void drawWare()
        {
            JObject bb = face.goodsInterface.getBBNAndCKN();
            JArray all = face.goodsInterface.getAllGoods();
            JArray ckAll = face.goodsInterface.getCkGoods();

            Transform hb = this.getStandardPageContent().Find("k2/hb");
            gameObjPool.getInstance().freeChildren(hb.gameObject);

            Vector2 size = hb.GetComponent<RectTransform>().sizeDelta;

            GameObject tale = gameObjPool.getInstance().get("wareIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("packCount_png", new Rect(22, 0, 22, 19))
                .setSizePos(new Vector2(115, 100), new Vector2(25, -50));

            tale = gameObjPool.getInstance().get("wareNum", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("仓库容量 " + ckAll.Count + " / " + bb["ckn"]).setAlign("left").setFontSize().setFontStyle()
                .setSizePos(new Vector2(400, 100), new Vector2(225, -50));
            //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));

            tale = gameObjPool.getInstance().get("packIcon", typeof(ImgUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<ImgUI>().loadRes("packCount_png", new Rect(0, 0, 22, 19))
                .setSizePos(new Vector2(115, 100), new Vector2(size.x - 125 - 30, -10));

            tale = gameObjPool.getInstance().get("packNum", typeof(TextUI));
            tale.transform.SetParent(hb.transform, false);
            tale.GetComponent<TextUI>().setColor32(PageSetting.FontColor).setText("" + all.Count + " / " + bb["bbn"]).setAlign().setFontSize().setFontStyle().horiOut()
                .setSizePos(new Vector2(200, 100), new Vector2(size.x - 200, -100));
            //tale.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
        }
        public override void init()
        {

        }
    }
}
