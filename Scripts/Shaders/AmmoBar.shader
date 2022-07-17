Shader "Custom/AmmoBar"
{
    Properties
    {
        _FullColor("Full Color", Color) = (1,1,1,1)
        _EmptyColor("Empty Color", Color) = (1,1,1,1)
        _BarValue("Bar Value", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _FullColor;
            float4 _EmptyColor;
            float _BarValue;

            Interpolators vert (MeshData v)
            {
                Interpolators i;
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = v.uv;
                return i;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float4 blackCol = float4(0,0,0,0);
                float2 coords = i.uv;
                
                float t =_BarValue; 
                float4 color =  lerp(_EmptyColor, _FullColor, t );
                color = lerp(color,blackCol,step(_BarValue,coords.x));
              
                return color;
            }
            ENDCG
        }
    }
}
