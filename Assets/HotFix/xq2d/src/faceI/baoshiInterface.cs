using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface baoshiInterface
    {

        public int getInlayLv(int lv);
        public JArray getBaoshiListByPart(string part);
        public JArray getPlayerBaoshiList();
        public void composeInlayBaoshi(string bsKey, Action callback);
    }
    public class baoshiInterfaceImpl : baoshiInterface
    {
        public void composeInlayBaoshi(string bsKey,Action callback)
        {
            if (!face.goodsInterface.isEnoughInPackAndTip(bsKey,5))
            {
                return;
            }
            Baoshi bs = (Baoshi)face.goodsInterface.getGoodsMsgByKey(bsKey);
            if (bs.lv >= 10)
            {
                msgCode.showMsg(656);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", bsKey);
            DoGet.getInstance().sendPost("/packageService/composeInlayBaoshi", dic, (res) =>
            {
                if (res.ToString().Equals("0"))
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 3);
                    msgCode.showMsg(659);
                }
                else
                {
                    face.goodsInterface.cutPlayerGoodsNumByKey(bsKey, 5);
                    face.rewardInterface.saveRewards((JArray)res);
                    msgCode.showMsg(658);
                }
                
                callback();
            });
        }
        /**根据装备部位获取可镶嵌的宝石 */
        public JArray getBaoshiListByPart(string part)
        {
            JArray arr = new JArray();
            JArray list = this.getPlayerBaoshiList();
            foreach (object p in list)
            {
                JObject obj = (JObject)p;
                Baoshi msg = (Baoshi)face.goodsInterface.getGoodsMsgByKey(obj["key"].ToString());
                string[] parts = msg.parts;
                foreach (object i in parts)
                {
                    if (part.Equals("fb")||part.Equals(i.ToString()))
                    {
                        arr.Add(obj);
                        break;
                    }
                }
            }
            return arr;
        }
        /**获取玩家宝石 */
        public JArray getPlayerBaoshiList()
        {
            JArray arr = new JArray();
            //获取玩家道具
            JArray list = face.goodsInterface.getAllGoods();
            //获取宝石
            foreach (object p in list)
            {
                JObject obj = (JObject)p;
                //属于宝石的key
                if (strUtils.isMatch(obj["key"].ToString(), "1003([0-9]{4})"))
                {
                    arr.Add(obj);
                }
            }
            return arr;
        }
        public int getInlayLv(int lv)
        {
            return 0;
        }


    }
}
