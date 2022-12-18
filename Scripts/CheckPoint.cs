using Godot;
using System;

public class CheckPoint : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, "OnBodyEntered");   
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    void OnBodyEntered(Node body) {
        if (!body.IsInGroup("player")) return;
        var p = body as PlayerBase;
        p.SavePosition();
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        GetNode<Node2D>("Text").RotationDegrees = rng.RandfRange(-45, 45);
        GetNode<AnimationPlayer>("Text/AnimationPlayer").Play("fly_and_fade");
        GetNode<AnimationPlayer>("AnimationPlayer").Play("picked");
        GetNode<AudioStreamPlayer2D>("Picked").Play();
    }
}
