Shader "Hidden/FogOfWarFade" {
    Properties {
        _MainTex("Current Frame", 2D) = "white" {}
        _PrevTex("Previous Frame", 2D) = "white" {}
        _FadeStep("Fade Step", Float) = 0.05
    }
    SubShader {
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _PrevTex;
            float _FadeStep;

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
                fixed curr = tex2D(_MainTex, i.uv).r;
                fixed prev = tex2D(_PrevTex, i.uv).r;
                fixed result = lerp(prev, curr, _FadeStep);
                return fixed4(result, result, result, 1);
            }
            ENDCG
        }
    }
}