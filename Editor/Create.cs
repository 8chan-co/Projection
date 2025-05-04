using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        [MenuItem("Tools/Ophura/Create")]
        private static void Create()
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

            AssetDatabase.CreateAsset(Controller, $"Assets/{Controller.name}.controller");

            List<ChildAnimatorStateMachine> StateMachines = new(PixelCount);
            List<ChildAnimatorState> States = new(Channels.Length);

            for (int PixelIndex = 0; PixelIndex < PixelCount; ++PixelIndex)
            {
                string Identifier = $"{PixelIndex:X4}";

                for (int ChannelIndex = 0; ChannelIndex < Channels.Length; ++ChannelIndex)
                {
                    string Channel = Channels[ChannelIndex];

                    AnimationClip MinValue = new()
                    {
                        name = "MinValue",
                        hideFlags = HideFlags.HideInHierarchy
                    };

                    AnimationClip MaxValue = new()
                    {
                        name = "MaxValue",
                        hideFlags = HideFlags.HideInHierarchy
                    };

                    EditorCurveBinding Binding = EditorCurveBinding.FloatCurve("Body", typeof(MeshRenderer), $"material.{Channel}_{Identifier}");

                    AnimationUtility.SetEditorCurve(MinValue, Binding, AnimationCurve.Linear(0F, 0F, 0.01F, 0F));
                    AnimationUtility.SetEditorCurve(MaxValue, Binding, AnimationCurve.Linear(0F, 1F, 0.01F, 1F));

                    AssetDatabase.AddObjectToAsset(MinValue, Controller);
                    AssetDatabase.AddObjectToAsset(MaxValue, Controller);

                    BlendTree BlendTree = new()
                    {
                        name = Channel,
                        hideFlags = HideFlags.HideInHierarchy,
                        blendParameter = Channel,
                        children = new[]
                        {
                            new ChildMotion
                            {
                                motion = MinValue,
                                timeScale = 1F
                            },
                            new ChildMotion
                            {
                                motion = MaxValue,
                                timeScale = 1F
                            }
                        }
                    };

                    AssetDatabase.AddObjectToAsset(BlendTree, Controller);

                    AnimatorState State = new()
                    {
                        name = Channel,
                        hideFlags = HideFlags.HideInHierarchy,
                        motion = BlendTree,
                        writeDefaultValues = false
                    };

                    AssetDatabase.AddObjectToAsset(State, Controller);

                    States.Add(new()
                    {
                        state = State,
                        position = new(ChannelIndex * 360 - 10, 100F, 0F)
                    });
                }

                AnimatorStateMachine StateMachine = new()
                {
                    name = Identifier,
                    hideFlags = HideFlags.HideInHierarchy,
                    states = States.ToArray(),
                    anyStatePosition = AnyStateNodePosition,
                    entryPosition = EntryNodePosition,
                    exitPosition = ExitNodePosition,
                    parentStateMachinePosition = ParentStateMachinePosition
                };

                States.Clear();

                StateMachines.Add(new()
                {
                    stateMachine = StateMachine,
                    position = GetSnakeOrder(PixelIndex)
                });

                AssetDatabase.AddObjectToAsset(StateMachine, Controller);
            }

            AnimatorStateMachine Workspace = new()
            {
                name = "Workspace",
                hideFlags = HideFlags.HideInHierarchy,
                stateMachines = StateMachines.ToArray(),
                anyStatePosition = AnyStateNodePosition,
                entryPosition = EntryNodePosition,
                exitPosition = ExitNodePosition
            };

            for (int StateMachineIndex = 0; StateMachineIndex < StateMachines.Count - 1; ++StateMachineIndex)
            {
                AnimatorStateMachine CurrentStateMachine = StateMachines[StateMachineIndex].stateMachine;
                AnimatorStateMachine NextStateMachine = StateMachines[StateMachineIndex + 1].stateMachine;

                for (int StateIndex = 0; StateIndex < CurrentStateMachine.states.Length - 1; ++StateIndex)
                {
                    AnimatorState CurrentState = CurrentStateMachine.states[StateIndex].state;
                    AnimatorState NextState = CurrentStateMachine.states[StateIndex + 1].state;

                    AnimatorStateTransition StateTransition = new()
                    {
                        name = BuildTransitionName(CurrentState.name, NextState.name),
                        hideFlags = HideFlags.HideInHierarchy,
                        destinationState = NextState,
                        duration = 0F,
                        interruptionSource = TransitionInterruptionSource.None,
                        orderedInterruption = false,
                        exitTime = 1F,
                        hasExitTime = true,
                        hasFixedDuration = true,
                        canTransitionToSelf = false
                    };

                    AssetDatabase.AddObjectToAsset(StateTransition, Controller);

                    CurrentState.transitions = new[] { StateTransition };
                }

                AnimatorState LastState = CurrentStateMachine.states[^1].state;

                AnimatorStateTransition StateExitTransition = new()
                {
                    name = BuildTransitionName(LastState.name, "Exit"),
                    hideFlags = HideFlags.HideInHierarchy,
                    isExit = true,
                    duration = 0F,
                    interruptionSource = TransitionInterruptionSource.None,
                    orderedInterruption = false,
                    exitTime = 1F,
                    hasExitTime = true,
                    hasFixedDuration = true,
                    canTransitionToSelf = false
                };

                AssetDatabase.AddObjectToAsset(StateExitTransition, Controller);

                LastState.transitions = new[] { StateExitTransition };

                AnimatorTransition StateMachineTransition = new()
                {
                    name = BuildTransitionName(CurrentStateMachine.name, NextStateMachine.name),
                    hideFlags = HideFlags.HideInHierarchy,
                    destinationStateMachine = NextStateMachine,
                    conditions = new[]
                    {
                        new AnimatorCondition
                        {
                            mode = AnimatorConditionMode.If,
                            parameter = QueueParameter
                        }
                    }
                };

                AssetDatabase.AddObjectToAsset(StateMachineTransition, Controller);

                Workspace.SetStateMachineTransitions(CurrentStateMachine, new[] { StateMachineTransition });
            }

            AnimatorStateMachine LastStateMachine = StateMachines[^1].stateMachine;

            for (int StateIndex = 0; StateIndex < LastStateMachine.states.Length - 1; ++StateIndex)
            {
                AnimatorState CurrentState = LastStateMachine.states[StateIndex].state;
                AnimatorState NextState = LastStateMachine.states[StateIndex + 1].state;

                AnimatorStateTransition StateTransition = new()
                {
                    name = BuildTransitionName(CurrentState.name, NextState.name),
                    hideFlags = HideFlags.HideInHierarchy,
                    destinationState = NextState,
                    duration = 0F,
                    interruptionSource = TransitionInterruptionSource.None,
                    orderedInterruption = false,
                    exitTime = 1F,
                    hasExitTime = true,
                    hasFixedDuration = true,
                    canTransitionToSelf = false
                };

                AssetDatabase.AddObjectToAsset(StateTransition, Controller);

                CurrentState.transitions = new[] { StateTransition };
            }

            AnimatorState LastStateMachineLastState = LastStateMachine.states[^1].state;

            AnimatorStateTransition LastStateExitTransition = new()
            {
                name = BuildTransitionName(LastStateMachineLastState.name, "Exit"),
                hideFlags = HideFlags.HideInHierarchy,
                isExit = true,
                duration = 0F,
                interruptionSource = TransitionInterruptionSource.None,
                orderedInterruption = false,
                exitTime = 1F,
                hasExitTime = true,
                hasFixedDuration = true,
                canTransitionToSelf = false
            };

            AssetDatabase.AddObjectToAsset(LastStateExitTransition, Controller);

            LastStateMachineLastState.transitions = new[] { LastStateExitTransition };

            AnimatorTransition StateMachineExitTransition = new()
            {
                name = BuildTransitionName(LastStateMachine.name, "Exit"),
                hideFlags = HideFlags.HideInHierarchy,
                isExit = true
            };

            AssetDatabase.AddObjectToAsset(StateMachineExitTransition, Controller);

            Workspace.SetStateMachineTransitions(LastStateMachine, new[] { StateMachineExitTransition });

            Controller.layers = new[]
            {
                new AnimatorControllerLayer
                {
                    name = "Pilot",
                    stateMachine = Workspace
                }
            };

            AssetDatabase.AddObjectToAsset(Workspace, Controller);

            AssetDatabase.SaveAssetIfDirty(Controller);

            SetActiveAndPing(Controller.GetInstanceID());
        }
    }
}
