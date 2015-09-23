// public enum Defense {
// 	Shield,
// 	EShield,
// 	Dodge,
// 	Parry,
// }

public class Defense : MonoBehaviour {
	public float cooldown; // time until re-activation
	public float power;    // damage reduction
	public float time;     // how long the shield lasts
	public float parry;    // the percent of damage deflected [over 100% reflects the shot back]
}