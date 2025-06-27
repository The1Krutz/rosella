// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class GroundState : State {
  // Signals

  // Exports
  [Export] public float JumpVelocity = -300.0f;
  [Export] public AirState AirState;
  [Export] public string JumpAnimationName = "jump_start";

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Public Functions
  public override void StateInput(InputEvent @event) {
    if (@event.IsActionPressed("jump")) {
      Jump();
    }
  }

  public override void StatePhysicsProcess(double delta) {
    if (!Character.IsOnFloor()) {
      NextState = AirState;
    }
  }

  // Private Functions
  private void Jump() {
    Vector2 velocity = Character.Velocity;
    velocity.Y = JumpVelocity;

    Character.Velocity = velocity;
    NextState = AirState;

    AnimationStateMachine.Travel(JumpAnimationName);
  }
}
