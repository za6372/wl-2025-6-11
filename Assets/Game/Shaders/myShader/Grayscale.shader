//灰度滤镜
Shader "myDefind/Grayscale"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Alphy("Alphy", Float) = 0.2
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}
        Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Alphy;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114)); // 计算灰度值
                return fixed4(gray, gray, gray, _Alphy); // 返回灰度颜色和原始alpha值
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}