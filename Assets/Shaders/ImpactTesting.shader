// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.


Shader "Custom/RefractionShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_PointColor("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ImpactSize("ImpactSize", Float) = 0.5
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows nolightmap alpha

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			fixed4 _PointColor;

			float _ImpactSize;

			int _PointsSize;
			fixed4 _Points[50];

			/*void vert(inout appdata_full i, out Input o) {
				float4 pos = UnityObjectToClipPos(i.vertex);
				o.grabUV = ComputeGrabScreenPos(pos);
				o.refract = float4(i.normal,0) * _RefractionAmount;
				o.worldPos = pos;
			}

			
*/
			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)


			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				fixed emissive = 0;

				float3 objPos = mul(unity_WorldToObject, float4(IN.worldPos, 1)).xyz;

				for (int i = 0; i < _PointsSize; ++i) {
					//emissive += max(0, 1 - distance(_Points[i].xyz, objPos.xyz));
					emissive += frac(1.0 - max(0, (_Points[i].w * _ImpactSize) - distance(_Points[i].xyz, objPos.xyz)) / _ImpactSize) * (1 - _Points[i].w);
				}

				// Albedo comes from a texture tinted by color
				
				o.Albedo = c.rgb;
				o.Emission = emissive * _PointColor;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}