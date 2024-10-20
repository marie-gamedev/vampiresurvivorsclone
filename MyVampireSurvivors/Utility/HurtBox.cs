using Godot;
using System;

//get hurt
public partial class HurtBox : Area2D
{
	enum HurtBoxType{
		Cooldown,
		HitOnce,
		DisableHitbox
	}

	[Export] private HurtBoxType hurtBoxType = HurtBoxType.Cooldown;

	private CollisionShape2D collision;
	private Timer disableTimer;
	private PackedScene dmgPopup;
	private Node2D dmgPopUpsNodeHolder;
	private Player player;

	[Signal] public delegate void HurtEventHandler(int damage);

	public override void _Ready()
	{
		collision = GetNode<CollisionShape2D>("CollisionShape2D");
		disableTimer = GetNode<Timer>("DisableTimer");
		dmgPopup = GD.Load<PackedScene>("res://Utility/dmg_popup.tscn");
		dmgPopUpsNodeHolder = (Node2D) GetTree().GetFirstNodeInGroup("dmgpopups");
		player = (Player) GetTree().GetFirstNodeInGroup("player");

		AreaEntered += OnHurtAreaEntered;
		disableTimer.Timeout += OnDisableTimerTimeout;
	}

	public override void _Process(double delta)
	{
	}

	private void OnHurtAreaEntered(Area2D area){
		if(!area.IsInGroup("attack")){
			return;
		}
		if(area.Get("Damage").Equals(null)){
			return;
		}
		switch(hurtBoxType){
			case HurtBoxType.Cooldown:
				collision.CallDeferred("set", "disabled", true);
				disableTimer.Start();
				break;
			case HurtBoxType.HitOnce:
				break;
			case HurtBoxType.DisableHitbox:
				break;

		}

		Random r = new Random();
		int dmg = (int) area.Get("Damage");
		int randomisedDmg = r.Next((int)(dmg * 0.8f), (int) (dmg * 1.2f));

		if(!GetParent().IsInGroup("player")){
			double rD = r.NextDouble();
			bool isCrit;

			if(rD < player.critChance){
				randomisedDmg = (int) (randomisedDmg * player.critDmgMulitplier);
				isCrit = true;
			} else isCrit = false;

			SpawnNewDmgPopup(this, randomisedDmg, isCrit);
		}
		EmitSignal("Hurt", randomisedDmg);

		if(area.HasMethod("EnemyHit")){
			Attack areaAttackScript = (Attack) area;
			areaAttackScript.EnemyHit(1);
		}
	}

	private void OnDisableTimerTimeout(){
		collision.CallDeferred("set", "disabled", false);
	}

	private void SpawnNewDmgPopup(Area2D hurtBoxArea, int damage, bool isCrit){
		DmgPopup newDmgPopup = (DmgPopup) dmgPopup.Instantiate();
		newDmgPopup.Position = hurtBoxArea.GlobalPosition;
		newDmgPopup.Damage = damage;
		newDmgPopup.isCrit = isCrit;
		dmgPopUpsNodeHolder.AddChild(newDmgPopup);
	}
}
