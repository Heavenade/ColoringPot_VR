Shader "Custom/armchair"
{
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Metallic("Metallic", Range(0,1)) = 0
		_Smoothness("Smoothness", Range(0,1)) = 0.5
		_BumpMap("Normalmap",2D) = "bump"{}                         //NormalMap 변수 사용시 에러 표시가 난다.
		_Occlusion("Occlusion", 2D) = "white"{}						//Ambident Occlusion: 환경 차폐. 환경광이 닿지 못하는 곳 고려
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Occlusion;
		float _Metallic;
		float _Smoothness;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Occlusion = tex2D(_Occlusion, IN.uv_MainTex);			//물론 동일한 텍스쳐를 Alpha 채널로 처리하면 된다(포토샵에서)
			fixed4 n = tex2D(_BumpMap, IN.uv_BumpMap);
			o.Normal = UnpackNormal(n);								//Normal map
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;								//smoothness increase->정반사(Specular) increase, 난반사(diffuse) decrease
																	//Unreal: Roughness
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
