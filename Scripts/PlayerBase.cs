using Godot;
using Godot.Collections;
using System;

public class PlayerBase : KinematicBody2D
{
    // Настраиваемые переменные
    [Export] public float acceleration = 4500f;
    [Export] public float move_speed = 200f;
    [Export] public float jump_height = 600f;
    [Export] public float gravity_scale = 120f;
    [Export] public float gravity_dir = 1f;
    [Export] public int max_jump_count = 1;

    // Вспомогательные настраиваемые переменные 
    public float h_move = 0;
    public bool btn_is_jump_released;
    public bool btn_is_jump_pressed;

    // Включение и выкл каких-то механик
    public bool current = false;

    // Вспомогательные переменные 
    protected bool stopped_animation = false;
    bool jump_released = false;
    bool jumping;
    bool being_on_floor;
    int jump_count = 0;
    public int last_dir = 1;
    protected Vector2 velocity;
    Vector2 snap_normal;
    Vector2 FLOOR = Vector2.Up;
    Vector2 savedPosition;

    public CameraMovement cam;

    private AnimationPlayer base_animator;
    private AnimationPlayer char_animator;
    private Node2D char_model;

    private AudioStreamPlayer2D jump_player;
    private Listener2D listener;
    private AudioStreamPlayer2D land_player;
    private AudioStreamPlayer2D hit_player;

    public Array<Toggle> interacted_items = new Array<Toggle>();

    // Сигналы
    [Signal] delegate void OnJump(int jump_count);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SavePosition();
        jump_player = GetNode<AudioStreamPlayer2D>("Jump");
        base_animator = GetNode<AnimationPlayer>("BaseAnimator");
        char_animator = GetNode<AnimationPlayer>("CharModel/AnimationPlayer");
        listener = GetNode<Listener2D>("Listener2D");
        land_player = GetNode<AudioStreamPlayer2D>("Land");
        hit_player = GetNode<AudioStreamPlayer2D>("Hit");
        char_model = GetNode<Node2D>("CharModel");
    }

    public override void _PhysicsProcess(float delta)
    {
        HorizontalMovement(delta);
        VerticalMovement(delta);
        velocity = MoveAndSlideWithSnap(velocity, snap_normal, FLOOR);
    }

    public override void _Process(float delta)
    {
        if (!stopped_animation) Animate();
        // Получаем последний поворот
        if (current & h_move != 0) last_dir = (int)(h_move / Mathf.Abs(h_move));

        if (current) listener.MakeCurrent();
        else listener.ClearCurrent();
    }

    void HorizontalMovement(float delta)
    {
        if (!current) return;
        velocity = velocity.MoveToward(new Vector2(h_move * move_speed, velocity.y), acceleration * delta);
    }

    void VerticalMovement(float delta)
    {
        if (btn_is_jump_released) jump_released = true;

        // Аркадная физика IDK
        velocity.y += 9.807f * gravity_scale * delta;
        // Падает
        if (velocity.y * gravity_dir > 0) velocity.y += 9.807f * gravity_scale * delta;
        // Прыгает
        else if (velocity.y * gravity_dir < 0 && jump_released) velocity.y += 9.807f * gravity_scale * delta;

        SetSnapNormal(Vector2.Down);
        if (!IsOnFloorNew()) SetSnapNormal(Vector2.Zero);

        if (IsOnFloorNew()){
            // Ставим дефолтные значения при приземлении
            if (jumping) {
                base_animator.Play("land");
                land_player.Play();
            }
            jumping = false;
            being_on_floor = true;
            jump_count = 0;
        } else if (being_on_floor && !jumping) {
            // Убираем один прыжок, если спрыгнули с платформы
            being_on_floor = false;
            jump_count += 1;
        }

        if (current) {
            // Сам прыжок
            if (btn_is_jump_pressed && jump_count < max_jump_count){
                jump_count += 1;
                jumping = true;
                jump_released = false;
                base_animator.Play("jump");
                jump_player.PitchScale = Mathf.Min(jump_count, 1.25f);
                jump_player.Play();
                velocity.y = -jump_height * gravity_dir;
                EmitSignal("OnJump", jump_count);
            }
        }
    }

    void SetSnapNormal(Vector2 snap_dir, float count = 1) => snap_normal = snap_dir * count;

    bool IsOnFloorNew() => gravity_dir < 0 ? IsOnCeiling() : IsOnFloor();

    public void StopMoving() {
        h_move = 0;
        velocity.x = 0;
    }

    public void Die() {
        hit_player.Play();
        cam.Shake(12, 0.2f);
        velocity = Vector2.Zero;
        RestoreAll();
        GlobalPosition = savedPosition;
    }

    public void SavePosition() {
        savedPosition = GlobalPosition;
        interacted_items.Clear();
    }

    public void RestoreAll() {
        foreach (Toggle i in interacted_items) {
            i.Activated = false;
        }
        interacted_items.Clear();
    }

    public void AddToInteracted(Toggle toggle) => interacted_items.Add(toggle);

    void Animate() {
        char_model.Scale = new Vector2(last_dir * Mathf.Abs(char_model.Scale.x), char_model.Scale.y);
        char_animator.PlaybackSpeed = 1f;
        if (IsOnFloorNew()) {
            if (velocity.x != 0) {
                char_animator.PlaybackSpeed = 1.5f;
                char_animator.Play("run");
            }
            else char_animator.Play("idle");
        } else {
            if (velocity.y < 0) char_animator.Play("jump");
            else char_animator.Play("jump");
        }
    }
}
