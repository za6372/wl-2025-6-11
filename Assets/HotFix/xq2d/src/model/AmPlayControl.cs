using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    /**记录帧播放开始及结束的位置*/
    public class AmPlayControl
    {
        public bool isNull;//表示不需要这个动作
        public int start;
        public int end;

        public AmPlayControl(bool isNull)
        {
            this.isNull = isNull;
        }

        public AmPlayControl(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
