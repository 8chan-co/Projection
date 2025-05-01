using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using AvatarExpressionParameter = VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionParameters.Parameter;
using AvatarExpressionParameterType = VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionParameters.ValueType;

namespace Ophura
{
    internal static partial class Projection
    {
        private static void CreateSynchronizationParameters(AnimatorController Controller, VRCAvatarDescriptor Descriptor)
        {
            List<AvatarExpressionParameter> Parameters = new(PropertyNames.Length + 1);

            foreach (string Name in PropertyNames)
            {
                Controller.AddParameter(new()
                {
                    name = Name,
                    type = AnimatorControllerParameterType.Float
                });

                Parameters.Add(new()
                {
                    name = Name,
                    valueType = AvatarExpressionParameterType.Float,
                    saved = false
                });
            }

            Controller.AddParameter(new()
            {
                name = SemaphoreParameter,
                type = AnimatorControllerParameterType.Bool
            });

            Parameters.Add(new()
            {
                name = SemaphoreParameter,
                valueType = AvatarExpressionParameterType.Bool,
                saved = false
            });

            Descriptor.expressionParameters.parameters = Parameters.ToArray();

            EditorUtility.SetDirty(Descriptor.expressionParameters);
        }
    }
}
