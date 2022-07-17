 Shader "Custom/BackFaceShader" {
     Properties {
         _Color ("Highlight Color", Color) = (1,1,1,0)
     }
 SubShader {
         Pass {
             Cull Front
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             fixed4 _Color;
            
                struct MeshData 
                {
                    float4 vertex : POSITION; 
                };

                struct Interpolators 
                {
                    float4 vertex : SV_POSITION;
                
                };
                
                Interpolators vert(MeshData v) 
                {
                    Interpolators i;
                    i.vertex=UnityObjectToClipPos(v.vertex);
                    return i;
                }

                fixed4 frag(Interpolators i) : SV_TARGET
                {
                    return _Color;
                }
             ENDCG
         }
     }
     FallBack "Diffuse"
 }
 
