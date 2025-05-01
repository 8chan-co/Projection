using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static AnimatorStateTransition CreateTransition(string Source, AnimatorState Destination = null)
        {
            AnimatorStateTransition Transition = new()
            {
                name = BuildTransitionName(Source, Destination == null ? "Exit" : Destination.name),
                hideFlags = HideFlags.HideInHierarchy,
                isExit = Destination == null,
                destinationState = Destination,
                conditions = new[]
                {
                    new AnimatorCondition
                    {
                        mode = AnimatorConditionMode.If,
                        parameter = SemaphoreParameter,
                        threshold = 0F
                    }
                },
                duration = 0F,
                interruptionSource = TransitionInterruptionSource.None,
                orderedInterruption = false,
                exitTime = 0F,
                hasExitTime = true,
                hasFixedDuration = true
            };

            return Transition;
        }
    }
}
