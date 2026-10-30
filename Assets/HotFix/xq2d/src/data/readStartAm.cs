using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static PwdStruct;

namespace Assets.Res.script.src.data
{
    /**加载启动动画*/
    class readStartAm
    {
        public static GameObject read(Action<GameObject> callback)
        {
            string[] pwds =
            {
                "start_am_1_pwd","start_am_2_pwd", "start_am_3_pwd", "start_am_4_pwd", "start_am_5_pwd",
                "start_am_6_pwd", "start_am_7_pwd", "start_am_8_pwd", "start_am_9_pwd", "start_am_10_pwd"
            };
            string[] aefs =
            {
                "start_am_cv_aef", "start_am_l_aef", "start_am_load_aef"
            };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(ImgUI));
            bg.GetComponent<ImgUI>().setColor("#000000")
                .setSize(new Vector2(ScreenUtils.width, ScreenUtils.height));
            DoGet.getInstance().collectionAnyRes(() =>
            {

                List<GameObject> list = drawGameObj(bg.transform, pwds, aefs);
                float scale = ScreenUtils.width / 320f;
                for (int i = 0; i < list.Count; i++)
                {
                    GameObject g = list[i].gameObject;
                    g.transform.localScale = Vector2.one * scale;
                    Vector2 dd = TextureUtils.countZhenPicSize(g);
                    Vector2 vs = dd * scale;
                    g.transform.localPosition = new Vector2(-ScreenUtils.width / 2f, vs.y / 2f);
                }
                list[0].gameObject.SetActive(false);

                list[1].gameObject.SetActive(true);

                list[2].gameObject.SetActive(false);
                GameObject.DestroyImmediate(list[2].gameObject);
                AniRuntime am = list[1].gameObject.GetComponent<AniRuntime>();
                am.setCall(() =>
                {
                    list[0].gameObject.SetActive(true);
                    GameObject.DestroyImmediate(list[1].gameObject);
                    if (callback != null) callback(bg);
                });
                list[1].gameObject.GetComponent<AniRuntime>().playOnce();

            }, all);
            return bg;
        }
        public static GameObject readLoadingAm(Action<GameObject> callback)
        {
            string[] pwds =
            {
                "start_am_1_pwd","start_am_2_pwd", "start_am_3_pwd", "start_am_4_pwd", "start_am_5_pwd",
                "start_am_6_pwd", "start_am_7_pwd", "start_am_8_pwd", "start_am_9_pwd", "start_am_10_pwd"
            };
            string[] aefs =
            {
                "start_am_load_aef"
            };
            List<string> all = new List<string>();
            all.AddRange(pwds);
            all.AddRange(aefs);
            GameObject bg = gameObjPool.getInstance().get("bg", typeof(SimpleUI));
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = drawGameObj(bg.transform, pwds, aefs);
                GameObject g = list[0];
                Transform fs= g.transform.Find("frame_0");
                for(int i = 0; i < fs.childCount; i++)
                {
                    Transform t= fs.GetChild(i);
                    t.localPosition = new Vector2(t.localPosition.x, t.localPosition.y + 167);
                }
                g.SetActive(true);
                float scale = ScreenUtils.width / 320f;

                g.transform.localScale = Vector2.one * scale;

                Vector2 vs = TextureUtils.countZhenPicSize(g) * scale;
                g.transform.localPosition = new Vector2(-ScreenUtils.width / 2f, (ScreenUtils.height - vs.y) / 2f);
                SetGameObj.setSize(Vector2.zero, bg);

                callback(g);
            }, all);
            return bg;
        }



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

                AniRuntime aniRuntime = g.AddComponent<AniRuntime>();
                aniRuntime.frameObjs = new GameObject[pwdObjData.frameCnt];
                g.transform.localPosition = Vector2.zero;

                List<PwdObjFrame> pwdObjFrameList = pwdObjData.pwdObjFrameList;

                foreach (PwdObjFrame pwdObjFrame in pwdObjFrameList)
                {
                    GameObject frame = gameObjPool.getInstance().get(pwdObjFrame.name, typeof(SimpleUI));
                    frame.transform.SetParent(g.transform, false);
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
                        if ((frameSpriteInfo.spriteId - 1) >= pwdInfo.spriteInfoList.Count)
                        {
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
                // 解析PWD文件
                PWDInfo pwdInfo = ReadPWD(pwdFilePath);
                if (pwdInfo == null) continue;
                //临时生成大图
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
