using Godot;
using System;

public partial class Door : CharacterBody2D
{
    private void onBodyEntered(Node body)
    {
        if (body is Santa santa)
        {
            if (santa.HasKey)
            {
                QueueFree();
            }
        }
    }
}
