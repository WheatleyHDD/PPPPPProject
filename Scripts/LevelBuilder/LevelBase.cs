using Godot;
using System;

public class LevelBase : Node2D
{
    [Export] private AudioStream music;
    private PackedScene char_iter = GD.Load<PackedScene>("res://Objects/CharacterIterator.tscn");

    Node level_objs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<MusicPlayer>("/root/MusicPlayer/LevelMusic").SetMusic(music);
        GetNode<MusicPlayer>("/root/MusicPlayer/LevelMusic").FadeResume();
        LevelGen();
    }

    void LevelGen()
    {
        level_objs = GetNode("LevelObjects");
        var helper_objs = GetNode("LevelHelpers");

        // Добавляем итератор персонажей
        CharacterIterator ci = char_iter.Instance() as CharacterIterator;
        level_objs.AddChild(ci);

        // Заносим в него персонажей
        foreach (Node2D child in helper_objs.GetChildren()) {
            if (child.IsInGroup("CharacterSpawner")) 
            {
                CharacterSpawner spwn = child as CharacterSpawner;
                if (spwn.character is null) return;
                Node2D character = spwn.character.Instance() as Node2D;
                character.GlobalPosition = spwn.GlobalPosition;
                //level_objs.GetNode("CharacterIterator").AddChild(character);
                ci.AddChild(character);
            }
        }
        ci.camera = GetNode<Camera2D>("Camera2D") as CameraMovement;
        ci.SetCurrent(true);
    }
}
