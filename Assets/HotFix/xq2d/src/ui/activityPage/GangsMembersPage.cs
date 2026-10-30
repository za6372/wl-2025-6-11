using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
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
namespace Assets.HotFix.xq2d.src.ui.activityPage
{
    class GangsMembersPage : PageUI
    {
        //是否仅浏览信息
        private bool isViewMsg;
        private string bpId;
        public void drawUI(bool isViewMsg = false, string bpId = null)
        {
            this.createStandardPageLayout();
            this.setTitle("帮派");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            this.isViewMsg = isViewMsg;
            this.bpId = bpId;
            if (!isViewMsg)
            {
                JObject r = face.roleInterface.getRole();
                if (r["attr"]["msg"]["bp"].ToString().Equals(""))
                {
                    msgCode.showMsg(648);
                }
                else
                {
                    this.bpId = r["attr"]["msg"]["bp"]["Id"].ToString();
                }
            }

            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }


        public void clkTab(int index)
        {
            Transform content = this.getStandardPageContent();
            content.Find("Tab").GetComponent<Tab>().clkDefault(index);
        }
        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>() {
            "信息","成员","申请"
            };

            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {
                pageNum = 1;
                totalPage = 0;
                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);
            //tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 160), new Vector2(30, -30 - 100), k2.transform);
        }
        private int pageNum = 1;
        private int totalPage = 0;
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            if (index == 0)
            {
                draw0();
            }
            else if (index == 1)
            {
                if (isViewMsg) return;
                draw1();
            }
            else if (index == 2)
            {
                if (isViewMsg) return;
                draw2();
            }


        }
        private void draw0()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            if (this.bpId == null) return;
            face.gangsInterface.viewBPMsg(this.bpId, (res) =>
            {
                JArray list = new JArray();
                int max = 15000 * ((int)res["lever"] - 1) + 10000;
                getOneMsg("帮派名称:" + res["name"], list);
                getOneMsg("现任帮主:" + res["captain"], list);
                getOneMsg("等级:" + res["lever"] + "/" + 6, list);
                getOneMsg("帮派人数:" + res["num"] + "/" + res["sum"], list);
                getOneMsg("帮派建设金:" + res["money"], list);
                getOneMsg("活跃度:100", list);
                getOneMsg("商店建设度:" + res["shop"] + "/" + max, list);
                getOneMsg("建筑建设度:" + res["tec"] + "/" + max, list);
                getOneMsg("人才建设度:" + res["solicit"] + "/" + max, list);
                getOneMsg("技能建设度:" + res["book"] + "/" + max, list);
                getOneMsg("秘法建设度:" + res["mifa"] + "/" + max, list);

                GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {

                });
            });

        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        private void draw1()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JObject r = face.roleInterface.getRole();
            if (r["attr"]["msg"]["bp"].ToString().Equals(""))
            {
                msgCode.showMsg(648);
                return;
            }
            face.gangsInterface.viewMemberList(pageNum, (res) =>
            {
                JArray list = (JArray)res["list"];
                totalPage = (int)res["totalPage"];
                GoodsItem gdItem = GoodsItem.create(list, content.transform, 4);
                gdItem.addCallback((mIndex) =>
                {
                    string name = list[mIndex]["name"].ToString();
                    List<string> ml = new List<string>();
                    ml.Add("查看信息");
                    ml.Add("分配职权");
                    ml.Add("踢出");
                    ml.Add("退出");
                    ml.Add("密语");
                    ml.Add("传送过去");
                    ml.Add("添加好友");
                    ml.Add("发送邮件");
                    ml.Add("邀请组队");
                    ml.Add("申请入队");
                    Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                    menu.addCallback((mIndex2) =>
                    {
                        handle(ml[mIndex2], name);
                    });
                }).addScrollTopCall(() =>
                {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    drawList(1);
                }).addScrollBottomCall(() =>
                {
                    pageNum++;
                    if (pageNum > totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    drawList(1);
                });
            });
        }
        private void handle(string s,string name)
        {
            if (s.Equals("分配职权"))
            {
                //0帮主1副帮主2左护法3右护法4精英5帮众
                //菜单显示
                List<string> ml = new List<string>() { "转让帮主","副帮主", "左护法", "右护法", "精英", "帮众" };
                Menu menu = Menu.create(ml, PointGet.getIndexPage());
                menu.addCallback((mIndex) =>
                {
                    MsgSureUI.create(PointGet.getTipCanvas()).show("确定要这么做么？", () =>
                    {
                        face.gangsInterface.changeJob(name, mIndex+"", () => {
                            this.clkTab(1);
                        });
                    }, () => { });
                });
            }
            else if (s.Equals("踢出"))
            {
                string str = "确定要踢出该成员？（仅帮主、副帮主可踢）";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                {
                    face.gangsInterface.tichuGangs(name,() =>
                    {
                        this.clkTab(1);
                    });
                }, () => { });
            }else if (s.Equals("退出"))
            {
                string str = "确定要退出帮派？";
                MsgSureUI.create(PointGet.getTipCanvas()).show(str, () =>
                {
                    face.gangsInterface.existGangs(() =>
                    {
                        this.freeThisPage();
                        activityGet.toMap("m_1");
                    });
                }, () => { });
            }
        }
        private void draw2()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            JObject r = face.roleInterface.getRole();
            if (r["attr"]["msg"]["bp"].ToString().Equals(""))
            {
                msgCode.showMsg(648);
                return;
            }
            face.gangsInterface.viewReqList(pageNum, (res) =>
            {
                JArray list = (JArray)res["list"];
                totalPage = (int)res["totalPage"];
                JArray ml = new JArray();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = new JObject();
                    a.Add("name", "Lv" + list[i]["lever"] + " " + list[i]["name"]);
                    ml.Add(a);
                }
                GoodsItem gdItem = GoodsItem.create(ml, content.transform, -1);
                gdItem.addCallback((mIndex) =>
                {
                    List<string> ml = new List<string>();
                    ml.Add("查看信息");
                    ml.Add("同意申请");
                    ml.Add("拒绝申请");
                    Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                    menu.addCallback((mIndex2) =>
                    {
                        if (mIndex2 == 0)
                        {
                            face.roleInterface.getPlayerMsg(list[mIndex]["name"].ToString(), (res) =>
                            {
                                PageUI.createAcPage<ManPage>(PointGet.getIndexPageOfPage()).drawUI(false, res);
                            });
                        }
                        else if (mIndex2 == 1)
                        {
                            face.gangsInterface.agreeReq(list[mIndex]["name"].ToString(), () => { this.clkTab(2); });
                        }
                        else if (mIndex2 == 2)
                        {
                            face.gangsInterface.rejectReq(list[mIndex]["name"].ToString(), () => { this.clkTab(2); });
                        }
                    });
                }).addScrollTopCall(() =>
                {
                    pageNum--;
                    if (pageNum < 1)
                    {
                        pageNum = 1;
                        return;
                    }
                    drawList(2);
                }).addScrollBottomCall(() =>
                {
                    pageNum++;
                    if (pageNum > totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    drawList(2);
                });
            });
        }
        public override void init()
        {

        }
    }
}
