Shader "myDefind/FogEffect"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {} // 主纹理
		_FogColor("Fog Color", Color) = (1, 0, 0, 1) // 雾的颜色
		_MinDist("Min Dist", Range(0, 100000)) = 1 // 雾的最近距离(线性衰减函数才生效)
		_MaxDist("Max Dist", Range(30, 100000)) = 100000 // 雾的最远距离(线性衰减函数才生效)
		_R("Density", Range(0, 1)) = 0.01 // 衰减速率参数(反比衰减和指数衰减函数才生效)
	}

		SubShader{
			Pass {
				// 深度测试始终通过, 关闭深度写入
				ZTest Always ZWrite Off

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex; // 主纹理
				sampler2D _CameraDepthTexture; // 深度纹理
				float4x4 _FrustumCornersRay; // 视锥体四角射线向量(由相机指向近平面上四个角点的向量除以near后的坐标)
				fixed4 _FogColor; // 雾的颜色
				float _MinDist; // 雾的最近距离(线性衰减函数才生效)
				float _MaxDist; // 雾的最远距离(线性衰减函数才生效)
				float _R; // 衰减速率参数(反比衰减和指数衰减函数才生效)

				struct v2f {
					float4 pos : SV_POSITION; // 裁剪空间顶点坐标
					half2 uv : TEXCOORD0; // 纹理uv坐标
					float4 interpolatedRay : TEXCOORD1; // 插值射线向量(由相机指向近平面上点的向量除以near后的坐标)
				};

				float4 getInterpolatedRay(half2 uv) { // 获取插值射线向量(由相机指向近平面上四个角点的向量除以near后的坐标)
					int index = 0;
					if (uv.x < 0.5 && uv.y < 0.5) {
						index = 0;
					}
	 else if (uv.x > 0.5 && uv.y < 0.5) {
	  index = 1;
	  }
	else if (uv.x > 0.5 && uv.y > 0.5) {
	 index = 2;
	}
	else {
	 index = 3;
	}
	return _FrustumCornersRay[index];
	}

	float getFactory(float len) { // 获取雾效因子
		float factor = saturate((_MaxDist - len) / (_MaxDist - _MinDist)); // 线性雾效衰减因子
		//float factor = 1 / (_R * len + 1); // 反比雾效衰减因子
		//float factor = exp(-_R * len); // 指数雾效衰减因子
		return factor;
	}

	v2f vert(appdata_img v) { // 顶点着色器中只处理4个顶点, 每个顶点对应了1条射线(O点指向近平面的四角)
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex); // 计算裁剪坐标系中顶点坐标, 等价于: mul(unity_MatrixMVP, v.vertex)
		o.uv = v.texcoord;
		o.interpolatedRay = getInterpolatedRay(v.texcoord); // 获取插值射线向量(由相机指向近平面上四个角点的向量除以near后的坐标)
		return o;
	}

	fixed4 frag(v2f i) : SV_Target{ // v2f_img为内置结构体, 里面只包含pos和uv
		float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv); // 非线性的深度, tex2D(_CameraDepthTexture, i.uv).r
		float linearDepth = LinearEyeDepth(depth); // 线性的深度
		//if (linearDepth > _ProjectionParams.z - 2) {
		//  return tex2D(_MainTex, i.uv); // 天空不参与雾效
		//}
		float3 viewPos = linearDepth * i.interpolatedRay.xyz; // 顶点在相机坐标系下的坐标(不是观察坐标系, 观察坐标系与相机坐标系z轴方向相反)
		float len = length(viewPos);
		float factor = getFactory(len); // 获取雾效因子
		fixed4 tex = tex2D(_MainTex, i.uv);
		fixed4 color = lerp(_FogColor, tex, factor);
		return color;
	}

	ENDCG
	}
		}

			FallBack off

}
