using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    public class randomAttrRule : fightAttr
    {
        int lv; int quality;
        public randomAttrRule(int lv, int quality) : base()
        {
            this.lv = lv;
            this.quality = quality;
            //this.create();
        }

        /**获取其有效的属性 */
        /*public JObject getEffectAttr()
        {
            JObject obj = new JObject();
            string[] a = strUtils.getAttrName();
            foreach (string k in a)
            {
                JObject attr = strUtils.copyJSON<JObject>(this);
                if (attr[k] != null)
                {
                    obj[k] = attr[k];
                }
            }
            return obj;
        }*/
        /*public void create()
        {
            float k = this.lv * (1 + this.quality * 0.2f);//每个品质提升20%
            JObject obj = new JObject();
            obj["ll"] = 10 * k / 50;
            obj["zl"] = 10 * k / 50;
            obj["nl"] = 10 * k / 50;
            obj["js"] = 10 * k / 50;
            obj["mj"] = 10 * k / 50;

            obj["wg"] = 20 * k / 50;
            obj["fg"] = 20 * k / 50;
            obj["wf"] = 10 * k / 50;
            obj["ff"] = 10 * k / 50;
            obj["mz"] = 20 * k / 50;
            obj["sd"] = 20 * k / 50;
            obj["bj"] = 20 * k / 50;
            obj["css"] = 20 * k / 50;

            obj["max_xue"] = 60 * k / 50;
            obj["max_lan"] = 60 * k / 50;
            List<string> arr = this.filterAttr();
            foreach (string p in arr)
            {
                //strUtils.setAttrValue(this, p, obj[p]);


            }

        }*/
        //按品质随机保留属性
        /*public List<string> filterAttr()
        {
            int len = 0;
            if (this.quality == 1)
            {
                len = 1;
            }
            else if (this.quality > 1)
            {
                len = 2;
            }
            string[] arr = strUtils.getAttrName();
            List<string> res = new List<string>();
            for (int i = 0; i < len; i++)
            {
                int r = strUtils.getRandom(0, arr.Length);
                res.Add(arr[r]);
            }
            return res;
        }*/
    }
}
