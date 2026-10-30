using Assets.HotFix.MyUtils.src.common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    public class dbHandle
    {
        public static bool save(string key, object obj)
        {
            if (!key.Equals("user"))
            {
                key = sysUtils.projRootDir + "_" + strUtils.username + '_' + key;
            }
            string json = JsonConvert.SerializeObject(obj);
            byte[] a = strUtils.Deflate(Encoding.UTF8.GetBytes(json), 1);
            json = strUtils.byteArrToStr(a);
            //Debug.Log(key+"=>"+json);
            //string b64 = utils.common.zipFun(json);
            return write(key, json);
        }

        public static T get<T>(string key) where T : class
        {
            if (!key.Equals("user"))
            {
                if (strUtils.username == null)
                {
                    return default;
                }
                key = sysUtils.projRootDir + "_" + strUtils.username + '_' + key;
            }
            string json = read(key);
            try
            {
                byte[] b = strUtils.Inflate(strUtils.strToByte(json));
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(b));
            }
            catch (Exception e)
            {
                del(key);
            }

            return null;
        }

        private static bool write(string key, string json)
        {
            PlayerPrefs.SetString(key, json);
            return true;
        }
        private static string read(string key)
        {
            return PlayerPrefs.GetString(key);
        }
        public static void del(string key)
        {
            if (!key.Equals("user"))
            {
                if (strUtils.username == null)
                {
                    return;
                }
                key = sysUtils.projRootDir + "_" + strUtils.username + '_' + key;
            }
            PlayerPrefs.DeleteKey(key);
        }
    }
}
