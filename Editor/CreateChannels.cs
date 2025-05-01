using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static ChildMotion[] CreateChannels(string Identifier)
        {
            List<ChildMotion> Channels = new(PropertyNames.Length);

            foreach (string Name in PropertyNames)
            {
                string Property = $"{Name}_{Identifier}";

                (AnimationClip Infimum, AnimationClip Supremum) = CreateAnimationClips(Property);

                BlendTree Channel = new()
                {
                    name = $"Channel:{Property}",
                    hideFlags = HideFlags.HideInHierarchy,
                    blendParameter = Property,
                    blendType = BlendTreeType.Simple1D,
                    children = new ChildMotion[2]
                    {
                        new()
                        {
                            motion = Infimum,
                            timeScale = 1F
                        },
                        new()
                        {
                            motion = Supremum,
                            timeScale = 1F
                        }
                    }
                };

                Channels.Add(new()
                {
                    motion = Channel,
                    timeScale = 1F,
                    directBlendParameter = InfluenceParameter
                });

                AssetDatabase.AddObjectToAsset(Channel, ControllerFilename);
            }

            return Channels.ToArray();
        }
    }
}
