using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void CreateSynchronizationLogic(AnimatorController Controller)
        {
            AnimatorState Initial = new()
            {
                name = "State:0000",
                hideFlags = HideFlags.HideInHierarchy,
                motion = CreateNoneClip(),
                writeDefaultValues = false
            };

            AnimatorStateMachine IterativeMutation = new()
            {
                name = "Iterative Mutation",
                hideFlags = HideFlags.HideInHierarchy,
                anyStatePosition = AnyStatePosition,
                entryPosition = EntryPosition,
                exitPosition = ExitPosition,
                states = CreateSynchronizationStates(Initial)
            };

            AssetDatabase.AddObjectToAsset(Initial, ControllerFilename);
            AssetDatabase.AddObjectToAsset(IterativeMutation, ControllerFilename);

            Controller.AddLayer(new AnimatorControllerLayer
            {
                name = "Synchronization",
                stateMachine = IterativeMutation,
                defaultWeight = 1F
            });
        }
    }
}
