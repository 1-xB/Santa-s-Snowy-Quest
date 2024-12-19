using Godot;
using System;

public partial class Coin : Node2D
{
    private void OnBodyEntered(Node body)
    {
        // TODO : Logika dodania coina
        if (body is Santa)
        {
            QueueFree();
        }
    }
}
