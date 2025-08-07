Shader "Custom/SphereFog"
{
    Properties {
        _Radius ("Visibility Radius", Float) = 0.01
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _Radius;
            float3 _PlayerPos;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float dist = distance(i.worldPos, _PlayerPos);
                float alpha = saturate(dist / _Radius); // 线性渐变
                return fixed4(0,0,0, alpha); // 黑色迷雾
            }
            ENDCG
        }
    }
}