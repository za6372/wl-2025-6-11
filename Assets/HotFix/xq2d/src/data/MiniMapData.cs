using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.data
{
    class MiniMapData
    {
        /**小地图展开后小格的信息*/
        public static List<MiniData> getMiniMapData(int index)
        {
            ////0城池 1小县 2山 3路 4箭头
            //箭头方向 0上 1下 2左 3右
            List<MiniData> list = new List<MiniData>();
            switch (index)
            {
                case 1:
                    {
                        new MiniData().addMsg(2, new Vector2(0, 4)).addKey("m_101").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 4)).addKey("m_100").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 2)).addKey("m_99").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 2)).addKey("m_98").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 4)).addKey("m_96").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 6)).addKey("m_97").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(6, 4)).addKey("m_95").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(8, 4)).addKey("m_92").addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(8, 2)).addKey("m_90").addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(8, 0)).addKey("m_94").addToList(list);
                        new MiniData().addMsg(3, new Vector2(9, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(10, 2)).addKey("m_91").addToList(list);
                        new MiniData().addMsg(3, new Vector2(10, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(10, 4)).addKey("m_93").addToList(list);
                        new MiniData().addMsg(3, new Vector2(9, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(11, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(12, 4)).addKey("m_89").addToList(list);
                        new MiniData().addMsg(3, new Vector2(10, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(10, 6)).addKey("m_88").addToList(list);
                        new MiniData().addMsg(3, new Vector2(11, 6)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(12, 6)).addKey("m_86").addToList(list);
                        new MiniData().addMsg(3, new Vector2(12, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(10, 7)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(10, 8)).addKey("m_87").addToList(list);
                        new MiniData().addMsg(3, new Vector2(11, 8)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(12, 8)).addKey("m_84").addToList(list);
                        new MiniData().addMsg(3, new Vector2(12, 7)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(12, 9)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(12, 10)).addKey("m_85").addToList(list);
                        new MiniData().addMsg(4, new Vector2(13, 8)).setDire(3,4).addToList(list);
                        return list;
                    }
                case 3:
                    {
                        new MiniData().addMsg(1, new Vector2(0, 0)).addKey("m_1").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 2)).addKey("m_2").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 4)).addKey("m_3").addToList(list);
                        new MiniData().addMsg(4, new Vector2(1, 4)).setDire(3,6).addToList(list);
                        return list;
                    }
                case 4:
                    {
                        new MiniData().addMsg(4, new Vector2(0, 0)).setDire(2,1).addToList(list);
                        new MiniData().addMsg(2, new Vector2(1, 0)).addKey("m_83").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 0)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 0)).addKey("m_82").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 2)).addKey("m_80").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 4)).addKey("m_81").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(5, 2)).addKey("m_79").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 2)).addKey("m_78").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(1, new Vector2(5, 4)).addKey("m_76").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 4)).addKey("m_77").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(5, 6)).addKey("m_75").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 7)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(5, 8)).addKey("m_72").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 9)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(5, 10)).addKey("m_71").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 10)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 10)).addKey("m_70").addToList(list);
                        new MiniData().addMsg(4, new Vector2(3, 11)).setDire(1,8).addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 6)).addKey("m_74").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 6)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 7)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 8)).addKey("m_73").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 8)).setHori(true).addToList(list);
                        return list;
                    }
                case 5:
                    {
                        new MiniData().addMsg(2, new Vector2(2, 0)).addKey("m_51").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 0)).setHori(true).addToList(list);
                        new MiniData().addMsg(1, new Vector2(4, 0)).addKey("m_52").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 2)).addKey("m_44").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 2)).addKey("m_43").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 4)).addKey("m_42").addToList(list);
                        new MiniData().addMsg(4, new Vector2(0, 5)).setDire(1,9).addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 4)).addKey("m_45").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 2)).addKey("m_46").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 3)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 4)).addKey("m_47").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 4)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(6, 4)).addKey("m_50").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 5)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 6)).addKey("m_48").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 6)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(6, 6)).addKey("m_49").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 4)).setHori(true).addToList(list);
                        return list;
                    }
                case 6:
                    {
                        new MiniData().addMsg(4, new Vector2(0, 0)).setDire(2,3).addToList(list);
                        new MiniData().addMsg(2, new Vector2(1, 0)).addKey("m_4").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 0)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 0)).addKey("m_5").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 0)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(5, 0)).addKey("m_6").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 0)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(7, 0)).addKey("m_7").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(5, 2)).addKey("m_8").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(7, 2)).addKey("m_9").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 1)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(9, 2)).addKey("m_10").addToList(list);
                        new MiniData().addMsg(3, new Vector2(10, 2)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(11, 2)).addKey("m_11").addToList(list);
                        new MiniData().addMsg(4, new Vector2(11, 3)).setDire(1,7).addToList(list);
                        return list;
                    }
                case 7:
                    {
                        new MiniData().addMsg(4, new Vector2(0, 0)).setDire(0,6).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 1)).addKey("m_12").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 1)).addKey("m_13").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 3)).addKey("m_14").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 3)).addKey("m_15").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(4, new Vector2(3, 3)).setDire(3,8).addToList(list);
                        return list;
                    }
                case 8:
                    {
                        new MiniData().addMsg(4, new Vector2(0, 1)).setDire(2,7).addToList(list);
                        new MiniData().addMsg(4, new Vector2(3, 0)).setDire(0,4).addToList(list);
                        new MiniData().addMsg(4, new Vector2(3, 12)).setDire(1,10).addToList(list);
                        new MiniData().addMsg(4, new Vector2(4, 9)).setDire(3,9).addToList(list);
                        new MiniData().addMsg(2, new Vector2(1, 1)).addKey("m_16").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 1)).addKey("m_17").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(1, 3)).addKey("m_18").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(3, 3)).addKey("m_19").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(5, 3)).addKey("m_20").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 4)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(5, 5)).addKey("m_22").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 5)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(3, 5)).addKey("m_21").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 4)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 6)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 7)).addKey("m_23").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 8)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 9)).addKey("m_24").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 10)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 11)).addKey("m_25").addToList(list);
                        return list;
                    }
                case 9:
                    {
                        new MiniData().addMsg(4, new Vector2(0, 3)).setDire(2,8).addToList(list);
                        new MiniData().addMsg(4, new Vector2(9, 0)).setDire(0,5).addToList(list);
                        new MiniData().addMsg(2, new Vector2(1, 3)).addKey("m_32").addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(3, 3)).addKey("m_33").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(5, 3)).addKey("m_34").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(7, 3)).addKey("m_37").addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 3)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(9, 3)).addKey("m_38").addToList(list);
                        new MiniData().addMsg(3, new Vector2(9, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(9, 1)).addKey("m_36").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(7, 1)).addKey("m_35").addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 4)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 5)).addKey("m_39").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 6)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 7)).addKey("m_40").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 8)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(7, 9)).addKey("m_41").addToList(list);
                        return list;
                    }
                case 10:
                    {
                        new MiniData().addMsg(4, new Vector2(6, 0)).setDire(0,8).addToList(list);
                        new MiniData().addMsg(4, new Vector2(4, 4)).setDire(1,12).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 1)).addKey("m_30").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 1)).addKey("m_29").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 1)).addKey("m_27").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 1)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(6, 1)).addKey("m_26").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 3)).addKey("m_31").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 3)).addKey("m_28").addToList(list);
                        return list;
                    }
                case 12:
                    {
                        new MiniData().addMsg(4, new Vector2(4, 0)).setDire(0,10).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 1)).addKey("m_53").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 2)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 3)).addKey("m_54").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 4)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 5)).addKey("m_55").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 6)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 5)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 5)).addKey("m_66").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 5)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 5)).addKey("m_69").addToList(list);
                        new MiniData().addMsg(3, new Vector2(0, 6)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(0, 7)).addKey("m_68").addToList(list);
                        new MiniData().addMsg(3, new Vector2(1, 7)).setHori(true).addToList(list);
                        new MiniData().addMsg(3, new Vector2(2, 6)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(2, 7)).addKey("m_67").addToList(list);
                        new MiniData().addMsg(3, new Vector2(3, 7)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(4, 7)).addKey("m_56").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 8)).setHori(false).addToList(list);
                        new MiniData().addMsg(0, new Vector2(4, 9)).addKey("m_58").addToList(list);
                        new MiniData().addMsg(3, new Vector2(4, 10)).setHori(false).addToList(list);
                        new MiniData().addMsg(2, new Vector2(4, 11)).addKey("m_60").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 7)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(6, 7)).addKey("m_57").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 7)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(8, 7)).addKey("m_63").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 9)).setHori(true).addToList(list);
                        new MiniData().addMsg(0, new Vector2(6, 9)).addKey("m_59").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 9)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(8, 9)).addKey("m_62").addToList(list);
                        new MiniData().addMsg(3, new Vector2(9, 9)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(10, 9)).addKey("m_64").addToList(list);
                        new MiniData().addMsg(3, new Vector2(5, 11)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(6, 11)).addKey("m_61").addToList(list);
                        new MiniData().addMsg(3, new Vector2(7, 11)).setHori(true).addToList(list);
                        new MiniData().addMsg(2, new Vector2(8, 11)).addKey("m_65").addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 8)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 8)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(6, 10)).setHori(false).addToList(list);
                        new MiniData().addMsg(3, new Vector2(8, 10)).setHori(false).addToList(list);

                        return list;
                    }
            }
            return list;
        }
    }
}
