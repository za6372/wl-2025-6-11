using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.common
{
    public class SetGameObj
    {
       
        
        public static T AddOneComponent<T>(GameObject g)
        {
            addComponent(g, typeof(T));
            return g.GetComponent<T>();
        }
        /**当组件不存在时添加组件*/
        public static Component addComponent(GameObject obj, params Type[] t)
        {
            for (int i = 0; i < t.Length; i++)
            {
                if (!obj.GetComponent(t[i]))
                    obj.AddComponent(t[i]);
            }
            //返回第一个
            return obj.GetComponent(t[0]);
        }

        //第一次点击的时间
        private static long firstClkTime = 0;
        //默认延时
        private static long defaultDelay = 100;
        public static void addClk(GameObject g, Action callback, bool isDelay = true)
        {
            if (g.GetComponent<Button>())
            {
                g.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            else
            {
                g.gameObject.AddComponent<Button>();
            }
            g.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                long now = strUtils.getMillis();
                if (isDelay && (now - firstClkTime < defaultDelay))
                {
                    Debug.Log("点击频繁");
                    return;
                }
                firstClkTime = now;
                callback();
            });

        }
        public static void setScaleByVector(GameObject obj, Vector3 v)
        {
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            rt.localScale = v;
        }
        public static void setScaleByK(GameObject obj, float k = 1)
        {
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            rt.localScale = Vector3.one * k;
        }
        public static void setSize(Vector2 size, GameObject obj)
        {
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            rt.sizeDelta = size;
        }

        /**这是相对于可视区的中心*/
        public static void setScreenCenter(GameObject obj)
        {
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            Vector2 size = rt.sizeDelta;
            setLeftPos(new Vector2((ScreenUtils.width - size.x) / 2f, -(ScreenUtils.height - size.y) / 2f), obj);
        }
        /**这是调整到屏幕中心位置*/
        public static void setLayerCenter(GameObject obj)
        {
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            Vector2 size = rt.sizeDelta;
            float w = Screen.width / ScreenUtils.screenScaleRate;
            float h = Screen.height / ScreenUtils.screenScaleRate;
            setLeftPos(new Vector2((w - size.x) / 2f, -(h - size.y) / 2f), obj);
        }


        /**仅仅是隐藏这个组件，而不丢入空闲区*/
        public static void hide(GameObject gameObject)
        {
            gameObject.SetActive(false);
            //不用去变更池中status使用状态，假如变更成未使用，那其他ui取用时有可能拿到这个组件
        }
        /**将组件分解并放入空闲区*/
        public static void free(GameObject gameObject)
        {
            gameObjPool.getInstance().free(gameObject);
        }


        public static void addLongClk(GameObject gameObject, Action callback, bool isOnce = false, Action endCallback = null)
        {
            touchLong tl = gameObject.AddComponent<touchLong>();
            tl.longClkCallback = callback;
            tl.endCallback = endCallback;
            tl.isOnce = isOnce;

        }
        /**设置轴心*/
        public static void setPivot(Vector2 pos, GameObject gameObject)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            rt.pivot = pos;
        }

        public static void setPos(Vector2 pos, GameObject gameObject)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            rt.localPosition = pos;
        }

        /**设置ui位置及大小*/
        public static void setSizePos(Vector2 sizeDelta, Vector2 v, GameObject gameObject)
        {
            RectTransform rt = (RectTransform)addComponent(gameObject, typeof(RectTransform));
            rt.sizeDelta = sizeDelta;
            setLeftPos(v, gameObject);

            
        }
        /**以左上角为0点设置的位置*/
        public static void setLeftPos(Vector2 pos, GameObject obj)
        {
            //有些ui组件会带有RectTransform
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            Vector2 size = rt.sizeDelta;
            size.x = size.x / 2f;
            size.y = -size.y / 2f;
            rt.anchoredPosition = size;
            rt.anchorMax = Vector2.up;
            rt.anchorMin = Vector2.up;
            Vector2 v2 = rt.anchoredPosition;
            v2.x += pos.x;
            v2.y += pos.y;
            rt.anchoredPosition = v2;
        }
        public static void setLeftButtomPos(Vector2 pos, GameObject obj)
        {
            //有些ui组件会带有RectTransform
            RectTransform rt = (RectTransform)addComponent(obj, typeof(RectTransform));
            //左上对齐
            Vector2 size = rt.sizeDelta;
            size.x = size.x / 2f;
            size.y = -size.y / 2f;
            rt.anchoredPosition = size;
            rt.anchorMax = Vector2.right;
            rt.anchorMin = Vector2.right;
            //位置
            Vector2 v2 = rt.anchoredPosition;
            v2.x += pos.x;
            v2.y += pos.y;
            rt.anchoredPosition = v2;
        }
        public static void findAndRemChild(Transform ts)
        {
            
            while (ts.childCount > 0)
            {
                remGameObject(ts.GetChild(0).gameObject);
            }
        }
        /**发现并移除子项(销毁级别,父项不移除)*/
        private static void findAndRemChild(string path)
        {
            Transform ts = GameObject.Find(path).transform;
            findAndRemChild(ts);
        }
        /**销毁单个物体*/
        public static void remGameObject(GameObject obj)
        {
            if (obj != null)
                GameObject.DestroyImmediate(obj);
        }

        private static void remGameObject(Transform ts)
        {
            if (ts != null) remGameObject(ts.gameObject);
        }
        /**移除子物体及本身*/
        public static void remAllChildAndSelf(GameObject obj)
        {
            if (obj == null) return;
            findAndRemChild(obj.transform);
            remGameObject(obj);
        }
        /**移除组件*/
        private static void remComponent(Behaviour t)
        {
            if (t != null)
                GameObject.DestroyImmediate(t, true);
        }
        public static void remComponent(Component t)
        {
            if (t != null)
                GameObject.DestroyImmediate(t, true);
        }
        /**默认放于父节点内*/
        public static void putSence(Transform tf, Transform parent, bool isWorld = false)
        {
            //将子组件放入指定组件
            tf.SetParent(parent, isWorld);
        }
        public static void putSence(Transform tf, string path, bool isWorld = false)
        {
            tf.SetParent(GameObject.Find(path).transform, isWorld);
        }
        
        

    }
}
