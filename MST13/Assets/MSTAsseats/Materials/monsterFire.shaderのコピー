Shader "monsterFire"
{
	Properties 
	{
_toon1("_toon1", Range(0,1) ) = 0.946
_toon2("_toon2", Range(0,1) ) = 0.704
_toon3("_toon3", Range(0,1) ) = 0.462
_C1("_C1", Color) = (1,0.7352941,0.9598377,1)
_C2("_C2", Color) = (1,0.7352941,0.8941177,1)
_C3("_C3", Color) = (1,0.9779412,0.9948276,1)
_C4("_C4", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Overlay"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

 Pass {
            ZWrite On
            ColorMask 0
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


float _toon1;
float _toon2;
float _toon3;
float4 _C1;
float4 _C2;
float4 _C3;
float4 _C4;

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
				float3 viewDir;

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
				
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,float4( 0.4,0.4,0.4,0.4 ));
float4 Step0=step(Pow0,_toon1.xxxx);
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Multiply0=_C1 * Invert0;
float4 Step1=step(Pow0,_toon2.xxxx);
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - Step1;
float4 Subtract1=Invert1 - Invert0;
float4 Multiply4=_C2 * Subtract1;
float4 Add2=Multiply0 + Multiply4;
float4 Step2=step(Pow0,_toon3.xxxx);
float4 Invert2= float4(1.0, 1.0, 1.0, 1.0) - Step2;
float4 Subtract0=Invert2 - Invert1;
float4 Multiply2=_C3 * Subtract0;
float4 Add0=Add2 + Multiply2;
float4 Step3=step(Pow0,float4( 0.0, 0.0, 0.0, 0.0 ));
float4 Invert3= float4(1.0, 1.0, 1.0, 1.0) - Step3;
float4 Subtract2=Invert3 - Invert2;
float4 Multiply1=_C4 * Subtract2;
float4 Add1=Add0 + Multiply1;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Add1;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
