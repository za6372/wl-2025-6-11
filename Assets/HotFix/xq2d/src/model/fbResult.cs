using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    class fbResult
    {
        public string key;
        public float k;
        public float b;
        public int limitLv;

        public fbResult(string key, float k, float b)
        {
            this.key = key;
            this.k = k;
            this.b = b;
            this.limitLv = 100;
        }

        public fbResult(string key, float k, float b, int limitLv)
        {
            this.key = key;
            this.k = k;
            this.b = b;
            this.limitLv = limitLv;
        }

        /**获取最终增伤*/
        public float getFinalHurtAddOrCut(int lv)
        {
            return this.getV(lv);
        }
        /**获取通用计算值*/
        public float getV(int lv)
        {
            return k * lv + b;
        }
        public float getMaxV()
        {
            return k * limitLv + b;
        }
    }
}
