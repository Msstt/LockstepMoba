Shader "UI/BarWithTick"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FillAmount("Fill Amount", Range(0,1)) = 1
        _TickRange("Tick Range", Float) = 5
        _TickWidth("Tick Width", Float) = 0.005
        _TickColor("Tick Color", Color) = (0,0,0,1)
        _BarColor("Bar Color", Color) = (0,1,0,1)
        _BackgroundColor("Background Color", Color) = (0.3,0.3,0.3,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FillAmount;
            float _TickRange;
            float _TickWidth;
            float4 _TickColor;
            float4 _BarColor;
            float4 _BackgroundColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 背景
                float4 col = _BarColor;

                // 血条 fill
                // if (uv.x <= _FillAmount)
                //     col = _BarColor;
                
                // 刻度
                for (int t = 1000; t <= _TickRange + 500; t += 1000)
                {
                    float tickPos = t / _TickRange;
                    if (abs(uv.x - tickPos) < (_TickWidth / _MainTex_ST.x)) // 根据 UI 宽度调整
                    {
                        col = _TickColor;
                    }
                }

                for (int t = 100; t <= _TickRange + 50; t += 100)
                {
                    float tickPos = t / _TickRange;
                    if (abs(uv.x - tickPos) < (_TickWidth / _MainTex_ST.x) && uv.y > 0.5) // 根据 UI 宽度调整
                    {
                        col = _TickColor;
                    }
                }

                return col;
            }
            ENDCG
        }
    }
}
