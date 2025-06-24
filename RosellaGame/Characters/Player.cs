// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;
using RosellaGame.Components;
using RosellaGame.Types;

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

  [Export] public float DamageAmount;
  [Export] public DamageType DamageType;

  // Public Fields
  public Damage Damage = new();

  // Backing Fields

  // Private Fields
  private int DoubleJumpsRemaining = 1;
  private AnimatedSprite2D Sprite;
  private bool AnimationLocked;
  private Vector2 Direction = Vector2.Zero;
  private bool WasInAir;
  private bool IsDead;
  private int DefaultSpriteHeight = -24;
  private bool IsAttacking;
  private CollisionShape2D HitboxAttack1;

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    Sprite = GetNode<AnimatedSprite2D>("Sprite");
    HitboxAttack1 = GetNode<CollisionShape2D>("Hitbox/CollisionShape2D");

    Damage.Amount = DamageAmount;
    Damage.Type = DamageType;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(double delta) {
    Vector2 velocity = Velocity;

    // ripe for refactor to remove duplication of gravity-related code
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
    Direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (Direction != Vector2.Zero) {
      velocity.X = Direction.X * Speed;
    } else {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }


    if (Input.IsActionJustPressed("attack1")) {
      Attack();
    }

    Velocity = velocity;
    MoveAndSlide();
    UpdateAnimation();
    UpdateFacing();
  }

  // Public Functions
  /// <summary>
  /// Manages the player's response to health changes. The actual numbers are all managed inside the health node. This
  /// is for doing knockbacks, hitsparks, displaying damage numbers, etc
  /// </summary>
  /// <param name="newHealth">current health value</param>
  /// <param name="change">amount the health changed</param>
  /// <param name="percent">percent of total health remaining after this change</param>
  public void OnHealthChanged(float newHealth, Damage change, float percent) {
    if (change.Amount < 0) {
      // took damage
      // TODO - do knockback, hitsparks, etc
      GD.Print($"took {change.Amount} damage");
    } else if (change.Amount > 0) {
      // took healing
      // TODO - healspark? Is that a word?
      GD.Print($"took {change.Amount} healing");
    }

    // TODO - probably need to emit the new health total for ui stuff, but not right now
  }

  /// <summary>
  /// Manages the player's response to running out of health. The Health node manages this and lets us know when we're
  /// dead. This is for playing death animations, etc
  /// </summary>
  public void OnHealthDepleted() {
    GD.Print("unit dead");
    // TODO - death animation
    Sprite.Play("death");

    // slide the sprite down a bit because the death sprites are a little higher
    SetSpriteHeight(-16);

    IsDead = true;
  }

  public void OnSpriteAnimationFinished() {
    switch (Sprite.Animation) {
      case "jump_end":
        AnimationLocked = false;
        SetSpriteHeight(DefaultSpriteHeight);
        break;
      case "jump_start":
        SetSpriteHeight(DefaultSpriteHeight);
        Sprite.Play("falling");
        break;
      case "jump_double":
        Sprite.Play("falling");
        break;
      case "attack1":
        AnimationLocked = false;
        IsAttacking = false;
        HitboxAttack1.Disabled = true;
        break;
    }
  }

  // TODO - generify this
  public void OnHitboxEntered(Area2D area) {
    GD.Print($"Player.OnHitboxEntered {area.Name}, {area.Owner}");

    Health health;
    // The health node can be a child of the area or the area owner, depending. We have to check for both.
    if (area.Owner.HasNode("Health")) {
      health = area.Owner.GetNode<Health>("Health");
    } else if (area.HasNode("Health")) {
      health = area.GetNode<Health>("Health");
    } else {
      return;
    }

    health.TakeDamage(Damage);

    // TODO - hit sounds, maybe hitsparks too?
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
      // flip the sprite and also the attack hitbox
      Sprite.FlipH = true;
      UpdateHitboxPosition(-25);
    } else if (Direction.X > 0) {
      Sprite.FlipH = false;
      UpdateHitboxPosition(25);
    }
  }

  private void Jump(ref Vector2 velocity) {
    velocity.Y = JumpVelocity;
    SetSpriteHeight(-32);
    Sprite.Play("jump_start");
    AnimationLocked = true;
  }

  private void Land() {
    SetSpriteHeight(-32);
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

  private void SetSpriteHeight(int height) {
    Vector2 temp = Sprite.Position;
    temp.Y = height;
    Sprite.Position = temp;
  }

  private void UpdateHitboxPosition(float x) {
    Vector2 hbp = HitboxAttack1.Position;
    hbp.X = x;
    HitboxAttack1.Position = hbp;
  }

  private void Attack() {
    if (IsAttacking || AnimationLocked) {
      return;
    }

    SetSpriteHeight(DefaultSpriteHeight);
    Sprite.Play("attack1");
    AnimationLocked = true;
    IsAttacking = true;

    HitboxAttack1.Disabled = false;
  }
}
