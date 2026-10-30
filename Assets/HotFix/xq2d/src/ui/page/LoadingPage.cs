using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.ui.page
{
    /**进入游戏前的加载页面*/
    class LoadingPage : PageUI
    {
        private long time;
        public void drawUI()
        {
            GameObject hb = gameObjPool.getInstance().get("mask", typeof(ImgUI));
            hb.transform.SetParent(this.transform, false);
            hb.GetComponent<ImgUI>().setColor("#000000")
            .setSizePos(new Vector2(ScreenUtils.width, ScreenUtils.height), new Vector2(0, 0));

            GameObject text = gameObjPool.getInstance().get("tip", typeof(TextUI));
            text.transform.SetParent(this.transform, false);
            text.GetComponent<TextUI>().setFontSize(35).setColor("#ffffff").setAlign("left").setText("正在读取资源包(0%)，请稍后...").VertiOut()
            .setSizePos(new Vector2(ScreenUtils.width - 100, 60), new Vector2(50, -50));
            Debug.Log("正在读取资源包");

            time = strUtils.getMillis();
            //将图片转图集
            DoGet.getInstance().addPicToSpriteSheet();
            //todo:需要将显宠、人物、橙装特效这些合并成图集

            if (!sysUtils.isMobile())
            {
                this.check();
            }
            else
            {
                readResFile(() =>
                {
                    Debug.Log("耗时：" + (strUtils.getMillis() - time));
                    this.check();
                });
            }


        }



        private void check()
        {
            Action fn = () =>
            {
                this.freeThisPage();
                PageUI.create<LoginPage>().drawUI();
            };
            if (!sysUtils.isMobile())
            {

                fn();
            }
            else
            {
                versionCheck(fn);
            }
        }
        private void readResFile(Action ac)
        {
            if ("1".Equals(PlayerPrefs.GetString("xq-isinto")))
            {
                //跳过该过程
                ac();
                return;
            }

            AndroidJavaClass jclass = new AndroidJavaClass("android.os.Environment");
            AndroidJavaObject jobj = jclass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");
            string path = jobj.Call<string>("getAbsolutePath");
            Debug.Log("External Storage Path: " + path);

            string appPath = sysUtils.getUrl();
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

            //先读取指定目录资源
            string[] filePaths = Directory.GetFiles(appPath+"/", "*xq-data*");
            string fileName = null;
            if (filePaths.Length==0)
            {
                Debug.Log("不存在文件");
                //如果该目录没有资源则从qq目录复制过来，qq目录也没有时通知玩家先到qq群进行下载
                filePaths = Directory.GetFiles(path + "/Download/QQ/", "*xq-data*");
                if (filePaths.Length == 0)
                {
                    /*this.transform.Find("tip").GetComponent<TextUI>()
                        .setText("资源包找不到，请到qq群文件下载！xq-data.hl是资源包，文件右侧有三个点的按钮点它会有个弹出框，点左下角的[保存到手机]即可，有些版本的qq没有这个选项就参考模拟器的建文件夹，然后把资源包复制进去。模拟器玩家需要把资源包复制到Download/QQ中（找不到就自己建文件夹，或者打开一次游戏让它自己建）即可。注意：资源包文件名一定要是xq-data.hl，多个(1)或者有空格的都不行，需要自己到文件夹中重命名！");
                    return;*/
                    //跳过
                    PlayerPrefs.SetString("xq-isinto", "1");
                    readResFile(ac);
                    return;
                }
                 fileName = Path.GetFileName(filePaths[0]);
                string path2 = path + "/Download/QQ/" + fileName;
                Debug.Log("复制文件");
                //复制一份
                File.Copy(path2, appPath + "/" + fileName);
            }
            filePaths = Directory.GetFiles(appPath + "/", "*xq-data*");
            fileName = Path.GetFileName(filePaths[0]);
            //string fileName = "xq-data.hl";
            
            //Debug.Log(File.ReadAllText(path1 + "/" + fileName));
            Debug.Log("正在解压");
            //进行解压（跟picout一起打包，解压后要将androidBundle里的文件全部移动到应用目录，picout改成pic，然后移动到应用目录）
            string dir = appPath + "/" + "upziptemp";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            strUtils.UnZip(appPath + "/" + fileName, dir, (p) =>
            {
                this.transform.Find("tip").GetComponent<TextUI>().setText("正在解压(" + p * 100 + "%)，请稍后...");
            }, () => { });
            this.transform.Find("tip").GetComponent<TextUI>().setText("正在移动，请稍后...");
            //将文件移动到应用目录
            strUtils.MoveDir(dir + "/" + "androidBundle", sysUtils.getUrl());
            Directory.Move(dir + "/" + "picout", sysUtils.getUrl() + "/pic");

            //删除这个临时解压目录
            Directory.Delete(dir, true);
            Debug.Log("解压完毕");
            //最后缓存一个可跳过此过程的数据标志
            PlayerPrefs.SetString("xq-isinto", "1");
            //删除该资源文件
            File.Delete(appPath + "/" + fileName);

            ac();
        }
        private void versionCheck(Action fn)
        {
            DoGet doget = DoGet.getInstance();
            doget.sendGet("/xqAssetsPath?t=" + strUtils.getMillis(), (res) =>
            {
                string json = strUtils.DecodeB64(Encoding.GetEncoding("UTF-8").GetString((byte[])res));
                JArray list = JsonConvert.DeserializeObject<JArray>(json);
                //读取缓存的资源判断哪些旧的资源需要移除
                for (int i = 0; i < list.Count; i++)
                {
                    JObject a = (JObject)list[i];

                    if (doget.isNeetUpdate(a["key"].ToString(), (long)a["size"]))
                    {
                        //移除资源
                        doget.removeResource(a["key"].ToString());
                        Debug.Log("移除：" + a["key"]);
                    }
                }

                //进入游戏
                fn();
            });
        }
        public override void init()
        {

        }
    }
}
