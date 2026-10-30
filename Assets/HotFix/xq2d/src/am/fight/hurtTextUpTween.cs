using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.am.fight
{
    /**伤害文字向上缓动*/
    public class hurtTextUpTween : MonoBehaviour
    {

        private Vector3 pos1;
        private Vector3 pos2;
        private long oldTime;
        private float dy;
        //放大基数
        private float scaleA = 0.5f;
        private Vector2 oldScale;

        private bool isStop = false;
        private float speed = 2.5f;

        private Action callback;

        public hurtTextUpTween addCallback(Action callback)
        {
            this.callback = callback;
            return this;
        }
        public hurtTextUpTween setSpeed(float speed)
        {
            this.speed = speed;
            return this;
        }
        public hurtTextUpTween setPos(float dx, float dy, float scaleA = 0.5f)
        {
            this.oldTime = strUtils.getMillis();

            Vector3 pos1 = this.transform.localPosition * 1f;
            //左右偏移量
            pos1.x += dx;
            Vector3 pos2 = pos1 * 1f;
            pos2.y += dy;
            this.dy = dy;
            this.scaleA = scaleA;
            this.pos1 = pos1;
            this.pos2 = pos2;
            this.oldScale = this.transform.localScale * 1f;
            return this;
        }
        // Update is called once per frame
        void Update()
        {
            if (isStop) return;

            if (this.pos1 != null && this.pos2 != null && this.transform.localPosition.y < this.pos2.y)
            {
                //TODO:注意不要用Vector3.Lerp，它会关联多个ui，导致多个ui一起移动
                this.pos1.y += speed;
                this.transform.localPosition = this.pos1;

                float k = (1 - (pos2.y - pos1.y) / dy) + scaleA;
                this.transform.localScale = this.oldScale * k;
                if (this.GetComponent<Text>() != null)
                {
                    Color cl = this.GetComponent<Text>().color;
                    this.GetComponent<Text>().color = new Color(cl.r, cl.g, cl.b, k - scaleA);
                }

            }
            if (strUtils.getMillis() - this.oldTime > 2000)
            {
                isStop = true;
                //移除
                gameObjPool.getInstance().free(this.gameObject);
                if (callback != null) callback();

            }

        }
    }
}
