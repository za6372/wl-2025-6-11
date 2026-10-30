using Assets.HotFix.MyUtils.src.common;
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
    public interface monsterInterface
    {
        public JObject getMonster(string key);
    }
    class monsterInterfaceImpl : monsterInterface
    {
        /** 
         * TODO:改成跟服务器的monsterData一致
         * 任务怪物收集器需要显示名字
         * 任务面板怪物目标名字需要显示
         * 
         */
        public JObject getMonster(string key)
        {
            string name = null;

            if (key.Contains("abyss_"))
            {
                string[] arr = {
                                "龙血玄蛛", "赤灵妖狼", "霸灵巨兽", "食魂梦", "烈焰燃火虫",
                                "洞穴巨猿", "苍灵妖狼", "飞羽战鹰", "战魄玄兽", "赤焰铁甲兽",
                                "龙渊盘蛇", "吸血妖鼠", "喋血妖蝠", "夺魂魔兽", "长爪越兽",
                                "鹰头兽人", "护龙灵兽", "震空巨熊", "浴火凤凰", "破虚苍龙",

                                "六尾魔狐", "双头魔犬", "噬魂花妖", "残天凶兽", "狂尸蝎怪",
                                "裂魂鬼灵", "凶煞妖蛟", "魔焰煞魂", "上古灾兽", "吞日海魂",
                                "噬魂尸虫", "魅蛛魔女", "赤焰甲兽", "啸天巨犼", "金角大王",
                                "覆海鲛人", "幻境鹿妖", "齐天大圣", "万年树妖", "赤焰金龙"
                            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("qlhd_"))
            {
                string[] arr = {
                                "赤岩魔狼", "寒冰魔狼", "震天利爪兽","灵魂舔舐者"
                            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("petxl_"))
            {
                string[] arr = {
                    "牛头怪", "夜刃豹", "魅惑蛇妖", "地狱犬", "魔狼","狂暴巨熊", "赤炼魔蜥", "追魂猎鹰", "剧毒魔蛛", "千年灵猿",
                    "暗影邪神", "幽魂", "摄魂天狐", "碧眼苍狼", "上古魔将", "魔兵", "斩首者", "暗影猎杀者", "轮回恶灵", "怨灵聚体",
                    "真·暗影邪神", "真·幽魂", "真·摄魂天狐", "真·碧眼苍狼", "真·上古魔将", "真·魔兵", "真·斩首者", "真·暗影猎杀者", "真·轮回恶灵", "真·怨灵聚体",
                    "乱世逆仙", "幻化魔影", "白虎分身", "玄武分身", "朱雀分身", "金雕", "青龙分身", "玄蛇", "灭世妖皇", "冥界邪灵",
                    "真·乱世逆仙", "真·幻化魔影", "真·白虎分身", "真·玄武分身", "真·朱雀分身", "真·金雕", "真·青龙分身", "真·玄蛇", "真·灭世妖皇", "真·冥界邪灵",
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("blxt_"))
            {
                string[] arr = {
                                "离火剑", "散瘟鞭", "落宝金钱", "列瘟印", "紫金铃","撞心杵", "风火轮", "酱油瓶", "乾坤针", "斩仙飞刀",

                                "风袋", "梅花镖", "六根清净竹", "雾露乾坤网", "戳目珠", "照妖鉴", "长生根", "伤不旗", "逆鳞枪", "宝莲灯",

                                "万里起云烟", "钻心钉", "万鸦壶", "紫金钵", "定风珠", "魔神甲", "穿天弩", "咆哮梯", "金光锉", "五行旗",

                                "乱心尘", "阴阳二气瓶", "劈地珠", "杏黄旗", "阴阳刃", "穿心锁", "三尖两刃枪", "浮云", "金霞冠", "伏羲琴",

                                "落魂钟", "焰光旗", "化血神刀", "日月珠", "定海珠", "破军", "开天珠", "混元锤", "火星帖", "翻天印",

                                "四象塔", "天荡", "降魔杵", "乾坤圈", "如意乾坤袋", "捆仙绳", "招妖幡", "杯具", "水火锋", "苍刑逆天枪",

                                "黑砂", "混元幡", "乾坤弓", "落魄镜", "听谛印", "照天印", "遁龙桩", "鸭梨", "缚龙索", "混沌钟",
                            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("mjxw_"))
            {
                string[] arr = {
                    "尸妖王", "剑魂", "凶灵"
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("mprw_"))//门派任务
            {
                string[] arr = {
                    "风笑天", "唐镇", "李立","震天战魂","阿布"
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("bprw_"))//帮派任务
            {
                string[] arr = {
                    "寒风破", "冰啼", "千户花","江陵幕","凯玄"
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("shitu_"))
            {
                string[] arr = {
                    "叽叽", "嘎嘎", "咔咔", "叭叭", "哈哈", "哦哦", "嚯嚯", "呵呵"
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("smbz_"))
            {
                string[] arr = {
                     "花生舞", "伴生灵", "邪童子"
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("smbz_"))
            {
                string[] arr = {
                    "饕餮", "梼杌", "混沌", "穷奇"
            };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index];
            }
            else if (key.Contains("ztzs_"))
            {
                string[] arr = {
                     "玄甲神卫", "落音仙灵", "幽冥剑灵", "破天剑客", "玉弦仙灵", "玄冥紫魂", "震天战神","傀儡"
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("tianbing"))
            {
                name = "天兵";
            }
            else if (key.Contains("tupo_"))
            {
                name = "心魔";
            }
            else if (key.Contains("jianxi_"))
            {
                name = "奸细";
            }


            else if (key.Contains("boss_"))
            {
                name = bossMonster(key);
            }
            else if (key.Contains("fb_50_"))
            {
                string[] arr = {
                     "噬人妖", "噬魂魔将", "魔帅","游离鬼魂", "暴怒尸鬼", "太古真魔", "太古真魔分身"
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_60_"))
            {
                string[] arr = {
                     "亡灵刀兵", "亡灵戟兵","亡灵裨将","亡灵弓手","亡灵斧兵","亡灵统帅","亡灵副将",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_70_"))
            {
                string[] arr = {
                     "巨山熊", "守谷老人","隐月宗二弟子","猛山虎","隐月宗大弟子","倩倩","月尘化身",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_80_"))
            {
                string[] arr = {
                     "前世之灵","今生之魂","来世之魄","三生兽","轮回","前世","今生","来世"
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_90_"))
            {
                string[] arr = {
                     "青丘之灵","打手","瑞南羽","九尾幻影","六尾","九尾异兽",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_100_"))
            {
                string[] arr = {
                     "恶灵","洞渊战魂","洞渊战魔","魔影","百鬼之王",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_ls_"))
            {
                string[] arr = {
                     "煞阴尸鬼","虐杀之鬼","幽蓝狼魔","暗影杀手","怨杀魔狼",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_hh_"))
            {
                string[] arr = {
                     "巨角魔牛","震岳巨熊","灭城将军","心魔","碎魂阴尸",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_ys_"))
            {
                string[] arr = {
                     "黑煞木妖","苍岚狮鹫","龙女","君皇","通天眼","影行护卫",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_zx_"))
            {
                string[] arr = {
                     "冥罗妖","熔骨尸煞","蜃兽","魔化天星子","天鹰护卫","白虎护卫",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("fb_yzj_"))
            {
                string[] arr = {
                     "灵通仙兽","饕餮","青玄仙人","梼杌","陆吾","夔牛","沧海仙龙","天机老人","道家门主","墨家门主","阴阳家门主",
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index - 1];
            }
            else if (key.Contains("mshuwei_"))
            {
                string[] arr = {
                     "狂攻之护卫","铁壁之护卫","生命之护卫","神速之护卫","射手之护卫","法术之护卫","暴怒之护卫","喽啰",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("yhmkms_"))
            {
                string[] arr = {
                     "狂攻魔神","铁壁魔神","生命魔神","神速魔神","射手魔神","法术魔神","暴怒魔神",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("xhxy_"))
            {
                string[] arr = {
                     "吸魂小妖","小鬼",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("hhbk_"))
            {
                string[] arr = {
                     "机关恶兽", "迷路恶兽", "神龙后裔",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("dmkj_"))
            {
                string[] arr = {
                     "破天巨熊", "隐梦云豹", "噬梦虫","化梦鬼木","入梦双蛇","赤痕梦蛛","吞梦妖龙"
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("xxzd_"))
            {
                string[] arr = {
                     "旺财", "小强",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("cyby_"))
            {
                string[] arr = {
                     "狐萌萌",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("jyfy_"))
            {
                string[] arr = {
                     "太二真人", "西门好色", "鲁光光","东方必败","完颜失色",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("yzj_"))//云中界（跑商）
            {
                string[] arr = {
                     "玄铁石精", "炼神木灵",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (key.Contains("bz_box_"))//帮战箱子守卫
            {
                string[] arr = {
                     "守卫1", "守卫2","守卫3","守卫4"
                };
                int index = int.Parse(key.Split('_')[2]);
                name = arr[index];
            }
            else if (key.Contains("mpbwz_"))//门派保卫战
            {
                string[] arr = {
                     "鸡毛兽人", "长尾兽人", "短腿兽人",
                };
                int index = int.Parse(key.Split('_')[1]);
                name = arr[index - 1];
            }
            else if (strUtils.isMatch(key, "1([0-9]{3})"))
            {
                Pet pet = face.petInterface.getPetDataByKey(key);
                name = pet.name.ToString();
            }
            else
            {
                switch (key)
                {
                    case "mozun_boss":
                        {
                            name = "魔尊";
                            break;
                        }
                    case "world_boss":
                        {
                            name = "灭世魔龙";
                            break;
                        }
                    case "houzi":
                        {
                            name = "顽皮的猴子";
                            break;
                        }
                    case "wzyc":
                        {
                            name = "藏边小丑";
                            break;
                        }
                }
            }

            JObject monster = new JObject();
            monster.Add("key", key);
            monster.Add("name", name);
            return monster;
        }
        private string bossMonster(string key)
        {
            string name = null;

            switch (key)
            {
                case "boss_0":
                    {
                        name = "展护卫";
                        break;
                    }
                case "boss_1":
                    {
                        name = "心魔";
                        break;
                    }
                case "boss_1_0":
                    {
                        name = "贪婪";
                        break;
                    }
                case "boss_1_1":
                    {
                        name = "嫉妒";
                        break;
                    }
                case "boss_1_2":
                    {
                        name = "懒惰";
                        break;
                    }
                case "boss_1_3":
                    {
                        name = "暴食";
                        break;
                    }
                case "boss_1_4":
                    {
                        name = "色欲";
                        break;
                    }
                case "boss_2":
                    {
                        name = "神秘强者";
                        break;
                    }
                case "boss_3":
                    {
                        name = "邪月";
                        break;
                    }
                case "boss_4":
                    {
                        name = "韩信";
                        break;
                    }
                case "boss_5":
                    {
                        name = "暗影魔将";
                        break;
                    }
                case "boss_6":
                    {
                        name = "梦渊尸魔";
                        break;
                    }
                case "boss_7":
                    {
                        name = "天星子";
                        break;
                    }
                case "boss_7_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_8":
                    {
                        name = "尸王";
                        break;
                    }
                case "boss_9":
                    {
                        name = "猴王";
                        break;
                    }
                case "boss_10":
                    {
                        name = "狂暴兽王";
                        break;
                    }
                case "boss_11":
                    {
                        name = "炎狼";
                        break;
                    }
                case "boss_12":
                    {
                        name = "炎狼";
                        break;
                    }
                case "boss_12_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_13":
                    {
                        name = "手下";
                        break;
                    }
                case "boss_14":
                    {
                        name = "刘大刀";
                        break;
                    }
                case "boss_14_0":
                    {
                        name = "手下";
                        break;
                    }
                case "boss_15":
                    {
                        name = "天星子";
                        break;
                    }
                case "boss_15_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_16":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_17":
                    {
                        name = "啸月";
                        break;
                    }
                case "boss_18":
                    {
                        name = "牛头怪";
                        break;
                    }
                case "boss_19":
                    {
                        name = "修罗分身";
                        break;
                    }
                case "boss_20":
                    {
                        name = "心魔";
                        break;
                    }
                case "boss_21":
                    {
                        name = "村民";
                        break;
                    }
                case "boss_21_0":
                    {
                        name = "村民";
                        break;
                    }
                case "boss_22":
                    {
                        name = "修罗之神";
                        break;
                    }

                case "boss_23":
                    {
                        name = "狗腿子";
                        break;
                    }
                case "boss_23_0":
                    {
                        name = "狗腿子";
                        break;
                    }
                case "boss_24":
                    {
                        name = "狗王";
                        break;
                    }
                case "boss_25":
                    {
                        name = "湛蓝领主";
                        break;
                    }
                case "boss_26":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_26_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_27":
                    {
                        name = "水晶守卫";
                        break;
                    }
                case "boss_28":
                    {
                        name = "钥匙守卫";
                        break;
                    }

                case "boss_29":
                    {
                        name = "蚩尤";
                        break;
                    }
                case "boss_29_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_30":
                    {
                        name = "偷听者";
                        break;
                    }
                case "boss_31":
                    {
                        name = "蚩尤";
                        break;
                    }
                case "boss_31_0":
                    {
                        name = "喽啰";
                        break;
                    }
                case "boss_32":
                    {
                        name = "妖兽";
                        break;
                    }
                case "boss_33":
                    {
                        name = "龙啸天";
                        break;
                    }
                case "boss_34":
                    {
                        name = "子尔邑";
                        break;
                    }
                case "boss_35":
                    {
                        name = "龙姣王";
                        break;
                    }
                case "boss_36":
                    {
                        name = "子尔邑";
                        break;
                    }
                case "boss_36_0":
                    {
                        name = "分身";
                        break;
                    }
                case "boss_37":
                    {
                        name = "子尔邑";
                        break;
                    }
                case "boss_38":
                    {
                        name = "子尔邑";
                        break;
                    }
                case "boss_39":
                    {
                        name = "子尔邑";
                        break;
                    }
                case "boss_40":
                    {
                        name = "范小弟";
                        break;
                    }



                default:
                    {
                        Debug.Log("未找到boss怪物" + key);
                        return null;
                    }
            }
            return name;
        }
    }
}
