using Godot;
using System;

public class BulkBlock : StaticBody2D
{
    bool bulk = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Area2D>("BulkArea").Connect("body_entered", this, "OnBodyEntered");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    void OnBodyEntered(Node body) {
        if (!body.IsInGroup("player") || bulk) return;

        bulk = true;
        var tween = GetTree().CreateTween();
        tween.TweenInterval(0.25f);
        tween.TweenProperty(GetNode<Sprite>("BulkBlock"), "modulate:a", 0f, 0.1f);
        tween.TweenCallback(this, "SetBulked", new Godot.Collections.Array(true));
        tween.TweenInterval(3f);
        tween.TweenProperty(GetNode<Sprite>("BulkBlock"), "modulate:a", 1f, 0.1f);
        tween.TweenCallback(this, "SetBulked", new Godot.Collections.Array(false));
    }

    void SetBulked(bool bulked) {
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", bulked);
        bulk = bulked;

        var rng = new RandomNumberGenerator();
        rng.Randomize();
        var sound = GD.Load("res://Sounds/Crumble/Rock_crumble_"+rng.RandiRange(0,2)+".wav") as AudioStream;
        GetNode<AudioStreamPlayer2D>("Crumble").Stream = sound;
        GetNode<AudioStreamPlayer2D>("Crumble").Play();
    }
}
