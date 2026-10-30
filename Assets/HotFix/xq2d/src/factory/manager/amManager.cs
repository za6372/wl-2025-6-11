using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    /**将所有npc、宠物、人物动画都记录在这里*/
    public class amManager
    {
        public static JArray getAllKeys()
        {
            string[] arr = {
                "1220_1","1228",
                "zs_sjtx", "fs_sjtx", "fz_sjtx","pet_sjtx",

                "zs_nan","zs_nv","fs_nan","fs_nv","fz_nan","fz_nv",
                 "ms_nan","ms_nv","dj_nan","dj_nv","qm_nan","qm_nv","ty_nan","ty_nv","ym_nan","ym_nv","lc_nan","lc_nv",

                "zd_zs_jntx_1","zd_zs_jntx_2","zd_zs_jntx_3","zd_zs_jntx_4","zd_zs_jntx_5","zd_zs_jntx_6","zd_zs_jntx_7","zd_zs_jntx_8","zd_zs_jntx_9","zd_zs_jntx_10",
                "zd_zs_jntx_11","zd_zs_jntx_12","zd_zs_jntx_13","zd_zs_jntx_14","zd_zs_jntx_15","zd_zs_jntx_16","zd_zs_jntx_17","zd_zs_jntx_18","zd_zs_jntx_19","zd_zs_jntx_20",
                "zd_zs_jntx_21","zd_zs_jntx_22","zd_zs_jntx_23",

                "zd_fs_jntx_1","zd_fs_jntx_2","zd_fs_jntx_3","zd_fs_jntx_4","zd_fs_jntx_5","zd_fs_jntx_6","zd_fs_jntx_7","zd_fs_jntx_8","zd_fs_jntx_9","zd_fs_jntx_10",

                "zd_fz_jntx_1","zd_fz_jntx_2","zd_fz_jntx_3","zd_fz_jntx_4","zd_fz_jntx_5","zd_fz_jntx_6","zd_fz_jntx_8","zd_fz_jntx_9","zd_fz_jntx_10",
                "zd_fz_jntx_13","zd_fz_jntx_15","zd_fz_jntx_16","zd_fz_jntx_18",


                "xs_nan","xs_nv",
                "jqrw","sd","wcrw","sjtx","swtx","sl","cztx1","cztx2","cztx3",
                "100001","100003","100004","100005","100006","10002","1014","1025","1214","1215",
                "1217","1220","1221","1223","1224","1225","1227","16066","16067","16068","16069","16070",
                "16071","16072","16073","16074","16075","16076","16077","16078","16079","16080",
                "16081","18833","18835","18836","18837","18838","20002","20003","20004","20005",
                "30001","30003","30004","30005","30007","30009","30010","30011","30012","35932",
                "40001","40008","40010","50002","50003","50004","50005","50008","60002","60003",
                "60005","60006","60007","60009","60010","60012","60013","70001","70003","70006",
                "70007","80002","80003","80006","80007","80008","90002","90003","bs02","bs03",
                "bs04","bs05","bs06","bs07","bs08","bs09","bs10","bs11","bs15","bs16",
                "bs17","bs19","bs20","bs20(2)","bs21","bs22","bs23","bs24","bs25","bs26",
                "bs27","cw_sj","cw_sj2","cw_xlr","cw_xlrsj","entrance","gw_gcx","gw_kuangshi",
                "gw_shuyao",
                "NPC57_gw","NPC58_gw","NPC59_gw","paopao","pet_zr_sj",
                "sc01","sc05","sjbs","slm","ty21",
                "ty22","ty23","ty24","ty25","ty26","ty27","ty28","ty29","ty30","ty31",
                "ty32","ty33","ty34","ty35","ty36","ty37","ty38","ty39","ty40",
                "xueren","xz01","xz02","xz03","xz04","xz05","xz06","xz07","xz08",
                "xz09","xz10","xz11","xz12","xz13","xz14","xz15","xz16","xz17","xz18",
                "zms_sg"
            };
            JArray list = new JArray();
            foreach (string a in arr)
            {
                JObject temp = new JObject();
                temp.Add("name", a);
                list.Add(temp);
            }
            return list;
        }
        /**切换初始状态的头像*/
        public static void changeManHeadPwd(string head, AmModelMsg md)
        {
            for (int i = 0; i < md.changguiPwds.Length; i++)
            {
                if (md.changguiPwds[i].Contains("_t1_pwd") || md.changguiPwds[i].Contains("_t2_pwd") || md.changguiPwds[i].Contains("_t3_pwd"))
                {
                    md.changguiPwds[i] = head;
                }
            }
            for (int i = 0; i < md.zhandouPwds.Length; i++)
            {
                if (md.zhandouPwds[i].Contains("_t1_pwd") || md.zhandouPwds[i].Contains("_t2_pwd") || md.zhandouPwds[i].Contains("_t3_pwd"))
                {
                    md.zhandouPwds[i] = "zd_" + head;
                }
            }
            for (int i = 0; i < md.touxiangPwds.Length; i++)
            {
                if (md.touxiangPwds[i].Contains("_t1_pwd") || md.touxiangPwds[i].Contains("_t2_pwd") || md.touxiangPwds[i].Contains("_t3_pwd"))
                {
                    md.touxiangPwds[i] = head;
                }
            }
        }
        public static void changeManWqPwd(string wq, AmModelMsg md)
        {
            for (int i = 0; i < md.changguiPwds.Length; i++)
            {
                if (md.changguiPwds[i].Contains("_wq1_") || md.changguiPwds[i].Contains("_wq2_") || md.changguiPwds[i].Contains("_wq3_"))
                {
                    md.changguiPwds[i] = wq;
                }
            }
            for (int i = 0; i < md.zhandouPwds.Length; i++)
            {
                if (md.zhandouPwds[i].Contains("_wq1_") || md.zhandouPwds[i].Contains("_wq2_") || md.zhandouPwds[i].Contains("_wq3_"))
                {
                    md.zhandouPwds[i] = wq;
                }
            }
            for (int i = 0; i < md.touxiangPwds.Length; i++)
            {
                if (md.touxiangPwds[i].Contains("_wq1_") || md.touxiangPwds[i].Contains("_wq2_") || md.touxiangPwds[i].Contains("_wq3_"))
                {
                    md.touxiangPwds[i] = wq;
                }
            }
        }
        /**获取攻击特效*/
        public static AmModelMsg getAttackTx(string amKey, string sklKey = null)
        {
            List<string> txs = new List<string>();
            List<AmPlayControl> sklAction = new List<AmPlayControl>();
            List<AmPlayControl> mzAction = new List<AmPlayControl>();
            List<string> sjtxPwds = new List<string>();
            string sjtxAef = null;
            List<AmPlayControl> sjtxAction = new List<AmPlayControl>();

            AmModelMsg md = getAmByKey(amKey);

            if (amKey.Contains("ms_nan") || amKey.Contains("ms_nv"))
            {
                if (sklKey == null)
                {
                    txs.Add("zd_zs_jntx_8_pwd");//刀光
                    sklAction.Add(new AmPlayControl(3, 5));
                    mzAction.Add(new AmPlayControl(5, 9));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000002"))//猛虎纵
                    {
                        txs.Add("zd_zs_jntx_6_pwd");//脚上雾气
                        txs.Add("zd_zs_jntx_14_pwd");//飞身光环
                        txs.Add("zd_zs_jntx_17_pwd");//命中单条血色斜线
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_1");
                        sjtxPwds.Add("zs_sjtx_15");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(0, 7));
                    }
                    else if (sklKey.Equals("100210000003"))//破天吼
                    {
                        txs.Add("zd_zs_jntx_16_pwd");//冲天雾气
                        txs.Add("zd_zs_jntx_18_pwd");//脚下黄光
                        sklAction.Add(new AmPlayControl(16, 17));
                        sklAction.Add(new AmPlayControl(25, 31));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("zs_sjtx_16");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(22, 25));
                    }
                    else if (sklKey.Equals("100210000005"))//连斩
                    {
                        txs.Add("zd_zs_jntx_1_pwd");//黄色光球
                        txs.Add("zd_zs_jntx_8_pwd");//刀光
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        mzAction.Add(new AmPlayControl(20, 25));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_1");
                        sjtxPwds.Add("zs_sjtx_9");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(0, 4));
                        sjtxAction.Add(new AmPlayControl(20, 22));
                    }
                    else if (sklKey.Equals("100210000006"))//血祭
                    {
                        txs.Add("zd_zs_jntx_4_pwd");//脚部沙尘
                        txs.Add("zd_zs_jntx_20_pwd");//血色斜线
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_4");
                        sjtxPwds.Add("zs_sjtx_1");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(12, 16));
                    }
                    else if (sklKey.Equals("100210000007"))//狂龙吼
                    {
                        txs.Add("zd_zs_jntx_2_pwd");//蓝色光束球
                        sklAction.Add(new AmPlayControl(16, 17));
                        sklAction.Add(new AmPlayControl(25, 31));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("zs_sjtx_6");
                        sjtxPwds.Add("zs_sjtx_7");
                        sjtxPwds.Add("zs_sjtx_13");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(25, 36));
                    }
                }
            }
            else if (amKey.Contains("dj_nan") || amKey.Contains("dj_nv"))
            {
                if (sklKey == null)
                {
                    txs.Add("zd_zs_jntx_8_pwd");//刀光
                    sklAction.Add(new AmPlayControl(3, 5));
                    mzAction.Add(new AmPlayControl(5, 9));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000008"))//猛虎纵
                    {
                        txs.Add("zd_zs_jntx_6_pwd");//脚上雾气
                        txs.Add("zd_zs_jntx_14_pwd");//飞身光环
                        txs.Add("zd_zs_jntx_17_pwd");//命中单条血色斜线
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_1");
                        sjtxPwds.Add("zs_sjtx_15");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(0, 7));
                    }
                    else if (sklKey.Equals("100210000009"))//破天吼
                    {
                        txs.Add("zd_zs_jntx_16_pwd");//冲天雾气
                        txs.Add("zd_zs_jntx_18_pwd");//脚下黄光
                        sklAction.Add(new AmPlayControl(16, 17));
                        sklAction.Add(new AmPlayControl(25, 31));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("zs_sjtx_16");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(22, 25));
                    }
                    else if (sklKey.Equals("100210000011"))//破甲
                    {
                        txs.Add("zd_zs_jntx_18_pwd");//脚下黄光
                        txs.Add("zd_zs_jntx_3_pwd");//蓝色光束
                        txs.Add("zd_zs_jntx_13_pwd");//蓝色斜线
                        txs.Add("zd_zs_jntx_11_pwd");//
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_12");
                        sjtxPwds.Add("zs_sjtx_11");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(16, 20));
                    }
                    else if (sklKey.Equals("100210000012"))//战魂歌
                    {
                        txs.Add("zd_zs_jntx_9_pwd");//脚部沙尘
                        txs.Add("zd_zs_jntx_19_pwd");//血色斜线
                        sklAction.Add(new AmPlayControl(16, 17));
                        sklAction.Add(new AmPlayControl(25, 31));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("zs_sjtx_6");
                        sjtxPwds.Add("zs_sjtx_7");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(25, 36));
                    }
                    else if (sklKey.Equals("100210000013"))//挫骨
                    {
                        txs.Add("zd_zs_jntx_1_pwd");//黄色光球
                        txs.Add("zd_zs_jntx_14_pwd");//飞身光环
                        txs.Add("zd_zs_jntx_18_pwd");//脚下黄光
                        txs.Add("zd_zs_jntx_4_pwd");//脚部沙尘
                        txs.Add("zd_zs_jntx_10_pwd");//命中黄色光球
                        sklAction.Add(new AmPlayControl(16, 20));
                        mzAction.Add(new AmPlayControl(20, 25));
                        sjtxPwds.Add("zs_sjtx_8");
                        sjtxPwds.Add("zs_sjtx_9");
                        sjtxPwds.Add("zs_sjtx_3");
                        sjtxPwds.Add("zs_sjtx_10");
                        sjtxAef = "zs_cstx";
                        sjtxAction.Add(new AmPlayControl(7, 12));
                    }
                }
            }
            else if (amKey.Contains("qm_nan") || amKey.Contains("qm_nv"))
            {
                //.addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                if (sklKey == null)
                {
                    txs.Add("zd_fs_jntx_1_pwd");//琴炫光
                    sklAction.Add(new AmPlayControl(3, 11));
                    mzAction.Add(new AmPlayControl(true));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000015"))//荡魔曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_5_pwd");//
                        txs.Add("zd_fs_jntx_3_pwd");//
                        txs.Add("zd_fs_jntx_4_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_3");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(12, 17));
                    }
                    else if (sklKey.Equals("100210000016"))//灵脉曲
                    {
                        txs.Add("zd_fs_jntx_2_pwd");//
                        sklAction.Add(new AmPlayControl(35, 44));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_2");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(6, 12));
                    }
                    else if (sklKey.Equals("100210000018"))//魔魂曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_2_pwd");//
                        txs.Add("zd_fs_jntx_3_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_1");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(0, 6));
                    }
                    else if (sklKey.Equals("100210000019"))//夺魂曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_5_pwd");//
                        txs.Add("zd_fs_jntx_3_pwd");//
                        txs.Add("zd_fs_jntx_4_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_5");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(25, 32));
                    }
                    else if (sklKey.Equals("100210000020"))//缚灵曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_5_pwd");//
                        txs.Add("zd_fs_jntx_3_pwd");//
                        txs.Add("zd_fs_jntx_4_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_4");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(17, 25));
                    }
                }
            }
            else if (amKey.Contains("ty_nan") || amKey.Contains("ty_nv"))
            {
                //.addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                if (sklKey == null)
                {
                    txs.Add("zd_fs_jntx_1_pwd");//琴炫光
                    sklAction.Add(new AmPlayControl(3, 11));
                    mzAction.Add(new AmPlayControl(true));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000021"))//荡魔曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_5_pwd");//
                        txs.Add("zd_fs_jntx_3_pwd");//
                        txs.Add("zd_fs_jntx_4_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_3");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(12, 17));
                    }
                    else if (sklKey.Equals("100210000022"))//灵脉曲
                    {
                        txs.Add("zd_fs_jntx_2_pwd");//
                        sklAction.Add(new AmPlayControl(35, 44));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_2");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(6, 12));
                    }
                    else if (sklKey.Equals("100210000024"))//养生曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_6_pwd");//
                        txs.Add("zd_fs_jntx_8_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_10");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(32, 38));
                    }
                    else if (sklKey.Equals("100210000025"))//天籁曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_6_pwd");//
                        txs.Add("zd_fs_jntx_8_pwd");//
                        txs.Add("zd_fs_jntx_9_pwd");//
                        txs.Add("zd_fs_jntx_10_pwd");//
                        sklAction.Add(new AmPlayControl(17, 35));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_10");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(32, 38));
                    }
                    else if (sklKey.Equals("100210000026"))//返魂曲
                    {
                        txs.Add("zd_fs_jntx_7_pwd");//
                        txs.Add("zd_fs_jntx_6_pwd");//
                        sklAction.Add(new AmPlayControl(35, 44));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fs_sjtx_11");
                        sjtxAef = "fs_cstx";
                        sjtxAction.Add(new AmPlayControl(45, 52));
                    }
                }
            }
            else if (amKey.Contains("ym_nan") || amKey.Contains("ym_nv"))
            {
                //.addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32) .addZdMzAm(22, 27)
                if (sklKey == null)
                {
                    sklAction.Add(new AmPlayControl(3, 10));
                    mzAction.Add(new AmPlayControl(true));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000029"))//淬毒
                    {
                        txs.Add("zd_fz_jntx_3_pwd");//
                        sklAction.Add(new AmPlayControl(17, 22));
                        mzAction.Add(new AmPlayControl(22, 27));
                        sjtxPwds.Add("fz_sjtx_6");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(37, 40));
                    }
                    else if (sklKey.Equals("100210000030"))//扬沙
                    {
                        txs.Add("zd_fz_jntx_2_pwd");//
                        sklAction.Add(new AmPlayControl(27, 32));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fz_sjtx_7");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(40, 44));
                    }
                    else if (sklKey.Equals("100210000031"))//影袭
                    {
                        txs.Add("zd_fz_jntx_2_pwd");//
                        txs.Add("zd_fz_jntx_15_pwd");//
                        txs.Add("zd_fz_jntx_16_pwd");//
                        sklAction.Add(new AmPlayControl(17, 22));
                        mzAction.Add(new AmPlayControl(22, 27));
                        sjtxPwds.Add("fz_sjtx_17");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(33, 37));
                    }
                    else if (sklKey.Equals("100210000032"))//断脉
                    {
                        txs.Add("zd_fz_jntx_9_pwd");//
                        txs.Add("zd_fz_jntx_10_pwd");//
                        sklAction.Add(new AmPlayControl(17, 22));
                        mzAction.Add(new AmPlayControl(22, 27));
                        sjtxPwds.Add("fz_sjtx_4");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(18, 22));
                    }
                    else if (sklKey.Equals("100210000033"))//凝钢决
                    {
                        txs.Add("zd_fz_jntx_2_pwd");//
                        txs.Add("zd_fz_jntx_13_pwd");//
                        sklAction.Add(new AmPlayControl(27, 32));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fz_sjtx_12");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(22, 27));
                    }
                }
            }
            else if (amKey.Contains("lc_nan") || amKey.Contains("lc_nv"))
            {
                //.addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32) .addZdMzAm(22, 27)
                if (sklKey == null)
                {
                    sklAction.Add(new AmPlayControl(3, 10));
                    mzAction.Add(new AmPlayControl(true));
                }
                else
                {
                    //对每个技能进行特效设定
                    if (sklKey.Equals("100210000035"))//淬毒
                    {
                        txs.Add("zd_fz_jntx_3_pwd");//
                        sklAction.Add(new AmPlayControl(17, 22));
                        mzAction.Add(new AmPlayControl(22, 27));
                        sjtxPwds.Add("fz_sjtx_6");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(37, 40));
                    }
                    else if (sklKey.Equals("100210000036"))//扬沙
                    {
                        txs.Add("zd_fz_jntx_2_pwd");//
                        sklAction.Add(new AmPlayControl(27, 32));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fz_sjtx_7");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(40, 44));
                    }
                    else if (sklKey.Equals("100210000037"))//定身蛊
                    {
                        txs.Add("zd_fz_jntx_13_pwd");//
                        txs.Add("zd_fz_jntx_6_pwd");//
                        txs.Add("zd_fz_jntx_18_pwd");//
                        sklAction.Add(new AmPlayControl(27, 32));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fz_sjtx_15");
                        sjtxPwds.Add("fz_sjtx_14");
                        sjtxPwds.Add("fz_sjtx_13");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(27, 33));
                    }
                    else if (sklKey.Equals("100210000038"))//混乱蛊
                    {
                        txs.Add("zd_fz_jntx_5_pwd");//
                        txs.Add("zd_fz_jntx_6_pwd");//
                        sklAction.Add(new AmPlayControl(27, 32));
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("fz_sjtx_2");
                        sjtxPwds.Add("fz_sjtx_8");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(9, 14));
                    }
                    else if (sklKey.Equals("100210000039"))//碎魂
                    {
                        txs.Add("zd_fz_jntx_16_pwd");//
                        sklAction.Add(new AmPlayControl(17, 22));
                        mzAction.Add(new AmPlayControl(22, 27));
                        sjtxPwds.Add("fz_sjtx_4");
                        sjtxPwds.Add("fz_sjtx_9");
                        sjtxPwds.Add("fz_sjtx_10");
                        sjtxAef = "fz_cstx";
                        sjtxAction.Add(new AmPlayControl(15, 22));
                    }
                }
            }

            else
            {
                //不修改原有的sklAction、mzAction
                if (sklKey == null)
                {
                    sklAction.Add(md.zdSklList[0]);
                    mzAction.Add(md.zdMzList[0]);
                }
                else
                {
                    sklAction.Add(md.zdSklList[1]);
                    mzAction.Add(md.zdMzList[0]);
                    if (sklKey.Equals("100210010179"))//轰天狂雷
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_10");
                        sjtxPwds.Add("pet_sjtx_11");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(27, 37));
                    }
                    else if (sklKey.Equals("100210010181"))//真魔血破
                    {
                        sjtxPwds.Add("pet_sjtx_82");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(72, 79));
                    }
                    else if (sklKey.Equals("100210010184"))//浴火重生
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_6");
                        sjtxPwds.Add("pet_sjtx_8");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(16, 22));
                    }
                    else if (sklKey.Equals("100210010188"))//乾坤逆转
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_3");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(6, 10));
                    }
                    else if (sklKey.Equals("100210010193"))//万剑归宗
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_100");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(79, 84));
                    }
                    else if (sklKey.Equals("100210010199"))//天蛇噬血
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_103");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(90, 96));
                    }
                    else if (sklKey.Equals("100210010200"))//天火燎原
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_105");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(110, 119));
                    }
                    else if (sklKey.Equals("100210010202"))//都天炎爆
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_106");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(102, 110));
                    }
                    else if (sklKey.Equals("100210010205"))//神龙摆尾
                    {
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_103");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(107, 110));
                    }
                    else if(sklKey.Equals("100210010207"))
                    {
                        sklAction.Clear();
                        sklAction.Add(md.zdSklList[2]);
                        mzAction.Add(new AmPlayControl(true));
                        sjtxPwds.Add("pet_sjtx_103");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(107, 110));
                    }
                    else
                    {
                        //给一个默认的命中特效
                        sjtxPwds.Add("pet_sjtx_5");
                        sjtxAef = "pet_sjtx";
                        sjtxAction.Add(new AmPlayControl(12, 16));
                    }
                }

            }



            //将原有的动作删除（技能、命中）,使用时只取数组第一个
            if (sklAction.Count > 0)
            {
                md.zdSklList.Clear();
                md.addZdSklAm(sklAction);
            }
            if (mzAction.Count > 0)
            {
                md.zdMzList.Clear();
                if (!mzAction[0].isNull) md.addZdMzAm(mzAction);
                else md.zdMzList = null;
            }
            if (sjtxAction.Count > 0)
            {
                md.addZdSjtxAm(sjtxAction);
            }
            //将特效文件给替删除
            List<string> list = new List<string>(md.zhandouPwds);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Contains("_jntx_"))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
            list.AddRange(txs);
            md.zhandouPwds = list.ToArray();
            if (sjtxPwds.Count > 0)
            {
                md.addSjtxPwd(sjtxPwds.ToArray());
                md.addSjtxAef(sjtxAef);
            }

            //Debug.Log(strUtils.copyJSON<JObject>(md));
            return md;
        }
        /**包括待机动画（从哪一帧到哪一帧）、行走动画、战斗动画等*/
        public static AmModelMsg getAmByKey(string amKey)
        {
            AmModelMsg md = new AmModelMsg(amKey);
            //门派形象跟职业形象就是换个武器 zs_为门派 ms_为职业
            if (amKey.Contains("zs_nan"))
            {
                string wqPwd = "zs_wq2_cg_pwd";
                string zdWqPwd = "zd_zs_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "").addChangguiAef("zs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }else if (amKey.Contains("zs_nv"))
            {
                string wqPwd = "zs_wq2_cg_pwd";
                string zdWqPwd = "zd_zs_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addTxPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nv_t1_pwd", "zd_zs_nv_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nv_zd_aef", "").addChangguiAef("zs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (amKey.Contains("fs_nan"))
            {
                string wqPwd = "fs_wq3_cg_pwd";
                string zdWqPwd = "zd_fs_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "").addChangguiAef("fs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("fs_nv"))
            {
                string wqPwd = "fs_wq3_cg_pwd";
                string zdWqPwd = "zd_fs_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nv_t1_pwd", "zd_fs_nv_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nv_zd_aef", "").addChangguiAef("fs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("fz_nan"))
            {
                string wqPwd = "fz_wq3_cg_pwd";
                string zdWqPwd = "zd_fz_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                           .addTxPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                           .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", "zd_fz_jntx_1_pwd")
                        .addZhandouAef("zd_fz_nan_zd_aef", "").addChangguiAef("fz_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("fz_nv"))
            {
                string wqPwd = "fz_wq3_cg_pwd";
                string zdWqPwd = "zd_fz_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nv_t1_pwd", "zd_fz_nv_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nv_zd_aef", "").addChangguiAef("fz_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("ms_nan"))
            {
                string wqPwd = "zs_wq3_cg_pwd";
                string zdWqPwd = "zd_zs_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "").addChangguiAef("zs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (amKey.Contains("ms_nv"))
            {
                string wqPwd = "zs_wq3_cg_pwd";
                string zdWqPwd = "zd_zs_wq3_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addTxPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nv_t1_pwd", "zd_zs_nv_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nv_zd_aef", "").addChangguiAef("zs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (amKey.Contains("dj_nan"))
            {
                string wqPwd = "zs_wq1_cg_pwd";
                string zdWqPwd = "zd_zs_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "").addChangguiAef("zs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (amKey.Contains("dj_nv"))
            {
                string wqPwd = "zs_wq1_cg_pwd";
                string zdWqPwd = "zd_zs_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                           .addTxPwd(wqPwd, "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                           .addZdPwd("zd_zs_touying1_pwd", zdWqPwd, "zd_zs_nv_t1_pwd", "zd_zs_nv_zd_pwd", "zd_zs_jntx_1_pwd")
                        .addZhandouAef("zd_zs_nv_zd_aef", "").addChangguiAef("zs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (amKey.Contains("qm_nan"))
            {
                string wqPwd = "fs_wq1_cg_pwd";
                string zdWqPwd = "zd_fs_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "").addChangguiAef("fs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("qm_nv"))
            {
                string wqPwd = "fs_wq1_cg_pwd";
                string zdWqPwd = "zd_fs_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nv_t1_pwd", "zd_fs_nv_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nv_zd_aef", "").addChangguiAef("fs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("ty_nan"))
            {
                string wqPwd = "fs_wq2_cg_pwd";
                string zdWqPwd = "zd_fs_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "").addChangguiAef("fs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("ty_nv"))
            {
                string wqPwd = "fs_wq2_cg_pwd";
                string zdWqPwd = "zd_fs_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", zdWqPwd, "zd_fs_nv_t1_pwd", "zd_fs_nv_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nv_zd_aef", "").addChangguiAef("fs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("ym_nan"))
            {
                string wqPwd = "fz_wq2_cg_pwd";
                string zdWqPwd = "zd_fz_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                           .addTxPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                           .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", "zd_fz_jntx_1_pwd")
                        .addZhandouAef("zd_fz_nan_zd_aef", "").addChangguiAef("fz_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("ym_nv"))
            {
                string wqPwd = "fz_wq2_cg_pwd";
                string zdWqPwd = "zd_fz_wq2_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nv_t1_pwd", "zd_fz_nv_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nv_zd_aef", "").addChangguiAef("fz_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("lc_nan"))
            {
                string wqPwd = "fz_wq1_cg_pwd";
                string zdWqPwd = "zd_fz_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addTxPwd(wqPwd, "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nan_zd_aef", "").addChangguiAef("fz_nan_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (amKey.Contains("lc_nv"))
            {
                string wqPwd = "fz_wq1_cg_pwd";
                string zdWqPwd = "zd_fz_wq1_zd_pwd";
                md.addCgPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addTxPwd(wqPwd, "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", zdWqPwd, "zd_fz_nv_t1_pwd", "zd_fz_nv_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nv_zd_aef", "").addChangguiAef("fz_nv_cg_aef", "").addTouxiangAef(amKey, "");
                md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(22, 27).addZdMzAm(20, 25)
                    .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                md.setSimpleFightIsNear(false);
                return md;
            }
            else if (strUtils.isMatch(amKey, "zd_zs_jntx_[0-9]{1,}"))
            {
                md.addZdPwd("zd_zs_touying1_pwd", "zd_zs_wq3_zd_pwd", "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", amKey + "_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "");
                md.addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (strUtils.isMatch(amKey, "zd_fs_tx[0-9]{1,}"))
            {
                md.addZdPwd("zd_fs_touying1_pwd", "zd_fs_wq3_zd_pwd", "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", amKey + "_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "");
                md.addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            else if (strUtils.isMatch(amKey, "zd_fz_jntx_[0-9]{1,}"))
            {
                md.addZdPwd("zd_fz_touying1_pwd", "zd_fz_wq3_zd_pwd", "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", amKey + "_pwd")
                         .addZhandouAef("zd_fz_nan_zd_aef", "");
                md.addZdIdleAm(0, 3)
                    //3-4普攻 16-19跳跃 25-30原地吼 
                    .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                    //5-8普攻命中 20-24技能命中
                    .addZdMzAm(5, 9).addZdMzAm(20, 25)
                    .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                return md;
            }
            //命中特效
            else if (strUtils.isMatch(amKey, "(zs|fs|fz)_sjtx"))
            {
                string pre = amKey.Substring(0, 2);
                List<int> arr = null;
                if (pre.Contains("zs")) arr = new List<int>() { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
                else if (pre.Contains("fs")) arr = new List<int>() { 1, 2, 3, 4, 5, 7, 8, 10, 11, };
                else if (pre.Contains("fz")) arr = new List<int>() { 1, 2, 3, 4, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17 };
                string[] brr = new string[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    brr[i] = pre + "_sjtx_" + arr[i] + "_pwd";
                }
                md.addZdPwd(brr).addZhandouAef(pre + "_cstx", "_aef");
                md.addZdIdleAm(0, 5);
                return md;
            }
            else if (amKey.Equals("pet_sjtx"))
            {
                List<int> arr = new List<int>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 71, 72, 81, 82, 100, 101, 102, 103, 104, 105, 106 };
                string[] brr = new string[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    brr[i] = "pet_sjtx_" + arr[i] + "_pwd";
                }
                md.addZdPwd(brr).addZhandouAef("pet_sjtx", "_aef");
                md.addZdIdleAm(0, 5);
                return md;
            }

            switch (amKey)
            {
                case "jqrw"://接取任务
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 5);
                        return md;
                    }
                case "sd":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 5);
                        return md;
                    }
                case "wcrw"://完成任务
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "sjtx"://升级
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 7);
                        return md;
                    }
                case "swtx"://死亡
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 12).addZdIdleAm(5, 12).addZdMzAm(0, 5);
                        return md;
                    }
                case "sl"://选角时人物底座动画
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "cztx1"://金装特效
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "cztx2"://金装特效
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "cztx3"://金装特效
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                //npc
                case "kh":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "cwqh":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "jrhdds_NPC":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "shoushen":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "NPC_fbhf":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "npc38":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "dzfsb":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "npc39":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "npc37":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "cz_NPC":
                    {
                        md.addCgPwd("cz_NPC", "cztx_02").addChangguiAef("NPC_cz", "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }
                case "jqnpc01":
                    {
                        md.addCgPwd(amKey).addChangguiAef(amKey, "_aef");
                        md.addCgIdleAm(0, 6);
                        return md;
                    }

                case "xs_nan":
                    {
                        md.addCgPwd("xs_wq1_cg_pwd", "xs_nan_cg_pwd", "xs_nan_t1_pwd")
                            .addTxPwd("xs_wq1_cg_pwd", "xs_nan_cg_pwd", "xs_nan_t1_pwd")
                            .addZdPwd("zd_xs_touying1_pwd", "zd_xs_wq1_zd_pwd", "zd_xs_nan_t1_pwd", "zd_xs_nan_zd_pwd")
                         .addZhandouAef("zd_xs_nan_zd_aef", "").addChangguiAef("xs_nan_cg_aef", "").addTouxiangAef("xs_nan_t1_pwd", "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(3, 7)
                            .addZdMzAm(7, 10).addZdHurtAm(10, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "xs_nv":
                    {
                        md.addCgPwd("xs_wq2_cg_pwd", "xs_nv_cg_pwd", "xs_nv_t1_pwd")
                            .addTxPwd("xs_wq2_cg_pwd", "xs_nv_cg_pwd", "xs_nv_t1_pwd")
                            .addZdPwd("zd_xs_touying1_pwd", "zd_xs_wq2_zd_pwd", "zd_xs_nv_t1_pwd", "zd_xs_nv_zd_pwd")
                         .addZhandouAef("zd_xs_nv_zd_aef", "").addChangguiAef("xs_nv_cg_aef", "").addTouxiangAef("xs_nv_t1_pwd", "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(3, 7)
                            .addZdMzAm(7, 10).addZdHurtAm(10, 13).addZdSdAm(13, 15);
                        return md;
                    }
                /*case "ms_nan":
                    {
                        md.addCgPwd("zs_wq3_cg_pwd", "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addTxPwd("zs_wq3_cg_pwd", "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", "zd_zs_wq3_zd_pwd", "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "").addChangguiAef("zs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "ms_nv":
                    {
                        md.addCgPwd("zs_wq3_cg_pwd", "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addTxPwd("zs_wq3_cg_pwd", "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", "zd_zs_wq3_zd_pwd", "zd_zs_nv_t1_pwd", "zd_zs_nv_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nv_zd_aef", "").addChangguiAef("zs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "dj_nan":
                    {
                        md.addCgPwd("zs_wq1_cg_pwd", "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addTxPwd("zs_wq1_cg_pwd", "zs_nan_cg_pwd", "zs_nan_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", "zd_zs_wq1_zd_pwd", "zd_zs_nan_t1_pwd", "zd_zs_nan_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nan_zd_aef", "").addChangguiAef("zs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "dj_nv":
                    {
                        md.addCgPwd("zs_wq1_cg_pwd", "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addTxPwd("zs_wq1_cg_pwd", "zs_nv_cg_pwd", "zs_nv_t1_pwd")
                            .addZdPwd("zd_zs_touying1_pwd", "zd_zs_wq1_zd_pwd", "zd_zs_nv_t1_pwd", "zd_zs_nv_zd_pwd", "zd_zs_jntx_1_pwd")
                         .addZhandouAef("zd_zs_nv_zd_aef", "").addChangguiAef("zs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 5).addZdSklAm(16, 20).addZdSklAm(25, 31)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(9, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "qm_nan":
                    {
                        md.addCgPwd("fs_wq1_cg_pwd", "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addTxPwd("fs_wq1_cg_pwd", "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", "zd_fs_wq1_zd_pwd", "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "").addChangguiAef("fs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "qm_nv":
                    {
                        md.addCgPwd("fs_wq1_cg_pwd", "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addTxPwd("fs_wq1_cg_pwd", "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", "zd_fs_wq1_zd_pwd", "zd_fs_nv_t1_pwd", "zd_fs_nv_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nv_zd_aef", "").addChangguiAef("fs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty_nan":
                    {
                        md.addCgPwd("fs_wq3_cg_pwd", "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addTxPwd("fs_wq3_cg_pwd", "fs_nan_cg_pwd", "fs_nan_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", "zd_fs_wq3_zd_pwd", "zd_fs_nan_t1_pwd", "zd_fs_nan_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nan_zd_aef", "").addChangguiAef("fs_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty_nv":
                    {
                        md.addCgPwd("fs_wq3_cg_pwd", "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addTxPwd("fs_wq3_cg_pwd", "fs_nv_cg_pwd", "fs_nv_t1_pwd")
                            .addZdPwd("zd_fs_touying1_pwd", "zd_fs_wq3_zd_pwd", "zd_fs_nv_t1_pwd", "zd_fs_nv_zd_pwd", "zd_fs_jntx_1_pwd")
                         .addZhandouAef("zd_fs_nv_zd_aef", "").addChangguiAef("fs_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 11).addZdSklAm(17, 35).addZdSklAm(35, 44)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(5, 9).addZdMzAm(20, 25)
                            .addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ym_nan":
                    {
                        md.addCgPwd("fz_wq2_cg_pwd", "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addTxPwd("fz_wq2_cg_pwd", "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", "zd_fz_wq2_zd_pwd", "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nan_zd_aef", "").addChangguiAef("fz_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(22, 27).addZdMzAm(20, 25)
                            .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ym_nv":
                    {
                        md.addCgPwd("fz_wq2_cg_pwd", "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addTxPwd("fz_wq2_cg_pwd", "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", "zd_fz_wq2_zd_pwd", "zd_fz_nv_t1_pwd", "zd_fz_nv_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nv_zd_aef", "").addChangguiAef("fz_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(22, 27).addZdMzAm(20, 25)
                            .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "lc_nan":
                    {
                        md.addCgPwd("fz_wq1_cg_pwd", "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addTxPwd("fz_wq1_cg_pwd", "fz_nan_cg_pwd", "fz_nan_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", "zd_fz_wq1_zd_pwd", "zd_fz_nan_t1_pwd", "zd_fz_nan_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nan_zd_aef", "").addChangguiAef("fz_nan_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(22, 27).addZdMzAm(20, 25)
                            .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "lc_nv":
                    {
                        md.addCgPwd("fz_wq1_cg_pwd", "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addTxPwd("fz_wq1_cg_pwd", "fz_nv_cg_pwd", "fz_nv_t1_pwd")
                            .addZdPwd("zd_fz_touying1_pwd", "zd_fz_wq1_zd_pwd", "zd_fz_nv_t1_pwd", "zd_fz_nv_zd_pwd", "zd_fz_jntx_1_pwd")
                         .addZhandouAef("zd_fz_nv_zd_aef", "").addChangguiAef("fz_nv_cg_aef", "").addTouxiangAef(amKey, "");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            //3-4普攻 16-19跳跃 25-30原地吼 
                            .addZdSklAm(3, 10).addZdSklAm(17, 22).addZdSklAm(27, 32)
                            //5-8普攻命中 20-24技能命中
                            .addZdMzAm(22, 27).addZdMzAm(20, 25)
                            .addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }*/

                case "1014"://赤灵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "1025"://冥犬
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        return md;
                    }
                case "1214"://剑圣
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(3, 7)
                            .addZdMzAm(7, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        return md;
                    }
                case "1215"://梦瑶
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(7, 13).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1217"://落羽
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(7, 13).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1220"://龙
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 13).addZdSklAm(3, 13)
                            .addZdMzAm(7, 13).addZdHurtAm(13, 22).addZdSdAm(22, 28);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1220_1"://x8龙
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 13).addZdSklAm(3, 13)
                            .addZdMzAm(7, 13).addZdHurtAm(13, 22).addZdSdAm(22, 28);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1221"://姬
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(15, 28).addZdSklAm(28, 37)
                            .addZdMzAm(6, 13).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1221_1"://姬
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(15, 28).addZdSklAm(28, 37)
                            .addZdMzAm(6, 13).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1223"://女
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(7, 13).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1223_1"://女
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(7, 13).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1224"://兵
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "1224_1"://兵
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "1225"://帝君
                    {
                        md.addPwdToAll(amKey, 4)
                           .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(20, 28)
                            .addZdMzAm(6, 13).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1225_1"://帝君
                    {
                        md.addPwdToAll(amKey, 4)
                           .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(20, 28)
                            .addZdMzAm(6, 13).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "1227"://俑
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(8, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "1228"://测试怪物
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(8, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "10002"://赤翼蝠
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 4)
                            .addZdSklAm(4, 6).addZdSklAm(4, 6)
                            .addZdMzAm(6, 10).addZdHurtAm(10, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "16066":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 2)
                            .addZdSklAm(2, 4).addZdSklAm(2, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "16067":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 8).addZdHurtAm(9, 13).addZdSdAm(13, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16068":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 13).addZdSdAm(13, 15);
                        return md;
                    }
                case "16069":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16070":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 8).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16071":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "16072":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(4, 9).addZdHurtAm(11, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16073":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        return md;
                    }
                case "16074":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 10).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16075":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 13).addZdHurtAm(13, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16076":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(2, 3).addZdSklAm(2, 3)
                            .addZdMzAm(3, 8).addZdHurtAm(8, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "16077":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(3, 8).addZdHurtAm(12, 16).addZdSdAm(16, 19);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16078":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(3, 8).addZdHurtAm(11, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "16079":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 11).addZdHurtAm(11, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "16080":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(2, 3).addZdSklAm(2, 3)
                            .addZdMzAm(3, 9).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "16081":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(3, 9).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "18833":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        return md;
                    }
                case "18835":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "18836":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(16, 25)
                            .addZdMzAm(6, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "18837":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(6, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "18838":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(6, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "20002":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(6, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "20003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "20004":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "20005":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30001":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 8).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30004":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30005":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30007":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "30009":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(6, 9).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30010":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "30011":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "30012":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 8).addZdHurtAm(9, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "35932":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 5)
                            .addZdMzAm(16, 21).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "40001":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(16, 21).addZdHurtAm(8, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "40008":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "40010":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 9).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "50002":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(4, 9).addZdHurtAm(8, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "50003":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 9).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "50004":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "50005":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "50008":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 11).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        return md;
                    }
                case "60002":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 11).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "60003":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 11).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        return md;
                    }
                case "60005":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(20, 27)
                            .addZdMzAm(7, 14).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        return md;
                    }
                case "60006":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(7, 14).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "60007":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(7, 14).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "60009":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(7, 14).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "60010":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 15).addZdHurtAm(15, 18).addZdSdAm(18, 21);
                        return md;
                    }
                case "60012":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(6, 15).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "60013":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(6, 15).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "70001":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(6, 15).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "70003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "70006":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "70007":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "80002":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "80003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "80006":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(4, 8).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "80007":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(3, 14)
                            .addZdMzAm(14, 19).addZdHurtAm(19, 22).addZdSdAm(22, 25);
                        return md;
                    }
                case "80008":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(14, 19).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "90002":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(3, 7)
                            .addZdMzAm(7, 14).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        return md;
                    }
                case "90003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(7, 14).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "100001":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "100003":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "100004":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "100005":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "100006":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs02"://骷髅
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdMzAm(15, 18).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "bs03"://魔尊
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs04"://百副boss
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(18, 25)
                            .addZdMzAm(5, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs05"://树妖
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 12).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs06"://枪兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "bs07"://暗红大斧兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs08"://银头蓝眼刀兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdMzAm(16, 20).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "bs09"://暗紫骑马兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 11).addZdMzAm(18, 23).addZdHurtAm(11, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs10"://枪
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 10).addZdMzAm(16, 22).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "bs11"://白发刀
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 10).addZdMzAm(16, 21).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "bs15"://猿猴
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 11).addZdMzAm(18, 23).addZdHurtAm(11, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs16"://白头刀兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 12).addZdMzAm(18, 22).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        return md;
                    }
                case "bs17"://宝莲灯
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(18, 19)
                            .addZdMzAm(4, 12).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs19"://饕餮
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(4, 12).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs20"://狐狸
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 12).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs20_1"://狐狸
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 12).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs20(2)"://狐狸(x8)
                    {
                        md.addCgPwd("bs20(2)_2")
                          .addChangguiAef("bs20(2)");
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 12).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs21"://海盗
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdMzAm(16, 19).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "bs22"://骨头法师
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdMzAm(14, 18).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "bs23"://双刀骨头兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdMzAm(15, 18).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "bs24"://蓝老头
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(4, 9).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs25"://琴王
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(4, 9).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs26"://牛女
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(17, 22)
                            .addZdMzAm(4, 9).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "bs27"://尘老头
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 16).addZdMzAm(22, 26).addZdHurtAm(16, 19).addZdSdAm(19, 22);
                        return md;
                    }
                case "cw_sj"://青蛇
                    {
                        md.addPwdToAll(amKey, 4)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 13).addZdSklAm(19, 25)
                            .addZdMzAm(5, 16).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "cw_sj_1"://青蛇
                    {
                        md.addPwdToAll(amKey, 4)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 13).addZdSklAm(19, 25)
                            .addZdMzAm(5, 16).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "cw_sj2"://青蛇
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(6, 12).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 16).addZdMzAm(22, 26).addZdHurtAm(16, 19).addZdSdAm(19, 22);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "cw_xlr"://青龙
                    {
                        md.addPwdToAll(amKey, 5)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(6, 12).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(13, 19).addZdSklAm(19, 28)
                            .addZdMzAm(5, 16).addZdHurtAm(7, 10).addZdSdAm(10, 13);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "cw_xlr_1"://青龙
                    {
                        md.addPwdToAll(amKey, 5)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(6, 12).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(13, 19).addZdSklAm(19, 28)
                            .addZdMzAm(5, 16).addZdHurtAm(7, 10).addZdSdAm(10, 13);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "cw_xlrsj"://青龙
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 16).addZdHurtAm(16, 19).addZdSdAm(19, 22);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "entrance"://传送点
                    {
                        md.addCgPwd("dituzhuanjie")
                          .addZhandouAef(amKey).addChangguiAef("dituzhuanjie", "_aef").addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 16).addZdHurtAm(16, 19).addZdSdAm(19, 22);
                        return md;
                    }
                case "gw_gcx"://龟丞相
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(5, 16).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "gw_kuangshi"://矿石
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 6).addZdSklAm(3, 6)
                            .addZdMzAm(6, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        return md;
                    }
                case "gw_shuyao"://树妖
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(3, 14)
                            .addZdMzAm(6, 13).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "NPC57_gw"://道家门主
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 15).addZdSklAm(3, 15)
                            .addZdMzAm(6, 13).addZdHurtAm(15, 18).addZdSdAm(18, 21);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "NPC58_gw"://墨家门主
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(9, 14).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        return md;
                    }
                case "NPC59_gw"://阴阳家门主
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(8, 16).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(10, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        return md;
                    }
                case "paopao"://拾取物
                    {
                        md.addPwdToAll(amKey, 1)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(10, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        return md;
                    }
                case "pet_zr_sj"://帝君x8
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(10, 13).addZdHurtAm(13, 16).addZdSdAm(16, 19);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "sc01"://白头斧兵
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "sc02"://
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "sjbs"://世界boss
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(4, 8).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "slm"://洪荒boss
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(4, 8).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty21":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 7).addZdSklAm(3, 7)
                            .addZdMzAm(4, 8).addZdHurtAm(7, 10).addZdSdAm(10, 13);
                        return md;
                    }
                case "ty22":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "ty23":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty24":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty25":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "ty26":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty27":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty28":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "ty29":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "ty30":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty31":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty32":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(5, 10).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty33":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "ty34":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "ty35":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "ty36":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "ty37":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(5, 10).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty38":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 12).addZdSklAm(3, 12)
                            .addZdMzAm(5, 10).addZdHurtAm(12, 15).addZdSdAm(15, 18);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty39":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(3, 14)
                            .addZdMzAm(5, 10).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "ty40":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(3, 6).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xueren":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz01":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(5, 10).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz02":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "xz03":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(5, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz04":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(5, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz05":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(5, 8).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz06":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "xz07":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(5, 10).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz08"://顽猴
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 11).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        return md;
                    }
                case "xz09":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 8).addZdSklAm(3, 8)
                            .addZdMzAm(5, 11).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz10":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 8).addZdHurtAm(8, 11).addZdSdAm(11, 14);
                        return md;
                    }
                case "xz11":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz12":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz13":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 11).addZdSklAm(3, 11)
                            .addZdMzAm(4, 8).addZdHurtAm(11, 14).addZdSdAm(14, 17);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz14":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 10).addZdSklAm(3, 10)
                            .addZdMzAm(4, 8).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "xz15":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "xz16":
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 5).addZdSklAm(3, 5)
                            .addZdMzAm(5, 10).addZdHurtAm(10, 13).addZdSdAm(13, 16);
                        return md;
                    }
                case "xz17":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 4).addZdSklAm(3, 4)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        return md;
                    }
                case "xz18":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 9).addZdSklAm(3, 9)
                            .addZdMzAm(4, 9).addZdHurtAm(9, 12).addZdSdAm(12, 15);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }
                case "zms_sg":
                    {
                        md.addPwdToAll(amKey, 2)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(4, 8).addZdIdleAm(0, 3)
                            .addZdSklAm(3, 14).addZdSklAm(3, 14)
                            .addZdMzAm(4, 9).addZdHurtAm(14, 17).addZdSdAm(17, 20);
                        md.setSimpleFightIsNear(false);
                        return md;
                    }

                default:
                    {
                        md.addPwdToAll(amKey, 3)
                          .addZhandouAef(amKey).addChangguiAef(amKey).addTouxiangAef(amKey);
                        md.addHeadIdleAm(1, 2).addCgIdleAm(0, 4).addZdIdleAm(0, 4)
                            .addZdSklAm(4, 6).addZdSklAm(4, 6)
                            .addZdMzAm(6, 10).addZdHurtAm(10, 13).addZdSdAm(13, 15);
                        return md;
                    }
            }

        }
    }
    public class AmModelMsg : ModelMsg
    {
        public string amKey;
        public AmModelMsg(string amKey)
        {
            this.amKey = amKey;
        }

        public AmModelMsg addPwdToAll(string amKey, int num)
        {
            List<string> pwds = new List<string>();
            for (int i = 1; i <= num; i++)
            {
                pwds.Add(amKey + "_" + i + "_pwd");
            }
            this.changguiPwds = pwds.ToArray();
            this.touxiangPwds = changguiPwds;
            this.zhandouPwds = changguiPwds;
            return this;
        }
        public AmModelMsg addCgPwd(params string[] pwds)
        {
            for (int i = 0; i < pwds.Length; i++)
            {
                string pwd = pwds[i];
                if (!pwd.Contains("_pwd")) pwd += "_pwd";
                pwds[i] = pwd;
            }
            this.changguiPwds = pwds;
            return this;
        }
        public AmModelMsg addZdPwd(params string[] pwds)
        {
            for (int i = 0; i < pwds.Length; i++)
            {
                string pwd = pwds[i];
                if (!pwd.Contains("_pwd")) pwd += "_pwd";
                pwds[i] = pwd;
            }
            this.zhandouPwds = pwds;
            return this;
        }
        public AmModelMsg addTxPwd(params string[] pwds)
        {
            for (int i = 0; i < pwds.Length; i++)
            {
                string pwd = pwds[i];
                if (!pwd.Contains("_pwd")) pwd += "_pwd";
                pwds[i] = pwd;
            }
            this.touxiangPwds = pwds;
            return this;
        }
        public AmModelMsg addSjtxPwd(params string[] pwds)
        {
            for (int i = 0; i < pwds.Length; i++)
            {
                string pwd = pwds[i];
                if (!pwd.Contains("_pwd")) pwd += "_pwd";
                pwds[i] = pwd;
            }
            this.zdSjtxPwds = pwds;
            return this;
        }
        public AmModelMsg addZhandouAef(string zhandouAef, string suf = "_zhandou_aef")
        {
            this.zhandouAef = zhandouAef + suf;
            return this;
        }
        public AmModelMsg addChangguiAef(string changguiAef, string suf = "_changgui_aef")
        {
            this.changguiAef = changguiAef + suf;
            return this;
        }
        public AmModelMsg addTouxiangAef(string touxiangAef, string suf = "_touxiang_aef")
        {
            this.touxiangAef = touxiangAef + suf;
            return this;
        }
        public AmModelMsg addSjtxAef(string zdSjtxAef, string suf = "_aef")
        {
            this.zdSjtxAef = zdSjtxAef + suf;
            return this;
        }
    }

}
