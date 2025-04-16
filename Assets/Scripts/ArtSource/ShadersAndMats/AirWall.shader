Shader "My Shaders/AirWall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _PlayerPos("Player Position", Vector) = (1, 0, 1, 0)
        _DetectRange("Detect Range", Vector) = (2, 6, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
			#pragma fragment frag
            
            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 posWorld : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _PlayerPos;
            float4 _DetectRange;

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.posWorld = TransformObjectToWorld(v.vertex.xyz);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // 获取距离并且采样主纹理
                float dist = distance(i.posWorld, _PlayerPos);
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv); 
                
                texColor.a *= 1- smoothstep(_DetectRange.x, _DetectRange.y, dist);


                return texColor;
            }

            ENDHLSL
        }
    }
}
