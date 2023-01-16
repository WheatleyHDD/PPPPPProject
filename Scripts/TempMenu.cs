using Godot;
using System;

public class TempMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Resume();
        GetNode<Godot.Button>("%ContinueButton").Disabled = !SaveSystem.HasLevel();

        GetNode<Godot.Button>("%ContinueButton").Connect("pressed", this, "ContinuePressed");
        GetNode<Godot.Button>("%PlayButton").Connect("pressed", this, "PlayPressed");
        GetNode<Godot.Button>("%SettingsButton").Connect("pressed", this, "SettingsPressed");
        GetNode<Godot.Button>("%CreditsButton").Connect("pressed", this, "CreditsPressed");
        GetNode<Godot.Button>("%ExitButton").Connect("pressed", this, "ExitPressed");
        
        GetNode<Godot.Button>("%StartNewGameButton").Connect("pressed", this, "NewGameComfirmed");
        GetNode<Godot.Button>("%CancelNewGameBtn").Connect("pressed", this, "NewGameCanceled");
    }

    public void PlayPressed() => GetNode<AnimationPlayer>("StartNewGamePanel/AnimationPlayer").Play("show");
    public void ContinuePressed() => GoToLevel(SaveSystem.LoadLevel());
    public void SettingsPressed() => GetNode<SettingsScreen>("/root/Settings").ShowThisShit();
    public void CreditsPressed() => GoToLevel("res://Scenes/WIPScreen.tscn");

    public void ExitPressed() {
        GetTree().Quit();
    }

    public void GoToLevel(string lvlName) {
        GetNode<Transition>("/root/Transition").ChangeSceneTo(GD.Load<PackedScene>(lvlName));
        GetNode<MusicPlayer>("/root/MusicPlayer/MenuMusic").Pause();
    }

    public void NewGameComfirmed() => GoToLevel("res://Scenes/Levels/Level1.tscn");
    public void NewGameCanceled() => GetNode<AnimationPlayer>("StartNewGamePanel/AnimationPlayer").Play("hide");

    public override void _Process(float delta)
    {
        if (OS.IsDebugBuild()) Cheats();
        else if (OS.HasFeature("dev")) Cheats();
    }

    void Cheats() {
        if (Input.IsActionPressed("cheat_menu"))
        {
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key1)) GoToLevel("res://Scenes/Levels/Level1.tscn");
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key2)) GoToLevel("res://Scenes/Levels/Level2.tscn");
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key3)) GoToLevel("res://Scenes/Levels/Level3.tscn");
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key4)) GoToLevel("res://Scenes/Levels/Level4.tscn");
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key5)) GoToLevel("res://Scenes/Levels/Level5.tscn");
            if (Input.IsPhysicalKeyPressed((int)KeyList.Key6)) GoToLevel("res://Scenes/Levels/Level6.tscn");
        }
    }
}
