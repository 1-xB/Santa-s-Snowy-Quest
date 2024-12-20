using Godot;
using System;

public partial class GameManager : Node
{
    [Export] private HBoxContainer _livesContainer;

    public void UpdateHearts(int lives)
    {
        for (int i = 0; i < _livesContainer.GetChildren().Count; i++)
        {
            if (i < lives)
            {
                _livesContainer.GetChild<Control>(i).Visible = true;
            }
            else
            {
                _livesContainer.GetChild<Control>(i).Visible = false;
            }
        }
    }
    
}
