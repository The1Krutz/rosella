// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

namespace RosellaGame.Types;

/// <summary>
/// SO. Apparently a regular C# struct can't be passed as a parameter through a Godot Signal. There's some fancy engine
/// stuff Godot needs to do, and every Signal param needs to descend from Godot.GodotObject. Which I don't think a
/// struct can descend from anything? Anyway this might just need to be a class instead. Which, whatever it's fine.
/// </summary>
public partial class Damage : Godot.GodotObject {
  public float Amount;
  public DamageType Type;

  public Damage() {
    Amount = 0.0f;
    Type = DamageType.Bludgeoning;
  }

  public Damage(float amount, DamageType type) {
    Amount = amount;
    Type = type;
  }
}

/// <summary>
/// Right now it's every damage type in PF2e, but I don't expect to need all of them. Feel free to prune the list if you
/// decide it's too much
/// </summary>
public enum DamageType {
  // Physical Damage
  Bludgeoning,
  Piercing,
  Slashing,

  // Energy Damage
  Acid,
  Cold,
  Electricity,
  Fire,
  Force,
  Sonic,
  Vitality,
  Void,

  // Unique
  Bleed,
  Mental,
  Poison,
  Precision,
  Spirit
}
