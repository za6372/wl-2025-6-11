using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/**将热更新代码的dll上传到服务器*/
public class HotDllToServer : MonoBehaviour
{
    [MenuItem("AssetBundle / dll上传到服务器")]
    static void uploadDll()
    {
        //工具类
        string path1 = "E:\\unity3d\\my\\wl-2025-6-11\\HybridCLRData\\HotUpdateDlls\\Android\\MyUtils.dll";
        string out1 = "E:\\xq-new\\server\\c_xq_server\\c_wl_server\\src\\main\\resources\\assets\\dll\\MyUtils";
        endcode(path1, out1);
        path1 = "E:\\unity3d\\my\\wl-2025-6-11\\HybridCLRData\\HotUpdateDlls\\Android\\MyRoute.dll";
        out1 = "E:\\xq-new\\server\\c_xq_server\\c_wl_server\\src\\main\\resources\\assets\\dll\\MyRoute";
        endcode(path1, out1);
        
        path1 = "E:\\unity3d\\my\\wl-2025-6-11\\HybridCLRData\\HotUpdateDlls\\Android\\xq2d.dll";
        out1 = "E:\\xq-new\\server\\c_xq_server\\c_wl_server\\src\\main\\resources\\assets\\dll\\xq2d";
        endcode(path1, out1);
        
        
       

        Debug.Log("dll上传成功");
    }

    static void endcode(string path, string outPath)
    {
        var data_bytes = File.ReadAllBytes(path);
        var key_bytes = System.Text.Encoding.UTF8.GetBytes("_jkl.1997");
        for (int i = 0; i < data_bytes.Length; i++)
        {
            data_bytes[i] = (byte)(data_bytes[i] ^ key_bytes[i % key_bytes.Length]);
        }
        data_bytes = Deflate(data_bytes, 1);
        File.WriteAllBytes(outPath, data_bytes);
    }
    static byte[] Deflate(byte[] input, int level)
    {
        MemoryStream mMemory = new MemoryStream();
        Deflater mDeflater = new Deflater(level);
        using (DeflaterOutputStream mStream = new DeflaterOutputStream(mMemory, mDeflater, 1024 * 1000))
        {
            mStream.Write(input, 0, input.Length);
        }

        return mMemory.ToArray();
    }

}
