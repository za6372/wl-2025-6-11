using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    public class fightAttr
    {
        //带上get、set才能用反射
        public float ll { get; set; }
        public float mj { get; set; }
        public float zl { get; set; }
        public float nl { get; set; }
        public float js { get; set; }
        public float max_lan { get; set; }
        public float max_xue { get; set; }
        public float max_exp { get; set; }
        public float xue { get; set; }
        public float lan { get; set; }
        public float exp { get; set; }
        public float wg { get; set; }
        public float fg { get; set; }
        public float wf { get; set; }
        public float ff { get; set; }
        public float mz { get; set; }
        public float sd { get; set; }
        public float bj { get; set; }
        public float css { get; set; }
        public float bjkx { get; set; }
        public float lxkx { get; set; }
        public float hlkx { get; set; }
        public float hskx { get; set; }
        public float hurt { get; set; }
        public void initBase(float ll, float mj, float zl, float nl, float js)
        {
            this.ll = ll;
            this.mj = mj;
            this.zl = zl;
            this.nl = nl;
            this.js = js;
        }
    }
}
