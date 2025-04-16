Shader "UI_Shader/Effect/distort_add_URP" {
	Properties {
		_Brightness("Brightness",float) = 1
  		_Contrast ("Contrast", float) = 1
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Main Tex (A)", 2D) = "white" {}
		_MainPannerX  ("Main Panner X", float) = 0
		_MainPannerY  ("Main Panner X", float) = 0
		_TurbulenceTex ("Turbulence Tex", 2D) = "white" {}
		_MaskTex ("Mask Tex", 2D) = "white" {}
		_DistortPower  ("Distort Power", float) = 0
		_PowerX  ("Power X", range (0,1)) = 0
		_PowerY  ("Power Y", range (0,1)) = 0
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off
		ZWrite Off

		Pass {
			// Use Universal Render Pipeline's Core Library
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct Attributes {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct Varyings {
				float4 vertex : SV_POSITION;
				half2 uvmain : TEXCOORD1;
				half2 uvnoise : TEXCOORD2;
				half2 uvMask : TEXCOORD3;
				float4 vertexColor : COLOR;
			};

			float _Brightness;
			float _Contrast;
			float4 _MainColor;
			float _PowerX;
			float _PowerY;
			float _DistortPower;
			float4 _MainTex_ST;
			float4 _TurbulenceTex_ST;
			float4 _MaskTex_ST;
			float _MainPannerX;
      		float _MainPannerY;

			TEXTURE2D(_MainTex);
			TEXTURE2D(_TurbulenceTex);
			TEXTURE2D(_MaskTex);
			SAMPLER(sampler_MainTex);
			SAMPLER(sampler_TurbulenceTex);
			SAMPLER(sampler_MaskTex);

			Varyings vert(Attributes IN) {
				Varyings OUT;
				OUT.vertex = TransformObjectToHClip(IN.vertex.xyz);
				OUT.uvmain = TRANSFORM_TEX(IN.texcoord, _MainTex);
				OUT.uvnoise = TRANSFORM_TEX(IN.texcoord, _TurbulenceTex);
				OUT.uvMask = TRANSFORM_TEX(IN.texcoord, _MaskTex);
				OUT.vertexColor = IN.vertexColor;
				return OUT;
			}

			float4 frag(Varyings IN) : SV_Target {
				float4 offsetColor1 = SAMPLE_TEXTURE2D(_TurbulenceTex, sampler_TurbulenceTex, IN.uvnoise + fmod(_Time.xz*_DistortPower, 1));
				float4 offsetColor2 = SAMPLE_TEXTURE2D(_TurbulenceTex, sampler_TurbulenceTex, IN.uvnoise + fmod(_Time.yx*_DistortPower, 1));

				float4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, IN.uvMask);

				half2 oldUV = IN.uvmain;

				IN.uvmain.x += ((offsetColor1.r + offsetColor2.r) - 1) * _PowerX;
				IN.uvmain.y += ((offsetColor1.r + offsetColor2.r) - 1) * _PowerY;

				half2 resUV = lerp(oldUV, IN.uvmain, mask.xy);
				resUV.x += fmod(_MainPannerX * _Time.y, 1);
			    resUV.y += fmod(_MainPannerY * _Time.y, 1);

				float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, resUV);
				float4 _MaskTex_var = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, IN.uvMask);

				float3 emissive = (_MainColor.rgb * _Brightness * pow(abs(_MainTex_var.rgb), _Contrast) * IN.vertexColor.rgb) 
				* (_MainColor.a * _MainTex_var.a * IN.vertexColor.a * _MaskTex_var.r);
				float3 finalColor = emissive;

				return float4(finalColor, 1);
			}

			ENDHLSL
		}
	}
}
