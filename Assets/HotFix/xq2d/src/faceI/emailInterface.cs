using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{

    public interface emailInterface
    {
        public JArray getEmailList();

        public void initEmailList(JArray emailList);
        public void readEmail(string Id, Action ac);
        public void receiveEmail(string Id);
        public void delEmail(string Id);
        public void getGoodsEmail(JObject obj);
        public void sendEmail(JObject email);
        //public bool isNoEmailToPlayer(string key);
        public bool isNoAllowedSend(JObject goods);
        public void delEmailByType(int type);
        public void tuihuiEmail(string Id);
        public void delEmailFromCache(string Id);
    }

    class emailInterfaceImpl : emailInterface
    {
        /**是否允许交易*/
        public bool isNoAllowedSend(JObject goods)
        {
            //Debug.Log(goods);
            object isBind = goods["isBind"];
            if (isBind == null || goods["key"] == null ||
                    (int)goods["isBind"] == 1)
            {
                //Debug.Log(111);
                return true;
            }
            return isNoEmailToPlayer(goods["key"].ToString());
        }
        /**不允许邮寄 */
        private bool isNoEmailToPlayer(string key)
        {

            return false;
        }
        /**发送邮件 */
        public void sendEmail(JObject email)
        {
            //复制一份，不要去改变外部的引用
            email = strUtils.copyJSON<JObject>(email);
            //附件是完整的字段，需要删除不需要的字段
            JArray ens = new JArray();
            JArray list = (JArray)email["enclosure"];
            if ((int)email["price"] > 0 && list.Count==0)
            {
                msgCode.showMsg(216);
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                JObject en = (JObject)list[i];
                if ((int)en["nowNum"] <= 0)
                {
                    msgCode.showMsg(217);
                    return;
                }
                JObject obj = new JObject();
                obj.Add("Id", en["Id"]);
                obj.Add("num", en["nowNum"]);
                obj.Add("enType", en["enType"]);
                ens.Add(obj);
            }
            email["enclosure"] = ens;
            //Debug.Log(email);
            //if (true) return;

            //todo 发送后判断是否存在该用户，没有就退回邮件
            if ((int)email["price"] < 0)
            {
                msgCode.showMsg(651, "设置价格有误！");
                return;
            }
            if ((int)email["tale"] < 0)
            {
                msgCode.showMsg(651, "设置银两有误！");
                return;
            }
            if (!strUtils.isEnoughLen(email["title"].ToString(), 1, 10))
            {
                msgCode.showMsg(651, "主题文本长度有误！");
                return;
            }
            if (!strUtils.isEnoughLen(email["content"].ToString(), 1, 50))
            {
                msgCode.showMsg(651, "内容文本长度有误！");
                return;
            }
            if (!strUtils.isEnoughLen(email["receiver"].ToString(), 1, 6))
            {
                msgCode.showMsg(651, "收件人文本长度有误！");
                return;
            }
            if (((JArray)email["enclosure"]).Count > 3)
            {
                msgCode.showMsg(651, "附件超出3个！");
                return;
            }

            //验证是否足够
            JObject role = face.roleInterface.getRole();
            if ((int)role["attr"]["msg"]["tale"] - (int)email["tale"] < 0)
            {
                msgCode.showMsg(634);
                return;
            }


            email.Remove("sender");
            email.Remove("isObtain");

            Dictionary<string, object> dic = new Dictionary<string, object>();
            IEnumerable<JProperty> ps = email.Properties();
            foreach (JProperty p in ps)
            {
                dic.Add(p.Name, p.Value);
            }

            DoGet.getInstance().sendPost("/emailService/sendGoodsEmail", dic, (res) =>
            {
                if (int.Parse(res.ToString()) == 1)
                {
                    //附件、赠送银两、设置价格等
                    if ((int)email["tale"] > 0)
                    {
                        role["attr"]["msg"]["tale"] = (int)role["attr"]["msg"]["tale"] - (int)email["tale"];
                        face.roleInterface.saveRole(role);
                    }
                    if (((JArray)email["enclosure"]).Count > 0)
                    {
                        JArray ens = (JArray)email["enclosure"];
                        for (int p = 0; p < ens.Count; p++)
                        {
                            JObject a = (JObject)ens[p];
                            int enType = (int)a["enType"];
                            if (enType == 0)
                            {
                                face.goodsInterface.cutPlayerGoodsNum(a["Id"].ToString(), (int)a["num"]);
                            }
                            else
                            {
                                face.petInterface.remFromCache(a["Id"].ToString());
                            }
                            
                        }
                    }
                    msgCode.showMsg(677);
                }
                else
                {
                    msgCode.showMsg(-1);
                }
            });
        }
        /**接取附件 */
        public void getGoodsEmail(JObject obj)
        {
            JObject r = face.roleInterface.getRole();
            if ((int)obj["priceType"] == 1)
            {
                if ((int)r["attr"]["msg"]["tale"] - (int)obj["price"] < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            else
            {
                if ((int)r["attr"]["msg"]["gold"] - (int)obj["price"] < 0)
                {
                    msgCode.showMsg(634);
                    return;
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", obj["Id"].ToString());
            DoGet.getInstance().sendPost("/emailService/getGoodsEmail", dic, (res) =>
            {
                if ((int)obj["priceType"] == 1)
                {
                    r["attr"]["msg"]["tale"] = (int)r["attr"]["msg"]["tale"] - (int)obj["price"];
                }
                else
                {
                    r["attr"]["msg"]["gold"] = (int)r["attr"]["msg"]["gold"] - (int)obj["price"];
                }
                //提取银两
                if ((int)obj["tale"] > 0)
                {
                    r["attr"]["msg"]["tale"] = (int)r["attr"]["msg"]["tale"] + (int)obj["tale"];
                }
                //保存基本信息
                face.roleInterface.saveRole(r);
                //将状态设为已接收
                obj["isObtain"] = 1;
                obj["isRead"] = 1;
                //保存邮件
                this.saveEamil(obj);
                //缓存道具
                JArray ens = (JArray)obj["enclosure"];
                for (int p = 0; p < ens.Count; p++)
                {
                    JObject good = (JObject)ens[p];
                    int enType = (int)good["enType"];

                    //附件id和真实id的一个映射
                    JObject rs = (JObject)res;
                    //Debug.Log(rs);
                    IEnumerable<JProperty> properties = rs.Properties();
                    foreach (JProperty item in properties)
                    {
                        JObject t = (JObject)item.Value;
                        if ((int)t["enType"] == enType && good["Id"].ToString().Equals(item.Name.ToString()))
                        {
                            //替换成真实id
                            good["Id"] = t["Id"].ToString();
                            break;
                        }
                    }
                    good.Remove("enType");
                    if (enType == 0)
                    {
                        face.goodsInterface.copyGoods(good);
                    }
                    else
                    {
                        face.petInterface.savePet(good);
                    }
                    //delete good['addNum'];
                }
                msgCode.showMsg(784);
            });


        }
        public void receiveEmail(string Id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/emailService/getOne", dic, (res) =>
            {
                taskManager.getInstance().putTask(() =>
                {
                    this.saveEamil((JObject)res);
                });
                this.noticeUpdateUI();
            });

        }
        /**设置为已读取 */
        public void readEmail(string Id, Action ac)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/emailService/readEmail", dic, (res) =>
            {
                JArray list = this.getEmailList();
                foreach (object p in list)
                {
                    JObject a = (JObject)p;
                    if (a["Id"].ToString().Equals(Id))
                    {
                        a["isRead"] = 1;
                        this.saveEamil(a);
                        break;
                    }
                }
            });
            ac();
        }
        /**退回邮件*/
        public void tuihuiEmail(string Id)
        {
            JArray list = this.getEmailList();
            int index = -1;
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (a["Id"].ToString().Equals(Id))
                {
                    index = p;
                    break;
                }
            }
            if (index == -1)
            {
                msgCode.showMsg(0);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/emailService/tuihuiEmail", dic, (res) =>
            {
                list.RemoveAt(index);
                this.initEmailList(list);
                this.noticeUpdateUI();
            });

        }
        public void delEmailFromCache(string Id)
        {
            JArray list = this.getEmailList();
            int index = -1;
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (a["Id"].ToString().Equals(Id))
                {
                    index = p;
                    break;
                }
            }
            if (index == -1)
            {
                return;
            }
            list.RemoveAt(index);
            this.initEmailList(list);
        }
        /**删除邮件 */
        public void delEmail(string Id)
        {
            JArray list = this.getEmailList();
            int index = -1;
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (a["Id"].ToString().Equals(Id))
                {
                    if ((int)a["price"] > 0 && (int)a["isObtain"] == 0)
                    {
                        msgCode.showMsg(980);
                        return;
                    }
                    index = p;
                    break;
                }
            }
            if (index == -1)
            {
                msgCode.showMsg(0);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Id", Id);
            DoGet.getInstance().sendPost("/emailService/delEmail", dic, (res) =>
            {
                list.RemoveAt(index);
                this.initEmailList(list);
                this.noticeUpdateUI();
            });

        }
        /**删除邮件
         * type 0删除已读 1删除所有（两者都是除了未接取的）
         */
        public void delEmailByType(int type)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("type", type);
            DoGet.getInstance().sendPost("/emailService/delEmailByType", dic, (res) =>
            {
                JArray list = this.getEmailList();
                for (int p = 0; p < list.Count; p++)
                {
                    JObject a = (JObject)list[p];
                    if ((a["enclosure"] != null && ((JArray)a["enclosure"]).Count() > 0) ||
                    (int)a["tale"] > 0)
                    {
                        if ((int)a["isObtain"] == 0) continue;
                    }
                    if (type == 0 && (int)a["isRead"] == 1)
                    {
                        list.RemoveAt(p);
                        p--;
                    }
                    else if (type == 1)
                    {
                        list.RemoveAt(p);
                        p--;
                    }
                }
                this.initEmailList(list);
                this.noticeUpdateUI();
            });

        }
        /**保存邮件 */
        public void saveEamil(JObject email)
        {
            JArray list = this.getEmailList();
            bool b = false;
            for (int p = 0; p < list.Count; p++)
            {
                JObject a = (JObject)list[p];
                if (a["Id"].ToString().Equals(email["Id"].ToString()))
                {
                    b = true;//列表中存在该邮件
                    list[p] = email;
                    break;
                }
            }
            if (!b)
            {
                list.Add(email);
            }
            this.initEmailList(list);
            this.noticeUpdateUI();
        }
        /**通知更新邮件ui */
        private void noticeUpdateUI()
        {
            JObject j = new JObject();
            j.Add("callback", "802");
            eventsUtils.dispatchEvent("ws", j);
        }
        /**获取邮件列表 */
        public JArray getEmailList()
        {
            JArray list = dbHandle.get<JArray>("emailList");
            //将邮件按时间排序
            return new JArray(list.OrderByDescending(obj => (string)obj["created"]));
        }
        /**初始化邮件 */
        public void initEmailList(JArray emailList)
        {
            dbHandle.save("emailList", emailList);
        }
    }
}
