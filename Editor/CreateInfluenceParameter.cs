using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void CreateInfluenceParameter(AnimatorController Controller) => Controller.AddParameter(new()
        {
            name = InfluenceParameter,
            type = AnimatorControllerParameterType.Float,
            defaultFloat = 1F
        });
    }
}
