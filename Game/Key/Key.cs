using Godot;
using System;

public partial class Key : Area2D
{
    private void onBodyEntered(Node body)
    {
        if (body is Santa santa)
        {
            santa.HasKey = true;
            QueueFree();
        }
    }
}
