// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaterShader"
{
	Properties
	{
		_CausticsTex("Caustics texture", 2D) = "white" {}
		_Color("Water color", Color) = (0, 0, 0, 0)
		_ShallowColor("Shallow water colow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DeepColor("Deep water colow", Color) = (0.086, 0.407, 1, 0.749)
		_WaveCutoff("Wave cutoff", Range(0, 1)) = 0.5
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

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

			sampler2D _CausticsTex;
			fixed4 _CausticsTex_ST;
			fixed4 _Color;
			float _WaveCutoff;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _CausticsTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed col = tex2D(_CausticsTex, i.uv).r;
				int noise = col > _WaveCutoff ? 1 : 0;
				return _Color + noise;
				
			}

		ENDCG
		}
	}
}
