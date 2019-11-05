Shader "Customized/DistortionCorrection"
{
	Properties{
		k1("k1", Float) = -0.244450627//e-01
		k2("k2", Float) = 0.0719937237//e-02
		p1("p1", Float) = 0.000514066681//e-04
		p2("p2", Float) = 0.000235650087//e-04
		k3("k3", Float) = -0.0102982879//e-02
		fx("fx", Float) = 885.94368422
		cx("cx", Float) = 948.48891401
		fy("fy", Float) = 896.37970921
		cy("cy", Float) = 515.39652463
		
		videoWidth("Video Width", Float) = 1920
		videoHeight("Video Height", Float) = 1080
		scale("scale", Float) = 0.7
		_MainTex("Texture", 2D) = "white" { }
		_Color("Main Color", Color) = (1,1,1,0.5)
		_SwapRedAndGreen("Swap Red and Green Channel", Int) = 1
	}

	SubShader {
		Tags { 
			"RenderType" = "Opaque" 
		}
		// LOD 100
		Pass {			
			CGPROGRAM
			#pragma enable_d3d11_debug_symbols
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			int _SwapRedAndGreen;
			float k1, k2, p1, p2, k3, fx, fy, cx, cy;

			float videoWidth, videoHeight;
			float scale;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			float2 radial(float2 input, float rSquare, float k1, float k2, float k3) {
				float2 output = input;
				output.x = input.x * (1 + k1 * rSquare + k2 * pow(rSquare, 2) + k3 * pow(rSquare, 3));
				output.y = input.y * (1 + k1 * rSquare + k2 * pow(rSquare, 2) + k3 * pow(rSquare, 3));
				return output;
			}

			float2 tangential(float2 input, float rSquare, float p1, float p2) {
				float2 output = input;
				output.x = (2 * p1 * input.x * input.y + p2 * (rSquare + 2 * pow(input.x, 2)));
				output.y = (p1 * (rSquare + 2 * pow(input.y, 2)) + 2 * p2 * input.x * input.y);
				return output;
			}

			float2 ApplyBarrelDistortion(float2 texCoord) {
				float2 relativePos;
				relativePos.x = (texCoord.x * videoWidth - cx) / (fx * scale);
				relativePos.y = (texCoord.y * videoHeight - cy) / (fy * scale);

				//if (outOfBound(relativePos))
				//	return relativePos;

				float rSquare = pow(relativePos.x, 2) + pow(relativePos.y, 2);
				// Apply radial and tangentinal undistortion.
				float2 correctedRelativePos = radial(relativePos, rSquare, k1, k2, k3) + tangential(relativePos, rSquare, p1, p2);

				// Convert to back absolute coords.
				float2 correctedPos;
				correctedPos.x = (correctedRelativePos.x * fx + cx) / videoWidth;
				correctedPos.y = (correctedRelativePos.y * fy + cy) / videoHeight;

				return correctedPos;
			}

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex); // automatically apply MVP matrix
				//o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); // make sure texture tiling and offset is applied correctly
				o.uv = ApplyBarrelDistortion(v.texcoord); // 
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				if(_SwapRedAndGreen > 0)
					return texcol.bgra * _Color;
				else
					return texcol.rgba * _Color;
			}
			ENDCG
		}
	}
}