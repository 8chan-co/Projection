Shader "Ophura/Projection" {
  Properties {
    Pixels_0000 ("Pixels 0000", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0001 ("Pixels 0001", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0002 ("Pixels 0002", Color) = (1.0, 1.0, 1.0, 1.0)
    Pixels_0003 ("Pixels 0003", Color) = (1.0, 1.0, 1.0, 1.0)
  }
  CustomEditor "Ophura.Totally"
  SubShader {
    Tags {
      "Queue" = "Geometry"
      "RenderType" = "Opaque"
      "ForceNoShadowCasting" = "True"
      "DisableBatching" = "True"
      "IgnoreProjector" = "True"
      "PreviewType" = "Plane"
    }

    HLSLINCLUDE
      #pragma vertex VSMain
      #pragma fragment PSMain

      #include <UnityShaderVariables.cginc>

      class HomogenousSpace {
        vector<float, 2> Coordinates :texcoord;
        vector<float, 4> Position :sv_position;
      };

      #define __intrinsic inline static

      __intrinsic float invert(float x) {
        #if UNITY_UV_STARTS_AT_TOP
          return 1.0F - x;
        #else
          return x;
        #endif
      }
    ENDHLSL

    Pass {
      Name "0000"
      Tags {
        "LightMode" = "Always"
      }
      HLSLPROGRAM
        extern uniform vector<unorm float, 4> Pixels_0000[64 * 64] <string Quadrant="First";>;

        class HomogenousSpace VSMain(vector<float, 4> Position :position, vector<float, 2> Coordinates :texcoord) {
          class HomogenousSpace Result;

          Coordinates.y = invert(Coordinates.y);
          Result.Coordinates = Coordinates * 128.0F;

          Result.Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, Position));

          return Result;
        }

        vector<unorm float, 4> PSMain(vector<float, 2> Coordinates :texcoord) :sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(Coordinates);

          if (Quadrant.x >= 64U || Quadrant.y >= 64U) {
            discard; return vector<unorm float, 4>(1.0F, 1.0F, 1.0F, 1.0F);
          }

          return Pixels_0000[mad(Quadrant.y, 64U, Quadrant.x)];
        }
      ENDHLSL
    }

    Pass {
      Name "0001"
      Tags {
        "LightMode" = "Always"
      }
      HLSLPROGRAM
        extern uniform vector<unorm float, 4> Pixels_0001[64 * 64] <string Quadrant="Second";>;

        class HomogenousSpace VSMain(vector<float, 4> Position :position, vector<float, 2> Coordinates :texcoord) {
          class HomogenousSpace Result;

          Coordinates.y = invert(Coordinates.y);
          Result.Coordinates = Coordinates * 128.0F;

          Result.Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, Position));

          return Result;
        }

        vector<unorm float, 4> PSMain(vector<float, 2> Coordinates :texcoord) :sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(Coordinates);

          if (Quadrant.x < 64U || Quadrant.y >= 64U) {
            discard; return vector<unorm float, 4>(1.0F, 1.0F, 1.0F, 1.0F);
          }

          return Pixels_0001[mad(Quadrant.y, 64U, Quadrant.x - 64U)];
        }
      ENDHLSL
    }

    Pass {
      Name "0002"
      Tags {
        "LightMode" = "Always"
      }
      HLSLPROGRAM
        extern uniform vector<unorm float, 4> Pixels_0002[64 * 64] <string Quadrant="Third";>;

        class HomogenousSpace VSMain(vector<float, 4> Position :position, vector<float, 2> Coordinates :texcoord) {
          class HomogenousSpace Result;

          Coordinates.y = invert(Coordinates.y);
          Result.Coordinates = Coordinates * 128.0F;

          Result.Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, Position));

          return Result;
        }

        vector<unorm float, 4> PSMain(vector<float, 2> Coordinates :texcoord) :sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(Coordinates);

          if (Quadrant.x >= 64U || Quadrant.y < 64U) {
            discard; return vector<unorm float, 4>(1.0F, 1.0F, 1.0F, 1.0F);
          }

          return Pixels_0002[mad(Quadrant.y - 64U, 64U, Quadrant.x)];
        }
      ENDHLSL
    }

    Pass {
      Name "0003"
      Tags {
        "LightMode" = "Always"
      }
      HLSLPROGRAM
        extern uniform vector<unorm float, 4> Pixels_0003[64 * 64] <string Quadrant="Fourth";>;

        class HomogenousSpace VSMain(vector<float, 4> Position :position, vector<float, 2> Coordinates :texcoord) {
          class HomogenousSpace Result;

          Coordinates.y = invert(Coordinates.y);
          Result.Coordinates = Coordinates * 128.0F;

          Result.Position = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, Position));

          return Result;
        }

        vector<unorm float, 4> PSMain(vector<float, 2> Coordinates :texcoord) :sv_target {
          vector<uint, 2> Quadrant = vector<uint, 2>(Coordinates);

          if (Quadrant.x < 64U || Quadrant.y < 64U) {
            discard; return vector<unorm float, 4>(1.0F, 1.0F, 1.0F, 1.0F);
          }

          return Pixels_0003[mad(Quadrant.y - 64U, 64U, Quadrant.x - 64U)];
        }
      ENDHLSL
    }
  }
}
