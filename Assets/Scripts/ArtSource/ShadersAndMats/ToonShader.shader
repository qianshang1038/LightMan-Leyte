Shader "MyShaders/URPCartoonShader" {
    Properties {
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
        _AddLightColor ("Add Light Color Tint", Color) = (1, 1, 1, 1)
        _AddLightPower ("Add Light Power", Float) = 1.0
        _MainTex ("Main Tex", 2D) = "white" {}
        _Ramp ("Ramp Texture", 2D) = "white" {}
        _BumpMap("Bump Map", 2D) = "white" {}

        _Specular ("Specular", Color) = (1, 1, 1, 1)
        _SpecularScale ("Specular Scale", Range(0, 0.1)) = 0.01
        _SpecularRotate("Specular Rotate", Float) = 1
        _BumpScale ("Bump Scale", Float) = 1
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
        
            // float4 uv 充分利用空间计算凹凸纹理
            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 worldTangent  : TEXCOORD3;
                float4 TtoW0 : TEXCOORD4;
                float4 TtoW1 : TEXCOORD5;
                float4 TtoW2 : TEXCOORD6;
            };

            CBUFFER_START(UnityPerMaterial);
            float4 _Color;
            float4 _AddLightColor;
            float4 _MainTex_ST;
            float4 _Ramp_ST;
            float4 _BumpMap_ST;
            float4 _Specular;
            float _SpecularScale;
            float _SpecularRotate;
            float _AddLightPower;
            float _BumpScale;
            CBUFFER_END;

            TEXTURE2D(_MainTex);
            TEXTURE2D(_Ramp);
            TEXTURE2D(_BumpMap);
            
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_Ramp);
            SAMPLER(sampler_BumpMap);

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
                //o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

                 float3 worldNormal = TransformObjectToWorldNormal(v.normal); // 定义并计算世界法线
                o.worldNormal = worldNormal;
                float3 worldPos = TransformObjectToWorldDir(v.vertex.xyz);
                o.worldPos = worldPos;
    
                float3 worldTangent = TransformObjectToWorldNormal(v.tangent.xyz); // 计算世界切线
                o.worldTangent = float4(worldTangent, 1.0); // 将其转换为 float4
                
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);


                
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
                
                // 切线空间下计算法线偏移
                float3 bump = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uv.zw));
                // 为法线设置Bump强度只需要影响xy
                bump.xy *= _BumpScale;
                // 勾股定理算出z值
                bump.z = sqrt(1.0 - saturate(dot(bump.xy, bump.xy)));

                bump = normalize(half3(dot(i.TtoW0.xyz, bump), dot(i.TtoW1.xyz, bump), dot(i.TtoW2.xyz, bump)));

                half3 albedo = texColor.rgb * _Color.rgb;
                // Ambient lighting
                half3 ambient = _GlossyEnvironmentColor.rgb * albedo;

                half bumpLight =max(0, dot(bump, worldLightDir));

                bumpLight = (bumpLight*0.5) +0.5;
                
                half3 diffuse = _MainLightColor.rgb * albedo * bumpLight*
                SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(bumpLight, bumpLight)).rgb;

                // Specular lighting
                half spec = dot(worldNormal, worldHalfDir);
                half w = fwidth(spec) * 2.0;
                
                // Diffuse lighting
               // half diff = dot(bump, worldLightDir);
                //diff = (diff * 0.5 + 0.5); // Normalize to [0, 1]
                //half3 diffuse = _MainLightColor.rgb * albedo * SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(diff, diff)).rgb ;

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


                // 计算多光源阴影: 阴影Map且打开LightMap
                #if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
				    half4 shadowMask = inputData.shadowMask;
				#elif !defined (LIGHTMAP_ON)
				    half4 shadowMask = unity_ProbesOcclusion;
				#else
				    half4 shadowMask = half4(1, 1, 1, 1);
				#endif

                // 处理多光源
                #ifdef _AdditionalLights
                    half3 additionalLights = half3(0, 0, 0); // 初始化为零
                    int addLightsCount = GetAdditionalLightsCount(); // 获取额外光源的数量

                    for (int j = 0; j < addLightsCount; j++)
                    {
                        // 获取第 j 个附加光源
                        Light addLight = GetAdditionalLight(j,i.worldPos, shadowMask);
                        float3 addLightDirWorld = normalize(addLight.direction); // 附加光源的方向
                        float3 addLightHalfDir = normalize(addLightDirWorld + worldViewDir); // 附加光源的半向量

                        // 漫反射计算，就不采样Ramp了
                        half addDiff = dot(worldNormal, addLightDirWorld);
                        addDiff = saturate(addDiff)* SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(addDiff, addDiff)).rgb * _AddLightPower; // 归一化到 [0, 1]

                        // 高光计算
                        half addSpec = dot(worldNormal, addLightHalfDir);
                        half addWidth = fwidth(addSpec) * 2.0;
                        half3 addSpecular = _Specular.rgb * lerp(0, 1, smoothstep(-addWidth, addWidth, addSpec + _SpecularScale - 1)) * step(0.05, _SpecularScale);

                        // 衰减处理 (根据距离进行衰减)
                        float attenuation = addLight.distanceAttenuation;
                        float addLightShadowAttenuation = addLight.shadowAttenuation;
                        // 计算附加光源的贡献
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
}