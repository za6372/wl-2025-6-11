using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.data
{
    public class Res3dLoadHandle: MonoBehaviour
    {
        //3d资源请求的队列
        private List<ResReqMsg> reqList = new List<ResReqMsg>();
        private bool isHandleReqList = false;
        private void Update()
        {
            if (isHandleReqList || reqList.Count == 0) return;
            singleLoad3dRes();
        }
        private void multyLoad3dRes()
        {
            isHandleReqList = true;
            ResReqMsg reqMsg = reqList[0];
            reqList.RemoveAt(0);
            isHandleReqList = false;
            DoGet doGet = this.GetComponent<DoGet>();
            timeManager.getTimeManageOne().putThread(() =>
            {
                doGet.loadMatchPrefab(reqMsg.key, (ab) =>
                {
                    
                    reqMsg.callback(ab);
                });
            });
            
        }
        private void singleLoad3dRes()
        {
            isHandleReqList = true;
            ResReqMsg reqMsg = reqList[0];
            this.GetComponent<DoGet>().loadMatchPrefab(reqMsg.key, (ab) =>
            {
                reqMsg.callback(ab);
                reqList.RemoveAt(0);
                isHandleReqList = false;
            });
        }
        /**所有ab包资源加载的入口*/
        public void add(string key, Action<AssetBundle> ac)
        {
            reqList.Add(new ResReqMsg(key, ac));

        }
    }
    public class ResReqMsg
    {
        public Action<AssetBundle> callback;
        public string key;

        public ResReqMsg(string key, Action<AssetBundle> callback)
        {
            this.callback = callback;
            this.key = key;
        }
    }
}
