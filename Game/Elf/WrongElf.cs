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
    
    [Export] Timer _throwTimer;
    bool _isAttacking = false;
    bool canThrow = true;

    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;
        
        
        MovingOnPlatform();
        ThrowSnowball();
        
        
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
        
    }
    
    private void ThrowSnowball()
    {
        if (_isAttacking && canThrow)
        {
            Snowball snowball = (Snowball)GD.Load<PackedScene>("res://Scenes/Snowball.tscn").Instantiate();
            GetTree().Root.AddChild(snowball);
            snowball.Position = Position;
            if (!_isMovingRight) snowball.Speed = -snowball.Speed;
            canThrow = false;
            _throwTimer.Start();
        }
    }
    
    private void _on_LeftVision_body_entered(Node2D body)
    {
        GD.Print("LeftVision body entered");
        if (body is Santa player)
        {
            _isAttacking = true;
        }
    }
    
    private void _on_LeftVision_body_exited(Node2D body)
    {
        GD.Print("LeftVision body exited");
        if (body is Santa player)
        {
            _isAttacking = false;
        }
    }
    
    private void _on_RightVision_body_entered(Node2D body)
    {
        GD.Print("RightVision body entered");
        if (body is Santa player)
        {
            _isAttacking = true;
        }
    }
    
    private void _on_RightVision_body_exited(Node2D body)
    {
        GD.Print("RightVision body exited");
        if (body is Santa player)
        {
            _isAttacking = false;
        }
    }
    
    private void _on_ThrowTimer_timeout()
    {
        canThrow = true;
    }
}
