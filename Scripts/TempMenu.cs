using Godot;
using System;

public class TempMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Resume();

        GetNode<Godot.Button>("%ContinueButton").Connect("pressed", this, "ContinuePressed");
        GetNode<Godot.Button>("%PlayButton").Connect("pressed", this, "PlayPressed");
        GetNode<Godot.Button>("%SettingsButton").Connect("pressed", this, "SettingsPressed");
        GetNode<Godot.Button>("%ExitButton").Connect("pressed", this, "ExitPressed");
        
        GetNode<Godot.Button>("%StartNewGameButton").Connect("pressed", this, "NewGameComfirmed");
        GetNode<Godot.Button>("%CancelNewGameBtn").Connect("pressed", this, "NewGameCanceled");
    }

    public void PlayPressed() => GetNode<AnimationPlayer>("StartNewGamePanel/AnimationPlayer").Play("show");
    public void ContinuePressed() => GoToLevel(SaveSystem.LoadLevel());
    public void SettingsPressed() => GetNode<SettingsScreen>("/root/Settings").ShowThisShit();

    public void ExitPressed() {
        GetTree().Quit();
    }

    public void GoToLevel(string lvlName) {
        GetNode<Transition>("/root/Transition").ChangeSceneTo(GD.Load<PackedScene>(lvlName));
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Pause();
    }

    public void NewGameComfirmed() => GoToLevel("res://Scenes/Levels/Level1.tscn");
    public void NewGameCanceled() => GetNode<AnimationPlayer>("StartNewGamePanel/AnimationPlayer").Play("hide");
}
