Shader "Custom/RPG_UIBackground_Procedural"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.8, 0.7, 0.6, 1)
        _BottomColor ("Bottom Color", Color) = (0.4, 0.3, 0.2, 1)
        _VignetteStrength ("Vignette Strength", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Background" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            fixed4 _TopColor;
            fixed4 _BottomColor;
            float _VignetteStrength;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Vertical gradient
                fixed4 col = lerp(_BottomColor, _TopColor, i.uv.y);

                // Vignette
                float2 centeredUV = i.uv - 0.5;
                float dist = length(centeredUV) * 2.0;
                float vignette = 1.0 - _VignetteStrength * dist;
                vignette = saturate(vignette);

                col.rgb *= vignette;

                return col;
            }
            ENDCG
        }
    }
}
