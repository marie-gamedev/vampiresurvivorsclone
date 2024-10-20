using Godot;
using System;

public partial class Menu : Control
{
	private String levelPath = "res://World/world.tscn";
	private BasicButton playButton;
	private BasicButton exitButton;

	public override void _Ready()
	{
		playButton = GetNode<BasicButton>("btn_play");
		exitButton = GetNode<BasicButton>("btn_exit");

		playButton.ClickEnd += OnPlayButtonClickEnd;
		exitButton.ClickEnd += OnExitButtonClickEnd;
	}

	private void OnPlayButtonClickEnd(){
		GetTree().ChangeSceneToFile(levelPath);
	}

	private void OnExitButtonClickEnd(){
		GetTree().Quit();
	}
}
