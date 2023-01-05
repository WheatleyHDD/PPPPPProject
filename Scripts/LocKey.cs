using Godot;
using System;

public class LocKey : Area2D
{
    [Export] private NodePath lockBlocks;
    private bool picked = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, "BodyEntered");
    }

    void BodyEntered(Node2D body) {
        if (picked) return;
        if (!body.IsInGroup("player") || lockBlocks.IsEmpty()) return;
        if (!GetNode<Node2D>(lockBlocks).IsInGroup("LockBreak")) return;
        
        var pb = body as PlayerBase;
        GetNode<LockBlocks>(lockBlocks).Unlock();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("pickup");
        picked = true;

        pb.AddToInteracted(this);
    }

    public void NotPick() {
        picked = false;
        GetNode<LockBlocks>(lockBlocks).Lock();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
    }
}
