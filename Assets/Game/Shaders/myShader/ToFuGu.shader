//Í¼Æ¬¸´¹Å·ç¸ñ
Shader "myDefind/ToFuGu"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _VignetteIntensity("Vignette Intensity", Range(0, 1)) = 0.5
        _Saturation("Saturation", Range(0, 1)) = 0.8
    }
        SubShader
        {
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
                float _VignetteIntensity;
                float _Saturation;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                // Vignette effect
                float vFactor = (1 - distance(i.uv, float2(0.5, 0.5)));
                col *= lerp(1, vFactor, _VignetteIntensity);
                // Desaturate effect (simplified)
                float luminance = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;
                fixed3 greyScaleColor = lerp(col.rgb, luminance.xxx, 1 - _Saturation);
                col.rgb = greyScaleColor;
                return col;
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}