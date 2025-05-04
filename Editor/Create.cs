using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using AvatarParameterDriverType = VRC.SDKBase.VRC_AvatarParameterDriver.ChangeType;

namespace Ophura
{
    internal static partial class Projection
    {
        [MenuItem("Tools/Ophura/Build Controller")]
        private static void BuildController()
        {
            AnimatorController Controller = new()
            {
                name = "Projection",
                parameters = new AnimatorControllerParameter[]
                {
                    new()
                    {
                        name = "R",
                        type = AnimatorControllerParameterType.Float
                    },
                    new()
                    {
                        name = "G",
                        type = AnimatorControllerParameterType.Float
                    },
                    new()
                    {
                        name = "B",
                        type = AnimatorControllerParameterType.Float
                    },
                    new()
                    {
                        name = TensionParameter,
                        type = AnimatorControllerParameterType.Float,
                        defaultFloat = 1F
                    },
                    new()
                    {
                        name = QueueParameter,
                        type = AnimatorControllerParameterType.Bool
                    }
                }
            };

            AssetDatabase.CreateAsset(Controller, $"Assets/Projection/{Controller.name}.controller");

            AnimationClip None = new()
            {
                name = "None",
                hideFlags = HideFlags.HideInHierarchy
            };

            AssetDatabase.AddObjectToAsset(None, Controller);

            EditorCurveBinding NoneBinding = EditorCurveBinding.FloatCurve(string.Empty, typeof(Object), string.Empty);

            AnimationCurve NoneCurve = AnimationCurve.Linear(0F, 0F, 1F / None.frameRate, 0F);

            AnimationUtility.SetEditorCurve(None, NoneBinding, NoneCurve);

            List<ChildAnimatorState> States = new(PixelCount * (Channels.Length + 1));

            for (int Y = 0; Y < PixelCount; ++Y)
            {
                string Identifier = $"{Y:X4}";

                for (int X = 0; X < Channels.Length; ++X)
                {
                    string Channel = Channels[X];

                    string Name = $"{Identifier}:{Channel}";

                    AnimationClip Animation = new()
                    {
                        name = Name,
                        hideFlags = HideFlags.HideInHierarchy
                    };

                    EditorCurveBinding Binding = EditorCurveBinding.FloatCurve("Body", typeof(MeshRenderer), $"material.{Channel}_{Identifier}");

                    AnimationCurve Curve = AnimationCurve.Linear(0F, 0F, 1F / Animation.frameRate, 1F);

                    AnimationUtility.SetEditorCurve(Animation, Binding, Curve);

                    AssetDatabase.AddObjectToAsset(Animation, Controller);

                    AnimatorState State = new()
                    {
                        name = Name,
                        hideFlags = HideFlags.HideInHierarchy,
                        motion = Animation,
                        speed = 1F,
                        writeDefaultValues = false,
                        speedParameter = TensionParameter,
                        timeParameter = Channel,
                        speedParameterActive = true,
                        timeParameterActive = true
                    };

                    AssetDatabase.AddObjectToAsset(State, Controller);

                    States.Add(new()
                    {
                        state = State,
                        position = new((X * 240F) - 10F, (Y * 50F) + 100F, 0F)
                    });
                }

                AnimatorState Stop = new()
                {
                    name = $"{Identifier}:S",
                    hideFlags = HideFlags.HideInHierarchy,
                    motion = None,
                    writeDefaultValues = false
                };

                AssetDatabase.AddObjectToAsset(Stop, Controller);

                States.Add(new()
                {
                    state = Stop,
                    position = new(710F, (Y * 50F) + 100F, 0F)
                });

                VRCAvatarParameterDriver Driver = Stop.AddStateMachineBehaviour<VRCAvatarParameterDriver>();
                Driver.name = Stop.name;

                Driver.parameters.Add(new()
                {
                    type = AvatarParameterDriverType.Set,
                    name = QueueParameter
                });
            }

            for (int Index = 0; Index < States.Count - 1; ++Index)
            {
                AnimatorState CurrentState = States[Index].state;
                AnimatorState NextState = States[Index + 1].state;

                bool ShouldExit = (Index & 3) is not 3;

                AnimatorStateTransition Transition = new()
                {
                    name = BuildTransitionName(CurrentState.name, NextState.name),
                    hideFlags = HideFlags.HideInHierarchy,
                    destinationState = NextState,
                    conditions = ShouldExit ? null : new[]
                    {
                        new AnimatorCondition
                        {
                            mode = AnimatorConditionMode.If,
                            parameter = QueueParameter
                        }
                    },
                    duration = 0F,
                    interruptionSource = TransitionInterruptionSource.None,
                    orderedInterruption = false,
                    exitTime = ShouldExit ? 1F : 0F,
                    hasExitTime = ShouldExit,
                    hasFixedDuration = true,
                    canTransitionToSelf = false
                };

                AssetDatabase.AddObjectToAsset(Transition, Controller);

                CurrentState.transitions = new[] { Transition };
            }

            AnimatorState First = States[0].state;
            AnimatorState Last = States[^1].state;

            AnimatorStateTransition Roundtrip = new()
            {
                name = BuildTransitionName(Last.name, First.name),
                hideFlags = HideFlags.HideInHierarchy,
                destinationState = First,
                conditions = new[]
                {
                    new AnimatorCondition
                    {
                        mode = AnimatorConditionMode.If,
                        parameter = QueueParameter
                    }
                },
                duration = 0F,
                interruptionSource = TransitionInterruptionSource.None,
                orderedInterruption = false,
                exitTime = 0F,
                hasExitTime = false,
                hasFixedDuration = true,
                canTransitionToSelf = false
            };

            Last.transitions = new[] { Roundtrip };

            AnimatorStateMachine Workspace = new()
            {
                name = "Workspace",
                hideFlags = HideFlags.HideInHierarchy,
                states = States.ToArray(),
                defaultState = Last,
                anyStatePosition = AnyStateNodePosition,
                entryPosition = EntryNodePosition,
                exitPosition = ExitNodePosition
            };

            AssetDatabase.AddObjectToAsset(Workspace, Controller);

            Controller.layers = new[]
            {
                new AnimatorControllerLayer
                {
                    name = "Pilot",
                    stateMachine = Workspace
                }
            };

            AssetDatabase.SaveAssetIfDirty(Controller);
        }
    }
}
