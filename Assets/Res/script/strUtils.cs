using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using UnityEngine;

namespace Assets.Res.script
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
        /**获取当前时间戳-毫秒*/
        public static long getMillis()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
       
        public static string EncodeDES(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(str);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                StringBuilder builder = new StringBuilder();
                foreach (byte num in stream.ToArray())
                {
                    builder.AppendFormat("{0:X2}", num);
                }
                stream.Close();
                return builder.ToString();
            }
            catch (Exception) { return "xxxx"; }
        }

        public static string DecodeDES(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < (str.Length / 2); i++)
                {
                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                    buffer[i] = (byte)num2;
                }
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream.Close();
                return Encoding.GetEncoding("UTF-8").GetString(stream.ToArray());
            }
            catch (Exception) { return ""; }
        }
        public static string EncodeB64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return EncodeB64(bytes);
        }
        public static string EncodeB64(byte[] bytes)
        {
            var result = Convert.ToBase64String(bytes, 0, bytes.Length);
            //根据需要是否替换或者移除
            return result.Replace("+", "-").Replace("/", "_").Replace("\r|\n", "").Replace("=", "").ToString();
        }
        public static byte[] DecodeB64Byte(string input)
        {
            //TIPS:java base64字符处理 根据需要是否替换或者移除
            var converted = input.Replace("-", "+").Replace("_", "/");
            return Convert.FromBase64String(converted);
        }
        public static string DecodeB64(string input)
        {
            return Encoding.UTF8.GetString(DecodeB64Byte(input));
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
        public static void writeDll(byte[] dll, string fileName)
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
            if (!Directory.Exists(targetDir)) return;
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
