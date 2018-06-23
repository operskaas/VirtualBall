// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Motion Vectors" {
	SubShader {
		Tags { "RenderType"="Moving" }
		Pass {
			Fog { Mode Off }
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_fog_exp2
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				struct appdata_simple {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};
				
				struct v2f { 
					float4 pos : POSITION;
					float4 col : COLOR;
				};
				
				uniform float4x4 _mv;
				uniform float4x4 _mvPrev;
				uniform float4x4 _mvInvTrans;
				uniform float4x4 _mvpPrev;
				
				v2f vert (appdata_simple v)
				{
					v2f o;
					float4 P = mul(_mv, v.vertex);
					float4 Pprev = mul(_mvPrev, v.vertex);
					
					// transform normal
					float3 N = (float3)mul(_mvInvTrans, float4(v.normal, 1));
					
					// calculate eye space motion vector
					float3 eyeMotion = P.xyz - Pprev.xyz;
					
					P = UnityObjectToClipPos(v.vertex);
					Pprev = mul(_mvpPrev, v.vertex);
					
					// choose previous or current position based
					// on dot product between motion vector and normal
					float dotMN = dot(eyeMotion, N);
					float4 Pstretch =	dotMN > 0 ?
										P :
										Pprev;
					o.pos = Pstretch;
					
					// do divide by W -> NDC coordinates
					P.xyz = P.xyz / P.w;
					Pprev.xyz = Pprev.xyz / Pprev.w;
					
					// calculate screen space velocity
					float2 screenMotion = P.xy - Pprev.xy;
					
					screenMotion.xy = screenMotion.xy*0.25 + 0.5;
					
					o.col = float4(EncodeFloatRG(screenMotion.x), EncodeFloatRG(screenMotion.y)); //RGBA storage
					
					return o;
				}
				
				float4 frag (v2f i) : COLOR
				{
					return i.col;
				}
			ENDCG
		}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Fog { Mode Off }
			Color (0.4980392, 0.5, 0.4980392, 0.5)
		}
	}
}
