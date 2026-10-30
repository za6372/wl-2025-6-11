using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src
{
    public class AniRuntime : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] frameObjs;
        public float frameInterval = 0.1f;//更新间隔时间0.1秒
        private float timer;
        private int curFrame;
        //是否重复播放
        private bool isRepeat = true;
        //是否允许播放
        private bool isAllowed = false;
        //播放完后回调
        private Action callback;
        //开始帧
        private int startFrame = 0;
        //结束帧
        private int endFrame = -1;
        void Awake()
        {
            //Application.targetFrameRate = 10;//100是设置的帧数
        }
        void Update()
        {
            if (!isAllowed) return;
            timer += Time.deltaTime;
            if (timer >= frameInterval)
            {
                timer = 0;
                

                if (curFrame >= endFrame)
                {
                    if (!isRepeat)
                    {
                        isAllowed = false;
                        if (callback != null) callback();
                    }
                    curFrame = startFrame;
                }
                for (int i = 0; i < frameObjs.Length; ++i)
                {
                    if (null != frameObjs[i])
                        frameObjs[i].SetActive(curFrame == i);
                }
                ++curFrame;
            }
        }
        public void resetFrame()
        {
            this.curFrame = startFrame;
        }
        public AniRuntime setCall(Action callback)
        {
            this.callback = callback;
            return this;
        }
       
        private void setRepeat(bool b)
        {
            isRepeat = b;
        }
        public void play(int startFrame = 0, int endFrame = -1)
        {
            this.startFrame = startFrame;
            this.curFrame = this.startFrame;
            this.endFrame = endFrame;
            if (this.endFrame == -1) this.endFrame = frameObjs.Length;
            setRepeat(true);
            isAllowed = true;
        }
        /**只播放一次，从指定帧开始*/
        public void playOnce(int startFrame = 0, int endFrame = -1)
        {
            play(startFrame, endFrame);
            setRepeat(false);
        }
        public void pause()
        {
            isAllowed = false;
        }
        public void stop()
        {
            isAllowed = false;
            curFrame = startFrame;
        }
        /**设置开始帧和结束帧（不包含end帧，从0帧开始）*/
        /*public void setStartAndEnd(int start, int end)
        {
            this.startFrame = start;
            this.endFrame = end;
        }*/
    }
}
