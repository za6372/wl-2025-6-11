using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.MyUtils.src.common;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface mapInterface
    {
        public MapData getMapByKey(string key);
    }
    public class mapInterfaceImpl : mapInterface
    {
        
        /**获取地图相关数据*/
        public MapData getMapByKey(string key)
        {
            MapData mapData = new MapData(key);
            mapData.manPos = new JObject();
            mapData.manPos.Add("x", ScreenUtils.width/2f);
            mapData.manPos.Add("y", (ScreenUtils.height-300)/2f);
            switch (key)
            {
                case "sgzc":
                    {
                        mapData.mapeId = "999960";
                        mapData.name = "上古战场";
                        mapData.des = "";
                        return mapData;
                    }
                case "m_1":
                    {
                        mapData.mapeId = "1001";
                        mapData.name = "Lv3-恒山村";
                        mapData.des = "依山傍水，隐世小村。";
                        return mapData;
                    }
                case "m_2":
                    {
                        mapData.mapeId = "1002";
                        mapData.name = "Lv6-恒山村郊";
                        mapData.des = "小村之郊，风柔林茂。\n出没的怪物：狼蛛";
                        mapData.addPetKeys("1001");
                        return mapData;
                    }
                case "m_3":
                    {
                        mapData.mapeId = "1003";
                        mapData.name = "Lv9-清溪谷";
                        mapData.des = "山清水秀，鸟鸣蝶舞。\n出没的怪物：棕毛土狼";
                        mapData.addPetKeys("1002");
                        return mapData;
                    }
                case "m_4":
                    {
                        mapData.mapeId = "2001";
                        mapData.name = "Lv11-赵村";
                        mapData.des = "要道之村，暗流涌动。\n出没的怪物：利爪幼虎、食尸虫";
                        mapData.addPetKeys("1003", "1004");
                        return mapData;
                    }
                case "m_5":
                    {
                        mapData.mapeId = "2002";
                        mapData.name = "Lv14-云雾谷";
                        mapData.des = "云雾缭绕，迷人山谷。\n出没的怪物：双头毒蛇、黑衣山贼";
                        mapData.addPetKeys("1005", "1006");
                        return mapData;
                    }
                case "m_6":
                    {
                        mapData.mapeId = "2003";
                        mapData.name = "汉中商业区";
                        mapData.des = "西蜀重地，汉家源头。本城市进行买卖交易的区域。";
                        return mapData;
                    }
                case "m_7":
                    {
                        mapData.mapeId = "2004";
                        mapData.name = "汉中活动区";
                        mapData.des = "西蜀重地，汉家源头。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_8":
                    {
                        mapData.mapeId = "2005";
                        mapData.name = "汉中工业区";
                        mapData.des = "西蜀重地，汉家源头。本城市炼化升级装备的区域。";
                        return mapData;
                    }
                case "m_9":
                    {
                        mapData.mapeId = "2006";
                        mapData.name = "汉中行政区";
                        mapData.des = "西蜀重地，汉家源头。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_10":
                    {
                        mapData.mapeId = "2007";
                        mapData.name = "Lv14-合江村";
                        mapData.des = "浩劫侵袭，残破村落。\n出没的怪物：焰火虫、异变尸煞";
                        mapData.addPetKeys("1007", "1008");
                        return mapData;
                    }
                case "m_11":
                    {
                        mapData.mapeId = "2008";
                        mapData.name = "Lv14-石松林";
                        mapData.des = "怪石嶙峋，青松蔽日。\n出没的怪物：铁钩手、山野顽猴";
                        mapData.addPetKeys("1009", "1010");
                        return mapData;
                    }
                case "m_12":
                    {
                        mapData.mapeId = "3001";
                        mapData.name = "Lv21-恶狼谷";
                        mapData.des = "恶狼盘踞，危机四伏。\n出没的怪物：赤眼霜狼、金仓鼠";
                        mapData.addPetKeys("1011", "1012");
                        return mapData;
                    }
                case "m_13":
                    {
                        mapData.mapeId = "3002";
                        mapData.name = "Lv24-黔岭";
                        mapData.des = "崇山险峻，百折成岭。\n出没的怪物：苍鹰、赤灵";
                        mapData.addPetKeys("1013", "1014");
                        return mapData;
                    }
                case "m_14":
                    {
                        mapData.mapeId = "3003";
                        mapData.name = "Lv27-九江";
                        mapData.des = "山峦重叠，江流汇聚。\n出没的怪物：江鳇鱼、狂暴兽人";
                        mapData.addPetKeys("1015", "1016");
                        return mapData;
                    }
                case "m_15":
                    {
                        mapData.mapeId = "3004";
                        mapData.name = "Lv29-故道";
                        mapData.des = "三秦险要，兵家重地。\n出没的怪物：草藤妖、轻骑兵";
                        mapData.addPetKeys("1017", "1018");
                        return mapData;
                    }
                case "m_16":
                    {
                        mapData.mapeId = "4001";
                        mapData.name = "Lv31-殷墟";
                        mapData.des = "繁华逝去，商周故地。\n出没的怪物：恶犬、大刀兵";
                        mapData.addPetKeys("1019", "1020");
                        return mapData;
                    }
                case "m_17":
                    {
                        mapData.mapeId = "4002";
                        mapData.name = "Lv32-安阳废墟";
                        mapData.des = "古城遗址，荒芜苍凉。\n出没的怪物：崩角牛、青竹怪";
                        mapData.addPetKeys("1021", "1022");
                        return mapData;
                    }
                case "m_18":
                    {
                        mapData.mapeId = "4003";
                        mapData.name = "Lv33-强盗山洞";
                        mapData.des = "幽深山洞，冷意袭人。\n出没的怪物：吸血蝙蝠、懒熊";
                        mapData.addPetKeys("1023", "1024");
                        return mapData;
                    }
                case "m_19":
                    {
                        mapData.mapeId = "4004";
                        mapData.name = "邯郸商业区";
                        mapData.des = "晋赵之地，昌盛繁荣。本城市进行买卖交易的区域。";
                        return mapData;
                    }
                case "m_20":
                    {
                        mapData.mapeId = "4005";
                        mapData.name = "邯郸活动区";
                        mapData.des = "晋赵之地，昌盛繁荣。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_21":
                    {
                        mapData.mapeId = "4006";
                        mapData.name = "邯郸工业区";
                        mapData.des = "晋赵之地，昌盛繁荣。本城市炼化升级装备的区域。";
                        return mapData;
                    }
                case "m_22":
                    {
                        mapData.mapeId = "4007";
                        mapData.name = "邯郸行政区";
                        mapData.des = "晋赵之地，昌盛繁荣。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_23":
                    {
                        mapData.mapeId = "4008";
                        mapData.name = "Lv35-邯郸城郊";
                        mapData.des = "环境优越，物资丰厚。\n出没的怪物：湛岚犬、树精";
                        mapData.addPetKeys("1025", "1026");
                        return mapData;
                    }
                case "m_24":
                    {
                        mapData.mapeId = "4009";
                        mapData.name = "Lv37-殇山";
                        mapData.des = "山高壁峭，路绝成殇。\n出没的怪物：食人狼、苍狼";
                        mapData.addPetKeys("1027", "1028");
                        return mapData;
                    }
                case "m_25":
                    {
                        mapData.mapeId = "4010";
                        mapData.name = "Lv39-百兽谷";
                        mapData.des = "百兽横行，恶梦山谷。\n出没的怪物：鹰狮、蜥蜴";
                        mapData.addPetKeys("1029", "1030");
                        return mapData;
                    }
                case "m_26":
                    {
                        mapData.mapeId = "5001";
                        mapData.name = "Lv41-鹰喙岭";
                        mapData.des = "山崖险峻，状如鹰喙。\n出没的怪物：红羽凶鹰、青岩兽、狩猎者";
                        mapData.addPetKeys("1031", "1032","1033");
                        return mapData;
                    }
                case "m_27":
                    {
                        mapData.mapeId = "5002";
                        mapData.name = "Lv42-隐龙山";
                        mapData.des = "气势雄浑，盘如卧龙。\n出没的怪物：龙纹蛟、古牙兽";
                        mapData.addPetKeys("1034", "1035");
                        return mapData;
                    }
                case "m_28":
                    {
                        mapData.mapeId = "5003";
                        mapData.name = "Lv44-栖凤岩";
                        mapData.des = "奇岩瑰丽，形若雏凤。\n出没的怪物：斧兵、铁锤兵、铁骑枪兵";
                        mapData.addPetKeys("1036", "1037", "1038");
                        return mapData;
                    }
                case "m_29":
                    {
                        mapData.mapeId = "5004";
                        mapData.name = "Lv45-鸿雁岭";
                        mapData.des = "峭壁惊鸿，悬岭落雁。\n出没的怪物：散仙、幽魂";
                        mapData.addPetKeys("1039", "1040");
                        return mapData;
                    }
                case "m_30":
                    {
                        mapData.mapeId = "5005";
                        mapData.name = "Lv47-青阳城郊";
                        mapData.des = "古城郊野，奥妙深藏。\n出没的怪物：泥石兵俑、遁甲兵、熔骨血尸";
                        mapData.addPetKeys("1041", "1042", "1043");
                        return mapData;
                    }
                case "m_31":
                    {
                        mapData.mapeId = "5006";
                        mapData.name = "Lv49-青阳古城";
                        mapData.des = "玄妙古城，奇绝诡异。\n出没的怪物：开山力士、恶灵、摄魂使者";
                        mapData.addPetKeys("1044", "1045", "1046");
                        return mapData;
                    }
                case "m_32":
                    {
                        mapData.mapeId = "6001";
                        mapData.name = "Lv51-白马坡";
                        mapData.des = "地平坡缓，白马不惊。\n出没的怪物：山贼哨兵、黑煞甲士、赤炎甲龟";
                        mapData.addPetKeys("1047", "1048", "1049");
                        return mapData;
                    }
                case "m_33":
                    {
                        mapData.mapeId = "6002";
                        mapData.name = "Lv52-黑风谷";
                        mapData.des = "黑雾缭绕，狂风呼啸。\n出没的怪物：积怨行尸、黑风狼、阴魁猴";
                        mapData.addPetKeys("1050", "1051", "1052");
                        return mapData;
                    }
                case "m_34":
                    {
                        mapData.mapeId = "6003";
                        mapData.name = "Lv54-紫竹林";
                        mapData.des = "紫竹苍翠，郁郁成林。\n出没的怪物：木精、利爪猛虎、褐甲蜥蜴";
                        mapData.addPetKeys("1053", "1054", "1055");
                        return mapData;
                    }
                case "m_35":
                    {
                        mapData.mapeId = "6004";
                        mapData.name = "吴中商业区";
                        mapData.des = "浩荡中原，丰沃繁盛。本城市进行买卖交易的区域。";
                        return mapData;
                    }
                case "m_36":
                    {
                        mapData.mapeId = "6005";
                        mapData.name = "吴中活动区";
                        mapData.des = "浩荡中原，丰沃繁盛。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_37":
                    {
                        mapData.mapeId = "6006";
                        mapData.name = "吴中工业区";
                        mapData.des = "浩荡中原，丰沃繁盛。本城市炼化升级装备的区域。";
                        return mapData;
                    }
                case "m_38":
                    {
                        mapData.mapeId = "6007";
                        mapData.name = "吴中行政区";
                        mapData.des = "浩荡中原，丰沃繁盛。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_39":
                    {
                        mapData.mapeId = "6008";
                        mapData.name = "Lv55-彭城城郊";
                        mapData.des = "楚军要地，征伐四起。\n出没的怪物：黑寡妇、擎斧恶汉、擎钩先锋";
                        mapData.addPetKeys("1056", "1057", "1058");
                        return mapData;
                    }
                case "m_40":
                    {
                        mapData.mapeId = "6009";
                        mapData.name = "Lv57-灵璧";
                        mapData.des = "灵石绚丽，壁画绝尘。\n出没的怪物：双刀大盗、绿食怪、沙虫、猎命鹫";
                        mapData.addPetKeys("1059", "1060", "1061","1062");
                        return mapData;
                    }
                case "m_41":
                    {
                        mapData.mapeId = "6010";
                        mapData.name = "Lv59-须水";
                        mapData.des = "清流成血，煞气凛冽。\n出没的怪物：古炽灵、千年树妖、魔怨雪狼";
                        mapData.addPetKeys("1063", "1064", "1065");
                        return mapData;
                    }
                case "m_42":
                    {
                        mapData.mapeId = "7001";
                        mapData.name = "Lv60-虎丘";
                        mapData.des = "岩壑之势，出于天成。\n出没的怪物：幼麟鳇鱼、盘蛟兽、血吸妖木";
                        mapData.addPetKeys("1066", "1067", "1068");
                        return mapData;
                    }
                case "m_43":
                    {
                        mapData.mapeId = "7002";
                        mapData.name = "Lv61-京县";
                        mapData.des = "一隅小城，凡庸之地。\n出没的怪物：叱炎犬、恶浪蛟、荒野僵尸";
                        mapData.addPetKeys("1069", "1070", "1071");
                        return mapData;
                    }
                case "m_44":
                    {
                        mapData.mapeId = "7003";
                        mapData.name = "Lv62-鹿原";
                        mapData.des = "中原逐鹿，征伐之处。\n出没的怪物：伴生妖蛇、火帘鹰、紫魂使魔";
                        mapData.addPetKeys("1072", "1073", "1074");
                        return mapData;
                    }
                case "m_45":
                    {
                        mapData.mapeId = "7004";
                        mapData.name = "Lv63-荥阳城郊";
                        mapData.des = "战略之地，兵家必争。\n出没的怪物：山越兽人、巨掌黑熊、破劫半仙";
                        mapData.addPetKeys("1075", "1076", "1077");
                        return mapData;
                    }
                case "m_46":
                    {
                        mapData.mapeId = "7005";
                        mapData.name = "Lv64-索亭";
                        mapData.des = "大索之地，成名于楚。\n出没的怪物：弓骑兵、冲锋斧手、蓝魔";
                        mapData.addPetKeys("1078", "1079", "1080");
                        return mapData;
                    }
                case "m_47":
                    {
                        mapData.mapeId = "7006";
                        mapData.name = "Lv65-六合谷";
                        mapData.des = "六合之形，气蕴于谷。\n出没的怪物：虚魂犬、震岳荒兽、紫命玄魄";
                        mapData.addPetKeys("1081", "1082", "1083");
                        return mapData;
                    }
                case "m_48":
                    {
                        mapData.mapeId = "7007";
                        mapData.name = "Lv66-太行山";
                        mapData.des = "五行聚势，八径克敌。\n出没的怪物：纳灵竹妖、白首兽、藤甲射手";
                        mapData.addPetKeys("1084", "1085", "1086");
                        return mapData;
                    }
                case "m_49":
                    {
                        mapData.mapeId = "7008";
                        mapData.name = "Lv67-井径";
                        mapData.des = "易守难攻，兵家胜地。\n出没的怪物：啮齿鼠、狼人战士、飞廉骑兵";
                        mapData.addPetKeys("1087", "1088", "1089");
                        return mapData;
                    }
                case "m_50":
                    {
                        mapData.mapeId = "7009";
                        mapData.name = "Lv68-潍水";
                        mapData.des = "悠悠之水，源远流长。\n出没的怪物：大刀护卫、业火狼人、亡命逃兵";
                        mapData.addPetKeys("1090", "1091", "1092");
                        return mapData;
                    }
                case "m_51":
                    {
                        mapData.mapeId = "7010";
                        mapData.name = "Lv69-坠星岭";
                        mapData.des = "高耸入云，流星坠岭。\n出没的怪物：飞羽死士、凶牙血蝠、长毛猛犸";
                        mapData.addPetKeys("1093", "1094", "1095");
                        return mapData;
                    }
                case "m_52":
                    {
                        mapData.mapeId = "7011";
                        mapData.name = "安邑";
                        mapData.des = "魏国都城，争霸之地。";
                        return mapData;
                    }
                case "m_53":
                    {
                        mapData.mapeId = "8001";
                        mapData.name = "Lv70-万仞岭";
                        mapData.des = "壁立千仞，自成绝岭。\n出没的怪物：血魄炼尸、吸魄魔蛛、啸冥犬";
                        mapData.addPetKeys("1096", "1097", "1098");
                        return mapData;
                    }
                case "m_54":
                    {
                        mapData.mapeId = "8002";
                        mapData.name = "Lv70-不归道";
                        mapData.des = "去之难归，死地古道。\n出没的怪物：巨斧死士、冷血刀客、嗜血狂鹰";
                        mapData.addPetKeys("1099", "1100", "1101");
                        return mapData;
                    }
                case "m_55":
                    {
                        mapData.mapeId = "8003";
                        mapData.name = "Lv71-黄泉谷";
                        mapData.des = "怨灵游荡，死气盈谷。\n出没的怪物：丧魂魔将、幽冥之狼、赤瞳魔俑";
                        mapData.addPetKeys("1102", "1103", "1104");
                        return mapData;
                    }
                case "m_56":
                    {
                        mapData.mapeId = "8004";
                        mapData.name = "酆都商业区";
                        mapData.des = "万鬼之都，幽冥之城。本城市进行买卖交易的区域。";
                        return mapData;
                    }
                case "m_57":
                    {
                        mapData.mapeId = "8005";
                        mapData.name = "酆都活动区";
                        mapData.des = "万鬼之都，幽冥之城。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_58":
                    {
                        mapData.mapeId = "8006";
                        mapData.name = "酆都工业区";
                        mapData.des = "万鬼之都，幽冥之城。本城市炼化升级装备的区域。";
                        return mapData;
                    }
                case "m_59":
                    {
                        mapData.mapeId = "8007";
                        mapData.name = "酆都行政区";
                        mapData.des = "万鬼之都，幽冥之城。本城市负责各种活动的区域。";
                        return mapData;
                    }
                case "m_60":
                    {
                        mapData.mapeId = "8008";
                        mapData.name = "Lv71-双桂山";
                        mapData.des = "死地扼要，镇邪无双。\n出没的怪物：冥府守卫、阴风豹，夺命将军";
                        mapData.addPetKeys("1105", "1106", "1107");
                        return mapData;
                    }
                case "m_61":
                    {
                        mapData.mapeId = "8009";
                        mapData.name = "Lv72-大成殿";
                        mapData.des = "魂炼大成，修玄之殿。\n出没的怪物：吸魂木妖、夺魄护卫、阴火虫";
                        mapData.addPetKeys("1108", "1109", "1110");
                        return mapData;
                    }
                case "m_62":
                    {
                        mapData.mapeId = "8010";
                        mapData.name = "Lv73-名山";
                        mapData.des = "承世之名，幽绝天下。\n出没的怪物：游荡孤魂、青炎妖狼、巨灵守卫";
                        mapData.addPetKeys("1111", "1112", "1113");
                        return mapData;
                    }
                case "m_63":
                    {
                        mapData.mapeId = "8011";
                        mapData.name = "Lv74-鬼门关";
                        mapData.des = "死灵来处，魂魄归所。\n出没的怪物：引路使者、白魔猿、玄魄妖";
                        mapData.addPetKeys("1114", "1115", "1116");
                        return mapData;
                    }
                case "m_64":
                    {
                        mapData.mapeId = "8012";
                        mapData.name = "Lv75-天子殿";
                        mapData.des = "坐观轮回，运筹生死。\n出没的怪物：丧魂魔尸、地狱犬、般涅雏凤";
                        mapData.addPetKeys("1117", "1118", "1119");
                        return mapData;
                    }
                case "m_65":
                    {
                        mapData.mapeId = "8013";
                        mapData.name = "Lv76-阴阳界";
                        mapData.des = "审判阴阳，裁决功过。\n出没的怪物：恋尘阴灵、毒尸怪，阻阳界灵";
                        mapData.addPetKeys("1120", "1121", "1122");
                        return mapData;
                    }
                case "m_66":
                    {
                        mapData.mapeId = "8014";
                        mapData.name = "Lv77-绝命渊";
                        mapData.des = "绝境试炼，判命魔渊。\n出没的怪物：幽玄枪客、枯煞木灵、白苍魔狼";
                        mapData.addPetKeys("1123", "1124", "1125");
                        return mapData;
                    }
                case "m_67":
                    {
                        mapData.mapeId = "8015";
                        mapData.name = "Lv78-奈河";
                        mapData.des = "血色腥风，轮回之河。\n出没的怪物：奈河守将、厌世花、黑魇兽";
                        mapData.addPetKeys("1126", "1127", "1128");
                        return mapData;
                    }
                case "m_68":
                    {
                        mapData.mapeId = "8016";
                        mapData.name = "Lv79-奈何桥";
                        mapData.des = "前生遗弃，往生之桥。\n出没的怪物：吞魂兽、阴冥护卫、幽蓝匠魂";
                        mapData.addPetKeys("1129", "1130", "1131");
                        return mapData;
                    }
                case "m_69":
                    {
                        mapData.mapeId = "8017";
                        mapData.name = "Lv79-酆都内府";
                        mapData.des = "鬼城中心，统御之地。\n出没的怪物：守魂兽、冥仙、通灵鼠";
                        mapData.addPetKeys("1132", "1133", "1134");
                        return mapData;
                    }
                case "m_70":
                    {
                        mapData.mapeId = "9001";
                        mapData.name = "Lv80-四象台";
                        mapData.des = "隐世清地，四皓居所。\n出没的怪物：通玄鳇鱼、天煞老妖、化梦犬";
                        mapData.addPetKeys("1135", "1136", "1137");
                        return mapData;
                    }
                case "m_71":
                    {
                        mapData.mapeId = "9002";
                        mapData.name = "Lv81-宛城城郊";
                        mapData.des = "三山环绕，中原福地。\n出没的怪物：沙化蜥蜴、天刀护卫、破军猿王";
                        mapData.addPetKeys("1138", "1139", "1140");
                        return mapData;
                    }
                case "m_72":
                    {
                        mapData.mapeId = "9003";
                        mapData.name = "Lv81-朱雀岭";
                        mapData.des = "炽火成灵，朱雀遗址。\n出没的怪物：苍刑飞骑、玄影妖灵、诛灵天鹰";
                        mapData.addPetKeys("1141", "1142", "1143");
                        return mapData;
                    }
                case "m_73":
                    {
                        mapData.mapeId = "9004";
                        mapData.name = "Lv81-勾陈谷";
                        mapData.des = "众星所向，勾陈遗址。\n出没的怪物：阴阳玄蛇、暗影妖狼、勾陈古树";
                        mapData.addPetKeys("1144", "1145", "1146");
                        return mapData;
                    }
                case "m_74":
                    {
                        mapData.mapeId = "9005";
                        mapData.name = "Lv82-青龙洞";
                        mapData.des = "风雷并涌，青龙遗址。\n出没的怪物：龙胆将军、青莲竹妖、龙血鳇鱼";
                        mapData.addPetKeys("1147", "1148", "1149");
                        return mapData;
                    }
                case "m_75":
                    {
                        mapData.mapeId = "9006";
                        mapData.name = "Lv82-颖川";
                        mapData.des = "文明源地，人杰地灵。\n出没的怪物：沧浪妖狼、翻江藤、镇川巨熊";
                        mapData.addPetKeys("1150", "1151", "1152");
                        return mapData;
                    }
                case "m_76":
                    {
                        mapData.mapeId = "9007";
                        mapData.name = "成皋";
                        mapData.des = "山岭高矗，比邻长河。";
                        return mapData;
                    }
                case "m_77":
                    {
                        mapData.mapeId = "9008";
                        mapData.name = "Lv83-广武山";
                        mapData.des = "广延崇峻，襄武之地。\n出没的怪物：赤影妖蝠、盘丝玄蛛、龙爪凶狼";
                        mapData.addPetKeys("1153", "1154", "1155");
                        return mapData;
                    }
                case "m_78":
                    {
                        mapData.mapeId = "9009";
                        mapData.name = "Lv84-鸿沟";
                        mapData.des = "楚汉之界，天下中分。\n出没的怪物：穿天弩手、荡岳妖熊、裂地将军";
                        mapData.addPetKeys("1156", "1157", "1158");
                        return mapData;
                    }
                case "m_79":
                    {
                        mapData.mapeId = "9010";
                        mapData.name = "Lv85-玄武山";
                        mapData.des = "奇岩异石，玄武遗址。\n出没的怪物：擎山兽人、混世散仙、玄幽魔匠";
                        mapData.addPetKeys("1159", "1160", "1161");
                        return mapData;
                    }
                case "m_80":
                    {
                        mapData.mapeId = "9011";
                        mapData.name = "Lv86-青莲湖";
                        mapData.des = "青莲漫布，灾祸潜伏。\n出没的怪物：遁甲卫士、道化枪兵、千幻音蝠、不朽木灵";
                        mapData.addPetKeys("1162", "1163", "1164", "1165");
                        return mapData;
                    }
                case "m_81":
                    {
                        mapData.mapeId = "9012";
                        mapData.name = "Lv87-白虎林";
                        mapData.des = "万兽朝拜，白虎遗址。\n出没的怪物：沧澜兽、震天将军、虎魄将军";
                        mapData.addPetKeys("1166", "1167", "1168");
                        return mapData;
                    }
                case "m_82":
                    {
                        mapData.mapeId = "9013";
                        mapData.name = "Lv88-络合谷";
                        mapData.des = "混沌交汇，天元之地。\n出没的怪物：离火蛟、钩玄统领、开荒兽人";
                        mapData.addPetKeys("1169", "1170", "1171");
                        return mapData;
                    }
                case "m_83":
                    {
                        mapData.mapeId = "9014";
                        mapData.name = "Lv89-七星岩";
                        mapData.des = "奇山并立，势若七星。\n出没的怪物：七绝斧手、陨星妖灵、破岩天蛇";
                        mapData.addPetKeys("1172", "1173", "1174");
                        return mapData;
                    }
                case "m_84":
                    {
                        mapData.mapeId = "10001";
                        mapData.name = "Lv90-十里荒野";
                        mapData.des = "妖魔扫荡，万物虚无。\n出没的怪物：追风魔豹、霸荒战狼";
                        mapData.addPetKeys("1175", "1176");
                        return mapData;
                    }
                case "m_85":
                    {
                        mapData.mapeId = "10002";
                        mapData.name = "Lv90-白骨洞";
                        mapData.des = "枯骨遍地，死气袭人。\n出没的怪物：狂骨血魔、不灭炽灵、枯魂枪客";
                        mapData.addPetKeys("1177", "1178", "1179");
                        return mapData;
                    }
                case "m_86":
                    {
                        mapData.mapeId = "10003";
                        mapData.name = "Lv91-三川";
                        mapData.des = "三川聚首，川流交汇。\n出没的怪物：噬川虫、流星猎手、穿江巨蜥";
                        mapData.addPetKeys("1180", "1181", "1182");
                        return mapData;
                    }
                case "m_87":
                    {
                        mapData.mapeId = "10004";
                        mapData.name = "Lv91-澄湖";
                        mapData.des = "澄明已浊，湖光尽衰。\n出没的怪物：洞天鼠、迷幻影狼、覆海藤";
                        mapData.addPetKeys("1183", "1184", "1185");
                        return mapData;
                    }
                case "m_88":
                    {
                        mapData.mapeId = "10005";
                        mapData.name = "Lv92-咸阳古道";
                        mapData.des = "皇朝的商路，盛极一时。\n出没的怪物：不死秦俑、妖焰虫、古皇兽";
                        mapData.addPetKeys("1186", "1187", "1188");
                        return mapData;
                    }
                case "m_89":
                    {
                        mapData.mapeId = "10006";
                        mapData.name = "Lv92-鸿门";
                        mapData.des = "命运错点，转折之地。\n出没的怪物：风原妖狼、天鹰玄兽、惊鸿神鹰";
                        mapData.addPetKeys("1189", "1190", "1191");
                        return mapData;
                    }
                case "m_90":
                    {
                        mapData.mapeId = "10007";
                        mapData.name = "咸阳商业区";
                        mapData.des = "帝皇霸业，始皇故都，本城市进行买卖交易的地区";
                        return mapData;
                    }
                case "m_91":
                    {
                        mapData.mapeId = "10008";
                        mapData.name = "咸阳活动区";
                        mapData.des = "帝皇霸业，始皇故都，本城市负责各种活动的地区";
                        return mapData;
                    }
                case "m_92":
                    {
                        mapData.mapeId = "10009";
                        mapData.name = "咸阳工业区";
                        mapData.des = "帝皇霸业，始皇故都，本城市炼化升级装备的地区";
                        return mapData;
                    }
                case "m_93":
                    {
                        mapData.mapeId = "10010";
                        mapData.name = "咸阳行政区";
                        mapData.des = "帝皇霸业，始皇故都，本城市负责各种活动的地区";
                        return mapData;
                    }
                case "m_94":
                    {
                        mapData.mapeId = "10011";
                        mapData.name = "Lv93-子虚林";
                        mapData.des = "太虚奇境，穿梭古今。\n出没的怪物：太炎巨蜥、破虚兽、震苍兽";
                        mapData.addPetKeys("1192", "1193", "1194");
                        return mapData;
                    }
                case "m_95":
                    {
                        mapData.mapeId = "10012";
                        mapData.name = "Lv94-咸阳城郊";
                        mapData.des = "皇都郊野，杀气凛然。\n出没的怪物：盘龙兽、射日飞骑、斩月铁骑";
                        mapData.addPetKeys("1195", "1196", "1197");
                        return mapData;
                    }
                case "m_96":
                    {
                        mapData.mapeId = "10013";
                        mapData.name = "Lv95-两仪川";
                        mapData.des = "双川并交，势成两仪。\n出没的怪物：尸煞妖王、两仪蛟、太乙散仙";
                        mapData.addPetKeys("1198", "1199", "1200");
                        return mapData;
                    }
                case "m_97":
                    {
                        mapData.mapeId = "10014";
                        mapData.name = "Lv96-阳谷";
                        mapData.des = "真阳之脉，阳息斥谷。\n出没的怪物：真阳火凤、少阳兽、虚阳古木";
                        mapData.addPetKeys("1201", "1202", "1203");
                        return mapData;
                    }
                case "m_98":
                    {
                        mapData.mapeId = "10015";
                        mapData.name = "Lv97-阴山";
                        mapData.des = "太阴之脉，阴元镇山。\n出没的怪物：太阴斧魔、玄阴古兽、化阴魔尸";
                        mapData.addPetKeys("1204", "1205", "1206");
                        return mapData;
                    }
                case "m_99":
                    {
                        mapData.mapeId = "10016";
                        mapData.name = "Lv98-昆仑绝地";
                        mapData.des = "道玄秘境，飘渺神山。\n出没的怪物：天煞统领、地魔统领、乾坤箭俑";
                        mapData.addPetKeys("1207", "1208", "1209");
                        return mapData;
                    }
                case "m_100":
                    {
                        mapData.mapeId = "10017";
                        mapData.name = "Lv99-昆仑";
                        mapData.des = "两界缝隙，万魔源头。\n出没的怪物：道玄赤灵、无双刀客";
                        mapData.addPetKeys("1210", "1211");
                        return mapData;
                    }
                case "m_101":
                    {
                        mapData.mapeId = "10018";
                        mapData.name = "Lv100-昆仑之巅";
                        mapData.des = "俯仰天地，意指乾坤。\n出没的怪物：兽神统领、太虚妖龙、劈天霸王";
                        mapData.addPetKeys("1212", "1213", "1214");
                        return mapData;
                    }
                case "txzf":
                    {
                        mapData.mapeId = "999999";
                        mapData.name = "太虚之峰";
                        return mapData;
                    }
                case "txzl":
                    {
                        mapData.mapeId = "999998";
                        mapData.name = "太虚之林";
                        return mapData;
                    }
                case "txzg":
                    {
                        mapData.mapeId = "999997";
                        mapData.name = "太虚之谷";
                        return mapData;
                    }
                case "cbd":
                    {
                        mapData.mapeId = "999869";
                        mapData.name = "半岛欢乐广场";
                        return mapData;
                    }
                case "wzy":
                    {
                        mapData.mapeId = "999913";
                        mapData.name = "武状元赛场";
                        return mapData;
                    }
                case "hhbk_1":
                    {
                        mapData.mapeId = "999858";
                        mapData.name = "一层-古藤神树";
                        return mapData;
                    }
                case "hhbk_2":
                    {
                        mapData.mapeId = "999859";
                        mapData.name = "二层-生死门入口";
                        return mapData;
                    }
                case "hhbk_2_1":
                    {
                        mapData.mapeId = "999861";
                        mapData.name = "二层-死门";
                        return mapData;
                    }
                case "hhbk_2_2":
                    {
                        mapData.mapeId = "999860";
                        mapData.name = "二层-生门";
                        return mapData;
                    }
                case "hhbk_3":
                    {
                        mapData.mapeId = "999862";
                        mapData.name = "神龙门";
                        return mapData;
                    }
                case "lyzd":
                    {
                        mapData.mapeId = "999827";
                        mapData.name = "炼狱之地";
                        return mapData;
                    }
                case "xlzd":
                    {
                        mapData.mapeId = "999826";
                        mapData.name = "修罗之地";
                        return mapData;
                    }
                case "hdzd":
                    {
                        mapData.mapeId = "999825";
                        mapData.name = "混沌之地";
                        return mapData;
                    }
                case "kgmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "狂攻魔神殿";
                        return mapData;
                    }
                case "tbmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "铁壁魔神殿";
                        return mapData;
                    }
                case "smmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "生命魔神殿";
                        return mapData;
                    }
                case "ssmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "神速魔神殿";
                        return mapData;
                    }
                case "sheshoumsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "射手魔神殿";
                        return mapData;
                    }
                case "fsmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "法术魔神殿";
                        return mapData;
                    }
                case "bnmsd":
                    {
                        mapData.mapeId = "999767";
                        mapData.name = "暴怒魔神殿";
                        return mapData;
                    }
                case "blxt1":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔1";
                        return mapData;
                    }
                case "blxt2":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔2";
                        return mapData;
                    }
                case "blxt3":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔3";
                        return mapData;
                    }
                case "blxt4":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔4";
                        return mapData;
                    }
                case "blxt5":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔5";
                        return mapData;
                    }
                case "blxt6":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔6";
                        return mapData;
                    }
                case "blxt7":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔7";
                        return mapData;
                    }
                case "blxt8":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔8";
                        return mapData;
                    }
                case "blxt9":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔9";
                        return mapData;
                    }
                case "blxt10":
                    {
                        mapData.mapeId = "999875";
                        mapData.name = "百炼玄塔10";
                        return mapData;
                    }
                case "abyss1":
                    {
                        mapData.mapeId = "999947";
                        mapData.name = "天渊1";
                        return mapData;
                    }
                case "abyss2":
                    {
                        mapData.mapeId = "999946";
                        mapData.name = "天渊2";
                        return mapData;
                    }
                case "abyss3":
                    {
                        mapData.mapeId = "999945";
                        mapData.name = "天渊3";
                        return mapData;
                    }
                case "abyss4":
                    {
                        mapData.mapeId = "999944";
                        mapData.name = "天渊4";
                        return mapData;
                    }
                case "abyss5":
                    {
                        mapData.mapeId = "999943";
                        mapData.name = "天渊5";
                        return mapData;
                    }
                case "abyss6":
                    {
                        mapData.mapeId = "999942";
                        mapData.name = "天渊6";
                        return mapData;
                    }
                case "abyss7":
                    {
                        mapData.mapeId = "999941";
                        mapData.name = "天渊7";
                        return mapData;
                    }
                case "abyss8":
                    {
                        mapData.mapeId = "999940";
                        mapData.name = "天渊8";
                        return mapData;
                    }
                case "abyss9":
                    {
                        mapData.mapeId = "999939";
                        mapData.name = "天渊9";
                        return mapData;
                    }
                case "abyss10":
                    {
                        mapData.mapeId = "999938";
                        mapData.name = "天渊10";
                        return mapData;
                    }
                case "abyss11":
                    {
                        mapData.mapeId = "999937";
                        mapData.name = "天渊11";
                        return mapData;
                    }
                case "abyss12":
                    {
                        mapData.mapeId = "999936";
                        mapData.name = "天渊12";
                        return mapData;
                    }
                case "abyss13":
                    {
                        mapData.mapeId = "999935";
                        mapData.name = "天渊13";
                        return mapData;
                    }
                case "abyss14":
                    {
                        mapData.mapeId = "999934";
                        mapData.name = "天渊14";
                        return mapData;
                    }
                case "abyss15":
                    {
                        mapData.mapeId = "999933";
                        mapData.name = "天渊15";
                        return mapData;
                    }
                case "abyss16":
                    {
                        mapData.mapeId = "999932";
                        mapData.name = "天渊16";
                        return mapData;
                    }
                case "abyss17":
                    {
                        mapData.mapeId = "999931";
                        mapData.name = "天渊17";
                        return mapData;
                    }
                case "abyss18":
                    {
                        mapData.mapeId = "999930";
                        mapData.name = "天渊18";
                        return mapData;
                    }
                case "abyss19":
                    {
                        mapData.mapeId = "999929";
                        mapData.name = "天渊19";
                        return mapData;
                    }
                case "abyss20":
                    {
                        mapData.mapeId = "999928";
                        mapData.name = "天渊20";
                        return mapData;
                    }
                case "abyss21":
                    {
                        mapData.mapeId = "999947";
                        mapData.name = "天渊21";
                        return mapData;
                    }
                case "abyss22":
                    {
                        mapData.mapeId = "999946";
                        mapData.name = "天渊22";
                        return mapData;
                    }
                case "abyss23":
                    {
                        mapData.mapeId = "999945";
                        mapData.name = "天渊23";
                        return mapData;
                    }
                case "abyss24":
                    {
                        mapData.mapeId = "999944";
                        mapData.name = "天渊24";
                        return mapData;
                    }
                case "abyss25":
                    {
                        mapData.mapeId = "999943";
                        mapData.name = "天渊25";
                        return mapData;
                    }
                case "abyss26":
                    {
                        mapData.mapeId = "999942";
                        mapData.name = "天渊26";
                        return mapData;
                    }
                case "abyss27":
                    {
                        mapData.mapeId = "999941";
                        mapData.name = "天渊27";
                        return mapData;
                    }
                case "abyss28":
                    {
                        mapData.mapeId = "999940";
                        mapData.name = "天渊28";
                        return mapData;
                    }
                case "abyss29":
                    {
                        mapData.mapeId = "999939";
                        mapData.name = "天渊29";
                        return mapData;
                    }
                case "abyss30":
                    {
                        mapData.mapeId = "999938";
                        mapData.name = "天渊30";
                        return mapData;
                    }
                case "abyss31":
                    {
                        mapData.mapeId = "999937";
                        mapData.name = "天渊31";
                        return mapData;
                    }
                case "abyss32":
                    {
                        mapData.mapeId = "999936";
                        mapData.name = "天渊32";
                        return mapData;
                    }
                case "abyss33":
                    {
                        mapData.mapeId = "999935";
                        mapData.name = "天渊33";
                        return mapData;
                    }
                case "abyss34":
                    {
                        mapData.mapeId = "999934";
                        mapData.name = "天渊34";
                        return mapData;
                    }
                case "abyss35":
                    {
                        mapData.mapeId = "999933";
                        mapData.name = "天渊35";
                        return mapData;
                    }
                case "abyss36":
                    {
                        mapData.mapeId = "999932";
                        mapData.name = "天渊36";
                        return mapData;
                    }
                case "abyss37":
                    {
                        mapData.mapeId = "999931";
                        mapData.name = "天渊37";
                        return mapData;
                    }
                case "abyss38":
                    {
                        mapData.mapeId = "999930";
                        mapData.name = "天渊38";
                        return mapData;
                    }
                case "abyss39":
                    {
                        mapData.mapeId = "999929";
                        mapData.name = "天渊39";
                        return mapData;
                    }
                case "abyss40":
                    {
                        mapData.mapeId = "999928";
                        mapData.name = "天渊40";
                        return mapData;
                    }
                case "cwly":
                    {
                        mapData.mapeId = "999851";
                        mapData.name = "宠物乐园";
                        return mapData;
                    }
                case "prison":
                    {
                        mapData.mapeId = "999924";
                        mapData.name = "监狱";
                        return mapData;
                    }
                case "qlhd1":
                    {
                        mapData.mapeId = "999950";
                        mapData.name = "普通黑洞";
                        return mapData;
                    }
                case "qlhd2":
                    {
                        mapData.mapeId = "999949";
                        mapData.name = "英雄黑洞";
                        return mapData;
                    }
                case "qlhd3":
                    {
                        mapData.mapeId = "999948";
                        mapData.name = "地狱黑洞";
                        return mapData;
                    }
                case "qlhd4":
                    {
                        mapData.mapeId = "999828";
                        mapData.name = "炼狱黑洞";
                        return mapData;
                    }
                case "fb50_1":
                    {
                        mapData.mapeId = "999979";
                        mapData.name = "古城外围";
                        return mapData;
                    }
                case "fb50_2":
                    {
                        mapData.mapeId = "999980";
                        mapData.name = "紫阴废墟";
                        return mapData;
                    }
                case "fb50_3":
                    {
                        mapData.mapeId = "999981";
                        mapData.name = "怨魂之地";
                        return mapData;
                    }
                case "fb50_4":
                    {
                        mapData.mapeId = "999982";
                        mapData.name = "紫阴中枢";
                        return mapData;
                    }
                case "fb60_1":
                    {
                        mapData.mapeId = "999990";
                        mapData.name = "葬刀绝地";
                        return mapData;
                    }
                case "fb60_2":
                    {
                        mapData.mapeId = "999987";
                        mapData.name = "落矢绝地";
                        return mapData;
                    }
                case "fb60_3":
                    {
                        mapData.mapeId = "999986";
                        mapData.name = "陷阵绝地";
                        return mapData;
                    }
                case "fb60_4":
                    {
                        mapData.mapeId = "999985";
                        mapData.name = "万魂核心";
                        return mapData;
                    }
                case "fb70_1":
                    {
                        mapData.mapeId = "999992";
                        mapData.name = "猛兽之林";
                        return mapData;
                    }
                case "fb70_2":
                    {
                        mapData.mapeId = "999993";
                        mapData.name = "清风幽径";
                        return mapData;
                    }
                case "fb70_3":
                    {
                        mapData.mapeId = "999995";
                        mapData.name = "兽王古道";
                        return mapData;
                    }
                case "fb70_4":
                    {
                        mapData.mapeId = "999996";
                        mapData.name = "太阴奇界";
                        return mapData;
                    }
                case "fb80_1":
                    {
                        mapData.mapeId = "999978";
                        mapData.name = "前生古道";
                        return mapData;
                    }
                case "fb80_2":
                    {
                        mapData.mapeId = "999977";
                        mapData.name = "今生古道";
                        return mapData;
                    }
                case "fb80_3":
                    {
                        mapData.mapeId = "999976";
                        mapData.name = "来生古道";
                        return mapData;
                    }
                case "fb80_4":
                    {
                        mapData.mapeId = "999974";
                        mapData.name = "轮回古道";
                        return mapData;
                    }
                case "fb90_1":
                    {
                        mapData.mapeId = "999972";
                        mapData.name = "青丘入口";
                        return mapData;
                    }
                case "fb90_2":
                    {
                        mapData.mapeId = "999971";
                        mapData.name = "灵隐绝境";
                        return mapData;
                    }
                case "fb90_3":
                    {
                        mapData.mapeId = "999970";
                        mapData.name = "禁忌古道";
                        return mapData;
                    }
                case "fb90_4":
                    {
                        mapData.mapeId = "999969";
                        mapData.name = "迷影禁地";
                        return mapData;
                    }
                case "fb100_1":
                    {
                        mapData.mapeId = "999966";
                        mapData.name = "惊凡渊";
                        return mapData;
                    }
                case "fb100_2":
                    {
                        mapData.mapeId = "999965";
                        mapData.name = "裂影渊";
                        return mapData;
                    }
                case "fb100_3":
                    {
                        mapData.mapeId = "999964";
                        mapData.name = "泣魔渊";
                        return mapData;
                    }
                case "fb100_4":
                    {
                        mapData.mapeId = "999963";
                        mapData.name = "陨仙渊";
                        return mapData;
                    }
                case "fbls_1":
                    {
                        mapData.mapeId = "999774";
                        mapData.name = "乱葬废墟";
                        return mapData;
                    }
                case "fbls_2":
                    {
                        mapData.mapeId = "999775";
                        mapData.name = "虐杀之地";
                        return mapData;
                    }
                case "fbls_3":
                    {
                        mapData.mapeId = "999776";
                        mapData.name = "枯魂阴牢";
                        return mapData;
                    }
                case "fbls_4":
                    {
                        mapData.mapeId = "999777";
                        mapData.name = "赤炼血池";
                        return mapData;
                    }
                case "fbhh_1":
                    {
                        mapData.mapeId = "999814";
                        mapData.name = "虚幻之地";
                        return mapData;
                    }
                case "fbhh_2":
                    {
                        mapData.mapeId = "999815";
                        mapData.name = "渴望之地";
                        return mapData;
                    }
                case "fbhh_3":
                    {
                        mapData.mapeId = "999816";
                        mapData.name = "回忆之地";
                        return mapData;
                    }
                case "fbhh_4":
                    {
                        mapData.mapeId = "999817";
                        mapData.name = "痛苦之地";
                        return mapData;
                    }
                case "fbys_1":
                    {
                        mapData.mapeId = "999804";
                        mapData.name = "焚火废墟";
                        return mapData;
                    }
                case "fbys_2":
                    {
                        mapData.mapeId = "999805";
                        mapData.name = "烈焰野地";
                        return mapData;
                    }
                case "fbys_3":
                    {
                        mapData.mapeId = "999806";
                        mapData.name = "龙隐秘地";
                        return mapData;
                    }
                case "fbys_4":
                    {
                        mapData.mapeId = "999807";
                        mapData.name = "龙啸古地";
                        return mapData;
                    }
                case "fbzx_1":
                    {
                        mapData.mapeId = "999794";
                        mapData.name = "焚谷熔岩";
                        return mapData;
                    }
                case "fbzx_2":
                    {
                        mapData.mapeId = "999797";
                        mapData.name = "隐之境";
                        return mapData;
                    }
                case "fbzx_3":
                    {
                        mapData.mapeId = "999795";
                        mapData.name = "炼魂祭坛";
                        return mapData;
                    }
                case "fbzx_4":
                    {
                        mapData.mapeId = "999796";
                        mapData.name = "炼魂祭坛";
                        return mapData;
                    }
                    //隐藏本 云之境（由100英雄本图二心魔处进入）
                case "fb_yzj_1":
                    {
                        mapData.mapeId = "999784";
                        mapData.name = "云之境";
                        return mapData;
                    }
                case "fb_yzj_2":
                    {
                        mapData.mapeId = "999786";
                        mapData.name = "幻之境";
                        return mapData;
                    }
                case "fb_yzj_3":
                    {
                        mapData.mapeId = "999787";
                        mapData.name = "仙之境";
                        return mapData;
                    }
                case "hunyin":
                    {
                        mapData.mapeId = "999718";
                        mapData.name = "婚姻大殿";
                        return mapData;
                    }
                case "xxzd":
                    {
                        mapData.mapeId = "999914";
                        mapData.name = "血腥之地";
                        mapData.addPetKeys("xxzd_1", "xxzd_2");
                        return mapData;
                    }
                case "dmkj1":
                    {
                        mapData.mapeId = "999914";
                        mapData.name = "Lv70-凶兽梦境";
                        mapData.addPetKeys("dmkj_1");
                        return mapData;
                    }
                case "dmkj1_1":
                    {
                        mapData.mapeId = "999914";
                        mapData.name = "Lv70-青木梦境";
                        mapData.addPetKeys("dmkj_1");
                        return mapData;
                    }
                case "dmkj2":
                    {
                        mapData.mapeId = "999915";
                        mapData.name = "Lv75-恶鬼梦境";
                        mapData.addPetKeys("dmkj_2");
                        return mapData;
                    }
                case "dmkj2_1":
                    {
                        mapData.mapeId = "999915";
                        mapData.name = "Lv75-白金梦境";
                        mapData.addPetKeys("dmkj_2");
                        return mapData;
                    }
                case "dmkj3":
                    {
                        mapData.mapeId = "999916";
                        mapData.name = "Lv80-修罗梦境";
                        mapData.addPetKeys("dmkj_3");
                        return mapData;
                    }
                case "dmkj3_1":
                    {
                        mapData.mapeId = "999916";
                        mapData.name = "Lv80-赤火梦境";
                        mapData.addPetKeys("dmkj_3");
                        return mapData;
                    }
                case "dmkj4":
                    {
                        mapData.mapeId = "999917";
                        mapData.name = "Lv85-噬人梦境";
                        mapData.addPetKeys("dmkj_4");
                        return mapData;
                    }
                case "dmkj4_1":
                    {
                        mapData.mapeId = "999917";
                        mapData.name = "Lv85-冥水梦境";
                        mapData.addPetKeys("dmkj_4");
                        return mapData;
                    }
                case "dmkj5":
                    {
                        mapData.mapeId = "999918";
                        mapData.name = "Lv90-腐地梦境";
                        mapData.addPetKeys("dmkj_5");
                        return mapData;
                    }
                case "dmkj5_1":
                    {
                        mapData.mapeId = "999918";
                        mapData.name = "Lv90-玄阴梦境";
                        mapData.addPetKeys("dmkj_5");
                        return mapData;
                    }
                case "dmkj6":
                    {
                        mapData.mapeId = "999919";
                        mapData.name = "Lv95-焚天梦境";
                        mapData.addPetKeys("dmkj_6");
                        return mapData;
                    }
                case "dmkj6_1":
                    {
                        mapData.mapeId = "999919";
                        mapData.name = "Lv95-烈阳梦境";
                        mapData.addPetKeys("dmkj_6");
                        return mapData;
                    }
                case "dmkj7":
                    {
                        mapData.mapeId = "999904";
                        mapData.name = "Lv100-十绝梦境";
                        mapData.addPetKeys("dmkj_7");
                        return mapData;
                    }
                case "dmkj7_1":
                    {
                        mapData.mapeId = "999904";
                        mapData.name = "Lv100-逆天梦境";
                        mapData.addPetKeys("dmkj_7");
                        return mapData;
                    }
                case "kongmiao1":
                    {
                        mapData.mapeId = "999895";
                        mapData.name = "孔庙庙门";
                        return mapData;
                    }
                case "kongmiao2":
                    {
                        mapData.mapeId = "999896";
                        mapData.name = "孔庙过道";
                        return mapData;
                    }
                case "kongmiao3":
                    {
                        mapData.mapeId = "999897";
                        mapData.name = "孔庙长廊";
                        return mapData;
                    }
                case "kongmiao4":
                    {
                        mapData.mapeId = "999898";
                        mapData.name = "孔庙阶梯";
                        return mapData;
                    }
                case "kongmiao5":
                    {
                        mapData.mapeId = "999899";
                        mapData.name = "孔庙殿外";
                        return mapData;
                    }
                case "kongmiao6":
                    {
                        mapData.mapeId = "999900";
                        mapData.name = "孔庙主殿";
                        return mapData;
                    }
                case "gangs1":
                    {
                        mapData.mapeId = "999903";
                        mapData.name = "一级帮派";
                        return mapData;
                    }
                case "gangs2":
                    {
                        mapData.mapeId = "999902";
                        mapData.name = "二级帮派";
                        return mapData;
                    }
                case "gangs3":
                    {
                        mapData.mapeId = "999901";
                        mapData.name = "三级帮派";
                        return mapData;
                    }
                case "gangs4":
                    {
                        mapData.mapeId = "999868";
                        mapData.name = "四级帮派";
                        return mapData;
                    }
                case "gangs5":
                    {
                        mapData.mapeId = "999867";
                        mapData.name = "五级帮派";
                        return mapData;
                    }
                case "gangs6":
                    {
                        mapData.mapeId = "999866";
                        mapData.name = "六级帮派";
                        return mapData;
                    }
                case "bz":
                    {
                        mapData.mapeId = "999865";
                        mapData.name = "帮战";
                        return mapData;
                    }
                case "tianshoufeng":
                    {
                        mapData.mapeId = "999921";
                        mapData.name = "天守峰";
                        mapData.addPetKeys("mpbwz_1");
                        return mapData;
                    }
                case "mizonglin":
                    {
                        mapData.mapeId = "999920";
                        mapData.name = "迷踪林";
                        mapData.addPetKeys("mpbwz_2");
                        return mapData;
                    }
                case "taixugu":
                    {
                        mapData.mapeId = "999922";
                        mapData.name = "太虚谷";
                        mapData.addPetKeys("mpbwz_3");
                        return mapData;
                    }
                case "yzj_1":
                    {
                        mapData.mapeId = "999888";
                        mapData.name = "天元";
                        mapData.addPetKeys("yzj_1");
                        return mapData;
                    }
                case "yzj_2":
                    {
                        mapData.mapeId = "999885";
                        mapData.name = "南离城郊";
                        mapData.addPetKeys("yzj_2");
                        return mapData;
                    }
                case "yzj_3":
                    {
                        mapData.mapeId = "999891";
                        mapData.name = "南离城";
                        return mapData;
                    }
                case "yzj_4":
                    {
                        mapData.mapeId = "999889";
                        mapData.name = "北冥城郊";
                        mapData.addPetKeys("yzj_2");
                        return mapData;
                    }
                case "yzj_5":
                    {
                        mapData.mapeId = "999890";
                        mapData.name = "北冥城";
                        return mapData;
                    }
                case "yzj_6":
                    {
                        mapData.mapeId = "999887";
                        mapData.name = "西沙城郊";
                        mapData.addPetKeys("yzj_1");
                        return mapData;
                    }
                case "yzj_7":
                    {
                        mapData.mapeId = "999893";
                        mapData.name = "西沙城";
                        return mapData;
                    }
                case "yzj_8":
                    {
                        mapData.mapeId = "999886";
                        mapData.name = "东云城郊";
                        mapData.addPetKeys("yzj_1");
                        return mapData;
                    }
                case "yzj_9":
                    {
                        mapData.mapeId = "999892";
                        mapData.name = "东云城";
                        return mapData;
                    }
                case "dsx":
                    {
                        mapData.mapeId = "999852";
                        mapData.name = "大师兄赛场";
                        return mapData;
                    }






                default:
                    {
                        mapData.mapeId = "1001";
                        mapData.name = "恒山村";
                        return mapData;
                    }
            }

        }
    }
}
