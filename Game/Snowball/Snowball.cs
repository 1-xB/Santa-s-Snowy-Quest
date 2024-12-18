using Godot;
using System;

public partial class Snowball : Area2D
{
    
    Vector2 _velocity = Vector2.Zero;
    [Export] AnimatedSprite2D _animatedSprite;
    public float Speed = 20000f;
    float _gravity = 200f; 
    bool destroy = false;
    bool first = true;
    

    public override void _PhysicsProcess(double delta)
    {
        
        if (!destroy)
        {
            _velocity = Vector2.Zero;
            
            ApplyGravity(delta);
            ApplyMovement(delta);
        
            Position += _velocity * (float)delta;
        }
        
    }
    
    private void ApplyGravity(double delta)
    {
        // Note : Ta grawitacja nie wygląda na zbyt realistycznie, można troche poprawić.
        _velocity.Y += _gravity * (float)delta;
    }
    
    private void ApplyMovement(double delta)
    {
        _velocity.X += Speed * (float)delta;
    }
    
    private void OnCollisionBodyEntered(Node2D body)
    {
        if (first)
        {
            first = false;
            return;
        }
        if (body is Santa)
        {
            Santa santa = body as Santa;
            santa.TakeDamage();
        }
        else if (body is WrongElf)
        {
            WrongElf elf = body as WrongElf;
            elf.TakeDamage();
        }
        destroy = true;
        _animatedSprite.Play("destroy");
        
    }
    
    private void OnAnimationFinished()
    {
        if (destroy)
        {
            QueueFree();
        }
    }
    

}
