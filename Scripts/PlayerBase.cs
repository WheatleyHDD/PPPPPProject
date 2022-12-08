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

    public Array<Toggle> interacted_items = new Array<Toggle>();

    // Сигналы
    [Signal] delegate void OnJump(int jump_count);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SavePosition();
    }

    public override void _PhysicsProcess(float delta)
    {
        HorizontalMovement(delta);
        VerticalMovement(delta);
        velocity = MoveAndSlideWithSnap(velocity, snap_normal, FLOOR);
    }

    public override void _Process(float delta)
    {
        // Получаем последний поворот
        if (h_move != 0) last_dir = (int)(h_move / Mathf.Abs(h_move));
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

        if (current) {
            if (IsOnFloorNew()){
                // Ставим дефолтные значения при приземлении
                if (jumping)
                    GetNode<AnimationPlayer>("BaseAnimator").Play("land");
                jumping = false;
                being_on_floor = true;
                jump_count = 0;
            } else if (being_on_floor && !jumping) {
                // Убираем один прыжок, если спрыгнули с платформы
                being_on_floor = false;
                jump_count += 1;
            }

            // Сам прыжок
            if (btn_is_jump_pressed && jump_count < max_jump_count){
                jump_count += 1;
                jumping = true;
                jump_released = false;
                GetNode<AnimationPlayer>("BaseAnimator").Play("jump");
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
        // GD.Print("Персонаж ", Name, " помер");
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
}
