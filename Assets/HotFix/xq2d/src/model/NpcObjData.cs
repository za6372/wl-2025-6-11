using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    public class NpcObjData : ModelMsg
    {
        //type:0贩卖NPC、1剧情NPC、2任务NPC、3传送NPC、4怪物、5采集物、6宝箱类（靠近时触发请求）、7农场作物
        public int type = 2;
        //图片路径 png/pwd
        public string picPath;
        //绑定的活动
        public List<string> acKeys;
        //传送的地图点
        public string toMapKey;
        //对应怪物
        public string toMonKey;
        //是否允许移动（类型4使用）
        public bool isMove = true;
        public NpcObjData(string key)
        {
            this.key = key;
            this.toMonKey = key;
        }
        public NpcObjData addAcKeys(params string[] keys)
        {
            if (this.acKeys == null) this.acKeys = new List<string>();
            this.acKeys.AddRange(keys);
            return this;
        }
        public NpcObjData init(int type, string name, string picPath)
        {
            this.type = type;
            this.name = name;
            this.picPath = picPath;
            return this;
        }
        /**传送点npc初始化*/
        public NpcObjData initTransmit(string name, string toMapKey)
        {
            this.type = 3;
            this.name = name;
            this.picPath = "entrance";
            this.toMapKey = toMapKey;
            return this;
        }
    }
}
