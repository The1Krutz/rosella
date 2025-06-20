using Godot;
using System;

namespace RosellaGame;

/// <summary>
/// template
/// </summary>
public partial class Destructible : Area2D {
  // Signals

  // Exports
  [Export] public Sprite2D SpriteIntact;

  [Export] public Sprite2D SpriteDamaged;

  [Export] public Sprite2D SpriteDestroyed;

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Public Functions
  /// <summary>
  /// Manages the player's response to health changes. The actual numbers are all managed inside the health node. This
  /// is for doing knockbacks, hitsparks, displaying damage numbers, etc
  /// </summary>
  /// <param name="newHealth">current health value</param>
  /// <param name="change">amount the health changed</param>
  /// <param name="percent">percent of total health remaining after this change</param>
  public void OnHealthChanged(float newHealth, float change, float percent) {
    if (change < 0) {
      // took damage
      // TODO - do knockback, hitsparks, etc
      GD.Print($"{Name} took {change} damage");
    }

    // change sprite when barrel gets low on health
    if (percent < 0.5) {
      SwapToDamagedSprite();
    }
  }

  /// <summary>
  /// Manages the player's response to running out of health. The Health node manages this and lets us know when we're
  /// dead. This is for playing death animations, etc
  /// </summary>
  public void OnHealthDepleted() {
    GD.Print("barrel destroyed");
    SwapToDestroyedSprite();

    // stop processing this node
    ProcessMode = ProcessModeEnum.Disabled;
  }

  // Private Functions
  private void SwapToDamagedSprite() {
    SpriteIntact.Visible = false;
    SpriteDamaged.Visible = true;
    SpriteDestroyed.Visible = false;
  }

  private void SwapToDestroyedSprite() {
    SpriteIntact.Visible = false;
    SpriteDamaged.Visible = false;
    SpriteDestroyed.Visible = true;
  }
}
