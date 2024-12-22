using Godot;
using System;

public partial class LevelMenu : CanvasLayer
{
    [Export] HBoxContainer _levelsContainer;
    [Export] Label _coinLabel;
    int _coins = 0;

    int _level2Cost = 5;
    int _level3Cost = 10;
    int _level4Cost = 15;
    int _level5Cost = 20;

    public override void _Ready() 
    {
        _coins = _coinLabel.Text.ToInt();
        LoadLevels();
    }

    private void LoadLevels() 
    {
        for (int i = 1; i < _levelsContainer.GetChildren().Count; i++)
        {
            Button button = _levelsContainer.GetChild<Button>(i);
            Control costLabel = button.GetNode<Control>("Price");
            switch (i) 
            {
                case 1:
                    if (_coins >= _level2Cost)
                    {
                        costLabel.Visible = false;
                    }
                    break;
                case 2:
                    if (_coins >= _level3Cost)
                    {
                        costLabel.Visible = false;
                    }
                    break;
                case 3:
                    if (_coins >= _level4Cost)
                    {
                        costLabel.Visible = false;
                    }
                    break;
                case 4:
                    if (_coins >= _level5Cost)
                    {
                        costLabel.Visible = false;
                    }
                    break;
                default:
                    break;
            }

            
        }

    }

    private void On1LevelButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level1.tscn");
    }

    private void On2LevelButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level2.tscn");
    }

    private void On3LevelButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level3.tscn");
    }

    private void On4LevelButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level4.tscn");
    }

    private void On5LevelButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/Level5.tscn");
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/StartMenu.tscn");
    }
}
