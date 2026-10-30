using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    /**当计数达到指定数量时执行回调*/
    public class TaskCountManager
    {
        private static TaskCountManager one;
        public int num;
        public int sum;
        private Action ac;
        public static TaskCountManager getInstanceBySingle()
        {
            if (one == null) one = new TaskCountManager();
            return one;
        }
        public TaskCountManager init(int sum, Action ac)
        {
            this.sum = sum;
            this.num = 0;
            this.ac = ac;
            return this;
        }


        public TaskCountManager addNum()
        {
            lock (this)
            {
                this.num++;
                this.doCall();
            }
            /*Interlocked.Increment(ref this.num);// 原子地将指定变量的值加 1。Interlocked.Add(ref int location, int value)
            //Interlocked.Decrement(ref int location)：原子地将指定变量的值减 1。
            this.doCall();*/
            return this;
        }
        private TaskCountManager doCall()
        {
            if (this.num >= this.sum)
            {
                this.ac();
            }
            return this;
        }
    }
}
