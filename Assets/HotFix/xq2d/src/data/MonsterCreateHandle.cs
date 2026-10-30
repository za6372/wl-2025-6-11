using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.fight;
using Assets.Res.script.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.data
{
    /**野区怪物生成*/
    class MonsterCreateHandle
    {
        private static MonsterCreateHandle one;
        public static MonsterCreateHandle getInstance()
        {
            if (one == null) one = new MonsterCreateHandle();
            return one;
        }
        //每次调用则累计+1
        private int num = 0;
        private int sum = 150;
        public void resetNum()
        {
            num = 0;
        }
        public void create()
        {
            string mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
            MapData msg = face.mapInterface.getMapByKey(mapKey);

            if (!msg.isYeQu() || fightCache.getInstance().isDoing) return;

            JObject team = face.teamInterface.getTeam();
            if (team != null && !face.teamInterface.isCaptain())
            {
                //不是队长则不允许触发战斗
                return;
            }
            if (num < sum)
            {
                num++;
                return;
            }

            if (fightCache.getInstance().isDoing)
            {
                return;
            }
            if (face.roleInterface.isUseQdxc())//使用驱敌香草
            {
                return;
            }

            Vector2 pos = PosCountUtils.controlPosRelativeMapePos(PointGet.getControlPoint());
            face.roleInterface.isUploadPos(pos);

            num = 0;
            //下次需要累计
            sum = strUtils.getRandom(150, 300);


            string k = msg.petKeys[strUtils.getRandom(0, msg.petKeys.Count)];
            if (strUtils.isMatch(k, "1([0-9]{3})"))
            {
                k = "gw" + k;
            }
            else if (k.Contains("mpbwz_"))//门派保卫战的怪物需要先验证当前地图是否还有剩余怪物
            {
                face.activityInterface.isHasMonsterByMpbwz((r) =>
                {
                    if (r == 0) return;
                    face.fightInterface.createFightByMonster(k);
                });
                return;
            }
            face.fightInterface.createFightByMonster(k);

        }
        /**由诱敌香草创建怪物*/
        public void createByydxc()
        {
            num = sum;
            this.create();
        }
    }
}
