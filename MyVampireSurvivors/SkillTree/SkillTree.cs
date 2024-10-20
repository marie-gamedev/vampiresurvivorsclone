using Godot;
using System;
using System.Collections.Generic;
using System.IO;

using Dictionary = Godot.Collections.Dictionary;

public partial class SkillTree : ColorRect
{
	public Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>> skills;
	public List<SkillTreeItem> LockedSkills = new();
	public List<SkillTreeItem> UnlockedSkills = new();
	private BasicButton backButton;
	private Player player;
	private Camera2D camera;
	private PackedScene skillTreeItem;
	[Export] string startSkill;
	private SkillTreeItem startSkillInstance;
	private Polygon2D hiddenAreaPolygon;
	public int numberOfSkillPopUpsActive;


	//Is called before Ready (needed here since ready function traverses tree down -> up, so ready function in children gets called before ready function in parents, here child is trying to access skills (cant do that if it hasnt been initialized))
	public override void _EnterTree()
	{
		backButton = GetNode<BasicButton>("back_button");
		player = (Player) GetTree().GetFirstNodeInGroup("player");
		backButton.ClickEnd += BackButton;
		camera = (Camera2D) GetTree().GetFirstNodeInGroup("camera");
		skillTreeItem = GD.Load<PackedScene>("res://SkillTree/skill_tree_item.tscn");
		hiddenAreaPolygon = GetNode<Polygon2D>("Polygon2D");

	#region Convert Json File to Dictionary
		//Only works in editor, not exported project!!
		string path = ProjectSettings.GlobalizePath("res://SkillTree/Skills");

		//SaveTexToFile(path, "skillstest.json", json);

		string loadedData = LoadTextFromFile(path, "skills.json");
		Json jsonLoader = new Json();
		Error error = jsonLoader.Parse(loadedData);
		if(error != Error.Ok){
			GD.Print(error);
			return;
		}
		skills = jsonLoader.Data.AsGodotDictionary<string, Godot.Collections.Dictionary<string, Variant>>();
	#endregion

		GenerateRandomSkillTreeItems();
		//GetSkillList();
	}

	public override void _Ready()
	{
		startSkillInstance.EmitSignal("SkillUnlocked");
		startSkillInstance.UpgradeSkill();
	}

	public override void _Process(double delta)
	{
		hiddenAreaPolygon.GlobalPosition = player.GlobalPosition;
	}

	private string LoadTextFromFile(string path, string fileName){
		string data = null;

		path = Path.Join(path, fileName);

		if(!File.Exists(path)){
			return null;
		}

		try
		{
			data = File.ReadAllText(path);
		}
		catch (System.Exception e)
		{
			GD.Print(e);
		}

		return data;
	}

	private void SaveTexToFile(string path, string fileName, string data){
		if(!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}

		path = Path.Join(path, fileName);

		GD.Print(path);

		try
		{
			File.WriteAllText(path, data);
		}
		catch (System.Exception e)
		{
			GD.Print(e);
		}
	}

	private void GetSkillList(){
		var children = GetChildren();
		foreach (var skill in children)
			{
				if(skill is not SkillTreeItem) return;
				SkillTreeItem skillTreeItem = (SkillTreeItem) skill;
				LockedSkills.Add(skillTreeItem);
			}
	}

	private void GenerateRandomSkillTreeItems(){
		int minX = -500;
		int maxX = 500;
		int minY = -500;
		int maxY = 500;
		Random r = new Random();

		foreach (var skill in skills)
		{
			string keyText = skill.Key;

			if(!char.IsDigit(keyText[keyText.Length - 1])){
				var newSkill = (SkillTreeItem) skillTreeItem.Instantiate();
				LockedSkills.Add(newSkill);
				var x = r.Next(minX, maxX);
				var y = r.Next(minY, maxY);

				if(skill.Key == startSkill){
					x = 0;
					y = 0;
					startSkillInstance = newSkill;
				}

				GD.Print("x: " + x + ", y: " + y);
				newSkill.Position = new Vector2(x,y);
				GD.Print("newSkill.Position = " + newSkill.Position);
				newSkill.skill = (string) skill.Key;
				GD.Print("newSkill.skill: (only name without number?)" + newSkill.skill);
				newSkill.Name = (string) skill.Value["Name"];

				AddChild(newSkill);
			}
		}
	}

	private void BackButton(){
		player.ShowSkillTree(false);
	}
}
