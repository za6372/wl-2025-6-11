using Assets.Res.script;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.wl
{
    class CreateMiYao
    {
        [MenuItem("Tool / 创建ip密钥")]
        static void createIpMiYao()
        {
            //对指定IP进行des加密
            string ip = "127.0.0.1:9668";
            //"101.126.32.173:9668";
            //"192.168.1.2:9668";
            JObject j = new JObject();
            j["ip"] = ip;
            j["time"] = strUtils.getMillis();
            string json = JsonConvert.SerializeObject(j);
            string DESKEY_SIMPLE = "ca#!pq1*";
            string str = strUtils.EncodeDES(json, DESKEY_SIMPLE);
            string base64 = strUtils.EncodeB64(str);
            Debug.Log(base64);
            var dummyData = base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");//.Replace("_", "/").Replace("-", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            string des = strUtils.DecodeB64(dummyData.Trim('\0'));
            JObject d = JsonConvert.DeserializeObject<JObject>(strUtils.DecodeDES(des, DESKEY_SIMPLE));
            Debug.Log(d);
            //Mjk1QTI1RjQxMDlCOEMyRjY2OTlCM0NBMkFEMEIyMThEMEFBNjFBMUM5OTlERjY5NzA3NzI0QjRGQkU2NDA1ODM0RjUwRTBCOERDMERGMjdENDEyNTJGQThBMDUwNjE2QzU5RDgwRjcwOEQ2OUU3NA
        }
    }
}
