using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src.factory.basicObj
{
    public class lightObj : baseObj
    {
        public static lightObj getInstance(string name)
        {
            GameObject g = new GameObject(name);
            return g.AddComponent<lightObj>();
        }

        /**创建灯光*/
        public lightObj init(LightType type, string color, float intensity, int cullingMask = 1 << 0)
        {
            Light light = this.gameObject.AddComponent<Light>();
            light.type = type;
            light.color = strUtils.toRGBColor(color);
            light.intensity = intensity;
            //light.lightmapBakeType = LightmapBakeType.Baked;
            light.cullingMask = cullingMask;
            return this;
        }
        public lightObj init(LightType type, Color32 color, float intensity, int cullingMask = 1 << 0)
        {
            Light light = this.gameObject.AddComponent<Light>();
            light.type = type;
            light.color = color;
            light.intensity = intensity;
            //light.lightmapBakeType = LightmapBakeType.Baked;
            light.cullingMask = cullingMask;
            return this;
        }
    }
}
