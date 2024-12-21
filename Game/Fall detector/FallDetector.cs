using Godot;
using System;

public partial class FallDetector : Area2D
{
    private void OnBodyEntered(Node body)
    {
        
        if (body is Santa)
        {
            Santa player = (Santa)body;
            player.Die();
        }
    }
}
