using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.HotFix.MyUtils.src.common
{
    public class netAbstract
    {

        public static netAbstract one;
        public static netAbstract getInstance()
        {
            if (one == null) one = new netAbstract();
            return one;
        }
        public string sbh;
        public string secret;
        //java只会取前8位
        public string DESKEY_SIMPLE = "9pc8.#ik";
        public ClientWebSocket ws;
        public CancellationToken ct;
        private List<object> byList = new List<object>();
        //登录后的角色数据（仅选角时使用，选完后销毁）
        public JObject body;

        //无需登录后才能请求的路径
        private List<string> paths;

        public Action wsCloseCall;
        public Action<JObject> recvMsgCall;

        public Action<int> reqErrCall;

        public void sendVoice(string path, JObject msg)
        {

            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("url", path);
            obj.Add("json", msg);
            obj.Add("sbh", this.sbh);

            string json = JsonConvert.SerializeObject(obj);

            string d = strUtils.EncodeB64(json);
            d = 10086 + d;
            var sendResult = ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(d)), WebSocketMessageType.Binary, true, ct);
            sendResult.Wait();
        }
        public void sendWs(string path, JObject msg)
        {
           
            Task.Run(() =>
            {
                JObject obj = new JObject();
                obj.Add("url", path);
                obj.Add("json", msg);
                obj.Add("sbh", this.sbh);
                string json = JsonConvert.SerializeObject(obj);
                string str = EncodeDES(json, DESKEY_SIMPLE);
                string base64 = strUtils.EncodeB64(str);
                byte[] bs = Encoding.UTF8.GetBytes(base64);
                sendWs(bs);
                
            });
        }
        
        private void sendWs(byte[] bs)
        {
            ws.SendAsync(new ArraySegment<byte>(bs), WebSocketMessageType.Binary, true, ct);
        }

        public async void openWs(Action callback)
        {
            this.ws = new ClientWebSocket();
            this.ct = new CancellationToken();
            ws.Options.SetRequestHeader("user-agent", "Android");
            string a = "?" + secret + "&" + strUtils.username + "&" + sbh + "&" + sysUtils.projRootDir;
            Uri url = new Uri("ws://" + strUtils.ip + "/qwe" + a);
            await ws.ConnectAsync(url, ct);
            callback();
            //startReceive是while，不会继续往下走
            //await startReceive();
            await Task.Run(startReceive);


        }
        public void Dispose()
        {

            if (ws != null && ws.State != WebSocketState.Closed)
            {
                ws?.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, ct).Wait();
                //ws.Dispose();
            }

            byList.Clear();
            //one = null;
        }
        

        public async Task startReceive()
        {
            while (true)
            {
                

                if (ws.State != WebSocketState.Open)
                {
                    wsCloseCall();
                    return;
                }
                

                try
                {
                    //todo:好像有问题，当同时接收到多组数据时可能会数据错乱（byList同一个导致）？
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result = await ws.ReceiveAsync(buffer, new CancellationToken());
                    //WebSocketReceiveResult result = ws.ReceiveAsync(buffer, new CancellationToken()).Result;
                    byList.Add(buffer);
                    //接收完毕
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        wsCloseCall();
                        return;
                    }
                    else if (result.EndOfMessage)
                    {
                        ReceiveOver();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                    if (e is WebSocketException)
                    {
                        WebSocketException ae = (WebSocketException)e;

                        if (ae.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                        {
                            wsCloseCall();
                            return;
                        }
                    }
                }

            }
        }
        private void ReceiveOver()
        {
            int num = 0;
            for (int i = 0; i < byList.Count; i++)
            {
                num += ((ArraySegment<byte>)byList[i]).Array.Length;
            }
            List<byte> arr = new List<byte>();
            for (int i = 0; i < byList.Count; i++)
            {
                byte[] s = ((ArraySegment<byte>)byList[i]).Array;
                for (int j = 0; j < s.Length; j++)
                {
                    arr.Add(s[j]);
                }
            }
            string b64 = Encoding.UTF8.GetString(arr.ToArray());
            //Debug.Log(byList.Count + " 接收：" + b64);
            byList.Clear();

            var dummyData = b64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");//.Replace("_", "/").Replace("-", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            //Debug.Log("b64解析：" + dummyData);
            string des = strUtils.DecodeB64(dummyData.Trim('\0'));
            //Debug.Log("des：" + des);
            string js = DecodeDES(des, DESKEY_SIMPLE);
            //Debug.Log("des解析后数据:" + js);
            //todo:json转换异常
            JObject res = JsonConvert.DeserializeObject<JObject>(js);
            //Debug.Log(res);
            

            recvMsgCall(res);
        }

        
        public netAbstract addSkipWsOpenCheck(params string[] path)
        {
            if (paths == null) paths = new List<string>();
            paths.AddRange(path);
            return this;
        }
        public IEnumerator sendPost(string path, object obj, Action<object> result, Action failCall = null)
        {

            //请求前验证是否连接ws
            if (paths != null && !paths.Contains(path))
            {
                //一定要确保ws先启动
                if (this.ws.State != WebSocketState.Open)
                {
                    Debug.Log("ws未启动导致：" + path);
                    //网络断开，进行重连
                    //netErrorUI.getInstance().show();
                    MsgRecHandle.setNetErr();
                    yield return null;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sbh", sbh);
            dic.Add("created", strUtils.getMillis());
            if (obj != null)
                dic.Add("json", obj);
            dic.Add("url", path);
            string json = JsonConvert.SerializeObject(dic);
            //Debug.Log(json);
            string str = EncodeDES(json, DESKEY_SIMPLE);

            string base64 = strUtils.EncodeB64(str);

            UnityWebRequest uwr = UnityWebRequest.PostWwwForm("http://" + strUtils.ip, base64);
            yield return uwr.SendWebRequest();

            if (uwr.error != null)
            {
                Debug.Log("net error =>" + path + "=>" + uwr.error);

                if (failCall != null)
                {
                    failCall();
                }
                else
                {
                    reqErrCall(0);
                }
            }
            else
            {

                string des = strUtils.DecodeB64(uwr.downloadHandler.text);
                string js = DecodeDES(des, DESKEY_SIMPLE);
                JObject res = JsonConvert.DeserializeObject<JObject>(js);
                //Dictionary<string, object> res =JsonConvert.DeserializeObject<Dictionary<string,object>>(js);
                if ((int)res["code"] == 200)
                {
                    if (result != null) result(res["msg"]);
                }
                else
                {
                    reqErrCall((int)res["code"]);
                }


            }
        }


        public string EncodeDES(string str, string key)
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

        public string DecodeDES(string str, string key)
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
        /**获取本机ip*/
        public string GetIP()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adater in adapters)
            {
                if (adater.Supports(NetworkInterfaceComponent.IPv4))
                {
                    UnicastIPAddressInformationCollection UniCast = adater.GetIPProperties().UnicastAddresses;
                    if (UniCast.Count > 0)
                    {
                        foreach (UnicastIPAddressInformation uni in UniCast)
                        {
                            if (uni.Address.AddressFamily == AddressFamily.InterNetwork)

                            {
                                Debug.Log(uni.Address.ToString());
                                return uni.Address.ToString();
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
