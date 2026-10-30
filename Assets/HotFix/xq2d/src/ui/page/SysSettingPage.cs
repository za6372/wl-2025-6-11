using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.ui.basicUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.page
{
    class SysSettingPage : PageUI
    {
        public void drawUI()
        {
            this.createStandardPageLayout();
            this.setTitle("系统设置");
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
            
        }
        
        public override void init()
        {

        }
    }
}
