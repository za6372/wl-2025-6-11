using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.HotFix.MyUtils.src.factory
{
    public class gameObjPool
    {
        private static gameObjPool one;
        private static Dictionary<int, GameObject> objMap = new Dictionary<int, GameObject>();
        private static Dictionary<int, status> statusMap = new Dictionary<int, status>();

        public static gameObjPool getInstance()
        {
            if (one == null)
            {
                one = new gameObjPool();
            }
            return one;
        }
        /**判断池中是否存在这个id对应的对象*/
        public bool isExist(int Id)
        {
            status st = null;
            return statusMap.TryGetValue(Id, out st);
        }
        /**对象是否被激活*/
        public bool isActice(int Id)
        {
            GameObject obj = objMap[Id];
            return obj.activeSelf;
        }
        private void put(GameObject g)
        {
            objMap.Add(g.GetInstanceID(), g);
            statusMap.Add(g.GetInstanceID(), new status());
        }

        /**通过id获取池中对象*/
        public GameObject getById(int Id)
        {
            status st = statusMap[Id];
            if (st == null)
            {
                Debug.Log("对象已被销毁，无法通过id获取");
                return null;
            }
            st.setUsing(true);
            GameObject obj = objMap[Id];
            obj.SetActive(true);
            return obj;
        }
        /**获取一个未使用的*/
        public GameObject get(string name, params Type[] t)
        {
            GameObject obj = null;
            foreach (int id in statusMap.Keys)
            {
                status st = statusMap[id];
                if (st.getIsUing()) continue;
                obj = objMap[id];
                break;
            }
            if (obj == null)
            {
                //进行创建
                obj = createEmptyObj(name);
                put(obj);
            }
            //设置为使用中
            statusMap[obj.GetInstanceID()].setUsing(true);
            obj.name = name;
            //能够取到，直接初始化
            for (int i = 0; i < t.Length; i++)
            {
                Component cp;
                if (!(cp = obj.GetComponent(t[i])))
                    cp = obj.AddComponent(t[i]);
                //判断是否继承ui基类
                if (t[i].IsSubclassOf(typeof(BaseUI)))
                {
                    //调取其初始化方法
                    BaseUI bui = (BaseUI)cp;
                    bui.init();
                }
            }
            obj.SetActive(true);
            
            return obj;
        }
        /**标记对象空闲（包括子对象）*/
        public void free(GameObject obj)
        {
            //先标记子的，再标记自己
            freeChildren(obj);

            obj.SetActive(false);
            //标记空闲
            status stu;
            if (!statusMap.TryGetValue(obj.GetInstanceID(), out stu))
            {
                //不被管理的直接销毁
                SetGameObj.remGameObject(obj.gameObject);
                return;
            }
            statusMap[obj.GetInstanceID()].setUsing(false);

            //分离父子层次关系，并且销毁关联的behaviour
            obj.transform.SetParent(GameObject.Find("main").transform.Find("free"));
            remAllComponent(obj.transform);
        }
        /**标记该对象的所有子对象为空闲*/
        
        public void freeChildren(GameObject parent)
        {
            freeChildren(parent, GameObject.Find("main").transform.Find("free"));
        }
        /**标记包含的gameobject为未使用*/
        private void freeChildren(GameObject parent, Transform free)
        {
            if (parent.transform.childCount == 0) return;
            //当前对象
            Transform ts = parent.transform.GetChild(0);
            //当前对象还有子对象
            if (ts.childCount > 0)
            {
                freeChildren(ts.gameObject);
            }
            ts.gameObject.SetActive(false);
            //Debug.Log(ts.gameObject);
            //todo:注意：输入框的光标是不被池管理的，预制体创建的对象也是不被管理的，这样的直接销毁
            status stu;
            if (!statusMap.TryGetValue(ts.gameObject.GetInstanceID(), out stu))
            {
                SetGameObj.remGameObject(ts.gameObject);
                freeChildren(parent);
                return;
                //不在池中那就添加进池中
                /*statusMap.Add(ts.gameObject.GetInstanceID(), new status());
                objMap.Add(ts.gameObject.GetInstanceID(), ts.gameObject);*/
            }
            statusMap[ts.gameObject.GetInstanceID()].setUsing(false);
            //从父级中移动到空闲区，但不销毁。每执行一次这个childCount就会少1
            ts.SetParent(free);
            //销毁关联的behaviour
            remAllComponent(ts);
            freeChildren(parent);
        }
        private void remAllComponent(Transform ts)
        {
            //重置位置变换
            resetTransform(ts);
            Component[] arr = ts.GetComponents<Component>();
            //由后往前删
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Component c = arr[i];
                //RectTransform不能被删除
                if (c.GetType().Name == "RectTransform" ||
                    c.GetType().Name == "Transform")
                {
                    continue;
                }

                //Debug.Log("移除" + arr[j].GetType().Name);
                SetGameObj.remComponent(c);
            }

        }
        private void resetTransform(Transform ts)
        {
            RectTransform rc = ts.GetComponent<RectTransform>();
            Transform rc0 = ts.GetComponent<Transform>();
            if (rc0 != null)
            {
                rc0.position = Vector2.zero;
                rc0.localPosition = Vector2.zero;
                rc0.rotation = Quaternion.Euler(Vector3.zero);
                rc0.localRotation = Quaternion.Euler(Vector3.zero);
                rc0.localScale = Vector3.one;
            }
            if (rc != null)
            {
                rc.sizeDelta = Vector2.zero;
                rc.anchorMax = Vector2.one / 2f;
                rc.anchorMin = Vector2.one / 2f;
                rc.pivot = Vector2.one / 2f;
                rc.anchoredPosition = Vector2.zero;
                rc.position = Vector2.zero;
                rc.localPosition = Vector2.zero;
                rc.rotation = Quaternion.Euler(Vector3.zero);
                rc.localRotation = Quaternion.Euler(Vector3.zero);
                rc.localScale = Vector3.one;
            }

        }
        /**定时任务，销毁*/
        public void startDestroyTask()
        {

        }
        /**销毁长时间不使用的对象*/
        public void destoryObj()
        {
            Transform ts = GameObject.Find("main").transform.Find("free");
            for (int i = 0; i < ts.childCount; i++)
            {
                GameObject g = ts.GetChild(i).gameObject;
                if (!statusMap[g.GetInstanceID()].isExpire()) continue;
                statusMap.Remove(g.GetInstanceID());
                objMap.Remove(g.GetInstanceID());
                SetGameObj.remGameObject(g);
                i--;
            }
            Debug.Log("池中剩余：" + objMap.Count);
            /*foreach(int k in objMap.Keys)
            {
                Debug.Log(objMap[k].name);
            }*/
        }

        /**创建空对象*/
        private GameObject createEmptyObj(string name)
        {
            GameObject a = new GameObject(name);
            return a;
        }
        //where T:new()指明了创建T的实例时应该具有构造函数。
        //一般情况下,无法创建一个泛型类型参数的实例。
        //然而,new()约束改变了这种情况,要求类型参数必须提供一个无参数的构造函数
        /**只增加单个组件*/
        /*private GameObject createUI<T>(string name) where T : new()
        {
            GameObject a = createEmptyObj(name);
            Type t = typeof(T);
            a.AddComponent(t);
            return a;
        }*/

    }
    /**对象的一些状态信息*/
    class status
    {
        //是否正处于使用中
        private bool isUsing;
        //创建时间
        private long created;

        public status()
        {

        }
        /**设置使用时会刷新时间*/
        public void setUsing(bool b)
        {
            this.isUsing = b;
            this.created = strUtils.getMillis();
        }
        public bool getIsUing()
        {
            return this.isUsing;
        }
        /**是否过期（超过10分钟不用）*/
        public bool isExpire()
        {
            if (!this.isUsing && strUtils.getMillis() - this.created > 10 * 60 * 1000L)
            {
                return true;
            }
            return false;
        }
    }
}
