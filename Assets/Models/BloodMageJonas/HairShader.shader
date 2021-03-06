Shader "Bumped Specular Hair" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_OpacMap ("Ppacitymap", 2D) = "bump" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5


}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong alphatest:_Cutoff
#pragma exclude_renderers flash
#pragma target 3.0

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _OpacMap;
fixed4 _Color;
half _Shininess;


struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float2 uv_OpacMap;
	float3 worldPos;

};

void surf (Input IN, inout SurfaceOutput o) {


	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 tex3 = tex2D(_OpacMap, IN.uv_OpacMap);
	o.Albedo = tex.rgb * _Color.rgb;
	o.Gloss = tex.a;
	o.Alpha = tex3.r * _Color.a;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));



}
ENDCG
}

FallBack "Transparent/Cutout/VertexLit"
}
