//将图片变成像素风
Shader "myDefind/LCDShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Float) = 4
        _Brightness("Brightness", Float) = 1.0
        _Contrast("Contrast", Float) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

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
                float _PixelSize;
                float _Brightness;
                float _Contrast;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Pixelation effect
                    float2 pixelUV = floor(i.uv * _PixelSize) / _PixelSize;
                    // Brightness and contrast effect can be added here using simple math operations.
                    fixed4 col = tex2D(_MainTex, pixelUV);
                    // Apply brightness and contrast adjustments if needed. For example:
                    col = (col - 0.5) * _Contrast + 0.5; // Adjust contrast first, then brightness.
                    col *= _Brightness; // Apply brightness.
                    return col;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}