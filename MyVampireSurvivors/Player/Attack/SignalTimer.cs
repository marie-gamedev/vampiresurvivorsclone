using Godot;
using System;

//Wtf is this class? lool
public partial class SignalTimer : Godot.Timer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timeout += OnTimerTimeoutSignal;
	}
	
	private void OnTimerTimeoutSignal()
	{
		GD.Print("Timer timeout");
	}

}
