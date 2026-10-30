using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.MyUtils.src.common
{
	/**回调任务执行器*/
	public class taskManager
	{
		private static taskManager t;
		//是否有任务正在执行中
		public bool isUser = false;
		//需要执行的任务
		public List<Action> taskList;
		public static taskManager getInstance()
		{
			if (t == null) t = new taskManager();
			return t;
		}

		public taskManager()
		{
			taskList = new List<Action>();

		}

		public void putTask(Action fn)
		{
			this.taskList.Add(fn);
			this.doHandle();
		}
		private void doHandle()
		{
			if (!this.isUser && this.taskList.Count > 0)
			{
				this.isUser = true;
				this.taskList[0]();
				this.taskList.RemoveAt(0);
				this.isUser = false;
				this.doHandle();
			}
		}
		public bool isHandleFinish()
		{
			if (this.taskList.Count == 0)
			{
				return true;
			}
			return false;
		}

	}
}
