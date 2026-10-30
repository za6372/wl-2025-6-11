using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.shape
{
    /**文字描边*/
    public class UIOutline : Shadow
    {
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            var verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);

            var neededCpacity = verts.Count * 9;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            var start = 0;
            var end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, 0);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, -effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, 0);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, effectDistance.y);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, effectDistance.y);

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            
            
        }
        public UIOutline setSome(Color effectColor,Vector2 vc)
        {
            this.effectColor = effectColor;
            this.effectDistance = vc;
            return this;
        }
    }
}
