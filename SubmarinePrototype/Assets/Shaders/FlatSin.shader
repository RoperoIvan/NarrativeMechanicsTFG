Shader "Custom/FlatSin"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ForegroundColor("Foreground Color", Color) = (1,1,1,1)
		_BackgroundColor("Background Color", Color) = (0,0,0,1)
		_ForegroundMask("Foreground Mask", 2D) = "white" {}
		_ForegroundCutoff("Foreground Cutoff", Range(0,1)) = 0.5
		_BackgroundCutoff("Background Cutoff", Range(0,1)) = 0.5
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0

			sampler2D _ForegroundMask;

		struct Input {
			float2 uv_ForegroundMask;
		};

		fixed4 _ForegroundColor;
		fixed4 _BackgroundColor;
		half _ForegroundCutoff;
		half _BackgroundCutoff;

		void surf(Input IN, inout SurfaceOutputStandard o) {

			fixed x = (-0.5 + IN.uv_ForegroundMask.x) * 2;
			fixed y = (-0.5 + IN.uv_ForegroundMask.y) * 2;

			// Albedo comes from a texture tinted by color
			fixed radius = 0.5 + sin(x * 3.1415926535) * 0.5;
			radius -= (0.5 * y + 0.5);
			clip(radius - _BackgroundCutoff);
			o.Albedo = _BackgroundColor;
			if (radius > _ForegroundCutoff) {
				o.Albedo = _ForegroundColor;
			}
			//o.Albedo.r = sin(x * 3.14) * 0.5 + 0.5;
			//o.Albedo.g = 0;
			//o.Albedo.b = 0;
			o.Alpha = 1;
		}
		ENDCG
		}
			FallBack "Diffuse"
}
