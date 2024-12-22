using Godot;
using System;

public partial class FlyingPlatform : CharacterBody2D
{
    [Export] float _speed = 100.0f;
    Vector2 _velocity = Vector2.Zero;
    bool _isMovingRight = true;

    public override void _PhysicsProcess(double delta)
    {


        _velocity = Velocity;

        Moving();

        _velocity.Y = 0;
        Velocity = _velocity;
        MoveAndSlide();
        base._PhysicsProcess(delta);

    }

    private void Moving()
    {
        
        if (IsOnWall())
        {
            _isMovingRight = !_isMovingRight;
        }

        if (_isMovingRight)
        {
            _velocity.X = _speed;
        }
        else
        {
            _velocity.X = -_speed;
        }

    }



    

}