using Assets.HotFix.xq2d.src.data;
using Assets.HotFix.xq2d.src.model;
using Assets.HotFix.xq2d.src.ui.basicUI;
using Assets.Res.script.src.model;
using Assets.Res.script.src.touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.HotFix.MyUtils.src.data;
using Assets.HotFix.xq2d.src.faceI;
using Assets.HotFix.xq2d.src.common;
using Assets.HotFix.MyUtils.src.common;
using Assets.HotFix.MyUtils.src.factory;
using Assets.HotFix.MyUtils.src.ui.basicUI;
using Assets.HotFix.xq2d.src.factory.playerObjBehaviour;
using Assets.HotFix.xq2d.src.ui.part;
using Assets.HotFix.xq2d.src.fight;

namespace Assets.HotFix.xq2d.src.factory.manager
{
    class mapManager
    {
        //传送点记录的上一个地图位置
        private string prevMapKey;

        private static mapManager mg;
        public static mapManager getInstance()
        {
            if (mg == null)
            {
                mg = new mapManager();
            }
            return mg;
        }
        /**仅传送点使用，缓存上个地图位置*/
        public void savePreMapKey()
        {
            JObject r = face.roleInterface.getRole();
            this.prevMapKey = r["pos"]["map"].ToString();
        }
        /**点击任务触发的寻路*/
        public void findRoad(string taskKey, int progressIndex)
        {
            task ts = face.taskInterface.getTask(taskKey);
            ts.progressIndex = progressIndex;
            JObject map = ts.getTargetPos();
            findRoad(map);
        }
        /**通过地图key和npcKey来寻路*/
        public void findRoad(string mapKey, string npcKey)
        {
            JObject map = new JObject();
            map.Add("mapKey", mapKey);
            map.Add("npcKey", npcKey);
            findRoad(map);
        }
        /**map {mapKey,pos} */
        private void findRoad(JObject map)
        {
            if (map == null || map["npcKey"] == null)
            {
                //提示位置不可寻找
                //textTipUI.getInstance().setText("该场景不能寻路");
                return;
            }
            Action<string> fn = (npcKey) =>
            {
                GameObject n = npcManager.getNpcByKey(npcKey);
                if (n == null)
                {
                    return;
                }
                //找到npc直接走
                Move pm = PointGet.getMapPoint().GetComponent<Move>();
                pm.putTargetPos(n.transform.position * 1f);
            };
            //判断当前地图是否是寻路的地图
            JObject r = face.roleInterface.getRole();
            if (r["pos"]["map"].ToString().Equals(map["mapKey"].ToString()))
            {
                //直接寻找npc
                fn(map["npcKey"].ToString());
            }
            else
            {
                //先跳转地图再寻找npc
                LoadingUI.getOne();
                reloadBef(map["mapKey"].ToString(), () =>
                {
                    fn(map["npcKey"].ToString());
                });
            }
        }
        public void reloadBef()
        {
            string mapKey = null;
            if (face.roleInterface.getRole()["pos"] == null || face.roleInterface.getRole()["pos"]["map"] == null)
            {
                mapKey = "m_1";
            }
            else
            {
                mapKey = face.roleInterface.getRole()["pos"]["map"].ToString();
            }
            reloadBef(mapKey);
        }
        public void reloadBef(string mapKey)
        {
            reload(mapKey, null);
        }
        public void reloadBef(string mapKey, Action callback)
        {
            reload(mapKey, callback);
        }
        public bool isDoingLoadingMap = false;
        private void reload(string mapKey, Action callback)
        {
            isDoingLoadingMap = true;
            //重置遇怪计数
            MonsterCreateHandle.getInstance().resetNum();

            LoadingUI.getOne().updateJd(0, "开始加载场景");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("key", mapKey);
            DoGet.getInstance().sendPost("/mapService/toMap", dic, (res) =>
            {
                
                try
                {
                    mapDataCache = res;
                    reDrawMap(() =>
                    {
                        callback?.Invoke();
                        isDoingLoadingMap = false;
                    });
                    /*timeManager.getTimeManageOne().putDelayTask(() =>
                    {
                        reDrawMap(() =>
                        {
                            callback?.Invoke();
                            isDoingLoadingMap = false;
                        });
                    }, 5000);*/

                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                    
                }
            });
        }
        private object mapDataCache;
        public void reDrawMap(Action callback)
        {
            float num = 0;
            float sum = 6;
            LoadingUI.getOne().updateJd((++num) / sum, "加载场景 " + num + "/" + sum);

            JObject obj = (JObject)mapDataCache;
            string mapKey = obj["key"].ToString();
            //Debug.Log(obj);

            MapData mapData = face.mapInterface.getMapByKey(mapKey);
            //修改地图名
            PointGet.getIndexPage().Find("Top/Title").GetComponent<Text>().text = mapData.name;
            //先清理npc、建筑等
            clearNpcAndBuild();
            //请求mape文件
            DoGet.getInstance().collectionAnyRes(() =>
            {
                LoadingUI.getOne().updateJd((++num) / sum, "加载场景 " + num + "/" + sum);

                Mape mape = readMape(mapData);
                mape.mapKey = mapKey;


                getMiniPic(mape, () =>
                {
                    LoadingUI.getOne().updateJd((++num) / sum, "加载场景 " + num + "/" + sum);

                    drawMap(mape);

                    Transform mp = PointGet.getMapPoint();
                    Transform grid = mp.Find("mape");
                    RectTransform rt = grid.GetComponent<RectTransform>();
                    Debug.Log("1.mape是否为null?" + (grid == null));
                    float rate = GameAttrConst.getMapScaleRate();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x * rate, rt.sizeDelta.y * rate);
                    SetGameObj.setLeftPos(Vector2.zero, grid.gameObject);


                    buildManager.drawBuild(mape, () =>
                    {
                        Debug.Log("2.mape是否为null?" + (PointGet.getMapMapePoint() == null));
                        LoadingUI.getOne().updateJd((++num) / sum, "加载场景 " + num + "/" + sum);

                        npcManager.drawNpc((JArray)obj["list"], mape, () =>
                        {
                            Debug.Log("3.mape是否为null?" + (PointGet.getMapMapePoint() == null));

                            Action fn = () =>
                            {
                                //控制玩家移动脚本
                                Move mv = (Move)SetGameObj.addComponent(PointGet.getMapPoint().gameObject, typeof(Move));
                                //因为地图的规格大小可能不同，所以需要重置一下限制区
                                mv.init();

                                LoadingUI.getOne().updateJd((++num) / sum, "加载场景 " + num + "/" + sum);

                                Transform player = PointGet.getControlPoint();

                                //读取玩家在此地图的初始位置
                                MapData mapMsg = face.mapInterface.getMapByKey(mapKey);
                                JObject manPos = mapMsg.manPos;
                                JObject r = face.roleInterface.getRole();
                                if (mapKey.Equals(r["pos"]["map"].ToString()) && !strUtils.isNull(r["pos"]["pos"]))
                                {
                                    //从其他场景切换回来就取用缓存的位置，跳转地图时不需要调取
                                    //manPos = (JObject)r["pos"]["pos"];
                                    JObject e = (JObject)r["pos"]["pos"];
                                    //因为保存的是相对于mape的位置，所以要转换成position
                                    Vector2 pos = PosCountUtils.relativeMapePosToControlPosition(new Vector2((float)e["x"], (float)e["y"]));
                                    manPos = new JObject();
                                    manPos["x"] = pos.x;
                                    manPos["y"] = pos.y;
                                    //Debug.Log("==============读取原位置===============");
                                }

                                if (true)
                                {
                                    npcManager.allowedMove();


                                    //将人物位置移动到传送点位置
                                    Vector2 vs = transmitHanle(grid, manPos);

                                    //可能恢复战斗时导致这里为null
                                    if (player == null) return;
                                    //设置位置并上传
                                    //player.localPosition = vs;
                                    player.position = vs;

                                    //保存地图
                                    face.roleInterface.saveRolePos(mapKey);
                                    Vector2 pos = PosCountUtils.controlPosRelativeMapePos(player.transform);
                                    face.roleInterface.isUploadPos(pos);


                                    //当位置超出显示区域时，将人物设置到屏幕中心 高的范围是90-(ScreenUtils.height-396)
                                    float centerX = 0;
                                    float centerY = -90;
                                    //地图位置偏移
                                    float dx = player.localPosition.x - centerX;
                                    float dy = player.localPosition.y - centerY;
                                    //当超出地图限制区处理
                                    Vector4 v4 = overMapLimitHandle(grid, dx, dy, centerX, centerY);

                                    player.localPosition = new Vector2(v4.x, v4.y);
                                    //显示宠物
                                    controlManager.updatePetModel();
                                    //显示橙装特效
                                    controlManager.updateCzTx();
                                    //显示变身卡提示
                                    controlManager.updateShenFu();

                                    //地图平移
                                    grid.localPosition = new Vector2(v4.z, v4.w);

                                    //允许加载其他玩家
                                    playerManager.getInstance().reLoad();

                                    //需要先保存地图后才刷新任务图标，比如天渊
                                    npcManager.updateTaskIcon();

                                    LoadingUI.getOne().free();

                                    //3s后才允许再次遇怪
                                    timeManager.getTimeManageOne().putDelayTask(() =>
                                    {
                                        fightCache.getInstance().isDoing = false;
                                    }, 3000);

                                    if (mapKey.Equals("sgzc") && netUtils.getInstance().body["mzzd"].ToString().Equals("0"))
                                    {
                                        //对于上古战场需要弹出跟魔尊战斗的确认框
                                        timeManager.getTimeManageOne().putDelayTask(() =>
                                        {
                                            MsgSureUI.create(PointGet.getTipCanvas()).setMaskAlpha(0).maskNoClk()
                                            .show("秦朝末年，烽烟四起，征战不断。百姓尸横遍野，九州血流成河。被封印许久的昆仑绝地在此时突然开启，妖界之门大开，无数妖魔蜂拥而出，妄图倾覆天下......", () =>
                                            {
                                                            //跟魔尊进行战斗
                                                            face.fightInterface.attackMozun();
                                            }, () =>
                                            {
                                                face.fightInterface.attackMozun();
                                            });
                                        }, 5000);
                                    }

                                    //自动遇怪
                                    mv.continueAutoMove();



                                    if (callback != null) callback();
                                }

                            };

                            if (PointGet.getControlPoint() == null)
                            {
                                controlManager.drawPlayer(mape, () =>
                                {
                                    fn();
                                });
                            }
                            else
                            {
                                fn();
                            }
                        });

                    });


                });

            }, mapData.mapeId + "_mape");
        }
        /**传送点跳转时处理*/
        private Vector2 transmitHanle(Transform grid, JObject manPos)
        {
            //manpos是玩家position位置，不是localposition
            Vector2 vs = new Vector2((float)manPos["x"], (float)manPos["y"]);
            //不经过传送点的直接返回
            if (prevMapKey == null) return vs;

            Transform mit = npcManager.getTransmit(prevMapKey);
            prevMapKey = null;
            RectTransform rt1 = grid.GetComponent<RectTransform>();
            Vector2 sceneSize = rt1.sizeDelta;
            Vector2 temp = mit.transform.position;
            if (temp.x < ScreenUtils.width / 2f)
            {
                vs.x = temp.x + 200;// - ScreenUtils.width / 2f + sceneSize.x / 2f;
            }
            else if (temp.x > ScreenUtils.width / 2f)
            {
                vs.x = temp.x - 200;// - ScreenUtils.width / 2f + sceneSize.x / 2f;
            }
            if (temp.y < ScreenUtils.height / 2f)
            {
                vs.y = temp.y + 200 - 90;// + ScreenUtils.height / 2f - sceneSize.y / 2f;
            }
            else if (temp.y > ScreenUtils.height / 2f)
            {
                vs.y = temp.y - 200 - 90;// + ScreenUtils.height / 2f - sceneSize.y / 2f;
            }
            return vs;
        }
        /**超出限制区时人物、地图位置的修正*/
        private Vector4 overMapLimitHandle(Transform grid, float dx, float dy, float centerX, float centerY)
        {
            RectTransform rt1 = grid.GetComponent<RectTransform>();
            Vector2 sceneSize = rt1.sizeDelta;
            Vector2 scenePos = grid.transform.localPosition;
            //计算地图四个极限位置
            float leftLimit = scenePos.x - (sceneSize.x - ScreenUtils.width);
            float rightLimit = scenePos.x;
            float topLimit = scenePos.y + (sceneSize.y - (ScreenUtils.height - 300));
            float bottomLimit = scenePos.y;
            //判断此偏移量是否会导致地图超出限制区
            float gx = grid.localPosition.x - dx;
            float gy = grid.localPosition.y - dy;
            if (gx < leftLimit)
            {
                float overX = leftLimit - gx;
                gx = leftLimit;
                centerX += overX;
            }
            else if (gx > rightLimit)
            {
                float overX = rightLimit - gx;
                gx = rightLimit;
                centerX += overX;
            }
            if (gy < bottomLimit)
            {
                float overY = bottomLimit - gy;
                gy = bottomLimit;
                centerY += overY;
            }
            else if (gy > topLimit)
            {
                float overY = topLimit - gy;
                gy = topLimit;
                centerY += overY;
            }
            return new Vector4(centerX, centerY, gx, gy);
        }
        public void loadFightMap(string mapKey, Action ac)
        {
            MapData mapData = face.mapInterface.getMapByKey(mapKey);
            DoGet.getInstance().collectionAnyRes(() =>
            {

                Mape mape = readMape(mapData);
                mape.mapKey = mapKey;
                getMiniPic(mape, () =>
                {
                    drawMap(mape);

                    Transform mp = PointGet.getMapPoint();
                    Transform grid = mp.Find("mape");
                    RectTransform rt = grid.GetComponent<RectTransform>();
                    float rate = GameAttrConst.getMapScaleRate();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x * rate, rt.sizeDelta.y * rate);
                    SetGameObj.setLeftPos(Vector2.zero, grid.gameObject);
                    ac();
                });

            }, mapData.mapeId + "_mape");
        }
        private Mape readMape(MapData mapData)
        {
            byte[] bs = DoGet.getInstance().readAny(mapData.mapeId + "_mape");

            Mape mape = new Mape();
            using (MemoryStream stream = new MemoryStream(bs))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    strUtils.ReadInt16(br);//0
                    int len = br.ReadBytes(1)[0];
                    br.ReadBytes(len);
                    len = strUtils.ReadInt16(br);
                    mape.fileName = Encoding.UTF8.GetString(br.ReadBytes(len));
                    byte d4 = br.ReadBytes(1)[0];
                    byte d1 = br.ReadBytes(1)[0];
                    byte d2 = br.ReadBytes(1)[0];
                    byte d3 = br.ReadBytes(1)[0];
                    mape.row = strUtils.ReadInt32(br);
                    mape.col = strUtils.ReadInt32(br);
                    mape.kw = strUtils.ReadInt32(br);
                    mape.kh = strUtils.ReadInt32(br);
                    mape.setRGBA(d1, d2, d3, d4, mape.kw, mape.kh);
                    strUtils.ReadInt16(br);
                    strUtils.ReadInt16(br);
                    strUtils.ReadInt16(br);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        if (!doHandleFloor(br, mape))
                        {
                            break;
                        }
                    }
                }
            }
            return mape;

        }
        /**将地图中所用到的图块下载到本地*/
        private void getMiniPic(Mape mape, Action callback)
        {
            string[] newPics = new string[mape.pics.Length];
            string[] pics = mape.pics;
            for (int i = 0; i < pics.Length; i++)
            {
                string pn = pics[i].Replace(".", "_");
                newPics[i] = pn;
            }
            mape.pics = newPics;
            DoGet.getInstance().collectionAnyRes(mape.pics, () =>
            {
                callback();
            });
        }
        private void drawMap(Mape mape)
        {
            string[] pics = mape.pics;
            int mRow = mape.row;
            int mCol = mape.col;
            int[] arr = mape.arr;
            Color[] bgColorArr = mape.bgColorArr;
            int idSum = 0;
            Texture2D[] txs = new Texture2D[pics.Length];
            int[] idStart = new int[pics.Length];
            int[] idEnd = new int[pics.Length];
            for (int i = 0; i < pics.Length; i++)
            {
                byte[] bs = DoGet.getInstance().readAny(pics[i]);
                Texture2D td = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                td.wrapMode = TextureWrapMode.Clamp;
                td.filterMode = FilterMode.Point;
                td.LoadImage(bs);
                td.Apply();
                txs[i] = td;

                idStart[i] = idSum;
                idSum += (td.width / 16) * (td.height / 16);
                idEnd[i] = idSum;
            }
            Texture2D spriteTexture = new Texture2D(16 * mCol, 16 * mRow, TextureFormat.RGBA32, false);
            spriteTexture.wrapMode = TextureWrapMode.Clamp;
            spriteTexture.filterMode = FilterMode.Point;
            string str3 = "";
            for (int i = 0; i < arr.Length; i++)
            {
                int value = arr[i];
                
                int x = i % mCol;
                int y = i / mCol;
                str3 += value + "(" + x + "," + y + "),";
                int posX = x * 16;
                int posY = (mRow - 1) * 16 - y * 16;

                int id = Math.Abs(value) - 1;
                Texture2D td = null;
                int startId = 0;
                for (int p0 = 0; p0 < idStart.Length; p0++)
                {
                    if (id >= idStart[p0] && id < idEnd[p0])
                    {
                        td = txs[p0];
                        startId = idStart[p0];
                        break;
                    }
                }
                if (td == null)
                {
                    spriteTexture.SetPixels(posX, posY, 16, 16, bgColorArr);
                    continue;
                }
                int xw = td.width / 16;
                int xh = td.height / 16;
                id = id - startId;
                int idX = id % xw * 16;
                int idY = (td.height - 16) - id / xw * 16;
                spriteTexture.SetPixels(posX, posY, 16, 16, td.GetPixels(idX, idY, 16, 16));

            }
            //Debug.Log(str3);
            spriteTexture.Apply();

            Transform ts = PointGet.getMapPoint();
            GameObject spriteObj = null;
            Image renderer;
            if (ts.Find("mape") != null)
            {
                spriteObj = ts.Find("mape").gameObject;
                renderer = spriteObj.GetComponent<Image>();
            }
            else
            {
                spriteObj = gameObjPool.getInstance().get("mape", typeof(SimpleUI));
                spriteObj.layer = 5;
                renderer = spriteObj.AddComponent<Image>();
                spriteObj.transform.SetParent(ts, false);

            }


            renderer.sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f));

            SetGameObj.setSize(new Vector2(spriteTexture.width, spriteTexture.height), spriteObj);

        }
        private bool doHandleFloor(BinaryReader br, Mape mape)
        {
            int floor = strUtils.ReadInt16(br);
            switch (floor)
            {
                case 1: { break; }
                case 2: { break; }
                case 3: { break; }
                case 4: { break; }
                case 5: { return handleNpc(br, mape); }
                case 6: { return handleBuild(br, mape); }
                case 7: { break; }
                case 8: { return handleSurface(br, mape); }
                case 9: { return handleDoor(br, mape); }
            }
            return true;
        }
        private bool handleSurface(BinaryReader br, Mape mape)
        {
            int len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            strUtils.ReadInt32(br);

            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            int num = strUtils.ReadInt32(br);
            string[] pics = new string[num];
            for (int i = 0; i < num; i++)
            {
                len = strUtils.ReadInt16(br);
                pics[i] = Encoding.UTF8.GetString(br.ReadBytes(len));
                strUtils.ReadInt32(br);
                strUtils.ReadInt32(br);
            }
            mape.pics = pics;


            strUtils.ReadInt32(br);
            strUtils.ReadInt32(br);
            strUtils.ReadInt32(br);

            len = strUtils.ReadInt32(br);

            int[] arr = new int[len];
            for (int i = 0; i < len; i++)
            {
                int b = strUtils.ReadInt32(br);
                arr[i] = b;
            }
            mape.arr = arr;


            strUtils.ReadInt16(br);
            return true;
        }
        private bool handleDoor(BinaryReader br, Mape mape)
        {
            int len = strUtils.ReadInt16(br);
            if (len == 0) return true;
            br.ReadBytes(len);
            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            int num = strUtils.ReadInt32(br);
            for (int i = 0; i < num; i++)
            {
                int a = strUtils.ReadInt32(br);
                int b = strUtils.ReadInt32(br);
                int c = strUtils.ReadInt32(br);
                int d = strUtils.ReadInt32(br);
                int e = strUtils.ReadInt32(br);
                int f = strUtils.ReadInt32(br);
                len = strUtils.ReadInt16(br);
                br.ReadBytes(len);
                strUtils.ReadInt32(br);
            }


            return true;
        }
        /**对npc层的处理*/
        private bool handleNpc(BinaryReader br, Mape mape)
        {
            int len = strUtils.ReadInt16(br);
            if (len == 0) return true;
            br.ReadBytes(len);
            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            strUtils.ReadInt16(br);

            strUtils.ReadInt16(br);
            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);
            strUtils.ReadInt32(br);

            strUtils.ReadInt32(br);
            len = strUtils.ReadInt16(br);

            mape.npc = new Npc();
            string picName = Encoding.UTF8.GetString(br.ReadBytes(len));
            mape.npc.picName = picName;
            int n = strUtils.ReadInt16(br);
            if (n > 0)
            {
                string picName1 = Encoding.UTF8.GetString(br.ReadBytes(n));
                int t = strUtils.ReadInt32(br);
                mape.npc.arr = new List<NpcData>();
                for (int i = 0; i < t; i++)
                {
                    int t0 = strUtils.ReadInt32(br);
                    int t1 = strUtils.ReadInt32(br);
                    int x = strUtils.ReadInt32(br);
                    int y = strUtils.ReadInt32(br);
                    int w = strUtils.ReadInt32(br);
                    int h = strUtils.ReadInt32(br);
                    len = strUtils.ReadInt16(br);
                    string mark = Encoding.UTF8.GetString(br.ReadBytes(len));
                    strUtils.ReadInt32(br);
                    mape.npc.arr.Add(new NpcData(mark, x, y, w, h, t0, t1));
                }
                if (br.BaseStream.Position >= br.BaseStream.Length) return true;
            }
            int num = strUtils.ReadInt16(br);
            mape.npc.arr = new List<NpcData>();
            for (int i = 0; i < num; i++)
            {
                int t0 = strUtils.ReadInt32(br);
                int t1 = strUtils.ReadInt32(br);
                int x = strUtils.ReadInt32(br);
                int y = strUtils.ReadInt32(br);
                int w = strUtils.ReadInt32(br);
                int h = strUtils.ReadInt32(br);
                                               
                len = strUtils.ReadInt16(br);
                string mark = Encoding.UTF8.GetString(br.ReadBytes(len));
                strUtils.ReadInt32(br);
                mape.npc.arr.Add(new NpcData(mark, x, y, w, h, t0, t1));
            }



            return true;
        }
        private bool handleBuild(BinaryReader br, Mape mape)
        {

            int len = strUtils.ReadInt16(br);
            if (len == 0) return true;
            br.ReadBytes(len);
            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);

            strUtils.ReadInt32(br);

            len = strUtils.ReadInt16(br);
            br.ReadBytes(len);

            mape.build = new Build();


            len = strUtils.ReadInt16(br);
            mape.build.aefName = Encoding.UTF8.GetString(br.ReadBytes(len));

            strUtils.ReadInt32(br);

            len = strUtils.ReadInt16(br);
            mape.build.pwdName = Encoding.UTF8.GetString(br.ReadBytes(len));

            int num = strUtils.ReadInt32(br);
            mape.build.buildFrames = new BuildFrame[num];
            for (int i = 0; i < num; i++)
            {
                int w = strUtils.ReadInt32(br);
                int h = strUtils.ReadInt32(br);
                int t0 = strUtils.ReadInt32(br);
                int t1 = strUtils.ReadInt32(br);
                                                
                int x = strUtils.ReadInt32(br);
                int y = strUtils.ReadInt32(br);

                len = strUtils.ReadInt16(br);
                br.ReadBytes(len);
                int id = strUtils.ReadInt32(br);
                mape.build.buildFrames[i] = new BuildFrame(id, w, h, x, y, t0, t1);
            }
            return true;
        }
        private void clearNpcAndBuild()
        {
            Transform mp = PointGet.getMapPoint();
            Transform mpp = mp.Find("mape");
            if (mpp != null)
            {
                List<int> ids = new List<int>();
                for (int i = 0; i < mpp.childCount; i++)
                {
                    if (mpp.GetChild(i).name.Contains("build_") || mpp.GetChild(i).name.Contains("npc_"))
                    {
                        ids.Add(mpp.GetChild(i).GetInstanceID());
                    }
                }
                foreach (int id in ids)
                {
                    for (int i = 0; i < mpp.childCount; i++)
                    {
                        Transform d = mpp.GetChild(i);
                        if (d.GetInstanceID() == id)
                        {
                            //GameObject.Destroy(d.gameObject);
                            gameObjPool.getInstance().free(d.gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }
}
