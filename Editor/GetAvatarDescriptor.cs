using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Avatars.Components;

namespace Ophura
{
    internal static partial class Projection
    {
        private static VRCAvatarDescriptor GetAvatarDescriptor()
        {
            foreach (GameObject GameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (GameObject.TryGetComponent(out VRCAvatarDescriptor Descriptor))
                {
                    return Descriptor;
                }
            }

            return null;
        }
    }
}
