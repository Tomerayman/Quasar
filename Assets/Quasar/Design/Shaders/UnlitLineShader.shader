Shader "Custom/GridShader"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _BackgroundColor ("Background Color", Color) = (0,0,0,0)
        _LineThickness ("Line Thickness", Float) = 0.02
        _EdgeSmoothness ("Edge Smoothness", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _LineColor;
            fixed4 _BackgroundColor;
            float _LineThickness;
            float _EdgeSmoothness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Assuming UVs are normalized across the quads
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the grid edge mask with edge smoothness
                float edgeX = smoothstep(_LineThickness - _EdgeSmoothness, _LineThickness + _EdgeSmoothness, 0.5 - abs(frac(i.uv.x) - 0.5));
                float edgeY = smoothstep(_LineThickness - _EdgeSmoothness, _LineThickness + _EdgeSmoothness, 0.5 - abs(frac(i.uv.y) - 0.5));

                // Combine the horizontal and vertical edges
                float gridEdges = 1 - min(edgeX, edgeY);

                // Blend between line color and background color
                return lerp(_BackgroundColor, _LineColor, gridEdges);
            }
            ENDCG
        }
    }
    FallBack "Transparent/VertexLit"
}
