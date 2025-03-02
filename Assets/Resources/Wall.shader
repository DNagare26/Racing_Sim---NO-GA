Shader "Custom/DoubleSided"
{
    SubShader
    {
        Cull Off  // Renders both sides of the wall
        Tags {"Queue"="Geometry"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return fixed4(1, 0, 0, 1) ; // White color (change this)
            }
            ENDCG
        }
    }
}
