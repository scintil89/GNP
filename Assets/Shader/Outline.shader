Shader "Custom/Outline" 
{
	Properties
	{
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata 
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct vOut 
	{
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float4 _OutlineColor;
	uniform float _Outline;


	vOut vert(appdata v)
	{
		vOut vout;
		vout.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);

		float2 offset = TransformViewToProjection(norm.xy);

		vout.pos.xy += offset * vout.pos.z * _Outline;

		vout.color = _OutlineColor;

		return vout;
	}
	ENDCG

	SubShader
	{
		Tags{ "Queue" = "Overlay" }

		Pass
		{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ZTest Less
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Offset 15,15

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(vOut i) : COLOR
			{ 
				return i.color; 
			}

			ENDCG
		}
	}

		Fallback "Diffuse"
}