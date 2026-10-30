
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    /**线程、任务处理*/
    public class timeManager : MonoBehaviour
    {
        private List<threadTask> taskList = new List<threadTask>();
        public static timeManager getTimeManageOne()
        {
            return GameObject.Find("main").GetComponent<timeManager>();
        }
        //是否正在执行
        private bool isDoing = false;
        void Start()
        {
            StartCoroutine(ExecuteEverySecond());
        }
        IEnumerator ExecuteEverySecond()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                //未处于执行状态时方可调用
                if (!isDoing)
                {
                    isDoing = true;
                    try
                    {
                        //先把标记删除的给移除
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            if (taskList[i].isDel)
                            {
                                taskList.RemoveAt(i);
                                i--;
                            }
                        }
                        //取一段长度，因为执行过程中可能有新的任务加入
                        int len = taskList.Count;
                        for (int i = 0; i < len; i++)
                        {
                            taskList[i].doCall();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.StackTrace);
                    }
                    finally
                    {
                        isDoing = false;
                    }
                }
            }
        }
        //1/30秒调用一次
       /* void Update()
        {
            //未处于执行状态时方可调用
            if (!isDoing)
            {
                isDoing = true;
                try
                {
                    //先把标记删除的给移除
                    for (int i = 0; i < taskList.Count; i++)
                    {
                        if (taskList[i].isDel)
                        {
                            taskList.RemoveAt(i);
                            i--;
                        }
                    }
                    //取一段长度，因为执行过程中可能有新的任务加入
                    int len = taskList.Count;
                    for (int i = 0; i < len; i++)
                    {
                        taskList[i].doCall();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                finally
                {
                    isDoing = false;
                }
            }
        }*/
        public string putThread(Action callback, string des = null)
        {
            threadTask ts = new threadTask(strUtils.getMillis(), callback, 0, 0, des);
            taskList.Add(ts);
            return ts.Id;
        }
        /**延时任务*/
        public string putDelayTask(Action callback, long delay, string des = null)
        {
            threadTask ts = new threadTask(strUtils.getMillis(), callback, 1, delay, des);
            taskList.Add(ts);
            return ts.Id;
        }
        /**周期任务*/
        public string putPeriodTask(Action callback, long delay, string des = null)
        {
            threadTask ts = new threadTask(strUtils.getMillis(), callback, 2, delay, des);
            taskList.Add(ts);
            return ts.Id;
        }
        /**移除任务*/
        public void remTask(string Id)
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].Id == Id)
                {
                    taskList[i].isDel = true;
                    break;
                }
            }
        }
        public void remAllTask()
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                taskList[i].isDel = true;
            }
        }

    }
    public class threadTask
    {
        public string Id;
        public long oldTime;
        public Action callback;
        //0线程1延时任务2周期任务
        public int type;
        //延时 ms
        public long delayTime;
        //是否需要被移除队列
        public bool isDel = false;
        public string des;


        public threadTask(long oldTime, Action callback, int type, long delayTime, string des = null)
        {
            this.Id = strUtils.getId();
            this.oldTime = oldTime;
            this.callback = callback;
            this.type = type;
            this.delayTime = delayTime;
            this.des = des;
            if (des != null)
            {
                Debug.Log(strUtils.getMillis() + des);
            }
        }

        /**执行回调*/
        public void doCall()
        {
            if (isDel) return;
            if (des != null)
            {
                Debug.Log(strUtils.getMillis() + "执行了线程:" + des);
            }
            if (type == 0)
            {
                if (callback != null) callback();
                isDel = true;
            }
            else if (type == 1)
            {
                //判断是否达到指定的时间
                long t = strUtils.getMillis();
                if (t > (oldTime + delayTime))
                {
                    if (callback != null) callback();
                    isDel = true;
                }
            }
            else if (type == 2)
            {
                //判断是否达到指定的时间
                long t = strUtils.getMillis();
                if (t > (oldTime + delayTime))
                {
                    //刷新旧时间
                    oldTime = t;
                    if (callback != null) callback();
                }
            }
        }
    }
}
