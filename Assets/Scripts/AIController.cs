using UnityEngine;
using System.Collections;

public class AIController : RobotController {
	public Transform target;
	public float maxSpeed = 50;

	new Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = Arrive.calculate((Vector2)transform.position, (Vector2)rigidbody2D.velocity, (Vector2)target.position, maxSpeed, 3) / rigidbody2D.mass;
	}
}