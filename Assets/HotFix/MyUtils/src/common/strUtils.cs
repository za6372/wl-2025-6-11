using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.common
{
    public class strUtils
    {
        public static string username;
        public static string password;
        public static string ip;
        //要求跟主项目的版本一致（LoadDll中）
        public static string apkVer = "ver=2026.1.21";

        private static System.Random rd = new System.Random();
        /**判断是否为数字*/
        public static bool isNumber(object str)
        {
            string a = str.ToString();
            try
            {
                int.Parse(a);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        /**字符串转int*/
        public static int strToInt(object str)
        {
            string a = str.ToString();
            if (a.Contains("."))
            {
                a = a.Split('.')[0];
            }
            return int.Parse(a);
        }
        /**比较两个float是否相等*/
        public static bool isSameFloat(float a, float b)
        {
            const float epsilon = 0.1f;
            return Math.Abs(a - b) < epsilon;
        }
        /**距离指定时间多久 */
        public static string nowTimeToEndTime(long nowTime, long endTime)
        {
            long dis = endTime - nowTime;
            if (dis < 0) dis = 0;
            int d = Convert.ToInt32(dis / 1000 / 60 / 60 / 24);
            int hour = (int)(dis / 1000 / 60 / 60 % 24) + d * 24;
            int min = (int)(dis / 1000 / 60 % 60);
            int sec = (int)(dis / 1000 % 60);
            return (hour < 10 ? ("0" + hour) : hour + "") + ":" + (min < 10 ? ("0" + min) : min + "") + ":" + (sec < 10 ? ("0" + sec) : sec + "");
        }
        /**
	    * 将毫秒转格式时间
	    */
        public static string sToTime(long timestamp)
        {
            long begtime = timestamp * 10000;
            DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
            long time_tricks = tricks_1970 + begtime;//日志日期刻度
            DateTime dt = new DateTime(time_tricks);//转化为DateTime
            return dt.ToShortDateString() + " " + dt.ToLongTimeString();
        }
        /**匹配正则表达式 "[A-Za-z0-9]"*/
        public static bool isMatch(string str, string reg)
        {
            if (new Regex("^" + reg + "$").IsMatch(str))
            {
                return true;
            }
            return false;
        }
        /**判断是否满足字符长度*/
        public static bool isEnoughLen(string str, int min, int max)
        {
            if (isNull(str) || str.Length < min || str.Length > max) return false;
            return true;
        }
        /**判断是否为空*/
        public static bool isNull(object str)
        {
            if (str == null || str.ToString().Trim().Equals("")) return true;
            return false;
        }
        /**vector转json*/
        public static JObject vectorToJSONObject(Vector3 vc)
        {
            return vectorToJSONObject(vc.x, vc.y, vc.z);
        }
        public static JObject vectorToJSONObject(float x, float y, float z)
        {
            JObject a = new JObject();
            a.Add("x", x);
            a.Add("y", y);
            a.Add("z", z);
            return a;
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
        /**一段byte数据，其字符串用，隔开*/
        public static byte[] strToByte(string str)
        {
            string[] arr = str.Split(',');
            byte[] bs = new byte[arr.Length];
            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (byte)int.Parse(arr[i]);
            }
            return bs;
        }
        /**byte数据转,隔开的字符串*/
        public static string byteArrToStr(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                sb.Append(bs[i] + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /**压缩*/
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
        /**对byte数据进行解密*/
        public static byte[] decodeBytes(byte[] data_bytes, string pwd)
        {
            if (data_bytes == null) return null;
            var key_bytes = System.Text.Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < data_bytes.Length; i++)
            {
                data_bytes[i] = (byte)(data_bytes[i] ^ key_bytes[i % key_bytes.Length]);
            }
            return data_bytes;
        }
        /**16进制转rgb*/
        public static Color toRGBColor(string color)
        {
            //#000000
            Color NowColor;
            ColorUtility.TryParseHtmlString(color, out NowColor);
            return NowColor;
        }
        /**获取当前时间戳-毫秒*/
        public static long getMillis()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        public static string getId()
        {
            return getMillis() + "" + getRandom(0, 10) + getRandom(0, 10) + getRandom(0, 10);
        }
        /**获取随机数*/
        public static int getRandom(int min, int max)
        {
            return rd.Next(min, max);
        }
        public static void UnZip(string path, string outPath, Action<float> progressCallback, Action compileCallback)
        {
            // 生成解压目录
            /*if (!Directory.Exists(outPath + "pic\\"))
            {
                Directory.CreateDirectory(outPath + "pic\\");
            }*/

            ZipInputStream s = new ZipInputStream(File.OpenRead(path));
            long i = 0;
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
                if (directoryName.Length > 0)
                {
                    string dirName = Path.Combine(outPath, directoryName);
                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);

                }
                if (fileName != String.Empty)
                {
                    // 解压文件到指定的目录
                    string fn = Path.Combine(outPath, directoryName, fileName); // 目标文件路径

                    FileStream streamWriter = File.Create(fn);

                    int size = 2048;
                    byte[] data = new byte[size];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();


                    i++;
                    progressCallback(1f * i);
                }
            }
            s.Close();
            compileCallback();
        }
        /**合并数组*/
        public static T[] concatArr<T>(T[] arr1, T[] arr2)
        {

            return arr1.Concat(arr2).ToArray();
        }
        /**json复制*/
        public static T copyJSON<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }
        public static T strToJSONObj<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public static Int16 ReadInt16(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(2);
            // 反字节序
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        public static Int32 ReadInt32(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            // 反字节序
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        public static Int16 ReadInt16(byte[] bytes, int index)
        {
            byte[] destinationArray = new byte[2];
            Array.Copy(bytes, index, destinationArray, 0, 2);
            // 反字节序
            Array.Reverse(destinationArray);
            return BitConverter.ToInt16(destinationArray, 0);
        }
        public static Int32 ReadInt32(byte[] bytes, int index)
        {
            byte[] destinationArray = new byte[4];
            Array.Copy(bytes, index, destinationArray, 0, 4);
            // 反字节序
            Array.Reverse(destinationArray);
            return BitConverter.ToInt32(destinationArray, 0);
        }
        /**对文件夹的复制*/
        public static void CopyDir(string sourceDir, string destinationDir)
        {
            // 确保目标目录存在
            Directory.CreateDirectory(destinationDir);

            // 复制文件（非递归）
            foreach (var file in Directory.EnumerateFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, true); // 第三个参数设置为true以覆盖已存在的文件
            }

            // 复制子目录（非递归）
            foreach (var dir in Directory.EnumerateDirectories(sourceDir))
            {
                string destDir = Path.Combine(destinationDir, new DirectoryInfo(dir).Name);
                CopyDir(dir, destDir); // 非递归复制子目录，如果要递归，去掉注释并恢复原始递归调用形式即可。
            }
        }
        /**对文件夹的移动*/
        public static void MoveDir(string sourceDir, string destinationDir)
        {
            // 确保目标目录存在
            Directory.CreateDirectory(destinationDir);
            //移动文件
            foreach (var file in Directory.EnumerateFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Move(file, destFile);
            }

            // 移动目录
            foreach (var dir in Directory.EnumerateDirectories(sourceDir))
            {
                string destDir = Path.Combine(destinationDir, new DirectoryInfo(dir).Name);
                Directory.Move(dir, destDir);
            }
        }
    }
}
