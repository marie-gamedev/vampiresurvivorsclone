using Godot;
using System;

public partial class SkillInfoPopUp : ColorRect
{
	private TextureRect icon;
	private Label name;
	private Label description;
	private Label level;

	public override void _Ready()
	{
		icon = GetNode<TextureRect>("ColorRect/ItemIcon");
		name = GetNode<Label>("lbl_name");
		description = GetNode<Label>("lbl_description");
		level = GetNode<Label>("lbl_level");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetProperties(string iconPath, string name, string description, int level){
		icon.Texture = GD.Load<Texture2D>(iconPath.ToString());
		this.name.Text = name;
		this.description.Text = description;
		this.level.Text = "Level: " + level;
	}
}
