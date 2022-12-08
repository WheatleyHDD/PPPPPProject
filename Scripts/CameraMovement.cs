using Godot;
using System;

public class CameraMovement : Camera2D
{
    public bool follow = true;
    public Node2D target;

    [Export] public Vector2 screen_size = new Vector2(1280, 720); 

    private float shake_amount = 0;
    private Timer shakeTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        shakeTimer = new Timer();
        shakeTimer.OneShot = true;
        AddChild(shakeTimer);
        shakeTimer.Connect("timeout", this, "OnShakeStop");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (shake_amount != 0) {
            GD.Randomize();
            Offset = new Vector2(
                (float)GD.RandRange(-1, 1) * shake_amount,
                (float)GD.RandRange(-1, 1) * shake_amount
            );
        } else Offset = Vector2.Zero;

        if (target != null && follow)
        {
            var new_pos = GlobalPosition;

            if (target.GlobalPosition.x > new_pos.x + screen_size.x / 2)
                new_pos.x = new_pos.x + screen_size.x;
            else if (target.GlobalPosition.x < new_pos.x - screen_size.x / 2)
                new_pos.x = new_pos.x - screen_size.x;
            
            if (target.GlobalPosition.y > new_pos.y + screen_size.y / 2)
                new_pos.y = new_pos.y + screen_size.y;
            else if (target.GlobalPosition.y < new_pos.y - screen_size.y / 2)
                new_pos.y = new_pos.y - screen_size.y;
            
            GlobalPosition = new_pos;
        }
    }

    public void Shake(float amount, float time) {
        shake_amount = amount;
        if (!shakeTimer.IsStopped()) shakeTimer.Stop();
        shakeTimer.Start(time);
    }

    void OnShakeStop() => shake_amount = 0;
}
