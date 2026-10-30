using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.am.fight;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.touch;
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
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.fight
{
    class partsMenu
    {
        private static partsMenu one;
        //是否为人物菜单
        private bool isRole = true;
        public static partsMenu getInstance()
        {
            if (one == null)
            {
                one = new partsMenu();
            }

            return one;
        }
        public partsMenu create()
        {
            GameObject partsLayer = PointGet.getPartsLayer().gameObject;
            //回合、倒计时
            drawHuihe(partsLayer);
            //聊天窗
            drawChatWindow(partsLayer);
            if (!fightCache.getInstance().isViewFight)
            {
                //头像
                drawHead(partsLayer);
                drawTeamMemberHead(partsLayer);

                //中间菜单
                drawMenu(partsLayer);
                //底部按钮
                drawBtn(partsLayer);
            }

            DoGet.getInstance().startReqImg();
            hideMenu();
            return this;
        }
        public void drawMsg3009(JObject a)
        {
            //Debug.Log(a);
            //更新这个冷却技能
            fightCache.getInstance().setCoolList((JArray)a["coolList"]);
            partsMenu.getInstance().setHuiheNum(a["huihe"].ToString());
            partsMenu.getInstance().resetDjs();
            partsMenu.getInstance().showDjs(true);

            if (!fightCache.getInstance().isViewFight)
            {
                partsMenu.getInstance().showMenu(true);
                partsMenu.getInstance().visiblebottomBtn(true);
            }

            //显示名称
            playerOperate.getInstance().visibleNames(true);
            //显示沙漏
            partsMenu.getInstance().drawHourglass();
        }
        /**只设置成自动，而不上传命令*/
        public void setAutoIcon()
        {
            PointGet.getPartsLayer().Find("bottomBtn/auto").GetComponent<BtnUI>().addText("手动");
            //将技能图标隐藏
            this.showSkillCard(false);
            //标记为自动
            fightCache.getInstance().setAutoFight(true);
            //隐藏箭头
            playerOperate.getInstance().setNoTouch();
        }
        public void putOrder(string targetKey, int action, string skillKey)
        {
            string posKey = fightCache.getInstance().getControlRolePosKey();
            if (!this.isRole) posKey = fightCache.getInstance().getControlPetPosKey();
            orderOperate.getInstance().putOrder(posKey, action, skillKey);
            orderOperate.getInstance().putTarget(targetKey);
        }
        /**显示菜单*/
        public void showMenu(bool isRole)
        {
            this.isRole = isRole;
            //将技能图标变换
            if (!fightCache.getInstance().getIsAutoFight())
            {//非自动情况下才展示
                showSkillCard(true);
                showDjs(true);
            }
            visibleCheXiao(false);
        }
        /**这个是多个玩家战斗时等待其他玩家下命令的菜单显示*/
        public void waitMenu()
        {
            //命令菜单不显示，只显示倒计时
            showSkillCard(false);
            showDjs(true);
            setTipText("等待其他玩家下达命令");
        }
        public void setTipText(string str)
        {
            PointGet.getPartsLayer().Find("huihe/tip").GetComponent<TextUI>().setText(str);
        }
        public void hideMenu()
        {
            showSkillCard(false);
            showDjs(false);
        }
        public void showSkillName(string text, string posKey)
        {
            GameObject partsLayer = PointGet.getPartsLayer().gameObject;
            modelMsgBind mb = playerOperate.getInstance().getPlayer(posKey);
            Vector2 vs = mb.transform.localPosition;
            GameObject tx1 = gameObjPool.getInstance().get("buff", typeof(TextUI));
            tx1.transform.SetParent(partsLayer.transform, false);
            tx1.GetComponent<TextUI>().setText(text).setColor("#23CDF1").setAlign("center").setFontSize(40)
            .setSize(new Vector2(600, 200));
            tx1.transform.localScale = Vector2.one * 0.5f;
            tx1.transform.localPosition =
                new Vector2(vs.x, vs.y + 170f);
            UIOutline ul = tx1.AddComponent<UIOutline>();
            ul.effectDistance = new Vector2(2, -2);
            tx1.AddComponent<hurtTextUpTween>().setPos(0, 50);


        }
        public void showBuff(string text, string color, string posKey, float dy = 0)
        {
            //todo:多个buff提示时需要向下排列
            GameObject partsLayer = PointGet.getPartsLayer().gameObject;
            /*modelMsgBind mb = playerOperate.getInstance().getPlayer(posKey);
            Vector2 vs = mb.transform.localPosition;*/
            //要目标原来的位置
            Vector2 vs = playerOperate.getInstance().getPosByPosKey(posKey);
            GameObject tx1 = gameObjPool.getInstance().get("buff", typeof(TextUI));
            tx1.transform.SetParent(partsLayer.transform, false);
            tx1.GetComponent<TextUI>().setText(text).setColor(color).setAlign().setFontSize(40).setFontStyle().horiOut()
            .setSize(new Vector2(200, 50));
            tx1.transform.localScale = Vector2.one * 0.5f;
            tx1.transform.localPosition =
                new Vector2(vs.x + (posKey.Contains("r") ? -200 : 200), vs.y + 100f + dy);
            tx1.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
            tx1.AddComponent<hurtTextUpTween>().setPos(0, 50);
        }
        public void showHurt(string text, string posKey, int isBaoji, float dy = 0)
        {
            GameObject partsLayer = PointGet.getPartsLayer().gameObject;
            /*modelMsgBind mb = playerOperate.getInstance().getPlayer(posKey);
            Vector2 vs = mb.transform.localPosition;*/
            //要目标原来的位置
            Vector2 vs = playerOperate.getInstance().getPosByPosKey(posKey);
            string png = "hphurt_png";
            if (!text.Contains("-"))
            {
                png = "jiahp_png";
                text = "+" + text;
            }
            if (true)
            {
                GameObject tx1 = gameObjPool.getInstance().get("shanghai", typeof(TextImgUI));
                tx1.transform.SetParent(partsLayer.transform, false);
                tx1.GetComponent<TextImgUI>().setSize(new Vector2(600, 200));
                tx1.GetComponent<TextImgUI>().setBase(new Vector2Int(11, 14), "0123456789-+", 60).loadRes(png).setText(text);
                tx1.transform.localScale = Vector2.one * 0.5f;
                tx1.transform.localPosition =
                    new Vector2(vs.x, vs.y + 150f + dy);
                tx1.AddComponent<hurtTextUpTween>().setPos(0, 50);
            }

            if (isBaoji == 1)
            {
                GameObject tx1 = gameObjPool.getInstance().get("buff", typeof(TextUI));
                tx1.transform.SetParent(partsLayer.transform, false);
                tx1.GetComponent<TextUI>().setText("暴击").setColor("#FF8E00").setAlign().setFontSize(50).setFontStyle().horiOut()
                .setSize(new Vector2(200, 60));
                tx1.transform.localScale = Vector2.one * 0.5f;
                tx1.transform.localPosition =
                    new Vector2(vs.x, vs.y + 150f + dy);
                tx1.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
                tx1.AddComponent<hurtTextUpTween>().setSpeed(20).setPos(0, 170);
            }
        }
        /**给模型绘制沙漏*/
        public void drawHourglass()
        {

            Transform partsLayer = PointGet.getPartsLayer();
            Transform ts = PointGet.getModelLayer();
            for (int i = 0; i < ts.childCount; i++)
            {
                Transform a = ts.GetChild(i);
                //必须是未隐藏的才可以
                if (!a.gameObject.activeSelf) continue;
                modelMsgBind mb = a.GetComponent<modelMsgBind>();
                //战斗玩家、怪物模型必然带有modelMsgBind，没有就是灯光之类的玩意
                if (mb == null || (mb.msg.roleType != 0 && mb.msg.roleType != 1)) continue;

                GameObject shalou = gameObjPool.getInstance().get("shalou-" + mb.msg.posKey, typeof(ImgUI));
                shalou.transform.SetParent(partsLayer.transform, false);
                shalou.GetComponent<ImgUI>().loadRes("funnel_png").addCallback((sp) =>
                {
                    shalou.AddComponent<shalouRotation>().init(sp);
                }).setNoClk().setSize(new Vector2(50, 50));
                shalou.transform.localPosition = new Vector2(a.transform.localPosition.x, a.transform.localPosition.y + 300);

            }
            DoGet.getInstance().startReqImg();
        }
        public void clearShalou(string posKey)
        {
            Transform partsLayer = PointGet.getPartsLayer();
            Transform ts = partsLayer.Find("shalou-" + posKey);
            if (ts != null)
            {
                gameObjPool.getInstance().free(ts.gameObject);
            }
        }

        /**显示技能选项卡*/
        public void showSkillCard(bool b)
        {
            Transform partsLayer = PointGet.getPartsLayer();
            if (partsLayer.Find("menu") == null) return;
            if (b)
            {
                partsLayer.Find("menu").gameObject.SetActive(true);
            }
            else
            {
                partsLayer.Find("menu").gameObject.SetActive(false);
            }

            setTipText("请下达" + (isRole ? "玩家" : "宠物") + "命令");
        }

        private void drawMenu(GameObject partsLayer)
        {
            GameObject bottomCard = gameObjPool.getInstance().get("menu", typeof(SimpleUI));
            bottomCard.transform.SetParent(partsLayer.transform, false);
            bottomCard.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(600, 600), new Vector2((ScreenUtils.width - 600) / 2, -(ScreenUtils.height - 600) / 2));

            //技
            GameObject item1 = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item1.transform.SetParent(bottomCard.transform, false);
            item1.GetComponent<ImgUI>().loadRes("kk_02_png")
                .setSizePos(new Vector2(120, 61), new Vector2(240, -50));
            GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item1.transform, false);
            icon.GetComponent<ImgUI>().loadRes("zd_01_png", new Rect(0, 0, 20, 18))
                .setSizePos(new Vector2(80, 72), new Vector2(20, 5.5f))
                .addClk(() =>
                {
                    JArray coolList = fightCache.getInstance().getCoolList();
                    List<string> ml = new List<string>();
                    JArray arr;
                    string posKey;
                    if (isRole)
                    {
                        arr = fightCache.getInstance().getControlRoleSkill();
                        posKey = fightCache.getInstance().getControlRolePosKey();
                    }
                    else
                    {
                        arr = fightCache.getInstance().getControlPetSkill();
                        posKey = fightCache.getInstance().getControlPetPosKey();
                    }

                    //Debug.Log(coolList);
                    //先去除被动技能
                    for (int i = 0; i < arr.Count; i++)
                    {
                        Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(arr[i].ToString());
                        //过滤被动技能
                        if (skl.triggerType == 1)
                        {
                            arr.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    //再判断是否冷却
                    for (int i = 0; i < arr.Count; i++)
                    {
                        Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(arr[i].ToString());
                        string sklName = skl.name;
                        for (int j = 0; j < coolList.Count; j++)
                        {
                            JObject a = (JObject)coolList[j];
                            if (a["posKey"].ToString().Equals(posKey) && a["skillKey"].ToString().Equals(skl.key))
                            {
                                sklName += "（冷）";
                                break;
                            }
                        }
                        ml.Add(sklName);
                    }

                    if (isRole)
                    {
                        ml.Add("捕捉");
                    }


                    Menu menu = Menu.create(ml, PointGet.getPartsLayer());
                    menu.addCallback((mIndex) =>
                    {
                        if (ml[mIndex].Contains("（冷）"))
                        {
                            msgCode.showMsg(982);
                            return;
                        }
                        string key = null;
                        if (!ml[mIndex].Equals("捕捉")) key = arr[mIndex].ToString();
                        else
                        {
                            if ((int)fightCache.getInstance().downloadFightMsg["isCatch"] != 1)
                            {
                                msgCode.showMsg(1019);
                                return;
                            }
                        }
                        handleSkl(ml[mIndex], key);
                    });
                });
            //逃
            GameObject item2 = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item2.transform.SetParent(bottomCard.transform, false);
            item2.GetComponent<ImgUI>().loadRes("kk_02_png")
                .setSizePos(new Vector2(120, 61), new Vector2(480, -269.5f));
            icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item2.transform, false);
            icon.GetComponent<ImgUI>().loadRes("zd_01_png", new Rect(20, 0, 20, 18))
                .setSizePos(new Vector2(80, 72), new Vector2(20, 5.5f)).addClk(() =>
                {
                    if (!isRole)
                    {
                        msgCode.showMsg(981);
                        return;
                    }
                    MsgSureUI.create(PointGet.getPartsLayer()).show("是否选择逃跑？", () =>
                    {
                        this.showSkillCard(false);
                        putOrder(2, null);
                        string leavePosKey = fightCache.getInstance().getControlRolePosKey();
                        if (!isRole)
                        {
                            //leavePosKey = fightCache.getInstance().getControlPetPosKey();
                        }
                        orderOperate.getInstance().putTarget(leavePosKey);
                    }, () => { }).setMaskAlpha(0);
                });
            //物
            GameObject item3 = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item3.transform.SetParent(bottomCard.transform, false);
            item3.GetComponent<ImgUI>().loadRes("kk_02_png")
                .setSizePos(new Vector2(120, 61), new Vector2(240, -600 + 61 + 50));
            icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item3.transform, false);
            icon.GetComponent<ImgUI>().loadRes("zd_01_png", new Rect(40, 0, 20, 18))
                .setSizePos(new Vector2(80, 72), new Vector2(20, 5.5f)).addClk(() =>
                {
                    if (true) return;
                    //action=3
                    List<string> ml = new List<string>();
                    Menu menu = Menu.create(ml, PointGet.getPartsLayer());
                    menu.addCallback((mIndex) =>
                    {
                        Debug.Log(mIndex);
                    });
                });
            //攻
            GameObject item4 = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item4.transform.SetParent(bottomCard.transform, false);
            item4.GetComponent<ImgUI>().loadRes("kk_02_png")
                .setSizePos(new Vector2(120, 61), new Vector2(0, -269.5f));
            icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item4.transform, false);
            icon.GetComponent<ImgUI>().loadRes("zd_01_png", new Rect(60, 0, 20, 18))
                .setSizePos(new Vector2(80, 72), new Vector2(20, 5.5f)).addClk(() =>
                {
                    this.showSkillCard(false);
                    putOrder(0, null);
                    playerOperate.getInstance().setAllowedTouch(2, isRole);
                    setTipText("请选择敌方目标");
                    visibleCheXiao(true);
                });
            //防
            GameObject item5 = gameObjPool.getInstance().get("item", typeof(ImgUI));
            item5.transform.SetParent(bottomCard.transform, false);
            item5.GetComponent<ImgUI>().loadRes("kk_02_png")
                .setSizePos(new Vector2(120, 61), new Vector2(240, -269.5f));
            icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(item5.transform, false);
            icon.GetComponent<ImgUI>().loadRes("zd_01_png", new Rect(80, 0, 20, 18))
                .setSizePos(new Vector2(80, 72), new Vector2(20, 5.5f)).addClk(() =>
                {
                    //action=5
                    putOrder(5, null);
                    string posKey = fightCache.getInstance().getControlRolePosKey();
                    if (!this.isRole) posKey = fightCache.getInstance().getControlPetPosKey();
                    orderOperate.getInstance().putTarget(posKey);
                });
        }
        private void handleSkl(string name, string sklKey)
        {
            this.showSkillCard(false);

            if (name.Equals("捕捉"))
            {
                putOrder(4, null);
                playerOperate.getInstance().setAllowedTouch(2, isRole);
                setTipText("请选择敌方目标");
            }
            else
            {
                //选择对象
                Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(sklKey);
                int target = skl.target;
                putOrder(1, sklKey);
                playerOperate.getInstance().setAllowedTouch(target, isRole);
                string str;
                if (target == 0) str = "自己";
                else if (target == 1) str = "己方目标";
                else str = "敌方目标";

                setTipText("请选择" + str);
            }
            visibleCheXiao(true);
        }
        public void putOrder(int action, string skillKey)
        {
            string posKey = fightCache.getInstance().getControlRolePosKey();
            if (!this.isRole) posKey = fightCache.getInstance().getControlPetPosKey();
            orderOperate.getInstance().putOrder(posKey, action, skillKey);
        }
        /**自动*/
        public void autoMakeUpOrder(bool isAuto)
        {
            string role = fightCache.getInstance().getControlRolePosKey();
            if (isAuto)
                orderOperate.getInstance().putAutoOrder(role);
            else
                orderOperate.getInstance().putCancelAutoOrder(role);
        }
        public void visiblebottomBtn(bool b)
        {
            GameObject partsLayer = PointGet.getPartsLayer().gameObject;
            partsLayer.transform.Find("bottomBtn").gameObject.SetActive(b);
        }
        /**显示/关闭撤销按钮*/
        private void visibleCheXiao(bool b)
        {
            PointGet.getPartsLayer().Find("bottomBtn/chexiao").gameObject.SetActive(b);
        }
        private void drawBtn(GameObject partsLayer)
        {
            GameObject bottomCard = gameObjPool.getInstance().get("bottomBtn", typeof(SimpleUI));
            bottomCard.transform.SetParent(partsLayer.transform, false);
            bottomCard.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 96), new Vector2(0, -ScreenUtils.height + 396));
            bottomCard.SetActive(false);

            GameObject auto = gameObjPool.getInstance().get("auto", typeof(BtnUI));
            auto.transform.SetParent(bottomCard.transform, false);
            auto.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(ScreenUtils.width - 168, 0));
            auto.GetComponent<BtnUI>().addText(fightCache.getInstance().getIsAutoFight() ? "手动" : "自动").setTextColor().loadRes("gy_02_png")
                .addClk(() =>
                {
                    //将所有箭头隐藏
                    playerOperate.getInstance().setNoTouch();
                    //战斗过程中点击这个按钮会导致设置无效，改成准备阶段才显示这个按钮
                    if (fightCache.getInstance().getIsAutoFight())
                    {
                        fightCache.getInstance().setAutoFight(false);
                        //取消服务器上的自动战斗
                        this.autoMakeUpOrder(false);
                        auto.GetComponent<BtnUI>().addText("自动");
                        //显示菜单
                        showMenu(true);
                    }
                    else
                    {
                        fightCache.getInstance().setAutoFight(true);
                        //启用服务器上的自动战斗
                        this.autoMakeUpOrder(true);
                        auto.GetComponent<BtnUI>().addText("手动");
                        //将菜单隐藏
                        waitMenu();
                    }
                });

            //点击攻击或者菜单技能时显示
            GameObject chexiao = gameObjPool.getInstance().get("chexiao", typeof(BtnUI));
            chexiao.transform.SetParent(bottomCard.transform, false);
            chexiao.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(ScreenUtils.width - 168, 0));
            chexiao.GetComponent<BtnUI>().addText("撤销").setTextColor().loadRes("gy_02_png")
                .addClk(() =>
                {
                    //将所有箭头隐藏
                    playerOperate.getInstance().setNoTouch();
                    showMenu(true);
                });
            chexiao.SetActive(false);
        }
        private void drawHuihe(GameObject partsLayer)
        {
            GameObject bottomCard = gameObjPool.getInstance().get("huihe", typeof(SimpleUI));
            bottomCard.transform.SetParent(partsLayer.transform, false);
            bottomCard.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(300, 39 * 2), new Vector2((ScreenUtils.width - 300) / 2, -100));
            GameObject text = gameObjPool.getInstance().get("text", typeof(TextUI));
            text.transform.SetParent(bottomCard.transform, false);
            text.GetComponent<TextUI>().setText("第1回合").setColor("#FF0000").setFontSize(35).setAlign().setNoClk()
                .setSizePos(new Vector2(300, 50), Vector2.zero);
            text.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));
            //倒计时
            GameObject tx1 = gameObjPool.getInstance().get("djs", typeof(TextImgUI));
            tx1.transform.SetParent(bottomCard.transform, false);
            tx1.GetComponent<TextImgUI>()
            .setSizePos(new Vector2(300, 50), new Vector2(0, -60));
            tx1.GetComponent<TextImgUI>().setBase(new Vector2Int(11, 14), "0123456789", 50).loadRes("zd_22_png").setText("30");
            tx1.AddComponent<timeCutAm>().addCancelCall(() =>
            {

            }).resetTime();

            GameObject tip = gameObjPool.getInstance().get("tip", typeof(TextUI));
            tip.transform.SetParent(bottomCard.transform, false);
            tip.GetComponent<TextUI>().setText("请下达命令").setColor("#FF0000").setFontSize(35).setAlign().setNoClk()
                .setSizePos(new Vector2(300, 50), new Vector2(0, -60 * 2));
            tip.AddComponent<UIOutline>().setSome(Color.black, new Vector2(2, -2));



        }
        public void setHuiheNum(string num)
        {
            //Debug.Log(num + "|" + (PointGet.getPartsLayer()==null));
            Transform ts = PointGet.getPartsLayer().Find("huihe");
            if (ts == null) return;
            ts.Find("text").GetComponent<TextUI>().setText("第" + num + "回合");
        }
        public void showDjs(bool b)
        {
            Transform ts = PointGet.getPartsLayer().Find("huihe");
            if (ts == null) return;
            ts.gameObject.SetActive(b);
        }
        /**重置倒计时*/
        public void resetDjs()
        {
            Transform ts = PointGet.getPartsLayer().Find("huihe");
            if (ts == null) return;
            ts.Find("djs").GetComponent<timeCutAm>().init().resetTime();
        }
        public void drawTeamMemberHead(GameObject partsLayer)
        {
            GameObject icon = gameObjPool.getInstance().get("TeamIcon", typeof(SimpleUI));
            icon.transform.SetParent(partsLayer.transform, false);
            icon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(350, 200), new Vector2(ScreenUtils.width - 350, -96));
            drawTeamMemberIcon(icon.transform);
        }
        private void drawTeamMemberIcon(Transform ts)
        {
            //获取队员，向下排列
            List<string> frList = fightCache.getInstance().getFriendPosKeyExpSelf();
            float dx = 12 * 50 / 16f;

            for (int i = 0; i < frList.Count; i++)
            {
                GameObject memItem = gameObjPool.getInstance().get("memItem-" + frList[i], typeof(SimpleUI));
                memItem.transform.SetParent(ts, false);
                memItem.GetComponent<SimpleUI>()
                    .setSizePos(new Vector2(300 + dx, 50), new Vector2(0, -60 * i));

                GameObject a4 = gameObjPool.getInstance().get("a4", typeof(ImgUI));
                a4.transform.SetParent(memItem.transform, false);
                a4.GetComponent<ImgUI>().loadRes("cg_a4_png")
                    .setSizePos(new Vector2(dx, 50), Vector2.zero);

                GameObject manIcon = gameObjPool.getInstance().get("ManIcon", typeof(ImgUI));
                manIcon.transform.SetParent(memItem.transform, false);
                manIcon.GetComponent<ImgUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 50), new Vector2(dx, 0));

                GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(manIcon.transform, false);
                icon.GetComponent<ImgUI>().loadRes(GameAttrConst.getZdRoleHead(null) + "_png")
                .setSizePos(new Vector2(50, 50), new Vector2(0, 0));

                //宠物
                manIcon = gameObjPool.getInstance().get("PetIcon", typeof(ImgUI));
                manIcon.transform.SetParent(memItem.transform, false);
                manIcon.GetComponent<ImgUI>().setColor("#000000")
                    .setSizePos(new Vector2(150, 50), new Vector2(dx + 150, 0));

                icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
                icon.transform.SetParent(manIcon.transform, false);
                icon.GetComponent<ImgUI>().loadRes("zd_a1_png")
                .setSizePos(new Vector2(50, 50), new Vector2(0, 0));

                updateTeamMemberRoleXueTiao(frList[i]);
                updateTeamMemberPetXueTiao(frList[i]);
            }

        }
        public void updateTeamMemberRoleXueTiao(string posKey)
        {
            Transform bg = PointGet.getPartsLayer().Find("TeamIcon/memItem-" + posKey + "/ManIcon");
            if (bg == null) return;
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }

            JObject obj = fightCache.getInstance().getMsgByPosKey(posKey);
            JObject prop = (JObject)obj["prop"];
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(80, 20), new Vector2(60, -5));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(80, 20), new Vector2(60, -25f));
            drawTiao(mp, 1, prop);
        }
        public void updateTeamMemberPetXueTiao(string posKey)
        {
            Transform bg = PointGet.getPartsLayer().Find("TeamIcon/memItem-" + posKey + "/PetIcon");
            //Debug.Log("TeamIcon/memItem-" + posKey + "/PetIcon");
            if (bg == null) return;
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }
            
            JObject obj = fightCache.getInstance().getMsgByPosKey(posKey);
            if (obj == null) return;

            JObject prop = (JObject)obj["prop"];
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(80, 20), new Vector2(60, -5));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(80, 20), new Vector2(60, -25f));
            drawTiao(mp, 1, prop);
        }

        private void drawHead(GameObject partsLayer)
        {
            GameObject icon = gameObjPool.getInstance().get("Icon", typeof(SimpleUI));
            icon.transform.SetParent(partsLayer.transform, false);
            icon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(403, 200), Vector2.zero);
            drawIcon(icon.transform);
        }
        private void drawIcon(Transform ts)
        {
            GameObject manIcon = gameObjPool.getInstance().get("ManIcon", typeof(SimpleUI));
            manIcon.transform.SetParent(ts, false);
            manIcon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(304, 200), Vector2.zero);

            GameObject bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            bg.transform.SetParent(manIcon.transform, false);
            bg.GetComponent<ImgUI>().loadRes("cg_a1_png")
                .setSizePos(new Vector2(304, 200), Vector2.zero);

            JObject role = face.roleInterface.getRole();
            GameObject icon = gameObjPool.getInstance().get("icon", typeof(ImgUI));
            icon.transform.SetParent(bg.transform, false);
            icon.GetComponent<ImgUI>().loadRes(GameAttrConst.getRoleHead(role) + "_png")
            .setSizePos(new Vector2(120, 120), new Vector2(28, -22));

            updateRoleXueTiao();
            //宠物
            manIcon = gameObjPool.getInstance().get("PetIcon", typeof(SimpleUI));
            manIcon.transform.SetParent(ts, false);
            manIcon.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(243, 100), new Vector2(160, -50));

            bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            bg.transform.SetParent(manIcon.transform, false);
            bg.GetComponent<ImgUI>().loadRes("cg_a2_png")
                .setSizePos(new Vector2(243, 100), Vector2.zero);

            updatePetHeadIcon();
            updatePetXueTiao();
        }
        public void updatePetHeadIcon()
        {
            Transform bg = PointGet.getPartsLayer().Find("Icon/PetIcon/Bg");
            if (bg.Find("icon") != null) gameObjPool.getInstance().free(bg.Find("icon").gameObject);
            JObject a = face.petInterface.getIsFightPet();
            if (a != null)
            {
                Pet pet = face.petInterface.getPetDataByKey(a["key"].ToString());
                AmModelMsg am = amManager.getAmByKey(pet.pwdId);
                string[] pwds = am.touxiangPwds;
                string[] aefs = { am.touxiangAef };
                List<string> all = new List<string>();
                all.AddRange(pwds);
                all.AddRange(aefs);
                DoGet.getInstance().collectionAnyRes(() =>
                {
                    List<GameObject> list = readModel.drawGameObj(bg, pwds, aefs);

                    GameObject g = list[0].gameObject;
                    Transform ts0 = g.transform.GetChild(0);
                    Transform ts1 = ts0.GetChild(1);
                    if (ts1.childCount > 0)
                    {
                        Transform img = ts1.GetChild(0);
                        img.name = "icon";
                        img.localScale = 3 * Vector2.one;
                        img.transform.SetParent(bg.transform, false);
                        img.transform.localPosition = new Vector2((-243 + 24 * 3) / 2f + 10, 0);
                    }
                       
                    gameObjPool.getInstance().free(g);
                }, all);
            }

        }

        public void updatePetXueTiao()
        {
            Transform bg = PointGet.getPartsLayer().Find("Icon/PetIcon/Bg");
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }
            string petPosKey = fightCache.getInstance().getControlPetPosKey();
            if (petPosKey == null) return;
            JObject obj = fightCache.getInstance().getMsgByPosKey(petPosKey);
            JObject prop = (JObject)obj["prop"];
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(110, 13), new Vector2(86, -52f));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(110, 13), new Vector2(86, -74f));
            drawTiao(mp, 1, prop);
        }
        public void updateRoleXueTiao()
        {
            Transform bg = PointGet.getPartsLayer().Find("Icon/ManIcon/Bg");
            if (bg.Find("hp") != null)
            {
                gameObjPool.getInstance().free(bg.Find("hp").gameObject);
                gameObjPool.getInstance().free(bg.Find("mp").gameObject);
            }
            JObject obj = fightCache.getInstance().getMsgByPosKey(fightCache.getInstance().getControlRolePosKey());
            JObject prop = (JObject)obj["prop"];
            GameObject hp = gameObjPool.getInstance().get("hp", typeof(SimpleUI));
            hp.transform.SetParent(bg.transform, false);
            hp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(192, 13), new Vector2(38, -157.9f));
            drawTiao(hp, 0, prop);

            GameObject mp = gameObjPool.getInstance().get("mp", typeof(SimpleUI));
            mp.transform.SetParent(bg.transform, false);
            mp.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(192, 13), new Vector2(38, -179.3f));
            drawTiao(mp, 1, prop);
        }
        private void drawTiao(GameObject attr, int i, JObject prop)
        {
            Vector2 size = attr.GetComponent<RectTransform>().sizeDelta;
            string k1 = "xue";
            string k2 = "max_xue";
            Color32 color = new Color32(255, 16, 0, 255);
            Color32 color1 = new Color32(172, 11, 0, 255);
            if (i == 1)
            {
                k1 = "lan";
                k2 = "max_lan";
                color = new Color32(0, 174, 255, 255);
                color1 = new Color32(2, 140, 205, 255);
            }
            else if (i == 2)
            {
                k1 = "exp";
                k2 = "max_exp";
                color = new Color32(255, 183, 0, 255);
                color1 = new Color32(192, 137, 0, 255);
            }

            GameObject jdBg = gameObjPool.getInstance().get("jdBg", typeof(ImgUI));
            jdBg.transform.SetParent(attr.transform, false);
            jdBg.GetComponent<ImgUI>().setColor("#555555")
                .setSizePos(new Vector2(size.x, size.y), new Vector2(0, 0));
            float rate = (float)prop[k1] / (float)prop[k2];
            if (rate > 1) rate = 1;
            else if (rate < 0) rate = 0;
            GameObject jdt = gameObjPool.getInstance().get("jdt", typeof(ImgUI));
            jdt.transform.SetParent(jdBg.transform, false);
            jdt.GetComponent<ImgUI>()
                .setSizePos(new Vector2((size.x - 6) * rate, size.y - 6), new Vector2(3, -3));
            GradientDefinded gd = jdt.AddComponent<GradientDefinded>();
            gd.color1 = color;
            gd.color2 = color1;


            /*GameObject exp = gameObjPool.getInstance().get("str", typeof(TextUI));
            exp.transform.SetParent(jdBg.transform, false);
            exp.GetComponent<TextUI>().setColor("#ffffff").setAlign("right").setFontSize(24).setText(prop[k1] + "/" + prop[k2])
                .setSizePos(new Vector2(320, 30), new Vector2(0, 30));*/
        }

        private void drawChatWindow(GameObject partsLayer)
        {
            GameObject talk = gameObjPool.getInstance().get("Talk", typeof(SimpleUI));
            talk.transform.SetParent(partsLayer.transform, false);
            talk.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 300), new Vector2(0, -(ScreenUtils.height - 300)))
                .addClk(() =>
                {
                    PageUI.createAcPage<TalkPage>(PointGet.getFightPageOfPage()).drawUI(1);

                });
            drawTalk(talk.transform);
            talk.SetActive(false);
            GameObject laba = gameObjPool.getInstance().get("Laba", typeof(ImgUI));
            laba.transform.SetParent(partsLayer.transform, false);
            laba.GetComponent<ImgUI>().loadRes("pount_png")
                .setSizePos(new Vector2(60, 60), new Vector2((ScreenUtils.width - 60) / 2f, -ScreenUtils.height + 96 - 18)).addClk(() =>
                {
                    PageUI.createAcPage<TalkPage>(PointGet.getFightPageOfPage()).drawUI(1);
                });
            laba.SetActive(false);

            GameObject a4 = gameObjPool.getInstance().get("StopMeet", typeof(ImgUI));
            a4.transform.SetParent(partsLayer.transform, false);
            a4.GetComponent<ImgUI>().loadRes("stopmeet_png")
                .setSizePos(new Vector2(57 * 3, 21 * 3), new Vector2((ScreenUtils.width - 57 * 3) / 2f, -ScreenUtils.height + 96)).addClk(() =>
                {
                    a4.SetActive(false);
                    face.roleInterface.setStatusLan("ydxc", 0, (r) => { });
                });
            //如果诱敌香草未开启的话就不显示
            JObject role = face.roleInterface.getRole();
            if ((int)role["attr"]["status"]["ydxc"]["isOpen"] != 1)
            {
                a4.SetActive(false);
            }

            if (fightCache.getInstance().isViewFight)
            {
                GameObject auto = gameObjPool.getInstance().get("tuichuView", typeof(BtnUI));
                auto.transform.SetParent(partsLayer.transform, false);
                auto.GetComponent<BtnUI>()
                .setSizePos(new Vector2(168, 96), new Vector2(0, -ScreenUtils.height + 96));
                auto.GetComponent<BtnUI>().addText("退出").setTextColor().loadRes("gy_03_png")
                    .addClk(() =>
                    {
                        face.fightInterface.closeViewFight(() => { });
                    });
            }
        }
        public void isHideChatWindow()
        {
            Transform partsLayer = PointGet.getPartsLayer();
            RectTransform rc = PointGet.getMapMapePoint().GetComponent<RectTransform>();
            if (rc.sizeDelta.y > ScreenUtils.height)
            {
                //地图高比屏幕高还高时，隐藏聊天窗
                partsLayer.Find("Talk").gameObject.SetActive(false);
                partsLayer.Find("StopMeet").GetComponent<ImgUI>().setLeftPos(new Vector2((ScreenUtils.width - 57 * 3) / 2f, -ScreenUtils.height + 96 + 100));
                //底部滑到最低点
                if (partsLayer.Find("bottomBtn") != null)
                {
                    partsLayer.Find("bottomBtn").GetComponent<SimpleUI>().setLeftPos(new Vector2(0, -ScreenUtils.height + 96));
                }
                //绘制一个小图标即可
                partsLayer.Find("Laba").gameObject.SetActive(true);
                return;
            }
            partsLayer.Find("Talk").gameObject.SetActive(true);
            partsLayer.Find("StopMeet").GetComponent<ImgUI>().setLeftPos(new Vector2((ScreenUtils.width - 57 * 3) / 2f, -ScreenUtils.height + 96 + 400));

        }
        private void drawTalk(Transform ts)
        {
            GameObject Bg = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            Bg.transform.SetParent(ts, false);
            Bg.GetComponent<ImgUI>().setColor("#00BDFF")
                .setSizePos(new Vector2(ScreenUtils.width, 300), Vector2.zero);

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat1_png")
                .setSizePos(new Vector2(30, 30), Vector2.zero);

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat2_png")
                .setSizePos(new Vector2(30, 30), new Vector2(ScreenUtils.width - 30, 0));

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat4_png")
                .setSizePos(new Vector2(30, 30), new Vector2(0, -270));

            kuang = gameObjPool.getInstance().get("kuang", typeof(ImgUI));
            kuang.transform.SetParent(ts, false);
            kuang.GetComponent<ImgUI>().loadRes("chat3_png")
                .setSizePos(new Vector2(30, 30), new Vector2(ScreenUtils.width - 30, -270));
        }



    }
}
