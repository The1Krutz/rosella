// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class Player : CharacterBody2D {
  // Signals

  // Exports
  [Export] public float Speed = 200.0f;

  [Export] public float JumpVelocity = -300.0f;

  [Export] public float DoubleJumpVelocity = -200.0f;

  // Public Fields

  // Backing Fields

  // Private Fields
  private int DoubleJumpsRemaining = 1;
  private AnimatedSprite2D Sprite;
  private bool AnimationLocked;
  private Vector2 Direction = Vector2.Zero;
  private bool WasInAir;
  private bool IsDead;

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    Sprite = GetNode<AnimatedSprite2D>("Sprite");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(double delta) {
    Vector2 velocity = Velocity;

    if (IsDead) {
      // apply gravity so we don't die in midair
      if (!IsOnFloor()) {
        velocity += GetGravity() * (float)delta;
      }

      Velocity = velocity;
      MoveAndSlide();
      return;
    }

    // Add the gravity.
    if (!IsOnFloor()) {
      velocity += GetGravity() * (float)delta;
      WasInAir = true;
    } else {
      // Restore double jump when player touches ground
      DoubleJumpsRemaining = 1;
      if (WasInAir) {
        Land();
      }
    }

    // Handle Jump.
    if (Input.IsActionJustPressed("jump")) {
      if (IsOnFloor()) {
        Jump(ref velocity);
      } else if (DoubleJumpsRemaining > 0) {
        DoubleJump(ref velocity);
      }
    }

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (Direction != Vector2.Zero) {
      velocity.X = Direction.X * Speed;
    } else {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }

    Velocity = velocity;
    MoveAndSlide();
    UpdateAnimation();
    UpdateFacing();
  }

  // Public Functions
  public void OnHealthChanged(float oldHealth, float currentHealth) {
    float change = currentHealth - oldHealth;

    if (change < 0) {
      // took damage
      // TODO - do knockback, hitsparks, etc
      GD.Print($"took {change} damage");
    } else if (change > 0) {
      // took healing
      // TODO - healspark? Is that a word?
      GD.Print($"took {change} healing");
    }
  }

  public void OnHealthDepleted() {
    GD.Print("unit dead");
    // TODO - death animation
    Sprite.Play("death");

    Vector2 temp = Sprite.Position;
    temp.Y = -48;
    Sprite.Position = temp;


    // Sprite.Transform.Y = -48;
    IsDead = true;
  }

  public void OnSpriteAnimationFinished() {
    switch (Sprite.Animation) {
      case "jump_end":
        AnimationLocked = false;
        break;
      case "jump_start":
      case "jump_double":
        Sprite.Play("falling");
        break;
    }
  }

  // Private Functions
  private void UpdateAnimation() {
    if (AnimationLocked) {
      return;
    }

    if (Direction.X != 0) {
      Sprite.Play("run");
    } else {
      Sprite.Play("idle");
    }
  }

  private void UpdateFacing() {
    if (Direction.X < 0) {
      Sprite.FlipH = true;
    } else if (Direction.X > 0) {
      Sprite.FlipH = false;
    }
  }

  private void Jump(ref Vector2 velocity) {
    velocity.Y = JumpVelocity;
    Sprite.Play("jump_start");
    AnimationLocked = true;
  }

  private void Land() {
    Sprite.Play("jump_end");
    AnimationLocked = true;
    WasInAir = false;
  }

  private void DoubleJump(ref Vector2 velocity) {
    DoubleJumpsRemaining--;
    velocity.Y = DoubleJumpVelocity;
    Sprite.Play("jump_double");
    AnimationLocked = true;
  }
}
