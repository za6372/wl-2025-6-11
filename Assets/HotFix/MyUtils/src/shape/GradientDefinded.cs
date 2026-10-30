using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HotFix.MyUtils.src.shape
{
    // 定义渐变方向枚举
    [AddComponentMenu("UI/Effects/Gradient")]
    public enum Dir
    {
        Horizontal, // 水平方向
        Vertical,   // 垂直方向
    }
    // 自定义梯度效果类，继承自BaseMeshEffect
    public class GradientDefinded : BaseMeshEffect
    {
        [SerializeField] // 序列化字段，可在Inspector中编辑
        public Dir dir = Dir.Vertical; // 渐变方向，默认垂直

        [SerializeField]
        public Color32 color1 = Color.white; // 渐变起始颜色，默认白色

        [SerializeField]
        public Color32 color2 = Color.white; // 渐变结束颜色，默认白色

        [SerializeField]
        public float range = 0f; // 渐变范围，控制颜色的过渡区域，默认无范围（完全渐变）

        [SerializeField]
        public bool isFlip = false; // 是否翻转渐变方向，默认不翻转
        public void toUpdate()
        {
            this.graphic.SetVerticesDirty();
        }
        // 重写ModifyMesh方法，用于修改UI元素的网格
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive()) // 如果组件未激活，则不执行后续操作
            {
                return;
            }
            
            int count = vh.currentVertCount; // 获取当前顶点数量
            if (count > 0) // 如果有顶点，则进行处理
            {
                List<UIVertex> vertices = new List<UIVertex>(); // 创建顶点列表

                // 遍历所有顶点并添加到列表中
                for (int i = 0; i < count; i++)
                {
                    UIVertex uIVertex = new UIVertex();
                    vh.PopulateUIVertex(ref uIVertex, i); // 填充顶点信息
                    vertices.Add(uIVertex);
                }

                // 根据渐变方向调用相应的绘制方法
                switch (dir)
                {
                    case Dir.Horizontal:
                        DrawHorizontal(vh, vertices, count);
                        break;
                    case Dir.Vertical:
                        DrawVertical(vh, vertices, count);
                        break;
                    default:
                        break;
                }
            }
        }

        // 绘制垂直方向的渐变
        private void DrawVertical(VertexHelper vh, List<UIVertex> vertices, int count)
        {
            // 初始化顶部和底部Y坐标
            float topY = vertices[0].position.y;
            float bottomY = vertices[0].position.y;

            // 遍历所有顶点，找到最高和最低的Y坐标
            for (int i = 0; i < count; i++)
            {
                float y = vertices[i].position.y;
                if (y > topY) topY = y;
                else if (y < bottomY) bottomY = y;
            }

            float height = topY - bottomY; // 计算高度差

            // 遍历所有顶点，设置颜色渐变
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = vertices[i];
                Color32 color = Color.white;

                // 根据是否翻转，计算当前顶点的颜色
                if (isFlip)
                {
                    color = Color32.Lerp(color2, color1, 1 - (vertex.position.y - bottomY) / height * (1f - range));
                }
                else
                {
                    color = Color32.Lerp(color2, color1, (vertex.position.y - bottomY) / height * (1f - range));
                }

                vertex.color = color; // 设置顶点颜色
                vh.SetUIVertex(vertex, i); // 更新网格中的顶点
            }
        }

        // 绘制水平方向的渐变
        private void DrawHorizontal(VertexHelper vh, List<UIVertex> vertices, int count)
        {
            // 注意：这里应该是找到最左和最右的X坐标，注释中存在笔误
            float leftX = vertices[0].position.x;
            float rightX = vertices[0].position.x;

            // 遍历所有顶点，找到最左和最右的X坐标
            for (int i = 0; i < count; i++)
            {
                float x = vertices[i].position.x;
                if (x > rightX) rightX = x;
                else if (x < leftX) leftX = x;
            }

            float width = rightX - leftX; // 计算宽度差

            // 遍历所有顶点，设置颜色渐变
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = vertices[i];
                Color32 color = Color.white;

                // 根据是否翻转，计算当前顶点的颜色
                if (isFlip)
                {
                    color = Color32.Lerp(color2, color1, 1 - (vertex.position.x - leftX) / width * (1f - range));
                }
                else
                {
                    color = Color32.Lerp(color2, color1, (vertex.position.x - leftX) / width * (1f - range));
                }

                vertex.color = color; // 设置顶点颜色
                vh.SetUIVertex(vertex, i); // 更新网格中的顶点
            }
        }
    }
}
