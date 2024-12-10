Shader "Custom/2D Dark Blur"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        _Darkness ("Darkness Factor", Float) = 0.5 // 어두운 정도 조절
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Name "DarkBlur"
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _BlurSize;
            float _Darkness; // 어두운 정도 조절

            // Vertex shader
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            // Fragment shader
            float4 frag (v2f i) : SV_Target
            {
                float4 color = float4(0, 0, 0, 0);

                // Gaussian blur kernel
                float2 offset = _BlurSize * _MainTex_TexelSize.xy;
                float weight[5] = { 0.227027, 0.316216, 0.070270, 0.04045, 0.00440 };

                // Center pixel
                color += tex2D(_MainTex, i.uv) * weight[0];

                // Neighbor pixels
                for (int j = 1; j < 5; ++j)
                {
                    color += tex2D(_MainTex, i.uv + float2(offset.x * j, 0)) * weight[j];
                    color += tex2D(_MainTex, i.uv - float2(offset.x * j, 0)) * weight[j];
                    color += tex2D(_MainTex, i.uv + float2(0, offset.y * j)) * weight[j];
                    color += tex2D(_MainTex, i.uv - float2(0, offset.y * j)) * weight[j];
                }

                // 어두운 효과 적용
                color.rgb *= _Darkness;

                return color;
            }
            ENDCG
        }
    }
}
