Shader "Custom/BubblesShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SecondTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_BlendMask("Blendmask", 2D) = "blend" {}
		_Alpha("alpha value", Range(0,1)) = 0.0


        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Direction("Direction", Vector) = (0, 0, 0, 0)
		_DirectionLeft("Direction Left", Vector) = (0, 0, 0, 0)
		_Speed("Speed", float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows Lambert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _SecondTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
        };

		half _Alpha;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float4 _Direction;
		float4 _DirectionLeft;
		float _Speed;
		sampler2D _BumpMap;
		sampler2D _BlendMask;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float4 dir = _Direction;
			float4 dirlft = _DirectionLeft;
			float speed = _Speed;
			float a = _Alpha;

			fixed4 bm = tex2D(_BlendMask, IN.uv_MainTex * 2 + _SinTime.x + _Time * speed * float2(_SinTime.x, dir.y * 4));

			fixed4 d = tex2D(_SecondTex, IN.uv_MainTex) * bm.g;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * bm.r;

			o.Albedo = c.rgb + d.rgb * _Color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + _Time * speed * float2(-_SinTime.x / 2, dir.y)) * bm.g);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Alpha = a + bm.g;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
