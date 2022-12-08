using Godot;
using System;

public class MainCharacter : PlayerBase
{
    bool sliding = false;
    [Export] public float slide_speed = 1500;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_down") && !sliding && current) {
            StopMoving();
            velocity = new Vector2(1 * last_dir, 0.5f) * slide_speed;
            sliding = true;
            GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        }
        if (!sliding) h_move = Input.GetAxis("ui_left", "ui_right");

        if (sliding) {
            velocity.x = Mathf.Lerp(velocity.x, 0, 0.01f);
            if (velocity.y != 0) velocity.y = Mathf.Lerp(velocity.y, 0, 0.01f);
            if (velocity.x == 0) {
                if (GetNode<RayCast2D>("up").IsColliding() && GetNode<RayCast2D>("down").IsColliding())
                    Die();
                GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
                sliding = false;
            }
        }
        Animating();
        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!sliding) {
            btn_is_jump_pressed = Input.IsActionJustPressed("jump");
            btn_is_jump_released = Input.IsActionJustReleased("jump");
        }
        base._PhysicsProcess(delta);
    }

    void Animating() {
        var model = GetNode<Node2D>("Model");
        model.Scale = new Vector2(last_dir, model.Scale.y);

        var animator = GetNode<AnimationPlayer>("Slider");
        if (sliding) animator.Play("slide");
        else animator.Play("idle");
    }
}
