using Godot;
using System;
using System.Numerics;

public partial class ExperienceGem : Area2D
{
	[Export] public int experience  = 1;

	private Texture2D greenGem;
	private Texture2D blueGem;
	private Texture2D redGem;
	private Sprite2D sprite;
	private AudioStreamPlayer sound;
	private CollisionShape2D collision;

	public Node2D target;
	private double speed = -0.7;

	public override void _Ready()
	{
		greenGem = GD.Load<Texture2D>("res://Textures/Items/Gems/Gem_green.png");
		blueGem = GD.Load<Texture2D>("res://Textures/Items/Gems/Gem_blue.png");
		redGem = GD.Load<Texture2D>("res://Textures/Items/Gems/Gem_red.png");
		sprite = GetNode<Sprite2D>("Sprite2D");
		sound = GetNode<AudioStreamPlayer>("snd_collect");
		collision = GetNode<CollisionShape2D>("CollisionShape2D");

		sound.Finished += OnSoundCollectFinished;

		switch(experience){
			case <5:
				break;
			case <25:
				sprite.Texture = blueGem;
				break;
			case >=25:
				sprite.Texture = redGem;
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(target != null){
			GlobalPosition = GlobalPosition.MoveToward(target.GlobalPosition, (float)speed);
			speed +=  3 * delta;

		}
	}

	public int collect(){
		sound.Play();
		collision.CallDeferred("set", "disabled", true);
		sprite.Visible = false;
		return experience;
	}

	private void OnSoundCollectFinished(){
		CallDeferred("queue_free");
	}
}
