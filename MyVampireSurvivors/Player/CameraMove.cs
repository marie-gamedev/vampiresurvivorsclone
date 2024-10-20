using Godot;
using System;

public partial class CameraMove : Camera2D
{
	private float minZoom = 0.05f;
	private float maxZoom = 0.5f;
	private float zoomSpeed = 1;
	private float zoomFactor = 0.1f;
	private bool zoomingIn = false;
	private bool zoomingOut = false;

	public override void _Ready()
	{
	
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ZoomIn")){
			zoomingIn = true;
		} else if(Input.IsActionJustPressed("ZoomOut")){
			zoomingOut = true;
		}

		if(Input.IsActionPressed("MouseWheelClicked")){
			var oldMousePosition =  DisplayServer.MouseGetPosition();
			Position += (oldMousePosition - DisplayServer.MouseGetPosition()) * 25;
		}

		if (zoomingIn){
			var zoomXLerp = Mathf.Lerp(Zoom.X, Zoom.X + zoomFactor, zoomSpeed);
			var zoomYLerp = Mathf.Lerp(Zoom.Y, Zoom.Y + zoomFactor, zoomSpeed);

			Zoom = new Vector2(Math.Clamp(zoomXLerp, minZoom, maxZoom), Math.Clamp(zoomYLerp, minZoom, maxZoom));
			zoomingIn = false;
		}

		if(zoomingOut){
			var zoomXLerp = Mathf.Lerp(Zoom.X, Zoom.X - zoomFactor, zoomSpeed);
			var zoomYLerp = Mathf.Lerp(Zoom.Y, Zoom.Y - zoomFactor, zoomSpeed);

			Zoom = new Vector2(Math.Clamp(zoomXLerp, minZoom, maxZoom), Math.Clamp(zoomYLerp, minZoom, maxZoom));
			zoomingOut = false;
		}
	}
}
