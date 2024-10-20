using Godot;
using System;

public partial class SkillsDB : Node
{
	public const String ScenePath = "res://SkillTree/Skills/";
	public Godot.Collections.Dictionary SKILLS = new Godot.Collections.Dictionary {
		{"icespear", new Godot.Collections.Dictionary{
			{"icon", ".png"},
			{"name", "Ice Spear"},
			{"scene", ScenePath + "ice_spear.tscn"}
		}},
		{"attractorb", new Godot.Collections.Dictionary{
			{"icon", "dadwdwa"},
			{"name", "Attract Orb"},
			{"scene", ScenePath + "ice_spear.tscn"}
		}}
	};
}
