using Godot;
using System;

public class SettingsScreen : CanvasLayer
{
    public override void _Ready()
    {
        SaveSystem.LoadSettings();
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("sound"), GD.Linear2Db(SaveSystem.Settings.soundVolume));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), GD.Linear2Db(SaveSystem.Settings.musicVolume));
        GetNode<Slider>("%SoundVolume").Value = SaveSystem.Settings.soundVolume;
        GetNode<Slider>("%MusicVolume").Value = SaveSystem.Settings.musicVolume;

        GetNode<Slider>("%SoundVolume").Connect("drag_ended", this, "SoundChanged");
        GetNode<Slider>("%MusicVolume").Connect("drag_ended", this, "MusicChanged");

        GetNode<BaseButton>("%SettingsFullscreen").Connect("pressed", this, "FullscreenChange");
        GetNode<BaseButton>("%SettingsClose").Connect("pressed", this, "OnClose");
    }

    public void ShowThisShit() {
        Show();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("show");
    }

    public void HideThisShit() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("hide");
    }

    public void OnClose() => HideThisShit();
    public void FullscreenChange() {
        OS.WindowFullscreen = !OS.WindowFullscreen;
        SaveSystem.Settings.SetFullscreen(OS.WindowFullscreen);
        SaveSystem.SaveSettings();
    }

    public void SoundChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("sound"), GD.Linear2Db(value));
        SaveSystem.Settings.SetSoundVolume(value);
        SaveSystem.SaveSettings();
    }

    public void MusicChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), GD.Linear2Db(value));
        SaveSystem.Settings.SetMusicVolume(value);
        SaveSystem.SaveSettings();
    }
}
