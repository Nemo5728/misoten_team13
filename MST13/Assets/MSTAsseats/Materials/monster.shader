Shader "DefaultLighting"
{
	Properties 
	{
_Gloss("_Gloss", Range(0,1) ) = 0.567
_SpecularColor("_SpecularColor", Color) = (0.5441177,0.5441177,0.5441177,1)
_RimFalloff("_RimFalloff", Range(0,10) ) = 3.14
_RimColor("_RimColor", Color) = (1,0.3308824,0.6769778,1)
_Diffuse("_Diffuse", 2D) = "black" {}
_GradMask("_GradMask", 2D) = "black" {}
_effect1("_effect1", 2D) = "black" {}
_GradOffset("_GradOffset", Range(0,1) ) = 0.627
_GlowRange("_GlowRange", Range(0,1) ) = 0
_GlowColor("_GlowColor", Color) = (1,0.8676471,0.9470589,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float _Gloss;
float4 _SpecularColor;
float _RimFalloff;
float4 _RimColor;
sampler2D _Diffuse;
sampler2D _GradMask;
sampler2D _effect1;
float _GradOffset;
float _GlowRange;
float4 _GlowColor;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
float4 Multiply1=float4( s.Albedo.x, s.Albedo.y, s.Albedo.z, 1.0 ) * light;
float4 Splat0=light.w;
float4 Multiply0=float4( s.Gloss.x, s.Gloss.y, s.Gloss.z, 1.0 ) * Splat0;
float4 Multiply2=light * Multiply0;
float4 Add2=Multiply1 + Multiply2;
float4 Mask1=float4(Add2.x,Add2.y,Add2.z,0.0);
float4 Mask0=float4(0.0,0.0,0.0,s.Alpha.xxxx.w);
float4 Add1=Mask1 + Mask0;
return Add1;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float4 fullMeshUV1;
float4 fullMeshUV2;
float3 viewDir;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.fullMeshUV1 = v.texcoord;
o.fullMeshUV2 = v.texcoord1;

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Multiply2=_GlowColor * _GlowRange.xxxx;
float4 Tex2D0=tex2D(_Diffuse,(IN.fullMeshUV1).xy);
float4 Negative0= -_GradOffset.xxxx; 
 float4 UV_Pan0=float4((IN.fullMeshUV2).x,(IN.fullMeshUV2).y + Negative0.y,(IN.fullMeshUV2).z,(IN.fullMeshUV2).w);
float4 Tex2D1=tex2D(_GradMask,UV_Pan0.xy);
float4 Tex2D2=tex2D(_effect1,(IN.fullMeshUV2).xy);
float4 Multiply1=Tex2D1 * Tex2D2;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,_RimFalloff.xxxx);
float4 Multiply0=_RimColor * Pow0;
float4 Add0=Multiply1 + Multiply0;
float4 Add1=Tex2D0 + Add0;
float4 Add2=Multiply2 + Add1;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Add2;
o.Specular = _Gloss.xxxx;
o.Gloss = _SpecularColor;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
