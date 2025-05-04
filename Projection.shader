Shader "Ophura/Projection"
{
    Properties
    {
        [Header(1st Pixel)]
        R_0000 ("0000:R", Range(0.0, 1.0)) = 0.08353512
        G_0000 ("0000:G", Range(0.0, 1.0)) = 0.08353512
        B_0000 ("0000:B", Range(0.0, 1.0)) = 0.08353512

        [Header(2nd Pixel)]
        R_0001 ("0001:R", Range(0.0, 1.0)) = 0.08353512
        G_0001 ("0001:G", Range(0.0, 1.0)) = 0.08353512
        B_0001 ("0001:B", Range(0.0, 1.0)) = 0.08353512

        [Header(3rd Pixel)]
        R_0002 ("0002:R", Range(0.0, 1.0)) = 0.08353512
        G_0002 ("0002:G", Range(0.0, 1.0)) = 0.08353512
        B_0002 ("0002:B", Range(0.0, 1.0)) = 0.08353512

        [Header(4th Pixel)]
        R_0003 ("0003:R", Range(0.0, 1.0)) = 0.08353512
        G_0003 ("0003:G", Range(0.0, 1.0)) = 0.08353512
        B_0003 ("0003:B", Range(0.0, 1.0)) = 0.08353512

        [Header(5th Pixel)]
        R_0004 ("0004:R", Range(0.0, 1.0)) = 0.08353512
        G_0004 ("0004:G", Range(0.0, 1.0)) = 0.08353512
        B_0004 ("0004:B", Range(0.0, 1.0)) = 0.08353512

        [Header(6th Pixel)]
        R_0005 ("0005:R", Range(0.0, 1.0)) = 0.08353512
        G_0005 ("0005:G", Range(0.0, 1.0)) = 0.08353512
        B_0005 ("0005:B", Range(0.0, 1.0)) = 0.08353512

        [Header(7th Pixel)]
        R_0006 ("0006:R", Range(0.0, 1.0)) = 0.08353512
        G_0006 ("0006:G", Range(0.0, 1.0)) = 0.08353512
        B_0006 ("0006:B", Range(0.0, 1.0)) = 0.08353512

        [Header(8th Pixel)]
        R_0007 ("0007:R", Range(0.0, 1.0)) = 0.08353512
        G_0007 ("0007:G", Range(0.0, 1.0)) = 0.08353512
        B_0007 ("0007:B", Range(0.0, 1.0)) = 0.08353512

        [Header(9th Pixel)]
        R_0008 ("0008:R", Range(0.0, 1.0)) = 0.08353512
        G_0008 ("0008:G", Range(0.0, 1.0)) = 0.08353512
        B_0008 ("0008:B", Range(0.0, 1.0)) = 0.08353512

        [Header(10th Pixel)]
        R_0009 ("0009:R", Range(0.0, 1.0)) = 0.08353512
        G_0009 ("0009:G", Range(0.0, 1.0)) = 0.08353512
        B_0009 ("0009:B", Range(0.0, 1.0)) = 0.08353512

        [Header(11th Pixel)]
        R_000A ("000A:R", Range(0.0, 1.0)) = 0.08353512
        G_000A ("000A:G", Range(0.0, 1.0)) = 0.08353512
        B_000A ("000A:B", Range(0.0, 1.0)) = 0.08353512

        [Header(12th Pixel)]
        R_000B ("000B:R", Range(0.0, 1.0)) = 0.08353512
        G_000B ("000B:G", Range(0.0, 1.0)) = 0.08353512
        B_000B ("000B:B", Range(0.0, 1.0)) = 0.08353512

        [Header(13th Pixel)]
        R_000C ("000C:R", Range(0.0, 1.0)) = 0.08353512
        G_000C ("000C:G", Range(0.0, 1.0)) = 0.08353512
        B_000C ("000C:B", Range(0.0, 1.0)) = 0.08353512

        [Header(14th Pixel)]
        R_000D ("000D:R", Range(0.0, 1.0)) = 0.08353512
        G_000D ("000D:G", Range(0.0, 1.0)) = 0.08353512
        B_000D ("000D:B", Range(0.0, 1.0)) = 0.08353512

        [Header(15th Pixel)]
        R_000E ("000E:R", Range(0.0, 1.0)) = 0.08353512
        G_000E ("000E:G", Range(0.0, 1.0)) = 0.08353512
        B_000E ("000E:B", Range(0.0, 1.0)) = 0.08353512

        [Header(16th Pixel)]
        R_000F ("000F:R", Range(0.0, 1.0)) = 0.08353512
        G_000F ("000F:G", Range(0.0, 1.0)) = 0.08353512
        B_000F ("000F:B", Range(0.0, 1.0)) = 0.08353512
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "Projection"

            HLSLPROGRAM
            #include <UnityCG.cginc>
            
            extern uniform unorm float
            R_0000, G_0000, B_0000,
            R_0001, G_0001, B_0001,
            R_0002, G_0002, B_0002,
            R_0003, G_0003, B_0003,
            R_0004, G_0004, B_0004,
            R_0005, G_0005, B_0005,
            R_0006, G_0006, B_0006,
            R_0007, G_0007, B_0007,
            R_0008, G_0008, B_0008,
            R_0009, G_0009, B_0009,
            R_000A, G_000A, B_000A,
            R_000B, G_000B, B_000B,
            R_000C, G_000C, B_000C,
            R_000D, G_000D, B_000D,
            R_000E, G_000E, B_000E,
            R_000F, G_000F, B_000F;
            
            vector<unorm float, 4> QueryColourFor(dword Index)
            {
                switch (Index)
                {
                    case 0x0000U: return vector<unorm float, 4>(R_0000, G_0000, B_0000, 1.0F);
                    case 0x0001U: return vector<unorm float, 4>(R_0001, G_0001, B_0001, 1.0F);
                    case 0x0002U: return vector<unorm float, 4>(R_0002, G_0002, B_0002, 1.0F);
                    case 0x0003U: return vector<unorm float, 4>(R_0003, G_0003, B_0003, 1.0F);
                    case 0x0004U: return vector<unorm float, 4>(R_0004, G_0004, B_0004, 1.0F);
                    case 0x0005U: return vector<unorm float, 4>(R_0005, G_0005, B_0005, 1.0F);
                    case 0x0006U: return vector<unorm float, 4>(R_0006, G_0006, B_0006, 1.0F);
                    case 0x0007U: return vector<unorm float, 4>(R_0007, G_0007, B_0007, 1.0F);
                    case 0x0008U: return vector<unorm float, 4>(R_0008, G_0008, B_0008, 1.0F);
                    case 0x0009U: return vector<unorm float, 4>(R_0009, G_0009, B_0009, 1.0F);
                    case 0x000AU: return vector<unorm float, 4>(R_000A, G_000A, B_000A, 1.0F);
                    case 0x000BU: return vector<unorm float, 4>(R_000B, G_000B, B_000B, 1.0F);
                    case 0x000CU: return vector<unorm float, 4>(R_000C, G_000C, B_000C, 1.0F);
                    case 0x000DU: return vector<unorm float, 4>(R_000D, G_000D, B_000D, 1.0F);
                    case 0x000EU: return vector<unorm float, 4>(R_000E, G_000E, B_000E, 1.0F);
                    case 0x000FU: return vector<unorm float, 4>(R_000F, G_000F, B_000F, 1.0F);
                    
                    default: return vector<unorm float, 4>(0.08353512F, 0.08353512F, 0.08353512F, 1.0F);
                }
            }
            
            #pragma vertex VShader
            void VShader(appdata_img Input, out v2f_img Output)
            {
                Input.texcoord.y = 1.0F - Input.texcoord.y;

                Input.texcoord *= 4.0F;
                
                Output = vert_img(Input);
            }
            
            #pragma fragment FShader
            void FShader(v2f_img Input, out vector<unorm float, 4> Output : SV_Target)
            {
                Input.uv = floor(Input.uv);
                
                dword TheChosenOne = mad(dword(Input.uv.y), 4U, dword(Input.uv.x));
                
                Output = QueryColourFor(TheChosenOne);
            }
            ENDHLSL
        }
    }
}
