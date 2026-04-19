Shader "World/FogOfWarSprite" {
    Properties {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _FogTex("Fog Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        _FogStrength("Fog Strength", Range(0, 1)) = 0.75
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _FogTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _FogStrength;

            v2f vert(appdata_t IN) {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 baseCol = tex2D(_MainTex, IN.uv) * IN.color;
                fixed fogMask = tex2D(_FogTex, IN.uv).r;
                baseCol.a *= fogMask * _FogStrength;
                return baseCol;
            }
            ENDCG
        }
    }
}
