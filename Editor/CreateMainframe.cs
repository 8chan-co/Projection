using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static AnimatorStateMachine CreateMainframe(ChildMotion[] PixelActivators)
        {
            BlendTree CentralUnit = new()
            {
                name = "Central Unit",
                hideFlags = HideFlags.HideInHierarchy,
                blendType = BlendTreeType.Direct,
                children = PixelActivators
            };

            AnimatorState Vessel = new()
            {
                name = "Open At Your Own Risk",
                hideFlags = HideFlags.HideInHierarchy,
                motion = CentralUnit
            };

            AnimatorStateMachine Mainframe = new()
            {
                name = "Mainframe",
                hideFlags = HideFlags.HideInHierarchy,
                states = new[]
                {
                    new ChildAnimatorState
                    {
                        state = Vessel,
                        position= Vector3.zero
                    }
                },
                anyStatePosition = AnyStatePosition,
                entryPosition = EntryPosition,
                exitPosition = ExitPosition
            };

            AssetDatabase.AddObjectToAsset(CentralUnit, ControllerFilename);
            AssetDatabase.AddObjectToAsset(Vessel, ControllerFilename);
            AssetDatabase.AddObjectToAsset(Mainframe, ControllerFilename);

            return Mainframe;
        }
    }
}
