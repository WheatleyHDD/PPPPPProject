using Godot;
using System;

public class MusicPlayer : AudioStreamPlayer
{
    [Export] public float Volume;
    float _pausePos = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Volume = GD.Db2Linear(VolumeDb);
    }

    public override void _Process(float delta)
    {
        VolumeDb = GD.Linear2Db(Volume);
    }

    public void FadePause() {
        var _tween = GetTree().CreateTween();
        _tween.TweenProperty(this, "Volume", 0f, 1f);
        _tween.TweenCallback(this, "Pause");
    }

    public void FadeResume() {
        var _tween = GetTree().CreateTween();
        _tween.TweenProperty(this, "Volume", 1f, 1f);
        _tween.TweenCallback(this, "Resume");
    }

    public void Pause() {
        _pausePos = GetPlaybackPosition();
        Stop();
    }

    public void Resume() {
        Play(_pausePos);
    }

    public void SetMusic(AudioStream music) {
        if (music == Stream) return;
        Stream = music;
        _pausePos = 0f;
    }
}
