using Assets.HotFix.MyUtils.src.common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.fight
{
    /**战斗数据缓存*/
    public class fightCache
    {
        private static fightCache one;
        public bool isViewFight=false;
        //战斗数据-战斗创建时需要先放入
        public JObject downloadFightMsg;
        //是否有战斗正在进行
        public bool isDoing = false;
        //是否自动战斗
        private bool isAuto = false;
        //技能冷却集合
        private JArray coolList;
        //3009消息
        public JObject wsMsg3009;
        public bool isDownloadMsg = false;

        public static fightCache getInstance()
        {
            if (one == null)
            {
                one = new fightCache();
            }

            return one;
        }
        public fightCache()
        {

        }
        /**因为消息先到而界面未完成绘制会导致出错，所以先缓存*/
        public void setWsMsg3009(JObject msg)
        {
            this.wsMsg3009 = msg;
        }

        public void setCoolList(JArray coolList)
        {
            this.coolList = coolList;
        }
        public JArray getCoolList()
        {
            return this.coolList;
        }
        //命中特效是否播放过了
        private Dictionary<string, bool> mzTxPlayed = new Dictionary<string, bool>();
        //每个action结束都要清理一次
        public void clearMzTxPlayed()
        {
            this.mzTxPlayed.Clear();
        }

        public bool isMzTxPlayed(string posKey)
        {
            if (this.mzTxPlayed.ContainsKey(posKey)) return true;
            this.mzTxPlayed.Add(posKey, true);
            return false;
        }

        public void setAutoFight(bool b)
        {
            this.isAuto = b;
        }
        public bool getIsAutoFight()
        {
            return this.isAuto;
        }
        /**获取除自己外的其他队友站位*/
        public List<string> getFriendPosKeyExpSelf()
        {
            string posKey = getControlRolePosKey();
            string sub = posKey.Substring(0, 1);
            JObject fgMsg = this.downloadFightMsg;
            JArray roleList = (JArray)fgMsg["roleList"];
            List<string> list = new List<string>();
            for (int i = 0; i < roleList.Count; i++)
            {
                JObject r = (JObject)roleList[i];
                string temp = r["posKey"].ToString();
                if (!temp.Equals(posKey) && temp.Contains(sub) && (int)r["type"] == 0)
                {
                    list.Add(roleList[i]["posKey"].ToString());
                }
            }
            return list;
        }
        /**获取站位绑定的信息*/
        public JObject getMsgByPosKey(string posKey)
        {
            JObject fgMsg = this.downloadFightMsg;
            JArray roleList = (JArray)fgMsg["roleList"];
            for (int i = 0; i < roleList.Count; i++)
            {
                if (roleList[i]["posKey"].ToString().Equals(posKey)) return (JObject)roleList[i];
            }
            JArray monsterList = (JArray)fgMsg["monsterList"];
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i]["posKey"].ToString().Equals(posKey)) return (JObject)monsterList[i];
            }
            return null;
        }
        /**判断是否为己方*/
        public bool isFriend(string posKey)
        {
            string rolePos = getControlRolePosKey();
            if (posKey.ToCharArray()[0].ToString().Equals(rolePos.ToCharArray()[0].ToString())) return true;
            return false;
        }
        /**判断是否为人物/宠物站位*/
        public bool isRolePetPosKey(bool isRole, string posKey)
        {
            if (isRole)
            {
                string rolePos = getControlRolePosKey();
                if (rolePos.Equals(posKey)) return true;
                return false;
            }
            else
            {
                string petPos = getControlPetPosKey();
                if (petPos.Equals(posKey)) return true;
                return false;
            }

        }
        /***判断是否为自己控制的对象 */
        public bool isMeControlObj(string posKey)
        {
            string rolePos = getControlRolePosKey();
            string petPos = getControlPetPosKey();
            if (posKey.Equals(rolePos) ||
                (posKey.Equals(petPos)))
                return true;
            return false;
        }
        /**是否允许捕捉*/
        public bool isAllowedCatch()
        {
            return downloadFightMsg["type"].ToString().Equals("0");
        }


        /**获取玩家技能列表*/
        public JArray getControlRoleSkill()
        {
            string posKey = getControlRolePosKey();
            return getSkill(posKey);
        }
        public JArray getControlPetSkill()
        {
            string posKey = getControlPetPosKey();
            return getSkill(posKey);
        }
        /**获取控制方站位*/
        public string getControlRolePosKey()
        {
            //Debug.Log(fightCache.getInstance().downloadFightMsg);
            if (!downloadFightMsg.ContainsKey("control") || downloadFightMsg["control"]["role"] == null) return null;
            return downloadFightMsg["control"]["role"].ToString();
        }
        public string getControlPetPosKey()
        {
            if (!downloadFightMsg.ContainsKey("control")||downloadFightMsg["control"]["pet"] == null) return null;
            return downloadFightMsg["control"]["pet"].ToString();
        }
        public JArray getSkill(string posKey)
        {
            JArray arr = (JArray)downloadFightMsg["roleList"];
            for (int p = 0; p < arr.Count; p++)
            {
                if (arr[p]["posKey"].ToString().Equals(posKey))
                {
                    return strUtils.copyJSON<JArray>(arr[p]["skill"]);
                }
            }
            return new JArray();
        }

        public void clear()
        {
            this.downloadFightMsg = null;
            this.coolList = null;
            //这里不再改变是否正在战斗的状态，交由加载地图后3s再允许战斗
        }
    }
}
