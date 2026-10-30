using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Res.script.src.model
{
    /**玩家、npc、建筑身上的基本信息*/
    public class BindBaseMsg : MonoBehaviour
    {

        //多张图合成后的大小
        public Vector2 size;
        public void init(Vector2 pos, Vector2 size)
        {
            this.size = size;
            SetGameObj.setSize(this.size, this.gameObject);
            SetGameObj.setLeftPos(pos, this.gameObject);
        }
        
        public void updateSize(Vector2 size)
        {
            this.size = size;
            SetGameObj.setSize(this.size, this.gameObject);
        }
    }
}
