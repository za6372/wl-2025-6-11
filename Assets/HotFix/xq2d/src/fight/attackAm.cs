using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.am.fight;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.page;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.fight
{
    /**战斗流程*/
    public class attackAm
    {
        private static attackAm one;
        //每一个ws收到的actionList都要放进来，每播放完一个actionList就移除一个，退出战斗则移动要清空
        private Queue<JArray> acListQue = new Queue<JArray>();
        //是否正在播放动作
        private bool isPlayingAc = false;
        //执行步骤
        public JArray actionList;
        //步骤进度
        public int actionIndex;
        private JObject action;
        //是否处于结算中
        //private bool isOverFighting = false;
        //private timeManager tm;
        public static attackAm getInstance()
        {
            if (one == null) one = new attackAm();
            return one;
        }

        public attackAm()
        {

        }
        public void putActionList(JArray actionList)
        {
            acListQue.Enqueue(actionList);
            this.playAction();
        }
        private void playAction()
        {
            //保证在没播放完动画之前收到动作也不会替换上个执行的步骤
            if (!isPlayingAc)
            {
                doActionList();
            }
        }
        /**取一个动作进行执行*/
        public bool doActionList()
        {
            lock (acListQue)
            {
                //没有更多需要播放的动画就返回false
                if (acListQue.Count == 0) return false;
                isPlayingAc = true;
                this.actionList = acListQue.Dequeue();
                this.actionIndex = 0;
                playAm();
                return true;
            }
        }
        public void playAm()
        {
            //先清理一次命中特效是否播放过了的缓存
            fightCache.getInstance().clearMzTxPlayed();
            //一回合动画结束，开启下一回合
            if (this.actionIndex >= this.actionList.Count)
            {
                //将所有动画的待命帧进行同步
                playerOperate.getInstance().setSameFrame();
                //回合开始时清理buff状态图标
                clearOverTimeBuffIcon();
                //MYCONST.fightPage.removeBuff();
                //上一回合的动画还在播放。下一回合的结束标志已经出现就会导致这种问题
                //todo:bug:服务器先通知战斗结束，把fightMsg给清理了，这里再次调用put就会报错导致无法再次遇怪
                //一个回合结束则要求同步给服务器
                face.fightInterface.putAmPlayedOrder();
                isPlayingAc = false;
                this.playAction();
                return;
            }
            //Debug.Log(this.actionList);
            this.action = (JObject)this.actionList[this.actionIndex];
            this.actionIndex++;
            this.start();


        }
        private void start()
        {
            //判断步骤是否为战斗结束
            if (this.isOver())
            {
                //Debug.Log("战斗结束");
                return;
            }


            long t = strUtils.getMillis();
            //Debug.Log(this.action);
            playerOperate po = playerOperate.getInstance();
            string controlPosKey = (string)action["control"];
            JArray actionItems = (JArray)action["actionItems"];
            modelMsgBind control = po.getPlayer(controlPosKey);


            //回合开始时的buff
            if (controlPosKey == null && actionItems != null)
            {
                this.huiheStartHandle(actionItems);
                //需要延迟2s
                this.delayNext();

                return;
            }
            //TODO:判断是否退出战斗
            //（玩家控制）判断是否退出该场战斗 只要人物逃跑，宠物也跟着跑
            if (fightCache.getInstance().isMeControlObj(this.action["control"].ToString()) && this.isExist())
            {
                //清理战场
                this.clearHandle();
                return;
            }
            if ((this.action["isExist"] != null && this.action["isExist"].ToString().Equals("1")))
            {
                this.delayNext();
                return;
            }

            //处理玩家的逃跑
            if (this.action["isTao"] != null)
            {
                // fightCache.getInstance().isMeControlObj(this.action["control"].ToString())
                //将玩家隐藏
                string str = "逃跑失败";
                if (this.action["isTao"].ToString().Equals("1"))
                {
                    this.setTao(this.action["control"].ToString());
                    str = "逃跑成功";
                }
                partsMenu.getInstance().showBuff(str, "#F0E68C", this.action["control"].ToString());
                this.delayNext();
                return;
            }

            //对休息状态处理
            if (actionItems != null && actionItems.Count > 0
                && actionItems[0]["isRest"] != null && actionItems[0]["isRest"].ToString().Equals("1"))
            {
                string targetPosKey = actionItems[0]["target"].ToString();
                modelMsgBind target = po.getPlayer(targetPosKey);
                string value = GameAttrConst.propKeyToName("rest");
                partsMenu.getInstance().showBuff(value, "#F0E68C", targetPosKey);
                this.delayNext();
                return;
            }
            //对蛊状态的处理
            if (actionItems != null && actionItems.Count > 0
                && actionItems[0]["statusType"] != null && actionItems[0]["statusType"].ToString().Equals("3")
                && actionItems[0]["isRest"] != null && actionItems[0]["isRest"].ToString().Equals("1"))
            {
                string targetPosKey = actionItems[0]["target"].ToString();
                modelMsgBind target = po.getPlayer(targetPosKey);
                string value = GameAttrConst.propKeyToName(actionItems[0]["tipKey"].ToString());
                partsMenu.getInstance().showBuff(value, "#F0E68C", targetPosKey);
                this.delayNext();
                return;
            }
            //捕捉处理
            if (this.action["isCatch"] != null && this.action["isCatch"].ToString().Equals("1"))
            {
                catchHandle(actionItems, 0);
                this.delayNext();
                return;
            }
            //actionItems里面放置的是无序的，并不是每一次第一项statusType都是0
            if (true)
            {
                //重新组装actionItems。先将反击、连击等收入状态为0的item中
                actionItems = resetActionItems(actionItems);
                //Debug.Log(actionItems);
                if (actionItems == null || actionItems.Count == 0)
                {
                    Debug.Log("服务器导致actionItems.Count=0，（例如mp/hp不足）");
                }
                //Debug.Log("动作计数==============>" + actionItems.Count);
                //当计数完成后回调
                /* this.tcm.init(actionItems.Count, () =>
                 {
                     this.delayNext();
                 });*/

                Action fn = () =>
                {
                    //控制方还原idle
                    po.playByStatus(control.gameObject, 1);
                    int count = 0;
                    //允许同时进行
                    for (int i = 0; i < actionItems.Count; i++)
                    {
                        this.targetHurtHandle(controlPosKey, actionItems, i, () =>
                        {
                            count++;
                            //Debug.Log("计数：" + count + "/" + actionItems.Count);
                            if (actionItems.Count == count)
                            {
                                //当播放计数相等时开始播放子项动画，不存在子项则进行下一个动作
                                JArray acs = null;
                                for (int j = 0; j < actionItems.Count; j++)
                                {
                                    JObject item = (JObject)actionItems[j];
                                    if (item["actionItems"] != null)
                                    {
                                        acs = (JArray)item["actionItems"];
                                        break;
                                    }
                                }
                                //对援护、反击等动作的处理
                                //这里不在是同时执行，需要迭代执行
                                if (acs != null)
                                {
                                    //todo:多目标同时攻击时，因为遍历的目标是同时进行，此时遍历的目标中出现播放受伤动画，而在子项中又同时播放反击，就会把受伤动画的回调给替掉，导致计数少了
                                    doHandleChildren(controlPosKey, acs, 0, () =>
                                    {
                                        this.delayNext();
                                    });
                                }
                                else
                                {
                                    this.delayNext();
                                }

                            }
                        });
                    }
                };
                string targetPosKey = (string)actionItems[0]["target"];
                //判断是否为使用技能
                if (this.action["skillKey"] != null)
                {
                    playSklAm(controlPosKey, targetPosKey, () =>
                    {
                        fn();
                    });
                }
                else
                {
                    //默认普攻
                    playAttackAm(controlPosKey, targetPosKey, () =>
                    {
                        fn();
                    });
                }
            }
        }
        /**重新组装actionItems。先将反击、连击等收入状态为0的item中*/
        private JArray resetActionItems(JArray actionItems)
        {
            //反击、连击等状态的放入children
            JArray childrenArr = new JArray();
            //0状态的放入
            JArray zeroArr = new JArray();
            //其他状态的放入
            JArray elseArr = new JArray();

            string sameTarget = null;
            for (int i = 0; i < actionItems.Count; i++)
            {
                JObject item = (JObject)actionItems[i];
                int statusType = 0;
                if (item["statusType"] != null) statusType = (int)item["statusType"];
                if ((statusType == 29 && item["helper"] != null) || statusType == 35 || statusType == 36)
                {
                    childrenArr.Add(item);
                }
                else if (statusType == 0)
                {
                    //同一个目标多次伤害的放入children
                    if (sameTarget == null)
                    {
                        //记录第一次出现statusType=0的目标
                        sameTarget = item["target"].ToString();
                        zeroArr.Add(item);
                    }
                    else if (sameTarget != null && sameTarget.Equals(item["target"].ToString()))
                    {
                        //后续出现的则加入子集
                        childrenArr.Add(item);
                    }
                    else
                    {
                        //对于非同一个目标则加入zeroArr
                        zeroArr.Add(item);
                    }
                }
                else
                {
                    elseArr.Add(item);
                }
            }
            if (zeroArr.Count > 0 && childrenArr.Count > 0)
            {
                zeroArr[0]["actionItems"] = childrenArr;
            }
            zeroArr.Merge(elseArr);
            return zeroArr;
        }
        //对动画播放完毕的计数
        //private TaskCountManager tcm = TaskCountManager.getInstanceBySingle();
        /* private void addTcmNum()
         {
             tcm.addNum();
         }*/
        /**对目标受伤后处理*/
        private void targetHurtHandle(string controlPosKey, JArray actionItems, int i, Action callback)
        {
            JObject item = (JObject)actionItems[i];
            string targetPosKey = (string)item["target"];
            int isSd = 0;
            if (item["isSd"] != null) isSd = (int)item["isSd"];
            int value = 0;
            if (item["value"] != null) value = (int)item["value"];
            int isBaoJi = 0;
            if (item["isBaoJi"] != null) isBaoJi = (int)item["isBaoJi"];
            int statusType = 0;
            if (item["statusType"] != null) statusType = (int)item["statusType"];
            int isNoShowBuff = 0;
            if (item["isNoShowBuff"] != null) isNoShowBuff = (int)item["isNoShowBuff"];

            string hurtValue = value.ToString();

            Transform control = playerOperate.getInstance().getPlayer(controlPosKey).transform;
            Transform target = playerOperate.getInstance().getPlayer(targetPosKey).transform;
            playerOperate po = playerOperate.getInstance();
            Vector3 oldPos = po.getPosByPosKey(controlPosKey);
            Action fn = () =>
            {
                //播放完毕后计数
                //addTcmNum();
                //Debug.Log("回调计数============" + tcm.num + "/" + tcm.sum);
                //todo:因为buff调用这里导致受伤状态应该回调的方法被替换，所以导致计数不够无法播放下一个动作
                if (item["isDie"] == null || (item["isDie"] != null && (int)item["isDie"] != 1)) po.playByStatus(target.gameObject, 1);
                else if (item["isDie"] != null && (int)item["isDie"] == 1) po.playByStatus(target.gameObject, 3);
                callback();
            };


            string sklKey = this.action.ContainsKey("skillKey") ? this.action["skillKey"].ToString() : null;
            if (statusType == 0)
            {
                //像连斩这些是对同一目标造成多次伤害，而不是像群攻对多个目标造成伤害
                //非第一次伤害作用在同一个目标就只显示伤害，而不需要再次显示受伤动作
                if (isSd == 1)
                {
                    //提示闪避
                    partsMenu.getInstance().showBuff("闪躲", "#77BCFF", targetPosKey);
                    control.localPosition = oldPos;
                    po.playByStatus(target.gameObject, 20, null, (md) =>
                     {
                         //Debug.Log("1============");
                         fn();
                     });

                }
                else
                {
                    //播放附加的特效
                    po.playBoom(control.gameObject, target.localPosition, sklKey);
                    //目标播放受伤动画
                    po.playByStatus(target.gameObject, 4, null, (md) =>
                     {
                         control.localPosition = oldPos;
                         //Debug.Log("3============");
                         fn();
                     });
                    //Debug.Log(i + "=======测试==========>" + targetPosKey);
                    //伤害提示
                    partsMenu.getInstance().showHurt(hurtValue, targetPosKey, isBaoJi);
                    //血量加减计算
                    handleXueAddOrCut(item);
                }
            }
            else
            {
                //处理非攻击携带的buff命中特效
                if (isNoShowBuff != 1 && isAllowShowBoomTxOfBuffSkl(sklKey) &&
                    !fightCache.getInstance().isMzTxPlayed(targetPosKey))//判断是否已经播放过该target的命中特效（保证不同站位的目标只播放一次）
                {
                    //Debug.Log(targetPosKey + "========>" + sklKey + "=>" + statusType);
                    //播放命中的特效
                    po.playBoom(control.gameObject, target.localPosition, sklKey);
                }
                if (statusType == 13)
                {
                    //复活
                    po.playReliveStatus(target.gameObject);
                }
                /*if (statusType == 53)
                {
                    Debug.Log(actionIndex + "=>" + i);
                    Debug.Log(actionItems);
                }*/
                //其他状态为一些状态提示，并不涉及对象动画
                this.buffHandle(actionItems, i);
                //将buff以图标形式显示
                this.buffToIcon(actionItems, i);
                //清除buff icon
                this.remBuffIcon(actionItems, i);
                //Debug.Log("buff============");
                //只增加计数而不改变动作状态
                //addTcmNum();
                callback();

            }
        }


        private void handleXueAddOrCut(JObject item)
        {
            //Debug.Log(item);
            int statusType = 0;
            string targetPosKey = (string)item["target"];
            if (item["statusType"] != null) statusType = (int)item["statusType"];
            float value = 0;
            if (item["value"] != null) value = (float)item["value"];
            //获取缓存中的属性数据
            JObject obj = fightCache.getInstance().getMsgByPosKey(targetPosKey);
            //Debug.Log("=================handleXueAddOrCut=================");
            //Debug.Log(obj);
            JObject prop = (JObject)obj["prop"];
            if (isUpdateXueOfStatusType(statusType))
            {
                float xue = (float)prop["xue"] + value;
                if (xue - (float)prop["max_xue"] > 0) xue = (float)prop["max_xue"];
                else if (xue < 0) xue = 0;
                prop["xue"] = xue;
            }
            if (isUpdateLanOfStatusType(statusType))
            {
                float lan = (float)prop["lan"] + value;
                if (lan - (float)prop["max_lan"] > 0) lan = (float)prop["max_lan"];
                else if (lan < 0) lan = 0;
                prop["lan"] = lan;
            }
            if ((int)obj["type"] == 0 && targetPosKey.Equals(fightCache.getInstance().getControlRolePosKey()))
            {
                partsMenu.getInstance().updateRoleXueTiao();
            }
            else if ((int)obj["type"] == 1 && targetPosKey.Equals(fightCache.getInstance().getControlPetPosKey()))
            {
                partsMenu.getInstance().updatePetXueTiao();
            }
            else if ((int)obj["type"] == 0)
            {
                partsMenu.getInstance().updateTeamMemberRoleXueTiao(targetPosKey);
            }
            else if ((int)obj["type"] == 1)
            {
                //r6 - 9
                targetPosKey = targetPosKey.Substring(0, 1) + (int.Parse(targetPosKey.Substring(1)) - 5);
                //因为血条组件是按人物的posKey来命名的（0-5），而这里宠物的targetPosKey是6-9，所以要-5
                partsMenu.getInstance().updateTeamMemberPetXueTiao(targetPosKey);
            }

        }
        /**是否为允许显示buff命中特效的技能*/
        private bool isAllowShowBoomTxOfBuffSkl(string sklKey)
        {
            if (sklKey == null) return false;
            if (sklKey.Equals("100210000003") || sklKey.Equals("100210000007") || sklKey.Equals("100210000009") || sklKey.Equals("100210000012") ||
                sklKey.Equals("100210000016") || sklKey.Equals("100210000022") || sklKey.Equals("100210000026") || sklKey.Equals("100210000030") ||
                sklKey.Equals("100210000033") || sklKey.Equals("100210000036") || sklKey.Equals("100210000037") || sklKey.Equals("100210000038"))
            {
                return true;
            }
            return false;
        }
        private void doHandleChildren(string controlPosKey, JArray actionItems, int i, Action ac)
        {
            if (i >= actionItems.Count)
            {
                ac();
                return;
            }
            JObject item = (JObject)actionItems[i];
            string targetPosKey = (string)item["target"];
            int isSd = 0;
            if (item["isSd"] != null) isSd = (int)item["isSd"];
            int value = 0;
            if (item["value"] != null) value = (int)item["value"];
            int isBaoJi = 0;
            if (item["isBaoJi"] != null) isBaoJi = (int)item["isBaoJi"];
            int statusType = 0;
            if (item["statusType"] != null) statusType = (int)item["statusType"];
            int isDie = 0;
            if (item["isDie"] != null) isDie = (int)item["isDie"];
            string hurtValue = value.ToString();

            Transform control = playerOperate.getInstance().getPlayer(controlPosKey).transform;
            Transform target = playerOperate.getInstance().getPlayer(targetPosKey).transform;
            playerOperate po = playerOperate.getInstance();
            //Vector3 oldPos = po.getPosByPosKey(controlPosKey);
            Action fn = () =>
            {
                //血量加减计算
                handleXueAddOrCut(item);
                i++;
                doHandleChildren(controlPosKey, actionItems, i, ac);
            };
            //根据status来确定是否执行反击、连击、反震等==
            if (statusType == 0)//对同一个目标多次伤害
            {
                if (isSd == 1)
                {
                    partsMenu.getInstance().showBuff("闪躲", "#77BCFF", targetPosKey);
                    fn();
                    return;
                }
                partsMenu.getInstance().showHurt(hurtValue, targetPosKey, isBaoJi, -70 * (i + 1));
                if (isDie != 1) po.playByStatus(target.gameObject, 1);
                else po.playByStatus(target.gameObject, 3);
                fn();
            }
            else if (statusType == 29 && item["helper"] != null)//援护
            {
                //伤害提示
                partsMenu.getInstance().showBuff("保护", "#F0E68C", targetPosKey);
                partsMenu.getInstance().showHurt(value.ToString(), targetPosKey, isBaoJi);
                //移动到受保护者位置
                Vector3 oldPos = po.getPosByPosKey(targetPosKey);
                string helperPosKey = (string)item["helper"];
                Transform helper = playerOperate.getInstance().getPlayer(helperPosKey).transform;
                target.localPosition = helper.localPosition * 1f;
                po.playByStatus(target.gameObject, 4, null, (md) =>
                 {
                     if (isDie != 1) po.playByStatus(target.gameObject, 1);
                     else po.playByStatus(target.gameObject, 3);
                     //还原位置
                     target.localPosition = oldPos;
                     fn();
                 });

            }
            else if (statusType == 35)//反击
            {
                partsMenu.getInstance().showBuff("反击", "#F0E68C", targetPosKey);
                Vector3 oldPos = po.getPosByPosKey(targetPosKey);
                //播放反击动画
                FightModelMsg fm = (FightModelMsg)target.GetComponent<modelMsgBind>().msg;
                /*if (true)
                {
                    fn();
                    return;
                }*/
                Action fn1 = () =>
                {
                    po.playByStatus(target.gameObject, 1);
                    //还原位置
                    target.localPosition = oldPos;
                    //是否命中
                    if (isSd != 1)
                    {
                        //控制方播放受伤
                        po.playByStatus(control.gameObject, 4, null, (md) =>
                        {
                            if (isDie != 1) po.playByStatus(control.gameObject, 1);
                            else po.playByStatus(control.gameObject, 3);
                            fn();
                        });
                        //伤害提示
                        partsMenu.getInstance().showHurt(value.ToString(), controlPosKey, isBaoJi);
                    }
                    else
                    {
                        partsMenu.getInstance().showBuff("闪躲", "#77BCFF", controlPosKey);
                        po.playByStatus(control.gameObject, 20, null, (md) =>
                        {
                            po.playByStatus(control.gameObject, 1);
                            fn();
                        });
                    }

                };
                //反击者是近战或者远程
                bool isNear = fm.simpleFightIsNear;
                if (isNear)
                {
                    //播放attack
                    po.playByStatus(target.gameObject, 10, null, (md) =>
                    {
                        target.localPosition = control.localPosition * 1f;
                        //播放boom
                        po.playByStatus(target.gameObject, 15, null, (md2) =>
                        {
                            fn1();
                        });
                    });
                }
                else
                {
                    //播放attack
                    po.playByStatus(target.gameObject, 10, null, (md) =>
                    {
                        fn1();
                    });
                }

            }
            else if (statusType == 36)//连击
            {
                partsMenu.getInstance().showBuff("连击", "#F0E68C", controlPosKey);
                po.playByStatus(control.gameObject, 10, null, (md) =>
                 {
                     po.playByStatus(control.gameObject, 1);
                     if (isSd != 1)
                     {
                         //目标播放受伤
                         po.playByStatus(target.gameObject, 4, null, (md) => { fn(); });
                         //伤害提示
                         partsMenu.getInstance().showHurt(hurtValue, targetPosKey, isBaoJi);
                     }
                     else
                     {
                         partsMenu.getInstance().showBuff("闪躲", "#77BCFF", targetPosKey);
                         po.playByStatus(target.gameObject, 20, null, (md) =>
                         {
                             fn();
                         });
                     }
                 });
            }
            else
            {
                Debug.Log("未触发特殊状态动作");
                fn();
            }
        }

        /**播放技能动画*/
        private void playSklAm(string controlPosKey, string targetPosKey, Action attackCall)
        {
            //提示技能
            Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(this.action["skillKey"].ToString());
            partsMenu.getInstance().showSkillName(skl.name, this.action["control"].ToString());
            //对于非主动技能的就不用播放动画了
            if (skl.triggerType != 0||skl.key.Equals("100210000046"))
            {
                attackCall();
                return;
            }
            //播放技能动画
            //根据技能来选择对应的技能动作
            //int sklAc = sklKeyMatchAm.getSklAction(skl.key);

            Transform control = playerOperate.getInstance().getPlayer(controlPosKey).transform;
            //先缓存执行方位置信息，等移动完返回时使用
            //Vector3 oldPos = control.localPosition * 1f;
            //Vector3 oldRotation = control.localRotation.eulerAngles * 1f;

            Transform target = playerOperate.getInstance().getPlayer(targetPosKey).transform;
            //移动到指定站位
            playerOperate po = playerOperate.getInstance();

            //判断近战或者远程
            if (skl.isNear)
            {
                po.playByStatus(control.gameObject, 10, skl.key, (md) =>
                 {
                     control.localPosition = target.localPosition * 1f;
                     //播放命中动作
                     po.playByStatus(control.gameObject, 15, skl.key, (md2) =>
                      {
                          //这里只播放attack动画，至于是否命中则在attackCall中处理
                          attackCall();
                      });
                 });
            }
            else
            {
                //远程
                po.playByStatus(control.gameObject, 10, skl.key, (md) =>
                 {
                     //播放技能特效
                     /*List<AmMatchEffct> efHitList = skl.getHitEffect();
                     foreach (AmMatchEffct aef in efHitList)
                     {
                         po.playEffect(aef.key, target.gameObject, aef.dy);
                     }*/
                     //这里只播放attack动画，至于是否命中则在attackCall中处理
                     attackCall();
                 });
                //同时播放使用技能时的特效
                /* List<AmMatchEffct> efStartList = skl.getStartEffect();
                 foreach (AmMatchEffct aef in efStartList)
                 {
                     po.playEffect(aef.key, control.gameObject, aef.dy);
                 }*/
            }
        }
        /**播放普攻动画*/
        private void playAttackAm(string controlPosKey, string targetPosKey, Action attackCall)
        {
            //这里不需要移动到目标位置，要按法攻、物攻来区分近战远程
            Transform control = playerOperate.getInstance().getPlayer(controlPosKey).transform;
            Transform target = playerOperate.getInstance().getPlayer(targetPosKey).transform;
            //移动到指定站位
            playerOperate po = playerOperate.getInstance();

            FightModelMsg fm = (FightModelMsg)control.GetComponent<modelMsgBind>().msg;
            //AmModelMsg am = amManager.getAmByKey(fm.amKey);
            bool isNear = fm.simpleFightIsNear;
            if (isNear)
            {
                //播放attack
                po.playByStatus(control.gameObject, 10, null, (md) =>
                {
                    control.localPosition = target.localPosition * 1f;
                    //播放boom
                    po.playByStatus(control.gameObject, 15, null, (md2) =>
                    {
                        //这里只播放attack动画，至于是否命中则在attackCall中处理
                        attackCall();
                    });
                });
            }
            else
            {
                //播放attack
                po.playByStatus(control.gameObject, 10, null, (md) =>
                {
                    attackCall();
                });
            }

        }

        private void delayNext(long time = 1000)
        {
            //将所有动画的待命帧进行同步
            playerOperate.getInstance().setSameFrame();
            //一个动作播放完后显示buff图标
            drawIconToPlayer();

            timeManager.getTimeManageOne().putDelayTask(() =>
            {
                this.nextProgress();
            }, time);
        }




        /**标记逃跑 */
        public void setTao(string posKey)
        {
            JObject fgMsg = fightCache.getInstance().downloadFightMsg;
            //找到站位所控制的clientId，再找到相同的clientId站位
            List<string> arr = new List<string>();
            JArray roleList = (JArray)fgMsg["roleList"];
            foreach (object p in roleList)
            {
                JObject a = (JObject)p;
                if (a["posKey"].ToString().Equals(posKey))
                {
                    foreach (object i in roleList)
                    {
                        JObject b = (JObject)i;
                        if (a["clientId"].ToString().Equals(b["clientId"].ToString()))
                        {
                            arr.Add(b["posKey"].ToString());
                        }
                    }
                    break;
                }
            }
            //设置隐藏
            foreach (string p in arr)
            {
                playerOperate.getInstance().visiblePlayer(p, false);
            }
        }
        /**退出战斗 */
        public bool isExist()
        {
            //标志退出，并且控制站位为自己能控制的站位
            if (this.action["isExist"] != null && this.action["isExist"].ToString().Equals("1"))
            {
                JObject downloadFightMsg = fightCache.getInstance().downloadFightMsg;
                JObject control = (JObject)downloadFightMsg["control"];
                JArray roleList = (JArray)downloadFightMsg["roleList"];
                foreach (object p in roleList)
                {
                    JObject a = (JObject)p;
                    if ((a["posKey"].ToString().Equals(control["role"].ToString()) ||
                        a["posKey"].ToString().Equals(control["pet"].ToString()))
                        && a["clientId"].ToString().Equals(this.action["clientId"].ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /**处理攻击伤害*/
        private void hurtHandle(JArray actionItems, int i, string lzBoomKey)
        {
            JObject item = (JObject)actionItems[i];
            playerOperate po = playerOperate.getInstance();
            string targetPosKey = (string)item["target"];

            List<JObject> list = new List<JObject>();

            //统计攻击数量
            foreach (object o in actionItems)
            {
                JObject obj = (JObject)o;
                if (obj["statusType"] != null && (int)obj["statusType"] == 0)
                {
                    list.Add(obj);
                }
            }
            //获取攻击项所在的位置
            int n = 0;
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == item)
                {
                    n = j;
                    break;
                }
            }

            JToken h;
            if (item.TryGetValue("value", out h))
            {
                JToken bj;
                item.TryGetValue("isBaoJi", out bj);
                if (bj == null)
                {
                    bj = 0;
                }
                modelMsgBind target = po.getPlayer(targetPosKey);
                GameObject targetObj = target.gameObject;
                JToken isDie;
                item.TryGetValue("isDie", out isDie);
                //反击的情况
                //原先的1转0回调next，受反击后变成2转0，已经无法回调从1转0了
                //这里导致播放完受伤动画直接调用下一步，比技能播放完的时间快太多
                //TODO:bug目标被攻击后动画无法播放，第一次被攻击后的受伤动画未播放完就再次受伤导致（status未置0却等于2）
                if (lzBoomKey != null)
                {
                    //播放技能特效
                    //target.playLzBoom(lzBoomKey);
                }
                //需要延时1s播放受伤动画
                timeManager.getTimeManageOne().putDelayTask(() =>
                {
                    //当statusType=0并且值<0时才播放受伤动画
                    if ((int)h <= 0)
                    {
                        if ((int)isDie == 1)
                        {
                            //对于已经处于死亡状态的则不用再次播放
                            /*Animator a = targetObj.GetComponent<Animator>();
                            AnimatorStateInfo info=a.GetCurrentAnimatorStateInfo(0);
                            Debug.Log(info.IsName("hurt"));
                            if (!info.IsName("dead")&& !info.IsName("hurt"))
                            {
                                
                            }*/
                            //播放受伤、死亡
                            po.playByStatus(targetObj, 2);
                            po.playByStatus(targetObj, 3);
                        }
                        else
                        {
                            //播放受伤、待机
                            po.playByStatus(targetObj, 2);
                            po.playByStatus(targetObj, 0);
                        }

                    }
                    //显示伤害
                    //partsMenu.getInstance().showHurt((int)h, (int)bj, -n * 50f);
                }, 1000);


            }
        }
        /**回合开始时的buff处理*/
        private void huiheStartHandle(JArray actionItems)
        {
            if (actionItems.Count > 0)
            {
                JObject a = (JObject)actionItems[0];
                //对休息状态处理
                if (a["isRest"] != null && (int)a["isRest"] == 1)
                {
                    return;
                }
                //对蛊状态的处理
                /*if (a["isRest"] != null && (int)a["statusType"] == 3 && (int)a["isRest"] == 1)
                {
                    return;
                }*/
            }
            //回合buff
            for (int i = 0; i < actionItems.Count; i++)
            {
                JObject obj = (JObject)actionItems[i];
                if (obj["statusType"] == null) continue;
                int statusType = (int)obj["statusType"];
                if (statusType == 4 || statusType == 5 || statusType == 10 || statusType == 12 || statusType == 14)
                {
                    this.buffHandle(actionItems, i);
                }
                if (obj["isDie"] != null && (int)obj["isDie"] == 1)
                {
                    //播放死亡动画
                    playerOperate po = playerOperate.getInstance();
                    modelMsgBind target = po.getPlayer(obj["target"].ToString());
                    GameObject targetObj = target.gameObject;
                    po.playByStatus(targetObj, 3);
                }
            }
        }
        /**处理捕捉*/
        private void catchHandle(JArray actionItems, int i)
        {
            JObject item = (JObject)actionItems[i];
            string targetPosKey = (string)item["target"];
            playerOperate po = playerOperate.getInstance();
            modelMsgBind target = po.getPlayer(targetPosKey);
            string str = "失败";
            if (actionItems[0]["isCatch"].ToString().Equals("1"))
            {
                str = "成功";
                target.gameObject.SetActive(false);
            }
            partsMenu.getInstance().showBuff("捕捉" + str, "#00FF00", targetPosKey);
        }
        /**是否为刷新血量的类型*/
        private bool isUpdateXueOfStatusType(int statusType)
        {
            int[] arr = { 0, 4, 5, 6, 7, 8, 10, 13, 19, 20, 21, 22, 34, 39, 45, 46, 47, };
            if (arr.Contains(statusType)) return true;
            return false;
        }
        private bool isUpdateLanOfStatusType(int statusType)
        {
            int[] arr = { 14, 26, };
            if (arr.Contains(statusType)) return true;
            return false;
        }
        private List<JObject> buffIconList = new List<JObject>();
        /**回合结束前对过期的buff图标进行清理*/
        private void clearOverTimeBuffIcon()
        {
            try
            {
                playerOperate po = playerOperate.getInstance();
                for (int p = 0; p < buffIconList.Count; p++)
                {
                    JObject obj = buffIconList[p];
                    obj["statusKeep"] = (int)obj["statusKeep"] - 1;
                    int statusKeep = (int)obj["statusKeep"];
                    if (statusKeep > 0) continue;
                    //对于小于等于0的要清理
                    string posKey = obj["target"].ToString();
                    int statusType = (int)obj["statusType"];
                    modelMsgBind target = po.getPlayer(posKey);

                    po.clearBuffStatusIcon(target.gameObject.transform, statusType);
                    buffIconList.RemoveAt(p);
                    p--;
                }
                DoGet.getInstance().startReqImg();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }
        /**移除指定buff图标*/
        private void remBuffIcon(JArray actionItems, int i)
        {
            try
            {
                JObject item = (JObject)actionItems[i];
                int inputStatus = 0;
                if (item["statusType"] != null) inputStatus = (int)item["statusType"];
                if (inputStatus != 54)//移除霸体的icon
                {
                    return;
                }
                string inputPosKey = (string)item["target"];

                playerOperate po = playerOperate.getInstance();
                for (int p = 0; p < buffIconList.Count; p++)
                {
                    JObject obj = buffIconList[p];

                    string posKey = obj["target"].ToString();
                    int statusType = (int)obj["statusType"];
                    if (inputPosKey.Equals(posKey) && inputStatus == statusType)
                    {
                        modelMsgBind target = po.getPlayer(posKey);
                        po.clearBuffStatusIcon(target.gameObject.transform, statusType);
                        buffIconList.RemoveAt(p);
                        p--;
                    }
                }
                DoGet.getInstance().startReqImg();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        /**将需要绘制的状态图标一次性绘制到模型上*/
        private void drawIconToPlayer()
        {
            try
            {
                playerOperate po = playerOperate.getInstance();
                for (int p = 0; p < buffIconList.Count; p++)
                {
                    JObject obj = buffIconList[p];
                    string posKey = obj["target"].ToString();
                    int statusType = (int)obj["statusType"];
                    int buffNowAddNum = 1;
                    if (obj.ContainsKey("buffNowAddNum")) buffNowAddNum = (int)obj["buffNowAddNum"];
                    modelMsgBind target = po.getPlayer(posKey);
                    po.showBuffStatusIcon(target.gameObject.transform, statusType, buffNowAddNum);
                }
                DoGet.getInstance().startReqImg();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }
        /**将buff以图标的形式显示*/
        private void buffToIcon(JArray actionItems, int i)
        {
            try
            {
                JObject item = (JObject)actionItems[i];
                /*Debug.Log("---------------");
                Debug.Log(item);*/
                //状态持续的回合数
                int statusKeep = 0;
                if (item["statusKeep"] != null) statusKeep = (int)item["statusKeep"];
                if (statusKeep <= 0) return;
                int statusType = 0;
                if (item["statusType"] != null) statusType = (int)item["statusType"];
                if (statusType != 4 && statusType != 5 && statusType != 9 && statusType != 10 && statusType != 49
                    && statusType != 52 && statusType != 53)
                {
                    return;
                }

                string targetPosKey = (string)item["target"];

                JObject a = new JObject();
                a["statusType"] = statusType;
                a["statusKeep"] = statusKeep;
                a["target"] = targetPosKey;
                if (item.ContainsKey("buffNowAddNum"))
                {
                    a["buffNowAddNum"] = (int)item["buffNowAddNum"];
                }
                bool isAdd = true;
                for (int p = 0; p < buffIconList.Count; p++)
                {
                    JObject obj = buffIconList[p];
                    //状态一致的就替换持续回合即可
                    if (obj["target"].ToString().Equals(targetPosKey) && (int)obj["statusType"] == statusType)
                    {
                        obj["statusKeep"] = statusKeep;
                        if (obj.ContainsKey("buffNowAddNum"))
                        {
                            obj["buffNowAddNum"] = (int)item["buffNowAddNum"];
                            //Debug.Log(statusType + "-----buff刷新----" + obj["buffNowAddNum"]);
                        }
                        isAdd = false;
                        break;
                    }
                }
                if (isAdd)
                {
                    buffIconList.Add(a);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


        }
        /**处理buff*/
        private void buffHandle(JArray actionItems, int i)
        {
            JObject item = (JObject)actionItems[i];
            //Debug.Log(item);
            //血量加减计算
            handleXueAddOrCut(item);

            int statusType = 0;
            if (item["statusType"] != null)
            {
                statusType = (int)item["statusType"];
            }
            string targetPosKey = (string)item["target"];

            List<JObject> list = new List<JObject>();

            //统计buff数量
            foreach (object o in actionItems)
            {
                JObject obj = (JObject)o;
                if (obj["statusType"] != null && (int)obj["statusType"] > 0 &&
                    obj["target"] != null && obj["target"].ToString().Equals(targetPosKey))
                {
                    list.Add(obj);
                }
            }
            //获取所在buff项的位置
            int n = 0;
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == item)
                {
                    n = j;
                    break;
                }
            }
            //Debug.Log(n+"=>"+item["statusType"]);
            float dy = -n * 40;

            string zyColor = "#00FF00";//绿色
            string jyColor = "#FF4626";//红色
            string elseColor = "#F0E68C";//黄色
            string lanColor = "#6495ED";//蓝色
            playerOperate po = playerOperate.getInstance();

            modelMsgBind target = po.getPlayer(targetPosKey);

            switch (statusType)
            {
                case 1:
                    {
                        string tp = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        string value = null;

                        if ((int)item["value"] > 0)
                        {
                            value = tp + " +" + Math.Abs((int)item["value"]) + '%';
                        }
                        else
                        {
                            value = tp + " -" + Math.Abs((int)item["value"]) + '%';
                        }
                        if (item["tipKey"].ToString().Equals("sufferHurt"))
                        {
                            value = tp + " -" + Math.Abs((int)item["value"]) + '%';
                        }

                        partsMenu.getInstance().showBuff(value, zyColor, targetPosKey, dy);
                        break;
                    }
                case 2:
                    {
                        //Debug.Log(targetPosKey + "=>" + n);
                        //Debug.Log(actionItems);
                        string tp = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        string value = null;
                        /*if (item["tipKey"].ToString().Equals("sufferHurt"))
                        {
                            value = tp + " -" + Math.Abs((int)item["value"]) + '%';
                        }
                        else
                        {
                            value = tp + " +" + Math.Abs((int)item["value"]) + '%';
                        }*/
                        if ((int)item["value"] > 0)
                        {
                            value = tp + " +" + Math.Abs((int)item["value"]) + '%';
                        }
                        else
                        {
                            value = tp + " -" + Math.Abs((int)item["value"]) + '%';
                        }
                        if ((int)item["value"] > 1000)
                        {
                            value = value.Replace("00%", "");
                        }
                        partsMenu.getInstance().showBuff(value, jyColor, targetPosKey, dy);
                        break;
                    }
                case 3:
                    {
                        string value = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        partsMenu.getInstance().showBuff(value, elseColor, targetPosKey, dy);
                        break;
                    }
                case 4:
                    {
                        if (item["value"] == null)
                        {
                            partsMenu.getInstance().showBuff("流血", jyColor, targetPosKey, dy);
                            break;
                        }
                        string value = "流血 " + item["value"];
                        if ((int)item["value"] == 0)
                        {
                            value = "抵抗";
                        }
                        partsMenu.getInstance().showBuff(value, jyColor, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 5://回血
                    {
                        string value = null;
                        string color = null;
                        if (item["value"] == null) break;
                        if ((int)item["value"] >= 0)
                        {
                            value = " +" + Math.Abs((int)item["value"]);
                            color = zyColor;
                        }
                        else
                        {
                            value = " -" + Math.Abs((int)item["value"]);
                            color = jyColor;
                        }
                        partsMenu.getInstance().showBuff("回血" + value, color, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 6://吸血
                    {
                        string value = null;
                        string color = null;
                        if ((int)item["value"] >= 0)
                        {
                            value = " +" + Math.Abs((int)item["value"]);
                            color = zyColor;
                        }
                        else
                        {
                            value = " -" + Math.Abs((int)item["value"]);
                            color = jyColor;
                        }
                        partsMenu.getInstance().showBuff("吸血" + value, color, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 7:
                    {
                        partsMenu.getInstance().showBuff("反震 " + item["value"], jyColor, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 8:
                    {
                        string color = zyColor;
                        string str = "免伤 ";
                        if (item["value"] != null)
                        {
                            if ((int)item["value"] < 0)
                            {
                                str = "背刺 ";
                                color = jyColor;
                            }
                            else str = "回血 ";
                        }
                        partsMenu.getInstance().showBuff(str + item["value"], color, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 9:
                    {
                        partsMenu.getInstance().showBuff("魔化", elseColor, targetPosKey, dy);
                        break;
                    }
                case 10:
                    {
                        partsMenu.getInstance().showBuff("中毒 " + item["value"], jyColor, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 11:
                    {
                        partsMenu.getInstance().showBuff("休息", elseColor, targetPosKey, dy);
                        break;
                    }
                case 12:
                    {
                        partsMenu.getInstance().showBuff("解除", elseColor, targetPosKey, dy);
                        break;
                    }
                case 13:
                    {
                        partsMenu.getInstance().showBuff("复活 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量

                        break;
                    }
                case 14:
                    {
                        partsMenu.getInstance().showBuff("回蓝 +" + Math.Abs((int)item["value"]), lanColor, targetPosKey, dy);
                        //TODO:刷新蓝量

                        break;
                    }
                case 15:
                    {
                        partsMenu.getInstance().showBuff("免疫", elseColor, targetPosKey, dy);
                        break;
                    }
                case 16:
                    {
                        if (item["value"] == null)
                        {
                            break;
                        }
                        string value = "消耗 " + item["value"];
                        partsMenu.getInstance().showBuff(value, jyColor, targetPosKey, dy);
                        break;
                    }
                case 17:
                    {
                        partsMenu.getInstance().showBuff("斩杀", elseColor, targetPosKey, dy);
                        break;
                    }
                case 18:
                    {//指定位置遭受攻击
                        partsMenu.getInstance().showBuff("", elseColor, targetPosKey, dy);
                        break;
                    }
                case 19:
                    {
                        partsMenu.getInstance().showBuff("偷血 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 20:
                    {
                        partsMenu.getInstance().showBuff("" + item["value"], jyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 21:
                    {//减少一定比例的血量
                        partsMenu.getInstance().showBuff("消耗" + item["value"], jyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 22:
                    {
                        partsMenu.getInstance().showBuff("增伤 +" + +Math.Abs((int)item["value"]) + "%", zyColor, targetPosKey, dy);
                        // TODO:刷新血量
                        break;
                    }
                case 23:
                    {
                        partsMenu.getInstance().showBuff("伤害扩大", elseColor, targetPosKey, dy);
                        break;
                    }
                case 24:
                    {
                        string tp = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        partsMenu.getInstance().showBuff(tp, elseColor, targetPosKey, dy);
                        break;
                    }
                case 25:
                    {
                        partsMenu.getInstance().showBuff("无视", elseColor, targetPosKey, dy);
                        break;
                    }
                case 26:
                    {
                        if (item.ContainsKey("isNoShowBuff") && (int)item["isNoShowBuff"] == 1)
                        {
                            //如使用技能时无需提示耗蓝多少
                        }
                        else
                        {
                            partsMenu.getInstance().showBuff("减蓝 " + item["value"], lanColor, targetPosKey, dy);
                        }

                        //TODO:刷新蓝量
                        break;
                    }
                case 27:
                    {
                        partsMenu.getInstance().showBuff("恶梦", zyColor, targetPosKey, dy);
                        break;
                    }
                case 28:
                    {
                        partsMenu.getInstance().showBuff("反弹", zyColor, targetPosKey, dy);
                        break;
                    }
                case 29:
                    {
                        partsMenu.getInstance().showBuff("抵挡", zyColor, targetPosKey, dy);
                        break;
                    }
                case 31:
                    {
                        partsMenu.getInstance().showBuff("阻止", elseColor, targetPosKey, dy);
                        break;
                    }
                case 34:
                    {
                        partsMenu.getInstance().showBuff("不死 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }

                case 38:
                    {
                        partsMenu.getInstance().showBuff("逆转", elseColor, targetPosKey, dy);
                        break;
                    }
                case 39:
                    {
                        partsMenu.getInstance().showBuff("不死 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 40:
                    {
                        partsMenu.getInstance().showBuff("撕裂", elseColor, targetPosKey, dy);
                        break;
                    }
                case 41:
                    {
                        partsMenu.getInstance().showBuff("雷电", elseColor, targetPosKey, dy);
                        break;
                    }
                case 42:
                    {
                        Skill skl = (Skill)face.goodsInterface.getGoodsMsgByKey(item["msg"]["key"].ToString());
                        partsMenu.getInstance().showBuff("学会-" + skl.name, elseColor, targetPosKey, dy);
                        break;
                    }
                case 43:
                    {
                        partsMenu.getInstance().showBuff("禁活", elseColor, targetPosKey, dy);
                        break;
                    }
                case 44:
                    {
                        partsMenu.getInstance().showBuff("禁神佑", elseColor, targetPosKey, dy);
                        break;
                    }
                case 45:
                    {
                        partsMenu.getInstance().showBuff("重生 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 46:
                    {
                        partsMenu.getInstance().showBuff("无敌 +" + item["value"], zyColor, targetPosKey, dy);
                        //TODO:刷新血量
                        break;
                    }
                case 49:
                    {
                        partsMenu.getInstance().showBuff("残废", elseColor, targetPosKey, dy);
                        break;
                    }
                case 51:
                    {
                        string tp = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        string color = zyColor;
                        string str = "";
                        if (item["value"] != null)
                        {
                            if ((int)item["value"] < 0)
                            {
                                str = "";
                                color = jyColor;
                            }
                            else str = "+";
                        }
                        partsMenu.getInstance().showBuff(tp + str + ((int)item["value"]), color, targetPosKey, dy);
                        // TODO:刷新血量
                        break;
                    }
                case 52:
                    {
                        partsMenu.getInstance().showBuff("恐惧", elseColor, targetPosKey, dy);
                        break;
                    }
                case 53:
                    {
                        partsMenu.getInstance().showBuff("霸体", elseColor, targetPosKey, dy);
                        break;
                    }
                case 1000:
                    {
                        string tp = GameAttrConst.propKeyToName(item["tipKey"].ToString());
                        partsMenu.getInstance().showBuff(tp, elseColor, targetPosKey, dy);
                        break;
                    }
            }
        }
        private void nextProgress()
        {

            //播放下一个步骤
            this.playAm();

        }
        /**判断步骤是否为战斗结束*/
        private bool isOver()
        {
            if (action["isOver"] != null && (int)action["isOver"] == 1)
            {

                JObject fgMsg = fightCache.getInstance().downloadFightMsg;
                string fightId = fgMsg["Id"].ToString();
                //提示战斗胜利、失败 isSuccess==1并且玩家控制方为右方 isSuccess==0并且玩家控制方为左方
                bool b;
                string role = (string)fgMsg["control"]["role"];
                if (role == null)
                {
                    b = (int)action["isSuccess"] == 1 ? true : false;
                }
                else
                {
                    b = ((int)action["isSuccess"] == 1 && role.Contains("r")) ||
                        ((int)action["isSuccess"] == 0 && role.Contains("l"));
                }
                string prefab = null;
                if (b)
                {
                    prefab = "victory_png";
                }
                else
                {
                    prefab = "failure_png";
                }


                //显示战斗结果
                GameObject tt = gameObjPool.getInstance().get("item", typeof(ImgUI));
                tt.transform.SetParent(PointGet.getPartsLayer(), false);
                tt.GetComponent<ImgUI>().loadRes(prefab)
                    .setSizePos(new Vector2(106 * 3, 39 * 3), new Vector2((ScreenUtils.width - 106 * 3) / 2f, -(ScreenUtils.height - 39 * 3) / 2f));
                tt.AddComponent<hurtTextUpTween>().addCallback(() =>
                {
                    JObject fgMsg = fightCache.getInstance().downloadFightMsg;
                    if (fgMsg != null && !fightId.Equals(fgMsg["Id"].ToString()))
                    {
                        //不是同一场战斗时不需要清理
                        return;
                    }
                    //3s后关闭战斗场景
                    this.clearHandle();

                }).setPos(0, 50);
                DoGet.getInstance().startReqImg();

                return true;
            }
            return false;
        }
        public void clearCache()
        {
            this.buffIconList.Clear();
            this.acListQue.Clear();
            this.isPlayingAc = false;
            fightCache.getInstance().clear();
            playerOperate.getInstance().clear();


        }
        public void clearHandle()
        {
            //清理战场
            this.clearCache();
            //返回主界面
            PointGet.getFightPage().GetComponent<FightPage>().freeThisPage();
            PageUI.create<IndexPage>().drawUI();
        }
        /**立即切换到下一场战斗*/
        public void nextToFight()
        {
            //清理战场
            this.clearCache();
            //返回主界面
            if (PointGet.getFightPage() != null)
            {
                PointGet.getFightPage().GetComponent<FightPage>().freeThisPage();
                PageUI.create<IndexPage>().drawUI();
            }
        }
    }
}

