using Godot;
using System;

public class PauseScreen : CanvasLayer
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<BaseButton>("%PauseResume").Connect("pressed", this, "ButtonsResume");
        GetNode<BaseButton>("%PauseRestart").Connect("pressed", this, "ButtonsRestart");
        GetNode<BaseButton>("%PauseExit").Connect("pressed", this, "ButtonsExit");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Visible) {
            GetNode<TextureRect>("Panel").RectPosition = new Vector2(GetNode<TextureRect>("Panel").RectPosition.x,
                Mathf.Lerp(GetNode<TextureRect>("Panel").RectPosition.y, 136, 0.5F*60*delta));
            GetNode<ColorRect>("bg").SelfModulate = new Color(1, 1, 1,
                Mathf.Lerp(GetNode<ColorRect>("bg").SelfModulate.a, 1, 0.5F*60*delta));
        } else {
            GetNode<TextureRect>("Panel").RectPosition = new Vector2(GetNode<TextureRect>("Panel").RectPosition.x,
                Mathf.Lerp(GetNode<TextureRect>("Panel").RectPosition.y, 832, 0.5F*60*delta));
            GetNode<ColorRect>("bg").SelfModulate = new Color(1, 1, 1,
                Mathf.Lerp(GetNode<ColorRect>("bg").SelfModulate.a, 0, 0.5F*60*delta));
        }
    }

    public void ButtonsResume() {
        GetTree().Paused = false;
        Visible = false;
    }

    public void ButtonsRestart() {
        GetTree().Paused = false;
        Visible = false;
        GetTree().ReloadCurrentScene();
        GetNode<FastDialogue>("/root/FastDialogue").StopAll();
    }

    public void ButtonsExit() {
        GetTree().ChangeScene("res://Scenes/TempMenu.tscn");
        GetTree().Paused = false;
        Visible = false;
    }
}
