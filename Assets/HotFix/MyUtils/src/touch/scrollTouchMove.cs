using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.touch
{
    public class scrollTouchMove : MonoBehaviour
    {
        //底部回调
        public Action bCall;
        //顶部回调
        public Action tCall;
        //拖动过程中回调
        public Action<Vector2> moveCall;

        private Vector2 first;
        //private Vector2 second;
        private float viewportH;
        void Start()
        {

        }

        public void init(float viewportH, Action tCall, Action bCall,Action<Vector2> moveCall)
        {
            this.viewportH = viewportH;
            this.tCall = tCall;
            this.bCall = bCall;
            this.moveCall = moveCall;
        }
        // Update is called once per frame
        void Update()
        {
            if (tCall != null && bCall != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //记录鼠标按下的位置 　　
                    first = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    //记录鼠标拖动的位置 　　
                    Vector2 second = Input.mousePosition;
                    if (second.x == first.x && second.y == first.y)
                    {
                        //未发生拖拽
                    }
                    else
                    {
                        RectTransform arc = this.GetComponent<RectTransform>();
                        float y = arc.anchoredPosition.y;
                        //相对原点位置是 -arc.sizeDelta.y/2 内容高的一半
                        float zeroY = -arc.sizeDelta.y / 2f;
                        if (y < zeroY)
                        {//小于原点位置
                            tCall();
                        }
                        else if (y > arc.sizeDelta.y + zeroY - viewportH)
                        {//大于终点位置 内容高度+原点位置-视口高度
                            bCall();
                        }
                        //Debug.Log(y + "=>" + arc.sizeDelta.y + "=>" + zeroY);
                        if (moveCall != null) moveCall(arc.anchoredPosition);
                    }
                }


            }


        }
    }
}
