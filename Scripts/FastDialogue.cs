using Godot;
using System.Collections;

public class FastDialogue : CanvasLayer
{
    private ArrayList query = new ArrayList();
    int voice_count = 0;

    bool _dialoguePlaying = false;
    bool _phaseType = false;

    Label UpLabel;
    Label DownLabel;

    CharacterIterator _ci;
    Timer dial_time;
    AudioStreamPlayer voice_player;

    SceneTreeTween _tween;

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
        if (!IsInstanceValid(_ci)) return;
        
        // Звук диалога
        /*
        if (voice_count > 0) {
            if (!voice_player.Playing) {
                voice_player.Play();
                //voice_player.PitchScale = (float)GD.RandRange(1, 2);
                voice_count--;
            }
        } else if (dial_time.IsStopped()) dial_time.Start();
        */

        if (_tween.IsRunning() && !voice_player.Playing) {
            voice_player.Play();
        }
        else voice_player.Stop();

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
        if (_dialoguePlaying) return;
        _ci = GetTree().CurrentScene.GetNode<Node2D>("LevelObjects/CharacterIterator") as CharacterIterator;
        NextDialogue();
    }

    void NextDialogue() {
        _dialoguePlaying = true;
        Show();
        var _current = query[0] as DialogueData;

        UpLabel.Text = string.Format("{0}: {1}", _current.DialogueName, _current.DialogueText);
        UpLabel.VisibleCharacters = string.Format("{0}: ", _current.DialogueName).Length;

        DownLabel.Text = string.Format("{0}: {1}", _current.DialogueName, _current.DialogueText);
        DownLabel.VisibleCharacters = string.Format("{0}: ", _current.DialogueName).Length;

        if (_current.DialogueVoice != null) 
        {
            voice_player.Stream = _current.DialogueVoice;
            voice_player.VolumeDb = _current.DialogueVolume;
        }

        _tween = GetTree().CreateTween();
        _tween.TweenProperty(UpLabel, "percent_visible", 1f, (_current.DialogueText.Count(" ") + 1) * 1.5f / 8);
        _tween.Parallel().TweenProperty(DownLabel, "percent_visible", 1f, (_current.DialogueText.Count(" ") + 1) * 1.5f / 8);
        _tween.TweenCallback(dial_time, "start");
    }

    void OnDialogueEnd() {
        if (query.Count > 0) query.RemoveAt(0);
        if (query.Count > 0) NextDialogue();
        else {
            _dialoguePlaying = false;
            Hide();
        }
    }

    public void StopAll() {
        query.Clear();
        Hide();
    }
}

class DialogueData {

    public string DialogueName;
    public string DialogueText;
    public AudioStream DialogueVoice;
    public float DialogueVolume;

    public DialogueData(string name, string text, AudioStream voice, float volume = 0) {
        DialogueName = name;
        DialogueText = text;
        DialogueVoice = voice;
        DialogueVolume = volume;
    }
}
