using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.factory.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.touch
{
    public class monsterMove : MonoBehaviour
    {
        public float speed = 50f;
        private Vector2 targetPos;
        private bool isAllowedMove;
        private Vector2 dir;
        private Vector2 dis;
        void Start()
        {
            targetPos = this.transform.position;

        }
        public void setIsAllowedMove(bool b)
        {
            isAllowedMove = b;
        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (!isAllowedMove) return;

            this.transform.position =
                   Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);

            if (numberUtils.isSameFloat(this.transform.position.x, targetPos.x) &&
                    numberUtils.isSameFloat(this.transform.position.y, targetPos.y))
            {
                //isAllowedMove = false;
                /*int len = this.transform.Find("frames").childCount - 1;
                if (dir.x == 1)
                {
                    this.GetComponent<AniRuntime>().play(0, len / 2);
                }
                else
                {
                    this.GetComponent<AniRuntime>().play(len / 2, len);
                }*/
                //其他玩家上传时因为传的就是相对于mape的位置，所以无需再计算跟mape的位置
                dis = this.transform.position - PointGet.getMapMapePoint().position;

                Vector2 size = PointGet.getMapMapePoint().GetComponent<RectTransform>().sizeDelta;
                float dw = (size.x - 100) / 2f;
                float dh = (size.y - 100) / 2f;
                //重新输入位置
                Vector3 pos = this.transform.position;
                float x = strUtils.getRandom(-100, 100);
                float y = strUtils.getRandom(-100, 100);

                float tx = this.transform.localPosition.x + x;
                float ty = this.transform.localPosition.y + y;
                if (tx > dw) x = -(tx - dw);
                else if (tx < -dw) x = -dw - tx;
                if (ty > dh) y = -(ty - dh);
                else if (ty < -dh) y = -dh - ty;
                inputPos(new Vector2(pos.x + x, pos.y + y));
            }
        }
        public Vector2 getDis()
        {
            return this.dis;
        }
        public void inputPos(Vector2 touch)
        {
            float xd = 1;
            float yd = 1;
            int len = this.transform.Find("frames").childCount - 1;
            if (touch.x - this.transform.position.x > 0)//右
            {
                xd = 1;
                this.GetComponent<AniRuntime>().play(0, len / 2);
            }
            else//左
            {
                xd = -1;
                this.GetComponent<AniRuntime>().play(len / 2, len);
            }
            if (touch.y - this.transform.position.y < 0)//下
            {
                yd = -1;
            }
            else//上
            {
                yd = 1;
            }
            dir = new Vector2(xd, yd);
            this.targetPos = touch;
            isAllowedMove = true;

        }
    }
}
