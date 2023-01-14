using Godot;

public class CreditsScreen : ColorRect
{
    public override void _Ready()
    {
        GetNode<Button>("%BackToMenuButton").Connect("pressed", this, "BackPressed");
    }

    public void BackPressed() {
        GetNode<Transition>("/root/Transition").ChangeSceneTo(GD.Load<PackedScene>("res://Scenes/TempMenu.tscn"));
    }
}
