using Assets.HotFix.MyUtils.src.factory;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.ui.basicUI
{//fixme:凡是部件类必须依赖于页面，实现时部件整体只设置大小，不设置位置，使用时才设置位置
    public class PartUI : BaseUI
    {
        public override void init()
        {
            this.gameObject.layer = 5;
        }
        /**添加一个默认遮罩*/
        public void addMask(float alpha = 0f)
        {
            GameObject bg = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            bg.transform.SetParent(this.transform);
            bg.GetComponent<ImgUI>().setColor(0,0,0,alpha)
            .setSizePos(new Vector2(Screen.width, Screen.height), Vector2.zero).addClk(() =>
            {
                this.free();
            });
        }
    }
}
