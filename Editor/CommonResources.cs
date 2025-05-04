using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private const int PixelCount = 4 * 4;

        private static string TensionParameter { get; } = "T";
        private static string QueueParameter { get; } = "Q";

        private static string[] Channels { get; } = { "R", "G", "B" };

        private static Vector3 AnyStateNodePosition { get; } = new(10F, 0F, 0F);
        private static Vector3 EntryNodePosition { get; } = new(370F, 0F, 0F);
        private static Vector3 ExitNodePosition { get; } = new(730F, 0F, 0F);
    }
}
