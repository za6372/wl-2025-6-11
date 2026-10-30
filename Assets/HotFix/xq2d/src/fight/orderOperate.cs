using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.fight
{
    /**下达的指令*/
    public class orderOperate
    {
        private static orderOperate one;
        //待上传的命令
        private JObject uploadOrder;
        private order myOrder;
        public static orderOperate getInstance()
        {
            if (one == null)
            {
                one = new orderOperate();
                one.uploadOrder = new JObject();
                one.myOrder = new order();
            }

            return one;
        }
        public orderOperate()
        {

        }
        public void putAutoOrder(string control)
        {
            //清理命令
            this.clear();
            //Debug.Log("上传自动战斗命令");
            face.fightInterface.putAutoOrder(control);
        }
        public void putCancelAutoOrder(string control)
        {
            //清理命令
            this.clear();
            //Debug.Log("上传取消自动战斗命令");
            face.fightInterface.putCancelAutoOrder(control);
        }
        /**放置指令*/
        public void putOrder(string control, int action, string skillKey = null)
        {
            if (myOrder == null)
            {
                myOrder = new order();
            }
            myOrder.control = control;
            myOrder.action = action;
            if (skillKey != null)
                myOrder.putSkill(skillKey);
        }
        /**放置指令目标*/
        public void putTarget(string target)
        {

            if (uploadOrder[myOrder.control] != null)
            {
                uploadOrder.Remove(myOrder.control);
            }
            myOrder.target = target;
            //Debug.Log(myOrder.control);
            //判断是否两个都已经放置
            uploadOrder.Add(myOrder.control, strUtils.copyJSON<JObject>(myOrder));
            if (isRelease())
            {
                //上传命令
                face.fightInterface.putOrder(uploadOrder);
                //清理命令
                this.clear();
                //Debug.Log("上传命令");
                partsMenu.getInstance().setTipText("等待其他玩家下达命令");
            }
            else
            {
                //显示宠物菜单
                partsMenu.getInstance().showMenu(false);
            }
        }

        /**判断命令是否均已下达*/
        private bool isRelease()
        {
            JObject fgMsg = fightCache.getInstance().downloadFightMsg;
            JObject control = (JObject)fgMsg["control"];
            //Debug.Log(uploadOrder);
            if (control.Count == 1)
            {
                //只有人物，没有宠物
                if (uploadOrder.Count == 1)
                {
                    return true;
                }
            }
            else
            {
                if (uploadOrder.Count == 2)
                {
                    return true;
                }
            }
            return false;
        }

        private void clear()
        {
            this.uploadOrder.RemoveAll();
            this.myOrder.clear();
        }


        class order
        {
            //操作者 posKey
            public string control;
            //目标 posKey l0-l5 r0-r5
            public string target;
            //选择的行为 0攻击1技能2逃跑3物品4捕捉
            public int action;
            //选择的技能key
            public skill skill;
            public void putSkill(string key)
            {
                this.skill = new skill(key);
            }
            /**清除操作 */
            public void clear()
            {
                this.control = null;
                this.target = null;
                this.action = 0;
                this.skill = null;
            }
        };
        class skill
        {
            public string key;

            public skill(string key)
            {
                this.key = key;
            }
        }

    }
}
