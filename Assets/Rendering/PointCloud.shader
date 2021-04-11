Shader "Unlit/PointCloud"
{
	Properties
	{
		_MainTex("Color texture", 2D) = "white" {}
		_Depth("Depth texture", 2D) = "white" {}
		_Width("Mesh width", Int) = 640
		_Height("Mesh height", Int) = 480
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				sampler2D _Depth;
				int _Width;
				int _Height;

				v2f vert(appdata v)
				{
					v2f o;
					float2 fracUV = frac(v.uv);
					float centerDistCurr = tex2Dlod(_Depth, float4(fracUV.x, 1 - fracUV.y, 0, 0)).r * 65536 / 1000;

					// Average over time.
					float centerDist = centerDistCurr;

					float upPixelDist = tex2Dlod(_Depth, float4(fracUV.x, (1 - fracUV.y) - 1.0 / _Height, 0, 0)).r * 65536 / 1000;
					float downPixelDist = tex2Dlod(_Depth, float4(fracUV.x, (1 - fracUV.y) + 1.0 / _Height, 0, 0)).r * 65536 / 1000;
					float leftPixelDist = tex2Dlod(_Depth, float4(fracUV.x - 1.0 / _Width, (1 - fracUV.y), 0, 0)).r * 65536 / 1000;
					float rightPixelDist = tex2Dlod(_Depth, float4(fracUV.x + 1.0 / _Width, (1 - fracUV.y), 0, 0)).r * 65536 / 1000;

					// Sample depth and convert from 16-bit depth in mm to meters
					float2 offset = (v.uv - fracUV) - float2(0.5, 0.5);
					fracUV -= float2(offset.x / (float)_Width, offset.y / (float)_Height);
					
					// Distance into scene at top-left position in pixel
					float dist = tex2Dlod(_Depth, float4(fracUV.x, 1 - fracUV.y, 0, 0)).r * 65536 / 1000;
					if (abs(dist - centerDist) < .1) {
						v.vertex *= dist;
					}
					else {
						v.vertex *= centerDist;
					}
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = fracUV;

					float avg = (upPixelDist + downPixelDist + leftPixelDist + rightPixelDist) / 4;

					//if (abs(centerDistCurr - centerDistPrev) > 0.005)
					//	o.uv.x = o.uv.x + 1;
					if (abs(centerDist - avg) > 0.2)
						o.uv.x = o.uv.x + 1;

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					if (i.uv.x > 1.0)
						discard;

					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
					return col;
				}

				ENDCG
			}
		}
}