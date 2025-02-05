Shader "Custom/Outline Mask" {
  Properties {

  }

  SubShader {
    Tags {
      "Queue" = "Transparent+100"
      "RenderType" = "Transparent"
    }

    Pass {
      Name "Mask"
      Cull Off
      ZTest Always
      ZWrite Off
      ColorMask 0

      Stencil {
        Ref 1
        Pass Replace
      }
    }
  }
}
