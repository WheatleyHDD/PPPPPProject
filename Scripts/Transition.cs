using Godot;

public class Transition : CanvasLayer
{   
    TextureRect _textureRect;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TransitionTexture");
        _textureRect.RectPosition = new Vector2(1280f, 0f);
    }

    public void ChangeSceneTo(PackedScene scene, float timeTo = 0.5f, float timeFrom = 0.5f) {
        var arg = new Godot.Collections.Array();
        arg.Add(scene);

        var _tween = GetTree().CreateTween();
        _tween.TweenProperty(_textureRect, "rect_position:x", -320f, timeTo);
        _tween.TweenCallback(GetTree(), "change_scene_to", arg);
        _tween.TweenProperty(_textureRect, "rect_position:x", -1920f, timeFrom);

        _textureRect.RectPosition = new Vector2(1280f, 0f);
    }
}
