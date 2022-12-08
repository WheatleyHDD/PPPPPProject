using Godot;
using System;

[Tool]
public class HelperScreenBorders : Node2D
{
    [Export(PropertyHint.Range, "1,20,")] public int screen_count_horizontal = 1;
    [Export(PropertyHint.Range, "1,20,")] public int screen_count_vertical = 1;

    [Export] public int screen_width = 1280;
    [Export] public int screen_height = 720;

    [Export(PropertyHint.Range, "1,20,")] public int line_width = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        if (Engine.EditorHint) Update();
    }

    public override void _Draw()
    {
        if (Engine.EditorHint) {
            // По X
            for (int i = 0; i < screen_count_horizontal+1; i++) {
                DrawLine(
                    new Vector2(i * screen_width, 0),
                    new Vector2(i * screen_width, screen_count_vertical * screen_height),
                    Colors.Red,
                    line_width
                );
            }
            // По Y
            for (int i = 0; i < screen_count_vertical+1; i++) {
                DrawLine(
                    new Vector2(0, i * screen_height),
                    new Vector2(screen_count_horizontal * screen_width, i * screen_height),
                    Colors.Red,
                    line_width
                );
            }
        }
    }
}
