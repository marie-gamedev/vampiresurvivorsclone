using Godot;
using System;

public partial class Cacodaemon : Enemy
{
	private Timer chargeTimer;
	private bool charging;

	public override void _Ready(){
		base._Ready();

		chargeTimer = GetNode<Timer>("ChargeTimer");

		chargeTimer.Timeout += OnChargeTimerTimeout;
		animPlayer.AnimationFinished += OnChargeAnimationFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveToPlayer(speed);
	}

	private void OnChargeTimerTimeout(){
		charging = true;
		speed = 400;
		animPlayer.Play("charge");
	}

	private void OnChargeAnimationFinished(StringName animName){
		if(animName == "charge"){
			charging = false;
			speed = baseSpeed;
			chargeTimer.Start();
			animPlayer.Play("walk");
		}
	}
}
