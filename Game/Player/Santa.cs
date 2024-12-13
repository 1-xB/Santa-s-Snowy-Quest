using Godot;
using System;

public partial class Santa : CharacterBody2D
{
    private Vector2 _velocity = Vector2.Zero;
    [Export] AnimatedSprite2D _animatedSprite;
    [Export] public float Speed = 200f;
    [Export] public float Gravity = 500f;
    [Export] public float JumpForce = 100f;
    [Export] public float Friction = 800f;

    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;

        if (!IsOnFloor())
        {
            ApplyGravity(delta);
        }
        
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            ApplyJump();
        }
        
        float inputDirection = Input.GetAxis("move_left", "move_right");
        ApplyMovement(inputDirection, delta);
        
        UpdateAnimation();
        
        Velocity = _velocity;
        MoveAndSlide();
        
        
    }
    

    private void ApplyJump()
    {
        _velocity.Y = -JumpForce;
    }

    private void ApplyGravity(double delta)
    {
        _velocity.Y += Gravity * (float)delta;
    }

    private void ApplyMovement(float inputDirection, double delta)
    {
        if (inputDirection != 0)
            _velocity.X = Speed * inputDirection * (float)delta;
        else
            _velocity.X = Mathf.MoveToward(_velocity.X, 0, Friction * (float)delta);
    }
    
    private void UpdateAnimation()
    {
        if (!IsOnFloor())
        {
            if (_velocity.Y < 0)
            {
                _animatedSprite.Play("jump_up");
            }
            else
            {
                _animatedSprite.Play("jump_down");
            }
        }
        else
        {
            if (_velocity.X != 0)
            {
                _animatedSprite.Play("run");
            }
            else
            {
                _animatedSprite.Play("idle");
            }
        }
        _animatedSprite.FlipH = _velocity.X < 0;
        
    }
}