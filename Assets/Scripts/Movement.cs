// public enum Movement {
// 	Run,
// 	Tread,
// 	Wheel,
// 	Hover,
// 	Booster,
// }

public class Movement : MonoBehaviour {
	public bool aerial;    // whether the robot becomes airborne
	public float cooldown; // time until re-activation
	public float distance; // how far the robot will move
	public float speed;    // how fast the robot will move
}