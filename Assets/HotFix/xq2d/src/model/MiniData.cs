using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Res.script.src.model
{
    public class MiniData
    {
        //位置坐标，每格是80，以左上角位置为原点
        public Vector2 pos;
        public string mapKey;
        //0城池 1小县 2山 3路 4箭头
        public int type;
        //是否为横向路
        public bool hori;
        //箭头方向 0上 1下 2左 3右
        public int direction;
        //展开小地图的索引
        public int minIndex;

        public MiniData addMsg(int type, Vector2 pos)
        {
            this.type = type;
            this.pos = pos;
            return this;
        }
        public MiniData addKey(string mapKey)
        {
            this.mapKey = mapKey;
            return this;
        }
        public MiniData setHori(bool hori)
        {
            this.hori = hori;
            return this;
        }
        public MiniData setDire(int direction,int minIndex)
        {
            this.direction = direction;
            this.minIndex = minIndex;
            return this;
        }
        public void addToList(List<MiniData> list)
        {
            list.Add(this);
        }

    }
}
