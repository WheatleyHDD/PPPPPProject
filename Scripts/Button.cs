using Godot;
using System;

public class Button : Node2D
{
    [Export]
    public NodePath[] connected_object;

    public bool Pressed;
    
    bool executedState = false;
    Area2D CollisionArea;
    Sprite sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CollisionArea = GetNode<Area2D>("Area2D");
        sprite = GetNode<Sprite>("Button");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var isAtLeastOne = false;
        foreach (var p in GetTree().GetNodesInGroup("player")) {
            if (p is PlayerBase) {
                var pb = p as PlayerBase;
                if (CollisionArea.OverlapsBody(pb)) {
                    isAtLeastOne = true;
                    break;
                }
            }
        }
        Pressed = isAtLeastOne;
    }

    public override void _Process(float delta)
    {
        sprite.Frame = Pressed ? 1 : 0;

        if (Pressed != executedState) {
            executedState = Pressed;
            ExecuteObjects(Pressed);
        }
    }

    public void ExecuteObjects(bool state) {
        var method = state ? "Activate" : "Deactivate";
        if (connected_object.Length > 0) {
            foreach (NodePath obj in connected_object) {
                if (obj != "") {
                    if (GetNode(obj).HasMethod(method))
                        GetNode(obj).Call(method);
                }
            }
        }
    }
}
