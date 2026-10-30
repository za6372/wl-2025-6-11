using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PwdStruct 
{
    public class AEFInfo
    {
        public int frameCnt;
        public FrameInfo[] frameInfo;
        public string toConsole()
        {
            string a = "AEFInfo: " + frameCnt + "\n";
            for (int i = 0; i < frameInfo.Length; i++)
            {
                a += frameInfo[i].toConsole() + "\n";
            }
            return a;
        }
    }

    public class FrameInfo
    {
        public int frameId;
        public int pngCnt;
        public FrameSpriteInfo[] frameSpriteInfo;
        public string toConsole()
        {
            string a = "FrameInfo: " + frameId + "," + pngCnt + "\n";
            for (int i = 0; i < frameSpriteInfo.Length; i++)
            {
                a += frameSpriteInfo[i].toConsole() + "\n";
            }
            return a;
        }
    }

    public class FrameSpriteInfo
    {
        public int pwdId;
        public int spriteId;
        public int x;
        public int y;
        public string toConsole()
        {
            return "FrameSpriteInfo: " + pwdId + "," + spriteId + "," + x + "," + y;
        }
    }
    public class PWDInfo
    {
        public short id;  
        public int pngLen; 
        public byte[] png; 
        public int splitCnt;  
        public List<SpriteInfo> spriteInfoList;
        public Texture2D tex;

        public string toConsole()
        {
            string a = id + "," + pngLen + "," + png + "," + splitCnt + "\n";
            foreach (SpriteInfo p in spriteInfoList)
            {
                a += p.toConsole() + "\n";
            }

            return a;
        }
    }
    
    public class SpriteInfo
    {
        public int index;
        public int key;
        public int x; 
        public int y;  
        public int width; 
        public int height;
        public string toConsole()
        {
            return index + "," + key + "," + x + "," + y + "," + width + "," + height;
        }

    }
}
