Shader "gradTest"
{
	Properties 
	{
_BaseTex("_BaseTex", 2D) = "black" {}
_BaseTexB("_BaseTexB", 2D) = "black" {}
_LightTex("_LightTex", 2D) = "black" {}
_LightTexB("_LightTexB", 2D) = "black" {}
_loadingMask("_loadingMask", 2D) = "black" {}
_loadingLight("_loadingLight", 2D) = "black" {}
_noiseMask("_noiseMask", 2D) = "black" {}
_loadUVpan("_loadUVpan", Range(0,1) ) = 1
_RingRound("_RingRound", Range(0,1) ) = 0
_RingLight("_RingLight", Range(0,1) ) = 0
_ChangeColor("_ChangeColor", Range(0,1) ) = 0
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

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:add vertex:vert
#pragma target 3.0


sampler2D _BaseTex;
sampler2D _BaseTexB;
sampler2D _LightTex;
sampler2D _LightTexB;
sampler2D _loadingMask;
sampler2D _loadingLight;
sampler2D _noiseMask;
float _loadUVpan;
float _RingRound;
float _RingLight;
float _ChangeColor;
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
				float2 uv_BaseTex;
float2 uv_LightTex;
float2 uv_LightTexB;
float2 uv_noiseMask;
float4 meshUV;

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
				
float4 UV_Pan1=float4((IN.uv_BaseTex.xyxy).x,(IN.uv_BaseTex.xyxy).y + _RingRound.xxxx.y,(IN.uv_BaseTex.xyxy).z,(IN.uv_BaseTex.xyxy).w);
float4 Tex2D4=tex2D(_BaseTex,UV_Pan1.xy);
float4 Invert3= float4(1.0, 1.0, 1.0, 1.0) - _ChangeColor.xxxx;
float4 Multiply8=Tex2D4 * Invert3;
float4 Tex2D5=tex2D(_BaseTexB,UV_Pan1.xy);
float4 Multiply7=Tex2D5 * _ChangeColor.xxxx;
float4 Add12=Multiply8 + Multiply7;
float4 Sampled2D2=tex2D(_LightTex,IN.uv_LightTex.xy);
float4 Invert4= float4(1.0, 1.0, 1.0, 1.0) - _ChangeColor.xxxx;
float4 Multiply10=Sampled2D2 * Invert4;
float4 Sampled2D0=tex2D(_LightTexB,IN.uv_LightTexB.xy);
float4 Multiply9=Sampled2D0 * _ChangeColor.xxxx;
float4 Add13=Multiply10 + Multiply9;
float4 Multiply6=Add13 * _RingLight.xxxx;
float4 Add11=Add12 + Multiply6;
float4 Sampled2D1=tex2D(_noiseMask,IN.uv_noiseMask.xy);
float4 Add6=Add11 + Sampled2D1;
float4 Multiply0=_loadUVpan.xxxx * float4( -2,-2,-2,-2 );
float4 Add0=Multiply0 + float4( 1.1,1.1,1.1,1.1 );
float4 UV_Pan0=float4((IN.meshUV.xyxy).x,(IN.meshUV.xyxy).y + Add0.y,(IN.meshUV.xyxy).z,(IN.meshUV.xyxy).w);
float4 Add5=UV_Pan0 + float4( 0.3,0.3,0.3,0.3 );
float4 Tex2D1=tex2D(_loadingLight,Add5.xy);
float4 Multiply3=Add6 * Tex2D1;
float4 Tex2D2=tex2D(_loadingLight,UV_Pan0.xy);
float4 Add4=Multiply3 + Tex2D2;
float4 Add8=Add4 + Add11;
float4 Add1=_Lighten.xxxx + Add8;
float4 Multiply1=Add11 * _glow.xxxx;
float4 Add3=Add1 + Multiply1;
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
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Subtract0 );
o.Emission = Add3;
o.Alpha = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
