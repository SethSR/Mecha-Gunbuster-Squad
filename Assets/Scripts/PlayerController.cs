using UnityEngine;
using System.Collections;

public class PlayerController : RobotController {
	public Transform tempBullet;
	public float     bulletSpeed;

	new Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update() {
		var dir = new Vector2(Input.GetAxis("Horizontal"),
		                      Input.GetAxis("Vertical")).normalized;

		if (Input.GetButton("Fire1")) {
			Rigidbody2D bullet = ((Transform)Instantiate(tempBullet, transform.position, Quaternion.identity)).GetComponent<Rigidbody2D>();
			bullet.velocity = dir * bulletSpeed;
		}

		rigidbody2D.velocity = dir * maxSpeed;
	}
}