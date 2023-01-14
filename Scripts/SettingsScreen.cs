using Godot;
using System;

public class SettingsScreen : CanvasLayer
{
    public override void _Ready()
    {
        SaveSystem.LoadSettings();

        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("sound"), GD.Linear2Db((float)SaveSystem.Settings["sound_vol"]));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), GD.Linear2Db((float)SaveSystem.Settings["music_vol"]));
        OS.WindowFullscreen = (bool)SaveSystem.Settings["fullscreen"];

        GetNode<Slider>("%SoundVolume").Value = (float)SaveSystem.Settings["sound_vol"];
        GetNode<Slider>("%MusicVolume").Value = (float)SaveSystem.Settings["music_vol"];

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
        SaveSystem.Settings["fullscreen"] = OS.WindowFullscreen;
        SaveSystem.SaveSettings();
    }

    public void SoundChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("sound"), GD.Linear2Db((float)GetNode<Slider>("%SoundVolume").Value));
        SaveSystem.Settings["sound_vol"] = value;
        SaveSystem.SaveSettings();
    }

    public void MusicChanged(float value) {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), GD.Linear2Db((float)GetNode<Slider>("%MusicVolume").Value));
        SaveSystem.Settings["music_vol"] = value;
        SaveSystem.SaveSettings();
    }
}
