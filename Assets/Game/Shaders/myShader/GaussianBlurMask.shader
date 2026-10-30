//模糊滤镜（在屏幕最上层建一个img，然后创建一个材质，再把材质设置到img）
Shader "myDefind/GaussianBlurMask"
{
    Properties
    {
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        _BlurSize("Blur Size", Float) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha

            GrabPass { }                        //不带参数名，默认抓取到_GrabTexture，否则只会抓取一次

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct a2v
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float4 scrPos : TEXCOORD0;
                    float2 uv : TEXCOORD1;
                    fixed4 color : TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _BlurSize;

                sampler2D _GrabTexture;             //GrabPass抓取的纹理
                float4 _GrabTexture_TexelSize;      //纹理大小

                v2f vert(a2v v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.scrPos = ComputeGrabScreenPos(o.pos);                     //抓取屏幕的坐标
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 diffuse = tex2D(_MainTex, i.uv);
                    fixed4 col = diffuse * i.color;

                    float weight[3] = {0.4026, 0.2442, 0.0545};                 //一维5*5高斯滤波参数，乘起来就是二维的正态分布
                    fixed3 sum = fixed3(0, 0, 0);

                    for (int x = -2; x <= 2; x++)
                    {
                        for (int y = -2; y <= 2; y++)
                        {
                            fixed2 offset = fixed2(x ,y) * _GrabTexture_TexelSize.xy * _BlurSize;
                            fixed2 cur_UVxy = offset * i.scrPos.z + i.scrPos.xy;
                            sum += tex2D(_GrabTexture,cur_UVxy / i.scrPos.w).rgb * weight[abs(x)] * weight[abs(y)];
                        }
                    }
                    return col * fixed4(sum,1);
                }
                ENDCG
            }
        }
}