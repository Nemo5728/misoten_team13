Shader "minionShader"
{
	Properties 
	{
_DiffuseTex("_DiffuseTex", 2D) = "black" {}
_EmiTex("_EmiTex", 2D) = "black" {}
_SpeGloTex("_SpeGloTex", 2D) = "black" {}
_loadingMask("_loadingMask", 2D) = "black" {}
_loadingLight("_loadingLight", 2D) = "black" {}
_noiseMask("_noiseMask", 2D) = "black" {}
_loadUVpan("_loadUVpan", Range(0,1) ) = 1
_alpha("_alpha", Range(0,1) ) = 1
_Lighten("_Lighten", Range(0,1) ) = 0
_glow("_glow", Range(-1,1) ) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}
 Pass {
            ZWrite On
            ColorMask 0
        }
		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
#pragma target 3.0


sampler2D _DiffuseTex;
sampler2D _EmiTex;
sampler2D _SpeGloTex;
sampler2D _loadingMask;
sampler2D _loadingLight;
sampler2D _noiseMask;
float _loadUVpan;
float _alpha;
float _Lighten;
float _glow;

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
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec) * s.Alpha;
c.a = s.Alpha;
return c;

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
				float2 uv_DiffuseTex;
float2 uv_EmiTex;
float2 uv_noiseMask;
float4 meshUV;
float2 uv_SpeGloTex;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.meshUV.xy = v.texcoord.xy;
o.meshUV.zw = v.texcoord1.xy;

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D0=tex2D(_DiffuseTex,IN.uv_DiffuseTex.xy);
float4 Add10=_glow.xxxx + float4( 1.0, 1.0, 1.0, 1.0 );
float4 Multiply8=Add10 * float4( 0.5,0.5,0.5,0.5 );
float4 Multiply7=Sampled2D0 * Multiply8;
float4 Sampled2D2=tex2D(_EmiTex,IN.uv_EmiTex.xy);
float4 Multiply1=Multiply8 * Sampled2D2;
float4 Sampled2D1=tex2D(_noiseMask,IN.uv_noiseMask.xy);
float4 Add6=Sampled2D2 + Sampled2D1;
float4 Multiply0=_loadUVpan.xxxx * float4( -1.75,-1.75,-1.75,-1.75 );
float4 Add0=Multiply0 + float4( 1.1,1.1,1.1,1.1 );
float4 UV_Pan0=float4((IN.meshUV.xyxy).x,(IN.meshUV.xyxy).y + Add0.y,(IN.meshUV.xyxy).z,(IN.meshUV.xyxy).w);
float4 Add5=UV_Pan0 + float4( 0.3,0.3,0.3,0.3 );
float4 Tex2D1=tex2D(_loadingLight,Add5.xy);
float4 Multiply3=Add6 * Tex2D1;
float4 Tex2D2=tex2D(_loadingLight,UV_Pan0.xy);
float4 Add4=Multiply3 + Tex2D2;
float4 Add8=Multiply1 + Add4;
float4 Add1=_Lighten.xxxx + Add8;
float4 Add3=Add1 + Multiply1;
float4 Sampled2D3=tex2D(_SpeGloTex,IN.uv_SpeGloTex.xy);
float4 Split0=Sampled2D3;
float4 Multiply2=float4( 1.0, 1.0, 1.0, 1.0 ) * _alpha.xxxx;
float4 Add7=UV_Pan0 + float4( -0.1,-0.1,-0.1,-0.1 );
float4 Tex2D3=tex2D(_loadingMask,Add7.xy);
float4 Step0=step(Tex2D3,float4( 0.1,0.1,0.1,0.1 ));
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Multiply4=Sampled2D1 * Invert1;
float4 Step1=step(Multiply4,float4( 0.5,0.5,0.5,0.5 ));
float4 Invert2= float4(1.0, 1.0, 1.0, 1.0) - Step1;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Tex2D2;
float4 Add9=Invert2 + Invert0;
float4 Tex2D0=tex2D(_loadingMask,UV_Pan0.xy);
float4 Add2=Add9 + Tex2D0;
float4 Saturate0=saturate(Add2);
float4 Multiply5=Saturate0 * Multiply2;
float4 Subtract0=Multiply5 - float4( 0.1,0.1,0.1,0.1 );
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Subtract0 );
o.Albedo = Multiply7;
o.Emission = Add3;
o.Specular = float4( Split0.y, Split0.y, Split0.y, Split0.y);
o.Gloss = float4( Split0.x, Split0.x, Split0.x, Split0.x);
o.Alpha = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
