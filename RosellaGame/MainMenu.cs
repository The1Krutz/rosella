using Godot;

namespace RosellaGame;

/// <summary>
/// template
/// </summary>
public partial class MainMenu : Control {
  // Signals

  // Exports

  // Public Fields

  // Backing Fields

  // Private Fields

  // Constructor

  // Lifecycle Hooks

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
  }

  // Public Functions
  public void OnStartPressed() {
    GD.Print("OnStartPressed");
    GetTree().ChangeSceneToFile(""); // TODO
  }

  public void OnOptionsPressed() {
    GD.Print("OnOptionsPressed");
  }

  public void OnExitPressed() {
    GD.Print("OnExitPressed");
    GetTree().Quit();
  }

  // Private Functions
}
