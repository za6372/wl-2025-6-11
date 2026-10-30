using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.shape;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.ui.activityPage;
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
    class GangsListPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("帮派列表");
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
            drawK2(content);
            drawList();
        }
        private void drawK2(Transform content)
        {
            Vector2 size = content.GetComponent<RectTransform>().sizeDelta;
            GameObject k2 = gameObjPool.getInstance().get("k2", typeof(SimpleUI));
            k2.transform.SetParent(content.transform, false);
            k2.GetComponent<SimpleUI>()
                .setSizePos(new Vector2(size.x, size.y ), new Vector2(0, 0));

            //列表部分
            bgStyle1 k1 = bgStyle1.create(new Vector2(size.x - 60, size.y  - 60), new Vector2(30, -30), k2.transform);

            GameObject sure = gameObjPool.getInstance().get("create", typeof(BtnUI));
            sure.transform.SetParent(this.transform.Find("Bottom"), false);
            sure.GetComponent<BtnUI>()
            .setSizePos(new Vector2(168, 96), new Vector2(0, 0));
            sure.GetComponent<BtnUI>().addText("创建").setTextColor().loadRes("gy_03_png")
                .addClk(() =>
                {
                    UseNumInputUI.create(this.transform).renderText("请输入帮派名").addTextCallback((bpName) =>
                    {
                        face.gangsInterface.create(bpName,()=> { this.drawList(); });
                    });
                });
            sure.GetComponent<BtnUI>().getTextUI().gameObject
                .AddComponent<UIOutline>().setSome(new Color32(136, 136, 136, 255), new Vector2(2, -2));
        }
        private int pageNum = 1;
        private int totalPage = 0;
        /**绘制列表*/
        public void drawList()
        {
            Transform content = this.getStandardPageContent().Find("k2/bgStyle1").GetComponent<bgStyle1>().getContent();
            gameObjPool.getInstance().freeChildren(content.gameObject);
            face.gangsInterface.viewList(pageNum, (res)=> {
                JArray list = (JArray) res["list"];
                totalPage = (int)res["totalPage"];
                GoodsItem gdItem = GoodsItem.create(list, content.transform,3);
                gdItem.addCallback((mIndex) =>
                {
                    List<string> ml = new List<string>();
                    ml.Add("查看");
                    ml.Add("申请加入");
                    ml.Add("手动输入");
                    Menu menu = Menu.create(ml, PointGet.getTipCanvas());
                    menu.addCallback((mIndex2) =>
                    {
                        if (mIndex2 == 0)
                        {
                            PageUI.createAcPage<GangsMembersPage>(PointGet.getIndexPageOfPage()).drawUI(true, list[mIndex]["Id"].ToString());
                            PointGet.getAcPage<GangsMembersPage>().GetComponent<GangsMembersPage>().clkTab(0);
                        }
                        else if (mIndex2 == 1)
                        {
                            face.gangsInterface.reqInto(list[mIndex]["Id"].ToString());
                        }
                        else if (mIndex2 == 2)
                        {

                        }
                    });
                }).addScrollTopCall(()=> {
                    pageNum--;
                    if(pageNum<1)
                    {
                        pageNum = 1;
                        return;
                    }
                    drawList();
                }).addScrollBottomCall(() => {
                    pageNum++;
                    if (pageNum >totalPage)
                    {
                        pageNum = totalPage;
                        return;
                    }
                    drawList();
                });
            });
            
        }
        public override void init()
        {

        }
    }
}
