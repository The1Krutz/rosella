// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

namespace RosellaGame.Types;

public struct Damage {
  public float Amount;
  public DamageType Type;
}

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
