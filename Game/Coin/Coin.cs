using Godot;
using System;

public partial class Coin : Node2D
{

    [Export] Label label;
    private int _coins = 0;
    private void OnBodyEntered(Node body)
    {
        
        if (body is Santa)
        {
            _coins = label.Text.ToInt();
            _coins++;
            label.Text = _coins.ToString();
            QueueFree();
        }
    }
}
