using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.am.fight
{
    /**沙漏旋转*/
    class shalouRotation : MonoBehaviour
    {
        private Sprite sp;
        private Sprite[] list;
        private int index;
        private long oldTime;
        public shalouRotation init(Sprite sp)
        {
            list = new Sprite[] {
            Sprite.Create(sp.texture, new Rect(sp.rect.x +13 * 0,sp.rect.y + 0, 13, 13), new Vector2(0.5f, 0.5f)),
            Sprite.Create(sp.texture, new Rect(sp.rect.x +13 * 1,sp.rect.y + 0, 13, 13), new Vector2(0.5f, 0.5f)),
            Sprite.Create(sp.texture, new Rect(sp.rect.x +13 * 2,sp.rect.y + 0, 13, 13), new Vector2(0.5f, 0.5f)),
            Sprite.Create(sp.texture, new Rect(sp.rect.x +13 * 3,sp.rect.y + 0, 13, 13), new Vector2(0.5f, 0.5f))
            };

            this.sp = sp;
            return this;
        }
        private void LateUpdate()
        {
            if (sp == null) return;
            if (index > 3) index = 0;

            long nowTime = strUtils.getMillis();
            if (nowTime - oldTime < 200)
            {
                return;
            }
            oldTime = nowTime;

            this.gameObject.GetComponent<Image>().sprite = list[index];
            index++;
        }

    }
}
