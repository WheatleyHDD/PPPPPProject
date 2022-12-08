using Godot;
using System;

public class LaserPoint : Sprite
{
    [Export] public bool IsCasting = true;    

    [Export] public float OnTime = 0f;
    [Export] public float OffTime = 0f;

    [Export] public NodePath connected_to;
    
    Vector2 connPosition;

    RayCast2D laser;
    Particles2D smoke;
    Timer onofftimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        onofftimer = GetNode<Timer>("OnOffTimer");
        if (OnTime > 0f && OffTime > 0f) {
            onofftimer.Connect("timeout", this, "OnOffTimer_Timeout");
            onofftimer.WaitTime = IsCasting ? OnTime : OffTime;
            onofftimer.Start(IsCasting ? OnTime : OffTime);
        }
        if (connected_to != null) {
            var ct = GetNode(connected_to) as Node2D;
            connPosition = GlobalPosition - ct.GlobalPosition;
        }
        laser = GetNode<RayCast2D>("laser");
        smoke = GetNode<Particles2D>("smoke");
    }

    public override void _Process(float delta)
    {
        if (connected_to != null) {
            var ct = GetNode(connected_to) as Node2D;
            GlobalPosition = ct.GlobalPosition + connPosition;
        }
        laser.Enabled = IsCasting;
        laser.CastTo = IsCasting ? Vector2.Right * 1299 : Vector2.Zero;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!laser.Visible) return;

        var cast_point = laser.CastTo;
        laser.ForceRaycastUpdate();

        if (laser.IsColliding()) {
            smoke.Emitting = true;
            smoke.Position = ToLocal(laser.GetCollisionPoint());
            cast_point = ToLocal(laser.GetCollisionPoint());
        } else {
            smoke.Emitting = false;
        }

        if (IsCasting && laser.IsColliding() && laser.GetCollider() is PlayerBase) {
            var p = laser.GetCollider() as PlayerBase;
            p.Die();
        }
        
        if (IsCasting)
            GetNode<Line2D>("Line2D").Points = new Vector2[] { GetNode<Line2D>("Line2D").Points[0], new Vector2(cast_point.x, 0) };
        else GetNode<Line2D>("Line2D").Points = new Vector2[] { GetNode<Line2D>("Line2D").Points[0] };
    }

    public void OnOffTimer_Timeout() {
        IsCasting = !IsCasting;
        onofftimer.Start(IsCasting ? OnTime : OffTime);
    }

    public void Activate() {
        IsCasting = !IsCasting;
        smoke.Emitting = IsCasting;
        laser.Visible = IsCasting;
        GetNode<Line2D>("Line2D").Visible = IsCasting;
        /*
        smoke.Emitting = false;
        laser.Hide();
        GetNode<Line2D>("Line2D").Hide();
        */
    }
    public void Deactivate() {
        IsCasting = !IsCasting;
        smoke.Emitting = IsCasting;
        laser.Visible = IsCasting;
        GetNode<Line2D>("Line2D").Visible = IsCasting;
        /*
        smoke.Emitting = true;
        laser.Show();
        GetNode<Line2D>("Line2D").Show();
        */
    }
}
