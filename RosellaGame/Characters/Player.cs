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

  // Public Fields

  // Backing Fields

  // Private Fields

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
    }

    // Handle Jump.
    if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
      velocity.Y = JumpVelocity;
    }

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
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
