using Godot;
using System.Collections;

public class FastDialogue : CanvasLayer
{
    private ArrayList query = new ArrayList();
    int voice_count = 0;

    Label UpLabel;
    Label DownLabel;

    CharacterIterator _ci;
    Timer dial_time;
    AudioStreamPlayer voice_player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();

        UpLabel = GetNode<Label>("Dialogue/Up");
        DownLabel = GetNode<Label>("Dialogue/Down");

        dial_time = GetNode<Timer>("Timer");
        dial_time.Connect("timeout", this, "OnDialogueEnd");

        voice_player = GetNode<AudioStreamPlayer>("voice");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!Visible) return;

        // Звук диалога
        if (voice_count > 0) {
            if (!voice_player.Playing) {
                voice_player.Play();
                voice_player.PitchScale = (float)GD.RandRange(1, 2);
                voice_count--;
            }
        } else if (dial_time.IsStopped()) dial_time.Start();

        // Какой показывать
        if (Mathf.Ceil(_ci.GetCurrent().GlobalPosition.y / 360) % 2 != 0) {
            UpLabel.Hide();
            DownLabel.Show();
        } else {
            UpLabel.Show();
            DownLabel.Hide();
        }
    }

    public void AddToQuery(string dname, string dtext, AudioStream dvoice) =>
        query.Add(new DialogueData(dname, dtext, dvoice));
    
    public void StartDialogue() {
        _ci = GetTree().CurrentScene.GetNode<Node2D>("LevelObjects/CharacterIterator") as CharacterIterator;
        NextDialogue();
    }

    void NextDialogue() {
        Show();
        var _current = query[0] as DialogueData;
        UpLabel.Text = string.Format("{0}: {1}", _current.DialogueName, _current.DialogueText);
        DownLabel.Text = string.Format("{0}: {1}", _current.DialogueName, _current.DialogueText);
        
        if (_current.DialogueVoice != null) 
        {
            voice_count = (_current.DialogueText.Count(" ") + 1);
            voice_player.Stream = _current.DialogueVoice;
            voice_player.VolumeDb = _current.DialogueVolume;
        }
    }

    void OnDialogueEnd() {
        query.RemoveAt(0);
        if (query.Count > 0) NextDialogue();
        else Hide();
    }
}

class DialogueData {

    public string DialogueName;
    public string DialogueText;
    public AudioStream DialogueVoice;
    public float DialogueVolume;

    public DialogueData(string name, string text, AudioStream voice, float volume = -6) {
        DialogueName = name;
        DialogueText = text;
        DialogueVoice = voice;
        DialogueVolume = volume;
    }
}
