Shader "Universal Render Pipeline/Custom/MiniPBR"
{
    Properties
    {
		[Toggle(_SPECULAR_SETUP)]ifSpecularSetup("ifSpecularSetup",float)=0
		[Toggle(_METALLICSPECGLOSSMAP)]ifUseSpecOrGlossMap("ifUseSpecOrGlossMap",float) = 0
		[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
		[MainTexture] _BaseMap("Albedo", 2D) = "white" {}

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		_SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		_SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}

		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		_ReceiveShadows("Receive Shadows", Float) = 1.0
		[HideInInspector] _QueueOffset("Queue offset", Float) = 0.0

		[HideInInspector] _Surface("__surface", Float) = 0.0
		[HideInInspector] _Blend("__blend", Float) = 0.0
		[HideInInspector] _AlphaClip("__clip", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0
		[HideInInspector] _Cull("__cull", Float) = 2.0

    }
    SubShader
    {
		Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
		LOD 300
        Pass
        {
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]

            HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"  
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			// -------------------------------------
			// Material Keywords
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICSPECGLOSSMAP
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _OCCLUSIONMAP

			#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
			#pragma shader_feature _SPECULAR_SETUP
			#pragma shader_feature _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			// -------------------------------------
			// Unity defined keywords
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

			#ifdef _SPECULAR_SETUP
			#define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
			#else
			#define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
			#endif

			struct Attributes
			{
				float4 positionOS   : POSITION;
				float3 normalOS     : NORMAL;
				float4 tangentOS    : TANGENT;
				float2 texcoord     : TEXCOORD0;
				float2 lightmapUV   : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct Varyings
			{
				float2 uv                       : TEXCOORD0;
				DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
				float3 positionWS               : TEXCOORD2;
				float3 normalWS                 : TEXCOORD3;
				#ifdef _NORMALMAP
				float4 tangentWS                : TEXCOORD4;    // xyz: tangent, w: sign
				#endif
				float3 viewDirWS                : TEXCOORD5;
				half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light
				float4 positionCS               : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			///////////////////////////////////////////////////////////////////////////////
			//                  Vertex and Fragment functions                            //
			///////////////////////////////////////////////////////////////////////////////
			Varyings vert (Attributes input)
            {
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);

				float3 positionWS = TransformObjectToWorld(input.positionOS);
				float3 positionVS = TransformWorldToView(positionWS);
				float4 positionCS = TransformWorldToHClip(positionWS);

				float4 ndc = positionCS * 0.5f;
				float4 positionNDC;
				positionNDC.xy = float2(ndc.x, ndc.y * _ProjectionParams.x) + ndc.w;
				positionNDC.zw = positionCS.zw;

				real sign = input.tangentOS.w * GetOddNegativeScale();
				float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
				float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
				float3 bitangentWS = cross(normalWS, tangentWS) * sign;

				float3 viewDirWS = GetCameraPositionWS() - positionWS;
				half3 vertexLight = VertexLighting(positionWS, normalWS);
				half fogFactor = ComputeFogFactor(positionCS.z);
				output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
				output.normalWS = normalWS;
				output.viewDirWS = viewDirWS;
				#ifdef _NORMALMAP
				//real sign = input.tangentOS.w * GetOddNegativeScale();
				output.tangentWS = half4(tangentWS.xyz, sign);
				#endif

				OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
				OUTPUT_SH(output.normalWS.xyz, output.vertexSH);
				output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				output.positionWS = positionWS;
				output.positionCS = positionCS;

				return output;
            }


			half3 CDirectBDRF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
			{
				float3 halfDir = SafeNormalize(float3(lightDirectionWS)+float3(viewDirectionWS));

				float NoH = saturate(dot(normalWS, halfDir));
				half LoH = saturate(dot(lightDirectionWS, halfDir));
				float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

				half LoH2 = LoH * LoH;
				half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

				#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
				specularTerm = specularTerm - HALF_MIN;
				specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
				#endif

				half3 color = specularTerm * brdfData.specular + brdfData.diffuse;
				return color;
			}

			half3 CLightingPhysicallyBased(BRDFData brdfData, half3 lightColor, half3 lightDirectionWS, half lightAttenuation, half3 normalWS, half3 viewDirectionWS)
			{
				half NdotL = saturate(dot(normalWS, lightDirectionWS));
				half3 radiance = lightColor * (lightAttenuation * NdotL);
				return CDirectBDRF(brdfData, normalWS, lightDirectionWS, viewDirectionWS) * radiance;
			}

			float4 frag (Varyings input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				float2 uv = input.uv;
				half4 albedoAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
				half alpha = _BaseColor.a*albedoAlpha.a;
				#if defined(_ALPHATEST_ON)
				clip(alpha - _Cutoff);
				#endif

				half4 specGloss;
				#ifdef _METALLICSPECGLOSSMAP
					specGloss = SAMPLE_METALLICSPECULAR(uv);
					specGloss.a *= _Smoothness;
				#else
					#if _SPECULAR_SETUP
						specGloss.rgb = _SpecColor.rgb;
					#else
						specGloss.rgb = _Metallic.rrr;
					#endif

					#ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
						specGloss.a = albedoAlpha * _Smoothness;
					#else
						specGloss.a = _Smoothness;
					#endif
				#endif


				float3 albedo = albedoAlpha.rgb * _BaseColor.rgb;
				//return float4(albedo, alpha);
				float metallic;
				float specular;
				#if _SPECULAR_SETUP
				metallic = 1.0h;
				specular = specGloss.rgb;
				#else
				metallic = specGloss.r;
				specular = half3(0.0h, 0.0h, 0.0h);
				#endif

				float smoothness = specGloss.a;
				half4 normalTS = half4( SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale),0);
				float occlusion = SampleOcclusion(uv);
				half3 emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));


				half4 positionWS = half4( input.positionWS,1);
				half3 viewDirWS = SafeNormalize(input.viewDirWS);
				#ifdef _NORMALMAP
				float sgn = input.tangentWS.w;      // should be either +1 or -1
				float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
				float4 normalWS = float4(TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz)),0);
				#else
				float4 normalWS = float4( input.normalWS,0);
				#endif

				normalWS = float4(NormalizeNormalPerPixel(normalWS),0);
				float3 viewDirectionWS = viewDirWS;

				#if defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
				#else
				float4 shadowCoord = float4(0, 0, 0, 0);
				#endif

				half fogCoord = input.fogFactorAndVertexLight.x;
				half3 vertexLighting = input.fogFactorAndVertexLight.yzw;
				half3 bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, normalWS);

				BRDFData brdfData;
				InitializeBRDFData(albedo, metallic, specular, smoothness, alpha, brdfData);

				Light mainLight = GetMainLight(shadowCoord);
				MixRealtimeAndBakedGI(mainLight,normalWS, bakedGI, half4(0, 0, 0, 0));

				float3 color = GlobalIllumination(brdfData, bakedGI, occlusion, normalWS, viewDirectionWS);
				color += CLightingPhysicallyBased(brdfData, mainLight.color, mainLight.direction, mainLight.distanceAttenuation * mainLight.shadowAttenuation, normalWS, viewDirectionWS);

				#ifdef _ADDITIONAL_LIGHTS
				uint pixelLightCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
				{
					Light light = GetAdditionalLight(lightIndex, positionWS);
					color += LightingPhysicallyBased(brdfData, light,normalWS, viewDirectionWS);
				}
				#endif

				color += emission;

				color.rgb = MixFog(color.rgb, fogCoord);
				return float4(color, alpha);
			}

            ENDHLSL
        }
    }
FallBack "Universal Render Pipeline/Lit"
}