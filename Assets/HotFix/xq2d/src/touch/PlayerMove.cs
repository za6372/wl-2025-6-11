using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.touch
{
    class PlayerMove: MonoBehaviour
    {
        public float speed = 200f;
        private Vector2 targetPos;
        private bool isAllowedMove;
        private Vector2 dir;
        private Vector2 dis;
        
        private void Update()
        {
            if (!isAllowedMove) return;
            this.transform.position =
                    Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
            if (numberUtils.isSameFloat(this.transform.position.x, targetPos.x) &&
                    numberUtils.isSameFloat(this.transform.position.y, targetPos.y))
            {
                isAllowedMove = false;
                if (dir.x == 1)
                {
                    this.GetComponent<AniRuntime>().playOnce(7, 8);
                   
                }
                else
                {
                    this.GetComponent<AniRuntime>().playOnce(3, 4);
                    
                }
                //其他玩家上传时因为传的就是相对于mape的位置，所以无需再计算跟mape的位置
                dis = this.transform.position - PointGet.getMapMapePoint().position;

            }
        }
        public Vector2 getDis()
        {
            return this.dis;
        }
        public void inputPos(Vector2 touch)
        {
            //每次有输入点都要更新速度
            this.speed = face.roleInterface.getMoveSpeed();

            float xd = 1;
            float yd = 1;
            if (touch.x - this.transform.position.x > 0)//右
            {
                xd = 1;
                this.GetComponent<AniRuntime>().play(4, 7);
                if (this.transform.Find("player_pet") != null)
                {
                    Transform pTs = this.transform.Find("player_pet");
                    pTs.localPosition = new Vector2(-150 / GameAttrConst.getMapScaleRate(), 0);
                    int len = pTs.Find("frames").childCount;
                    AniRuntime am = pTs.GetComponent<AniRuntime>();
                    am.play(len / 2, len);
                }
            }
            else//左
            {
                xd = -1;
                this.GetComponent<AniRuntime>().play(0, 3);
                if (this.transform.Find("player_pet") != null)
                {
                    Transform pTs = this.transform.Find("player_pet");
                    pTs.localPosition = new Vector2(150 / GameAttrConst.getMapScaleRate(), 0);
                    int len = pTs.Find("frames").childCount;
                    AniRuntime am = pTs.GetComponent<AniRuntime>();
                    am.play(0, len / 2);
                }
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
