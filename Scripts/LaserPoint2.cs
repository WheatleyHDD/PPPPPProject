using Godot;
using System;

public class LaserPoint2 : Sprite
{
    [Export] public bool IsCasting = true;    

    [Export] public float OnTime = 0f;
    [Export] public float OffTime = 0f;

    [Export] public NodePath connected_to;
    
    Vector2 connPosition;

    Timer onofftimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (connected_to != null) {
            var ct = GetNode(connected_to) as Node2D;
            connPosition = GlobalPosition - ct.GlobalPosition;
        }
        GetNode<LaserPoint>("Laser1").IsCasting = IsCasting;
        GetNode<LaserPoint>("Laser1").OnTime = OnTime;
        GetNode<LaserPoint>("Laser1").OffTime = OffTime;

        GetNode<LaserPoint>("Laser2").IsCasting = IsCasting;
        GetNode<LaserPoint>("Laser2").OnTime = OnTime;
        GetNode<LaserPoint>("Laser2").OffTime = OffTime;
    }

    public override void _Process(float delta)
    {
        if (connected_to != null) {
            var ct = GetNode(connected_to) as Node2D;
            GlobalPosition = ct.GlobalPosition + connPosition;
        }
    }
}
