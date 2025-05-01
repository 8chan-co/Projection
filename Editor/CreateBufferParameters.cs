using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void CreateBufferParameters(AnimatorController Controller)
        {
            for (int Index = 0; Index < PixelCount; ++Index)
            {
                foreach (string Name in PropertyNames)
                {
                    Controller.AddParameter(new AnimatorControllerParameter
                    {
                        name = $"{Name}_{Index:X4}",
                        type = AnimatorControllerParameterType.Float
                    });
                }
            }
        }
    }
}
