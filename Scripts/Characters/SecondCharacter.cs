using Godot;
using System;

public class SecondCharacter : PlayerBase
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        h_move = Input.GetAxis("ui_left", "ui_right");
        Animating();
        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        btn_is_jump_pressed = Input.IsActionJustPressed("jump");
        btn_is_jump_released = Input.IsActionJustReleased("jump");
        base._PhysicsProcess(delta);
    }

    void Animating() {
        var model = GetNode<Node2D>("Model");
        model.Scale = new Vector2(last_dir, model.Scale.y);
    }
}
