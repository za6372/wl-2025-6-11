using Assets.HotFix.xq2d.src.faceI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.model
{
    public class fixedAttrRule : fightAttr
    {
        string equipKey;
        string part; int lv; int quality; string job;
        /**传入装备部位、等级、品质 */
        public fixedAttrRule(string equipKey, string part, int lv, int quality, string job) : base()
        {
            this.equipKey = equipKey;
            this.part = part;
            this.lv = lv;
            this.quality = quality;
            this.job = job;
            this.create();
        }
        public void create()
        {
            //每个部位都有自己确定的基础值，等级为该基础值得倍数，品质影响倍数
            float k = this.lv * (1f + this.quality * 0.2f) * 0.35f;//每个品质提升20%
                                                           //对于物理职业的物攻就需要增加2倍
                                                           //对于套装还要提升10%
            if (face.equipInterface.isTaoZhuang(equipKey))
            {
                k += 0.1f;
            }
            switch (this.part)
            {
                case "wq":
                    {//武器 影响法攻、物攻 fg、wg
                        this.wg = 100f * k / 50f;
                        this.fg = 100f * k / 50f;
                        break;
                    }
                case "jb":
                    {//颈部
                        this.wg = 50f * k / 50f;
                        this.fg = 50f * k / 50f;
                        this.max_lan = 200f * k / 50f;
                        break;
                    }
                case "sz":
                    {//手指
                        this.wg = 50f * k / 50f;
                        this.fg = 50f * k / 50f;
                        break;
                    }
                case "wb":
                    {//腕部
                        this.wf = 30f * k / 50f;
                        this.ff = 30f * k / 50f;
                        break;
                    }
                case "tb":
                    {//头部
                        this.max_xue = 600f * k / 50f;
                        break;
                    }
                case "xb":
                    {//胸部
                        this.max_xue = 800f * k / 50f;
                        break;
                    }
                case "yb":
                    {//腰部
                        this.wf = 30f * k / 50f;
                        this.ff = 30f * k / 50f;
                        break;
                    }
                case "tuib":
                    {//腿部
                        this.wf = 30f * k / 50f;
                        this.ff = 30f * k / 50f;
                        break;
                    }
                case "jiaob":
                    {//脚部
                        this.css = 150f * k / 50f;
                        this.wf = 20f * k / 50f;
                        this.ff = 20f * k / 50f;
                        break;
                    }
                case "bjb":
                    {//腿部
                        this.max_xue = this.quality * 100f;
                        break;
                    }
            }
            switch (this.job)
            {
                case "ms":
                    {
                        //物攻、物防翻倍
                        if (this.wg > 0)
                            this.wg *= 2f;
                        if (this.wf > 0)
                            this.wf *= 2f;
                        break;
                    }
                case "dj":
                    {
                        //物攻、物防翻倍
                        if (this.wg > 0)
                            this.wg *= 2f;
                        if (this.wf > 0)
                            this.wf *= 2f;
                        break;
                    }
                case "qm":
                    {
                        if (this.fg > 0)
                            this.fg *= 2f;
                        if (this.ff > 0)
                            this.ff *= 2f;
                        break;
                    }
                case "ty":
                    {
                        if (this.fg > 0)
                            this.fg *= 2f;
                        if (this.ff > 0)
                            this.ff *= 2f;
                        break;
                    }
                case "ym":
                    {
                        if (this.wg > 0)
                            this.wg *= 2f;
                        if (this.css > 0)
                            this.css *= 2f;
                        break;
                    }
                case "lc":
                    {
                        if (this.wg > 0)
                            this.wg *= 2f;
                        if (this.css > 0)
                            this.css *= 2f;
                        break;
                    }

            }
        }

    }
}
