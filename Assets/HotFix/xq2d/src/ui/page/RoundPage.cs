using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.childPage.talk;
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
    class RoundPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("周围");
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
            ml.Add("玩家");
            ml.Add("NPC");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {

                drawList(mIndex);
                Debug.Log(mIndex);
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
        private void getOneMsg(string name, string playerName, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            a["playerName"] = playerName;
            list.Add(a);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);

            JArray arr = new JArray();
            if (index == 0)
            {
                JArray list = face.playerInterface.getPlayerList();
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];
                    getOneMsg("【Lv" + a["lever"] + "】 【" + GameAttrConst.getJobToSimpleName(GameAttrConst.getModel(a)) + "】" + a["name"], a["name"].ToString(), arr);
                }
            }
            else
            {
                JArray list = npcManager.getNpcList();
                for (int i = 0; i < list.Count; i++)
                {
                    NpcObjData a = face.npcInterface.getNpc(list[i].ToString());
                    getOneMsg(a.name, a.name, arr);
                }
            }


            GoodsItem gdItem = GoodsItem.create(arr, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                string playerName = null;
                List<string> ml = new List<string>();
                if (index == 0)
                {
                    ml.Add("查看玩家");
                    ml.Add("查看宠物");
                    if (!face.roleInterface.isInMap("xxzd"))
                    {
                        ml.Add("创建队伍");
                        ml.Add("邀请组队");
                        ml.Add("申请入队");
                        ml.Add("添加好友");
                        ml.Add("密语");
                        ml.Add("发送邮件");
                        ml.Add("拜师");
                        ml.Add("收徒");
                        ml.Add("偷袭");
                        ml.Add("切磋");
                        ml.Add("悬赏");
                        ml.Add("观战");
                    }
                    
                    //JArray list = face.playerInterface.getPlayerList();
                    playerName = arr[mIndex]["playerName"].ToString();
                }
                else
                {
                    ml.Add("自动寻路");
                }

                Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                menu.addCallback((mIndex) =>
                {
                    handle(ml[mIndex], playerName);
                });
            });
        }
        public void handle(string menuName, string playerName)
        {
            if (menuName.Equals("查看玩家"))
            {
                face.roleInterface.getPlayerMsg(playerName, (res) =>
                {
                    PageUI.createAcPage<ManPage>(PointGet.getIndexPageOfPage()).drawUI(false, res);
                });
            }
            else if (menuName.Equals("查看宠物"))
            {
                face.petInterface.getPlayerPet(playerName, (res) =>
                {
                    if (((JArray)res).Count == 0)
                    {
                        msgCode.showMsg(628);
                        return;
                    }
                    PageUI.createAcPage<PetPage>(this.transform).drawUI(false, (JObject)res[0]);
                });

            }
            else if (menuName.Equals("创建队伍"))
            {
                face.teamInterface.createTeam();
            }
            else if (menuName.Equals("邀请组队"))
            {
                face.teamInterface.inviteTeam(playerName);
            }
            else if (menuName.Equals("申请入队"))
            {
                face.teamInterface.reqTeam(playerName);
            }
            else if (menuName.Equals("添加好友"))
            {
                face.friendInterface.addFriend(playerName, () => { });
            }
            else if (menuName.Equals("发送邮件"))
            {
                this.freeThisPage();
                EmailPage ep = PageUI.createAcPage<EmailPage>(PointGet.getIndexPageOfPage());
                ep.drawUI();
                ep.writeNewEmail(playerName);
            }
            else if (menuName.Equals("拜师"))
            {
                face.activityInterface.apprenticeBs(playerName, () => { });
            }
            else if (menuName.Equals("收徒"))
            {
                face.activityInterface.reqSt(playerName, () => { });
            }
            else if (menuName.Equals("偷袭"))
            {
                face.fightInterface.createFightBySneak(playerName, () => { });
            }
            else if (menuName.Equals("切磋"))
            {
                face.fightInterface.createFightByPK(playerName, () => { });
            }
            else if (menuName.Equals("悬赏"))
            {
                face.fightInterface.createFightByZsl(playerName, () => { });
            }
            else if (menuName.Equals("密语"))
            {
                //弹出文字输入
                SendMsgPage.create(this.transform, playerName);
            }
            else if (menuName.Equals("观战"))
            {
                face.fightInterface.viewFight(playerName, () => { });
            }
        }
        public override void init()
        {

        }
    }
}
