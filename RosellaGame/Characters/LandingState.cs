// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class LandingState : State {
  // Signals

  // Exports
  [Export] public string LandingAnimationName = "jump_end";
  [Export] public GroundState GroundState;

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Public Functions
  public void OnAnimationTreeAnimationFinished(string name) {
    if (name == LandingAnimationName) {
      NextState = GroundState;
    }
  }

  // Private Functions
}
