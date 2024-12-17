using Godot;
using System;

public partial class WrongElf : CharacterBody2D
{
    [Export] private Area2D _leftArea2D;
    [Export] private Area2D _rightArea2D;

    [Export] private Area2D _leftVision;
    [Export] private Area2D _rightVision;
    
    bool _isLeftColliding = false;
    bool _isRightColliding = false;
    
    bool _isMovingRight = true;
    
    Vector2 _velocity = Vector2.Zero;
    
    float _speed = 50f;
    
    bool _isAttacking = false;

    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;
        
        MovingOnPlatform();

        Velocity = _velocity;
        MoveAndSlide();
        
        base._PhysicsProcess(delta);
    }

    private void MovingOnPlatform()
    {
        if (_isAttacking) return;

        // Note : .GetOverlappingBodies() zwraca listę obiektów,
        // które kolidują z danym obszarem (czyli tutaj z tilemapą)
        _isLeftColliding = _leftArea2D.GetOverlappingBodies().Count > 0;
        _isRightColliding = _rightArea2D.GetOverlappingBodies().Count > 0;
        
        // Działa to tak, że jeśli elf jest w ruchu w prawo i kolizja z prawą ścianą to idź w prawo.
        if (_isMovingRight && _isRightColliding)
        {
            _velocity.X = _speed;
            _leftVision.Monitoring = false;
            _rightVision.Monitoring = true;
        }
        else if (!_isMovingRight && _isLeftColliding)
        {
            _velocity.X = -_speed;
            _leftVision.Monitoring = true;
            _rightVision.Monitoring = false;
        }
        else if (!_isRightColliding || !_isLeftColliding) // Jeśli brak kolizji to zmienia kierunek
        {
            _isMovingRight = !_isMovingRight;
        }
        
        // TODO : Zaimplementować atak elfa
    }

    private void ApplyMovement(double delta)
    {
        _velocity.X = _speed * (float)delta;
    }
}
