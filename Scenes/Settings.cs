using Godot;
using System;

public partial class Settings : CanvasLayer
{
    private void OnBackButtonPressed()
    {
        var musicPlayer = (MusicPlayer)GetNode("/root/MusicPlayer");
        musicPlayer.SaveSettings();
        GetTree().ChangeSceneToFile("res://Scenes/StartMenu.tscn");
    }
}
