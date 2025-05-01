using UnityEditor.Animations;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void CreateWorkspace(AnimatorController Controller) => Controller.AddLayer(new AnimatorControllerLayer
        {
            name = "Workspace",
            stateMachine = CreateMainframe(CreatePixelActivators())
        });
    }
}
