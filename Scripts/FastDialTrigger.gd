extends Area2D

var activated = false
export(Array, String) var body_to_collide: Array = ["player"]
export(Array, Resource) var query: Array


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func start_dialogue():
	for q in query:
		if q is FastDialogueContainer:
			$"/root/FastDialogue".AddToQuery(q.name, q.text, q.voice)
	$"/root/FastDialogue".StartDialogue()


func _on_FastDialTrigger_body_entered(body):
	if activated: return
	else:
		var is_in_array = false
		for b in body_to_collide:
			if body.is_in_group(b): is_in_array = true
		if !is_in_array: return
		else:
			activated = true
			start_dialogue()
