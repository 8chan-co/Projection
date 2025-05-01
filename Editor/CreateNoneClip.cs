using UnityEditor;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static AnimationClip CreateNoneClip()
        {
            AnimationClip None = new()
            {
                name = "None",
                hideFlags = HideFlags.HideInHierarchy
            };

            EditorCurveBinding Binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(MeshRenderer), string.Empty);

            AnimationUtility.SetEditorCurve(None, Binding, AnimationCurve.Linear(0F, 0F, 0.01F, 0F));

            AssetDatabase.AddObjectToAsset(None, ControllerFilename);

            return None;
        }
    }
}
