// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;
using RosellaGame.Types;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class AttackState : State {
  // Signals

  // Exports
  [Export] public float DamageAmount;
  [Export] public DamageType DamageType;
  [Export] public CollisionShape2D HitboxAttack1;


  // Public Fields
  public Damage Damage = new();

  // Backing Fields

  // Private Fields
  private bool IsAttacking;

  // Constructor

  // Lifecycle Hooks
  public override void _Ready() {
    
    
    Damage.Amount = DamageAmount;
    Damage.Type = DamageType;
  }

  public override void StateInput(InputEvent @event) {
    if (@event.IsActionPressed("attack1")) {
      Attack();
    }
  }

  // Public Functions

  // Private Functions

  private void Attack() {
    if (IsAttacking) {
      return;
    }

    // Sprite.Play("attack1");
    IsAttacking = true;

    HitboxAttack1.Disabled = false;
  }
  
  // TODO - make this work
  private void UpdateFacing() {
    // if (Character.Direction.X < 0) {
    //   // flip the attack hitbox
    //   UpdateHitboxPosition(-25);
    // } else if (Direction.X > 0) {
    //   UpdateHitboxPosition(25);
    // }
  }

  private void UpdateHitboxPosition(float x) {
    Vector2 hbp = HitboxAttack1.Position;
    hbp.X = x;
    HitboxAttack1.Position = hbp;
  }
}
