using Godot;
using System;

public partial class LevelMenu : CanvasLayer
{
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
}
