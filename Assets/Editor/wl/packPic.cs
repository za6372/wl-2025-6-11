using Assets.Res.script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class packPic
    {

        
       
        [MenuItem("AssetBundle / 服务端资源文件打包 / xq2d")]
        static void pack()
        {
            string cachePath = "E:\\unity3d\\my\\wl-2025-6-11\\Assets\\StreamingAssets\\xq2d\\";
            strUtils.DeleteAllFile(cachePath);
            string path = "E:\\xq-new\\server\\c_xq_server\\c_wl_server\\src\\main\\resources\\assets\\xq2d\\";
            work(path);
            Debug.Log("完成");
        }
        
        private static void work(string path)
        {
            strUtils.DeleteAllFile(path + "picout\\");

            string[] files = Directory.GetFiles(path + "pic\\", "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                String[] t = Path.GetFileName(file).Split('.');
                String name = t[0] + "_" + t[1];
                var data_bytes = File.ReadAllBytes(file);
                var key_bytes = System.Text.Encoding.UTF8.GetBytes("qaz1997");
                for (int p = 0; p < data_bytes.Length; p++)
                {
                    data_bytes[p] = (byte)(data_bytes[p] ^ key_bytes[p % key_bytes.Length]);
                }
                data_bytes = strUtils.Deflate(data_bytes, 1);



                File.WriteAllBytes(path + "picout\\" + name, data_bytes);
            }
        }
    }
}
