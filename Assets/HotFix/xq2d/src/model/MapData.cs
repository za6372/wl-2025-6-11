using Assets.Res.script.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Res.script.src.model
{
    public class MapData
    {
        public string key;
        //对应mape的id，无后缀名
        public string mapeId;
        public string name;
        public string des;
        public JObject manPos;
        public List<string> petKeys;

        public MapData(string key)
        {
            this.key = key;
        }
        /**出没的宠物key*/
        public MapData addPetKeys(params string[] petKeys)
        {
            if (this.petKeys == null) this.petKeys = new List<string>();
            this.petKeys.AddRange(petKeys);
            return this;
        }
        /**是否为野区*/
        public bool isYeQu()
        {
            if (this.petKeys != null) return true;
            return false;
        }
    }
}
