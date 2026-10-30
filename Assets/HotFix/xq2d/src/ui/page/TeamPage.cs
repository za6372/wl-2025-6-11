using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class TeamPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("队伍");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("成员");
            ml.Add("请求");
            ml.Add("邀请");
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
        public void updateWindow()
        {
            Tab tab = this.getStandardPageContent().Find("Tab").GetComponent<Tab>();
            tab.clkDefault(tab.chooseIndex);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            JObject team = null;
            JArray arr = new JArray();

            if (index == 0)
            {
                team = face.teamInterface.getTeam();
                if (team != null)
                {
                    JArray list = (JArray)team["list"];
                    //Debug.Log(team);
                    for (int i = 0; i < list.Count; i++)
                    {
                        JObject a = (JObject)list[i];
                        int isFollow = (int)a["isFollow"];
                        int isOnline = (int)a["isOnline"];
                        string st = "";
                        if (isOnline == 0) st = "【离线】";
                        else if (isFollow == 0) st = "【暂离】";
                        else if (isFollow == 1) st = "【跟随】";
                        string str = "【Lv" + a["lever"] + "】【" + GameAttrConst.getJobToSimpleName(GameAttrConst.getModel(a)) + "】" + a["name"] + " ";
                        if (a["name"].ToString().Equals(team["captain"].ToString()))
                        {
                            str = "【队】" + str;
                        }
                        else
                        {
                            str += st;
                        }
                        getOneMsg(str, arr);
                    }
                }
            }
            else if (index == 1)
            {
                //获取申请列表
                JArray list = face.msgInterface.readMsg(1);
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i]["params"];
                    string str = a.ContainsKey("model") ? GameAttrConst.getJobToName(a["model"].ToString()) : "无";
                    getOneMsg("【Lv" + a["lv"] + "】 【" + str + "】" + a["name"], arr);
                }
            }
            else
            {
                JArray list = face.msgInterface.readMsg(0);
                //Debug.Log(list);
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i]["params"];
                    string str= a.ContainsKey("model") ? GameAttrConst.getJobToName(a["model"].ToString()) : "无";
                    getOneMsg("【Lv" + a["lv"] + "】 【" + str + "】" + a["name"], arr);
                }
            }

            GoodsItem gdItem = GoodsItem.create(arr, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                List<string> ml = new List<string>();
                if (index == 0)
                {
                    JObject role = face.roleInterface.getRole();
                    if (role["name"].ToString().Equals(team["captain"].ToString()))
                    {
                        JArray list = (JArray)team["list"];
                        if (!list[mIndex]["name"].ToString().Equals(team["captain"].ToString()))
                        {
                            ml.Add("请离队员");
                            ml.Add("移交队长");
                        }
                        else
                        {
                            ml.Add("解散队伍");
                        }
                    }
                    else
                    {
                        ml.Add("离开队伍");
                    }
                }
                else if (index == 1)
                {
                    ml.Add("同意申请");
                    ml.Add("拒绝申请");
                }
                else
                {
                    ml.Add("同意邀请");
                    ml.Add("拒绝邀请");
                }

                Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                menu.addCallback((p) =>
                {
                    handle(ml[p], mIndex);
                });
            });
            if (index == 0 && team == null)
            {
                Vector2 size = content.GetComponent<RectTransform>().sizeDelta;

                GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
                text.transform.SetParent(content.transform, false);
                text.GetComponent<TextUI>().setText("点击创建队伍").setAlign().setColor("#0CA894").setFontSize(35).setFontStyle()
                    .setSizePos(size, Vector2.zero).addClk(() =>
                    {
                        face.teamInterface.createTeam();
                    });
                text.AddComponent<UIOutline>().setSome(Color.white, new Vector2(2, -2));
            }
        }
        private void handle(string str, int index)
        {
            switch (str)
            {
                case "解散队伍":
                    {
                        JObject team = face.teamInterface.getTeam();
                        face.teamInterface.leaveTeam(team["captain"].ToString());
                        break;
                    }
                case "离开队伍":
                    {
                        JObject role = face.roleInterface.getRole();
                        face.teamInterface.leaveTeam(role["name"].ToString());
                        break;
                    }
                case "请离队员":
                    {
                        JArray list = (JArray)face.teamInterface.getTeam()["list"];
                        face.teamInterface.leaveTeam(list[index]["name"].ToString());
                        break;
                    }
                case "移交队长":
                    {
                        JArray list = (JArray)face.teamInterface.getTeam()["list"];
                        face.teamInterface.captainToName(list[index]["name"].ToString());
                        break;
                    }
                case "同意申请":
                    {
                        JArray list = face.msgInterface.readMsg(1);
                        face.teamInterface.agreeTeam(list[index]["params"]["name"].ToString());
                        face.msgInterface.del(list[index]["Id"].ToString());
                        break;
                    }
                case "拒绝申请":
                    {
                        JArray list = face.msgInterface.readMsg(1);
                        face.msgInterface.del(list[index]["Id"].ToString());
                        this.updateWindow();
                        break;
                    }
                case "同意邀请":
                    {
                        JArray list = face.msgInterface.readMsg(0);
                        face.teamInterface.agreeInvite(list[index]["params"]["Id"].ToString());
                        face.msgInterface.del(list[index]["Id"].ToString());
                        break;
                    }
                case "拒绝邀请":
                    {
                        JArray list = face.msgInterface.readMsg(0);
                        face.msgInterface.del(list[index]["Id"].ToString());
                        this.updateWindow();
                        break;
                    }
            }
        }
        public override void init()
        {

        }
    }
}
