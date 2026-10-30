using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface taskInterface
    {
        public void createOneStartTask(string taskKey, int status);
        public void noticeUIUpdate(int type);
        public void checkTaskStart(Action ac);
        public void submitTask(string key);
        public void gainTask(string key, Action ac);
        //public void getTaskTargetStr(string key, int progressSum,Action<string> ac);
        public task getTask(string key);
        public JObject getTaskFromCache(string key);
        public JArray getTaskListFromCache();
        public void initTaskList(JArray taskList, JArray overTask, JArray taskIndex);
        public string[] getLimitTimeAcList();
        public string[] getNoLimitTimeAcList();

        public void commitMonster(string monsterKey, int num);
        public JArray filterShowTask();

        public JArray getCacheTaskListByStatus(int status);
        public JArray getCacheTaskListByStatus(int status1, int status2);
        public List<task> getStartActivity();
        public void trigger(string key);

        public void collectionCjw(string npcKey);
        public void commitNpc(string npcKey, int num);
        public JArray getStartTaskList(int type);
        public bool isExistTaskFromCacheTaskListAndCommit(string key);
        public bool isExistTaskFromCacheTaskList(string key);
        public void remTaskAndOverTaskFromCache(string[] keys);
        public void remTaskAndOverTaskFromCache(JArray keys);
        public void savePlayerTask(string taskKey, int progressIndex, JObject taskProgress, int status);
        public bool isExistCommitTask(string key);
        public void getHDTask(string hdKey, Action<object> callback);
        public string[] getElseAcList();
        public void triggerByProgressType(string key);
        public void getElseAcMsg(Action<JArray> callback);
        public bool isDoingTask(string key);
        public bool isDoingTaskAndProgressSame(string key, int pIndex);
        public void reGetAllTask();
    }
    public class taskInterfaceImpl : taskInterface
    {
        /**重新拉取任务*/
        public void reGetAllTask()
        {
            DoGet.getInstance().sendPost("/taskService/reGetAllTask", null, (res) =>
            {
                JObject a = (JObject)res;
                this.initTaskList((JArray)a["playerTask"],
                    (JArray)a["overTask"], (JArray)a["taskIndex"]);
                this.noticeUIUpdate(0);
            });
        }
        /**获取其他活动列表信息*/
        public void getElseAcMsg(Action<JArray> callback)
        {
            DoGet.getInstance().sendPost("/taskService/getElseAcMsg", null, (res) =>
            {
                callback((JArray)res);
            });
        }

        /**接取特殊的活动任务*/
        public void getHDTask(string hdKey, Action<object> callback)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", hdKey);
            DoGet.getInstance().sendPost("/taskService/getHDTask", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    msgCode.showMsg(696);
                }
                else if (res.ToString().Equals("-1"))
                {
                    msgCode.showMsg(613);
                }
                else if (res.ToString().Equals("-2"))
                {
                    msgCode.showMsg(853);
                }
                else if (res.ToString().Equals("-3"))
                {
                    msgCode.showMsg(610);
                }
                else
                {
                    callback(res);
                }

            });
        }

        /**获取所有已开启的任务(只返回name、key)
         type: 0主线1副本2活动3支线
         */
        public JArray getStartTaskList(int type)
        {
            JArray arr = new JArray();
            JArray playerTask = getTaskListFromCache();
            for (int p = 0; p < playerTask.Count; p++)
            {
                JObject a = (JObject)playerTask[p];
                if (type == 0 && (int.Parse(a["key"].ToString()) < 1000 || int.Parse(a["key"].ToString()) >= 2000)) continue;
                else if (type == 1 &&
                    (int.Parse(a["key"].ToString()) < 3163 || int.Parse(a["key"].ToString()) >= 3242)) continue;
                else if (type == 2 &&
                    (int.Parse(a["key"].ToString()) < 3000 ||
                    (int.Parse(a["key"].ToString()) >= 3163 && int.Parse(a["key"].ToString()) < 3242))) continue;
                else if (type == 3 && (int.Parse(a["key"].ToString()) < 2000 || int.Parse(a["key"].ToString()) >= 3000)) continue;

                task t = getTask(a["key"].ToString());
                //要求接取
                if (t == null)
                {
                    continue;
                }

                int index = (int)a["progressIndex"];
                JObject progress = (JObject)t.progressList[index];
                JObject o = new JObject();
                o.Add("name", progress["name"].ToString());
                o.Add("key", t.key);
                o.Add("type", type);
                arr.Add(o);
            }
            return arr;
        }
        /**帅选出可显示在任务栏的任务*/
        public JArray filterShowTask()
        {
            JArray arr = new JArray();
            JArray playerTask = getTaskListFromCache();
            for (int p = 0; p < playerTask.Count; p++)
            {
                //要求接取
                if ((int)playerTask[p]["status"] < 2)
                {
                    continue;
                }
                arr.Add(playerTask[p]);
            }
            return arr;
        }
        /**收集采集物*/
        public void commitNpc(string npcKey, int num)
        {
            JArray playerTask = getTaskListFromCache();
            for (int p = 0; p < playerTask.Count; p++)
            {
                //只有正在进行中的任务才允许提交数量
                if ((int)playerTask[p]["status"] != 2)
                {
                    continue;
                }
                task task = getTask(playerTask[p]["key"].ToString());
                if (task == null) continue;
                int index = (int)playerTask[p]["progressIndex"];
                JObject progress = (JObject)task.progressList[index];
                if ((int)progress["type"] != 1)
                {
                    continue;
                }
                if ((int)progress["target"]["targetType"] == 0 &&
                    progress["target"]["key"].ToString().Equals(npcKey))
                {
                    int collectionNum = (int)playerTask[p]["taskProgress"]["target"]["num"] + num;
                    if ((int)progress["target"]["sum"] > collectionNum)
                    {
                        playerTask[p]["taskProgress"]["target"]["num"] = collectionNum;
                    }
                    else
                    {//收集完毕
                        if (index + 1 >= task.progressList.Count)
                        {//达到最大进度，状态变为待提交
                            playerTask[p]["taskProgress"]["target"]["num"] = (int)progress["target"]["sum"];
                            playerTask[p]["status"] = 3;
                        }
                        else
                        {//未达到最大进度，进度+1
                            playerTask[p]["taskProgress"]["target"]["num"] = 0;
                            playerTask[p]["progressIndex"] = index + 1;
                        }
                    }
                    //通知更新走马灯
                    JObject msg = new JObject();
                    msg.Add("taskKey", task.key);
                    msg.Add("num", collectionNum);
                    msg.Add("sum", (int)progress["target"]["sum"]);
                    msg.Add("targetKey", npcKey);
                    msg.Add("targetType", (int)progress["target"]["targetType"]);

                    JObject j = new JObject();
                    j.Add("callback", "811");
                    j.Add("msg", msg);
                    eventsUtils.dispatchEvent("ws", j);
                }
            }
            dbHandle.save("playerTask", playerTask);
            this.noticeUIUpdate(0);
        }
        /**提交怪物数量 */
        public void commitMonster(string monsterKey, int num)
        {
            JArray playerTask = getTaskListFromCache();
            for (int p = 0; p < playerTask.Count; p++)
            {
                //只有正在进行中的任务才允许提交数量
                if ((int)playerTask[p]["status"] != 2)
                {
                    continue;
                }
                task task = getTask(playerTask[p]["key"].ToString());
                if (task == null) continue;

                int index = (int)playerTask[p]["progressIndex"];
                JObject progress = (JObject)task.progressList[index];

                if ((int)progress["target"]["targetType"] == 1 &&
                    progress["target"]["key"].ToString().Equals(monsterKey))
                {
                    int collectionNum = (int)playerTask[p]["taskProgress"]["target"]["num"] + num;
                    if ((int)progress["target"]["sum"] > collectionNum)
                    {
                        playerTask[p]["taskProgress"]["target"]["num"] = collectionNum;
                    }
                    else
                    {
                        if (index + 1 >= task.progressList.Count)
                        {
                            playerTask[p]["taskProgress"]["target"]["num"] = (int)progress["target"]["sum"];
                            playerTask[p]["status"] = 3;
                        }
                        else
                        {
                            playerTask[p]["taskProgress"]["target"]["num"] = 0;
                            playerTask[p]["progressIndex"] = index + 1;
                        }
                    }
                    //通知更新走马灯
                    JObject msg = new JObject();
                    msg.Add("taskKey", task.key);
                    msg.Add("num", collectionNum);
                    msg.Add("sum", (int)progress["target"]["sum"]);
                    msg.Add("targetKey", monsterKey);
                    msg.Add("targetType", (int)progress["target"]["targetType"]);

                    JObject j = new JObject();
                    j.Add("callback", "811");
                    j.Add("msg", msg);
                    eventsUtils.dispatchEvent("ws", j);
                }
            }
            dbHandle.save("playerTask", playerTask);
            this.noticeUIUpdate(0);
        }
        public void checkTaskStart(Action ac)
        {
            JArray doList = getTaskListFromCache();
            JArray overList = getOverListFromCache();
            //1000-5000中任务数据实际存在的key
            JArray taskList = getTaskIndexFromCache();
            JObject r = face.roleInterface.getRole();
            Action<JArray> fn = (newList) =>
            {
                timeManager.getTimeManageOne().putThread(() =>
                {
                    if (newList.Count > 0)
                    {
                        doList.Merge(newList);
                        dbHandle.save("playerTask", doList);

                        //Debug.Log(dbHandle.get<JArray>("playerTask"));
                    }
                    ac();
                });
            };
            checkTaskStart(doList, overList, taskList, r, (newList) =>
            {
                fn(newList);
            });
            
        }
        private void checkTaskStart(JArray doList, JArray overList, JArray taskList, JObject r,Action<JArray> ac )
        {
            JArray newList = new JArray();
            for (int p = 0; p < taskList.Count; p++)
            {
                string key = taskList[p].ToString();
                task taskMsg = getTask(key);
                if (taskMsg == null)
                {
                    continue;
                }
                bool b = false;
                foreach (object o in doList)
                {
                    if (((JObject)o)["key"].ToString().Equals(key))
                    {
                        b = true;
                        break;
                    }
                }
                //已经在任务表中存在
                if (b) continue;
                foreach (object o in overList)
                {
                    if (o.ToString().Equals(key))
                    {
                        b = true;
                        break;
                    }
                }
                //已经在提交表中存在
                if (b) continue;


                //释放达到开启的条件
                JObject condition = taskMsg.condition;


                if ((int)condition["lever"] > (int)r["lever"])
                {
                    continue;
                }
                //todo:注意：condition["taskKey"]可能会变成""，所以要这么判断
                if (!strUtils.isNull(condition["taskKey"]))
                {
                    bool b1 = false;
                    foreach (object o in overList)
                    {
                        if (o.ToString().Equals(condition["taskKey"].ToString()))
                        {
                            b1 = true;
                            break;
                        }
                    }
                    if (!b1)
                    {
                        continue;
                    }
                }

                JObject item = new JObject();
                item.Add("progressIndex", 0);
                item.Add("key", key);
                item.Add("status", 1);
                JObject target = new JObject();
                target.Add("num", 0);
                JObject taskProgress = new JObject();
                taskProgress.Add("target", target);
                item.Add("taskProgress", taskProgress);


                newList.Add(item);

            }
            ac(newList);
        }

        /**提交任务*/
        public void submitTask(string key)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/taskService/submitTask", dic, (res) =>
            {
                if (res != null)
                {
                    JObject a = (JObject)res;
                    face.rewardInterface.saveRewards((JArray)a["reward"]);

                    this.remTaskFromCache(key);
                    //将其加入提交任务集
                    this.addTaskToOver(key);

                    //当副本还有次数时会进行一次相关任务的清理
                    if (a["isDel"] != null && (int)a["isDel"] == 1)
                    {
                        //需要将对应一系列的key给清除
                        List<string> keys = getSerialTask(key);
                        foreach (string k in keys)
                        {
                            this.remOverTaskFromCache(k);
                        }
                    }

                    //刷新任务（某个任务提交后可能并没有升级，但是却达到了下个任务的开启条件）
                    this.checkTaskStart(()=> {
                        //通知刷新ui
                        this.noticeUIUpdate(2);
                    });
                    
                }
            });
        }
        /**获取相关的副本key（由结尾任务确定）*/
        private List<string> getSerialTask(string key)
        {
            List<String> list = new List<string>();
            int start = 0; int end = 0;
            if (key.Equals("3168"))
            {
                start = 3163; end = 3169;
            }
            else if (key.Equals("3174"))
            {
                start = 3169; end = 3175;
            }
            else if (key.Equals("3183"))
            {
                start = 3175; end = 3184;
            }
            else if (key.Equals("3188"))
            {
                start = 3184; end = 3189;
            }
            else if (key.Equals("3195"))
            {
                start = 3189; end = 3196;
            }
            else if (key.Equals("3202"))
            {
                start = 3196; end = 3203;
            }
            else if (key.Equals("3212"))
            {
                start = 3203; end = 3213;
            }
            else if (key.Equals("3220"))
            {
                start = 3213; end = 3221;
            }
            else if (key.Equals("3228"))
            {
                start = 3221; end = 3229;
            }
            else if (key.Equals("3234"))
            {
                start = 3229; end = 3235;
            }
            else if (key.Equals("3241"))
            {
                start = 3235; end = 3242;
            }
            else if (int.Parse(key) >= 3275 && int.Parse(key) < 3282)//盗梦
            {
                start = int.Parse(key); end = int.Parse(key) + 1;
            }
            for (int i = start; i < end; i++)
            {
                list.Add(i + "");
            }
            return list;
        }
        /**接取任务*/
        public void gainTask(string key, Action ac)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/taskService/getTask", dic, (res) =>
            {
                if (int.Parse(res + "") == 1)
                {
                    JObject task = this.getTaskFromCache(key);
                    //变更为接取状态
                    task["status"] = 2;
                    this.updateTaskFromCache(task);
                    //fixme:接取后不触发进度++
                    //this.triggerByProgressType(task["key"].ToString());
                    //通知刷新ui
                    this.noticeUIUpdate(1);
                    ac();
                }
            });

        }

        /**采集物收集*/
        public void collectionCjw(string npcKey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", npcKey);
            DoGet.getInstance().sendPost("/taskService/commitCjw", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 1)
                {
                    //ws返回处理时会更新缓存，不需要在这里刷新
                }
            });
        }
        /**主动触发任务进度的更新（比如多个任务对话进度时可用）*/
        public void trigger(string key)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", key);
            DoGet.getInstance().sendPost("/taskService/trigger", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 1)
                {
                    this.triggerByProgressType(key);
                }
            });
        }
        /**
         * 触发对进度类型的判断 （进度开启时才会调用）
         * 何时调用：接取后触发，进度提交后也会触发
        * 根据进度的类型判断是否立即触发（战斗、显示隐藏的npc、直接完成任务等）
        */
        public void triggerByProgressType(string key)
        {
            JObject playerTask = this.getTaskFromCache(key);
            //对于已完成的任务是不会存在的,不处于进行中状态的任务不允许触发
            if (playerTask == null || (int)playerTask["status"] != 2) return;
            task task = getTask(key);
            if (task == null) return;
            int index = (int)playerTask["progressIndex"];
            JObject progress = (JObject)task.progressList[index];
            switch ((int)progress["type"])
            {
                case 0:
                    {//对话任务（对话完目标数量+1） 触发进度更新
                        this.updateTask(key, 1);
                        break;
                    }
                case 1:
                    {//摘取任务 触发隐藏npc的显示
                        //判断当前地图是否跟任务的npc所在地图一致
                        task ts = getTask(key);
                        string a = ts.targetPosList[index]["mapKey"].ToString();
                        string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                        if (a.Equals(mapKey))
                        {
                            string npcKey = ts.targetPosList[index]["npcKey"].ToString();
                            npcManager.createCjw(npcKey);
                        }
                        break;
                    }
                case 2:
                    {//战斗任务 触发战斗

                        break;
                    }
                case 3:
                    {//接取即完成，无论后续有什么步骤，设计时只会存在一个进度，总数量也是1
                        this.updateTask(key, 1);
                        break;
                    }
                case 4:
                    {//啥也不做，用于接取后目的是在周围收集一定数量的怪物
                        break;
                    }
            }
        }
        /**更新任务 */
        public void updateTask(string key, int num)
        {
            JObject playerTask = this.getTaskFromCache(key);
            task task = getTask(key);
            if (task == null) return;
            int index = (int)playerTask["progressIndex"];
            JObject progress = (JObject)task.progressList[index];
            playerTask["taskProgress"]["target"]["num"] = (int)playerTask["taskProgress"]["target"]["num"] + num;
            if ((int)playerTask["taskProgress"]["target"]["num"] >= (int)progress["target"]["sum"])
            {//收集足够数量
                if ((int)playerTask["progressIndex"] + 1 >= task.progressList.Count)
                {
                    this.savePlayerTask(key, (int)playerTask["progressIndex"], (JObject)playerTask["taskProgress"], 3);
                }
                else
                {
                    //进度已完成
                    playerTask["progressIndex"] = index + 1;
                    //playerTask["taskProgress"]["target"]["num"] = 0;
                    this.savePlayerTask(key, (int)playerTask["progressIndex"], (JObject)playerTask["taskProgress"], 2);
                }
            }
            else
            {
                //未收集到足够数量
                this.savePlayerTask(key, (int)playerTask["progressIndex"], (JObject)playerTask["taskProgress"], 2);
            }

        }
        /**创建一个起始任务*/
        public void createOneStartTask(string taskKey, int status)
        {
            JObject progress = new JObject();
            JObject target = new JObject();
            target.Add("num", 0);
            progress.Add("target", target);
            savePlayerTask(taskKey, 0, progress, status);
        }
        /**
                 * 保存任务（可接取型的任务，不包括大盘等特殊活动）
                 * 参数：所有
                 * {name,key，progressIndex，status,taskProgress}
                 * 服务器保存的是json对象数组，将key相同的任务覆盖即可
                 */
        public void savePlayerTask(string taskKey, int progressIndex, JObject taskProgress, int status)
        {
            this.addTaskFromCache(taskKey, status, progressIndex, taskProgress);
            this.noticeUIUpdate(0);
        }
        /**通知相关ui更新 
         type 0不播放动画 1接取动画 2提交动画
         */
        public void noticeUIUpdate(int type)
        {
            //任务状态改变，通知ui更新
            eventsUtils.dispatchWsEvent("801", type);
        }
        /**将提交的任务放入提交缓存*/
        private void addTaskToOver(string key)
        {
            JArray list = this.getOverListFromCache();
            list.Add(key);
            dbHandle.save("overTask", list);
        }
        /**更新缓存*/
        private void updateTaskFromCache(JObject task)
        {
            JArray list = this.getTaskListFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject l = (JObject)list[i];
                if ((l["key"] + "").Equals(task["key"] + ""))
                {
                    list[i] = task;
                    dbHandle.save("playerTask", list);
                    break;
                }
            }
        }

        /**添加任务到缓存*/
        private void addTaskFromCache(string taskKey, int status, int progressIndex, JObject taskProgress)
        {
            JArray list = this.getTaskListFromCache();
            bool b = false;
            for (int i = 0; i < list.Count; i++)
            {
                JObject l = (JObject)list[i];
                if ((l["key"] + "").Equals(taskKey))
                {
                    b = true;
                    l["progressIndex"] = progressIndex;
                    l["status"] = status;
                    l["taskProgress"] = taskProgress;
                    break;
                }
            }
            if (!b)
            {
                JObject obj = new JObject();
                obj.Add("key", taskKey);
                obj.Add("progressIndex", progressIndex);
                obj.Add("status", status);
                obj.Add("taskProgress", taskProgress);
                list.Add(obj);
            }
            dbHandle.save("playerTask", list);
        }
        /**从缓存中移除任务*/
        public void remTaskFromCache(string key)
        {
            JArray list = this.getTaskListFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject l = (JObject)list[i];
                if ((l["key"].ToString()).Equals(key))
                {
                    list.RemoveAt(i);
                    dbHandle.save("playerTask", list);
                    break;
                }
            }
        }
        /**移除完成的缓存*/
        public void remOverTaskFromCache(string key)
        {
            JArray list = this.getOverListFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ToString().Equals(key))
                {
                    list.RemoveAt(i);
                    dbHandle.save("overTask", list);
                    break;
                }
            }
        }
        /**移除缓存中的任务，包括提交的*/
        public void remTaskAndOverTaskFromCache(string[] keys)
        {
            foreach (string key in keys)
            {
                remTaskFromCache(key);
                remOverTaskFromCache(key);
            }

        }
        public void remTaskAndOverTaskFromCache(JArray keys)
        {
            foreach (object key in keys)
            {
                remTaskFromCache(key.ToString());
                remOverTaskFromCache(key.ToString());
            }

        }
        /**获取缓存的任务*/
        public JArray getTaskListFromCache()
        {
            JArray js = dbHandle.get<JArray>("playerTask");
            if (js == null) js = new JArray();
            return js;
        }
        /**获取标记提交的任务*/
        public JArray getOverListFromCache()
        {
            JArray js = dbHandle.get<JArray>("overTask");
            if (js == null) js = new JArray();
            return js;
        }
        /**
         1000-5000中任务数据实际存在的key
         */
        public JArray getTaskIndexFromCache()
        {
            JArray js = dbHandle.get<JArray>("taskIndex");
            if (js == null) js = new JArray();
            return js;
        }
        /**判断任务是否存在并且状态是待完成*/
        public bool isDoingTask(string key)
        {
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key) && (int)a["status"] == 2) return true;
            }

            return false;
        }
        /**正在做的任务并且进度值一致*/
        public bool isDoingTaskAndProgressSame(string key, int pIndex)
        {
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key) && (int)a["status"] == 2 && (int)a["progressIndex"] == pIndex) return true;
            }

            return false;
        }
        /**判断是否是提交的任务*/
        public bool isExistCommitTask(string key)
        {
            JArray list = this.getOverListFromCache();
            foreach (object p in list)
            {
                if (p.ToString().Equals(key)) return true;
            }
            return false;
        }
        /**是否存在某些任务*/
        public bool isExistTaskFromCacheTaskListAndCommit(string key)
        {
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key)) return true;
            }
            list = this.getOverListFromCache();
            foreach (object p in list)
            {
                if (p.ToString().Equals(key)) return true;
            }
            return false;
        }
        public bool isExistTaskFromCacheTaskList(string key)
        {
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if (a["key"].ToString().Equals(key)) return true;
            }

            return false;
        }
        /**根据状态从缓存中帅选任务*/
        public JArray getCacheTaskListByStatus(int status1, int status2)
        {
            JArray arr = new JArray();
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                int status = (int)a["status"];
                if (status1 <= status && status <= status2) arr.Add(a);
            }
            return arr;
        }
        /**根据状态从缓存中帅选任务*/
        public JArray getCacheTaskListByStatus(int status)
        {
            JArray arr = new JArray();
            JArray list = this.getTaskListFromCache();
            foreach (object p in list)
            {
                JObject a = (JObject)p;
                if ((int)a["status"] == status) arr.Add(a);
            }
            return arr;
        }

        public JObject getTaskFromCache(string key)
        {
            JArray list = this.getTaskListFromCache();
            foreach (object l in list)
            {
                JObject o = (JObject)l;
                if ((o.GetValue("key") + "").Equals(key))
                {
                    return o;
                }
            }
            return null;
        }
        /**缓存任务列表*/
        public void initTaskList(JArray playerTask, JArray overTask, JArray taskIndex)
        {
            dbHandle.save("playerTask", playerTask);
            dbHandle.save("overTask", overTask);
            dbHandle.save("taskIndex", taskIndex);
        }
        /**获取不限时活动y*/
        public string[] getNoLimitTimeAcList()
        {
            string[] arr = { "2000", "2002", "2003", "2005", "2007", "2008",  "2011", "2016", "2017",
        "2018","2019","2021","2022","2023","2025","2028","2029",};
            return arr;
        }
        public string[] getLimitTimeAcList()
        {
            string[] arr = { "2001", "2004", "2006", "2009", "2012", "2013", "2024", };
            return arr;
        }
        public string[] getElseAcList()
        {
            List<string> list = new List<string>();
            for (int i = 20430000; i < 20430014; i++)
            {
                list.Add(i + "");
            }
            return list.ToArray();
        }
        public List<task> getAcTaskList()
        {
            List<task> arr = new List<task>();
            for (int i = 2000; i < 2050; i++)
            {
                task a = this.getTask(i + "");
                if (a != null)
                    arr.Add(a);
            }
            for (int i = 20410000; i < 20410006; i++)
            {
                task a = this.getTask(i + "");
                if (a != null)
                    arr.Add(a);
            }
            return arr;
        }
        /**获取已开启的活动 */
        public List<task> getStartActivity()
        {
            List<task> arr = new List<task>();
            List<task> list = this.getAcTaskList();
            //int lv = (int)face.roleInterface.getRole()["lever"];
            foreach (task a in list)
            {
                //判断是否达到开启条件
                if (a.type == 2)
                {
                    arr.Add(a);
                }
            }
            return arr;
        }
        /**判断一个任务是否达到开启条件 */
        private bool isStartCondition(task task, int lever)
        {
            bool b = false;
            int type = (int)task.type;
            if (type == 0)
            {//等级是否达到开启条件
                if (lever >= (int)task.acMsg["limitLv"]) b = true;
            }
            else if (type == 1)
            {//时间
                b = false;
            }
            else if (type == 2)
            {//上个任务的key
             //改为判断是否在overTask里面
                if (this.isInSubmit(task.key)) b = true;
            }
            return b;
        }
        /**判断是否位已提交的任务*/
        private bool isInSubmit(string key)
        {
            JArray list = getOverListFromCache();
            foreach (object p in list)
            {
                if (key == p.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        /**获取任务基本信息*/
        public task getTask(string key)
        {
            task task = null;
            switch (key)
            {
                /*case "999":
                    {
                        task = new task(key, 0);
                        //0对话1摘取2战斗3接取即完成（指的是当前进度）4什么也不发生
                        task.addTarget(2, "小试牛刀", "干掉一只小蝙蝠", "1002", "1000", 0, 1, 1);
                        task.bindCondition(1);
                        break;
                    }*/
                //遗弃的村庄
                case "1000":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000000", "10000000")
                            .addTarget(0, "与村长交谈", "与<color=#1b8100>村长</color>交谈", "10000000", "10000000", 0, 1)
                            .addTargetPos("m_1", "10000000")
                            .addTarget(0, "与村长告别1", "苏醒后向<color=#1b8100>村长</color>告别", "10000000", "10000000", 0, 1)
                            .addTargetPos("m_1", "10000000")
                            .addTarget(0, "与村长告别2", "苏醒后向<color=#91db64>村长</color>告别", "10000000", "10000000", 0, 1)
                            .addTargetPos("m_1", "10000000")
                            .bindCondition(1);//赠送 封妖石（开赤翼蝠）
                        break;
                    }
                case "1001":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000003", "10000003")
                            .addTarget(0, "展护卫的邀战", "<color=#1b8100>展护卫</color>的邀战", "10000003", "10000003", 0, 1)
                            .addTargetPos("m_2", "10000003")
                            .addTarget(2, "与展护卫战斗", "与<color=#1b8100>展护卫</color>战斗", "10000003", "boss_0", 0, 1, 1)
                            .addTargetPos("m_2", "10000003")
                            .addTarget(0, "与展护卫交谈", "与<color=#1b8100>展护卫</color>交谈", "10000003", "10000003", 0, 1)
                            .addTargetPos("m_2", "10000003")
                            .bindCondition(1, "1000");
                        break;
                    }
                case "1002":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000002", "10000002")
                            .addTarget(0, "向扁老鹊道谢", "向<color=#1b8100>扁老鹊</color>道谢", "10000002", "10000002", 0, 1)
                            .addTargetPos("m_2", "10000002")
                            .addTarget(2, "心魔袭来", "<color=#1b8100>心魔</color>袭来", "10000002", "boss_1", 0, 1, 1)
                            .addTargetPos("m_2", "10000002")
                            .bindCondition(1, "1001");
                        break;
                    }
                case "1003":
                    {
                        task = new task(key, 0);//清溪子
                        task.addGetAndSubNpcKey("10000005", "10000005")
                            .addTarget(1, "探查", "探查<color=#1b8100>神韵</color>", "10000005", "cj1000", 0, 1)
                            .addTargetPos("m_3", "cj1000")
                            .addTarget(0, "与陨神残念交谈", "与<color=#1b8100>陨神残念</color>交谈", "10000005", "10000005", 0, 1)
                            .addTargetPos("m_3", "10000005")
                            .addTarget(0, "陨神的馈赠", "与<color=#1b8100>陨神残念</color>交谈", "10000005", "10000005", 0, 1)
                            .addTargetPos("m_3", "10000005")
                            .bindCondition(1, "1002");
                        break;
                    }
                case "1004":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000009", "10000009")
                            .addTarget(0, "与墨家接引师交谈", "与<color=#1b8100>墨家接引师</color>交谈", "10000009", "10000009", 0, 1)
                            .addTargetPos("m_3", "10000009")
                            .bindCondition(1, "1003");
                        break;
                    }
                case "1005":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000007", "10000007")
                            .addTarget(0, "与道家接引师交谈", "与<color=#1b8100>道家接引师</color>交谈", "10000007", "10000007", 0, 1)
                            .addTargetPos("m_3", "10000007")
                            .bindCondition(1, "1004");
                        break;
                    }
                case "1006":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000008", "10000008")
                            .addTarget(0, "与阴阳接引师交谈", "与<color=#1b8100>阴阳接引师</color>交谈", "10000008", "10000008", 0, 1)
                            .addTargetPos("m_3", "10000008")
                            .bindCondition(1, "1005");
                        break;
                    }
                case "1007":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000010", "10000010")
                            .addTarget(2, "神秘力量", "来自<color=#1b8100>赵村村长</color>的神秘力量", "10000010", "boss_2", 0, 1, 1)
                            .addTargetPos("m_4", "10000010")
                            .addTarget(0, "梦境", "与<color=#1b8100>赵村村长</color>交谈", "10000010", "10000010", 0, 1)
                            .addTargetPos("m_4", "10000010")
                            .bindCondition(1, "1006");
                        break;
                    }
                case "1008":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000010", "10000010")
                            .addTarget(4, "血盟", "清理<color=#1b8100>10只食尸虫</color>", "10000010", "1004", 1, 10, 1)
                            .addTargetPos("m_4", "10000010")
                            .bindCondition(1, "1007");
                        break;
                    }
                case "1009"://除魔军士卒
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000011", "10000011")
                           .addTarget(2, "妖魔袭来", "击败来袭的<color=#1b8100>邪月</color>", "10000011", "boss_3", 0, 1, 1)
                           .addTargetPos("m_4", "10000011")
                           .bindCondition(1, "1008");
                        break;
                    }
                case "1010":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000011", "10000011")
                           .addTarget(4, "乘胜追击", "铲除<color=#1b8100>10只利爪幼虎</color>", "10000011", "1003", 1, 10, 1)
                           .addTargetPos("m_4", "10000011")
                           .bindCondition(1, "1009");
                        break;
                    }
                case "1011":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000012", "10000012")
                            .addTarget(0, "收风", "与<color=#1b8100>萧恨水</color>交谈", "10000012", "10000012", 0, 1)
                            .addTargetPos("m_4", "10000012")
                            .bindCondition(1, "1010");
                        break;
                    }
                case "1012":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000013", "10000013")
                           .addTarget(2, "误会", "击败<color=#1b8100>韩信</color>", "10000013", "boss_4", 0, 1, 1)
                           .addTargetPos("m_5", "10000013")
                           .bindCondition(1, "1011");
                        break;
                    }
                //至此18级
                case "1013":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000013", "10000013")
                            .addTarget(0, "天星子", "与<color=#1b8100>韩信</color>交谈", "10000013", "10000013", 0, 1)
                            .addTargetPos("m_5", "10000013")
                            .bindCondition(1, "1012");
                        break;
                    }
                case "1014":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000013", "10000013")
                            .addTarget(0, "临别", "与<color=#1b8100>韩信</color>交谈", "10000013", "10000013", 0, 1)
                            .addTargetPos("m_5", "10000013")
                            .bindCondition(1, "1013");
                        break;
                    }
                case "1015":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000014", "10000014")
                            .addTarget(0, "装备", "与<color=#1b8100>装备商人</color>交谈", "10000014", "10000014", 0, 1)
                            .addTargetPos("m_6", "10000014")
                            .bindCondition(1, "1014");
                        break;
                    }
                case "1016":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000018", "10000018")
                            .addTarget(0, "那一晚", "与<color=#1b8100>白素</color>交谈", "10000018", "10000018", 0, 1)
                            .addTargetPos("m_6", "10000018")
                            .addTarget(4, "蛇皮大衣", "去云雾谷，消灭<color=#1b8100>10只双头毒蛇</color>", "10000018", "1005", 1, 10, 1)
                            .addTargetPos("m_5", "10000018")
                            .bindCondition(1, "1015");
                        break;
                    }
                case "1017":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000018", "10000018")
                            .addTarget(0, "送信", "与<color=#1b8100>白素</color>交谈", "10000018", "10000018", 0, 1)
                            .addTargetPos("m_6", "10000018")
                            .bindCondition(1, "1016");
                        break;
                    }
                case "1018":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000026", "10000026")
                            .addTarget(0, "送信1", "与<color=#1b8100>萧恨水</color>交谈", "10000026", "10000026", 0, 1)
                            .addTargetPos("m_7", "10000026")
                            .bindCondition(1, "1017");
                        break;
                    }
                case "1019":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000026", "10000026")
                           .addTarget(2, "遇袭", "击败<color=#1b8100>暗影魔将</color>", "10000026", "boss_5", 0, 1, 1)
                           .addTargetPos("m_7", "10000026")
                           .bindCondition(1, "1018");
                        break;
                    }
                case "1020":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000026", "10000026")
                            .addTarget(0, "阻碍", "与<color=#1b8100>萧恨水</color>交谈", "10000026", "10000026", 0, 1)
                            .addTargetPos("m_7", "10000026")
                            .bindCondition(1, "1019");
                        break;
                    }

                case "1021":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000018", "10000018")
                           .addTarget(2, "再入梦境", "击败<color=#1b8100>梦渊尸魔</color>", "10000018", "boss_6", 0, 1, 1)
                           .addTargetPos("m_6", "10000018")
                           .bindCondition(1, "1020");
                        break;
                    }
                case "1022":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000018", "10000018")
                           .addTarget(2, "诱惑", "击败<color=#1b8100>天星子</color>", "10000018", "boss_7", 0, 1, 1)
                           .addTargetPos("m_6", "10000018")
                           .bindCondition(1, "1021");
                        break;
                    }
                case "1023":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000018", "10000018")
                            .addTarget(0, "追击", "与<color=#1b8100>白素</color>交谈", "10000018", "10000018", 0, 1)
                            .addTargetPos("m_6", "10000018")
                            .bindCondition(1, "1022");
                        break;
                    }
                case "1024":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000053", "10000053")
                            .addTarget(0, "伤势", "与<color=#1b8100>韩信</color>交谈", "10000053", "10000053", 0, 1)
                            .addTargetPos("m_9", "10000053")
                            .bindCondition(1, "1023");
                        break;
                    }
                case "1025":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000052", "10000052")
                            .addTarget(1, "采药", "采集<color=#1b8100>九心海棠</color>后交给除魔军统领", "10000052", "cj1001", 0, 1)
                            .addTargetPos("m_10", "cj1001")
                            .bindCondition(1, "1024");
                        break;
                    }
                case "1026":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000052", "10000052")
                           .addTarget(2, "尸王", "击败<color=#1b8100>尸王</color>", "10000052", "boss_8", 0, 1, 1)
                           .addTargetPos("m_9", "10000052")
                           .bindCondition(1, "1025");
                        break;
                    }
                case "1027":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000052", "10000052")
                            .addTarget(4, "探哨", "清理<color=#1b8100>10只异变尸煞</color>", "10000052", "1008", 0, 10, 1)
                            .addTargetPos("m_10", "-1")
                            .bindCondition(1, "1026");
                        break;
                    }
                case "1028":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000058", "10000058")
                            .addTarget(4, "解围1", "清理<color=#1b8100>10只铁钩手</color>", "10000058", "1009", 0, 10, 1)
                            .addTargetPos("m_11", "-1")
                            .bindCondition(1, "1027");
                        break;
                    }
                case "1029":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000058", "10000058")
                            .addTarget(4, "解围2", "清理<color=#1b8100>10只山野顽猴</color>", "10000058", "1010", 0, 10, 1)
                            .addTargetPos("m_11", "-1")
                            .bindCondition(1, "1028");
                        break;
                    }
                case "1030":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000058", "10000058")
                            .addTarget(0, "消息", "与<color=#1b8100>戚薇</color>交谈", "10000058", "10000058", 0, 1)
                            .addTargetPos("m_11", "10000058")
                            .bindCondition(1, "1029");
                        break;
                    }
                case "1031":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000058", "10000058")
                           .addTarget(2, "猴王", "击败<color=#1b8100>猴王</color>", "10000058", "boss_9", 0, 1, 1)
                           .addTargetPos("m_11", "10000058")
                           .bindCondition(1, "1030");
                        break;
                    }
                case "1032":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000059", "10000059")
                           .addTarget(4, "解决", "清理<color=#1b8100>10只赤灵</color>", "10000059", "1014", 0, 10, 1)
                           .addTargetPos("m_13", "10000059")
                           .bindCondition(1, "1031");
                        break;
                    }
                case "1033":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000059", "10000059")
                            .addTarget(0, "阵法", "与<color=#1b8100>见影</color>交谈", "10000059", "10000059", 0, 1)
                            .addTargetPos("m_13", "10000059")
                            .bindCondition(1, "1032");
                        break;
                    }
                case "1034":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000059", "10000059")
                           .addTarget(4, "取灵", "清理<color=#1b8100>10只苍鹰</color>", "10000059", "1013", 0, 10, 1)
                           .addTargetPos("m_13", "10000059")
                           .bindCondition(1, "1033");
                        break;
                    }
                case "1035":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000059", "10000059")
                            .addTarget(0, "告别", "与<color=#1b8100>见影</color>交谈", "10000059", "10000059", 0, 1)
                            .addTargetPos("m_13", "10000059")
                            .bindCondition(1, "1034");
                        break;
                    }
                case "1036":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000060", "10000060")
                           .addTarget(4, "镇压1", "清理<color=#1b8100>10只江鳇鱼</color>", "10000060", "1015", 0, 10, 1)
                           .addTargetPos("m_14", "10000060")
                           .bindCondition(1, "1035");
                        break;
                    }
                case "1037":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000060", "10000060")
                           .addTarget(4, "镇压2", "清理<color=#1b8100>10只狂暴兽人</color>", "10000060", "1016", 0, 10, 1)
                           .addTargetPos("m_14", "10000060")
                           .bindCondition(1, "1036");
                        break;
                    }
                case "1038":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000060", "10000060")
                           .addTarget(2, "狂暴兽王", "击败<color=#1b8100>狂暴兽王</color>", "10000060", "boss_10", 0, 1, 1)
                           .addTargetPos("m_14", "10000060")
                           .bindCondition(1, "1037");
                        break;
                    }
                case "1039":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                            .addTarget(0, "再见", "与<color=#1b8100>除魔军士卒</color>交谈", "10000061", "10000061", 0, 1)
                            .addTargetPos("m_15", "10000061")
                            .bindCondition(1, "1038");
                        break;
                    }
                case "1040":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                           .addTarget(2, "炎狼", "击败<color=#1b8100>炎狼</color>", "10000061", "boss_11", 0, 1, 1)
                           .addTargetPos("m_15", "10000061")
                           .bindCondition(1, "1039");
                        break;
                    }
                case "1041":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                           .addTarget(4, "除妖", "清理<color=#1b8100>10只草藤妖</color>", "10000061", "1017", 0, 10, 1)
                           .addTargetPos("m_15", "10000061")
                           .bindCondition(1, "1040");
                        break;
                    }
                case "1042":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                            .addTarget(0, "再见", "与<color=#1b8100>除魔军士卒</color>交谈", "10000061", "10000061", 0, 1)
                            .addTargetPos("m_15", "10000061")
                            .bindCondition(1, "1041");
                        break;
                    }
                case "1043":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                           .addTarget(2, "再遇炎狼", "击败<color=#1b8100>炎狼</color>", "10000061", "boss_12", 0, 1, 1)
                           .addTargetPos("m_15", "10000061")
                           .bindCondition(1, "1042");
                        break;
                    }
                case "1044":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000061", "10000061")
                            .addTarget(0, "练习", "与<color=#1b8100>除魔军士卒</color>交谈", "10000061", "10000061", 0, 1)
                            .addTargetPos("m_15", "10000061")
                            .bindCondition(1, "1043");
                        break;
                    }
                case "1045":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000062", "10000062")
                            .addTarget(0, "暗杀", "与<color=#1b8100>陆风</color>交谈", "10000062", "10000062", 0, 1)
                            .addTargetPos("m_16", "10000062")
                            .bindCondition(1, "1044");
                        break;
                    }
                case "1046":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000062", "10000062")
                            .addTarget(0, "失手", "与<color=#1b8100>陆风</color>交谈", "10000062", "10000062", 0, 1)
                            .addTargetPos("m_16", "10000062")
                            .bindCondition(1, "1045");
                        break;
                    }
                case "1047":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000062", "10000062")
                           .addTarget(4, "清除", "清理<color=#1b8100>10只大刀兵</color>", "10000062", "1020", 0, 10, 1)
                           .addTargetPos("m_16", "10000062")
                           .bindCondition(1, "1046");
                        break;
                    }
                case "1048":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000066", "10000066")
                           .addTarget(2, "拦截", "击败<color=#1b8100>手下</color>", "10000066", "boss_13", 0, 1, 1)
                           .addTargetPos("m_16", "10000066")
                           .bindCondition(1, "1047");
                        break;
                    }
                case "1049":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000066", "10000066")
                           .addTarget(2, "拦截", "击败<color=#1b8100>刘大刀</color>", "10000066", "boss_14", 0, 1, 1)
                           .addTargetPos("m_16", "10000066")
                           .bindCondition(1, "1048");
                        break;
                    }
                case "1050":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000064", "10000064")
                           .addTarget(4, "赶走", "清理<color=#1b8100>10只恶犬</color>", "10000064", "1019", 0, 10, 1)
                           .addTargetPos("m_16", "10000064")
                           .bindCondition(1, "1049");
                        break;
                    }
                case "1051":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000064", "10000064")
                            .addTarget(0, "再遇", "与<color=#1b8100>吴月越</color>交谈", "10000064", "10000064", 0, 1)
                            .addTargetPos("m_16", "10000064")
                            .bindCondition(1, "1050");
                        break;
                    }
                case "1052":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000064", "10000064")
                           .addTarget(2, "天星子", "击败<color=#1b8100>天星子</color>", "10000064", "boss_15", 0, 1, 1)
                           .addTargetPos("m_16", "10000064")
                           .bindCondition(1, "1051");
                        break;
                    }
                case "1053":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000064", "10000064")
                            .addTarget(0, "伤重", "与<color=#1b8100>吴月越</color>交谈", "10000064", "10000064", 0, 1)
                            .addTargetPos("m_16", "10000064")
                            .bindCondition(1, "1052");
                        break;
                    }
                case "1054":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000068", "10000068")
                           .addTarget(4, "收集", "清理<color=#1b8100>10只崩角牛</color>", "10000068", "1021", 0, 10, 1)
                           .addTargetPos("m_17", "10000068")
                           .bindCondition(1, "1053");
                        break;
                    }
                case "1055":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000068", "10000068")
                            .addTarget(0, "解救", "与<color=#1b8100>白老怪</color>交谈", "10000068", "10000068", 0, 1)
                            .addTargetPos("m_17", "10000068")
                            .bindCondition(1, "1054");
                        break;
                    }
                case "1056":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000068", "10000068")
                           .addTarget(2, "喽啰", "击败<color=#1b8100>喽啰</color>", "10000068", "boss_16", 0, 1, 1)
                           .addTargetPos("m_17", "10000068")
                           .bindCondition(1, "1055");
                        break;
                    }
                case "1057":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000068", "10000068")
                           .addTarget(2, "啸月", "击败<color=#1b8100>啸月</color>", "10000068", "boss_17", 0, 1, 1)
                           .addTargetPos("m_17", "10000068")
                           .bindCondition(1, "1056");
                        break;
                    }
                case "1058":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000067", "10000067")
                            .addTarget(0, "提醒", "与<color=#1b8100>邯郸怪人</color>交谈", "10000067", "10000067", 0, 1)
                            .addTargetPos("m_17", "10000067")
                            .bindCondition(1, "1057");
                        break;
                    }
                case "1059":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000069", "10000069")
                           .addTarget(2, "牛头怪", "击败<color=#1b8100>牛头怪</color>", "10000069", "boss_18", 0, 1, 1)
                           .addTargetPos("m_17", "10000069")
                           .bindCondition(1, "1058");
                        break;
                    }
                case "1060":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000069", "10000069")
                            .addTarget(1, "采药", "采集<color=#1b8100>草止血</color>", "10000069", "cj1002", 0, 1)
                            .addTargetPos("m_17", "cj1002")
                            .bindCondition(1, "1059");
                        break;
                    }
                case "1061":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                           .addTarget(2, "修罗分身", "击败<color=#1b8100>修罗分身</color>", "10000071", "boss_19", 0, 1, 1)
                           .addTargetPos("m_19", "10000071")
                           .bindCondition(1, "1060");
                        break;
                    }
                case "1062":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                           .addTarget(2, "心魔", "击败<color=#1b8100>心魔</color>", "10000071", "boss_20", 0, 1, 1)
                           .addTargetPos("m_19", "10000071")
                           .bindCondition(1, "1061");
                        break;
                    }
                case "1063":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                            .addTarget(0, "抱歉", "与<color=#1b8100>酒泉子</color>交谈", "10000071", "10000071", 0, 1)
                            .addTargetPos("m_19", "10000071")
                            .bindCondition(1, "1062");
                        break;
                    }
                case "1064":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                           .addTarget(2, "村民", "击败<color=#1b8100>村民</color>", "10000071", "boss_21", 0, 1, 1)
                           .addTargetPos("m_19", "10000071")
                           .bindCondition(1, "1063");
                        break;
                    }
                case "1065":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                           .addTarget(2, "修罗之神", "击败<color=#1b8100>修罗之神</color>", "10000071", "boss_22", 0, 1, 1)
                           .addTargetPos("m_19", "10000071")
                           .bindCondition(1, "1064");
                        break;
                    }
                case "1066":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000071", "10000071")
                            .addTarget(1, "采药", "采集<color=#1b8100>明心草</color>后交给酒泉子", "10000071", "cj1003", 0, 1)
                            .addTargetPos("m_18", "cj1003")
                            .bindCondition(1, "1065");
                        break;
                    }
                case "1067":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000075", "10000075")
                            .addTarget(0, "提醒", "与<color=#1b8100>文心燕</color>交谈", "10000075", "10000075", 0, 1)
                            .addTargetPos("m_23", "10000075")
                            .bindCondition(1, "1066");
                        break;
                    }
                case "1068":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000075", "10000075")
                           .addTarget(2, "狗群", "击败<color=#1b8100>狗群</color>", "10000075", "boss_23", 0, 1, 1)
                           .addTargetPos("m_23", "10000075")
                           .bindCondition(1, "1067");
                        break;
                    }
                case "1069":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000075", "10000075")
                           .addTarget(2, "狗王", "击败<color=#1b8100>狗王</color>", "10000075", "boss_24", 0, 1, 1)
                           .addTargetPos("m_23", "10000075")
                           .bindCondition(1, "1068");
                        break;
                    }
                case "1070":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000075", "10000075")
                            .addTarget(0, "提醒", "与<color=#1b8100>文心燕</color>交谈", "10000075", "10000075", 0, 1)
                            .addTargetPos("m_23", "10000075")
                            .bindCondition(1, "1069");
                        break;
                    }
                case "1071":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000072", "10000072")
                           .addTarget(4, "帮忙", "清理<color=#1b8100>10只湛蓝犬</color>", "10000072", "1025", 0, 10, 1)
                           .addTargetPos("m_23", "10000072")
                           .bindCondition(1, "1070");
                        break;
                    }
                case "1072":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000072", "10000072")
                            .addTarget(0, "领主", "与<color=#1b8100>木道人</color>交谈", "10000072", "10000072", 0, 1)
                            .addTargetPos("m_23", "10000072")
                            .bindCondition(1, "1071");
                        break;
                    }
                case "1073":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000072", "10000072")
                           .addTarget(2, "湛蓝领主", "击败<color=#1b8100>湛蓝领主</color>", "10000072", "boss_25", 0, 1, 1)
                           .addTargetPos("m_23", "10000072")
                           .bindCondition(1, "1072");
                        break;
                    }
                case "1074":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000072", "10000072")
                           .addTarget(4, "召集", "清理<color=#1b8100>10只树精</color>", "10000072", "1026", 0, 10, 1)
                           .addTargetPos("m_23", "10000072")
                           .bindCondition(1, "1073");
                        break;
                    }
                case "1075":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000076", "10000076")
                           .addTarget(2, "喽啰", "击败<color=#1b8100>喽啰</color>", "10000076", "boss_26", 0, 1, 1)
                           .addTargetPos("m_24", "10000076")
                           .bindCondition(1, "1074");
                        break;
                    }
                case "1076":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000076", "10000076")
                           .addTarget(2, "水晶", "击败<color=#1b8100>水晶守卫</color>", "10000076", "boss_27", 0, 1, 1)
                           .addTargetPos("m_24", "10000076")
                           .bindCondition(1, "1075");
                        break;
                    }
                case "1077":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000076", "10000076")
                           .addTarget(2, "钥匙", "击败<color=#1b8100>钥匙守卫</color>", "10000076", "boss_28", 0, 1, 1)
                           .addTargetPos("m_24", "10000076")
                           .bindCondition(1, "1076");
                        break;
                    }
                case "1078":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000076", "10000076")
                            .addTarget(0, "感谢", "与<color=#1b8100>陈道瑜</color>交谈", "10000076", "10000076", 0, 1)
                            .addTargetPos("m_24", "10000076")
                            .bindCondition(1, "1077");
                        break;
                    }
                case "1079":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000077", "10000077")
                            .addTarget(0, "消息", "与<color=#1b8100>刘捕头</color>交谈", "10000077", "10000077", 0, 1)
                            .addTargetPos("m_24", "10000077")
                            .bindCondition(1, "1078");
                        break;
                    }
                case "1080":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000077", "10000077")
                            .addTarget(0, "祭祀神殿", "与<color=#1b8100>刘捕头</color>交谈", "10000077", "10000077", 0, 1)
                            .addTargetPos("m_24", "10000077")
                            .bindCondition(1, "1079");
                        break;
                    }
                case "1081":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000077", "10000077")
                           .addTarget(4, "清理妖兽", "清理<color=#1b8100>10只食人狼</color>", "10000077", "1027", 0, 10, 1)
                           .addTargetPos("m_24", "10000077")
                           .bindCondition(1, "1080");
                        break;
                    }
                case "1082":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000079", "10000079")
                            .addTarget(0, "通知", "与<color=#1b8100>李峰</color>交谈", "10000079", "10000079", 0, 1)
                            .addTargetPos("m_25", "10000079")
                            .bindCondition(1, "1081");
                        break;
                    }
                case "1083":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000079", "10000079")
                            .addTarget(0, "行迹", "与<color=#1b8100>李峰</color>交谈", "10000079", "10000079", 0, 1)
                            .addTargetPos("m_25", "10000079")
                            .bindCondition(1, "1082");
                        break;
                    }
                case "1084":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000079", "10000079")
                           .addTarget(4, "前哨", "清理<color=#1b8100>10只鹰狮</color>", "10000079", "1029", 0, 10, 1)
                           .addTargetPos("m_25", "10000079")
                           .bindCondition(1, "1083");
                        break;
                    }
                case "1085":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000080", "10000080")
                           .addTarget(2, "蚩尤", "击败<color=#1b8100>蚩尤</color>", "10000080", "boss_29", 0, 1, 1)
                           .addTargetPos("m_25", "10000080")
                           .bindCondition(1, "1084");
                        break;
                    }
                case "1086":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000080", "10000080")
                            .addTarget(0, "行迹", "与<color=#1b8100>怪盗一枝花</color>交谈", "10000080", "10000080", 0, 1)
                            .addTargetPos("m_25", "10000080")
                            .bindCondition(1, "1085");
                        break;
                    }
                case "1087":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000078", "10000078")
                           .addTarget(4, "驱逐", "清理<color=#1b8100>10只蜥蜴</color>", "10000078", "1030", 0, 10, 1)
                           .addTargetPos("m_25", "10000078")
                           .bindCondition(1, "1086");
                        break;
                    }
                case "1088":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000078", "10000078")
                            .addTarget(0, "提醒", "与<color=#1b8100>薛贾</color>交谈", "10000078", "10000078", 0, 1)
                            .addTargetPos("m_25", "10000078")
                            .bindCondition(1, "1087");
                        break;
                    }
                case "1089":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000081", "10000081")
                            .addTarget(4, "报酬", "清理<color=#1b8100>30只红羽凶鹰</color>", "10000081", "1031", 0, 30, 1)
                            .addTargetPos("m_26", "10000081")
                            .bindCondition(1, "1088");
                        break;
                    }
                case "1090":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000081", "10000081")
                           .addTarget(2, "偷听者", "击败<color=#1b8100>偷听者</color>", "10000081", "boss_30", 0, 1, 1)
                           .addTargetPos("m_26", "10000081")
                           .bindCondition(1, "1089");
                        break;
                    }
                case "1091":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000082", "10000082")
                            .addTarget(0, "惊吓", "与<color=#1b8100>李利</color>交谈", "10000082", "10000082", 0, 1)
                            .addTargetPos("m_26", "10000082")
                            .bindCondition(1, "1090");
                        break;
                    }
                case "1092":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000082", "10000082")
                           .addTarget(2, "再遇蚩尤", "击败<color=#1b8100>蚩尤</color>", "10000082", "boss_31", 0, 1, 1)
                           .addTargetPos("m_26", "10000082")
                           .bindCondition(1, "1091");
                        break;
                    }
                case "1093":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000082", "10000082")
                           .addTarget(2, "阻碍", "击败<color=#1b8100>阻挡的妖兽</color>", "10000082", "boss_32", 0, 1, 1)
                           .addTargetPos("m_26", "10000082")
                           .bindCondition(1, "1092");
                        break;
                    }
                case "1094":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000084", "10000084")
                            .addTarget(0, "探查", "与<color=#1b8100>唐甜甜</color>交谈", "10000084", "10000084", 0, 1)
                            .addTargetPos("m_27", "10000084")
                            .bindCondition(1, "1093");
                        break;
                    }
                case "1095":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000083", "10000083")
                           .addTarget(2, "觉悟", "击败<color=#1b8100>龙啸天</color>", "10000083", "boss_33", 0, 1, 1)
                           .addTargetPos("m_27", "10000083")
                           .bindCondition(1, "1094");
                        break;
                    }
                case "1096":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000083", "10000083")
                            .addTarget(0, "马屁", "与<color=#1b8100>龙啸天</color>交谈", "10000083", "10000083", 0, 1)
                            .addTargetPos("m_27", "10000083")
                            .bindCondition(1, "1095");
                        break;
                    }
                case "1097":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000083", "10000083")
                           .addTarget(2, "子尔邑", "击败<color=#1b8100>子尔邑</color>", "10000083", "boss_34", 0, 1, 1)
                           .addTargetPos("m_27", "10000083")
                           .bindCondition(1, "1096");
                        break;
                    }
                case "1098":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000083", "10000083")
                            .addTarget(0, "良心未泯", "与<color=#1b8100>龙啸天</color>交谈", "10000083", "10000083", 0, 1)
                            .addTargetPos("m_27", "10000083")
                            .bindCondition(1, "1097");
                        break;
                    }
                case "1099":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000084", "10000084")
                            .addTarget(4, "宁静", "清理<color=#1b8100>10只古牙兽</color>", "10000084", "1035", 0, 10, 1)
                            .addTargetPos("m_27", "10000084")
                            .bindCondition(1, "1098");
                        break;
                    }
                case "1100":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000084", "10000084")
                            .addTarget(4, "引龙草", "清理<color=#1b8100>10只龙姣姣</color>", "10000084", "1034", 0, 10, 1)
                            .addTargetPos("m_27", "10000084")
                            .bindCondition(1, "1099");
                        break;
                    }
                case "1101":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000084", "10000084")
                           .addTarget(2, "龙姣王", "击败<color=#1b8100>龙姣王</color>", "10000084", "boss_35", 0, 1, 1)
                           .addTargetPos("m_27", "10000084")
                           .bindCondition(1, "1100");
                        break;
                    }
                case "1102":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                            .addTarget(0, "嫁妆", "与<color=#1b8100>郭大娘</color>交谈", "10000087", "10000087", 0, 1)
                            .addTargetPos("m_28", "10000087")
                            .bindCondition(1, "1101");
                        break;
                    }
                case "1103":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000088", "10000088")
                            .addTarget(0, "送回", "与<color=#1b8100>唐少渊</color>交谈", "10000088", "10000088", 0, 1)
                            .addTargetPos("m_28", "10000088")
                            .bindCondition(1, "1102");
                        break;
                    }
                case "1104":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                           .addTarget(2, "毒计", "击败<color=#1b8100>子尔邑</color>", "10000087", "boss_36", 0, 1, 1)
                           .addTargetPos("m_28", "10000087")
                           .bindCondition(1, "1103");
                        break;
                    }
                case "1105":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                           .addTarget(2, "抢回", "击败<color=#1b8100>子尔邑</color>", "10000087", "boss_37", 0, 1, 1)
                           .addTargetPos("m_28", "10000087")
                           .bindCondition(1, "1104");
                        break;
                    }
                case "1106":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                            .addTarget(0, "送回", "与<color=#1b8100>郭大娘</color>交谈", "10000087", "10000087", 0, 1)
                            .addTargetPos("m_28", "10000087")
                            .bindCondition(1, "1105");
                        break;
                    }
                case "1107":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                           .addTarget(2, "再揍", "击败<color=#1b8100>子尔邑</color>", "10000087", "boss_38", 0, 1, 1)
                           .addTargetPos("m_28", "10000087")
                           .bindCondition(1, "1106");
                        break;
                    }
                case "1108":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000087", "10000087")
                            .addTarget(0, "拆散", "与<color=#1b8100>郭大娘</color>交谈", "10000087", "10000087", 0, 1)
                            .addTargetPos("m_28", "10000087")
                            .bindCondition(1, "1107");
                        break;
                    }
                case "1109":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000086", "10000086")
                            .addTarget(0, "入魔", "与<color=#1b8100>范小弟</color>交谈", "10000086", "10000086", 0, 1)
                            .addTargetPos("m_28", "10000086")
                            .bindCondition(1, "1108");
                        break;
                    }
                case "1110":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000086", "10000086")
                           .addTarget(2, "赶尽杀绝", "击败<color=#1b8100>子尔邑</color>", "10000086", "boss_39", 0, 1, 1)
                           .addTargetPos("m_28", "10000086")
                           .bindCondition(1, "1109");
                        break;
                    }
                case "1111":
                    {
                        task = new task(key, 0);
                        task.addGetAndSubNpcKey("10000086", "10000086")
                           .addTarget(2, "入魔", "击败<color=#1b8100>范小弟</color>", "10000086", "boss_40", 0, 1, 1)
                           .addTargetPos("m_28", "10000086")
                           .bindCondition(1, "1110");
                        break;
                    }
                case "1112":
                    {
                        task = new task(key, 0, "50级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "50级突破", "击败心魔", "10000047", "tupo_1", 0, 1, 1);
                        task.bindCondition(49, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                case "1113":
                    {
                        task = new task(key, 0, "60级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "60级突破", "击败心魔", "10000047", "tupo_2", 0, 1, 1);
                        task.bindCondition(59, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                case "1114":
                    {
                        task = new task(key, 0, "70级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "70级突破", "击败心魔", "10000047", "tupo_3", 0, 1, 1);
                        task.bindCondition(69, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                case "1115":
                    {
                        task = new task(key, 0, "80级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "80级突破", "击败心魔", "10000047", "tupo_4", 0, 1, 1);
                        task.bindCondition(79, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                case "1116":
                    {
                        task = new task(key, 0, "90级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "90级突破", "击败心魔", "10000047", "tupo_5", 0, 1, 1);
                        task.bindCondition(89, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                case "1117":
                    {
                        task = new task(key, 0, "100级突破");
                        task.addGetAndSubNpcKey("10000047", "10000047")
                                .addTarget(2, "100级突破", "击败心魔", "10000047", "tupo_6", 0, 1, 1);
                        task.bindCondition(99, null);
                        task.addTargetPos("m_9", "10000047");
                        break;
                    }
                //支线
                case "2000":
                    {
                        task = new task(key, 0, "门派的抉择");
                        task.addGetAndSubNpcKey("10000457", "10000457")
                                .addTarget(0, "门派的抉择", "与<color=#1b8100>墨家门主</color>交谈", "10000457", "10000457", 0, 1);
                        task.bindCondition(15, null);
                        task.addTargetPos("txzf", "10000457");
                        break;
                    }
                case "2001":
                    {
                        task = new task(key, 0, "分堂的抉择");
                        task.addGetAndSubNpcKey("10000458", "10000458")
                                .addTarget(0, "分堂的抉择", "与<color=#1b8100>猛士堂主</color>交谈", "10000458", "10000458", 0, 1);
                        task.bindCondition(30, null);
                        task.addTargetPos("txzf", "10000458");
                        break;
                    }
                case "2002":
                    {
                        task = new task(key, 0, "拜师");
                        task.addGetAndSubNpcKey("10000027", "10000027")
                                .addTarget(0, "拜师", "与<color=#1b8100>孔夫子</color>交谈", "10000027", "10000027", 0, 1);
                        task.bindCondition(15, null);
                        task.addTargetPos("m_7", "10000027");
                        break;
                    }
                case "2003":
                    {
                        task = new task(key, 0, "帮派");
                        task.addGetAndSubNpcKey("10000050", "10000050")
                                .addTarget(0, "帮派", "与<color=#1b8100>帮派管理员</color>交谈", "10000050", "10000050", 0, 1);
                        task.bindCondition(15, null);
                        task.addTargetPos("m_9", "10000050");
                        break;
                    }
                case "2004":
                    {
                        task = new task(key, 0, "法宝");
                        task.addGetAndSubNpcKey("10000045", "10000045")
                                .addTarget(0, "法宝", "与<color=#1b8100>菩提老祖</color>交谈", "10000045", "10000045", 0, 1);
                        task.bindCondition(60, null);
                        task.addTargetPos("m_8", "10000045");
                        break;
                    }

                default:
                    {
                        if (int.Parse(key) >= 3000 && int.Parse(key) < 3040)
                        {//深渊
                            int n = int.Parse(key) - 2999;
                            task = new task(key, 0);
                            string monKey = "abyss_" + (int.Parse(key) - 2999);
                            string npcKey = "10000481";
                            //由mapKey决定是否npc能显示任务
                            string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
                            string temp = mapKey.Replace("abyss", "");
                            if (mapKey == null || !mapKey.Contains("abyss") || !temp.Equals(n.ToString()))
                            {
                                npcKey = "-1";
                            }
                            task.addGetAndSubNpcKey(npcKey, npcKey)
                                .addTarget(4, "天渊 " + n, "天渊 " + n, npcKey, monKey, 0, 1, 1);
                            if (int.Parse(key) == 3000)
                            {
                                task.bindCondition(40);
                            }
                            else
                            {
                                task.bindCondition(40, int.Parse(key) - 1 + "");
                            }
                            task.addTargetPos("abyss" + (int.Parse(key) - 2999), npcKey);
                        }
                        else if (int.Parse(key) >= 3040 && int.Parse(key) < 3070)
                        {//修混
                            int n = int.Parse(key) - 3039;
                            task = new task(key, 0);
                            string monKey = null;
                            string title = null;
                            string npcKey = null;
                            if (n <= 10)
                            {
                                monKey = "petxl_" + (int.Parse(key) - 3039);
                                title = "炼狱之路" + n;
                                npcKey = "10000475";
                            }
                            else if (n > 10 && n <= 20)
                            {
                                title = "修罗之路" + (n - 10);
                                npcKey = "10000477";
                                monKey = "petxl_" + ((int.Parse(key) - 3050) * 2 + 11);
                            }
                            else if (n > 20)
                            {
                                title = "混沌之路" + (n - 20);
                                npcKey = "10000479";
                                monKey = "petxl_" + ((int.Parse(key) - 3050) * 2 + 11);
                            }
                            task.addGetAndSubNpcKey(npcKey, npcKey)
                            .addTarget(2, title, title, npcKey, monKey, 0, 1, 1);
                            if (int.Parse(key) == 3040 || int.Parse(key) == 3050 || int.Parse(key) == 3060)
                            {
                                task.bindCondition(40);
                            }
                            else
                            {
                                task.bindCondition(40, int.Parse(key) - 1 + "");
                            }
                            task.addTargetPos("petxl", npcKey);

                        }
                        else if (int.Parse(key) >= 3100 && int.Parse(key) < 3120)
                        {//职业跑环，10次，每次都是随机接取一个
                            task = new task(key, 0);
                            JObject r= face.roleInterface.getRole();
                            string mp= GameAttrConst.getMenPaiFromModels(r);
                            string subNpcKey = null;
                            if (mp!=null&&mp.Contains("zs")) subNpcKey = "10000457";
                            else if (mp != null && mp.Contains("fs")) subNpcKey = "10000463";
                            else if (mp != null && mp.Contains("fz")) subNpcKey = "10000469";
                            task.addGetAndSubNpcKey(subNpcKey, subNpcKey);
                            //从1开始
                            int index = int.Parse(key) - 3099;
                            int progressType = 0;
                            string des = null;
                            string toKey = null;
                            string targetKey = null;
                            int sum = 1;
                            int targetType = 0;//0npc 1怪物
                            if (index < 6)
                            {//跟npc谈话
                                progressType = 0;
                                if (index == 1)
                                {
                                    des = "跟储物箱对话";
                                    toKey = "10000001";
                                    targetKey = "10000001";
                                    task.addTargetPos("m_1", "10000001");
                                }
                                else if (index == 2)
                                {
                                    des = "跟村医扁老鹊对话";
                                    toKey = "10000002";
                                    targetKey = "10000002";
                                    task.addTargetPos("m_2", "10000002");
                                }
                                else if (index == 3)
                                {
                                    des = "跟展护卫对话";
                                    toKey = "10000003";
                                    targetKey = "10000003";
                                    task.addTargetPos("m_2", "10000003");
                                }
                                else if (index == 4)
                                {
                                    des = "跟兽医华小佗对话";
                                    toKey = "10000004";
                                    targetKey = "10000004";
                                    task.addTargetPos("m_2", "10000004");
                                }
                                else if (index == 5)
                                {
                                    des = "跟道家接引师对话";
                                    toKey = "10000007";
                                    targetKey = "10000007";
                                    task.addTargetPos("m_3", "10000007");
                                }
                            }
                            else if (index < 11)
                            {//收集道具
                                progressType = 1;
                                if (index == 6)
                                {
                                    des = "采集仙草";
                                    toKey = subNpcKey;
                                    targetKey = "cj1012";
                                    task.addTargetPos("m_2", "cj1012");
                                }
                                else if (index == 7)
                                {
                                    des = "采集灵木";
                                    toKey = subNpcKey;
                                    targetKey = "cj1013";
                                    task.addTargetPos("m_2", "cj1013");
                                }
                                else if (index == 8)
                                {
                                    des = "采集当归";
                                    toKey = subNpcKey;
                                    targetKey = "cj1014";
                                    task.addTargetPos("m_2", "cj1014");
                                }
                                else if (index == 9)
                                {
                                    des = "采集侧柏叶";
                                    toKey = subNpcKey;
                                    targetKey = "cj1015";
                                    task.addTargetPos("m_2", "cj1015");
                                }
                                else if (index == 10)
                                {
                                    des = "采集灵芝";
                                    toKey = subNpcKey;
                                    targetKey = "cj1016";
                                    task.addTargetPos("m_2", "cj1016");
                                }
                            }
                            else if (index < 16)
                            {//跟boss战斗
                                progressType = 2;
                                targetType = 1;
                                if (index == 11)
                                {
                                    des = "击败风笑天";
                                    toKey = subNpcKey;
                                    targetKey = "mprw_1";
                                }
                                else if (index == 12)
                                {
                                    des = "击败唐镇";
                                    toKey = subNpcKey;
                                    targetKey = "mprw_2";
                                }
                                else if (index == 13)
                                {
                                    des = "击败李立";
                                    toKey = subNpcKey;
                                    targetKey = "mprw_3";
                                }
                                else if (index == 14)
                                {
                                    des = "击败震天战魂";
                                    toKey = subNpcKey;
                                    targetKey = "mprw_4";
                                }
                                else if (index == 15)
                                {
                                    des = "击败阿布";
                                    toKey = subNpcKey;
                                    targetKey = "mprw_5";
                                }
                            }
                            else if (index < 21)
                            {//收集指定怪物数量
                                progressType = 4;
                                targetType = 1;
                                sum = 10;
                                if (index == 16)
                                {
                                    des = "击败10只狼蛛";
                                    toKey = subNpcKey;
                                    targetKey = "1001";
                                    task.addTargetPos("m_2", "-1");
                                }
                                else if (index == 17)
                                {
                                    des = "击败10只棕毛土狼";
                                    toKey = subNpcKey;
                                    targetKey = "1002";
                                    task.addTargetPos("m_3", "-1");
                                }
                                else if (index == 18)
                                {
                                    des = "击败10只利爪幼虎";
                                    toKey = subNpcKey;
                                    targetKey = "1003";
                                    task.addTargetPos("m_4", "-1");
                                }
                                else if (index == 19)
                                {
                                    des = "击败10只食尸虫";
                                    toKey = subNpcKey;
                                    targetKey = "1004";
                                    task.addTargetPos("m_4", "-1");
                                }
                                else if (index == 20)
                                {
                                    des = "击败10只双头毒蛇";
                                    toKey = subNpcKey;
                                    targetKey = "1005";
                                    task.addTargetPos("m_5", "-1");
                                }
                            }
                            task.addTarget(progressType, "门派任务" + index, des, toKey, targetKey, 0, sum, targetType);
                            //task.addTarget(0, "回复" , "跟宫主交谈", "2018", "2018", 0, 1);
                            task.bindCondition(20, "-1");
                        }
                        else if (int.Parse(key) >= 3120 && int.Parse(key) < 3123)
                        {//摸金校尉
                            int index = int.Parse(key) - 3120;
                            task = new task(key, 0);
                            string[] npcs = { "2055", "2056", "2057" };
                            string[] targetKey = { "mjxw_1", "mjxw_2", "mjxw_3" };
                            task.addTarget(2, "探秘 " + (index + 1), "探秘 " + (index + 1), npcs[index], targetKey[index], 0, 1, 1);
                            task.bindCondition(30, int.Parse(key) == 3120 ? null : (int.Parse(key) - 1 + ""));
                            task.addTargetPos("md1", npcs[index]);
                        }
                        else if (int.Parse(key) >= 3123 && int.Parse(key) < 3131)
                        {//云梯
                            int index = int.Parse(key) - 3123;
                            task = new task(key, 0, "扫塔任务");
                            string[] npcs = new string[8];
                            for (int i = 586; i < 592; i++)
                            {
                                npcs[i - 586] = "10000" + i;
                            }
                            string[] targetKey = new string[8];
                            for (int i = 1; i < 9; i++)
                            {
                                targetKey[i - 1] = "shitu_" + i;
                            }
                            task.addGetAndSubNpcKey(npcs[index], npcs[index]);
                            task.addTarget(2, "扫塔 " + (index + 1), "扫塔 " + (index + 1), npcs[index], targetKey[index], 0, 1, 1);
                            task.bindCondition(15 + (int.Parse(key) - 3123) * 5, "-1");
                            task.addTargetPos("kongmiao", npcs[index]);
                        }
                        else if (int.Parse(key) >= 3131 && int.Parse(key) < 3135)
                        {//神秘宝藏任务
                            task = new task(key, 0);
                            if (int.Parse(key) == 3131)
                            {
                                task.addTarget(0, "神秘宝藏", "神秘宝藏", "2068", "2068", 0, 1);
                                task.addTarget(4, "击败邪童子", "击败邪童子", "2068", "smbz_3", 0, 1, 1);
                                task.bindCondition(15, "-1");
                                task.addTargetPos("m_7", "2068");
                            }
                            else if (int.Parse(key) == 3132)
                            {
                                task.addTarget(2, "击败花生舞", "击败花生舞", "2069", "smbz_1", 0, 1, 1);
                                task.bindCondition(15, "-1");
                                task.addTargetPos("smbz", "2069");
                            }
                            else if (int.Parse(key) == 3133)
                            {
                                task.addTarget(2, "击败伴生灵", "击败伴生灵", "2070", "smbz_2", 0, 1, 1);
                                task.bindCondition(15, "3132");
                                task.addTargetPos("smbz", "2070");
                            }
                            else if (int.Parse(key) == 3134)
                            {
                                task.addTarget(2, "击败邪童子", "击败邪童子", "2071", "smbz_3", 0, 1, 1);
                                task.bindCondition(15, "3133");
                                task.addTargetPos("smbz", "2071");
                            }


                        }

                        else if (int.Parse(key) >= 3143 && int.Parse(key) < 3163)
                        {//帮派任务，10次，每次都是随机接取一个
                            task = new task(key, 0, "帮派任务");
                            task.addGetAndSubNpcKey("10000592", "10000592");
                            task.bindCondition(20, "-1");

                            //从1开始
                            int index = int.Parse(key) - 3142;
                            int progressType = 0;
                            string des = null;
                            string toKey = null;
                            string targetKey = null;
                            int sum = 1;
                            int targetType = 0;//0npc 1怪物
                            if (index < 6)
                            {//跟npc谈话
                                progressType = 0;
                                if (index == 1)
                                {
                                    des = "跟村长对话";
                                    toKey = "10000000";
                                    targetKey = "10000000";
                                    task.addTargetPos("m_1", "10000000");
                                }
                                else if (index == 2)
                                {
                                    des = "跟清溪子对话";
                                    toKey = "10000005";
                                    targetKey = "10000005";
                                    task.addTargetPos("m_3", "10000005");
                                }
                                else if (index == 3)
                                {
                                    des = "跟韩信对话";
                                    toKey = "10000013";
                                    targetKey = "10000013";
                                    task.addTargetPos("m_5", "10000013");
                                }
                                else if (index == 4)
                                {
                                    des = "跟见影对话";
                                    toKey = "10000059";
                                    targetKey = "10000059";
                                    task.addTargetPos("m_13", "10000059");
                                }
                                else if (index == 5)
                                {
                                    des = "跟金多多对话";
                                    toKey = "10000069";
                                    targetKey = "10000069";
                                    task.addTargetPos("m_17", "10000069");
                                }
                            }
                            else if (index < 11)
                            {//收集道具
                                progressType = 1;
                                if (index == 6)
                                {
                                    des = "采集琉璃花";
                                    toKey = "10000592";
                                    targetKey = "cj1012";
                                    task.addTargetPos("m_2", "cj1012");
                                }
                                else if (index == 7)
                                {
                                    des = "采集含香木";
                                    toKey = "10000592";
                                    targetKey = "cj1013";
                                    task.addTargetPos("m_2", "cj1013");
                                }
                                else if (index == 8)
                                {
                                    des = "采集西金岩";
                                    toKey = "10000592";
                                    targetKey = "cj1014";
                                    task.addTargetPos("m_2", "cj1014");
                                }
                                else if (index == 9)
                                {
                                    des = "采集鬼手花";
                                    toKey = "10000592";
                                    targetKey = "cj1015";
                                    task.addTargetPos("m_2", "cj1015");
                                }
                                else if (index == 10)
                                {
                                    des = "采集幽草";
                                    toKey = "10000592";
                                    targetKey = "cj1016";
                                    task.addTargetPos("m_2", "cj1016");
                                }
                            }
                            else if (index < 16)
                            {//跟boss战斗
                                progressType = 2;
                                targetType = 1;
                                if (index == 11)
                                {
                                    des = "击败寒风破";
                                    toKey = "10000592";
                                    targetKey = "bprw_1";
                                }
                                else if (index == 12)
                                {
                                    des = "击败冰啼";
                                    toKey = "10000592";
                                    targetKey = "bprw_2";
                                }
                                else if (index == 13)
                                {
                                    des = "击败千户花";
                                    toKey = "10000592";
                                    targetKey = "bprw_3";
                                }
                                else if (index == 14)
                                {
                                    des = "击败江陵幕";
                                    toKey = "10000592";
                                    targetKey = "bprw_4";
                                }
                                else if (index == 15)
                                {
                                    des = "击败凯玄";
                                    toKey = "10000592";
                                    targetKey = "bprw_5";
                                }
                            }
                            else if (index < 21)
                            {//收集指定怪物数量
                                progressType = 4;
                                targetType = 1;
                                sum = 10;
                                if (index == 16)
                                {
                                    des = "击败10只树精";
                                    toKey = "10000592";
                                    targetKey = "1026";
                                    task.addTargetPos("m_23", "-1");
                                }
                                else if (index == 17)
                                {
                                    des = "击败10只蜥蜴";
                                    toKey = "10000592";
                                    targetKey = "1030";
                                    task.addTargetPos("m_25", "-1");
                                }
                                else if (index == 18)
                                {
                                    des = "击败10只红羽凶鹰";
                                    toKey = "10000592";
                                    targetKey = "1031";
                                    task.addTargetPos("m_26", "-1");
                                }
                                else if (index == 19)
                                {
                                    des = "击败10只狩猎者";
                                    toKey = "10000592";
                                    targetKey = "1033";
                                    task.addTargetPos("m_26", "-1");
                                }
                                else if (index == 20)
                                {
                                    des = "击败10只铁骑枪兵";
                                    toKey = "10000592";
                                    targetKey = "1038";
                                    task.addTargetPos("m_28", "-1");
                                }
                            }
                            task.addTarget(progressType, "帮派任务" + index, des, toKey, targetKey, 0, sum, targetType);
                        }
                        else if (int.Parse(key) >= 3163 && int.Parse(key) < 3169)
                        {//50副本（4npc，4任务）
                            task = new task(key, 0, "紫阴古城");
                            if (int.Parse(key) == 3163)
                            {
                                task.addGetAndSubNpcKey("10000092", "10000500")
                                .addTarget(4, "噬人妖核", "收集6个噬人妖", "10000500", "fb_50_1", 0, 6, 1);
                                task.bindCondition(50, null);
                                task.addTargetPos("m_30", "10000092");
                            }
                            else if (int.Parse(key) == 3164)
                            {
                                task.addGetAndSubNpcKey("10000500", "10000500")
                                .addTarget(2, "噬魂魔将", "击败噬魂魔将", "10000500", "fb_50_2", 0, 1, 1);
                                task.bindCondition(50, "3163");
                                task.addTargetPos("m_30", "10000092");
                            }
                            else if (int.Parse(key) == 3165)
                            {
                                task.addGetAndSubNpcKey("10000500", "10000502")
                                .addTarget(0, "镇魔将军", "去紫阴废墟寻找镇魔将军", "10000502", "10000502", 0, 1);
                                task.bindCondition(50, "3164");
                                task.addTargetPos("m_30", "10000092");
                            }
                            else if (int.Parse(key) == 3166)
                            {
                                task.addGetAndSubNpcKey("10000502", "10000502")
                                .addTarget(2, "妖魔统帅", "消灭突然来袭的魔帅", "10000502", "fb_50_3", 0, 1, 1);
                                task.bindCondition(50, "3165");
                                task.addTargetPos("m_30", "10000092");
                            }
                            else if (int.Parse(key) == 3167)
                            {
                                task.addGetAndSubNpcKey("10000502", "10000505")
                                .addTarget(4, "太古真魔", "在紫阴中枢消灭6个暴怒尸鬼", "10000505", "fb_50_5", 0, 6, 1);
                                task.bindCondition(50, "3166");
                                task.addTargetPos("m_30", "10000092");
                            }
                            else if (int.Parse(key) == 3168)
                            {
                                task.addGetAndSubNpcKey("10000505", "10000092")
                                .addTarget(2, "真魔之核", "击败太古真魔", "10000505", "fb_50_6", 0, 1, 1)
                                 .addTarget(0, "交予", "将魔核交于古城守卫", "10000092", "10000092", 0, 1);
                                task.bindCondition(50, "3167");
                                task.addTargetPos("m_30", "10000092");
                            }
                        }
                        else if (int.Parse(key) >= 3169 && int.Parse(key) < 3175)
                        {//60副本（4npc，4任务）
                            task = new task(key, 0, "万魂沟壑");
                            if (int.Parse(key) == 3169)
                            {
                                task.addGetAndSubNpcKey("10000145", "10000508")
                                .addTarget(4, "万魂煞气", "消灭6个亡灵刀兵", "10000508", "fb_60_1", 0, 6, 1);
                                task.bindCondition(60, null);
                                task.addTargetPos("m_41", "10000145");
                            }
                            else if (int.Parse(key) == 3170)
                            {
                                task.addGetAndSubNpcKey("10000508", "10000508")
                                .addTarget(2, "亡灵裨将", "击败亡灵裨将", "10000508", "fb_60_3", 0, 1, 1);
                                task.bindCondition(60, "3169");
                                task.addTargetPos("m_41", "10000145");
                            }
                            else if (int.Parse(key) == 3171)
                            {
                                task.addGetAndSubNpcKey("10000508", "10000508")
                                .addTarget(4, "亡灵戟兵", "消灭6个亡灵戟兵", "10000508", "fb_60_2", 0, 6, 1);
                                task.bindCondition(60, "3170");
                                task.addTargetPos("m_41", "10000145");
                            }
                            else if (int.Parse(key) == 3172)
                            {
                                task.addGetAndSubNpcKey("10000508", "10000511")
                                .addTarget(4, "深仇大恨", "消灭6个亡灵斧兵", "10000511", "fb_60_5", 0, 6, 1);
                                task.bindCondition(60, "3171");
                                task.addTargetPos("m_41", "10000145");
                            }
                            else if (int.Parse(key) == 3173)
                            {
                                task.addGetAndSubNpcKey("10000511", "10000511")
                                .addTarget(2, "亡灵统帅", "击败亡灵统帅", "10000511", "fb_60_6", 0, 1, 1);
                                task.bindCondition(60, "3172");
                                task.addTargetPos("m_41", "10000145");
                            }
                            else if (int.Parse(key) == 3174)
                            {
                                task.addGetAndSubNpcKey("10000511", "10000145")
                                .addTarget(0, "魔煞帅印", "将魔煞帅印交给镇守士兵", "10000145", "10000145", 0, 1);
                                task.bindCondition(60, "3173");
                                task.addTargetPos("m_41", "10000145");
                            }
                        }
                        else if (int.Parse(key) >= 3175 && int.Parse(key) < 3184)
                        {//70副本（4npc，4任务）
                            task = new task(key, 0, "隐月幽谷");
                            if (int.Parse(key) == 3175)
                            {
                                task.addGetAndSubNpcKey("10000205", "10000513")
                                .addTarget(4, "阴阳失衡", "去猛兽之林消灭6个巨山熊后去见守谷老人", "10000513", "fb_70_1", 0, 6, 1);
                                task.bindCondition(70, null);
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3176)
                            {
                                task.addGetAndSubNpcKey("10000513", "10000513")
                                .addTarget(2, "守谷老人", "击败守谷老人", "10000513", "fb_70_2", 0, 1, 1);
                                task.bindCondition(70, "3175");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3177)
                            {
                                task.addGetAndSubNpcKey("10000513", "10000514")
                                .addTarget(0, "隐月之宗", "去清风幽径见隐月宗二弟子", "10000514", "10000514", 0, 1);
                                task.bindCondition(70, "3176");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3178)
                            {
                                task.addGetAndSubNpcKey("10000514", "10000514")
                                .addTarget(2, "力敌次徒", "击败隐月宗二弟子", "10000514", "fb_70_3", 0, 1, 1);
                                task.bindCondition(70, "3177");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3179)
                            {
                                task.addGetAndSubNpcKey("10000514", "10000516")
                                .addTarget(0, "隐月首徒", "去兽王古道见隐月宗大弟子", "10000516", "10000516", 0, 1);
                                task.bindCondition(70, "3178");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3180)
                            {
                                task.addGetAndSubNpcKey("10000516", "10000516")
                                .addTarget(2, "合击之术", "击败隐月宗大弟子", "10000516", "fb_70_5", 0, 1, 1);
                                task.bindCondition(70, "3179");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3181)
                            {
                                task.addGetAndSubNpcKey("10000516", "10000517")
                                .addTarget(0, "隐月宗主", "去太阴奇境见月尘子", "10000517", "10000517", 0, 1);
                                task.bindCondition(70, "3180");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3182)
                            {
                                task.addGetAndSubNpcKey("10000517", "10000517")
                                .addTarget(2, "月尘化身", "击败月尘化身", "10000517", "fb_70_7", 0, 1, 1);
                                task.bindCondition(70, "3181");
                                task.addTargetPos("m_52", "10000205");
                            }
                            else if (int.Parse(key) == 3183)
                            {
                                task.addGetAndSubNpcKey("10000517", "10000205")
                                .addTarget(0, "太阴之精", "将太阴之精交给安邑的诸葛玄衣", "10000205", "10000205", 0, 1);
                                task.bindCondition(70, "3182");
                                task.addTargetPos("m_52", "10000205");
                            }
                        }
                        else if (int.Parse(key) >= 3184 && int.Parse(key) < 3189)
                        {//80副本（4npc，4任务）
                            task = new task(key, 0, "往生殿");
                            if (int.Parse(key) == 3184)
                            {
                                task.addGetAndSubNpcKey("10000273", "10000521")
                                .addTarget(4, "往世玄碑", "前往古道击败6个前世之灵再去来生古道见三生兽", "10000521", "fb_80_1", 0, 6, 1);
                                task.bindCondition(80, null);
                                task.addTargetPos("m_68", "10000273");
                            }
                            else if (int.Parse(key) == 3185)
                            {
                                task.addGetAndSubNpcKey("10000521", "10000521")
                                .addTarget(4, "今生之魂", "去今生古道击败6个今生之魂再去见三生兽", "10000521", "fb_80_2", 0, 6, 1);
                                task.bindCondition(80, "3184");
                                task.addTargetPos("m_68", "10000273");
                            }
                            else if (int.Parse(key) == 3186)
                            {
                                task.addGetAndSubNpcKey("10000521", "10000521")
                                .addTarget(2, "三生奇兽", "击败三生兽", "10000521", "fb_80_4", 0, 1, 1);
                                task.bindCondition(80, "3185");
                                task.addTargetPos("m_68", "10000273");
                            }
                            else if (int.Parse(key) == 3187)
                            {
                                task.addGetAndSubNpcKey("10000521", "10000522")
                                .addTarget(4, "三生轮回", "击败6个来世之魄再去轮回古道一窥三生轮回碑", "10000522", "fb_80_3", 0, 6, 1);
                                task.bindCondition(80, "3186");
                                task.addTargetPos("m_68", "10000273");
                            }
                            else if (int.Parse(key) == 3188)
                            {
                                task.addGetAndSubNpcKey("10000522", "10000522")
                                .addTarget(2, "轮回奥义", "击败前世、今生、来世", "10000522", "fb_80_7", 0, 1, 1);
                                task.bindCondition(80, "3187");
                                task.addTargetPos("m_68", "10000273");
                            }

                        }
                        else if (int.Parse(key) >= 3189 && int.Parse(key) < 3196)
                        {//90副本（4npc，4任务）
                            task = new task(key, 0, "青丘境");
                            if (int.Parse(key) == 3189)
                            {
                                task.addGetAndSubNpcKey("10000319", "10000525")
                                .addTarget(4, "神器失踪", "到青丘入口击杀青丘之灵后，与灵隐绝境的瑞南羽对话", "10000525", "fb_90_1", 0, 6, 1);
                                task.bindCondition(90, null);
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3190)
                            {
                                task.addGetAndSubNpcKey("10000525", "10000525")
                                .addTarget(2, "怪盗之踪", "击败瑞南羽", "10000525", "fb_90_3", 0, 1, 1);
                                task.bindCondition(90, "3189");
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3191)
                            {
                                task.addGetAndSubNpcKey("10000525", "10000525")
                                .addTarget(4, "怪盗脱困", "到青丘入口击杀6个青丘之灵后与灵隐绝境的瑞南羽对话", "10000525", "fb_90_1", 0, 6, 1);
                                task.bindCondition(90, "3190");
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3192)
                            {
                                task.addGetAndSubNpcKey("10000525", "10000525")
                                .addTarget(4, "引出九尾", "到禁忌古道杀死九尾幻影后，与瑞南羽对话", "10000525", "fb_90_4", 0, 6, 1);
                                task.bindCondition(90, "3191");
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3193)
                            {
                                task.addGetAndSubNpcKey("10000525", "10000528")
                                .addTarget(0, "九尾所在", "找到迷影禁地的九尾异兽", "10000528", "10000528", 0, 1);
                                task.bindCondition(90, "3192");
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3194)
                            {
                                task.addGetAndSubNpcKey("10000528", "10000528")
                                .addTarget(2, "异兽悲鸣", "击败九尾异兽", "10000528", "fb_90_6", 0, 1, 1);
                                task.bindCondition(90, "3193");
                                task.addTargetPos("m_77", "10000319");
                            }
                            else if (int.Parse(key) == 3195)
                            {
                                task.addGetAndSubNpcKey("10000528", "10000528")
                                .addTarget(0, "封印青丘", "将四象之灵交给九尾异兽", "10000528", "10000528", 0, 1, 1);
                                task.bindCondition(90, "3194");
                                task.addTargetPos("m_77", "10000319");
                            }

                        }
                        else if (int.Parse(key) >= 3196 && int.Parse(key) < 3203)
                        {//100副本（4npc，4任务）
                            task = new task(key, 0, "混沌邪灵渊");
                            if (int.Parse(key) == 3196)
                            {
                                task.addGetAndSubNpcKey("10000427", "10000530")
                                .addTarget(4, "初进洞渊", "消灭恶灵获取6份灵精石交给裂影渊的洞渊战魂", "10000530", "fb_100_1", 0, 6, 1);
                                task.bindCondition(100, null);
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3197)
                            {
                                task.addGetAndSubNpcKey("10000530", "10000530")
                                .addTarget(2, "好战之魂", "击败洞渊战魂", "10000530", "fb_100_2", 0, 1, 1);
                                task.bindCondition(100, "3196");
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3198)
                            {
                                task.addGetAndSubNpcKey("10000530", "10000532")
                                .addTarget(4, "得遇鬼王", "消灭魔影取6份影魂石给陨仙渊的百鬼之王", "10000532", "fb_100_4", 0, 6, 1);
                                task.bindCondition(100, "3197");
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3199)
                            {
                                task.addGetAndSubNpcKey("10000532", "10000532")
                                .addTarget(4, "鬼王考验", "杀死6个魔影，然后与百鬼之王对话", "10000532", "fb_100_4", 0, 6, 1);
                                task.bindCondition(100, "3198");
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3200)
                            {
                                task.addGetAndSubNpcKey("10000532", "10000532")
                                .addTarget(2, "与鬼一战", "击败百鬼之王", "10000532", "fb_100_5", 0, 1, 1);
                                task.bindCondition(100, "3199");
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3201)
                            {
                                task.addGetAndSubNpcKey("10000532", "10000532")
                                .addTarget(0, "侠客之迹", "与百鬼之王继续对话", "10000532", "10000532", 0, 1);
                                task.bindCondition(100, "3200");
                                task.addTargetPos("m_97", "10000427");
                            }
                            else if (int.Parse(key) == 3202)
                            {
                                task.addGetAndSubNpcKey("10000532", "10000427")
                                .addTarget(0, "道者之悔", "回阳谷与修道者对话", "10000427", "10000427", 0, 1);
                                task.bindCondition(100, "3201");
                                task.addTargetPos("m_97", "10000427");
                            }

                        }
                        else if (int.Parse(key) >= 3203 && int.Parse(key) < 3213)
                        {//100副本（4npc，4任务）
                            task = new task(key, 0, "赤炼洞窟");
                            if (int.Parse(key) == 3203)
                            {
                                task.addGetAndSubNpcKey("10000379", "10000534")
                                .addTarget(0, "衙门之托", "进乱葬废墟找生还的人", "10000534", "10000534", 0, 1);
                                task.bindCondition(95, null);
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3204)
                            {
                                task.addGetAndSubNpcKey("10000534", "10000534")
                                .addTarget(4, "众妖围袭", "击杀乱葬废墟的煞阴尸鬼", "10000534", "fb_ls_1", 0, 6, 1);
                                task.bindCondition(95, "3203");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3205)
                            {
                                task.addGetAndSubNpcKey("10000534", "10000536")
                                .addTarget(0, "尸葬地窟", "进入虐杀之地探索", "10000536", "10000536", 0, 1);
                                task.bindCondition(95, "3204");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3206)
                            {
                                task.addGetAndSubNpcKey("10000536", "10000536")
                                .addTarget(2, "虐杀之鬼", "击败虐杀之鬼", "10000536", "fb_ls_2", 0, 1, 1);
                                task.bindCondition(95, "3205");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3207)
                            {
                                task.addGetAndSubNpcKey("10000536", "10000534")
                                .addTarget(0, "生之欺骗", "回去乱葬废墟质问小女孩", "10000534", "10000534", 0, 1);
                                task.bindCondition(95, "3206");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3208)
                            {
                                task.addGetAndSubNpcKey("10000534", "10000537")
                                .addTarget(0, "黑衣之影", "去枯魂阴牢找黑衣人", "10000537", "10000537", 0, 1);
                                task.bindCondition(95, "3207");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3209)
                            {
                                task.addGetAndSubNpcKey("10000537", "10000538")
                                .addTarget(4, "暗影之兄", "消灭幽蓝狼魔后与小男孩对话", "10000538", "fb_ls_3", 0, 6, 1);
                                task.bindCondition(95, "3208");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3210)
                            {
                                task.addGetAndSubNpcKey("10000538", "10000540")
                                .addTarget(0, "暗影杀手", "去赤炼血池找暗影杀手", "10000540", "10000540", 0, 1);
                                task.bindCondition(95, "3209");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3211)
                            {
                                task.addGetAndSubNpcKey("10000540", "10000540")
                                .addTarget(2, "那位仙人", "击败暗影杀手", "10000540", "fb_ls_4", 0, 1, 1);
                                task.bindCondition(95, "3210");
                                task.addTargetPos("m_85", "10000379");
                            }
                            else if (int.Parse(key) == 3212)
                            {
                                task.addGetAndSubNpcKey("10000540", "10000379")
                                .addTarget(0, "戏之谢幕", "回白骨洞找衙门捕头", "10000379", "10000379", 0, 1);
                                task.bindCondition(95, "3211");
                                task.addTargetPos("m_85", "10000379");
                            }
                        }
                        else if (int.Parse(key) >= 3213 && int.Parse(key) < 3221)
                        {
                            task = new task(key, 0, "浮游城");
                            if (int.Parse(key) == 3213)
                            {
                                task.addGetAndSubNpcKey("10000399", "10000541")
                                .addTarget(0, "游浮之城", "进入虚幻之地找红衣女", "10000541", "10000541", 0, 1);
                                task.bindCondition(95, null);
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3214)
                            {
                                task.addGetAndSubNpcKey("10000541", "10000543")
                                .addTarget(4, "凶残角牛", "击杀巨角魔牛取得牛角后给夫人", "10000543", "fb_hh_1", 0, 6, 1);
                                task.bindCondition(95, "3213");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3215)
                            {
                                task.addGetAndSubNpcKey("10000543", "10000544")
                                .addTarget(4, "震岳巨熊", "去回忆之地斩杀震岳巨熊后和红衣女对话", "10000544", "fb_hh_2", 0, 6, 1);
                                task.bindCondition(95, "3214");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3216)
                            {
                                task.addGetAndSubNpcKey("10000544", "10000546")
                                .addTarget(0, "灭村之祸", "去痛苦之境看看发生了什么事", "10000546", "10000546", 0, 1);
                                task.bindCondition(95, "3215");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3217)
                            {
                                task.addGetAndSubNpcKey("10000546", "10000546")
                                .addTarget(2, "弑亲烧庄", "击败灭城将军", "10000546", "fb_hh_3", 0, 1, 1);
                                task.bindCondition(95, "3216");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3218)
                            {
                                task.addGetAndSubNpcKey("10000546", "10000547")
                                .addTarget(0, "大仇未报", "安慰红衣女", "10000547", "10000547", 0, 1);
                                task.bindCondition(95, "3217");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3219)
                            {
                                task.addGetAndSubNpcKey("10000547", "10000547")
                                .addTarget(2, "红衣之恨", "击败心魔", "10000547", "fb_hh_4", 0, 1, 1);
                                task.bindCondition(95, "3218");
                                task.addTargetPos("m_89", "10000399");
                            }
                            else if (int.Parse(key) == 3220)
                            {
                                task.addGetAndSubNpcKey("10000547", "10000399")
                                .addTarget(0, "解决之道", "回去鸿门找玄机子", "10000399", "10000399", 0, 1);
                                task.bindCondition(95, "3219");
                                task.addTargetPos("m_89", "10000399");
                            }
                        }
                        else if (int.Parse(key) >= 3221 && int.Parse(key) < 3229)
                        {
                            task = new task(key, 0, "隐龙古城");
                            if (int.Parse(key) == 3221)
                            {
                                task.addGetAndSubNpcKey("10000413", "10000548")
                                .addTarget(4, "古城故事", "进去焚火废墟斩杀黑煞木妖取木之灵气后寻找里面的人", "10000548", "fb_ys_1", 0, 6, 1);
                                task.bindCondition(100, null);
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3222)
                            {
                                task.addGetAndSubNpcKey("10000548", "10000548")
                                .addTarget(4, "国之无君", "斩杀黑煞木妖取枝头露水后交给说书人", "10000548", "fb_ys_1", 0, 6, 1);
                                task.bindCondition(100, "3221");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3223)
                            {
                                task.addGetAndSubNpcKey("10000548", "10000550")
                                .addTarget(0, "情之一字", "去龙隐秘地找龙女", "10000550", "10000550", 0, 1);
                                task.bindCondition(100, "3222");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3224)
                            {
                                task.addGetAndSubNpcKey("10000550", "10000550")
                                .addTarget(2, "龙女君皇", "击败龙女", "10000550", "fb_ys_3", 0, 1, 1);
                                task.bindCondition(100, "3223");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3225)
                            {
                                task.addGetAndSubNpcKey("10000550", "10000552")
                                .addTarget(0, "欺骗之事", "去龙啸古地找通天眼", "10000552", "10000552", 0, 1);
                                task.bindCondition(95, "3224");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3226)
                            {
                                task.addGetAndSubNpcKey("10000552", "10000552")
                                .addTarget(4, "笑话天命", "斩杀苍岚狮鹫后质问通天眼", "10000552", "fb_ys_2", 0, 6, 1);
                                task.bindCondition(100, "3225");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3227)
                            {
                                task.addGetAndSubNpcKey("10000552", "10000552")
                                .addTarget(2, "阻止毁灭", "击败通天眼", "10000552", "fb_ys_5", 0, 1, 1);
                                task.bindCondition(100, "3226");
                                task.addTargetPos("m_94", "10000413");
                            }
                            else if (int.Parse(key) == 3228)
                            {
                                task.addGetAndSubNpcKey("10000552", "10000413")
                                .addTarget(0, "天星子大计", "回去子虚林找古城遗民", "10000413", "10000413", 0, 1);
                                task.bindCondition(100, "3227");
                                task.addTargetPos("m_94", "10000413");
                            }
                        }
                        else if (int.Parse(key) >= 3229 && int.Parse(key) < 3235)
                        {
                            task = new task(key, 0, "魔界之门");
                            if (int.Parse(key) == 3229)
                            {
                                task.addGetAndSubNpcKey("10000454", "10000554")
                                .addTarget(0, "天下何归", "进去焚骨熔岩找心魔", "10000554", "10000554", 0, 1);
                                task.bindCondition(100, null);
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3230)
                            {
                                task.addGetAndSubNpcKey("10000554", "10000554")
                                .addTarget(4, "掩盖人气", "击杀冥罗妖取彼岸花给心魔", "10000554", "fb_zx_1", 0, 6, 1);
                                task.bindCondition(100, "3229");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3231)
                            {
                                task.addGetAndSubNpcKey("10000554", "10000556")
                                .addTarget(0, "心魔之助", "去鬼爪炼狱找蜃兽", "10000556", "10000556", 0, 1);
                                task.bindCondition(100, "3230");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3232)
                            {
                                task.addGetAndSubNpcKey("10000556", "10000556")
                                .addTarget(2, "鬼爪炼狱", "击败蜃兽", "10000556", "fb_zx_3", 0, 1, 1);
                                task.bindCondition(100, "3231");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3233)
                            {
                                task.addGetAndSubNpcKey("10000556", "10000558")
                                .addTarget(4, "炼魂祭坛", "击杀熔骨尸煞后去炼魂祭坛找天星子", "10000558", "fb_zx_2", 0, 6, 1);
                                task.bindCondition(95, "3232");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3234)
                            {
                                task.addGetAndSubNpcKey("10000558", "10000558")
                                .addTarget(2, "阻止天星子", "击败魔化天星子", "10000558", "fb_zx_4", 0, 1, 1);
                                task.bindCondition(100, "3233");
                                task.addTargetPos("m_101", "10000454");
                            }
                        }
                        else if (int.Parse(key) >= 3235 && int.Parse(key) < 3242)
                        {
                            task = new task(key, 0, "隐藏");
                            if (int.Parse(key) == 3235)
                            {
                                task.addGetAndSubNpcKey("10000555", "10000559")
                                .addTarget(0, "冥冥天意", "进入云之境与饕餮对话", "10000559", "10000559", 0, 1);
                                task.bindCondition(100, null);
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3236)
                            {
                                task.addGetAndSubNpcKey("10000559", "10000559")
                                .addTarget(2, "击败饕餮", "击败饕餮", "10000559", "fb_yzj_2", 0, 1, 1);
                                task.bindCondition(100, "3235");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3237)
                            {
                                task.addGetAndSubNpcKey("10000559", "10000561")
                                .addTarget(0, "往幻之境", "去幻之境与梼杌对话", "10000561", "10000561", 0, 1);
                                task.bindCondition(100, "3236");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3238)
                            {
                                task.addGetAndSubNpcKey("10000561", "10000561")
                                .addTarget(2, "击败梼杌", "击败梼杌", "10000561", "fb_yzj_4", 0, 1, 1);
                                task.bindCondition(100, "3237");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3239)
                            {
                                task.addGetAndSubNpcKey("10000561", "10000563")
                                .addTarget(0, "天机老人", "去仙之境找天机老人", "10000563", "10000563", 0, 1);
                                task.bindCondition(100, "3238");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3240)
                            {
                                task.addGetAndSubNpcKey("10000563", "10000563")
                                .addTarget(2, "辩道", "击败天机老人", "10000563", "fb_yzj_8", 0, 1, 1);
                                task.bindCondition(100, "3239");
                                task.addTargetPos("m_101", "10000454");
                            }
                            else if (int.Parse(key) == 3241)
                            {
                                task.addGetAndSubNpcKey("10000563", "10000563")
                                .addTarget(0, "结束", "与天机老人对话", "10000563", "10000563", 0, 1);
                                task.bindCondition(100, "3240");
                                task.addTargetPos("m_101", "10000454");
                            }


                        }
                        else if (int.Parse(key) >= 3250 && int.Parse(key) < 3265)
                        {//周日门派
                            task = new task(key, 0);
                            if (int.Parse(key) == 3250)
                            {
                                task.addGetAndSubNpcKey("10000565", "10000565")
                                .addTarget(0, "神秘老朽", "与神秘老朽继续交谈", "10000565", "10000565", 0, 1);
                                task.bindCondition(30, "-1");
                            }
                            else if (int.Parse(key) == 3251)
                            {
                                task.addGetAndSubNpcKey("10000565", "10000459")
                                .addTarget(0, "挑战遁甲堂", "前往墨家门派与遁甲堂主交谈", "10000459", "10000459", 0, 1);
                                task.bindCondition(30, "3250");
                            }
                            else if (int.Parse(key) == 3252)
                            {
                                task.addGetAndSubNpcKey("10000459", "10000459")
                                .addTarget(2, "玄甲神卫", "击败玄甲神卫", "10000459", "ztzs_1", 0, 1, 1);
                                task.bindCondition(30, "3251");// 1227 1014*5
                            }
                            else if (int.Parse(key) == 3253)
                            {
                                task.addGetAndSubNpcKey("10000464", "10000464")
                                .addTarget(0, "挑战琴魔堂", "前往道家门派与琴魔堂主交谈", "10000464", "10000464", 0, 1);
                                task.bindCondition(30, "3252");
                            }
                            else if (int.Parse(key) == 3254)
                            {
                                task.addGetAndSubNpcKey("10000464", "10000464")
                                .addTarget(2, "落音仙灵", "击败落音仙灵", "10000464", "ztzs_2", 0, 1, 1);
                                task.bindCondition(30, "3253");// 1217 1014*5
                            }
                            else if (int.Parse(key) == 3255)
                            {
                                task.addGetAndSubNpcKey("10000471", "10000471")
                                .addTarget(0, "挑战幽冥堂", "前往阴阳家门派与幽冥堂主交谈", "10000471", "10000471", 0, 1);
                                task.bindCondition(30, "3254");
                            }
                            else if (int.Parse(key) == 3256)
                            {
                                task.addGetAndSubNpcKey("10000471", "10000471")
                                .addTarget(2, "幽冥剑灵", "击败幽冥剑灵", "10000471", "ztzs_3", 0, 1, 1);
                                task.bindCondition(30, "3255");// 60005 1014*5
                            }
                            else if (int.Parse(key) == 3257)
                            {
                                task.addGetAndSubNpcKey("10000458", "10000458")
                                .addTarget(0, "挑战猛士堂", "前往墨家门派与猛士堂主交谈", "10000458", "10000458", 0, 1);
                                task.bindCondition(30, "3256");
                            }
                            else if (int.Parse(key) == 3258)
                            {
                                task.addGetAndSubNpcKey("10000458", "10000458")
                                .addTarget(2, "破天剑客", "击败破天剑客", "10000458", "ztzs_4", 0, 1, 1);
                                task.bindCondition(30, "3257");// 1214 1014*5
                            }
                            else if (int.Parse(key) == 3259)
                            {
                                task.addGetAndSubNpcKey("10000465", "10000465")
                                .addTarget(0, "挑战天音堂", "前往道家门派与天音堂主交谈", "10000465", "10000465", 0, 1);
                                task.bindCondition(30, "3258");
                            }
                            else if (int.Parse(key) == 3260)
                            {
                                task.addGetAndSubNpcKey("10000465", "10000465")
                                .addTarget(2, "玉弦仙灵", "击败玉弦仙灵", "10000465", "ztzs_5", 0, 1, 1);
                                task.bindCondition(30, "3259");// 1215 1014*5
                            }
                            else if (int.Parse(key) == 3261)
                            {
                                task.addGetAndSubNpcKey("10000470", "10000470")
                                .addTarget(0, "挑战罗刹堂", "前往阴阳家门派与罗刹堂主交谈", "10000470", "10000470", 0, 1);
                                task.bindCondition(30, "3260");
                            }
                            else if (int.Parse(key) == 3262)
                            {
                                task.addGetAndSubNpcKey("10000470", "10000470")
                                .addTarget(2, "玄冥紫魂", "击败玄冥紫魂", "10000470", "ztzs_6", 0, 1, 1);
                                task.bindCondition(30, "3261");// 35932 1014*5
                            }
                            else if (int.Parse(key) == 3263)
                            {
                                task.addGetAndSubNpcKey("10000565", "10000565")
                                .addTarget(0, "封印将解", "前往邯郸活动区与神秘老朽交谈", "10000565", "10000565", 0, 1);
                                task.bindCondition(30, "3262");
                            }
                            else if (int.Parse(key) == 3264)
                            {
                                task.addGetAndSubNpcKey("10000565", "10000565")
                                .addTarget(2, "震天战神", "击败震天战神", "10000565", "ztzs_7", 0, 1, 1);
                                task.bindCondition(30, "3263");// bs03 1014*5
                            }


                            task.addTargetPos("m_7", "2054");
                        }
                        else if (int.Parse(key) >= 3265 && int.Parse(key) < 3272)
                        {//魔神任务
                            task = new task(key, 0, "魔神窟日常");
                            if (int.Parse(key) == 3265)
                            {
                                task.addGetAndSubNpcKey("10000492", "10000492")
                                .addTarget(4, "狂攻之怒", "击败狂攻之护卫", "10000492", "mshuwei_1", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3266)
                            {
                                task.addGetAndSubNpcKey("10000493", "10000493")
                                .addTarget(4, "铁壁之怒", "击败铁壁之护卫", "10000493", "mshuwei_2", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3267)
                            {
                                task.addGetAndSubNpcKey("10000494", "10000494")
                                .addTarget(4, "生命之怒", "击败生命之护卫", "10000494", "mshuwei_3", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3268)
                            {
                                task.addGetAndSubNpcKey("10000495", "10000495")
                                .addTarget(4, "神速之怒", "击败神速之护卫", "10000495", "mshuwei_4", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3269)
                            {
                                task.addGetAndSubNpcKey("10000496", "10000496")
                                .addTarget(4, "射手之怒", "击败射手之护卫", "10000496", "mshuwei_5", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3270)
                            {
                                task.addGetAndSubNpcKey("10000497", "10000497")
                                .addTarget(4, "法术之怒", "击败法术之护卫", "10000497", "mshuwei_6", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }
                            else if (int.Parse(key) == 3271)
                            {
                                task.addGetAndSubNpcKey("10000498", "10000498")
                                .addTarget(4, "暴怒之怒", "击败暴怒之护卫", "10000498", "mshuwei_7", 0, 1, 1);
                                task.bindCondition(60, "-1");
                            }



                            task.addTargetPos("m_7", "2054");
                        }
                        else if (int.Parse(key) == 3272)
                        {
                            task = new task(key, 0, "锄奸卫道");
                            task.addGetAndSubNpcKey("10000048", "10000048")
                                    .addTarget(2, "捉拿奸细", "击败奸细", "-1", "jianxi_1", 0, 1, 1);
                            task.bindCondition(80, "-1");
                            task.addTargetPos("m_9", "10000048");
                        }
                        else if (int.Parse(key) == 3273)
                        {
                            task = new task(key, 0, "仗剑除魔");
                            task.addGetAndSubNpcKey("10000047", "10000047")
                                    .addTarget(4, "清理吸魂小妖", "击败吸魂小妖", "-1", "xhxy_1", 0, 1, 1);
                            task.bindCondition(50, "-1");
                            task.addTargetPos("m_9", "10000047");
                        }
                        else if (int.Parse(key) == 3274)
                        {
                            task = new task(key, 0, "洪荒宝库");
                            task.addGetAndSubNpcKey("10000610", "10000610")
                                    .addTarget(2, "最后的挑战", "击败龙神后裔", "10000610", "hhbk_3", 0, 1, 1);
                            task.bindCondition(1, "-1");
                            task.addTargetPos("hhbk_3", "10000610");
                        }
                        else if (int.Parse(key) >= 3275 && int.Parse(key) < 3282)//盗梦空间
                        {
                            task = new task(key, 0, "盗梦空间");
                            if (int.Parse(key) == 3275)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                                .addTarget(4, "Lv70-傲慢之罪", "收集破天巨熊", "10000613", "dmkj_1", 0, 399, 1);
                                task.bindCondition(70, null);
                                task.addTargetPos("dmkj1", "10000613");
                            }
                            else if (int.Parse(key) == 3276)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                               .addTarget(4, "Lv75-妒忌之罪", "收集隐梦云豹", "10000613", "dmkj_2", 0, 399, 1);
                                task.bindCondition(75, null);
                                task.addTargetPos("dmkj2", "10000613");
                            }
                            else if (int.Parse(key) == 3277)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                               .addTarget(4, "Lv80-暴怒之罪", "收集噬梦虫", "10000613", "dmkj_3", 0, 399, 1);
                                task.bindCondition(80, null);
                                task.addTargetPos("dmkj3", "10000613");
                            }
                            else if (int.Parse(key) == 3278)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                                .addTarget(4, "Lv85-懒惰之罪", "收集化梦鬼木", "10000613", "dmkj_4", 0, 399, 1);
                                task.bindCondition(85, null);
                                task.addTargetPos("dmkj4", "10000613");
                            }
                            else if (int.Parse(key) == 3279)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                                .addTarget(4, "Lv90-贪婪之罪", "收集入梦双蛇", "10000613", "dmkj_5", 0, 399, 1);
                                task.bindCondition(90, null);
                                task.addTargetPos("dmkj5", "10000613");
                            }
                            else if (int.Parse(key) == 3280)
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                                .addTarget(4, "Lv95-色欲之罪", "收集赤痕梦蛛", "10000613", "dmkj_6", 0, 399, 1);
                                task.bindCondition(95, null);
                                task.addTargetPos("dmkj6", "10000613");
                            }
                            else if (int.Parse(key) == 3281)//100级经验 212400
                            {
                                task.addGetAndSubNpcKey("10000613", "10000613")
                                .addTarget(4, "Lv100-暴食之罪", "收集吞梦妖龙", "10000613", "dmkj_7", 0, 399, 1);
                                task.bindCondition(100, null);
                                task.addTargetPos("dmkj7", "10000613");
                            }
                        }
                        else if (int.Parse(key) == 3282)
                        {
                            task = new task(key, 0, "采阴补阳");
                            task.addGetAndSubNpcKey("10000033", "10000033")
                                    .addTarget(2, "天狐精元", "击败狐萌萌", "10000033", "cyby_1", 0, 1, 1);
                            task.bindCondition(30, "-1");
                            task.addTargetPos("m_30", "cyby_1");
                        }
                        else if (int.Parse(key) >= 3283 && int.Parse(key) < 3288)
                        {
                            task = new task(key, 0, "监狱风云");
                            if (int.Parse(key) == 3283)
                            {
                                task.addGetAndSubNpcKey("10000033", "10000033")
                                .addTarget(4, "缉拿太二真人", "击败太二真人", "10000033", "jyfy_1", 0, 1, 1);
                                task.bindCondition(30, "-1");
                                task.addTargetPos("m_12", "jyfy_1");
                            }
                            else if (int.Parse(key) == 3284)
                            {
                                task.addGetAndSubNpcKey("10000033", "10000033")
                               .addTarget(4, "缉拿西门好色", "击败西门好色", "10000033", "jyfy_2", 0, 1, 1);
                                task.bindCondition(30, "-1");
                                task.addTargetPos("m_5", "jyfy_2");
                            }
                            else if (int.Parse(key) == 3285)
                            {
                                task.addGetAndSubNpcKey("10000033", "10000033")
                               .addTarget(4, "缉拿鲁光光", "击败鲁光光", "10000033", "jyfy_3", 0, 1, 1);
                                task.bindCondition(30, "-1");
                                task.addTargetPos("m_4", "jyfy_3");
                            }
                            else if (int.Parse(key) == 3286)
                            {
                                task.addGetAndSubNpcKey("10000033", "10000033")
                                .addTarget(4, "缉拿东方必败", "击败东方必败", "10000033", "jyfy_4", 0, 1, 1);
                                task.bindCondition(30, "-1");
                                task.addTargetPos("m_10", "jyfy_4");
                            }
                            else if (int.Parse(key) == 3287)
                            {
                                task.addGetAndSubNpcKey("10000033", "10000033")
                                .addTarget(4, "缉拿完颜失色", "击败完颜失色", "10000033", "jyfy_5", 0, 1, 1);
                                task.bindCondition(30, "-1");
                                task.addTargetPos("m_11", "jyfy_5");
                            }
                        }
                        else if (int.Parse(key) >= 3288 && int.Parse(key) < 3291)//王者遗产任务
                        {
                            task = new task(key, 0, "王者遗产");
                            if (key.Equals("3288"))
                            {
                                task.addGetAndSubNpcKey("10000000", "10000000")
                               .addTarget(0, "王者遗产", "与<color=#1b8100>村长</color>交谈", "10000000", "10000000", 0, 1)
                               .addTargetPos("m_1", "10000000")
                               .bindCondition(1, "-1");
                            }
                            else if (key.Equals("3289"))
                            {
                                task.addGetAndSubNpcKey("10000021", "10000021")
                                .addTarget(2, "王者遗产", "与<color=#1b8100>藏边小丑</color>战斗", "10000021", "wzyc", 0, 1, 1)
                                .addTargetPos("m_6", "10000021")
                                .bindCondition(1, "-1");
                            }
                            else if (key.Equals("3290"))
                            {
                                task.addGetAndSubNpcKey("10000021", "10000021")
                                .addTarget(4, "王者遗产", "清理<color=#1b8100>10只食尸虫</color>", "10000021", "1004", 1, 10, 1)
                                .addTargetPos("m_4", "-1")
                                .bindCondition(1, "-1");
                            }
                        }
                        break;
                    }
            }
            return task;
        }



    }
}
