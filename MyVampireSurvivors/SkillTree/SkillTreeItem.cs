using Godot;
using System;

public partial class SkillTreeItem : TextureButton
{
	//fix this with list iterating through skills database to create list of all skill types on ready
	/*enum SkillType{
		Attractor,
		IceSpear,
		MagicCircle
	}*/

	//[Export] SkillType skill;
	public string skill = "";
	public bool skillUnlocked = false; //skill CAN be activated/ is in unlock range
	private bool skillActivated = false; //Skill got activated
	private int skillLevel = 0;
	private bool skillMaxxed = false;

	//private Texture2D icon;
	private PackedScene skillSpawnerScene;
	
	private Player player;
	private SkillTree skillTree;
	private Node2D skillsNode;
	private TextureRect icon;
	private bool mouseOver;

	private SkillSpawn instantiatedSkill;
	private SkillInfoPopUp skillInfoPopUp;
	private PackedScene skillStatusChangedPopup;

	[Signal] public delegate void SkillUnlockedEventHandler();

	public override void _Ready()
	{
		player = (Player) GetTree().GetFirstNodeInGroup("player");
		skillTree = (SkillTree) GetTree().GetFirstNodeInGroup("skilltree");
		skillsNode = player.GetNode<Node2D>("Skills");
		icon = GetNode<TextureRect>("TextureRect");
		skillSpawnerScene = GD.Load<PackedScene>("res://SkillTree/Skills/skill_spawn.tscn");
		skillInfoPopUp = GetNode<SkillInfoPopUp>("SkillInfoPopUp");
		skillStatusChangedPopup = GD.Load<PackedScene>("res://SkillTree/skill_status_changed_popup.tscn");

		string iconPath = (String) skillTree.skills[skill]["IconPath"];
		GD.Print("iconPath: " + skillTree.skills[skill]["IconPath"]);
		GD.Print("skill.ToString() + (skillLevel + 1): " + skill);
		icon.Texture = GD.Load<Texture2D>(iconPath.ToString());

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		SkillUnlocked += OnSkillUnlocked;

		var currentSkill = skillTree.skills[skill];
		skillInfoPopUp.SetProperties(iconPath, (string) currentSkill["Name"], (string) currentSkill["Description"], skillLevel);

		GD.Print(skill + " position = " + GlobalPosition);
	}

	public override void _Process(double delta)
	{
		if(mouseOver && skillUnlocked){
			skillInfoPopUp.Visible = true;
			skillInfoPopUp.ZIndex = 1;

			if(Input.IsActionJustPressed("click") && skillUnlocked &&!skillMaxxed){
				GD.Print("skillUnlocked" + skillActivated + ", skillMaxxed" + skillMaxxed);
				UpgradeSkill();
			}
		} else {
			skillInfoPopUp.Visible = false;
			skillInfoPopUp.ZIndex = 0;
		}
	}

	private void OnMouseEntered(){
		if(!mouseOver){
			Tween tween = CreateTween();
			tween.TweenProperty(this, "scale", new Vector2(Scale.X + Scale.X/10, Scale.Y + Scale.Y/10), 0.5).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);
			mouseOver = true;
		}
	}

	private void OnMouseExited(){
		if(mouseOver){
			Tween tween = CreateTween();
			tween.TweenProperty(this, "scale", new Vector2(5,5), 0.5).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);
			mouseOver = false;
		}
	}

	public void UpgradeSkill(){
		if(skillTree.skills.ContainsKey(skill + (skillLevel + 1))){
			skillLevel += 1;
			var currentSkillif = skillTree.skills[skill + skillLevel];
			skillInfoPopUp.SetProperties((string) currentSkillif["IconPath"], (string) currentSkillif["Name"], (string) currentSkillif["Description"], skillLevel);
			GD.Print(skill + " is now level: " + skillLevel);
		}

		var currentSkill = skillTree.skills[skill + skillLevel];

		switch((bool) currentSkill["IsPassiveSkill"]){
			case false: //Active SKill
				if(!skillActivated){
				instantiatedSkill = (SkillSpawn)skillSpawnerScene.Instantiate();
				skillActivated = true;
				}

				instantiatedSkill.Name = (string)currentSkill["Name"];
				instantiatedSkill.SkillScenePath = (string)currentSkill["ScenePath"];

				instantiatedSkill.Damage = (int)currentSkill["Damage"];
				instantiatedSkill.Knockback = (int) currentSkill["Knockback"];
				instantiatedSkill.AttackSize = (float) currentSkill["AttackSize"];
				instantiatedSkill.Speed = (int) currentSkill["Speed"];
				instantiatedSkill.Hp = (int) currentSkill["Hp"];

				instantiatedSkill.BaseAmmo = (int)currentSkill["Charges"];
				instantiatedSkill.CoolDownTime = (float)currentSkill["CooldownTime"];
				instantiatedSkill.AmmoCooldownTime = (float)currentSkill["AmmoCooldownTime"];

				if (skillLevel == 1) skillsNode.AddChild(instantiatedSkill);
				instantiatedSkill.UpdateStats();

				break;

			case true: //Passive Skill
				if(!skillActivated) skillActivated = true;
				player.IncreaseStat((string) currentSkill["Stat"], (int) currentSkill["Amount"]);
				break;
		}

		if(!skillTree.skills.ContainsKey(skill + (skillLevel + 1))){
			skillMaxxed = true;
			GD.Print(currentSkill["Name"] + " is maxxed out");
		}

		SpawnSkillLeveledUpPopup();
		player.ShowSkillTree(false);
	}

	private void OnSkillUnlocked(){
		GD.Print("skillUnlocked = " + skillUnlocked);
		Modulate = new Color(1,1,1,1);
		skillUnlocked = true;
		skillTree.LockedSkills.Remove(this);
		skillTree.UnlockedSkills.Add(this);
		ZIndex = 2;

		SkillStatusChangedPopup newSkillUnlockedPopUp = (SkillStatusChangedPopup ) skillStatusChangedPopup.Instantiate();
		//newSkillUnlockedPopUp.Position = new Vector2(0,200);
		player.playerGUI.AddChild(newSkillUnlockedPopUp);
		newSkillUnlockedPopUp.SetProperties((string) skillTree.skills[skill]["Name"], (string) skillTree.skills[skill]["IconPath"], "New Skill Unlocked!");
	}

	private void SpawnSkillLeveledUpPopup(){
		SkillStatusChangedPopup skillLeveledUpPopUp = (SkillStatusChangedPopup ) skillStatusChangedPopup.Instantiate();
		//skillLeveledUpPopUp.Position = new Vector2(0,200 - (int) (skillTree.numberOfSkillPopUpsActive * 52));
		player.playerGUI.AddChild(skillLeveledUpPopUp);
		skillLeveledUpPopUp.SetProperties((string) skillTree.skills[skill]["Name"], (string) skillTree.skills[skill]["IconPath"], "Skill Leveled up to Level: " + skillLevel);
	}
}
