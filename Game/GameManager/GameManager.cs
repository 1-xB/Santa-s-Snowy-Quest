using Godot;
using System;

public partial class GameManager : Node
{
    [Export] private HBoxContainer _livesContainer;
    [Export] CanvasLayer _pauseMenu;
    
    [Export] AnimationPlayer _animationPlayer;
    [Export] Label _coinLabel;

    private int _coins = 0;
    private string _saveSavePath = "user://data.cfg";
    private ConfigFile _configFile = new ConfigFile();

    public override void _Ready()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        
        LoadCoins();
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

    public void SaveCoins()
    {
        // https://www.youtube.com/watch?v=tfqJjDw0o7Y
        _coins = _coinLabel.Text.ToInt();
        _configFile.SetValue("player", "coins", _coins);
        Error error = _configFile.Save(_saveSavePath);
        if (error != Error.Ok)
        {
            GD.Print("Error saving file");
        }
        else 
        {
            GD.Print("File saved");
        }
    }

    public void LoadCoins()
    {
        // https://www.youtube.com/watch?v=tfqJjDw0o7Y
        Error error = _configFile.Load(_saveSavePath);
        if (error == Error.Ok)
        {
            _coins = (int)_configFile.GetValue("player", "coins");
            _coinLabel.Text = _coins.ToString();
        }
        else
        {
            _coins = 0;
            _coinLabel.Text = _coins.ToString();
            GD.Print("Error loading file");
        }
        
    }

    public void Victory()
    {
        SaveCoins();
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
