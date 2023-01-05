using Godot;
using System;

public class PauseScreen : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<BaseButton>("panel/Buttons/Resume").Connect("pressed", this, "ButtonsResume");
        GetNode<BaseButton>("panel/Buttons/Restart").Connect("pressed", this, "ButtonsRestart");
        GetNode<BaseButton>("panel/Buttons/Exit").Connect("pressed", this, "ButtonsExit");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Visible)
            GetNode<Control>("panel").RectPosition = new Vector2(
                Mathf.Lerp(GetNode<Control>("panel").RectPosition.x, 0, 0.5F*60*delta),
                GetNode<Control>("panel").RectPosition.y);
        else GetNode<Control>("panel").RectPosition = new Vector2(
                Mathf.Lerp(GetNode<Control>("panel").RectPosition.x, 352, 0.5F*60*delta),
                GetNode<Control>("panel").RectPosition.y);
    }

    public void ButtonsResume() {
        GetTree().Paused = false;
        GetNode<CanvasLayer>("/root/Pause").Visible = false;
    }

    public void ButtonsRestart() {
        GetTree().Paused = false;
        GetNode<CanvasLayer>("/root/Pause").Visible = false;
        GetTree().ReloadCurrentScene();
        GetNode<FastDialogue>("/root/FastDialogue").StopAll();
    }

    public void ButtonsExit() {
        GetTree().ChangeScene("res://Scenes/TempMenu.tscn");
        GetTree().Paused = false;
        GetNode<CanvasLayer>("/root/Pause").Visible = false;
    }
}
