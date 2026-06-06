Shader "Hidden/FogOfWarBlur" {
    Properties {
        _MainTex("Source Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType" = "Opaque"
        }
        Cull Off ZWrite Off ZTest Always

        Pass {
            Name "Horizontal"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_horizontal
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _FogBlurOffset;

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

            static const float weights[5] = { 0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162 };

            fixed4 frag_horizontal(v2f i) : SV_Target {
                float2 offset = float2(_MainTex_TexelSize.x * _FogBlurOffset, 0);
                fixed4 col = tex2D(_MainTex, i.uv) * weights[0];
                for (int j = 1; j < 5; j++) {
                    col += tex2D(_MainTex, i.uv + offset * j) * weights[j];
                    col += tex2D(_MainTex, i.uv - offset * j) * weights[j];
                }
                return col;
            }
            ENDCG
        }

        Pass {
            Name "Vertical"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_vertical
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _FogBlurOffset;

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

            static const float weights[5] = { 0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162 };

            fixed4 frag_vertical(v2f i) : SV_Target {
                float2 offset = float2(0, _MainTex_TexelSize.y * _FogBlurOffset);
                fixed4 col = tex2D(_MainTex, i.uv) * weights[0];
                for (int j = 1; j < 5; j++) {
                    col += tex2D(_MainTex, i.uv + offset * j) * weights[j];
                    col += tex2D(_MainTex, i.uv - offset * j) * weights[j];
                }
                return col;
            }
            ENDCG
        }
    }
}