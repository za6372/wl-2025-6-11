using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    public abstract class ModelMsg
    {
        public string Id;
        public string key;
        public string name;
        public string headIcon;
        //对应模型的编号（战斗返回的是这个）
        public string prefabCode;
        //总图
        public string[] changguiPwds;
        public string[] touxiangPwds;
        public string[] zhandouPwds;
        //命中后的一些特效
        public string[] zdSjtxPwds;
        //aef
        public string changguiAef;
        public string touxiangAef;
        public string zhandouAef;
        public string zdSjtxAef;
        //头像动画
        public AmPlayControl headIdle;
        //常规待机动画
        public AmPlayControl cgIdle;
        //战斗待命动画帧（从某帧到某帧）
        public AmPlayControl zdIdle;
        //战斗技能动作帧（可多个，迭代播放，比如连斩这些有多段动作）
        public List<AmPlayControl> zdSklList;
        //命中动作帧（可多个，迭代播放，比如连斩这些有多段动作）
        public List<AmPlayControl> zdMzList;
        //受伤动作帧
        public AmPlayControl zdHurt;
        //闪躲
        public AmPlayControl zdSd;
        //特效补充（比如命中后爆炸效果）
        public List<AmPlayControl> zdSjtxList;
        //默认普攻近战(默认)、远程
        public bool simpleFightIsNear = true;
        //站位
        public string posKey;
        //区分敌友
        public string tag;
        //0人物 1宠物 2怪物 4ai 5伙伴
        public int roleType;
        public ModelMsg setHeadIcon(string headIcon)
        {
            this.headIcon = headIcon;
            return this;
        }
        public ModelMsg setSimpleFightIsNear(bool b)
        {
            this.simpleFightIsNear = b;
            return this;
        }
        public ModelMsg addHeadIdleAm(int start, int end)
        {
            this.headIdle = new AmPlayControl(start, end);
            return this;
        }
        public ModelMsg addCgIdleAm(int start, int end)
        {
            this.cgIdle = new AmPlayControl(start, end);
            return this;
        }
        public ModelMsg addZdIdleAm(int start, int end)
        {
            this.zdIdle = new AmPlayControl(start, end);
            return this;
        }
        public ModelMsg addZdSklAm(int start, int end)
        {
            if (this.zdSklList == null) this.zdSklList = new List<AmPlayControl>();
            this.zdSklList.Add(new AmPlayControl(start, end));
            return this;
        }
        public ModelMsg addZdSklAm(List<AmPlayControl> arr)
        {
            if (this.zdSklList == null) this.zdSklList = new List<AmPlayControl>();
            this.zdSklList.AddRange(arr);
            return this;
        }
        public ModelMsg addZdMzAm(int start, int end)
        {
            if (this.zdMzList == null) this.zdMzList = new List<AmPlayControl>();
            this.zdMzList.Add(new AmPlayControl(start, end));
            return this;
        }
        public ModelMsg addZdMzAm(List<AmPlayControl> arr)
        {
            if (this.zdMzList == null) this.zdMzList = new List<AmPlayControl>();
            this.zdMzList.AddRange(arr);
            return this;
        }
        public ModelMsg addZdHurtAm(int start, int end)
        {
            this.zdHurt = new AmPlayControl(start, end);
            return this;
        }
        public ModelMsg addZdSdAm(int start, int end)
        {
            this.zdSd = new AmPlayControl(start, end);
            return this;
        }
        public ModelMsg addZdSjtxAm(int start, int end)
        {
            if (this.zdSjtxList == null) this.zdSjtxList = new List<AmPlayControl>();
            this.zdSjtxList.Add(new AmPlayControl(start, end));
            return this;
        }
        public ModelMsg addZdSjtxAm(List<AmPlayControl> arr)
        {
            if (this.zdSjtxList == null) this.zdSjtxList = new List<AmPlayControl>();
            this.zdSjtxList.AddRange(arr);
            return this;
        }

        public ModelMsg addTag(string tagName)
        {
            this.tag = tagName;
            return this;
        }
        public ModelMsg addRoleType(int roleType)
        {
            this.roleType = roleType;
            return this;
        }

        //专属技能
        public List<string> zsSklList;
        public ModelMsg addZsSkl(params string[] sklKey)
        {
            if (zsSklList == null) zsSklList = new List<string>();
            zsSklList.AddRange(sklKey);
            return this;
        }
    }
}
