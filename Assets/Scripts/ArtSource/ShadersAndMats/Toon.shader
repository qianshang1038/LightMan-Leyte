Shader "MyShaders/URPCartoonShader" {
    Properties {
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
        _AddLightColor ("Add Light Color Tint", Color) = (1, 1, 1, 1)
        _AddLightPower ("Add Light Power", Float) = 1.0
        _MainTex ("Main Tex", 2D) = "white" {}
        _Ramp ("Ramp Texture", 2D) = "white" {}

        _Specular ("Specular", Color) = (1, 1, 1, 1)
        _SpecularScale ("Specular Scale", Range(0, 0.1)) = 0.01
        _SpecularRotate("Specular Rotate", Float) = 1
        [Toggle(_AdditionalLights)] _AddLights ("AddLights", Float) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}

        Pass {
            Tags { "LightMode"="UniversalRenderPipeline" }

            Cull Off

            HLSLINCLUDE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
            }; 
        
            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial);
            float4 _Color;
            float4 _AddLightColor;
            float4 _MainTex_ST;
            float4 _Ramp_ST;
            float4 _Specular;
            float _SpecularScale;
            float _SpecularRotate;
            float _AddLightPower;
            CBUFFER_END;

            TEXTURE2D(_MainTex);
            TEXTURE2D (_Ramp);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_Ramp);

            ENDHLSL
        }

        Pass {
            Tags { "LightMode"="UniversalForward" }

            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _AdditionalLights

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS 
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE  
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS 
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS 
            #pragma multi_compile _ _SHADOWS_SOFT


            v2f vert(a2v v)
            {
                v2f o;

                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half3 worldNormal = normalize(i.worldNormal);

                //half3 worldLightDir = normalize(GetMainLightDirection(i.worldPos));
                half3 worldLightDir = normalize((_MainLightPosition.xyz));
                half3 worldViewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                half3 worldHalfDir = normalize(worldLightDir + worldViewDir);
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                half3 albedo = texColor.rgb * _Color.rgb;
                // Ambient lighting
                half3 ambient = _GlossyEnvironmentColor.rgb * albedo;
                // Diffuse lighting
                half diff = dot(worldNormal, worldLightDir);
                diff = (diff * 0.5 + 0.5); // Normalize to [0, 1]
                half3 diffuse = _MainLightColor.rgb * albedo * SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(diff, diff)).rgb;
                // Specular lighting
                half spec = dot(worldNormal, worldHalfDir);
                half w = fwidth(spec) * 2.0;
                
                // Rotation logic for specular direction
                const float DEG2RAD = 0.0174532925;
                float cosRA = cos(_SpecularRotate * DEG2RAD);
                float sinRA = sin(_SpecularRotate * DEG2RAD);

                // Rotate the half vector around the Y-axis (for example)
                half3 rotatedHalfDir = float3(
                    cosRA * worldHalfDir.x - sinRA * worldHalfDir.z,
                    worldHalfDir.y,
                    sinRA * worldHalfDir.x + cosRA * worldHalfDir.z
                );

                half specRotated = dot(worldNormal, rotatedHalfDir);
                
                half3 specular = _Specular.rgb * lerp(0, 1, smoothstep(-w, w, specRotated + _SpecularScale - 1)) * step(0.0001, _SpecularScale);
                //half3 specular = _Specular.rgb * step(1.0 - _SpecularScale, spec);

                // Calculate Shadows:
                Light light = GetMainLight(TransformWorldToShadowCoord(i.worldPos));
                float3 finalColor = (ambient + diffuse + specular)*light.shadowAttenuation ;


                // ������Դ��Ӱ
                #if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
				    half4 shadowMask = inputData.shadowMask;
				#elif !defined (LIGHTMAP_ON)
				    half4 shadowMask = unity_ProbesOcclusion;
				#else
				    half4 shadowMask = half4(1, 1, 1, 1);
				#endif

                // ������Դ
                #ifdef _AdditionalLights
                    half3 additionalLights = half3(0, 0, 0); // ��ʼ��Ϊ��
                    int addLightsCount = GetAdditionalLightsCount(); // ��ȡ�����Դ������

                    for (int j = 0; j < addLightsCount; j++)
                    {
                        // ��ȡ�� j �����ӹ�Դ
                        Light addLight = GetAdditionalLight(j,i.worldPos, shadowMask);
                        float3 addLightDirWorld = normalize(addLight.direction); // ���ӹ�Դ�ķ���
                        float3 addLightHalfDir = normalize(addLightDirWorld + worldViewDir); // ���ӹ�Դ�İ�����

                        // ��������㣬�Ͳ�����Ramp��
                        half addDiff = dot(worldNormal, addLightDirWorld);
                        addDiff = saturate(addDiff)* SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(addDiff, addDiff)).rgb * _AddLightPower; // ��һ���� [0, 1]

                        // �߹����
                        half addSpec = dot(worldNormal, addLightHalfDir);
                        half addWidth = fwidth(addSpec) * 2.0;
                        half3 addSpecular = _Specular.rgb * lerp(0, 1, smoothstep(-addWidth, addWidth, addSpec + _SpecularScale - 1)) * step(0.05, _SpecularScale);

                        // ˥������ (���ݾ������˥��)
                        float attenuation = addLight.distanceAttenuation;
                        float addLightShadowAttenuation = addLight.shadowAttenuation;
                        // ���㸽�ӹ�Դ�Ĺ���
                        additionalLights += (addLight.color.rgb * albedo * addDiff + addSpecular) * attenuation * addLightShadowAttenuation;
                    }
                    finalColor += additionalLights;
                #endif



                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }

    FallBack "Universal Render Pipeline/Lit"
}
