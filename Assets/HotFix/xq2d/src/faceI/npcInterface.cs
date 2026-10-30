using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.model;
using Assets.Res.script.src.model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface npcInterface
    {
        public NpcObjData getNpc(string key);
        public JArray getNpcTaskAndProgressList(string npcKey);
    }
    public class npcInterfaceImpl : npcInterface
    {
        /**获取npc相关任务、进度列表 */
        public JArray getNpcTaskAndProgressList(string npcKey)
        {
            //需要任务目标为npcKey的任务，并且状态在1-3
            JArray a1 = this.getNpcTaskList(npcKey);
            //获取已开启的活动
            JArray a2 = this.getNpcActivity(npcKey);
            a1.Merge(a2);
            return a1;
        }
        /**获取npc相关已开启的活动 */
        private JArray getNpcActivity(string npcKey)
        {
            JArray arr = new JArray();
            //需要增加活动接口，跟任务分离，同一个活动可以绑定多个npc入口（任务只能一对一）
            List<string> acKeys = this.getNpc(npcKey).acKeys;
            if (acKeys == null) return arr;
            foreach (string k in acKeys)
            {
                List<MultyMenu> ms = face.activityInterface.getAcMenus(k, npcKey);
                if (ms == null) continue;
                for (int i = 0; i < ms.Count; i++)
                {
                    JObject item = new JObject();
                    item.Add("key", k);
                    item.Add("name", ms[i].name);
                    item.Add("type", 2);
                    item.Add("index", i);
                    arr.Add(item);
                }
            }
            return arr;
        }

        /**获取npc相关的任务(非活动) */
        private JArray getNpcTaskList(string nKey)
        {
            JArray arr = new JArray();

            JArray list = face.taskInterface.getTaskListFromCache();
            for (int i = 0; i < list.Count; i++)
            {
                JObject tk = (JObject)list[i];
                int status = (int)tk["status"];
                if (status == 0) continue;
                string npcKey = null;
                task ts = face.taskInterface.getTask(tk["key"].ToString());
                if (status == 1)//可接取状态的npc
                {
                    npcKey = ts.getNpcKey;
                }
                else if (status == 2)//进行中，需要按进度来获取相应的npc
                {
                    int progressIndex = (int)tk["progressIndex"];
                    JObject progress = (JObject)ts.progressList[progressIndex];
                    JObject target = (JObject)progress["target"];
                    npcKey = target["toKey"].ToString();
                }
                else if (status == 3)//已完成（可提交）
                {
                    npcKey = ts.subNpcKey;
                }

                if (nKey.Equals(npcKey))
                {
                    int index = (int)tk["progressIndex"];
                    JObject progress = (JObject)ts.progressList[index];
                    JObject item = new JObject();
                    item.Add("key", ts.key);
                    item.Add("name", "（任）" + progress["name"].ToString());
                    item.Add("type", ts.type);
                    arr.Add(item);
                }

            }
            return arr;
        }
        public NpcObjData getNpc(string key)
        {
            NpcObjData npcObjData = new NpcObjData(key);
            switch (key)
            {
                case "10000000":
                    {
                        npcObjData.init(2, "村长", "npc14_png");
                        return npcObjData;
                    }
                case "10000001":
                    {
                        npcObjData.name = "储物箱";
                        npcObjData.picPath = "npc56_png";
                        return npcObjData;
                    }
                case "10000002":
                    {
                        npcObjData.name = "村医扁老鹊";
                        npcObjData.picPath = "npc_cyblq_png";
                        return npcObjData;
                    }
                case "10000003":
                    {
                        npcObjData.name = "展护卫";
                        npcObjData.picPath = "npc_zhw_png";
                        return npcObjData;
                    }
                case "10000004":
                    {
                        npcObjData.name = "兽医华小佗";
                        npcObjData.picPath = "npc_syhxt_png";
                        return npcObjData;
                    }
                case "10000005":
                    {
                        npcObjData.name = "清溪子";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000006":
                    {
                        npcObjData.name = "密保大使";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000007":
                    {
                        npcObjData.name = "道家接引师";
                        npcObjData.picPath = "npc13_png";
                        npcObjData.addAcKeys("toMenPai");
                        return npcObjData;
                    }
                case "10000008":
                    {
                        npcObjData.name = "阴阳接引师";
                        npcObjData.picPath = "npc07_png";
                        npcObjData.addAcKeys("toMenPai");
                        return npcObjData;
                    }
                case "10000009":
                    {
                        npcObjData.name = "墨家接引师";
                        npcObjData.picPath = "npc33_png";
                        npcObjData.addAcKeys("toMenPai");
                        return npcObjData;
                    }
                case "10000010":
                    {
                        npcObjData.name = "赵村村长";
                        npcObjData.picPath = "npc_qxz_png";
                        return npcObjData;
                    }
                case "10000011":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000012":
                    {
                        npcObjData.name = "萧恨水";
                        npcObjData.picPath = "npc_xhs_png";
                        return npcObjData;
                    }
                case "10000013":
                    {
                        npcObjData.name = "韩信";
                        npcObjData.picPath = "npc_hx_png";
                        return npcObjData;
                    }
                case "10000014":
                    {
                        npcObjData.name = "装备商人";
                        npcObjData.picPath = "npc15_png";
                        npcObjData.addAcKeys("zbsr");
                        return npcObjData;
                    }
                case "10000015":
                    {
                        npcObjData.name = "仓库管理员";
                        npcObjData.picPath = "npc_ckgly_png";
                        npcObjData.addAcKeys("ckgl");
                        return npcObjData;
                    }
                case "10000016":
                    {
                        npcObjData.name = "寄卖商人";
                        npcObjData.picPath = "npc_jmsr_png";
                        npcObjData.addAcKeys("jmsr");
                        return npcObjData;
                    }
                case "10000017":
                    {
                        npcObjData.name = "宠物商人";
                        npcObjData.picPath = "cwqh";
                        npcObjData.addAcKeys("cwsr");
                        return npcObjData;
                    }
                case "10000018":
                    {
                        npcObjData.name = "白素";
                        npcObjData.picPath = "npc_bs_png";
                        return npcObjData;
                    }
                case "10000019":
                    {
                        npcObjData.name = "恶人谷接引员";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000020":
                    {
                        npcObjData.name = "期货总管";
                        npcObjData.picPath = "npc_qhzg_png";
                        npcObjData.addAcKeys("2029");
                        return npcObjData;
                    }
                case "10000021":
                    {
                        npcObjData.name = "宝图大使";
                        npcObjData.picPath = "npc25_png";
                        npcObjData.addAcKeys("wzyc");
                        return npcObjData;
                    }
                case "10000022":
                    {
                        npcObjData.name = "宠物强化师";
                        npcObjData.picPath = "npc_cwsr_png";
                        npcObjData.addAcKeys("cwqh");
                        return npcObjData;
                    }
                case "10000023":
                    {
                        npcObjData.name = "斗宠小郡主";
                        npcObjData.picPath = "npc20_png";
                        npcObjData.addAcKeys("douchong");
                        return npcObjData;
                    }
                case "10000024":
                    {
                        npcObjData.name = "期货大盘商人";
                        npcObjData.picPath = "npc_qhdpsr_png";
                        npcObjData.addAcKeys("2000");
                        return npcObjData;
                    }
                case "10000025":
                    {
                        npcObjData.name = "不倦先生";
                        npcObjData.picPath = "npc_bjxs_png";
                        npcObjData.addAcKeys("bjxs");
                        return npcObjData;
                    }
                case "10000026":
                    {
                        npcObjData.name = "萧恨水";
                        npcObjData.picPath = "npc_xhs_png";
                        return npcObjData;
                    }
                case "10000027":
                    {
                        npcObjData.name = "孔夫子";
                        npcObjData.picPath = "npc_kfz_png";
                        npcObjData.addAcKeys("shitu");
                        return npcObjData;
                    }
                case "10000028":
                    {
                        npcObjData.name = "排行榜管理员";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("phb");
                        return npcObjData;
                    }
                case "10000029":
                    {
                        npcObjData.name = "密保大使";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000030":
                    {
                        npcObjData.name = "擂台报名员";
                        npcObjData.picPath = "npc33_png";
                        npcObjData.addAcKeys("yxl");
                        return npcObjData;
                    }
                case "10000031":
                    {
                        npcObjData.name = "节日活动大使";
                        npcObjData.picPath = "jrhdds_NPC";
                        npcObjData.addAcKeys("jrhd", "shenjiduihuan", "vipGift");
                        return npcObjData;
                    }
                case "10000032":
                    {
                        npcObjData.name = "武状元";
                        npcObjData.picPath = "npc35_png";
                        npcObjData.addAcKeys("2020");
                        return npcObjData;
                    }
                case "10000033":
                    {
                        npcObjData.name = "兽神分身";
                        npcObjData.picPath = "shoushen";
                        npcObjData.addAcKeys("ssfs");
                        return npcObjData;
                    }
                case "10000034":
                    {
                        npcObjData.name = "寻秦夫人";
                        npcObjData.picPath = "NPC_xqfr_png";
                        return npcObjData;
                    }
                case "10000035":
                    {
                        npcObjData.name = "盗梦空间";
                        npcObjData.picPath = "npc43_png";
                        npcObjData.addAcKeys("dmkj");
                        return npcObjData;
                    }
                case "10000036":
                    {
                        npcObjData.name = "装备锻造师";
                        npcObjData.picPath = "npc_zbdzs_png";
                        npcObjData.addAcKeys("dzs");
                        return npcObjData;
                    }
                case "10000037":
                    {
                        npcObjData.name = "装备精练师";
                        npcObjData.picPath = "npc_zbjls_png";
                        npcObjData.addAcKeys("jls");
                        return npcObjData;
                    }
                case "10000038":
                    {
                        npcObjData.name = "装备镶嵌师";
                        npcObjData.picPath = "npc_zbxqs_png";
                        npcObjData.addAcKeys("xqs");
                        return npcObjData;
                    }
                case "10000039":
                    {
                        npcObjData.name = "装备绑定师";
                        npcObjData.picPath = "npc_zbbds_png";
                        npcObjData.addAcKeys("bds");
                        return npcObjData;
                    }
                case "10000040":
                    {
                        npcObjData.name = "装备潜力师";
                        npcObjData.picPath = "npc_zbqls_png";
                        npcObjData.addAcKeys("qls");
                        return npcObjData;
                    }
                case "10000041":
                    {
                        npcObjData.name = "曲琛";
                        npcObjData.picPath = "npc41_png";
                        return npcObjData;
                    }
                case "10000042":
                    {
                        npcObjData.name = "花渐离";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000043":
                    {
                        npcObjData.name = "生活技能大师";
                        npcObjData.picPath = "npc_shjnds_png";
                        npcObjData.addAcKeys("shjn");
                        return npcObjData;
                    }
                case "10000044":
                    {
                        npcObjData.name = "保钓大使";
                        npcObjData.picPath = "npc_qw_png";
                        return npcObjData;
                    }
                case "10000045":
                    {
                        npcObjData.name = "菩提老祖";
                        npcObjData.picPath = "NPC_fbhf";
                        npcObjData.addAcKeys("ptlz");
                        return npcObjData;
                    }
                case "10000046":
                    {
                        npcObjData.name = "洪荒宝库";
                        npcObjData.picPath = "20226_png";
                        npcObjData.addAcKeys("2021");
                        return npcObjData;
                    }
                case "10000047":
                    {
                        npcObjData.name = "日常活动大使";
                        npcObjData.picPath = "npc_rchdds_png";
                        npcObjData.addAcKeys("2002", "2003", "2005", "2007", "2012", "2022", "2025", "2028", "2030", "zjcm");
                        return npcObjData;
                    }
                case "10000048":
                    {
                        npcObjData.name = "休闲活动大使";
                        npcObjData.picPath = "npc_xxhdds_png";
                        npcObjData.addAcKeys("2013", "2017", "2032", "2033", "2034");
                        return npcObjData;
                    }
                case "10000049":
                    {
                        npcObjData.name = "主城传送师";
                        npcObjData.picPath = "npc_css_png";
                        npcObjData.addAcKeys("chuansong");
                        return npcObjData;
                    }
                case "10000050":
                    {
                        npcObjData.name = "帮派管理员";
                        npcObjData.picPath = "npc_bpgly_png";
                        npcObjData.addAcKeys("bpgl");
                        return npcObjData;
                    }
                case "10000051":
                    {
                        npcObjData.name = "监狱传送师";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("jycs");
                        return npcObjData;
                    }
                case "10000052":
                    {
                        npcObjData.name = "除魔军统领";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000053":
                    {
                        npcObjData.name = "韩信";
                        npcObjData.picPath = "npc_hx_png";
                        return npcObjData;
                    }
                case "10000054":
                    {
                        npcObjData.name = "张良";
                        npcObjData.picPath = "npc_zl_png";
                        return npcObjData;
                    }
                case "10000055":
                    {
                        npcObjData.name = "门派传送师";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000056":
                    {
                        npcObjData.name = "百战千军";
                        npcObjData.picPath = "NPC_rxwj_png";
                        npcObjData.addAcKeys("2035");
                        return npcObjData;
                    }
                case "10000057":
                    {
                        npcObjData.name = "斗战封神榜";
                        npcObjData.picPath = "dzfsb";
                        npcObjData.addAcKeys("dzfsb");
                        return npcObjData;
                    }
                case "10000058":
                    {
                        npcObjData.name = "戚薇";
                        npcObjData.picPath = "npc_qw_png";
                        return npcObjData;
                    }
                case "10000059":
                    {
                        npcObjData.name = "见影";
                        npcObjData.picPath = "npc42_png";
                        return npcObjData;
                    }
                case "10000060":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000061":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000062":
                    {
                        npcObjData.name = "陆风";
                        npcObjData.picPath = "npc39";
                        return npcObjData;
                    }
                case "10000063":
                    {
                        npcObjData.name = "吴小弟";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000064":
                    {
                        npcObjData.name = "吴月越";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000065":
                    {
                        npcObjData.name = "吴三娘";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000066":
                    {
                        npcObjData.name = "刘大刀";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000067":
                    {
                        npcObjData.name = "邯郸怪人";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000068":
                    {
                        npcObjData.name = "白老怪";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000069":
                    {
                        npcObjData.name = "金多多";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000070":
                    {
                        npcObjData.name = "金绍绍";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000071":
                    {
                        npcObjData.name = "酒泉子";
                        npcObjData.picPath = "npc_jqz_png";
                        return npcObjData;
                    }
                case "10000072":
                    {
                        npcObjData.name = "木道人";
                        npcObjData.picPath = "npc_mdr_png";
                        return npcObjData;
                    }
                case "10000073":
                    {
                        npcObjData.name = "林天明";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000074":
                    {
                        npcObjData.name = "林大娘";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000075":
                    {
                        npcObjData.name = "文心燕";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000076":
                    {
                        npcObjData.name = "陈道瑜";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000077":
                    {
                        npcObjData.name = "刘捕头";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000078":
                    {
                        npcObjData.name = "薛贾";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000079":
                    {
                        npcObjData.name = "李峰";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000080":
                    {
                        npcObjData.name = "怪盗一枝花";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000081":
                    {
                        npcObjData.name = "刘成君";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000082":
                    {
                        npcObjData.name = "李利";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000083":
                    {
                        npcObjData.name = "龙啸天";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000084":
                    {
                        npcObjData.name = "唐甜甜";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000085":
                    {
                        npcObjData.name = "范佩佩";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000086":
                    {
                        npcObjData.name = "范小弟";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000087":
                    {
                        npcObjData.name = "郭大娘";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000088":
                    {
                        npcObjData.name = "唐少渊";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000089":
                    {
                        npcObjData.name = "士兵首领";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000090":
                    {
                        npcObjData.name = "温远";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000091":
                    {
                        npcObjData.name = "郭道长";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000092":
                    {
                        npcObjData.name = "古城守卫（50级副本）";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000093":
                    {
                        npcObjData.name = "宁超";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000094":
                    {
                        npcObjData.name = "凤青青";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000095":
                    {
                        npcObjData.name = "黄石公";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000096":
                    {
                        npcObjData.name = "凤京";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000097":
                    {
                        npcObjData.name = "郑乾";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000098":
                    {
                        npcObjData.name = "陈大民";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000099":
                    {
                        npcObjData.name = "荆无忌";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000100":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000101":
                    {
                        npcObjData.name = "吴三富";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000102":
                    {
                        npcObjData.name = "楚月月";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000103":
                    {
                        npcObjData.name = "黄珊珊";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000104":
                    {
                        npcObjData.name = "江三道";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000105":
                    {
                        npcObjData.name = "马涛";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000106":
                    {
                        npcObjData.name = "黄老大";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000107":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000108":
                    {
                        npcObjData.name = "李由";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000109":
                    {
                        npcObjData.name = "周带孝";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000110":
                    {
                        npcObjData.name = "周旭龙";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000111":
                    {
                        npcObjData.name = "黄来驰";
                        npcObjData.picPath = "npc36_png";
                        return npcObjData;
                    }
                case "10000112":
                    {
                        npcObjData.name = "刘贺才";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000113":
                    {
                        npcObjData.name = "周伏凰";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000114":
                    {
                        npcObjData.name = "除魔军统领";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000115":
                    {
                        npcObjData.name = "扶苏";
                        npcObjData.picPath = "npc_fs_png";
                        return npcObjData;
                    }
                case "10000116":
                    {
                        npcObjData.name = "李御风";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000117":
                    {
                        npcObjData.name = "齐梅梅";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000118":
                    {
                        npcObjData.name = "李乘风";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000119":
                    {
                        npcObjData.name = "李如风";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000120":
                    {
                        npcObjData.name = "大善大恶使者";
                        npcObjData.picPath = "npc18_png";
                        npcObjData.addAcKeys("seb");
                        return npcObjData;
                    }
                case "10000121":
                    {
                        npcObjData.name = "戚薇";
                        npcObjData.picPath = "npc_qw_png";
                        return npcObjData;
                    }
                case "10000122":
                    {
                        npcObjData.name = "除魔军统领";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000123":
                    {
                        npcObjData.name = "除魔军士卒";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000124":
                    {
                        npcObjData.name = "香香";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000125":
                    {
                        npcObjData.name = "呼延灼";
                        npcObjData.picPath = "npc33_png";
                        return npcObjData;
                    }
                case "10000126":
                    {
                        npcObjData.name = "木云香";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000127":
                    {
                        npcObjData.name = "戴蒙";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000128":
                    {
                        npcObjData.name = "楚军士兵";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000129":
                    {
                        npcObjData.name = "刘邦";
                        npcObjData.picPath = "npc_lb_png";
                        return npcObjData;
                    }
                case "10000130":
                    {
                        npcObjData.name = "汉军俘虏";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000131":
                    {
                        npcObjData.name = "宋成仙";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000132":
                    {
                        npcObjData.name = "程陵";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000133":
                    {
                        npcObjData.name = "高诚信";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000134":
                    {
                        npcObjData.name = "季礼";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000135":
                    {
                        npcObjData.name = "木云芝";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000136":
                    {
                        npcObjData.name = "汉军伤兵";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000137":
                    {
                        npcObjData.name = "楚军谋士";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000138":
                    {
                        npcObjData.name = "项羽";
                        npcObjData.picPath = "npc43_png";
                        return npcObjData;
                    }
                case "10000139":
                    {
                        npcObjData.name = "楚军将领";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000140":
                    {
                        npcObjData.name = "杜师师";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000141":
                    {
                        npcObjData.name = "胡姬";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000142":
                    {
                        npcObjData.name = "李太白";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000143":
                    {
                        npcObjData.name = "刘菁";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000144":
                    {
                        npcObjData.name = "宋修仙";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000145":
                    {
                        npcObjData.name = "镇守士兵（60级副本）";
                        npcObjData.picPath = "npc32_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000146":
                    {
                        npcObjData.name = "邱余岩";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000147":
                    {
                        npcObjData.name = "虎大霸";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000148":
                    {
                        npcObjData.name = "毛不平";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000149":
                    {
                        npcObjData.name = "铁公鸡";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000150":
                    {
                        npcObjData.name = "包王孙";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000151":
                    {
                        npcObjData.name = "白素";
                        npcObjData.picPath = "npc_bs_png";
                        return npcObjData;
                    }
                case "10000152":
                    {
                        npcObjData.name = "萧恨水";
                        npcObjData.picPath = "npc_xhs_png";
                        return npcObjData;
                    }
                case "10000153":
                    {
                        npcObjData.name = "杨助";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000154":
                    {
                        npcObjData.name = "楚鸿飞";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000155":
                    {
                        npcObjData.name = "萧禾";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000156":
                    {
                        npcObjData.name = "段正德";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000157":
                    {
                        npcObjData.name = "陆欣欣";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000158":
                    {
                        npcObjData.name = "庞非道";
                        npcObjData.picPath = "npc33_png";
                        return npcObjData;
                    }
                case "10000159":
                    {
                        npcObjData.name = "陈心原";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000160":
                    {
                        npcObjData.name = "韩凌";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000161":
                    {
                        npcObjData.name = "蛇女";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000162":
                    {
                        npcObjData.name = "王利发";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000163":
                    {
                        npcObjData.name = "陈平";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000164":
                    {
                        npcObjData.name = "宋词词";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000165":
                    {
                        npcObjData.name = "唐诗诗";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000166":
                    {
                        npcObjData.name = "秦斐";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000167":
                    {
                        npcObjData.name = "杨泉";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000168":
                    {
                        npcObjData.name = "樊哙";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000169":
                    {
                        npcObjData.name = "蒋天成";
                        npcObjData.picPath = "npc33_png";
                        return npcObjData;
                    }
                case "10000170":
                    {
                        npcObjData.name = "关兴平";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000171":
                    {
                        npcObjData.name = "何里活";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000172":
                    {
                        npcObjData.name = "孟长胜";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000173":
                    {
                        npcObjData.name = "蒋小弟";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000174":
                    {
                        npcObjData.name = "沈忠胜";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000175":
                    {
                        npcObjData.name = "合添一";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000176":
                    {
                        npcObjData.name = "常义光";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000177":
                    {
                        npcObjData.name = "魏幸之";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000178":
                    {
                        npcObjData.name = "彭越";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000179":
                    {
                        npcObjData.name = "吕牧丰";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000180":
                    {
                        npcObjData.name = "连壁";
                        npcObjData.picPath = "npc27_png";
                        return npcObjData;
                    }
                case "10000181":
                    {
                        npcObjData.name = "屠殷";
                        npcObjData.picPath = "npc18_png";
                        return npcObjData;
                    }
                case "10000182":
                    {
                        npcObjData.name = "傅宗颜";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000183":
                    {
                        npcObjData.name = "见影";
                        npcObjData.picPath = "npc42_png";
                        return npcObjData;
                    }
                case "10000184":
                    {
                        npcObjData.name = "韩信";
                        npcObjData.picPath = "npc_hx_png";
                        return npcObjData;
                    }
                case "10000185":
                    {
                        npcObjData.name = "贺然";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000186":
                    {
                        npcObjData.name = "合大娘";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000187":
                    {
                        npcObjData.name = "丘机";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000188":
                    {
                        npcObjData.name = "郝钰";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000189":
                    {
                        npcObjData.name = "孙二";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000190":
                    {
                        npcObjData.name = "汉军将领";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000191":
                    {
                        npcObjData.name = "萧何";
                        npcObjData.picPath = "npc_xh_png";
                        return npcObjData;
                    }
                case "10000192":
                    {
                        npcObjData.name = "吴一水";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000193":
                    {
                        npcObjData.name = "薛红娘";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000194":
                    {
                        npcObjData.name = "卢未来";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000195":
                    {
                        npcObjData.name = "公孙大娘";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000196":
                    {
                        npcObjData.name = "蒋历史";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000197":
                    {
                        npcObjData.name = "樊华慧";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000198":
                    {
                        npcObjData.name = "席眸";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000199":
                    {
                        npcObjData.name = "石泉";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000200":
                    {
                        npcObjData.name = "金虹";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000201":
                    {
                        npcObjData.name = "曲琛";
                        npcObjData.picPath = "npc41_png";
                        return npcObjData;
                    }
                case "10000202":
                    {
                        npcObjData.name = "项少龙";
                        npcObjData.picPath = "npc_xsl_png";
                        return npcObjData;
                    }
                case "10000203":
                    {
                        npcObjData.name = "花渐离";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000204":
                    {
                        npcObjData.name = "张良";
                        npcObjData.picPath = "npc_zl_png";
                        return npcObjData;
                    }
                case "10000205":
                    {
                        npcObjData.name = "诸葛玄衣（70级副本）";
                        npcObjData.picPath = "npc31_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000206":
                    {
                        npcObjData.name = "师安";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000207":
                    {
                        npcObjData.name = "王冥";
                        npcObjData.picPath = "npc05_png";
                        return npcObjData;
                    }
                case "10000208":
                    {
                        npcObjData.name = "武笑笑";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000209":
                    {
                        npcObjData.name = "司空美雪";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000210":
                    {
                        npcObjData.name = "慕容祖贤";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000211":
                    {
                        npcObjData.name = "尉迟嘉玲";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000212":
                    {
                        npcObjData.name = "守路人";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000213":
                    {
                        npcObjData.name = "司徒吉";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000214":
                    {
                        npcObjData.name = "拓跋秀";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000215":
                    {
                        npcObjData.name = "南宫白";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000216":
                    {
                        npcObjData.name = "守谷者";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000217":
                    {
                        npcObjData.name = "义天";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000218":
                    {
                        npcObjData.name = "郭四";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000219":
                    {
                        npcObjData.name = "阿蛮";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000220":
                    {
                        npcObjData.name = "小兵";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000221":
                    {
                        npcObjData.name = "陆风";
                        npcObjData.picPath = "npc39";
                        return npcObjData;
                    }
                case "10000222":
                    {
                        npcObjData.name = "鬼厨";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000223":
                    {
                        npcObjData.name = "义云";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000224":
                    {
                        npcObjData.name = "剑灵";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000225":
                    {
                        npcObjData.name = "城将";
                        npcObjData.picPath = "npc02_png";
                        return npcObjData;
                    }
                case "10000226":
                    {
                        npcObjData.name = "大成殿守卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000227":
                    {
                        npcObjData.name = "大成殿殿主";
                        npcObjData.picPath = "npc37";
                        return npcObjData;
                    }
                case "10000228":
                    {
                        npcObjData.name = "年少奇";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000229":
                    {
                        npcObjData.name = "小六";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000230":
                    {
                        npcObjData.name = "成新";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000231":
                    {
                        npcObjData.name = "殷悲";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000232":
                    {
                        npcObjData.name = "王阿公";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000233":
                    {
                        npcObjData.name = "名山守卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000234":
                    {
                        npcObjData.name = "李老伯";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000235":
                    {
                        npcObjData.name = "霍太古";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000236":
                    {
                        npcObjData.name = "茶寮货商";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000237":
                    {
                        npcObjData.name = "月儿";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000238":
                    {
                        npcObjData.name = "义云";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000239":
                    {
                        npcObjData.name = "成新";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000240":
                    {
                        npcObjData.name = "招魂使者";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000241":
                    {
                        npcObjData.name = "薛绍";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000242":
                    {
                        npcObjData.name = "千寻";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000243":
                    {
                        npcObjData.name = "小鬼";
                        npcObjData.picPath = "npc05_png";
                        return npcObjData;
                    }
                case "10000244":
                    {
                        npcObjData.name = "卞凉";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000245":
                    {
                        npcObjData.name = "天子殿护卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000246":
                    {
                        npcObjData.name = "天子殿殿主";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000247":
                    {
                        npcObjData.name = "鬼夫子";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000248":
                    {
                        npcObjData.name = "星璇";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000249":
                    {
                        npcObjData.name = "九命";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000250":
                    {
                        npcObjData.name = "赤炎";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000251":
                    {
                        npcObjData.name = "鬼姑";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000252":
                    {
                        npcObjData.name = "阴阳界守卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000253":
                    {
                        npcObjData.name = "判官";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000254":
                    {
                        npcObjData.name = "乔双鱼";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000255":
                    {
                        npcObjData.name = "燎日";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000256":
                    {
                        npcObjData.name = "水寒月";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000257":
                    {
                        npcObjData.name = "行货商人";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000258":
                    {
                        npcObjData.name = "鬼神卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000259":
                    {
                        npcObjData.name = "鬼紫电";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000260":
                    {
                        npcObjData.name = "独孤绝";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000261":
                    {
                        npcObjData.name = "画儿";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000262":
                    {
                        npcObjData.name = "鸣鸠";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000263":
                    {
                        npcObjData.name = "鬼侍卫";
                        npcObjData.picPath = "npc05_png";
                        return npcObjData;
                    }
                case "10000264":
                    {
                        npcObjData.name = "奈河渡者";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000265":
                    {
                        npcObjData.name = "济月";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000266":
                    {
                        npcObjData.name = "松青";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000267":
                    {
                        npcObjData.name = "小贰";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000268":
                    {
                        npcObjData.name = "王三";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000269":
                    {
                        npcObjData.name = "沈离嫣";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000270":
                    {
                        npcObjData.name = "花渐离";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000271":
                    {
                        npcObjData.name = "郦食其";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000272":
                    {
                        npcObjData.name = "孟婆";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000273":
                    {
                        npcObjData.name = "往世碑（80级副本）";
                        npcObjData.picPath = "ptsb_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000274":
                    {
                        npcObjData.name = "凡行";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000275":
                    {
                        npcObjData.name = "鬼差黑魅";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000276":
                    {
                        npcObjData.name = "曼珠";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000277":
                    {
                        npcObjData.name = "景幽";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000278":
                    {
                        npcObjData.name = "内府守卫";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000279":
                    {
                        npcObjData.name = "黄石公";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000280":
                    {
                        npcObjData.name = "婢笙";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000281":
                    {
                        npcObjData.name = "洛伞";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000282":
                    {
                        npcObjData.name = "莫川";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000283":
                    {
                        npcObjData.name = "南山童子";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000284":
                    {
                        npcObjData.name = "玄灵子";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000285":
                    {
                        npcObjData.name = "四象";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000286":
                    {
                        npcObjData.name = "扇若";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000287":
                    {
                        npcObjData.name = "崔广";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000288":
                    {
                        npcObjData.name = "货郎";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000289":
                    {
                        npcObjData.name = "剑亦";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000290":
                    {
                        npcObjData.name = "官差小白";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000291":
                    {
                        npcObjData.name = "崔广分身";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000292":
                    {
                        npcObjData.name = "朱雀石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000293":
                    {
                        npcObjData.name = "亭长夫人";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000294":
                    {
                        npcObjData.name = "令史";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000295":
                    {
                        npcObjData.name = "亭长";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000296":
                    {
                        npcObjData.name = "啬夫";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000297":
                    {
                        npcObjData.name = "县尉";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000298":
                    {
                        npcObjData.name = "朱饶";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000299":
                    {
                        npcObjData.name = "唐秉";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000300":
                    {
                        npcObjData.name = "捕蛇人";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000301":
                    {
                        npcObjData.name = "程捕头";
                        npcObjData.picPath = "npc24_png";
                        return npcObjData;
                    }
                case "10000302":
                    {
                        npcObjData.name = "陈小欢";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000303":
                    {
                        npcObjData.name = "唐秉分身";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000304":
                    {
                        npcObjData.name = "青龙石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000305":
                    {
                        npcObjData.name = "樊青烈";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000306":
                    {
                        npcObjData.name = "扶秦";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000307":
                    {
                        npcObjData.name = "赵静业";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000308":
                    {
                        npcObjData.name = "铁官长";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000309":
                    {
                        npcObjData.name = "荆无忌";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000310":
                    {
                        npcObjData.name = "陈少川";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000311":
                    {
                        npcObjData.name = "仓吏";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000312":
                    {
                        npcObjData.name = "刘老六";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000313":
                    {
                        npcObjData.name = "郡尉";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000314":
                    {
                        npcObjData.name = "除魔军统领";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000315":
                    {
                        npcObjData.name = "张良";
                        npcObjData.picPath = "npc_zl_png";
                        return npcObjData;
                    }
                case "10000316":
                    {
                        npcObjData.name = "刘邦";
                        npcObjData.picPath = "npc_lb_png";
                        return npcObjData;
                    }
                case "10000317":
                    {
                        npcObjData.name = "项少龙";
                        npcObjData.picPath = "npc_xsl_png";
                        return npcObjData;
                    }
                case "10000318":
                    {
                        npcObjData.name = "李由";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000319":
                    {
                        npcObjData.name = "青丘守卫（90级副本）";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000320":
                    {
                        npcObjData.name = "郭平";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000321":
                    {
                        npcObjData.name = "王将军";
                        npcObjData.picPath = "npc35_png";
                        return npcObjData;
                    }
                case "10000322":
                    {
                        npcObjData.name = "刘副将";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000323":
                    {
                        npcObjData.name = "梅六";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000324":
                    {
                        npcObjData.name = "柳烟";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000325":
                    {
                        npcObjData.name = "吴实";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000326":
                    {
                        npcObjData.name = "华拓";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000327":
                    {
                        npcObjData.name = "孔如鸿";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000328":
                    {
                        npcObjData.name = "令史";
                        npcObjData.picPath = "npc04_png";
                        return npcObjData;
                    }
                case "10000329":
                    {
                        npcObjData.name = "啬夫";
                        npcObjData.picPath = "npc22_png";
                        return npcObjData;
                    }
                case "10000330":
                    {
                        npcObjData.name = "县尉";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000331":
                    {
                        npcObjData.name = "吴实分身";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000332":
                    {
                        npcObjData.name = "玄武石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000333":
                    {
                        npcObjData.name = "秦璇";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000334":
                    {
                        npcObjData.name = "鲁非";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000335":
                    {
                        npcObjData.name = "孙元";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000336":
                    {
                        npcObjData.name = "泉碧先生";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000337":
                    {
                        npcObjData.name = "周术";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000338":
                    {
                        npcObjData.name = "青莲仙子";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000339":
                    {
                        npcObjData.name = "杉风";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000340":
                    {
                        npcObjData.name = "青莲居士";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000341":
                    {
                        npcObjData.name = "袁锋";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000342":
                    {
                        npcObjData.name = "老董";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000343":
                    {
                        npcObjData.name = "小破";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000344":
                    {
                        npcObjData.name = "周术分身";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000345":
                    {
                        npcObjData.name = "白虎石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000346":
                    {
                        npcObjData.name = "威霸天";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000347":
                    {
                        npcObjData.name = "王员";
                        npcObjData.picPath = "npc25_png";
                        return npcObjData;
                    }
                case "10000348":
                    {
                        npcObjData.name = "李铁";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000349":
                    {
                        npcObjData.name = "虎千云";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000350":
                    {
                        npcObjData.name = "先锋官";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000351":
                    {
                        npcObjData.name = "潘红梅";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000352":
                    {
                        npcObjData.name = "混沌碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000353":
                    {
                        npcObjData.name = "四皓化身";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000354":
                    {
                        npcObjData.name = "威雄天";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000355":
                    {
                        npcObjData.name = "潘大夫";
                        npcObjData.picPath = "npc14_png";
                        return npcObjData;
                    }
                case "10000356":
                    {
                        npcObjData.name = "神秘黑影";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000357":
                    {
                        npcObjData.name = "云五行";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000358":
                    {
                        npcObjData.name = "李茂";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000359":
                    {
                        npcObjData.name = "萧恨水";
                        npcObjData.picPath = "npc_xhs_png";
                        return npcObjData;
                    }
                case "10000360":
                    {
                        npcObjData.name = "惠娘";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000361":
                    {
                        npcObjData.name = "高齐";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000362":
                    {
                        npcObjData.name = "焚炎";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000363":
                    {
                        npcObjData.name = "焚冰";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000364":
                    {
                        npcObjData.name = "冯在行";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000365":
                    {
                        npcObjData.name = "许文";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000366":
                    {
                        npcObjData.name = "潘红梅";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000367":
                    {
                        npcObjData.name = "除魔军统领";
                        npcObjData.picPath = "npc32_png";
                        return npcObjData;
                    }
                case "10000368":
                    {
                        npcObjData.name = "除魔军东将";
                        npcObjData.picPath = "npc35_png";
                        return npcObjData;
                    }
                case "10000369":
                    {
                        npcObjData.name = "万三";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000370":
                    {
                        npcObjData.name = "风自行";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000371":
                    {
                        npcObjData.name = "小砂子";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000372":
                    {
                        npcObjData.name = "吕牙子";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000373":
                    {
                        npcObjData.name = "伯期";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000374":
                    {
                        npcObjData.name = "除魔军南将";
                        npcObjData.picPath = "npc35_png";
                        return npcObjData;
                    }
                case "10000375":
                    {
                        npcObjData.name = "石柴";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000376":
                    {
                        npcObjData.name = "小雨";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000377":
                    {
                        npcObjData.name = "灵封";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000378":
                    {
                        npcObjData.name = "徐福";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000379":
                    {
                        npcObjData.name = "衙门捕头（95级英雄副本）";
                        npcObjData.picPath = "npc06_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000380":
                    {
                        npcObjData.name = "除魔军北将";
                        npcObjData.picPath = "npc35_png";
                        return npcObjData;
                    }
                case "10000381":
                    {
                        npcObjData.name = "黎西西";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000382":
                    {
                        npcObjData.name = "高大富";
                        npcObjData.picPath = "npc02_png";
                        return npcObjData;
                    }
                case "10000383":
                    {
                        npcObjData.name = "煞天";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000384":
                    {
                        npcObjData.name = "霜霖";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000385":
                    {
                        npcObjData.name = "除魔军西将";
                        npcObjData.picPath = "npc35_png";
                        return npcObjData;
                    }
                case "10000386":
                    {
                        npcObjData.name = "捕蛇人";
                        npcObjData.picPath = "npc02_png";
                        return npcObjData;
                    }
                case "10000387":
                    {
                        npcObjData.name = "渔歌";
                        npcObjData.picPath = "npc11_png";
                        return npcObjData;
                    }
                case "10000388":
                    {
                        npcObjData.name = "苏妹";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000389":
                    {
                        npcObjData.name = "七婶";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000390":
                    {
                        npcObjData.name = "石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000391":
                    {
                        npcObjData.name = "绣娘";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000392":
                    {
                        npcObjData.name = "洪武";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000393":
                    {
                        npcObjData.name = "筱瑶";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000394":
                    {
                        npcObjData.name = "清束";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000395":
                    {
                        npcObjData.name = "范儿";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000396":
                    {
                        npcObjData.name = "王生";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000397":
                    {
                        npcObjData.name = "甘龙";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000398":
                    {
                        npcObjData.name = "旬向";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000399":
                    {
                        npcObjData.name = "玄饥子（95级英雄副本）";
                        npcObjData.picPath = "npc13_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000400":
                    {
                        npcObjData.name = "云虚子";
                        npcObjData.picPath = "npc_yxz_png";
                        return npcObjData;
                    }
                case "10000401":
                    {
                        npcObjData.name = "玄阴子";
                        npcObjData.picPath = "npc_xyz_png";
                        return npcObjData;
                    }
                case "10000402":
                    {
                        npcObjData.name = "天阳子";
                        npcObjData.picPath = "npc_tyz_png";
                        return npcObjData;
                    }
                case "10000403":
                    {
                        npcObjData.name = "紫凝（紫装飞升）";
                        npcObjData.picPath = "cz_NPC";
                        npcObjData.addAcKeys("zzfs");
                        return npcObjData;
                    }
                case "10000404":
                    {
                        npcObjData.name = "扶苏";
                        npcObjData.picPath = "npc_fs_png";
                        return npcObjData;
                    }
                case "10000405":
                    {
                        npcObjData.name = "曲琛";
                        npcObjData.picPath = "npc41_png";
                        return npcObjData;
                    }
                case "10000406":
                    {
                        npcObjData.name = "花渐离";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000407":
                    {
                        npcObjData.name = "杂货商人";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000408":
                    {
                        npcObjData.name = "林希";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000409":
                    {
                        npcObjData.name = "晴雪";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000410":
                    {
                        npcObjData.name = "暗岚";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000411":
                    {
                        npcObjData.name = "刘氏";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000412":
                    {
                        npcObjData.name = "旄玑";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000413":
                    {
                        npcObjData.name = "古城遗民（100级英雄副本）";
                        npcObjData.picPath = "npc32_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000414":
                    {
                        npcObjData.name = "灵儿";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000415":
                    {
                        npcObjData.name = "刘礼贤";
                        npcObjData.picPath = "npc34_png";
                        return npcObjData;
                    }
                case "10000416":
                    {
                        npcObjData.name = "临安";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000417":
                    {
                        npcObjData.name = "通天教主";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000418":
                    {
                        npcObjData.name = "刘络";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000419":
                    {
                        npcObjData.name = "陆风";
                        npcObjData.picPath = "npc39";
                        return npcObjData;
                    }
                case "10000420":
                    {
                        npcObjData.name = "朱三豪";
                        npcObjData.picPath = "npc02_png";
                        return npcObjData;
                    }
                case "10000421":
                    {
                        npcObjData.name = "苏仪";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000422":
                    {
                        npcObjData.name = "沈落石";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000423":
                    {
                        npcObjData.name = "两仪蛟母";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000424":
                    {
                        npcObjData.name = "慕天";
                        npcObjData.picPath = "npc17_png";
                        return npcObjData;
                    }
                case "10000425":
                    {
                        npcObjData.name = "龙力";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000426":
                    {
                        npcObjData.name = "白素";
                        npcObjData.picPath = "npc_bs_png";
                        return npcObjData;
                    }
                case "10000427":
                    {
                        npcObjData.name = "修道者（100级副本）";
                        npcObjData.picPath = "npc14_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000428":
                    {
                        npcObjData.name = "古谚";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000429":
                    {
                        npcObjData.name = "吴道远";
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000430":
                    {
                        npcObjData.name = "段祺";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000431":
                    {
                        npcObjData.name = "晶柯";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000432":
                    {
                        npcObjData.name = "高清阳";
                        npcObjData.picPath = "npc33_png";
                        return npcObjData;
                    }
                case "10000433":
                    {
                        npcObjData.name = "道玄";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000434":
                    {
                        npcObjData.name = "萧恨水";
                        npcObjData.picPath = "npc_xhs_png";
                        return npcObjData;
                    }
                case "10000435":
                    {
                        npcObjData.name = "周量";
                        npcObjData.picPath = "npc19_png";
                        return npcObjData;
                    }
                case "10000436":
                    {
                        npcObjData.name = "高清月";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000437":
                    {
                        npcObjData.name = "李艺";
                        npcObjData.picPath = "npc03_png";
                        return npcObjData;
                    }
                case "10000438":
                    {
                        npcObjData.name = "李银";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000439":
                    {
                        npcObjData.name = "燕师师";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000440":
                    {
                        npcObjData.name = "见影";
                        npcObjData.picPath = "npc42_png";
                        return npcObjData;
                    }
                case "10000441":
                    {
                        npcObjData.name = "云峰";
                        npcObjData.picPath = "npc16_png";
                        return npcObjData;
                    }
                case "10000442":
                    {
                        npcObjData.name = "连径";
                        npcObjData.picPath = "npc15_png";
                        return npcObjData;
                    }
                case "10000443":
                    {
                        npcObjData.name = "流云";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000444":
                    {
                        npcObjData.name = "徐岸";
                        npcObjData.picPath = "npc07_png";
                        return npcObjData;
                    }
                case "10000445":
                    {
                        npcObjData.name = "黑煞";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000446":
                    {
                        npcObjData.name = "云珠";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000447":
                    {
                        npcObjData.name = "戚薇";
                        npcObjData.picPath = "npc_qw_png";
                        return npcObjData;
                    }
                case "10000448":
                    {
                        npcObjData.name = "周舟";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000449":
                    {
                        npcObjData.name = "恍惚的幽灵";
                        npcObjData.picPath = "npc12_png";
                        return npcObjData;
                    }
                case "10000450":
                    {
                        npcObjData.name = "徐安峰";
                        npcObjData.picPath = "npc06_png";
                        return npcObjData;
                    }
                case "10000451":
                    {
                        npcObjData.name = "紫媚";
                        npcObjData.picPath = "npc09_png";
                        return npcObjData;
                    }
                case "10000452":
                    {
                        npcObjData.name = "李金龙";
                        npcObjData.picPath = "npc26_png";
                        return npcObjData;
                    }
                case "10000453":
                    {
                        npcObjData.name = "天星子";
                        npcObjData.picPath = "npc_yxz_png";
                        return npcObjData;
                    }
                case "10000454":
                    {
                        npcObjData.name = "石碑（100级英雄副本）";
                        npcObjData.picPath = "ptsb_png";
                        npcObjData.addAcKeys("fb");
                        return npcObjData;
                    }
                case "10000455":
                    {
                        npcObjData.name = "旷世石碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000456":
                    {
                        npcObjData.name = "太乙真人";
                        npcObjData.picPath = "jmxt_png";
                        npcObjData.addAcKeys("jingmai");
                        return npcObjData;
                    }
                case "10000457":
                    {
                        npcObjData.name = "墨家门主";
                        npcObjData.picPath = "npc58_png";
                        npcObjData.addAcKeys("2011", "joinMenPai");
                        return npcObjData;
                    }
                case "10000458":
                    {
                        npcObjData.name = "猛士堂主";
                        npcObjData.picPath = "npc49_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000459":
                    {
                        npcObjData.name = "遁甲堂主";
                        npcObjData.picPath = "npc51_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000460":
                    {
                        npcObjData.name = "墨家传送师";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000461":
                    {
                        npcObjData.name = "猛士大师兄";
                        npcObjData.picPath = "mengshi_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000462":
                    {
                        npcObjData.name = "遁甲大师兄";
                        npcObjData.picPath = "dunjia_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000463":
                    {
                        npcObjData.name = "道家门主";
                        npcObjData.picPath = "npc57_png";
                        npcObjData.addAcKeys("2011", "joinMenPai");
                        return npcObjData;
                    }
                case "10000464":
                    {
                        npcObjData.name = "琴魔堂主";
                        npcObjData.picPath = "npc45_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000465":
                    {
                        npcObjData.name = "天音堂主";
                        npcObjData.picPath = "npc47_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000466":
                    {
                        npcObjData.name = "道家传送师";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000467":
                    {
                        npcObjData.name = "琴魔大师兄";
                        npcObjData.picPath = "qinmo_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000468":
                    {
                        npcObjData.name = "天音大师兄";
                        npcObjData.picPath = "tianyin_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000469":
                    {
                        npcObjData.name = "阴阳家门主";
                        npcObjData.picPath = "npc59_png";
                        npcObjData.addAcKeys("2011", "joinMenPai");
                        return npcObjData;
                    }
                case "10000470":
                    {
                        npcObjData.name = "罗刹堂主";
                        npcObjData.picPath = "npc55_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000471":
                    {
                        npcObjData.name = "幽冥堂主";
                        npcObjData.picPath = "npc54_png";
                        npcObjData.addAcKeys("mpjnStudy");
                        return npcObjData;
                    }
                case "10000472":
                    {
                        npcObjData.name = "阴阳家传送师";
                        npcObjData.picPath = "npc29_png";
                        return npcObjData;
                    }
                case "10000473":
                    {
                        npcObjData.name = "罗刹大师兄";
                        npcObjData.picPath = "youming_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000474":
                    {
                        npcObjData.name = "幽冥大师兄";
                        npcObjData.picPath = "luosha_NPC_png";
                        npcObjData.addAcKeys("mpdsx");
                        return npcObjData;
                    }
                case "10000475":
                    {
                        npcObjData.name = "镇塔将军";
                        npcObjData.picPath = "tttw_png";
                        return npcObjData;
                    }
                case "10000476":
                    {
                        npcObjData.name = "闯关指引师";
                        npcObjData.picPath = "npc20_png";
                        npcObjData.addAcKeys("2007_1");
                        return npcObjData;
                    }
                case "10000477":
                    {
                        npcObjData.name = "镇塔将军";
                        npcObjData.picPath = "tttw_png";
                        return npcObjData;
                    }
                case "10000478":
                    {
                        npcObjData.name = "闯关指引师";
                        npcObjData.picPath = "npc20_png";
                        npcObjData.addAcKeys("2007_1");
                        return npcObjData;
                    }
                case "10000479":
                    {
                        npcObjData.name = "镇塔将军";
                        npcObjData.picPath = "tttw_png";
                        return npcObjData;
                    }
                case "10000480":
                    {
                        npcObjData.name = "闯关指引师";
                        npcObjData.picPath = "npc20_png";
                        npcObjData.addAcKeys("2007_1");
                        return npcObjData;
                    }
                case "10000481":
                    {
                        npcObjData.name = "天渊使者";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000482":
                    {
                        npcObjData.name = "天渊传送师";
                        npcObjData.picPath = "npc_css_png";
                        npcObjData.addAcKeys("2003_1");
                        return npcObjData;
                    }
                case "10000483":
                    {
                        npcObjData.name = "近卫武士";
                        npcObjData.picPath = "jqnpc01";
                        return npcObjData;
                    }
                case "10000484":
                    {
                        npcObjData.name = "上古士兵";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000485":
                    {
                        npcObjData.name = "狂攻魔神";
                        npcObjData.picPath = "NPC_sg_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000486":
                    {
                        npcObjData.name = "铁壁魔神";
                        npcObjData.picPath = "NPC_long_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000487":
                    {
                        npcObjData.name = "生命魔神";
                        npcObjData.picPath = "NPC_sm_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000488":
                    {
                        npcObjData.name = "神速魔神";
                        npcObjData.picPath = "NPC_ssu_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000489":
                    {
                        npcObjData.name = "射手魔神";
                        npcObjData.picPath = "NPC_js_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000490":
                    {
                        npcObjData.name = "法术魔神";
                        npcObjData.picPath = "NPC_mfa_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000491":
                    {
                        npcObjData.name = "暴怒魔神";
                        npcObjData.picPath = "NPC_bj_png";
                        npcObjData.addAcKeys("2028_2");
                        return npcObjData;
                    }
                case "10000492":
                    {
                        npcObjData.name = "狂攻祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000493":
                    {
                        npcObjData.name = "铁壁祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000494":
                    {
                        npcObjData.name = "生命祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000495":
                    {
                        npcObjData.name = "神速祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000496":
                    {
                        npcObjData.name = "射手祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000497":
                    {
                        npcObjData.name = "法术祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000498":
                    {
                        npcObjData.name = "暴怒祭司";
                        npcObjData.picPath = "NPC_ty28_png";
                        npcObjData.addAcKeys("2028_1");
                        return npcObjData;
                    }
                case "10000499":
                    {
                        npcObjData.name = "魔神传送师";
                        npcObjData.picPath = "npc_css_png";
                        npcObjData.addAcKeys("2028_3");
                        return npcObjData;
                    }
                case "10000500":
                    {
                        npcObjData.name = "古城调查员";
                        npcObjData.picPath = "npc36_png";
                        return npcObjData;
                    }
                case "10000501":
                    {
                        npcObjData.name = "噬人妖";
                        npcObjData.picPath = "30005";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_50_1";
                        return npcObjData;
                    }
                case "10000502":
                    {
                        npcObjData.name = "镇魔将军";
                        npcObjData.picPath = "npc08_png";
                        return npcObjData;
                    }
                case "10000503":
                    {
                        npcObjData.name = "游离鬼魂";
                        npcObjData.picPath = "40010";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_50_4";
                        return npcObjData;
                    }
                case "10000504":
                    {
                        npcObjData.name = "暴怒尸鬼";
                        npcObjData.picPath = "50008";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_50_5";
                        return npcObjData;
                    }
                case "10000505":
                    {
                        npcObjData.name = "太古真魔";
                        npcObjData.picPath = "npc60_png";
                        return npcObjData;
                    }
                case "10000506":
                    {
                        npcObjData.name = "亡灵刀兵";
                        npcObjData.picPath = "20003";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_60_1";
                        return npcObjData;
                    }
                case "10000507":
                    {
                        npcObjData.name = "亡灵戟兵";
                        npcObjData.picPath = "70003";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_60_2";
                        return npcObjData;
                    }
                case "10000508":
                    {
                        npcObjData.name = "亡灵裨将";
                        npcObjData.picPath = "npc65_bs08_png";
                        return npcObjData;
                    }
                case "10000509":
                    {
                        npcObjData.name = "亡灵弓手";
                        npcObjData.picPath = "50004";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_60_4";
                        return npcObjData;
                    }
                case "10000510":
                    {
                        npcObjData.name = "亡灵斧兵";
                        npcObjData.picPath = "bs07";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_60_5";
                        return npcObjData;
                    }
                case "10000511":
                    {
                        npcObjData.name = "亡灵统帅";
                        npcObjData.picPath = "npc66_bs09_png";
                        return npcObjData;
                    }
                case "10000512":
                    {
                        npcObjData.name = "巨山熊";
                        npcObjData.picPath = "80003";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_70_1";
                        return npcObjData;
                    }
                case "10000513":
                    {
                        npcObjData.name = "守谷老人";
                        npcObjData.picPath = "npc68_bs11_png";
                        return npcObjData;
                    }
                case "10000514":
                    {
                        npcObjData.name = "隐月宗二弟子";
                        npcObjData.picPath = "npc67_bs10_png";
                        return npcObjData;
                    }
                case "10000515":
                    {
                        npcObjData.name = "猛山虎";
                        npcObjData.picPath = "60007";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_70_4";
                        return npcObjData;
                    }
                case "10000516":
                    {
                        npcObjData.name = "隐月宗大弟子";
                        npcObjData.picPath = "npc73_bs25_png";
                        return npcObjData;
                    }
                case "10000517":
                    {
                        npcObjData.name = "月尘子";
                        npcObjData.picPath = "npc74_bs27_png";
                        return npcObjData;
                    }
                case "10000518":
                    {
                        npcObjData.name = "前世之灵";
                        npcObjData.picPath = "40010";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_80_1";
                        return npcObjData;
                    }
                case "10000519":
                    {
                        npcObjData.name = "今生之魂";
                        npcObjData.picPath = "1014";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_80_2";
                        return npcObjData;
                    }
                case "10000520":
                    {
                        npcObjData.name = "来世之魄";
                        npcObjData.picPath = "1025";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_80_3";
                        return npcObjData;
                    }
                case "10000521":
                    {
                        npcObjData.name = "三生兽";
                        npcObjData.picPath = "npc69_bs19_png";
                        return npcObjData;
                    }
                case "10000522":
                    {
                        npcObjData.name = "三生轮回碑";
                        npcObjData.picPath = "ptsb_png";
                        return npcObjData;
                    }
                case "10000523":
                    {
                        npcObjData.name = "青丘之灵";
                        npcObjData.picPath = "40001";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_90_1";
                        return npcObjData;
                    }
                case "10000524":
                    {
                        npcObjData.name = "打手";
                        npcObjData.picPath = "70006";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_90_2";
                        return npcObjData;
                    }
                case "10000525":
                    {
                        npcObjData.name = "瑞南羽";
                        npcObjData.picPath = "npc71_bs21_png";
                        return npcObjData;
                    }
                case "10000526":
                    {
                        npcObjData.name = "九尾幻影";
                        npcObjData.picPath = "1025";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_90_4";
                        return npcObjData;
                    }
                case "10000527":
                    {
                        npcObjData.name = "六尾";
                        npcObjData.picPath = "1025";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_90_5";
                        return npcObjData;
                    }
                case "10000528":
                    {
                        npcObjData.name = "九尾异兽";
                        npcObjData.picPath = "npc70_bs20_png";
                        return npcObjData;
                    }
                case "10000529":
                    {
                        npcObjData.name = "恶灵";
                        npcObjData.picPath = "40010";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_100_1";
                        return npcObjData;
                    }
                case "10000530":
                    {
                        npcObjData.name = "洞渊战魂";
                        npcObjData.picPath = "npc72_bs22_png";
                        return npcObjData;
                    }
                case "10000531":
                    {
                        npcObjData.name = "魔影";
                        npcObjData.picPath = "60003";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_100_4";
                        return npcObjData;
                    }
                case "10000532":
                    {
                        npcObjData.name = "百鬼之王";
                        npcObjData.picPath = "npc64_bs04_png";
                        return npcObjData;
                    }
                case "10000533":
                    {
                        npcObjData.name = "冒险者";
                        npcObjData.picPath = "npc36_png";
                        return npcObjData;
                    }
                case "10000534":
                    {
                        npcObjData.name = "小女孩";
                        npcObjData.picPath = "npc10_png";
                        return npcObjData;
                    }
                case "10000535":
                    {
                        npcObjData.name = "煞阴尸鬼";
                        npcObjData.picPath = "ty28";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_ls_1";
                        return npcObjData;
                    }
                case "10000536":
                    {
                        npcObjData.name = "虐杀之鬼";
                        npcObjData.picPath = "npc16066_png";
                        return npcObjData;
                    }
                case "10000537":
                    {
                        npcObjData.name = "黑衣人";
                        npcObjData.picPath = "npc38";
                        return npcObjData;
                    }
                case "10000538":
                    {
                        npcObjData.name = "小男孩";
                        npcObjData.picPath = "npc21_png";
                        return npcObjData;
                    }
                case "10000539":
                    {
                        npcObjData.name = "幽蓝狼魔";
                        npcObjData.picPath = "ty29";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_ls_3";
                        return npcObjData;
                    }
                case "10000540":
                    {
                        npcObjData.name = "暗影杀手";
                        npcObjData.picPath = "npc16067_png";
                        return npcObjData;
                    }
                case "10000541":
                    {
                        npcObjData.name = "红衣女";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000542":
                    {
                        npcObjData.name = "巨角魔牛";
                        npcObjData.picPath = "ty34";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_hh_1";
                        return npcObjData;
                    }
                case "10000543":
                    {
                        npcObjData.name = "夫人";
                        npcObjData.picPath = "npc23_png";
                        return npcObjData;
                    }
                case "10000544":
                    {
                        npcObjData.name = "红衣女";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000545":
                    {
                        npcObjData.name = "荡月巨熊";
                        npcObjData.picPath = "ty35";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_hh_2";
                        return npcObjData;
                    }
                case "10000546":
                    {
                        npcObjData.name = "灭城将军";
                        npcObjData.picPath = "npc16068_png";
                        return npcObjData;
                    }
                case "10000547":
                    {
                        npcObjData.name = "红衣女";
                        npcObjData.picPath = "npc20_png";
                        return npcObjData;
                    }
                case "10000548":
                    {
                        npcObjData.name = "说书人";
                        npcObjData.picPath = "npc16072_png";
                        return npcObjData;
                    }
                case "10000549":
                    {
                        npcObjData.name = "黑煞木妖";
                        npcObjData.picPath = "ty39";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_ys_1";
                        return npcObjData;
                    }
                case "10000550":
                    {
                        npcObjData.name = "龙女";
                        npcObjData.picPath = "npc16070_png";
                        return npcObjData;
                    }
                case "10000551":
                    {
                        npcObjData.name = "苍岚狮鹫";
                        npcObjData.picPath = "30011";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_ys_2";
                        return npcObjData;
                    }
                case "10000552":
                    {
                        npcObjData.name = "说书人";
                        npcObjData.picPath = "npc16072_png";
                        return npcObjData;
                    }
                case "10000553":
                    {
                        npcObjData.name = "冥罗妖";
                        npcObjData.picPath = "ty23";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_zx_1";
                        return npcObjData;
                    }
                case "10000554":
                    {
                        npcObjData.name = "心魔";
                        npcObjData.picPath = "npc16069_png";
                        return npcObjData;
                    }
                case "10000555":
                    {
                        npcObjData.name = "心魔";
                        npcObjData.picPath = "npc16074_png";
                        npcObjData.addAcKeys("fb_yzj");
                        return npcObjData;
                    }
                case "10000556":
                    {
                        npcObjData.name = "蜃兽";
                        npcObjData.picPath = "npc16074_png";
                        return npcObjData;
                    }
                case "10000557":
                    {
                        npcObjData.name = "熔骨尸煞";
                        npcObjData.picPath = "ty31";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_zx_2";
                        return npcObjData;
                    }
                case "10000558":
                    {
                        npcObjData.name = "天星子";
                        npcObjData.picPath = "npc_qhzg_png";
                        return npcObjData;
                    }
                case "10000559":
                    {
                        npcObjData.name = "饕餮";
                        npcObjData.picPath = "npc16078_png";
                        return npcObjData;
                    }
                case "10000560":
                    {
                        npcObjData.name = "灵通仙兽";
                        npcObjData.picPath = "ty37";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_yzj_1";
                        return npcObjData;
                    }
                case "10000561":
                    {
                        npcObjData.name = "梼杌";
                        npcObjData.picPath = "npc16081_png";
                        return npcObjData;
                    }
                case "10000562":
                    {
                        npcObjData.name = "青玄仙人";
                        npcObjData.picPath = "ty38";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_yzj_3";
                        return npcObjData;
                    }
                case "10000563":
                    {
                        npcObjData.name = "天机老人";
                        npcObjData.picPath = "npc13_png";
                        return npcObjData;
                    }
                case "10000564":
                    {
                        npcObjData.name = "沧海仙龙";
                        npcObjData.picPath = "ty40";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "fb_yzj_7";
                        return npcObjData;
                    }
                case "10000565":
                    {
                        npcObjData.name = "神秘老朽";
                        npcObjData.picPath = "npc74_bs27_png";
                        return npcObjData;
                    }
                case "10000566":
                    {
                        npcObjData.name = "小妞";
                        npcObjData.picPath = "npc_qw_png";
                        npcObjData.addAcKeys("cbdt");
                        return npcObjData;
                    }
                case "10000567":
                    {
                        npcObjData.name = "农夫";
                        npcObjData.picPath = "npc01_png";
                        npcObjData.addAcKeys("nongchang");
                        return npcObjData;
                    }
                case "10000568":
                    {
                        npcObjData.name = "月下老人";
                        npcObjData.picPath = "hyyxlr_png";
                        return npcObjData;
                    }
                case "10000569":
                    {
                        npcObjData.name = "房屋总管";
                        npcObjData.picPath = "npc30_png";
                        return npcObjData;
                    }
                case "10000570":
                    {
                        npcObjData.name = "好感培养师";
                        npcObjData.picPath = "npc_bs_png";
                        return npcObjData;
                    }
                case "10000571":
                    {
                        npcObjData.name = "第三者";
                        npcObjData.picPath = "dssn_NPC_png";
                        return npcObjData;
                    }
                case "10000572":
                    {
                        npcObjData.name = "驯兽师";
                        npcObjData.picPath = "npc_cwsr_png";
                        return npcObjData;
                    }
                case "10000573":
                    {
                        npcObjData.name = "龙翔天兵";
                        npcObjData.picPath = "4htb_png";
                        return npcObjData;
                    }
                case "10000574":
                    {
                        npcObjData.name = "九天玄女";
                        npcObjData.picPath = "3hxn_png";
                        return npcObjData;
                    }
                case "10000575":
                    {
                        npcObjData.name = "玄晶天狐";
                        npcObjData.picPath = "2hth_png";
                        return npcObjData;
                    }
                case "10000576":
                    {
                        npcObjData.name = "逆天魔龙";
                        npcObjData.picPath = "5hml_png";
                        return npcObjData;
                    }
                case "10000577":
                    {
                        npcObjData.name = "太虚之魂";
                        npcObjData.picPath = "6htx_png";
                        return npcObjData;
                    }
                case "10000578":
                    {
                        npcObjData.name = "青玄剑灵";
                        npcObjData.picPath = "1hjl_png";
                        return npcObjData;
                    }
                case "10000579":
                    {
                        npcObjData.name = "聚灵真火";
                        npcObjData.picPath = "kh";
                        npcObjData.addAcKeys("jldh");
                        return npcObjData;
                    }
                case "10000580":
                    {
                        npcObjData.name = "天文学士";//汉中活动
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000581":
                    {
                        npcObjData.name = "地理学士";//汉中行政
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000582":
                    {
                        npcObjData.name = "历史学士";//吴中活动
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000583":
                    {
                        npcObjData.name = "人文学士";//吴中行政
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000584":
                    {
                        npcObjData.name = "乐园学士";//邯郸活动
                        npcObjData.picPath = "npc31_png";
                        return npcObjData;
                    }
                case "10000585":
                    {
                        npcObjData.name = "翰林学士";//邯郸行政
                        npcObjData.picPath = "npc74_bs27_png";
                        return npcObjData;
                    }
                case "10000586":
                    {
                        npcObjData.name = "孔庙门生";
                        npcObjData.picPath = "npc21_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000587":
                    {
                        npcObjData.name = "夫子门客";
                        npcObjData.picPath = "npc21_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000588":
                    {
                        npcObjData.name = "孔庙弟子";
                        npcObjData.picPath = "npc21_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000589":
                    {
                        npcObjData.name = "孔庙贤士";
                        npcObjData.picPath = "npc16_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000590":
                    {
                        npcObjData.name = "孔庙夫子";
                        npcObjData.picPath = "npc13_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000591":
                    {
                        npcObjData.name = "老儒生";
                        npcObjData.picPath = "npc14_png";
                        npcObjData.addAcKeys("kongmiao");
                        return npcObjData;
                    }
                case "10000592":
                    {
                        npcObjData.name = "内务总管";
                        npcObjData.picPath = "npc_bjxs_png";
                        npcObjData.addAcKeys("bpnw");
                        return npcObjData;
                    }
                case "10000593":
                    {
                        npcObjData.name = "帮派护法";
                        npcObjData.picPath = "npc_zhw_png";
                        npcObjData.addAcKeys("bphf");
                        return npcObjData;
                    }
                case "10000594":
                    {
                        npcObjData.name = "福利大使";
                        npcObjData.picPath = "npc_qhzg_png";
                        npcObjData.addAcKeys("flds");
                        return npcObjData;
                    }
                case "10000595":
                    {
                        npcObjData.name = "研发长老";
                        npcObjData.picPath = "npc_xh_png";
                        npcObjData.addAcKeys("yfzl");
                        return npcObjData;
                    }
                case "10000596":
                    {
                        npcObjData.name = "守护玄碑";
                        npcObjData.picPath = "ptsb_png";
                        npcObjData.addAcKeys("shxb");
                        return npcObjData;
                    }
                case "10000597":
                    {
                        npcObjData.name = "建设堂主";
                        npcObjData.picPath = "npc_zbjls_png";
                        npcObjData.addAcKeys("jstz");
                        return npcObjData;
                    }
                case "10000598":
                    {
                        npcObjData.name = "帮贡长老";
                        npcObjData.picPath = "npc_lb_png";
                        npcObjData.addAcKeys("bgzl");
                        return npcObjData;
                    }
                case "10000599":
                    {
                        npcObjData.name = "传送长老";
                        npcObjData.picPath = "npc_css_png";
                        npcObjData.addAcKeys("cszl");
                        return npcObjData;
                    }
                case "10000600":
                    {
                        npcObjData.name = "顽猴";
                        npcObjData.picPath = "npc_ph_png";
                        npcObjData.addAcKeys("houzi");
                        return npcObjData;
                    }
                case "10000601":
                    {
                        npcObjData.name = "后山守卫";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("houshanshouwei");
                        return npcObjData;
                    }
                case "10000602":
                    {
                        npcObjData.name = "迷宫寻路人";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("2021_1");
                        return npcObjData;
                    }
                case "10000603":
                    {
                        npcObjData.name = "迷宫机关";
                        npcObjData.picPath = "20227_png";
                        npcObjData.addAcKeys("2021_2");
                        return npcObjData;
                    }
                case "10000604":
                    {
                        npcObjData.name = "迷宫指路人";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("2021_3");
                        return npcObjData;
                    }
                case "10000605":
                    {
                        npcObjData.name = "生死门";
                        npcObjData.picPath = "20228_png";
                        npcObjData.addAcKeys("2021_4");
                        return npcObjData;
                    }
                case "10000606":
                    {
                        npcObjData.name = "迷宫拯救者";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("2021_5");
                        return npcObjData;
                    }
                case "10000607":
                    {
                        npcObjData.name = "迷路恶兽";
                        npcObjData.picPath = "ty27";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "hhbk_2";
                        return npcObjData;
                    }
                case "10000608":
                    {
                        npcObjData.name = "财富守护者";
                        npcObjData.picPath = "npc24_png";
                        npcObjData.addAcKeys("2021_6");
                        return npcObjData;
                    }
                case "10000609":
                    {
                        npcObjData.name = "二层宝箱";
                        npcObjData.picPath = "20229_png";
                        npcObjData.addAcKeys("2021_7");
                        return npcObjData;
                    }
                case "10000610":
                    {
                        npcObjData.name = "神龙后裔";
                        npcObjData.picPath = "20231_png";
                        npcObjData.addAcKeys("2021_8");
                        return npcObjData;
                    }
                case "10000611":
                    {
                        npcObjData.name = "三层宝箱";
                        npcObjData.picPath = "20230_png";
                        npcObjData.addAcKeys("2021_9");
                        return npcObjData;
                    }
                case "10000612":
                    {
                        npcObjData.name = "机关恶兽";
                        npcObjData.picPath = "ty25";
                        npcObjData.type = 4;
                        npcObjData.toMonKey = "hhbk_1";
                        return npcObjData;
                    }
                case "10000613":
                    {
                        npcObjData.name = "梦境守护者";
                        npcObjData.picPath = "npc74_bs27_png";
                        npcObjData.addAcKeys("dmkj_1");
                        return npcObjData;
                    }
                case "10000614":
                    {
                        npcObjData.name = "南离城商人";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("paoshang_buy");//购买/出售 货物 茶叶、鹿茸、人参、玉瓷
                        return npcObjData;
                    }
                case "10000615":
                    {
                        npcObjData.name = "北冥城商人";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("paoshang_buy");//购买/出售 货物  人参、玉瓷、丝绸、珠宝
                        return npcObjData;
                    }
                case "10000616":
                    {
                        npcObjData.name = "跑商管理员";
                        npcObjData.picPath = "npc74_bs27_png";
                        npcObjData.addAcKeys("paoshang_task");//开始跑商，接取一个跑商任务
                        return npcObjData;
                    }
                case "10000617":
                    {
                        npcObjData.name = "西沙城商人";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("paoshang_buy");//购买/出售 货物  面粉、酒料、茶叶、鹿茸
                        return npcObjData;
                    }
                case "10000618":
                    {
                        npcObjData.name = "东云城商人";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("paoshang_buy");//购买/出售 货物  面粉、香料、丝绸、珠宝
                        return npcObjData;
                    }
                case "10000619":
                    {
                        npcObjData.name = "帮战大使";
                        npcObjData.picPath = "npc23_png";
                        npcObjData.addAcKeys("bz_npc");
                        return npcObjData;
                    }
                case "10000620":
                    {
                        npcObjData.name = "守卫";
                        npcObjData.picPath = "npc74_bs27_png";
                        npcObjData.addAcKeys("prison");
                        return npcObjData;
                    }
                case "10000621":
                    {
                        npcObjData.name = "血腥老祖";
                        npcObjData.picPath = "npc74_bs27_png";
                        npcObjData.addAcKeys("xxzd");
                        return npcObjData;
                    }

                default:
                    {
                        if (strUtils.isMatch(key, "gw([0-9]{4})"))
                        {
                            Pet pet = face.petInterface.getPetDataByKey(key.Replace("gw", ""));
                            npcObjData.name = pet.name;
                            npcObjData.picPath = pet.getNpcPicPath();
                            return npcObjData;
                        }
                        else if (strUtils.isMatch(key, "cj([0-9]{4})"))//采集物
                        {
                            npcObjData.name = "采集物";
                            npcObjData.picPath = "paopao";
                            npcObjData.type = 5;
                            return npcObjData;
                        }
                        else if (key.Contains("abyss_"))//由1开始
                        {//天渊怪
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
                            string[] arr1 = {
                                "20005","20004","60007","30009","80008",
                                "60003","60002","30010","80002","30004",
                                "20002","30007","10002","80006","50003",
                                "30011","80007","80003","60012","90003",

                                "ty21","ty22","ty23","ty24","ty25",
                                "ty26","ty27","ty28","ty29","ty30",
                                "ty31","ty32","ty33","ty34","ty35",
                                "ty36","ty37","ty38","ty39","ty40",
                            };
                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1];
                            npcObjData.type = 4;
                            npcObjData.picPath = arr1[index - 1];

                            return npcObjData;
                        }
                        else if (key.Contains("qlhd_"))//由1开始
                        {//天渊怪
                            string[] arr = {
                                "赤岩魔狼", "寒冰魔狼", "震天利爪兽","灵魂舔舐者"
                            };
                            string[] arr1 = {
                                "20004","60002","50003","ty39",
                            };
                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1];
                            npcObjData.type = 4;
                            npcObjData.picPath = arr1[index - 1];

                            return npcObjData;
                        }
                        else if (key.Contains("blxt_"))//由1开始
                        {//百炼玄塔
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
                            npcObjData.name = arr[index - 1];
                            npcObjData.picPath = "npc_rchdds_png";
                            npcObjData.addAcKeys("2030_1");
                            return npcObjData;
                        }
                        else if (key.Contains("mshuwei_"))//由1开始
                        {//魔神护卫
                            string[] arr = {
                                 "狂攻", "铁壁", "生命", "神速", "射手","法术", "暴怒",
                            };

                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1] + "之护卫";
                            npcObjData.picPath = "ty23";
                            npcObjData.type = 4;
                            npcObjData.isMove = false;//不允许移动
                            return npcObjData;
                        }
                        else if (key.Contains("xhxy_"))//由1开始
                        {//仗剑除魔
                            string[] arr = {
                                 "吸魂小妖",
                            };

                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1];
                            npcObjData.picPath = "40010";
                            npcObjData.type = 4;
                            npcObjData.isMove = false;//不允许移动
                            return npcObjData;
                        }
                        else if (key.Contains("cyby_"))//由1开始
                        {//采阴补阳
                            string[] arr = {
                                 "狐萌萌",
                            };

                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1];
                            npcObjData.picPath = "bs20";
                            npcObjData.type = 4;
                            npcObjData.isMove = false;//不允许移动
                            return npcObjData;
                        }
                        else if (key.Contains("jyfy_"))//由1开始
                        {//采阴补阳
                            string[] arr = {
                                 "太二真人", "西门好色", "鲁光光","东方必败","完颜失色",
                            };
                            string[] arr1 = { "30012", "100006", "sc05", "100001", "30003", };
                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.name = arr[index - 1];
                            npcObjData.picPath = arr1[index - 1];
                            npcObjData.type = 4;
                            npcObjData.isMove = false;//不允许移动
                            return npcObjData;
                        }
                        else if (key.Contains("bz_box_"))//由1开始
                        {//帮战宝箱 bz_box_b1-16  
                            string str = null;
                            string pic = "20229_png";
                            if (key.Contains("bz_box_sj")) str = "随机宝箱";
                            else if (key.Equals("bz_box_b1") || key.Equals("bz_box_b2"))
                            {
                                str = "金宝箱"; pic = "20230_png";
                            }
                            else if (key.Equals("bz_box_b3") || key.Equals("bz_box_b4") || key.Equals("bz_box_b5") || key.Equals("bz_box_b6")) str = "银宝箱";
                            else str = "铜宝箱";

                            npcObjData.name = str;
                            npcObjData.picPath = pic;
                            npcObjData.addAcKeys("bz_box");
                            return npcObjData;
                        }

                        else if (key.Contains("AbyssTransmit_"))//由1开始
                        {
                            //天渊传送
                            int index = int.Parse(key.Split('_')[1]);
                            npcObjData.initTransmit("天渊" + index + "层", "abyss" + index);
                            return npcObjData;
                        }
                        else if (key.Contains("MapTransmit="))//后带上地图的key
                        {
                            string mapKey = key.Split('=')[1];
                            MapData data = face.mapInterface.getMapByKey(mapKey);
                            string name = data.name;
                            if (data.name.Contains("-")) name = data.name.Split("-")[1];
                            npcObjData.initTransmit(name, mapKey);
                            return npcObjData;
                        }
                        break;
                    }

            }
            return null;
        }
    }
}
