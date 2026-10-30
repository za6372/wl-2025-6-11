using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.fight;
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
    class TalkPage : PageUI
    {
        private int channelIndex;
        public void drawUI(int clkTabIndex)
        {
            this.createStandardPageLayout();
            this.setTitle("聊天");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content, clkTabIndex);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content, int clkTabIndex)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("系");
            ml.Add("世");
            ml.Add("队");
            ml.Add("密");
            ml.Add("帮");
            ml.Add("战");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {

                drawList(mIndex);

            });
            drawK2(content);

            tab.clkDefault(clkTabIndex);
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

            GameObject scroll = gameObjPool.getInstance().get("scroll", typeof(ScrollUI));
            scroll.transform.SetParent(k1.getContent(), false);
            scroll.GetComponent<ScrollUI>()
                .setSizePos(new Vector2(size.x - 160, size.y - 200 - 60), new Vector2(50, -50));
            scroll.GetComponent<ScrollUI>().initSetting();

            GameObject sure = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            sure.transform.SetParent(this.transform.Find("Bottom"), false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("发言").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    List<string> ml = new List<string>();
                    ml.Add("发言");
                    ml.Add("聊天设置");
                    Menu menu = Menu.create(ml, this.transform);
                    menu.addCallback((mIndex) =>
                    {
                        if (mIndex == 0)
                        {
                            SendMsgPage.create(this.transform);
                        }
                        else if (mIndex == 1)
                        {

                        }

                    });
                });
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            //0系统1跨服2世界3当前4帮派5组队
            if (index == 0)
            {
                this.channelIndex = 0;
            }
            else if (index == 1)
            {
                this.channelIndex = 2;
            }
            else if (index == 2)
            {
                this.channelIndex = 5;
            }
            else if (index == 3)
            {
                this.channelIndex = 10;//私信
            }
            else if (index == 4)
            {
                this.channelIndex = 4;
            }
            updateChatWindow();

        }
        public void updateChatWindow()
        {
            if (this.channelIndex == 10)
            {
                /*Transform ts = this.transform.Find("sixinUI");
                if (ts != null && ts.gameObject.activeSelf)
                {
                    sixinUI.getInstance().updateData();
                }*/
                JArray list = face.chatInterface.getZongHeSiXinMsg();
                this.drawChatFace(list);
            }
            else
            {
                JArray list = face.chatInterface.getMsg(null, this.channelIndex + "");
                this.drawChatFace(list);
            }

            DoGet.getInstance().startReqImg();
        }
        //需要移动的位置 0不移动 1旧位置 2底部
        private int mPos = 0;
        /**绘制聊天*/
        public void drawChatFace(JArray list)
        {
            ScrollUI content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent()
                .Find("scroll").GetComponent<ScrollUI>();
            //对于超出显示范围的组件要隐藏
            content.saveOldPosSizeMsg();
            GameObject g = gameObjPool.getInstance().get("panel");
            content.setContent(g);

            Action<int> fn = (start) =>
            {
                int end = start + 10;
                if (list.Count < 10)
                {
                    end = list.Count;
                }
                //先删除多余的item
                int n = g.transform.childCount - end;
                if (n < 0)
                {
                    for (int p = 0; p < n; p++)
                    {
                        gameObjPool.getInstance().free(g.transform.GetChild(0).gameObject);
                    }
                }

                updateChatItem(0, start, end, list, g, -10, (y) =>
                 {
                     //重新设置内容高度
                     content.updateHeight(-y);
                     DoGet.getInstance().startReqImg();
                     if (mPos == 0) content.scrollZero();
                     else if (mPos == 1) content.scrollBotton();
                     else content.scrollBotton();
                     //todo:mPos==2时，表示正在看中间的内容，即使刷新消息也不要滑到最下方
                     /*if (mPos == 1) content.scrollPrePos();
                     else if (mPos == 2) content.scrollBotton();*/
                 });
            };
            //最多10个item，不够10个数据对于不使用的item要进行清理
            //上一个item的y位置
            int len = 10;
            int index = list.Count - len;
            if (index < 0) index = 0;
            mPos = 2;
            fn(index);


            content.addTouchMoveScript(() =>
            {
                //索引往前移动一位
                index--;
                if (index < 0)
                {
                    index = 0;
                    return;
                }
                mPos = 0;
                fn(index);
            }, () =>
            {
                index++;
                if (index > list.Count - len)
                {
                    index = list.Count - len;
                    return;
                }
                mPos = 1;
                fn(index);

            });

        }
        private void updateChatItem(int i, int start, int end, JArray list, GameObject g, float y, Action<float> ac)
        {
            if (start >= end)
            {
                ac(y);
                return;
            }
            JObject data = (JObject)list[start];
            string name = (string)data["sender"];
            string receiver = data["receiver"] == null ? null : data["receiver"].ToString();
            string str = (string)data["content"];
            int type = (int)data["type"];
            string head = (string)data["head"];
            JObject en = null;
            if (!strUtils.isNull(data["en"]))
            {
                en = (JObject)data["en"];
            }


            ChatItemUI one = null;
            if (g.transform.childCount > i)
            {
                one = g.transform.GetChild(i).GetComponent<ChatItemUI>();
            }
            else
            {
                one = ChatItemUI.create(g.transform);
            }


            if (type == 0)
            {//文字
                one.setText(name, receiver, str, en, (size) =>
                 {
                     //计算高度是否超出显示区，超出显示区的组件要隐藏
                     one.updateHead(head).setSizePos(size, new Vector2(10, y));
                     y -= (size.y + 30);
                     start++;
                     i++;
                     updateChatItem(i, start, end, list, g, y, ac);
                 }).addClkIcon(() =>
                 {
                     JObject r = face.roleInterface.getRole();
                     if (name.Equals("系统") || name.Equals(r["name"].ToString())) return;
                     this.showMenu(name, en);
                 });
            }
        }
        private void showMenu(string playerName, JObject en)
        {
            List<string> ml = new List<string>();
            if (en != null)
            {
                ml.Add("查看附件");
            }
            ml.Add("查看玩家");
            ml.Add("查看宠物");
            ml.Add("邀请组队");
            ml.Add("申请入队");
            ml.Add("添加好友");
            ml.Add("密语");
            ml.Add("发送邮件");
            ml.Add("拜师");
            ml.Add("收徒");
            ml.Add("申请入帮");

            Menu menu = Menu.create(ml, PointGet.getTipCanvas());
            menu.addCallback((mIndex) =>
            {
                handle(ml[mIndex], playerName, en);
            });
        }
        public void handle(string menuName, string playerName, JObject en)
        {
            if (menuName.Equals("查看玩家"))
            {
                face.roleInterface.getPlayerMsg(playerName, (res) =>
                {
                    Transform ts = null;
                    if (fightCache.getInstance().isDoing)
                    {
                        ts = PointGet.getFightPageOfPage();
                    }
                    else
                    {
                        ts=PointGet.getIndexPageOfPage();
                    }
                    PageUI.createAcPage<ManPage>(ts).drawUI(false, res);
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
                    Transform ts = null;
                    if (fightCache.getInstance().isDoing)
                    {
                        ts = PointGet.getFightPageOfPage();
                    }
                    else
                    {
                        ts = PointGet.getIndexPageOfPage();
                    }
                    PageUI.createAcPage<PetPage>(ts).drawUI(false, (JObject)res[0]);
                });

            }
            else if (menuName.Equals("邀请组队"))
            {
                if (fightCache.getInstance().isDoing)
                {
                    msgCode.showMsg(223);
                    return;
                }
                    face.teamInterface.inviteTeam(playerName);
            }
            else if (menuName.Equals("申请入队"))
            {
                if (fightCache.getInstance().isDoing)
                {
                    msgCode.showMsg(223);
                    return;
                }
                face.teamInterface.reqTeam(playerName);
            }
            else if (menuName.Equals("发送邮件"))
            {
                if (fightCache.getInstance().isDoing)
                {
                    msgCode.showMsg(223);
                    return;
                }
                this.freeThisPage();
                EmailPage ep = PageUI.createAcPage<EmailPage>(PointGet.getIndexPageOfPage());
                ep.drawUI();
                ep.writeNewEmail(playerName);

            }
            else if (menuName.Equals("切磋"))
            {
                if (fightCache.getInstance().isDoing)
                {
                    msgCode.showMsg(223);
                    return;
                }
                face.fightInterface.createFightByPK(playerName, () => { });
            }
            else if (menuName.Equals("查看附件"))
            {
                Transform ts = null;
                if (fightCache.getInstance().isDoing)
                {
                    ts = PointGet.getFightPageOfPage();
                }
                else
                {
                    ts = PointGet.getIndexPageOfPage();
                }
                face.goodsInterface.getPlayerGoodsOrPetMsgById(en["Id"].ToString(), (int)en["type"], playerName, (res) =>
                 {
                     if ((int)en["type"] == 0)//道具详情
                     {
                         GoodsDesUI.create(res, ts).renderText();
                     }
                     else//宠物详情
                     {
                         PageUI.createAcPage<PetPage>(ts).drawUI(false, res);
                     }
                 });
            }
            else if (menuName.Equals("密语"))
            {
                //弹出文字输入
                SendMsgPage.create(this.transform, playerName);
            }
        }

        public override void init()
        {

        }
    }
}
