using Godot;
using System;

public partial class WrongElf : CharacterBody2D
{
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
    [Export] ProgressBar _healthBar;
    float _health = 3;
    
    // Animacja
    [Export] AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _healthBar.Value = _health;
    }

    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;

        MovingOnPlatform();
        ThrowSnowball();
        AnimationHandler();

        _velocity.Y = 0;
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
        else if (!_isRightColliding || !_isLeftColliding)
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
            snowball.SetOwner(this);
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
            _moveTimer.Stop();
            _throwTimer.Start();
        }
    }

    private void _on_LeftVision_body_exited(Node2D body)
    {
        if (body is Santa player )
        {
            _isAttacking = false;
            _canThrow = false;
            _canMove = false;
            _moveTimer.Start();
        }
    }

    private void _on_RightVision_body_entered(Node2D body)
    {
        if (body is Santa player)
        {
            _isAttacking = true;
            _canThrow = false;
            _moveTimer.Stop();
            _throwTimer.Start();
        }
    }

    private void _on_RightVision_body_exited(Node2D body)
    {

        if (body is Santa player)
        {
            _isAttacking = false;
            _canThrow = false;
            _canMove = false;
            _moveTimer.Start();
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
        _healthBar.Value = _health;
        if (_health <= 0)
        {
            Coin coin = (Coin)GD.Load<PackedScene>("res://Scenes/coin.tscn").Instantiate();
            GetTree().Root.AddChild(coin);
            coin.Position = Position;
            QueueFree();
        }
    }
    
    private void _on_LeftArea2D_body_entered(Node body)
    {
        if (body is TileMapLayer)
        {
            _isLeftColliding = true;
        }
        if (body is Box)
        {
            _isMovingRight = !_isMovingRight;
        }
    }
    
    private void _on_LeftArea2D_body_exited(Node body)
    {
        if (body is TileMapLayer)
        {
            _isLeftColliding = false;
        }
    }
    
    private void _on_RightArea2D_body_entered(Node body)
    {
        if (body is TileMapLayer)
        {
            _isRightColliding = true;
        }
        if (body is Box)
        {
            _isMovingRight = !_isMovingRight;
        }
    }
    
    private void _on_RightArea2D_body_exited(Node body)
    {
        if (body is TileMapLayer)
        {
            _isRightColliding = false;
        }
    }

    private void _on_HeadArea2D_body_entered(Node body)
    {
        // Jak spadnie box na głowę elfa to umiera
        if (body is Box)
        {
            QueueFree();
        }
    }
}