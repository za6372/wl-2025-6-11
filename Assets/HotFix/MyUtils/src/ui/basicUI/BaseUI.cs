using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{
    /** 所有ui的顶级类*/
    public abstract class BaseUI : UIBehaviour
    {
        public abstract void init();
        public T GetOneComponent<T>()
        {
            return SetGameObj.AddOneComponent<T>(this.gameObject);
        }
        //第一次点击的时间
        private long firstClkTime = 0;
        //默认延时
        private long defaultDelay = 100;


        /**仅仅是隐藏这个组件，而不丢入空闲区*/
        public void hide()
        {
            SetGameObj.hide(this.gameObject);

        }
        /**将组件分解并放入空闲区*/
        public void free()
        {
            string id = this.GetInstanceID().ToString();

            if (this!= null)
                SetGameObj.free(this.gameObject);
            LoadResource.clearABByMantisty(id);
        }

        /**添加点击事件*/
        public BaseUI addClk(Action callback, bool isDelay = true)
        {
            SetGameObj.addClk(this.gameObject, callback, isDelay);

            return this;
        }
        public BaseUI addLongClk(Action callback, bool isOnce = false, Action endCallback = null)
        {
            SetGameObj.addLongClk(this.gameObject, callback, isOnce, endCallback);

            return this;
        }
        /**设置轴心*/
        public void setPivot(Vector2 pos)
        {
            SetGameObj.setPivot(pos, this.gameObject);
        }
        public BaseUI setSize(Vector2 size)
        {
            SetGameObj.setSize(size, this.gameObject);

            return this;
        }
        public BaseUI setPos(Vector2 pos)
        {
            SetGameObj.setPos(pos, this.gameObject);
            return this;
        }
        /**以左上角为0点设置的位置*/
        public BaseUI setLeftPos(Vector2 pos)
        {
            SetGameObj.setLeftPos(pos, this.gameObject);

            return this;
        }
        /**设置ui位置及大小*/
        public BaseUI setSizePos(Vector2 sizeDelta, Vector2 v)
        {
            SetGameObj.setSizePos(sizeDelta, v, this.gameObject);
            return this;
        }
        public BaseUI setScreenCenter()
        {
            SetGameObj.setScreenCenter(this.gameObject);
            return this;
        }
        public BaseUI setLayerCenter()
        {
            SetGameObj.setLayerCenter(this.gameObject);
            return this;
        }
        public T putSence<T>(Transform parent, bool isWorld = false)
        {
            SetGameObj.putSence(this.transform, parent, isWorld);
            return this.GetComponent<T>();
        }


    }
}
