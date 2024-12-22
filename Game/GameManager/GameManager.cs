using Godot;
using System;

public partial class GameManager : Node
{
    [Export] private HBoxContainer _livesContainer;
    [Export] CanvasLayer _pauseMenu;
    
    [Export] AnimationPlayer _animationPlayer;
    [Export] CoinCounter _coinCounter;

    private int _coins = 0;
    private string _saveSavePath = "user://data.cfg";
    private ConfigFile _configFile = new ConfigFile();

    public override void _Ready()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("escape"))
        {
            _pauseMenu.Visible = !_pauseMenu.Visible;
            GetTree().Paused = _pauseMenu.Visible;
        }
    }
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

    public void RestartLevel()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    public void GoToMainMenu()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/StartMenu.tscn");
    }

    public void ResumeGame()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
    }

    public void GameOver()
    {
        _animationPlayer.Play("FadeOut");
    }

    

    public void Victory()
    {
        _coinCounter.SaveCoins();
        _animationPlayer.Play("FadeOut");

    }

    private void OnWinAreaBodyEntered(Node body)
    {
        if (body is Santa)
        {
            Victory();
        }
    }   
    
}
