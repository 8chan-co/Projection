using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal sealed class MecanimHelpers
    {
        private AnimatorController Controller { get; set; }

        internal MecanimHelpers(AnimatorController Controller) => this.Controller = Controller;

        internal TAsset CreateAsset<TAsset>(string Name) where TAsset : Object, new()
        {
            TAsset Asset = new()
            {
                name = Name,
                hideFlags = HideFlags.HideInHierarchy
            };

            AssetDatabase.AddObjectToAsset(Asset, Controller);

            return Asset;
        }
    }
}
