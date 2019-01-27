Shader "Custom/ShapeCutout"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        ColorMask 0
        ZWrite Off

        Stencil {
            Ref 2
            Comp always
            Pass replace
        }

        Pass
        {
        }
    }
}
