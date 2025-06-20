using Godot;
using RosellaGame.Components;

namespace RosellaGame;

/// <summary>
/// template
/// </summary>
public partial class DamageArea : Area2D {
  // Signals

  // Exports
  [Export] public float Damage = 10.0f;

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    GD.Print("DamageArea ready");
  }

  // Public Functions
  public void OnAreaEntered(Area2D area) {
    GD.Print($"DamageArea.OnAreaEntered {area.Name}");
    if (area.Owner.HasNode("Health")) {
      Health health = area.Owner.GetNode<Health>("Health");
      health.TakeDamage(Damage);
    }
  }

  public void OnAreaExited(Area2D area) {
    GD.Print($"DamageArea.OnAreaExited {area.Name}");
  }

  // Private Functions
}
