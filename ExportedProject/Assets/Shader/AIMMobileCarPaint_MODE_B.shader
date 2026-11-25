Shader "AIMMobile/CarPaint_MODE_B" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MetalVal ("MetalVal", Float) = 0.5
 _PlasticVal ("PlasticVal", Float) = 0.5
 _MainTex ("MainTex (RGBA)", 2D) = "white" {}
 _Cubemap ("Cubemap", CUBE) = "_Skybox" {}
 _CubemapIntensity ("Cubemap Intensity", Float) = 1
 _LightIntensity ("Light Intensity", Float) = 1
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}