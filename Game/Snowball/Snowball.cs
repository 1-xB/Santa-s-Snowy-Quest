using Godot;
using System;

public partial class Snowball : Area2D
{
    
    Vector2 _velocity = Vector2.Zero;
    [Export] AnimatedSprite2D _animatedSprite;
    public float Speed = 20000f;
    float _gravity = 200f; 
    bool destroy = false;
    

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
        _velocity.Y += _gravity * (float)delta;
    }
    
    private void ApplyMovement(double delta)
    {
        _velocity.X += Speed * (float)delta;
    }
    
    private void OnCollisionBodyEntered(Node2D body)
    {
        if (body is Santa)
        {
            return;
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
