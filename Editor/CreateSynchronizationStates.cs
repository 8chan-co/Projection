using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static ChildAnimatorState[] CreateSynchronizationStates(AnimatorState Initial)
        {
            List<ChildAnimatorState> States = new(PixelCount)
            {
                new()
                {
                    state = Initial,
                    position = new(0F, 0F, 0F)
                }
            };

            AddParameterDriver(Initial, 0);

            AnimatorState Former = Initial;

            for (int Index = 1; Index < PixelCount; ++Index)
            {
                AnimatorState Current = new()
                {
                    name = $"State:{Index:X4}",
                    hideFlags = HideFlags.HideInHierarchy,
                    motion = Initial.motion,
                    writeDefaultValues = false
                };

                AddParameterDriver(Current, Index);

                AnimatorStateTransition Transition = CreateTransition(Former.name, Current);
                Former.AddTransition(Transition);
                EditorUtility.SetDirty(Former);

                AssetDatabase.AddObjectToAsset(Transition, ControllerFilename);
                AssetDatabase.AddObjectToAsset(Current, ControllerFilename);

                States.Add(new()
                {
                    state = Current,
                    position = GetSnakeOrder(Index)
                });

                Former = Current;
            }

            AnimatorStateTransition Exit = CreateTransition(Former.name);
            Former.AddTransition(Exit);
            EditorUtility.SetDirty(Former);

            AssetDatabase.AddObjectToAsset(Exit, ControllerFilename);

            return States.ToArray();
        }
    }
}
