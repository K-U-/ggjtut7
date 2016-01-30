var _time:float;

function Start () {
	yield WaitForSeconds (_time);
	Destroy (gameObject);
}