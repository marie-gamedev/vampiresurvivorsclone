using Godot;
using System;

public partial class GameTimer : Node
{
	private Label timerlabel;
	public double passedTime;
	public double remainingTime;
	[Export] double Timer;
	
	public override void _Ready()
	{
		timerlabel = GetNode<Label>("../GUILayer/GUI/lblTimer");
		timerlabel.Text = "meow";
	}

	public override void _Process(double delta)
	{
		passedTime += delta;
		remainingTime = Timer - passedTime;

		int get_m = (int) (remainingTime/60.0);
		int get_s = (int) (remainingTime % 60);

		String minutesString = "";
		String secondsString = "";
		if(get_m < 10){
			minutesString = "0" + get_m;
		} else {
			minutesString = get_m.ToString();
		}
		if(get_s < 10){
			secondsString = "0" + get_s;
		} else {
			secondsString = get_s.ToString();
		}

		timerlabel.Text = minutesString + ":" + secondsString;
	}
}
