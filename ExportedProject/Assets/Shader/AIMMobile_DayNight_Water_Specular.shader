Shader "AIMMobile/AIM - Day Night Water Specular" {
Properties {
 _MainTex ("Texture 1", 2D) = "white" {}
 _WaveSpeed ("WaveSpeed", Range(0,0.02)) = 0.05
 _Shininess ("Shininess", Float) = 10
 _SpecularIntensity ("Specular Intensity", Float) = 10
 _DayNight ("Day / Night", Range(0,1)) = 0
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