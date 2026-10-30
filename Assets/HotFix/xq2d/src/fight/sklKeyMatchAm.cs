using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.fight
{
    /**技能匹配动画*/
    class sklKeyMatchAm
    {
        /**返回技能动作
         attack 普攻 
        10 attack 11 skill 12 skill1 13 skill2 14 skill3
         */
        public static int getSklAction(string sklKey)
        {
            if (isMatchSkill1(sklKey))
            {
                return 12;
            }
            else if (isMatchSkill2(sklKey))
            {
                return 13;
            }
            else if (isMatchSkill3(sklKey))
            {
                return 14;
            }
            //不设置则默认技能动画
            return 11;
        }

        private static bool isMatchSkill1(string sklKey)
        {
            string[] arr =
            {

            };
            foreach (string a in arr)
            {
                if (a.Equals(sklKey)) return true;
            }
            return false;
        }
        private static bool isMatchSkill2(string sklKey)
        {
            string[] arr =
            {

            };
            foreach (string a in arr)
            {
                if (a.Equals(sklKey)) return true;
            }
            return false;
        }
        private static bool isMatchSkill3(string sklKey)
        {
            string[] arr =
            {

            };
            foreach (string a in arr)
            {
                if (a.Equals(sklKey)) return true;
            }
            return false;
        }
    }
}
