using UnityEditor;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using AvatarParameterDriverType = VRC.SDKBase.VRC_AvatarParameterDriver.ChangeType;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void AddParameterDriver(AnimatorState State, int Index)
        {
            VRCAvatarParameterDriver Driver = State.AddStateMachineBehaviour<VRCAvatarParameterDriver>();

            foreach (string Name in PropertyNames)
            {
                Driver.parameters.Add(new()
                {
                    type = AvatarParameterDriverType.Copy,
                    name = $"{Name}_{Index:X4}",
                    source = Name,
                    valueMax = 0F,
                    chance = 0F
                });
            }

            Driver.parameters.Add(new()
            {
                type = AvatarParameterDriverType.Set,
                name = SemaphoreParameter,
                value = 0F,
                valueMax = 0F,
                chance = 0F
            });

            AssetDatabase.AddObjectToAsset(Driver, ControllerFilename);
        }
    }
}
