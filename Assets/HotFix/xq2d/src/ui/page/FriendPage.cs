using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
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
    class FriendPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("好友");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
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
            List<string> ml = new List<string>();
            ml.Add("好友");
            ml.Add("黑名单");
            ml.Add("仇人");
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
            k2.transform.SetParent(content.transform,false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 60), new Vector2(30, -30), k2.transform);

            GameObject sure = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            sure.transform.SetParent(this.transform.Find("Bottom"), false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("搜索").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    UseNumInputUI.create(this.transform).renderText("请输入要添加的好友").addTextCallback((bpName) =>
                    {
                        face.friendInterface.addFriend(bpName,()=> { this.clkTab(0); });
                    });
                });
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            JArray ml = new JArray();
            JArray list = null;

            if (index == 0)
            {
                list = face.friendInterface.readFriendList();
            }else if (index == 1)
            {
                list = new JArray();
            }
            else if (index == 2)
            {
                list = new JArray();
            }

            for (int i = 0; i < list.Count; i++)
            {
                JObject a = (JObject)list[i];
                getOneMsg("【Lv" + a["lever"] + "】【" + GameAttrConst.getJobToSimpleName(GameAttrConst.getModel(a)) + "】 " + a["name"], ml);
            }
            GoodsItem gdItem = GoodsItem.create(ml, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                string playerName = list[mIndex]["name"].ToString();
                List<string> ml = new List<string>();
                ml.Add("查看玩家");
                ml.Add("查看宠物");
                ml.Add("邀请组队");
                ml.Add("申请入队");
                ml.Add("添加好友");
                ml.Add("密语");
                ml.Add("发送邮件");
                ml.Add("删除好友");
                ml.Add("移入黑名单");
                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((mIndex2) =>
                {
                    handle(ml[mIndex2], playerName);
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
            else if (menuName.Equals("删除好友"))
            {
                face.friendInterface.delFriend(playerName, () => { this.clkTab(0); });
            }
            else if (menuName.Equals("移入黑名单"))
            {
                
            }
            else if (menuName.Equals("密语"))
            {
                //弹出文字输入
                SendMsgPage.create(this.transform, playerName);
            }
        }
        private void getOneMsg(string name, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            list.Add(a);
        }
        public override void init()
        {

        }
    }
}
