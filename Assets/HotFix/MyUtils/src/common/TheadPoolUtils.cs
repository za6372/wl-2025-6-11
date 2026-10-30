using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.HotFix.MyUtils.src.common
{
    /**线程池*/
    public class TheadPoolUtils
    {
        public static void putThread(Action ac)
        {
            Task.Run(ac);
        }
    }
}
