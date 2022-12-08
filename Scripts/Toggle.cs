using Godot;
using System;

public class Toggle : Area2D
{
    [Export]
    public NodePath[] connected_object;
    [Export]
    public bool StartPos;

    private bool _activated;
    public bool Activated
    {
        get { return _activated; }
        set {
            _activated = value;
            if (value) Activate();
            else Deactivate();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Activated != StartPos)
            Activated = StartPos;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        foreach (var p in GetTree().GetNodesInGroup("player")) {
            if (p is PlayerBase) {
                var pb = p as PlayerBase;
                if (OverlapsBody(pb) && Input.IsActionJustPressed("use") && pb.current) {
                    Activated = !Activated;
                    pb.AddToInteracted(this);
                }
            }
        }
    }

    void Activate() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("pull_down");
        if (connected_object.Length > 0) {
            foreach (NodePath obj in connected_object) {
                if (obj != "") {
                    if (GetNode(obj).HasMethod("Activate"))
                        GetNode(obj).Call("Activate");
                }
            }
        }         
    }

    void Deactivate() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("pull_up");
        if (connected_object.Length > 0) {
            foreach (NodePath obj in connected_object) {
                if (obj != "") {
                    if (GetNode(obj).HasMethod("Deactivate"))
                        GetNode(obj).Call("Deactivate");
                }
            }
        }
    }
}
