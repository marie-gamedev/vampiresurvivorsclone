extends CharacterBody2D

var movement_speed = 40

@onready var player = get_tree().get_first_node_in_group("player")
@onready var anim_player = $AnimationPlayer

func _ready():
	anim_player.play("idle")

func _physics_process(delta):
	if global_position.distance_to(player.global_position) < 20:
		return
	else:
		movement_speed = global_position.distance_to(player.global_position) * 0.75

	if movement_speed < 40:
		movement_speed = 40
	
	var direction = global_position.direction_to(player.global_position)
	velocity = direction * movement_speed
	move_and_slide()
