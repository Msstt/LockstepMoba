Shader "World/FogOfWarUpscale2" {
    Properties {
        _MainTex("Source Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType" = "Opaque"
        }
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            // ref: https://www.riotgames.com/en/news/story-fog-and-war
            static const half pattern[16][16] = {
                { 0, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0,},
                
                { 0, 0, 0.5, 1,
                    0, 0, 0, 0.5,
                    0, 0, 0, 0,
                    0, 0, 0, 0,},

                { 1, 0.5, 0, 0,
                    0.5, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0,},
                
                { 1, 1, 1, 1,
                    1, 1, 1, 1,
                    0, 0, 0, 0,
                    0, 0, 0, 0,},
                
                { 0, 0, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0.5,
                    0, 0, 0.5, 1,},
                
                { 0, 0, 1, 1,
                    0, 0, 1, 1,
                    0, 0, 1, 1,
                    0, 0, 1, 1,},
                
                { 1, 0.5, 0, 0,
                    0.5, 0, 0, 0,
                    0, 0, 0, 0.5,
                    0, 0, 0.5, 1,},
                
                { 1, 1, 1, 1,
                    1, 1, 1, 1,
                    0.5, 1, 1, 1,
                    0, 0.5, 1, 1,},
                
                { 0, 0, 0, 0,
                    0, 0, 0, 0,
                    0.5, 0, 0, 0,
                    1, 0.5, 0, 0,},

                { 0, 0, 0.5, 1,
                    0, 0, 0, 0.5,
                    0.5, 0, 0, 0,
                    1, 0.5, 0, 0,},
                
                { 1, 1, 0, 0,
                    1, 1, 0, 0,
                    1, 1, 0, 0,
                    1, 1, 0, 0,},
                
                { 1, 1, 1, 1,
                    1, 1, 1, 1,
                    1, 1, 1, 0.5,
                    1, 1, 0.5, 0,},
                
                { 0, 0, 0, 0,
                    0, 0, 0, 0,
                    1, 1, 1, 1,
                    1, 1, 1, 1,},

                { 0, 0.5, 1, 1,
                    0.5, 1, 1, 1,
                    1, 1, 1, 1,
                    1, 1, 1, 1,},
                
                { 1, 1, 0.5, 0,
                    1, 1, 1, 0.5,
                    1, 1, 1, 1,
                    1, 1, 1, 1,},
                
                { 1, 1, 1, 1,
                    1, 1, 1, 1,
                    1, 1, 1, 1,
                    1, 1, 1, 1,},
            };

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 srcSize = _MainTex_TexelSize.zw;
                float2 px = i.uv * srcSize;
                int2 cell = int2(floor(px.x), floor(px.y));
                float2 f = frac(px);

                int x0 = clamp(cell.x, 0, (int)srcSize.x - 1);
                int y0 = clamp(cell.y, 0, (int)srcSize.y - 1);
                int x1 = clamp(cell.x + 1, 0, (int)srcSize.x - 1);
                int y1 = clamp(cell.y + 1, 0, (int)srcSize.y - 1);

                float2 uvA = (float2(x0, y0) + 0.5) * _MainTex_TexelSize.xy;
                float2 uvB = (float2(x1, y0) + 0.5) * _MainTex_TexelSize.xy;
                float2 uvC = (float2(x0, y1) + 0.5) * _MainTex_TexelSize.xy;
                float2 uvD = (float2(x1, y1) + 0.5) * _MainTex_TexelSize.xy;

                int index = tex2D(_MainTex, uvA).r > 0.5 ? 2 : 0;
                index |= tex2D(_MainTex, uvB).r > 0.5 ? 1 : 0;
                index |= tex2D(_MainTex, uvC).r > 0.5 ? 8 : 0;
                index |= tex2D(_MainTex, uvD).r > 0.5 ? 4 : 0;

                int index2 = 0;
                index2 += px.y < cell.y + 0.25 ? 0 : 1;
                index2 += px.y < cell.y + 0.5 ? 0 : 1;
                index2 += px.y < cell.y + 0.75 ? 0 : 1;
                index2 <<= 2;
                index2 += px.x < cell.x + 0.25 ? 0 : 1;
                index2 += px.x < cell.x + 0.5 ? 0 : 1;
                index2 += px.x < cell.x + 0.75 ? 0 : 1;

                half color = pattern[index][index2];
                return fixed4(color, color, color, 1.0);
            }
            ENDCG
        }
    }
}