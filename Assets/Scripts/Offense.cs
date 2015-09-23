using UnityEngine;
using System.Collections;

// public enum Offense {
// 	Pistol,
// 	EPistol,
// 	Rifle,
// 	ERifle,
// 	Sniper,
// 	ESniper,
// 	Gatling,
// 	Rocket,
// 	Grenades,
// }

public class Offense : MonoBehaviour {
	public Transform bullet;

	public float cooldown; // time until re-activation
	public float distance; // how far the attack can travel
	public float impact;   // size of the area affected by the attack
	public float power;    // attack damage and stun length
	public float speed;    // attack movement speed
	public int   hits;     // number of attacks launched

	public bool isCooled {
		get { return timer < 0; }
	}

	public void activate() {
		if (isCooled) {
			timer = cooldown;
			Rigidbody2D bullet = (Rigidbody2D)Instantiate(bullet);
		}
	}

	float timer;

	void Update() {
		if (!isCooled) {
			timer -= Time.deltaTime;
		}
	}
}