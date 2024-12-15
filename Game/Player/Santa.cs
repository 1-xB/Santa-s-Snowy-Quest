using Godot;
using System;

public partial class Santa : CharacterBody2D
{
    private Vector2 _velocity = Vector2.Zero;
    [Export] AnimatedSprite2D _animatedSprite;
    
    
    // Movement Variables
    [Export] public float Speed = 200f;
    [Export] public float Gravity = 500f;
    [Export] public float JumpForce = 100f;
    [Export] public float Friction = 800f;
    
    // Snowball Variables
    bool _canThrow = true;

    
    // ledder variables
    [Export] RayCast2D _rayCast;
    bool _isOnLadder = false;
    public override void _PhysicsProcess(double delta)
    {
        
        _velocity = Velocity;

        _isOnLadder = _rayCast.IsColliding() && _rayCast.GetCollider() is TileMapLayer;

        if (_isOnLadder)
        {
            ApplyLadderMovement(delta);
        }
        else
        {
            // Gravity
            if (!IsOnFloor())
            {
                ApplyGravity(delta);
            }
        
            // Jump
            if (Input.IsActionPressed("jump") && IsOnFloor())
            {
                ApplyJump();
            }
        }
        
        // Movement
        float inputDirection = Input.GetAxis("move_left", "move_right");
        ApplyMovement(inputDirection, delta);

        
        // Throw snowball
        if (Input.IsActionJustPressed("throw") && _canThrow)
        {
            _canThrow = false;
            _animatedSprite.Play("throw");
        }
        
        // Update Animation
        UpdateAnimation();
        
        
        Velocity = _velocity;
        MoveAndSlide();
        
        
    }

    private void ThrowSnowball()
    {

        Snowball snowball = (Snowball)GD.Load<PackedScene>("res://Scenes/Snowball.tscn").Instantiate();
        GetTree().Root.AddChild(snowball);
        snowball.Position = Position;
        if (_animatedSprite.FlipH) snowball.Speed = -snowball.Speed;
        _canThrow = true;
    }
    
    private void OnAnimationFinished()
    {
        if (_animatedSprite.Animation == "throw")
        {
            ThrowSnowball();
        }
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
        if (!_canThrow) return;
        if (_isOnLadder)
        {
            _animatedSprite.Play("idle"); // nie mam animacji do wchodzenia po drabinie
            return;
        }
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
            _animatedSprite.FlipH = _velocity.X < 0;
        }
        else
        {
            if (_velocity.X != 0)
            {
                _animatedSprite.Play("run");
                _animatedSprite.FlipH = _velocity.X < 0;
            }
            else
            {
                _animatedSprite.Play("idle");
            }
        }
        
        
    }
    
    private void ApplyLadderMovement(double delta)
    {
        _velocity = Vector2.Zero;
        if (Input.IsActionPressed("move_up"))
        {
            _velocity.Y = -Speed * (float)delta;
        }
        else if (Input.IsActionPressed("move_down"))
        {
            _velocity.Y = Speed * (float)delta;
        }
    }
}