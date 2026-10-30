using Assets.HotFix.xq2d.src.faceI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    public class task
    {
        //任务key
        public string key;
        //任务类型 0主线、1支线、2活动、3副本
        public int type;
        //任务名 
        public string name;
        //进度列表
        public JArray progressList;
        //进行到某个进度的下标
        public int progressIndex;
        //活动信息
        public JObject acMsg;
        //开启条件
        public JObject condition;
        //进度目标位置
        public JArray targetPosList;
        //活动跳转的地图信息
        public JObject toMapMsg;
        //奖励
        public JArray rewards;
        //接取时的npc。中间转换npc交由任务的toKey字段完成
        public string getNpcKey;
        //提交时的npc
        public string subNpcKey;

        public task(string key, int type, string name = null)
        {
            this.key = key;
            this.type = type;
            this.name = name;
            this.progressIndex = 0;
            this.progressList = new JArray();
            this.targetPosList = new JArray();
        }
        public task setProgressIndex(JObject cache)
        {
            this.progressIndex = (int)cache["progressIndex"];
            return this;
        }
        public task addGetAndSubNpcKey(string getNpcKey, string subNpcKey)
        {
            this.getNpcKey = getNpcKey;
            this.subNpcKey = subNpcKey;
            return this;
        }
        /**开启条件 lever开启等级、preTaskKey需完成的任务
         * preTaskKey=-1表示存在该任务，但需要菜单上去创建它
         * preTaskKey=null不需要上一个任务作为触发开启
         */
        public void bindCondition(int lever = 1, string preTaskKey = null)
        {
            JObject condition = new JObject();
            condition.Add("lever", lever);//lever是必要条件
            condition.Add("taskKey", preTaskKey);
            this.condition = condition;
        }
        /**添加活动所对应的npc，由npc找出其位置*/
        public task addNpc(string npcKey)
        {
            if (this.acMsg == null)
            {
                Debug.Log("请先设置msg");
                return this;
            }
            this.acMsg.Add("npcKey", npcKey);
            return this;
        }
        /**对于点击菜单项后直接跳转*/
        public task addToMap(string mapKey)
        {
            this.toMapMsg = new JObject();
            this.toMapMsg.Add("mapKey", mapKey);
            return this;
        }
        /**添加活动信息*/
        public task addAcMsg(int times, int jf, int limitLv = 20, int multy = 0, string openTime = "全天（凌晨0点刷新）")
        {
            JObject item = new JObject();
            item.Add("times", times);//-1无限
            item.Add("jf", jf);//活跃度
            item.Add("openTime", openTime);//开放时间
            item.Add("limitLv", limitLv);//限制等级
            item.Add("multy", multy);//0单人1多人
            this.acMsg = item;
            return this;
        }
        /**放置奖励 num=-1为任意*/
        public task addReward(string key, int num)
        {
            if (this.rewards == null) this.rewards = new JArray();
            JObject a = new JObject();
            a.Add("key", key);
            a.Add("num", num);
            this.rewards.Add(a);
            return this;
        }
        /**任务详情*/
        public task addDes(string des = "暂无活动详情")
        {
            this.acMsg.Add("des", des);
            return this;
        }

        /**获取任务目标*/
        public JObject getTarget()
        {
            JObject progress = (JObject)this.progressList[this.progressIndex];
            return (JObject)progress["target"];
        }
        /**获取目标描述*/
        public string getTargetDes()
        {
            JObject t = face.taskInterface.getTaskFromCache(key);
            JObject progress = (JObject)this.progressList[this.progressIndex];
            JObject target = (JObject)progress["target"];
            int targetType = (int)target["targetType"];
            string targetStr = null;
            string targetKey = target["key"].ToString();
            int num = (int)t["taskProgress"]["target"]["num"];
            int sum = (int)target["sum"];
            Debug.Log(targetType);
            if (targetType == 0)
            {
                NpcObjData a = face.npcInterface.getNpc(targetKey);
                string targetName = a.name.ToString();
                if (a.type == 2)
                {
                    targetStr = "跟<color=#1b8100>" + targetName + "</color>对话";
                }
                else if (a.type == 5)
                {
                    targetStr = "收集<color=#1b8100>" + targetName + "（" + num + "/" + sum + "）</color>";
                }

            }
            else
            {
                JObject a = face.monsterInterface.getMonster(targetKey);
                string targetName = a["name"].ToString();
                targetStr = "击败<color=#1b8100>" + targetName + "（" + num + "/" + sum + "）</color>";

            }
            return targetStr;
        }
        /**获取进度描述*/
        public string getProgressDes()
        {
            JObject progress = (JObject)this.progressList[this.progressIndex];
            return progress["des"].ToString();
        }
        /**获取进度标题*/
        public string getProgressTitle()
        {
            //todo: 注意使用前先要给progressIndex赋值
            JObject progress = (JObject)this.progressList[this.progressIndex];
            return progress["name"].ToString();
        }
        /**要按进度放置*/
        /*public void addTargetPos(string mapKey, Vector3 pos)
        {
            //改成直接获取目标npc位置
            JObject msg = new JObject();
            msg.Add("mapKey", mapKey);
            JObject p = new JObject();
            p.Add("x", pos.x);
            p.Add("y", pos.y);
            p.Add("z", pos.z);
            msg.Add("pos", p);
            this.targetPosList.Add(msg);
        }*/
        public task addTargetPos(string mapKey, string npcKey)
        {
            //默认获取任务目标npc位置
            JObject msg = new JObject();
            msg.Add("mapKey", mapKey);
            msg.Add("npcKey", npcKey == null ? getTarget()["key"] : npcKey);
            this.targetPosList.Add(msg);
            return this;
        }

        public JObject getTargetPos()
        {
            if (this.progressIndex >= this.targetPosList.Count) return null;
            return (JObject)this.targetPosList[this.progressIndex];
        }
        /**
         toKey：需要转移的npc
        key:当前收集的npcKey或怪物key
         */
        public task addTarget(int progressType, string name, string des, string toKey, string key, int num, int sum, int targetType = 0, int isCreate = 0)
        {
            JObject progress = createProgressOne(progressType, name, des);
            putTarget(progress, toKey, key, num, sum, targetType, isCreate);
            addProgress(progress);
            return this;
        }
        /**创建一个进度
         * 0对话1摘取2战斗3接取即完成4只接取
         * **/
        private JObject createProgressOne(int type, string name, string des)
        {
            JObject obj = new JObject();
            obj.Add("type", type);
            obj.Add("status", 0);
            obj.Add("name", name);
            obj.Add("des", des);
            return obj;
        }

        /**将进度目标放置
         * toKey:任务点转移显示的npckey
         * targetType目标类型 0npc、1怪物
         * isCreate是否需要创建一个npc
         * **/
        private void putTarget(JObject progress, string toKey, string key, int num, int sum, int targetType = 0, int isCreate = 0)
        {
            JObject progressTarget = new JObject();
            progressTarget.Add("toKey", toKey);
            progressTarget.Add("key", key);
            progressTarget.Add("num", num);
            progressTarget.Add("sum", sum);
            progressTarget.Add("targetType", targetType);
            progressTarget.Add("isCreate", isCreate);
            progress.Add("target", progressTarget);
        }
        /**添加进度*/
        private void addProgress(JObject progress)
        {
            progressList.Add(progress);
        }
    }
}
