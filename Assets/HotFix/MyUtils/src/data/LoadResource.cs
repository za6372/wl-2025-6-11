using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    public class LoadResource
    {
        //图片缓存 直接从本地拿会导致重复的Texture2D生成太多，这样不仅慢还占内存
        public static Dictionary<string, Sprite> pics = new Dictionary<string, Sprite>();

        /**判断ab包缓存是否已经存在,不存在返回null*/
        public static AssetBundle getFromAB(string k)
        {
            if (k.Equals("AssetBundles") || k.Equals("androidBundle")) k = "";
            //从AssetBundle找，不需要记录在缓存中
            IEnumerable<AssetBundle> abs = AssetBundle.GetAllLoadedAssetBundles();
            foreach (AssetBundle a in abs)
            {
                if (a.name.Equals(k)) return a;
            }
            return null;
        }
        /**清理上个地图申请的ab资源*/
        public static void clearAB()
        {
            IEnumerable<AssetBundle> abs = AssetBundle.GetAllLoadedAssetBundles();
            foreach (AssetBundle a in abs)
            {
                a.Unload(true);
            }
        }
        public static void clearSomeAB(params string[] names)
        {
            IEnumerable<AssetBundle> abs = AssetBundle.GetAllLoadedAssetBundles();
            for (int i = 0; i < names.Length; i++)
            {
                foreach (AssetBundle a in abs)
                {
                    if (a.name == names[i])
                    {
                        a.Unload(true);
                        break;
                    }

                }
            }

        }
        private static Dictionary<string, HashSet<string>> clearABRes = new Dictionary<string, HashSet<string>>();
        /**
         * 每次ui申请ab资源时调用
         * objId 实例的id
         names 实例所引用的ab资源
         */
        public static void addToClearABRes(string objId, params string[] names)
        {
            if (!clearABRes.ContainsKey(objId))
            {
                clearABRes.Add(objId, new HashSet<string>());
            }
            HashSet<string> list = clearABRes[objId];
            foreach (string n in names)
            {
                list.Add(n);
            }
        }
        /**依赖也一并卸下*/
        public static void clearABByMantisty(string objId)
        {
            if (!clearABRes.ContainsKey(objId)) return;
            string[] keys = clearABRes[objId].ToArray();
            clearABRes.Remove(objId);

            ABRefCount.getInstance().cutRef(keys, (list) =>
            {
                IEnumerable<AssetBundle> abs = AssetBundle.GetAllLoadedAssetBundles();
                for (int i = 0; i < list.Count; i++)
                {
                    foreach (AssetBundle a in abs)
                    {
                        //将引用计数为0的卸载
                        if (a != null && a.name == list[i])
                        {
                            a.Unload(true);
                            break;
                        }
                    }
                }
            });


        }




        public static Sprite getFromPics(string k)
        {
            Sprite a = null;
            pics.TryGetValue(k, out a);
            return a;
        }
        public static void putInPics(string k, Sprite t2d)
        {
            if (getFromPics(k) == null)
            {
                pics.Add(k, t2d);
            }
        }
        /**对加密的文件进行解密*/
        public static byte[] LoadAny(byte[] bs)
        {
            return strUtils.decodeBytes(strUtils.Inflate(bs), "qaz1997");
        }
        public static Sprite LoadTexture(byte[] bs)
        {
            //Texture2D texture = new Texture2D(w, h);
            Texture2D texture = copyTexture();
            texture.LoadImage(LoadAny(bs));
            Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        private static Texture2D copyTexture()
        {
            Texture2D t = new Texture2D(1, 1, Texture2D.whiteTexture.format, false);
            //Texture2D t = new Texture2D(sourceTexture.width, sourceTexture.height, sourceTexture.format, false);
            // 对于运行时纹理生成，也可以通过GetRawTextureData直接写入纹理数据，返回一个Unity.Collections.NativeArray
            // 这可以更快，因为它避免了 LoadRawTextureData 会执行的内存复制。
            //t.LoadRawTextureData(t.GetRawTextureData());
            //t.Apply();
            return t;
        }
        /**mat、assets等文件的读取*/
        public static object readRes(string path)
        {
            return Resources.Load(path);

        }
    }
}
