using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Export] private PackedScene _levelMenuScene;
    [Export] private PackedScene _settingsScene;

    [Export] ColorRect _credits;
    
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
        _credits.Visible = true;
    }
    
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnBackButtonPressed()
    {
        _credits.Visible = false;
    }
    
}
