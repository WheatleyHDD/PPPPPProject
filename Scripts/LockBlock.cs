using Godot;
using System;

public class LockBlock : StaticBody2D
{
    [Export] public AudioStream[] streamsToPlay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Sprite>("LockBlock").Material = GetNode<Sprite>("LockBlock").Material.Duplicate() as Material;
    }

    public void Unlock() {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        var stream = rng.Randi() % streamsToPlay.Length;
        GetNode<AudioStreamPlayer2D>("lockbreak").Stream = streamsToPlay[stream];
        GetNode<AudioStreamPlayer2D>("lockbreak").Play();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("destroy");
    }

    public void Lock() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
    }
}
