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
public class readDirAll
{
    

    public static List<GameObject> drawBuildAm(Transform ts, string[] pwdNames, string[] aefNames, float scale = 1f)
    {

        Dictionary<int, PWDInfo> dic = getPwdT2dDic(pwdNames);
        List<PwdObjData> list = read(pwdNames, aefNames, scale);
        return drawBuildAm(ts, dic, list);
    }
    private static List<GameObject> drawBuildAm(Transform ts, Dictionary<int, PWDInfo> dic, List<PwdObjData> list)
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < list.Count; i++)
        {
            PwdObjData pwdObjData = list[i];
            GameObject g = gameObjPool.getInstance().get("test", typeof(SimpleUI));
            g.transform.SetParent(ts, false);

            AniRuntime aniRuntime = g.AddComponent<AniRuntime>();
            aniRuntime.frameObjs = new GameObject[pwdObjData.frameCnt];

            Vector2 pwdSize = pwdObjData.getSize();
            SetGameObj.setSizePos(pwdSize, Vector2.zero, g);
            //g.transform.position = Vector2.zero;
            Vector2 pwdNoScaleSize = pwdObjData.getNoScaleSize();
            //构造下一层
            List<PwdObjFrame> pwdObjFrameList = pwdObjData.pwdObjFrameList;
            foreach (PwdObjFrame pwdObjFrame in pwdObjFrameList)
            {
                GameObject p = gameObjPool.getInstance().get(pwdObjFrame.name, typeof(SimpleUI));
                p.transform.SetParent(g.transform, false);
                //Vector2 pSize = pwdObjFrame.getSize();
                //居中显示
                Vector2 pPos = pwdObjFrame.getPos();

                Vector2 fSize = pwdObjFrame.getNoScaleSize();

                SetGameObj.setSize(fSize * pwdObjFrame.scale, p);
                RectTransform rt = p.GetComponent<RectTransform>();
                //轴心设为左下方
                rt.pivot = new Vector2(0, 0);
                //假设原图原点以投影（18*8）为参考，unity的原点坐标就是(-9,-4)
                Vector2 pBottomPos = pwdObjFrame.getBottomPos();
                p.transform.localPosition = new Vector2(-pwdSize.x / 2f, -pwdSize.y / 2f);

                //构建最后一层
                List<PwdObjFrameSprite> pwdObjFrameSpriteList = pwdObjFrame.pwdObjFrameSpriteList;
                //将多张小图混合成一张大图
                //fSize = new Vector2(300, 300);
                Texture2D frameImg = new Texture2D((int)fSize.x, (int)fSize.y, TextureFormat.RGBA32, false);
                frameImg.wrapMode = TextureWrapMode.Clamp;
                frameImg.filterMode = FilterMode.Point;
                //以透明色填充
                Color[] pixels = new Color[(int)(fSize.x * fSize.y)];
                for (int tt = 0; tt < pixels.Length; tt++)
                {
                    pixels[tt] = new Color(0, 0, 0, 0); // R, G, B 都为0，A为0表示透明
                }
                frameImg.SetPixels(pixels);
                //先获取所有拼接位置最左上角的坐标，后续要以这个坐标作为原点来绘制大图
                int dx = pwdObjFrameSpriteList[0].pos.x, dy = pwdObjFrameSpriteList[0].pos.y;
                foreach (PwdObjFrameSprite sprite in pwdObjFrameSpriteList)
                {
                    PWDInfo pwdInfo = dic[sprite.pwdId];
                    if (dx > sprite.pos.x) dx = sprite.pos.x;
                    if (dy < sprite.pos.y) dy = sprite.pos.y;
                }
                foreach (PwdObjFrameSprite sprite in pwdObjFrameSpriteList)
                {
                    PWDInfo pwdInfo = dic[sprite.pwdId];
                    Texture2D td = pwdInfo.tex;
                    //将部位绘制到帧图上
                    for (int y = 0; y < sprite.size.y; ++y)
                    {
                        for (int x = 0; x < sprite.size.x; ++x)
                        {
                            int pxX = (sprite.pos.x - dx + x);
                            int pxY = (sprite.pos.y - dy + (int)fSize.y - y - 1);
                            //绘制像素点时是以左下角为原点
                            //获取像素点颜色
                            var color = td.GetPixel(sprite.rect.x + x, td.height - sprite.rect.y - y - 1);
                            if (color.a == 0) continue;
                            frameImg.SetPixel(pxX, pxY, color);
                        }
                    }
                }


                frameImg.Apply();
                p.AddComponent<Image>().sprite = Sprite.Create(frameImg, new Rect(0, 0, fSize.x, fSize.y), new Vector2(0.5f, 0.5f));
                aniRuntime.frameObjs[pwdObjFrame.frameId] = p;
                p.SetActive(pwdObjFrame.frameId == 0);
            }

            objs.Add(g);
        }
        return objs;
    }

    

    private static List<PwdObjData> read(string[] pwdFilePaths, string[] aefFilePaths, float scale)
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
            pwdObjData.scale = scale;
            List<PwdObjFrame> pwdObjFrameList = new List<PwdObjFrame>();

            foreach (var frame in aefInfo.frameInfo)
            {
                PwdObjFrame pwdObjFrame = new PwdObjFrame();
                pwdObjFrame.frameId = frame.frameId;
                pwdObjFrame.scale = scale;
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

                    SpriteInfo spriteInfo = pwdInfo.spriteInfoList[frameSpriteInfo.spriteId - 1];


                    PwdObjFrameSprite pwdObjFrameSprite = new PwdObjFrameSprite();
                    pwdObjFrameSprite.name = "sprite_" + frameSpriteInfo.spriteId;
                    pwdObjFrameSprite.pwdId = frameSpriteInfo.pwdId;
                    pwdObjFrameSprite.rect = new RectInt(spriteInfo.x, spriteInfo.y, spriteInfo.width, spriteInfo.height);
                    pwdObjFrameSprite.scale = scale;
                    pwdObjFrameSprite.pos = new Vector2Int(frameSpriteInfo.x,
                        -frameSpriteInfo.y);
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

    /**获得图库*/
    public static Dictionary<int, PWDInfo> getPwdT2dDic(string[] pwdFilePaths)
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


    public static AEFInfo ReadAEF(string aefFilePath)
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

    public static PWDInfo ReadPWD(string pwdFilePath)
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
