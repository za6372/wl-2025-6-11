using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    class buildManager
    {
        public static void isNearNpc()
        {
            
            bool b = false;
            Transform mape = PointGet.getMapMapePoint();
            Transform rl = PointGet.getControlPoint();
            Vector2 vs = PosCountUtils.controlPosRelativeMapePos(rl);

            float k = ScreenUtils.width / 1080f * ScreenUtils.screenScaleRate;

            for (int i = 0; i < mape.childCount; i++)
            {
                Transform build = mape.GetChild(i);

                if (build.name.Contains("npc_"))
                {
                    //因为npc位置比玩家位置向上偏移了100
                    if (Math.Abs(rl.position.x - build.position.x) < 100 * k &&
                    (((rl.position.y - build.position.y) < 100 * k) && ((rl.position.y - build.position.y) > -100 * k)))
                    {
                        b = true;
                        //Debug.Log("靠近怪物");
                        //靠近处理（比如遇怪）
                        npcManager.nearNpcHandle(build);
                        break;
                    }
                }
                else if (build.name.Contains("build_"))
                {
                    Vector2 size = build.GetComponent<RectTransform>().sizeDelta * k;
                    Vector2 buildVs = PosCountUtils.buildPosRelativeMapePos(build);
                    if ((vs.x - buildVs.x > 0 && vs.x - buildVs.x < size.x) &&
                    ((vs.y - buildVs.y < size.y) && (vs.y - buildVs.y > 0)))
                    {
                        b = true;
                        break;
                    }
                }

            }
            if (b)
            {
                Image[] list = rl.GetComponentsInChildren<Image>(true);
                foreach (Image img in list)
                {
                    img.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else
            {
                Image[] list = rl.GetComponentsInChildren<Image>(true);
                foreach (Image img in list)
                {
                    img.color = new Color(1, 1, 1, 1f);
                }
            }
            
        }
        public static void drawBuild(Mape mape, Action ac)
        {
            if (mape.build == null || mape.build.buildFrames.Length <= 0)
            {
                ac();
                return;
            }
            Build build = mape.build;
            Transform ts = PointGet.getMapMapePoint();
            Vector2 sizeDelta = ts.GetComponent<RectTransform>().sizeDelta;
            float ax = -sizeDelta.x / 2f;
            float ay = sizeDelta.y / 2f;
            build.pwdName = build.pwdName.Replace(".", "_");
            build.aefName = build.aefName.Replace(".", "_");
            string[] pwds = { build.pwdName };
            string[] aefs = { build.aefName };
            DoGet.getInstance().collectionAnyRes(() =>
            {
                List<GameObject> list = readDirAll.drawBuildAm(ts, pwds, aefs, GameAttrConst.getMapScaleRate());

                for (int i = 0; i < build.buildFrames.Count(); i++)
                {
                    BuildFrame buildFrame = build.buildFrames[i];
                    int x = buildFrame.x;
                    int y = buildFrame.y;

                    Transform bt = list[0].transform.Find("frame_" + buildFrame.id);
                    if (bt == null)
                    {
                        continue;
                    }
                    GameObject buildObj = bt.gameObject;
                    //要复制出来
                    GameObject a = GameObject.Instantiate(buildObj);
                    a.SetActive(true);
                    a.layer = 5;
                    a.name = "build_" + buildFrame.id;
                    a.transform.SetParent(ts, false);
                    Vector2 size = a.GetComponent<RectTransform>().sizeDelta;

                    Texture2D td = a.GetComponent<Image>().sprite.texture;
                    float rate = GameAttrConst.getMapScaleRate();
                    float dx = -sizeDelta.x / 2f + x * rate;
                    float dy = sizeDelta.y / 2f - (y + td.height) * rate;
                    a.transform.localPosition = new Vector2(dx, dy);

                }
                gameObjPool.getInstance().free(list[0]);
                ac();
            }, build.pwdName, build.aefName);


        }
    }
}
