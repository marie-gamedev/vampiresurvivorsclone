using Godot;
using System;

public partial class Garlic : Attack
{
	public override void _Ready()
	{
		base._Ready();

		//GetParent().RemoveChild(this);
		//GetTree().GetFirstNodeInGroup("player").AddChild(this);

	}

	public override void _Process(double delta)
	{
	}
}
