using Godot;
using System;

public partial class CoinCounter : Control
{


    [Export] private Label _coinLabel;
    private int _coins = 0;
    private string _saveSavePath = "user://data.cfg";
    private ConfigFile _configFile = new ConfigFile();

    public override void _Ready()
    {
        LoadCoins();
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
}
