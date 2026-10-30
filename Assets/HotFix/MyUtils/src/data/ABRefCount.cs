using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    /**对ab包的引用计数*/
    public class ABRefCount
    {
        private static ABRefCount one;
        public static ABRefCount getInstance()
        {
            if (one == null) one = new ABRefCount();
            return one;
        }
        private Dictionary<string, int> dic = new Dictionary<string, int>();
        private void addRef(string abName)
        {
            if (!dic.ContainsKey(abName))
            {
                dic.Add(abName, 0);
            }
            dic[abName] += 1;
        }
        private bool cutRef(string abName)
        {
            if (!dic.ContainsKey(abName))
            {
                Debug.Log("不存在资源："+abName);
                return false;
            }
            dic[abName] -= 1;
            if (dic[abName] <= 0)
            {
                dic.Remove(abName);
                return true;
            }
            return false;
        }
        public void addRef(List<string> names)
        {
            foreach (string n in names)
            {
                addRef(n);
            }
        }
        public void cutRef(string[] names, Action<List<string>> ac)
        {
            //根据Mantisty找到相关的依赖
            DoGet.getInstance().getAssetBundleManifest(names, (abNames) =>
            {
                //对引用为0的资源名返回
                List<string> list = new List<string>();
                for (int i = 0; i < abNames.Count; i++)
                {
                    string n = abNames[i];
                    if (cutRef(n))
                    {
                        list.Add(n);
                    }
                }
                ac(list);
            });

        }
    }
}
