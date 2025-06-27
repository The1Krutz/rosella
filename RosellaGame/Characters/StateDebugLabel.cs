// SPDX-FileCopyrightText: The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using Godot;

namespace RosellaGame.Characters;

/// <summary>
/// template
/// </summary>
public partial class StateDebugLabel : Label {
  // Signals

  // Exports
  [Export] private CharacterStateMachine StateMachine;

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
    Text = "State: " + StateMachine.CurrentState.Name;
  }

  // Public Functions

  // Private Functions
}
