using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private const int PixelCount = 4 * 4;

        private static string ControllerFilename { get; } = "Assets/Projection.controller";
        private static string InfluenceParameter { get; } = "Influence";
        private static string SemaphoreParameter { get; } = "Semaphore";

        private static string[] PropertyNames { get; } = { "R", "G", "B" };

        private static AnimationCurve MinValue { get; } = AnimationCurve.Linear(0F, 0F, 0.01F, 0F);
        private static AnimationCurve MaxValue { get; } = AnimationCurve.Linear(0F, 1F, 0.01F, 1F);

        internal static Vector3 EntryPosition { get; } = new(-280F, -100F, 0F);
        internal static Vector3 AnyStatePosition { get; } = new(20F, -100F, 0F);
        internal static Vector3 ExitPosition { get; } = new(320F, -100F, 0F);
    }
}
