Shader "Sign_downTimer"
{
	Properties 
	{
_TimerTex("_TimerTex", 2D) = "black" {}
_TimerBar("_TimerBar", 2D) = "black" {}
_girdMask("_girdMask", 2D) = "black" {}
_TimerValue("_TimerValue", Range(0,1) ) = 0
_alpha("_alpha", Range(0,1) ) = 1

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Overlay"
"IgnoreProjector"="False"
"RenderType"="Transparent"

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
#pragma surface surf BlinnPhongEditor  alpha decal:add vertex:vert
#pragma target 3.0


sampler2D _TimerTex;
sampler2D _TimerBar;
sampler2D _girdMask;
float _TimerValue;
float _alpha;

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
				float2 uv_TimerTex;
float2 uv_TimerBar;
float2 uv_girdMask;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D1=tex2D(_TimerTex,IN.uv_TimerTex.xy);
float4 Sampled2D2=tex2D(_TimerBar,IN.uv_TimerBar.xy);
float4 Multiply1=Sampled2D2 * float4( 0.7,0.7,0.7,0.7 );
float4 Add1=_TimerValue.xxxx + float4( 0,0,0,0 );
float4 Add0=_TimerValue.xxxx + float4( 0.01,0.01,0.01,0.01 );
float4 SmoothStep0=smoothstep(Add1,Add0,Sampled2D1.aaaa);
float4 Multiply0=Multiply1 * SmoothStep0;
float4 Multiply7=Multiply0 * _alpha.xxxx;
float4 Sampled2D0=tex2D(_girdMask,IN.uv_girdMask.xy);
float4 Multiply8=Multiply7 * Sampled2D0;
float4 Add2=_alpha.xxxx + float4( -0.01,-0.01,-0.01,-0.01 );
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Add2 );
o.Albedo = Sampled2D1;
o.Emission = Multiply8;
o.Alpha = _alpha.xxxx;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
