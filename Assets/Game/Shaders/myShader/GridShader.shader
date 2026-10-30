//Õ¤¸ñÂË¾µ£¨Òº¾§£©
Shader "myDefind/GridShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GridSize("Grid Size", Float) = 100
        _Color1("Color 1", Color) = (0,0,0,0.11)
        _Color2("Color 2", Color) = (1,1,1,0)
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
                float _GridSize;
                float4 _Color1;
                float4 _Color2;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 grid = abs(frac(i.uv * _GridSize) * 2 - 1);
                    grid = smoothstep(0.45, 0.55, grid); // Smooth edges of the grid lines.
                    float checker = step(frac(i.uv.x * _GridSize), 0.5) * step(frac(i.uv.y * _GridSize*2), 0.5); // Create checker pattern.
                    fixed4 color = lerp(_Color1, _Color2, checker); // Mix colors based on checker pattern.
                    fixed4 texColor = tex2D(_MainTex, i.uv); // Sample texture color.
                    return color * texColor; // Combine texture color with grid color.
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}