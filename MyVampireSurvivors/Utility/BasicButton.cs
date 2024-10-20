using Godot;
using System;

public partial class BasicButton : Button
{
	AudioStreamPlayer snd_hover;
	AudioStreamPlayer snd_click;

	[Signal] public delegate void ClickEndEventHandler();
	
	public override void _Ready()
	{	
		snd_hover = GetNode<AudioStreamPlayer>("snd_hover");
		snd_click = GetNode<AudioStreamPlayer>("snd_click");

		MouseEntered += OnMouseEntered;
		Pressed += OnPressed;
		snd_click.Finished += OnSoundClickFinished;

	}

	private void OnMouseEntered(){
		snd_hover.Play();
	}

	private void OnPressed(){
		snd_click.Play();
	}

	public void OnSoundClickFinished(){
		EmitSignal("ClickEnd");
	}
}
