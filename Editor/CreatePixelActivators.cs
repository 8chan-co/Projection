using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static ChildMotion[] CreatePixelActivators()
        {
            List<ChildMotion> Pixels = new(PixelCount);

            for (int Index = 0; Index < PixelCount; ++Index)
            {
                string Identifier = $"{Index:X4}";

                BlendTree Pixel = new()
                {
                    name = $"Pixel Activator:{Identifier}",
                    hideFlags = HideFlags.HideInHierarchy,
                    blendType = BlendTreeType.Direct,
                    children = CreateChannels(Identifier)
                };

                Pixels.Add(new()
                {
                    motion = Pixel,
                    timeScale = 1F,
                    directBlendParameter = InfluenceParameter
                });

                AssetDatabase.AddObjectToAsset(Pixel, ControllerFilename);
            }

            return Pixels.ToArray();
        }
    }
}
