// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;
using RosellaGame.Components;
using RosellaGame.Types;

namespace RosellaGame;

/// <summary>
/// template
/// </summary>
public partial class DamageArea : Area2D {
  // Signals

  // Exports
  [Export] public float DamageAmount;
  [Export] public DamageType DamageType;

  // Public Fields
  public Damage Damage = new();

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    GD.Print("DamageArea ready");

    Damage.Amount = DamageAmount;
    Damage.Type = DamageType;
  }

  // Public Functions
  public void OnAreaEntered(Area2D area) {
    GD.Print($"DamageArea.OnAreaEntered {area.Name}");
    if (area.Owner.HasNode("Health")) {
      Health health = area.Owner.GetNode<Health>("Health");
      health.TakeDamage(Damage);

      // TODO - hit sounds, maybe hitsparks too?
    }
  }

  public void OnAreaExited(Area2D area) {
    GD.Print($"DamageArea.OnAreaExited {area.Name}");
    // TODO - use this eventually for things like persistent damage areas. Ie: if you stand in the fire it should keep
    // burning you every few seconds, etc
  }

  // Private Functions
}
