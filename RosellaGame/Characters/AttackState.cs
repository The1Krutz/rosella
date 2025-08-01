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
  [Export] public string Attack1AnimationName = "attack1";
  [Export] public string Attack2AnimationName = "attack2";
  [Export] public GroundState ReturnState;
  [Export] public string ReturnAnimationName = "move";

  // Public Fields
  public Damage Damage = new();

  // Backing Fields

  // Private Fields
  private Godot.Timer ComboTimer;

  // Constructor

  // Lifecycle Hooks
  public override void _Ready() {
    Damage.Amount = DamageAmount;
    Damage.Type = DamageType;

    ComboTimer = GetNode<Godot.Timer>("Timer");
  }

  public override void OnEnter() {
    AnimationStateMachine.Travel("attack1");
  }

  public override void StateInput(InputEvent @event) {
    if (@event.IsActionPressed("attack1")) {
      ComboTimer.Start();
    }
  }

  // Public Functions
  public void OnAnimationTreeAnimationFinished(string name) {
    GD.Print("OnAnimationTreeAnimationFinished" + name);
    if (name == Attack1AnimationName) {
      if (ComboTimer.IsStopped()) {
        NextState = ReturnState;
        AnimationStateMachine.Travel(ReturnAnimationName);
      } else {
        AnimationStateMachine.Travel(Attack2AnimationName);
      }
    }

    if (name == Attack2AnimationName) {
      NextState = ReturnState;
      AnimationStateMachine.Travel(ReturnAnimationName);
    }
  }

  // Private Functions

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
