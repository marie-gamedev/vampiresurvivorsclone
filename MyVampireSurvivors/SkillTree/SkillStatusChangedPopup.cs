using Godot;
using System;

public partial class SkillStatusChangedPopup : ColorRect
{
	private Label name;
	private Label skillEvent;
	private TextureRect icon;
	private Timer timer;
	private SkillTree skillTree;

	public override void _Ready()
	{
		name = GetNode<Label>("lbl_name");
		icon = GetNode<TextureRect>("ColorRect/ItemIcon");
		timer = GetNode<Timer>("Timer");
		skillEvent = GetNode<Label>("lbl_event");
		skillTree = (SkillTree) GetTree().GetFirstNodeInGroup("skilltree");

		Position = new Vector2(0,200 - (int) (skillTree.numberOfSkillPopUpsActive * 52));
		skillTree.numberOfSkillPopUpsActive += 1;
		
		timer.Timeout += OnTimerTimeout;

		Tween tween = CreateTween();
		tween.TweenProperty(this, "modulate", new Color(1,1,1,0), timer.WaitTime).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.In);
	}

	public void SetProperties(string name, string iconPath, string skillEvent){
		this.name.Text = name;
		this.skillEvent.Text = skillEvent;
		icon.Texture = GD.Load<Texture2D>(iconPath.ToString());
	}

	private void OnTimerTimeout(){
		skillTree.numberOfSkillPopUpsActive -= 1;
		QueueFree();
	}
}
