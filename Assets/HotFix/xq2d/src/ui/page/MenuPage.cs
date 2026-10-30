using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.ui.activityPage;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.Res.script.src.touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class MenuPage : PageUI
    {
        //当前菜单层级
        private List<int> levers = new List<int>();
        public void drawUI()
        {
            this.setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);
            this.gameObject.AddComponent<Image>().gameObject.AddComponent<ImgUI>().setAlpha(0);
            GameObject top = gameObjPool.getInstance().get("title", typeof(SimpleUI));
            top.transform.SetParent(this.transform, false);
            top.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(320, 100), new Vector2((ScreenUtils.width - 320) / 2, -((ScreenUtils.height - 600) / 2 - 200)));
            drawK1(top.transform);
            top = gameObjPool.getInstance().get("menu", typeof(SimpleUI));
            top.transform.SetParent(this.transform, false);
            top.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(600, 600), new Vector2((ScreenUtils.width - 600) / 2, -(ScreenUtils.height - 600) / 2));
            drawK2(top.transform);



            this.addClk(() =>
            {
                if (levers.Count <= 0)
                {
                    this.freeThisPage();
                    return;
                }
                //移除最后一位
                levers.RemoveAt(levers.Count - 1);
                //返回上一层菜单
                drawList();
            });

            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            Vector2 size = new Vector2(320, 100);
            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(content.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, Vector2.zero);

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

            kuang = gameObjPool.getInstance().get("text", typeof(TextUI));
            kuang.transform.SetParent(content.transform, false);
            kuang.GetComponent<TextUI>().setText("主菜单").setColor().setAlign().setFontSize()
                .setSizePos(size, Vector2.zero);
        }
        private void drawK2(Transform content)
        {
            Vector2 size = new Vector2(600, 600);
            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(content.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, Vector2.zero);

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

            for (int i = 0; i < 12; i++)
            {
                int index = i;
                kuang = gameObjPool.getInstance().get("item" + (i + 1), typeof(SimpleUI));
                kuang.transform.SetParent(content.transform, false);
                kuang.GetComponent<SimpleUI>()
                    .setSizePos(new Vector2(200, 150), new Vector2(200 * (i % 3), -150 * (int)(i / 3)))
                    .addClk(()=> {
                        levers.Add(index);//记录当前选项
                                          //判断是页面还是菜单，菜单就继续绘制
                        menuItem m = getAny(0, getData());
                        if (m == null || m.list == null)
                        {

                            toPage();
                            //Debug.Log("页面");
                            levers.RemoveAt(levers.Count - 1);
                        }
                        else
                        {
                            //Debug.Log("绘制菜单"+levers[0]);
                            drawList();
                        }

                    });
                GameObject text = gameObjPool.getInstance().get("text" , typeof(TextUI));
                text.transform.SetParent(kuang.transform, false);
                text.GetComponent<TextUI>().setText("").setFontSize(30).setColor().setAlign()
                    .setSizePos(new Vector2(200, 150), new Vector2(0,0));

            }
            drawList();

        }
        public void toPage()
        {
            if (levers[0] == 0)//日常
            {
                if(levers[1]==0)PageUI.createAcPage<TaskPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 1) PageUI.createAcPage<ForgetPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 1)//宠物
            {
                PageUI.createAcPage<PetPage>(PointGet.getIndexPageOfPage()).drawUI(true);
            }
            else if (levers[0] == 2)//背包
            {
                PageUI.createAcPage<PackagePage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 3)//社交
            {
                if (levers[1] == 0) PageUI.createAcPage<FriendPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 1) PageUI.createAcPage<RoundPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 2) PageUI.createAcPage<TeamPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 3) PageUI.createAcPage<TalkPage>(PointGet.getIndexPageOfPage()).drawUI(1);
                else if (levers[1] == 4) PageUI.createAcPage<EmailPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 5) PageUI.createAcPage<FriendPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 6) PageUI.createAcPage<FriendPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 4)//主角
            {
                PageUI.createAcPage<ManPage>(PointGet.getIndexPageOfPage()).drawUI(true);
            }
            else if (levers[0] == 5)//商城
            {
                if (levers[1] == 1) PageUI.createAcPage<GoldShopPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 2) PageUI.createAcPage<TaleShopPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 6)//生活
            {
                PageUI.createAcPage<LiftSkillPage>(PointGet.getIndexPageOfPage()).drawUI().clkTab(0);
            }
            else if (levers[0] == 7)//帮派
            {
                if (levers[1] == 0)
                {
                    activityGet.bpNames();
                }
                else if (levers[1] == 1)
                {
                    activityGet.bpMsg();
                }
                else if (levers[1] == 3)
                {
                    //回到帮派
                    activityGet.toMap("gangs");
                }
                else if (levers[1] == 4)
                {
                    activityGet.bpReq();
                }
                else if (levers[1] == 5) activityGet.bpList();
            }
            else if (levers[0] == 8)//地图
            {
                PageUI.createAcPage<MiniMapPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 9)//邮件
            {
                PageUI.createAcPage<EmailPage>(PointGet.getIndexPageOfPage()).drawUI();
            }
            else if (levers[0] == 10)//系统
            {
                if (levers[1] == 1) PageUI.createAcPage<ChatSettingPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 2) PointGet.getMapPoint().GetComponent<Move>().startAutoMove();//自动遇怪
                else if (levers[1] == 4)
                {
                    netUtils.getInstance().Dispose();
                    gameObjPool.getInstance().freeChildren(PointGet.getTipCanvas().gameObject);
                    PointGet.getIndexPage().GetComponent<IndexPage>().freeThisPage();
                    PageUI.create<LoginPage>().drawUI();
                }
                else if (levers[1] == 5) PageUI.createAcPage<SysSettingPage>(PointGet.getIndexPageOfPage()).drawUI();
                else if (levers[1] == 6)
                {
                    netUtils.getInstance().Dispose();
                    //Application.Quit();
                }
                else if (levers[1] == 7)
                {
                    this.freeThisPage();
                    string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                    mapManager.getInstance().reloadBef(mapKey);
                }
            }
        }
        private void drawList()
        {
            List<menuItem> ms = getItems();
            for (int i = 0; i < 12; i++)
            {
                Transform t = this.transform.Find("menu/item" + (i + 1));

                if (i >= ms.Count)
                {
                    //超出部分隐藏
                    t.gameObject.SetActive(false);
                    continue;
                }
                t.gameObject.SetActive(true);
                t.Find("text").GetComponent<Text>().text = ms[i].name;
            }


        }
        private List<menuItem> getItems()
        {
            List<menuItem> list = getData();
            //比如点了 第一项 进入后又点了第二项 [0,1]
            //当前所在层级
            int len = levers.Count;
            if (len == 0) return list;
            return getAny(0, list).list;

        }
        private menuItem getAny(int lvIndex, List<menuItem> list)
        {
            int index = levers[lvIndex];
            //if (index >= list.Count) return null;
            menuItem m = list[index];
            lvIndex++;
            //Debug.Log(lvIndex+"/"+levers.Count);
            if (lvIndex >= levers.Count) return m;
            if (m.list == null) return null;

            return getAny(lvIndex, m.list);

        }
        private List<menuItem> getData()
        {
            List<menuItem> list = new List<menuItem>();
            menuItem m = new menuItem("日常");
            m.put(new menuItem("任务"));
            m.put(new menuItem("备忘"));
            m.put(new menuItem("在线有礼"));
            list.Add(m);
            m = new menuItem("宠物");
            list.Add(m);
            m = new menuItem("背包");
            list.Add(m);
            m = new menuItem("社交");
            m.put(new menuItem("好友"));
            m.put(new menuItem("周围"));
            m.put(new menuItem("组队"));
            m.put(new menuItem("聊天"));
            m.put(new menuItem("邮件"));
            m.put(new menuItem("仇人"));
            m.put(new menuItem("黑名单"));
            list.Add(m);
            m = new menuItem("主角");
            list.Add(m);
            m = new menuItem("商城");
            m.put(new menuItem("充值"));
            m.put(new menuItem("元宝商城"));
            m.put(new menuItem("银两商城"));
            m.put(new menuItem("信用商城"));
            m.put(new menuItem("团购专区"));
            m.put(new menuItem("秒杀抢购"));
            list.Add(m);
            m = new menuItem("生活");
            list.Add(m);
            m = new menuItem("帮派");
            m.put(new menuItem("帮派名册"));
            m.put(new menuItem("帮派信息"));
            m.put(new menuItem("帮派外交"));
            m.put(new menuItem("回到帮派"));
            m.put(new menuItem("入帮审批"));
            m.put(new menuItem("帮派列表"));
            list.Add(m);
            m = new menuItem("地图");
            list.Add(m);
            m = new menuItem("邮件");
            list.Add(m);
            m = new menuItem("系统");
            m.put(new menuItem("求助客服"));
            m.put(new menuItem("聊天设置"));
            m.put(new menuItem("自动遇怪"));
            m.put(new menuItem("游戏帮助"));
            m.put(new menuItem("重选角色"));
            m.put(new menuItem("系统设置"));
            m.put(new menuItem("退出游戏"));
            m.put(new menuItem("刷新地图"));
            list.Add(m);
            return list;
        }

        class menuItem
        {
            public string name;//
            public List<menuItem> list;

            public menuItem(string name)
            {
                this.name = name;

            }
            public void put(menuItem m)
            {
                if (list == null) this.list = new List<menuItem>();
                this.list.Add(m);
            }


        }
        public override void init()
        {

        }
    }
}
