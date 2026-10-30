using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.HotFix.xq2d.src.ui.part;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class ForgetPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("备忘");
            this.setCancelCallback(() =>
            {
                this.freeThisPage();

            });
            Transform content = this.getStandardPageContent();
            drawK1(content);
            DoGet.getInstance().startReqImg();
        }



        private void drawK1(Transform content)
        {
            //选项卡
            List<string> ml = new List<string>();
            ml.Add("活动");
            ml.Add("任务");
            ml.Add("帮派");
            ml.Add("副本");
            ml.Add("其他");
            Tab tab = Tab.create(ml, Vector2.zero, content.transform);
            tab.addCallback((mIndex) =>
            {

                drawList(mIndex);
                DoGet.getInstance().startReqImg();
            });
            drawK2(content);

            tab.clkDefault();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y - 100), new Vector2(0, -100));

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y - 100 - 60), new Vector2(30, -30), k2.transform);
        }
        private void getOneMsg(string name,string key, JArray list)
        {
            JObject a = new JObject();
            a["name"] = name;
            a["key"] = key;
            list.Add(a);
        }
        /**绘制列表*/
        public void drawList(int index)
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            JArray list = new JArray();
            if (index == 0)//活动
            {
                string[] arr = face.activityInterface.getNoLimitTimeAcList();
                list = face.activityInterface.keysToObj(arr);
            }
            else if (index == 1)//任务
            {
                JArray arr = face.taskInterface.getCacheTaskListByStatus(1);
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject a = (JObject)arr[i];
                    task tk = face.taskInterface.getTask(a["key"].ToString());
                    getOneMsg(tk.getProgressTitle(), tk.key, list);
                }
            }
            else if (index == 2)//帮派
            {
                string[] arr = { "bpz", "bpps", "fsywt", "bprw", };
                list = face.activityInterface.keysToObj(arr);
            }
            else if (index == 3)//副本
            {
                List<string> al = new List<string>();
                for(int i = 0; i < 10; i++)
                {
                    al.Add("fb_" + i);
                }
                list = face.activityInterface.keysToObj(al.ToArray());
            }
            else if (index == 4)//其他
            {

            }

            GoodsItem gdItem = GoodsItem.create(list, content.transform, -1);
            gdItem.addCallback((mIndex) =>
            {
                List<string> ml = new List<string>();
                if (index == 1)
                {
                    ml.Add("瞬间传送");
                }
                else
                {
                    ml.Add("瞬间传送");
                    ml.Add("查看详情");
                }

                Menu menu = Menu.create(ml, this.transform);
                menu.addCallback((itemIndex) =>
                {
                    this.handle(list, mIndex, index, itemIndex);
                });
            });
        }
        private void handle(JArray list, int acIndex, int typeIndex, int itemIndex)
        {
            JObject obj = (JObject)list[acIndex];
            if (typeIndex == 1)
            {
                if (itemIndex == 0)//自动寻路
                {
                    JObject ts = face.taskInterface.getTaskFromCache(obj["key"].ToString());
                    mapManager.getInstance().findRoad(obj["key"].ToString(), (int)ts["progressIndex"]);
                    this.freeThisPage();
                }
            }
            else
            {
                if (itemIndex == 0)//自动寻路
                {
                    if (!obj.ContainsKey("posMsg")|| obj["posMsg"]==null)
                    {
                        msgCode.showMsg(641);
                        return;
                    }
                    JObject posMsg = (JObject)obj["posMsg"];
                    mapManager.getInstance().findRoad(posMsg["mapKey"].ToString(), posMsg["npcKey"].ToString());
                    this.freeThisPage();
                }
                else if (itemIndex == 1)//查看详情
                {
                    JObject acMsg = (JObject)obj["acMsg"];
                    Dialog dialog = Dialog.create(new Vector2(800, 400), this.transform);
                    dialog.renderText(acMsg["des"].ToString());
                    DoGet.getInstance().startReqImg();
                }
            }
            
            
        }
        public override void init()
        {

        }
    }
}
