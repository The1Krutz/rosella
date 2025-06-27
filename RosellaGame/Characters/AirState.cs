using Godot;
using System;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class AirState : State {
  // Signals

  // Exports
  [Export] public GroundState GroundState;
  [Export] public float DoubleJumpVelocity = -200.0f;
  [Export] public int MaxDoubleJumps = 1;

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
    if (NextState is GroundState) {
      // only restore double jumps when the player lands
      DoubleJumpsRemaining = MaxDoubleJumps;
    }
  }

  public override void StatePhysicsProcess(double delta) {
    if (Character.IsOnFloor()) {
      NextState = GroundState;
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
    }
  }
}
