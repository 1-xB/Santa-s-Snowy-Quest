using Godot;
using System;

public partial class Coin : CharacterBody2D
{

    Label label;
    private int _coins = 0;
    Vector2 _velocity = Vector2.Zero;

    private float Gravity = 1000f;

    public override void _Ready()
    {

        var currentLevel = GetTree().CurrentScene; 
        label = currentLevel.GetNode<Label>("UI/Coin2/HBoxContainer/text/Coins");
    }
    public override void _Process(double delta)
    {
        _velocity = Velocity;

        if (!IsOnFloor()) {
            _velocity.Y += Gravity * (float)delta;
        }
        
        Velocity = _velocity;
        MoveAndSlide();
        
        base._Process(delta);
    }
    private void OnBodyEntered(Node body)
    {
        
        if (body is Santa)
        {
            _coins = label.Text.ToInt();
            _coins++;
            label.Text = _coins.ToString();
            QueueFree();
        }
    }
}
