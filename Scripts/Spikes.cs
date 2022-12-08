using Godot;
using System;

public class Spikes : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
        
    // }

    public override void _PhysicsProcess(float delta)
    {
        foreach (var p in GetTree().GetNodesInGroup("player")) {
            if (p is PlayerBase) {
                var pb = p as PlayerBase;
                if (OverlapsBody(pb)) {
                    pb.Die();
                }
            }
        }
    }
}
