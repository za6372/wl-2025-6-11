using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyRoute.src
{
    public class strUtils
    {
        private static string projRootDir = "zhangtianai";
        /**获取资源目录*/
        public static string getUrl()
        {
            string path = Application.streamingAssetsPath + "/" + projRootDir;
            //string path = Application.dataPath + "/Res/apple/xqol_/resource";
            if (isMobile()) path = Application.persistentDataPath + "/" + projRootDir;
            return path;
        }
        public static string md5Endcode(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组并计算其哈希值
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将哈希值转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        /**将dll写入本地*/
        public static void writeDll(byte[] dll,string fileName)
        {
            string appPath = getUrl();
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            File.WriteAllBytes(appPath + "/" + md5Endcode(fileName), dll);
        }
        public static byte[] readDll(string fileName)
        {
            string appPath = getUrl();
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            return File.ReadAllBytes(appPath + "/" + md5Endcode(fileName));
        }
        /**获取dll文件大小*/
        public static int getDllSize(string fileName)
        {
            string appPath = getUrl();
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            if (!File.Exists(appPath + "/" + md5Endcode(fileName)))
            {
                return 0;
            }
            return File.ReadAllBytes(appPath + "/" + md5Endcode(fileName)).Length;
        }
        /**判断是否为移动端*/
        public static bool isMobile()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return true;
            }
            return false;
        }
        /**删除该目录的文件，不删除该目录*/
        public static void DeleteAllFile(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteAllFile(dir);
            }

        }
        /**压缩
         注意：图片中有很多不识别的字符，会导致压缩失败，所以文件显示很小
         */
        public static byte[] Deflate(byte[] input, int level)
        {
            MemoryStream mMemory = new MemoryStream();
            Deflater mDeflater = new Deflater(level);
            using (DeflaterOutputStream mStream = new DeflaterOutputStream(mMemory, mDeflater, 1024 * 1000))
            {
                mStream.Write(input, 0, input.Length);
            }
            return mMemory.ToArray();
        }
        /**解压*/
        public static byte[] Inflate(byte[] input)
        {
            MemoryStream mMemory = new MemoryStream();
            using (InflaterInputStream mStream = new InflaterInputStream(new MemoryStream(input)))
            {
                Int32 mSize;
                byte[] mWriteData = new byte[1024 * 4];
                while (true)
                {
                    mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                    if (mSize > 0)
                        mMemory.Write(mWriteData, 0, mSize);
                    else
                        break;
                }
            }
            return mMemory.ToArray();
        }
    }
}
