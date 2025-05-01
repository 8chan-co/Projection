using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;

namespace Ophura
{
    internal static partial class Projection
    {
        private static MethodInfo BuildTransitionNameMethod { get; } = AccessTools.DeclaredMethod(typeof(AnimatorTransitionBase), nameof(BuildTransitionName));

        private static object[] BuildTransitionNameArguments { get; } = new object[2];

        private static string BuildTransitionName(string Source, string Destination)
        {
            BuildTransitionNameArguments[0] = Source;
            BuildTransitionNameArguments[1] = Destination;

            return Unsafe.As<string>(BuildTransitionNameMethod.Invoke(null, BuildTransitionNameArguments));
        }
    }
}
