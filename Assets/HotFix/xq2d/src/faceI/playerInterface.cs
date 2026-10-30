using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json.Linq;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface playerInterface
    {
        public void receivePlayersPos(JArray list);
        public void initPlayerPos();
        public JArray getPlayerList();
        public JObject getPlayerOne(string name);
        public void getPlayerPos(Action<JArray> ac);
    }

    class playerInterfaceImpl : playerInterface
    {
        public void getPlayerPos(Action<JArray> ac)
        {
            DoGet.getInstance().sendPost("/onlineService/getPlayerPos", null, (res) =>
            {
                ac((JArray)res);
            });
        }
        public void receivePlayersPos(JArray list)
        {
            dbHandle.save("playerList", list);
        }
        public void initPlayerPos()
        {
            dbHandle.save("playerList", new JArray());
        }
        public JArray getPlayerList()
        {
            return dbHandle.get<JArray>("playerList");
        }
        public JObject getPlayerOne(string name)
        {
            JArray list = this.getPlayerList();
            for (int i = 0; i < list.Count; i++)
            {
                JObject a = (JObject)list[i];
                if (a["name"].ToString().Equals(name)) return a;
            }
            return null;
        }
    }
}
