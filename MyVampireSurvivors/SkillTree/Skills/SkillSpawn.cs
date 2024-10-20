using Godot;
using System;

public partial class SkillSpawn : Node2D
{
	private Timer attackCooldownTimer;
	private Timer ammoCooldownTimer;
	private Node player = null;
	private Player playerScript = null;
	private PackedScene skill = null;
	
	public String SkillScenePath;

	public int Damage;
	public int Knockback;
	public float AttackSize;
	public int Speed;
	public int Hp;
	public float CoolDownTime;
	public float AmmoCooldownTime;
	public int BaseAmmo = 1;

	private int ammo = 0;
	private int additionalAttacks = 0;

	//int level = 0;

	Attack spawnSkillScene;

	public override void _Ready()
	{
		attackCooldownTimer = GetNode<Timer>("%AttackCooldown");
		ammoCooldownTimer = GetNode<Timer>("%AmmunitionCooldown");
		player = GetTree().GetFirstNodeInGroup("player");
		playerScript = (Player) player;
		skill = GD.Load<PackedScene>(SkillScenePath);

		SpawnNewSkill();

		attackCooldownTimer.Timeout += OnAttackCooldownTimerTimeoutSignal;
		ammoCooldownTimer.Timeout += OnAmmoTimerTimeoutSignal;
	}

	public override void _Process(double delta)
	{
	}

	private void OnAttackCooldownTimerTimeoutSignal()
	{
		//only attacks when enemies close, is that actually wanted?
		if(playerScript.enemysClose.Count > 0){
			ammo = BaseAmmo + additionalAttacks;
			ammoCooldownTimer.Start();
		}
	}

	private void OnAmmoTimerTimeoutSignal()
	{
		if(ammo > 0){
			SpawnNewSkill();

			ammo -= 1;
			if(ammo > 0){
				ammoCooldownTimer.Start();
			} else {
				ammoCooldownTimer.Stop();
			}
		}
	}

	private void SpawnNewSkill(){
			Attack newSkill = (Attack) skill.Instantiate();

			newSkill.Position = playerScript.Position;
			newSkill.target = playerScript.GetClosestTarget();
			newSkill.Damage = Damage;
			newSkill.AttackSize = AttackSize;
			newSkill.Hp = Hp;
			newSkill.Speed = Speed;
			newSkill.Knockback = Knockback;

			AddChild(newSkill);
	}
	
	//hopefully fixxed now: cooldown change times also wont work like this :( e.g. shorter magic circle, make it so its updating every time the skill gets leveled up
	public void UpdateStats(){
		attackCooldownTimer.WaitTime = CoolDownTime;
		ammoCooldownTimer.WaitTime = AmmoCooldownTime;
	}
}
