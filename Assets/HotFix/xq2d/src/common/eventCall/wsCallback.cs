using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.fight;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.activityPage;
using Assets.HotFix.xq2d.src.ui.page;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common.eventCall
{
    class wsCallback
    {
        private static wsCallback ws;
        public static wsCallback getInstance()
        {
            if (ws == null) ws = new wsCallback();
            return ws;
        }
        public void call(JToken obj)
        {
            //Debug.Log(obj);
            if (obj == null) return;
            JObject jb = null;
            try
            {
                jb = (JObject)obj;
            }
            catch (Exception e)
            {
                Debug.Log("不是json对象：" + obj.ToString());
                return;
            }
            JToken callback;
            if (!jb.TryGetValue("callback", out callback)) return;

            string callbackCode = (string)obj["callback"];
            object msg = obj["msg"];

            switch (callbackCode)
            {
                //动态创建npc
                case "102":
                    {
                        if (fightCache.getInstance().isDoing) return;
                        JObject a = (JObject)msg;
                        JArray list = (JArray)a["list"];
                        string mapKey = a["mapKey"].ToString();
                        //判断是否在指定的地图，先清理后创建
                        if (!face.roleInterface.isInMap(mapKey)) return;
                        if (a["clearList"] != null)
                        {
                            JArray clearList = (JArray)a["clearList"];
                            npcManager.clearNpc(clearList);
                        }
                        npcManager.appendNpc(list);
                        return;
                    }
                //公共奖励通知
                case "103":
                    {
                        JObject a = (JObject)msg;
                        string name = a["name"].ToString();
                        JArray keys = (JArray)a["keys"];
                        int getPath = (int)a["getPath"];
                        string str = "";
                        //解析
                        foreach (object k in keys)
                        {
                            GoodsDes g = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(k.ToString());
                            if (g == null) continue;
                            str += g.name + "、";
                        }
                        str = str.Substring(0, str.Length - 1);
                        string s = face.rewardInterface.getPathByKey(getPath);
                        face.chatInterface.writeSysMsg("这怕是要上天哟！玩家【" + name + "】 通过【" + s + "】获得了稀有道具 " + str);

                        return;
                    }
                //刷出聊天消息
                case "106":
                    {

                        taskManager.getInstance().putTask(() =>
                        {
                            Transform ctp = PointGet.getAcPage<TalkPage>();
                            if (ctp != null)
                            {
                                ctp.GetComponent<TalkPage>().updateChatWindow();
                            }
                            Transform indexPage = PointGet.getIndexPage();
                            if (indexPage != null)
                            {
                                indexPage.GetComponent<IndexPage>().updateChatMsg();
                            }

                        });

                        return;
                    }
                //帮战宝箱积分通知
                case "107":
                    {
                        //textTipUI.getInstance().setText("获得积分" + msg);
                        face.chatInterface.writeSysMsg("获得帮战积分" + msg);
                        return;
                    }
                //帮战积分排行通知
                case "108":
                    {
                        if (face.roleInterface.isInMap("bz"))
                        {
                            //bzJfOrderUI.getInstance().putTaskContent((JArray)msg);
                        }

                        return;
                    }
                //战斗结束后练潜装备的修改
                case "109":
                    {
                        JObject o = (JObject)msg;
                        JObject equip = face.goodsInterface.getPlayerGoodsById(o["Id"].ToString());
                        equip["num"] = 0;
                        equip["potential"]["num"] = (int)o["potential"]["num"];
                        equip["inlay"] = o["inlay"];
                        face.goodsInterface.savePlayerGoods(equip);
                        return;
                    }
                //变身卡效果到期
                case "110":
                    {
                        face.chatInterface.writeSysMsg("变身效果消失！");
                        JObject r = face.roleInterface.getRole();
                        r["attr"]["shenfu"] = null;
                        face.roleInterface.saveRole(r);

                        controlManager.updateShenFu();
                        return;
                    }
                //变身卡效果提示
                case "111":
                    {
                        controlManager.updateShenFu();
                        return;
                    }
                //按code给提示 {code,params,channel}
                case "700":
                    {
                        JObject o = (JObject)msg;
                        string str = msgCode.getStrByCode((int)o["code"], o["params"]);
                        face.chatInterface.writeSysMsg(str, (int)o["channel"]);
                        return;
                    }

                //刷新人物/宠物模型
                case "793":
                    {
                        //todo:刷新人物模型（换装、特效、宠物、坐骑、称号、翅膀等）
                        controlManager.updatePetModel();
                        return;
                    }
                //聊天窗通知
                case "794":
                    {
                        face.chatInterface.writeSysMsg(msg.ToString());
                        return;
                    }
                //抢购物品购买成功通知
                case "795":
                    {
                        SetGameObj.findAndRemChild(PointGet.getTipCanvas());
                        msgCode.showMsg(668);
                        return;
                    }
                case "796":
                    {
                        SetGameObj.findAndRemChild(PointGet.getTipCanvas());
                        JObject o = (JObject)msg;
                        JArray rewards = (JArray)o["list"];
                        face.rewardInterface.saveRewards(rewards);
                        face.roleInterface.updateMoney(-(int)o["price"], (int)o["priceType"]);
                        GoodsDes a = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(rewards[0]["goods"]["key"].ToString());
                        msgCode.showMsg(904, a.name);
                        return;
                    }
                //将抢购商品推送给未推送过的玩家
                case "797":
                    {
                        JObject o = (JObject)msg;
                        face.goodsInterface.saveQiangGouGoods(o);
                        //当界面打开时则刷新
                        Transform ac = PointGet.getAcPage<MiaoShaShopPage>();
                        if (ac != null)
                        {
                            ac.GetComponent<MiaoShaShopPage>().clkTab(0);
                        }
                        return;
                    }
                //提示通知
                case "799":
                    {
                        msgCode.showMsg(int.Parse(msg.ToString()));
                        return;
                    }
                //接收窗口消息
                case "800":
                    {
                        long a;
                        if (long.TryParse(msg + "", out a))
                        {
                            //放置拉取消息的时间点
                            face.chatInterface.putTime(a.ToString());
                            return;
                        }
                        //保存信息
                        foreach (object p in (JArray)msg)
                        {
                            face.chatInterface.receiveMsg(p);
                        }
                        return;
                    }

                case "801":
                    {
                        int type = int.Parse(msg.ToString());
                        if (PointGet.getIndexPage() != null)
                        {
                            PointGet.getIndexPage().GetComponent<IndexPage>().playTaskGainOrSubmitAm(type);
                        }
                        //更新npc头上状态
                        npcManager.updateTaskIcon();
                        return;
                    }
                //刷新邮件
                case "802":
                    {
                        //通知有新邮件，触发对新邮件的拉取，这里只接收邮件id
                        if (msg != null)
                        {
                            face.emailInterface.receiveEmail(msg.ToString());
                            //小邮标
                            if (PointGet.getIndexPage() != null)
                                PointGet.getIndexPage().GetComponent<IndexPage>().shan("emailBtn");
                        }
                        //更新邮件列表ui
                        Transform cp = PointGet.getAcPage<EmailPage>();
                        if (cp != null)
                        {
                            cp.GetComponent<EmailPage>().updateWindow();
                        }
                        return;
                    }
                //对玩家位置的监听
                case "803":
                    {
                        if (msg != null && msg.ToString().Equals("1"))
                        {
                            //避免战斗结算时，ws发送多次这个而调用多次请求导致频繁，而使加载地图时被拒绝加载资源
                            if (PointGet.getIndexPage() == null || fightCache.getInstance().isDoing ||
                                PointGet.getMapPoint() == null || PointGet.getMapMapePoint() == null) return;
                            //每次有同步数据就请求
                            face.playerInterface.getPlayerPos((res) =>
                            {
                                playerManager.getInstance().handlePlayerPos(res);
                            });
                            return;
                        }
                        return;
                    }
                //刷新任务/队伍ui
                case "804":
                    {
                        Transform ts1 = PointGet.getAcPage<TeamPage>();
                        if (ts1 != null && ts1.gameObject.activeSelf)
                        {
                            ts1.GetComponent<TeamPage>().updateWindow();
                        }
                        //显示或隐藏跟随按钮
                        if (PointGet.getIndexPage() != null)
                            PointGet.getIndexPage().GetComponent<IndexPage>().updateTeamStatus();
                        return;
                    }

                case "809":
                    {
                        //入队成功后同步队伍信息/请离队员/接受邀请后
                        if (msg != null)
                        {
                            face.teamInterface.inTeamSuc((JObject)msg);
                        }
                        else
                        {//队伍不存在时，视为主动离开队伍
                            face.teamInterface.delTeam();
                        }


                        return;
                    }
                //通告任务进度
                case "811":
                    {
                        JObject pt = (JObject)msg;
                        string name;
                        if ((int)pt["targetType"] == 0)
                        {//npcKey
                            NpcObjData npc = face.npcInterface.getNpc(pt["targetKey"].ToString());
                            name = npc.name;
                        }
                        else
                        {
                            JObject monster = face.monsterInterface.getMonster(pt["targetKey"].ToString());
                            name = monster["name"].ToString();
                        }
                        int num = (int)pt["num"];
                        int sum = (int)pt["sum"];
                        string str = "任务收集：" + name + "(" + (num > sum ? sum : num)
                            + '/' + sum + ')';
                        face.chatInterface.writeSysMsg(str);
                        return;
                    }
                //队长跳转地图后调用，接收同步队长位置（注意：这里是队长同步给队员）
                case "812":
                    {
                        //保存队长位置，用于点击跟随时判断是否处于同一地图
                        JObject o = (JObject)msg;
                        face.teamInterface.saveCaptainPos(o);
                        return;
                    }
                //接取副本任务时
                case "813":
                    {
                        //todo:已经改成由任务检测触发开启，而不用由入口npc手动接取
                        //根据k来决定创建哪些任务
                        string[] taskKeys = null;
                        if (msg.ToString().Equals("50"))
                        {
                            string[] arr = { "3163", "3164", "3165", "3166" };
                            taskKeys = arr;
                        }
                        else if (msg.ToString().Equals("60"))
                        {
                            string[] arr = { "3167", "3168", "3169", "3170" };
                            taskKeys = arr;
                        }
                        else if (msg.ToString().Equals("70"))
                        {
                            string[] arr = { "3171", "3172", "3173", "3174" };
                            taskKeys = arr;
                        }
                        else if (msg.ToString().Equals("80"))
                        {
                            string[] arr = { "3175", "3176", "3177", "3178" };
                            taskKeys = arr;
                        }
                        else if (msg.ToString().Equals("90"))
                        {
                            string[] arr = { "3179", "3180", "3181", "3182" };
                            taskKeys = arr;
                        }
                        else if (msg.ToString().Equals("100"))
                        {
                            string[] arr = { "3183", "3184", "3185", "3186" };
                            taskKeys = arr;
                        }
                        //移除任务
                        face.taskInterface.remTaskAndOverTaskFromCache(taskKeys);
                        //创建任务
                        for (int i = 0; i < 2; i++)
                        {
                            //设置为可接取
                            JObject progress = new JObject();
                            JObject target = new JObject();
                            target.Add("num", 0);
                            progress.Add("target", target);
                            face.taskInterface.savePlayerTask(taskKeys[i], 0, progress, 1);
                        }


                        return;
                    }
                //接收队长通知队员接取任务
                case "814":
                    {
                        face.taskInterface.createOneStartTask(msg.ToString(), 2);
                        //触发任务事件
                        face.taskInterface.triggerByProgressType(msg.ToString());
                        return;
                    }
                //接收队长通知队员更新任务
                case "815":
                    {
                        face.taskInterface.triggerByProgressType(msg.ToString());
                        return;
                    }
                //接收队长通知队员提交任务
                case "816":
                    {
                        face.taskInterface.submitTask(msg.ToString());
                        return;
                    }
                //刷新背包
                case "818":
                    {
                        Transform ac = PointGet.getAcPage<PackagePage>();
                        if (ac != null)
                        {
                            //刷新左侧装备\背包
                            ac.GetComponent<PackagePage>().updateWindow();

                        }

                        return;
                    }
                case "820":
                    {
                        //刷新人物面板

                        return;
                    }
                //同意入帮后通知
                case "822":
                    {
                        JObject o = (JObject)msg;
                        face.chatInterface.writeSysMsg("已加入帮派：" + o["name"]);
                        JObject r = face.roleInterface.getRole();
                        JObject bp = new JObject();
                        bp.Add("name", o["name"]);
                        bp.Add("Id", o["Id"]);
                        r["attr"]["msg"]["bp"] = bp;
                        face.roleInterface.saveRole(r);
                        return;
                    }
                //提示创建成功
                case "823":
                    {
                        Debug.Log("ws/该通知已经被废弃");
                        /*JObject o = (JObject)msg;
                        face.chatInterface.writeSysMsg("创建帮派 [" + o["name"].ToString() + "] 成功！");
                        JObject r = face.roleInterface.getRole();
                        r["attr"]["msg"]["tale"] = (int)r["attr"]["msg"]["tale"] - 20000;
                        JObject bp = new JObject();
                        bp.Add("Id", o["Id"].ToString());
                        bp.Add("name", o["name"].ToString());
                        r["attr"]["msg"]["bp"] = bp;
                        face.roleInterface.saveRole(r);*/
                        return;
                    }
                //通知领悟宠物技能
                case "824":
                    {
                        JArray list = (JArray)msg;
                        //刷新缓存
                        JObject pet = face.petInterface.getIsFightPet();
                        face.petInterface.addSkillToPet(pet, list);
                        return;
                    }
                //通知创建任务
                case "825":
                    {
                        JObject m = (JObject)msg;

                        string title = "";
                        if (m["acKey"].ToString().Equals("ZManswer"))
                        {
                            title = "周六答题";
                        }
                        else if (m["acKey"].ToString().Equals("ztzs"))
                        {
                            title = "门派闯关";
                        }
                        if ((int)m["remove"] == 1)
                        {
                            title += "活动已结束";
                            face.taskInterface.remTaskAndOverTaskFromCache((JArray)m["list"]);
                            //通知刷新任务ui
                            face.taskInterface.noticeUIUpdate(0);
                        }
                        else
                        {
                            title += "活动已开启";
                            //创建一个接取的起始任务
                            face.taskInterface.createOneStartTask(((JArray)m["list"])[0].ToString(), 1);
                        }
                        face.chatInterface.writeSysMsg(title);

                        return;
                    }
                //拍卖（出售道具成功）
                case "826":
                    {
                        JObject o = (JObject)msg;
                        if ((int)o["type"] == 0)
                        {//出售道具
                            JObject goods = face.goodsInterface.getPlayerGoodsById(o["Id"].ToString());
                            GoodsDes g = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(goods["key"].ToString());
                            string str = "您出售的商品 [" + g.name + "] x" + o["num"] + "被玩家购买，获得" +
                                ((int)o["moneyType"] == 0 ? "元宝" : "银两") + o["money"];
                            face.chatInterface.writeSysMsg(str);
                            //缓存银两、元宝、道具
                            face.goodsInterface.cutPlayerGoodsNum(o["Id"].ToString(), (int)o["num"]);
                        }
                        else if ((int)o["type"] == 1)
                        {//出售宠物
                         //移除宠物
                            face.petInterface.remFromCache(o["Id"].ToString());
                            face.chatInterface.writeSysMsg("您的宠物已被玩家购买，获得" +
                                ((int)o["moneyType"] == 0 ? "元宝" : "银两") + o["money"]);
                        }
                        else if ((int)o["type"] == 2)
                        {//邮件获利
                            face.chatInterface.writeSysMsg("对方接取了您的邮件，获得" +
                                ((int)o["moneyType"] == 0 ? "元宝" : "银两") + o["money"]);
                        }
                        face.roleInterface.updateMoney((int)o["money"], (int)o["moneyType"]);
                        return;
                    }
                //偷袭被夺走物品
                case "829":
                    {
                        JObject o = (JObject)msg;
                        string gdName = null;
                        string str = "与玩家 [" + o["name"] + "] 偷袭战斗中不幸失去物品 ";
                        if (o.ContainsKey("Id"))//夺走的是背包物品
                        {
                            JObject a = face.goodsInterface.getPlayerGoodsById(o["Id"].ToString());
                            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(a["key"].ToString());
                            gdName = gd.name;
                            face.goodsInterface.cutPlayerGoodsNum(o["Id"].ToString(), 1);
                        }
                        else if (o.ContainsKey("part"))//夺走的是装备
                        {
                            JObject role = face.roleInterface.getRole();
                            GoodsDes gd = (GoodsDes)face.goodsInterface.getGoodsMsgByKey(role["attr"]["equip"][o["part"].ToString()]["key"].ToString());
                            gdName = gd.name;
                            role["attr"]["equip"][o["part"].ToString()] = "";
                            face.roleInterface.saveRole(role);
                        }
                        str += "[" + gdName + "] x1 ";
                        if (o.ContainsKey("tale"))//夺走银两
                        {
                            face.roleInterface.updateMoney(-(int)o["tale"], 1);
                            str += "银两" + o["tale"];
                        }
                        face.chatInterface.writeSysMsg(str);
                        return;
                    }
                //通知进入某个场景,如监狱
                case "830":
                    {
                        mapManager.getInstance().reloadBef(msg.ToString());
                        return;
                    }
                //收徒成功
                case "833":
                    {
                        JObject r = face.roleInterface.getRole();
                        JObject o = (JObject)msg;
                        JObject m = new JObject();
                        m.Add("name", o["name"].ToString());
                        m.Add("qhd", 0);
                        if (o["type"].ToString().Equals("2"))
                        {
                            //收徒
                            r["attr"]["msg"][o["key"].ToString()] = m;
                            face.chatInterface.writeSysMsg("已收" + o["name"] + "为徒，有空可以拿小皮鞭抽抽他！");
                        }
                        else
                        {
                            //拜师
                            r["attr"]["msg"]["teacher"] = m;
                            face.chatInterface.writeSysMsg("已拜" + o["name"] + "为师，有空可以带他到野区放生！");
                        }

                        face.roleInterface.saveRole(r);
                        return;
                    }
                //解除师徒关系
                case "834":
                    {
                        JObject r = face.roleInterface.getRole();
                        JObject o = (JObject)msg;
                        if (o["type"].ToString().Equals("0"))
                        {//徒弟解除师傅
                            face.chatInterface.writeSysMsg("你的徒弟" + r["attr"]["msg"][o["index"].ToString()]["name"] + "背叛了你，并向你放了个屁！");
                            r["attr"]["msg"][o["index"].ToString()] = "";
                        }
                        else
                        {
                            face.chatInterface.writeSysMsg("你的师傅狠心将你遗弃了！");
                            r["attr"]["msg"]["teacher"] = "";
                        }
                        face.roleInterface.saveRole(r);
                        return;
                    }
                //出师
                case "835":
                    {
                        JObject r = face.roleInterface.getRole();
                        JObject o = (JObject)msg;
                        if (o["type"].ToString().Equals("0"))
                        {
                            face.chatInterface.writeSysMsg("你的徒弟" + r["attr"]["msg"][o["index"].ToString()]["name"] + "已出师！");
                            r["attr"]["msg"][o["index"].ToString()] = "";
                        }

                        face.roleInterface.saveRole(r);
                        return;
                    }
                //退出帮派通知
                case "836":
                    {
                        JObject r = face.roleInterface.getRole();
                        face.chatInterface.writeSysMsg("很遗憾，该帮派已不能再收容你，请8小时后再申请吧！");
                        r["attr"]["msg"]["bp"] = "";
                        face.roleInterface.saveRole(r);
                        //todo:刷新地图

                        return;
                    }
                //重新拉取任务
                case "838":
                    {
                        face.taskInterface.reGetAllTask();
                        return;
                    }

                //帮派活动开启通知
                case "840":
                    {
                        JObject o = (JObject)msg;
                        int type = int.Parse(o["type"].ToString());
                        if (type == 0)
                        {
                            face.chatInterface.writeSysMsg("帮派boss已开启", 4);
                            //生成bossnpc
                            JArray list = new JArray();
                            JObject a = new JObject();
                            a.Add("key", "2105");
                            a.Add("pos", strUtils.vectorToJSONObject(100f, 0f, 100f));
                            a.Add("scale", strUtils.vectorToJSONObject(1, 1, 1));
                            list.Add(a);

                            JArray clearList = new JArray();
                            clearList.Add("2105");
                            JObject m = new JObject();
                            m.Add("mapKey", "gangs");
                            m.Add("list", list);
                            m.Add("clearList", clearList);

                            eventsUtils.dispatchWsEvent("102", m);
                        }
                        else if (type == 1)
                        {
                            face.chatInterface.writeSysMsg("封赏演武堂已开启", 4);
                        }
                        else if (type == 2)
                        {
                            face.chatInterface.writeSysMsg("帮派跑商已开启", 4);
                        }
                        return;
                    }
                //通知怪物收集
                case "842":
                    {
                        JObject m = (JObject)msg;
                        JToken monKey = null;
                        m.TryGetValue("monKey", out monKey);
                        if (monKey != null)
                        {
                            face.taskInterface.commitMonster(monKey.ToString(), (int)m["num"]);
                            return;
                        }
                        JToken npcKey = null;
                        m.TryGetValue("npcKey", out npcKey);
                        if (npcKey != null)
                        {
                            face.taskInterface.commitNpc(npcKey.ToString(), (int)m["num"]);
                            return;
                        }

                        return;
                    }
                //通知悬赏任务完成
                case "843":
                    {
                        JObject a = (JObject)msg;
                        face.roleInterface.updateMoney((int)a["money"], (int)a["moneyType"]);
                        //通知奖励
                        face.chatInterface.writeSysMsg("恭喜完成悬赏任务，获得" + a["money"] +
                            ((int)a["moneyType"] == 0 ? "元宝" : "银两"));
                        return;
                    }
                //通知悬赏退款
                case "844":
                    {
                        JObject a = (JObject)msg;
                        face.roleInterface.updateMoney((int)a["money"], (int)a["moneyType"]);
                        if (a["type"].ToString().Equals("0"))
                        {
                            face.chatInterface.writeSysMsg("悬赏超时无人接取，退还赏金" + a["money"] +
                            ((int)a["moneyType"] == 0 ? "元宝" : "银两"));
                        }
                        else
                        {
                            face.chatInterface.writeSysMsg("玩家超时未完成您的悬赏，退还赏金+违约金" + a["money"] +
                            ((int)a["moneyType"] == 0 ? "元宝" : "银两"));
                        }

                        return;
                    }
                //消息通知
                case "845":
                    {
                        JObject m = (JObject)msg;
                        //Debug.Log(m);
                        if ((int)m["type"] == 4)
                        {
                            //私信需要写入indexDB   obj.msg{ type, params, Id, created }
                            m["params"]["obj"]["Id"] = strUtils.getId();
                            face.chatInterface.receiveMsg(m["params"]["obj"]);
                            face.msgInterface.receive(m);

                            /*GameObject cp = pageUI.getAcPage("chatPage");
                            if (cp != null)
                            {
                                cp.GetComponent<chatPage>().updateChatWindow(1);
                            }*/
                            PointGet.getIndexPage().GetComponent<IndexPage>().shan("chatBtn");
                        }
                        else if ((int)m["type"] == 3)//通知有人申请做你的徒弟
                        {
                            //face.msgInterface.receive(m);
                            //直接弹出谁申请
                            MsgSureUI.create(PointGet.getTipCanvas()).show("玩家【" + m["params"]["name"] + "】想要成为你的师傅，是否同意？", () =>
                            {
                                face.activityInterface.agreeBecomeStu(m["params"]["name"].ToString(), () =>
                                {

                                });
                            }, () => { });
                        }
                        else if ((int)m["type"] == 2)//通知有人申请做你的徒弟
                        {
                            //face.msgInterface.receive(m);
                            //直接弹出谁申请
                            MsgSureUI.create(PointGet.getTipCanvas()).show("玩家【" + m["params"]["name"] + "】想要成为你的徒弟，是否同意？", () =>
                            {
                                face.activityInterface.agreeApprentice(m["params"]["name"].ToString(), () =>
                                 {

                                 });
                            }, () => { });
                        }
                        else if ((int)m["type"] == 1)//1入队申请 
                        {
                            //提示有新申请
                            PointGet.getIndexPage().GetComponent<IndexPage>().shan("teamBtn");
                            face.msgInterface.receive(m);
                            face.chatInterface.writeSysMsg("收到来自玩家 [" + m["params"]["name"] + "] 的入队申请");
                        }
                        else if ((int)m["type"] == 0)//0邀请组队
                        {
                            PointGet.getIndexPage().GetComponent<IndexPage>().shan("teamBtn");
                            face.msgInterface.receive(m);
                            face.chatInterface.writeSysMsg("收到来自玩家 [" + m["params"]["name"] + "] 的组队邀请");
                        }

                        return;
                    }
                //通知更新hp、mp，这里返回的是最终的血量
                case "848":
                    {
                        JObject m = (JObject)msg;

                        JObject r = face.roleInterface.getRole();
                        if (m.ContainsKey("mxue"))
                        {
                            int mxue = (int)m["mxue"];
                            r["attr"]["prop"]["xue"] = mxue;
                        }
                        if (m.ContainsKey("mlan"))
                        {
                            Debug.Log(m["mlan"]);
                            int mlan = (int)m["mlan"];
                            r["attr"]["prop"]["lan"] = mlan;
                        }

                        JObject pet = face.petInterface.getIsFightPet();
                        if (pet != null)
                        {
                            JObject a = new JObject();
                            if (m.ContainsKey("pxue"))
                            {
                                int pxue = (int)m["pxue"];
                                a["xue"] = pxue;
                            }
                            if (m.ContainsKey("plan"))
                            {
                                int plan = (int)m["plan"];
                                a["lan"] = plan;
                            }
                            face.petInterface.updatePetXueById(a, pet["Id"].ToString());
                        }
                        if (m.ContainsKey("capacity"))
                        {
                            int capacity = (int)m["capacity"];
                            r["attr"]["equip"]["bjb"]["num"] = capacity;
                        }


                        face.roleInterface.saveRole(r);
                        //刷新游戏界面的血条
                        /*IndexPage pg = PointGet.getIndexPage().GetComponent<IndexPage>();
                        pg.updateRoleXueTiao();
                        pg.updatePetXueTiao();*/
                        return;
                    }
                //回天书结果处理（定时器）
                case "849":
                    {
                        JObject a = (JObject)msg;
                        int roleType = (int)a["roleType"];
                        string name = a["name"].ToString();
                        int index = (int)a["index"];
                        string key = a["key"].ToString();
                        string xhKey = null;
                        if (roleType == 0) xhKey = "10000128";
                        else xhKey = "10000120";
                        JObject g = face.goodsInterface.getPlayerGoodsByKey(xhKey);
                        if (g == null || (int)g["num"] - 1 < 1)
                        {
                            if (roleType == 0)
                            {
                                JObject role = face.roleInterface.getRole();
                                JArray list = (JArray)role["attr"]["skill"];
                                JObject item = face.skillInterface.getSkl(3, index, list);
                                item["key"] = key;
                                item["lv"] = 1;
                                face.roleInterface.saveRole(role);
                            }
                            else
                            {
                                //处理宠物的
                                string petId = a["petId"].ToString();
                                JObject pet = face.petInterface.getOneById(petId);
                                JArray skls = (JArray)pet["attr"]["skill"];
                                JObject skl = (JObject)skls[index];
                                skl["key"] = key;
                                skl["lv"] = 1;
                                face.petInterface.savePet(pet);
                            }
                            face.chatInterface.writeSysMsg("因回天书使用超时未确认，背包中无相关取消道具，故系统已为其覆盖");
                        }
                        else
                        {
                            //取消覆盖
                            face.goodsInterface.cutPlayerGoodsNumByKey(xhKey, 1);
                            face.chatInterface.writeSysMsg("因回天书使用超时未确认，系统已经取消覆盖");
                        }


                        return;
                    }
                //删除邮件指定id的缓存
                case "850":
                    {
                        face.emailInterface.delEmailFromCache(msg.ToString());
                        //更新邮件列表ui
                        Transform cp = PointGet.getAcPage<EmailPage>();
                        if (cp != null)
                        {
                            cp.GetComponent<EmailPage>().updateWindow();

                        }
                        return;
                    }
                //聚灵真火
                case "851":
                    {
                        JObject a = (JObject)msg;
                        face.rewardInterface.saveRewards((JArray)a["list"]);
                        face.chatInterface.writeSysMsg("聚灵真火剩余时间：" + strUtils.nowTimeToEndTime(strUtils.getMillis(), (long)a["end"]));
                        return;
                    }
                //洪荒宝库创建任务
                case "852":
                    {
                        face.taskInterface.remTaskAndOverTaskFromCache(new string[1] { "3274" });
                        face.taskInterface.createOneStartTask("3274", 2);
                        face.goodsInterface.cutPlayerGoodsNumByKey("10000194", 1);
                        activityGet.toMap("hhbk_1");
                        return;
                    }
                //诱敌香草遇怪
                case "853":
                    {
                        MonsterCreateHandle.getInstance().createByydxc();
                        return;
                    }
                //通知需要关闭哪些状态（这个时到期的才会关闭）
                case "854":
                    {
                        face.roleInterface.clearOverTimeStatus((JArray)msg);
                        return;
                    }
                //增加情义值
                case "855":
                    {
                        JObject a = (JObject)msg;
                        JObject r = face.roleInterface.getRole();
                        JObject t = (JObject)r["attr"]["msg"][a["k"].ToString()];
                        t["qhd"] = (int)a["qhd"];
                        face.roleInterface.saveRole(r);
                        return;
                    }
                //修改魔神等级
                case "856":
                    {
                        JObject r = face.roleInterface.getRole();
                        r["attr"]["msLv"]["lv" + msg] = (int)r["attr"]["msLv"]["lv" + msg] + 1;
                        face.roleInterface.saveRole(r);
                        return;
                    }
                //刷新状态时间
                case "857":
                    {
                        face.roleInterface.updateStatus((JObject)msg);
                        return;
                    }
                //血腥积分
                case "858":
                    {
                        int n = int.Parse(msg.ToString());
                        if (n > 0)
                        {
                            face.chatInterface.writeSysMsg("获得血腥徽章x" + msg);
                        }
                        else if (n > 0)
                        {
                            face.chatInterface.writeSysMsg("失去血腥徽章x" + msg);
                        }
                        return;
                    }

                //进入战斗
                case "900":
                    {
                        //通知战斗信息已经创建完毕，可以拉取信息了
                        face.fightInterface.getFightMsg(msg.ToString());
                        return;
                    }
                //角色菜单显示
                /*case "3000":
                    {
                        partsMenu.getInstance().showMenu(true);
                        return;
                    }*/
                //宠物菜单显示
                /*case "3001":
                    {
                        partsMenu.getInstance().showMenu(false);
                        return;
                    }*/
                //自动战斗处理
                /*case "3003":
                    {

                        return;
                    }*/
                //通知显示自动
                case "3006":
                    {
                        partsMenu.getInstance().setAutoIcon();
                        return;
                    }
                //通知准备阶段结束，监听服务端控制回合的开启。每个回合开始时调用(必须只能给服务器调这里，客户端不允许调这里)
                case "3009":
                    {
                        //msg {time,huihe,coolList}
                        JObject a = (JObject)msg;
                        fightCache.getInstance().setWsMsg3009(a);
                        return;
                    }
                //接收战斗执行步骤，播放动画
                case "3010":
                    {
                        //菜单、回合等组件并播放开始动画
                        partsMenu.getInstance().showDjs(false);
                        if (!fightCache.getInstance().isViewFight)
                        {
                            partsMenu.getInstance().showSkillCard(false);
                            partsMenu.getInstance().visiblebottomBtn(false);
                        }
                        playerOperate.getInstance().visibleNames(false);
                        //执行战斗动画逻辑
                        attackAm.getInstance().putActionList((JArray)msg);
                        return;
                    }
                //反馈成功/失败接收到指令
                case "3011":
                    {
                        JObject a = (JObject)msg;
                        if ((int)a["status"] == 1)
                        {
                            //成功，关闭沙漏
                            //成功，关闭沙漏
                            string posKey = a["posKey"].ToString();
                            //将站位的沙漏关闭
                            partsMenu.getInstance().clearShalou(posKey);
                            string petPosKey = posKey.Substring(0, 1) + (int.Parse(posKey.Substring(1)) + 5);
                            partsMenu.getInstance().clearShalou(petPosKey);
                        }
                        else
                        {
                            //失败，打开沙漏，并将菜单重新打开
                        }
                        return;
                    }

                //奖励通知
                case "10000":
                    {
                        face.rewardInterface.saveRewards((JArray)msg);
                        return;
                    }

                default:
                    {
                        Debug.Log("未找到ws " + callbackCode);
                        return;
                    }
            }
        }
    }
}
