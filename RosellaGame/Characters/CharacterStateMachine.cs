// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class CharacterStateMachine : Node {
  // Signals

  // Exports
  [Export] public State CurrentState;
  [Export] public CharacterBody2D Character;
  [Export] public AnimationTree AnimationTree;

  // Public Fields

  // Backing Fields

  // Private Fields
  private List<State> States = [];

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    GrabAllChildState();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(double delta) {
    if (CurrentState.NextState != null) {
      SwitchStates(CurrentState.NextState);
    }

    CurrentState.StatePhysicsProcess(delta);
  }

  public override void _Input(InputEvent @event) {
    CurrentState.StateInput(@event);
  }

  // Public Functions
  public bool CanMove() {
    return CurrentState.CanMove;
  }

  // Private Functions
  private void GrabAllChildState() {
    foreach (Node child in GetChildren()) {
      if (child is State state) {
        // give the states what they need to work
        state.Character = Character;
        state.AnimationStateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");

        // add to states list
        States.Add(state);
      } else {
        GD.PushWarning("bro don't put things in the state machine that aren't states!" + child.Name);
      }
    }
  }

  private void SwitchStates(State newState) {
    CurrentState.OnExit();
    CurrentState.NextState = null;

    CurrentState = newState;

    CurrentState.OnEnter();
  }
}
