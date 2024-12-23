using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class MusicPlayer : AudioStreamPlayer
{
    private AudioStream music1 = (AudioStream)ResourceLoader.Load("res://Assets/Music/deckthehalls.mp3");
    private AudioStream music2 = (AudioStream)ResourceLoader.Load("res://Assets/Music/goodkingwenceslas.mp3");
    private AudioStream music3 = (AudioStream)ResourceLoader.Load("res://Assets/Music/jinglebells.mp3");
    private AudioStream music4 = (AudioStream)ResourceLoader.Load("res://Assets/Music/ochristmastree.mp3");
    private AudioStream music5 = (AudioStream)ResourceLoader.Load("res://Assets/Music/silentnight.mp3");
    private AudioStream music6 = (AudioStream)ResourceLoader.Load("res://Assets/Music/twelvedaysofchristmas.mp3");
    private AudioStream music7 = (AudioStream)ResourceLoader.Load("res://Assets/Music/wewishyouamerrychristmas.mp3");
    bool muted = false;
    public float Volume = -20f;
    int lastMusic = -80;
    float lastMoment = 0f;

    private string _saveSavePath = "user://data.cfg";
    private ConfigFile _configFile = new ConfigFile();

    // Pomógł mi ten filmik : https://www.youtube.com/watch?v=lILnUD3xph8
    private void PlayMusic()
    {
        Random random = new Random();
        float randomNumber;

        do {
            randomNumber = random.Next(1, 8);
        }
        while (randomNumber == lastMusic);

        switch (randomNumber)
        {
            case 1:
                Stream = music1;
                break;
            case 2:
                Stream = music2;
                break;
            case 3:
                Stream = music3;
                break;
            case 4:
                Stream = music4;
                break;
            case 5:
                Stream = music5;
                break;
            case 6:
                Stream = music6;
                break;
            case 7:
                Stream = music7;
                break;
        }

        lastMusic = (int)randomNumber;
        Play();
    }

    public override void _Ready()
    {
        LoadSettings();
        PlayMusic();
    }

    public override void _Process(double delta)
    {
        if (!IsPlaying() && !muted)
        {
            PlayMusic();
        }
    }

    public void SetVolume(float volume)
    {
        if (!IsPlaying()) {
            muted = false;
            Play(lastMoment);
        }
        // Działa to tak że volume to jest wartość od 0 do 100,
        // a VolumeDb to jest wartość od -80 do 0, ale od -40 prawie nic nie słychać,
        // a od 5 jest już bardzo głośno, więc zrobiłem tak że od 0 do 100 to jest od -40 do 5
        float volumeDb = Mathf.Lerp(-40f, 5f, volume / 100f);
        Volume = volumeDb;
        SetVolumeDb(Volume);
    }

    public void MuteMusic()
    {
        muted = true;
        Volume = -80f;
        lastMoment = GetPlaybackPosition();
        SetVolumeDb(Volume);
        Stop();
    }

    private void LoadSettings()
    {
        Error error = _configFile.Load(_saveSavePath);
        if (error == Error.Ok)
        {
            Volume = (int)_configFile.GetValue("Settings", "volume");
            SetVolumeDb(Volume);
        }
        else
        {
            GD.Print("Error loading file");
        }
    }

    public void SaveSettings()
    {
        Error error = _configFile.Load(_saveSavePath);
        if (error != Error.Ok)
        {
            GD.Print("Error loading file");
        }
        else
        {
            GD.Print("File loaded");
        }
        // Naprawione : Trzeba było załadować kolejny raz plik, bo kod pamieta plik z którego wczytał ostatnio
        _configFile.SetValue("Settings", "volume", Volume);
        Error error2 = _configFile.Save(_saveSavePath);
        if (error != Error.Ok)
        {
            GD.Print("Error saving file");
        }
        else
        {
            GD.Print("File saved");
        }
    }


}
