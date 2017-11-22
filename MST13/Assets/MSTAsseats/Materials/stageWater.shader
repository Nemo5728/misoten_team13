Shader "stageWater"
{
	Properties 
	{
_BaseTex("_BaseTex", 2D) = "black" {}
_CircuitTex("_CircuitTex", 2D) = "black" {}
_WaveMask("_WaveMask", 2D) = "black" {}
_FlowMask("_FlowMask", 2D) = "black" {}
_loadingMask("_loadingMask", 2D) = "black" {}
_loadingLight("_loadingLight", 2D) = "black" {}
_noiseMask("_noiseMask", 2D) = "black" {}
_loadUVpan("_loadUVpan", Range(0,1) ) = 1
_alpha("_alpha", Range(0,1) ) = 1
_Lighten("_Lighten", Range(0,1) ) = 0
_glow("_glow", Range(-1,1) ) = 0
_CircuitPos("_CircuitPos", Range(0,1) ) = 0
_WaterWavePos("_WaterWavePos", Range(0,1) ) = 0
_WaterFlowPos("_WaterFlowPos", Range(0,1) ) = 0

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


sampler2D _BaseTex;
sampler2D _CircuitTex;
sampler2D _WaveMask;
sampler2D _FlowMask;
sampler2D _loadingMask;
sampler2D _loadingLight;
sampler2D _noiseMask;
float _loadUVpan;
float _alpha;
float _Lighten;
float _glow;
float _CircuitPos;
float _WaterWavePos;
float _WaterFlowPos;

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
				
float4 Sampled2D0=tex2D(_BaseTex,IN.uv_BaseTex.xy);
float4 Sampled2D1=tex2D(_noiseMask,IN.uv_noiseMask.xy);
float4 Add6=Sampled2D0 + Sampled2D1;
float4 Multiply0=_loadUVpan.xxxx * float4( -3,-3,-3,-3 );
float4 Add0=Multiply0 + float4( 1.5,1.5,1.5,1.5 );
float4 UV_Pan0=float4((IN.meshUV.xyxy).x,(IN.meshUV.xyxy).y + Add0.y,(IN.meshUV.xyxy).z,(IN.meshUV.xyxy).w);
float4 Add5=UV_Pan0 + float4( 0.3,0.3,0.3,0.3 );
float4 Tex2D1=tex2D(_loadingLight,Add5.xy);
float4 Multiply3=Add6 * Tex2D1;
float4 Tex2D2=tex2D(_loadingLight,UV_Pan0.xy);
float4 Add4=Multiply3 + Tex2D2;
float4 Add8_1_NoInput = float4(0,0,0,0);
float4 Add8=Add4 + Add8_1_NoInput;
float4 Add1=_Lighten.xxxx + Add8;
float4 Multiply13=Sampled2D0 * float4( 0.9,0.9,0.9,0.9 );
float4 Add10=_glow.xxxx + Sampled2D0;
float4 Multiply1=Multiply13 * Add10;
float4 Add3=Add1 + Multiply1;
float4 Multiply10=Sampled2D0 * float4( 0.7,0.7,0.7,0.7 );
float4 Multiply11=_WaterFlowPos.xxxx * float4( -1,-1,-1,-1 );
float4 Add15=Multiply11 + float4( 0,0,0,0 );
float4 UV_Pan3=float4((IN.meshUV.zwzw).x,(IN.meshUV.zwzw).y + Add15.y,(IN.meshUV.zwzw).z,(IN.meshUV.zwzw).w);
float4 Tex2D7=tex2D(_FlowMask,UV_Pan3.xy);
float4 Multiply12=Multiply10 * Tex2D7;
float4 Multiply8=_WaterWavePos.xxxx * float4( -0.7,-0.7,-0.7,-0.7 );
float4 Add13=Multiply8 + float4( 0,0,0,0 );
float4 UV_Pan2=float4((IN.meshUV.zwzw).x,(IN.meshUV.zwzw).y + Add13.y,(IN.meshUV.zwzw).z,(IN.meshUV.zwzw).w);
float4 Tex2D6=tex2D(_WaveMask,UV_Pan2.xy);
float4 Multiply9=Multiply10 * Tex2D6;
float4 Add17=Multiply12 + Multiply9;
float4 Tex2D5=tex2D(_CircuitTex,(IN.meshUV.xyxy).xy);
float4 Multiply15=Tex2D5 * float4( 3,3,3,3 );
float4 Multiply6=_CircuitPos.xxxx * float4( -0.8,-0.8,-0.8,-0.8 );
float4 Add11=Multiply6 + float4( 0,0,0,0 );
float4 UV_Pan1=float4((IN.meshUV.zwzw).x,(IN.meshUV.zwzw).y + Add11.y,(IN.meshUV.zwzw).z,(IN.meshUV.zwzw).w);
float4 Tex2D4=tex2D(_CircuitTex,UV_Pan1.xy);
float4 Multiply7=Multiply15 * Tex2D4.aaaa;
float4 Add16=Add17 + Multiply7;
float4 Add12=Add3 + Add16;
float4 Add14=Tex2D7 + Tex2D6;
float4 Split0=Multiply7;
float4 Add18=Add14 + float4( Split0.z, Split0.z, Split0.z, Split0.z);
float4 Multiply14=Add18 * float4( 0.2,0.2,0.2,0.2 );
float4 Multiply18=Sampled2D0.aaaa * float4( 4,4,4,4 );
float4 Saturate1=saturate(Multiply18);
float4 Multiply17=Multiply14 * Saturate1;
float4 Add19=Sampled2D0.aaaa + Multiply17;
float4 Multiply2=Add19 * _alpha.xxxx;
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
float4 Multiply5=Saturate0 * float4( 1,1,1,1 );
float4 Subtract0=Multiply5 - float4( 0.1,0.1,0.1,0.1 );
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Subtract0 );
o.Emission = Add12;
o.Alpha = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
