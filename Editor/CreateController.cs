using UnityEditor;
using UnityEditor.Animations;

namespace Ophura
{
    internal static partial class Projection
    {
        private static AnimatorController CreateController()
        {
            AnimatorController Controller = new()
            {
                name = "Projection"
            };

            AssetDatabase.CreateAsset(Controller, ControllerFilename);

            return Controller;
        }
    }
}
