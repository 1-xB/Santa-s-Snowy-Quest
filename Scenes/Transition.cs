using Godot;
using System;

public partial class Transition : CanvasLayer
{
    [Export] AnimationPlayer animationPlayer;
    [Export] Santa santa;


    // Poradnik który mi pomógł : https://www.youtube.com/watch?v=yZQStB6nHuI
    public override void _Ready()
    {
        animationPlayer.Play("FadeIn");
    }

    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        if (anim_name == "FadeIn")
        {
            santa.CanMove = true;   
        }
        else if (anim_name == "FadeOut")
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
