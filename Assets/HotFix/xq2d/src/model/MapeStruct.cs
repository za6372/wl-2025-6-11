using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    public class BuildFrame
    {
        public int id;
        public int w;
        public int h;
        public int x;
        public int y;
        public int t0;
        public int t1;

        public BuildFrame(int id, int w, int h, int x, int y, int t0, int t1)
        {
            this.id = id;
            this.w = w;
            this.h = h;
            this.x = x;
            this.y = y;
            this.t0 = t0;
            this.t1 = t1;
        }
    }
    public class Build
    {
        public string aefName;
        public string pwdName;
        public BuildFrame[] buildFrames;
    }
    public class NpcData
    {
        public string mark;
        public int x;
        public int y;
        public int w;
        public int h;
        public int t0;
        public int t1;

        public NpcData(string mark, int x, int y, int w, int h, int t0, int t1)
        {
            this.mark = mark;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.t0 = t0;
            this.t1 = t1;
        }
        private string[] sp()
        {
            return mark.Split("#");
        }
        public string getPicName()
        {
            string[] arr = sp();
            return arr[0];
        }
        public string getNpcName()
        {
            string[] arr = sp();
            if (arr.Length == 1) return arr[0];
            return arr[1].Split(",")[0];
        }
        public string getNpcId()
        {
            string[] arr = sp();
            if (arr.Length == 1) return arr[0];
            return arr[1].Split(",")[1];
        }
    }
    public class Npc
    {
        public string picName;
        public List<NpcData> arr;
    }
    public class Mape
    {
        public string fileName;
        public Color[] bgColorArr;
        public int row;
        public int col;
        public int kw;
        public int kh;
        public string[] pics;
        public int[] arr;
        public Build build;
        public Npc npc;
        public string mapKey;

        public void setRGBA(byte r, byte g, byte b, byte a, int width, int height)
        {
            Color32 bgColor = new Color32(r, g, b, a);
            Color[] bgColorArr = new Color[16 * 16];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bgColorArr[y * width + x] = bgColor;
                }
            }
            this.bgColorArr = bgColorArr;
        }
        public void toConsole()
        {
            Debug.Log(fileName);
            Debug.Log(pics.Length);
            Debug.Log(arr.Length);
        }
    }
}
