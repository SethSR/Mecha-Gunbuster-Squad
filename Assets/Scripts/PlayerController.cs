using UnityEngine;
using System.Collections;

public class PlayerController : RobotController {
	Vector2 velocity = Vector2.zero;
	float   maxSpeed = 10; // 10 m/s
	float   velX     = 0;
	float   velY     = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetMouseButton(0)) {
		// 	target = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// }
		velX = Input.GetAxis("Horizontal");
		velY = Input.GetAxis("Vertical");
		velocity = new Vector2(velX, velY).normalized * maxSpeed;
		transform.position += (Vector3)velocity * Time.deltaTime;
	}
}