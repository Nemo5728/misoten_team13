Shader "Sign"
{
	Properties 
	{
_gradBar("_gradBar", 2D) = "black" {}
_girdMask("_girdMask", 2D) = "black" {}
_tex("_tex", 2D) = "black" {}
_Player("_Player", Range(1,4) ) = 1
_barValue("_barValue", Range(0,1) ) = 0
_Crown("_Crown", Range(0,1) ) = 0
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

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:add vertex:vert
#pragma target 3.0


sampler2D _gradBar;
sampler2D _girdMask;
sampler2D _tex;
float _Player;
float _barValue;
float _Crown;
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
				float2 uv_tex;
float4 fullMeshUV2;
float2 uv_girdMask;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

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
				
float4 UV_Pan4=float4((IN.uv_tex.xyxy).x,(IN.uv_tex.xyxy).y + float4( -0.75,-0.75,-0.75,-0.75 ).y,(IN.uv_tex.xyxy).z,(IN.uv_tex.xyxy).w);
float4 Ceil0=ceil(_Player.xxxx);
float4 Subtract1=Ceil0 - float4( 1.0, 1.0, 1.0, 1.0 );
float4 Divide0=Subtract1 / float4( 4,4,4,4 );
float4 UV_Pan8=float4(UV_Pan4.x + Divide0.x,UV_Pan4.y,UV_Pan4.z,UV_Pan4.w);
float4 Tex2D4=tex2D(_tex,UV_Pan8.xy);
float4 Step0=step(_Crown.xxxx,float4( 0.5,0.5,0.5,0.5 ));
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Multiply6=Tex2D4 * Invert0;
float4 UV_Pan3=float4((IN.uv_tex.xyxy).x,(IN.uv_tex.xyxy).y + float4( -0.5,-0.5,-0.5,-0.5 ).y,(IN.uv_tex.xyxy).z,(IN.uv_tex.xyxy).w);
float4 UV_Pan7=float4(UV_Pan3.x + Divide0.x,UV_Pan3.y,UV_Pan3.z,UV_Pan3.w);
float4 Tex2D3=tex2D(_tex,UV_Pan7.xy);
float4 Multiply2=Tex2D3 * _barValue.xxxx;
float4 Multiply5=Multiply2 * float4( 2.2,2.2,2.2,2.2 );
float4 Add2=Multiply5 + float4( -0.65,-0.65,-0.65,-0.65 );
float4 Saturate0=saturate(Add2);
float4 UV_Pan1=float4((IN.uv_tex.xyxy).x,(IN.uv_tex.xyxy).y + float4( -0.25,-0.25,-0.25,-0.25 ).y,(IN.uv_tex.xyxy).z,(IN.uv_tex.xyxy).w);
float4 UV_Pan6=float4(UV_Pan1.x + Divide0.x,UV_Pan1.y,UV_Pan1.z,UV_Pan1.w);
float4 Tex2D1=tex2D(_tex,UV_Pan6.xy);
float4 Multiply3=_barValue.xxxx * float4( -0.6,-0.6,-0.6,-0.6 );
float4 Subtract0=Multiply3 - float4( -0.41,-0.41,-0.41,-0.41 );
float4 UV_Pan0=float4((IN.fullMeshUV2).x,(IN.fullMeshUV2).y + Subtract0.y,(IN.fullMeshUV2).z,(IN.fullMeshUV2).w);
float4 Tex2D0=tex2D(_gradBar,UV_Pan0.xy);
float4 Multiply0=Tex2D1 * Tex2D0;
float4 Multiply4=_barValue.xxxx * float4( 0.5,0.5,0.5,0.5 );
float4 Add1=Multiply4 + float4( 0.2,0.2,0.2,0.2 );
float4 Multiply1=Multiply0 * Add1;
float4 UV_Pan5=float4((IN.uv_tex.xyxy).x,(IN.uv_tex.xyxy).y + float4( 0,0,0,0 ).y,(IN.uv_tex.xyxy).z,(IN.uv_tex.xyxy).w);
float4 UV_Pan2=float4(UV_Pan5.x + Divide0.x,UV_Pan5.y,UV_Pan5.z,UV_Pan5.w);
float4 Tex2D2=tex2D(_tex,UV_Pan2.xy);
float4 Add0=Multiply1 + Tex2D2;
float4 Add3=Saturate0 + Add0;
float4 Add4=Multiply6 + Add3;
float4 Multiply7=Add4 * _alpha.xxxx;
float4 Sampled2D0=tex2D(_girdMask,IN.uv_girdMask.xy);
float4 Multiply8=Multiply7 * Sampled2D0;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply8;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}
