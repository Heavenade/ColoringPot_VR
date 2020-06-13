Shader "Custom/ceramics"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("NormalMap",2D) = "bump"{}
		_GlossTex("Gloss Tex", 2D) = "white"{}
		_SpecCol("Specular Color", Color) = (1,1,1,1)
		_SpecPow("Specular Power", Range(10,100)) = 100
		_SpecCol2("Specular Color2", Color) = (1,1,1,1)
		_SpecPow2("Specular Power2", Range(10,100)) = 100
		_RimCol("Rim Color", Color) = (1,1,1,1)
		_RimcPow("Rim Power",Range(1,10)) = 3
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Test

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _GlossTex;
			float4 _SpecCol;
			float _SpecPow;
			float4 _SpecCol2;
			float _SpecPow2;
			float4 _RimCol;
			float _RimcPow;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_GlossTex;
				//절대로 절대로! SpecColor을 Input에서 받지 않는다! Properties에서만!
				//공식 상 이미 받았기 때문이다(예약어 기능)
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				float4 m = tex2D(_GlossTex, IN.uv_GlossTex);
				o.Albedo = c.rgb;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Gloss = m.a;
				o.Alpha = c.a;
			}

			float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {
				float4 final;

				//Lambert term
				float3 DiffColor;
				float ndot1 = saturate(dot(s.Normal, lightDir));
				DiffColor = ndot1 * s.Albedo*_LightColor0.rgb*atten;

				//Spec term
				float3 SpecColor;
				float3 H = normalize(lightDir + viewDir);
				float spec = saturate(dot(H, s.Normal));
				spec = pow(spec, _SpecPow);
				SpecColor = spec * _SpecCol.rgb * s.Gloss;

				//Rim term
				float3 rimColor;
				float rim = abs(dot(viewDir, s.Normal));
				float invrim = 1 - rim;
				rimColor = pow(invrim, _RimcPow)*_RimCol.rgb;

				//Fake Spec term
				float3 SpecColor2;
				SpecColor2 = pow(rim, _SpecPow2)*_SpecCol2.rgb*s.Gloss;

				//final term
				final.rgb = DiffColor.rgb + SpecColor.rgb + rimColor.rgb + SpecColor2.rgb;
				final.a = s.Alpha;
				return final;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
