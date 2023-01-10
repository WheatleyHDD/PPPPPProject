using Godot;
using System;

public class TempMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Resume();

        GetNode<Godot.Button>("%PlayButton").Connect("pressed", this, "PlayPressed");
        GetNode<Godot.Button>("%ExitButton").Connect("pressed", this, "ExitPressed");
        GetNode<Godot.Button>("%FullscreenButton").Connect("pressed", this, "FullscreenPressed");
    }

    public void PlayPressed() {
        GetNode<Transition>("/root/Transition").ChangeSceneTo(GD.Load<PackedScene>("res://Scenes/Levels/Level1.tscn"));
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Pause();
    }

    public void ExitPressed() {
        GetTree().Quit();
    }

    public void FullscreenPressed() {
        OS.WindowFullscreen = !OS.WindowFullscreen;
    }
}
