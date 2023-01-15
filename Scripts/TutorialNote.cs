using Godot;
using Godot.Collections;

public class TutorialNote : Node2D
{
    Array players;

    [Export(PropertyHint.Enum, "All,MainCharacter,SecondCharacter")]
    public int CharToShow = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        players = GetTree().GetNodesInGroup("player");
        foreach(PlayerBase player in players) {
            if (CharToShow == 0) {
                if (player.current)
                    Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 1 - player.GlobalPosition.DistanceTo(GlobalPosition)/200);
            } else if (CharToShow == 1) {
                if (player.current && player.Name == "MainCharacter")
                    Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 1 - player.GlobalPosition.DistanceTo(GlobalPosition)/200);
            } else if (CharToShow == 2) {
                if (player.current && player.Name == "SecondCharacter")
                    Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 1 - player.GlobalPosition.DistanceTo(GlobalPosition)/200);
                else Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 0);
            }
        }
    }
}
