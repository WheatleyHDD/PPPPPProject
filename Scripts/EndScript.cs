using Godot;
using Godot.Collections;

public class EndScript : Sprite
{
    [Export] public string dialogueStart = "";
    [Export] public PackedScene nextLevel;
    [Export] public bool spawnDoor;
    [Export] public PackedScene door;

    AnimatedSprite dObj;
    Node dialogue;
    Array players;
    Area2D area2d;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        area2d = GetNode<Area2D>("Area2D");
        if (spawnDoor) {
            dObj = door.Instance() as AnimatedSprite;
            GetParent().CallDeferred("add_child",dObj);
            dObj.GlobalPosition = GetNode<Position2D>("DoorPos").GlobalPosition;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        players = GetTree().GetNodesInGroup("player");
        if (players.Count == 0) return;
        int playersOnEnd = 0;
        foreach(KinematicBody2D player in players)
            if (area2d.OverlapsBody(player) && (player as PlayerBase).IsOnFloor()) playersOnEnd++;

        if (playersOnEnd == players.Count) EndLevel();
    }

    public void EndLevel() {
        if (dialogueStart != "") {
            SetProcess(false);
            ((players[0] as Node2D).GetParent() as CharacterIterator).SetCurrent(false);
            foreach(PlayerBase player in players) player.StopMoving();

            if (spawnDoor) dObj.Play("open");

            dialogue = DialogicSharp.Start(dialogueStart);
            dialogue.Connect("dialogic_signal", this, "OnSignal");
            GetParent().AddChild(dialogue);
        } else ChangeScene();
    }

    public void OnSignal(string value) {
        if (value != "end") return;
        dialogue.QueueFree();
        ChangeScene();
    }
    public void ChangeScene() {
        GetTree().ChangeSceneTo(nextLevel);
        GetNode("/root/FancyFade").Call("tile_reveal", nextLevel.Instance());
    }
}
