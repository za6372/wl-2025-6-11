using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.common
{

    /**游戏的一些常量*/
    public class GameAttrConst
    {
        //自动遇怪
        public static bool isAutoYG;
        //自动遇怪次数
        public static int autoYgTimes;
        //地图的缩放比率
        //public static float MapScaleRate = 1;
        
        public static float getMapScaleRate()
        {
            //一张图是 352-416

            //以宽比率来获取地图放大后的高度,原高度对应缩放是128
            float dh = ScreenUtils.height - 300;
            //要求为功能机的比例 原地图大小22*16，现在通过增加像素整体放大两倍，故*2
            float rate = ScreenUtils.width / 240f;
            //Debug.Log(rate);
            //屏幕1.5倍宽度下的高度
            float width = 352 * rate;
            float height = 416 * rate;
            //如果高度还是不够，则将高度放大到地图区最大高度，宽度按比率放大
            if (height < dh)
            {
                float oldH = height;
                height = dh;
                //获取高的缩放系数
                float k = height / oldH;
                width = width * k;
                //Debug.Log(width + "/" + height);
                rate = rate * k;
            }
            return rate;
        }
        /**根据身份返回名字颜色*/
        public static string getColorBySf(int sez)
        {
            string sf = "#ffffff";
            if (sez < 0) sf = "#FF0000";
            else if (sez < 3) sf = "#FFB500";
            else if (sez > 10) sf = "#68D238";
            else if (sez > 7) sf = "#B4FF00";
            return sf;
        }
        /**获取身份*/
        public static string getSf(int sez)
        {
            string sf = "平民";
            if (sez < 0) sf = "魔头";
            else if (sez < 3) sf = "恶人";
            else if (sez > 10) sf = "英雄";
            else if (sez > 7) sf = "侠士";
            return sf;
        }
        public static bool isMenPaiSklKey(string sklKey)
        {
            if (!sklKey.Equals("100210000040") && !sklKey.Equals("100210000041") &&
                !sklKey.Equals("100210000042") && !sklKey.Equals("100210000043") &&
                !sklKey.Equals("100210000044") && !sklKey.Equals("100210000045")) return false;
            return true;
        }
        public static String mpSklToJobSkl(String job, String sklKey)
        {
            if (job.Contains("ms"))
            {
                if (sklKey.Equals("100210000040")) return "100210000002";
                else if (sklKey.Equals("100210000041")) return "100210000003";
            }
            else if (job.Contains("dj"))
            {
                if (sklKey.Equals("100210000040")) return "100210000008";
                else if (sklKey.Equals("100210000041")) return "100210000009";
            }
            else if (job.Contains("qm"))
            {
                if (sklKey.Equals("100210000042")) return "100210000015";
                else if (sklKey.Equals("100210000043")) return "100210000016";
            }
            else if (job.Contains("ty"))
            {
                if (sklKey.Equals("100210000042")) return "100210000021";
                else if (sklKey.Equals("100210000043")) return "100210000022";
            }
            else if (job.Contains("ym"))
            {
                if (sklKey.Equals("100210000044")) return "100210000029";
                else if (sklKey.Equals("100210000045")) return "100210000030";
            }
            else if (job.Contains("lc"))
            {
                if (sklKey.Equals("100210000044")) return "100210000035";
                else if (sklKey.Equals("100210000045")) return "100210000036";
            }
            return sklKey;
        }
        /**将6个初始的门派技能转职业技能的key*/
        public static string mpSklToJobSkl(JObject role, string sklKey)
        {
            if (!isMenPaiSklKey(sklKey)) return sklKey;
            string job = GameAttrConst.getJobFromModels(role);
            if (job != null)//说明加入了门派，就要转化
            {
                sklKey = mpSklToJobSkl(job, sklKey);
            }
            return sklKey;
        }
        /**获取性别*/
        public static string getSexFromModels(JObject role)
        {
            if (!role.ContainsKey("models")) return null;
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            for (int i = 0; i < models.Count(); i++)
            {
                string job = models[i].ToString();
                if (job.Contains("_nan")) return "nan";
                else if (job.Contains("_nv")) return "nv";
            }
            return null;
        }
        /**
         * 获取包含门派的model
         */
        public static string getMenPaiFromModels(JObject role)
        {
            if (!role.ContainsKey("models")) return null;
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            for (int i = 0; i < models.Count(); i++)
            {
                string job = models[i].ToString();
                if (job.Contains("zs") || job.Contains("fs") ||
                        job.Contains("fz"))
                {
                    return job;
                }
            }
            return null;//为null就说明是初始形象，还未转职
        }
        /**
         * 获取包含职业的model
         */
        public static string getJobFromModels(JObject role)
        {
            if (!role.ContainsKey("models")) return null;
            //因为字段model可从models中选取，而初始形象不包含职业字段（ms/qm/ym...）
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            for (int i = 0; i < models.Count(); i++)
            {
                string job = models[i].ToString();
                if (job.Contains("ms") || job.Contains("dj") ||
                        job.Contains("qm") || job.Contains("ty") ||
                        job.Contains("ym") || job.Contains("lc"))
                {
                    return job;
                }
            }
            return null;//为null就说明是初始形象，还未转职
        }
        public static string getJobFirst(JObject role)
        {
            if (!role.ContainsKey("models")) return null;
            //因为字段model可从models中选取，而初始形象不包含职业字段（ms/qm/ym...）
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            return models[0].ToString().Substring(0, 2);
        }
        /**从数组中选出最终的model*/
        public static string getModel(JObject role)
        {
            //先选职业
            string md = getJobFromModels(role);
            if (md == null)
            {
                //不存在则选门派
                md = getMenPaiFromModels(role);
            }
            if (!role.ContainsKey("model"))
            {
                return getJobFirst(role);
            }
            //还是没有则返回原model
            if (md == null) md = role["model"].ToString();

            return md;
        }
        /**获取角色头像*/
        public static string getRoleHead(JObject role)
        {
            JArray models = strUtils.strToJSONObj<JArray>(role["models"].ToString());
            string model = null;
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i].ToString().Contains("xs_"))
                {
                    model = models[i].ToString();
                    break;
                }
            }
            return getRoleHead(model);
        }

        public static string getRoleHead(string model)
        {
            if (model.Contains("xs_nan_t1")) return "001";
            else if (model.Contains("xs_nan_t2")) return "011";
            else if (model.Contains("xs_nan_t3")) return "021";
            else if (model.Contains("xs_nv_t1")) return "002";
            else if (model.Contains("xs_nv_t2")) return "012";
            else if (model.Contains("xs_nv_t3")) return "022";
            return "001";
        }
        public static string getZdRoleHead(string model)
        {
            if (model == null) return "101";
            if (model.Contains("xs_nan_t1")) return "101";
            else if (model.Contains("xs_nan_t2")) return "111";
            else if (model.Contains("xs_nan_t3")) return "121";
            else if (model.Contains("xs_nv_t1")) return "102";
            else if (model.Contains("xs_nv_t2")) return "112";
            else if (model.Contains("xs_nv_t3")) return "122";
            return "101";
        }
        /**状态属性转中文*/
        public static string statusKeyToName(string key)
        {
            switch (key)
            {
                case "ljxg": return "滤镜效果";
                case "ztys": return "主题颜色";
                case "pk": return "切磋";
                case "moveSpeed": return "移动速度";
                case "jsxx": return "更换角色显示";
                case "bjb": return "药囊";
                case "rwzhd": return "造化丹";
                case "cwzhd": return "宠物造化丹";
                case "ydxc": return "诱敌香草";
                case "qdxc": return "驱敌香草";
                case "blx": return "百里香";
                case "jml": return "聚魔铃";
                case "dblq": return "多倍练潜";
                case "sffs": return "施法方式";
            }
            return null;
        }
        /**
        * 属性key转中文名
         */
        public static string propKeyToName(string key)
        {
            switch (key)
            {
                case "max_xue": return "生命";
                case "max_lan": return "蓝量";
                case "max_exp": return "经验";
                case "xue": return "生命";
                case "lan": return "蓝量";
                case "exp": return "经验";
                case "ll": return "力量";
                case "mj": return "敏捷";
                case "zl": return "智力";
                case "nl": return "耐力";
                case "js": return "精神";
                case "wg": return "物攻";
                case "fg": return "法攻";
                case "wf": return "物防";
                case "ff": return "法防";
                case "mz": return "命中";
                case "sd": return "闪躲";
                case "bj": return "暴击";
                case "css": return "速度";
                case "bjkx": return "暴击抗性";
                case "hlkx": return "混乱抗性";
                case "hskx": return "昏睡抗性";
                case "lxkx": return "流血抗性";
                case "attackHurt": return "伤害";
                case "sufferHurt": return "受伤";
                case "attackCure": return "治疗";
                case "sufferCure": return "受治疗";
                case "dingshen": return "定身";
                case "shuimian": return "睡眠";
                case "hunluan": return "混乱";
                case "fail": return "抵抗";
                case "immune": return "免疫";
                case "rest": return "休息";
                case "hp_fail": return "hp不足";
                case "mp_fail": return "mp不足";

                case "xue_zz": return "气血资质";
                case "lan_zz": return "魔法资质";
                case "wg_zz": return "物攻资质";
                case "fg_zz": return "法攻资质";
                case "wf_zz": return "物防资质";
                case "ff_zz": return "法防资质";
                case "bj_zz": return "暴击资质";
                case "sd_zz": return "闪躲资质";
                case "css_zz": return "出手速资质";
                case "mz_zz": return "命中资质";

                case "zy": return "职业";
                case "ch": return "称号";
                case "bp": return "帮派";
                case "sf": return "身份";
                case "gold": return "元宝";
                case "tale": return "银两";
                case "yp": return "银票";
                case "jf": return "竞技积分";
                case "djph": return "等级排行";
                case "wxz": return "武勋";
                case "teacher": return "师傅";
                case "stu1": return "徒弟1";
                case "stu2": return "徒弟2";
                case "stu3": return "徒弟3";
                case "jmPoint": return "真元点";
                case "bg": return "帮贡";
                case "wings": return "翅膀";
                case "zuoqi": return "坐骑";
                case "xld": return "修炼点";
                case "bb": return "帮币";
                case "ldjf": return "领地积分";

                case "qdxc": return "驱敌香草";
                case "sbjy": return "双倍经验丹";
                case "ydxc": return "诱敌香草";
                case "bjb": return "补给包";
                case "pk": return "切磋";
                case "bg_music": return "背景音乐";
                case "g_v": return "切换新版";
                case "map_size": return "调整场景大小";
                case "zhanli": return "战力";
                case "sez": return "身份";
                case "vip": return "vip经验";
                case "qhd": return "亲和度";
                case "realmLv": return "境界";
                default:
                    {
                        Debug.Log("找不到" + key);
                        break;
                    }
            }
            return null;
        }
        /**
        * 装备部位key转中文名
        */
        public static string equipPartKeyToName(string key)
        {
            switch (key)
            {
                case "wq": return "武器";
                case "jb": return "颈部";
                case "sz": return "手指";
                case "wb": return "腕部";
                case "tb": return "头部";
                case "xb": return "胸部";
                case "yb": return "腰部";
                case "tuib": return "腿部";
                case "jiaob": return "脚部";
                case "hf": return "护符";
                case "fb": return "法宝";
                case "gf": return "功法";
                case "bjb": return "药囊";
                case "pet_wq": return "宠物武器";
                case "pet_fj": return "宠物防具";
                case "pet_sp": return "宠物饰品";
                default: return null;
            }
        }
        /**根据物品品质获取底图*/
        public static string getGoodsQualityPic(int quality)
        {
            string pinzp = "WuPinKuangPinZhi_PuTong_png";
            if (quality == 1) pinzp = "WuPinKuangPinZhiLan_png";
            else if (quality == 2) pinzp = "WuPinKuangPinZhiZi_png";
            else if (quality == 3) pinzp = "WuPinKuangPinZhiCheng_png";
            else if (quality == 4) pinzp = "WuPinKuangPinZhiJin_png";
            else if (quality == 5) pinzp = "WuPinKuangPinZhiHong_png";
            return pinzp;
        }
        public static string getGoodsQualityBg(int quality)
        {
            string rpath = "QualityBlack_png";
            if (quality == 1) rpath = "QualityBlue_png";
            else if (quality == 2) rpath = "QualityPurple_png";
            else if (quality == 3) rpath = "QualityOrange_png";
            else if (quality == 4) rpath = "QualityGold_png";
            else if (quality == 5) rpath = "QualityRed_png";
            return rpath;
        }
        public static string getGoodsQualityColor(int quality)
        {
            string rpath = "#111210";
            if (quality == 1) rpath = "#1662fa";
            else if (quality == 2) rpath = "#d03cd3";
            else if (quality == 3) rpath = "#f9b024";
            else if (quality == 4) rpath = "#FFE000";
            else if (quality == 5) rpath = "#ff3e3e";
            return rpath;
        }
        public static List<string> getXianjueSkls(int type)
        {
            List<string> arr = null;
            if (type == 0)//主动
            {
                arr = new List<string>() { "100210030000", "100210030001", "100210030002", "100210030003",
                "100210030004", "100210030005", "100210030006", "100210030007", "100210030008", "100210030009",
                    "100210030010", "100210030011"};
            }
            else if (type == 1)//被动
            {
                arr = new List<string>() { "100210030012", "100210030013", "100210030014", "100210030015",
                "100210030016", "100210030017", "100210030018", "100210030019", "100210030020", "100210030021",
                    "100210030022","100210030023","100210030024","100210030025", "100210030026"};
            }
            return arr;
        }
        public static List<string> getJingMaiSkls()
        {
            List<string> arr = new List<string>() { "100210020000", "100210020001", "100210020002", "100210020003",
                "100210020004"};
            return arr;
        }
        public static List<string> getJobSkls(string key)
        {
            List<string> arr = null;
            if (key.Contains("ms"))
            {
                arr = new List<string>() { "100210000001", "100210000002", "100210000003", "100210000004",
                "100210000005","100210000006","100210000007",};
            }
            else if (key.Contains("dj"))
            {
                arr = new List<string>() { "100210000001", "100210000008", "100210000009", "100210000010",
                "100210000011","100210000012","100210000013",};
            }
            else if (key.Contains("qm"))
            {
                arr = new List<string>() { "100210000014", "100210000015", "100210000016", "100210000017",
                "100210000018","100210000019","100210000020",};
            }
            else if (key.Contains("ty"))
            {
                arr = new List<string>() { "100210000014", "100210000021", "100210000022", "100210000023",
                "100210000024","100210000025","100210000026",};
            }
            else if (key.Contains("ym"))
            {
                arr = new List<string>() { "100210000027", "100210000028", "100210000029", "100210000030",
                "100210000031","100210000032","100210000033",};
            }
            else if (key.Contains("lc"))
            {
                arr = new List<string>() { "100210000027", "100210000034", "100210000035", "100210000036",
                "100210000037","100210000038","100210000039",};
            }
            return arr;
        }
        public static int getTypeByJob(string key)
        {
            if (key.Contains("ms")) return 0;
            else if (key.Contains("dj")) return 0;
            else if (key.Contains("qm")) return 1;
            else if (key.Contains("ty")) return 1;
            else if (key.Contains("ym")) return 0;
            else return 0;
        }
        public static string getTalkByJob(string key)
        {
            if (key.Contains("ms")) return "月黑风高，老子一人射大雕";
            else if (key.Contains("dj")) return "踏歌行，吾辈身躯保太平";
            else if (key.Contains("qm")) return "一曲相思，任尔霸王亦落马";
            else if (key.Contains("ty")) return "愈离愁，千丝万缕皆挥去";
            else if (key.Contains("ym")) return "千里独行，万马千军任我行";
            else return "妙计生，不及吾咒定乾坤";
        }
        public static string getDesByJob(string key)
        {
            if (key.Contains("ms")) return "单点破杀";
            else if (key.Contains("dj")) return "防护肉盾";
            else if (key.Contains("qm")) return "群攻暴杀";
            else if (key.Contains("ty")) return "治疗返生";
            else if (key.Contains("ym")) return "吸血投毒";
            else return "咒术控制";
        }
        public static string getJobToSimpleName(string key)
        {
            if (key.Contains("ms/dj") || key.Contains("zs")) return "墨";
            else if (key.Contains("qm/ty") || key.Contains("fs")) return "道";
            else if (key.Contains("ym/lc") || key.Contains("fz")) return "阳";
            else if (key.Contains("ms")) return "猛";
            else if (key.Contains("dj")) return "遁";
            else if (key.Contains("qm")) return "琴";
            else if (key.Contains("ty")) return "音";
            else if (key.Contains("ym")) return "冥";
            else if (key.Contains("lc")) return "刹";

            else return "无";
        }
        public static string getJobToName(string key)
        {
            if (key.Contains("ms/dj") || key.Contains("zs")) return "墨";
            else if (key.Contains("qm/ty") || key.Contains("fs")) return "道";
            else if (key.Contains("ym/lc") || key.Contains("fz")) return "阳";
            else if (key.Contains("ms")) return "猛士";
            else if (key.Contains("dj")) return "遁甲";
            else if (key.Contains("qm")) return "琴魔";
            else if (key.Contains("ty")) return "天音";
            else if (key.Contains("ym")) return "幽冥";
            else if (key.Contains("lc")) return "罗刹";

            else return "无";
        }


        public static bool isRoleModel(string model)
        {
            string[] arr = getJobs();
            foreach (string a in arr)
            {
                if (model.Contains(a)) return true;
            }
            return false;
        }
        /**获取12个职业*/
        public static string[] getJobs()
        {
            string[] arr = { "xs_nan","xs_nv",
                "zs_nan","zs_nv","fs_nan","fs_nv","fz_nan","fz_nv",
                "ms_nan", "ms_nv", "dj_nan", "dj_nv", "qm_nan", "qm_nv",
                "ty_nan","ty_nv","ym_nan","ym_nv","lc_nan","lc_nv"};
            return arr;
        }
        public static string[] getAmStatusKeys()
        {
            //0 run 1 idle 2 show2 3 die 4 hit 5 run2 6 show 7 fangyu 8 bianshen 9 battle_run
            //10 attack 11 skill 12 skill1 13 skill2 14 skill3
            string[] STARUS_KEYS = {
            "run","idle","show2","die","hit","run2","show","fangyu","bianshen","battle_run","attack",
            "skill","skill1","skill2","skill3",
            };
            return STARUS_KEYS;
        }
        /**获取属性名 */
        public static string[] getAttrName()
        {
            string[] arr = {"ll", "mj", "zl", "nl", "js",
            "max_lan", "max_xue",
            "wg", "fg", "wf", "ff", "mz", "sd", "bj", "css" };
            return arr;
        }
    }
}
