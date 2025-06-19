// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;
using System;

namespace RosellaGame;

/// <summary>
/// template
/// </summary>
public partial class Player : CharacterBody2D {
  // Signals

  // Exports
  [Export]
  public float Speed = 200.0f;

  [Export]
  public float JumpVelocity = -300.0f;

  [Export]
  public float DoubleJumpVelocity = -200.0f;

  // Public Fields

  // Backing Fields

  // Private Fields
  private int DoubleJumpsRemaining = 1;

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(double delta) {
    Vector2 velocity = Velocity;

    // Add the gravity.
    if (!IsOnFloor()) {
      velocity += GetGravity() * (float)delta;
    } else {
      // Restore double jump when player touches ground
      DoubleJumpsRemaining = 1;
    }

    // Handle Jump.
    if (Input.IsActionJustPressed("jump")) {
      if (IsOnFloor()) {
        velocity.Y = JumpVelocity;
      } else if (DoubleJumpsRemaining > 0) {
        DoubleJumpsRemaining--;
        velocity.Y = DoubleJumpVelocity;
      }
    }

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (direction != Vector2.Zero) {
      velocity.X = direction.X * Speed;
    } else {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }

    Velocity = velocity;
    MoveAndSlide();
  }
  // Public Functions

  // Private Functions
}
