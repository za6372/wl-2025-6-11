using Assets.HotFix.xq2d.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    /**整个由帧对象组合而成的对象*/
    public class PwdObjData
    {
        public int frameCnt;
        //大小
        private Vector2 size;
        public float scale;
        public List<PwdObjFrame> pwdObjFrameList;
        public string name;
        public Vector2 getSize()
        {
            List<Vector4> arr = new List<Vector4>();
            for (int i = 0; i < pwdObjFrameList.Count; i++)
            {
                PwdObjFrame p = pwdObjFrameList[i];
                Vector2 size = p.getSize();
                Vector2 pos = p.getPos();
                arr.Add(new Vector4(pos.x, pos.y, size.x, size.y));
            }
            this.size = getAllPicGround(arr);
            return this.size;
        }
        public Vector2 getNoScaleSize()
        {
            List<Vector4> arr = new List<Vector4>();
            for (int i = 0; i < pwdObjFrameList.Count; i++)
            {
                PwdObjFrame p = pwdObjFrameList[i];
                Vector2 size = p.getNoScaleSize();
                Vector2 pos = p.getNoScalePos();
                arr.Add(new Vector4(pos.x, pos.y, size.x, size.y));
            }
            return getAllPicGround(arr);
        }
        private Vector2 getAllPicGround(List<Vector4> arr)
        {
            float minX = arr[0].x;//取最小
            float maxX = arr[0].x;//取最大
            float minY = arr[0].y;//取
            float maxY = arr[0].y;
            for (int p = 0; p < arr.Count; p++)
            {
                var a = arr[p];
                if (a.x < minX) minX = a.x;
                if ((a.x + a.z) > maxX) maxX = a.x + a.z;
                if (a.y < minY) minY = a.y;
                if ((a.y + a.w) > maxY) maxY = a.y + a.w;
            }
            //Debug.Log("范围："+leftX + "/" + rightX + "/" + topY + "/" + bottomY);
            return new Vector2(Math.Abs(maxX - minX), Math.Abs(maxY - minY));
        }
    }
    public class PwdObjFrame
    {
        public int frameId;
        //大小
        private Vector2 size;
        private Vector2 pos;
        public float scale;
        public List<PwdObjFrameSprite> pwdObjFrameSpriteList;
        public string name;
        //帧图位置
        public Vector2 framePos;
        /***/
        public Vector2 getSize()
        {
            List<Vector4> arr = new List<Vector4>();
            for (int i = 0; i < pwdObjFrameSpriteList.Count; i++)
            {
                PwdObjFrameSprite p = pwdObjFrameSpriteList[i];
                arr.Add(new Vector4(p.pos.x * p.scale, -p.pos.y * p.scale, p.size.x * p.scale, p.size.y * p.scale));
                //Debug.Log(p.pos.x * p.scale + "/" + p.pos.y * p.scale + "/" + p.size.x * p.scale + "/" + p.size.x * p.scale);
            }
            this.size = getAllPicGround(arr);
            //Debug.Log(name + " 合成大小：" + this.size);
            return this.size;
        }
        public Vector2 getNoScaleSize()
        {
            List<Vector4> arr = new List<Vector4>();
            for (int i = 0; i < pwdObjFrameSpriteList.Count; i++)
            {
                PwdObjFrameSprite p = pwdObjFrameSpriteList[i];
                arr.Add(new Vector4(p.pos.x, -p.pos.y, p.size.x, p.size.y));
                //Debug.Log(p.pos.x + "/" + p.pos.y + "/" + p.size.x + "/" + p.size.y);
            }
            this.size = getAllPicGround(arr);
            //Debug.Log(name + " 合成大小：" + this.size);
            return this.size;
        }
        /**y轴为正数*/
        private Vector2 getAllPicGround(List<Vector4> arr)
        {
            float minX = arr[0].x;//取最小
            float maxX = arr[0].x;//取最大
            float minY = arr[0].y;//取
            float maxY = arr[0].y;
            for (int p = 0; p < arr.Count; p++)
            {
                var a = arr[p];
                if (a.x < minX) minX = a.x;
                if ((a.x + a.z) > maxX) maxX = a.x + a.z;
                if (a.y < minY) minY = a.y;
                if ((a.y + a.w) > maxY) maxY = a.y + a.w;
            }
            //Debug.Log("范围："+leftX + "/" + rightX + "/" + topY + "/" + bottomY);
            return new Vector2(Math.Abs(maxX - minX), Math.Abs(maxY - minY));
        }
        public Vector2 getNoScalePos()
        {
            //取PwdObjFrameSprite小图左上角
            List<Vector2> arr = new List<Vector2>();
            for (int i = 0; i < pwdObjFrameSpriteList.Count; i++)
            {
                PwdObjFrameSprite p = pwdObjFrameSpriteList[i];
                arr.Add(new Vector2(p.pos.x , p.pos.y ));
            }
            return numberUtils.getMinXY(arr);
        }
        public Vector2 getPos()
        {
            //取PwdObjFrameSprite小图左上角
            List<Vector2> arr = new List<Vector2>();
            for (int i = 0; i < pwdObjFrameSpriteList.Count; i++)
            {
                PwdObjFrameSprite p = pwdObjFrameSpriteList[i];
                arr.Add(new Vector2(p.pos.x * p.scale, p.pos.y * p.scale));
            }
            this.pos = numberUtils.getMinXY(arr);
            return this.pos;
        }
        public Vector2 getBottomPos()
        {
            //取PwdObjFrameSprite小图左下角
            List<Vector2> arr = new List<Vector2>();
            for (int i = 0; i < pwdObjFrameSpriteList.Count; i++)
            {
                PwdObjFrameSprite p = pwdObjFrameSpriteList[i];
                arr.Add(new Vector2(p.pos.x * p.scale, p.pos.y * p.scale));
            }
            float leftX = arr[0].x;
            float bottomY = arr[0].y;
            for (int p = 0; p < arr.Count; p++)
            {
                var a = arr[p];
                if (a.x < leftX) leftX = a.x;
                if (a.y < bottomY) bottomY = a.y;
            }
            this.pos = new Vector2(leftX, bottomY);
            return this.pos;
        }
    }
    public class PwdObjFrameSprite
    {
        //未缩放的大小和位置，使用时需要*scale
        public Vector2Int pos;
        public Vector2Int size;
        public float scale;
        //从大图中截取的部分
        public RectInt rect;
        public int pwdId;
        public string name;
    }

}
