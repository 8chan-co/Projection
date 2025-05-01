using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static (AnimationClip Infimum, AnimationClip Supremum) CreateAnimationClips(string Property)
        {
            AnimationClip Infimum = new()
            {
                name = $"Infimum:{Property}",
                hideFlags = HideFlags.HideInHierarchy
            };

            AnimationClip Supremum = new()
            {
                name = $"Supremum:{Property}",
                hideFlags = HideFlags.HideInHierarchy
            };

            EditorCurveBinding Binding = EditorCurveBinding.FloatCurve("Body", typeof(MeshRenderer), $"material.{Property}");

            AnimationUtility.SetEditorCurve(Infimum, Binding, MinValue);
            AnimationUtility.SetEditorCurve(Supremum, Binding, MaxValue);

            AssetDatabase.AddObjectToAsset(Infimum, ControllerFilename);
            AssetDatabase.AddObjectToAsset(Supremum, ControllerFilename);

            return (Infimum, Supremum);
        }
    }
}
