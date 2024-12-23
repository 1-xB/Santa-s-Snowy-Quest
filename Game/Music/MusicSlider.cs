using Godot;
using System;

public partial class MusicSlider : HSlider
{
    MusicPlayer musicPlayer;
    bool firstTime = true;
    public override void _Ready()
    {   
        musicPlayer = (MusicPlayer)GetNode("/root/MusicPlayer");

        Value = Mathf.Lerp(0, 100, Mathf.InverseLerp(-40f, 5f, musicPlayer.Volume));
    }

    public void OnValueChanged(float value)
    {
        if (firstTime)
        {
            firstTime = false;
            return;
        }
        if (value == 0)
        {
            musicPlayer.MuteMusic();
            return;
        }
        musicPlayer.SetVolume(value);
    }
}
