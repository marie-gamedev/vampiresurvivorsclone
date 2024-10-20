using Godot;
using System;
using System.Diagnostics;

public partial class Attack : Area2D
{
	
	public Godot.Vector2 target = new Godot.Vector2();
	protected Vector2 angle = new Vector2();

	//private int _count = 0;

	//dont set values here that get changed from different script
	public float AttackSize;
	//dmg gets accessed in hurtbox script to apply damage
	public int Damage;
	public int Hp;
	public int Speed;
	public int Knockback;

	//protected int ammo = 0;

	protected Timer lifetimeTimer;
	protected Player player = null;
	
	//[Signal] public delegate void RemoveFromArrayEventHandler(Enemy enemy);
	
	public override void _Ready()
	{
		player = (Player) GetTree().GetFirstNodeInGroup("player");
		lifetimeTimer = GetNode<Timer>("LifeTimeTimer");

		lifetimeTimer.Timeout += OnLifeTimeTimerTimeoutSignal;

		GD.Print("spawned skill: " + Name);
	}

	public void EnemyHit(int charge = 1){
		Hp -= charge;
		if(Hp <= 0){
			CallDeferred("queue_free");
		}
	}

	private void OnLifeTimeTimerTimeoutSignal()
	{
		//EmitSignal(SignalName.RemoveFromArray, this);
		CallDeferred("queue_free");
	}

	public virtual void UpdateSkill(){}
}
