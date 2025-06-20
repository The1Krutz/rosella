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
  public delegate void HealthChangedEventHandler(float newHealth, float change, float percent);

  [Signal]
  public delegate void HealthDepletedEventHandler();

  // Exports

  // Public Fields

  [Export] public float MaxHealth = 100.0f;
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
    // TODO - resistances go here
    CurrentHealth -= damage;
    float percent = CurrentHealth / MaxHealth;
    EmitSignal(SignalName.HealthChanged, CurrentHealth, damage, percent);

    if (CurrentHealth <= 0) {
      Die();
    }

    GD.Print($"took {damage} damage, have {CurrentHealth} ({percent * 100}%) health remaining");

    return damage;
  }

  // Private Functions
  /// <summary>
  /// We've run out of health, now we signal up that this unit is dead. The owner will handle everything else
  /// </summary>
  private void Die() {
    GD.Print("health ran out, now we die");

    EmitSignal(SignalName.HealthDepleted);
  }
}
