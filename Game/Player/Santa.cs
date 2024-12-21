using Godot;
using System;

public partial class Santa : CharacterBody2D
{
    private Vector2 _velocity = Vector2.Zero;
    [Export] AnimatedSprite2D _animatedSprite;
    
    
    // Zmienne do poruszania się
    [Export] public float Speed = 5000f;
    [Export] public float Gravity = 1000f;
    [Export] public float JumpForce = 300f;
    [Export] public float Friction = 800f;
    public bool CanMove = false;
    
    // Zmienna do rzucania śnieżką
    bool _canThrow = true;

    
    // Zmienna do drabiny
    [Export] RayCast2D _rayCast;
    bool _isOnLadder = false;
    
    // Wykrywanie lodu
    [Export] RayCast2D _iceRayCast;
    bool _isOnIce = false;
    
    // Życie 
    [Export] public int Health = 3;
    [Export] GameManager _gameManager;
    bool _isHitted = false;
     
    // Poruszanie boxem
    bool _isPushed = false;

    // klucz
    public bool HasKey = false;
    
    public override void _PhysicsProcess(double delta)
    {
        
        if (!CanMove) return;
        _velocity = Velocity;
        
        IceDetect();
        
        // Raycast sprawdzający czy mikołaj jest na drabinie, jeśli tak to porusza się w górę i w dół (:
        // Note : Raycast to jest takie coś jak promień, który sprawdza czy jest kolizja z jakimś obiektem 
        
        
        // Dla Krologa : to nie chatgpt mi to zrobilo, obejżałem filmik na yt i zrobiłem to sam :D 
        // https://www.youtube.com/watch?v=8F0jrZE-nEI
        
        _isOnLadder = _rayCast.IsColliding() && _rayCast.GetCollider() is TileMapLayer;
        
        if (_isOnLadder)
        {
            ApplyLadderMovement(delta);
        }
        else
        {
            // Grawitacja
            if (!IsOnFloor())
            {
                ApplyGravity(delta);
            }
        
            // Skok
            if (Input.IsActionPressed("jump") && IsOnFloor())
            {
                ApplyJump();
            }
        }
        
        // Poruszanie się
        float inputDirection = Input.GetAxis("move_left", "move_right");
        ApplyMovement(inputDirection, delta);

        
        // Rzucanie śnieżką
        if (Input.IsActionJustPressed("throw") && _canThrow && !_isHitted)
        {
            // Ogólnie to działa to tak że jeśli naciśniemy przycisk throw, to zmienia się animacja na throw
            // i po zakończeniu animacji wywołuje się metoda OnAnimationFinished która tworzy śnieżkę i zmienia _canThrow na true
            _canThrow = false;
            _animatedSprite.Play("throw");
        }
        
        UpdateAnimation();
        
        
        Velocity = _velocity;
        MoveAndSlide();
        
        
    }

    private void IceDetect()
    {
        _isOnIce = _iceRayCast.IsColliding() && _iceRayCast.GetCollider() is TileMapLayer;

        if (_isOnIce)
        {
            Speed = 7500;
            Friction = 100f;
        }
        else
        {
            Speed = 5000;
            Friction = 800f;
        }
    }

    private void ThrowSnowball()
    {

        Snowball snowball = (Snowball)GD.Load<PackedScene>("res://Scenes/Snowball.tscn").Instantiate();
        GetTree().Root.AddChild(snowball);
        snowball.Position = Position;
        // Zmiana kierunku śnieżki na kierunek, w którym patrzy mikołaj. 
        if (_animatedSprite.FlipH) snowball.Speed = -snowball.Speed;
        snowball.SetOwner(this); // Ustawianie własciciela na siebie, by nie dostać obrażeń bo śniezka respi się w mikolaju i od razu go trafia.
        _canThrow = true;
    }
    
    private void OnAnimationFinished()
    {
        if (_animatedSprite.Animation == "throw")
        {
            ThrowSnowball();
        }
        if (_animatedSprite.Animation == "hit")
        {
            _isHitted = false;
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
        // musi byc tutaj bo inaczej jak rzucasz śnieżką to nie da sie zmienic kierunku
        if (_velocity.X != 0)
        {
            _animatedSprite.FlipH = _velocity.X < 0;
        }

        if (!_canThrow) return;
        if (_isHitted) return;

        if (!IsOnFloor() && !_isOnLadder)
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
            if (_isPushed)
            {
                _animatedSprite.Play("push");
            }
            else if (_velocity.X != 0)
            {
                _animatedSprite.Play("run");
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
    
    public void TakeDamage()
    {
        Health--;
        if (Health <= 0)
        {
            Die();
            return;
        }
        _gameManager.UpdateHearts(Health);
        _isHitted = true;
        _canThrow = true; // bez tego występował błąd, że jak mikolaj dostanie obrażenia to nie może rzucać śnieżkami, i nie zmienia się animacja.
        _animatedSprite.Play("hit");
        
    }

    public void Die()
    {
        Health = 0;
        _gameManager.UpdateHearts(Health);
        
        // wyłączenie wykrywania kolizji
        foreach (Node2D child in GetChildren())
        {
            if (child is CollisionShape2D collisionShape)
        {
            collisionShape.CallDeferred("set_disabled", true);
        }
        }
        Visible = false;
        _gameManager.GameOver();
    }
    
    public void EnterBox()
    {
        _isPushed = true;
    }
    
    public void ExitBox()
    {
        _isPushed = false;
    }
    
}