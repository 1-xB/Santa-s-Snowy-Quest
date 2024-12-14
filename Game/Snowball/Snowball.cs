using Godot;
using System;

public partial class Snowball : Area2D
{
    
    Vector2 _velocity = Vector2.Zero;
    float _speed = 10000f;
    float _gravity = 200f;
    
    public override void _PhysicsProcess(double delta)
    {
        _velocity = Vector2.Zero;
        
        ApplyGravity(delta);
        ApplyMovement(delta);
        
        Position += _velocity * (float)delta;
        
        
    }
    
    private void ApplyGravity(double delta)
    {
        _velocity.Y += _gravity * (float)delta;
    }
    
    private void ApplyMovement(double delta)
    {
        _velocity.X += _speed * (float)delta;
    }
    
    private void OnCollisionBodyEntered(Node2D body)
    {
        if (body is TileMapLayer)
        {
            QueueFree();
        }
    }
    

}
