using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Editor
{
    class SocketTest : MonoBehaviour
    {
        public string text = "xca 一段发送测试文本 spo@";
        public bool isSend = false;
        private Dictionary<IntPtr, socketMsg> socketMap = new Dictionary<IntPtr, socketMsg>();
        public bool isClose = false;
        private Socket wsClient;

        public bool isConectWs = false;
        public bool isSendWs = false;
        private void Start()
        {
            
        }
        private Socket connect()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IPAdress = "127.0.0.1";
            int nPort = Convert.ToInt32("9668");
            IPEndPoint IP = new IPEndPoint(IPAddress.Parse(IPAdress), nPort);
            try
            {
                clientSocket.Connect(IP);
            }
            catch (SocketException e)
            {
                Debug.Log("连接失败!\n" + e.ToString());
                return null;
            }
            return clientSocket;


            /*ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();*/

        }
        /**ws测试*/
        private void testWs()
        {
            if (this.wsClient != null) this.wsClient.Close();
            //获得一个新连接
            Socket client = connect();
            //接收处理
            receiveResp(client, (dataList) =>
            {

            });
            this.wsClient = client;
            //发送一段注册信息
            JObject obj = new JObject();
            obj.Add("path", "/RegClient/ws");
            obj.Add("pwd", "c90p");
            obj.Add("sbh", "test");
            string json = JsonConvert.SerializeObject(obj);
            sendWsMsg(json);
        }
        private void sendWsMsg(string str)
        {
            //发送请求头
            byte[] bs = getPostReqHead(this.wsClient, 1, str);
            this.wsClient.Send(bs);
        }
        /**断开连接测试*/
        private void testClose()
        {
            Socket client = connect();
            client.Close();
        }
        private void receiveData(Socket ClientSocket)
        {
            byte[] buffer = new byte[1024];
            ClientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ar =>
            {
                int received = ClientSocket.EndReceive(ar);
                if (received > 0)
                {
                    // string str = Encoding.UTF8.GetString(buffer, 0, received);
                    string str = Encoding.UTF8.GetString(buffer);
                    Debug.Log(received + "/" + str);
                }

                receiveData(ClientSocket);
            }, null);
        }
        /**短连接的请求头*/
        private void sendPost(string str)
        {
            //获得一个新连接
            Socket client = connect();
            //接收处理
            receiveResp(client, (dataList) =>
            {
                /*Debug.Log("========有响应========");
                byte[] bs = dataList.ToArray();
                //判断头响应状态 1就是允许 （服务器只返回两位的byte）
                Int16 status = ReadInt16(bs, 0);
                Debug.Log("头响应状态：" + status);
                if (status == 1)
                {
                    //创建内容体
                    //两位byte表示消息id

                    //开始发送消息内容

                }*/
            });
            //发送一段注册信息
            JObject obj = new JObject();
            obj.Add("path", "/WlGame/test/kk");
            obj.Add("data",str);
            obj.Add("sbh", "test");
            string json = JsonConvert.SerializeObject(obj);

            //发送请求头
            byte[] bs = getPostReqHead(client, 0, json);
            client.Send(bs);

            /*Debug.Log(ReadInt16(bs, 0));
            Debug.Log(ReadInt16(bs, 2));
            Debug.Log(ReadInt32(bs, 4));
            Debug.Log(ReadInt32(bs, 8));
            Debug.Log(ReadInt32(bs, 12));*/
        }
        /**接收响应*/
        private void receiveResp(Socket client, Action<object> call)
        {
            byte[] buffer = new byte[1028];
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ar =>
            {
                int received = client.EndReceive(ar);
                if (received > 0)//表示连接中可以接收数据
                {
                    Debug.Log(buffer[0] + "," + buffer[1] + "," + buffer[2] + "," + buffer[3]);
                    int status = ReadInt16(buffer, 0);
                    Debug.Log("响应状态码：" + status);
                    if (status == 200)//200表明客户端已经收到请求头，客户端可以开始发送消息
                    {
                        Debug.Log("======发送了========");
                        byteToServer(buffer, client);
                    }
                    else//数据响应
                    {
                        //List<byte> dataList;
                        //消息结构
                        //2个字节表示响应类型 0头响应 1数据响应
                        int respType = ReadInt16(buffer, 0);
                        if (respType == 0)
                        {
                            //创建响应的缓存
                            socketMsg scMsg = socketMap[client.Handle];

                            RespHead respHead = new RespHead();
                            respHead.respType = respType;
                            respHead.gkNum = ReadInt32(buffer, 2);
                            respHead.qf64kNum = ReadInt32(buffer, 6);
                            respHead.qf1kNum = ReadInt32(buffer, 10);
                            respHead.blNum = ReadInt32(buffer, 14);
                            respHead.msgId = ReadInt16(buffer, 18);

                            RespStruct respStruct = new RespStruct();
                            respStruct.respHead = respHead;
                            respStruct.contentList = new List<byte>();

                            scMsg.respMap[respHead.msgId] = respStruct;
                            Debug.Log("====创建响应缓存=====" + respHead.msgId + "/" + respHead.qf1kNum + "/" + respHead.blNum);

                        }
                        else
                        {
                            //2个字节表示消息id
                            int msgId = ReadInt16(buffer, 2);
                            Debug.Log("响应消息id：" + msgId);
                            socketMsg scMsg = socketMap[client.Handle];
                            RespStruct respStruct = scMsg.respMap[msgId];
                            Debug.Log("====响应缓存=====" + respStruct.respHead.msgId + "/" +
                                respStruct.respHead.qf1kNum + "/" + respStruct.respHead.blNum);
                            //1k内容
                            for (int i = 4; i < buffer.Length; i++)
                            {
                                respStruct.contentList.Add(buffer[i]);
                            }
                            respStruct.qf1kNum++;
                            if (respStruct.qf1kNum >= 64)
                            {
                                respStruct.qf64kNum++;
                            }
                            if (respStruct.qf64kNum > 16384)
                            {
                                respStruct.gkNum++;
                            }
                            socketMap[client.Handle] = scMsg;
                            if ((respStruct.respHead.gkNum == respStruct.gkNum) &&
                                (respStruct.respHead.qf64kNum == respStruct.qf64kNum) &&
                                ((respStruct.respHead.qf1kNum + 1) <= respStruct.qf1kNum))
                            {
                                string str = Encoding.UTF8.GetString(respStruct.contentList.ToArray());


                                //移除该消息id
                                scMsg.respMap.Remove(msgId);
                                Debug.Log(scMsg.respMap.Count() + "/响应数据接收完毕：" + str);
                            }
                        }
                        //dataList.AddRange(buffer.Take(received));
                        // string str = Encoding.UTF8.GetString(buffer, 0, received);



                    }
                    receiveResp(client, call);
                }
                else//表示已经断开或者发生错误
                {
                    Debug.Log("连接已经断开");
                    socketMap.Remove(client.Handle);
                }

            }, null);
        }
        private void byteToServer(byte[] buffer, Socket client)
        {
            int msgId = ReadInt16(buffer, 2);
            Debug.Log("=====消息Id：" + msgId);
            socketMsg scMsg = socketMap[client.Handle];
            MsgStruct mStr = scMsg.msgMap[msgId];
            //两个字节表明请求类型为 1数据请求
            List<byte> data = new List<byte>();
            data.Add(0);
            data.Add(1);
            //2字节 消息id
            data.Add(buffer[2]);
            data.Add(buffer[3]);
            //截取1k内容，发送后计数+1
            byte[] contentList = mStr.contentList;
            int index = mStr.gkNum * 1024 * 1024 * 1024 + mStr.qf64kNum * 64 * 1024 + mStr.qf1kNum * 1024;
            Debug.Log(index + "/" + contentList.Length);
            for (int i = index; i < (index + 1024); i++)
            {
                if (i >= contentList.Length)
                {
                    //补0
                    data.Add(0);
                }
                else
                {
                    data.Add(contentList[i]);
                }
            }
            Debug.Log("byte长度：" + contentList.Length + "/" + Encoding.UTF8.GetString(contentList));
            int len = client.Send(data.ToArray());
            if (len == data.ToArray().Length)
            {
                
                //计数++
                mStr.qf1kNum++;
                if (mStr.qf1kNum >= 64)
                {
                    mStr.qf64kNum++;
                }
                if (mStr.qf64kNum > 16384)
                {
                    mStr.gkNum++;
                }
                Debug.Log("======" + mStr.qf1kNum + "/" + mStr.reqHead.qf1kNum);
                socketMap[client.Handle] = scMsg;

                //每发送一段，服务器回回执一条已经收到的响应（响应状态码 200和msgId）
                if ((mStr.reqHead.gkNum == mStr.gkNum) && (mStr.reqHead.qf64kNum == mStr.qf64kNum) &&
                    ((mStr.reqHead.qf1kNum + 1) <= mStr.qf1kNum))
                {
                    //移除该消息id
                    scMsg.msgMap.Remove(msgId);
                    Debug.Log("已经全部发送完毕！剩余msgMap：" + scMsg.msgMap.Count);
                }
            }
            /*string t = "";
            foreach(byte b in data.ToArray())
            {
                t += b + ",";
            }
            Debug.Log(t);*/

        }
        /**获取一个未使用的id*/
        private int getNoUseId(Dictionary<int, MsgStruct> msgMap, int id)
        {
            if (msgMap.ContainsKey(id)) return getNoUseId(msgMap, id + 1);
            return id;
        }

        /**自定义请求头*/
        private byte[] getPostReqHead(Socket client, byte linkType, string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);

            //建立一个消息结构体（由socketId和msgId取），缓存请求头信息和消息内容，切片计数
            IntPtr socketId = client.Handle;
            if (!socketMap.ContainsKey(socketId))
            {
                socketMsg sm0 = new socketMsg();
                sm0.socketId = socketId;
                sm0.msgMap = new Dictionary<int, MsgStruct>();
                sm0.respMap = new Dictionary<int, RespStruct>();
                socketMap.Add(socketId, sm0);
            }
            socketMsg sm = socketMap[socketId];

            int n1 = 1024 * 1024 * 1024;
            int len1 = data.Length / n1;//不够1g就是0
            int sy1 = data.Length % n1;//剩余数据

            int n2 = 64 * 1024;//每64k数据作为一段
            int len2 = sy1 / n2;//按64k得到的分段数
            int sy2 = sy1 % n2;//剩余数据

            int n3 = 1 * 1024;//每1k数据作为一段
            int len3 = sy2 / n3;//按1k得到的分段数
            int sy3 = sy2 % n3;//剩余数据

            int num = n3 - sy3 % n3;

            MsgStruct mstr = new MsgStruct();
            mstr.contentList = data;
            mstr.gkNum = 0;
            mstr.qf64kNum = 0;
            mstr.qf1kNum = 0;
            mstr.blNum = 0;
            mstr.reqHead = new ReqHead();
            mstr.reqHead.reqType = 0;
            mstr.reqHead.linkType = linkType;
            mstr.reqHead.gkNum = len1;
            mstr.reqHead.qf64kNum = len2;
            mstr.reqHead.qf1kNum = len3;
            mstr.reqHead.blNum = num;
            mstr.reqHead.msgId = getNoUseId(sm.msgMap, 1);

            sm.msgMap.Add(mstr.reqHead.msgId, mstr);
            Debug.Log("请求头信息：" + mstr.reqHead.blNum);

            List<byte> bs = new List<byte>();
            //fixme:作为一个请求头，服务端先得到这个请求头才会允许客户端传输数据
            //2个字节表示请求类型 0头请求 1数据请求
            putByte(bs, 0, 0);
            //2个字节表示连接类型 0短连接 1长连接
            putByte(bs, 0, linkType);
            //4个字节表示数据块拆分，按G拆分（0到4294967295，1G是1073741824）
            byte[] tb = BitConverter.GetBytes(len1);
            // 如果你需要大端字节序，你可以这样处理：
            Array.Reverse(tb);
            bs.AddRange(tb);
            //4个字节表示64k切分数目
            tb = BitConverter.GetBytes(len2);
            Array.Reverse(tb);
            bs.AddRange(tb);
            //4个字节表示1k切分数目
            tb = BitConverter.GetBytes(len3);
            Array.Reverse(tb);
            bs.AddRange(tb);
            //最后一段可能不够64k长度，需要补0
            //4个字节记录补0数目
            //Debug.Log(data.Length + "/"+num + "/" + sy3 % n3);
            tb = BitConverter.GetBytes(num);
            Array.Reverse(tb);
            bs.AddRange(tb);
            //消息id
            byte[] id = numberToTowByte(mstr.reqHead.msgId);
            //Debug.Log("消息id：" + id[0]+"/"+id[1]);
            putByte(bs, id[0], id[1]);
            byte[] brr = bs.ToArray();


            return brr;
        }

        private void Update()
        {
            if (isSend)
            {
                isSend = false;
                sendPost(this.text);
            }
            if (isClose)
            {
                isClose = false;
                testClose();
            }
            if (isConectWs)
            {
                isConectWs = false;
                testWs();
            }
            if (isSendWs)
            {
                isSendWs = false;
                JObject obj = new JObject();
                obj.Add("path", "/WlGame/test/kk");
                obj.Add("data", this.text);
                obj.Add("sbh", "test");
                string json = JsonConvert.SerializeObject(obj);
                sendWsMsg(json);
            }
        }
        private byte[] numberToTowByte(int number)
        {
            byte[] twoBytes = new byte[2];
            // 确保数字不会超出2字节范围（65535）
            number &= 0xFFFF; // 使用掩码操作确保数字在0到65535之间
             // 将整数拆分为高字节和低字节
            twoBytes[0] = (byte)(number >> 8); // 高8位（右移8位）
            twoBytes[1] = (byte)(number & 0xFF); // 低8位（与操作保留低8位）
            return twoBytes;
        }
        private void putByte(List<byte> bs, params byte[] b)
        {
            bs.AddRange(b);
        }
        public Int16 ReadInt16(byte[] bytes, int index)
        {
            byte[] destinationArray = new byte[2];
            Array.Copy(bytes, index, destinationArray, 0, 2);
            // 反字节序
            Array.Reverse(destinationArray);
            return BitConverter.ToInt16(destinationArray, 0);
        }
        public Int32 ReadInt32(byte[] bytes, int index)
        {
            byte[] destinationArray = new byte[4];
            Array.Copy(bytes, index, destinationArray, 0, 4);
            // 反字节序
            Array.Reverse(destinationArray);
            return BitConverter.ToInt32(destinationArray, 0);
        }
    }
    class ReqHead//连接请求头
    {
        public int reqType;//2个字节 请求类型 0头请求 1数据请求
        public int linkType;//2个字节 连接类型 0短连接 1长连接
        public int gkNum;//4个字节表示数据块拆分，按G拆分（0到4294967295，1G是1073741824）
        public int qf64kNum;//4个字节 64k切分数
        public int qf1kNum;//4个字节 1k切分数
        public int blNum;//4个字节 补0数
        public int msgId;//2字节 消息id
    };
    class MsgStruct
    {
        public ReqHead reqHead;
        //这里只是计数，初始值为0，每发送一条+1
        public int gkNum;//g块计数
        public int qf64kNum;//64k计数
        public int qf1kNum;//1k计数
        public int blNum;//补零计数
        //当这四个计数都满足请求头的计数情况时，则表示发送完整（每发送一次就会++）
        //消息的具体内容（每次发送都要截取1k）
        public byte[] contentList;

    };
    class RespHead
    {
        public int respType;//2个字节 响应类型 0头响应 1数据响应
        public int gkNum;//4个字节表示数据块拆分，按G拆分（0到4294967295，1G是1073741824）
        public int qf64kNum;//4个字节 64k切分数
        public int qf1kNum;//4个字节 1k切分数
        public int blNum;//4个字节 补0数
        public int msgId;//2字节 消息id
    }
    class RespStruct
    {
        public RespHead respHead;
        //这里只是计数，初始值为0，每发送一条+1
        public int gkNum;//g块计数
        public int qf64kNum;//64k计数
        public int qf1kNum;//1k计数
        public int blNum;//补零计数
        //当这四个计数都满足请求头的计数情况时，则表示发送完整（每发送一次就会++）
        //消息的具体内容
        public List<byte> contentList;

    };

    class socketMsg
    {
        public IntPtr socketId;
        //消息id=》byte数据，同一时刻同个socket不可能发送超过整型范围的消息，所以用int即可
        public Dictionary<int, MsgStruct> msgMap;
        //响应的消息id=》  注意：响应id跟请求的消息id不一样，因为长连接可以由服务器主动发送，而不必经过客户端请求
        public Dictionary<int, RespStruct> respMap;
    };
}
