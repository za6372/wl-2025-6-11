using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.Res.script.src.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Res.script.src.touch
{
    class Move : MonoBehaviour, IPointerDownHandler
    {
        //人物
        private GameObject player;
        //地图底图
        private GameObject mape;

        //场景大小
        private Vector2 sceneSize;
        //场景位置
        private Vector2 scenePos;
        //是否允许移动
        private bool isAllowedMove = false;
        private int targetPosIndex;
        private List<Vector2> planTargetList = new List<Vector2>();

        //方向
        private Vector3 dir;
        public float speed;
        public float scLimitX;
        public float scLimitY;
        public float leftLimit;
        public float rightLimit;
        public float topLimit;
        public float bottomLimit;

        public float up, down, left, right;

        private Vector2 mapTargetPos = default;
        private Vector2 playerTargetPos = default;
        private bool isCreate = true;

        public void init()
        {
            if (this.transform.Find("control_player") == null) return;
            this.player = this.transform.Find("control_player").gameObject;

            RectTransform rt = this.gameObject.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;

            this.mape = this.transform.Find("mape").gameObject;
            RectTransform rt1 = mape.GetComponent<RectTransform>();
            this.sceneSize = rt1.sizeDelta;
            this.scenePos = this.mape.transform.position;

            this.scLimitX = ScreenUtils.width / 4f;
            this.scLimitY = ScreenUtils.width / 4f;

            this.leftLimit = (this.scenePos.x - (this.sceneSize.x - ScreenUtils.width) * ScreenUtils.screenScaleRate);
            this.rightLimit = this.scenePos.x;
            this.topLimit = (this.scenePos.y + (this.sceneSize.y - (ScreenUtils.height - 300)) * ScreenUtils.screenScaleRate);
            this.bottomLimit = this.scenePos.y;

            Vector2 vs = ScreenUtils.getCanvasSize();
            //Debug.Log(vs);
            left = ((vs.x - ScreenUtils.width) / 2f + scLimitX) * ScreenUtils.screenScaleRate;
            right = ((vs.x - ScreenUtils.width) / 2f + ScreenUtils.width - scLimitX) * ScreenUtils.screenScaleRate;
            up = ((vs.y - ScreenUtils.height) / 2f + ScreenUtils.height - scLimitY - 90) * ScreenUtils.screenScaleRate;
            down = ((vs.y - ScreenUtils.height) / 2f + scLimitY + 396) * ScreenUtils.screenScaleRate;
        }
       
        /**获取方向*/
        public Vector2 getDir()
        {
            return dir;
        }
        /**进入屏幕限制区时生成地图、玩家的目标点*/
        private void createTargetPos()
        {

            bool isMapX = true;
            bool isMapY = true;

            mapTargetPos = this.mape.transform.position;
            playerTargetPos = this.player.transform.position;
            //Debug.Log(isMapX + "/" + isMapY);

            if (isMapX)
            {
                float x = planTargetList[targetPosIndex].x - this.player.transform.position.x;
                x = this.mape.transform.position.x - x;
                float y = this.mape.transform.position.y;
                mapTargetPos = new Vector2(x, y);
            }
            else
            {
                float y = this.player.transform.position.y;
                float x = planTargetList[targetPosIndex].x;
                playerTargetPos = new Vector2(x, y);
            }
            if (isMapY)
            {
                if (!isMapX)
                {
                    float y = planTargetList[targetPosIndex].y - this.player.transform.position.y;
                    float x = this.mape.transform.position.x;
                    y = this.mape.transform.position.y - y;
                    mapTargetPos = new Vector2(x, y);
                }
                else
                {
                    float y = planTargetList[targetPosIndex].y - this.player.transform.position.y;
                    float x = mapTargetPos.x;
                    y = this.mape.transform.position.y - y;
                    mapTargetPos = new Vector2(x, y);
                }
            }
            else
            {
                if (isMapX)
                {
                    float y = planTargetList[targetPosIndex].y;
                    float x = this.player.transform.position.x;
                    playerTargetPos = new Vector2(x, y);
                }
                else
                {
                    float y = planTargetList[targetPosIndex].y;
                    float x = playerTargetPos.x;
                    playerTargetPos = new Vector2(x, y);
                }
            }
            if (mapTargetPos.x > rightLimit && dir.x == -1)
            {
                float dx = rightLimit - mapTargetPos.x;
                playerTargetPos = new Vector2(playerTargetPos.x + dx, playerTargetPos.y);
                mapTargetPos = new Vector2(rightLimit, mapTargetPos.y);
            }
            else if (mapTargetPos.x < leftLimit && dir.x == 1)
            {
                float dx = leftLimit - mapTargetPos.x;
                playerTargetPos = new Vector2(playerTargetPos.x + dx, playerTargetPos.y);
                mapTargetPos = new Vector2(leftLimit, mapTargetPos.y);
            }
            if (mapTargetPos.y < bottomLimit && dir.y == 1)
            {
                float dy = bottomLimit - mapTargetPos.y;
                playerTargetPos = new Vector2(playerTargetPos.x, playerTargetPos.y + dy);
                mapTargetPos = new Vector2(mapTargetPos.x, bottomLimit);
            }
            else if (mapTargetPos.y > topLimit && dir.y == -1)
            {
                float dy = topLimit - mapTargetPos.y;
                playerTargetPos = new Vector2(playerTargetPos.x, playerTargetPos.y + dy);
                mapTargetPos = new Vector2(mapTargetPos.x, topLimit);
            }

        }
        /**自动遇怪*/
        public void startAutoMove()
        {
            GameAttrConst.isAutoYG = true;
            GameAttrConst.autoYgTimes = 200;
            this.continueAutoMove();
        }

        public void continueAutoMove()
        {
            //次数不足时-1
            if (GameAttrConst.autoYgTimes <= 0)
            {
                GameAttrConst.isAutoYG = false;
            }
            if (GameAttrConst.isAutoYG)
            {
                this.autoCreatePos();
            }
        }
        private void autoCreatePos()
        {
            Transform ts = PointGet.getControlPoint();
            Vector3 pos = ts.position;
            float x = strUtils.getRandom(-100, 100);
            float y = strUtils.getRandom(-100, 100);

            float tx = ts.localPosition.x + x;
            float ty = ts.localPosition.y + y;
            if (tx > 400) x = -(tx - 400);
            else if (tx < -400) x = -400 - tx;
            if (ty > 400) y = -(ty - 400);
            else if (ty < -400) y = -400 - ty;
            putTargetPos(new Vector2(pos.x + x, pos.y + y));
        }
        public void stopMove()
        {
            clearTargetList();
        }
        private void Start()
        {
            //StartCoroutine(ExecuteEverySecond());
        }
        IEnumerator ExecuteEverySecond()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                doMove();
            }
        }
        private void Update()
        {
            doMove();
        }
        void doMove()
        {
            if (!isAllowedMove || planTargetList.Count == 0) return;
            //Debug.Log(!isAllowedMove + "/" + (planTargetList.Count == 0));
            //player.transform.localPosition += dir * speed;
            //player.GetComponent<BindBaseMsg>().updatePos(new Vector2(touchX, touchY));

            //Debug.Log("地图位置：" + this.mape.transform.position);
            bool isMapMove = false;


            if ((player.transform.position.x > right && dir.x == 1) ||
            (player.transform.position.x < left && dir.x == -1) ||
            (player.transform.position.y > up && dir.y == 1) ||
            (player.transform.position.y < down && dir.y == -1)
            )
            {
                isMapMove = true;
            }
            eventsUtils.dispatchElseEvent("100", null);

            //Debug.Log("isMapMove" + isMapMove);
            if (!isAllowedMove || planTargetList.Count == 0) return;

            if (isMapMove)
            {
                if (isCreate)
                {
                    isCreate = false;
                    createTargetPos();
                }

                this.player.transform.position =
                    Vector3.MoveTowards(player.transform.position, playerTargetPos, speed * Time.deltaTime);
                this.mape.transform.position =
                    Vector3.MoveTowards(this.mape.transform.position, mapTargetPos, speed * Time.deltaTime);

                playerManager.getInstance().keepDis();

                /*Debug.Log(this.player.transform.position.x + "/" + playerTargetPos.x + "/" +
                    this.player.transform.position.y + "/" + playerTargetPos.y);*/
                if (numberUtils.isSameFloat(this.mape.transform.position.x, mapTargetPos.x) &&
                         numberUtils.isSameFloat(this.mape.transform.position.y, mapTargetPos.y) &&
                         numberUtils.isSameFloat(this.player.transform.position.x, playerTargetPos.x) &&
                     numberUtils.isSameFloat(this.player.transform.position.y, playerTargetPos.y))
                {
                    
                    targetPosIndex++;
                    isCreate = true;

                    if (targetPosIndex >= planTargetList.Count)
                    {
                        this.clearTargetList();
                    }
                }
            }
            else
            {
                Vector2 targetPos = planTargetList[targetPosIndex];
                player.transform.position =
                    Vector3.MoveTowards(player.transform.position, targetPos, speed * Time.deltaTime);
                if (numberUtils.isSameFloat(player.transform.position.x, targetPos.x) &&
                    numberUtils.isSameFloat(player.transform.position.y, targetPos.y))
                {
                    targetPosIndex++;
                    isCreate = true;
                    if (targetPosIndex >= planTargetList.Count)
                    {
                        this.clearTargetList();
                    }
                }
            }

        }

        private void clearTargetList()
        {
            this.isAllowedMove = false;
            this.planTargetList.Clear();
            this.targetPosIndex = 0;
            this.isCreate = true;
            if (dir.x == 1)
            {
                player.GetComponent<AniRuntime>().playOnce(7, 8);
            }
            else
            {
                player.GetComponent<AniRuntime>().playOnce(3, 4);
            }
            //需要上传相对于mape的位置
            Transform mape = PointGet.getMapMapePoint();
            //Vector2 size = mape.GetComponent<RectTransform>().sizeDelta;
            //Vector3 mapePos = new Vector2(mape.localPosition.x - size.x / 2f, mape.localPosition.y + size.y / 2f);
            //Vector3 mapePos = mape.position;
            //Vector2 pos = player.transform.position - mapePos;

            //Vector2 pos = player.transform.localPosition- mape.GetComponent<modelRealPosMsgBind>().getDis();
            Vector2 pos = PosCountUtils.controlPosRelativeMapePos(player.transform);
            face.roleInterface.isUploadPos(pos);

            //自动遇怪的情况下需要重新给个位置
            if (GameAttrConst.isAutoYG)
            {
                this.autoCreatePos();
            }
        }
        public void putTargetPos(Vector2 pos)
        {
            inputPoint(pos);
        }
        private bool handleGround(Vector2 touch)
        {
            bool b = npcManager.isTouchNpc(touch);
            if (b) return false;
            return true;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!face.teamInterface.isAllowedTouchMove())
            {
                msgCode.showMsg(1026);
                return;
            }
            if (GameAttrConst.isAutoYG)
            {
                GameAttrConst.isAutoYG = false;
                msgCode.showMsg(996);
                return;
            }
            if (eventData.pointerPress != null)
            {
                
            }
            else
            {
                //Vector3(27.5, 468.365417, 0)
                this.clearTargetList();
                float touchX = eventData.position.x;
                float touchY = eventData.position.y;
                
                Vector2 touch = new Vector2(touchX, touchY);

                if (!handleGround(touch))
                {
                    return;
                }
                //BindBaseMsg msg = player.GetComponent<BindBaseMsg>();

                inputPoint(touch);
            }

        }
        private void inputPoint(Vector2 touch)
        {
            this.speed = face.roleInterface.getMoveSpeed();
            float xd = 1;
            float yd = 1;
            if (touch.x - this.player.transform.position.x > 0)
            {
                xd = 1;
                player.GetComponent<AniRuntime>().play(4, 7);
            }
            else
            {
                xd = -1;
                player.GetComponent<AniRuntime>().play(0, 3);
            }
            if (touch.y - this.player.transform.position.y < 0)
            {
                yd = -1;
            }
            else
            {
                yd = 1;
            }
            dir = new Vector2(xd, yd);
            Vector2[] zaw = {
                    new Vector2(400,-400),new Vector2(500,-400),
                    new Vector2(500,-500),new Vector2(400,-500)
                };
            zaw = new Vector2[0];
            getPlanPath(this.player.transform.position * 1f, touch, zaw, this.planTargetList);
            /*for (int i = 0; i < this.planTargetList.Count; i++)
            {
                Debug.Log(this.planTargetList[i]);
            }*/
            controlManager.updatePetPos();
            this.isAllowedMove = true;
        }
        
        private void getPlanPath(Vector2 startPos, Vector2 endPos, Vector2[] vs, List<Vector2> targetList)
        {
            //Debug.Log("输入：" + startPos.x + "," + startPos.y + "=====" + endPos.x + "," + endPos.y);
            if (vs.Length == 0)
            {
                targetList.Add(endPos);
                return;
            }
            float b1, k1;
            k1 = getK(endPos, startPos);
            b1 = startPos.y - k1 * startPos.x;

            float b2, k2;
            k2 = getK(vs[1], vs[0]);
            b2 = vs[0].y - k2 * vs[0].x;
            Vector4 fw2 = getGround(vs[0], vs[1]);

            float b3, k3;
            k3 = getK(vs[2], vs[1]);
            b3 = vs[1].y - k3 * vs[1].x;
            Vector4 fw3 = getGround(vs[1], vs[2]);

            float b4, k4;
            k4 = getK(vs[3], vs[2]);
            b4 = vs[2].y - k4 * vs[2].x;
            Vector4 fw4 = getGround(vs[2], vs[3]);

            float b5, k5;
            k5 = getK(vs[0], vs[3]);
            Debug.Log(k5);
            b5 = vs[3].y - k5 * vs[3].x;
            Vector4 fw5 = getGround(vs[3], vs[0]);


            int[] xjMark = { 0, 0, 0, 0 };
            List<Vector2> jds = new List<Vector2>();
            float jdX1, jdY1;
            if (k1 == float.PositiveInfinity
                && k2 != float.PositiveInfinity)
            {
                jdX1 = startPos.x;
                jdY1 = k2 * jdX1 + b2;
            }
            else if (k1 != float.PositiveInfinity
                && k2 == float.PositiveInfinity)
            {
                jdY1 = k1 * vs[0].x + b1;
                jdX1 = vs[0].x;
            }
            else if ((k1 == float.PositiveInfinity
               && k2 == float.PositiveInfinity) || k1 == k2)
            {
                jdY1 = float.PositiveInfinity;
                jdX1 = float.PositiveInfinity;
            }
            else
            {
                jdX1 = (b2 - b1) / (k1 - k2);
                jdY1 = k1 * jdX1 + b1;
            }

            if (isGround(fw2, new Vector2(jdX1, jdY1)))
            {
                jds.Add(new Vector2(jdX1, jdY1));
                xjMark[0] = 1;
            }





            float jdX2, jdY2;
            if (k1 == float.PositiveInfinity
                && k3 != float.PositiveInfinity)
            {
                jdX2 = startPos.x;
                jdY2 = k3 * jdX2 + b3;
            }
            else if (k1 != float.PositiveInfinity
                && k3 == float.PositiveInfinity)
            {
                jdY2 = k1 * vs[1].x + b1;
                jdX2 = vs[1].x;
            }
            else if ((k1 == float.PositiveInfinity
               && k3 == float.PositiveInfinity) || k1 == k3)
            {
                jdY2 = float.PositiveInfinity;
                jdX2 = float.PositiveInfinity;
            }
            else
            {
                jdX2 = (b3 - b1) / (k1 - k3);
                jdY2 = k1 * jdX2 + b1;
            }
            if (isGround(fw3, new Vector2(jdX2, jdY2)))
            {
                jds.Add(new Vector2(jdX2, jdY2));
                xjMark[1] = 1;
            }

            float jdX3, jdY3;
            if (k1 == float.PositiveInfinity
                && k4 != float.PositiveInfinity)
            {
                jdX3 = startPos.x;
                jdY3 = k4 * jdX3 + b4;
            }
            else if (k1 != float.PositiveInfinity
                && k4 == float.PositiveInfinity)
            {
                jdY3 = k1 * vs[2].x + b1;
                jdX3 = vs[2].x;
            }
            else if ((k1 == float.PositiveInfinity
               && k4 == float.PositiveInfinity) || k1 == k4)
            {
                jdY3 = float.PositiveInfinity;
                jdX3 = float.PositiveInfinity;
            }
            else
            {
                jdX3 = (b4 - b1) / (k1 - k4);
                jdY3 = k1 * jdX3 + b1;
            }
            if (isGround(fw4, new Vector2(jdX3, jdY3)))
            {
                jds.Add(new Vector2(jdX3, jdY3));
                xjMark[2] = 1;
            }

            float jdX4, jdY4;
            if (k1 == float.PositiveInfinity
                && k5 != float.PositiveInfinity)
            {
                jdX4 = startPos.x;
                jdY4 = k5 * jdX4 + b5;
            }
            else if (k1 != float.PositiveInfinity
                && k5 == float.PositiveInfinity)
            {
                jdY4 = k1 * vs[3].x + b1;
                jdX4 = vs[3].x;
            }
            else if ((k1 == float.PositiveInfinity
               && k5 == float.PositiveInfinity) || k1 == k5)
            {
                jdY4 = float.PositiveInfinity;
                jdX4 = float.PositiveInfinity;
            }
            else
            {
                jdX4 = (b5 - b1) / (k1 - k5);
                jdY4 = k1 * jdX4 + b1;
            }
            if (isGround(fw5, new Vector2(jdX4, jdY4)))
            {
                jds.Add(new Vector2(jdX4, jdY4));
                xjMark[3] = 1;
            }

            if (jds.Count == 1)
            {
                if (isIn(jds[0], fw2, fw3, fw4, fw5))
                {
                    targetList.Add(jds[0]);
                }
                else
                {
                    targetList.Add(endPos);
                }
            }
            else if (jds.Count == 2)
            {
                bool b = false;
                for (int i = 0; i < xjMark.Length - 1; i++)
                {
                    if ((xjMark[i] == 1 && xjMark[i + 1] == 1) || (xjMark[i] == 0 && xjMark[i + 1] == 0))
                    {
                        b = true;
                        break;
                    }
                }
                if (b)
                {
                    float d1 = getDis(startPos, jds[0]);
                    float d2 = getDis(startPos, jds[1]);
                    if (d1 > d2) jds.RemoveAt(0);
                    else jds.RemoveAt(1);

                    List<Vector2> dds = getDd(jds[0], k2, b2, k3, b3, k4, b4, k5, b5, vs);
                    d1 = getDis(endPos, dds[0]);
                    d2 = getDis(endPos, dds[1]);
                    if (d1 > d2) targetList.Add(dds[1]);
                    else targetList.Add(dds[0]);
                    //Debug.Log(d1 + "============" + d2);
                    targetList.Add(endPos);
                }
                else
                {
                    float d1 = getDis(startPos, jds[0]);
                    float d2 = getDis(startPos, jds[1]);
                    if (d1 > d2) jds.RemoveAt(0);
                    else jds.RemoveAt(1);
                    List<Vector2> dds = getDd(jds[0], k2, b2, k3, b3, k4, b4, k5, b5, vs);
                    Vector2 minDd;
                    d1 = getDis(endPos, dds[0]);
                    d2 = getDis(endPos, dds[1]);
                    if (d1 > d2) minDd = dds[1];
                    else minDd = dds[0];
                    targetList.Add(minDd);
                    List<Vector2> ddsList = getDd(minDd, k2, b2, k3, b3, k4, b4, k5, b5, vs);
                    removeRepeat(minDd, ddsList);
                    d1 = getDis(endPos, ddsList[0]);
                    d2 = getDis(endPos, ddsList[1]);
                    if (d1 > d2) targetList.Add(ddsList[1]);
                    else targetList.Add(ddsList[0]);

                    targetList.Add(endPos);
                }
            }
            else if (jds.Count == 3)
            {

                if (jds[0] == jds[1]) jds.RemoveAt(1);
                else if (jds[0] == jds[2]) jds.RemoveAt(2);
                else jds.RemoveAt(1);
                float d1 = getDis(startPos, jds[0]);
                float d2 = getDis(startPos, jds[1]);
                if (d1 > d2) jds.RemoveAt(0);
                else jds.RemoveAt(1);
                List<Vector2> ddsList = getDd(jds[0], k2, b2, k3, b3, k4, b4, k5, b5, vs);
                removeRepeat(jds[0], ddsList);
                d1 = getDis(startPos, ddsList[0]) + getDis(endPos, ddsList[0]);
                d2 = getDis(startPos, ddsList[1]) + getDis(endPos, ddsList[1]);
                if (d1 > d2) targetList.Add(ddsList[1]);
                else targetList.Add(ddsList[0]);
                targetList.Add(endPos);
            }
            else if (jds.Count == 4)
            {
                if (jds[0] == jds[1])
                {
                    jds.RemoveAt(1);
                    jds.RemoveAt(1);
                }
                else
                {
                    jds.RemoveAt(2);
                    jds.RemoveAt(2);
                }

                for (int i = 0; i < vs.Length; i++)
                {
                    if (vs[i] != jds[0] && vs[i] != jds[1])
                    {
                        targetList.Add(vs[i] * 1f);
                        break;
                    }
                }
                targetList.Add(endPos);
            }
            else
            {
                targetList.Add(endPos);
            }

        }
        private float getK(Vector2 pos0, Vector2 pos1)
        {
            float k = (pos1.y - pos0.y) / (pos1.x - pos0.x);
            if (k == float.NegativeInfinity) k = float.PositiveInfinity;
            return k;
        }
        private void removeRepeat(Vector2 input, List<Vector2> ddsList)
        {
            for (int i = 0; i < ddsList.Count; i++)
            {
                if (ddsList[i] == input)
                {
                    ddsList.RemoveAt(i);
                    i--;
                }
            }
        }
        private List<Vector2> getDd(Vector2 jd, float k2, float b2, float k3, float b3, float k4, float b4, float k5, float b5, Vector2[] vs)
        {
            List<Vector2> dd = new List<Vector2>();
            if (((k2 == float.PositiveInfinity) && numberUtils.isSameFloat(jd.x, vs[0].x)) ||
                numberUtils.isSameFloat((k2 * jd.x + b2), jd.y))
            {
                dd.Add(vs[0] * 1f);
                dd.Add(vs[1] * 1f);
            }
            if (((k3 == float.PositiveInfinity) && numberUtils.isSameFloat(jd.x, vs[1].x)) ||
                numberUtils.isSameFloat((k3 * jd.x + b3), jd.y))
            {
                dd.Add(vs[1] * 1f);
                dd.Add(vs[2] * 1f);
            }
            if (((k4 == float.PositiveInfinity) && numberUtils.isSameFloat(jd.x, vs[2].x)) ||
                numberUtils.isSameFloat((k4 * jd.x + b4), jd.y))
            {
                dd.Add(vs[2] * 1f);
                dd.Add(vs[3] * 1f);
            }
            if (((k5 == float.PositiveInfinity) && numberUtils.isSameFloat(jd.x, vs[3].x)) ||
                numberUtils.isSameFloat((k5 * jd.x + b5), jd.y))
            {
                dd.Add(vs[3] * 1f);
                dd.Add(vs[0] * 1f);
            }

            return dd;
        }
        private float getDis(Vector2 startPos, Vector2 endPos)
        {
            return (endPos.x - startPos.x) * (endPos.x - startPos.x) +
                        (endPos.y - startPos.y) * (endPos.y - startPos.y);
        }
        private bool isIn(Vector2 endPos, Vector4 fw1, Vector4 fw2, Vector4 fw3, Vector4 fw4)
        {
            if (isGround(fw1, endPos) && isGround(fw2, endPos) &&
                isGround(fw3, endPos) && isGround(fw4, endPos)) return true;
            return false;
        }
        private bool isGround(Vector4 ground, Vector2 jd)
        {
            if (jd.x == float.PositiveInfinity || jd.y == float.PositiveInfinity) return false;
            bool b1 = numberUtils.isSameFloat(jd.x, ground.x);
            bool b2 = numberUtils.isSameFloat(jd.x, ground.y);
            bool b3 = numberUtils.isSameFloat(jd.y, ground.z);
            bool b4 = numberUtils.isSameFloat(jd.y, ground.w);
            //Debug.Log(b1 + " " + b2 + " " + b3 + " " + b4);
            if ((!b1 && jd.x < ground.x) || (!b2 && jd.x > ground.y)) return false;
            if ((!b3 && jd.y < ground.z) || (!b4 && jd.y > ground.w)) return false;
            return true;
        }
        private Vector4 getGround(Vector2 aPos, Vector2 bPos)
        {
            float a, b, c, d;
            if (bPos.x > aPos.x)
            {
                a = aPos.x; b = bPos.x;
            }
            else
            {
                a = bPos.x; b = aPos.x;
            }
            if (bPos.y > aPos.y)
            {
                c = aPos.y; d = bPos.y;
            }
            else
            {
                c = bPos.y; d = aPos.y;
            }
            //Debug.Log("范围：" + a + "/" + b + "/" + c + "/" + d);
            return new Vector4(a, b, c, d);
        }
    }
}
