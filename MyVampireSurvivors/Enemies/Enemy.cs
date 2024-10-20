using Godot;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class Enemy : CharacterBody2D
{
	[Export] int experience = 10;
	[Export] int hp = 40;
	[Export] int damage = 1; //damage the enemy deals
	[Export] protected float baseSpeed = 20;

	protected float speed;
	public const float JumpVelocity = -400.0f;


	private CharacterBody2D player;

	protected AnimationPlayer animPlayer;
	private Sprite2D sprite;
	private Area2D hurtBoxArea;
	private HurtBox hurtBox;

	private PackedScene expGem;
	private Node lootNode;
	private bool facingLeft;
	private Tween hurtTween;

	//[Signal] public delegate void RemoveFromArrayEventHandler(Enemy enemy);

	public override void _Ready()
	{
		player = (CharacterBody2D) GetTree().GetFirstNodeInGroup("player");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<Sprite2D>("Sprite2D");
		hurtBoxArea = GetNode<Area2D>("HurtBox");
		hurtBox = (HurtBox)hurtBoxArea;
		expGem = GD.Load<PackedScene>("res://Objects/experience_gem.tscn");
		lootNode = GetTree().GetFirstNodeInGroup("loot");

		animPlayer.Play("walk");

		hurtBox.Hurt += OnHurtBoxHurt;

		facingLeft = sprite.FlipH;
		speed = baseSpeed;

		
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveToPlayer(speed);
	}

	protected void MoveToPlayer(float speed){
		Godot.Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition);
		Velocity = direction * speed;
		MoveAndSlide();

		//Animation
		if(direction.X > 0.1){
			sprite.FlipH = !facingLeft;
		} else if(direction.X < -0.1){
			sprite.FlipH = facingLeft;
		}
	}

	protected void death(){
		//EmitSignal(SignalName.RemoveFromArray, this);
		ExperienceGem newGem = (ExperienceGem) expGem.Instantiate();
		newGem.GlobalPosition = GlobalPosition;
		newGem.experience = experience;
		lootNode.CallDeferred("add_child",newGem);
		CallDeferred("queue_free");
	}

	protected void OnHurtBoxHurt(int damage){
		hp -= damage;
		Tween hurtTween = CreateTween();
		hurtTween.TweenProperty(sprite, "modulate", new Color (1,0,0,1), 0.15f);
		hurtTween.TweenProperty(sprite, "modulate", new Color (1,1,1,1), 0.15f);
		//GD.Print("enemy took dmg: " + damage + "and has now hp left: " + hp);
		if(hp <= 0){
			death();
		}
	}
}
