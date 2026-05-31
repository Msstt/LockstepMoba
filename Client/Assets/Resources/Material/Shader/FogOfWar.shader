Shader "World/FogOfWarSprite" {
    Properties {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _FogTex("Fog Texture", 2D) = "white" {}
        _FogStart("Fog Start (World)", Vector) = (0, 0, 0, 0)
        _FogCellSize("Fog Cell Size", Vector) = (1, 1, 1, 0)
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _FogTex;
            float4 _MainTex_ST;
            float4 _FogTex_TexelSize;
            float4 _FogStart;
            float4 _FogCellSize;
            fixed4 _Color;
            float _FogStrength;

            v2f vert(appdata_t IN) {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 baseCol = tex2D(_MainTex, IN.uv) * IN.color;
                float2 cellCoord = (IN.worldPos.xz - _FogStart.xz) / _FogCellSize.xz;
                float2 fogUV = (cellCoord + 0.5) * _FogTex_TexelSize.xy;
                fixed fogMask = tex2D(_FogTex, fogUV).r;
                fixed fogShadow = saturate(1.0 - fogMask) * _FogStrength;
                baseCol.rgb *= (1.0 - fogShadow);
                return baseCol;
            }
            ENDCG
        }
    }
}
