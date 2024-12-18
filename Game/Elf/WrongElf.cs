using Godot;
using System;

public partial class WrongElf : CharacterBody2D
{
    // Wykrywanie kolizji z dołu
    [Export] private Area2D _leftArea2D;
    [Export] private Area2D _rightArea2D;
    bool _isMovingRight = true;
    
    // Wykrywanie gracza
    [Export] private Area2D _leftVision;
    [Export] private Area2D _rightVision;
    
    bool _isLeftColliding = false;
    bool _isRightColliding = false;
    
    
    // Poruszanie się
    Vector2 _velocity = Vector2.Zero;
    [Export] Timer _moveTimer;
    bool _canMove = true;
    float _speed = 30f;
    
    // Rzucanie śnieżką
    [Export] Timer _throwTimer;
    bool _isAttacking = false;
    bool _canThrow = false;

    // Życie
    float _health = 3;
    
    // Animacja
    [Export] AnimatedSprite2D _animatedSprite;
    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;
        
        
        MovingOnPlatform();
        ThrowSnowball();
        AnimationHandler();
        
        
        Velocity = _velocity;
        MoveAndSlide();
        
        base._PhysicsProcess(delta);
    }
    

    private void MovingOnPlatform()
    {
        if (_isAttacking || !_canMove)
        {
            _velocity.X = 0;
            return;
        };

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
        if (_isAttacking && _canThrow)
        {
            Snowball snowball = (Snowball)GD.Load<PackedScene>("res://Scenes/Snowball.tscn").Instantiate();
            GetTree().Root.AddChild(snowball);
            snowball.Position = Position;
            if (!_isMovingRight) snowball.Speed = -snowball.Speed;
            snowball.SetOwner(this); // Ustawianie własciciela na siebie, by nie dostać obrażeń bo śniezka respi się w elfie i od razu go trafia.
            _canThrow = false;
            _throwTimer.Start();
        }
    }

    private void AnimationHandler()
    {
        if (_velocity.X != 0)
        {
            _animatedSprite.Play("walk");
            _animatedSprite.FlipH = _velocity.X < 0;
        }
    }
    
    
    private void _on_LeftVision_body_entered(Node2D body)
    {
        if (body is Santa player)
        {
            _isAttacking = true;
            _canThrow = false;
            _moveTimer.Stop(); // Stop the movement timer when the player is seen
            _throwTimer.Start(); // Start the throw timer to shoot after a delay
        }
    }

    private void _on_LeftVision_body_exited(Node2D body)
    {
        if (body is Santa player)
        {
            _isAttacking = false;
            _canThrow = false;
            _canMove = false;
            _moveTimer.Start(); // Start the movement timer to resume moving after a delay
        }
    }

    private void _on_RightVision_body_entered(Node2D body)
    {
        if (body is Santa player)
        {
            _isAttacking = true;
            _canThrow = false;
            _moveTimer.Stop(); // Stop the movement timer when the player is seen
            _throwTimer.Start(); // Start the throw timer to shoot after a delay
        }
    }

    private void _on_RightVision_body_exited(Node2D body)
    {
        if (body is Santa player)
        {
            _isAttacking = false;
            _canThrow = false;
            _canMove = false;
            _moveTimer.Start(); // Start the movement timer to resume moving after a delay
        }
    }

    private void _on_ThrowTimer_timeout()
    {
        _canThrow = true;
    }

    private void _on_MoveTimer_timeout()
    {
        _canMove = true;
    }
    
    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            QueueFree();
        }
    }
}
