Shader "Ophura/Projection" {
  Properties {
    Pixels_0000 ("Pixels 0000", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0001 ("Pixels 0001", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0002 ("Pixels 0002", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0003 ("Pixels 0003", Color) = (1.0, 1.0, 1.0, 1.0)
  }
  SubShader {
    Tags {
      "Queue" = "Geometry"
      "RenderType" = "Opaque"
      "PreviewType" = "Plane"
    }

    HLSLINCLUDE
      #pragma vertex VertexProgram
      #pragma fragment PixelProgram

      #include <UnityShaderVariables.cginc>

      #define __intrinsic inline static

      __intrinsic float invert(float x) {
        return 1.0F - x;
      }
    ENDHLSL

    Pass {
      Name "0000"
      Tags {
        "LightMode"="Always"
      }
      HLSLPROGRAM
        extern uniform vector<float, 4> Pixels_0000[32 * 32];

        vector<float, 2> VertexProgram(inout vector<float, 4> Position: position, in vector<float, 4> UV: texcoord): texcoord {
          Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, vector<float, 4>(Position.xyz, 1.0F)));

          UV.y = invert(UV.y);
          UV *= 64;

          return UV;
        }

        vector<float, 4> PixelProgram(in vector<float, 4> UV: texcoord): sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(UV.xy); // (uint2)trunc(UV.xy)

          if (Quadrant.x >= 32U || Quadrant.y >= 32U) {
            discard; // NOTE: execution continues after this, should consider returning as well?
          }

          return Pixels_0000[mad(Quadrant.y, 32U, Quadrant.x)];
        }
      ENDHLSL
    }

    Pass {
      Name "0001"
      Tags {
        "LightMode"="Always"
      }
      HLSLPROGRAM
        extern uniform vector<float, 4> Pixels_0001[32 * 32];

        vector<float, 2> VertexProgram(inout vector<float, 4> Position: position, in vector<float, 4> UV: texcoord): texcoord {
          Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, vector<float, 4>(Position.xyz, 1.0F)));

          UV.y = invert(UV.y);
          UV *= 64.0F;

          return UV;
        }

        vector<float, 4> PixelProgram(in vector<float, 4> UV: texcoord): sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(UV.xy); // (uint2)trunc(UV.xy)

          if (Quadrant.x < 32U || Quadrant.y >= 32U) {
            discard; // NOTE: execution continues after this, should consider returning as well?
          }

          return Pixels_0001[mad(Quadrant.y, 32U, Quadrant.x - 32U)];
        }
      ENDHLSL
    }

    Pass {
      Name "0002"
      Tags {
        "LightMode"="Always"
      }
      HLSLPROGRAM
        extern uniform vector<float, 4> Pixels_0002[32 * 32];

        vector<float, 2> VertexProgram(inout vector<float, 4> Position: position, in vector<float, 4> UV: texcoord): texcoord {
          Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, vector<float, 4>(Position.xyz, 1.0F)));

          UV.y = invert(UV.y);
          UV *= 64.0F;

          return UV;
        }

        vector<float, 4> PixelProgram(in vector<float, 4> UV: texcoord): sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(UV.xy); // (uint2)trunc(UV.xy)

          if (Quadrant.x >= 32U || Quadrant.y < 32U) {
            discard; // NOTE: execution continues after this, should consider returning as well?
          }

          return Pixels_0002[mad(Quadrant.y - 32U, 32U, Quadrant.x)];
        }
      ENDHLSL
    }

    Pass {
      Name "0003"
      Tags {
        "LightMode"="Always"
      }
      HLSLPROGRAM
        extern uniform vector<float, 4> Pixels_0003[32 * 32];

        vector<float, 2> VertexProgram(inout vector<float, 4> Position: position, in vector<float, 4> UV: texcoord): texcoord {
          Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, vector<float, 4>(Position.xyz, 1.0F)));

          UV.y = invert(UV.y);
          UV *= 64.0F;

          return UV;
        }

        vector<float, 4> PixelProgram(in vector<float, 4> UV: texcoord): sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(UV.xy); // (uint2)trunc(UV.xy)

          if (Quadrant.x < 32U || Quadrant.y < 32U) {
            discard; // NOTE: execution continues after this, should consider returning as well?
          }

          return Pixels_0003[mad(Quadrant.y - 32U, 32U, Quadrant.x - 32U)];
        }
      ENDHLSL
    }
  }
}
