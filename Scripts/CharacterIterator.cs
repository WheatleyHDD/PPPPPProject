using Godot;
using System;

public class CharacterIterator : Node2D
{
    public int current_char = 0;
    public CameraMovement camera;
    private Listener2D listener;
    private AudioStreamPlayer changeControlSound;
    public bool LevelChanging = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        changeControlSound = new AudioStreamPlayer();
        GetParent().CallDeferred("add_child", changeControlSound);
        changeControlSound.Stream = GD.Load<AudioStream>("res://Sounds/change_control.wav");
        changeControlSound.Bus = "sound";

        listener = new Listener2D();
        GetParent().CallDeferred("add_child", listener);
        listener.MakeCurrent();
        if (GetChildCount() > 0) SetCurrent(true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (GetCurrent().current && Input.IsActionJustPressed("ui_cancel")) {
            GetTree().Paused = !GetTree().Paused;
            GetNode<CanvasLayer>("/root/Pause").Visible = !GetNode<CanvasLayer>("/root/Pause").Visible;
        }

        if (GetTree().Paused || LevelChanging) return;
        
        if (Input.IsActionJustPressed("next_char")) NextCharacter();
        if (Input.IsActionJustPressed("prev_char")) PrevCharacter();
        if (IsInstanceValid(listener)) {
            listener.GlobalPosition = new Vector2(
                Mathf.Lerp(listener.GlobalPosition.x, GetCurrent().GlobalPosition.x, 0.03f),
                Mathf.Lerp(listener.GlobalPosition.y, GetCurrent().GlobalPosition.y, 0.03f));
        }
        
        Update();
    }

    void NextCharacter() {
        var char_count = GetChildCount() - 1;
        if (char_count > 0) changeControlSound.Play();
        SetCurrent(false);
        if (current_char < char_count)
            current_char += 1;
        else current_char = 0;
        SetCurrent(true);
    }

    void PrevCharacter() {
        var char_count = GetChildCount() - 1;
        if (char_count > 0) changeControlSound.Play();
        SetCurrent(false);
        if (current_char > 0)
            current_char -= 1;
        else current_char = char_count;
        SetCurrent(true);
    }
    
    public void SetCurrent(bool _current) {
        PlayerBase character = GetChild(current_char) as PlayerBase;
        character.current = _current;
        if (_current && camera != null) {
            camera.target = character as Node2D;
            character.cam = camera;
        }
        character.StopMoving();
    }
    
    public override void _Draw()
    {
        Node2D curr = GetChild(current_char) as Node2D;
        
        Texture selector_tex = GD.Load<Texture>("res://Sprites/selector.png");
        DrawTexture(selector_tex, curr.GlobalPosition - new Vector2(5, 55)); 
    }

    public PlayerBase GetCurrent() => GetChild(current_char) as PlayerBase;
}
