using Godot;
using System;

public class MovingPlatform : KinematicBody2D
{
    [Export(PropertyHint.Enum, "Down,Up,Left,Right")]
    public int StartVectorInt = 0;
    public Vector2 StartVector;
    [Export] public float Speed = 100f;
    [Export] public bool Activated = true;

    public Vector2 startPosition;
    public float moveVector = 1;

    Vector2 Velocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startPosition = GlobalPosition;
        StartVector = Vector2.Down;
        switch (StartVectorInt) {
            case 0:
                StartVector = Vector2.Down;
                break;
            case 1:
                StartVector = Vector2.Up;
                break;
            case 2:
                StartVector = Vector2.Left;
                break;
            case 3:
                StartVector = Vector2.Right;
                break;
        }
        
        GetNode<Area2D>("CollisionAreaDown").Connect("body_entered", this, "OnDownCollide");
        GetNode<Area2D>("CollisionAreaUp").Connect("body_entered", this, "OnUpCollide");
        GetNode<Area2D>("CollisionAreaRight").Connect("body_entered", this, "OnRightCollide");
        GetNode<Area2D>("CollisionAreaLeft").Connect("body_entered", this, "OnLeftCollide");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _PhysicsProcess(float delta)
    {
        if (!Activated) return;

        if (StartVectorInt == 0) if (GlobalPosition.y < startPosition.y) moveVector = 1;
        if (StartVectorInt == 1) if (GlobalPosition.y > startPosition.y) moveVector = 1;
        if (StartVectorInt == 2) if (GlobalPosition.x > startPosition.x) moveVector = 1;
        if (StartVectorInt == 3) if (GlobalPosition.x < startPosition.x) moveVector = 1;

        Velocity = StartVector * Speed * moveVector;
        Position += Velocity * delta;
    }

    void OnDownCollide(Node body) {
        if (body == this || StartVectorInt != 0) return;
        if (body is PhysicsBody2D || body is TileMap) moveVector = -1;
    }

    void OnUpCollide(Node body) {
        if (body == this || StartVectorInt != 1) return;
        if (body is PhysicsBody2D || body is TileMap) moveVector = -1;
    }

    void OnLeftCollide(Node body) {
        if (body == this || StartVectorInt != 2) return;
        if (body is PhysicsBody2D || body is TileMap) moveVector = -1;
    }

    void OnRightCollide(Node body) {
        if (body == this || StartVectorInt != 3) return;
        if (body is PhysicsBody2D || body is TileMap) moveVector = -1;
    }

    public void Activate() => Activated = true;
    public void Deactivate() => Activated = false;
}
