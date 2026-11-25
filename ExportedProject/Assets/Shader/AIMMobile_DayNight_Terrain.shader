Shader "AIMMobile/AIM - Day Night Terrain" {
Properties {
 _MainTex ("Mask Texture", 2D) = "white" {}
 _Texture1 ("Texture 1", 2D) = "white" {}
 _Texture2 ("Texture 2", 2D) = "white" {}
 _Texture3 ("Texture 3", 2D) = "white" {}
 _Texture4 ("Texture 4", 2D) = "white" {}
 _Lightmap1 ("Day LightMap", 2D) = "unity_Lightmap" {}
 _Lightmap2 ("Night LightMap", 2D) = "unity_Lightmap" {}
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