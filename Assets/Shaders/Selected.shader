Shader "CardSorter/Selected"
{
	Properties
	{
		_StartColor("Start Color", Color) = (1, 1, 1, 1)
		_EndColor("End Color", Color) = (1, 1, 1, 1)
		_AnimationDuration("Animation Duration", float) = 1
		_GlowWidth("Glow Width", float) = 0.1
		_GlowXModifier("Glow X Modifier", float) = 0.1
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
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			fixed4 _StartColor;
			fixed4 _EndColor;
			float _AnimationDuration;
			float _GlowWidth;
			float _GlowXModifier;
			uniform float _LastSelectTime;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float time : TEXCOORD1;
				fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.uv;
				o.time = (_Time.y - _LastSelectTime) / _AnimationDuration; // Animation time
				float clampedTime = min(o.time.x, 1); // Clamped animation time
				o.color = lerp(_StartColor, _EndColor, clampedTime);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float glowPosition = i.texcoord.y + (i.texcoord.x - 0.5) * _GlowXModifier;
				float glowInterpolation = max(1 - distance(i.time, glowPosition) / _GlowWidth, 0);
				return lerp(i.color, fixed4(1, 1, 1, 1), glowInterpolation);
            }
            ENDCG
        }
    }
}
