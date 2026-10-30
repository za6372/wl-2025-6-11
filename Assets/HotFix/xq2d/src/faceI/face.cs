using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.HotFix.xq2d.src.faceI
{
    public static class face
    {
        public static roleInterface roleInterface = new roleInterfaceImpl();
        public static mapInterface mapInterface = new mapInterfaceImpl();
        public static teamInterface teamInterface = new teamInterfaceImpl();
        public static taskInterface taskInterface = new taskInterfaceImpl();
        public static rewardInterface rewardInterface = new rewardInterfaceImpl();
        public static goodsInterface goodsInterface = new goodsInterfaceImpl();
        public static emailInterface emailInterface = new emailInterfaceImpl();
        public static equipInterface equipInterface = new equipInterfaceImpl();
        public static skillInterface skillInterface = new skillInterfaceImpl();
        public static petInterface petInterface = new petInterfaceImpl();
        public static baoshiInterface baoshiInterface = new baoshiInterfaceImpl();
        public static goodsDesInterface goodsDesInterface = new goodsDesInterfaceImpl();
        public static chatInterface chatInterface = new chatInterfaceImpl();
        public static friendInterface friendInterface = new friendInterfaceImpl();
        public static playerInterface playerInterface = new playerInterfaceImpl();
        public static gangsInterface gangsInterface = new gangsInterfaceImpl();
        public static npcInterface npcInterface = new npcInterfaceImpl();
        public static fightInterface fightInterface = new fightInterfaceImpl();
        public static taskDesInterface taskDesInterface = new taskDesInterfaceImpl();
        public static monsterInterface monsterInterface = new monsterInterfaceImpl();
        public static activityInterface activityInterface = new activityInterfaceImpl();
        public static msgInterface msgInterface = new msgInterfaceImpl();
    }
}
