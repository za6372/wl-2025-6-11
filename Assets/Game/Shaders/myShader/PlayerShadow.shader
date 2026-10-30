Shader "myDefind/PlayerShadow"
{
	Properties
	{
	_Color("Main Color",Color) = (1,1,1,1)
	_MainTex("Base (RGB) Alpha(A)", 2D) = "white" {}
	_Cutoff("Base Alpha cutoff",Range(0,.1)) = .02
	_SpcTex("Base (RGB) Alpha (A)",2D) = "white"{}
	}
		SubShader
	{
	Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RanderType" = "TransparentCutout"}
	Lighting off
	ZWrite off
	Cull off
	Blend SrcAlpha OneMinusSrcAlpha
	Pass
	{
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag



	#include "UnityCG.cginc"




	struct appdata_t
	{
	float4 vertex : POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	};
	struct v2f
	{
	float4 vertex : SV_POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	};
	sampler2D _MainTex;
	sampler2D _SpecTex;
	float4 _MainTex_ST;
	float _Cutoff;

	v2f vert(appdata_t v)
	{

	v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o);
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.color = v.color;
	o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

	return o;
	}
	float4 _Color;
	half4 frag(v2f i) : SV_Target
	{
	half4 col = tex2D(_MainTex,i.texcoord);
	half4 col1 = tex2D(_SpecTex, i.texcoord);

	if (col.a < _Cutoff)
	{
	clip(col.a - _Cutoff);
	}
	else
	{
	col.rgb = col.rgb * float3(0, 0, 0);
	col.rgb = col.rgb + _Color;
	col.a = _Color.a;
	}
	return col;
	}
	ENDCG
	}
	}
}