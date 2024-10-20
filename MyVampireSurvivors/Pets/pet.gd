extends Node2D

@export var pet:Resource

func _ready():
	var spawn_pet = pet.instantiate()
	spawn_pet.position = position
	add_child(spawn_pet)
