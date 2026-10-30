using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    public class SimpleUI : BaseUI
    {
        public override void init()
        {
            this.gameObject.layer = 5;
        }
    }
}
