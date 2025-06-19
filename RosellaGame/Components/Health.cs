using Godot;

namespace RosellaGame;


/// <summary>
/// template
/// </summary>
public class Health {
  // Signals

  // Exports

  // Public Fields
  public float MaxHealth = 0.0f;
  public float CurrentHealth = 0.0f;

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Public Functions

  // Applies damage to the unit
  // returns the amount of damage that was actually dealt (in case of resistances, etc)
  public float TakeDamage(float damage) {
    CurrentHealth -= damage;
    if (CurrentHealth <= 0) {
      Die();
    }

    return damage;
  }

  public void Die() {
    GD.Print("death!");
  }

  // Private Functions
}
