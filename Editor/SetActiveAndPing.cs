using UnityEditor;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void SetActiveAndPing(int Identifier)
        {
            Selection.activeInstanceID = Identifier;

            EditorGUIUtility.PingObject(Identifier);
        }
    }
}
