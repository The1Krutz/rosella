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

  // Public Fields

  // Backing Fields

  // Private Fields
  private Sprite2D Sprite;
  private AnimationTree AnimTree;
  private Vector2 Direction = Vector2.Zero;
  private bool IsDead;
  private CharacterStateMachine StateMachine;
  private Area2D Hitbox;
  private Vector2 HitboxOriginalLocation;
  private Vector2 HitboxInvertedLocation;

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    // setup references, @onready, etc
    Sprite = GetNode<Sprite2D>("Sprite2D");
    AnimTree = GetNode<AnimationTree>("AnimationTree");
    StateMachine = GetNode<CharacterStateMachine>("CharacterStateMachine");
    Hitbox = GetNode<Area2D>("Hitbox");
    HitboxOriginalLocation = Hitbox.Position;
    HitboxInvertedLocation = new Vector2(Hitbox.Position.X * -1, Hitbox.Position.Y);

    // set scene defaults
    AnimTree.Active = true;
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
    }

    // Get the input direction and handle the movement/deceleration.
    Direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (Direction != Vector2.Zero && StateMachine.CanMove()) {
      velocity.X = Direction.X * Speed;
    } else {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }


    Velocity = velocity;
    MoveAndSlide();
    UpdateAnimationParameters();
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
    if (change.Amount > 0) {
      // took damage
      // TODO - do knockback, hitsparks, etc
      GD.Print($"took {change.Amount} damage");
    } else if (change.Amount < 0) {
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
    // Sprite.Play("death");

    // slide the sprite down a bit because the death sprites are a little higher
    // SetSpriteHeight(-16);

    IsDead = true;
  }

  // TODO - generify this
  public void OnHitboxEntered(Area2D area) {
    GD.Print($"Player.OnHitboxEntered {area.Name}, {area.Owner}");

    Health health;
    // The health node can be a child of the area or the area owner, depending. We have to check for both.
    // TODO - fix the weirdness here lol
    if (area.Owner.HasNode("Health")) {
      health = area.Owner.GetNode<Health>("Health");
    } else if (area.HasNode("Health")) {
      health = area.GetNode<Health>("Health");
    } else {
      return;
    }

    // fix this to come from the actual weapon
    health.TakeDamage(new Damage() {
      Amount = 40.0f,
      Type = DamageType.Piercing
    });

    // TODO - hit sounds, maybe hitsparks too?
  }

  // Private Functions
  private void UpdateAnimationParameters() {
    AnimTree.Set("parameters/move/blend_position", Direction.X);
  }

  private void UpdateFacing() {
    if (Direction.X < 0) {
      // make evreything face left
      Sprite.FlipH = true;
      Hitbox.Position = HitboxInvertedLocation;
    } else if (Direction.X > 0) {
      // make everything face right
      Sprite.FlipH = false;
      Hitbox.Position = HitboxOriginalLocation;
    }
  }
}
