// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Components;

/// <summary>
/// Health and damage tracking
/// </summary>
public partial class Health : Node {
  // Signals
  [Signal]
  public delegate void HealthChangedEventHandler(float oldHealth, float newHealth);

  [Signal]
  public delegate void HealthDepletedEventHandler();

  // Exports

  // Public Fields

  [Export] public float MaxHealth;
  public float CurrentHealth;

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks
  public override void _Ready() {
    CurrentHealth = MaxHealth;
  }

  // Public Functions

  /// <summary>
  /// Applies damage to the unit
  /// Returns the amount of damage that was actually dealt (in case of resistances, etc)
  /// </summary>
  /// <param name="damage"></param>
  /// <returns></returns>
  public float TakeDamage(float damage) {
    float oldHealth = CurrentHealth;
    CurrentHealth -= damage;
    EmitSignal(nameof(HealthChangedEventHandler), oldHealth, CurrentHealth);

    if (CurrentHealth <= 0) {
      Die();
    }

    return damage;
  }

  // Private Functions
  private void Die() {
    EmitSignal(nameof(HealthDepletedEventHandler));
  }
}
