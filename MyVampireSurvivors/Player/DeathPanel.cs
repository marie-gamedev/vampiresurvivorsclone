using Godot;
using System;

public partial class DeathPanel : Panel
{
	private Label resultLabel;
	private AudioStreamPlayer victorySound;
	private AudioStreamPlayer defeatSound;
	private BasicButton menuButton;

	public override void _Ready()
	{
		resultLabel = GetNode<Label>("lbl_Result");
		victorySound = GetNode<AudioStreamPlayer>("snd_victory");
		defeatSound = GetNode<AudioStreamPlayer>("snd_defeat");
		menuButton = GetNode<BasicButton>("btn_menu");

		menuButton.ClickEnd += OnMenuButtonClicked;
	}

	public void DisplayDeathLabel(bool victory){
		if(victory){
			resultLabel.Text = "Congratulations! You won";
			victorySound.Play();
		}else{
			resultLabel.Text = "Defeat! Try again.";
			defeatSound.Play();
		}
	}

	private void OnMenuButtonClicked(){
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://TitleScreen/menu.tscn");
	}
}
