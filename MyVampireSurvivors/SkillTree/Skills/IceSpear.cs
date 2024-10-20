using Godot;
using System;

public partial class IceSpear : Attack
{
	public override void _Ready()
	{
		base._Ready();

		angle = GlobalPosition.DirectionTo(target);
		Rotation = angle.Angle() + ((float) (Math.PI / 180) * 135);

		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", new Vector2(1,1) * AttackSize, 1).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);
	}

	public override void _Process(double delta)
	{
		Position += angle * Speed * (float) delta;
	}
}
