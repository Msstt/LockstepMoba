Shader "Hidden/FogOfWarUpscale" {
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
                float2 px = i.uv * srcSize - 0.5;
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

                bool a = tex2D(_MainTex, uvA).r > 0.5;
                bool b = tex2D(_MainTex, uvB).r > 0.5;
                bool c = tex2D(_MainTex, uvC).r > 0.5;
                bool d = tex2D(_MainTex, uvD).r > 0.5;

                bool allFull = a && b && c && d;
                bool allEmpty = !a && !b && !c && !d;

                if (allFull) return fixed4(1, 1, 1, 1);
                if (allEmpty) return fixed4(0, 0, 0, 1);

                float result = 0;

                if (a) result += (1.0 - f.x) * (1.0 - f.y);
                if (b) result += f.x * (1.0 - f.y);
                if (c) result += (1.0 - f.x) * f.y;
                if (d) result += f.x * f.y;

                return fixed4(result, result, result, 1.0);
            }
            ENDCG
        }
    }
}