using Godot;
using System;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class State : Node {
  // Signals

  // Exports
  [Export] public bool CanMove = true;

  // Public Fields
  public CharacterBody2D Character;
  public State NextState;
  public AnimationNodeStateMachinePlayback AnimationStateMachine;

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Public Functions
  public virtual void StateInput(InputEvent @event) {
    // GD.Print("State.StateInput : " + @event);
  }

  public virtual void StatePhysicsProcess(double delta) {
    // GD.Print("State.StateProcess : " + @event);
  }

  public virtual void OnEnter() {
    GD.Print("State.OnEnter");
  }

  public virtual void OnExit() {
    GD.Print("State.OnExit");
  }

  // Private Functions
}
