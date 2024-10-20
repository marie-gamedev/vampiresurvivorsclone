using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	private float speed = 50;
	private float speedSprint = 90;
	private float dashSpeed = 500;
	private bool isDashing;
	private bool canDash = true;
	public int hp = 100;
	private int maxHP = 100;
	
	//Stats
	public float critChance = 0.2f;
	public float critDmgMulitplier = 2f;

	[Export] private HealthBar healthBar;
	[Export] private ExperienceBar expBar;
	private Label lvllbl;
	private AnimationPlayer animPlayer;
	private Sprite2D playerSprite;
	public Area2D grabArea;
	private Area2D collectArea;
	private Timer dashTimer;
	private Timer dashCooldown;
	private Area2D enemeyDetectionArea = null;
	private SkillTree skillTree;
	private DeathPanel deathPanel;
	private GameTimer gameTimer;

	private PackedScene attackTimerNode = null;
	private HurtBox hurtBox;
	private Camera2D camera;
	private Camera2D skillTreeCamera;
	public Control playerGUI;


	public List<Node2D> enemysClose;
	
	private int XpCap;
	private int currentXp;
	private int level = 1;


	public override void _Ready(){
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		playerSprite = GetNode<Sprite2D>("Sprite2D");
		enemeyDetectionArea = GetNode<Area2D>("EnemyDetectionArea");
		//attackTimerNode = GD.Load<PackedScene>("res://Player/Attack/attack_timer.tscn");
		dashTimer = GetNode<Timer>("Dashtimer");
		dashCooldown = GetNode<Timer>("DashCooldown");
		hurtBox = GetNode<HurtBox>("HurtBox");
		grabArea = GetNode<Area2D>("GrabArea");
		collectArea = GetNode<Area2D>("CollectArea");
		lvllbl = expBar.GetNode<Label>("label_lvl");
		skillTree = (SkillTree) GetTree().GetFirstNodeInGroup("skilltree");
		skillTreeCamera = skillTree.GetNode<Camera2D>("Camera2D");
		deathPanel = GetNode<DeathPanel>("GUILayer/GUI/DeathPanel");
		gameTimer = (GameTimer) GetNode<GameTimer>("GameTimer");
		camera = GetNode<Camera2D>("Camera2D");
		playerGUI = GetNode<Control>("GUILayer/GUI");

		enemysClose = new();

		//var newAttack = (Node) attackTimerNode.Instantiate();
		//AddChild(newAttack);

		enemeyDetectionArea.AreaEntered += OnEnemyDetectionAreaEntered;
		enemeyDetectionArea.AreaExited += OnEnemyDetectionAreaExited;
		dashTimer.Timeout += OnDashTimerTimeout;
		dashCooldown.Timeout += OnDashCooldownTimerTimeout;
		hurtBox.Hurt += OnHurtBoxHurt;
		grabArea.AreaEntered += OnGrabAreaEntered;
		collectArea.AreaEntered += OnCollectAreaEntered;

		calculateXpCap();

		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	public override void _Process(double delta)
	{
		foreach (var skill in skillTree.LockedSkills)
		{
			if((GlobalPosition.DistanceTo(skill.GlobalPosition) < 300) && !skill.skillUnlocked){
				GD.Print(skill.Name + "is in Range and unlocked");
				skill.EmitSignal("SkillUnlocked");
				break;
			}
		}

		/*
		var skills = skillTree.GetChildren();
		foreach (var skill in skills)
			{
				if(skill is not SkillTreeItem) return;

				SkillTreeItem skillTreeItem = (SkillTreeItem) skill;

				if((GlobalPosition.DistanceTo(skillTreeItem.GlobalPosition) < 150) && !skillTreeItem.skillUnlocked){
					GD.Print(skill.Name + "is in Range and unlocked");
					skillTreeItem.EmitSignal("SkillUnlocked");
				}
			}*/
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
	}
	
	private void Movement()
	{
		//Dash
		if(canDash && Input.IsActionJustPressed("dash")){
			isDashing = true;
			dashTimer.Start();
			dashCooldown.Start();
			dashAnimation();
			canDash = false;
		}

	
		float xMov = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		float yMov = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		Vector2 movement = new Vector2(xMov, yMov);

		//Animation
		if(movement.X > 0){
			animPlayer.Play("walk_right");
			playerSprite.FlipH = false;
		} else if(movement.X <0){
			animPlayer.Play("walk_right");
			playerSprite.FlipH = true;
		} else if(movement.Y > 0){
			animPlayer.Play("walk_down");
		} else if(movement.Y < 0){
			animPlayer.Play("walk_up");
		} else{
			animPlayer.Play("idle");
		}

		float moveSpeed = 0;

		if(Input.GetActionStrength("sprint") > 0)
		{
			moveSpeed = speedSprint;
		}
		else
		{
			moveSpeed = speed;
		}

		if(isDashing){
			moveSpeed = dashSpeed;
		}
		
		
		Velocity = movement.Normalized() * moveSpeed;
		MoveAndSlide();
	}

	public Vector2 GetClosestTarget(){
		if(enemysClose.Count > 0){
			var closestEnemy = enemysClose[0];
			foreach (var enemy in enemysClose)
			{
				if(enemy.GlobalPosition.DistanceTo(GlobalPosition) < closestEnemy.GlobalPosition.DistanceTo(GlobalPosition)){
					closestEnemy = enemy;
				}
			}
			return closestEnemy.GlobalPosition;
		} else {
			return Vector2.Up;
		}
	}

	public Vector2 GetRandomTarget(){
		if(enemysClose.Count > 0){
			Random rnd = new Random();
			int r = rnd.Next(enemysClose.Count);
			return  enemysClose[r].GlobalPosition;
		} else {
			return Vector2.Up;
		}
	}

	private void OnEnemyDetectionAreaEntered(Node2D enemy){
		if(!enemy.GetParent().IsInGroup("enemy")) return;

		if(!enemysClose.Contains(enemy)){
			enemysClose.Add(enemy);
		}
	}

	private void OnEnemyDetectionAreaExited(Node2D enemy){
		if(!enemy.GetParent().IsInGroup("enemy")) return;

		if(enemysClose.Contains(enemy)){
			enemysClose.Remove(enemy);
		}
	}

	private void OnDashTimerTimeout(){
		isDashing = false;
	}

	private void OnDashCooldownTimerTimeout(){
		canDash = true;
	}

	private void dashAnimation(){
		playerSprite.Modulate = new Color(1,1,1,0.3f);
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(playerSprite, "modulate", new Color (1,1,1,1), 0.3f);
	}

	private void OnHurtBoxHurt(int damage){
		hp -= Math.Clamp(damage, 1, 999);
		healthBar.AdjustHealthBar(damage);
		if(hp <= 0){
			death();
		}
	}

	private void death(){
		GD.Print("Player died");

		deathPanel.Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		//EmitSignal("playerdeath");
		GetTree().Paused = true;

		if(gameTimer.remainingTime <= 0)
		{
			deathPanel.DisplayDeathLabel(true);
		} else if(gameTimer.remainingTime > 0)
		{
			deathPanel.DisplayDeathLabel(false);
		}
	}

	private void OnGrabAreaEntered(Area2D area){
		if(area.IsInGroup("loot")){
			ExperienceGem gem = (ExperienceGem) area;
			gem.target = this;
		}
	}

	private void OnCollectAreaEntered(Area2D area){
		if(area.IsInGroup("loot")){
			ExperienceGem gem = (ExperienceGem) area;
			var gem_exp = gem.collect();
			updateXp(gem.experience);
		}
	}

	private int calculateXpCap(){
		XpCap = (int) Math.Pow(level, 0.8) * 2 + 30;
		expBar.MaxValue = XpCap;
		expBar.Value = currentXp;
		return XpCap;
	}

	private void updateXp(int newxp){
		currentXp += newxp;
		expBar.Value = currentXp;
		if(currentXp > XpCap){
			int restXp = currentXp - XpCap;
			currentXp = 0;
			levelup(restXp);
			
		}
	}

	private void levelup(int restXp){
		level += 1;
		lvllbl.Text = new string("Level: " + level);
		calculateXpCap();
		ShowSkillTree(true);
		updateXp(restXp);
	}

	public void ShowSkillTree(bool show){
		if(show){
			skillTree.Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			playerGUI.Visible = false;
			//skillTreeCamera.GlobalPosition = camera.GlobalPosition;
			skillTreeCamera.MakeCurrent();
			GetTree().Paused = true;
			
		} else if(!show){
			skillTree.Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Hidden;
			playerGUI.Visible = true;
			camera.MakeCurrent();
			GetTree().Paused = false;
		}
	}



#region player related stats increase function
	public void IncreaseStat(string stat, int amount){
		switch(stat){
			case "GrabArea":
				var col = grabArea.GetNode<CollisionShape2D>("CollisionShape2D");
				col.Shape.Set("radius", (int) col.Shape.Get("radius") + amount);
				GD.Print(col.Shape.Get("radius"));
				break;
			case "Hp":
				hp += amount;
				break;
			case "CritChance":
				break;
			case "CritDmg":
				break;
		}

		GD.Print("increased: " + stat + " by:" + amount);
	}
}
#endregion
