using Godot;
using System;

public class LockBlocks : Node2D
{
    SceneTreeTween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void Unlock() {
        _tween = GetTree().CreateTween();
        foreach(LockBlock block in GetChildren()) {
            _tween.TweenInterval(0.1f);
            _tween.TweenCallback(block, "Unlock");
            //block.GetNode<AnimationPlayer>("AnimationPlayer").Play("destroy");
        }
    }

    public void Lock() {
        foreach(LockBlock block in GetChildren())
            block.Lock();
    }
}
