using Godot;
using System;

public partial class DmgPopup : Label
{
	public int Damage;
	public bool isCrit;
	private Timer timer;
	private int timeToDissapear = 3;


	public override void _Ready()
	{
		Text = Damage.ToString();
		GD.Print(Damage.ToString());
		timer = GetNode<Timer>("Timer");

		timer.WaitTime = timeToDissapear;
		timer.Timeout += onTimerTimeout;

		if(isCrit) Modulate = new Color(1,0,0,1);

		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", new Vector2(1, 1), timeToDissapear/5f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		//tween.SetParallel();
		//tween.TweenProperty(this, "position", new Vector2(Position.X, Position.Y - 10f), timeToDissapear/3f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(this, "scale", new Vector2(0.2f, 0.2f), timeToDissapear/5f * 4f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
		tween.SetParallel();
		tween.TweenProperty(this, "modulate", new Color(1,1,1,0), timeToDissapear).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
	}

	private void onTimerTimeout(){
		QueueFree();
	}
}
