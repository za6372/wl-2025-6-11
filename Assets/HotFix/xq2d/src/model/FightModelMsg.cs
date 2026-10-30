using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.factory.manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.model
{
    /**战斗时绑定的信息*/
    public class FightModelMsg : ModelMsg
    {
        //模型key
        public string model;
        //对应的amKey，实际是模型对应的pwdId
        public string amKey;
        
        public FightModelMsg(string model)
        {
            this.model = model;
            this.init();
        }
        public FightModelMsg init()
        {
            if (GameAttrConst.isRoleModel(model))
            {
                AmModelMsg am = null;
                if (model.Contains("xs_nan"))
                {
                    am = amManager.getAmByKey("xs_nan");
                    amManager.changeManHeadPwd(model+"_pwd", am);
                }
                else if (model.Contains("xs_nv"))
                {
                    am = amManager.getAmByKey("xs_nv");
                    amManager.changeManHeadPwd(model + "_pwd", am);
                }

                else if (model.Contains("zs_nan"))
                {
                    am = amManager.getAmByKey("zs_nan");
                }
                else if (model.Contains("zs_nv"))
                {
                    am = amManager.getAmByKey("zs_nv");
                }
                else if (model.Contains("fs_nan"))
                {
                    am = amManager.getAmByKey("fs_nan");
                }
                else if (model.Contains("fs_nv"))
                {
                    am = amManager.getAmByKey("fs_nv");
                }
                else if (model.Contains("fz_nan"))
                {
                    am = amManager.getAmByKey("fz_nan");
                }
                else if (model.Contains("fz_nv"))
                {
                    am = amManager.getAmByKey("fz_nv");
                }

                else if (model.Contains("ms_nan"))
                {
                    am = amManager.getAmByKey("ms_nan");
                }
                else if (model.Contains("ms_nv"))
                {
                    am = amManager.getAmByKey("ms_nv");
                }
                else if (model.Contains("dj_nan"))
                {
                    am = amManager.getAmByKey("dj_nan");
                }
                else if (model.Contains("dj_nv"))
                {
                    am = amManager.getAmByKey("dj_nv");
                }
                else if (model.Contains("qm_nan"))
                {
                    am = amManager.getAmByKey("qm_nan");
                }
                else if (model.Contains("qm_nv"))
                {
                    am = amManager.getAmByKey("qm_nv");
                }
                else if (model.Contains("ty_nan"))
                {
                    am = amManager.getAmByKey("ty_nan");
                }
                else if (model.Contains("ty_nv"))
                {
                    am = amManager.getAmByKey("ty_nv");
                }
                else if (model.Contains("ym_nan"))
                {
                    am = amManager.getAmByKey("ym_nan");
                }
                else if (model.Contains("ym_nv"))
                {
                    am = amManager.getAmByKey("ym_nv");
                }
                else if (model.Contains("lc_nan"))
                {
                    am = amManager.getAmByKey("lc_nan");
                }
                else if (model.Contains("lc_nv"))
                {
                     am = amManager.getAmByKey("lc_nv");
                }
                this.amToThis(am);
            }

            else if (model.Contains("petxl_"))
            {
                int index = int.Parse(model.Split('_')[1]) - 1;
                string[] pwdIds = {
                    "80007", "60007", "xz01", "30004", "ty35" ,"xz10", "xz12", "xz04", "20005", "60003",

                    "ty26","50002", "bs20", "ty35", "100005" ,"20003", "bs07", "100001", "60009", "40010",

                    "ty26","50002", "bs20", "ty35", "100005" ,"20003", "bs07", "100001", "60009", "40010",

                    "30012","20003", "60007", "60013", "60012" , "30011", "90003", "20002", "bs03", "100004",

                    "30012","20003", "60007", "60013", "60012" , "30011", "90003", "20002", "bs03", "100004",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index]);
                this.amToThis(am);

            }
            else if (model.Contains("abyss_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "20005","20004","60007","30009","80008",
                    "60003","60002","30010","80002","30004",
                    "20002","30007","10002","80006","50003",
                    "30011","80007","80003","60012","90003",

                    "ty21","ty22","ty23","ty24","ty25",
                    "ty26","ty27","ty28","ty29","ty30",
                    "ty31","ty32","ty33","ty34","ty35",
                    "ty36","ty37","ty38","ty39","ty40",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("qlhd_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "20004","60002","50003","ty39",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("blxt_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "80008","30007","10002","20005","xz08","20004","30004","1014","30009","18833",

                    "60002","30005","40001","40010","60009", "50008","50002","100004","70003","18837",

                    "xz09","80002","30010","xz17","xz02", "20003","30001","100003","70001","18836",

                    "60007","20002","xz06","30011","70006", "100001","90002","40010","1014","18838",

                    "60003","xz18","xz05","80003","60013", "xz16","sc01","60009","40008","18835",

                    "60013","70007","100003","xz15","1025", "xz14","50005","40010","xz12","bs10",

                    "60009","60010","50004","100004","xz07","xz04","80006","60013","xz01", "bs04",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("mozun_boss"))
            {
                AmModelMsg am = amManager.getAmByKey("bs03");
                this.amToThis(am);
            }
            else if (model.Contains("houzi"))
            {
                AmModelMsg am = amManager.getAmByKey("xz08");
                this.amToThis(am);
            }
            else if (model.Contains("fb_50"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "30005","bs02","bs03","40010","50008","bs05","bs05",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_60"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "20003","bs06","bs08","50004","bs07","bs09","bs24",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_70"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "80003","bs11","bs10","60007","bs25","bs26","bs27",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_80"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "40010","1014","1025","bs19","100003","bs15","bs16","bs17",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_90"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "40001","70006","bs21","1025","1025","bs20",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_100"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "40010","bs22","bs23","60003","bs04",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_ls"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "ty28","16066","ty29","16067","ty22",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_hh"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "ty34","ty35","16068","16069","ty33",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_ys"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "ty39","30011","16070","16071","16072","16073",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_zx"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "ty23","ty31","16074","16075","16077","16076",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("fb_yzj"))
            {
                int index = int.Parse(model.Split('_')[2]);
                string[] pwdIds = {
                    "ty37","16078","ty38","16081","16080","16079","ty40","16075","NPC57_gw","NPC58_gw","NPC59_gw",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("ztzs_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "1227","1217","60005","1214","1215","35932","bs03","1014",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("mshuwei_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "ty23","ty23","ty23","ty23","ty23","ty23","ty23","xz03",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("yhmkms_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "zms_sg","zms_sg","zms_sg","zms_sg","zms_sg","zms_sg","zms_sg",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("tianbing"))
            {
                AmModelMsg am = amManager.getAmByKey("bs10");
                this.amToThis(am);
            }
            else if (model.Contains("xhxy_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "40010","50002",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("hhbk_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "ty25","ty27","slm",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("dmkj_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "80003","60007","30009","50002","20002","20005","xz03",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("xxzd_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "80003","60007",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("cyby_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "bs20",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("jyfy_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "30012","100006","sc05","100001","30003",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (strUtils.isMatch(model, "yzj_([0-9]{1})"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "gw_kuangshi", "gw_shuyao",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("bz_box_"))//帮战箱子守卫
            {
                AmModelMsg am = amManager.getAmByKey("30012");
                this.amToThis(am);
            }
            else if (model.Contains("mpbwz_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "30012","100006","sc05",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("bprw_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "30012","100006","sc05","100001","30003",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("mprw_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "30012","100006","sc05","100001","30003",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else if (model.Contains("shitu_"))
            {
                int index = int.Parse(model.Split('_')[1]);
                string[] pwdIds = {
                    "80003","60007","30009","50002","20002","20005","100001","xz03",
                };
                AmModelMsg am = amManager.getAmByKey(pwdIds[index - 1]);
                this.amToThis(am);
            }
            else
            {
                Pet pet = face.petInterface.getPetDataByKey(model);
                AmModelMsg am= amManager.getAmByKey(pet.pwdId);
                this.amToThis(am);
            }

            return this;
        }
        public void amToThis(AmModelMsg am)
        {
            this.amKey = am.amKey;
            this.simpleFightIsNear = am.simpleFightIsNear;
            this.changguiPwds = am.changguiPwds;
            this.touxiangPwds = am.touxiangPwds;
            this.zhandouPwds = am.zhandouPwds;
            this.changguiAef = am.changguiAef;
            this.touxiangAef = am.touxiangAef;
            this.zhandouAef = am.zhandouAef;
            this.cgIdle = am.cgIdle;
            this.headIdle = am.headIdle;
            this.zdIdle = am.zdIdle;
            this.zdSklList = am.zdSklList;
            this.zdMzList = am.zdMzList;
            this.zdHurt = am.zdHurt;
            this.zdSd = am.zdSd;

        }
       

    }
}
