using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.part
{
    class TaskTalkUI : PartUI
    {
        //选中的npcid
        private int npcId;
        //当前对话索引
        private int talkIndex;
        private string taskKey;
        public static TaskTalkUI create(int npcId, Transform parent)
        {
            Vector2 size = new Vector2(800, 800);
            GameObject one = gameObjPool.getInstance().get("TaskTalkUI", typeof(TaskTalkUI));
            TaskTalkUI bs = one.GetComponent<TaskTalkUI>();
            bs.setSize(new Vector2(ScreenUtils.width, ScreenUtils.height)).putSence<TaskTalkUI>(parent).setScreenCenter();
            bs.draw(npcId, size);

            DoGet.getInstance().startReqImg();
            return bs;
        }

        private TaskTalkUI draw(int npcId, Vector2 size)
        {
            GameObject mask = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            mask.transform.SetParent(this.transform, false);
            mask.GetComponent<ImgUI>().setColor(0, 0, 0, 0f)
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0));

            GameObject kuang = gameObjPool.getInstance().get("kuang", typeof(SimpleUI));
            kuang.transform.SetParent(this.transform, false);
            kuang.GetComponent<SimpleUI>()
                .setSizePos(size, new Vector2((ScreenUtils.width - size.x) / 2, -(ScreenUtils.height - size.y) / 2));

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

            //内容区
            GameObject content = gameObjPool.getInstance().get("content", typeof(SimpleUI));
            content.transform.SetParent(bg.transform, false);
            content.GetComponent<SimpleUI>()
            .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));

            GameObject testText = gameObjPool.getInstance().get("talk", typeof(TextUI));
            testText.transform.SetParent(content.transform, false);
            testText.GetComponent<TextUI>().setColor().setAlign("leftTop").setFontSize(30)
                .setText("").setIsRichText()
                .setSizePos(new Vector2(size.x - 40, size.y - 40), new Vector2(0, 0));

            GameObject btn = gameObjPool.getInstance().get("cancel", typeof(BtnUI));
            btn.transform.SetParent(bg.transform, false);
            btn.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(0, -700));
            btn.GetComponent<BtnUI>().addText("取消", 30).setTextColor().loadRes("gy_03_png").addClk(() =>
                 {
                     this.free();
                 });
            GameObject btn1 = gameObjPool.getInstance().get("sure", typeof(BtnUI));
            btn1.transform.SetParent(bg.transform, false);
            btn1.GetComponent<BtnUI>()
                .setSizePos(new Vector2(120, 60), new Vector2(640, -700));
            btn1.GetComponent<BtnUI>().addText("确定", 30).setTextColor().loadRes("gy_02_png").addClk(() =>
            {
                if (this.taskKey == null)
                {
                    this.free();
                    return;
                }
                //分为人物对话（任务对话不需要有继续按钮）、任务对话
                //显示下一条
                //最后一条时弹出任务详情并关闭这个弹窗
                this.talkIndex++;
                this.nextDes();

            });
            return this;
        }
        /**显示下一条描述*/
        private void nextDes()
        {
            face.taskDesInterface.getTaskDes(this.taskKey,(talks)=> {
                if (talks == null)
                {
                    Debug.Log("talks=null/原因：未有对话");
                }
                //获取缓存的任务
                JObject task = face.taskInterface.getTaskFromCache(this.taskKey);
                if (task == null)
                {
                    Debug.Log("task=null/原因：未登录/已提交");
                    this.free();
                    return;
                }

                //根据进度来提取对话
                int status = int.Parse(task.GetValue("status") + "");
                int progressIndex = int.Parse(task.GetValue("progressIndex") + "");
                JArray desList = null;
                if (status == 1)
                {
                    //未接取状态
                    desList = (JArray)talks.GetValue("get");
                }
                else if (status == 2)
                {
                    //进行中
                    desList = (JArray)talks.GetValue("p" + progressIndex);
                }
                else if (status == 3)
                {
                    //完成
                    desList = (JArray)talks.GetValue("over");
                }
                //Debug.Log(this.talkIndex + "=>" + desList.Count);
                if (this.talkIndex >= desList.Count)
                {
                    if (this.talkIndex > desList.Count)
                    {
                        //关闭对话框
                        this.free();
                        return;
                    }
                    if (status == 1)
                    {
                        //获取任务基本信息
                        task ts = face.taskInterface.getTask(this.taskKey);
                        JObject target = ts.getTarget();
                        TaskGetUI tg = TaskGetUI.create();
                        tg.setTitle(ts.getProgressTitle());
                        tg.appendContent(ts.getProgressDes());
                        tg.setTarget(ts.getTargetDes());
                        face.rewardInterface.findTaskRewards(ts.key, (res) =>
                        {
                            //将奖励格式转道具格式
                            JArray rs = face.rewardInterface.goodsToRewardFormat(res);
                            tg.setRewards(rs);
                            DoGet.getInstance().startReqImg();
                        });

                        //显示接取按钮
                        tg.showGainBtn(() =>
                        {
                            face.taskInterface.gainTask(this.taskKey, () =>
                            {
                                //聊天窗提示已接取，并将提示栏关闭
                                //tg.free();
                            });
                        });
                        DoGet.getInstance().startReqImg();

                    }
                    else if (status == 2)
                    {
                        //触发对进度类型的处理
                        face.taskInterface.trigger(this.taskKey);
                    }
                    else if (status == 3)
                    {

                        //请求提交
                        face.taskInterface.submitTask(this.taskKey);
                    }

                    //关闭对话框
                    this.free();
                    return;
                }
                JObject tk = (JObject)desList[this.talkIndex];
                int tp = (int)tk["type"];

                this.transform.Find("kuang/bg/content/talk").GetComponent<TextUI>()
                    .setText(strToTalk(tk["talk"].ToString()));
            });
            

        }
        /**对对话进一步解析*/
        private string strToTalk(string str)
        {
            if (str.Contains("<player_name>"))
            {
                JObject r = face.roleInterface.getRole();
                str = str.Replace("<player_name>", r["name"].ToString());
            }
            return str;
        }
        private static void clkItem(JArray list, int i, int npcId)
        {
            JObject obj = (JObject)list[i];
            //回调菜单项对应的方法，任务、活动
            int type = (int)obj.GetValue("type");
            if (type == 0)
            {
                //显示对话框
                TaskTalkUI tui = TaskTalkUI.create(npcId, PointGet.getIndexPage());
                tui.taskKey = (string)obj.GetValue("key");
                tui.talkIndex = 0;
                tui.nextDes();
            }
            else if (type == 2)
            {
                /*task ts = face.taskInterface.getTask(obj["key"].ToString());
                if (ts.toMapMsg != null)
                {
                    //直接跳转地图
                    string mapKey = ts.toMapMsg["mapKey"].ToString();
                    mapManager.getInstance().reloadBef(mapKey);
                }
                else
                {
                    TaskTalkUI tui = TaskTalkUI.create(npcId, PointGet.getIndexPage());
                    tui.showTaskAc(ts);
                }*/
                modelMsgBind npc = npcManager.getNpcById(npcId).GetComponent<modelMsgBind>();
                //判断是否还有子级
                List<MultyMenu> ms = face.activityInterface.getAcMenus(obj["key"].ToString(), npc.msg.key,npc);
                int index = (int)obj["index"];
                showMultyMenu(ms, index);
            }
        }
        private static void showMultyMenu(List<MultyMenu> ms, int index)
        {
            if (ms == null || ms.Count == 0) return;
            MultyMenu m = ms[index];
            if (m.menus != null && m.menus.Count > 0)
            {
                List<string> arr = new List<string>();
                for (int p = 0; p < m.menus.Count; p++)
                {
                    arr.Add(m.menus[p].name);
                }
                float h = arr.Count * 110 + 30;
                if (h > (4 * 110 + 30)) h = 4 * 110 + 30;
                Menu menu = Menu.create(arr, PointGet.getIndexPage());
                menu.addCallback((index0) =>
                {
                    showMultyMenu(m.menus, index0);
                });
                DoGet.getInstance().startReqImg();
            }
            else
            {
                //调取绑定的方法
                m.type.GetMethod(m.fnName).Invoke(m.type, m.ps);
            }
        }
        /**显示由菜单进入的活动，活动页进入的不在这里*/
        private void showTaskAc(task ts)
        {

        }
        /**弹出任务、活动等*/
        public static void showNpcMenu(modelMsgBind mb)
        {
            JArray list = face.npcInterface.getNpcTaskAndProgressList(mb.msg.key);
            if (list == null || list.Count == 0)
            {
                Dialog dialog = Dialog.create(new Vector2(800, 400), PointGet.getIndexPage());
                dialog.renderText("我没有什么可以跟你说的！");
                DoGet.getInstance().startReqImg();
                return;
            }

            List<string> arr = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                JObject l = (JObject)list[i];
                arr.Add(l["name"].ToString());
            }
            //存在锄奸卫道就要增加一个选项
            JObject cjTask = face.taskInterface.getTaskFromCache("3272");
            if (cjTask != null && (int)cjTask["status"] == 2)
            {
                arr.Add("我猜你是奸细");
            }
            float h = arr.Count * 110 + 30;
            if (h > (4 * 110 + 30)) h = 4 * 110 + 30;
            Menu menu = Menu.create(arr, PointGet.getIndexPage());
            menu.addCallback((index) =>
            {
                if (arr[index].Equals("我猜你是奸细"))
                {
                    face.activityInterface.isJianxi(mb.msg.key, () => { });
                    return;
                }
                clkItem(list, index, mb.gameObject.GetInstanceID());
            });
            DoGet.getInstance().startReqImg();
        }



    }
}
