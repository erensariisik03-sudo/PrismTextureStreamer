Shader "AIMMobile/AIM - Light Projector" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _Light ("Light Intensity", Range(0,10)) = 0
 _DayScale ("Day Scale", Range(0,1)) = 0
 _DayNight ("Day / Night", Range(0,1)) = 0
 _ShadowTex ("Cookie", 2D) = "" {}
 _FalloffTex ("FallOff", 2D) = "" { TexGen ObjectLinear }
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