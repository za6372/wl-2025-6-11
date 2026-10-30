using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static PwdStruct;

namespace Assets.HotFix.xq2d.src.data
{
    /**npc\玩家等模型*/
    class readModel
    {
        public static List<GameObject> drawGameObj(Transform ts, string[] pwdNames, string[] aefNames)
        {

            Dictionary<int, PWDInfo> dic = getPwdT2dDic(pwdNames);
            List<PwdObjData> list = read(pwdNames, aefNames);
            return getGameList(ts, dic, list);
        }
        private static List<GameObject> getGameList(Transform ts, Dictionary<int, PWDInfo> dic, List<PwdObjData> list)
        {
            List<GameObject> objs = new List<GameObject>();
            for (int i = 0; i < list.Count; i++)
            {
                PwdObjData pwdObjData = list[i];
                GameObject g = gameObjPool.getInstance().get("test", typeof(SimpleUI));
                g.transform.SetParent(ts, false);
                g.transform.localPosition = Vector2.zero;
                GameObject frames = gameObjPool.getInstance().get("frames", typeof(SimpleUI));
                frames.transform.SetParent(g.transform, false);
                frames.transform.localPosition = Vector2.zero;

                AniRuntime aniRuntime = g.AddComponent<AniRuntime>();
                aniRuntime.frameObjs = new GameObject[pwdObjData.frameCnt];
                

                List<PwdObjFrame> pwdObjFrameList = pwdObjData.pwdObjFrameList;

                foreach (PwdObjFrame pwdObjFrame in pwdObjFrameList)
                {
                    GameObject frame = gameObjPool.getInstance().get(pwdObjFrame.name, typeof(SimpleUI));
                    frame.transform.SetParent(frames.transform, false);
                    frame.transform.localPosition = new Vector2(0, 0);

                    List<PwdObjFrameSprite> pwdObjFrameSpriteList = pwdObjFrame.pwdObjFrameSpriteList;
                    foreach (PwdObjFrameSprite sprite in pwdObjFrameSpriteList)
                    {
                        PWDInfo pwdInfo = dic[sprite.pwdId];
                        Texture2D td = pwdInfo.tex;

                        Vector2 spritePos = sprite.pos;

                        GameObject img = gameObjPool.getInstance().get("part", typeof(SimpleUI));
                        img.AddComponent<Image>().sprite = Sprite.Create(td,
                            new Rect(sprite.rect.x, td.height - sprite.rect.height - sprite.rect.y, sprite.rect.width, sprite.rect.height), new Vector2(0.5f, 0.5f));
                        img.transform.SetParent(frame.transform, false);
                        img.transform.localPosition = new Vector2(spritePos.x + sprite.rect.width / 2f, -spritePos.y - sprite.rect.height / 2f);
                        img.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
                    }
                    aniRuntime.frameObjs[pwdObjFrame.frameId] = frame;
                    frame.SetActive(pwdObjFrame.frameId == 0);
                }

                objs.Add(g);
            }
            return objs;
        }

        private static List<PwdObjData> read(string[] pwdFilePaths, string[] aefFilePaths)
        {

            Dictionary<int, PWDInfo> dic = new Dictionary<int, PWDInfo>();
            foreach (string pwdFilePath in pwdFilePaths)
            {
                PWDInfo pwdInfo = ReadPWD(pwdFilePath);
                if (pwdInfo == null) continue;
                dic.Add(pwdInfo.id, pwdInfo);
            }

            List<PwdObjData> list = new List<PwdObjData>();

            foreach (string aefFilePath in aefFilePaths)
            {
                AEFInfo aefInfo = ReadAEF(aefFilePath);
                PwdObjData pwdObjData = new PwdObjData();
                pwdObjData.frameCnt = aefInfo.frameCnt;
                List<PwdObjFrame> pwdObjFrameList = new List<PwdObjFrame>();

                foreach (var frame in aefInfo.frameInfo)
                {
                    PwdObjFrame pwdObjFrame = new PwdObjFrame();
                    pwdObjFrame.frameId = frame.frameId;
                    pwdObjFrame.name = "frame_" + frame.frameId;

                    List<PwdObjFrameSprite> pwdObjFrameSprites = new List<PwdObjFrameSprite>();
                    for (int i = 0; i < frame.frameSpriteInfo.Length; i++)
                    {
                        var frameSpriteInfo = frame.frameSpriteInfo[i];

                        PWDInfo pwdInfo;
                        if (!dic.TryGetValue(frameSpriteInfo.pwdId, out pwdInfo))
                        {

                            continue;
                        }
                        if((frameSpriteInfo.spriteId - 1)>= pwdInfo.spriteInfoList.Count)
                        {
                            Debug.Log("spriteInfoList下标溢出：" + frameSpriteInfo.pwdId);
                            continue;
                        }
                        SpriteInfo spriteInfo = pwdInfo.spriteInfoList[frameSpriteInfo.spriteId - 1];



                        PwdObjFrameSprite pwdObjFrameSprite = new PwdObjFrameSprite();
                        pwdObjFrameSprite.name = "sprite_" + frameSpriteInfo.spriteId;
                        pwdObjFrameSprite.pwdId = frameSpriteInfo.pwdId;
                        pwdObjFrameSprite.rect = new RectInt(spriteInfo.x, spriteInfo.y, spriteInfo.width, spriteInfo.height);
                        pwdObjFrameSprite.pos = new Vector2Int(frameSpriteInfo.x, frameSpriteInfo.y);
                        pwdObjFrameSprite.size = new Vector2Int(spriteInfo.width, spriteInfo.height);
                        pwdObjFrameSprites.Add(pwdObjFrameSprite);
                    }

                    pwdObjFrame.pwdObjFrameSpriteList = pwdObjFrameSprites;
                    pwdObjFrameList.Add(pwdObjFrame);
                }
                pwdObjData.pwdObjFrameList = pwdObjFrameList;
                list.Add(pwdObjData);
            }
            return list;

        }

        private static Dictionary<int, PWDInfo> getPwdT2dDic(string[] pwdFilePaths)
        {

            Dictionary<int, PWDInfo> dic = new Dictionary<int, PWDInfo>();

            foreach (string pwdFilePath in pwdFilePaths)
            {
                PWDInfo pwdInfo = ReadPWD(pwdFilePath);
                if (pwdInfo == null) continue;
                Texture2D td = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                td.wrapMode = TextureWrapMode.Clamp;
                td.filterMode = FilterMode.Point;
                td.LoadImage(pwdInfo.png);
                td.Apply();
                pwdInfo.tex = td;
                dic.Add(pwdInfo.id, pwdInfo);

            }
            return dic;
        }


        private static AEFInfo ReadAEF(string aefFilePath)
        {
            byte[] bs = DoGet.getInstance().readAny(aefFilePath);
            AEFInfo aefInfo = new AEFInfo();
            using (MemoryStream fs = new MemoryStream(bs))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    aefInfo.frameCnt = ReadInt16(br);
                    aefInfo.frameInfo = new FrameInfo[aefInfo.frameCnt];
                    for (int i = 0; i < aefInfo.frameCnt; i++)
                    {
                        FrameInfo frameInfo = new FrameInfo();
                        int id = ReadInt16(br);
                        frameInfo.frameId = i;

                        frameInfo.pngCnt = ReadInt32(br);
                      
                        frameInfo.frameSpriteInfo = new FrameSpriteInfo[frameInfo.pngCnt];
                        for (int j = 0; j < frameInfo.pngCnt; j++)
                        {
                            FrameSpriteInfo spriteInfo = new FrameSpriteInfo();
  
                            spriteInfo.pwdId = ReadInt16(br);
                            spriteInfo.spriteId = ReadInt16(br);
                   
                            spriteInfo.x = ReadInt16(br);
                            spriteInfo.y = ReadInt16(br);

                            frameInfo.frameSpriteInfo[j] = spriteInfo;
      
                        }
                        
                        aefInfo.frameInfo[i] = frameInfo;
                    }


                }
            }
            return aefInfo;
        }

        private static PWDInfo ReadPWD(string pwdFilePath)
        {
            byte[] bs = DoGet.getInstance().readAny(pwdFilePath);
            if (bs == null)
            {
                Debug.Log(pwdFilePath + "读取为null");
                return null;
            }
            PWDInfo pwdInfo = new PWDInfo();
            using (MemoryStream fs = new MemoryStream(bs))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    pwdInfo.id = ReadInt16(br);
                    pwdInfo.pngLen = ReadInt32(br);

                    pwdInfo.png = br.ReadBytes(pwdInfo.pngLen);


                    int spriteCnt = ReadInt16(br);

                    List<SpriteInfo> list = new List<SpriteInfo>();
                    for (int i = 0; i < spriteCnt; ++i)
                    {
                        SpriteInfo spriteInfo = new SpriteInfo();
                        spriteInfo.index = i;
                        spriteInfo.x = ReadInt16(br);
                        spriteInfo.y = ReadInt16(br);

                        spriteInfo.width = ReadInt16(br);
                        spriteInfo.height = ReadInt16(br);
                        list.Add(spriteInfo);
                    }
                    pwdInfo.spriteInfoList = list;
                }
            }
            return pwdInfo;
        }
        private static Int16 ReadInt16(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(2);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        private static Int32 ReadInt32(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
