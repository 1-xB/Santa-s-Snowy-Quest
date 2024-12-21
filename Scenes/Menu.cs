using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Export] private PackedScene _levelMenuScene;
    [Export] private PackedScene _settingsScene;
    [Export] private PackedScene _creditsScene;
    
    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToPacked(_levelMenuScene);
    }
    
    private void OnSettingsButtonPressed()
    {
        GetTree().ChangeSceneToPacked(_settingsScene);
    }
    
    private void OnCreditsButtonPressed()
    {
        GetTree().ChangeSceneToPacked(_creditsScene);
    }
    
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
    
}
