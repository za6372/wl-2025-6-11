using Assets.HotFix.MyUtils.src;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using Assets.HotFix.xq2d.src.fight;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class FightPage : PageUI
    {
        private bool isDoingUpdate = false;
        private bool isDrawOver1 = false;
        private bool isDrawOver2 = false;
        private bool isAllowedLoadRole = false;
        private JArray roleList;
        private JArray monsterList;
        private void FixedUpdate()
        {
            if (!isDoingUpdate&& isDrawOver1&& isDrawOver2)
            {
                isDoingUpdate = true;
                JObject a = fightCache.getInstance().wsMsg3009;
                if (a != null)
                {
                    partsMenu.getInstance().drawMsg3009(a);
                    fightCache.getInstance().setWsMsg3009(null);
                }
                isDoingUpdate = false;
            }
            //没有加载地图并且已经拉取完战斗信息时才允许创建战斗场景
            //Debug.Log(fightCache.getInstance().isDownloadMsg+"/"+ !mapManager.getInstance().isDoingLoadingMap);
            if (fightCache.getInstance().isDownloadMsg &&!mapManager.getInstance().isDoingLoadingMap)
            {
                
                fightCache.getInstance().isDownloadMsg = false;
                PointGet.getIndexPage().GetComponent<IndexPage>().freeThisPage();
                this.drawUI();
                isAllowedLoadRole = true;
            }

            if (isAllowedLoadRole)
            {
                isAllowedLoadRole = false;
                timeManager.getTimeManageOne().putDelayTask(() =>
                {
                    this.loadPlayer();
                }, 100);
            }
        }
        /**设置排序好的玩家及怪物*/
        public void putOrderRoleAndMonster(JArray roleList,JArray monsterList)
        {
            this.roleList = roleList;
            this.monsterList = monsterList;
        }
        public void drawUI()
        {
            GameObject map = gameObjPool.getInstance().get("Map", typeof(SimpleUI));
            map.transform.SetParent(this.transform, false);
            map.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);
            map.AddComponent<RectMask2D>();

            GameObject top = gameObjPool.getInstance().get("Top", typeof(SimpleUI));
            top.transform.SetParent(this.transform, false);
            top.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(ScreenUtils.width, 90), Vector2.zero);
            drawTop(top.transform);

            GameObject modelLayer = gameObjPool.getInstance().get("modelLayer", typeof(SimpleUI));
            modelLayer.transform.SetParent(this.transform, false);
            modelLayer.GetComponent<SimpleUI>().setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);
            /*GameObject touchLayer = gameObjPool.getInstance().get("touchLayer", typeof(SimpleUI));
            touchLayer.transform.SetParent(this.transform, false);
            touchLayer.GetComponent<SimpleUI>().setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);*/
            //部件层（按钮、聊天窗、任务框等）
            GameObject partsLayer = gameObjPool.getInstance().get("partsLayer", typeof(SimpleUI));
            partsLayer.transform.SetParent(this.transform, false);
            partsLayer.GetComponent<SimpleUI>().setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);
            //至少有 1 个基础组件（Text、Image、RawImage），否则事件无法触发
            //partsLayer.AddComponent<RawImage>().color = new Color(0, 0, 0, 0);

            //this.drawTestBtn(partsLayer);
            partsMenu.getInstance().create();
            //页面层
            GameObject pageLayer = gameObjPool.getInstance().get("pageLayer", typeof(SimpleUI));
            pageLayer.transform.SetParent(this.transform, false);
            pageLayer.GetComponent<SimpleUI>().setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), Vector2.zero);

            JObject r = face.roleInterface.getRole();
            string mapKey = r["pos"]["map"].ToString();
            //MapUtils.reloadFightBef(mapKey);
            mapManager.getInstance().loadFightMap(mapKey,()=> {
                partsMenu.getInstance().isHideChatWindow();
            });
            //todo:隐藏npc、建筑、玩家等
            /*timeManager.getTimeManageOne().putDelayTask(() =>
            {
                this.loadData();
            },2000);*/



        }



        
        
        private void loadPlayer()
        {
            //todo:先将怪物、角色、特效等 战斗的图集合并，再调取，切换帧图时需将上一个帧图做缓存
            //从上至下加载，4、9、2、7、0、5、1、6、3、8
            
            playerOperate.getInstance().loadList(roleList,()=> { isDrawOver1 = true; });
            playerOperate.getInstance().loadList(monsterList,()=> { isDrawOver2 = true; });
            
        }
        
        private void drawTop(Transform ts)
        {
            GameObject img = gameObjPool.getInstance().get("Bg", typeof(ImgUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<ImgUI>().setColor("#000000")
            .setSizePos(new Vector2(ScreenUtils.width, 90), new Vector2(0, 0));

            img = gameObjPool.getInstance().get("Title", typeof(TextUI));
            img.transform.SetParent(ts, false);
            img.GetComponent<TextUI>().setText("").setFontSize(30).setColor().setAlign()
            .setSizePos(new Vector2(250, 90), new Vector2(ScreenUtils.width - 250, 0));
        }
        public override void init()
        {

        }
    }
}
