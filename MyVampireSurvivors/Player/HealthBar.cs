using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	private Player player;

	public override void _Ready()
	{
		player = (Player) GetTree().GetFirstNodeInGroup("player");

		SetMaxHp();
	}

	private void SetMaxHp(){
		MaxValue = player.hp;
		Value = player.hp;
	}

	public void AdjustHealthBar(int damage){
		Value = player.hp;
	}
}
