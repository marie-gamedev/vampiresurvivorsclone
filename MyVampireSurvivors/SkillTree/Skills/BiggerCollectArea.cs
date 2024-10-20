using Godot;
using System;

public partial class BiggerCollectArea : Attack
{
	CollisionShape2D col;

	public override void _Ready()
	{
		base._Ready();
		//Name = "BiggerCollectArea";
		col = player.grabArea.GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _Process(double delta)
	{
	}

	public override void UpdateSkill(){
		//GD.Print("skillspecificstat " + skillSpecificStat);
		GD.Print("current radius is: " + (float) col.Shape.Get("radius"));
		//GD.Print("adjusting grab circle radius to" + ((int) col.Shape.Get("radius") + (int) skillSpecificStat));

		//col.Shape.Set("radius", (int) col.Shape.Get("radius") + (int) skillSpecificStat);
	}
}
