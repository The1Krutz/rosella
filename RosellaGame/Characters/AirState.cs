// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class AirState : State {
  // Signals

  // Exports
  [Export] public LandingState LandingState;
  [Export] public float DoubleJumpVelocity = -200.0f;
  [Export] public int MaxDoubleJumps = 1;
  [Export] public string JumpEndAnimation = "jump_end";
  [Export] public string JumpDoubleAnimation = "jump_double";
  [Export] public string JumpLoopAnimation = "jump_loop";

  // Public Fields

  // Backing Fields

  // Private Fields
  private int DoubleJumpsRemaining;

  // Constructor
  public AirState() {
    DoubleJumpsRemaining = MaxDoubleJumps;
  }

  // Lifecycle Hooks
  public override void OnExit() {
    if (NextState is LandingState) {
      // only restore double jumps when the player lands
      DoubleJumpsRemaining = MaxDoubleJumps;
    }
  }

  public override void StatePhysicsProcess(double delta) {
    if (Character.IsOnFloor()) {
      NextState = LandingState;

      AnimationStateMachine.Travel(JumpEndAnimation);
    }
  }

  public override void StateInput(InputEvent @event) {
    if (@event.IsActionPressed("jump")) {
      GD.Print("AirState.JumpPressed");
      DoubleJump();
    }
  }

  // Public Functions

  // Private Functions
  private void DoubleJump() {
    if (DoubleJumpsRemaining > 0) {
      Vector2 velocity = Character.Velocity;
      velocity.Y = DoubleJumpVelocity;

      Character.Velocity = velocity;

      AnimationStateMachine.Travel(JumpDoubleAnimation);
    }
  }
}
