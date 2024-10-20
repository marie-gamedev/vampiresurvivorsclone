using Godot;
using System;

public partial class Debug : Panel
{
	private BasicButton ExpCollectableButton;

	//Disable Exp
	private Area2D grabArea;
	private Area2D collectArea;
	private Player player;
	private SkillTree skillTree;

	public override void _Ready()
	{
		ExpCollectableButton = GetNode<BasicButton>("dis_exp_btn");
		ExpCollectableButton.ClickEnd += disableExp;
		player = (Player) GetTree().GetFirstNodeInGroup("player");
		skillTree = (SkillTree) GetTree().GetFirstNodeInGroup("skilltree");

		grabArea = GetTree().GetFirstNodeInGroup("player").GetNode<Area2D>("GrabArea");
		collectArea = GetTree().GetFirstNodeInGroup("player").GetNode<Area2D>("CollectArea");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("debug"))
		{
			GD.Print("meow?");
			if(Visible){
				Visible = false;
				Input.MouseMode = Input.MouseModeEnum.Hidden;
			} 
			else if(!Visible){
				Visible = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
			} 
		}

		if(Input.IsActionJustPressed("openMap")){
			if(!skillTree.Visible){
				player.ShowSkillTree(true);
			} else if(skillTree.Visible){
				player.ShowSkillTree(false);	
			} 
		}
	}

	private void disableExp(){
		grabArea.SetCollisionMaskValue(4, !grabArea.GetCollisionMaskValue(4));
		collectArea.SetCollisionMaskValue(4, !collectArea.GetCollisionMaskValue(4));

		if(grabArea.GetCollisionMaskValue(4)){
			ExpCollectableButton.Text = "Disable Exp";
		} else {
			ExpCollectableButton.Text = "Enable Exp";
		}
	}
}
